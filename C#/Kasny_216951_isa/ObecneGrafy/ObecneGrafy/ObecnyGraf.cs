using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObecneGrafy.Vrcholy;
using ObecneGrafy.Hrany;
using ObecneGrafy.Strategie;



namespace ObecneGrafy
{
    public class ObecnyGraf
    { // vlastnosti jsou přístupné pouze pro čtení a jde je inicializovat jen v konstruktoru

        // vlastnost je přístupná pouze v dědicích ObecnyGraf (protected)
        // strategie ovlivňující průběh prohledávámí grafu;
        protected readonly IStrategie Strategie;

        //vlastnosti s přístupem "private" nevidí dědicové
        // seznam vrcholů grafu
        private readonly List<Vrchol> Vrcholy;
        // seznam hran grafu
        private readonly List<Hrana> Hrany;
        // aktuálně rozpracovaná cesta (seznam hran aktuální cesty)
        private readonly List<Hrana> Cesta;

        // vlastnost je přístupná pouze v dědicích ObecnyGraf
        // seznam všech nalezených cest (seznam seznamů hran)
        protected List<List<Hrana>> Cesty;
        
        public ObecnyGraf()
        {
            // dobrou praxí je incializovat v konstruktoru všechna odkazovaná pole (field) i vlastnosti
            // -proč: kvůli přehlednosti a pořádku... instance ObecnyGraf je vytvořena jednou, hned na začátku a tedy se list inicializuje 
            //                pouze v konstruktoru (kdybychom měli list např v metodě, mohl by se inicializovat vícekrát a tedy se přepisovat)
            // a je ještě lepší praxí nastavit vlastnosti tak, aby to jinak nešlo
            // defaultní stategií je nalezení všech cest, neuvažující ohodnocení
            Strategie = new VsechnyNeohod();
            Vrcholy = new List<Vrchol>();
            Hrany = new List<Hrana>();
            Cesta = new List<Hrana>();
            Cesty = new List<List<Hrana>>();
        }


        // změna strategie (z defaultní)
        public ObecnyGraf(IStrategie strategie) : this()
        { // je možná pouze v konstruktoru
          // 1) proveď konstruktor bez parametrů
          // 2) přiřaď novou strategii (do vlastnosti pouze pro čtení)
            Strategie = strategie;
        }


        private Vrchol V(IOhodV ohodV) 
            //v seznamu vrcholů hledá vrchol s ohodnocením předaným v prametru--> vrátí nalezený
            // Když nenajde vrchol, vytvoří nový s tímto ohodnocením a přidá jej do seznamu vrcholů
        { 
            
            foreach (Vrchol vrchol in Vrcholy)//hledá existující ohodnocení (vrchol)
                if (ohodV.JeEkvivalentni(vrchol.ohodV))
                    // vrchol byl nalezen
                    return vrchol;

            // vrchol nebyl nalezen v seznamu vrcholů
            if (!ohodV.PouzeProHledani)
            { // vrchol má být vložen do seznamu vrcholů
                // vytvoř nový vrchol
                Vrchol vrchol = new Vrchol(ohodV);
                // a přidej jej do seznamu vrcholů grafu
                Vrcholy.Add(vrchol);
                // vrať nový vrchol
                return vrchol;
            }
            else // vlastnost ".PouzeProHledani" je zděděná z rozhraní a je defaultně nastavena dědicem (Zamestnanec) v programu (readonly)
                throw new Exception("Neúplně definovaný vrchol (" + ohodV + ") nebyl nalezen.");
        }


        public Hrana PridejHranu(int v1, int v2)
        { // přidej neohodnocenou hranu

            // přidej hranu jako neohodnocenou
            return PridejHranu(v1, v2, null, null, null);
        }


        public Hrana PridejHranu(IOhodV ohodV1, IOhodV ohodV2, IOhodH ohodH) 
        { // přidej ohodnocenou hranu

            // získej "vrchol1" pro ohodnocení "ohodV1"            
            if (ohodV1 == null)// není-li ohodnocení k dispozici, není možno získat číslo vrcholu
                throw new Exception("Chybějící ohodnocení vrcholu v1.");
            Vrchol vrchol1 = V(ohodV1); 

            // získej vrchol2 pro ohodnocení ohodV2            
            if (ohodV2 == null)// není-li ohodnocení k dispozici, není možno získat číslo vrcholu
                throw new Exception("Chybějící ohodnocení vrcholu v2.");
            Vrchol vrchol2 = V(ohodV2);

            // přidej hranu jako ohodnocenou
            // ohodnocení hrany nemusí být k dispozici (ohodH==null)
            return PridejHranu(vrchol1.v, vrchol2.v,
                vrchol1.ohodV, vrchol2.ohodV, ohodH);
        }


        private Hrana PridejHranu(int v1, int v2,
            IOhodV ohodV1, IOhodV ohodV2, IOhodH ohodH)
        { // přidej hranu (vč. ohodnocení, pokud ohodnocení vrcholů existuje)

            // kontrola, že vrcholy v1, v2 jsou reprezentovány nezápornými (celými) čísly
            if (v1 < 0)
                throw new Exception("Nepřípustná hodnota čísla vrcholu (" + v1 + ").");
            if (v2 < 0)
                throw new Exception("Nepřípustná hodnota čísla vrcholu (" + v2 + ").");


            if (!Strategie.PovoleneNasobneHrany)
                // nesmí obsahovat násobné hrany
                foreach (Hrana hrana in Hrany)
                    if ((hrana.v1 == v1) && (hrana.v2 == v2))
                        // přidání hrany (v1, v2) v situaci, kdy už v grafu je -
                        // násobná hrana způsobí výjimku
                        throw new Exception("Nepřípustná násobná hrana " + hrana + ".");

            // vytvoření nové hrany
            Hrana h = new Hrana(v1, v2, ohodV1, ohodV2, ohodH);
            // a její přidání do seznamu hran grafu
            Hrany.Add(h);
            // vrať novou hranu (pro všechny případy)
            return h;
        }


        public List<List<Hrana>> Hledej(IOhodV ohodV1, IOhodV ohodV2)
        { // hledej cesty/cestu z vrcholu s ohodnocením ohodV1 do vrcholu s ohodnodením OhodV2

            // hledej cesty/cestu z vrcholu číslo v1 do vrcholu číslo v2
            return Hledej(V(ohodV1).v, V(ohodV2).v);
        }


        public List<List<Hrana>> Hledej(int v1, int v2)
        {
            // cesta z uzlu v1 do v1 je triviální
            if (v1 == v2)
                throw new Exception("Nalezení cesty je triviální.");

            // vyprázdni aktuálně rozpracovanou cestu
            Cesta.Clear();
            // vyprázdni seznam nalezených cest
            Cesty.Clear();
            // (re)inicializuj strategii
            Strategie.Nastav();

            // hledej cestu z v1 do v2
            ExCesta(v1, v2);

            // ze seznamu nalezených cest sestav seznam (požadovaných) cest
            return Strategie.Vyber(Cesty);
        }


        private void ExCesta(int v1, int v2)
        {
            if (v1 == v2)
            { // nalezena cesta (prochází cílovým vrcholem s číslem v2)
                // přidej nalezenou cestu do seznamu nalezených cest
                Cesty.Add(new List<Hrana>(Cesta));
                // pro tuto cestu proveď akci strategie
                Strategie.Eviduj(Cesta);
                return;
            }

            
            foreach (Hrana hrana in Hrany)
            { // zkus (další) hranu
                if ((hrana.v1 != v1) || (!Strategie.JePerspektivni(Cesta, hrana)))
                    continue;
                // exHrana(v1, x), perspektivní, x je hrana.v2
                int x = hrana.v2;
                // dá se hledanou cestou dostat do vrcholu s číslem x?
                if (Contains(x))
                    // cesta by tvořila násobnou hranu, zkus další hranu
                    continue;
                // hrana zatím vyhovuje, přidej ji na konec cesty
                Strategie.Pridej(Cesta, hrana);

                // opakuj rekurzívně proceduru pro exCesta(x,v2),
                // tj. exHrana(x, y) & exCesta(y,v2)
                ExCesta(x, v2);
                if (Strategie.Stop)
                    // ukonči prohledávání
                    return;

                // hrana už není potřebná, odstraň ji z konce cesty
                Strategie.Odeber(Cesta, hrana);
            }
            // všichni přímí následníci v1 byli vyčerpáni, přes v1 cesta neprochází
            return;
        }

        private bool Contains(int v)
        { //  dá se dosaženou cestou (cesta) dostat do vrcholu v?

            foreach (Hrana hrana in Cesta)
                if (hrana.v2 == v)
                    if (Strategie.PovoleneCykly)
                        return true;
                    else
                    {
                        foreach (Vrchol vrchol in Vrcholy)
                            if (vrchol.v == v)
                                throw new Exception("Nepřípustný cyklus procházející vrcholem " + vrchol + ".");
                    }
            return false; //neobsahuje vrchol x a program může pokračovat
        }
    }
}
