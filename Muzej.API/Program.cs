
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Muzej.Application.Autori.Commands.KreirajAutora;
using Muzej.Domain.Entities;
using Muzej.Domain.Repositories;
using Muzej.Infrastructure;
using Muzej.Infrastructure.Identity;

using Scalar.AspNetCore;

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

            var app = builder.Build();

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
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
