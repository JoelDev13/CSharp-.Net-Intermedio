namespace GestionNomina.TipoEmpleado
{
    public class EmpleadoPorHora : Empleado
    {
        public decimal SalarioPorHora { get; private set; }
        public decimal HorasTrabajadas { get; private set; }

        public EmpleadoPorHora(string nombre, decimal salarioPorHora, decimal horasTrabajadas) : base(nombre)
        {
            SalarioPorHora = salarioPorHora;
            HorasTrabajadas = horasTrabajadas;
        }

        public override decimal CalcularPago()
        {
            if (HorasTrabajadas <= 40)
            {
                return SalarioPorHora * HorasTrabajadas;
            }
            else
            {
                // Pago normal + tiempo extra 
                decimal pagoNormal = SalarioPorHora * 40;
                decimal horasExtra = HorasTrabajadas - 40;
                decimal pagoExtra = horasExtra * SalarioPorHora * 1.5m;
                return pagoNormal + pagoExtra;
            }
        }
    }
}
