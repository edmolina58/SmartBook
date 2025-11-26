using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Dtos.Requests.ClientesRequest;
public record ConsultarClienteFiltradoNombreRequest
([Required]
    string nombrecompleto
    
    );
