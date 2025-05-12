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
    public class AlgorithmesGloutonCroissant : Algorithme
    {
        /// <summary>
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Personnage[] personnages =jeuTest.Personnages;
            Array.Sort(personnages,new ComparateurPersonnageParNiveauPrincipal());
            Repartition repartition = new Repartition(jeuTest);
            for (int i = 0; i <= personnages.Length - 4; i+=4)
            {
                Equipe equipe = new Equipe();                
                for (int j = i; j < i + 4; j++)
                {
                    equipe.AjouterMembre(personnages[j]);
                }
                repartition.AjouterEquipe(equipe);              
            }
            return repartition;
        }
    }

}
