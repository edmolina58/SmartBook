namespace SmartBook.Domain.Dtos.Requests.ClienteRequest;

public record CrearClienteRequest
(
    string IdentificacionCliente,
    string NombreCliente,
    string EmailCliente,
    string Celular,
    DateOnly FechaNacimiento
    
    );
