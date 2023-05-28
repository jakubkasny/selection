using System;
using System.Collections.Generic;
using System.Text;

namespace Kasny_Legendre_0._1
{
    partial class Program
    {
        private static void NactiUzloveBody(ref double[] xhodnoty, ref double[] yhodnoty, int n)
        {
            bool ok;
            Console.WriteLine("Zadejte hodnoty uzlových bodů:");
            for (int m = 1; m <= n; m++)//načtení hodnot uzlových bodů
            {
                Console.Write("x({0})= ", m);
                ok = double.TryParse(Console.ReadLine(), out xhodnoty[m - 1]);
                if (!ok)
                    throw new Exception("Nasla chyba při zadávání hodnot.");

                Console.Write("y({0})= ", m);
                ok = double.TryParse(Console.ReadLine(), out yhodnoty[m - 1]);
                if (!ok)
                    throw new Exception("Nasla chyba při zadávání hodnot.");
                Console.WriteLine();
            }
        }
    }
}
