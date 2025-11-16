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
    public string Password { get; set; }
    [Required]
    public string NombreCompleto {  get; init; }
   
    [Required]
    [EmailAddress]
    [RegularExpression(@"@cecar\.edu\.co$",ErrorMessage = "Hubo un problema con el ingreso, intentelo nuevamente")]
    public string Email { get; init; }

    [Required]
    public RolUsuario RolUsuario { get; init; }
    


}
