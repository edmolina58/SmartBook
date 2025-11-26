using SmartBook.Application.Services.Email.Interfaces;

namespace SmartBook.Application.Services.Email.Templates
{
    public class RestablecimientoPasswordTemplate : IEmailTemplate
    {
        public string ObtenerAsunto() => "Restablecer Contraseña - SmartBook";

        public string GenerarHtml(Dictionary<string, string> datos)
        {
            var nombre = datos["nombre"];
            var token = datos["token"];

            return $@"
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
                        
                        <p>Usa este token para restablecer tu contraseña. El token es válido por 20 minutos.</p>
                        <p style='color: #999; font-size: 12px;'>Si no realizaste esta acción, contacta inmediatamente al administrador.</p>
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