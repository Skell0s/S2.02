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
            Repartition repartition = new Repartition(jeuTest);
            repartition = AlgoPb3(repartition, jeuTest);

            return repartition;
        }

        private Repartition AlgoPb3(Repartition r, JeuTest jeuTest) 
        {
            List<Personnage> tanks = new List<Personnage>();
            List<Personnage> supports = new List<Personnage>();
            List<Personnage> dps = new List<Personnage>();

            foreach (var p in jeuTest.Personnages)
            {
                switch (p.RolePrincipal)
                {
                    case Role.TANK: tanks.Add(p); break;
                    case Role.SUPPORT: supports.Add(p); break;
                    case Role.DPS: dps.Add(p); break;
                }
                if (p.RoleSecondaire != Role.AUCUN)
                {
                    switch (p.RoleSecondaire)
                    {
                        case Role.TANK: tanks.Add(p); break;
                        case Role.SUPPORT: supports.Add(p); break;
                        case Role.DPS: dps.Add(p); break;
                    }
                }
            }

            
            return repartition;
        }
    }
}
