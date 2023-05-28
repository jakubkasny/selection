using ObecneGrafy.Vrcholy;
using System.Collections.Generic;
using System;
using System.IO;

namespace Kasny_216951_isa.Vrcholy
{    
    //třída odvozená od interface "IOhodV" pro vrcholy
    internal class Zamestnanec : IOhodV
    {
        private readonly string jmeno;
        private readonly string prijmeni;
        private readonly int plat;

        public Zamestnanec(string jmeno, string prijmeni, int plat)
        {
            this.jmeno = jmeno;
            this.prijmeni = prijmeni;
            this.plat = plat;
        }

        //povinná implementace z rozhraní
        public bool JeEkvivalentni(IOhodV ohodV)
        {
            return ((jmeno == (ohodV as Zamestnanec).jmeno) &&
                    (prijmeni == (ohodV as Zamestnanec).prijmeni));
        }
       
        //povinná implementace z rozhraní
        public bool PouzeProHledani
        {
            get { return false; }
        }

        //přepisuje metodu "ToString()"
        public override string ToString()
        {
            return jmeno + " " + prijmeni+ " ("+plat+",-)";
        }
    }
}
