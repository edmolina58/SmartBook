using SmartBook.Domain.Enums;

namespace SmartBook.Application.Services.Libros.Interfaces
{
    public interface IValidacionLibroService
    {
        void ValidarLibroNoExiste(string nombre, string nivel, TipoLibro tipoLibro, string edicion);
    }
}