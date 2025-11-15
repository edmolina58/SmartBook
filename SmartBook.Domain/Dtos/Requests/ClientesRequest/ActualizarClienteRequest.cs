


namespace SmartBook.Domain.Dtos.Requests.ClienteRequest;

public record ActualizarClienteRequest
    (
    string identificacion,
    string NombreCliente,
    string EmailCliente,
    string CelularCliente,
    DateTime FechaNacimientoCliente  
    );