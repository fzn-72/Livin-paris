using System;
using System.IO;

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
                Console.WriteLine("2. Afficher le graphe des relations clients-cuisiniers par station de métro");
                Console.WriteLine("3. Retour au menu principal");

                Console.Write("\nChoisissez une option : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        GestionBDD.AfficherGrapheRelations();
                        break;
                    case "2":
                        AfficherGrapheRelationsMetro();
                        break;
                    case "3":
                        continuer = false;
                        break;
                    default:
                        GestionBDD.AfficherMessageErreur("Option invalide.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        /// <summary>
        /// Affiche le graphe des relations clients-cuisiniers par station de métro
        /// </summary>
        public static void AfficherGrapheRelationsMetro()
        {
            try
            {
                Console.Clear();
                GestionBDD.AfficherTitre("GRAPHE DES RELATIONS CLIENTS-CUISINIERS PAR STATION DE MÉTRO");

                Console.WriteLine("Génération du graphe des relations clients-cuisiniers par station de métro...");
                Console.WriteLine("Ce graphe affiche les clients et cuisiniers positionnés selon leurs stations de métro les plus proches.");
                Console.WriteLine();

                // Créer une instance de la classe ClientCuisinierMetroGraphe
                ClientCuisinierMetroGraphe graphe = new ClientCuisinierMetroGraphe();

                // Charger les données
                bool donneeChargees = graphe.ChargerDonnees();
                if (!donneeChargees)
                {
                    GestionBDD.AfficherMessageErreur("Erreur lors du chargement des données.");
                    Console.ReadKey();
                    return;
                }

                // Créer un chemin pour l'image
                string cheminImage = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                    "graphe_clients_cuisiniers_metro.png");

                // Visualiser le graphe
                bool visualisationReussie = graphe.VisualiserGraphe(cheminImage);

                if (!visualisationReussie)
                {
                    GestionBDD.AfficherMessageErreur("Erreur lors de la génération du graphe.");
                }
                else
                {
                    GestionBDD.AfficherMessageSucces($"Le graphe a été généré et enregistré sous : {cheminImage}");
                    Console.WriteLine("Une fenêtre devrait s'ouvrir pour afficher le graphe.");
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur lors de l'affichage du graphe des relations : {ex.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}
