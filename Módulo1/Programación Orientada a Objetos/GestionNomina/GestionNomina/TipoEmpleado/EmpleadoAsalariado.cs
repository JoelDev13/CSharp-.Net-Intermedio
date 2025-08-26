namespace GestionNomina.TipoEmpleado
{
    public class EmpleadoAsalariado : Empleado
    {
        private decimal salarioSemanal;

        public EmpleadoAsalariado(string nombre, decimal salarioSemanal) : base(nombre)
        {
            this.salarioSemanal = salarioSemanal;
        }

        public decimal SalarioSemanal { get { return salarioSemanal; } }

        public override decimal CalcularPago()
        {
            return salarioSemanal;
        }
    }
}
