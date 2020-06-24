using System;

namespace Substring
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Please Enter the input binary strings");
            string[] input = new string[3];

            for (int i = 0; i < input.Length; i++)
            {
                input[i] = Console.ReadLine();
            }

            foreach (string x in input)
            {
                string[] p = x.Split(' ');
                if (p[0].Contains(p[1]))
                    Console.WriteLine("1");
                else
                    Console.WriteLine("0");
            }
            Console.ReadLine();
        }
    }
}