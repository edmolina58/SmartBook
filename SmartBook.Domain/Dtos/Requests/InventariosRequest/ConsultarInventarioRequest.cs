using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Dtos.Requests.InventariosRequest;
public record ConsultarInventarioRequest
(
    string? Lote
    
    );

// este y el de ventadetallerequest hay que mirar bien
// si se debe o no corregir
