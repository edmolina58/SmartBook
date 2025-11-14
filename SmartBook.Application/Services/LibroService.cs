
using Microsoft.Extensions.Configuration;
using SmartBook.Aplicacion.Extensions;
using SmartBook.Application.Interface;
using SmartBook.Domain.Dtos.Reponses.LibrosReponse;
using SmartBook.Domain.Dtos.Requests.LibroRequest;
using SmartBook.Domain.Dtos.Requests.LibrosRequest;
using SmartBook.Domain.Entities;
using SmartBook.Domain.Exceptions;
using SmartBook.Persistence.Repositories;
using SmartBook.Persistence.Repositories.Interface;
using System.Configuration;

namespace SmartBook.WebApi.Services;

public class LibroService : ILibroService
{

    private readonly ILibroRepository _librosRepository;
    private readonly IConfiguration _configuration;
    public LibroService(ILibroRepository libroRepository, IConfiguration configuration)
    {
        _configuration = configuration;
        _librosRepository = libroRepository;

    }
 
    public LibroReponse? Crear(CrearLibroRequest request)
    {
       var librosConElMismoNombre = _librosRepository.ExisteLibro(request.Nombre,request.Nivel,request.TipoLibro,request.Edicion);

        if (librosConElMismoNombre is true)
        {

            throw new BusinessRoleException("Ya existe un Libro  con la misma informacion ingresada");
        }
        
        var libro = new Libro
        {
            IdLibro = DateTime.Now.Ticks.ToString(),
            Nombre = request.Nombre.Sanitize().RemoveAccents().Sanitize().RemoveAccents(),
            Nivel = request.Nivel,
            TipoLibro = request.TipoLibro,
            Editorial =request.Editorial,
            Edicion = request.Edicion,
            Stock = request.stock,

        };


        _librosRepository.Crear(libro);

        return new LibroReponse(libro.IdLibro);

    }

    public bool Borrar(string id)
    {
        return _librosRepository.Borrar(id);
    }

    public ConsultarLibroReponse Consultar(string id)
    {
        return _librosRepository.Consultar(id);


    }

    public bool Actualizar(string id, ActualizarLibrosRequest request)
    {

        return _librosRepository.Actualizar(id, request);

    }


    public IEnumerable<ConsultarLibroReponse> ConsultarProductosCompletos(ConsultarLibroFiltrados request)
    {

        return _librosRepository.ConsultarPorCampos(request.Nombre,request.Nivel,request.TipoLibro,request.Edicion);
    }
}
