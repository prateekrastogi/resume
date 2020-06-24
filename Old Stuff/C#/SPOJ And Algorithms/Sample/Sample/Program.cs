using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("{0}", Experiment.prtk);
            Console.ReadLine();
        }
    }

    static class Experiment
    {
        static int x;
        static Experiment()
        {
            x = 96;
        }
        public static int prtk
        {
            get { return x; }
        }
    }
}