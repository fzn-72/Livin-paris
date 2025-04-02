using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// La classe VisualisationGraphe permet de visualiser un graphe en générant une image représentant les nœuds et leurs liens. 
    /// Elle génère d'abord les positions des nœuds de manière optimisée (ou aléatoire si nécessaire), puis dessine le graphe sur une image, 
    /// en sauvegardant le résultat sous un fichier PNG. Elle contient des méthodes pour dessiner les liens entre nœuds et afficher les nœuds eux-mêmes.
    /// </summary>
    internal class VisualisationGraphe<T>
    {
        private Graphe<T> graphe;
        private int largeur;
        private int hauteur;
        private Dictionary<T, Point> positionsNoeuds;
        private Random random;

        /// <summary>
        /// Constructeur de la classe VisualisationGraphe. Il initialise les propriétés nécessaires pour la visualisation du graphe, 
        /// y compris le graphe lui-même, la largeur et la hauteur de l'image, ainsi que la génération des positions des nœuds en utilisant 
        /// une disposition optimisée.
        /// </summary>
        /// <param name="graphe"></param>
        /// <param name="largeur"></param>
        /// <param name="hauteur"></param>
        public VisualisationGraphe(Graphe<T> graphe, int largeur = 800, int hauteur = 600)
        {
            this.graphe = graphe;
            this.largeur = largeur;
            this.hauteur = hauteur;
            this.positionsNoeuds = new Dictionary<T, Point>();
            this.random = new Random();
            GenererPositionsOptimisees(); /// Utilisation de la méthode optimisée pour générer les positions
        }

        /// <summary>
        /// Génère les positions des nœuds de manière aléatoire dans la zone définie par la largeur et la hauteur. 
        /// Chaque nœud est assigné une position (x, y) générée aléatoirement à l'intérieur de la zone de dessin.
        /// </summary>
        private void GenererPositionsNoeuds()
        {
            foreach (var noeud in graphe.ListeAdjacence.Keys)
            {
                int x = random.Next(50, largeur - 50);
                int y = random.Next(50, hauteur - 50);
                positionsNoeuds[noeud] = new Point(x, y);
            }
        }

        /// <summary>
        /// Génère les positions des nœuds en utilisant une disposition optimisée. 
        /// Cette méthode crée une instance de la classe DispositionGraphe pour calculer et attribuer les positions des nœuds en fonction de la taille du graphe et de la zone de dessin.
        /// </summary>
        /// </summary>
        private void GenererPositionsOptimisees()
        {
            DispositionGraphe<T> disposition = new DispositionGraphe<T>(graphe, largeur, hauteur);
            positionsNoeuds = disposition.CalculerPositions();
        }

        /// <summary>
        /// Dessine le graphe en créant une image à partir des positions des nœuds et des liens. 
        /// Les nœuds sont représentés par des cercles bleus, et les liens sont dessinés comme des lignes noires. 
        /// Le nom de chaque nœud est affiché au centre de chaque cercle. L'image est ensuite sauvegardée dans un fichier au format PNG.
        /// </summary>
        /// <param name="cheminFichier"></param>
        public void DessinerGraphe(string cheminFichier)
        {
            using (Bitmap bitmap = new Bitmap(largeur, hauteur))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);

                Pen penLien = new Pen(Color.Black, 2);
                foreach (var lien in graphe.LiensNoeuds)
                {
                    Point p1 = positionsNoeuds[lien.Noeud1.Id];
                    Point p2 = positionsNoeuds[lien.Noeud2.Id];
                    g.DrawLine(penLien, p1, p2);
                }

                Brush brushNoeud = Brushes.Blue;
                Font font = new Font("Arial", 10);
                Brush brushTexte = Brushes.White;
                foreach (var noeud in graphe.ListeAdjacence.Keys)
                {
                    Point p = positionsNoeuds[noeud];
                    g.FillEllipse(brushNoeud, p.X - 15, p.Y - 15, 30, 30);
                    g.DrawString(noeud.ToString(), font, brushTexte, p.X - 7, p.Y - 7);
                }

                bitmap.Save(cheminFichier, ImageFormat.Png);
            }
        }
    }
}
