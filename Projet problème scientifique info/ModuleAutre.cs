using System;
using System.IO;
using Projet_problème_scientifique_info;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Module pour les fonctionnalités supplémentaires de l'application
    /// </summary>
    internal class ModuleAutre
    {
        /// <summary>
        /// Affiche le menu principal du module Autre
        /// </summary>
        public static void MenuPrincipalAutre()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("MODULE AUTRE");

                Console.WriteLine("1. Afficher le graphe des relations clients-cuisiniers");
                Console.WriteLine("2. Retour au menu principal");

                Console.Write("\nChoisissez une option : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        GestionBDD.AfficherGrapheRelations();
                        break;
                    case "2":
                        continuer = false;
                        break;
                    default:
                        GestionBDD.AfficherMessageErreur("Option invalide.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
