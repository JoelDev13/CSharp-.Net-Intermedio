using GestionNomina.TipoEmpleado;

namespace GestionNomina.Factory
{
    public class EmpleadoFactory
    {
        public static Empleado CrearEmpleadoAsalariado()
        {
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Salario semanal: ");
            decimal salario = Convert.ToDecimal(Console.ReadLine());

            return new EmpleadoAsalariado(nombre, salario);
        }

        public static Empleado CrearEmpleadoPorHora()
        {
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Salario por hora: ");
            decimal salarioHora = Convert.ToDecimal(Console.ReadLine());

            Console.Write("Horas trabajadas: ");
            decimal horas = Convert.ToDecimal(Console.ReadLine());

            return new EmpleadoPorHora(nombre, salarioHora, horas);
        }

        public static Empleado CrearEmpleadoPorComision()
        {
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Ventas brutas: ");
            decimal ventas = Convert.ToDecimal(Console.ReadLine());

            Console.Write("Tarifa de comision: ");
            decimal tarifa = Convert.ToDecimal(Console.ReadLine());

            return new EmpleadoPorComision(nombre, ventas, tarifa);
        }

        public static Empleado CrearEmpleadoAsalariadoConComision()
        {
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Salario base: ");
            decimal salarioBase = Convert.ToDecimal(Console.ReadLine());

            Console.Write("Ventas brutas: ");
            decimal ventas = Convert.ToDecimal(Console.ReadLine());

            Console.Write("Tarifa de comision: ");
            decimal tarifa = Convert.ToDecimal(Console.ReadLine());

            return new EmpleadoAsalariadoConComision(nombre, salarioBase, ventas, tarifa);
        }
    }
}
