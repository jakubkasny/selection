namespace ObecneGrafy.Vrcholy
{    
    public interface IOhodV
    { // rozhraní pro implementaci ohodnocení vrcholu
      // vrcholy musí být dědici tohoto rozhraní a musí mít implementovánu následující metodu a vlastnost

        // je ohodnocení Vrcholu ekvivalentní ohodnocení ohodV?
        bool JeEkvivalentni(IOhodV ohodV);

        // false - k vložení do seznamu vrcholů
        // true - pouze k vyhledávání v seznamu
        bool PouzeProHledani { get; }
    }
}