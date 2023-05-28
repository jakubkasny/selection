using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasny_216951_isa.Vrcholy
{
    internal class Manazer : Zamestnanec
    {
        //třída odvozená od třídy "Zamestnanec"
        public Manazer(string jmeno, string prijmeni, int plat) :
            base(jmeno, prijmeni, plat)
        { }

        // používá zděděný ToString()
        public override string ToString()
        {
            return "manažer, " + base.ToString();
        }
    }
}
