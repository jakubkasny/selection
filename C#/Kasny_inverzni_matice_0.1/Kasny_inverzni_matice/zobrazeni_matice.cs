using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasny_inverzni_matice
{
    partial class Program
    {
        private static void ZobrazMatici(float[,] matA)
        {
            int n = matA.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(Math.Round(matA[i, j], 4) + " ");
                    string prvek = matA[i, j].ToString();
                    int delka = prvek.Length;
                    int pocetMezer = 10 - delka;
                    for (int k = 0; k < pocetMezer; k++)
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}
