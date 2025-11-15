

using SmartBook.Domain.Dtos.Reponses.UsuariosReponses;
using SmartBook.Domain.Dtos.Requests.UsuarioRequest;
using SmartBook.Domain.Entities;

namespace SmartBook.Application.Interface;
public interface IUsuarioService
{
    UsuarioReponse? Crear(CrearUsuarioRequest request);
    IEnumerable<ConsultarUsuarioReponse> ConsultarUsuario(ConsultarUsuarioRequest request);
   
    string encriptarpassword(string texto);
    string generarJWT(Usuario modelo);
  
}
