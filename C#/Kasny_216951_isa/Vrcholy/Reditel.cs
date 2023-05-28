using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasny_216951_isa.Vrcholy
{
    internal class Reditel : Zamestnanec
    {
        //třída odvozená od třídy "Zamestnanec"
        public Reditel(string jmeno, string prijmeni, int plat) :
            base(jmeno, prijmeni, plat)
        { }

        // používá zděděný ToString()
        public override string ToString()
        {
            return "ředitel, " + base.ToString();
        }
    }
}