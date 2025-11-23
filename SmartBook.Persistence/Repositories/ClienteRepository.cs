using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SmartBook.Domain.Dtos.Reponses.ClienteReponse;
using SmartBook.Domain.Dtos.Requests.ClienteRequest;
using SmartBook.Domain.Entities;
using SmartBook.Persistence.Repositories.Interface;

namespace SmartBook.Persistence.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    private const string FORMATO_FECHA = "yyyy-MM-dd";
    public ClienteRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("smarkbook")!;
    }

    private string Sql { get; set; }

    public bool ExisteCliente(string identificacion)
    {
        using (var conexion = new MySqlConnection(_connectionString))
        {
            conexion.Open();

            Sql = @"SELECT COUNT(*) FROM clientes 
                WHERE identificacion = @identificacion";

            using (var cmd = new MySqlCommand(Sql, conexion))
            {
                cmd.Parameters.AddWithValue("@identificacion", identificacion);
                long cantidad = (long)cmd.ExecuteScalar();
                return cantidad > 0;
            }
        }
    }


    public bool Actulizar(string id, ActualizarClienteRequest request)
    {
        using (var conexion = new MySqlConnection(_connectionString))
        {
            conexion.Open();

            Sql = @"UPDATE clientes 
               SET fecha_nacimiento = @fecha_nacimiento, 
                   identificacion = @identificacion, 
                   nombreCompleto = @nombreCompleto, 
                   email = @email, 
                   celular = @celular,
                   fecha_actualizacion = @fecha_actualizacion
                   WHERE identificacion = @id";

            using (var cmd = new MySqlCommand(Sql, conexion))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@fecha_nacimiento", request.FechaNacimientoCliente.ToString(FORMATO_FECHA));
                cmd.Parameters.AddWithValue("@identificacion", request.identificacion);
                cmd.Parameters.AddWithValue("@nombreCompleto", request.NombreCliente);
                cmd.Parameters.AddWithValue("@email", request.EmailCliente);
                cmd.Parameters.AddWithValue("@celular", request.CelularCliente);
                cmd.Parameters.AddWithValue("@fecha_actualizacion", DateTime.Now);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }

    public void Crear(Cliente cliente)
    {
        using (var conexion = new MySqlConnection(_connectionString))
        {
            conexion.Open();
            Sql = @"INSERT INTO clientes (id_cliente, fecha_nacimiento, identificacion, nombreCompleto, email, celular, fecha_creacion) 
                    VALUES(@id_cliente, @fecha_nacimiento, @identificacion, @nombrecompleto, @email, @celular, @fecha_creacion)";

            using (var cmd = new MySqlCommand(Sql, conexion))
            {
                cmd.Parameters.AddWithValue("@id_cliente", cliente.IdCliente);
                cmd.Parameters.AddWithValue("@fecha_nacimiento", cliente.FechaNacimiento.ToString(FORMATO_FECHA));
                cmd.Parameters.AddWithValue("@identificacion", cliente.Identificacion);
                cmd.Parameters.AddWithValue("@nombrecompleto", cliente.Nombres);
                cmd.Parameters.AddWithValue("@email", cliente.Email);
                cmd.Parameters.AddWithValue("@celular", cliente.Celular);
                cmd.Parameters.AddWithValue("@fecha_creacion", cliente.fecha_creacion.ToString(FORMATO_FECHA));

                cmd.ExecuteNonQuery();
            }
        }
    }

    public ConsultarClienteReponse Consultar(string identificacion)
    {
        using (var conexion = new MySqlConnection(_connectionString))
        {
            conexion.Open();
            Sql = @"SELECT id_cliente, fecha_nacimiento, identificacion, nombreCompleto, email, 
                    celular, fecha_creacion, fecha_actualizacion 
                    FROM clientes 
                    WHERE identificacion = @identificacion";

            using (var cmd = new MySqlCommand(Sql, conexion))
            {
                cmd.Parameters.AddWithValue("@identificacion", identificacion);
                var reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    return null!;
                }

                reader.Read();

                var celular = reader["celular"].ToString();
                var fecha_nacimiento = DateOnly.FromDateTime(Convert.ToDateTime(reader["fecha_nacimiento"]));
                var id = reader["id_cliente"].ToString();
                var nombreCompleto = reader["nombreCompleto"].ToString();
                var email = reader["email"].ToString();
                var fechaCreacion = Convert.ToDateTime(reader["fecha_creacion"]);

                
                DateTime? fechaActualizacion = reader["fecha_actualizacion"] != DBNull.Value
                    ? Convert.ToDateTime(reader["fecha_actualizacion"])
                    : null;

                return new ConsultarClienteReponse(
                    id!,
                    identificacion,
                    nombreCompleto!,
                    email!,
                    celular!,
                    fecha_nacimiento,
                    fechaCreacion,
                    fechaActualizacion ?? DateTime.MinValue
                );
            }
        }
    }

    public IEnumerable<ConsultarClienteReponse> ConsultarPorNombre(string nombreCompleto)
    {
        using (var conexion = new MySqlConnection(_connectionString))
        {
            conexion.Open();

            Sql = @"SELECT id_cliente, fecha_nacimiento, identificacion, nombreCompleto, email, 
                    celular, fecha_creacion, fecha_actualizacion 
                    FROM clientes
                    WHERE nombreCompleto = @nombreCompleto";

            using (var cmd = new MySqlCommand(Sql, conexion))
            {
                cmd.Parameters.AddWithValue("@nombreCompleto", nombreCompleto);

                var reader = cmd.ExecuteReader();
                var resultados = new List<ConsultarClienteReponse>();

                while (reader.Read())
                {
                    var Id = reader["id_cliente"].ToString();
                    var fecha_nacimiento = DateOnly.FromDateTime(Convert.ToDateTime(reader["fecha_nacimiento"]));
                    var identificacion = reader["identificacion"].ToString();
                    var email = reader["email"].ToString();
                    var celular = reader["celular"].ToString();
                    var fechaCreacion = Convert.ToDateTime(reader["fecha_creacion"]);

                   
                    DateTime? fechaActualizacion = reader["fecha_actualizacion"] != DBNull.Value
                        ? Convert.ToDateTime(reader["fecha_actualizacion"])
                        : null;

                    resultados.Add(new ConsultarClienteReponse(
                        Id!,
                        identificacion!,
                        nombreCompleto,
                        email!,
                        celular!,
                        fecha_nacimiento,
                        fechaCreacion,
                        fechaActualizacion ?? DateTime.MinValue 
                    ));
                }

                return resultados;
            }
        }
    }
}