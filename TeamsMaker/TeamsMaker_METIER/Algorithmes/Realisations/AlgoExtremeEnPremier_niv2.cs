using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class AlgoExtremeEnPremier_niv2 : Algorithme
    {
        /// <summary>
        /// Algorithme de répartition des personnages en équipes de 4, en utilisant la méthode "Extrêmes en premier" niveau 2.
        /// </summary>
        /// <param name="jeuTest"></param>
        /// <returns></returns>
        /// 
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Personnage[] personnages = jeuTest.Personnages;
            Repartition repartition = new Repartition(jeuTest);

            List<Personnage> tanks = new List<Personnage>();
            List<Personnage> supports = new List<Personnage>();
            List<Personnage> dps = new List<Personnage>();

            //étape 1 : création des listes de personnages par rôle et tri croissant par niveau principal
            foreach (Personnage p in personnages)
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

            // étape 2 : création des équipes en utilisant la méthode "Extrêmes en premier" avec les rôles principaux
            while (t < tanks.Count && s < supports.Count && d - 1 >= 0)
            {
                if (d != 0)
                {
                    Equipe equipe = new Equipe();

                    equipe.AjouterMembre(tanks[t]);
                    equipe.AjouterMembre(dps[d]);
                    d -= 1; t += 1;

                    equipe.AjouterMembre(supports[s]);
                    equipe.AjouterMembre(dps[d]);
                    d -= 1; s += 1;


                    repartition.AjouterEquipe(equipe);
                }
                
            }

            return repartition;
        }
    }
}

