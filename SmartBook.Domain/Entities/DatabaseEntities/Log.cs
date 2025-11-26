using SmartBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Entities.DatabaseEntities;
public class Log
{
    public int IdLog { get; init; }
    public DateTime Fecha { get; init; }
    public string Tabla { get; init; } 
    public string Operacion { get; init; } 
    public string? IdRegistro { get; init; }
    public string? UsuarioSistema { get; init; }
    public string? Detalle { get; init; }
    public ResultadosLogs Resultado { get; init; }
}

