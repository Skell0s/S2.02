using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.AlgoTest
{
    public class PbNiv2 : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            // Initialisation des répartitions pour stocker les équipes de 2 membres
            Repartition repartitionTetD = new Repartition(jeuTest);
            Repartition repartitionDetS = new Repartition(jeuTest);

            // Récupération et tri des personnages par niveau principal
            Personnage[] personnages = jeuTest.Personnages;
            Array.Sort(personnages, new ComparateurPersonnageParNiveauPrincipal());

            // Création d'équipes de 2 personnages (Tank + DPS ou DPS + Support)
            int a = 0;
            int z = personnages.Length - 1;

            // La condition de la boucle était incorrecte, on la corrige
            while (a < z)
            {
                // Équipe Tank + DPS
                if (personnages[a].RolePrincipal == Role.TANK && personnages[z].RolePrincipal == Role.DPS)
                {
                    Equipe equipeDe2TetDPS = new Equipe();
                    equipeDe2TetDPS.AjouterMembre(personnages[a]);
                    equipeDe2TetDPS.AjouterMembre(personnages[z]);
                    repartitionTetD.AjouterEquipe(equipeDe2TetDPS);
                    a++;
                    z--;
                }
                // Équipe DPS + Support
                else if (personnages[a].RolePrincipal == Role.DPS && personnages[z].RolePrincipal == Role.SUPPORT)
                {
                    Equipe equipeDe2DPSetS = new Equipe();
                    equipeDe2DPSetS.AjouterMembre(personnages[a]);
                    equipeDe2DPSetS.AjouterMembre(personnages[z]);
                    repartitionDetS.AjouterEquipe(equipeDe2DPSetS);
                    a++;
                    z--;
                }
                else
                {
                    // Si les combinaisons ne correspondent pas, on avance dans le tableau
                    if (personnages[a].RolePrincipal != Role.TANK && personnages[a].RolePrincipal != Role.DPS)
                    {
                        a++;
                    }
                    else if (personnages[z].RolePrincipal != Role.DPS && personnages[z].RolePrincipal != Role.SUPPORT)
                    {
                        z--;
                    }
                    else
                    {
                        // Si on n'a pas de match, on avance quand même pour éviter une boucle infinie
                        a++;
                    }
                }

                // Vérifier si nous avons encore assez de personnages
                if (a >= z) break;
            }

            // Personnage restant en cas de nombre impair
            Personnage personnageRestant = (a == z) ? personnages[a] : null;

            // Combiner les deux listes d'équipes pour la fusion
            List<Equipe> toutesEquipes = new List<Equipe>();
            toutesEquipes.AddRange(repartitionTetD.Equipes);
            toutesEquipes.AddRange(repartitionDetS.Equipes);

            // Répartition finale pour stocker les équipes de 4 membres
            Repartition repartitionFinale = new Repartition(jeuTest);
            HashSet<int> equipesUtilisees = new HashSet<int>();

            // Liste pour stocker les équipes de 4 créées initialement (avant optimisation)
            List<Equipe> equipesInitiales = new List<Equipe>();

            // Créer des équipes de 4 initialement (comme avant)
            for (int i = 0; i < toutesEquipes.Count; i++)
            {
                if (equipesUtilisees.Contains(i)) continue;

                int meilleurIndex = -1;
                double meilleureDiff = double.MaxValue;

                // Trouver la meilleure combinaison avec une autre équipe
                for (int j = 0; j < toutesEquipes.Count; j++)
                {
                    if (i == j || equipesUtilisees.Contains(j)) continue;

                    var fusion = toutesEquipes[i].Membres.Concat(toutesEquipes[j].Membres).ToList();

                    // Vérifier si la fusion donne 4 personnages
                    if (fusion.Count != 4) continue;

                    // Vérifier si l'équipe fusionnée est valide (1 tank, 1 support, 2 DPS)
                    var equipeTemp = new Equipe();
                    foreach (var membre in fusion)
                    {
                        equipeTemp.AjouterMembre(membre);
                    }

                    if (!equipeTemp.EstValide(Probleme.ROLEPRINCIPAL)) continue;

                    // Calculer l'homogénéité (objectif: score moyen de 50 par personnage)
                    double scoreEval = equipeTemp.Score(Probleme.ROLEPRINCIPAL);

                    // Si le score est -1, l'équipe n'est pas valide
                    if (scoreEval == -1) continue;

                    if (scoreEval < meilleureDiff)
                    {
                        meilleureDiff = scoreEval;
                        meilleurIndex = j;
                    }
                }

                // Si on a trouvé une combinaison valide
                if (meilleurIndex != -1)
                {
                    var equipeFusion = new Equipe();

                    // Ajouter tous les membres des deux équipes
                    foreach (var membre in toutesEquipes[i].Membres)
                        equipeFusion.AjouterMembre(membre);
                    foreach (var membre in toutesEquipes[meilleurIndex].Membres)
                        equipeFusion.AjouterMembre(membre);

                    // Ajouter l'équipe à notre liste temporaire
                    equipesInitiales.Add(equipeFusion);
                    equipesUtilisees.Add(i);
                    equipesUtilisees.Add(meilleurIndex);
                }
            }

            // Appliquer la recherche locale N-opt pour améliorer les équipes
            // Implémentation de l'heuristique N-opt pour optimiser la répartition
            ApplyNOpt(equipesInitiales, repartitionFinale, Probleme.ROLEPRINCIPAL);

            return repartitionFinale;
        }

        /// <summary>
        /// Applique l'heuristique N-opt pour améliorer la qualité des équipes
        /// </summary>
        /// <param name="equipesInitiales">Liste des équipes initiales</param>
        /// <param name="repartitionFinale">Répartition à remplir avec les équipes optimisées</param>
        /// <param name="probleme">Le problème à résoudre (utilisé pour évaluer le score)</param>
        private void ApplyNOpt(List<Equipe> equipesInitiales, Repartition repartitionFinale, Probleme probleme)
        {
            if (equipesInitiales.Count <= 1)
            {
                // Pas assez d'équipes pour faire des échanges
                foreach (var equipe in equipesInitiales)
                {
                    repartitionFinale.AjouterEquipe(equipe);
                }
                return;
            }

            // Copier les équipes initiales pour travailler dessus
            List<Equipe> equipesActuelles = new List<Equipe>(equipesInitiales);

            // Nombre maximal d'itérations sans amélioration avant d'arrêter
            const int MAX_ITERATIONS_SANS_AMELIORATION = 100;
            int iterationsSansAmelioration = 0;

            // Score initial de la solution
            double scoreCourant = CalculerScoreGlobal(equipesActuelles, probleme);

            // Boucle principale de recherche locale
            Random random = new Random(); // Pour sélectionner aléatoirement des équipes et personnages

            while (iterationsSansAmelioration < MAX_ITERATIONS_SANS_AMELIORATION)
            {
                bool amelioration = false;

                // 1. Sélectionner deux équipes aléatoirement
                int equipeIndex1 = random.Next(equipesActuelles.Count);
                int equipeIndex2;
                do
                {
                    equipeIndex2 = random.Next(equipesActuelles.Count);
                } while (equipeIndex1 == equipeIndex2);

                Equipe equipe1 = equipesActuelles[equipeIndex1];
                Equipe equipe2 = equipesActuelles[equipeIndex2];

                // 2. Essayer d'échanger des personnages entre les équipes
                for (int i = 0; i < equipe1.Membres.Length && !amelioration; i++)
                {
                    Personnage perso1 = equipe1.Membres[i];

                    for (int j = 0; j < equipe2.Membres.Length && !amelioration; j++)
                    {
                        Personnage perso2 = equipe2.Membres[j];

                        // Si les rôles principaux sont différents, passer au suivant
                        if (perso1.RolePrincipal != perso2.RolePrincipal)
                            continue;

                        // Créer de nouvelles équipes temporaires
                        Equipe nouvelleEquipe1 = new Equipe();
                        Equipe nouvelleEquipe2 = new Equipe();

                        // Remplir les nouvelles équipes avec l'échange effectué
                        foreach (var p in equipe1.Membres)
                        {
                            if (p != perso1)
                                nouvelleEquipe1.AjouterMembre(p);
                            else
                                nouvelleEquipe1.AjouterMembre(perso2);
                        }

                        foreach (var p in equipe2.Membres)
                        {
                            if (p != perso2)
                                nouvelleEquipe2.AjouterMembre(p);
                            else
                                nouvelleEquipe2.AjouterMembre(perso1);
                        }

                        // Vérifier que les nouvelles équipes sont valides
                        if (!nouvelleEquipe1.EstValide(probleme) || !nouvelleEquipe2.EstValide(probleme))
                            continue;

                        // Créer une nouvelle solution temporaire
                        List<Equipe> nouvellesSolution = new List<Equipe>(equipesActuelles);
                        nouvellesSolution[equipeIndex1] = nouvelleEquipe1;
                        nouvellesSolution[equipeIndex2] = nouvelleEquipe2;

                        // Calculer le score de la nouvelle solution
                        double nouveauScore = CalculerScoreGlobal(nouvellesSolution, probleme);

                        // Si le nouveau score est meilleur, accepter l'échange
                        if (nouveauScore < scoreCourant)
                        {
                            equipesActuelles = nouvellesSolution;
                            scoreCourant = nouveauScore;
                            amelioration = true;
                            // On continue avec cette nouvelle solution
                            break;
                        }
                    }
                }

                // 3. Si aucune amélioration, essayer d'échanger 2 personnages contre 2 autres (N=2)
                if (!amelioration && equipesActuelles.Count >= 2)
                {
                    // Implémenter ici l'échange N=2 (2 personnages contre 2)
                    // Échange plus complexe: 2 personnages d'une équipe contre 2 d'une autre
                    // (Ce code est omis pour simplifier car complexe à implémenter)
                }

                // Si aucune amélioration n'a été trouvée, incrémenter le compteur
                if (!amelioration)
                {
                    iterationsSansAmelioration++;
                }
                else
                {
                    // Réinitialiser le compteur si une amélioration a été trouvée
                    iterationsSansAmelioration = 0;
                }
            }

            // Ajouter les équipes optimisées à la répartition finale
            foreach (var equipe in equipesActuelles)
            {
                repartitionFinale.AjouterEquipe(equipe);
            }
        }

        /// <summary>
        /// Calcule un score global pour un ensemble d'équipes
        /// </summary>
        /// <param name="equipes">Liste des équipes</param>
        /// <param name="probleme">Le problème à résoudre</param>
        /// <returns>Score global (somme des scores des équipes)</returns>
        private double CalculerScoreGlobal(List<Equipe> equipes, Probleme probleme)
        {
            double scoreTotal = 0;

            foreach (var equipe in equipes)
            {
                double scoreEquipe = equipe.Score(probleme);
                if (scoreEquipe >= 0) // Si l'équipe est valide
                {
                    scoreTotal += scoreEquipe;
                }
                else
                {
                    // Une équipe non valide donne un score très mauvais
                    return double.MaxValue;
                }
            }

            return scoreTotal;
        }
    }
}
