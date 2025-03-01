using System;
using System.Collections.Generic;
using System.Drawing;

namespace Projet_problème_scientifique_info
{
    internal class DispositionGraphe
    {
        private Graphe graphe;
        private int largeur;
        private int hauteur;
        private Dictionary<int, Point> positionsNoeuds;

        public DispositionGraphe(Graphe graphe, int largeur, int hauteur)
        {
            this.graphe = graphe;
            this.largeur = largeur;
            this.hauteur = hauteur;
            this.positionsNoeuds = new Dictionary<int, Point>();
        }

        /// <summary>
        /// Calcule une disposition optimisée des nœuds du graphe.
        /// </summary>
        /// <returns>Un dictionnaire associant chaque nœud à une position.</returns>
        public Dictionary<int, Point> CalculerPositions()
        {
            int n = graphe.ListeAdjacence.Count;
            double angleIncrement = 2 * Math.PI / n;
            int rayon = Math.Min(largeur, hauteur) / 3;

            int centreX = largeur / 2;
            int centreY = hauteur / 2;
            int i = 0;

            foreach (var noeud in graphe.ListeAdjacence.Keys)
            {
                double angle = i * angleIncrement;
                int x = (int)(centreX + rayon * Math.Cos(angle));
                int y = (int)(centreY + rayon * Math.Sin(angle));

                positionsNoeuds[noeud] = new Point(x, y);
                i++;
            }

            return positionsNoeuds;
        }
    }
}
