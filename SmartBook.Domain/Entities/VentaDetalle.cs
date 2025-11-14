using SmartBook.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartBook.Domain.Entities;
//venta puede tener multiples libros
public class VentaDetalle
{
    [Required]
    [Key]
    public string IdDetalle { get; init; }

    [Required]
    public Venta VentaId { get; init; }

    [Required]
    public int unidades { get; init; }

    [Required]
    public double precio_unidad {  get; init; }


}
