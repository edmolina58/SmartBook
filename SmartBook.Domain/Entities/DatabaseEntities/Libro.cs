using SmartBook.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartBook.Domain.Entities;

public class Libro
{
    [Required]
    [Key]
    public string IdLibro { get; init; }

    [Required]
    public string Nombre {  get; init; }

    [Required]
    public string Nivel { get; init; }

    [Required]
    public TipoLibro TipoLibro { get; init; }

    [Required]
    public string Editorial { get; init; }

    [Required]
    public DateTime fecha_creacion { get; init; }
    [Required]
    public DateTime fecha_actualizacion { get; set; }
    [Required]
    public string Edicion { get; init; }

    [Required]
    public int Stock { get; init; }
  

}
