using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace cryptography
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Please enter the no. of lines you may want to enter as input: ");
            LinkedList<string> list = new LinkedList<string>();
            short l;
            string a = Console.ReadLine();
            l = Int16.Parse(a);
            for (Int16 i = 0; i < l; i++)
            {
                string b = Console.ReadLine();
                list.AddLast(b);
            }
            foreach (string x in list)
            {
                BigInteger si, ei;
                string[] t= x.Split(' ');
                ei = BigInteger.Parse(t[1]);
                    si = BigInteger.Parse(t[0]);
                    if (ei < si)
                    {
                        BigInteger temp;
                        temp=ei ;
                        ei = si;
                        si = temp;
                    }
                    for (BigInteger i = (si+1); i < ei; i++)
                    {
                        
                        Int16 count = 0;
                       
                        
                            BigInteger r;
                            for (BigInteger j = 1; j <=(i/2); j++)
                            {
                                 r = (i % j);
                                
                                 if (r == 0)
                                     count++;
                                 if (count > 1)
                                     break;
  
                                }
                            if (count == 1)
                                Console.WriteLine(i);
                            else if (count == 0 && i == 1)
                                Console.WriteLine(i);
                           

                        }
                    Console.WriteLine();
                    }

            Console.ReadLine();
                
            }
        }

    }

