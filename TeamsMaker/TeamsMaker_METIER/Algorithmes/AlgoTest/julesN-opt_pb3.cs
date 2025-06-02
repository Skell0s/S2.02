using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.AlgoTest
{
    public class julesN_opt_pb3 : Algorithme
    {

        public override Repartition Repartir(JeuTest jeuTest)
        {
            Personnage[] personnages = jeuTest.Personnages;

            Repartition repartition = new Repartition(jeuTest);

            // ----------------------------------------------------------------------------------------étape 1 liste de tous les personnages doublon des perso avec role segondaire
            List<Personnage> avecRoleSecondaire = new List<Personnage>();
            List<Personnage> sansRoleSecondaire = new List<Personnage>();
            List<Personnage> doublonroleSecondaire = new List<Personnage>();

            foreach (Personnage p in personnages)
            {
                if (p.RoleSecondaire == Role.TANK || p.RoleSecondaire == Role.SUPPORT || p.RoleSecondaire == Role.DPS)
                {
                    avecRoleSecondaire.Add(p);
                    doublonroleSecondaire.Add(p);
                }
                else
                {
                    sansRoleSecondaire.Add(p);
                }
            }

            List<Personnage> tanks = new List<Personnage>();
            List<Personnage> supports = new List<Personnage>();
            List<Personnage> dps = new List<Personnage>();

            foreach (Personnage p in sansRoleSecondaire)
            {
                switch (p.RolePrincipal)
                {
                    case Role.TANK: tanks.Add(p); break;
                    case Role.SUPPORT: supports.Add(p); break;
                    case Role.DPS: dps.Add(p); break;
                }
            }
            foreach (Personnage p in avecRoleSecondaire)
            {
                switch (p.RolePrincipal)
                {
                    case Role.TANK: tanks.Add(p); break;
                    case Role.SUPPORT: supports.Add(p); break;
                    case Role.DPS: dps.Add(p); break;
                }
            }
            foreach (Personnage p in doublonroleSecondaire)
            {
                switch (p.RoleSecondaire)
                {
                    case Role.TANK: tanks.Add(p); break;
                    case Role.SUPPORT: supports.Add(p); break;
                    case Role.DPS: dps.Add(p); break;
                }
            }

            //---------------------------------------------------------------------------------------------------étape 2 tri création des equipes
            tanks.Sort(new ComparateurPersonnageParNiveauPrincipal());
            supports.Sort(new ComparateurPersonnageParNiveauPrincipal());
            dps.Sort(new ComparateurPersonnageParNiveauPrincipal());

            int t = 0, s = 0, d = dps.Count - 1;

            while (t < tanks.Count && s < supports.Count && d - 1 >= 0)
            {
                Equipe equipe = new Equipe();
                equipe.AjouterMembre(tanks[t++]);
                equipe.AjouterMembre(supports[s++]);
                equipe.AjouterMembre(dps[d--]);
                equipe.AjouterMembre(dps[d--]);

                repartition.AjouterEquipe(equipe);
            }

            //--------------------------------------------------étape 3 : comparer doublons avec version sans doublon
            foreach (Equipe equipe in repartition.Equipes)
            {
                foreach (Personnage membre in equipe.Membres)
                {
                    if (doublonroleSecondaire.Contains(membre))
                    {
                        Equipe equipeOriginale = new Equipe();
                        foreach (Personnage m in equipe.Membres)
                            equipeOriginale.AjouterMembre(m);

                        // Recréer équipe sans doublon (le personnage en rôle secondaire n'est plus compté)
                        Equipe equipeSansDoublon = new Equipe();
                        foreach (Personnage m in equipe.Membres)
                            equipeSansDoublon.AjouterMembre(m); // même liste pour comparaison

                        double scoreOriginal = equipeOriginale.Score(Probleme.ROLESECONDAIRE);
                        double scoreSansDoublon = equipeSansDoublon.Score(Probleme.ROLESECONDAIRE);

                        if (scoreSansDoublon <= scoreOriginal)
                        {
                            doublonroleSecondaire.Remove(membre); // le rôle secondaire n'est pas utile
                        }
                    }
                }
            }

            // -------------------- Étape 4 : Nettoyage des équipes incomplètes + tentative de recomposition -----------------------
            List<Equipe> equipesValides = new List<Equipe>();
            List<Personnage> personnagesUtilises = new List<Personnage>();

            foreach (Equipe equipe in repartition.Equipes)
            {
                if (equipe.EstValide(Probleme.ROLESECONDAIRE))
                {
                    equipesValides.Add(equipe);
                    personnagesUtilises.AddRange(equipe.Membres);
                }
            }

            // Récupère les personnages non utilisés
            List<Personnage> restants = new List<Personnage>();
            foreach (Personnage p in personnages)
            {
                if (!personnagesUtilises.Contains(p))
                    restants.Add(p);
            }

            // Essayer de former une dernière équipe valide avec les restants
            List<Personnage> restTanks = restants.Where(p => p.RolePrincipal == Role.TANK).ToList();
            List<Personnage> restSupports = restants.Where(p => p.RolePrincipal == Role.SUPPORT).ToList();
            List<Personnage> restDps = restants.Where(p => p.RolePrincipal == Role.DPS).ToList();

            if (restTanks.Count >= 1 && restSupports.Count >= 1 && restDps.Count >= 2)
            {
                Equipe nouvelleEquipe = new Equipe();
                nouvelleEquipe.AjouterMembre(restTanks[0]);
                nouvelleEquipe.AjouterMembre(restSupports[0]);
                nouvelleEquipe.AjouterMembre(restDps[0]);
                nouvelleEquipe.AjouterMembre(restDps[1]);

                if (nouvelleEquipe.EstValide(Probleme.ROLESECONDAIRE))
                    equipesValides.Add(nouvelleEquipe);
            }

            // On remplace les anciennes équipes par celles valides
            repartition = new Repartition(jeuTest);
            foreach (Equipe eq in equipesValides)
                repartition.AjouterEquipe(eq);

            return repartition;
        }
    }
}

