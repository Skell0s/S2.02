using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes
{
    /// <summary>
    /// Notion générique d'algorithme
    /// </summary>
    public abstract class Algorithme
    {
        #region --- Attributs ---
        private long tempsExecution = -1;    //Temps d'exécution de l'algorithme
        #endregion

        /// <summary>
        /// Lance la répartition des personnages d'un jeu de test donnée
        /// </summary>
        /// <param name="jeuTest">Jeu de test</param>
        /// <returns>La répartition</returns>
        public abstract Repartition Repartir(JeuTest jeuTest);

        /// <summary>
        /// Temps d'exécution de l'algorithme
        /// </summary>
        public long TempsExecution
        {
            get => this.tempsExecution;
            protected set => this.tempsExecution = value;
        }

        protected Repartition SupprimerEquipeScoreEleve(Repartition r, JeuTest jeuTest)
        {
            Repartition repartition = new Repartition(jeuTest);
            foreach (Equipe equipe in r.Equipes)
            {
                if (equipe.Score(Probleme.SIMPLE) < 600)
                {
                    repartition.AjouterEquipe(equipe);
                }
            }
            return repartition;
        }
    }
}
