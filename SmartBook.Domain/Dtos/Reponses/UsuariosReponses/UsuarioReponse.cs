using SmartBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Dtos.Reponses.UsuariosReponse;
public record UsuarioReponse
(
    string IdUsuario,
    string Identificacion,
    string Nombre,
    string EmailUsuario,
    RolUsuario RolUsuario,
    EstadoUsuario EstadoUsuario,
    DateTime FechaCreacion,
    DateTime FechaActualizacion
    /// mirar si se le debe añadir mas
    /// mirar si se le debe añadir mas
    );
