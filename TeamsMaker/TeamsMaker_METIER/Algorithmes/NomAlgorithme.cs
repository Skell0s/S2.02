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
        faible_fort_switch
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
                case NomAlgorithme.ALGOTEST: res = "Algorithme de test (à supprimer)"; break;
                case NomAlgorithme.GLOUTON_CROISSANT: res = "Glouton croissant"; break;
                case NomAlgorithme.LEVEL_BALANCING: res = "Level  balancing"; break;
                case NomAlgorithme.ALGOROLEPRINCIPALE: res = "Algorithme de répartition par rôle principal"; break;
                case NomAlgorithme.faible_fort_switch: res = "faible fort switch"; break;
            }

            return res;
        }
    }
}
