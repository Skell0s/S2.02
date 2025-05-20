using System.Collections.Generic;
using System.Data;
using System.Text;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.AlgoTest
{
    public class AlgoPb2 : Algorithme
    {

        public override Repartition Repartir(JeuTest jeuTest)
        {
            Personnage[] personnages = jeuTest.Personnages;
            Array.Sort(personnages, new ComparateurPersonnageParNiveauPrincipal());
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

            HashSet<Personnage> dejaUtilises = new HashSet<Personnage>();
            bool formationPossible = true;

            // Une équipe est valide si elle est composée de 4 personnages dont un tank, un support et 2 dps(en rôle principal)
            while (tanks.Count > 0 && supports.Count > 0 && dps.Count > 1)
            {
                Equipe meilleureEquipe = null;
                int meilleurEcart = int.MaxValue;

                int selectedTank = -1, selectedSupport = -1;
                int selectedDps1 = -1, selectedDps2 = -1;

                // On ne prend que les X meilleurs tanks/supports pour optimiser
                int maxTanks = Math.Min(5, tanks.Count);
                int maxSupports = Math.Min(5, supports.Count);

                for (int i = 0; i < maxTanks; i++)
                {
                    for (int j = 0; j < maxSupports; j++)
                    {
                        int niveauTS = tanks[i].LvlPrincipal + supports[j].LvlPrincipal;
                        int objectifDps = 200 - niveauTS;

                        // Chercher 2 DPS dont la somme est proche de objectifDps
                        int bestPairSum = -1;
                        int d1 = -1, d2 = -1;

                        for (int m = 0; m < dps.Count - 1; m++)
                        {
                            for (int n = m + 1; n < dps.Count; n++)
                            {
                                int sum = dps[m].LvlPrincipal + dps[n].LvlPrincipal;
                                int ecart = Math.Abs(sum - objectifDps);

                                if (ecart < Math.Abs(bestPairSum - objectifDps) || bestPairSum == -1)
                                {
                                    bestPairSum = sum;
                                    d1 = m;
                                    d2 = n;

                                    if (ecart == 0) break;
                                }
                            }
                            if (bestPairSum == objectifDps) break;
                        }

                        if (d1 != -1 && d2 != -1)
                        {
                            int total = niveauTS + bestPairSum;
                            int ecartTotal = Math.Abs(200 - total);

                            if (ecartTotal < meilleurEcart)
                            {
                                meilleurEcart = ecartTotal;
                                selectedTank = i;
                                selectedSupport = j;
                                selectedDps1 = d1;
                                selectedDps2 = d2;

                                if (ecartTotal == 0) break;
                            }
                        }
                    }
                    if (meilleurEcart == 0) break;
                }

                if (selectedTank == -1) break;

                // Créer et ajouter l’équipe
                Equipe equipe = new Equipe();
                equipe.AjouterMembre(tanks[selectedTank]);
                equipe.AjouterMembre(supports[selectedSupport]);
                equipe.AjouterMembre(dps[selectedDps1]);
                equipe.AjouterMembre(dps[selectedDps2]);

                repartition.AjouterEquipe(equipe);

                // Supprimer les membres utilisés (en commençant par les plus grands index)
                tanks.RemoveAt(selectedTank);
                supports.RemoveAt(selectedSupport);

                if (selectedDps1 > selectedDps2)
                {
                    dps.RemoveAt(selectedDps1);
                    dps.RemoveAt(selectedDps2);
                }
                else
                {
                    dps.RemoveAt(selectedDps2);
                    dps.RemoveAt(selectedDps1);
                }
            }
            return repartition;
        }
    }
}
