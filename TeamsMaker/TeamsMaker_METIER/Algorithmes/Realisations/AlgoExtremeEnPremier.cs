using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    internal class AlgoExtremeEnPremier : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Personnage[] personnages = jeuTest.Personnages;
            Array.Sort(personnages, new ComparateurPersonnageParNiveauPrincipal()); 
            Repartition repartition = new Repartition(jeuTest);
            int z = personnages.ToList<Personnage>().Count - 1;
            int a = 0;
            
            for (int i = 0; i <= personnages.Length - 3; i += 4) //ajout de joueur jusqu'a qu'il reste moins de 4 joueur
            {
                Equipe equipe = new Equipe();
                for (int j = i; j < i + 2; j+=1)
                {
                    equipe.AjouterMembre(personnages[a]);
                    equipe.AjouterMembre(personnages[z]);
                    z -= 1;
                    a += 1;
                }
                repartition.AjouterEquipe(equipe);
            }
            return repartition;
        }
    }

}