using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Cette classe permet de visualiser le réseau de métro parisien en utilisant les coordonnées géographiques des stations.
    /// </summary>
    public class VisualisationMetro
    {
        private MetroGraphe graphe;
        private int largeur;
        private int hauteur;
        private Dictionary<int, Point> positionsStations;

        // Coordonnées géographiques limites pour Paris
        private const double MIN_LONGITUDE = 2.22;
        private const double MAX_LONGITUDE = 2.47;
        private const double MIN_LATITUDE = 48.81;
        private const double MAX_LATITUDE = 48.91;

        /// <summary>
        /// Constructeur de la classe VisualisationMetro.
        /// </summary>
        /// <param name="graphe">Le graphe du métro à visualiser</param>
        /// <param name="largeur">Largeur de l'image en pixels</param>
        /// <param name="hauteur">Hauteur de l'image en pixels</param>
        public VisualisationMetro(MetroGraphe graphe, int largeur = 1200, int hauteur = 800)
        {
            this.graphe = graphe;
            this.largeur = largeur;
            this.hauteur = hauteur;
            this.positionsStations = new Dictionary<int, Point>();
            CalculerPositionsStations();
        }

        /// <summary>
        /// Calcule les positions des stations sur l'image en fonction de leurs coordonnées géographiques.
        /// </summary>
        private void CalculerPositionsStations()
        {
            // Marge en pixels autour de l'image
            int marge = 50;

            // Largeur et hauteur utiles de l'image
            int largeurUtile = largeur - 2 * marge;
            int hauteurUtile = hauteur - 2 * marge;

            // Calculer les positions des stations
            foreach (var station in graphe.Stations.Values)
            {
                // Normaliser les coordonnées géographiques en coordonnées d'image
                int x = marge + (int)(largeurUtile * (station.Longitude - MIN_LONGITUDE) / (MAX_LONGITUDE - MIN_LONGITUDE));
                int y = hauteur - marge - (int)(hauteurUtile * (station.Latitude - MIN_LATITUDE) / (MAX_LATITUDE - MIN_LATITUDE));

                positionsStations[station.Id] = new Point(x, y);
            }
        }

        /// <summary>
        /// Dessine le graphe du métro sur une image et la sauvegarde.
        /// </summary>
        /// <param name="cheminFichier">Chemin où sauvegarder l'image</param>
        public void DessinerMetro(string cheminFichier)
        {
            using (Bitmap bitmap = new Bitmap(largeur, hauteur))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // Fond blanc
                g.Clear(Color.White);

                // Dessiner une grille de référence
                DessinerGrille(g);

                // Dessiner les liens entre stations
                DessinerLiens(g);

                // Dessiner les stations
                DessinerStations(g);

                // Sauvegarder l'image
                bitmap.Save(cheminFichier, ImageFormat.Png);
            }

            Console.WriteLine($"Le plan du métro a été dessiné et sauvegardé sous '{cheminFichier}'.");
        }

        /// <summary>
        /// Dessine une grille de référence sur l'image.
        /// </summary>
        /// <param name="g">Objet Graphics pour dessiner</param>
        private void DessinerGrille(Graphics g)
        {
            Pen penGrille = new Pen(Color.LightGray, 1);

            // Grille horizontale
            for (double lat = MIN_LATITUDE; lat <= MAX_LATITUDE; lat += 0.01)
            {
                int y = hauteur - 50 - (int)((hauteur - 100) * (lat - MIN_LATITUDE) / (MAX_LATITUDE - MIN_LATITUDE));
                g.DrawLine(penGrille, 0, y, largeur, y);
            }

            // Grille verticale
            for (double lon = MIN_LONGITUDE; lon <= MAX_LONGITUDE; lon += 0.01)
            {
                int x = 50 + (int)((largeur - 100) * (lon - MIN_LONGITUDE) / (MAX_LONGITUDE - MIN_LONGITUDE));
                g.DrawLine(penGrille, x, 0, x, hauteur);
            }
        }

        /// <summary>
        /// Dessine les liens entre les stations.
        /// </summary>
        /// <param name="g">Objet Graphics pour dessiner</param>
        private void DessinerLiens(Graphics g)
        {
            // Dictionnaire pour stocker les couleurs des lignes de métro
            Dictionary<string, Color> couleursLignes = new Dictionary<string, Color>
            {
                {"1", Color.Yellow},
                {"2", Color.Blue},
                {"3", Color.Green},
                {"4", Color.Purple},
                {"5", Color.Orange},
                {"6", Color.LightGreen},
                {"7", Color.Pink},
                {"8", Color.Violet},
                {"9", Color.Yellow},
                {"10", Color.Gold},
                {"11", Color.Brown},
                {"12", Color.Green},
                {"13", Color.LightBlue},
                {"14", Color.Purple}
            };

            foreach (var lien in graphe.LiensNoeuds)
            {
                int idStation1 = lien.Noeud1.Id;
                int idStation2 = lien.Noeud2.Id;

                if (positionsStations.ContainsKey(idStation1) && positionsStations.ContainsKey(idStation2))
                {
                    Point p1 = positionsStations[idStation1];
                    Point p2 = positionsStations[idStation2];

                    // Déterminer la couleur du lien en fonction de la ligne
                    Color couleurLien = Color.Black;
                    if (graphe.Stations.ContainsKey(idStation1))
                    {
                        string ligne = graphe.Stations[idStation1].Ligne;
                        if (couleursLignes.ContainsKey(ligne))
                        {
                            couleurLien = couleursLignes[ligne];
                        }
                    }

                    Pen penLien = new Pen(couleurLien, 2);
                    g.DrawLine(penLien, p1, p2);
                }
            }
        }

        /// <summary>
        /// Dessine les stations sur l'image.
        /// </summary>
        /// <param name="g">Objet Graphics pour dessiner</param>
        private void DessinerStations(Graphics g)
        {
            Font fontNom = new Font("Arial", 8);
            Brush brushNom = Brushes.Black;

            foreach (var station in graphe.Stations.Values)
            {
                if (positionsStations.ContainsKey(station.Id))
                {
                    Point p = positionsStations[station.Id];

                    // Dessiner un cercle pour la station
                    g.FillEllipse(Brushes.Red, p.X - 5, p.Y - 5, 10, 10);
                    g.DrawEllipse(Pens.Black, p.X - 5, p.Y - 5, 10, 10);

                    // Afficher le nom de la station
                    g.DrawString(station.Nom, fontNom, brushNom, p.X + 5, p.Y - 5);
                }
            }
        }
    }
}