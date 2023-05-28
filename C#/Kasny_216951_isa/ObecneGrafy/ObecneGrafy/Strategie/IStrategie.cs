using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObecneGrafy.Hrany;

namespace ObecneGrafy.Strategie
{
    // rozhraní: definuje náležitosti, které musí dědicové mít (všichni dědicové mohou být typu rozhraní "IStrategie")
    public interface IStrategie
    { // rozhraní

        // (re)inicializuj strategii
        void Nastav();

        // Graf může obsahovat cykly
        bool PovoleneCykly { get; }

        // Graf nesmí obsahovat násobné hrany
        bool PovoleneNasobneHrany { get; }

        // je perspektivní přidat hranu do cesty sestavované během prohledávání?
        bool JePerspektivni(List<Hrana> cesta, Hrana hrana);

        // přidej k cestě sestavované během prohledávání hranu jako poslední
        void Pridej(List<Hrana> cesta, Hrana hrana);

        // odeber poslední hranu z cesty sestavované během prohledávání
        void Odeber(List<Hrana> cesta, Hrana hrana);

        // vlastnost řídící předčasné ukončení prohledávání
        bool Stop { get; }

        // akce bezprostředně po nalezení cesty: přidá cestu do sezanmu možných cest
        void Eviduj(List<Hrana> cesta);

        // ze seznamu nalezených cest sestav seznam (požadovaných) cest (pouze cesty splňující podmínky definované strategie)
        List<List<Hrana>> Vyber(List<List<Hrana>> cesty);
    }
}
