using System.ComponentModel.DataAnnotations;

namespace SmartBook.Domain.Entities;

public class Ingreso
{
    [Required]
    public string IdIngresos { get; init; }
    [Required]
    //libroid?
    public string Lote {  get; init; }

    [Required]
    public int Unidades { get; init; }
    [Required]
    public double ValorCompra {  get; init; }

    [Required]
    public double ValorVenta { get; init; }
    public DateTime FechaEntrega { get; init; }
    [Required]
    // un ingreso pertenece a un libro?
    public Libro libro { get; init; }

}
