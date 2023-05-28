using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasny_inverzni_matice
{
    partial class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("VÝPOČET INVERZNÍ MATICE"+Environment.NewLine);

                //získání dat (čtení n- řád čtvercové matice)
                float[,] matA = CtiA();
                Console.WriteLine(Environment.NewLine + "Zadaná matice:");
                ZobrazMatici(matA);

                //zpracování dat (v metodě: vytvořím si vlastní jednotkovou matici, na kterou budu aplikovat stejné změny,
                //                                          jako na původní matici, abych z původní získal jednotkovou)
                float[,] invA = InverzeMatice(ref matA);

                //vypsání zpracovaných dat (inverzní matice)
                Console.WriteLine(Environment.NewLine+"Původní matice upravená:");
                ZobrazMatici(matA);
                Console.WriteLine(Environment.NewLine+"Inverzní matice:");
                ZobrazMatici(invA);
            }
            catch (Exception e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Console.WriteLine("Dojde k ukončení programu.");
                Console.ReadKey();
            }            
        }
    }
}
