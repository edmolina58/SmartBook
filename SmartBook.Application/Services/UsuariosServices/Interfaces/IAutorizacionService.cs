using SmartBook.Domain.Enums;

namespace SmartBook.Application.Services.Usuarios.Interfaces
{
    public interface IAutorizacionService
    {
        void ValidarPermisoCreacion(RolUsuario rolActual, RolUsuario rolNuevoUsuario);
        void ValidarPermisoActualizacion(string rolActual, string idUsuarioActual, string idUsuarioObjetivo);
    }
}