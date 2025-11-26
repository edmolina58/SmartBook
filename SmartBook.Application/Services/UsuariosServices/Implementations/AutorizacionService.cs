using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Application.Services.Usuarios.Implementations;

using SmartBook.Application.Services.Usuarios.Interfaces;
using SmartBook.Domain.Enums;

using SmartBook.Domain.Exceptions;

public class AutorizacionService : IAutorizacionService
    {
        public void ValidarPermisoCreacion(RolUsuario rolActual, RolUsuario rolNuevoUsuario)
        {
            if (rolActual != RolUsuario.Admin)
            {
                throw new BusinessRoleException("Solo los administradores pueden crear usuarios");
            }

            if (rolNuevoUsuario == RolUsuario.Admin && rolActual != RolUsuario.Admin)
            {
                throw new BusinessRoleException("Solo los administradores pueden crear cuentas de administradores");
            }
        }

        public void ValidarPermisoActualizacion(string rolActual, string idUsuarioActual, string idUsuarioObjetivo)
        {
            if (rolActual != RolUsuario.Admin.ToString())
            {
                throw new BusinessRoleException("Solo los administradores pueden actualizar usuarios");
            }

            if (idUsuarioObjetivo == idUsuarioActual)
            {
                throw new BusinessRoleException("No puedes modificar tu propia cuenta");
            }
        }
    }
