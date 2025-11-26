using System.ComponentModel.DataAnnotations;

namespace SmartBook.Domain.Dtos.Requests.ClienteRequest;

public record ConsultarClienteRequest
([Required]
    string IdentificacionCliente
    
    
    );
