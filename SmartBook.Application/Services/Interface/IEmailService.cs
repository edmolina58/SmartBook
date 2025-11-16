using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Application.Services.Interface;
public interface IEmailService
{

    Task EnviarEmailVerificacionAsync(string destinatario, string nombre, string token);

     Task EnviarEmailConfirmacionCuentaAsync(string destinatario, string nombre);

     Task EnviarEmailRestablecimientoPasswordAsync(string destinatario, string nombre, string token);

     Task EnviarEmailConfirmacionRestablecimientoAsync(string destinatario, string nombre);

    Task EnviarEmailAsync(string destinatario, string asunto, string htmlBody);

}