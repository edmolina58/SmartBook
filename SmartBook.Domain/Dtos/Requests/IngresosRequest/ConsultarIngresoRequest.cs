using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Dtos.Requests.IngresosRequest;
public record ConsultarIngresoRequest
(
   DateOnly? Desde,
   DateOnly? Hasta,
   string? Lote,
   string? Libro
);

