using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kasny_216951_isa.Vrcholy
{
    //třída odvozená od třídy "Zamestnanec"
    internal class Delnik : Zamestnanec
    {
        public Delnik(string jmeno, string prijmeni, int plat) :
            base(jmeno, prijmeni, plat)
        { }

        // používá zděděný ToString()
        public override string ToString()
        {
            return "dělník, " + base.ToString();
        }
    }
}