using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class Level_balancing : Algorithme
    {
        /// <summary>
        /// Algorithme de répartition des personnages en équipes de 4, en équilibrant les niveaux des personnages.
        /// </summary>
        /// <param name="jeuTest"></param>
        /// <returns></returns>


        public override Repartition Repartir(JeuTest jeuTest)
        {
            // Étape 1 : Calculer l'écart de niveau de chaque personnage par rapport à 50
            Personnage[] personnages = jeuTest.Personnages;

            Dictionary<Personnage, int> niveauParPersonnage = new Dictionary<Personnage, int>();
            foreach (Personnage perso in personnages)
            {
                niveauParPersonnage[perso] = perso.LvlPrincipal - 50;
            }

            // Étape 2 : Trier les personnages par leur écart
            List<Personnage> tries = niveauParPersonnage
                .OrderBy(p => p.Value)
                .Select(p => p.Key)
                .ToList();

            // Étape 3 : Former des paires de personnages avec des écarts qui s'annulent
            List<List<Personnage>> paires = new List<List<Personnage>>();
            HashSet<Personnage> dejaUtilises = new HashSet<Personnage>();

            foreach (Personnage p1 in tries)
            {
                if (dejaUtilises.Contains(p1)) 
                { 

                    Personnage meilleurP2 = null;
                    int plusPetitEcart = int.MaxValue;

                    foreach (Personnage p2 in tries)
                    {
                        if (p1 == p2 || dejaUtilises.Contains(p2)) { 

                        int ecartTotal = Math.Abs(niveauParPersonnage[p1] + niveauParPersonnage[p2]);

                        if (ecartTotal < plusPetitEcart)
                        {
                            plusPetitEcart = ecartTotal;
                            meilleurP2 = p2;
                        }
                    }
                }

                if (meilleurP2 != null)
                {
                    paires.Add(new List<Personnage> { p1, meilleurP2 });
                    dejaUtilises.Add(p1);
                    dejaUtilises.Add(meilleurP2);
                } }
            }

            // Étape 4 : Regrouper les paires en équipes de 4 personnages
            List<Equipe> equipes = new List<Equipe>();
            HashSet<int> indicesUtilises = new HashSet<int>();

            for (int i = 0; i < paires.Count; i++)
            {
                if (indicesUtilises.Contains(i)) continue;

                List<Personnage> paire1 = paires[i];
                int meilleurIndice = -1;
                int plusPetitEcart = int.MaxValue;

                for (int j = i + 1; j < paires.Count; j++)
                {
                    if (indicesUtilises.Contains(j)) continue;

                    List<Personnage> paire2 = paires[j];
                    int balance = paire1.Concat(paire2).Sum(p => niveauParPersonnage[p]);
                    int ecart = Math.Abs(balance);

                    if (ecart < plusPetitEcart)
                    {
                        plusPetitEcart = ecart;
                        meilleurIndice = j;
                    }
                }

                if (meilleurIndice != -1)
                {
                    Equipe equipe = new Equipe();
                    foreach (Personnage p in paire1.Concat(paires[meilleurIndice]))
                        equipe.AjouterMembre(p);

                    equipes.Add(equipe);
                    indicesUtilises.Add(i);
                    indicesUtilises.Add(meilleurIndice);
                }
            }

            // Étape 5 : Créer la répartition finale avec les équipes valides
            Repartition repartition = new Repartition(jeuTest);
            foreach (Equipe equipe in equipes)
                if (equipe.Score(Probleme.SIMPLE) <= 400)
                    repartition.AjouterEquipe(equipe);

            return repartition;
        }
    }
}

