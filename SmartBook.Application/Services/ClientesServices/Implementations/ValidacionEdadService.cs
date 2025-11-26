using SmartBook.Application.Services.Clientes.Interfaces;
using SmartBook.Domain.Exceptions;

namespace SmartBook.Application.Services.Clientes.Implementations
{
    public class ValidacionEdadService : IValidacionEdadService
    {
        public bool EsMayorDeEdad(DateOnly fechaNacimiento)
        {
            var edad = CalcularEdad(fechaNacimiento);
            return edad >= 14;
        }

        public void ValidarEdadMinima(DateOnly fechaNacimiento, int edadMinima = 14)
        {
            var edad = CalcularEdad(fechaNacimiento);

            if (edad < edadMinima)
            {
                throw new BusinessRoleException($"El cliente debe tener al menos {edadMinima} años");
            }
        }

        private int CalcularEdad(DateOnly fechaNacimiento)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var edad = hoy.Year - fechaNacimiento.Year;

            if (fechaNacimiento > hoy.AddYears(-edad))
            {
                edad--;
            }

            return edad;
        }
    }
}