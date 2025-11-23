using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Dtos.Reponses.IngresosReponses;
public record IngresosReponse
(

string IdIngresos,
string Lote,
string libro,
int Unidades,
double ValorCompra,
double ValorVenta,
DateOnly Fecha


);
