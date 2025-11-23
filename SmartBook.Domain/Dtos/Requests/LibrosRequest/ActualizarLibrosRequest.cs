using SmartBook.Domain.Enums;

namespace SmartBook.Domain.Dtos.Requests.LibroRequest;

public record ActualizarLibrosRequest
(
    string Nombre,
    string Nivel,
    TipoLibro TipoLibro,
    string Editorial,
    string Edicion,
    DateTime fecha_creacion ,
    
    DateTime? fecha_actualizacion 

    );
