namespace SmartBook.Application.Services.Authentication.Interfaces
{
    public interface IVerificacionTokenService
    {
        string GenerarTokenVerificacion(string email);
        bool ValidarTokenVerificacion(string token, out string email);
    }
}