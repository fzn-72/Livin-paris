using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;
using System.Linq;
using System.Drawing.Drawing2D;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Classe améliorée pour visualiser le réseau de métro parisien.
    /// Gère les stations uniques (basées sur le nom), l'espacement des nœuds via "nudging",
    /// et le décalage des lignes parallèles pour une meilleure lisibilité.
    /// </summary>
    public class VisualisationMetroAmelioree
    {
        private MetroGraphe metroGraphe;
        private int largeur;
        private int hauteur;
        private Dictionary<string, PointF> positionsStationsUniques;
        private Dictionary<int, string> mapIdToNomUnique;
        private Dictionary<string, RectangleF> zonesTexte;
        private float minDistanceNodes = 12.0f;
        private float nudgingFactor = 0.4f;
        private int nudgingIterations = 100;
        private float lineOffsetDistance = 2.0f;
        private float epaisseurLigne = 8.0f;
        private float tailleStation = 5.0f;
        private float margeTexteStation = 2.0f;
        private Font fontNomStation = new Font("Arial", 14, FontStyle.Regular);
        private Font fontNomStationImportante = new Font("Arial", 18, FontStyle.Bold);
        private Brush brushNomStation = Brushes.Black;
        private Brush brushStationFill = Brushes.White;
        private Pen penStationBorder = new Pen(Color.Black, 0.8f);
        private int margeImage = 100;
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
            this.positionsStationsUniques = new Dictionary<string, PointF>(StringComparer.OrdinalIgnoreCase);
            this.mapIdToNomUnique = new Dictionary<int, string>();
            this.zonesTexte = new Dictionary<string, RectangleF>(StringComparer.OrdinalIgnoreCase);
            if (this.metroGraphe.Stations == null || !this.metroGraphe.Stations.Any())
            {
                throw new InvalidOperationException("Le MetroGraphe fourni ne contient aucune station. Assurez-vous que ChargerStations() a été appelé avec succès avant de créer la visualisation.");
            }
            PreparerStationsUniquesEtLimitesGeo();
            CalculerPositionsInitiales();
            AppliquerNudging();
        }

        /// <summary>
        /// Regroupe les stations par nom (en ignorant la casse), calcule une coordonnée géographique moyenne pour chaque nom unique,
        /// détermine les limites géographiques globales du réseau, et remplit le dictionnaire `mapIdToNomUnique`.
        /// </summary>
        private void PreparerStationsUniquesEtLimitesGeo()
        {
            Console.WriteLine("Préparation des stations uniques et calcul des limites géographiques...");
            var stationsParNom = metroGraphe.GetStationsByName();
            minLongitude = double.MaxValue; maxLongitude = double.MinValue;
            minLatitude = double.MaxValue; maxLatitude = double.MinValue;
            foreach (var kvp in stationsParNom)
            {
                string nomUnique = kvp.Key;
                List<Station> stationsGroupees = kvp.Value;
                if (stationsGroupees.Any())
                {
                    double avgLat = stationsGroupees.Average(s => s.Latitude);
                    double avgLon = stationsGroupees.Average(s => s.Longitude);
                    positionsStationsUniques[nomUnique] = new PointF((float)avgLon, (float)avgLat);
                    minLatitude = Math.Min(minLatitude, avgLat);
                    maxLatitude = Math.Max(maxLatitude, avgLat);
                    minLongitude = Math.Min(minLongitude, avgLon);
                    maxLongitude = Math.Max(maxLongitude, avgLon);
                    foreach (var station in stationsGroupees)
                    {
                        mapIdToNomUnique[station.Id] = nomUnique;
                    }
                }
            }
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
            if (rangeLon <= 0 || rangeLat <= 0)
            {
                Console.WriteLine("Avertissement : Étendue géographique nulle ou invalide. Positionnement au centre.");
                PointF centerPoint = new PointF(largeur / 2f, hauteur / 2f);
                var nomsUniques = positionsStationsUniques.Keys.ToList();
                foreach (string nomUnique in nomsUniques)
                {
                    positionsStationsUniques[nomUnique] = centerPoint;
                }
                return;
            }
            var nomsUniquesList = positionsStationsUniques.Keys.ToList();
            foreach (string nomUnique in nomsUniquesList)
            {
                PointF coordGeo = positionsStationsUniques[nomUnique];
                float lon = coordGeo.X;
                float lat = coordGeo.Y;
                float x = margeImage + (float)(largeurUtile * (lon - minLongitude) / rangeLon);
                float y = hauteur - margeImage - (float)(hauteurUtile * (lat - minLatitude) / rangeLat);
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
            int margeSecurite = 10;
            for (int iter = 0; iter < nudgingIterations; iter++)
            {
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
                        if (distanceSquared > 1e-6f && distanceSquared < minDistanceNodes * minDistanceNodes)
                        {
                            float distance = (float)Math.Sqrt(distanceSquared);
                            if (distance < 0.1f) distance = 0.1f;
                            float force = nudgingFactor * (minDistanceNodes - distance) / distance;
                            float moveX = (dx / distance) * force * 0.5f;
                            float moveY = (dy / distance) * force * 0.5f;
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
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                g.Clear(Color.White);
                DessinerLiens(g);
                DessinerStationsUniques(g);
                try
                {
                    bitmap.Save(cheminFichier, ImageFormat.Png);
                    Console.WriteLine($"Plan du métro amélioré sauvegardé sous '{cheminFichier}'.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de la sauvegarde de l'image '{cheminFichier}': {ex.Message}");
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
            var liensEntreStations = new Dictionary<Tuple<string, string>, List<string>>();
            foreach (var lien in metroGraphe.LiensNoeuds)
            {
                int idStation1 = lien.Noeud1.Id;
                int idStation2 = lien.Noeud2.Id;
                if (mapIdToNomUnique.TryGetValue(idStation1, out string nom1) &&
                    mapIdToNomUnique.TryGetValue(idStation2, out string nom2))
                {
                    if (nom1.Equals(nom2, StringComparison.OrdinalIgnoreCase)) continue;
                    Tuple<string, string> key = nom1.CompareTo(nom2) < 0 ? Tuple.Create(nom1, nom2) : Tuple.Create(nom2, nom1);
                    string ligne = "Inconnue";
                    if (metroGraphe.Stations.TryGetValue(idStation1, out Station station1))
                    {
                        ligne = station1.Ligne ?? "Inconnue";
                    }
                    if (!liensEntreStations.ContainsKey(key))
                    {
                        liensEntreStations[key] = new List<string>();
                    }
                    if (!liensEntreStations[key].Contains(ligne))
                    {
                        liensEntreStations[key].Add(ligne);
                    }
                }
            }
            foreach (var kvp in liensEntreStations)
            {
                string nomStationA = kvp.Key.Item1;
                string nomStationB = kvp.Key.Item2;
                List<string> lignesConnectant = kvp.Value.Distinct().OrderBy(l => l, StringComparer.Ordinal).ToList();
                if (positionsStationsUniques.TryGetValue(nomStationA, out PointF pA) &&
                    positionsStationsUniques.TryGetValue(nomStationB, out PointF pB))
                {
                    int nbLignes = lignesConnectant.Count;
                    if (nbLignes == 0) continue;
                    float dx = pB.X - pA.X;
                    float dy = pB.Y - pA.Y;
                    float length = (float)Math.Sqrt(dx * dx + dy * dy);
                    if (length < 0.1f) continue;
                    float perpX = -dy / length;
                    float perpY = dx / length;
                    float totalOffsetWidth = (nbLignes - 1) * lineOffsetDistance;
                    float startOffset = -totalOffsetWidth / 2.0f;
                    for (int i = 0; i < nbLignes; i++)
                    {
                        string ligne = lignesConnectant[i];
                        Color couleur = couleursLignes.TryGetValue(ligne, out Color c) ? c : Color.DarkGray;
                        float currentOffset = (nbLignes == 1) ? 0 : startOffset + i * lineOffsetDistance;
                        PointF pA_offset = new PointF(pA.X + perpX * currentOffset, pA.Y + perpY * currentOffset);
                        PointF pB_offset = new PointF(pB.X + perpX * currentOffset, pB.Y + perpY * currentOffset);
                        using (Pen penLien = new Pen(couleur, epaisseurLigne))
                        {
                            penLien.StartCap = LineCap.Round;
                            penLien.EndCap = LineCap.Round;
                            g.DrawLine(penLien, pA_offset, pB_offset);
                        }
                    }
                }
            }
            Console.WriteLine("Dessin des liens terminé.");
        }

        /// <summary>
        /// Dessine les stations uniques et leurs noms sur l'image.
        /// </summary>
        /// <param name="g">L'objet Graphics sur lequel dessiner.</param>
        private void DessinerStationsUniques(Graphics g)
        {
            Console.WriteLine($"Dessin des {positionsStationsUniques.Count} stations uniques et de leurs noms...");
            zonesTexte.Clear();
            var nomsTries = positionsStationsUniques.Keys.OrderBy(nom => nom).ToList();
            foreach (string nomUnique in nomsTries)
            {
                if (positionsStationsUniques.TryGetValue(nomUnique, out PointF p))
                {
                    float tailleStationActuelle = EstStationImportante(nomUnique) ? 15.0f : 5.0f;
                    float rayon = tailleStationActuelle / 2.0f;
                    g.FillEllipse(brushStationFill, p.X - rayon, p.Y - rayon, tailleStationActuelle, tailleStationActuelle);
                    g.DrawEllipse(penStationBorder, p.X - rayon, p.Y - rayon, tailleStationActuelle, tailleStationActuelle);
                    Font policeUtilisee = EstStationImportante(nomUnique) ? fontNomStationImportante : fontNomStation;
                    SizeF tailleTexte = g.MeasureString(nomUnique, policeUtilisee);
                    float espace = rayon + margeTexteStation;
                    var positionsCandidates = new[]
                    {
                        new PointF(p.X + espace, p.Y - tailleTexte.Height / 2),
                        new PointF(p.X - tailleTexte.Width / 2, p.Y + espace),
                        new PointF(p.X - tailleTexte.Width / 2, p.Y - espace - tailleTexte.Height),
                        new PointF(p.X - espace - tailleTexte.Width, p.Y - tailleTexte.Height / 2)
                    };
                    bool textePlace = false;
                    foreach (var posCandidate in positionsCandidates)
                    {
                        RectangleF zoneTexteCourante = new RectangleF(posCandidate, tailleTexte);
                        zoneTexteCourante.Inflate(1.5f, 1.5f);
                        bool dansImage = zoneTexteCourante.Left >= 0 && zoneTexteCourante.Top >= 0 &&
                                         zoneTexteCourante.Right <= largeur && zoneTexteCourante.Bottom <= hauteur;
                        if (dansImage)
                        {
                            bool chevauchement = zonesTexte.Values.Any(zoneExistante => zoneExistante.IntersectsWith(zoneTexteCourante));
                            if (!chevauchement)
                            {
                                g.DrawString(nomUnique, policeUtilisee, brushNomStation, posCandidate);
                                zonesTexte[nomUnique] = zoneTexteCourante;
                                textePlace = true;
                                break;
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
            return new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase)
            {
                {"1", Color.FromArgb(255, 206, 0)},
                {"2", Color.FromArgb(0, 100, 180)},
                {"3", Color.FromArgb(152, 199, 0)},
                {"3b", Color.FromArgb(148, 218, 209)},
                {"3bis", Color.FromArgb(148, 218, 209)},
                {"4", Color.FromArgb(187, 76, 158)},
                {"5", Color.FromArgb(255, 137, 0)},
                {"6", Color.FromArgb(118, 184, 139)},
                {"7", Color.FromArgb(255, 160, 173)},
                {"7b", Color.FromArgb(148, 218, 209)},
                {"7bis", Color.FromArgb(148, 218, 209)},
                {"8", Color.FromArgb(206, 152, 218)},
                {"9", Color.FromArgb(220, 190, 0)},
                {"10", Color.FromArgb(199, 174, 76)},
                {"11", Color.FromArgb(139, 87, 42)},
                {"12", Color.FromArgb(0, 128, 51)},
                {"13", Color.FromArgb(110, 200, 250)},
                {"14", Color.FromArgb(97, 33, 120)},
                {"A", Color.FromArgb(227, 6, 19)},
                {"B", Color.FromArgb(73, 169, 221)},
                {"C", Color.FromArgb(255, 194, 14)},
                {"D", Color.FromArgb(0, 171, 73)},
                {"E", Color.FromArgb(223, 105, 169)},
                {"Inconnue", Color.DarkGray},
                {"", Color.LightGray},
                {"X", Color.Transparent}
            };
        }

        /// <summary>
        /// Détermine si une station est importante.
        /// </summary>
        /// <param name="nomStation">Le nom de la station à vérifier.</param>
        private bool EstStationImportante(string nomStation)
        {
            var stationsImportantes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Chatelet", "Saint-Lazare", "Gare de Lyon", "Republique", "Montparnasse Bienvenue"
            };
            return stationsImportantes.Contains(nomStation);
        }
    }
}
