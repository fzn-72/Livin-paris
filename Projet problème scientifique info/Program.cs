using System;
using System.Runtime.Remoting.Lifetime;

namespace Projet_problème_scientifique_info
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graphe g = new Graphe(34);
            g.ChargerDepuisFichier("soc-karate.mtx");

            Console.WriteLine("Voici le Parcours en Largeur (BFS) depuis le sommet 0: ");
            g.ParcoursLargeur(0);
            Console.WriteLine("\n");

            Console.WriteLine("Voici le Parcours en Profondeur (DFS) depuis le sommet 0: ");
            g.ParcoursProfondeur(0);
            Console.WriteLine("\n");

            Console.WriteLine("Le graphe est-il connexe ? ");
            if(g.EstConnexe() == true)
            {
                Console.WriteLine("OUI : le graphe est bien connexe");
            }
            else 
            { 
                Console.WriteLine("NON : le graphe n'est pas connexe");
            }
            Console.WriteLine("\n");

            Console.WriteLine("Le graphe contient-il un cycle ? ");
            if (g.ContientCycle() == true)
            {
                Console.WriteLine("OUI : le graphe contient bien un cycle");
            }
            else
            {
                Console.WriteLine("NON : le graphe ne contient aucun cycle");
            }
            Console.WriteLine("\n");

            Console.WriteLine("Le graphe représentant la situation va maintenant être généré");    
            VisualisationGraphe visualisation = new VisualisationGraphe(g);
            visualisation.DessinerGraphe("graphe.png");
            Console.WriteLine("Le graphe a été dessiné et sauvegardé sous 'graphe.png'. Il va apparaître automatiquement.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("graphe.png") { UseShellExecute = true }); /// permet d'ouvrir l'image directement
        }
    }
}
