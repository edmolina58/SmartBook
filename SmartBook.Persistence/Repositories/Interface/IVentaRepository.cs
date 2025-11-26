
using Org.BouncyCastle.Asn1.Ocsp;
using SmartBook.Domain.Dtos.Reponses.VentasReponses;
using SmartBook.Domain.Entities.DatabaseEntities;


namespace SmartBook.Persistence.Repositories.Interface;
public interface IVentaRepository
{


     void Crear(Venta venta);

    ConsultarVentaReponse Consultar(string numero_recibo);

    public ConsultarVentaReponse ConsultarPrecio(string numero_recibo);
    IEnumerable<ConsultarVentaReponse> ConsultarPorCampos(DateTime? rangoFecha, DateTime? Hasta, string? cliente, string? libro);




}