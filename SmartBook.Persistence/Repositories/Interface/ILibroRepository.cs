using SmartBook.Domain.Dtos.Reponses.LibrosReponse;
using SmartBook.Domain.Entities;
using SmartBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Persistence.Repositories.Interface;
public interface ILibroRepository
{
    public bool ExisteLibro(string nombre, string nivel, TipoLibro tipo, string edicion);

    public void Crear(Libro Libro);
    public bool Borrar(string id);

    public ConsultarLibroReponse? Consultar(string id);

    public IEnumerable<ConsultarLibroReponse> ConsultarPorCampos(string? nombre, string? nivel, TipoLibro tipoLibro, string? edicion);
}
