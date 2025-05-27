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
    public class n_opt_level_2 : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Personnage[] personnages = jeuTest.Personnages;
            Array.Sort(personnages, new ComparateurPersonnageParNiveauPrincipal());

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

            int t = 0, s = 0, d = dps.Count - 1;

            Repartition repartition2td = new Repartition(jeuTest);
            Repartition repartition2sd = new Repartition(jeuTest);

            // Création des binômes TD
            while (t < tanks.Count && d >= 0)
            {
                Equipe equipe = new Equipe();
                equipe.AjouterMembre(tanks[t++]);
                equipe.AjouterMembre(dps[d--]);
                repartition2td.AjouterEquipe(equipe);
            }

            // Création des binômes SD
            while (s < supports.Count && d >= 0)
            {
                Equipe equipe = new Equipe();
                equipe.AjouterMembre(supports[s++]);
                equipe.AjouterMembre(dps[d--]);
                repartition2sd.AjouterEquipe(equipe);
            }

            List<Equipe> toutesEquipesTD = repartition2td.Equipes.ToList();
            List<Equipe> toutesEquipesSD = repartition2sd.Equipes.ToList();

            Repartition repartitionFinale = new Repartition(jeuTest);
            HashSet<int> equipesTDUtilisees = new HashSet<int>();
            HashSet<int> equipesSDUtilisees = new HashSet<int>();

            // Optimisation à la n_opt : pour chaque TD, on cherche le meilleur SD
            for (int i = 0; i < toutesEquipesTD.Count; i++)
            {
                if (equipesTDUtilisees.Contains(i)) continue;

                int meilleurIndex = -1;
                double meilleureDiff = double.MaxValue;

                for (int j = 0; j < toutesEquipesSD.Count; j++)
                {
                    if (equipesSDUtilisees.Contains(j)) continue;

                    var fusion = toutesEquipesTD[i].Membres.Concat(toutesEquipesSD[j].Membres).ToList();

                    if (fusion.Count != 4) continue;

                    double diff = Math.Abs((50 - fusion.Sum(p => p.LvlPrincipal)) * (50 - fusion.Sum(p => p.LvlPrincipal)));
                    if (diff < meilleureDiff)
                    {
                        meilleureDiff = diff;
                        meilleurIndex = j;
                    }
                }

                if (meilleurIndex != -1)
                {
                    var fusionEquipe = new Equipe();
                    foreach (var p in toutesEquipesTD[i].Membres)
                        fusionEquipe.AjouterMembre(p);
                    foreach (var p in toutesEquipesSD[meilleurIndex].Membres)
                        fusionEquipe.AjouterMembre(p);

                    if (fusionEquipe.EstValide(Probleme.SIMPLE))
                    {
                        repartitionFinale.AjouterEquipe(fusionEquipe);
                        equipesTDUtilisees.Add(i);
                        equipesSDUtilisees.Add(meilleurIndex);
                    }
                }
            }

            return repartitionFinale;
        }
    }
}

//                    if (fusion.Count == 4)
//{
//    double totalLvl = fusion.Sum(p => p.LvlPrincipal);
//    double moyenne = totalLvl / 4.0;

//    if (Math.Abs((50 - moyenne) * (50 - moyenne)) <= 400) //          (moyenne - 50)^2