
using Microsoft.AspNetCore.Cors.Infrastructure;
using SmartBook.Domain.Entities;
using SmartBook.Persistence.Repositories;
using SmartBook.Persistence.Repositories.Interface;

namespace SmartBook.WebApi
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
            

            //Conexion
            var _connectionString = "Server=localhost;Port=3306;Database=SmartBook;User Id=root;Password=admin";
            builder.Services.AddSingleton(_connectionString);
            //Los repositorios que se contectaran
            builder.Services.AddScoped<IClienteRepository,ClienteRepository>();
            builder.Services.AddScoped<ILibroRepository,LibroRepository>();
            builder.Services.AddScoped<IVentaRepository, VentaRepository>();
            builder.Services.AddScoped<UsuarioRepository>();

            // Email
            builder.Services.Configure<EmailSettings>(
            builder.Configuration.GetSection("EmailSettings")
                );// Email



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
