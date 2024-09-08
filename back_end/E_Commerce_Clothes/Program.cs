
using E_Commerce_Clothes.DTOs;
using E_Commerce_Clothes.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Clothes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            // Add services to the container.
            builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("YourConnectionString")));

            builder.Services.AddCors(option =>
       option.AddPolicy("Develpoment", builder
       =>
       {
           // هاي اذا بدي احدده بالدومين بكتب هذا الكود 
           //builder.WithOrigins("http://localhost:5501");
           builder.AllowAnyOrigin();
           builder.AllowAnyMethod();
           builder.AllowAnyHeader();



       }));


            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IEmailService, EmailService>();

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

            app.Run();
        }
    }
}
