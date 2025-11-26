






namespace SmartBook.Application.Services.Ingresos.Interfaces
{
    public interface ILoteGeneradorService
    {
        string GenerarLote();
        string GenerarLote(DateTime fecha);
    }
}