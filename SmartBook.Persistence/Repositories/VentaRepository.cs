using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SmartBook.Domain.Dtos.Reponses.ClienteReponse;
using SmartBook.Domain.Dtos.Reponses.VentasReponses;
using SmartBook.Domain.Entities;
using SmartBook.Persistence.Repositories.Interface;



namespace SmartBook.Persistence.Repositories;
public class VentaRepository : IVentaRepository
{
    private readonly string _connectionString;

    public VentaRepository(IConfiguration connectionString)
    {
        _connectionString = connectionString.GetSection("SmarBook").Value;
    }
    private string Sql { get; set; }





    public void Crear(Venta venta)
    {


        using (var conexion = new MySqlConnection(_connectionString))
        {


            conexion.Open();
            Sql = @"INSERT INTO ventas (id_ventas, numero_recibo, fecha, id_cliente, id_usuario,id_libro, observaciones) 
                             VALUES (@IdVenta, @NumeroRecibo, @Fecha, @IdCliente, @IdUsuario,@id_libro @Observaciones)";


            using (var cmd = new MySqlCommand(Sql, conexion))
            {

                //5 Remplazar valores

                //06 Ejecurar
                cmd.Parameters.AddWithValue("@IdVenta", venta.Id);
                cmd.Parameters.AddWithValue("@NumeroRecibo", venta.NumeroReciboPago);
                cmd.Parameters.AddWithValue("@Fecha", venta.Fecha);
                cmd.Parameters.AddWithValue("@IdCliente", venta.ClienteId);
                cmd.Parameters.AddWithValue("@IdUsuario", venta.UsuarioId);
                cmd.Parameters.AddWithValue("@id_libro", venta.LibroId);
                cmd.Parameters.AddWithValue("@Observaciones", venta.Observaciones);

                if (cmd.ExecuteNonQuery() > 0)
                {


                }

            }

        }

    }

    public ConsultarVentaReponse Consultar(string Id)
    {

        using (var conexion = new MySqlConnection(_connectionString))
        {

            conexion.Open();
            Sql = @"select numero_recibo, fecha, id_cliente,id_usuario,id_libro,observaciones from ventas where id_ventas=@id_ventas";

            using (var cmd = new MySqlCommand(Sql, conexion))
            {

                cmd.Parameters.AddWithValue("@identificacion", Id);
                var reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {

                    return null;
                }

                reader.Read();
                //Mapeo de la tabla al objeto
                var numero_recibo = Convert.ToInt32((reader["numero_recibo"]));
                var fecha_nacimiento = Convert.ToDateTime(reader["fecha"]);
                var id_cliente = reader["id_cliente"].ToString();
                var Idusuario = reader["id_usuario"].ToString();
                var id_libro = reader["id_libro"].ToString();
                var observaciones = reader["observaciones"].ToString();
                return new ConsultarVentaReponse(
                    Id,
                    numero_recibo,
                    fecha_nacimiento,
                    id_cliente,
                    Idusuario,
                    id_libro,
                    observaciones

                );

            }

        }
    }

    /*
    public IEnumerable<ConsultarVentaReponse> ConsultarFiltros(DateTime Fecha, string IdCliente, string IdLibro)
    {
        using (var conexion = new MySqlConnection(_connectionString))
        {
            conexion.Open();


            Sql = @"SELECT id_cliente,fecha_nacimiento,identificacion,nombreCompleto,email  
                ,celular FROM cliente 
                WHERE  nombreCompleto = @nombreCompleto";

            using (var cmd = new MySqlCommand(Sql, conexion))
            {
                cmd.Parameters.AddWithValue("@nombreCompleto", Fecha);
                cmd.Parameters.AddWithValue("@nombreCompleto", IdCliente);
                cmd.Parameters.AddWithValue("@nombreCompleto", IdLibro);

                var reader = cmd.ExecuteReader();

                var resultados = new List<ConsultarVentaReponse>();

                while (reader.Read())
                {
                    var Id = reader["id_cliente"].ToString();
                    var fecha_nacimiento = DateOnly.FromDateTime(Convert.ToDateTime(reader["fecha_nacimiento"]));
                    var identificacion = reader["identificacion"].ToString();
                    var email = reader["email"].ToString();
                    var celular = reader["celular"].ToString();
                    resultados.Add(new ConsultarVentaReponse(
                    Id,
                    identificacion,
                    nombreCompleto,
                    email,
                    celular,
                    fecha_nacimiento
                    ));
                }

                return resultados;
            }
        }
    }*/

}
