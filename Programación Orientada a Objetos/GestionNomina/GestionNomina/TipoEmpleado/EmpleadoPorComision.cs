namespace GestionNomina.TipoEmpleado
{
    public class EmpleadoPorComision : Empleado
    {
        public decimal VentasBrutas { get; private set; }
        public decimal TarifaComision { get; private set; }

        public EmpleadoPorComision(string nombre, decimal ventasBrutas, decimal tarifaComision) : base(nombre)
        {
            VentasBrutas = ventasBrutas;
            TarifaComision = tarifaComision;
        }

        public override decimal CalcularPago()
        {
            return VentasBrutas * TarifaComision;
        }
    }
}
