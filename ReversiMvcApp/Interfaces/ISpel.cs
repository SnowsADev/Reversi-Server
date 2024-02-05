using System.Collections.Generic;

namespace ReversiMvcApp.Models
{
    public interface ISpel
    {
        string ID { get; set; }
        string Omschrijving { get; set; }
        ICollection<Speler> Spelers { get; set; }
        List<List<int>> MogelijkeZetten { get; }
        Kleur[,] Bord { get; set; }
        Kleur AandeBeurt { get; set; }
        bool Pas();
        bool Afgelopen();
        Kleur OverwegendeKleur();
        bool ZetMogelijk(int rijZet, int kolomZet);
        bool DoeZet(int rijZet, int kolomZet);
    }
}
