using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Classe pour calculer des distances entre stations et trouver les chemins les plus courts (PCC) en utilisant différents algorithmes de graphes.
    /// </summary>
    public class CalculDistance
    {

        #region Calcul et affichage de la distance entre deux stations et acquisition des coordonnées des stations

        /// <summary>
        /// Affiche la distance entre deux stations en utilisant leurs coordonnées GPS et la formule de Haversine pour le calcul.
        /// </summary>
        public static void DistanceEntreDeuxStations()
        {
            //string chemin = @"C:\Users\thoma\OneDrive\Documents\GitHub\PSI_2025_ThoFaizMart\Feuille_noeud_convertie.csv";
            string chemin = @"C:\Users\fzzn7\Documents\GitHub\PSI_2025_ThoFaizMart\Feuille_noeud_convertie.csv";

            if (!File.Exists(chemin))
            {
                Console.WriteLine("Erreur: Le fichier " + chemin + " n'existe pas.");
                return;
            }

            Console.WriteLine("Entrez le nom de deux stations dont vous voulez connaître la distance de l'une à l'autre.");
            Console.WriteLine("La saisie se fait sans accents (grave, aigu, circonflexe), sans cédille et sans tréma, et en respectant les espaces et les tirets (pour les noms de métros composés)");
            Console.WriteLine();
            Console.Write("Station 1 : ");
            string station1 = Console.ReadLine();
            Console.Write("Station 2 : ");
            string station2 = Console.ReadLine();
            
            (double lat1, double long1) = AcquisitionCoordonnees(chemin, station1);
            (double lat2, double long2) = AcquisitionCoordonnees(chemin, station2);

            if (lat1 != 0 && lat2 != 0)
            {
                double distance = Haversine(lat1, long1, lat2, long2);
                Console.WriteLine($"Voici la distance entre {station1} et {station2}: {distance:F2} km.");
            }
            else
            {
                if (lat1 == 0)
                    Console.WriteLine("Station " + station1 + " introuvable !");
                if (lat2 == 0)
                    Console.WriteLine("Station " + station2 + " introuvable !");
            }
        }

        /// <summary>
        /// Récupère les coordonnées GPS (latitude, longitude) d'une station à partir du fichier CSV des noeuds.
        /// </summary>
        public static (double, double) AcquisitionCoordonnees(string chemin, string stationName)
        {
            try
            {
                string[] lignes = File.ReadAllLines(chemin);

                foreach (string ligne in lignes.Skip(1))
                {
                    var data = ligne.Split(',');
                    if (data.Length < 5)
                    {
                        continue;
                    }
                    string nomStation = data[2].Trim().Trim('"');


                    if (nomStation.Equals(stationName, StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            double latitude = double.Parse(data[4].Trim().Trim('"').Replace('.', ','));
                            double longitude = double.Parse(data[3].Trim().Trim('"').Replace('.', ','));
                            return (latitude, longitude);
                        }
                        catch
                        {
                            Console.WriteLine("Erreur de conversion des coordonnées pour " + nomStation);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la recherche de la station: {ex.Message}");
            }
            return (0, 0);

        }

        /// <summary>
        /// Calcule la distance entre deux points GPS en utilisant la formule de Haversine et la retourne en kilomètres.
        /// </summary>
        public static double Haversine(double lat1, double long1, double lat2, double long2)
        {
            const double R = 6371;
            double deltaPhi = ConversionEnRadian(lat2 - lat1);
            double deltaLambda = ConversionEnRadian(long2 - long1);

            double phi1 = ConversionEnRadian(lat1);
            double phi2 = ConversionEnRadian(lat2);

            double a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                       Math.Cos(phi1) * Math.Cos(phi2) *
                       Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            double c = 2 * R * Math.Asin(Math.Sqrt(a));

            return c;
        }

        /// <summary>
        /// Convertit des degrés en radians pour les calculs de trigo.
        /// </summary>
        public static double ConversionEnRadian(double degre)
        {
            return Math.PI * degre / 180.0;
        }
        #endregion

        #region Les 3 algorithmes de recherche de PCC

        /// <summary>
        /// Implémente l'algorithme de Bellman-Ford pour trouver le chemin le plus court entre deux noeuds, capable de gérer les poids négatifs.
        /// </summary>
        public static List<Noeud<int>> BellmanFordChemin(Noeud<int> depart, Noeud<int> arrivee, List<Noeud<int>> noeuds, List<Lien<int>> liens)
        {
            var distances = new Dictionary<Noeud<int>, double>();
            var predecesseurs = new Dictionary<Noeud<int>, Noeud<int>>();

            foreach (var n in noeuds)
            {
                distances[n] = double.PositiveInfinity;
                predecesseurs[n] = null;
            }
            distances[depart] = 0;

            for (int i = 0; i < noeuds.Count - 1; i++)
            {
                bool changementEffectue = false;
                foreach (var lien in liens)
                {
                    if (distances.ContainsKey(lien.Noeud1) && distances.ContainsKey(lien.Noeud2))
                    {
                        if (distances[lien.Noeud1] != double.PositiveInfinity &&
                            distances[lien.Noeud1] + lien.Poids < distances[lien.Noeud2])
                        {
                            distances[lien.Noeud2] = distances[lien.Noeud1] + lien.Poids;
                            predecesseurs[lien.Noeud2] = lien.Noeud1;
                            changementEffectue = true;
                        }

                        if (distances[lien.Noeud2] != double.PositiveInfinity &&
                            distances[lien.Noeud2] + lien.Poids < distances[lien.Noeud1])
                        {
                            distances[lien.Noeud1] = distances[lien.Noeud2] + lien.Poids;
                            predecesseurs[lien.Noeud1] = lien.Noeud2;
                            changementEffectue = true;
                        }
                    }
                }

                if (!changementEffectue)
                {
                    break;
                }
            }

            foreach (var lien in liens)
            {
                if (distances.ContainsKey(lien.Noeud1) && distances.ContainsKey(lien.Noeud2))
                {
                    if (distances[lien.Noeud1] != double.PositiveInfinity &&
                        distances[lien.Noeud1] + lien.Poids < distances[lien.Noeud2])
                    {
                        Console.WriteLine("Le graphe contient un cycle de poids négatif.");
                        return new List<Noeud<int>>(); 
                    }

                    if (distances[lien.Noeud2] != double.PositiveInfinity &&
                        distances[lien.Noeud2] + lien.Poids < distances[lien.Noeud1])
                    {
                        Console.WriteLine("Le graphe contient un cycle de poids négatif.");
                        return new List<Noeud<int>>(); 
                    }
                }
            }

            var chemin = new List<Noeud<int>>();
            Noeud<int> noeudActuel = arrivee;

            if (predecesseurs[arrivee] == null && !arrivee.Equals(depart))
            {
                Console.WriteLine("Aucun chemin n'a été trouvé entre ces deux stations.");
                return chemin; 
            }

            while (noeudActuel != null)
            {
                chemin.Insert(0, noeudActuel);
                noeudActuel = predecesseurs[noeudActuel];
            }

            return chemin;
        }

        /// <summary>
        /// Implémente l'algorithme de Dijkstra pour trouver le chemin le plus court entre deux noeuds, optimal pour les graphes sans poids négatifs.
        /// </summary>
        public static List<Noeud<int>> DijkstraChemin(Noeud<int> depart, Noeud<int> arrivee, List<Noeud<int>> noeuds, List<Lien<int>> liens)
        {
            var distances = new Dictionary<Noeud<int>, double>();
            var predecesseurs = new Dictionary<Noeud<int>, Noeud<int>>();
            var traites = new HashSet<Noeud<int>>();

            foreach (var n in noeuds)
            {
                distances[n] = double.PositiveInfinity;
                predecesseurs[n] = null;
            }
            distances[depart] = 0;

            var voisins = new Dictionary<Noeud<int>, List<(Noeud<int>, double)>>();
            foreach (var n in noeuds)
            {
                voisins[n] = new List<(Noeud<int>, double)>();
            }

            foreach (var lien in liens)
            {
                if (voisins.ContainsKey(lien.Noeud1) && voisins.ContainsKey(lien.Noeud2))
                {
                    voisins[lien.Noeud1].Add((lien.Noeud2, lien.Poids));
                    voisins[lien.Noeud2].Add((lien.Noeud1, lien.Poids)); 
                }
            }

            while (traites.Count < noeuds.Count)
            {
                
                Noeud<int> noeudCourant = null;
                double distanceMin = double.PositiveInfinity;

                foreach (var n in noeuds)
                {
                    if (!traites.Contains(n) && distances[n] < distanceMin)
                    {
                        distanceMin = distances[n];
                        noeudCourant = n;
                    }
                }

                if (noeudCourant == null || noeudCourant.Equals(arrivee))
                {
                    break;
                }
                   
                traites.Add(noeudCourant);
                
                foreach (var (voisin, poids) in voisins[noeudCourant])
                {
                    if (!traites.Contains(voisin))
                    {
                        double nouvelleDistance = distances[noeudCourant] + poids;
                        if (nouvelleDistance < distances[voisin])
                        {
                            distances[voisin] = nouvelleDistance;
                            predecesseurs[voisin] = noeudCourant;
                        }
                    }
                }
            }

            var chemin = new List<Noeud<int>>();
            Noeud<int> noeudActuel = arrivee;

            if (predecesseurs[arrivee] == null && !arrivee.Equals(depart))
            {
                Console.WriteLine("Aucun chemin n'a été trouvé entre ces deux stations.");
                return chemin; 
            }

            while (noeudActuel != null)
            {
                chemin.Insert(0, noeudActuel);
                noeudActuel = predecesseurs[noeudActuel];
            }
            return chemin;
        }

        /// <summary>
        /// Implémente l'algorithme de Floyd-Warshall pour trouver le chemin le plus court entre deux noeuds, calculant tous les chemins possibles.
        /// </summary>
        public static List<Noeud<int>> FloydWarshallChemin(Noeud<int> depart, Noeud<int> arrivee, List<Noeud<int>> noeuds, List<Lien<int>> liens)
        {
            int n = noeuds.Count;

            var noeudToIndex = new Dictionary<Noeud<int>, int>();
            var indexToNoeud = new Dictionary<int, Noeud<int>>();

            for (int i = 0; i < n; i++)
            {
                noeudToIndex[noeuds[i]] = i;
                indexToNoeud[i] = noeuds[i];
            }

            double[,] distances = new double[n, n];
            int[,] predecesseurs = new int[n, n];
 
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    distances[i, j] = double.PositiveInfinity;
                    predecesseurs[i, j] = -1;
                }
            
                distances[i, i] = 0;
            }

            foreach (var lien in liens)
            {
                int i = noeudToIndex[lien.Noeud1];
                int j = noeudToIndex[lien.Noeud2];

                distances[i, j] = lien.Poids;
                distances[j, i] = lien.Poids;

                predecesseurs[i, j] = i;
                predecesseurs[j, i] = j;
            }

            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (distances[i, k] + distances[k, j] < distances[i, j])
                        {
                            distances[i, j] = distances[i, k] + distances[k, j];
                            predecesseurs[i, j] = predecesseurs[k, j];
                        }
                    }
                }
            }

            var chemin = new List<Noeud<int>>();

            int departIndex = noeudToIndex[depart];
            int arriveeIndex = noeudToIndex[arrivee];

            if (double.IsInfinity(distances[departIndex, arriveeIndex]))
            {
                Console.WriteLine("Aucun chemin n'a été trouvé entre ces deux stations.");
                return chemin; 
            }

            int courantIndex = arriveeIndex;
            while (courantIndex != departIndex)
            {
                chemin.Insert(0, indexToNoeud[courantIndex]);
                courantIndex = predecesseurs[departIndex, courantIndex];
            }
            chemin.Insert(0, depart); 

            return chemin;
        }

        /// <summary>
        /// Trouve et affiche le chemin le plus court entre deux stations en utilisant l'algorithme choisi par l'utilisateur (Dijkstra, Bellman-Ford ou Floyd-Warshall).
        /// </summary>
        public static void CheminLePlusCourt()
        {
            string cheminFichierNoeuds = @"C:\Users\fzzn7\Documents\GitHub\PSI_2025_ThoFaizMart\Feuille_noeud_convertie.csv";
            //string cheminFichierNoeuds = @"C:\Users\thoma\OneDrive\Documents\GitHub\PSI_2025_ThoFaizMart\Feuille_noeud_convertie.csv";
            //string cheminFichierArcs = @"C:\Users\thoma\OneDrive\Documents\GitHub\PSI_2025_ThoFaizMart\Feuille_arc_convertie.csv";
            string cheminFichierArcs = @"C:\Users\fzzn7\Documents\GitHub\PSI_2025_ThoFaizMart\Feuille_arc_convertie.csv";

            if (!File.Exists(cheminFichierNoeuds))
            {
                Console.WriteLine($"Erreur: Le fichier {cheminFichierNoeuds} n'existe pas.");
                return;
            }

            if (!File.Exists(cheminFichierArcs))
            {
                Console.WriteLine($"Erreur: Le fichier {cheminFichierArcs} n'existe pas.");
                return;
            }

            try
            {
                var stations = ChargerStationsDirectement(cheminFichierNoeuds);
                if (stations.Count == 0)
                {
                    Console.WriteLine("Erreur: Aucune station n'a pu être chargée.");
                    return;
                }

                Console.WriteLine("Entrez le nom de deux stations pour trouver le chemin le plus court entre elles.");
                Console.WriteLine("La saisie se fait sans accents (grave, aigu, circonflexe), sans cédille et sans tréma, et en respectant les espaces");
                Console.WriteLine();
                Console.Write("Station de départ : ");
                string stationDepart = Console.ReadLine();
                Console.Write("Station d'arrivée : ");
                string stationArrivee = Console.ReadLine();

                Console.WriteLine();
                Console.WriteLine("Choisissez l'algorithme à utiliser :");
                Console.WriteLine("1. Dijkstra (optimal pour les graphes sans poids négatifs)");
                Console.WriteLine("2. Bellman-Ford (peut gérer les poids négatifs, mais plus lent)");
                Console.WriteLine("3. Floyd-Warshall (calcule tous les plus courts chemins, efficace pour les requêtes multiples)");
                Console.Write("Votre choix (1, 2 ou 3) : ");
                string choixAlgo = Console.ReadLine();

                int algorithmeChoisi = 1;
                if (choixAlgo == "2")
                {
                    algorithmeChoisi = 2;
                }
                else if (choixAlgo == "3")
                {
                    algorithmeChoisi = 3;
                }

                var stationDep = TrouverStation(stations, stationDepart);
                var stationArr = TrouverStation(stations, stationArrivee);

                if (stationDep == null || stationArr == null)
                {
                    if (stationDep == null)
                        Console.WriteLine($"Station '{stationDepart}' introuvable !");
                    if (stationArr == null)
                        Console.WriteLine($"Station '{stationArrivee}' introuvable !");
                    return;
                }

                string nomAlgo = algorithmeChoisi == 1 ? "de Dijkstra" :
                         algorithmeChoisi == 2 ? "de Bellman-Ford" :
                         "de Floyd-Warshall";
                Console.WriteLine($"Recherche du chemin entre {stationDep.Nom} et {stationArr.Nom} avec l'algorithme {nomAlgo}...");

                var (noeuds, liens) = ConstruireGrapheComplet(stations, cheminFichierArcs);

                var mapIdToNoeud = noeuds.ToDictionary(n => n.Id);

                if (!mapIdToNoeud.ContainsKey(stationDep.Id) || !mapIdToNoeud.ContainsKey(stationArr.Id))
                {
                    Console.WriteLine("Erreur: Les stations ne sont pas correctement mappées dans le graphe.");
                    return;
                }

                var noeudDepart = mapIdToNoeud[stationDep.Id];
                var noeudArrivee = mapIdToNoeud[stationArr.Id];

                var grapheAdjacence = ConstruireGrapheAdjacence(noeuds, liens);
                bool sontConnectes = VerifierConnectivite(grapheAdjacence, noeudDepart, noeudArrivee);

                if (!sontConnectes)
                {
                    AjouterConnexionsSupplementaires(stations, noeuds, liens, grapheAdjacence);

                    sontConnectes = VerifierConnectivite(grapheAdjacence, noeudDepart, noeudArrivee);

                    if (!sontConnectes)
                    {
                        double distance = Haversine(stationDep.Latitude, stationDep.Longitude, stationArr.Latitude, stationArr.Longitude);
                        int poids = Math.Max((int)(distance * 1000), 1);

                        liens.Add(new Lien<int>(noeudDepart, noeudArrivee, poids));
                        grapheAdjacence[noeudDepart].Add(noeudArrivee);
                        grapheAdjacence[noeudArrivee].Add(noeudDepart);
                    }
                }

                List<Noeud<int>> chemin;
                switch (algorithmeChoisi)
                {
                    case 2:
                        chemin = BellmanFordChemin(noeudDepart, noeudArrivee, noeuds, liens);
                        break;
                    case 3:
                        chemin = FloydWarshallChemin(noeudDepart, noeudArrivee, noeuds, liens);
                        break;
                    default:
                        chemin = DijkstraChemin(noeudDepart, noeudArrivee, noeuds, liens);
                        break;
                }

                if (chemin.Count <= 1 && !stationDep.Id.Equals(stationArr.Id))
                {
                    Console.WriteLine("Aucun chemin trouvé entre ces deux stations.");
                    return;
                }

                AfficherChemin(chemin, stations);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur est survenue: {ex.Message}");
            }
        }
        #endregion

        #region Lecture du fichier CSV et chargement des stations
        /// <summary>
        /// Charge la liste des stations depuis le fichier CSV des noeuds en ignorant les lignes mal formatées.
        /// </summary>
        public static List<Station> ChargerStationsDirectement(string cheminFichier)
        {
            var stations = new List<Station>();
            try
            {
                string[] lignes = File.ReadAllLines(cheminFichier);

                foreach (string ligne in lignes.Skip(1)) 
                {
                    var data = ligne.Split(',');
                    if (data.Length < 5) continue;

                    try
                    {
                        int id = int.Parse(data[0].Trim().Trim('"'));
                        string ligne_metro = data[1].Trim().Trim('"');
                        string nom = data[2].Trim().Trim('"');

                        double longitude = 0, latitude = 0;

                        if (double.TryParse(data[3].Trim().Trim('"').Replace('.', ','), out double lon))
                            longitude = lon;
                        else if (double.TryParse(data[3].Trim().Trim('"'), out lon))
                            longitude = lon;

                        if (double.TryParse(data[4].Trim().Trim('"').Replace('.', ','), out double lat))
                            latitude = lat;
                        else if (double.TryParse(data[4].Trim().Trim('"'), out lat))
                            latitude = lat;

                        string commune = data.Length > 5 ? data[5].Trim().Trim('"') : "Paris";
                        string codeInsee = data.Length > 6 ? data[6].Trim().Trim('"') : "75000";

                        stations.Add(new Station(id, ligne_metro, nom, longitude, latitude, commune, codeInsee));
                    }
                    catch
                    {
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des stations: {ex.Message}");
            }
            return stations;
        }

        /// <summary>
        /// Trouve une station par son nom (recherche insensible à la casse) dans la liste des stations chargées.
        /// </summary>
        public static Station TrouverStation(List<Station> stations, string nom)
        {
            return stations.FirstOrDefault(s => s.Nom.Equals(nom, StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        #region Construction du graphe en entier
        /// <summary>
        /// Construit le graphe complet (noeuds et liens) à partir des stations et des connexions du fichier CSV des arcs.
        /// </summary>
        public static (List<Noeud<int>>, List<Lien<int>>) ConstruireGrapheComplet(List<Station> stations, string cheminFichierArcs)
        {
            var noeuds = new List<Noeud<int>>();
            var liens = new List<Lien<int>>();
            var mapIdToNoeud = new Dictionary<int, Noeud<int>>();
         
            foreach (var station in stations)
            {
                var noeud = new Noeud<int>(station.Id, station.Id);
                noeuds.Add(noeud);
                mapIdToNoeud[station.Id] = noeud;
            }
        
            try
            {
                string[] lignes = File.ReadAllLines(cheminFichierArcs);

                foreach (string ligne in lignes.Skip(1)) 
                {
                    var data = ligne.Split(',');
                    if (data.Length < 3)
                    {
                        continue;
                    }
                       
                    try
                    {
                        int id1 = int.Parse(data[1].Trim().Trim('"'));
                        int id2 = int.Parse(data[2].Trim().Trim('"'));

                        if (mapIdToNoeud.ContainsKey(id1) && mapIdToNoeud.ContainsKey(id2))
                        {
                            var noeud1 = mapIdToNoeud[id1];
                            var noeud2 = mapIdToNoeud[id2];
                           
                            var station1 = stations.Find(s => s.Id == id1);
                            var station2 = stations.Find(s => s.Id == id2);

                            if (station1 != null && station2 != null)
                            {
                                double distance = Haversine(station1.Latitude, station1.Longitude, station2.Latitude, station2.Longitude);
                                int poids = Math.Max((int)(distance * 1000), 1); 

                                liens.Add(new Lien<int>(noeud1, noeud2, poids));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors du traitement de la ligne '{ligne}': {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des arcs: {ex.Message}");
            }

            var stationsParNom = new Dictionary<string, List<Station>>(StringComparer.OrdinalIgnoreCase);
            foreach (var station in stations)
            {
                if (!stationsParNom.ContainsKey(station.Nom))
                    stationsParNom[station.Nom] = new List<Station>();
                stationsParNom[station.Nom].Add(station);
            }

            foreach (var groupe in stationsParNom.Values.Where(g => g.Count > 1))
            {
                for (int i = 0; i < groupe.Count; i++)
                {
                    for (int j = i + 1; j < groupe.Count; j++)
                    {
                        var station1 = groupe[i];
                        var station2 = groupe[j];

                        if (station1.Ligne != station2.Ligne &&
                            mapIdToNoeud.ContainsKey(station1.Id) &&
                            mapIdToNoeud.ContainsKey(station2.Id))
                        {
                            var noeud1 = mapIdToNoeud[station1.Id];
                            var noeud2 = mapIdToNoeud[station2.Id];
                        
                            liens.Add(new Lien<int>(noeud1, noeud2, 100));
                        }
                    }
                }
            }
         
            var stationsParLigne = new Dictionary<string, List<Station>>();
            foreach (var station in stations)
            {
                if (!stationsParLigne.ContainsKey(station.Ligne))
                    stationsParLigne[station.Ligne] = new List<Station>();
                stationsParLigne[station.Ligne].Add(station);
            }

            foreach (var ligne in stationsParLigne.Keys)
            {
                var stationsDeLigne = stationsParLigne[ligne]
                    .OrderBy(s => s.Id) 
                    .ToList();

                for (int i = 0; i < stationsDeLigne.Count - 1; i++)
                {
                    var station1 = stationsDeLigne[i];
                    var station2 = stationsDeLigne[i + 1];

                    if (mapIdToNoeud.ContainsKey(station1.Id) && mapIdToNoeud.ContainsKey(station2.Id))
                    {
                        var noeud1 = mapIdToNoeud[station1.Id];
                        var noeud2 = mapIdToNoeud[station2.Id];
                        
                        double distance = Haversine(station1.Latitude, station1.Longitude, station2.Latitude, station2.Longitude);
                        int poids = Math.Max((int)(distance * 1000), 1);
                       
                        bool lienExiste = liens.Any(l =>
                            (l.Noeud1 == noeud1 && l.Noeud2 == noeud2) ||
                            (l.Noeud1 == noeud2 && l.Noeud2 == noeud1));

                        if (!lienExiste)
                        {
                            liens.Add(new Lien<int>(noeud1, noeud2, poids));
                        }
                    }
                }
            }
            return (noeuds, liens);
        }

        /// <summary>
        /// Construit un graphe d'adjacence à partir des noeuds et liens pour faciliter les vérifications de connectivité.
        /// </summary>
        public static Dictionary<Noeud<int>, List<Noeud<int>>> ConstruireGrapheAdjacence(List<Noeud<int>> noeuds, List<Lien<int>> liens)
        {
            var graphe = new Dictionary<Noeud<int>, List<Noeud<int>>>();

            foreach (var noeud in noeuds)
            {
                graphe[noeud] = new List<Noeud<int>>();
            }

            foreach (var lien in liens)
            {
                graphe[lien.Noeud1].Add(lien.Noeud2);
                graphe[lien.Noeud2].Add(lien.Noeud1);
            }
            return graphe;
        }
        #endregion

        #region Vérification de la connexion entre deux noeuds (parcours BFS) et ajout de connexion pour bien relier deux stations

        /// <summary>
        /// Vérifie si deux noeuds sont connectés dans le graphe en utilisant un parcours en largeur (BFS).
        /// </summary>
        public static bool VerifierConnectivite(Dictionary<Noeud<int>, List<Noeud<int>>> graphe, Noeud<int> depart, Noeud<int> arrivee)
        {
            if (depart.Equals(arrivee)) return true;

            var visites = new HashSet<Noeud<int>>();
            var file = new Queue<Noeud<int>>();

            file.Enqueue(depart);
            visites.Add(depart);

            while (file.Count > 0)
            {
                var courant = file.Dequeue();

                if (courant.Equals(arrivee))
                    return true;

                foreach (var voisin in graphe[courant])
                {
                    if (!visites.Contains(voisin))
                    {
                        file.Enqueue(voisin);
                        visites.Add(voisin);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Ajoute des connexions supplémentaires au graphe pour assurer la connectivité entre toutes les stations.
        /// </summary>
        public static void AjouterConnexionsSupplementaires(List<Station> stations, List<Noeud<int>> noeuds, List<Lien<int>> liens, Dictionary<Noeud<int>, List<Noeud<int>>> grapheAdjacence)
        {
            var mapIdToNoeud = noeuds.ToDictionary(n => n.Id);

            var stationsParLigne = stations.GroupBy(s => s.Ligne).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var ligne in stationsParLigne.Keys)
            {
                var stationsDeLigne = stationsParLigne[ligne];

                for (int i = 0; i < stationsDeLigne.Count; i++)
                {
                    for (int j = i + 1; j < stationsDeLigne.Count; j++)
                    {
                        var station1 = stationsDeLigne[i];
                        var station2 = stationsDeLigne[j];

                        if (mapIdToNoeud.ContainsKey(station1.Id) && mapIdToNoeud.ContainsKey(station2.Id))
                        {
                            var noeud1 = mapIdToNoeud[station1.Id];
                            var noeud2 = mapIdToNoeud[station2.Id];

                            if (!grapheAdjacence[noeud1].Contains(noeud2))
                            {
                                double distance = Haversine(station1.Latitude, station1.Longitude, station2.Latitude, station2.Longitude);
                                int poids = Math.Max((int)(distance * 1000), 1);

                                liens.Add(new Lien<int>(noeud1, noeud2, poids));
                                grapheAdjacence[noeud1].Add(noeud2);
                                grapheAdjacence[noeud2].Add(noeud1);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < stations.Count; i++)
            {
                for (int j = i + 1; j < stations.Count; j++)
                {
                    var station1 = stations[i];
                    var station2 = stations[j];

                    if (mapIdToNoeud.ContainsKey(station1.Id) && mapIdToNoeud.ContainsKey(station2.Id))
                    {
                        var noeud1 = mapIdToNoeud[station1.Id];
                        var noeud2 = mapIdToNoeud[station2.Id];

                        if (!grapheAdjacence[noeud1].Contains(noeud2))
                        {
                            double distance = Haversine(station1.Latitude, station1.Longitude, station2.Latitude, station2.Longitude);

                            if (distance < 0.5)
                            {
                                int poids = Math.Max((int)(distance * 1000), 1);

                                liens.Add(new Lien<int>(noeud1, noeud2, poids));
                                grapheAdjacence[noeud1].Add(noeud2);
                                grapheAdjacence[noeud2].Add(noeud1);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Affichage du chemin et coloration du graphe métro
        /// <summary>
        /// Affiche le chemin trouvé entre deux stations avec des couleurs différentes pour chaque ligne de métro et indique les changements.
        /// </summary>
        public static void AfficherChemin(List<Noeud<int>> chemin, List<Station> stations)
        {
            if (chemin.Count <= 1)
            {
                Console.WriteLine("Chemin vide ou incomplet.");
                return;
            }
         
            var stationsParId = stations.ToDictionary(s => s.Id);
            
            int nbStations = chemin.Count;
            int nbChangements = 0;
            string lignePrecedente = "";

            for (int i = 0; i < chemin.Count; i++)
            {
                int idStation = chemin[i].Id;
                if (stationsParId.TryGetValue(idStation, out Station station))
                {
                    if (i > 0 && station.Ligne != lignePrecedente)
                    {
                        nbChangements++;
                    }
                    lignePrecedente = station.Ligne;
                }
            }
        
            Console.Clear();
            Console.WriteLine($"Le chemin le plus court comporte {nbStations} stations avec {nbChangements} changement(s) :\n");
        
            var couleursLignes = new Dictionary<string, ConsoleColor>
            {
                {"1", ConsoleColor.Yellow},
                {"2", ConsoleColor.DarkBlue},
                {"3", ConsoleColor.Green},
                {"3bis", ConsoleColor.Cyan},
                {"4", ConsoleColor.Magenta},
                {"5", ConsoleColor.DarkMagenta},
                {"6", ConsoleColor.DarkCyan},
                {"7", ConsoleColor.DarkGreen},
                {"7bis", ConsoleColor.Gray},
                {"8", ConsoleColor.DarkYellow},
                {"9", ConsoleColor.DarkRed},
                {"10", ConsoleColor.Red},
                {"11", ConsoleColor.DarkGray},
                {"12", ConsoleColor.Blue},
                {"13", ConsoleColor.Cyan},
                {"14", ConsoleColor.Magenta}
            };

            ConsoleColor couleurParDefaut = ConsoleColor.White;
        
            ConsoleColor couleurOriginale = Console.ForegroundColor;
    
            lignePrecedente = "";

            for (int i = 0; i < chemin.Count; i++)
            {
                int idStation = chemin[i].Id;
                if (stationsParId.TryGetValue(idStation, out Station station))
                {      
                    if (station.Ligne != lignePrecedente)
                    {
                        if (couleursLignes.TryGetValue(station.Ligne, out ConsoleColor couleurLigne))
                        {
                            Console.ForegroundColor = couleurLigne;
                        }
                        else
                        {
                            Console.ForegroundColor = couleurParDefaut;
                        }
              
                        if (i > 0)
                        {
                            int idStationPrecedente = chemin[i - 1].Id;
                            if (stationsParId.TryGetValue(idStationPrecedente, out Station stationPrecedente))
                            {
                                Console.ForegroundColor = couleurLigne;
                                Console.Write($"\n\n (Changement de la ligne {stationPrecedente.Ligne} à la {station.Ligne}) \n");
                            }
                        }
                    }

                    Console.Write($"{station.Nom}");

                    if (i < chemin.Count - 1)
                    {
                        Console.Write(" -> ");
                    }

                    lignePrecedente = station.Ligne;
                
                    if (couleursLignes.TryGetValue(lignePrecedente, out ConsoleColor couleurLigneSuivante))
                    {
                        Console.ForegroundColor = couleurLigneSuivante;
                    }
                    else
                    {
                        Console.ForegroundColor = couleurParDefaut;
                    }
                }
            }         
            Console.ForegroundColor = couleurOriginale;
            Console.WriteLine();
        }
        #endregion

        #region Algo de Kruskal
        /// <summary>
        /// Implémente l'algorithme de Kruskal pour trouver l'arbre couvrant minimal d'un graphe, en sélectionnant les arêtes de poids minimal sans créer de cycle.
        /// </summary>
        public static List<Noeud<int>> KruskalArbreCouvranceMinimal(Noeud<int> depart, Noeud<int> arrivee, List<Noeud<int>> noeuds, List<Lien<int>> liens)
        {

            Dictionary<Noeud<int>, Noeud<int>> parent = new Dictionary<Noeud<int>, Noeud<int>>();
            Dictionary<Noeud<int>, int> rang = new Dictionary<Noeud<int>, int>();


            void MakeSet(Noeud<int> noeud)
            {
                parent[noeud] = noeud;
                rang[noeud] = 0;
            }


            Noeud<int> Find(Noeud<int> noeud)
            {
                if (!parent[noeud].Equals(noeud))
                    parent[noeud] = Find(parent[noeud]);
                return parent[noeud];
            }

            void Union(Noeud<int> x, Noeud<int> y)
            {
                Noeud<int> racineX = Find(x);
                Noeud<int> racineY = Find(y);

                if (racineX.Equals(racineY))
                    return;

                if (rang[racineX] < rang[racineY])
                    parent[racineX] = racineY;
                else if (rang[racineX] > rang[racineY])
                    parent[racineY] = racineX;
                else
                {
                    parent[racineY] = racineX;
                    rang[racineX]++;
                }
            }

            var arbreCouvranceMinimal = new List<Lien<int>>();
            var noeudsACM = new HashSet<Noeud<int>>();

            foreach (var noeud in noeuds)
            {
                MakeSet(noeud);
            }

            var liensTriesParPoids = liens.OrderBy(l => l.Poids).ToList();

            foreach (var lien in liensTriesParPoids)
            {
                Noeud<int> racine1 = Find(lien.Noeud1);
                Noeud<int> racine2 = Find(lien.Noeud2);


                if (!racine1.Equals(racine2))
                {
                    arbreCouvranceMinimal.Add(lien);
                    Union(lien.Noeud1, lien.Noeud2);


                    noeudsACM.Add(lien.Noeud1);
                    noeudsACM.Add(lien.Noeud2);
                }
            }

            if (noeudsACM.Count == 0 || !noeudsACM.Contains(depart) || !noeudsACM.Contains(arrivee))
            {
                Console.WriteLine("Impossible de construire un arbre couvrant minimal incluant les stations de départ et d'arrivée.");
                return new List<Noeud<int>>();
            }

            var grapheACM = new Dictionary<Noeud<int>, List<Noeud<int>>>();
            foreach (var noeud in noeudsACM)
            {
                grapheACM[noeud] = new List<Noeud<int>>();
            }

            foreach (var lien in arbreCouvranceMinimal)
            {
                grapheACM[lien.Noeud1].Add(lien.Noeud2);
                grapheACM[lien.Noeud2].Add(lien.Noeud1);
            }

            var chemin = new List<Noeud<int>>();
            var visites = new HashSet<Noeud<int>>();
            var predecesseurs = new Dictionary<Noeud<int>, Noeud<int>>();
            var file = new Queue<Noeud<int>>();

            file.Enqueue(depart);
            visites.Add(depart);

            bool cheminTrouve = false;
            while (file.Count > 0 && !cheminTrouve)
            {
                var courant = file.Dequeue();

                if (courant.Equals(arrivee))
                {
                    cheminTrouve = true;
                    break;
                }

                foreach (var voisin in grapheACM[courant])
                {
                    if (!visites.Contains(voisin))
                    {
                        file.Enqueue(voisin);
                        visites.Add(voisin);
                        predecesseurs[voisin] = courant;
                    }
                }
            }

            if (cheminTrouve)
            {
                var noeudActuel = arrivee;
                while (!noeudActuel.Equals(depart))
                {
                    chemin.Insert(0, noeudActuel);
                    noeudActuel = predecesseurs[noeudActuel];
                }
                chemin.Insert(0, depart);
            }
            else
            {
                Console.WriteLine("Aucun chemin n'a été trouvé entre ces deux stations dans l'arbre couvrant minimal.");
            }
            return chemin;
        }
        #endregion
    }
}

