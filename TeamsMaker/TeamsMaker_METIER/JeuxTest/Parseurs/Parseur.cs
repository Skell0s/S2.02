using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;

namespace TeamsMaker_METIER.JeuxTest.Parseurs
{
    public class Parseur
    {
        private Personnage ParserLigne(string ligne)
        {
            string[] morceau = ligne.Split(" ");
            Classe classe = (Classe)Enum.Parse(typeof(Classe), morceau[0]);
            int lvlPrincipal = Int32.Parse(morceau[1]);
            int lvlSecondaire = Int32.Parse(morceau[2]);
            Personnage personnage = new Personnage(classe, lvlPrincipal, lvlSecondaire); //Classe, niveau principal, niveau secondaire
            return personnage;
        }   
        public JeuTest Parser(string nomFichier)
        {
            JeuTest jeuTest = new JeuTest(); 
            
            string cheminFichier = Path.Combine(Directory.GetCurrentDirectory(), "JeuxTest/Fichiers/" + nomFichier); 
            using (StreamReader stream = new StreamReader(cheminFichier))
            {
                string ligne; while ((ligne = stream.ReadLine()) != null)
                {
                    jeuTest.AjouterPersonnage(this.ParserLigne(ligne)); //On parse la ligne
                } 
            }
            return jeuTest ;
        }
    }
}
