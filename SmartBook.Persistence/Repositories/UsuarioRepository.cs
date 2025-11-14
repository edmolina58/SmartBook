using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using SmartBook.Domain.Dtos.Requests.UsuarioRequest;
using SmartBook.Domain.Entities;
using SmartBook.Domain.Enums;

namespace SmartBook.Persistence.Repositories;
public class UsuarioRepository
{
    private readonly IConfiguration _configuration;

    private readonly string _connectionString;

    public UsuarioRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("smarkbook");
    }


    private string Sql { get; set; }
    //valida la creacion del Cliente
    public bool ValidarCreacionCliente(string nombreCliente)
    {

        using (var conexion = new MySqlConnection(_connectionString))
        {

            conexion.Open();
            // revisar y corregir aqui todo esta plano 
            Sql = @"SELECT COUNT(id) 
                    FROM Clientes 
                    WHERE nombre =@nombre  AND estado = 1";


            using (var cmd = new MySqlCommand(Sql, conexion))
            {

                cmd.Parameters.AddWithValue("@nombre", nombreCliente);

                return (long)cmd.ExecuteScalar() == 0;

            }
        }

    }

    //
    public void Crear(Usuario usuario)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = @"INSERT INTO usuarios 
                         (id, identificacion, contraseña, nombres, email, rol, estado, fecha_creacion, fecha_actualizacion) 
                         VALUES (@Id, @Identificacion, @Contraseña, @Nombres, @Email, @Rol, @Estado, @FechaCreacion, @FechaActualizacion)";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", usuario.IdUsuario);
        command.Parameters.AddWithValue("@Identificacion", usuario.Identificacion);
        command.Parameters.AddWithValue("@Contraseña", (usuario.Password)); // Encriptar contraseña
        command.Parameters.AddWithValue("@Nombres", usuario.Nombre);
        command.Parameters.AddWithValue("@Email", usuario.Email);
        command.Parameters.AddWithValue("@Rol", usuario.RolUsuario.ToString());
        command.Parameters.AddWithValue("@Estado", usuario.EstadoUsuario.ToString());
        command.Parameters.AddWithValue("@FechaCreacion", usuario.FechaCreacion);
        command.Parameters.AddWithValue("@FechaActualizacion", usuario.FechaActualizacion);

        command.ExecuteNonQuery();
    }

    public bool Borrar(string id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "DELETE FROM usuarios WHERE id = @Id";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        return command.ExecuteNonQuery() > 0;
    }

    public Usuario? Consultar(string id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT * FROM usuarios WHERE id = @Id";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return MapToUsuario(reader);
        }

        return null;
    }
    private Usuario MapToUsuario(MySqlDataReader reader)
    {
        return new Usuario(
            reader["identificacion"].ToString(),
            reader["contraseña"].ToString(), // Contraseña ya encriptada
            reader["nombres"].ToString(),
            reader["email"].ToString(),
            Enum.Parse<RolUsuario>(reader["rol"].ToString())
        )
        {
            IdUsuario = reader["id"].ToString(),
            EstadoUsuario = Enum.Parse<EstadoUsuario>(reader["estado"].ToString()),
            FechaCreacion = Convert.ToDateTime(reader["fecha_creacion"]),
            FechaActualizacion = Convert.ToDateTime(reader["fecha_actualizacion"])
        };
    }
}
