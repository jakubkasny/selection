using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasny_inverzni_matice
{
    partial class Program
    {
        private static float[,] CtiA()
        {
            int n = 0;
            float a = 0;
            bool ok = false;
            do
            {
                Console.Clear();
                Console.Write("Zadejte řád matice (n): ");
                string radek = Console.ReadLine();
                ok = int.TryParse(radek, out n);
                if (ok && n < 1)
                    ok = false;
                if (ok == false)
                {
                    Console.Write("Zadaná hodnota nebyla přijata, zadejte hodnotu v oboru přirozených čísel.");
                    Console.ReadKey();
                }
            } while (!ok);

            float[,] matA = new float[n, n];
            n = matA.GetLength(0);
            Console.WriteLine("Zadejte hodnoty matice");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    do
                    {
                        Console.Write("[{0},{1}]:", i+1, j+1);
                        string radek = Console.ReadLine();
                        ok = float.TryParse(radek, out a);
                        if (ok == true)
                            matA[i, j] = a;
                    } while (!ok);
                }
            }
            return matA;
        }
    }
}
