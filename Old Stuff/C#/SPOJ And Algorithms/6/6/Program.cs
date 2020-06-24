using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Calculator
{
    #region Program
    class Program
    {
        #region Main
        static void Main(string[] args)
        {
            Console.Write("Please Enter The Number Of Input Lines:");
            Int16 nl = Int16.Parse(Console.ReadLine());
            List<string> list = new List<string>();
            for (int i = 0; i < nl; i++)                //input
            {
                list.Add(Console.ReadLine());
            }
            foreach (string x in list)
            {
                if (x.Contains('+'))
                {
                    Add(x);
                }
                else if (x.Contains('-'))
                {
                    Minus(x);                           //various operations to be performed based on input
                }
                else if (x.Contains('*'))
                {
                    Multiply(x);
                }
                else
                {
                    Console.WriteLine("Sorry,no valid operation mentioned");
                }
            } Console.ReadLine();                   //to be able to see output
        }
        #endregion
        #region Calculation
        static void Add(string x)
        {
            string[] s = x.Split('+');
            BigInteger[] b = new BigInteger[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                b[i] = BigInteger.Parse(s[i]);
            }
            BigInteger l = b[0] + b[1];
            Format(b[0], b[1], l, '+');

        }
        static void Minus(string x)
        {
            string[] s = x.Split('-');
            BigInteger l = new BigInteger();
            BigInteger[] b = new BigInteger[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                b[i] = BigInteger.Parse(s[i]);
            }
            if (b[0] >= b[1])
            {
                l = b[0] - b[1];
            }
            else
            {
                Console.WriteLine("Wrong Format");
            }
            Format(b[0], b[1], l, '-');
        }
        static void Multiply(string x)
        {
            string[] s = x.Split('*');
            BigInteger[] b = new BigInteger[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                b[i] = BigInteger.Parse(s[i]);
            }
            BigInteger l = b[0] * b[1];
            FormatM(b[0], b[1], l);
        }
        #endregion
        #region Output+-
        static void Format(BigInteger a, BigInteger b, BigInteger l, char c)
        {
            string t, p, q;
            t = a.ToString();           //All of them is converted to string
            p = b.ToString();
            q = l.ToString();
            int z, m, n;
            z = t.Length;
            m = p.Length;               //length of each of the item is found to calculate required spacing
            n = q.Length;
            if (z > m)
            {
                if (n == z + 1)
                {
                    Console.WriteLine(" {0}", t);
                }
                else
                {
                    Console.WriteLine(t);
                }
                for (int i = 0; i < (z - 1) - m; i++)   //formatted according to z>m condition
                {
                    Console.Write(" ");
                }
                Console.WriteLine("{0}{1}", c, p);
                for (int i = 0; i < (z); i++)
                {
                    Console.Write("-");
                }
                Console.WriteLine();
                if (n == z + 1)
                {
                    Console.WriteLine(l);
                }
                else if (n == z)
                {
                    Console.WriteLine("{0}", q);
                }

            }
            else
            {
                for (int i = 0; i < (m + 1) - z; i++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine(t);                           //formatted according to m>=z condition
                Console.WriteLine("{0}{1}", c, p);
                for (int i = 0; i < (m + 1); i++)
                {
                    Console.Write("-");
                }
                Console.WriteLine();
                if (n == m + 1)
                {
                    Console.WriteLine("{0}", l);
                }
                else
                {
                    Console.WriteLine(" {0}", q);
                }
            }
            Console.WriteLine();                               //new line seprator for each output
        }
        #endregion
        #region OutputM
        static void FormatM(BigInteger a, BigInteger b, BigInteger l)
        {
            string t, p, q;
            t = a.ToString();
            p = b.ToString();                   //same as in output+- but final will always contain greatest no.of digit 
            q = l.ToString();
            int z, m, n;
            z = t.Length;
            m = p.Length;
            n = q.Length;
            for (int i = 0; i < n - z; i++)
            {
                Console.Write(" ");
            }
            Console.WriteLine(t);
            for (int i = 0; i < n - (m + 1); i++)
            {
                Console.Write(" ");                 //formatting of the both inputs 
            }
            Console.WriteLine("*{0}", p);
            for (int i = 0; i < n - (m + 1); i++)
            {
                Console.Write(" ");
            }
            for (int i = 0; i < (m + 1); i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
            int count = 0;
            foreach (char x in p)
            {
                string y = x.ToString();
                BigInteger temp = BigInteger.Parse(y);
                BigInteger value = a * temp;
                string tem = value.ToString();              //splitting of multiplicand
                int space = tem.Length;
                for (int i = 0; i < n - (count + space); i++)
                {
                    Console.Write(" ");                     //formatting of each operation with the help of count
                }
                Console.WriteLine(tem);
                count++;
            }
            for (int i = 0; i < n; i++)
            {                                               //final horizontal line before the result
                Console.Write("-");
            }
            Console.WriteLine();
            Console.WriteLine(l);                          //final result and new line before output
            Console.WriteLine();
        }
        #endregion
    } 
    #endregion
}
