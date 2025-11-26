using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SmartBook.Application.Services.Email.Interfaces;
using SmartBook.Application.Services.Email.Templates;
using SmartBook.Domain.Entities.DomainEntities;

namespace SmartBook.Application.Services.Email.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public void EnviarReciboVenta(string destinatario, long numeroRecibo, byte[] pdf)
        {
            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress("SmartBook", _emailSettings.User));
            mensaje.To.Add(new MailboxAddress("", destinatario));
            mensaje.Subject = $"Recibo de venta {numeroRecibo}";

            var body = new BodyBuilder
            {
                HtmlBody = "<p>En este mensaje encontrará el PDF de su compra.</p>"
            };

            body.Attachments.Add($"Recibo_{numeroRecibo}.pdf", pdf, new ContentType("application", "pdf"));

            mensaje.Body = body.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();
            client.Connect(_emailSettings.Host, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate(_emailSettings.User, _emailSettings.Password);
            client.Send(mensaje);
            client.Disconnect(true);
        }

        public void EnviarEmailVerificacionAsync(string destinatario, string nombre, string token)
        {
            var template = new VerificacionEmailTemplate();
            var datos = new Dictionary<string, string>
            {
                { "nombre", nombre },
                { "verificationLink", $"{_emailSettings.BaseUrl}/api/usuarios/verificar-email?token={Uri.EscapeDataString(token)}" }
            };

            var htmlBody = template.GenerarHtml(datos);
            EnviarEmailAsync(destinatario, template.ObtenerAsunto(), htmlBody);
        }

        public void EnviarEmailConfirmacionCuentaAsync(string destinatario, string nombre)
        {
            var template = new ConfirmacionCuentaTemplate();
            var datos = new Dictionary<string, string>
            {
                { "nombre", nombre }
            };

            var htmlBody = template.GenerarHtml(datos);
            EnviarEmailAsync(destinatario, template.ObtenerAsunto(), htmlBody);
        }

        public void EnviarEmailRestablecimientoPasswordAsync(string destinatario, string nombre, string token)
        {
            var template = new RestablecimientoPasswordTemplate();
            var datos = new Dictionary<string, string>
            {
                { "nombre", nombre },
                { "token", token }
            };

            var htmlBody = template.GenerarHtml(datos);
            EnviarEmailAsync(destinatario, template.ObtenerAsunto(), htmlBody);
        }

        public void EnviarEmailConfirmacionRestablecimientoAsync(string destinatario, string nombre)
        {
            var template = new ConfirmacionRestablecimientoTemplate();
            var datos = new Dictionary<string, string>
            {
                { "nombre", nombre }
            };

            var htmlBody = template.GenerarHtml(datos);
            EnviarEmailAsync(destinatario, template.ObtenerAsunto(), htmlBody);
        }

        public void EnviarEmailInicioSesionAsync(string destinatario, string nombre, string horaLocal)
        {
            var template = new InicioSesionTemplate();
            var datos = new Dictionary<string, string>
            {
                { "nombre", nombre },
                { "horaLocal", horaLocal }
            };

            var htmlBody = template.GenerarHtml(datos);
            EnviarEmailAsync(destinatario, template.ObtenerAsunto(), htmlBody);
        }

        private void EnviarEmailAsync(string destinatario, string asunto, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("SmartBook CDI", _emailSettings.User));
            message.To.Add(new MailboxAddress("", destinatario));
            message.Subject = asunto;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(_emailSettings.Host, _emailSettings.Port,
                    _emailSettings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                client.Authenticate(_emailSettings.User, _emailSettings.Password);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}