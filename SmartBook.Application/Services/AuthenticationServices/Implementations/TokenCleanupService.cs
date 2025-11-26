using SmartBook.Application.Services.Authentication.Interfaces;
using SmartBook.Persistence.Stores.Interface;

namespace SmartBook.Application.Services.Authentication.Implementations
{
    public class TokenCleanupService : ITokenCleanupService
    {
        private readonly ITokenVerificacionStore _tokenStore;

        public TokenCleanupService(ITokenVerificacionStore tokenStore)
        {
            _tokenStore = tokenStore;
        }

        public void LimpiarTokensExpirados()
        {
            _tokenStore.LimpiarExpirados();
        }
    }
}