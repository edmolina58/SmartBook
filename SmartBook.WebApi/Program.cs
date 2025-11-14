
using Microsoft.AspNetCore.Cors.Infrastructure;
using SmartBook.Domain.Entities;
using SmartBook.Persistence.Repositories;
using SmartBook.Persistence.Repositories.Interface;
using SmartBook.Services;
using SmartBook.WebApi.Services;


            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            




            //Los repositorios que se contectaran
            builder.Services.AddScoped<IClienteRepository,ClienteRepository>();
            builder.Services.AddScoped<ClienteService>();

            builder.Services.AddScoped<ILibroRepository,LibroRepository>();
            builder.Services.AddScoped<LibroService>();


            builder.Services.AddScoped<IVentaRepository, VentaRepository>();
            builder.Services.AddScoped<VentaService>();

            builder.Services.AddScoped<UsuarioRepository>();
            //Registro mis dependencias en el contenedor de dependencias






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
        
