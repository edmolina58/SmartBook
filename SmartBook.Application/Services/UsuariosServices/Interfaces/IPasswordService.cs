namespace SmartBook.Application.Services.Usuarios.Interfaces
{
    public interface IPasswordService
    {
        bool ValidarPassword(string password);
        string HashPassword(string password);
        bool VerificarPassword(string passwordIngresado, string passwordHasheado);
    }
}