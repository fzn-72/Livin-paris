using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Projet_problème_scientifique_info
{
    public class ModuleTrajets
    {

        #region Menu Principal de la classe ModuleTrajets
        /// <summary>
        /// Cette méthode initialise un graphe et charge les données à partir du fichier fourni.
        /// Elle génère et affiche une visualisation de ce graphe. Elle présente aussi une série de propriétes de ce graphe: 
        /// elle exécute les parcours en largeur (BFS) et en profondeur (DFS), vérifie la connexité et la présence de cycles dans le graphe.
        /// Elle fait aussi appel à la méthode "AnalyserProprietesGraphe" qui vérifie si le graphe est pondéré, s'il est orienté, et donne 
        /// la taille ainsi que l'ordre du graphe. 
        /// </summary>
        public static void MenuPrincipalTrajets()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                Console.WriteLine("Que souhaitez-vous faire ? Choisissez une option (1, 2, 3 ou 4) : ");
                Console.WriteLine("1. Construire et visualiser le graphe du métro");
                Console.WriteLine("2. Calculer la distance entre deux stations de métro");
                Console.WriteLine("3. Trouver le chemin le plus court entre deux stations");
                Console.WriteLine("4. Retour au menu principal");

                string choix = Console.ReadLine();

                try
                {
                    switch (choix)
                    {
                        case "1":
                            ConstruireEtVisualiserGrapheMetro();
                            break;
                        case "2":
                            CalculDistance.DistanceEntreDeuxStations();
                            break;
                        case "3":
                            CalculDistance.CheminLePlusCourt();
                            //ModuleCuisinier.TrouverCheminOptimal();
                            break;
                        case "4":
                            continuer = false;
                            break;
                        default:
                            Console.WriteLine("Option non valide. Appuyez sur une touche pour continuer...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Une erreur s'est produite: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                }
            }
        }
        #endregion
    
        #region Construction et Visualisation du graphe
        /// <summary>
        /// Construit et visualise un graphe représentant le réseau de métro à partir de fichiers CSV contenant les nœuds (stations) et les arcs (liaisons).
        /// </summary>
        static void ConstruireEtVisualiserGrapheMetro()
        {
            try
            {
                string cheminFichierNoeuds = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    @"..\..\..\Feuille_noeud_convertie.csv"
                    );

                cheminFichierNoeuds = Path.GetFullPath(cheminFichierNoeuds);
                
                string cheminFichierArcs = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    @"..\..\..\Feuille_arc_convertie.csv"
                    );

                cheminFichierArcs = Path.GetFullPath(cheminFichierArcs);


                if (!File.Exists(cheminFichierNoeuds))
                {
                    Console.WriteLine($"Erreur: Le fichier {cheminFichierNoeuds} n'existe pas.");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                if (!File.Exists(cheminFichierArcs))
                {
                    Console.WriteLine($"Erreur: Le fichier {cheminFichierArcs} n'existe pas.");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                MetroGraphe metroGraphe = new MetroGraphe(cheminFichierNoeuds, cheminFichierArcs);

                Console.WriteLine("Chargement des stations...");
                int nbStations = metroGraphe.ChargerStations();
                Console.WriteLine("Chargement des arcs et création des liens...");
                int nbArcs = metroGraphe.ChargerArcs();

                if (nbStations > 0 && metroGraphe.LiensNoeuds.Any())
                {
                    Console.WriteLine($"Chargement terminé: {nbStations} stations, {metroGraphe.LiensNoeuds.Count} liens dans le graphe.");
                    Console.WriteLine("Génération de la visualisation...");
                    string cheminImage = "graphe_metro.png";

                    VisualisationMetroAmelioree visuMetro = new VisualisationMetroAmelioree(metroGraphe, 4000, 3000);
                    visuMetro.DessinerMetro(cheminImage);

                    try
                    {
                        Console.WriteLine($"Image sauvegardée sous: {Path.GetFullPath(cheminImage)}");
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(cheminImage) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de l'ouverture de l'image : {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Erreur : Aucune station ou aucun lien n'a pu être chargé. Vérifiez les fichiers CSV et les chemins.");
                }

                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur générale est survenue : {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
        }
        #endregion
    }
}

