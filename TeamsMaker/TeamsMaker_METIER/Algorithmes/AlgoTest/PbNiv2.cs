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
            Repartition repartition = new Repartition(jeuTest);
            Personnage[] personnages = jeuTest.Personnages;
            Array.Sort(personnages, new ComparateurPersonnageParNiveauPrincipal());

            int a = 0;
            int z = personnages.Length - 1;

            // Création d'équipes de 2 : du plus faible au plus fort
            while (a >= z && (personnages.ToList().Count > 1 || personnages.ToList().Count == 0))
            {
              
                Equipe equipeDe2 = new Equipe();
                if (personnages[a].RolePrincipal == Role.TANK && personnages[z].RolePrincipal == Role.DPS)
                {
                    equipeDe2.AjouterMembre(personnages[a]);
                    equipeDe2.AjouterMembre(personnages[z]);
                }
                else if (personnages[a].RolePrincipal == Role.DPS && personnages[z].RolePrincipal == Role.SUPPORT)
                {
                    equipeDe2.AjouterMembre(personnages[z]);
                    equipeDe2.AjouterMembre(personnages[a]);
                }
            }

            // Garder le personnage restant s'il y a un nombre impair
            Personnage personnageRestant = (a == z) ? personnages[a] : null;

            Equipe[] tableauequipe2 = repartition.Equipes;
            int nbEquipes = tableauequipe2.Length;

            Repartition repartitionFinale = new Repartition(jeuTest);
            HashSet<int> equipesUtilisees = new HashSet<int>();

            for (int i = 0; i < nbEquipes; i++)
            {
                if (equipesUtilisees.Contains(i)) continue;

                int meilleurIndex = -1;
                double meilleureDiff = double.MaxValue;

                for (int j = i + 1; j < nbEquipes; j++)
                {
                    if (equipesUtilisees.Contains(j)) continue;

                    var fusion = tableauequipe2[i].Membres.Concat(tableauequipe2[j].Membres).ToList();
                    if (fusion.Count != 4) continue;

                    double diff = Math.Abs(200 - fusion.Sum(p => p.LvlPrincipal));
                    if (diff < meilleureDiff)
                    {
                        meilleureDiff = diff;
                        meilleurIndex = j;
                    }
                }

                while(tableauequipe2)
                if (meilleurIndex != -1)
                {
                    var equipeFusion = new Equipe();
                    foreach (var membre in tableauequipe2[i].Membres)
                        equipeFusion.AjouterMembre(membre);
                    foreach (var membre in tableauequipe2[meilleurIndex].Membres)
                        equipeFusion.AjouterMembre(membre);
                
                    if (equipeFusion.EstValide(Probleme.ROLEPRINCIPAL))
                    {
                        repartitionFinale.AjouterEquipe(equipeFusion);
                        equipesUtilisees.Add(i);
                        equipesUtilisees.Add(meilleurIndex);
                    }
                }
            }
            return repartitionFinale;
        }
    }
}