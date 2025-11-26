using SmartBook.Application.Services.Ingresos.Interfaces;

namespace SmartBook.Application.Services.Ingresos.Implementations
{
    public class LoteGeneradorService : ILoteGeneradorService
    {
        public string GenerarLote()
        {
            return GenerarLote(DateTime.Now);
        }

        public string GenerarLote(DateTime fecha)
        {
            var semestre = "";

            if (fecha.Month <= 6)
            {
                semestre = "01";
            }
            else
            {
                semestre = "02";
            }
            return $"{fecha.Year}-{semestre}";
        }
    }
}