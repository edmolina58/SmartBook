using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SmartBook.Domain.Dtos.Reponses.UsuariosReponses;
using SmartBook.Domain.Entities;
using SmartBook.Domain.Enums;
using SmartBook.Persistence.Repositories.Interface;

namespace SmartBook.Persistence.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SmarkBook");
        }

        private string Sql { get; set; }

        public bool ValidarCreacionUsuario(string identificacion)
        {
            using (var conexion = new MySqlConnection(_connectionString))
            {
                conexion.Open();
                Sql = @"SELECT COUNT(*) 
                        FROM usuarios 
                        WHERE identificacion = @identificacion";

                using (var cmd = new MySqlCommand(Sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@identificacion", identificacion);
                    return (long)cmd.ExecuteScalar() == 0;
                }
            }
        }

        public void Crear(Usuario usuario)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var query = @"INSERT INTO usuarios 
                             (id_usuario, identificacion, password, nombreCompleto, correo, rol) 
                             VALUES (@id_usuario, @Identificacion, @password, @nombreCompleto, @correo, @rol)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id_usuario", usuario.IdUsuario);
            command.Parameters.AddWithValue("@Identificacion", usuario.Identificacion);
            command.Parameters.AddWithValue("@password", usuario.Password);
            command.Parameters.AddWithValue("@nombreCompleto", usuario.NombreCompleto);
            command.Parameters.AddWithValue("@correo", usuario.Email);
            command.Parameters.AddWithValue("@rol", usuario.RolUsuario.ToString());

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

        public Usuario IniciarUsuario(string email, string passwordHash)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var query = @"SELECT id_usuario, identificacion, nombreCompleto, correo, password, rol
                          FROM usuarios
                          WHERE correo = @correo AND password = @password";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@correo", email);
            cmd.Parameters.AddWithValue("@password", passwordHash);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null!;

            return new Usuario
            {
                IdUsuario = reader["id_usuario"].ToString(),
                Identificacion = reader["identificacion"].ToString(),
                NombreCompleto = reader["nombreCompleto"].ToString(),
                Email = reader["correo"].ToString(),
                Password = reader["password"].ToString(),
                RolUsuario = Enum.Parse<RolUsuario>(reader["rol"].ToString())
            };
        }

        public Usuario ObtenerPorEmail(string email)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var query = @"SELECT id_usuario, identificacion, nombreCompleto, correo, password, rol
                          FROM usuarios
                          WHERE correo = @correo";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@correo", email);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Usuario
            {
                IdUsuario = reader["id_usuario"].ToString(),
                Identificacion = reader["identificacion"].ToString(),
                NombreCompleto = reader["nombreCompleto"].ToString(),
                Email = reader["correo"].ToString(),
                Password = reader["password"].ToString(),
                RolUsuario = Enum.Parse<RolUsuario>(reader["rol"].ToString())
            };
        }
        public void Actualizar(Usuario usuario)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var query = @"UPDATE usuarios
                  SET password = @password
                  WHERE correo = @correo";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@password", usuario.Password);
            command.Parameters.AddWithValue("@correo", usuario.Email);

            command.ExecuteNonQuery();
        }

        public IEnumerable<ConsultarUsuarioReponse> ConsultarPorNombre(string nombreCompleto, RolUsuario? rolUsuario)
        {
            using (var conexion = new MySqlConnection(_connectionString))
            {
                conexion.Open();

                Sql = @"SELECT nombreCompleto, rol
                     FROM usuarios
                    WHERE nombreCompleto = @nombreCompleto OR rol = @rol";

                using (var cmd = new MySqlCommand(Sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@nombreCompleto", nombreCompleto);
                    cmd.Parameters.AddWithValue("@rol", rolUsuario.HasValue ? rolUsuario.Value.ToString() : (object)DBNull.Value);

                    var reader = cmd.ExecuteReader();
                    var resultados = new List<ConsultarUsuarioReponse>();

                    while (reader.Read())
                    {
                        var listanombreCompleto = reader["nombreCompleto"].ToString();
                        var rolUsuarios = Enum.Parse<RolUsuario>(reader["rol"].ToString());

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
}
