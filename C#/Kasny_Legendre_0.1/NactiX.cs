using System;
using System.Collections.Generic;
using System.Text;

namespace Kasny_Legendre_0._1
{
    partial class Program
    {
        private static float NactiX()//nezávisle proměnná
        {
            float x = 0;
            bool ok;
            do//hodnota x
            {
                Console.Write("Zadejte hodnotu x= ");
                string uzivatel = Console.ReadLine();

                ok = float.TryParse(uzivatel, out x);
                if (!ok)
                {
                    Console.WriteLine("Ujistěte se, že jste zadal(a) hodnotu z oboru reálných čísel.");
                    Console.ReadKey();
                }
            } while (!ok);
            return x;
        }
    }
}
