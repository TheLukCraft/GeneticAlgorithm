using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorytm_Evolucyjny
{
    class Environment
    {
        bool positive = true;
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        public Environment(double a, double b, double c)
        {
            A = a; B = b; C = c;
        }

        public void Change(double a, double b, double c)
        {
            A = a; B = b; C = c;
        }

        public double Check(Subject s)
        {
            return A * s.Value * s.Value + B * s.Value + C;
        }




        public override string ToString()
        {
            return $"f(x) = {A}x^2 + {B}x + {C}";
        }


    }
}
