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

        public VisualisationGraphe(Graphe graphe)
        {
            this.graphe = graphe;
        }

        public void GenererEtAfficherGraphe()
        {
            int largeur = 600;
            int hauteur = 600;
            Bitmap bitmap = new Bitmap(largeur, hauteur);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);

            Random rnd = new Random();
            Dictionary<int, PointF> positions = new Dictionary<int, PointF>();

            foreach (var noeud in graphe.ObtenirNoeuds())
            {
                positions[noeud.Id] = new PointF(rnd.Next(50, largeur - 50), rnd.Next(50, hauteur - 50));
            }

            Pen pen = new Pen(Color.Black, 2);
            foreach (var lien in graphe.ObtenirLiens())
            {
                g.DrawLine(pen, positions[lien.Noeud1.Id], positions[lien.Noeud2.Id]);
            }

            foreach (var noeud in graphe.ObtenirNoeuds())
            {
                PointF pos = positions[noeud.Id];
                g.FillEllipse(Brushes.Blue, pos.X - 10, pos.Y - 10, 20, 20);
                g.DrawString(noeud.Id.ToString(), new Font("Arial", 10), Brushes.White, pos.X - 5, pos.Y - 5);
            }

            g.Dispose();

            Form form = new Form { Text = "Visualisation du Graphe", Width = largeur, Height = hauteur };
            PictureBox pb = new PictureBox { Dock = DockStyle.Fill, Image = bitmap, SizeMode = PictureBoxSizeMode.Zoom };
            form.Controls.Add(pb);
            form.ShowDialog();
        }

    }
}

