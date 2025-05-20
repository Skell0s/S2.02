using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.Realisations;

namespace TeamsMaker_METIER.Algorithmes
{
    /// <summary>
    /// Liste des noms d'algorithmes
    /// </summary>
    public enum NomAlgorithme
    {
        ALGOTEST,
        GLOUTON_CROISSANT,
        LEVEL_BALANCING,
        ALGOROLEPRINCIPALE,
        ExtremePrem,
        AlgoJulesPRBLÉME2,
        AlgoPRogressif,
        n_opt,
        AlgoExtremeEnPremier_niv2,
        NSWAP,
        AlgoTestPbNiv2
    }


    public static class NomAlgorithmeExt
    {
        /// <summary>
        /// Affichage du nom de l'algorithme
        /// </summary>
        /// <param name="algo">NomAlgorithme</param>
        /// <returns>La chaine de caractères à afficher</returns>
        public static string Affichage(this NomAlgorithme algo)
        {
            string res = "Algorithme non nommé :(";
            switch(algo)
            {
                case NomAlgorithme.ALGOTEST: res = "Algorithme de test (à supprimer)----TEST----"; break;
                case NomAlgorithme.GLOUTON_CROISSANT: res = "Glouton croissant----TEST----"; break;
                case NomAlgorithme.LEVEL_BALANCING: res = "Level  balancing----TEST----"; break;
                case NomAlgorithme.ALGOROLEPRINCIPALE: res = "Algorithme de répartition par rôle principal----TEST----"; break;
                case NomAlgorithme.ExtremePrem: res = "Extremes en premier"; break;
                case NomAlgorithme.AlgoExtremeEnPremier_niv2: res = "Extremes en premier level 2"; break;
                case NomAlgorithme.AlgoJulesPRBLÉME2: res = "ProblèmeJules----TEST----"; break;
                case NomAlgorithme.AlgoPRogressif: res = "Algorithme progressif"; break;
                case NomAlgorithme.n_opt: res = "Algorithme n-opt"; break;
                case NomAlgorithme.NSWAP: res = "Algorithme n-swap"; break;
                case NomAlgorithme.AlgoTestPbNiv2: res = "Algorithme de test niveau 2----------------TEST--------------"; break;
            }
            

            return res;
        }
    }
}
