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
    /// ECRIS ICI MARTIN
    /// </summary>
    internal class VisualisationGraphe
    {
        private Graphe graphe;
        private int largeur;
        private int hauteur;
        private Dictionary<int, Point> positionsNoeuds;
        private Random random;

        /// <summary>
        /// ECRIS ICI MARTIN
        /// </summary>
        /// <param name="graphe"></param>
        /// <param name="largeur"></param>
        /// <param name="hauteur"></param>
        public VisualisationGraphe(Graphe graphe, int largeur = 800, int hauteur = 600)
        {
            this.graphe = graphe;
            this.largeur = largeur;
            this.hauteur = hauteur;
            this.positionsNoeuds = new Dictionary<int, Point>();
            this.random = new Random();
            GenererPositionsOptimisees(); // Utilisation de la méthode optimisée pour générer les positions
        }

        /// <summary>
        /// ECRIS ICI MARTIN
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
        /// </summary>
        private void GenererPositionsOptimisees()
        {
            DispositionGraphe disposition = new DispositionGraphe(graphe, largeur, hauteur);
            positionsNoeuds = disposition.CalculerPositions();
        }

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
