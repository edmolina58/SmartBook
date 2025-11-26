using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SmartBook.Domain.Dtos.Reponses.LibrosReponse;
using SmartBook.Domain.Dtos.Requests.LibroRequest;
using SmartBook.Domain.Entities;
using SmartBook.Domain.Enums;
using SmartBook.Persistence.Repositories.Interface;
using System.Data;
namespace SmartBook.Persistence.Repositories.Implementation;
public class LibroRepository : ILibroRepository
{
    private readonly IConfiguration _configuration;

    private readonly string _connectionString;

    private const string FORMATO_FECHA = "yyyy-MM-dd";
    public LibroRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("smarkbook")!;

    }

    private string sql { get; set; }

    public bool ExisteLibro(string nombre, string nivel, TipoLibro tipo, string edicion)
    {
        using (var conexion = new MySqlConnection(_connectionString))
        {
            conexion.Open();

            sql = @"SELECT COUNT(*) 
                       FROM libros 
                       WHERE nombre = @nombre 
                         AND nivel = @nivel 
                         AND tipo = @tipo 
                         AND edicion = @edicion";

            using (var cmd = new MySqlCommand(sql, conexion))
            {
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@nivel", nivel);
                cmd.Parameters.AddWithValue("@tipo", tipo.ToString());
                cmd.Parameters.AddWithValue("@edicion", edicion);

                long cantidad = (long)cmd.ExecuteScalar();
                return cantidad > 0;
            }
        }
    }
    public void Crear(Libro Libro)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = @"INSERT INTO libros 
                         (id_libro, nombre, nivel, tipo, editorial, edicion, stock, fecha_creacion)
                  VALUES (@IdLibro, @Nombre, @Nivel, @TipoLibro, @Editorial, @Edicion, @Stock, @FechaCreacion)";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdLibro", Libro.IdLibro);
        command.Parameters.AddWithValue("@Nombre", Libro.Nombre);
        command.Parameters.AddWithValue("@Nivel", Libro.Nivel);
        command.Parameters.AddWithValue("@TipoLibro", Libro.TipoLibro);
        command.Parameters.AddWithValue("@Editorial", Libro.Editorial);
        command.Parameters.AddWithValue("@Edicion", Libro.Edicion);
        command.Parameters.AddWithValue("@Stock", Libro.Stock);
        command.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);

        command.ExecuteNonQuery();
    }

    public bool Borrar(string id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "DELETE FROM libros WHERE id_libro = @IdLibro";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdLibro", id);

        return command.ExecuteNonQuery() > 0;
    }

    public ConsultarLibroReponse Consultar(string id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT id_libro, nombre, nivel, stock, tipo, editorial, edicion, fecha_creacion, fecha_actualizacion FROM libros WHERE id_libro = @IdLibro";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdLibro", id);

        using var reader = command.ExecuteReader();
        if (!reader.Read()) return null;
        var nombre = reader["nombre"].ToString();
        var nivel = reader["nivel"].ToString();
        var stock = Convert.ToInt32(reader["stock"]);
        var tipo = Enum.Parse<TipoLibro>(reader["tipo"].ToString()!);
        var editorial = reader["editorial"].ToString();
        var edicion = reader["edicion"].ToString();
        var fecha_creacion = Convert.ToDateTime(reader["fecha_creacion"]);
        var fecha_actualizacion = reader["fecha_actualizacion"] != DBNull.Value ? reader["fecha_actualizacion"].ToString() : null;
        return new ConsultarLibroReponse(
                       id,
                       nombre!,
                       nivel!,
                       tipo,
                       stock,
                      editorial!,
                      edicion!,
                      fecha_creacion,
                      fecha_actualizacion
       );

    }


    public IEnumerable<ConsultarLibroReponse> ConsultarPorCampos(string? nombre, string? nivel, TipoLibro tipoLibro, string? edicion)
    {
        var resultados = new List<ConsultarLibroReponse>();

        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = @"
        SELECT id_libro, nombre, nivel, stock, tipo, editorial, edicion, fecha_creacion, fecha_actualizacion
        FROM libros
        WHERE (@nombre IS NULL OR nombre LIKE CONCAT('%', @nombre, '%'))
          AND (@nivel IS NULL OR nivel LIKE CONCAT('%', @nivel, '%'))
          AND (@tipoLibro IS NULL OR tipo = @tipoLibro)
          AND (@edicion IS NULL OR edicion LIKE CONCAT('%', @edicion, '%'))
    ";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@nombre", string.IsNullOrEmpty(nombre) ? DBNull.Value : nombre);
        command.Parameters.AddWithValue("@nivel", string.IsNullOrEmpty(nivel) ? DBNull.Value : nivel);
        command.Parameters.AddWithValue("@tipoLibro", tipoLibro.ToString());
        command.Parameters.AddWithValue("@edicion", string.IsNullOrEmpty(edicion) ? DBNull.Value : edicion);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var libro = new ConsultarLibroReponse(
                id: reader["id_libro"].ToString()!,
                nombre: reader["nombre"].ToString()!,
                nivel = reader["nivel"].ToString()!,
                tipoLibro = Enum.TryParse(reader["tipo"].ToString(), out TipoLibro tipo) ? tipo : TipoLibro.StudensBoook,
                stock: Convert.ToInt32(reader["stock"]),
                editorial: reader["editorial"].ToString() ?? string.Empty,
                edicion: reader["edicion"].ToString()!,
                fecha_creacion: Convert.ToDateTime(reader["fecha_creacion"]),
                fecha_actualizacion: reader["fecha_actualizacion"].ToString() ?? string.Empty
            );

            resultados.Add(libro);
        }

        return resultados;
    }

    public bool Actualizar(string IdLibro, ActualizarLibrosRequest Libro)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = @"UPDATE libros
                        SET 
                        nombre = @Nombre, nivel = @Nivel, tipo = @TipoLibro, editorial = @Editorial,
                        edicion = @edicion, stock = @Stock, fecha_actualizacion = @fecha_actualizacion
                        WHERE id_libro = @IdLibro";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdLibro", IdLibro);
        command.Parameters.AddWithValue("@Nombre", Libro.Nombre);
        command.Parameters.AddWithValue("@Nivel", Libro.Nivel);
        command.Parameters.AddWithValue("@TipoLibro", Libro.TipoLibro.ToString());
        command.Parameters.AddWithValue("@Editorial", Libro.Editorial ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@edicion", Libro.Edicion);
        command.Parameters.AddWithValue("@Stock", Libro.stock);
        command.Parameters.AddWithValue("@fecha_actualizacion", DateTime.Now);

        return command.ExecuteNonQuery() > 0;
    }

}