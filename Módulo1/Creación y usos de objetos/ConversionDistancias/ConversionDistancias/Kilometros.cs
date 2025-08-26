using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversionDistancias
{
    public class Kilometros
    {
        public double Valor { get; set; }

        public Kilometros(double valor)
        {
            Valor = valor;
        }

        
        public Millas Millas()
        {
            return new Millas(Valor / 1.60934);
        }
    }
}
