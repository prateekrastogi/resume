using System;
using System.Collections.Generic;

namespace RPN
{
    class Program
    {
        private static void Main(string[] args)
        {
            string input;
            Console.Write("Please enter the no. of inputs you want to provide: ");
            input = Console.ReadLine();
            Int16 nl = Int16.Parse(input);
            List<string> list = new List<string>();
            for (int i = 0; i < nl; i++)
                list.Add(Console.ReadLine());

            #region Algorithm

            foreach (string i in list)
            {
                char[] ca = i.ToCharArray();

                Stack<char> sy = new Stack<char>();
                LinkedList<char> l = new LinkedList<char>();
                foreach (char m in ca)
                {
                    if (m == '+' || m == '-' || m == '*' || m == '/' || m == '^')
                        sy.Push(m);
                    else if (m != ')' || sy.Count == 0)
                        l.AddLast(m);
                    else
                    {
                        l.AddLast(sy.Pop());
                        l.AddLast(m);
                    }
                }
                foreach (char t in l)
                {
                    if (t == '(' || t == ')')
                        ;
                    else
                    {
                        Console.Write(t);
                    }
                }

            #endregion Algorithm
            }
            Console.ReadLine();
        }
}