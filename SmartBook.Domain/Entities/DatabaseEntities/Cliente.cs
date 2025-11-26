using System.ComponentModel.DataAnnotations;

namespace SmartBook.Domain.Entities.DatabaseEntities;


public class Cliente
{


    
    [Key]  
    public string IdCliente { get; init; }

    [Required]
    [StringLength(100)]
    public string Identificacion { get; init; }
    [Required]
    public string Nombres { get; init; }
    [Required]
    [EmailAddress]
    public string Email { get; init; }
    
    [Phone]
    public string Celular { get; init; }

    [Required]
    public DateOnly FechaNacimiento { get; init; }

    [Required]
    public DateTime fecha_creacion { get; init; }
    [Required]
    public DateTime fecha_actualizacion { get; set; }
    

}
