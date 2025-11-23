using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SmartBook.Persistence.Repositories.Interface;
using SmartBook.Domain.Entities;

namespace SmartBook.Application.Services
{
    public class LimpiadorCuentasPendientesService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _intervaloRevision = TimeSpan.FromMinutes(2);

        private static readonly List<CuentaPendiente> _cuentasPendientes = new();
        private static readonly object _lock = new();

        public LimpiadorCuentasPendientesService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await RevisarYEliminarCuentasExpiradas();
                await Task.Delay(_intervaloRevision, stoppingToken);
            }
        }

        private async Task RevisarYEliminarCuentasExpiradas()
        {
            try
            {
                lock (_lock)
                {
                    var ahora = DateTime.UtcNow;
                    var cuentasAEliminar = _cuentasPendientes
                        .Where(c => !c.Verificada && (ahora - c.FechaCreacion).TotalMinutes > 20)
                        .ToList();

                    if (cuentasAEliminar.Any())
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var usuarioRepository = scope.ServiceProvider.GetRequiredService<IUsuarioRepository>();

                            foreach (var cuenta in cuentasAEliminar)
                            {
                                usuarioRepository.BorrarNoVerificado(cuenta.Identificacion);
                                _cuentasPendientes.Remove(cuenta);
                                Console.WriteLine($"[LIMPIEZA] Cuenta eliminada: {cuenta.Email} (Identificación: {cuenta.Identificacion})");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error al limpiar cuentas: {ex.Message}");
            }
        }

        public static void AgregarCuentaPendiente(string identificacion, string email)
        {
            lock (_lock)
            {
                _cuentasPendientes.Add(new CuentaPendiente
                {
                    Identificacion = identificacion,
                    Email = email,
                    FechaCreacion = DateTime.UtcNow,
                    Verificada = false
                });
            }
        }

        public static void MarcarComoVerificada(string email)
        {
            lock (_lock)
            {
                var cuenta = _cuentasPendientes.FirstOrDefault(c => c.Email == email);
                if (cuenta != null)
                {
                    // Como CuentaPendiente es inmutable (propiedades init), hay que reemplazar el objeto
                    var nuevaCuenta = new CuentaPendiente
                    {
                        Identificacion = cuenta.Identificacion,
                        Email = cuenta.Email,
                        FechaCreacion = cuenta.FechaCreacion,
                        Verificada = true
                    };
                    _cuentasPendientes.Remove(cuenta);
                    _cuentasPendientes.Add(nuevaCuenta);
                }
            }
        }
        public static bool EstaVerificado(string email)
        {
            lock (_lock)
            {
                var cuenta = _cuentasPendientes.FirstOrDefault(c => c.Email == email);
                return cuenta == null || cuenta.Verificada;
            }
        }
    }
}