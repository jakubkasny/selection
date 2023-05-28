using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasny_Legendre_0._1
{
    partial class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("LEGENDROVA INTERPOLACE" + Environment.NewLine);
                //načtení dat-> (záv. prom. x; počet hodnot n; uzlové body (xi,yi))
                double x = NactiX();
                int n = NactiN();
                double[] xHodnoty = new double[n];
                double[] yHodnoty = new double[n];
                NactiUzloveBody(ref xHodnoty, ref yHodnoty, n);

                //zpracování dat (n, xhodnoty, yhodnoty) vrací suma y= suma(Li*yi)
                double y = LegendreZpracuj(x, n, xHodnoty, yHodnoty);

                //vypsání dat
                LegendreVypis(y, x);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Dojde k ukončení programu.");
                Console.ReadKey();
            }
        }
    }
}
