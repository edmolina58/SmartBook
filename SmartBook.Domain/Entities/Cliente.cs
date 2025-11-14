using System.ComponentModel.DataAnnotations;

namespace SmartBook.Domain.Entities;

// entidades y enums a la logica de negocio
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

    // un cliente puede tener muchas ventas
    public ICollection<Venta> Ventas { get; init; }

 
}
















// 
// COMPRA -> INGRESO -> Aumenta INVENTARIO
                      //    ↓
// VENTA → VENTA_DETALLE → Disminuye INVENTARIO
                        //  ↓
// CLIENTE recibe comprobante + USUARIO registra la venta












