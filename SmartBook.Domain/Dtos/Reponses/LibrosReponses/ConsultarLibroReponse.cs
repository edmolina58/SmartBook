using SmartBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Dtos.Reponses.LibrosReponse;
public record ConsultarLibroReponse
(   string id,
    string nombre,
    string nive,
    TipoLibro tipolibro,
    int stock,
    string editorial,
    string edicion,
    DateTime fecha_creacion,
    string fecha_actualizacion


    );
