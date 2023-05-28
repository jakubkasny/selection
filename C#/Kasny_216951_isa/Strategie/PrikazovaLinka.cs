using ObecneGrafy.Strategie;

namespace Kasny_216951_isa.Strategie 
{
    class PrikazovaLinka : PrvniNeohod
    { 
        //upřesnění strategie (v našem grafu teoreticky nejsou cykly ani možné, alespoň jsem se ještě nesetkal s takovou firemní hierarchií)
        public override bool PovoleneCykly { get { return false; } }
    }
}
