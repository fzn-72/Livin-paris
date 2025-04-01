using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;

namespace Projet_problème_scientifique_info
{
    internal class Program
    {
        /// <summary>
        /// Point d'entrée principal du code. Cette méthode initialise un graphe et charge les données à partir du fichier fourni.
        /// Elle génère et affiche une visualisation de ce graphe. Elle présente aussi une série de propriétes de ce graphe: 
        /// elle exécute les parcours en largeur (BFS) et en profondeur (DFS), vérifie la connexité et la présence de cycles dans le graphe.
        /// Elle fait aussi appel à la méthode "AnalyserProprietesGraphe" qui vérifie si le graphe est pondéré, s'il est orienté, et donne 
        /// la taille ainsi que l'ordre du graphe. 
        /// </summary>
        static void Main(string[] args)
        {
            ///// Créer le graphe
            //Graphe<int> g = new Graphe<int>();
            //g.ChargerDepuisFichier("soc-karate.mtx", s => (int)Convert.ChangeType(s, typeof(int)));

            ///// Calculer les positions des nœuds
            //DispositionGraphe<int> disposition = new DispositionGraphe<int>(g, 800, 600);
            //Dictionary<int, Point> positions = disposition.CalculerPositions();

            ///// Visualisation du graphe avec les positions calculées
            //VisualisationGraphe<int> visualisation = new VisualisationGraphe<int>(g);
            //visualisation.DessinerGraphe("graphe.png");

            //Console.WriteLine("Le graphe a été dessiné et sauvegardé sous 'graphe.png'. Il va apparaître automatiquement.");
            //System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("graphe.png") { UseShellExecute = true }); /// permet d'ouvrir l'image créée (fourni par l'IA)

            ///// Analyser les propriétés du graphe
            //g.AnalyserProprietesGraphe();

            //Console.WriteLine("-Voici le Parcours en Largeur (BFS) depuis le sommet 0: ");
            //g.ParcoursLargeur(0);
            //Console.WriteLine("\n");

            //Console.WriteLine("-Voici le Parcours en Profondeur (DFS) depuis le sommet 0: ");
            //g.ParcoursProfondeur(0);
            //Console.WriteLine("\n");

            //Console.WriteLine("-Le graphe est-il connexe ?");
            //if (g.EstConnexe() == true)
            //{
            //    Console.WriteLine("OUI : le graphe est bien connexe");
            //}
            //else
            //{
            //    Console.WriteLine("NON : le graphe n'est pas connexe");
            //}
            //Console.WriteLine();

            //Console.WriteLine("-Le graphe contient-il un cycle ?");
            //if (g.ContientCycle() == true)
            //{
            //    Console.WriteLine("OUI : le graphe contient bien un cycle");
            //}
            //else
            //{
            //    Console.WriteLine("NON : le graphe ne contient aucun cycle");
            //}
            //Console.WriteLine();

            /// CalculDistance.DistanceEntreDeuxStations();

            Console.WriteLine("=== Bienvenue dans l'apllication console Liv'In Paris ===");
            Console.WriteLine();
            Console.WriteLine("Que souhaitez-vous faire ? Choisissez une option (1 ou 2) : ");
            Console.WriteLine("1. Construire et visualiser le graphe du métro");
            Console.WriteLine("2. Calculer la distance entre deux stations de métro");

            string choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    ConstruireEtVisualiserGrapheMetro();
                    break;
                case "2":
                    CalculDistance.DistanceEntreDeuxStations();
                    break;
                default:
                    Console.WriteLine("Option non valide.");
                    break;
            }

            Console.WriteLine("\nAppuyez sur une touche pour quitter...");
            Console.ReadKey();
        }

        static void ConstruireEtVisualiserGrapheMetro()
        {
            // Chemins vers les fichiers CSV
            string cheminFichierNoeuds = @"C:\Users\fzzn7\Documents\GitHub\PSI_2025_ThoFaizMart\Feuille_noeud_convertie.csv";
            string cheminFichierArcs = @"C:\Users\fzzn7\Documents\GitHub\PSI_2025_ThoFaizMart\Feuille_arc_convertie.csv";

            // Créer une instance de MetroGraphe
            MetroGraphe metroGraphe = new MetroGraphe(cheminFichierNoeuds, cheminFichierArcs);

            // Construire le graphe
            if (metroGraphe.ConstruireGraphe())
            {
                Console.WriteLine("Voici une visualisation géographique (basée sur les coordonnées):");
                string cheminImage = "graphe_metro.png";

                VisualisationMetro visuMetro = new VisualisationMetro(metroGraphe, 1200, 800);
                visuMetro.DessinerMetro(cheminImage);

                // Ouvrir l'image
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(cheminImage) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de l'ouverture de l'image : {ex.Message}");
                }

                // Analyser les propriétés du graphe
                metroGraphe.AnalyserProprietesGraphe();
            }
        }
    }
}
