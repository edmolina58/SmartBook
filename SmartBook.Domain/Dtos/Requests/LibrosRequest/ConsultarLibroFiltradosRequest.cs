using SmartBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Dtos.Requests.LibrosRequest;
public record ConsultarLibroFiltradosRequest
(
    string? Nombre
    , string? Nivel, TipoLibro TipoLibro, string? Edicion


    );
