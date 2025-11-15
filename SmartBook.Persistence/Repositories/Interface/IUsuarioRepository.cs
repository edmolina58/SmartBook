using SmartBook.Domain.Dtos.Reponses.UsuariosReponses;
using SmartBook.Domain.Entities;
using SmartBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Persistence.Repositories.Interface;
public interface IUsuarioRepository
{
    bool ValidarCreacionUsuario(string identificacion);
    void Crear(Usuario usuario);
    bool Borrar(string id);
    IEnumerable<ConsultarUsuarioReponse> ConsultarPorNombre(string nombreCompleto, RolUsuario? rolUsuario);
    
}
