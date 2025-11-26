using SmartBook.Aplicacion.Extensions;
using SmartBook.Application.Services.Libros.Interfaces;
using SmartBook.Domain.Dtos.Reponses.LibrosReponse;
using SmartBook.Domain.Dtos.Requests.LibroRequest;
using SmartBook.Domain.Dtos.Requests.LibrosRequest;
using SmartBook.Domain.Entities;
using SmartBook.Persistence.Repositories.Interface;

namespace SmartBook.Application.Services.Libros.Implementations
{
    public class LibroService : ILibroService
    {
        private readonly ILibroRepository _libroRepository;
        private readonly IValidacionLibroService _validacionLibroService;

        public LibroService(
            ILibroRepository libroRepository,
            IValidacionLibroService validacionLibroService)
        {
            _libroRepository = libroRepository;
            _validacionLibroService = validacionLibroService;
        }

        public LibroReponse? Crear(CrearLibroRequest request)
        {
            var nombreNormalizado = request.Nombre.Sanitize().RemoveAccents();
            var nivelNormalizado = request.Nivel.Sanitize().RemoveAccents();
            var editorialNormalizada = request.Editorial.Sanitize().RemoveAccents();
            var edicionNormalizada = request.Edicion.Sanitize().RemoveAccents();

            _validacionLibroService.ValidarLibroNoExiste(
                nombreNormalizado,
                nivelNormalizado,
                request.TipoLibro,
                edicionNormalizada
            );

            var libro = new Libro
            {
                IdLibro = DateTime.Now.Ticks.ToString(),
                Nombre = nombreNormalizado,
                Nivel = nivelNormalizado,
                TipoLibro = request.TipoLibro,
                Editorial = editorialNormalizada,
                Edicion = edicionNormalizada,
                Stock = request.stock,
                fecha_creacion = DateTime.Now
            };

            _libroRepository.Crear(libro);

            return new LibroReponse(libro.IdLibro);
        }

        public bool Borrar(string id)
        {
            return _libroRepository.Borrar(id);
        }

        public ConsultarLibroReponse Consultar(string id)
        {
            return _libroRepository.Consultar(id)!;
        }

        public bool Actualizar(string id, ActualizarLibrosRequest request)
        {
            var libroActualizado = new ActualizarLibrosRequest
            (
                Nombre: request.Nombre.Sanitize().RemoveAccents(),
                Nivel: request.Nivel.Sanitize().RemoveAccents(),
                TipoLibro: request.TipoLibro,
                Editorial: request.Editorial.Sanitize().RemoveAccents(),
                Edicion: request.Edicion.Sanitize().RemoveAccents(),
                stock: request.stock
            );

            return _libroRepository.Actualizar(id, libroActualizado);
        }

        public IEnumerable<ConsultarLibroReponse> ConsultarProductosCompletos(ConsultarLibroFiltradosRequest request)
        {
            return _libroRepository.ConsultarPorCampos(
                request.Nombre.Sanitize().RemoveAccents(),
                request.Nivel,
                request.TipoLibro,
                request.Edicion.Sanitize().RemoveAccents()
            );
        }
    }
}