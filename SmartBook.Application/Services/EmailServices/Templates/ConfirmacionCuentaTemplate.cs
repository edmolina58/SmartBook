using SmartBook.Application.Services.Email.Interfaces;

namespace SmartBook.Application.Services.Email.Templates
{
    public class ConfirmacionCuentaTemplate : IEmailTemplate
    {
        public string ObtenerAsunto() => "Cuenta verificada - SmartBook";

        public string GenerarHtml(Dictionary<string, string> datos)
        {
            var nombre = datos["nombre"];

            return $@"
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
                </html>";
        }
    }
}