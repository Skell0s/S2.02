﻿using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.AlgoTest
{
    internal class Level_balancing : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Personnage[] personnages = jeuTest.Personnages;
            Dictionary<Personnage, int> balances = personnages

            .ToDictionary(p => p, p => p.LvlPrincipal - 50);            // transformation d'une liste en dictonnaire qu'on diminue par 50

            var sorted = balances.OrderBy(kv => kv.Value).ToList();             // On trie les personnages par balance

            List<List<Personnage>> paires = new List<List<Personnage>>();
            HashSet<Personnage> utilises = new HashSet<Personnage>();

            // Étape 1 : former les meilleures paires possibles
            for (int i = 0; i < sorted.Count; i++)                 //parcour la liste jusqu'a qu'il n'y a plus de personne 
            {
                if (utilises.Contains(sorted[i].Key)) continue;   //on regarde si la personne esf déja utilsé

                Personnage p1 = sorted[i].Key; //p1 prend le nom du personnage
                int b1 = sorted[i].Value;      //b1 prend la valeur du personnage

                int meilleurEcart = int.MaxValue;
                Personnage meilleurP2 = null;

                for (int j = i + 1; j < sorted.Count; j++)
                {
                    Personnage p2 = sorted[j].Key;
                    if (utilises.Contains(p2)) continue;

                    int b2 = sorted[j].Value;
                    int ecart = Math.Abs(b1 + b2); // plus proche de 0

                    if (ecart < meilleurEcart)
                    {
                        meilleurEcart = ecart;
                        meilleurP2 = p2;
                    }

                    if (ecart == 0) break;
                }

                if (meilleurP2 != null)
                {
                    paires.Add(new List<Personnage> { p1, meilleurP2 });
                    utilises.Add(p1);
                    utilises.Add(meilleurP2);
                }
            }

            // Étape 2 : regrouper les paires en équipes de 4
            List<Equipe> equipes = new List<Equipe>();
            HashSet<int> pairesUtilisées = new HashSet<int>();

            for (int i = 0; i < paires.Count; i++)
            {
                if (pairesUtilisées.Contains(i)) continue;

                var paire1 = paires[i];
                int balance1 = paire1.Sum(p => balances[p]);

                int meilleurEcart = int.MaxValue;
                int meilleurIndex = -1;

                for (int j = i + 1; j < paires.Count; j++)
                {
                    if (pairesUtilisées.Contains(j)) continue;

                    var paire2 = paires[j];
                    int balance2 = paire2.Sum(p => balances[p]);

                    int ecart = Math.Abs(balance1 + balance2);

                    if (ecart < meilleurEcart)
                    {
                        meilleurEcart = ecart;
                        meilleurIndex = j;
                    }

                    if (ecart == 0) break;
                }

                if (meilleurIndex != -1)
                {
                    Equipe equipe = new Equipe();
                    foreach (var p in paire1.Concat(paires[meilleurIndex]))
                        equipe.AjouterMembre(p);

                    equipes.Add(equipe);
                    pairesUtilisées.Add(i);
                    pairesUtilisées.Add(meilleurIndex);
                }
            }

            // Étape 3 : créer la répartition
            Repartition repartition = new Repartition(jeuTest);
            
            foreach (var equipe in equipes)               // parcours les équipes
            {
                if (equipe.Score(Probleme.SIMPLE) <= 400) //si l'équipe est suppéreur a 400 alors ne pas crée l'équipe
                {
                    repartition.AjouterEquipe(equipe);
                }
            }

            return repartition;
        }
    }
}


