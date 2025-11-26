using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartBook.Application.Services.Authentication.Implementations;
using SmartBook.Application.Services.Authentication.Interfaces;
using SmartBook.Application.Services.Clientes.Implementations;
using SmartBook.Application.Services.Clientes.Interfaces;
using SmartBook.Application.Services.ClientesServices.Interfaces;
using SmartBook.Application.Services.Email.Implementations;
using SmartBook.Application.Services.Email.Interfaces;
using SmartBook.Application.Services.Ingresos.Implementations;
using SmartBook.Application.Services.Ingresos.Interfaces;
using SmartBook.Application.Services.Libros.Implementations;
using SmartBook.Application.Services.Libros.Interfaces;
using SmartBook.Application.Services.LogsServices.Implementations;
using SmartBook.Application.Services.LogsServices.Interface;
using SmartBook.Application.Services.PDF.Implementations;
using SmartBook.Application.Services.PDF.Interfaces;
using SmartBook.Application.Services.Usuarios.Implementations;
using SmartBook.Application.Services.Usuarios.Interfaces;
using SmartBook.Application.Services.ValidationServices.Implementations;
using SmartBook.Application.Services.ValidationServices.Interface;
using SmartBook.Application.Services.Ventas.Implementations;
using SmartBook.Application.Services.Ventas.Interfaces;
using SmartBook.Domain.Entities.DomainEntities;
using SmartBook.Persistence.Repositories;
using SmartBook.Persistence.Repositories.Implementation;
using SmartBook.Persistence.Repositories.Interface;
using SmartBook.Persistence.Stores.Implementation;
using SmartBook.Persistence.Stores.Interface;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SmartBook API - CDI CECAR",
        Version = "v1",
        Description = "API REST para la gestión de inventario del Centro de Idiomas CECAR"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Autenticación JWT usando el esquema Bearer.
                      
Ingresa tu token JWT en el campo de abajo (sin incluir 'Bearer').

Ejemplo: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});




builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);

builder.Services.Configure<PdfSettings>(
    builder.Configuration.GetSection("PdfSettings")
);




builder.Services.AddScoped<IVentaPdfFormatter, VentaPdfFormatter>();








builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

builder.Services.AddScoped<ILibroRepository, LibroRepository>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<IIngresoRepository, IngresoRepository>();

builder.Services.AddScoped<IVentaRepository, VentaRepository>();

builder.Services.AddSingleton<ITokenVerificacionStore, InMemoryTokenVerificacionStore>();


builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IVerificacionTokenService, VerificacionTokenService>();
builder.Services.AddScoped<ITokenCleanupService, TokenCleanupService>();

builder.Services.AddScoped<IEmailService, EmailService>();


builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<IVentaPdfFormatter, VentaPdfFormatter>();



builder.Services.AddScoped<IVentaService, VentaService>();


builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IAutorizacionService, AutorizacionService>();


builder.Services.AddScoped<ILogRepository,LogRepository>();
builder.Services.AddScoped<ILogService,LogService>();


builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IValidacionEdadService, ValidacionEdadService>();



builder.Services.AddScoped<ILibroService, LibroService>();
builder.Services.AddScoped<IValidacionLibroService, ValidacionLibroService>();



builder.Services.AddScoped<IIngresoService, IngresoService>();
builder.Services.AddScoped<ILoteGeneradorService, LoteGeneradorService>();


builder.Services.AddScoped<IEmailValidationService, EmailValidationService>();



builder.Services.AddHostedService<LimpiadorCuentasPendientesService>();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartBook API v1");
        c.DocumentTitle = "SmartBook API - CDI CECAR";
        c.ConfigObject.PersistAuthorization = true;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
