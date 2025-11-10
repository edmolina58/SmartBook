using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Dtos.Reponses.IngresosReponse;
public record IngresosReponse
(
    string IdIngresos,
    string LibroId,
    string LibroNombre,
    string Lote,
    double ValorCompra,
    DateTime FechaEntrega,
    DateTime FechaCreacion 
    );
