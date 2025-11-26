namespace SmartBook.Application.Services.Clientes.Interfaces
{
    public interface IValidacionEdadService
    {
        bool EsMayorDeEdad(DateOnly fechaNacimiento);
        void ValidarEdadMinima(DateOnly fechaNacimiento, int edadMinima = 14);
    }
}