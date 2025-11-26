using SmartBook.Domain.Entities.DomainEntities;
using SmartBook.Persistence.Stores.Interface;

namespace SmartBook.Persistence.Stores.Implementation
{
    public class InMemoryTokenVerificacionStore : ITokenVerificacionStore
    {
        private static readonly List<TokenVerificacion> _tokens = new();
        private static readonly object _lock = new();

        public void Guardar(TokenVerificacion token)
        {
            lock (_lock)
            {
                _tokens.Add(token);
            }
        }

        public TokenVerificacion? ObtenerPorToken(string token)
        {
            lock (_lock)
            {
                return _tokens.FirstOrDefault(t => t.Token == token);
            }
        }

        public void Eliminar(string token)
        {
            lock (_lock)
            {
                _tokens.RemoveAll(t => t.Token == token);
            }
        }

        public void EliminarPorEmail(string email)
        {
            lock (_lock)
            {
                _tokens.RemoveAll(t => t.Email == email);
            }
        }

        public void LimpiarExpirados()
        {
            lock (_lock)
            {
                _tokens.RemoveAll(t => t.Expiracion < DateTime.UtcNow);
            }
        }
    }
}