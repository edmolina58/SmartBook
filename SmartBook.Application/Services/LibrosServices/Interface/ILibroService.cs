using SmartBook.Domain.Dtos.Reponses.LibrosReponse;
using SmartBook.Domain.Dtos.Requests.LibroRequest;
using SmartBook.Domain.Dtos.Requests.LibrosRequest;

namespace SmartBook.Application.Services.Libros.Interfaces
{
    public interface ILibroService
    {
        LibroReponse? Crear(CrearLibroRequest request);
        bool Borrar(string id);
        ConsultarLibroReponse Consultar(string id);
        bool Actualizar(string id, ActualizarLibrosRequest request);
        IEnumerable<ConsultarLibroReponse> ConsultarProductosCompletos(ConsultarLibroFiltradosRequest request);
    }
}