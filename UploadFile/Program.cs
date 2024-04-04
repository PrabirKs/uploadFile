
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using UploadFile.Data;

namespace UploadFile
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //cors policy
            builder.Services.AddCors(options =>
            {
                //define policy for the react app
                options.AddPolicy("reactapp", policyBuilder => {
                    policyBuilder.WithOrigins("http://localhost:5174");
                    policyBuilder.AllowAnyHeader();
                    policyBuilder.AllowAnyMethod();
                    policyBuilder.AllowCredentials();
                }); ;
            });
            //database

            builder.Services.AddDbContext<ApplicationDbContext>(optoins =>
            {
                optoins.UseSqlServer(builder.Configuration.GetConnectionString("PdfDb"));
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            //use it in the app
            app.UseCors("reactapp");

            app.Run();
        }
    }
}
