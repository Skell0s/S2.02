using System;
using System.Collections.Generic;
using System.Linq;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    internal class algopb2jul : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Personnage[] personnages = jeuTest.Personnages;
            Repartition repartition = new Repartition(jeuTest);

            // Trier les personnages par rôle principal
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


            // Marquer les personnages déjà utilisés
            HashSet<Personnage> dejaUtilises = new HashSet<Personnage>();

            // Essayer toutes les combinaisons Tank + Support + 2 DPS
            foreach (var tank in tanks)
            {
                if (dejaUtilises.Contains(tank)) continue; //si tank existe pas

                foreach (var support in supports)
                {
                    if (dejaUtilises.Contains(support)) continue; //si support existe pas


                    for (int i = 0; i < dps.Count; i++)              //cherche si il reste des dps paire 
                    {
                        if (dejaUtilises.Contains(dps[i])) continue;  // si dps paire existe pas

                        for (int j = i + 1; j < dps.Count; j++)  // cherche si il reste des dps paire 
                        {
                            if (dejaUtilises.Contains(dps[j])) continue; // si dps impaire existe pas 

                            var equipeTemp = new List<Personnage> { tank, support, dps[i], dps[j] };  //crée une liste de personnage sous forme d'une équipe

                            
                            if (equipeTemp.Distinct().Count() != 4) continue;   // S’assurer qu’il n’y a pas de doublons


                            Equipe equipe = new Equipe();                             // Créer et valider l’équipe
                            
                            foreach (var p in equipeTemp) 
                            { 
                                equipe.AjouterMembre(p);
                            }

                            if (equipe.EstValide(Probleme.ROLEPRINCIPAL))
                            {
                                repartition.AjouterEquipe(equipe);
                                foreach (var p in equipeTemp)
                                    dejaUtilises.Add(p);

                                // On passe au prochain tank après avoir formé une équipe
                                goto ProchainTank;
                            }
                        }
                    }
                }

            ProchainTank:; // continue avec le tank suivant
            }

            return repartition;
        }
    }
}
