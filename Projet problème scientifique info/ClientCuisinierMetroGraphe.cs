using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using MySql.Data.MySqlClient;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Classe pour visualiser les relations entre clients et cuisiniers
    /// basées sur leurs stations de métro les plus proches
    /// </summary>
    public class ClientCuisinierMetroGraphe
    {
        //private string connectionString = "Server=localhost;Database=livinparis2;Uid=root;Pwd=REDACTED;";
        private static string connectionString = "Server=localhost;Database=livinparis2;Uid=root;Pwd=REDACTED;";

        private Dictionary<string, StationInfo> stations; 
        private Dictionary<string, List<PersonneInfo>> personnesParStation;
        private List<RelationInfo> relations; 

        private int largeur = 4000;
        private int hauteur = 3000;
        private int margeImage = 100;
        private float tailleStation = 10.0f;
        private float epaisseurLigne = 2.0f;
        private Font fontNomStation = new Font("Arial", 14, FontStyle.Bold);
        private Font fontNomPersonne = new Font("Arial", 12, FontStyle.Regular);
        private Color couleurClient = Color.Blue;
        private Color couleurCuisinier = Color.Red;
        private Color couleurStation = Color.Black;
        private Color couleurRelation = Color.Green;

        /// Limites géographiques
        private double minLongitude = double.MaxValue;
        private double maxLongitude = double.MinValue;
        private double minLatitude = double.MaxValue;
        private double maxLatitude = double.MinValue;

        /// <summary>
        /// Constructeur
        /// </summary>
        public ClientCuisinierMetroGraphe()
        {
            stations = new Dictionary<string, StationInfo>(StringComparer.OrdinalIgnoreCase);
            personnesParStation = new Dictionary<string, List<PersonneInfo>>(StringComparer.OrdinalIgnoreCase);
            relations = new List<RelationInfo>();
        }

        /// <summary>
        /// Charge les données des stations de métro, clients, cuisiniers et leurs relations
        /// </summary>
        /// <returns>True si les données ont été chargées avec succès</returns>
        public bool ChargerDonnees()
        {
            try
            {
                Console.WriteLine("Chargement des données pour le graphe métro clients-cuisiniers...");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    /// 1. Charger les stations de métro avec leurs coordonnées
                    ChargerStationsMetro(connection);

                    /// 2. Charger les clients et leurs stations de métro
                    ChargerClients(connection);

                    /// 3. Charger les cuisiniers et leurs stations de métro
                    ChargerCuisiniers(connection);

                    /// 4. Charger les relations entre clients et cuisiniers (commandes)
                    ChargerRelations(connection);
                }

                Console.WriteLine($"Données chargées avec succès: {stations.Count} stations, {personnesParStation.Values.Sum(l => l.Count)} personnes, {relations.Count} relations");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des données: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Détails: {ex.InnerException.Message}");
                }
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Charge les stations de métro depuis la base de données
        /// </summary>
        private void ChargerStationsMetro(MySqlConnection connection)
        {
            Console.WriteLine("Chargement des stations de métro...");

            /// Requête pour récupérer les stations de métro uniques à partir des tables CLIENT et CUISINIER
            string requete = @"
        SELECT DISTINCT metro_le_plus_proche_client AS nom_station
        FROM CLIENT
        WHERE metro_le_plus_proche_client IS NOT NULL AND metro_le_plus_proche_client != ''
        UNION
        SELECT DISTINCT metro_le_plus_proche_cuisinier AS nom_station
        FROM CUISINIER
        WHERE metro_le_plus_proche_cuisinier IS NOT NULL AND metro_le_plus_proche_cuisinier != ''";

            MySqlCommand cmd = new MySqlCommand(requete, connection);

            /// Dictionnaire de coordonnées GPS pour les principales stations de métro parisiennes
            Dictionary<string, (double lat, double lon)> coordonneesStations = new Dictionary<string, (double, double)>(StringComparer.OrdinalIgnoreCase);

            /// Ajouter les coordonnées des stations
            void AjouterCoordonnees(string nom, double lat, double lon)
            {
                if (!coordonneesStations.ContainsKey(nom))
                {
                    coordonneesStations[nom] = (lat, lon);
                }
            }

            /// Ligne 1
            AjouterCoordonnees("Château de Vincennes", 48.8448, 2.4372);
            AjouterCoordonnees("Bérault", 48.8456, 2.4287);
            AjouterCoordonnees("Saint-Mandé", 48.8469, 2.4187);
            AjouterCoordonnees("Porte de Vincennes", 48.8478, 2.4086);
            AjouterCoordonnees("Nation", 48.8483, 2.3962);
            AjouterCoordonnees("Reuilly-Diderot", 48.8472, 2.3854);
            AjouterCoordonnees("Gare de Lyon", 48.8448, 2.3735);
            AjouterCoordonnees("Bastille", 48.8531, 2.3692);
            AjouterCoordonnees("Saint-Paul", 48.8554, 2.3607);
            AjouterCoordonnees("Hôtel de Ville", 48.8570, 2.3508);
            AjouterCoordonnees("Châtelet", 48.8591, 2.3462);
            AjouterCoordonnees("Louvre-Rivoli", 48.8606, 2.3402);
            AjouterCoordonnees("Palais Royal-Musée du Louvre", 48.8620, 2.3366);
            AjouterCoordonnees("Tuileries", 48.8642, 2.3302);
            AjouterCoordonnees("Concorde", 48.8656, 2.3242);
            AjouterCoordonnees("Champs-Élysées-Clemenceau", 48.8680, 2.3131);
            AjouterCoordonnees("Franklin D. Roosevelt", 48.8697, 2.3094);
            AjouterCoordonnees("George V", 48.8728, 2.3007);
            AjouterCoordonnees("Charles de Gaulle-Étoile", 48.8742, 2.2950);
            AjouterCoordonnees("Argentine", 48.8757, 2.2901);
            AjouterCoordonnees("Porte Maillot", 48.8775, 2.2833);
            AjouterCoordonnees("Les Sablons", 48.8811, 2.2722);
            AjouterCoordonnees("Pont de Neuilly", 48.8844, 2.2609);
            AjouterCoordonnees("Esplanade de La Défense", 48.8883, 2.2506);
            AjouterCoordonnees("La Défense", 48.8918, 2.2387);

            /// Ligne 2
            AjouterCoordonnees("Avron", 48.8516, 2.3982);
            AjouterCoordonnees("Alexandre Dumas", 48.8563, 2.3939);
            AjouterCoordonnees("Philippe Auguste", 48.8583, 2.3892);
            AjouterCoordonnees("Père Lachaise", 48.8613, 2.3881);
            AjouterCoordonnees("Ménilmontant", 48.8665, 2.3843);
            AjouterCoordonnees("Couronnes", 48.8695, 2.3801);
            AjouterCoordonnees("Belleville", 48.8717, 2.3771);
            AjouterCoordonnees("Colonel Fabien", 48.8772, 2.3704);
            AjouterCoordonnees("Jaurès", 48.8819, 2.3701);
            AjouterCoordonnees("Stalingrad", 48.8841, 2.3658);
            AjouterCoordonnees("La Chapelle", 48.8845, 2.3602);
            AjouterCoordonnees("Barbès-Rochechouart", 48.8833, 2.3495);
            AjouterCoordonnees("Anvers", 48.8828, 2.3444);
            AjouterCoordonnees("Pigalle", 48.8823, 2.3372);
            AjouterCoordonnees("Blanche", 48.8833, 2.3328);
            AjouterCoordonnees("Place de Clichy", 48.8839, 2.3275);
            AjouterCoordonnees("Rome", 48.8844, 2.3222);
            AjouterCoordonnees("Villiers", 48.8805, 2.3147);
            AjouterCoordonnees("Monceau", 48.8799, 2.3083);
            AjouterCoordonnees("Courcelles", 48.8789, 2.3042);
            AjouterCoordonnees("Ternes", 48.8778, 2.2986);
            AjouterCoordonnees("Victor Hugo", 48.8704, 2.2851);
            AjouterCoordonnees("Porte Dauphine", 48.8714, 2.2775);

            /// Ligne 3
            AjouterCoordonnees("Gallieni", 48.8652, 2.4163);
            AjouterCoordonnees("Porte de Bagnolet", 48.8645, 2.4079);
            AjouterCoordonnees("Gambetta", 48.8650, 2.3986);
            AjouterCoordonnees("Rue Saint-Maur", 48.8647, 2.3784);
            AjouterCoordonnees("Parmentier", 48.8654, 2.3742);
            AjouterCoordonnees("République", 48.8673, 2.3636);
            AjouterCoordonnees("Temple", 48.8664, 2.3594);
            AjouterCoordonnees("Arts et Métiers", 48.8654, 2.3553);
            AjouterCoordonnees("Réaumur-Sébastopol", 48.8667, 2.3522);
            AjouterCoordonnees("Sentier", 48.8689, 2.3472);
            AjouterCoordonnees("Bourse", 48.8687, 2.3417);
            AjouterCoordonnees("Quatre-Septembre", 48.8700, 2.3375);
            AjouterCoordonnees("Opéra", 48.8709, 2.3322);
            AjouterCoordonnees("Havre-Caumartin", 48.8736, 2.3275);
            AjouterCoordonnees("Saint-Lazare", 48.8753, 2.3253);
            AjouterCoordonnees("Europe", 48.8778, 2.3222);
            AjouterCoordonnees("Malesherbes", 48.8828, 2.3111);
            AjouterCoordonnees("Wagram", 48.8839, 2.3064);
            AjouterCoordonnees("Pereire", 48.8844, 2.2992);
            AjouterCoordonnees("Porte de Champerret", 48.8853, 2.2931);
            AjouterCoordonnees("Louise Michel", 48.8889, 2.2889);
            AjouterCoordonnees("Anatole France", 48.8922, 2.2850);
            AjouterCoordonnees("Pont de Levallois-Bécon", 48.8975, 2.2811);

            /// Ligne 4
            AjouterCoordonnees("Porte de Clignancourt", 48.8972, 2.3456);
            AjouterCoordonnees("Simplon", 48.8936, 2.3478);
            AjouterCoordonnees("Marcadet-Poissonniers", 48.8908, 2.3494);
            AjouterCoordonnees("Château Rouge", 48.8867, 2.3494);
            AjouterCoordonnees("Gare du Nord", 48.8800, 2.3556);
            AjouterCoordonnees("Gare de l'Est", 48.8767, 2.3589);
            AjouterCoordonnees("Château d'Eau", 48.8722, 2.3567);
            AjouterCoordonnees("Strasbourg-Saint-Denis", 48.8694, 2.3539);
            AjouterCoordonnees("Étienne Marcel", 48.8639, 2.3489);
            AjouterCoordonnees("Les Halles", 48.8622, 2.3467);
            AjouterCoordonnees("Cité", 48.8556, 2.3444);
            AjouterCoordonnees("Saint-Michel", 48.8533, 2.3433);
            AjouterCoordonnees("Odéon", 48.8522, 2.3394);
            AjouterCoordonnees("Saint-Germain-des-Prés", 48.8539, 2.3336);
            AjouterCoordonnees("Saint-Sulpice", 48.8514, 2.3306);
            AjouterCoordonnees("Saint-Placide", 48.8472, 2.3267);
            AjouterCoordonnees("Montparnasse-Bienvenüe", 48.8439, 2.3242);
            AjouterCoordonnees("Vavin", 48.8422, 2.3300);
            AjouterCoordonnees("Raspail", 48.8394, 2.3325);
            AjouterCoordonnees("Denfert-Rochereau", 48.8342, 2.3325);
            AjouterCoordonnees("Mouton-Duvernet", 48.8311, 2.3300);
            AjouterCoordonnees("Alésia", 48.8281, 2.3267);
            AjouterCoordonnees("Porte d'Orléans", 48.8236, 2.3256);
            AjouterCoordonnees("Mairie de Montrouge", 48.8181, 2.3192);

            /// Ligne 5
            AjouterCoordonnees("Bobigny-Pablo Picasso", 48.9064, 2.4494);
            AjouterCoordonnees("Bobigny-Pantin-Raymond Queneau", 48.8936, 2.4428);
            AjouterCoordonnees("Église de Pantin", 48.8931, 2.4128);
            AjouterCoordonnees("Hoche", 48.8914, 2.4031);
            AjouterCoordonnees("Porte de Pantin", 48.8889, 2.3931);
            AjouterCoordonnees("Ourcq", 48.8869, 2.3856);
            AjouterCoordonnees("Laumière", 48.8844, 2.3775);
            AjouterCoordonnees("Jacques Bonsergent", 48.8708, 2.3608);
            AjouterCoordonnees("Oberkampf", 48.8647, 2.3686);
            AjouterCoordonnees("Richard-Lenoir", 48.8614, 2.3714);
            AjouterCoordonnees("Bréguet-Sabin", 48.8564, 2.3708);
            AjouterCoordonnees("Quai de la Rapée", 48.8472, 2.3667);
            AjouterCoordonnees("Gare d'Austerlitz", 48.8433, 2.3650);
            AjouterCoordonnees("Saint-Marcel", 48.8383, 2.3608);
            AjouterCoordonnees("Campo-Formio", 48.8356, 2.3567);
            AjouterCoordonnees("Place d'Italie", 48.8317, 2.3556);

            /// Autres stations importantes
            AjouterCoordonnees("Montparnasse", 48.8439, 2.3242);
            AjouterCoordonnees("Gare Saint-Lazare", 48.8753, 2.3253);
            AjouterCoordonnees("Gare Montparnasse", 48.8417, 2.3217);
            AjouterCoordonnees("Châtelet-Les Halles", 48.8622, 2.3467);
            AjouterCoordonnees("Notre-Dame-de-Lorette", 48.8761, 2.3378);
            AjouterCoordonnees("Sèvres-Lecourbe", 48.8458, 2.3097);
            AjouterCoordonnees("Rennes", 48.8486, 2.3267);
            AjouterCoordonnees("Tolbiac", 48.8261, 2.3578);
            AjouterCoordonnees("Convention", 48.8364, 2.2983);
            AjouterCoordonnees("Télégraphe", 48.8756, 2.3981);

            List<string> nomsStations = new List<string>();

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string nomStation = reader["nom_station"].ToString().Trim();
                    if (!string.IsNullOrEmpty(nomStation))
                    {
                        nomsStations.Add(nomStation);
                    }
                }
            }

            foreach (string nomStation in nomsStations)
            {
                try
                {
                    if (stations.ContainsKey(nomStation))
                    {
                        Console.WriteLine($"Station déjà ajoutée: {nomStation}");
                        continue;
                    }

                    if (coordonneesStations.TryGetValue(nomStation, out var coords))
                    {
                        double latitude = coords.lat;
                        double longitude = coords.lon;

                        /// Mettre à jour les limites géographiques
                        minLatitude = Math.Min(minLatitude, latitude);
                        maxLatitude = Math.Max(maxLatitude, latitude);
                        minLongitude = Math.Min(minLongitude, longitude);
                        maxLongitude = Math.Max(maxLongitude, longitude);

                        /// Ajouter la station
                        if (!stations.ContainsKey(nomStation))
                        {
                            stations[nomStation] = new StationInfo
                            {
                                Nom = nomStation,
                                Latitude = latitude,
                                Longitude = longitude
                            };
                            personnesParStation[nomStation] = new List<PersonneInfo>();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Coordonnées non trouvées pour la station: {nomStation}");

                        /// Coordonnées du centre de Paris
                        double centreLat = 48.8566;
                        double centreLon = 2.3522;

                        /// Générer des coordonnées fictives en cercle autour du centre
                        /// Utiliser le hashcode du nom de la station pour avoir une position cohérente
                        int hashCode = nomStation.GetHashCode();
                        double angle = (hashCode % 360) * Math.PI / 180;
                        double rayon = 0.01; // ~1km

                        double latitude = centreLat + rayon * Math.Sin(angle);
                        double longitude = centreLon + rayon * Math.Cos(angle);

                        /// Mettre à jour les limites géographiques
                        minLatitude = Math.Min(minLatitude, latitude);
                        maxLatitude = Math.Max(maxLatitude, latitude);
                        minLongitude = Math.Min(minLongitude, longitude);
                        maxLongitude = Math.Max(maxLongitude, longitude);

                        /// Ajouter la station
                        if (!stations.ContainsKey(nomStation))
                        {
                            stations[nomStation] = new StationInfo
                            {
                                Nom = nomStation,
                                Latitude = latitude,
                                Longitude = longitude
                            };

                            /// Initialiser la liste des personnes pour cette station
                            personnesParStation[nomStation] = new List<PersonneInfo>();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors du traitement de la station {nomStation}: {ex.Message}");
                }
            }

            Console.WriteLine($"{stations.Count} stations de métro chargées");
        }

        /// <summary>
        /// Charge les clients et leurs stations de métro depuis la base de données
        /// </summary>
        private void ChargerClients(MySqlConnection connection)
        {
            Console.WriteLine("Chargement des clients...");

            string requete = @"
                SELECT id_client, nom_client, prenom_client, metro_le_plus_proche_client
                FROM CLIENT
                WHERE metro_le_plus_proche_client IS NOT NULL AND metro_le_plus_proche_client != ''";

            MySqlCommand cmd = new MySqlCommand(requete, connection);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string idClient = reader["id_client"].ToString();
                    string nomClient = reader["nom_client"].ToString();
                    string prenomClient = reader["prenom_client"].ToString();
                    string stationMetro = reader["metro_le_plus_proche_client"].ToString().Trim();

                    /// Vérifier si la station existe dans notre dictionnaire
                    if (stations.ContainsKey(stationMetro))
                    {
                        /// Ajouter le client à la liste des personnes pour cette station
                        personnesParStation[stationMetro].Add(new PersonneInfo
                        {
                            Id = idClient,
                            Nom = nomClient,
                            Prenom = prenomClient,
                            Type = "Client"
                        });
                    }
                    else
                    {
                        Console.WriteLine($"Station non trouvée pour le client {idClient}: {stationMetro}");
                    }
                }
            }

            int nbClients = personnesParStation.Values.Sum(l => l.Count(p => p.Type == "Client"));
            Console.WriteLine($"{nbClients} clients chargés");
        }

        /// <summary>
        /// Charge les cuisiniers et leurs stations de métro depuis la base de données
        /// </summary>
        private void ChargerCuisiniers(MySqlConnection connection)
        {
            Console.WriteLine("Chargement des cuisiniers...");

            string requete = @"
                SELECT id_cuisinier, nom_cuisinier, prenom_cuisinier, metro_le_plus_proche_cuisinier
                FROM CUISINIER
                WHERE metro_le_plus_proche_cuisinier IS NOT NULL AND metro_le_plus_proche_cuisinier != ''";

            MySqlCommand cmd = new MySqlCommand(requete, connection);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string idCuisinier = reader["id_cuisinier"].ToString();
                    string nomCuisinier = reader["nom_cuisinier"].ToString();
                    string prenomCuisinier = reader["prenom_cuisinier"].ToString();
                    string stationMetro = reader["metro_le_plus_proche_cuisinier"].ToString().Trim();

                    /// Vérifier si la station existe dans notre dictionnaire
                    if (stations.ContainsKey(stationMetro))
                    {
                        /// Ajouter le cuisinier à la liste des personnes pour cette station
                        personnesParStation[stationMetro].Add(new PersonneInfo
                        {
                            Id = idCuisinier,
                            Nom = nomCuisinier,
                            Prenom = prenomCuisinier,
                            Type = "Cuisinier"
                        });
                    }
                    else
                    {
                        Console.WriteLine($"Station non trouvée pour le cuisinier {idCuisinier}: {stationMetro}");
                    }
                }
            }

            int nbCuisiniers = personnesParStation.Values.Sum(l => l.Count(p => p.Type == "Cuisinier"));
            Console.WriteLine($"{nbCuisiniers} cuisiniers chargés");
        }

        /// <summary>
        /// Charge les relations entre clients et cuisiniers (commandes) depuis la base de données
        /// </summary>
        private void ChargerRelations(MySqlConnection connection)
        {
            Console.WriteLine("Chargement des relations clients-cuisiniers...");

            string requete = @"
                SELECT DISTINCT c.id_client, cu.id_cuisinier
                FROM COMMANDE cmd
                JOIN CLIENT c ON cmd.id_client = c.id_client
                JOIN LIGNE_COMMANDE lc ON cmd.id_commande = lc.id_commande
                JOIN CUISINIER cu ON lc.id_cuisinier = cu.id_cuisinier
                WHERE cmd.statut_commande = 'Livrée'
                  AND c.metro_le_plus_proche_client IS NOT NULL
                  AND cu.metro_le_plus_proche_cuisinier IS NOT NULL";

            MySqlCommand cmd = new MySqlCommand(requete, connection);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string idClient = reader["id_client"].ToString();
                    string idCuisinier = reader["id_cuisinier"].ToString();

                    relations.Add(new RelationInfo
                    {
                        IdClient = idClient,
                        IdCuisinier = idCuisinier
                    });
                }
            }

            Console.WriteLine($"{relations.Count} relations chargées");
        }

        /// <summary>
        /// Génère la visualisation du graphe et l'enregistre dans un fichier image
        /// </summary>
        /// <param name="cheminFichier">Chemin du fichier où enregistrer l'image</param>
        /// <returns>True si la visualisation a été générée avec succès</returns>
        public bool VisualiserGraphe(string cheminFichier)
        {
            try
            {
                Console.WriteLine("Génération de la visualisation du graphe...");

                Dictionary<string, PointF> positionsStations = CalculerPositionsStations();

                /// Créer l'image
                using (Bitmap bitmap = new Bitmap(largeur, hauteur))
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    g.Clear(Color.White);

                    /// Dessiner les stations de métro
                    DessinerStations(g, positionsStations);

                    /// Dessiner les clients et cuisiniers
                    Dictionary<string, PointF> positionsPersonnes = DessinerPersonnes(g, positionsStations);

                    /// Dessiner les relations entre clients et cuisiniers
                    DessinerRelations(g, positionsPersonnes);

                    /// Dessiner la légende
                    DessinerLegende(g);

                    /// Enregistrer l'image
                    bitmap.Save(cheminFichier, ImageFormat.Png);
                    Console.WriteLine($"Visualisation enregistrée sous '{cheminFichier}'");

                    /// Ouvrir l'image
                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(cheminFichier) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de l'ouverture de l'image: {ex.Message}");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la génération de la visualisation: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Calcule les positions des stations sur l'image en fonction de leurs coordonnées GPS
        /// </summary>
        /// <returns>Dictionnaire associant chaque station à sa position sur l'image</returns>
        private Dictionary<string, PointF> CalculerPositionsStations()
        {
            Dictionary<string, PointF> positions = new Dictionary<string, PointF>(StringComparer.OrdinalIgnoreCase);

            /// Calculer la largeur et la hauteur utiles (sans les marges)
            float largeurUtile = largeur - 2 * margeImage;
            float hauteurUtile = hauteur - 2 * margeImage;

            /// Calculer l'étendue des coordonnées géographiques
            double rangeLon = maxLongitude - minLongitude;
            double rangeLat = maxLatitude - minLatitude;

            /// Ajouter une petite marge aux limites géographiques pour éviter les bords
            double margeGeo = 0.05;
            minLongitude -= rangeLon * margeGeo;
            maxLongitude += rangeLon * margeGeo;
            minLatitude -= rangeLat * margeGeo;
            maxLatitude += rangeLat * margeGeo;

            /// Recalculer l'étendue avec les marges
            rangeLon = maxLongitude - minLongitude;
            rangeLat = maxLatitude - minLatitude;

            /// Calculer la position de chaque station
            foreach (var station in stations.Values)
            {
                /// Convertir les coordonnées géographiques en coordonnées sur l'image
                float x = margeImage + (float)(largeurUtile * (station.Longitude - minLongitude) / rangeLon);
                float y = hauteur - margeImage - (float)(hauteurUtile * (station.Latitude - minLatitude) / rangeLat);

                positions[station.Nom] = new PointF(x, y);
            }

            return positions;
        }

        /// <summary>
        /// Dessine les stations de métro sur l'image
        /// </summary>
        /// <param name="g">Objet Graphics pour dessiner</param>
        /// <param name="positionsStations">Positions des stations sur l'image</param>
        private void DessinerStations(Graphics g, Dictionary<string, PointF> positionsStations)
        {
            Console.WriteLine("Dessin des stations de métro...");

            using (Brush brushStation = new SolidBrush(couleurStation))
            using (Pen penStation = new Pen(couleurStation, 1.5f))
            {
                foreach (var kvp in positionsStations)
                {
                    string nomStation = kvp.Key;
                    PointF position = kvp.Value;

                    /// Dessiner le cercle de la station
                    float rayon = tailleStation / 2;
                    g.FillEllipse(brushStation, position.X - rayon, position.Y - rayon, tailleStation, tailleStation);
                    g.DrawEllipse(penStation, position.X - rayon, position.Y - rayon, tailleStation, tailleStation);

                    /// Dessiner le nom de la station
                    SizeF tailleTexte = g.MeasureString(nomStation, fontNomStation);
                    g.DrawString(nomStation, fontNomStation, brushStation,
                        position.X - tailleTexte.Width / 2,
                        position.Y - tailleTexte.Height - rayon - 5);
                }
            }
        }

        /// <summary>
        /// Dessine les clients et cuisiniers sur l'image
        /// </summary>
        /// <param name="g">Objet Graphics pour dessiner</param>
        /// <param name="positionsStations">Positions des stations sur l'image</param>
        /// <returns>Dictionnaire associant chaque personne (client/cuisinier) à sa position sur l'image</returns>
        private Dictionary<string, PointF> DessinerPersonnes(Graphics g, Dictionary<string, PointF> positionsStations)
        {
            Console.WriteLine("Dessin des clients et cuisiniers...");

            /// Dictionnaire pour stocker les positions des personnes
            Dictionary<string, PointF> positionsPersonnes = new Dictionary<string, PointF>();

            using (Brush brushClient = new SolidBrush(couleurClient))
            using (Brush brushCuisinier = new SolidBrush(couleurCuisinier))
            {
                foreach (var kvp in personnesParStation)
                {
                    string nomStation = kvp.Key;
                    List<PersonneInfo> personnes = kvp.Value;

                    if (!positionsStations.ContainsKey(nomStation) || personnes.Count == 0)
                        continue;

                    PointF positionStation = positionsStations[nomStation];

                    int nbPersonnes = personnes.Count;
                    double angleIncrement = 2 * Math.PI / Math.Max(nbPersonnes, 8); /// Au moins 8 positions pour éviter trop de superposition
                    double rayonPersonnes = tailleStation * 4; 

                    for (int i = 0; i < nbPersonnes; i++)
                    {
                        PersonneInfo personne = personnes[i];
                        double angle = i * angleIncrement;
                        
                        /// Calculer la position de la personne
                        float x = positionStation.X + (float)(rayonPersonnes * Math.Cos(angle));
                        float y = positionStation.Y + (float)(rayonPersonnes * Math.Sin(angle));

                        /// Créer l'étiquette
                        string etiquette = $"{nomStation} - {personne.Type}: {personne.Prenom}";

                        /// Dessiner l'étiquette
                        Brush brush = personne.Type == "Client" ? brushClient : brushCuisinier;
                        SizeF tailleTexte = g.MeasureString(etiquette, fontNomPersonne);

                        /// Dessiner un fond blanc semi-transparent pour améliorer la lisibilité
                        using (Brush brushFond = new SolidBrush(Color.FromArgb(200, Color.White)))
                        {
                            g.FillRectangle(brushFond,
                                x - tailleTexte.Width / 2 - 2,
                                y - tailleTexte.Height / 2 - 2,
                                tailleTexte.Width + 4,
                                tailleTexte.Height + 4);
                        }

                        g.DrawString(etiquette, fontNomPersonne, brush,
                            x - tailleTexte.Width / 2,
                            y - tailleTexte.Height / 2);

                        /// Dessiner une ligne entre la personne et sa station
                        using (Pen penLien = new Pen(Color.FromArgb(100, personne.Type == "Client" ? couleurClient : couleurCuisinier), 1))
                        {
                            g.DrawLine(penLien, positionStation, new PointF(x, y));
                        }

                        /// Stocker la position de la personne pour dessiner les relations
                        PointF position = new PointF(x, y);
                        personne.Position = position;

                        /// Ajouter la position au dictionnaire
                        positionsPersonnes[personne.Id] = position;
                    }
                }
            }

            return positionsPersonnes;
        }

        /// <summary>
        /// Dessine les relations entre clients et cuisiniers
        /// </summary>
        /// <param name="g">Objet Graphics pour dessiner</param>
        /// <param name="positionsPersonnes">Positions des personnes sur l'image</param>
        private void DessinerRelations(Graphics g, Dictionary<string, PointF> positionsPersonnes)
        {
            Console.WriteLine("Dessin des relations clients-cuisiniers...");

            using (Pen penRelation = new Pen(couleurRelation, epaisseurLigne))
            {
                penRelation.DashStyle = DashStyle.Dash;

                foreach (var relation in relations)
                {
                    if (positionsPersonnes.TryGetValue(relation.IdClient, out PointF posClient) &&
                        positionsPersonnes.TryGetValue(relation.IdCuisinier, out PointF posCuisinier))
                    {
                        g.DrawLine(penRelation, posClient, posCuisinier);
                    }
                }
            }
        }

        /// <summary>
        /// Dessine la légende du graphe
        /// </summary>
        /// <param name="g">Objet Graphics pour dessiner</param>
        private void DessinerLegende(Graphics g)
        {
            Console.WriteLine("Dessin de la légende...");

            int x = 50;
            int y = 50;
            int espacement = 30;
            int tailleSymbole = 20;

            /// Titre de la légende
            using (Font fontTitre = new Font("Arial", 16, FontStyle.Bold))
            using (Brush brushTitre = new SolidBrush(Color.Black))
            {
                g.DrawString("Légende", fontTitre, brushTitre, x, y);
                y += 40;
            }

            /// Station de métro
            using (Brush brushStation = new SolidBrush(couleurStation))
            using (Pen penStation = new Pen(couleurStation, 1.5f))
            {
                g.FillEllipse(brushStation, x, y, tailleSymbole, tailleSymbole);
                g.DrawEllipse(penStation, x, y, tailleSymbole, tailleSymbole);
                g.DrawString("Station de métro", fontNomPersonne, brushStation, x + tailleSymbole + 10, y);
                y += espacement;
            }

            /// Client
            using (Brush brushClient = new SolidBrush(couleurClient))
            {
                g.FillRectangle(brushClient, x, y, tailleSymbole, tailleSymbole);
                g.DrawString("Client", fontNomPersonne, brushClient, x + tailleSymbole + 10, y);
                y += espacement;
            }

            /// Cuisinier
            using (Brush brushCuisinier = new SolidBrush(couleurCuisinier))
            {
                g.FillRectangle(brushCuisinier, x, y, tailleSymbole, tailleSymbole);
                g.DrawString("Cuisinier", fontNomPersonne, brushCuisinier, x + tailleSymbole + 10, y);
                y += espacement;
            }

            /// Relation
            using (Pen penRelation = new Pen(couleurRelation, epaisseurLigne))
            {
                penRelation.DashStyle = DashStyle.Dash;
                g.DrawLine(penRelation, x, y + tailleSymbole / 2, x + tailleSymbole, y + tailleSymbole / 2);
                g.DrawString("Relation (commande)", fontNomPersonne, new SolidBrush(couleurRelation), x + tailleSymbole + 10, y);
            }
        }

        /// <summary>
        /// Classe pour stocker les informations d'une station de métro
        /// </summary>
        private class StationInfo
        {
            public string Nom { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        /// <summary>
        /// Classe pour stocker les informations d'une personne (client ou cuisinier)
        /// </summary>
        private class PersonneInfo
        {
            public string Id { get; set; }
            public string Nom { get; set; }
            public string Prenom { get; set; }
            public string Type { get; set; } 
            public PointF Position { get; set; } 
        }

        /// <summary>
        /// Classe pour stocker les informations d'une relation client-cuisinier
        /// </summary>
        private class RelationInfo
        {
            public string IdClient { get; set; }
            public string IdCuisinier { get; set; }
        }
    }
}
