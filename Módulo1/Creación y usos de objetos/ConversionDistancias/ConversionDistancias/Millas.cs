using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversionDistancias
{
    public class Millas
    {
        public double Valor { get; set; }

        public Millas(double valor)
        {
            Valor = valor;
        }

        public Kilometros AKilometros()
        {
            return new Kilometros(Valor * 1.60934);
        }
    }
}
