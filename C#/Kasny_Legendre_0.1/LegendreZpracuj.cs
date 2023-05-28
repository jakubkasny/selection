using System;
using System.Collections.Generic;
using System.Text;

namespace Kasny_Legendre_0._1
{
    partial class Program
    {
        private static double LegendreZpracuj(double x, int n, double[] xHodnoty, double[] yHodnoty)
        {
            double y = 0, L = 1;
            for (int i = 0; i < n; i++) //výpočet y = suma Li       
            {
                L = 1;
                for (int j = 0; j < n; j++)//výpočet Li
                {
                    if (j != i)
                    {
                        if (xHodnoty[i] == xHodnoty[j])
                            throw new Exception("Interpolační polynom neexistuje.");

                        L *= (x - xHodnoty[j]) / (xHodnoty[i] - xHodnoty[j]);
                    }
                }
                y += L * yHodnoty[i];
            }
            return y;
        }
    }
}
