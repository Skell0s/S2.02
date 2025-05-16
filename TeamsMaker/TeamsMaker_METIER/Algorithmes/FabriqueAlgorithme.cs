using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.AlgoTest;
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
                case NomAlgorithme.LEVEL_BALANCING: res = new Level_balancing(); break;
                case NomAlgorithme.ALGOROLEPRINCIPALE: res = new AlgoPb2(); break;
                case NomAlgorithme.ExtremePrem: res = new AlgoExtremeEnPremier(); break; 
                case NomAlgorithme.AlgoJulesPRBLÉME2: res = new algopb2jul(); break;
                case NomAlgorithme.AlgoPRogressif: res = new AlgorithmeExtremeEnPremier(); break;
                case NomAlgorithme.n_opt: res = new n_opt(); break;
                case NomAlgorithme.AlgoExtremeEnPremier_niv2: res = new AlgoExtremeEnPremier_niv2(); break;
                case NomAlgorithme.NSWAP: res = new NSwap(); break;
            }
            return res;
        }
        #endregion
    }
}
