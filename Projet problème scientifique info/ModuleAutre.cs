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
                Console.WriteLine("2. Afficher le menu d'exportation");
                Console.WriteLine("3. Retour au menu principal");

                Console.Write("\nChoisissez une option : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        GestionBDD.AfficherGrapheRelations();
                        break;
                    case "2":
                        MenuExportation();
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
        /// Menu d'exportation des données
        /// </summary>
        public static void MenuExportation()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("MENU EXPORTATION");
                Console.WriteLine("Que souhaitez-vous exporter ?");
                Console.WriteLine("\n1. Exporter la liste des plats en XML");
                Console.WriteLine("2. Exporter la liste des plats en JSON");
                Console.WriteLine("3. Exporter les statistiques en XML");
                Console.WriteLine("4. Exporter les statistiques en JSON");
                Console.WriteLine("5. Exporter les commandes en JSON");
                Console.WriteLine("6. Lire un fichier JSON");
                Console.WriteLine("7. Retour au menu principal");
                Console.Write("\nChoisissez une option : ");

                string choix = Console.ReadLine();
                switch (choix)
                {
                    case "1":
                        ModulePlat.ExporterListePlats();
                        break;
                    case "2":
                        ModulePlat.ExporterListePlatsJSON();
                        break;
                    case "3":
                        if (SessionUtilisateur.EstConnecte())
                        {
                            if (SessionUtilisateur.TypeUtilisateur == "client")
                            {
                                ModuleStats.ExporterStatistiquesClient(SessionUtilisateur.IdUtilisateur);
                            }
                            else if (SessionUtilisateur.TypeUtilisateur == "cuisinier")
                            {
                                ModuleStats.ExporterStatistiquesCuisinier(SessionUtilisateur.IdUtilisateur);
                            }
                            else if (SessionUtilisateur.TypeUtilisateur == "admin")
                            {
                                ModuleStats.ExporterStatistiquesGlobales();
                            }
                        }
                        else
                        {
                            ModuleStats.ExporterStatistiquesGlobales();
                        }
                        break;
                    case "4":
                        if (SessionUtilisateur.EstConnecte())
                        {
                            if (SessionUtilisateur.TypeUtilisateur == "client")
                            {
                                ModuleStats.ExporterStatistiquesClientJSON(SessionUtilisateur.IdUtilisateur);
                            }
                            else if (SessionUtilisateur.TypeUtilisateur == "cuisinier")
                            {
                                ModuleStats.ExporterStatistiquesCuisinierJSON(SessionUtilisateur.IdUtilisateur);
                            }
                            else if (SessionUtilisateur.TypeUtilisateur == "admin")
                            {
                                ModuleStats.ExporterStatistiquesGlobalesJSON();
                            }
                        }
                        else
                        {
                            ModuleStats.ExporterStatistiquesGlobalesJSON();
                        }
                        break;
                    case "5":
                        ModuleCommande.ExporterCommandesJSON();
                        break;
                    case "6":
                        LireFichierJSON();
                        break;
                    case "7":
                        continuer = false;
                        break;
                    default:
                        GestionBDD.AfficherMessageErreur("Option invalide.");
                        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        /// <summary>
        /// Lit un fichier JSON et affiche son contenu
        /// </summary>
        private static void LireFichierJSON()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("LECTURE DE FICHIER JSON");

            Console.Write("Entrez le chemin complet du fichier JSON à lire : ");
            string cheminFichier = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(cheminFichier))
            {
                GestionBDD.AfficherMessageErreur("Chemin de fichier invalide.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            if (!File.Exists(cheminFichier))
            {
                GestionBDD.AfficherMessageErreur("Le fichier spécifié n'existe pas.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nLecture du fichier JSON :");
            Console.WriteLine("-------------------------\n");
            ExportJson.LectureJson(cheminFichier);

            Console.WriteLine("\nAffichage formaté du fichier JSON :");
            Console.WriteLine("----------------------------------\n");
            ExportJson.AfficherPrettyJson(cheminFichier);

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}
