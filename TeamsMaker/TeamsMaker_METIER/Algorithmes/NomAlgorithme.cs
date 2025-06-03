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
        LEVEL_BALANCING,
        ExtremePrem,
        AlgoPRogressif,
        n_opt,
        AlgoExtremeEnPremier_niv2,
        NSWAP,
        NSWAP_NIVEAU3,
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
                case NomAlgorithme.LEVEL_BALANCING: res = "Level  balancing "; break;
                case NomAlgorithme.ExtremePrem: res = "Extremes en premier"; break;
                case NomAlgorithme.AlgoExtremeEnPremier_niv2: res = "Extremes en premier niveau 2"; break;
                case NomAlgorithme.AlgoPRogressif: res = "Algorithme progressif"; break;
                case NomAlgorithme.n_opt: res = "Algorithme n-opt"; break;
                case NomAlgorithme.NSWAP: res = "Algorithme n-swap"; break;
                case NomAlgorithme.NSWAP_NIVEAU3: res = "Algorithme n-swap niveau 3"; break;
            }
            

            return res;
        }
    }
}
