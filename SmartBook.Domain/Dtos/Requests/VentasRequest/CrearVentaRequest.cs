using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Dtos.Requests.VentasRequest;
public record CrearVentaRequest
(
    string ClienteId,

    string UsuarioId,

    string LibroId,

    string? Observaciones,

    int Unidades );
