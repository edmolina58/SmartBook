using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Application.Services.ValidationServices.Interface;
public interface IEmailValidationService
{
    void EnviarEmailVerificacionAsync(string destinatario, string nombre, string token);
    void EnviarEmailConfirmacionCuentaAsync(string destinatario, string nombre);
    void EnviarEmailRestablecimientoPasswordAsync(string destinatario, string nombre, string token);
    void EnviarEmailConfirmacionRestablecimientoAsync(string destinatario, string nombre);
    void EnviarEmailInicioSesionAsync(string destinatario, string nombre, string horaLocal);
    void EnviarEmailAsync(string destinatario, string asunto, string htmlBody);
}
