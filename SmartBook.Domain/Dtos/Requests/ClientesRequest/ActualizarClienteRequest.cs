


using System.ComponentModel.DataAnnotations;

namespace SmartBook.Domain.Dtos.Requests.ClienteRequest;

public record ActualizarClienteRequest
    (
    [Required]
    string identificacion,
    [Required]
    string NombreCliente,
    [EmailAddress]
    string EmailCliente,
    [Required]
    string CelularCliente,
    [Required]
    DateOnly FechaNacimientoCliente
    );