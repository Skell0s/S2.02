using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.AlgoTest;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class NSwapNiveau3 : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            AlgoExtremeEnPremier_niv2 algoInitial = new AlgoExtremeEnPremier_niv2();
            Repartition repartition = algoInitial.Repartir(jeuTest);

            repartition = AppliquerNSwapRole(repartition, jeuTest);

            repartition = SupprimerEquipeScoreEleve(repartition, jeuTest);

            stopwatch.Stop();
            TempsExecution = stopwatch.ElapsedMilliseconds;

            return repartition;
        }

        Repartition AppliquerNSwapRole(Repartition repartition, JeuTest jeuTest)
        {
            Repartition swapRepartition = repartition;
            bool ameliorationTrouvee = true;
    
            // Tant qu'on trouve des améliorations
            while (ameliorationTrouvee)
            {
                ameliorationTrouvee = false;

                // Parcourir toutes les paires d'équipes
                for (int i = 0; i < repartition.Equipes.Length; i++)
                {
                    for (int j = i + 1; j < repartition.Equipes.Length; j++)
                    {
                        Equipe equipe1 = repartition.Equipes[i];
                        Equipe equipe2 = repartition.Equipes[j];

                        // Pour chaque personnage dans la première équipe
                        foreach (Personnage personnage1 in equipe1.Membres)
                        {
                            // Pour chaque personnage dans la deuxième équipe
                            foreach (Personnage personnage2 in equipe2.Membres)
                            {
                                // Vérifier si l'échange est valide (niveau ET rôles secondaires)
                                if (EstEchangeValideNiveau3(equipe1, personnage1, equipe2, personnage2))
                                {
                                    // Calculer la différence de score après l'échange
                                    double scoreDifference = ScoreApresEchangeNiveau3(equipe1, personnage1, equipe2, personnage2) - ScoreActuelNiveau3(equipe1, equipe2);

                                    // Si l'échange améliore le score
                                    if (scoreDifference < 0)
                                    {
                                        // Effectuer l'échange
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

        // Vérifier si un échange est valide en prenant en compte les rôles secondaires
        private bool EstEchangeValideNiveau3(Equipe e1, Personnage p1, Equipe e2, Personnage p2)
        {
            // Créer les équipes résultantes de l'échange hypothétique
            Equipe[] equipesApresEchange = EquipesEchange(e1, p1, e2, p2);
            bool result = true;

            // Vérifier que les deux équipes sont valides après l'échange
            // en utilisant Probleme.ROLESECONDAIRE qui vérifie les rôles secondaires
            foreach (Equipe equipe in equipesApresEchange)
            {
                if (!equipe.EstValide(Probleme.ROLESECONDAIRE))
                {
                    result = result && false;
                }
            }

            return result;
        }

        // Calculer le score après échange avec niveau 3
        private double ScoreApresEchangeNiveau3(Equipe e1, Personnage p1, Equipe e2, Personnage p2)
        {
            double resultat = 0;
    
            // Calculer le score des équipes après échange
            foreach (Equipe equipe in EquipesEchange(e1, p1, e2, p2))
            {
                // Utiliser le score avec ROLESECONDAIRE
                resultat += equipe.Score(Probleme.ROLESECONDAIRE);
            }

            return resultat;
        }

        // Calculer le score actuel avec niveau 3
        private double ScoreActuelNiveau3(Equipe e1, Equipe e2)
        {
            // Utiliser le score avec ROLESECONDAIRE
            return e1.Score(Probleme.ROLESECONDAIRE) + e2.Score(Probleme.ROLESECONDAIRE);
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
    }
}
