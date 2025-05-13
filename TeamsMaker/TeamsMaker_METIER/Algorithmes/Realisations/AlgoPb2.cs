using System.Data;
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
            HashSet<Personnage> dejaUtilises = new HashSet<Personnage>();

            int z = personnages.ToList<Personnage>().Count - 1;
            int a = 0;
            bool formationPossible = true;

            while (z > 0)
            {
                for (int i = 0; i <= personnages.Length - 3; i += 4) //ajout de joueur jusqu'a qu'il reste moins de 4 joueur
                {
                    Equipe equipe = new Equipe();
                    for (int j = i; j < i + 2; j += 1)
                    {
                        equipe.AjouterMembre(personnages[a]);
                        equipe.AjouterMembre(personnages[z]);
                        z -= 1;
                        a += 1;
                    }
                    // Vérifie que l’équipe est complète (4 membres avec bonne répartition)
                    if (equipe.EstValide(Probleme.ROLEPRINCIPAL))
                    {
                        repartition.AjouterEquipe(equipe);
                        foreach (var p in equipe.Membres)
                        {
                            dejaUtilises.Add(p);
                        }
                    }
                    else
                    {
                        // Impossible de créer une nouvelle équipe valide
                        formationPossible = false;
                    }
                }
                return repartition;
            }
            
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
                        Role role = personnages[j].RolePrincipal;
                        bool ajoute = false;

                        switch (role)
                        {
                            case Role.TANK when nbtank < 1:
                                nbtank++;
                                ajoute = true;
                                break;
                            case Role.SUPPORT when nbsupport < 1:
                                nbsupport++;
                                ajoute = true;
                                break;
                            case Role.DPS when nbdps < 2:
                                nbdps++;
                                ajoute = true;
                                break;
                        }

                        if (ajoute)
                        {
                            equipe.AjouterMembre(personnages[j]);
                        }

                    }
                }
            }
            return repartition;
        }
    }
}
