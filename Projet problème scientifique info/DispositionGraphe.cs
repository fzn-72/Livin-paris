using System;
using System.Collections.Generic;
using System.Drawing;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// La classe DispositionGraphe est responsable du calcul de la disposition des nœuds d'un graphe dans une zone de dessin définie par sa largeur et sa hauteur. 
    /// Elle prend un graphe en entrée et génère les positions des nœuds de manière circulaire autour du centre de la zone. 
    /// Les positions sont ensuite retournées sous forme de dictionnaire, où chaque nœud est associé à un point de position (x, y).
    /// </summary>
    internal class DispositionGraphe<T>
    {
        private Graphe<T> graphe;
        private int largeur;
        private int hauteur;
        private Dictionary<T, Point> positionsNoeuds;

        #region Constructeur de la classe DispositionGraphe
        /// <summary>
        /// Constructeur qui initialise les propriétés nécessaires pour calculer la disposition des nœuds, 
        /// en prenant le graphe, la largeur et la hauteur de la zone de dessin comme paramètres. Un dictionnaire vide est créé pour stocker les positions des nœuds.
        /// </summary>
        /// <param name="graphe"></param>
        /// <param name="largeur"></param>
        /// <param name="hauteur"></param>
        public DispositionGraphe(Graphe<T> graphe, int largeur, int hauteur)
        {
            this.graphe = graphe;
            this.largeur = largeur;
            this.hauteur = hauteur;
            this.positionsNoeuds = new Dictionary<T, Point>();
        }
        #endregion

        #region Calcul des positions des noeuds 
        /// <summary>
        /// Calcule les positions des nœuds du graphe en les disposant de manière circulaire autour du centre de la zone de dessin. 
        /// Chaque nœud est positionné selon un angle calculé uniformément réparti autour du cercle. Les positions sont ensuite retournées sous forme de dictionnaire.
        /// </summary>
        /// <returns>Un dictionnaire associant chaque nœud à une position.</returns>
        public Dictionary<T, Point> CalculerPositions()
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
        #endregion 
    }
}
