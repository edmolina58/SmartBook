using SmartBook.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartBook.Domain.Entities;

public class Usuario
{ 

    [Required]
    [Key]
    public string IdUsuario { get; init; }
    [Required]
    public string Identificacion { get; init; }
    [Required]
    //Queda por modificar mas cosas
    public string Password { get; init; }
    [Required]
    public string NombreCompleto {  get; init; }
   
    [Required]
    [EmailAddress]
    [RegularExpression(@"@cecar\.edu\.co$",ErrorMessage = "Hubo un problema con el ingreso, intentelo nuevamente")]
    public string Email { get; init; }

    [Required]
    // verificar si estas declaraciones son correctas
    public RolUsuario RolUsuario { get; init; }
    


}
