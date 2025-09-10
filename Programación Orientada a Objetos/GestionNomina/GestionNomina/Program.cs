using GestionNomina;

class Program
{
    static void Main(string[] args)
    {
        NominaManager manager = new NominaManager();
        MenuConsola menu = new MenuConsola(manager);
        menu.Mostrar();
    }
}