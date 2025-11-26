using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SmartBook.Domain.Dtos.Reponses.ClienteReponse;
using SmartBook.Domain.Dtos.Reponses.LibrosReponse;
using SmartBook.Domain.Dtos.Reponses.VentasReponses;
using SmartBook.Domain.Entities;
using SmartBook.Domain.Entities.DatabaseEntities;
using SmartBook.Domain.Enums;
using SmartBook.Persistence.Repositories.Interface;



namespace SmartBook.Persistence.Repositories;
public class VentaRepository : IVentaRepository
{
    private readonly IConfiguration _configuration;

    private readonly string _connectionString;

    public VentaRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("smarkbook");
    }
    private string Sql { get; set; }

    public void Crear(Venta venta)
    {


        using (var conexion = new MySqlConnection(_connectionString))
        {


            conexion.Open();
            Sql = @"INSERT INTO ventas (id_ventas,numero_recibo, fecha, id_cliente, id_usuario,id_libro,unidades,precio_unidad, observaciones) 
                             VALUES (@id_ventas,@NumeroRecibo, @Fecha, @IdCliente, @IdUsuario,@id_libro,@unidades,@precio_unidad, @Observaciones);
                            
                UPDATE libros SET stock = stock-@unidades, fecha_actualizacion = @actualizar WHERE (`id_libro` = @id_libro);";


            using (var cmd = new MySqlCommand(Sql, conexion))
            {

                cmd.Parameters.AddWithValue("@actualizar", DateTime.Now);
                cmd.Parameters.AddWithValue("@id_ventas", venta.Id);
                cmd.Parameters.AddWithValue("@NumeroRecibo", venta.NumeroReciboPago);
                cmd.Parameters.AddWithValue("@Fecha", venta.Fecha);
                cmd.Parameters.AddWithValue("@IdCliente", venta.ClienteId);
                cmd.Parameters.AddWithValue("@IdUsuario", venta.UsuarioId);
                cmd.Parameters.AddWithValue("@id_libro", venta.LibroId);
                cmd.Parameters.AddWithValue("@unidades", venta.Unidades);
                cmd.Parameters.AddWithValue("@precio_unidad", venta.Precio_unidad);
                cmd.Parameters.AddWithValue("@Observaciones", venta.Observaciones);
                if (cmd.ExecuteNonQuery() > 0)
                {


                }

            }

        }

    }

    public ConsultarVentaReponse ConsultarPrecio(string idlibro)
    {

        using (var conexion = new MySqlConnection(_connectionString))
        {

            conexion.Open();
            Sql = @"select fecha,id_libro,valorVenta,unidades from ingresos where id_libro=@identificacion";

            using (var cmd = new MySqlCommand(Sql, conexion))
            {

                cmd.Parameters.AddWithValue("@identificacion", idlibro);
                var reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {

                    return null;
                }

                reader.Read();
                var Id = "";
                var fecha_nacimiento = Convert.ToDateTime(reader["fecha"]);
                var id_cliente = "";
                var Idusuario = "";
                var id_libro = reader["id_libro"].ToString();
                var unidades = Convert.ToInt32(reader["unidades"]);
                var precio_unidad = Convert.ToDouble(reader["valorVenta"]);
                var observaciones = "";
                return new ConsultarVentaReponse(
                    Id,
                    idlibro,
                    fecha_nacimiento,
                    id_cliente,
                    Idusuario,
                    id_libro,
                    unidades,
                    precio_unidad,
                    observaciones

                );

            }

        }
    }
    public ConsultarVentaReponse Consultar(string numero_recibo)
    {

        using (var conexion = new MySqlConnection(_connectionString))
        {

            conexion.Open();
            Sql = @"select id_ventas,numero_recibo, fecha, id_cliente,id_usuario,id_libro,unidades,precio_unidad,observaciones from ventas where numero_recibo=@identificacion";

            using (var cmd = new MySqlCommand(Sql, conexion))
            {

                cmd.Parameters.AddWithValue("@identificacion", numero_recibo);
                var reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {

                    return null;
                }

                reader.Read();
                var Id = reader["id_ventas"].ToString();
                var fecha_nacimiento = Convert.ToDateTime(reader["fecha"]);
                var id_cliente = reader["id_cliente"].ToString();
                var Idusuario = reader["id_usuario"].ToString();
                var id_libro = reader["id_libro"].ToString();
                var unidades = Convert.ToInt32(reader["unidades"]);
                var precio_unidad = Convert.ToInt32(reader["precio_unidad"]);
                var observaciones = reader["observaciones"].ToString();
                return new ConsultarVentaReponse(
                    Id,
                    numero_recibo,
                    fecha_nacimiento,
                    id_cliente,
                    Idusuario,
                    id_libro,
                    unidades,
                    precio_unidad,
                    observaciones

                );

            }

        }
    }

    public IEnumerable<ConsultarVentaReponse> ConsultarPorCampos(DateTime? rangoFecha, DateTime? Hasta, string? cliente, string? libro)
    {
        var resultados = new List<ConsultarVentaReponse>();

        using var connection = new MySqlConnection(_connectionString);
        connection.Open();

        var query = @"SELECT id_ventas, numero_recibo, fecha, id_cliente, id_usuario, id_libro, unidades, precio_unidad, observaciones
                            FROM ventas
                            WHERE 
                                (
                                    @rangoFecha IS NULL 
                                    AND @Hasta IS NULL 
                                    AND @cliente IS NULL 
                                    AND @libro IS NULL
                                )
                                OR
                                (
                                    (@rangoFecha IS NOT NULL AND @Hasta IS NOT NULL AND fecha BETWEEN @rangoFecha AND @Hasta)
        
                                    OR (@cliente IS NOT NULL AND id_cliente LIKE CONCAT('%', @cliente, '%'))
        
                                    OR (@libro IS NOT NULL AND id_libro LIKE CONCAT('%', @libro, '%'))
                                );
                                    ";

        using var command = new MySqlCommand(query, connection);

        command.Parameters.AddWithValue("@rangoFecha", rangoFecha.HasValue ? rangoFecha.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@Hasta", Hasta.HasValue ? Hasta.Value : (object)DBNull.Value); 
        command.Parameters.AddWithValue("@cliente", string.IsNullOrEmpty(cliente) ? DBNull.Value : cliente);
        command.Parameters.AddWithValue("@libro", string.IsNullOrEmpty(libro) ? DBNull.Value : libro);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var venta = new ConsultarVentaReponse(
               idVenta: reader["id_ventas"].ToString()!,
                NumeroRecibo: reader["numero_recibo"].ToString()!,
                FechaVenta: Convert.ToDateTime(reader["fecha"]),
                IdCliente: reader["id_cliente"].ToString()!,
                IdUsuario: reader["id_usuario"].ToString()!,
                IdLibro: reader["id_libro"].ToString()!,
                unidades: Convert.ToInt32(reader["unidades"]),
                precio_unidad: Convert.ToDouble(reader["precio_unidad"]),
                Observaciones: reader["id_libro"].ToString()
            );


            resultados.Add(venta);
        }

        return resultados;
    }

}
