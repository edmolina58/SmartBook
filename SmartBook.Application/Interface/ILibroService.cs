using SmartBook.Domain.Dtos.Reponses.LibrosReponse;
using SmartBook.Domain.Dtos.Requests.LibroRequest;
using SmartBook.Domain.Dtos.Requests.LibrosRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Application.Interface;
public interface ILibroService
{
     LibroReponse? Crear(CrearLibroRequest request);

     bool Borrar(string id);


    ConsultarLibroReponse Consultar(string id);
     bool Actualizar(string id, ActualizarLibrosRequest request);
    IEnumerable<ConsultarLibroReponse> ConsultarProductosCompletos(ConsultarLibroFiltradosRequest request);




}
