using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObecneGrafy.Hrany;

namespace ObecneGrafy.Strategie
{
    public class NejkratsiNeohod : IStrategie
    { // Implementace strategie nalezení cesty nejkratší cesty v neohodnoceném grafu, uvažující délku cesty jako počet hran
      // (délku hrany = 1). Její metody modifikují chování metod ObecnyGraf.Hledej a ObecnyGraf.ExCesta .

      // Všechny strategie musí být dědici rozhraní IStrategie, což znamená, že v nich musí být s viditelností public implementovány 
      // všechny metody a automatické vlastnosti uvedené v IStrategie.

      // Metody jsou implementovány jako virtuální, aby bylo možno vytvářet dědice strategie PrvniNeohod 
      //a v nich některé dále "jemně" přizpůsobovat.

        
        private int Delka { get; set; } // (aktuální) délka vytvářené cesty
        
        private int MinDelka { get; set; } // (zatím) dosažená délka nekratší cesty


        // (re)inicializuj strategii na defaultní
        public virtual void Nastav()
        {
            // nastav dosaženou délku aktuálně hledané cesty (na začátku je délka 0)
            Delka = 0;
            // nastav dosaženou délku nejkratší nalezené cesty (defaultně int.MaxValue)
            MinDelka = int.MaxValue;
        }


        // Graf může obsahovat cykly
        public virtual bool PovoleneCykly { get { return true; } }

        
        // Graf nesmí obsahovat násobné hrany (nemají smysl, neuvažuje se ohodnocení)
        public virtual bool PovoleneNasobneHrany { get { return false; } }


        // je perspektivní přidat hranu do cesty sestavované během prohledávání?
        public virtual bool JePerspektivni(List<Hrana> cesta, Hrana hrana)
        {
            // přidání první hrany je perspektivní 
            if (Delka == 0)
                return true;

            // perspektivní je i přidání hrany, pokud délka rozpracované cesty
            // s touto hranou není větší než délka zatím dosažené nekratší cesty 
            return Delka + 1 <= MinDelka;
        }


        // přidej k cestě sestavované během prohledávání hranu jako poslední
        public virtual void Pridej(List<Hrana> cesta, Hrana hrana)
        {
            // zvyš délku cesty
            Delka++;
            // přidej hranu jako poslední
            cesta.Add(hrana);
        }


        // odeber poslední hranu z cesty sestavované během prohledávání
        public virtual void Odeber(List<Hrana> cesta, Hrana hrana)
        {
            // odeber poslední hranu
            cesta.RemoveAt(cesta.Count - 1);
            // sniž délku cesty
            Delka--;
        }


        // vlastnost řídící předčasné ukončení prohledávání
        public virtual bool Stop { get { return false; } }


        // akce bezprostředně po nalezení cesty
        public virtual void Eviduj(List<Hrana> cesta)
        { // díky testování perspektivnosti přidání hrany nemůže být "Delka" vyšší než "MinDelka" 

            // eviduj dosaženou délku jako (zatím) minimální pro závěrečný výběr nejkratší cesty
            MinDelka = Delka;
        }


        // ze seznamu nalezených cest sestav seznam (požadovaných) cest
        public virtual List<List<Hrana>> Vyber(List<List<Hrana>> cesty)
        {
            // do seznamu (požadovaných) cest zařaď pouze cesty s minimální délkou
            List<List<Hrana>> minCesty = new List<List<Hrana>>();
            foreach (List<Hrana> cesta in cesty)
                if (cesta.Count == MinDelka)
                    minCesty.Add(cesta);
            return minCesty;
        }

    }
}
//poznámka... myslím, že by šlo vlastnost "Delka" nahradit vlastností "cesta.Count",
//                  ale nechtěl jsem nijak zasahovat do programu určeného k využití :)