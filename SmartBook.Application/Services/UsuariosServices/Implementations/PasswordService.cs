using SmartBook.Application.Services.Usuarios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartBook.Application.Services.Usuarios.Implementations;
public class PasswordService : IPasswordService
{
    public bool ValidarPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            return false;

        var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$");
        return regex.IsMatch(password);
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
    }

    public bool VerificarPassword(string passwordIngresado, string passwordHasheado)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(passwordIngresado, passwordHasheado);
        }
        catch
        {
            return false;
        }
    }
}
