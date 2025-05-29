using System;
using System.Collections.Generic;
using System.Linq;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.AlgoTest
{
    public class AlgoPb3V1 : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Personnage[] personnages = jeuTest.Personnages;
            Array.Sort(personnages, new ComparateurPersonnageParNiveauPrincipal());

            Repartition repartition = new Repartition(jeuTest);

            // Séparation des personnages en deux groupes
            List<Personnage> avecRoleSecondaire = new List<Personnage>();
            List<Personnage> sansRoleSecondaire = new List<Personnage>();

            foreach (var p in personnages)
            {
                if (p.RoleSecondaire == Role.TANK || p.RoleSecondaire == Role.SUPPORT || p.RoleSecondaire == Role.DPS)
                    avecRoleSecondaire.Add(p);
                else
                    sansRoleSecondaire.Add(p);
            }

            // ---- Phase 1 : crée des liste de de tank dps et de support ----
            List<Personnage> tanks = new List<Personnage>();
            List<Personnage> supports = new List<Personnage>();
            List<Personnage> dps = new List<Personnage>();

            foreach (var p in sansRoleSecondaire)
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

            //

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

            // Ajouter les restants de la 1ère phase
            List<Personnage> restants = new List<Personnage>();
            while (t < tanks.Count) restants.Add(tanks[t++]);
            while (s < supports.Count) restants.Add(supports[s++]);
            while (d >= 0) restants.Add(dps[d--]);

            if (restants.Count > 0)
            {
                Equipe equipeRestante = new Equipe();
                foreach (var p in restants)
                    equipeRestante.AjouterMembre(p);
                repartition.AjouterEquipe(equipeRestante);
            }

            // ---- Phase 2 : équipe avec ceux qui n'ont PAS de rôle secondaire ----
            if (avecRoleSecondaire.Count > 0)
            {
                for (int i = 0; i < avecRoleSecondaire.Count; i += 4)
                {
                    Equipe equipe = new Equipe();
                    for (int j = i; j < i + 4 && j < avecRoleSecondaire.Count; j++)
                    {
                        equipe.AjouterMembre(avecRoleSecondaire[j]);
                    }
                    //repartition.AjouterEquipe(equipe);
                }
            }

            return repartition;
        }
    }
}
