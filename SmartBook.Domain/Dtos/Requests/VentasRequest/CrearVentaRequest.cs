using SmartBook.Domain.Entities;


namespace SmartBook.Domain.Dtos.Requests.VentasRequest;

public record CrearVentaRequest
(


    string ClienteId ,

    string UsuarioId,

    string LibroId ,

    string? Observaciones 

    );
