
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Muzej.API.Middleware;
using Muzej.API.Service;
using Muzej.Application.Autori.Commands.KreirajAutora;
using Muzej.Application.Common.Behaviors;
using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;
using Muzej.Infrastructure;
using Muzej.Infrastructure.Identity;
using Scalar.AspNetCore;
using System.Text;


namespace Muzej.API
{
    public class Program
    {
        public static async Task Main(string[] args)
         {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<MuzejContext>(options =>
                    options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MuzejDb;Trusted_Connection=True;"));

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<KreirajAutoraCommand>());
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddIdentityCore<Korisnik>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredLength = 5;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MuzejContext>();

            builder.Services.AddValidatorsFromAssemblyContaining<KreirajAutoraCommand>();
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            builder.Services.AddScoped<JwtTokenService>();

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });

            var app = builder.Build();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            //using (var scope = app.Services.CreateScope())
            //{
            //    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Korisnik>>();

            //    var testKorisnik = await userManager.FindByEmailAsync("marko@test.com");

            //    if (testKorisnik is null)
            //    {
            //        testKorisnik = new Korisnik
            //        {
            //            UserName = "marko",
            //            Email = "marko@test.com",
            //            Ime = "Marko",
            //            Prezime = "Marković",
            //            TipPosetioca = TipPosetioca.Student
            //        };

            //        var result = await userManager.CreateAsync(testKorisnik, "Test123!");

            //        if (result.Succeeded)
            //            Console.WriteLine($"Test korisnik kreiran, Id: {testKorisnik.Id}");
            //        else
            //            Console.WriteLine(string.Join(", ", result.Errors.Select(e => e.Description)));
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Test korisnik već postoji, Id: {testKorisnik.Id}");
            //    }
            //}

            // Configure the HTTP request pipeline.

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                foreach (var uloga in new[] { "Posetilac", "Administrator" })
                {
                    if (!await roleManager.RoleExistsAsync(uloga))
                    {
                        await roleManager.CreateAsync(new IdentityRole(uloga));
                    }
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Korisnik>>();

                var admin = await userManager.FindByEmailAsync("admin@muzej.com");
                if (admin is null)
                {
                    admin = new Korisnik
                    {
                        UserName = "admin",
                        Email = "admin@muzej.com",
                        Ime = "Jovan",
                        Prezime = "Jovanović",
                        Zvanje = "Direktor"
                    };

                    var result = await userManager.CreateAsync(admin, "Admin123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Administrator");
                        Console.WriteLine($"Admin kreiran, Id: {admin.Id}");
                    }
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); //!!

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
