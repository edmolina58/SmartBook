using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SmartBook.Domain.Dtos.Reponses.IngresosReponses;
using SmartBook.Domain.Entities.DatabaseEntities;
using SmartBook.Persistence.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Persistence.Repositories.Implementation;
public class IngresoRepository : IIngresoRepository
{
    private readonly IConfiguration _configuration;

    private readonly string _connectionStrings;
    public string Sql { get; set; }
    private const string FORMATO_FECHA = "yyyy-MM-dd";
    public IngresoRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionStrings = _configuration.GetConnectionString("smarkbook")!;
    }



    public bool ExisteIngreso(string identificacion)
    {
        using (var conexion = new MySqlConnection(_connectionStrings))
        {
            conexion.Open();

            Sql = @"SELECT COUNT(*) FROM ingresos 
                WHERE idIngreso = @identificacion";

            using (var cmd = new MySqlCommand(Sql, conexion))
            {
                cmd.Parameters.AddWithValue("@identificacion", identificacion);
                long cantidad = (long)cmd.ExecuteScalar();
                return cantidad > 0;
            }
        }
    }

    public void Crear(Ingreso ingreso)
    {
       

        using (var conexion = new MySqlConnection(_connectionStrings))
        {

            conexion.Open();
            Sql = @"

                INSERT INTO `smartbook`.`ingresos` (`idIngreso`, `fecha`, `id_libro`, `lote`, `unidades`, `valorCompra`, `valorVenta`) 
                                            VALUES (@id, @fecha, @id_libro, @lote, @unidades, @valor_compra, @valor_venta);

                
                    UPDATE libros SET libros.stock =libros.stock+@unidades WHERE (libros.id_libro = @id_libro); ";


            using (var cmd = new MySqlCommand(Sql, conexion))
            {

            
                cmd.Parameters.AddWithValue("@id", ingreso.IdIngresos);
                cmd.Parameters.AddWithValue("@fecha", ingreso.Fecha.ToString(FORMATO_FECHA));

                cmd.Parameters.AddWithValue("@id_libro", ingreso.libro);
                cmd.Parameters.AddWithValue("@lote", ingreso.Lote);
                cmd.Parameters.AddWithValue("@unidades", ingreso.Unidades);
                cmd.Parameters.AddWithValue("@valor_compra", ingreso.ValorCompra);
                cmd.Parameters.AddWithValue("@valor_venta", ingreso.ValorVenta);

              
                cmd.ExecuteNonQuery();


            }

        }
    }




    public IEnumerable<ConsultarIngresosResponse> ConsultarPorFecha(DateOnly fechainicio, DateOnly fechaFin)
    {
        var ingresos = new List<ConsultarIngresosResponse>();


        using (var conexion = new MySqlConnection(_connectionStrings))
        {

            conexion.Open();

            Sql = @"SELECT idIngreso,id_libro,Unidades,valorCompra,valorVenta,fecha,lote FROM ingresos WHERE fecha BETWEEN @fechainicio AND @fechaFin";


            using (var cmd = new MySqlCommand(Sql, conexion))
            {
                cmd.Parameters.AddWithValue("@fechainicio", fechainicio.ToString(FORMATO_FECHA));
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin.ToString(FORMATO_FECHA));
                var reader = cmd.ExecuteReader();
                Sql = string.Empty;

                while (reader.Read())
                {

                    var idIngreso = reader["idIngreso"].ToString();
                    var Lote = reader["lote"].ToString();
                    var libro = reader["id_libro"].ToString();
                    var Unidades = Convert.ToInt32(reader["Unidades"]);
                    var valorCompra = Convert.ToDouble(reader["valorCompra"]);
                    var valorVenta = Convert.ToDouble(reader["valorVenta"]);
                    var Fecha = DateOnly.FromDateTime((DateTime)reader["fecha"]);


                    var ingreso = new ConsultarIngresosResponse(

                        idIngreso!,
                        Lote!,
                        libro!,
                        Unidades,
                        valorCompra,
                        valorVenta,
                        Fecha
                    );
                    ingresos.Add(ingreso);


                }

                return ingresos;

            }
        }
    }



    public ConsultarIngresosResponse Consultar(string idIngreso)
    {
        using (var conexion = new MySqlConnection(_connectionStrings))
        {
            conexion.Open();

            Sql = @"SELECT fecha,id_libro,lote,unidades,valorCompra,valorVenta FROM ingresos WHERE idIngreso = @id";

   
            using (var cmd = new MySqlCommand(Sql, conexion))
            {
                cmd.Parameters.AddWithValue("@id", idIngreso);
                var reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                    return null;

                reader.Read();

                var Lote = reader["lote"].ToString();
                var libro = reader["id_libro"].ToString();
                var Unidades = Convert.ToInt32(reader["Unidades"]);
                var valorCompra = Convert.ToDouble(reader["valorCompra"]);
                var valorVenta = Convert.ToDouble(reader["valorVenta"]);
                var Fecha = DateOnly.FromDateTime((DateTime)reader["fecha"]);

                return new ConsultarIngresosResponse(
                    idIngreso,
                    Lote!,
                    libro!,
                    Unidades,
                    valorCompra,
                    valorVenta,
                    Fecha
                );
            }
        }
    }

 

}
