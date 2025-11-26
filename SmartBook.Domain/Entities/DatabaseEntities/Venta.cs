using System.ComponentModel.DataAnnotations;
namespace SmartBook.Domain.Entities.DatabaseEntities;





public class Venta
{
    [Key]
    public string Id { get; init; }
    [Required]
    public long NumeroReciboPago { get; init; }
    [Required]
    public DateTime Fecha { get; init; }

    [Required]
    public string ClienteId { get; init; }
    [Required]
    public string UsuarioId { get; init; }

    [Required]
    public string LibroId { get; init; }

    [Required]
    public int Unidades { get; init; }

    [Required]
    public double Precio_unidad { get; init; }

    public string Observaciones { get; init; }

}
