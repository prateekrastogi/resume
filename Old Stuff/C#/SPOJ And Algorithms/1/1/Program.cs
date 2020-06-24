using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace life
{
    class Program
    {
        static void Main(string[] args)
        {
            Random s = new Random(0);
            
            long b;
            LinkedList<Int64> list = new LinkedList<long>();
            do
            {
               
                //string a;
                //a = Console.ReadLine();
                //b = Int64.Parse(a);
                b = s.Next(46);
               if (b != 42)
                    list.AddLast(b);
                
            } while (b != 42);
            foreach (long x in list)
                Console.WriteLine(x);
            Console.ReadLine();
            
        }
    }
}
