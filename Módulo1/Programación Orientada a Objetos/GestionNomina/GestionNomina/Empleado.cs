namespace GestionNomina
{
    public abstract class Empleado
    {
        private string nombre;

        public Empleado(string nombre)
        {
            this.nombre = nombre;
        }
        public string Nombre { get { return nombre; } }

        public abstract decimal CalcularPago();
    }
}
