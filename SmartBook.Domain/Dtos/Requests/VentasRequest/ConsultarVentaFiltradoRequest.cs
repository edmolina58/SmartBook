using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Dtos.Requests.VentasRequest;
public record ConsultarVentaFiltradoRequest
(
    DateTime? Desde,
    DateTime? Hasta,
    string? ClienteId,
    string? LibroId
    );


