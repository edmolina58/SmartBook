using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Dtos.Reponses.VentasReponse;
public record  VentaReponse
(
    string IdVenta,
    DateTime fechaVenta
    // deberia ir una lista con el deatlle de ventas?
    );
