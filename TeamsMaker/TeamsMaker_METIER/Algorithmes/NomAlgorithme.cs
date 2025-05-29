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
        GLOUTON_CROISSANT,
        LEVEL_BALANCING,
        ALGOROLEPRINCIPALE,
        ExtremePrem,
        AlgoJulesPRBLÉME2,
        AlgoPRogressif,
        n_opt,
        AlgoExtremeEnPremier_niv2,
        NSWAP,
        algoniv3V1,
        AlgoJulesPB3,
        n_opt_level_2
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
                case NomAlgorithme.GLOUTON_CROISSANT: res = "Glouton croissant----TEST----"; break;
                case NomAlgorithme.LEVEL_BALANCING: res = "Level  balancing----TEST----"; break;
                case NomAlgorithme.ALGOROLEPRINCIPALE: res = "Algorithme de répartition par rôle principal----TEST----"; break;
                case NomAlgorithme.AlgoJulesPRBLÉME2: res = "Glouton niv2 Jules----TEST----"; break;

                case NomAlgorithme.n_opt: res = "Algorithme n-opt"; break;
                case NomAlgorithme.n_opt_level_2: res = "Algorithme n_opt_level_2 Jules ----Pas fini----"; break;
                case NomAlgorithme.AlgoJulesPB3: res = "Algorithme n_opt_level_3 Jules ----Pas fini----"; break;

                case NomAlgorithme.ExtremePrem: res = "Extremes en premier"; break;
                case NomAlgorithme.AlgoExtremeEnPremier_niv2: res = "Extremes en premier level 2"; break;

                case NomAlgorithme.AlgoPRogressif: res = "Algorithme progressif"; break;

                case NomAlgorithme.NSWAP: res = "Algorithme n-swap"; break;

                case NomAlgorithme.algoniv3V1: res = "Algorithme niv3v1 mathieu"; break;

            }
            

            return res;
        }
    }
}
