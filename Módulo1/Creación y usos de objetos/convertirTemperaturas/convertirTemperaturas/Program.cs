using convertirTemperaturas;

public class program
{
    public static void Main(string[] args)
    {
       
        TemperaturaFahrenheit tempF = new TemperaturaFahrenheit(100);
        TemperaturaCelsius tempC = tempF.ConvertirACelsius();

        Console.WriteLine($"Fahrenheit: {tempF}");
        Console.WriteLine($"Convertido a Celsius: {tempC}");

        Console.WriteLine($"-----------------------------");

        TemperaturaCelsius otraTempC = new TemperaturaCelsius(0);
        TemperaturaFahrenheit otraTempF = otraTempC.ConvertirAFahrenheit();

        Console.WriteLine($"Celsius: {otraTempC}");
        Console.WriteLine($"Convertido a Fahrenheit: {otraTempF}");
    }
}
