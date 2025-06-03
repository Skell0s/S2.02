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
    public class n_opt_niv_2 : Algorithme
    {
         /// <summary>
         /// Algorithme de répartition des personnages en équipes de 4, en respectant les rôles principaux et secondaires.
         /// </summary>
         /// <param name="jeuTest">JeuDe test avec personnages</param>
         /// <returns></returns>
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

            }

            Equipe[] tableauequipe = repartition.Equipes;
            int nbEquipes = tableauequipe.Length-1;
            Repartition repartition2 = new Repartition(jeuTest);

            int Max = 0;

            for (int i = 0; i < nbEquipes - 1; i++)
            {
                for (int j = i + 1; j < nbEquipes; j++)   
                {
                    var scoreI = tableauequipe[i].Score(Probleme.SIMPLE);

                    var scoreJ = tableauequipe[j].Score(Probleme.SIMPLE);
                    
                    if (scoreI > scoreJ)
                    {
                        Max = j;
                    }
                }
                repartition.AjouterEquipe(tableauequipe[Max]);
            }


            return repartition;
        }
    }
}
