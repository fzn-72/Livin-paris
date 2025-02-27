using System;

namespace Projet_problème_scientifique_info
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graphe g = new Graphe(34);
            g.ChargerDepuisFichier("soc-karate.mtx");

            Console.WriteLine("Parcours en Largeur (BFS) depuis le sommet 0:");
            g.ParcoursLargeur(0);
            Console.WriteLine("\n");

            Console.WriteLine("Parcours en Profondeur (DFS) depuis le sommet 0:");
            g.ParcoursProfondeur(0);
            Console.WriteLine("\n");

            Console.WriteLine("Le graphe est-il connexe ? " + g.EstConnexe());
            Console.WriteLine("Le graphe contient-il un cycle ? " + g.ContientCycle());
        }
    }
}
