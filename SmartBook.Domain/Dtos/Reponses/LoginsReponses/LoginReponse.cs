
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Domain.Dtos.Reponses.LoginsReponse;
//para valirdar que iniciara sesion????
//Dejarlo aparte y mirar si es necesario añadirlo
public record  LoginReponse
(
    string Token,
    DateTime Expiracion
        
    );
