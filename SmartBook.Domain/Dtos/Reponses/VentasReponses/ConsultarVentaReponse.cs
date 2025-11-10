

namespace SmartBook.Domain.Dtos.Reponses.VentasReponses;
public record  ConsultarVentaReponse
(
   string idVenta,
   int NumeroRecibo,
   DateTime FechaVenta,
   string IdCliente,
   string IdUsuario,
   string IdLibro,
   string? Observaciones
    );
