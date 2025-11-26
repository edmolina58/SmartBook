using SmartBook.Domain.Dtos.Requests.LogsRequest;
using SmartBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Dtos.Reponses.LogsReponses;
public record ConsultarLogResponse(
int id_log ,
DateTime? fechaLog, 
string? tabla ,
string? operacion ,
string? id_registro ,
string? usuario_sistema ,
string? detalle,
ResultadosLogs resultados
);
