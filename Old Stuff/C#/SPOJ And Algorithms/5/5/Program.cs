using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Palindrome
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Please Enter the np. of lines you want Enter in test cases:");
            Int16 input = Int16.Parse(Console.ReadLine());
            LinkedList<string> ip = new LinkedList<string>();
            for (int i = 0; i < input; i++)
            {
                ip.AddLast(Console.ReadLine());
            }

            foreach (string x in ip)
            {
                FindNextPalindrome(x);
            }
            Console.ReadLine();
        }
        static void FindNextPalindrome(string x)
        {
            BigInteger b = BigInteger.Parse(x);
            bool test = false;
            b++;
            while (test == false)
            {
                b++;
                string w = b.ToString();
                char[] t = w.ToCharArray();
                for (int i = 0; i < t.Length; i++)
                {
                    int p = t.Length - (i + 1);
                    if (p >= i)
                    {
                        if (t[i] == t[p])
                        {
                            test = true;
                        }
                        else
                        {
                            test = false;
                            break;
                        }
                    }
                }
            }
            Console.WriteLine(b);
        }
    }
}