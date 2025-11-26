using SmartBook.Application.Services.LogsServices.Interface;
using SmartBook.Domain.Dtos.Reponses.LogsReponses;
using SmartBook.Domain.Dtos.Requests.LogsRequest;
using SmartBook.Persistence.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Application.Services.LogsServices.Implementations;
public class LogService: ILogService
{
    private readonly ILogRepository _logRepository;

    public LogService(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public ConsultarLogResponse? ConsultarPorId(string idLog)
    {
        return _logRepository.ConsultarPorId(idLog);
    }

    public IEnumerable<ConsultarLogResponse> ConsultarPorFiltros(ConsultarLogUsuarios request)
    {
        return _logRepository.ConsultarPorUsario( request.usuarioSistema );
    }
}
