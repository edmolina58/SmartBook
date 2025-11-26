namespace SmartBook.Application.Services.Authentication.Interfaces
{
    public interface ITokenCleanupService
    {
        void LimpiarTokensExpirados();
    }
}