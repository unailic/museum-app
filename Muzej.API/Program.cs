
using MediatR;
using Microsoft.EntityFrameworkCore;
using Muzej.Application.Autori.Commands.KreirajAutora;
using Muzej.Domain.Repositories;
using Muzej.Infrastructure;
using Scalar.AspNetCore;

namespace Muzej.API
{
    public class Program
    {
        public static void Main(string[] args)
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

            var app = builder.Build();

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
