using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObecneGrafy.Vrcholy
{
    public class Vrchol
    { // implementace vrcholu (neohodnoceného i ohodnoceného)

        // počitadlo počtu vytvořených instancí (jako pole(field) s hodnotou společnou pro všechny instance)
        private static int pocitadlo;

        // číslo vrcholu
        internal readonly int v;

        // ohodnocení vrcholu; třída skutečného ohodnocení musí být dědicem rozhraní IOhodV
        internal readonly IOhodV ohodV;

        public Vrchol(IOhodV ohodV)
        {
            // při každém vyvoření instance zvyš počitadlo o 1
            v = pocitadlo++;
            // zapamatuj ohodnonocení
            this.ohodV = ohodV;
        }

        public override string ToString()
        { // ohodnocení vrcholu: když není vloženo Ohodnocení, pak vrať číslo
            return ohodV == null ? v.ToString() : ohodV.ToString();
        }

    }
}
