
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SmartBook.Application.Interface;
using SmartBook.Domain.Entities;
using SmartBook.Persistence.Repositories;
using SmartBook.Persistence.Repositories.Interface;
using SmartBook.Services;
using SmartBook.WebApi.Services;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            




            //Los repositorios que se contectaran
            builder.Services.AddScoped<IClienteRepository,ClienteRepository>();
            builder.Services.AddScoped<IClienteService,ClienteService>();

            builder.Services.AddScoped<ILibroRepository,LibroRepository>();
            builder.Services.AddScoped<ILibroService,LibroService>();

            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();


            builder.Services.AddScoped<IVentaRepository, VentaRepository>();
            builder.Services.AddScoped<IVentaService, VentaService>();

            //Registro mis dependencias en el contenedor de dependencias


            // Email
            builder.Services.Configure<EmailSettings>(
            builder.Configuration.GetSection("EmailSettings")
                );// Email






            builder.Services.AddAuthentication(config =>
            {

                config.DefaultAuthenticateScheme= JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                ValidateIssuerSigningKey=true,
                ValidateIssuer=false,
                ValidateAudience=false,
                ValidateLifetime=true,
                ClockSkew=TimeSpan.Zero,

                IssuerSigningKey=new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:Key"]!)
                    )
                };
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            //autenticacion 
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        
