using System.ComponentModel.DataAnnotations;

namespace SmartBook.Domain.Entities;

public class Inventario
{
    [Required]
    public string IdInventario { get; init; }

    [Required]
    public Libro LibroId { get; init; }

    [Required]
    public string Lote {  get; init; }

    [Required]
    public int UnidadesDisponibles { get; init; }

    public DateTime FechaActualizacion { get; init; }




}
