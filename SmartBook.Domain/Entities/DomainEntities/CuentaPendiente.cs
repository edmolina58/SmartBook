using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Entities.DomainEntities;
public class CuentaPendiente
{    public string Identificacion { get; init; }
    public string Email { get; init; }
    public DateTime FechaCreacion { get; init; }
    public bool Verificada { get; init; }
}