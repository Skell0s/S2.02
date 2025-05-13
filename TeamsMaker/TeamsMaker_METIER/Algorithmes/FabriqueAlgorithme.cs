using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.Realisations;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes
{
    /// <summary>
    /// Fabrique des algorithmes
    /// </summary>
    public class FabriqueAlgorithme
    {
        #region --- Propriétés ---
        /// <summary>
        /// Liste des noms des algorithmes
        /// </summary>
        public string[] ListeAlgorithmes => Enum.GetValues(typeof(NomAlgorithme)).Cast<NomAlgorithme>().ToList().Select(nom => nom.Affichage()).ToArray();
        #endregion

        #region --- Méthodes ---
        /// <summary>
        /// Fabrique d'algorithme en fonction du nom de l'algorithme
        /// </summary>
        /// <param name="nomAlgorithme">Nom de l'algorithme</param>
        /// <returns></returns>
        public Algorithme? Creer(NomAlgorithme nomAlgorithme)
        {
            Algorithme res = null;
            switch(nomAlgorithme)
            {
                case NomAlgorithme.ALGOTEST: res = new AlgorithmeTest(); break;
                case NomAlgorithme.GLOUTON_CROISSANT: res = new AlgorithmesGloutonCroissant(); break;
<<<<<<< Updated upstream
=======
                case NomAlgorithme.LEVEL_BALANCING: res = new Level_balancing(); break;
                case NomAlgorithme.ALGOROLEPRINCIPALE: res = new AlgoPb2(); break;
                case NomAlgorithme.ALGOROLESECONDAIREV1: res = new AlgoPb3V1(); break;
>>>>>>> Stashed changes
            }
            return res;
        }
        #endregion
    }
}
