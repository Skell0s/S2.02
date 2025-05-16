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
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Repartition repartition = new Repartition(jeuTest);
            Personnage[] personnages = jeuTest.Personnages;
            Array.Sort(personnages, new ComparateurPersonnageParNiveauPrincipal());

            Repartition repartition2 = new Repartition(jeuTest);
            int a = 0;
            int z = personnages.Length - 1;
            int b = a;
            int y = z;

            // Création d'équipes de 2 : du plus faible au plus fort
            while (a < z)

            {
                Equipe equipeDe2 = new Equipe();
                equipeDe2.AjouterMembre(personnages[a]);
                equipeDe2.AjouterMembre(personnages[z]);
                repartition2.AjouterEquipe(equipeDe2);
                z -= 1;
                a += 1;
            }
            Personnage personnageRestant = null;
            if (a == z)
                personnageRestant = personnages[a];
            /////////////////////////////////////////////////////////

            Equipe[] tableauequipe2 = repartition2.Equipes;
            int nbEquipes = tableauequipe2.Length;

            Repartition repartitionfinal = new Repartition(jeuTest);
            HashSet<int> equipesUtilisees = new HashSet<int>();

            //Personnage personnage = repartition.Equipes[5].Membres[1];


            for (int i = 0; i < nbEquipes; i++) 
            {
                if (equipesUtilisees.Contains(i)) continue;

                int Max = 1;
                int cible = 0;

                for (int j = i + 1; j < nbEquipes; j++)
                {
                    if (equipesUtilisees.Contains(j)) continue;

                    List<Personnage> fusionj = new List<Personnage>();
                    fusionj.AddRange(tableauequipe2[i].Membres);
                    fusionj.AddRange(tableauequipe2[Max].Membres);

                    List<Personnage> fusionI = new List<Personnage>();
                    fusionI.AddRange(tableauequipe2[i].Membres);
                    fusionI.AddRange(tableauequipe2[j].Membres);

                    if (fusionI.Count != 4) continue;

                    int sommei = fusionI.Sum(p => p.LvlPrincipal);
                    double diffi = Math.Abs(200 - sommei);

                    int sommej = fusionj.Sum(p => p.LvlPrincipal);
                    double diffj = Math.Abs(200 - sommej);

                    if (Math.Abs(diffj - cible) < Math.Abs(diffi - cible))
                    {

                        Max = j;
                    }
                }

                if (Max != -1 && !equipesUtilisees.Contains(Max))

                    {
                        Equipe meilleureEquipe = new Equipe();
                    foreach (var membre in tableauequipe2[i].Membres)
                        meilleureEquipe.AjouterMembre(membre);
                    foreach (var membre in tableauequipe2[Max].Membres)
                        meilleureEquipe.AjouterMembre(membre);
                    
                    if (meilleureEquipe.EstValide(Probleme.SIMPLE))
                    {
                        repartitionfinal.AjouterEquipe(meilleureEquipe);
                        equipesUtilisees.Add(i);
                        equipesUtilisees.Add(Max);
                    }
                }

            }

            return repartitionfinal;
        }
    }
}
