using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SmartBook.Domain.Dtos.Reponses.LibrosReponse;
using SmartBook.Domain.Dtos.Requests.LibroRequest;
using SmartBook.Domain.Entities;
using SmartBook.Domain.Enums;
using SmartBook.Persistence.Repositories.Interface;
namespace SmartBook.Persistence.Repositories;
public class LibroRepository : ILibroRepository
{
    private readonly IConfiguration _configuration;

    private readonly string _connectionString;

    public LibroRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("smarkbook");
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
                cmd.Parameters.AddWithValue("@tipo", tipo);
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

        var query = @"INSERT INTO Libros 
                         (id_Libro, Nombre, Nivel, TipoLibro, Editorial, Edicion, Stock, );

                  VALUES (@IdLibro, @Nombre, @Nivel, @TipoLibro, @Editorial, @Edicion,@Stock, @FechaCreacion;

                        update detalle_venta set unidades=detalle_venta.unidades+@Stock";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdLibro", Libro);
        command.Parameters.AddWithValue("@Nombre", Libro.Nombre);
        command.Parameters.AddWithValue("@Nivel", Libro.Nivel);
        command.Parameters.AddWithValue("@TipoLibro", Libro.TipoLibro);
        command.Parameters.AddWithValue("@Editorial", Libro.Editorial);
        command.Parameters.AddWithValue("@Edicion", Libro.Edicion);
        command.Parameters.AddWithValue("@Stock", Libro.Stock);

        command.ExecuteNonQuery();
    }

    public bool Borrar(string id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "DELETE FROM Libros WHERE IdLibro = @IdLibro";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdLibro", id);

        return command.ExecuteNonQuery() > 0;
    }

    public ConsultarLibroReponse? Consultar(string id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = "SELECT id_libro, nombre,nivel,stock,tipo,editorial,edicion FROM Libros WHERE id_libro = @IdLibro";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdLibro", id);

        using var reader = command.ExecuteReader();
        //Mapeo de la tabla al objeto
        var nombre = reader["nombre"].ToString();
        var nivel = reader["nivel"].ToString();
        var stock = Convert.ToInt32(reader["stock"]);
        var tipo = (TipoLibro)Convert.ToInt32(reader["tipo"]);
        var editorial = reader["editorial"].ToString();
        var edicion = reader["edicion"].ToString();

        return new ConsultarLibroReponse(
        id,
        nombre,
        nivel,
        tipo,
        stock,
       editorial,
       edicion
        );

       
    }


    public IEnumerable<ConsultarLibroReponse> ConsultarPorCampos(string? nombre, string? nivel, TipoLibro tipoLibro, string? edicion)
    {
        var resultados = new List<ConsultarLibroReponse>();

        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = @"
        SELECT id_libro, nombre, nivel, stock, tipo, editorial, edicion
        FROM libros
        WHERE (@nombre IS NULL OR nombre LIKE CONCAT('%', @nombre, '%'))
          AND (@nivel IS NULL OR nivel LIKE CONCAT('%', @nivel, '%'))
          AND (@tipoLibro IS NULL OR tipo = @tipoLibro)
          AND (@edicion IS NULL OR edicion LIKE CONCAT('%', @edicion, '%'))
    ";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@nombre", string.IsNullOrEmpty(nombre) ? DBNull.Value : nombre);
        command.Parameters.AddWithValue("@nivel", string.IsNullOrEmpty(nivel) ? DBNull.Value : nivel);
        command.Parameters.AddWithValue("@tipoLibro", tipoLibro == null ? DBNull.Value : tipoLibro.ToString());
        command.Parameters.AddWithValue("@edicion", string.IsNullOrEmpty(edicion) ? DBNull.Value : edicion);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var libro = new ConsultarLibroReponse(
                id: reader["id_libro"].ToString(),
                nombre: reader["nombre"].ToString(),
                nivel= reader["nivel"].ToString(),
                tipoLibro= Enum.TryParse(reader["tipo"].ToString(), out TipoLibro tipo) ? tipo : TipoLibro.StudensBoook,
                stock: Convert.ToInt32(reader["stock"]),
                editorial: reader["editorial"].ToString(),
                edicion: reader["edicion"].ToString()
            );

            resultados.Add(libro);
        }

        return resultados;
    }

    public bool Actualizar(string IdLibro, ActualizarLibrosRequest Libro)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = @"UPDATE Libros
                        SET 
                        nombre = @Nombre, nivel = @Nivel, tipo = @TipoLibro, editorial = @Editorial,
                        edicion = @edicion, stock = Libros.stock WHERE id_Libro = @IdLibro";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdLibro", IdLibro);
        command.Parameters.AddWithValue("@Nombre", Libro.Nombre);
        command.Parameters.AddWithValue("@Nivel", Libro.Nivel);
        command.Parameters.AddWithValue("@TipoLibro", Libro.TipoLibro);
        command.Parameters.AddWithValue("@Editorial", Libro.Editorial);
        command.Parameters.AddWithValue("@edicion", Libro.Edicion);

        return command.ExecuteNonQuery() > 0;
    }

}