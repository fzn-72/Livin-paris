using System;
using System.Collections.Generic;
using System.Drawing;

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
            // Créer le graphe
            Graphe g = new Graphe(34);
            g.ChargerDepuisFichier("soc-karate.mtx");

            // Calculer les positions des nœuds
            DispositionGraphe disposition = new DispositionGraphe(g, 800, 600);
            Dictionary<int, Point> positions = disposition.CalculerPositions();

            // Visualisation du graphe avec les positions calculées
            VisualisationGraphe visualisation = new VisualisationGraphe(g);
            visualisation.DessinerGraphe("graphe.png");



            Console.WriteLine("Le graphe a été dessiné et sauvegardé sous 'graphe.png'. Il va apparaître automatiquement.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("graphe.png") { UseShellExecute = true });

            // Analyser les propriétés du graphe
            g.AnalyserProprietesGraphe();

            Console.WriteLine("-Voici le Parcours en Largeur (BFS) depuis le sommet 0: ");
            g.ParcoursLargeur(0);
            Console.WriteLine("\n");

            Console.WriteLine("-Voici le Parcours en Profondeur (DFS) depuis le sommet 0: ");
            g.ParcoursProfondeur(0);
            Console.WriteLine("\n");

            Console.WriteLine("-Le graphe est-il connexe ?");
            if (g.EstConnexe() == true)
            {
                Console.WriteLine("OUI : le graphe est bien connexe");
            }
            else
            {
                Console.WriteLine("NON : le graphe n'est pas connexe");
            }
            Console.WriteLine();

            Console.WriteLine("-Le graphe contient-il un cycle ?");
            if (g.ContientCycle() == true)
            {
                Console.WriteLine("OUI : le graphe contient bien un cycle");
            }
            else
            {
                Console.WriteLine("NON : le graphe ne contient aucun cycle");
            }
            Console.WriteLine();
        }
    }
}
