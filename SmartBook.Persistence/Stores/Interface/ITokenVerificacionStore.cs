using SmartBook.Domain.Entities.DomainEntities;

namespace SmartBook.Persistence.Stores.Interface
{
    public interface ITokenVerificacionStore
    {
        void Guardar(TokenVerificacion token);
        TokenVerificacion? ObtenerPorToken(string token);
        void Eliminar(string token);
        void EliminarPorEmail(string email);
        void LimpiarExpirados();
    }
}