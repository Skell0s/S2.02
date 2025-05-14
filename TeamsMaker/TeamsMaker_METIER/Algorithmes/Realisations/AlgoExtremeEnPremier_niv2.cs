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
        internal class AlgoExtremeEnPremier_niv2 : Algorithme
        {
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

                    equipe.AjouterMembre(tanks[t]);
                    equipe.AjouterMembre(dps[d]);
                    d -= 1; t += 1;

                    equipe.AjouterMembre(supports[s]);
                    equipe.AjouterMembre(dps[d]);
                    d -= 1; s += 1;


                    repartition.AjouterEquipe(equipe);
                }

                return repartition;
            }
        }
    }

