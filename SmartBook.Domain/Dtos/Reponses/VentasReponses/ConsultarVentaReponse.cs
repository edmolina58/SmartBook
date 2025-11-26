using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Dtos.Reponses.VentasReponses;
public record ConsultarVentaReponse
(
   string idVenta,
   string NumeroRecibo,
   DateTime FechaVenta,
   string IdCliente,
   string IdUsuario,
   string IdLibro,
   int unidades,
   double precio_unidad,
   string? Observaciones


    );
