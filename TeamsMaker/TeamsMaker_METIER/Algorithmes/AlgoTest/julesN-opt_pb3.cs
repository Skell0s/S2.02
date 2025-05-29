using System;
using System.Collections.Generic;
using System.Linq;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.AlgoTest
{
    internal class julesN_opt_pb3 : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Personnage[] personnages = jeuTest.Personnages;
            Repartition repartition = new Repartition(jeuTest);

            // Étape 1 : Générer toutes les combinaisons possibles de 4 personnages distincts
            var toutesCombinaisons = GetCombinations(personnages.ToList(), 4);

            HashSet<Personnage> personnagesUtilises = new HashSet<Personnage>();

            foreach (var combinaison in toutesCombinaisons)
            {
                // Vérifier si un personnage est déjà utilisé
                if (combinaison.Any(p => personnagesUtilises.Contains(p)))
                    continue;

                // Créer l'équipe et vérifier sa validité (en testant rôles secondaires)
                Equipe equipe = new Equipe();
                foreach (var p in combinaison)
                    equipe.AjouterMembre(p);

                if (equipe.EstValide(Probleme.ROLESECONDAIRE))
                {
                    repartition.AjouterEquipe(equipe);
                    foreach (var p in combinaison)
                        personnagesUtilises.Add(p); // Marquer comme utilisé
                }
            }

            return repartition;
        }

        // Génère toutes les combinaisons de 4 personnages parmi la liste
        private List<List<Personnage>> GetCombinations(List<Personnage> source, int taille)
        {
            var result = new List<List<Personnage>>();
            Combiner(source, new List<Personnage>(), 0, taille, result);
            return result;
        }

        private void Combiner(List<Personnage> source, List<Personnage> current, int index, int taille, List<List<Personnage>> result)
        {
            if (current.Count == taille)
            {
                result.Add(new List<Personnage>(current));
                return;
            }

            for (int i = index; i < source.Count; i++)
            {
                current.Add(source[i]);
                Combiner(source, current, i + 1, taille, result);
                current.RemoveAt(current.Count - 1);
            }
        }
    }
}
