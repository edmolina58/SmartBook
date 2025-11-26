namespace SmartBook.Application.Services.Email.Interfaces
{
    public interface IEmailService
    {
        void EnviarEmailVerificacionAsync(string destinatario, string nombre, string token);
        void EnviarEmailConfirmacionCuentaAsync(string destinatario, string nombre);
        void EnviarEmailRestablecimientoPasswordAsync(string destinatario, string nombre, string token);
        void EnviarEmailConfirmacionRestablecimientoAsync(string destinatario, string nombre);
        void EnviarEmailInicioSesionAsync(string destinatario, string nombre, string horaLocal);

        void EnviarReciboVenta(string destinatario, long numeroRecibo, byte[] pdf);
    }
}