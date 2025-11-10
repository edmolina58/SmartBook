using SmartBook.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartBook.Domain.Entities;

public class Usuario
{ //Esta es del que maneja los libros
    private string? passwordUsuario;
    private string? nombreUsuario;
    private string emailUsuario;
    private RolUsuario rolUsuario;

    public Usuario(string idUsuario, string passwordUsuario, string nombreUsuario, string emailUsuario, RolUsuario rolUsuario)
    {
        IdUsuario = idUsuario;
        this.passwordUsuario = passwordUsuario;
        this.nombreUsuario = nombreUsuario;
        this.emailUsuario = emailUsuario;
        this.rolUsuario = rolUsuario;
    }

    [Required]
    [Key]
    public string IdUsuario { get; init; }
    [Required]
    public string Identificacion { get; init; }
    [Required]
    //Queda por modificar mas cosas
    public string Password { get; init; }
    [Required]
    public string Nombre {  get; init; }
    [Required]
    [EmailAddress]
    [RegularExpression(@"@cecar\.edu\.co$",ErrorMessage = "Hubo un problema con el ingreso, intentelo nuevamente")]
    public string Email { get; init; }

    [Required]
    // verificar si estas declaraciones son correctas
    public RolUsuario RolUsuario { get; init; }
    public EstadoUsuario EstadoUsuario { get; init; }
    
    public DateTime FechaCreacion { get; init; }
    public DateTime FechaActualizacion { get; init; }
    public DateTime TiempoVerificacion{ get; init; }

    // un usuario puede realizar muchas ventas

    public ICollection<Venta> Ventas { get; init; }


}
