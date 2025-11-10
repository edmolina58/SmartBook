namespace SmartBook.Domain.Dtos.Requests.IngresosRequest;
// estos son los 
// ingresos de los libros
public record CrearIngresoRequest
(
    string? LibroId,
    int Unidades,
    double ValorCompra,
    double ValorVenta
    
    );
