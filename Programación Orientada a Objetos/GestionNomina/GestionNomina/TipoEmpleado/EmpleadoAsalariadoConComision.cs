namespace GestionNomina
{
    public class EmpleadoAsalariadoConComision : Empleado
    {
        public decimal SalarioBase { get; private set; }
        public decimal VentasBrutas { get; private set; }
        public decimal TarifaComision { get; private set; }
        private const decimal BONIFICACION = 0.10m;

        public EmpleadoAsalariadoConComision(string nombre, decimal salarioBase, decimal ventasBrutas, decimal tarifaComision) : base(nombre)
        {
            SalarioBase = salarioBase;
            VentasBrutas = ventasBrutas;
            TarifaComision = tarifaComision;
        }

        public override decimal CalcularPago()
        {
            decimal comision = VentasBrutas * TarifaComision;
            decimal bonificacion = SalarioBase * BONIFICACION;
            return SalarioBase + comision + bonificacion;
        }

        public decimal CalcularComision()
        {
            return VentasBrutas * TarifaComision;
        }

        public decimal CalcularBonificacion()
        {
            return SalarioBase * BONIFICACION;
        }
    }
}
