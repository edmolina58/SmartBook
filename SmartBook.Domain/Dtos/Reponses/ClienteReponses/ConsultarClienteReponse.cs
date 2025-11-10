using SmartBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Dtos.Reponses.ClienteReponse;
public record ConsultarClienteReponse
(
    string id,
    string identificacion,
    string nombreCompleto,
    string email,
    string celular,
    DateOnly fechanacimiento
    
    );
