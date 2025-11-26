using SmartBook.Application.Services.Email.Interfaces;

namespace SmartBook.Application.Services.Email.Templates
{
    public class VerificacionEmailTemplate : IEmailTemplate
    {
        public string ObtenerAsunto() => "Verifica tu cuenta - SmartBook";

        public string GenerarHtml(Dictionary<string, string> datos)
        {
            var nombre = datos["nombre"];
            var verificationLink = datos["verificationLink"];

            return $@"
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
                        
                        <p>Este link de verificación es válido por 20 minutos.</p>
                        <p style='color: #999; font-size: 12px;'>Si no solicitaste esta cuenta, puedes ignorar este correo.</p>
                        <div class='footer'>
                            <p><strong>Centro de Idiomas - CECAR</strong></p>
                            <p>Sincelejo, Sucre</p>
                        </div>
                    </div>
                </body>
                </html>";
        }
    }
}