using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Dtos.Requests.IngresosRequest;
public record ConsultarIngresoFiltrado
(
    DateOnly Desde,
    DateOnly Hasta
   
    );
