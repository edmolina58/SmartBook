using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Dtos.Requests.LogsRequest;
public record ConsultarLogRequest(
    string? IdRegistro,
    string? UsuarioSistema,
    DateTime? FechaLog
);