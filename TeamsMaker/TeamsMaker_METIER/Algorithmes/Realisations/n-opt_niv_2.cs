using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class n_opt_niv_2 : Algorithme
    {
        /* private List<Personnage> equipede2 = new List<Personnage>();
         private List<Equipe> equipes2;

         public void EquipeDe2(Personnage personnage)
         {
             this.equipede2.Add(personnage);
         }
         public void AjouterEquipe2(Equipe equipe)
         {
             this.equipes2.Add(equipe);
         }*/

        public override Repartition Repartir(JeuTest jeuTest)
        {
            Personnage[] personnages = jeuTest.Personnages;
            Array.Sort(personnages, new ComparateurPersonnageParNiveauPrincipal());
            Repartition repartition = new Repartition(jeuTest);

            List<Personnage> tanks = new List<Personnage>();
            List<Personnage> supports = new List<Personnage>();
            List<Personnage> dps = new List<Personnage>();

            foreach (var p in personnages)
            {
                switch (p.RolePrincipal)
                {
                    case Role.TANK: tanks.Add(p); break;
                    case Role.SUPPORT: supports.Add(p); break;
                    case Role.DPS: dps.Add(p); break;
                }
            }

            tanks.Sort(new ComparateurPersonnageParNiveauPrincipal());
            supports.Sort(new ComparateurPersonnageParNiveauPrincipal());
            dps.Sort(new ComparateurPersonnageParNiveauPrincipal());

            int d = dps.Count - 1;
            int t = 0;
            int s = 0;

            for (int i = 0; i <= personnages.Length - 4; i += 4)
            {
                Equipe equipeDe2 = new Equipe();
                Equipe equipeDe4 = new Equipe();
                // Ajouter Tank, DPS, Support, DPS
                
                equipeDe2.AjouterMembre(tanks[t]);
                equipeDe2.AjouterMembre(dps[d]);


                repartition.AjouterEquipe(equipeDe2);

                d -= 1; t += 1;

                equipeDe2.AjouterMembre(supports[s]);
                equipeDe2.AjouterMembre(dps[d]);


                repartition.AjouterEquipe(equipeDe2);

                d -= 1; s += 1;


                // Créer équipe de 2 à partir du tank et du premier dps

            }

            Equipe[] tableauequipe = repartition.Equipes;
            int nbEquipes = tableauequipe.Length-1;
            Repartition repartition2 = new Repartition(jeuTest);

            int Max = 0;

            for (int i = 0; i < nbEquipes - 1; i++)   //toute les équipe de équipe une a dernier
            {
                for (int j = i + 1; j < nbEquipes; j++)   //permet de faire une boucle pour comparé chaque truc
                {

                    //equipeDe2.AjouterMembre(supports[s]);
                    //equipeDe2.AjouterMembre(dps[d]);
                    //repartition.AjouterEquipe(equipeDe2);


                    var scoreI = tableauequipe[i].Score(Probleme.SIMPLE);     //

                    var scoreJ = tableauequipe[j].Score(Probleme.SIMPLE);
                    
                    
                    if (scoreI > scoreJ)
                    {
                        // Exemple : afficher ou mémoriser la meilleure équipe

                        Max = j;
                    }
                }
                repartition.AjouterEquipe(tableauequipe[Max]);
            }


            return repartition;
        }
    }
}
 /*

Equipe[] tableauequipe = repartition.Equipes;

int boucle = tableauequipe.Length - 1; // nombre de groupe 


for (int debut = 0; debut < boucle; debut++) //toute les équipe de équipe une a dernier
{
    for (int e = 0; e < boucle; debut++) //permet de faire une boucle pour comparé chaque truc
    {
        //crée une équipe avec léquipe de deux et une autre zéquipe de deux

        if (tableauequipe[boucle] == tableauequipe[e]) //si le score de debut est supérieur au score du nouveau groupe de deux   (equipe.Score(Probleme.SIMPLE) > )
        {

        }
    }



}


//Equipe derniereEquipe = tableauequipe.Last();
//Personnage dernierPerso = derniereEquipe.Membres.Last();

// prendre la première équipe dans un for (equipe.Score(Probleme.SIMPLE) <= 400)
return repartition;
}
}
}
            // 3. Optimisation des équipes avec l'algorithme n-opt

        
        private void OptimiserEquipes(Repartition repartition)
        {
            if (repartition.Equipes.Length < 2) return; // Pas d'optimisation possible avec moins de 2 équipes
            
            bool ameliorationTrouvee = true;
            int iterations = 0;
            
            // Continuer tant qu'on trouve des améliorations et qu'on n'a pas dépassé le nombre max d'itérations
            while (ameliorationTrouvee && iterations < maxIterations)
            {
                iterations++;
                ameliorationTrouvee = false;
                
                // Calculer le score global initial
                double scoreGlobalInitial = CalculerScoreGlobal(repartition);
                
                // Pour chaque paire d'équipes
                for (int i = 0; i < repartition.Equipes.Length - 1 && !ameliorationTrouvee; i++)
                {
                    Equipe equipe1 = repartition.Equipes[i];
                    
                    for (int j = i + 1; j < repartition.Equipes.Length && !ameliorationTrouvee; j++)
                    {
                        Equipe equipe2 = repartition.Equipes[j];
                        
                        // Pour chaque paire possible de membres à échanger
                        foreach (var membre1 in equipe1.Membres)
                        {
                            foreach (var membre2 in equipe2.Membres)
                            {
                                // Simuler l'échange
                                equipe1.RetirerMembre(membre1);
                                equipe2.RetirerMembre(membre2);
                                equipe1.AjouterMembre(membre2);
                                equipe2.AjouterMembre(membre1);
                                
                                // Calculer le nouveau score global
                                double nouveauScoreGlobal = CalculerScoreGlobal(repartition);
                                
                                // Si l'échange améliore le score global, le conserver
                                if (nouveauScoreGlobal < scoreGlobalInitial)
                                {
                                    ameliorationTrouvee = true;
                                    break; // On sort de la boucle pour continuer avec cette nouvelle configuration
                                }
                                else
                                {
                                    // Sinon, annuler l'échange
                                    equipe1.RetirerMembre(membre2);
                                    equipe2.RetirerMembre(membre1);
                                    equipe1.AjouterMembre(membre1);
                                    equipe2.AjouterMembre(membre2);
                                }
                            }
                            
                            if (ameliorationTrouvee) break;
                        }
                    }
                }
            }
        }
        
        // Fonction d'évaluation qui calcule un score global pour la répartition
        // Plus le score est bas, meilleure est la répartition (on minimise les différences entre équipes)
        private double CalculerScoreGlobal(Repartition repartition)
        {
            if (repartition.Equipes.Length == 0) return double.MaxValue;
            
            // Calculer le score moyen des équipes
            double sommeScores = 0;
            foreach (var equipe in repartition.Equipes)
            {
                sommeScores += equipe.Score(Probleme.SIMPLE);
            }
            double scoreMoyen = sommeScores / repartition.Equipes.Length;
            
            // Calculer l'écart-type des scores (mesure de déséquilibre)
            double sommeEcartsCarres = 0;
            foreach (var equipe in repartition.Equipes)
            {
                double ecart = equipe.Score(Probleme.SIMPLE) - scoreMoyen;
                sommeEcartsCarres += ecart * ecart;
            }
            
            // Pénaliser les équipes incomplètes
            double penaliteEquipesIncompletes = 0;
            foreach (var equipe in repartition.Equipes)
            {
                if (equipe.Membres.Count != 4)
                {
                    penaliteEquipesIncompletes += 1000 * (4 - equipe.Membres.Count);
                }
            }
            
            // Pénaliser les équipes qui n'ont pas la composition idéale (1 Tank, 1 Support, 2 DPS)
            double penaliteComposition = 0;
            foreach (var equipe in repartition.Equipes)
            {
                int nbTanks = equipe.Membres.Count(m => m.RolePrincipal == Role.TANK);
                int nbSupports = equipe.Membres.Count(m => m.RolePrincipal == Role.SUPPORT);
                int nbDPS = equipe.Membres.Count(m => m.RolePrincipal == Role.DPS);
                
                penaliteComposition += Math.Abs(nbTanks - 1) * 100;
                penaliteComposition += Math.Abs(nbSupports - 1) * 100;
                penaliteComposition += Math.Abs(nbDPS - 2) * 100;
            }
            
            // Le score global combine l'écart-type et les pénalités
            return Math.Sqrt(sommeEcartsCarres / repartition.Equipes.Length) + penaliteEquipesIncompletes + penaliteComposition;
        }
    }
}*/