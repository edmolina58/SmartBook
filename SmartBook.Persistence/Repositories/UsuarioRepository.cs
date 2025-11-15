using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SmartBook.Domain.Dtos.Reponses.UsuariosReponses;
using SmartBook.Domain.Entities;
using SmartBook.Domain.Enums;
using SmartBook.Persistence.Repositories.Interface;

namespace SmartBook.Persistence.Repositories;
public class UsuarioRepository : IUsuarioRepository
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
    
    
    public bool ValidarCreacionUsuario(string identificacion)
    {

        using (var conexion = new MySqlConnection(_connectionString))
        {

            conexion.Open();
            // revisar y corregir aqui todo esta plano 
            Sql = @"SELECT COUNT(*) 
                    FROM usuarios 
                    WHERE identificacion =@identificacion";


            using (var cmd = new MySqlCommand(Sql, conexion))
            {

                cmd.Parameters.AddWithValue("@identificacion", identificacion);

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
                         (id_usuario, identificacion, password, nombreCompleto, correo, rol) 
                         VALUES (@Id, @Identificacion, @Contraseña, @Nombres, @Email, @Rol, @Estado, @FechaCreacion, @FechaActualizacion)";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id_usuario", usuario.IdUsuario);
        command.Parameters.AddWithValue("@Identificacion", usuario.Identificacion);
        command.Parameters.AddWithValue("@password", (usuario.Password)); // Encriptar contraseña
        command.Parameters.AddWithValue("@nombreCompleto", usuario.NombreCompleto);
        command.Parameters.AddWithValue("@correo", usuario.Email);
        command.Parameters.AddWithValue("@rol", usuario.RolUsuario);

        command.ExecuteNonQuery();
    }

    public bool Borrar(string id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "DELETE FROM usuarios WHERE id_usuario = @Id";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        return command.ExecuteNonQuery() > 0;
    }



    public IEnumerable<ConsultarUsuarioReponse> ConsultarPorNombre(string nombreCompleto, RolUsuario? rolUsuario)
    {
        using (var conexion = new MySqlConnection(_connectionString))
        {
            conexion.Open();
                                /*
                    id_usuario 
                    identificacion 
                    password 
                    nombreCompleto 
                    correo 
                    rol 
                    */

            Sql = @"SELECT nombreCompleto,rol
                 FROM usuarios
                WHERE  nombreCompleto = @nombreCompleto or rol=@rol";


            using (var cmd = new MySqlCommand(Sql, conexion))
            {
                cmd.Parameters.AddWithValue("@nombreCompleto", nombreCompleto);
                cmd.Parameters.AddWithValue("@rol", rolUsuario);

                var reader = cmd.ExecuteReader();

                var resultados = new List<ConsultarUsuarioReponse>();

                while (reader.Read())
                {
                    var listanombreCompleto = reader["nombreCompleto"].ToString();
                    var rolUsuarios = (RolUsuario)Convert.ToInt32(reader["rol"].ToString());  
                    resultados.Add(new ConsultarUsuarioReponse(
                    listanombreCompleto!,
                    rolUsuarios
                    ));
                }

                return resultados;
            }
        }
    }

    
     
}
