using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        AlgoPRogressif
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
                case NomAlgorithme.AlgoJulesPRBLÉME2: res = "ProblèmeJules----TEST----"; break;
                case NomAlgorithme.AlgoPRogressif: res = "Algorithme progressif"; break;
            }

            return res;
        }
    }
}
