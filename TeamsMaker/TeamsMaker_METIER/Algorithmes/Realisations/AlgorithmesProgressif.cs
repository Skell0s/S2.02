using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class AlgorithmesProgressif : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Repartition AlgoPro = AlgorithmeProgressif(jeuTest);
            AlgoPro = SupprimerEquipeScoreEleve(AlgoPro, jeuTest);
            return AlgoPro;
        }

        public Repartition AlgorithmeProgressif(JeuTest jeuTest)
        {
            List<Personnage> disponibles = jeuTest.Personnages.ToList();
            Repartition repartition = new Repartition(jeuTest);
            bool formationPossible = true;

            while (formationPossible && disponibles.Count >= 4)
            {
                Equipe equipe = new Equipe();

                while (equipe.Membres.Length < 4)
                {
                    Personnage meilleurCandidat = null;
                    double meilleurEcart = double.MaxValue;

                    foreach (var candidat in disponibles)
                    {
                        var tempMembres = new List<Personnage>(equipe.Membres);
                        tempMembres.Add(candidat);

                        double moyenne = tempMembres.Average(p => p.LvlPrincipal);
                        double ecart = Math.Abs(moyenne - 50);

                        if (ecart < meilleurEcart)
                        {
                            meilleurEcart = ecart;
                            meilleurCandidat = candidat;
                        }
                    }

                    if (meilleurCandidat != null)
                    {
                        equipe.AjouterMembre(meilleurCandidat);
                        disponibles.Remove(meilleurCandidat);
                    }

                    if (equipe.Membres.Length == 4)
                    {
                        double moyenneEquipe = equipe.Membres.Average(p => p.LvlPrincipal);
                        double scoreEquipe = (moyenneEquipe - 50) * (moyenneEquipe - 50);

                        if (equipe.EstValide(Probleme.SIMPLE))
                        {
                            repartition.AjouterEquipe(equipe);
                        }
                        else
                        {
                            formationPossible = false;
                        }
                    }

                    if (disponibles.Count < 4)
                    {
                        formationPossible = false;
                    }
                }
            }
            return repartition;
        }
    }
}
