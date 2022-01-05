using System.Collections.Generic;

namespace Reversi_CL.Models
{
    public interface ISpel
    {
        int ID { get; set; }
        string Omschrijving { get; set; }
        string Token { get; set; }
        ICollection<Speler> Spelers { get; set; }
        Kleur[,] Bord { get; set; }
        Kleur AandeBeurt { get; set; }
        bool Pas();
        bool Afgelopen();
        Kleur OverwegendeKleur();
        bool ZetMogelijk(int rijZet, int kolomZet);
        bool DoeZet(int rijZet, int kolomZet);
    }
}
