namespace SmartBook.Application.Services.PDF.Interfaces
{
    public interface IPdfService
    {
        byte[] GenerarPdf<T>(T data, string formatter) where T : class;
    }
}