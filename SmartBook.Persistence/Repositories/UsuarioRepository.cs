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
                         (id_usuario, identificacion, password, nombreCompleto, correo, rol, 
                           fecha_creacion) 
                         VALUES (@id_usuario, @Identificacion, @password, @nombreCompleto, @correo, @rol,
                                 @fecha_creacion)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id_usuario", usuario.IdUsuario);
            command.Parameters.AddWithValue("@Identificacion", usuario.Identificacion);
            command.Parameters.AddWithValue("@password", usuario.Password);
            command.Parameters.AddWithValue("@nombreCompleto", usuario.NombreCompleto);
            command.Parameters.AddWithValue("@correo", usuario.Email);
            command.Parameters.AddWithValue("@rol", usuario.RolUsuario.ToString());
            command.Parameters.AddWithValue("@fecha_creacion", usuario.fecha_creacion);

            command.ExecuteNonQuery();
        }

  
       
        public Usuario ObtenerPorIdentificacion(string identificacion)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var query = @"SELECT id_usuario, identificacion, nombreCompleto, correo, password, rol
                  FROM usuarios
                  WHERE identificacion = @identificacion";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@identificacion", identificacion);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Usuario
            {
                IdUsuario = reader["id_usuario"].ToString()!,
                Identificacion = reader["identificacion"].ToString()!,
                NombreCompleto = reader["nombreCompleto"].ToString()!,
                Email = reader["correo"].ToString()!,
                Password = reader["password"].ToString()!,
                RolUsuario = Enum.Parse<RolUsuario>(reader["rol"].ToString()!)
            };
        }


        public void ActualizarDatos(Usuario usuario)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var query = @"UPDATE usuarios
                  SET nombreCompleto = @nombreCompleto,
                      correo = @correo,
                      rol = @rol,
                      fecha_actualizacion = @fecha_actualizacion
                  WHERE identificacion = @identificacion";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@nombreCompleto", usuario.NombreCompleto);
            command.Parameters.AddWithValue("@correo", usuario.Email);
            command.Parameters.AddWithValue("@rol", usuario.RolUsuario);
            command.Parameters.AddWithValue("@fecha_actualizacion", DateTime.Now);
            command.Parameters.AddWithValue("@identificacion", usuario.Identificacion);

            command.ExecuteNonQuery();
        }
        public Usuario IniciarUsuario(string email, string passwordHash)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var query = @"SELECT id_usuario, identificacion, nombreCompleto, correo, password, rol, 
                                 email_verificado, fecha_creacion
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
                IdUsuario = reader["id_usuario"].ToString()!,
                Identificacion = reader["identificacion"].ToString()!,
                NombreCompleto = reader["nombreCompleto"].ToString()!,
                Email = reader["correo"].ToString()!,
                Password = reader["password"].ToString()!,
                RolUsuario = Enum.Parse<RolUsuario>(reader["rol"].ToString()!),
                EmailVerificado = Convert.ToBoolean(reader["email_verificado"]),
                fecha_creacion = Convert.ToDateTime(reader["fecha_creacion"])
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
                IdUsuario = reader["id_usuario"].ToString()!,
                Identificacion = reader["identificacion"].ToString()!,
                NombreCompleto = reader["nombreCompleto"].ToString()!,
                Email = reader["correo"].ToString()!,
                Password = reader["password"].ToString()!,
                RolUsuario = Enum.Parse<RolUsuario>(reader["rol"].ToString()!) 
            };
        }

        public void Actualizar(Usuario usuario)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var query = @"UPDATE usuarios
                          SET password = @password, 
                              fecha_actualizacion = @fecha_actualizacion
                          WHERE correo = @correo";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@password", usuario.Password);
            command.Parameters.AddWithValue("@correo", usuario.Email);
            command.Parameters.AddWithValue("@fecha_actualizacion", usuario.fecha_actualizacion);

            command.ExecuteNonQuery();
        }

        public IEnumerable<ConsultarUsuarioReponse> ConsultarPorNombre(string nombreCompleto, RolUsuario? rolUsuario)
        {
            using (var conexion = new MySqlConnection(_connectionString))
            {
                conexion.Open();

                Sql = @"SELECT nombreCompleto, rol, fecha_creacion, fecha_actualizacion
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
                        var rolUsuarios = Enum.Parse<RolUsuario>(reader["rol"].ToString()!);
                        var fecha_creacion = Convert.ToDateTime(reader["fecha_creacion"]);

                        DateTime? fecha_actualizacion = reader["fecha_actualizacion"] != DBNull.Value
                            ? Convert.ToDateTime(reader["fecha_actualizacion"])
                            : null;

                        resultados.Add(new ConsultarUsuarioReponse(
                            listanombreCompleto!,
                            rolUsuarios,
                            fecha_creacion,
                            fecha_actualizacion ?? DateTime.MinValue
                        ));
                    }

                    return resultados;
                }
            }
        }

        public bool BorrarNoVerificado(string id)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            var query = "DELETE FROM usuarios WHERE id_usuario = @id_usuario";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id_usuario", id);

            return command.ExecuteNonQuery() > 0;
        }
        public bool ExistePorCorreo(string correo)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            var sql = "SELECT COUNT(*) FROM usuarios WHERE correo = @correo";
            using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@correo", correo);
            return Convert.ToInt64(cmd.ExecuteScalar()) > 0;
        }
    }
}