namespace SmartBook.Domain.Dtos.Requests.IngresosRequest;
// estos son los 
// ingresos de los libros
public record ConsultarIngresoRequest
(
    DateOnly? FechaDesde,
    DateOnly? FechaHasta,
    // esto aqui pueden ir en filtros
    string Lote,
    string LibroId
);
