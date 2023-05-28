using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasny_216951_isa.Vrcholy
{
    internal class Uklizec : Zamestnanec
    {
        //třída odvozená od třídy "Zamestnanec"
        public Uklizec(string jmeno, string prijmeni, int plat) :
            base(jmeno, prijmeni, plat)
        { }

        // používá zděděný ToString()
        public override string ToString()
        {
            return "uklízeč, " + base.ToString();
        }
    }
}
