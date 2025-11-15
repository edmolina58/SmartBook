
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartBook.Domain.Entities;
using SmartBook.Persistence.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace SmartBook.WebApi.Services;
public class UsuarioService 
{
    private readonly UsuarioRepository _usuarioRepository;
    private readonly IConfiguration _cofiguration;
    public UsuarioService(IConfiguration configuration)
    {

        _cofiguration = configuration;

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