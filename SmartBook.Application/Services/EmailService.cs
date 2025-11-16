using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SmartBook.Application.Services.Interface;
using SmartBook.Domain.Entities;

namespace SmartBook.Application.Services
{
    public class EmailService:IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task EnviarEmailVerificacionAsync(string destinatario, string nombre, string token)
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
                        .token-box {{ background-color: #f8f9fa; border: 2px dashed #007bff; padding: 15px; margin: 20px 0; border-radius: 5px; text-align: center; font-family: monospace; }}
                        .footer {{ margin-top: 30px; text-align: center; color: #666; font-size: 12px; border-top: 1px solid #ddd; padding-top: 20px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='logo'>
                            <h2>📚 SmartBook - CDI CECAR</h2>
                        </div>
                        <h3>Hola {nombre},</h3>
                        <p>Gracias por registrarte en SmartBook. Tu token de verificación es:</p>
                        
                        <div class='token-box'>
                            <strong style='font-size: 18px;'>{token}</strong>
                        </div>
                        
                        <p>Usa este token para verificar tu cuenta. El token es válido por 1 hora.</p>
                        <p style='color: #999; font-size: 12px;'>Si no solicitaste esta cuenta, puedes ignorar este correo.</p>
                        <div class='footer'>
                            <p><strong>Centro de Idiomas - CECAR</strong></p>
                            <p>Sincelejo, Sucre</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            await EnviarEmailAsync(destinatario, "Token de Verificación - SmartBook", htmlBody);
        }

        public async Task EnviarEmailConfirmacionCuentaAsync(string destinatario, string nombre)
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

            await EnviarEmailAsync(destinatario, "Cuenta verificada - SmartBook", htmlBody);
        }

        public async Task EnviarEmailRestablecimientoPasswordAsync(string destinatario, string nombre, string token)
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

            await EnviarEmailAsync(destinatario, "Restablecer Contraseña - SmartBook", htmlBody);
        }

        public async Task EnviarEmailConfirmacionRestablecimientoAsync(string destinatario, string nombre)
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

            await EnviarEmailAsync(destinatario, "Contraseña restablecida - SmartBook", htmlBody);
        }

        public async Task EnviarEmailAsync(string destinatario, string asunto, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("SmartBook CDI", _emailSettings.User));
            message.To.Add(new MailboxAddress("", destinatario));
            message.Subject = asunto;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port,
                    _emailSettings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                await client.AuthenticateAsync(_emailSettings.User, _emailSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}