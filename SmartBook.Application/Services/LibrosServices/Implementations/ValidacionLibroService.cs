using SmartBook.Application.Services.Libros.Interfaces;
using SmartBook.Domain.Enums;
using SmartBook.Domain.Exceptions;
using SmartBook.Persistence.Repositories.Interface;

namespace SmartBook.Application.Services.Libros.Implementations
{
    public class ValidacionLibroService : IValidacionLibroService
    {
        private readonly ILibroRepository _libroRepository;

        public ValidacionLibroService(ILibroRepository libroRepository)
        {
            _libroRepository = libroRepository;
        }

        public void ValidarLibroNoExiste(string nombre, string nivel, TipoLibro tipoLibro, string edicion)
        {
            var existe = _libroRepository.ExisteLibro(nombre, nivel, tipoLibro, edicion);

            if (existe)
            {
                throw new BusinessRoleException("Ya existe un libro con la misma información ingresada");
            }
        }
    }
}