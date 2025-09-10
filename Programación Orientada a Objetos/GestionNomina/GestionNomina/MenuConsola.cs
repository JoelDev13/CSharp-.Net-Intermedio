using GestionNomina.Factory;

namespace GestionNomina
{
    public class MenuConsola
    {
        private readonly NominaManager nominaManager;

        public MenuConsola(NominaManager manager)
        {
            nominaManager = manager;
        }

        public void Mostrar()
        {
            int opcion;
            do
            {
               

                Console.WriteLine("--- Sistema de Gestion de Nomina ---");
                Console.WriteLine("1. Registrar Empleado Asalariado");
                Console.WriteLine("2. Registrar Empleado por Hora");
                Console.WriteLine("3. Registrar Empleado por Comisión");
                Console.WriteLine("4. Registrar Empleado Asalariado con Comisión");
                Console.WriteLine("5. Mostrar Nomina");
                Console.WriteLine("0. Salir");
                Console.Write("Seleciona una opcion: ");

                if (!int.TryParse(Console.ReadLine(), out opcion)) opcion = -1;

                switch (opcion)
                {
                    case 1:
                        nominaManager.AgregarEmpleado(EmpleadoFactory.CrearEmpleadoAsalariado());
                        break;
                    case 2:
                        nominaManager.AgregarEmpleado(EmpleadoFactory.CrearEmpleadoPorHora());
                        break;
                    case 3:
                        nominaManager.AgregarEmpleado(EmpleadoFactory.CrearEmpleadoPorComision());
                        break;
                    case 4:
                        nominaManager.AgregarEmpleado(EmpleadoFactory.CrearEmpleadoAsalariadoConComision());
                        break;
                    case 5:
                        nominaManager.MostrarNomina();
                        break;
                    case 0:
                        Console.WriteLine("Saliendo del sistema..."); 
                        break;
                    default:
                        Console.WriteLine("Opcion invalida");
                        break;
                }

            } while (opcion != 0);
            


        }
    }
}
