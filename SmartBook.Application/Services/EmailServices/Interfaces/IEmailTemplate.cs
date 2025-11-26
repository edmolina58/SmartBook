namespace SmartBook.Application.Services.Email.Interfaces
{
    public interface IEmailTemplate
    {
        string GenerarHtml(Dictionary<string, string> datos);
        string ObtenerAsunto();
    }
}