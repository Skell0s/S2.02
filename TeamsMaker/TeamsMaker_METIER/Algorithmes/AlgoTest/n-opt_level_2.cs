using System;
using System.Collections.Generic;
using System.Linq;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.AlgoTest
{
    public class n_opt_level_2 : Algorithme
    {
        /// <summary>
        /// Algorithme de répartition des personnages en équipes de 4, en équilibrant les rôles principaux et les niveaux des personnages.
        /// </summary>
        /// <param name="jeuTest"></param>
        /// <returns></returns>
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Personnage[] personnages = jeuTest.Personnages;
            Array.Sort(personnages, new ComparateurPersonnageParNiveauPrincipal());

            List<Personnage> tanks = new List<Personnage>();
            List<Personnage> supports = new List<Personnage>();
            List<Personnage> dps = new List<Personnage>();

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

            int t = 0, s = 0, d = dps.Count - 1;

            var binomesTD = new List<(Equipe equipe, int niveau)>();
            var binomesSD = new List<(Equipe equipe, int niveau)>();

            while (t < tanks.Count && d >= 0)
            {
                Equipe e = new();
                e.AjouterMembre(tanks[t++]);
                e.AjouterMembre(dps[d--]);
                int niveau = e.Membres.Sum(p => p.LvlPrincipal);
                binomesTD.Add((e, niveau));
            }

            while (s < supports.Count && d >= 0)
            {
                Equipe e = new();
                e.AjouterMembre(supports[s++]);
                e.AjouterMembre(dps[d--]);
                int niveau = e.Membres.Sum(p => p.LvlPrincipal);
                binomesSD.Add((e, niveau));
            }

            // Trie des binômes SD par niveau croissant
            binomesSD = binomesSD.OrderBy(x => x.niveau).ToList();

            var repartitionFinale = new Repartition(jeuTest);
            var sdUtilises = new HashSet<int>();

            foreach (var (equipeTD, niveauTD) in binomesTD)
            {
                int niveauRecherche = 200 - niveauTD;
                int meilleurIndice = -1;
                double meilleureDiff = double.MaxValue;

                for (int i = 0; i < binomesSD.Count; i++)
                {
                    if (sdUtilises.Contains(i)) continue;

                    double diff = Math.Abs(binomesSD[i].niveau - niveauRecherche);
                    if (diff < meilleureDiff)
                    {
                        meilleureDiff = diff;
                        meilleurIndice = i;
                    }
                    if (diff == 0) break;
                }

                if (meilleurIndice != -1)
                {
                    var fusion = new Equipe();
                    foreach (Personnage p in equipeTD.Membres) fusion.AjouterMembre(p);
                    foreach (Personnage p in binomesSD[meilleurIndice].equipe.Membres) fusion.AjouterMembre(p);

                    if (fusion.EstValide(Probleme.SIMPLE))
                    {
                        repartitionFinale.AjouterEquipe(fusion);
                        sdUtilises.Add(meilleurIndice);
                    }
                }
            }

            return repartitionFinale;
        }
    }
}
