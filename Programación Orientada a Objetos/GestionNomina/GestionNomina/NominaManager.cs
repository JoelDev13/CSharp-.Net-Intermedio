namespace GestionNomina
{
    public class NominaManager
    {
        private List<Empleado> empleados = new List<Empleado>();

        public void AgregarEmpleado(Empleado empleado)
        {
            empleados.Add(empleado);
        }

        public void MostrarNomina()
        {
            Console.WriteLine("--- Nomina de la Semana ---");
            if (empleados.Count == 0)
            {
                Console.WriteLine("No hay empleados registrados");
                return;
            }

            foreach (var emp in empleados)
            {
                Console.WriteLine($"Empleado: {emp.Nombre} | Pago: {emp.CalcularPago()}");
            }
        }
    }
}
