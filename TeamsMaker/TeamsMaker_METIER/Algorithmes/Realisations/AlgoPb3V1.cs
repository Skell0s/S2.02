using System.Data;
using System.Text;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class AlgoPb3V1 : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Personnage[] personnages = jeuTest.Personnages;
            Array.Sort(personnages, new ComparateurPersonnageParNiveauPrincipal());
            Repartition repartition = new Repartition(jeuTest);

            HashSet<Personnage> dejaUtilises = new HashSet<Personnage>();

            bool formationPossible = true;
            while (formationPossible)
            {
                for (int i = 0; i < personnages.Length; i++)
                {
                    if (dejaUtilises.Contains(personnages[i])) continue;

                    Equipe equipe = new Equipe();
                    int nbtank = 0, nbsupport = 0, nbdps = 0;
                    for (int j = i; j < personnages.Length && equipe.Membres.Length < 4; j++)
                    {
                        bool ajoute = false;

                        switch (personnages[j].RolePrincipal)
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
                            default:
                                switch (personnages[j].RoleSecondaire)
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
                                break;
                        }

                        if (ajoute)
                        {
                            equipe.AjouterMembre(personnages[j]);
                        }

                    }
                    if (equipe.EstValide(Probleme.ROLESECONDAIRE))
                    {
                        repartition.AjouterEquipe(equipe);
                        foreach (var p in equipe.Membres)
                        {
                            dejaUtilises.Add(p);
                        }
                    }
                    else
                    {
                        formationPossible = false;
                    }
                }
            }
            return repartition;
        }
    }
}
