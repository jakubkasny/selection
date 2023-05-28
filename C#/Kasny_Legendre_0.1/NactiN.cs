using System;
using System.Collections.Generic;
using System.Text;

namespace Kasny_Legendre_0._1
{
    partial class Program
    {
        private static int NactiN()//počet uzlových bodů
        {
            bool ok;
            int n;
            do
            {
                Console.Write("Zadejte počet uzlových bodů= ");
                string uzivatel = Console.ReadLine();

                ok = int.TryParse(uzivatel, out n);
                if (!ok)
                {
                    Console.WriteLine("Ujistěte se, že jste zadal(a) hodnotu z oboru celých čísel.");
                    Console.ReadKey();
                }
            } while (!ok);

            Console.WriteLine();
            return n;
        }
    }
}
