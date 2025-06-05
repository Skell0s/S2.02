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
    public class NSwapNiveau3 : Algorithme
    {
        /// <summary>
        /// Algorithme de répartition des personnages en équipes de 4, en utilisant la méthode N-Swap pour optimiser les équipes, en prenant compte du rôle principale et secondaire,
        /// </summary>
        /// <param name="jeuTest"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Applique l'algorithme N-Swap pour optimiser les équipes en prenant en compte le rôle secondaire des personnages.
        /// </summary>
        /// <param name="repartition"></param>
        /// <param name="jeuTest"></param>
        /// <returns></returns>

        public Repartition AppliquerNSwapRole(Repartition repartition, JeuTest jeuTest)
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
                                if (EstEchangeValideNiveau3(equipe1, personnage1, equipe2, personnage2))
                                {
                                    double scoreDifference = ScoreApresEchangeNiveau3(equipe1, personnage1, equipe2, personnage2) - ScoreActuelNiveau3(equipe1, equipe2);

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

        /// <summary>
        /// Vérifie si l'échange entre deux personnages de deux équipes est valide pour le niveau 3 (rôle secondaire inclus).
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="p1"></param>
        /// <param name="e2"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private bool EstEchangeValideNiveau3(Equipe e1, Personnage p1, Equipe e2, Personnage p2)
        {
            Equipe[] equipesApresEchange = EquipesEchange(e1, p1, e2, p2);
            bool result = true;

            foreach (Equipe equipe in equipesApresEchange)
            {
                if (!equipe.EstValide(Probleme.ROLESECONDAIRE))
                {
                    result = result && false;
                }
            }

            return result;
        }

        /// <summary>
        /// Calculer le score après un échange de personnages entre deux équipes, en prenant en compte le rôle secondaire.
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="p1"></param>
        /// <param name="e2"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private double ScoreApresEchangeNiveau3(Equipe e1, Personnage p1, Equipe e2, Personnage p2)
        {
            double resultat = 0;
    
            foreach (Equipe equipe in EquipesEchange(e1, p1, e2, p2))
            {
                resultat += equipe.Score(Probleme.ROLESECONDAIRE);
            }

            return resultat;
        }

        /// <summary>
        /// Calculer le score actuel de deux équipes pour le niveau 3, en prenant en compte le rôle secondaire.
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        private double ScoreActuelNiveau3(Equipe e1, Equipe e2)
        {
              return e1.Score(Probleme.ROLESECONDAIRE) + e2.Score(Probleme.ROLESECONDAIRE);
        }

        /// <summary>
        /// Effectue l'échange de personnages entre deux équipes et crée une nouvelle répartition.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="jeuTest"></param>
        /// <param name="e1"></param>
        /// <param name="p1"></param>
        /// <param name="e2"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Crée deux nouvelles équipes après un échange de personnages entre deux équipes existantes.
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="p1"></param>
        /// <param name="e2"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
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
