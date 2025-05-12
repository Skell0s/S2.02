using System.Text;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class AlgoPb2 : Algorithme
    {

        public override Repartition Repartir(JeuTest jeuTest)
        {
            Personnage[] personnages = jeuTest.Personnages;
            Array.Sort(personnages, new ComparateurPersonnageParNiveauPrincipal());
            Repartition repartition = new Repartition(jeuTest);

            // Pour suivre les personnages déjà utilisés
            HashSet<Personnage> dejaUtilises = new HashSet<Personnage>();

            bool formationPossible = true;
            // Une équipe est valide si elle est composée de 4 personnages dont un tank, un support et 2 dps(en rôle principal)
            while (formationPossible) 
            { 
                for (int i = 0; i < personnages.Length; i++)
                {
                    if (dejaUtilises.Contains(personnages[i])) continue;

                    Equipe equipe = new Equipe();
                    int nbtank = 0, nbsupport = 0, nbdps = 0;
                    for (int j = i; j < personnages.Length && equipe.Membres.Length < 4; j++)
                    {
                        if (dejaUtilises.Contains(personnages[j])) continue;

                        if (personnages[j].RolePrincipal == Role.TANK && nbtank < 1)
                        {
                            equipe.AjouterMembre(personnages[j]);
                            dejaUtilises.Add(personnages[j]);
                            nbtank += 1;
                        }
                        else if (personnages[j].RolePrincipal == Role.SUPPORT && nbsupport < 1)
                        {
                            equipe.AjouterMembre(personnages[j]);
                            dejaUtilises.Add(personnages[j]);
                            nbsupport += 1;
                        }
                        else if (personnages[j].RolePrincipal == Role.DPS && nbdps < 2)
                        {
                            equipe.AjouterMembre(personnages[j]);
                            dejaUtilises.Add(personnages[j]);
                            nbdps += 1;
                        }

                    }
                    // Vérifie que l’équipe est complète (4 membres avec bonne répartition)
                    if (equipe.EstValide(Probleme.ROLEPRINCIPAL))
                    {
                        repartition.AjouterEquipe(equipe);
                        foreach (var p in equipe.Membres)
                            personnages.ToList<Personnage>().Remove(p);
                    }
                    else
                    {
                        // Si l’équipe est incomplète ou invalide, on remet les persos utilisés dans cette tentative
                        foreach (Personnage p in equipe.Membres)
                        {
                            dejaUtilises.Remove(p);
                        }
                        // On sort de la boucle
                        if (personnages.Length < 4)
                        {
                            formationPossible = false;
                            
                        }
                    }
                }
            }
            return repartition;
        }
    }
}
