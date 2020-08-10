using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorytm_Evolucyjny
{
    class Population
    {
        public Subject[] Subjects { get; set; }
        public Environment Environment { get; set; }

        public static Random rand = new Random();

        public Population(Subject [] s){ Subjects = s; }
        public Population(UInt32 size)
        {
            if (size > 0)
            {
                Subjects = new Subject[size];
                for (UInt32 i = 0; i < Subjects.Length; i++) Subjects[i] = new Subject();
            }
            else
                throw new System.ArgumentException("Size of population must by grater than 0");
        }

        public Population(Subject s, UInt32 size)
        {
            Subjects = new Subject[size];

            for (UInt32 i = 0; i < size; i++)
                Subjects[i] = new Subject(s.Value);

            Subjects = Mutate(0.9, 0.2).Subjects;
        }

        public double Sum()
        {
            double res = 0;

            if (Environment != null)
            {
                foreach (Subject x in Subjects)
                    res += Environment.Check(x);
            }

            return res;
        }
        
        public Subject GetSubject(UInt32 x)
        {
            return (Subjects != null && x < Subjects.Length) ? Subjects[x] : null;
        }

        public double ValOfSubject(UInt32 x)
        {
            return (Environment != null) ? Environment.Check(Subjects[x]) : 0;
        }


        public Population Mutate(double x = 0.1, double y = 0.1)
        {
            Subject[] res = new Subject[Subjects.Length];

            x *= 10;
            y *= 10;

            for (int i = 0; i < Subjects.Length; i++)
            {
                double chanse = rand.Next(101);

                if (chanse <= x * 10)
                {
                    byte mask = 0;
                    for (int j = 0; j < 9; j++)
                    {
                        chanse = rand.Next(101);
                        if (chanse <= y * 10)
                            mask += (byte)(1 << j);
                    }
                    
                    res[i] = new Subject((byte)(Subjects[i].Value ^ mask));
                }
                else
                {
                    res[i] = new Subject(Subjects[i].Value);
                }
            }

            Population ret = new Population(res);
            ret.Environment = this.Environment;

            return ret;
        }



        public Population Cross(double chanse = 1)
        {
            if (Subjects.Length % 2 == 0)
            {
                Subject[] source = new Subject[Subjects.Length];

                source = Subjects.OrderBy(x => rand.Next()).ToArray();

                Subject[] res = new Subject[Subjects.Length];


                for (int i = 0; i < Subjects.Length; i += 2)
                {

                    byte mask = 0;

                    if (rand.NextDouble() < chanse) 
                    {
                        int point = rand.Next(6) + 1;

                        for (int j = 0; j < point; j++)
                        {
                            mask += (byte)(byte)(1 << j);
                        }
                        res[i] = new Subject((byte)((source[i].Value & mask) + (source[i + 1].Value & ~mask)));
                        res[i + 1] = new Subject((byte)((source[i].Value & ~mask) + (source[i + 1].Value & mask)));
                    }
                    else
                    {
                        res[i] = new Subject((byte)(source[i].Value));
                        res[i + 1] = new Subject((byte)(source[i+1].Value));
                    }


                }

                Population ret = new Population(res);
                ret.Environment = this.Environment;

                return ret;
            }

            return null;
        }


        public Subject Roulette()
        {
            double point = rand.NextDouble();

            point *= Sum();
            double sum = 0;

            foreach(Subject x in Subjects)
            {
                double val = Environment.Check(x);

                if (point >= sum && point <= sum + val)
                    return new Subject(x.Value);

                sum += val;
            }
            return Subjects.Last();

        }



    }
}
