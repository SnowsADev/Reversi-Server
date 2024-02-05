using ReversiMvcApp.Extensions;
using ReversiMvcApp.Models.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ReversiMvcApp.Models
{

    public class Spel : Auditable, ISpel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string ID { get; set; }
        public Kleur AandeBeurt { get; set; }
        public string Omschrijving { get; set; }

        [Column("Afgelopen")]
        public bool SpelIsAfgelopen { get; set; } = false;

        [Column(TypeName = "nvarchar(255)")]
        public Kleur[,] Bord { get; set; }

        [MaxLength(2)]
        public ICollection<Speler> Spelers { get; set; }

        [NotMapped]
        public List<List<int>> MogelijkeZetten
        {
            get { return GetMogelijkeZetten(); }
        }

        public Spel()
        {
            this.Bord = new Kleur[8, 8];
            this.Bord[3, 3] = Kleur.Wit;
            this.Bord[3, 4] = Kleur.Zwart;
            this.Bord[4, 3] = Kleur.Zwart;
            this.Bord[4, 4] = Kleur.Wit;
            this.AandeBeurt = Kleur.Zwart;
        }

        private Kleur NietAandeBeurt()
        {
            return AandeBeurt == Kleur.Wit ? Kleur.Zwart : Kleur.Wit;
        }

        public bool Afgelopen()
        {
            if (this.MogelijkeZetten.Count() > 0)
            {
                return false;
            }

            //this.Spelers.Clear();
            SpelIsAfgelopen = true;
            UpdateResultaat();
            return true;
        }

        private void UpdateResultaat()
        {
            //Zet winnaar spel
            var groupedValues = Bord.Cast<Kleur>()
                .Where(color => color != Kleur.Geen)
                .GroupBy(steen => steen)
                .OrderByDescending(group => group.Count())
                .ThenBy(group => group.Key);

            var maxCount = groupedValues.First().Count();

            var mostCommonValues = groupedValues
            .TakeWhile(group => group.Count() == maxCount)
            .Select(group => group.Key);

            //draw
            if (mostCommonValues.Count() > 1)
            {
                Spelers.ToList().ForEach(s => s.AantalGelijk += 1);
                return;
            }

            //winner
            switch (mostCommonValues.First())
            {
                case Kleur.Wit:
                    Spelers.Where(s => s.Kleur == Kleur.Wit).FirstOrDefault().AantalGewonnen += 1;
                    Spelers.Where(s => s.Kleur == Kleur.Zwart).FirstOrDefault().AantalVerloren += 1;
                    break;
                case Kleur.Zwart:
                    Spelers.Where(s => s.Kleur == Kleur.Zwart).FirstOrDefault().AantalGewonnen += 1;
                    Spelers.Where(s => s.Kleur == Kleur.Wit).FirstOrDefault().AantalVerloren += 1;
                    break;

            }
        }

        public List<List<int>> GetMogelijkeZetten()
        {
            List<List<int>> mogelijkeZetten = new List<List<int>>();

            for (int rij = 0; rij < Bord.GetLength(0); rij++)
            {
                for (int kolom = 0; kolom < Bord.GetLength(1); kolom++)
                {
                    if (Bord[rij, kolom] == Kleur.Geen)
                    {
                        if (ZetMogelijk(rij, kolom))
                        {
                            mogelijkeZetten.Add(new List<int>() { rij, kolom });
                        }
                    }
                }
            }

            return mogelijkeZetten;
        }



        public bool DoeZet(int rijZet, int kolomZet)
        {
            if (Afgelopen()) return false;

            bool zetGedaan = false;

            if (ZetMogelijk(rijZet, kolomZet))
            {
                Bord[rijZet, kolomZet] = AandeBeurt;

                //checks all positions around chosen position
                int startCheckRijPositie = rijZet - 1;
                int startCheckKolomPositie = kolomZet - 1;

                int eindCheckrijPositie = rijZet + 1;
                int eindCheckKolomPositie = kolomZet + 1;

                //check if starting Iterations And end Iterations arent < 0 OR bigger than board size.
                if (startCheckKolomPositie < 0) // Column left is not possible
                    startCheckKolomPositie = 0;

                if (startCheckRijPositie < 0) // Row top is not possible
                    startCheckRijPositie = 0;

                if (eindCheckrijPositie >= Bord.GetLength(0)) // Row Bottom is not possible
                    eindCheckrijPositie = rijZet;

                if (eindCheckKolomPositie >= Bord.GetLength(1)) // Column Right is not possible
                    eindCheckKolomPositie = kolomZet;

                for (int rij = startCheckRijPositie; rij <= eindCheckrijPositie; rij++)
                {
                    for (int kolom = startCheckKolomPositie; kolom <= eindCheckKolomPositie; kolom++)
                    {
                        Kleur nietAandeBeurt = NietAandeBeurt();
                        //If position has a different colour around it
                        if (Bord[rij, kolom] == nietAandeBeurt)
                        {
                            //Row + Column the opposite colour that is found above
                            int rijVerschil = (rij - rijZet); //1
                            int kolomVerschil = (kolom - kolomZet); //0
                            int rPos = rij;
                            int kPos = kolom;

                            //kPos & rPos is the starting position
                            // Tel door tot dat rPos en kPos buiten het bord vallen OF 
                            while (rPos >= 0 && rPos < Bord.GetLength(0) && kPos >= 0 && kPos < Bord.GetLength(1))
                            {
                                if (Bord[rPos, kPos] == Kleur.Geen)
                                {
                                    break;
                                }

                                //if discs are surrounded by this move
                                if (Bord[rPos, kPos] == AandeBeurt)
                                {
                                    //Alle stenen ertussen omzetten naar de kleur van degene die de zet heeft gemaakt.
                                    DraaiStenen(rij, kolom, rPos, kPos);
                                    zetGedaan = true;
                                    break;
                                }
                                rPos += rijVerschil;
                                kPos += kolomVerschil;
                            }
                        }
                    }
                }

                this.AandeBeurt = NietAandeBeurt();
                Afgelopen();
            }
            return zetGedaan;
        }

        /*
        private bool isInMogelijkeZetten(int rijZet, int kolomZet)
        {
            return MogelijkeZetten.Any(list => list[0] == rijZet && list[1] == kolomZet);
        }
        */

        private void DraaiStenen(int beginX, int beginY, int eindX, int eindY)
        {
            int rijVerschil = (eindX - beginX).LimitToRange(-1, 1);
            int kolomVerschil = (eindY - beginY).LimitToRange(-1, 1);

            int currentX = beginX;
            int currentY = beginY;

            while (Bord[currentX, currentY] != AandeBeurt)
            {
                Bord[currentX, currentY] = AandeBeurt;

                currentX += rijVerschil;
                currentY += kolomVerschil;
            }
        }


        public void ResetBord()
        {
            this.Bord = new Kleur[8, 8];
            this.Bord[3, 3] = Kleur.Wit;
            this.Bord[3, 4] = Kleur.Zwart;
            this.Bord[4, 3] = Kleur.Zwart;
            this.Bord[4, 4] = Kleur.Wit;
            this.AandeBeurt = Kleur.Zwart;
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

        // MovePossible()
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
