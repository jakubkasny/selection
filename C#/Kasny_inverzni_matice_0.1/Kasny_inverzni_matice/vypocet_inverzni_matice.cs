using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasny_inverzni_matice
{
    partial class Program
    {
        private static float[,] InverzeMatice(ref float[,] matA)
        {
            float[,] invMat = GenerujJednotkovouMatici(matA.GetLength(0));
            int n = matA.GetLength(0);
            float x;

            if (n == 1)
            {
                if (matA[0, 0] == 0)
                    return matA;

                matA[0, 0] = 1 / matA[0, 0];
                return matA;
            }

            for (int i = 0; i < n - 1; i++)//nulování dolního trojúhelníku
            {
                for (int j = n - 1; j > i; j--)
                {
                    if (matA[i, i] != 0)
                    {
                        x = -(matA[j, i] / matA[i, i]);
                        OdectiRadekMatice(x, j, i, ref matA);
                        OdectiRadekMatice(x, j, i, ref invMat);
                    }
                }
            }

            //kontrola determinantu 
            float determinant = 1;
            for (int i = 0; i < n; i++)
            {
                determinant *= matA[i, i];
            }
            if (determinant == 0)
                throw new Exception("Determinant je roven nule, tedy inverzní matice neexistuje.");

            for (int i = n - 1; i > 0; i--)//nulování horního trojúhelníku
            {
                for (int j = 0; j < i; j++)
                {
                    if (matA[i, i] != 0)
                    {
                        x = -(matA[j, i] / matA[i, i]);
                        OdectiRadekMatice(x, j, i, ref matA);
                        OdectiRadekMatice(x, j, i, ref invMat);
                    }
                }
            }

            for (int i = 0; i < n; i++)//vydělení hlavní diagonály
            {
                if (matA[i, i] != 0)
                {
                    //if (matA[i, i] > 0)
                    //    x = 1 / matA[i, i];
                    //else
                    //    x = -1 / matA[i, i];
                    x = 1 / matA[i, i];
                    VynasobRadekMatice(x, i, ref matA);
                    VynasobRadekMatice(x, i, ref invMat);
                }
            }
            return invMat;
        }
        private static void VynasobRadekMatice(float x, int radek, ref float[,] matA)
        {//poznámka: je zbytečné pro aktuální program dělit celý řádek, když mi jde o hlavní diagonálu, 
         //                                                 ale v budoucnu bude metoda obecně použitelná :)
            int n = matA.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                matA[radek, i] = matA[radek, i] * x;
            }
        }

        private static float[,] GenerujJednotkovouMatici(int n)
        {
            float[,] jednotkova = new float[n, n];
            for (int i = 0; i < n; i++)
            {
                jednotkova[i, i] = 1;
            }
            return jednotkova;
        }

        private static void OdectiRadekMatice(float x, int nulovanyRadek, int nasobenyRadek, ref float[,] matA)
        {
            int n = matA.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                float scitanec = matA[nasobenyRadek, i] * x;
                matA[nulovanyRadek, i] = scitanec + matA[nulovanyRadek, i];
            }
        }
    }
}
