using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace convertirTemperaturas
{
    public class TemperaturaCelsius
    {
        double GradoCelsius {  get; set; }

        public TemperaturaCelsius(double gradoCelsius)
        {
            GradoCelsius = gradoCelsius;
        }
        public TemperaturaFahrenheit ConvertirAFahrenheit()
        {
            double fahrenheit = (GradoCelsius * 9 / 5) + 32;
            return new TemperaturaFahrenheit(fahrenheit);
        }
        public override string ToString()
        {
            return $"{GradoCelsius} °C";
        }
    }
}
