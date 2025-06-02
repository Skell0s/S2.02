using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class AlgoExtremeEnPremier : Algorithme
    {
        /// <summary>
        /// Algorithme de répartition des personnages en équipes de 4, en comparant les niveaux principaux sans prendre en compte les rôles.
        /// </summary>
        /// <param name="jeuTest"></param>
        /// <returns></returns>
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Personnage[] personnages = jeuTest.Personnages;
            Array.Sort(personnages, new ComparateurPersonnageParNiveauPrincipal());
            Repartition repartition = new Repartition(jeuTest);

            int a = 0;
            int z = personnages.Length - 1;

            while (z - a + 1 >= 4) 
            {
                Equipe equipe = new Equipe();
                equipe.AjouterMembre(personnages[a++]);
                equipe.AjouterMembre(personnages[a++]);
                equipe.AjouterMembre(personnages[z--]);
                equipe.AjouterMembre(personnages[z--]);

                repartition.AjouterEquipe(equipe);
            }
            if (z - a + 1 > 0)
            {
                Equipe equipeRestante = new Equipe();
                for (int i = a; i <= z; i++)
                    equipeRestante.AjouterMembre(personnages[i]);
            }
            stopwatch.Stop();
            TempsExecution = stopwatch.ElapsedMilliseconds;
            return repartition;
        }
    }
}