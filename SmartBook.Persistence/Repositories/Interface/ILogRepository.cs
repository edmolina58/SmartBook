using SmartBook.Domain.Dtos.Reponses.LogsReponses;
using SmartBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Persistence.Repositories.Interface;
public interface ILogRepository
{
    ConsultarLogResponse? ConsultarPorId(string idLog);
    IEnumerable<ConsultarLogResponse> ConsultarPorUsario(string usuarioSistema);
}
