using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObecneGrafy.Hrany;

namespace ObecneGrafy.Strategie
{
    public class PrvniNeohod : IStrategie
    { // Implementace strategie nalezení (jedné) cesty, neuvažující ohodnocení. 
      // Její metody modifikují chování metod ObecnyGraf.Hledej a ObecnyGraf.ExCesta .
      // Všechny strategie musí být dědici rozhraní IStrategie, což znamená, že v nich musí být s viditelností public implementovány 
      // všechny metody a automatické vlastnosti uvedené v IStrategie.
      // Metody jsou implementovány jako virtuální, aby bylo možno vytvářet dědice strategie PrvniNeohod
      // a v nich některé dále "jemně" přizpůsobovat.


        // (re)inicializuj strategii
        public virtual void Nastav()
        {
            // Umožni prohledávání grafu
            Stop = false;
        }


        // graf může obsahovat cykly
        public virtual bool PovoleneCykly { get { return true; } }


        // graf nesmí obsahovat násobné hrany (nemají smysl, neuvažuje se ohodnocení)
        public virtual bool PovoleneNasobneHrany { get { return false; } }


        // je perspektivní přidat hranu do cesty sestavované během prohledávání?
        public virtual bool JePerspektivni(List<Hrana> cesta, Hrana hrana)
        { // Je-li nastaveno zastavení prohledávání, není perspektivní přidávat hranu, jinak je.
            return !Stop;
        }


        // přidej k cestě sestavované během prohledávání hranu jako poslední
        public virtual void Pridej(List<Hrana> cesta, Hrana hrana)
        {
            cesta.Add(hrana);
        }


        // odeber poslední hranu z cesty sestavované během prohledávání
        public virtual void Odeber(List<Hrana> cesta, Hrana hrana)
        {
            cesta.RemoveAt(cesta.Count - 1);
        }


        // vlastnost řídící předčasné ukončení prohledávání
        public virtual bool Stop { get; private set; }


        // akce bezprostředně po nalezení cesty
        public virtual void Eviduj(List<Hrana> cesta)
        { // ukonči prohledávání
            Stop = true;
        }


        // ze seznamu nalezených cest sestav seznam (požadovaných) cest (v této strategii je to první nalezená cesta)
        public virtual List<List<Hrana>> Vyber(List<List<Hrana>> cesty)
        {
            // vyber všechny nalezené cesty
            return new List<List<Hrana>>(cesty);
        }

    }
}
