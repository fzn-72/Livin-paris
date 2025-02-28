using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace Projet_problème_scientifique_info
{
    internal class VisualisationGraphe
    {
        private Graphe graphe;
        private int largeur;
        private int hauteur;
        private Dictionary<int, Point> positionsNoeuds;
        private Random random;

        public VisualisationGraphe(Graphe graphe, int largeur = 800, int hauteur = 600)
        {
            this.graphe = graphe;
            this.largeur = largeur;
            this.hauteur = hauteur;
            this.positionsNoeuds = new Dictionary<int, Point>();
            this.random = new Random();
            GenererPositionsNoeuds();
        }

        private void GenererPositionsNoeuds()
        {
            foreach (var noeud in graphe.ListeAdjacence.Keys)
            {
                int x = random.Next(50, largeur - 50);
                int y = random.Next(50, hauteur - 50);
                positionsNoeuds[noeud] = new Point(x, y);
            }
        }

        public void DessinerGraphe(string cheminFichier)
        {
            using (Bitmap bitmap = new Bitmap(largeur, hauteur))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                Pen penLien = new Pen(Color.Black, 2);
                Brush brushNoeud = Brushes.Blue;
                Font font = new Font("Arial", 10);
                Brush brushTexte = Brushes.White;

                foreach (var lien in graphe.LiensNoeuds)
                {
                    Point p1 = positionsNoeuds[lien.Noeud1.Id];
                    Point p2 = positionsNoeuds[lien.Noeud2.Id];
                    g.DrawLine(penLien, p1, p2);
                }

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

