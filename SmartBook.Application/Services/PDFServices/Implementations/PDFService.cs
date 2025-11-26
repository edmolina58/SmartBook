using SmartBook.Application.Services.PDF.Interfaces;
using SmartBook.Domain.Entities.DatabaseEntities;

namespace SmartBook.Application.Services.PDF.Implementations
{
    public class PdfService : IPdfService
    {
        private readonly IVentaPdfFormatter _ventaPdfFormatter;

        public PdfService(IVentaPdfFormatter ventaPdfFormatter)
        {
            _ventaPdfFormatter = ventaPdfFormatter;
        }

        public byte[] GenerarPdf<T>(T data, string formatter) where T : class
        {
            return formatter switch
            {
                "venta" when data is Venta venta => _ventaPdfFormatter.FormatearVenta(venta),
                _ => throw new ArgumentException($"Formatter '{formatter}' no soportado para tipo {typeof(T).Name}")
            };
        }
    }
}