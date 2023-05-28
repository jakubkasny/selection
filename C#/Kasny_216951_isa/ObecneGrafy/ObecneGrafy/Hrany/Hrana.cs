using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObecneGrafy.Vrcholy;

namespace ObecneGrafy.Hrany
{
    public class Hrana
    { // implementace hrany (neohodnocené i ohodnocené)

        // číslo vrcholu "odkud", 
        internal readonly int v1;
        // číslo vrcholu "kam", 
        internal readonly int v2;

        // ohodnocení vrcholu "odkud"; null - neohodnocený vrchol
        // třída skutečného ohodnocení musí být dědicem rozhraní IOhodV
        public readonly IOhodV ohodV1;

        // ohodnocení vrcholu "kam"; null - neohodnocený vrchol
        // třída skutečného ohodnocení musí být dědicem rozhraní IOhodV
        public readonly IOhodV ohodV2;

        // ohodnocení hrany (v1, v2)
        // třída skutečného ohodnocení musí být dědicem rozhraní IOhodH
        public readonly IOhodH ohodH; // null - neohodnocená


        public Hrana(int v1, int v2,
            IOhodV ohodV1, IOhodV ohodV2, IOhodH ohodH)
        { // implementace v konstruktoru: v1, v2, ohodV1, ohodV2, ohodH            
            this.v1 = v1;
            this.v2 = v2;
            this.ohodV1 = ohodV1;
            this.ohodV2 = ohodV2;
            this.ohodH = ohodH;
        }


        public override string ToString()
        { // kompletní ohodnocení hrany (v1, v2) jako string

            //Když je ohodnocení == null vloží vygenerované číslo (generováno při vytváření vrcholu)
            // získej ohodnocení vrcholu s číslem v1 jako string
            string sv1 = ohodV1 == null ? v1.ToString() : ohodV1.ToString();
            // získej ohodnocení vrcholu s číslem v2 jako string
            string sv2 = ohodV2 == null ? v2.ToString() : ohodV2.ToString();

            //Když je ohodnocení == null vloží prázdný text: ""
            // získej ohodnocení hrany jako string
            string sh = ohodH == null ? "" : ohodH.ToString();

            return sh + "(" + sv1 + ", " + sv2 + ")";
        }
    }
}
