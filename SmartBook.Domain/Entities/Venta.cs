using System.ComponentModel.DataAnnotations;

namespace SmartBook.Domain.Entities;

public class Venta
{
    
    [Key]
    public string Id { get; init; }
    [Required]
    public int NumeroReciboPago { get; init; }
    [Required]
    public DateTime Fecha { get; init; }

    [Required]
    public string ClienteId { get; init; }
    [Required]
    public string UsuarioId { get; init; }

    [Required]
    public string LibroId { get; init; }


    public string Observaciones { get; init; }


}
