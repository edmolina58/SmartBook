namespace SmartBook.Domain.Dtos.Requests.ClienteRequest;

public record ConsultarClienteRequest
(
    //necesario que sea obligatorio??
    string IdentificacionCliente,
    string? NombreCliente
    
    
    );
