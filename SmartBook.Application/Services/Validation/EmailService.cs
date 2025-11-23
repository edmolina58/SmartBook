using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SmartBook.Application.Services.Interface;
using SmartBook.Domain.Entities;

namespace SmartBook.Application.Services.Validation
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public void EnviarEmailVerificacionAsync(string destinatario, string nombre, string token)
        {
            var verificationLink = $"{_emailSettings.BaseUrl}/api/usuarios/verificar-email?token={Uri.EscapeDataString(token)}";

            var htmlBody = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <style>
                body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }}
                .container {{ max-width: 600px; margin: 50px auto; background: white; padding: 30px; border-radius: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }}
                .logo {{ text-align: center; margin-bottom: 20px; }}
                .logo h2 {{ color: #007bff; margin: 0; }}
                .button {{ display: inline-block; padding: 12px 24px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px; font-size: 16px; margin: 20px 0; }}
                .verification-link {{ background-color: #f8f9fa; border: 1px solid #dee2e6; padding: 15px; margin: 20px 0; border-radius: 5px; word-break: break-all; font-size: 14px; color: #495057; }}
                .footer {{ margin-top: 30px; text-align: center; color: #666; font-size: 12px; border-top: 1px solid #ddd; padding-top: 20px; }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='logo'>
                    <h2>📚 SmartBook - CDI CECAR</h2>
                </div>
                <h3>Hola {nombre},</h3>
                <p>Gracias por registrarte en SmartBook. Para verificar tu cuenta, haz clic en el siguiente botón:</p>
                
                <div style='text-align: center;'>
                    <a href='{verificationLink}' class='button'>Verificar Mi Cuenta</a>
                </div>
                
                <p>O copia y pega el siguiente enlace en tu navegador:</p>
                <div class='verification-link'>
                    {verificationLink}
                </div>
                
                <p>Este link de verificación es válido por 1 hora.</p>
                <p style='color: #999; font-size: 12px;'>Si no solicitaste esta cuenta, puedes ignorar este correo.</p>
                <div class='footer'>
                    <p><strong>Centro de Idiomas - CECAR</strong></p>
                    <p>Sincelejo, Sucre</p>
                </div>
            </div>
        </body>
        </html>
    ";

            EnviarEmailAsync(destinatario, "Verifica tu cuenta - SmartBook", htmlBody);
        }

        public void EnviarEmailConfirmacionCuentaAsync(string destinatario, string nombre)
        {
            var htmlBody = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; }}
                        .container {{ max-width: 600px; margin: 50px auto; background: white; padding: 30px; border-radius: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }}
                        .logo {{ text-align: center; margin-bottom: 20px; }}
                        .success {{ color: #28a745; font-size: 48px; text-align: center; }}
                        .footer {{ margin-top: 30px; text-align: center; color: #666; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='logo'>
                            <h2>📚 SmartBook - CDI CECAR</h2>
                        </div>
                        <div class='success'>✓</div>
                        <h3 style='text-align: center;'>¡Cuenta Verificada Exitosamente!</h3>
                        <p>Hola <strong>{nombre}</strong>,</p>
                        <p>Tu cuenta ha sido verificada correctamente. Ya puedes iniciar sesión en SmartBook.</p>
                        <div class='footer'>
                            <p><strong>Centro de Idiomas - CECAR</strong></p>
                            <p>Sincelejo, Sucre</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

             EnviarEmailAsync(destinatario, "Cuenta verificada - SmartBook", htmlBody);
        }

        public void  EnviarEmailRestablecimientoPasswordAsync(string destinatario, string nombre, string token)
        {
            var htmlBody = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }}
                        .container {{ max-width: 600px; margin: 50px auto; background: white; padding: 30px; border-radius: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }}
                        .logo {{ text-align: center; margin-bottom: 20px; }}
                        .token-box {{ background-color: #f8f9fa; border: 2px dashed #dc3545; padding: 15px; margin: 20px 0; border-radius: 5px; text-align: center; font-family: monospace; }}
                        .footer {{ margin-top: 30px; text-align: center; color: #666; font-size: 12px; border-top: 1px solid #ddd; padding-top: 20px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='logo'>
                            <h2>📚 SmartBook - CDI CECAR</h2>
                        </div>
                        <h3>Hola {nombre},</h3>
                        <p>Has solicitado restablecer tu contraseña. Tu token de restablecimiento es:</p>
                        
                        <div class='token-box'>
                            <strong style='font-size: 18px;'>{token}</strong>
                        </div>
                        
                        <p>Usa este token para restablecer tu contraseña. El token es válido por 1 hora.</p>
                        <p style='color: #999; font-size: 12px;'>Si no realizaste esta acción, contacta inmediatamente al administrador.</p>
                        <div class='footer'>
                            <p><strong>Centro de Idiomas - CECAR</strong></p>
                            <p>Sincelejo, Sucre</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

             EnviarEmailAsync(destinatario, "Restablecer Contraseña - SmartBook", htmlBody);
        }

        public void EnviarEmailConfirmacionRestablecimientoAsync(string destinatario, string nombre)
        {
            var htmlBody = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; }}
                        .container {{ max-width: 600px; margin: 50px auto; background: white; padding: 30px; border-radius: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }}
                        .logo {{ text-align: center; margin-bottom: 20px; }}
                        .success {{ color: #28a745; font-size: 48px; text-align: center; }}
                        .footer {{ margin-top: 30px; text-align: center; color: #666; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='logo'>
                            <h2>📚 SmartBook - CDI CECAR</h2>
                        </div>
                        <div class='success'>✓</div>
                        <h3 style='text-align: center;'>¡Contraseña Restablecida!</h3>
                        <p>Hola <strong>{nombre}</strong>,</p>
                        <p>Tu contraseña ha sido restablecida exitosamente.</p>
                        <p>Ahora puedes iniciar sesión con tu nueva contraseña.</p>
                        <div class='footer'>
                            <p><strong>Centro de Idiomas - CECAR</strong></p>
                            <p>Sincelejo, Sucre</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

             EnviarEmailAsync(destinatario, "Contraseña restablecida - SmartBook", htmlBody);
        }

        public void EnviarEmailInicioSesionAsync(string destinatario, string nombre, string horaLocal)
        {
            var htmlBody = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <style>
                body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }}
                .container {{ max-width: 600px; margin: 50px auto; background: white; padding: 30px; border-radius: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }}
                .logo {{ text-align: center; margin-bottom: 20px; }}
                .logo h2 {{ color: #007bff; margin: 0; }}
                .success {{ color: #28a745; font-size: 48px; text-align: center; }}
                .info-box {{ background-color: #e7f3ff; border-left: 4px solid #007bff; padding: 15px; margin: 20px 0; }}
                .footer {{ margin-top: 30px; text-align: center; color: #666; font-size: 12px; border-top: 1px solid #ddd; padding-top: 20px; }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='logo'>
                    <h2>📚 SmartBook - CDI CECAR</h2>
                </div>
                <div class='success'>✓</div>
                <h3 style='text-align: center;'>Inicio de Sesión Exitoso</h3>
                <p>Hola <strong>{nombre}</strong>,</p>
                <p>Has iniciado sesión en SmartBook.</p>
                
                <div class='info-box'>
                    <strong>📅 Fecha y hora:</strong> {horaLocal}
                </div>
                
                <p style='color: #999; font-size: 12px;'>Si no fuiste tú quien inició sesión, cambia tu contraseña inmediatamente y contacta al administrador.</p>
                
                <div class='footer'>
                    <p><strong>Centro de Idiomas - CECAR</strong></p>
                    <p>Sincelejo, Sucre</p>
                </div>
            </div>
        </body>
        </html>
    ";

            EnviarEmailAsync(destinatario, "Inicio de sesión - SmartBook", htmlBody);
        }

        public  void EnviarEmailAsync(string destinatario, string asunto, string htmlBody)
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
                 client.Authenticate (_emailSettings.User, _emailSettings.Password);
                 client.Send(message);
                 client.Disconnect(true);
            }
        }
    }
}