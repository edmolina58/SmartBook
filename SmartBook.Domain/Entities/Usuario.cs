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
    public string Email { get; init; }

    [Required]
    public RolUsuario RolUsuario { get; init; }
    [Required]
    public DateTime fecha_creacion { get; init; }
    [Required]
    public DateTime fecha_actualizacion { get; set; }
    public bool EmailVerificado { get; set; }

}
