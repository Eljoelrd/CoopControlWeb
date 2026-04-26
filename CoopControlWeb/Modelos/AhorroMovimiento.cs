using System;

namespace CoopControlWeb.Modelos
{
    public enum MovimientoTipo
    {
        Ingreso = 0,
        Retiro = 1,
        Transferencia = 2
    }

    public class AhorroMovimiento
    {
        public int Id { get; set; }

        // FK opcional al Ahorro (null cuando el movimiento representa ingreso a "retirables")
        public int? AhorroId { get; set; }
        public Ahorro? Ahorro { get; set; }

        // FK opcional al socio (útil para movimientos que representan "retirables")
        public int? SocioId { get; set; }
        public Socios? Socio { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public decimal Monto { get; set; }
        public MovimientoTipo Tipo { get; set; }
        public string? Observaciones { get; set; }
        public string? UsuarioRegistro { get; set; }
    }
}