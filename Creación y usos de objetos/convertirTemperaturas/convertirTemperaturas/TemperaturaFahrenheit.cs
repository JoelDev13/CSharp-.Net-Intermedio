using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace convertirTemperaturas
{
    public class TemperaturaFahrenheit
    {
        double GradoFahrenheit { get; set; }

        public TemperaturaFahrenheit(double gradoFahrenheit)
        {
            GradoFahrenheit = gradoFahrenheit;
        }

        public TemperaturaCelsius ConvertirACelsius()
        {
            double celsius = (GradoFahrenheit - 32) * 5 / 9;
            return new TemperaturaCelsius(celsius);
        }

        public override string ToString()
        {
            return $"{GradoFahrenheit} °F";
        }
    }
}
