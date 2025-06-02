using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.AlgoTest
{
    internal class extremepremierniv3 : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Personnage[] personnages = jeuTest.Personnages;

            Repartition repartition = new Repartition(jeuTest);

            // ----------------------------------------------------------------------------------------étape 1 liste de tous les personnages doublon des perso avec role segondaire
            List<Personnage> avecRoleSecondaire = new List<Personnage>();
            List<Personnage> sansRoleSecondaire = new List<Personnage>();
            List<Personnage> doublonroleSecondaire = new List<Personnage>();

            foreach (var p in personnages)
            {
                if (p.RoleSecondaire == Role.TANK || p.RoleSecondaire == Role.SUPPORT || p.RoleSecondaire == Role.DPS)
                {
                    avecRoleSecondaire.Add(p);   //ajoute personnage a la liste avecRoleSecondaire
                    doublonroleSecondaire.Add(p);
                }
                else
                    sansRoleSecondaire.Add(p);
            }


            List<Personnage> tanks = new List<Personnage>();
            List<Personnage> supports = new List<Personnage>();
            List<Personnage> dps = new List<Personnage>();

            //on tri par role en crée avec les doublon
            foreach (var p in sansRoleSecondaire)
            {
                switch (p.RolePrincipal)
                {
                    case Role.TANK: tanks.Add(p); break;
                    case Role.SUPPORT: supports.Add(p); break;
                    case Role.DPS: dps.Add(p); break;
                }
            }
            foreach (var p in avecRoleSecondaire)
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
            //---------------------------------------------------------------------------------------------------étape 2 tri création des equipes
            tanks.Sort(new ComparateurPersonnageParNiveauPrincipal());
            supports.Sort(new ComparateurPersonnageParNiveauPrincipal());
            dps.Sort(new ComparateurPersonnageParNiveauPrincipal());

            // on fais un trie par role et on les mets dans la liste

            int t = 0, s = 0, d = dps.Count - 1;

            int total = tanks.Count + supports.Count + dps.Count;

            while (t < tanks.Count && s < supports.Count && d - 1 >= 0)
            {
                Equipe equipe = new Equipe();
                equipe.AjouterMembre(tanks[t++]);
                equipe.AjouterMembre(supports[s++]);
                equipe.AjouterMembre(dps[d--]);
                equipe.AjouterMembre(dps[d--]);

                repartition.AjouterEquipe(equipe);
            }

            //--------------------------------------------------étape 3 si le score de l'équipe est meilleur avecc le role principal supprimé le doublon
            if (tanks.Count > 0)//
            { }

            // -------------------- Étape 4 : Nettoyage des équipes incomplètes + n-opt fusion -----------------------



            // Return final : sans doublons, équipes complètes, après nettoyage
            return repartition;


        }
    }
}
 