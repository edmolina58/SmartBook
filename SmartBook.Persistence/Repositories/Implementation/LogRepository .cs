using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using SmartBook.Domain.Dtos.Reponses.LogsReponses;
using SmartBook.Domain.Enums;
using SmartBook.Persistence.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBook.Persistence.Repositories.Implementation;
public class LogRepository : ILogRepository
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public LogRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("SmarkBook")!;
    }

    private string sql { get; set; }

    public ConsultarLogResponse? ConsultarPorId(string idLog)
    {
        using (var conexion = new MySqlConnection(_connectionString))
        {
            conexion.Open();

            sql = @"SELECT id_log, fecha, tabla, operacion, id_registro, usuario_sistema, detalle, resultado 
                   FROM logs 
                   WHERE id_log = @idLog";

            using (var cmd = new MySqlCommand(sql, conexion))
            {
                cmd.Parameters.AddWithValue("@idLog", idLog);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new ConsultarLogResponse(
                        id_log: Convert.ToInt32(reader["id_log"]),
                        fechaLog: Convert.ToDateTime(reader["fecha"]),
                        tabla: reader["tabla"].ToString()!,
                        operacion: reader["operacion"].ToString()!,
                        id_registro: reader["id_registro"] as string,
                        usuario_sistema: reader["usuario_sistema"] as string,
                        detalle: reader["detalle"] as string,
                        resultados: Enum.Parse<ResultadosLogs>(reader["resultado"].ToString()!)
                    );
                }
            }
        }
    }

    public IEnumerable<ConsultarLogResponse> ConsultarPorUsario(string usuarioSistema)
    {
        var resultados = new List<ConsultarLogResponse>();

        using var conexion = new MySqlConnection(_connectionString);
        conexion.Open();

        sql = @"SELECT id_log, fecha, tabla, operacion, id_registro, usuario_sistema, detalle, resultado 
                FROM logs 
                WHERE @usuarioSistema=usuario_sistema";


        using var cmd = new MySqlCommand(sql, conexion);
        {
            cmd.Parameters.AddWithValue("@usuarioSistema", usuarioSistema);

            using var reader = cmd.ExecuteReader();
            {
                while (reader.Read())
                {
                    var log = new ConsultarLogResponse(
                        id_log: Convert.ToInt32(reader["id_log"]),
                        fechaLog: Convert.ToDateTime(reader["fecha"]),
                        tabla: reader["tabla"].ToString()!,
                        operacion: reader["operacion"].ToString()!,
                        id_registro: reader["id_registro"].ToString(),
                        usuario_sistema: reader["usuario_sistema"].ToString(),
                        detalle: reader["detalle"].ToString(),
                        resultados: Enum.Parse<ResultadosLogs>(reader["resultado"].ToString()!) 
                    );

                    resultados.Add(log);
                }
            }
        }

        return resultados;
    }
}