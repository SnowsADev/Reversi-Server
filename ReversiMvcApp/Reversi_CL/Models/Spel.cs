using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reversi_CL.Models
{

    public class Spel : ISpel
    {
        [Key]
        public int ID { get; set; }
        public Kleur AandeBeurt { get; set; }
        public string Omschrijving { get; set; }
        public string Token { get; set; }
        public ICollection<Speler> Spelers { get; set; }
        //public Kleur[] multiArray;
        [Column(TypeName = "nvarchar(255)")]
        public Kleur[,] Bord { get; set; }


        public Spel()
        {
            //this.multiArray = new Kleur[8];
            
            this.Bord = new Kleur[8, 8];
            this.Bord[3, 3] = Kleur.Wit;
            this.Bord[3, 4] = Kleur.Zwart;
            this.Bord[4, 3] = Kleur.Zwart;
            this.Bord[4, 4] = Kleur.Wit;
            this.AandeBeurt = Kleur.Zwart;
        }



        private Kleur NietAandeBeurt()
        {
            //Set nietAandebeurt
            if (AandeBeurt == Kleur.Wit)
                return Kleur.Zwart;
            else
                return Kleur.Wit;
        }

        public bool Afgelopen()
        {
            int aantalZonderKleur = 0;
            for (int rij = 0; rij <= Bord.GetLength(0) - 1; rij++)
            {
                for (int kolom = 0; kolom < Bord.GetLength(1) - 1; kolom++)
                {
                    if (Bord[rij, kolom] == Kleur.Geen)
                    {
                        if (ZetMogelijk(rij, kolom))
                        {
                            aantalZonderKleur++;
                        }
                    }
                }
            }

            if (aantalZonderKleur > 0)
                return false;

            return true;
        }

        public bool DoeZet(int rijZet, int kolomZet)
        {
            bool zetGedaan = false;

            // g g g g
            // g x g g 2,3
            // g w z g rij 3, kolom 4
            // g z w g rv = 1 kv = 1

            if (ZetMogelijk(rijZet, kolomZet))
            {
                Bord[rijZet, kolomZet] = AandeBeurt;

                //checks all positions around chosen position
                int startCheckRijPositie = rijZet - 1;
                int startCheckKolomPositie = kolomZet - 1;

                int eindCheckrijPositie = rijZet + 1;
                int eindCheckKolomPositie = kolomZet + 1;

                //check if starting Iterations And end Iterations arent < 0 OR bigger than board size
                if (startCheckKolomPositie < 0)
                    startCheckKolomPositie = 0;
                if (startCheckRijPositie < 0)
                    startCheckRijPositie = 0;
                if (eindCheckrijPositie >= Bord.GetLength(0))
                    eindCheckrijPositie = rijZet;
                if (eindCheckKolomPositie >= Bord.GetLength(1))
                    eindCheckKolomPositie = kolomZet;

                for (int rij = startCheckRijPositie; rij <= eindCheckrijPositie; rij++)
                {
                    for (int kolom = startCheckKolomPositie; kolom <= eindCheckKolomPositie; kolom++)
                    {
                        Kleur nietAandeBeurt = NietAandeBeurt();
                        //If position has a different colour around it
                        if (Bord[rij, kolom] == NietAandeBeurt())
                        {
                            //Rij + Column the opposite colour that is found above
                            int rijVerschil = (rij - rijZet); //1
                            int kolomVerschil = (kolom - kolomZet); //0
                            int rPos = rij;
                            int kPos = kolom;

                            //kPos & rPos is the starting position
                            while (rPos >= 0 && rPos < Bord.GetLength(0) && kPos >= 0 && kPos < Bord.GetLength(1))
                            {

                                if (Bord[rPos, kPos] == Kleur.Geen)
                                    break;

                                //if discs are surrounded by this move
                                if (Bord[rPos, kPos] == AandeBeurt)
                                {
                                    zetGedaan = true;
                                    break;
                                }
                                Bord[rPos, kPos] = AandeBeurt;
                                rPos += rijVerschil;
                                kPos += kolomVerschil;
                            }
                        }
                    }
                }
                this.AandeBeurt = NietAandeBeurt();
            }
            return zetGedaan;
        }

        public Kleur OverwegendeKleur()
        {
            int aantalWit = 0;
            int aantalZwart = 0;
            for (int rij = 0; rij <= Bord.GetLength(0) - 1; rij++)
            {
                for (int kolom = 0; kolom < Bord.GetLength(1) - 1; kolom++)
                {
                    if (Bord[rij, kolom] == Kleur.Wit)
                        aantalWit++;
                    if (Bord[rij, kolom] == Kleur.Zwart)
                        aantalZwart++;
                }
            }

            if (aantalWit > aantalZwart)
            {
                return Kleur.Wit;
            }
            else if (aantalZwart > aantalWit)
            {
                return Kleur.Zwart;
            }
            else
            {
                return Kleur.Geen;
            }
        }

        public bool Pas()
        {
            //check if any move is possible
            bool zetMogelijk = false;
            for (int rij = 0; rij <= Bord.GetLength(0) - 1; rij++)
            {
                for (int kolom = 0; kolom < Bord.GetLength(1) - 1; kolom++)
                {
                    if (Bord[rij, kolom] == Kleur.Geen)
                    {
                        if (ZetMogelijk(rij, kolom))
                        {
                            zetMogelijk = true;
                        }
                    }
                }
            }

            if (!zetMogelijk)
            {
                //switch sides & pass
                this.AandeBeurt = NietAandeBeurt();
                return true;
            }

            //move is possible
            return false;
        }

        public bool ZetMogelijk(int rijZet, int kolomZet)
        {
            bool zetMogelijk = false;

            //checks if position is on the board
            if (Bord.GetLength(0) <= rijZet || Bord.GetLength(1) <= kolomZet)
                return false;

            //checks if position is not already taken
            if (Bord[rijZet, kolomZet] != Kleur.Geen)
                return false;


            //checks all positions around chosen position
            int startCheckRijPositie = rijZet - 1;
            int startCheckKolomPositie = kolomZet - 1;

            int eindCheckrijPositie = rijZet + 1;
            int eindCheckKolomPositie = kolomZet + 1;

            //check if starting Iterations And end Iterations arent < 0 OR bigger than board size
            if (startCheckKolomPositie < 0)
                startCheckKolomPositie = 0;
            if (startCheckRijPositie < 0)
                startCheckRijPositie = 0;
            if (eindCheckrijPositie >= Bord.GetLength(0))
                eindCheckrijPositie = rijZet;
            if (eindCheckKolomPositie >= Bord.GetLength(1))
                eindCheckKolomPositie = kolomZet;

            for (int rij = startCheckRijPositie; rij <= eindCheckrijPositie; rij++)
            {
                for (int kolom = startCheckKolomPositie; kolom <= eindCheckKolomPositie; kolom++)
                {
                    //If position has a different colour around it
                    if (Bord[rij, kolom] == NietAandeBeurt())
                    {
                        //Rij + Column the opposite colour that is found above
                        int rijVerschil = (rij - rijZet);
                        int kolomVerschil = (kolom - kolomZet);
                        int kPos = kolom + kolomVerschil;
                        int rPos = rij + rijVerschil;

                        //kPos & rPos is the starting position
                        while (rPos >= 0 && rPos < Bord.GetLength(0) && kPos >= 0 && kPos < Bord.GetLength(1))
                        {
                            //stop when empty space
                            if (Bord[rPos, kPos] == Kleur.Geen)
                                break;

                            //if discs are surrounded by this move
                            if (Bord[rPos, kPos] == AandeBeurt)
                            {
                                zetMogelijk = true;
                                break;
                            }

                            rPos += rijVerschil;
                            kPos += kolomVerschil;
                        }
                    }
                }
            }

            return zetMogelijk;
        }
    }
}
