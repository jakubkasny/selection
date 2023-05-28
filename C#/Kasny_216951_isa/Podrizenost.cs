using System.Collections.Generic;
using ObecneGrafy; 
using ObecneGrafy.Hrany;
using Kasny_216951_isa.Vrcholy; 
using Kasny_216951_isa.Strategie; 

namespace Kasny_216951_isa
{
    internal class Podrizenost : ObecnyGraf
    {
        internal Zamestnanec Kdo { get; private set; }
        internal Zamestnanec Koho { get; private set; }

        internal Podrizenost() : base(new PrikazovaLinka()) { } 

        internal void Podrizeny(Zamestnanec kdo, Zamestnanec koho) //metoda přidávajíci hranu (konstrukce grafu)
        {
            PridejHranu(kdo, koho, null);//volání do "ObecnyGraf"
        }

        internal List<List<Hrana>> JePodrizeny(Zamestnanec kdo, Zamestnanec koho) 
            //metoda zjišťující, jestli existuje cesta od "kdo" ke "koho"
        {
            
            Kdo = kdo;
            Koho = koho;

            // nikdo není podřízeným sebe sama
            if (Kdo.JeEkvivalentni(Koho))
            {
                Cesty.Clear();
                return Cesty;
            }

            // hledej cestu prostřednictvím zděděné metody pro hledání cest v ohodnoceném grafu se strategií PrikazovaLinka
            //                                                                                          (stejná strtegie jak DedickaLinie)
            return Hledej(Kdo, Koho);
        }

        public override string ToString()
            //metoda k vypsání výsledku hledání cesty
        {
            return Kdo +
                (Cesty.Count == 0 ? " nemůže" : " může") + " dostat pokyny od " +
                Koho;
        }
    }
}
