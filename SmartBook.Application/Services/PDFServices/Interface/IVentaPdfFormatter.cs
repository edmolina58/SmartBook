using SmartBook.Domain.Entities.DatabaseEntities;

namespace SmartBook.Application.Services.PDF.Interfaces
{
    public interface IVentaPdfFormatter
    {
        byte[] FormatearVenta(Venta venta);
    }
}