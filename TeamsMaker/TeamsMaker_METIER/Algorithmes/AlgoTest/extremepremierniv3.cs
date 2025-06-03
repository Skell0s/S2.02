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
    internal class extremepremierniv3 : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Personnage[] personnages = jeuTest.Personnages;

            Repartition repartition = new Repartition(jeuTest);

            // Étape 1 : séparation des personnages
            List<Personnage> avecRoleSecondaire = new List<Personnage>();
            List<Personnage> sansRoleSecondaire = new List<Personnage>();
            List<Personnage> doublonroleSecondaire = new List<Personnage>();

            foreach (var p in personnages)
            {
                if (p.RoleSecondaire == Role.TANK || p.RoleSecondaire == Role.SUPPORT || p.RoleSecondaire == Role.DPS)
                {
                    avecRoleSecondaire.Add(p);
                    doublonroleSecondaire.Add(p);
                }
                else
                    sansRoleSecondaire.Add(p);
            }

            List<Personnage> tanks = new List<Personnage>();
            List<Personnage> supports = new List<Personnage>();
            List<Personnage> dps = new List<Personnage>();

            // Tri des rôles
            foreach (var p in sansRoleSecondaire.Concat(avecRoleSecondaire))
            {
                switch (p.RolePrincipal)
                {
                    case Role.TANK: tanks.Add(p); break;
                    case Role.SUPPORT: supports.Add(p); break;
                    case Role.DPS: dps.Add(p); break;
                }
            }

            foreach (var p in doublonroleSecondaire)
            {
                switch (p.RoleSecondaire)
                {
                    case Role.TANK: tanks.Add(p); break;
                    case Role.SUPPORT: supports.Add(p); break;
                    case Role.DPS: dps.Add(p); break;
                }
            }

            // Étape 2 : tri des personnages par niveau
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

            // Étape 3 : suppression des doublons inutiles
            foreach (var equipe in repartition.Equipes)
            {
                foreach (var membre in equipe.Membres.ToList()) // important : ToList() pour éviter modification pendant l’itération
                {
                    if (doublonroleSecondaire.Contains(membre))
                    {
                        Equipe equipeOriginale = new Equipe();
                        foreach (var m in equipe.Membres)
                            equipeOriginale.AjouterMembre(m);

                        // Créer une copie sans le membre doublon
                        Equipe equipeSansDoublon = new Equipe();
                        foreach (var m in equipe.Membres)
                        {
                            if (m != membre)
                                equipeSansDoublon.AjouterMembre(m);
                        }

                        // Comparer les scores uniquement si l’équipe est complète
                        if (equipeSansDoublon.Membres.Length == 4)
                        {
                            double scoreOriginal = equipeOriginale.Score(Probleme.ROLEPRINCIPAL);
                            double scoreSansDoublon = equipeSansDoublon.Score(Probleme.ROLESECONDAIRE);

                            if (scoreSansDoublon <= scoreOriginal)
                            {
                                doublonroleSecondaire.Remove(membre); // doublon inutile
                            }
                        }
                    }
                }
            }

            // Étape 4 : nettoyage et recomposition d’équipe
            List<Equipe> equipesValides = new List<Equipe>();
            List<Personnage> personnagesUtilises = new List<Personnage>();

            foreach (var equipe in repartition.Equipes)
            {
                if (equipe.EstValide(Probleme.ROLEPRINCIPAL) || equipe.EstValide(Probleme.ROLESECONDAIRE))
                {
                    equipesValides.Add(equipe);
                    personnagesUtilises.AddRange(equipe.Membres);
                }
            }

            List<Personnage> restants = personnages.Except(personnagesUtilises).ToList();

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

                if (nouvelleEquipe.EstValide(Probleme.ROLEPRINCIPAL) || nouvelleEquipe.EstValide(Probleme.ROLESECONDAIRE))
                    equipesValides.Add(nouvelleEquipe);
            }

            repartition = new Repartition(jeuTest);
            foreach (var eq in equipesValides)
                repartition.AjouterEquipe(eq);

            return repartition;
        }
    }
}
