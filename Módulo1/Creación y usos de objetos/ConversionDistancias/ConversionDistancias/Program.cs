using ConversionDistancias;

public class Program
{
    static void Main()
    {
        var k = new Kilometros(10);
        var m = k.Millas();
        Console.WriteLine($"{k.Valor} km = {m.Valor} millas");

        var mi = new Millas(5);
        var km = mi.AKilometros();
        Console.WriteLine($"{mi.Valor} mi = {km.Valor} kilometros");
    }
}