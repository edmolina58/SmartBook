using Org.BouncyCastle.Asn1.Ocsp;
using SmartBook.Domain.Dtos.Reponses.LogsReponses;
using SmartBook.Domain.Dtos.Requests.LogsRequest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Application.Services.LogsServices.Interface;
public interface ILogService
{
     ConsultarLogResponse? ConsultarPorId(string idLog);

    IEnumerable<ConsultarLogResponse> ConsultarPorFiltros(ConsultarLogUsuarios request);
}
