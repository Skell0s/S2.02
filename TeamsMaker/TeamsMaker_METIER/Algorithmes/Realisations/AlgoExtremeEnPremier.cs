using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    internal class AlgoExtremeEnPremier : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Personnage[] personnages = jeuTest.Personnages;
            Array.Sort(personnages, new ComparateurPersonnageParNiveauPrincipal());
            Repartition repartition = new Repartition(jeuTest);

            int a = 0;
            int z = personnages.Length - 1;

            while (z - a + 1 >= 4) // tant qu’il reste au moins 4 personnages
            {
                Equipe equipe = new Equipe();
                equipe.AjouterMembre(personnages[a++]);
                equipe.AjouterMembre(personnages[a++]);
                equipe.AjouterMembre(personnages[z--]);
                equipe.AjouterMembre(personnages[z--]);

                repartition.AjouterEquipe(equipe);
            }

            // Gestion des personnages restants (1 à 3)
            if (z - a + 1 > 0)
            {
                Equipe equipeRestante = new Equipe();
                for (int i = a; i <= z; i++)
                    equipeRestante.AjouterMembre(personnages[i]);
                //repartition.AjouterEquipe(equipeRestante);
            }
            stopwatch.Stop();
            TempsExecution = stopwatch.ElapsedMilliseconds;
            return repartition;
        }
    }
}