#region ancienne version pour afficher comme avant
/*using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;
using System.Linq;

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
        private Dictionary<int, Rectangle> zonesTexte;

        // Facteur d'étirement pour espacer les stations
        private float facteurEtirementX = 2.0f;
        private float facteurEtirementY = 2.2f; // Légèrement augmenté pour l'axe vertical

        /// <summary>
        /// Coordonnées géographiques limites pour Paris
        /// </summary>
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
        /// <param name="facteurEtirementX">Facteur d'étirement horizontal</param>
        /// <param name="facteurEtirementY">Facteur d'étirement vertical</param>
        public VisualisationMetro(MetroGraphe graphe, int largeur = 3800, int hauteur = 2600, float facteurEtirementX = 2.0f, float facteurEtirementY = 2.2f)
        {
            this.graphe = graphe;
            this.largeur = largeur;
            this.hauteur = hauteur;
            this.facteurEtirementX = facteurEtirementX;
            this.facteurEtirementY = facteurEtirementY;
            this.positionsStations = new Dictionary<int, Point>();
            this.zonesTexte = new Dictionary<int, Rectangle>();
            CalculerPositionsStations();
        }

        /// <summary>
        /// Calcule les positions des stations sur l'image en fonction de leurs coordonnées géographiques.
        /// </summary>
        private void CalculerPositionsStations()
        {
            // Calculer le centre géographique
            double centreLongitude = (MIN_LONGITUDE + MAX_LONGITUDE) / 2;
            double centreLatitude = (MIN_LATITUDE + MAX_LATITUDE) / 2;

            // Calculer les nouvelles limites étirées
            double largeurGeo = (MAX_LONGITUDE - MIN_LONGITUDE) * facteurEtirementX;
            double hauteurGeo = (MAX_LATITUDE - MIN_LATITUDE) * facteurEtirementY;

            double minLon = centreLongitude - largeurGeo / 2;
            double maxLon = centreLongitude + largeurGeo / 2;
            double minLat = centreLatitude - hauteurGeo / 2;
            double maxLat = centreLatitude + hauteurGeo / 2;

            // Marge en pixels autour de l'image
            int marge = 120;

            // Largeur et hauteur utiles de l'image
            int largeurUtile = largeur - 2 * marge;
            int hauteurUtile = hauteur - 2 * marge;

            // Calculer les positions des stations
            foreach (var station in graphe.Stations.Values)
            {
                // Calculer la position relative par rapport au centre
                double relLon = station.Longitude - centreLongitude;
                double relLat = station.Latitude - centreLatitude;

                // Étirer les coordonnées
                double etireLon = relLon * facteurEtirementX;
                double etireLat = relLat * facteurEtirementY;

                // Calculer les nouvelles coordonnées étirées
                double nouvelleLon = centreLongitude + etireLon;
                double nouvelleLat = centreLatitude + etireLat;

                // Normaliser les coordonnées géographiques en coordonnées d'image
                int x = marge + (int)(largeurUtile * (nouvelleLon - minLon) / (maxLon - minLon));
                int y = hauteur - marge - (int)(hauteurUtile * (nouvelleLat - minLat) / (maxLat - minLat));

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
                // Activer l'antialiasing pour un rendu plus lisse
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                // Fond blanc
                g.Clear(Color.White);

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
        /// Dessine les liens entre les stations.
        /// </summary>
        /// <param name="g">Objet Graphics pour dessiner</param>
        private void DessinerLiens(Graphics g)
        {
            // Dictionnaire pour stocker les couleurs des lignes de métro
            Dictionary<string, Color> couleursLignes = new Dictionary<string, Color>
            {
                {"1", Color.FromArgb(255, 255, 206, 0)},    // Jaune
                {"2", Color.FromArgb(255, 0, 100, 180)},    // Bleu
                {"3", Color.FromArgb(255, 152, 199, 0)},    // Vert
                {"4", Color.FromArgb(255, 187, 76, 158)},   // Violet
                {"5", Color.FromArgb(255, 255, 137, 0)},    // Orange
                {"6", Color.FromArgb(255, 118, 184, 0)},    // Vert clair
                {"7", Color.FromArgb(255, 255, 137, 170)},  // Rose
                {"8", Color.FromArgb(255, 155, 0, 222)},    // Violet
                {"9", Color.FromArgb(255, 220, 190, 0)},    // Jaune
                {"10", Color.FromArgb(255, 220, 170, 0)},   // Or
                {"11", Color.FromArgb(255, 139, 87, 42)},   // Marron
                {"12", Color.FromArgb(255, 0, 154, 73)},    // Vert
                {"13", Color.FromArgb(255, 110, 200, 250)}, // Bleu clair
                {"14", Color.FromArgb(255, 110, 0, 110)}    // Violet foncé
            };

            // Regrouper les liens par ligne pour un affichage cohérent
            var liensParLigne = new Dictionary<string, List<(Point, Point)>>();

            foreach (var lien in graphe.LiensNoeuds)
            {
                int idStation1 = lien.Noeud1.Id;
                int idStation2 = lien.Noeud2.Id;

                if (positionsStations.ContainsKey(idStation1) && positionsStations.ContainsKey(idStation2))
                {
                    Point p1 = positionsStations[idStation1];
                    Point p2 = positionsStations[idStation2];

                    // Déterminer la ligne du lien
                    string ligne = "0";
                    if (graphe.Stations.ContainsKey(idStation1))
                    {
                        ligne = graphe.Stations[idStation1].Ligne;
                    }

                    // Ajouter le lien à la liste des liens de cette ligne
                    if (!liensParLigne.ContainsKey(ligne))
                    {
                        liensParLigne[ligne] = new List<(Point, Point)>();
                    }
                    liensParLigne[ligne].Add((p1, p2));
                }
            }

            // Dessiner les liens par ligne
            foreach (var kvp in liensParLigne)
            {
                string ligne = kvp.Key;
                var liens = kvp.Value;

                // Déterminer la couleur de la ligne
                Color couleurLigne = Color.Black;
                if (couleursLignes.ContainsKey(ligne))
                {
                    couleurLigne = couleursLignes[ligne];
                }

                // Utiliser une épaisseur modérée pour les lignes
                using (Pen penLien = new Pen(couleurLigne, 2.5f))
                {
                    // Dessiner tous les liens de cette ligne
                    foreach (var (p1, p2) in liens)
                    {
                        g.DrawLine(penLien, p1, p2);
                    }
                }
            }
        }

        /// <summary>
        /// Dessine les stations sur l'image.
        /// </summary>
        /// <param name="g">Objet Graphics pour dessiner</param>
        private void DessinerStations(Graphics g)
        {
            // Taille des cercles représentant les stations (plus petite)
            int tailleStation = 5;

            // Police pour les noms des stations
            Font fontNom = new Font("Arial", 8, FontStyle.Bold);
            Brush brushNom = Brushes.Black;

            // Mesurer les dimensions des textes pour éviter les chevauchements
            zonesTexte.Clear();

            // Dessiner d'abord les stations sans les noms
            foreach (var station in graphe.Stations.Values)
            {
                if (positionsStations.ContainsKey(station.Id))
                {
                    Point p = positionsStations[station.Id];

                    // Dessiner un cercle blanc avec contour noir pour la station
                    g.FillEllipse(Brushes.White, p.X - tailleStation, p.Y - tailleStation, tailleStation * 2, tailleStation * 2);
                    g.DrawEllipse(Pens.Black, p.X - tailleStation, p.Y - tailleStation, tailleStation * 2, tailleStation * 2);
                }
            }

            // Trier les stations par importance (par exemple, par ligne)
            var stationsTriees = graphe.Stations.Values
                .OrderBy(s => int.TryParse(s.Ligne, out int ligne) ? ligne : 99)
                .ToList();

            // Dessiner les noms des stations en évitant les chevauchements
            foreach (var station in stationsTriees)
            {
                if (positionsStations.ContainsKey(station.Id))
                {
                    Point p = positionsStations[station.Id];

                    // Mesurer la taille du texte
                    SizeF tailleTexte = g.MeasureString(station.Nom, fontNom);

                    // Essayer différentes positions pour le texte avec plus d'espacement
                    bool textePlacé = false;

                    // Positions à essayer (droite, dessous, dessus, gauche) avec plus d'espacement
                    var positions = new[]
                    {
                        new { X = p.X + tailleStation + 3, Y = p.Y - (int)(tailleTexte.Height / 2) },
                        new { X = p.X - (int)(tailleTexte.Width / 2), Y = p.Y + tailleStation + 3 },
                        new { X = p.X - (int)(tailleTexte.Width / 2), Y = p.Y - tailleStation - (int)tailleTexte.Height - 3 },
                        new { X = p.X - tailleStation - (int)tailleTexte.Width - 3, Y = p.Y - (int)(tailleTexte.Height / 2) }
                    };

                    foreach (var pos in positions)
                    {
                        Rectangle zoneTexte = new Rectangle(pos.X, pos.Y, (int)tailleTexte.Width, (int)tailleTexte.Height);

                        // Vérifier s'il y a chevauchement avec d'autres textes déjà placés
                        bool chevauchement = false;
                        foreach (var zone in zonesTexte.Values)
                        {
                            if (zoneTexte.IntersectsWith(zone))
                            {
                                chevauchement = true;
                                break;
                            }
                        }

                        if (!chevauchement)
                        {
                            g.DrawString(station.Nom, fontNom, brushNom, pos.X, pos.Y);
                            zonesTexte[station.Id] = zoneTexte;
                            textePlacé = true;
                            break;
                        }
                    }

                    // Si aucune position ne fonctionne, essayer avec une police plus petite
                    if (!textePlacé)
                    {
                        Font fontPetite = new Font("Arial", 6, FontStyle.Regular);
                        SizeF taillePetiteTexte = g.MeasureString(station.Nom, fontPetite);

                        int xTexte = p.X + tailleStation + 3;
                        int yTexte = p.Y - (int)(taillePetiteTexte.Height / 2);

                        Rectangle zoneTexte = new Rectangle(xTexte, yTexte, (int)taillePetiteTexte.Width, (int)taillePetiteTexte.Height);

                        bool chevauchement = false;
                        foreach (var zone in zonesTexte.Values)
                        {
                            if (zoneTexte.IntersectsWith(zone))
                            {
                                chevauchement = true;
                                break;
                            }
                        }

                        if (!chevauchement)
                        {
                            g.DrawString(station.Nom, fontPetite, brushNom, xTexte, yTexte);
                            zonesTexte[station.Id] = zoneTexte;
                        }
                    }
                }
            }
        }
    }
}*/
#endregion

using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;
using System.Linq;
using System.Drawing.Drawing2D;

// Assurez-vous que ce namespace correspond à celui de vos autres classes
namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Classe améliorée pour visualiser le réseau de métro parisien.
    /// Gère les stations uniques (basées sur le nom), l'espacement des nœuds via "nudging",
    /// et le décalage des lignes parallèles pour une meilleure lisibilité.
    /// </summary>
    public class VisualisationMetroAmelioree
    {
        private MetroGraphe metroGraphe; // Référence au graphe de métro complet
        private int largeur;             // Largeur de l'image de sortie
        private int hauteur;            // Hauteur de l'image de sortie

        // Dictionnaire pour stocker la position calculée pour chaque NOM de station unique
        // La clé est le nom de la station (string, ignorant la casse), la valeur est le point sur l'image (PointF)
        private Dictionary<string, PointF> positionsStationsUniques;

        // Dictionnaire pour mapper l'ID original d'une station (int) à son nom unique (string)
        // Utile pour retrouver le nom unique (et donc la position) à partir d'un ID de lien
        private Dictionary<int, string> mapIdToNomUnique;

        // Dictionnaire pour stocker les rectangles occupés par les noms de stations déjà dessinés
        // La clé est le nom unique (string), la valeur est le rectangle (RectangleF)
        // Permet d'éviter la superposition des textes
        private Dictionary<string, RectangleF> zonesTexte;

        // Nudging (Ajustement des positions pour éviter la superposition des nœuds)
        private float minDistanceNodes = 12.0f; // Distance minimale souhaitée entre les centres des nœuds (pixels)
        private float nudgingFactor = 0.4f;     // Force de la poussée (0.1 à 0.8 recommandé)
        private int nudgingIterations = 100;    // Nombre de passes d'ajustement

        // Dessin des lignes
        private float lineOffsetDistance = 2.0f; // Décalage entre lignes parallèles (pixels)
        private float epaisseurLigne = 8.0f;     // Épaisseur des traits des lignes

        // Dessin des stations
        private float tailleStation = 5.0f;      // Diamètre du cercle représentant une station (pixels)
        private float margeTexteStation = 2.0f;  // Espace entre le cercle de la station et son nom (pixels)
        private Font fontNomStation = new Font("Arial", 14, FontStyle.Regular); // Police pour les noms
        private Font fontNomStationImportante = new Font("Arial", 18, FontStyle.Bold); // Taille de police pour les stations importantes
        private Brush brushNomStation = Brushes.Black;                          // Couleur du texte des noms
        private Brush brushStationFill = Brushes.White;                         // Couleur de remplissage des cercles de station
        private Pen penStationBorder = new Pen(Color.Black, 0.8f);              // Couleur et épaisseur du contour des cercles

        // Marge générale autour du dessin dans l'image
        private int margeImage = 100;

        // Limites géographiques (calculées dynamiquement)
        private double minLongitude = double.MaxValue;
        private double maxLongitude = double.MinValue;
        private double minLatitude = double.MaxValue;
        private double maxLatitude = double.MinValue;

        /// <summary>
        /// Constructeur de la classe VisualisationMetroAmelioree.
        /// Prépare les données et calcule les positions initiales.
        /// </summary>
        /// <param name="metroGraphe">L'objet MetroGraphe contenant les données du réseau.</param>
        /// <param name="largeur">Largeur souhaitée de l'image générée (en pixels).</param>
        /// <param name="hauteur">Hauteur souhaitée de l'image générée (en pixels).</param>
        public VisualisationMetroAmelioree(MetroGraphe metroGraphe, int largeur = 4000, int hauteur = 3000)
        {
            this.metroGraphe = metroGraphe ?? throw new ArgumentNullException(nameof(metroGraphe));
            this.largeur = largeur;
            this.hauteur = hauteur;

            // Initialisation des dictionnaires (ignorant la casse pour les noms de station)
            this.positionsStationsUniques = new Dictionary<string, PointF>(StringComparer.OrdinalIgnoreCase);
            this.mapIdToNomUnique = new Dictionary<int, string>();
            this.zonesTexte = new Dictionary<string, RectangleF>(StringComparer.OrdinalIgnoreCase);

            // Vérification essentielle : le graphe doit contenir des stations
            if (this.metroGraphe.Stations == null || !this.metroGraphe.Stations.Any())
            {
                throw new InvalidOperationException("Le MetroGraphe fourni ne contient aucune station. Assurez-vous que ChargerStations() a été appelé avec succès avant de créer la visualisation.");
            }

            // 1. Préparer les données des stations uniques et calculer les limites géographiques réelles
            PreparerStationsUniquesEtLimitesGeo();

            // 2. Calculer les positions initiales (basées sur la géographie) des stations uniques sur l'image
            CalculerPositionsInitiales();

            // 3. Appliquer l'algorithme de "Nudging" pour espacer les nœuds trop proches
            AppliquerNudging();
        }

        /// <summary>
        /// Regroupe les stations par nom (en ignorant la casse), calcule une coordonnée géographique moyenne pour chaque nom unique,
        /// détermine les limites géographiques globales du réseau, et remplit le dictionnaire `mapIdToNomUnique`.
        /// </summary>
        private void PreparerStationsUniquesEtLimitesGeo()
        {
            Console.WriteLine("Préparation des stations uniques et calcul des limites géographiques...");
            // Utilise la méthode GetStationsByName() ajoutée à MetroGraphe
            var stationsParNom = metroGraphe.GetStationsByName();

            // Réinitialisation des limites géographiques
            minLongitude = double.MaxValue; maxLongitude = double.MinValue;
            minLatitude = double.MaxValue; maxLatitude = double.MinValue;

            foreach (var kvp in stationsParNom)
            {
                string nomUnique = kvp.Key; // Nom de la station (clé du groupe)
                List<Station> stationsGroupees = kvp.Value; // Liste des stations ayant ce nom

                if (stationsGroupees.Any())
                {
                    // Calculer la moyenne des coordonnées pour ce groupe de stations (représentant l'emplacement unique)
                    double avgLat = stationsGroupees.Average(s => s.Latitude);
                    double avgLon = stationsGroupees.Average(s => s.Longitude);

                    // Stocker temporairement les coordonnées géographiques moyennes (Longitude, Latitude)
                    // Ces coordonnées seront converties en pixels plus tard
                    positionsStationsUniques[nomUnique] = new PointF((float)avgLon, (float)avgLat);

                    // Mettre à jour les limites géographiques globales en considérant les coordonnées moyennes
                    minLatitude = Math.Min(minLatitude, avgLat);
                    maxLatitude = Math.Max(maxLatitude, avgLat);
                    minLongitude = Math.Min(minLongitude, avgLon);
                    maxLongitude = Math.Max(maxLongitude, avgLon);

                    // Mapper chaque ID de station original vers le nom unique correspondant
                    foreach (var station in stationsGroupees)
                    {
                        mapIdToNomUnique[station.Id] = nomUnique;
                    }
                }
            }

            // Gestion du cas où toutes les stations sont au même point (ou une seule station)
            // pour éviter une étendue nulle (division par zéro lors de la mise à l'échelle)
            if (Math.Abs(maxLongitude - minLongitude) < 1e-9) { maxLongitude += 0.001; minLongitude -= 0.001; }
            if (Math.Abs(maxLatitude - minLatitude) < 1e-9) { maxLatitude += 0.001; minLatitude -= 0.001; }

            Console.WriteLine($"Préparation terminée : {positionsStationsUniques.Count} stations uniques trouvées.");
            Console.WriteLine($"Limites Géo : Lat [{minLatitude:F4} - {maxLatitude:F4}], Lon [{minLongitude:F4} - {maxLongitude:F4}]");
        }

        /// <summary>
        /// Calcule les positions initiales en pixels des stations uniques sur l'image,
        /// en se basant sur leurs coordonnées géographiques moyennes et les limites globales.
        /// </summary>
        private void CalculerPositionsInitiales()
        {
            Console.WriteLine("Calcul des positions initiales sur l'image...");
            float largeurUtile = largeur - 2 * margeImage;
            float hauteurUtile = hauteur - 2 * margeImage;

            double rangeLon = maxLongitude - minLongitude;
            double rangeLat = maxLatitude - minLatitude;

            // Vérification pour éviter la division par zéro (normalement géré dans PreparerStationsUniquesEtLimitesGeo)
            if (rangeLon <= 0 || rangeLat <= 0)
            {
                Console.WriteLine("Avertissement : Étendue géographique nulle ou invalide. Positionnement au centre.");
                PointF centerPoint = new PointF(largeur / 2f, hauteur / 2f);
                var nomsUniques = positionsStationsUniques.Keys.ToList(); // Nécessite une copie pour modifier le dictionnaire
                foreach (string nomUnique in nomsUniques)
                {
                    positionsStationsUniques[nomUnique] = centerPoint;
                }
                return;
            }

            // Copie des clés pour pouvoir modifier le dictionnaire pendant l'itération
            var nomsUniquesList = positionsStationsUniques.Keys.ToList();
            foreach (string nomUnique in nomsUniquesList)
            {
                PointF coordGeo = positionsStationsUniques[nomUnique]; // Récupère (Longitude, Latitude)
                float lon = coordGeo.X;
                float lat = coordGeo.Y;

                // Mise à l'échelle et transformation en coordonnées pixel
                // Longitude -> X, Latitude -> Y (avec inversion de l'axe Y car l'origine est en haut à gauche)
                float x = margeImage + (float)(largeurUtile * (lon - minLongitude) / rangeLon);
                float y = hauteur - margeImage - (float)(hauteurUtile * (lat - minLatitude) / rangeLat);

                // Mettre à jour le dictionnaire avec les coordonnées finales en pixels
                positionsStationsUniques[nomUnique] = new PointF(x, y);
            }
            Console.WriteLine("Calcul des positions initiales terminé.");
        }

        /// <summary>
        /// Applique un algorithme simple de "nudging" (poussée/répulsion)
        /// pour espacer les nœuds (stations uniques) qui sont trop proches les uns des autres
        /// après le positionnement géographique initial.
        /// </summary>
        private void AppliquerNudging()
        {
            Console.WriteLine($"Application du Nudging ({nudgingIterations} itérations, distance min: {minDistanceNodes}px)...");
            var nomsStations = positionsStationsUniques.Keys.ToList();
            int margeSecurite = 10; // Empêche les nœuds d'être poussés complètement hors de l'image

            for (int iter = 0; iter < nudgingIterations; iter++)
            {
                // Itérer sur toutes les paires uniques de stations
                for (int i = 0; i < nomsStations.Count; i++)
                {
                    for (int j = i + 1; j < nomsStations.Count; j++)
                    {
                        string nom1 = nomsStations[i];
                        string nom2 = nomsStations[j];

                        PointF p1 = positionsStationsUniques[nom1];
                        PointF p2 = positionsStationsUniques[nom2];

                        float dx = p1.X - p2.X;
                        float dy = p1.Y - p2.Y;
                        float distanceSquared = dx * dx + dy * dy;

                        // Si les points sont distincts et trop proches
                        if (distanceSquared > 1e-6f && distanceSquared < minDistanceNodes * minDistanceNodes)
                        {
                            float distance = (float)Math.Sqrt(distanceSquared);
                            // Éviter une force de répulsion excessive si les points sont quasi superposés
                            if (distance < 0.1f) distance = 0.1f;

                            // Calculer la force de répulsion (proportionnelle à l'empiètement)
                            float force = nudgingFactor * (minDistanceNodes - distance) / distance;

                            // Calculer le vecteur de déplacement (normalisé et pondéré par la force)
                            // On divise par 2 car chaque point bouge de la moitié de la distance nécessaire
                            float moveX = (dx / distance) * force * 0.5f;
                            float moveY = (dy / distance) * force * 0.5f;

                            // Appliquer le déplacement aux deux points, en les maintenant dans les limites de l'image
                            positionsStationsUniques[nom1] = new PointF(
                                Math.Max(margeSecurite, Math.Min(largeur - margeSecurite, p1.X + moveX)),
                                Math.Max(margeSecurite, Math.Min(hauteur - margeSecurite, p1.Y + moveY))
                            );
                            positionsStationsUniques[nom2] = new PointF(
                                Math.Max(margeSecurite, Math.Min(largeur - margeSecurite, p2.X - moveX)),
                                Math.Max(margeSecurite, Math.Min(hauteur - margeSecurite, p2.Y - moveY))
                            );
                        }
                    }
                }
            }
            Console.WriteLine("Nudging terminé.");
        }

        /// <summary>
        /// Méthode principale pour dessiner le graphe complet (liens puis stations)
        /// sur une image et la sauvegarder dans un fichier.
        /// </summary>
        /// <param name="cheminFichier">Chemin complet du fichier image PNG à créer.</param>
        public void DessinerMetro(string cheminFichier)
        {
            Console.WriteLine($"Début du dessin de l'image ({largeur}x{hauteur})...");
            using (Bitmap bitmap = new Bitmap(largeur, hauteur))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // Configuration de la qualité du rendu
                g.SmoothingMode = SmoothingMode.AntiAlias; // Lisser les lignes et cercles
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit; // Améliorer le rendu du texte

                // Remplir le fond en blanc
                g.Clear(Color.White);

                // 1. Dessiner les liens entre les stations
                DessinerLiens(g);

                // 2. Dessiner les nœuds des stations (cercles) et leurs noms
                DessinerStationsUniques(g);

                // 3. Sauvegarder l'image résultante
                try
                {
                    bitmap.Save(cheminFichier, ImageFormat.Png);
                    Console.WriteLine($"Plan du métro amélioré sauvegardé sous '{cheminFichier}'.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de la sauvegarde de l'image '{cheminFichier}': {ex.Message}");
                    // Tenter de sauvegarder dans un emplacement par défaut ?
                    try
                    {
                        string fallbackPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "metro_fallback.png");
                        bitmap.Save(fallbackPath, ImageFormat.Png);
                        Console.WriteLine($"Image sauvegardée dans un emplacement de secours: '{fallbackPath}'");
                    }
                    catch (Exception fallbackEx)
                    {
                        Console.WriteLine($"Échec de la sauvegarde de secours: {fallbackEx.Message}");
                    }
                }
            }

            // Libération explicite des ressources GDI (polices, pinceaux, stylos) si nécessaire
            // (bien que 'using' devrait le faire pour les objets créés localement)
            fontNomStation.Dispose();
            penStationBorder.Dispose();
        }

        /// <summary>
        /// Dessine les liens (connexions) entre les stations uniques.
        /// Applique un décalage pour les segments de ligne partagés par plusieurs lignes de métro.
        /// </summary>
        /// <param name="g">L'objet Graphics sur lequel dessiner.</param>
        private void DessinerLiens(Graphics g)
        {
            Console.WriteLine("Dessin des liens...");
            Dictionary<string, Color> couleursLignes = GetCouleursLignes();

            // Structure pour regrouper les lignes par paire de stations *uniques* qu'elles connectent
            // Clé: Tuple (nomStation1, nomStation2) trié par ordre alphabétique pour l'unicité
            // Valeur: Liste des numéros de ligne (string) qui connectent ces deux stations
            var liensEntreStations = new Dictionary<Tuple<string, string>, List<string>>();

            // Itérer sur les liens fournis par MetroGraphe (hérités de Graphe<int>)
            // Utilise la propriété publique LiensNoeuds
            foreach (var lien in metroGraphe.LiensNoeuds)
            {
                // Récupérer les IDs des stations connectées par ce lien
                int idStation1 = lien.Noeud1.Id;
                int idStation2 = lien.Noeud2.Id;

                // Trouver les noms uniques correspondants via le mapping créé précédemment
                if (mapIdToNomUnique.TryGetValue(idStation1, out string nom1) &&
                    mapIdToNomUnique.TryGetValue(idStation2, out string nom2))
                {
                    // Ignorer les liens connectant une station à elle-même (normalement pas possible, mais par sécurité)
                    if (nom1.Equals(nom2, StringComparison.OrdinalIgnoreCase)) continue;

                    // Créer une clé unique pour la paire de noms (indépendante de l'ordre)
                    Tuple<string, string> key = nom1.CompareTo(nom2) < 0 ? Tuple.Create(nom1, nom2) : Tuple.Create(nom2, nom1);

                    // Déterminer la ligne de ce segment. On prend celle de la station 1 par convention.
                    // (Hypothèse: un lien direct entre deux stations appartient à une seule ligne)
                    string ligne = "Inconnue"; // Valeur par défaut si la station n'est pas trouvée (ne devrait pas arriver)
                    if (metroGraphe.Stations.TryGetValue(idStation1, out Station station1))
                    {
                        ligne = station1.Ligne ?? "Inconnue"; // Utilise l'opérateur null-coalescing
                    }

                    // Ajouter ce segment à notre structure de regroupement
                    if (!liensEntreStations.ContainsKey(key))
                    {
                        liensEntreStations[key] = new List<string>();
                    }
                    // Ajouter la ligne seulement si elle n'est pas déjà dans la liste pour cette paire
                    // (évite les doublons si A->B et B->A existent pour la même ligne dans les données brutes)
                    if (!liensEntreStations[key].Contains(ligne))
                    {
                        liensEntreStations[key].Add(ligne);
                    }
                }
                // Optionnel : Log si un ID de lien ne correspond à aucune station mappée
                // else { Console.WriteLine($"Avertissement: IDs de lien ({idStation1}, {idStation2}) non trouvés dans mapIdToNomUnique."); }
            }

            // Maintenant, dessiner les liens regroupés, en appliquant le décalage
            foreach (var kvp in liensEntreStations)
            {
                string nomStationA = kvp.Key.Item1;
                string nomStationB = kvp.Key.Item2;
                // Trier les lignes pour un ordre de décalage cohérent
                List<string> lignesConnectant = kvp.Value.Distinct().OrderBy(l => l, StringComparer.Ordinal).ToList();

                // Récupérer les positions en pixels des deux stations uniques
                if (positionsStationsUniques.TryGetValue(nomStationA, out PointF pA) &&
                    positionsStationsUniques.TryGetValue(nomStationB, out PointF pB))
                {
                    int nbLignes = lignesConnectant.Count;
                    if (nbLignes == 0) continue; // Aucune ligne valide pour cette paire

                    // Calculer le vecteur directeur et sa longueur
                    float dx = pB.X - pA.X;
                    float dy = pB.Y - pA.Y;
                    float length = (float)Math.Sqrt(dx * dx + dy * dy);

                    // Si les points sont (presque) superposés, ne pas dessiner de ligne
                    if (length < 0.1f) continue;

                    // Calculer le vecteur perpendiculaire normalisé (pour le décalage)
                    float perpX = -dy / length;
                    float perpY = dx / length;

                    // Calculer le décalage total nécessaire et le point de départ du décalage
                    float totalOffsetWidth = (nbLignes - 1) * lineOffsetDistance;
                    float startOffset = -totalOffsetWidth / 2.0f;

                    // Dessiner chaque ligne avec son décalage
                    for (int i = 0; i < nbLignes; i++)
                    {
                        string ligne = lignesConnectant[i];
                        Color couleur = couleursLignes.TryGetValue(ligne, out Color c) ? c : Color.DarkGray; // Couleur par défaut

                        // Calculer le décalage spécifique pour cette i-ème ligne
                        // Pas de décalage si une seule ligne partage ce segment
                        float currentOffset = (nbLignes == 1) ? 0 : startOffset + i * lineOffsetDistance;

                        // Calculer les points de départ et d'arrivée décalés
                        PointF pA_offset = new PointF(pA.X + perpX * currentOffset, pA.Y + perpY * currentOffset);
                        PointF pB_offset = new PointF(pB.X + perpX * currentOffset, pB.Y + perpY * currentOffset);

                        // Créer le stylo avec la bonne couleur et épaisseur
                        using (Pen penLien = new Pen(couleur, epaisseurLigne))
                        {
                            // Ajouter des bouts arrondis pour un meilleur rendu
                            penLien.StartCap = LineCap.Round;
                            penLien.EndCap = LineCap.Round;
                            // Dessiner la ligne décalée
                            g.DrawLine(penLien, pA_offset, pB_offset);
                        }
                    }
                }
            }
            Console.WriteLine("Dessin des liens terminé.");
        }

        private void DessinerStationsUniques(Graphics g)
        {
            Console.WriteLine($"Dessin des {positionsStationsUniques.Count} stations uniques et de leurs noms...");

            // Vider le cache des zones de texte utilisées
            zonesTexte.Clear();

            // Trier les noms pour un ordre de dessin potentiellement plus stable (optionnel)
            var nomsTries = positionsStationsUniques.Keys.OrderBy(nom => nom).ToList();

            // Itérer sur chaque station unique pour la dessiner
            foreach (string nomUnique in nomsTries)
            {
                if (positionsStationsUniques.TryGetValue(nomUnique, out PointF p))
                {
                    // Déterminer la taille de la station en fonction de son importance
                    float tailleStationActuelle = EstStationImportante(nomUnique) ? 15.0f : 5.0f; // 15 pour les importantes, 5 pour les autres
                    float rayon = tailleStationActuelle / 2.0f;

                    // 1. Dessiner le cercle représentant la station
                    g.FillEllipse(brushStationFill, p.X - rayon, p.Y - rayon, tailleStationActuelle, tailleStationActuelle);
                    g.DrawEllipse(penStationBorder, p.X - rayon, p.Y - rayon, tailleStationActuelle, tailleStationActuelle);

                    // 2. Tenter de placer le nom de la station à côté
                    Font policeUtilisee = EstStationImportante(nomUnique) ? fontNomStationImportante : fontNomStation; // Choisir la police en fonction de l'importance
                    SizeF tailleTexte = g.MeasureString(nomUnique, policeUtilisee);
                    float espace = rayon + margeTexteStation; // Espace entre bord du cercle et début du texte

                    // Définir les positions candidates autour de la station (droite, dessous, dessus, gauche)
                    var positionsCandidates = new[]
                    {
                        new PointF(p.X + espace, p.Y - tailleTexte.Height / 2), // Droite (centré verticalement)
                        new PointF(p.X - tailleTexte.Width / 2, p.Y + espace), // Dessous (centré horizontalement)
                        new PointF(p.X - tailleTexte.Width / 2, p.Y - espace - tailleTexte.Height), // Dessus (centré horizontalement)
                        new PointF(p.X - espace - tailleTexte.Width, p.Y - tailleTexte.Height / 2) // Gauche (centré verticalement)
                    };

                    bool textePlace = false;
                    // Essayer chaque position candidate
                    foreach (var posCandidate in positionsCandidates)
                    {
                        // Calculer le rectangle occupé par le texte à cette position
                        RectangleF zoneTexteCourante = new RectangleF(posCandidate, tailleTexte);
                        // Ajouter une petite marge pour éviter que les textes se touchent visuellement
                        zoneTexteCourante.Inflate(1.5f, 1.5f);

                        // Vérifier si le texte sortirait de l'image
                        bool dansImage = zoneTexteCourante.Left >= 0 && zoneTexteCourante.Top >= 0 &&
                                         zoneTexteCourante.Right <= largeur && zoneTexteCourante.Bottom <= hauteur;

                        if (dansImage)
                        {
                            // Vérifier s'il y a chevauchement avec les textes déjà placés
                            bool chevauchement = zonesTexte.Values.Any(zoneExistante => zoneExistante.IntersectsWith(zoneTexteCourante));

                            // Si pas de chevauchement et dans l'image, c'est une position valide
                            if (!chevauchement)
                            {
                                g.DrawString(nomUnique, policeUtilisee, brushNomStation, posCandidate);
                                // Enregistrer la zone occupée par ce texte
                                zonesTexte[nomUnique] = zoneTexteCourante;
                                textePlace = true;
                                break; // Sortir de la boucle des positions candidates dès qu'une place est trouvée
                            }
                        }
                    }

                }
            }
            Console.WriteLine("Dessin des stations et noms terminé.");
        }

        /// <summary>
        /// Retourne un dictionnaire associant les numéros de ligne (string) à leurs couleurs officielles (Color).
        /// Peut être étendue pour inclure RER, Tramways, etc.
        /// </summary>
        private Dictionary<string, Color> GetCouleursLignes()
        {
            // S'assurer d'avoir toutes les variations possibles de noms de ligne (e.g., "3bis")
            return new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase) // Ignorer la casse pour les clés
            {
                {"1", Color.FromArgb(255, 206, 0)},    // Jaune RATP
                {"2", Color.FromArgb(0, 100, 180)},    // Bleu RATP
                {"3", Color.FromArgb(152, 199, 0)},    // Vert Olive RATP
                {"3b", Color.FromArgb(148, 218, 209)}, // Vert d'eau RATP (3bis)
                {"3bis", Color.FromArgb(148, 218, 209)}, // Vert d'eau RATP (3bis)
                {"4", Color.FromArgb(187, 76, 158)},   // Violet RATP
                {"5", Color.FromArgb(255, 137, 0)},    // Orange RATP
                {"6", Color.FromArgb(118, 184, 139)},  // Vert Clair/Cyan RATP
                {"7", Color.FromArgb(255, 160, 173)},  // Rose RATP
                {"7b", Color.FromArgb(148, 218, 209)}, // Vert d'eau RATP (7bis)
                {"7bis", Color.FromArgb(148, 218, 209)}, // Vert d'eau RATP (7bis)
                {"8", Color.FromArgb(206, 152, 218)},  // Lilas RATP
                {"9", Color.FromArgb(220, 190, 0)},    // Jaune-Or RATP
                {"10", Color.FromArgb(199, 174, 76)},  // Or vieilli RATP
                {"11", Color.FromArgb(139, 87, 42)},   // Marron RATP
                {"12", Color.FromArgb(0, 128, 51)},    // Vert Foncé RATP
                {"13", Color.FromArgb(110, 200, 250)}, // Bleu Ciel RATP
                {"14", Color.FromArgb(97, 33, 120)},   // Violet Foncé RATP

                // Couleurs RER (exemples)
                {"A", Color.FromArgb(227, 6, 19)},     // Rouge RER A
                {"B", Color.FromArgb(73, 169, 221)},   // Bleu RER B
                {"C", Color.FromArgb(255, 194, 14)},   // Jaune RER C
                {"D", Color.FromArgb(0, 171, 73)},     // Vert RER D
                {"E", Color.FromArgb(223, 105, 169)},  // Rose RER E

                // Autres cas
                {"Inconnue", Color.DarkGray},          // Ligne non trouvée dans les données
                {"", Color.LightGray},                 // Ligne vide
                {"X", Color.Transparent}               // Cas spécial ('X' dans les arcs CSV ?)
            };
        }

        private bool EstStationImportante(string nomStation)
        {
            // Liste des stations importantes
            var stationsImportantes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Chatelet", "Saint-Lazare", "Gare de Lyon", "Republique", "Montparnasse Bienvenue"
            };

            // Log pour débogage

            return stationsImportantes.Contains(nomStation);
        }
    }
}