using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Entities;
public class Ingreso
{
    [Required]
    [Key]
    public string? IdIngresos { get; init; }
    [Required]
   
    public string? Lote { get; init; }

    [Required]
    public int Unidades { get; set; }
    [Required]
    public double ValorCompra { get; init; }

    [Required]
    public double ValorVenta { get; set; }
    [Required]
    public DateOnly Fecha { get; init; }
    [Required]
    
    public string? libro { get; set; }

}

