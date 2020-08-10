using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorytm_Evolucyjny
{
    class Subject
    {
        public byte Value { set; get; }
        private static Random rand = new Random();

        public Subject(byte v) { Value = v; }
        public Subject()
        {
            Value = (byte)rand.Next(256);
        }

        public override string ToString()
        {
            string res = "";
            byte x = Value;

            while (x != 0) { res = (x % 2 == 0 ? "0" : "1") + res; x /= 2; }

            if (res.Length < 8)
            {
                int y = 8 - res.Length;

                for (int i = 0; i < y; i++)
                {
                    res = "0" + res;
                }
            }

            return res;
        }

    }
}
