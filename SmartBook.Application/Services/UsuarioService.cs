
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartBook.Aplicacion.Extensions;
using SmartBook.Application.Interface;
using SmartBook.Domain.Dtos.Reponses.UsuariosReponses;
using SmartBook.Domain.Dtos.Requests.UsuarioRequest;
using SmartBook.Domain.Entities;
using SmartBook.Domain.Exceptions;
using SmartBook.Persistence.Repositories;
using SmartBook.Persistence.Repositories.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;


namespace SmartBook.WebApi.Services;
public class UsuarioService: IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    
    private readonly IConfiguration _cofiguration;
    public UsuarioService(IUsuarioRepository usuarioRepository,IConfiguration configuration)
    {

        _cofiguration = configuration;
        _usuarioRepository = usuarioRepository;
    }

    public UsuarioReponse? Crear(CrearUsuarioRequest request)
    {

        if (_usuarioRepository.ValidarCreacionUsuario(request.identificacion!))
        {
            throw new BusinessRoleException("Ya existe un usuario con esta identificación");
        }
        if (!ValidarPassword(request.PasswordUsuario!))
        {
            throw new BusinessRoleException("Coloque una contraseña segura");
        }

        var usuario = new Usuario
        {
            IdUsuario = DateTime.Now.Ticks.ToString(),
            Identificacion = request.identificacion!.Sanitize().RemoveAccents(),
            NombreCompleto = request.NombreUsuario!.Sanitize().RemoveAccents(),
            Email = request.EmailUsuario!.Sanitize().RemoveAccents(),
            Password = encriptarpassword(request.PasswordUsuario!.Sanitize().RemoveAccents()),
            RolUsuario = request.RolUsuario
        };

        _usuarioRepository.Crear(usuario);



        var respouesta = new UsuarioReponse("Usuario Creado Exitosamente");


        return respouesta;
    }



    /*
    public bool Actualizar(string id, ActualizarUsuarioRequest request)
    {
        var usuario = _usuarioRepository.Actulizar(id, request);
        if (usuario is false) return false;


        return _usuarioRepository.Actulizar(id, request);
    }*/

    public IEnumerable<ConsultarUsuarioReponse> ConsultarUsuario(ConsultarUsuarioRequest request)
    {

        return _usuarioRepository.ConsultarPorNombre(request.NombreUsuario!, request.RolUsuario);
    }





















    private bool ValidarPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;
        // Expresión regular para contraseña segura
        var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$");

        return regex.IsMatch(password);
    }

    public string encriptarpassword(string texto)
    {
        using(SHA256 sha256 = SHA256.Create())
        {
        byte[] bytes=sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
        StringBuilder builder = new StringBuilder();
        for(int i=0;i<bytes.Length; i++)
            { 
            builder.Append(bytes[i].ToString("x2"));
            
            }
        return builder.ToString();

        }


    }

    public string generarJWT(Usuario modelo)
    {
        var userClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,modelo.IdUsuario.ToString()),
            new Claim(ClaimTypes.Email, modelo.Email!)

        };
        var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cofiguration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256Signature);

        var jwtConfig = new JwtSecurityToken(
            claims:userClaims,
            expires: DateTime.UtcNow.AddMinutes(59),
            signingCredentials: credentials
            );
        return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
    }


}