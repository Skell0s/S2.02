using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class n_swap : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            AlgoExtremeEnPremier algoInitial = new AlgoExtremeEnPremier();
            Repartition repartition = algoInitial.Repartir(jeuTest);

            repartition = AppliquerNSwap(repartition, jeuTest);
            repartition = SupprimerEquipeScoreEleve(repartition, jeuTest);
            stopwatch.Stop();
            TempsExecution = stopwatch.ElapsedMilliseconds;

            return repartition;
        }

        public Repartition AppliquerNSwap(Repartition repartition, JeuTest jeuTest)
        {
            Repartition swapRepartition = repartition;
            bool ameliorationTrouvee = true;

            while (ameliorationTrouvee)
            {
                ameliorationTrouvee = false;

                for (int i = 0; i < repartition.Equipes.Length; i++)
                {
                    for (int j = i + 1; j < repartition.Equipes.Length; j++)
                    {
                        Equipe equipe1 = repartition.Equipes[i];
                        Equipe equipe2 = repartition.Equipes[j];
                        foreach (Personnage personnage1 in equipe1.Membres)
                        {
                            foreach (Personnage personnage2 in equipe2.Membres)
                            {
                                if (EstEchangeValide(equipe1, personnage1, equipe2, personnage2))
                                {
                                    double scoreDifference = ScoreApresEchange(equipe1, personnage1, equipe2, personnage2) - ScoreActuel(equipe1, equipe2);
                                    if (scoreDifference < 0)
                                    {
                                        swapRepartition = EffectuerEchange(repartition, jeuTest, equipe1, personnage1, equipe2, personnage2);
                                        ameliorationTrouvee = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return swapRepartition;
        }

        private Equipe[] EquipesEchange(Equipe e1, Personnage p1, Equipe e2, Personnage p2)
        {
            Equipe equipe1 = new Equipe();
            Equipe equipe2 = new Equipe();
            List<Equipe> equipes = new List<Equipe>();
            equipe1.AjouterMembre(p1);
            equipe2.AjouterMembre(p2);

            foreach (Personnage p in e1.Membres)
            {
                if (p != p1)
                {
                    equipe1.AjouterMembre(p);
                }
            }
            foreach (Personnage p in e2.Membres)
            {
                if (p != p2)
                {
                    equipe2.AjouterMembre(p);
                }
            }
            equipes.Add(equipe1);
            equipes.Add(equipe2);
            return equipes.ToArray();
        }

        private bool EstEchangeValide(Equipe e1, Personnage p1, Equipe e2, Personnage p2)
        {
            bool result = true;
            
            foreach (Equipe equipe in EquipesEchange(e1, p1, e2, p2))
            {
                result = result && equipe.EstValide(Probleme.SIMPLE);
            }

            return result;
        }

        private double ScoreApresEchange(Equipe e1, Personnage p1, Equipe e2, Personnage p2)
        {
            double result = 0;

            foreach (Equipe equipe in EquipesEchange(e1, p1, e2, p2))
            {
                result += equipe.Score(Probleme.SIMPLE);
            }

            return result;
        }

        private double ScoreActuel(Equipe e1, Equipe e2)
        {
            return e1.Score(Probleme.SIMPLE) + e2.Score(Probleme.SIMPLE);
        }

        private Repartition EffectuerEchange(Repartition r, JeuTest jeuTest, Equipe e1, Personnage p1, Equipe e2, Personnage p2)
        {
            Repartition repartition = new Repartition(jeuTest);
            foreach (Equipe equipe in EquipesEchange(e1, p1, e2, p2))
            {
                repartition.AjouterEquipe(equipe);
            }
            foreach (Equipe equipe in r.Equipes)
            {
                if (equipe != e1 && equipe != e2) 
                { 
                    repartition.AjouterEquipe(equipe); 
                }

            }
            return repartition;
        }
    }
}
