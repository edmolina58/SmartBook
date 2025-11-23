using SmartBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Dtos.Reponses.UsuariosReponses;
public record ConsultarUsuarioReponse
(
    string NombrCompleto,
    RolUsuario RolUsuario,
    DateTime fecha_creacion,

    DateTime? fecha_actualizacion

    );
