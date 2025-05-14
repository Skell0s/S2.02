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
    internal class n_opt : Algorithme
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

                equipeDe4.AjouterMembre(tanks[t]);
                equipeDe4.AjouterMembre(dps[d]);
                //repartition.AjouterEquipe(equipeDe2);

                d -= 1; t += 1;

                equipeDe2.AjouterMembre(supports[s]);
                equipeDe2.AjouterMembre(dps[d]);

                equipeDe4.AjouterMembre(supports[s]);
                equipeDe4.AjouterMembre(dps[d]);
                //repartition.AjouterEquipe(equipeDe2);

                d -= 1; s += 1;


                // Créer équipe de 2 à partir du tank et du premier dps

                //////////////////////////////////////////////////////////////////////////////////////repartition.AjouterEquipe(equipeDe4);
            }
            Equipe[] tableauequipe = repartition.Equipes;
            int nbEquipes = tableauequipe.Length;

            for (int i = 0; i < nbEquipes - 1; i++)
            {
                for (int j = i + 1; j < nbEquipes; j++)
                {
                    // Comparer deux équipes par leur score sur le problème SIMPLE
                    var scoreI = tableauequipe[i].Score(Probleme.SIMPLE);
                    var scoreJ = tableauequipe[j].Score(Probleme.SIMPLE);

                    if (scoreI > scoreJ)
                    {
                        // Exemple : afficher ou mémoriser la meilleure équipe
                        repartition.AjouterEquipe(tableauequipe[i]);
                    }
                }
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
}*/
