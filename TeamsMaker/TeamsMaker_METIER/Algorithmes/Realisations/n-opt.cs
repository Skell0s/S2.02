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
                Equipe equipe = new Equipe();
                Equipe equipe2 = new Equipe();
                // Ajouter Tank, DPS, Support, DPS
                equipe.AjouterMembre(tanks[t]);
                equipe.AjouterMembre(dps[d]);

                equipe2.AjouterMembre(tanks[t]);
                equipe2.AjouterMembre(dps[d]);
                //repartition.AjouterEquipe(equipe2);

                d -= 1; t += 1;

                equipe.AjouterMembre(supports[s]);
                equipe.AjouterMembre(dps[d]);

                equipe2.AjouterMembre(supports[s]);
                equipe2.AjouterMembre(dps[d]);
                //repartition.AjouterEquipe(equipe2);

                d -= 1; s += 1;


                // Créer équipe de 2 à partir du tank et du premier dps

                repartition.AjouterEquipe(equipe);
            }

            Equipe[] tableauequipe = repartition.Equipes;

            int boucle = tableauequipe.Length - 1;

            //Equipe derniereEquipe = tableauequipe.Last();
            //Personnage dernierPerso = derniereEquipe.Membres.Last();

            // prendre la première équipe dans un for (equipe.Score(Probleme.SIMPLE) <= 400)
            return repartition;
        }
    }
}
