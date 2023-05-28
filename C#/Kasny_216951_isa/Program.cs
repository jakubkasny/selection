using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kasny_216951_isa.Hrany;
using Kasny_216951_isa.Vrcholy;
using Kasny_216951_isa.Strategie;

// relaci "isa", neboli "je" lze použít mezi třídami (neboli (odvozená třída)-isa-(bázová třída); např.: ovoce-isa-jídlo, nebo 
//                                                                       Manažer/Dělník/Reditel/Mistr/Uklízeč-isa("je")-zaměstnanec
// Dále je relaci isa možné použít v rámci metody v grafu. Např.: (člověkA)-isa(je otcem?)-(člověkaB) 
//        jiný příklad: (člověkA(uklízeč))-isa("je podřízeným")-(člověkaB(manažer)) 
//                      ....tedy, jestli když manažer(člověkA) vydá nějaký pokyn, tak jestli se to dostane k dělníkovi(člověkB)
// Modifikací poskytnutých programů jsem vytvořil metodu, která zjišťuje, zda-li existuje cesta, kterou se může pokyn dostat
namespace Kasny_216951_isa
{
    class Program
    {
        static void Main(string[] args)
        {
            //inicializace proměnné "podrizenost"; ovládá celý program
            //-přes volání konstruktoru se dostanu až na inicializaci v třídě "ObecnyGraf", která je bázová pro třídu "Podriznost"
            //v konstruktoru třídy "ObecnyGraf" se inicalizují listy, kde jsou uchovány seznamy vrcholů (zaměstnanců), hran (být podřízený),
            //cesty(aktuální seznam hran, po které hledám cestu) a cest(seznam všech možných cest jak se dostat k cíli)
            // + inicializace strategie (stejná strategie jak v PotomciDemo - PrikazovaLinka- PrvniNeohod (tedy strategie obecného grafu))
            Podrizenost podrizenost = new Podrizenost();
            
            // prametry metody: instance odvozené třídy (uklizec/delnik/...) -> odvozená od Zamestnanec-> odvozená od interface IOhoV
            // a volání metody ".Podrizeny(kdo, koho)"--> "PridejHranu" v třídě "ObecnyGraf"
            podrizenost.Podrizeny(new Uklizec("Jan", "Buchal", 15000), new Mistr("Jakub", " Krpec", 25000));
            podrizenost.Podrizeny(new Uklizec("Josef", "Lečo", 15000), new Mistr("Jakub", " Krpec", 25000));

            podrizenost.Podrizeny(new Delnik("Petr", "Sluch", 25000), new Mistr("Alena", " Všetičková", 33000));
            podrizenost.Podrizeny(new Delnik("Josef", "Novák", 26000), new Mistr("Alena", " Všetičková", 33000));
            podrizenost.Podrizeny(new Delnik("František", "Ježek", 24000), new Mistr("Alena", " Všetičková", 33000));
            podrizenost.Podrizeny(new Delnik("Petr", "Sokol", 23000), new Mistr("Alena", " Všetičková", 33000));
            podrizenost.Podrizeny(new Delnik("Pavla", "Šípková", 25000), new Mistr("Alena", " Všetičková", 33000));
            
            podrizenost.Podrizeny(new Delnik("Petra", "Láníčková", 26000), new Mistr("Tomáš", "Navrátil", 35000));
            podrizenost.Podrizeny(new Delnik("Radmila", "Nejezchlebová", 24000), new Mistr("Tomáš", "Navrátil", 35000));
            podrizenost.Podrizeny(new Delnik("Anna", "Veselá", 25000), new Mistr("Tomáš", "Navrátil", 35000));
            podrizenost.Podrizeny(new Delnik("Kateřina", "Svobodová", 23000), new Mistr("Tomáš", "Navrátil", 35000));

            podrizenost.Podrizeny(new Mistr("Alena", " Všetičková", 33000), new Manazer("Tomáš", "Svobodný", 55000));
            podrizenost.Podrizeny(new Mistr("Alena", " Všetičková", 33000), new Manazer("Leoš", "Juřík", 60000));
            
            podrizenost.Podrizeny(new Mistr("Tomáš", "Navrátil", 35000), new Manazer("Leoš", "Juřík", 60000));
            podrizenost.Podrizeny(new Mistr("Tomáš", "Navrátil", 35000), new Manazer("Ondřej", "Klidný", 55000));

            podrizenost.Podrizeny(new Manazer("Tomáš", "Svobodný", 55000), new Reditel("Jakub", "Kamzík", 150000));
            podrizenost.Podrizeny(new Manazer("Leoš", "Juřík", 60000), new Reditel("Jakub", "Kamzík", 150000));
            podrizenost.Podrizeny(new Manazer("Ondřej", "Klidný", 55000), new Reditel("Jakub", "Kamzík", 150000));
            podrizenost.Podrizeny(new Mistr("Jakub", " Krpec", 25000), new Reditel("Jakub", "Kamzík", 150000));


            //metoda: JePodrizeny - tedy, jestli existuje cesta, kterou může zaměstnanecB poslat pokyny zaměstnanci A (něco jako JePotomek)
            podrizenost.JePodrizeny(new Uklizec("Jan", "Buchal", 15000), new Manazer("Ondřej", "Klidný", 55000));
            Console.WriteLine(podrizenost + Environment.NewLine);

            podrizenost.JePodrizeny(new Uklizec("Jan", "Buchal", 15000), new Reditel("Jakub", "Kamzík", 150000));
            Console.WriteLine(podrizenost + Environment.NewLine);

            podrizenost.JePodrizeny(new Delnik("Kateřina", "Svobodová", 23000), new Manazer("Ondřej", "Klidný", 55000));
            Console.WriteLine(podrizenost + Environment.NewLine);

            podrizenost.JePodrizeny(new Delnik("Pavla", "Šípková", 25000), new Manazer("Ondřej", "Klidný", 55000));
            Console.WriteLine(podrizenost + Environment.NewLine);

            Console.ReadKey();
        }
    }
}
