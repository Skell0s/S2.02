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
        LOW_HIGH_MATCHING
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
                case NomAlgorithme.LEVEL_BALANCING: res = "Level  balancing (probleme n°1)"; break;
                case NomAlgorithme.LOW_HIGH_MATCHING: res = "fort avec faible (probleme n°1)"; break;
            }
            

            return res;
        }
    }
}
