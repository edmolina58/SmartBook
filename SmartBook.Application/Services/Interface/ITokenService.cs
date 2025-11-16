namespace SmartBook.Application.Services.Interface
{
    public interface ITokenService
    {
        string GenerarTokenVerificacion(string email);
        bool ValidarToken(string token, out string email);
        void LimpiarTokensExpirados();
    }
}