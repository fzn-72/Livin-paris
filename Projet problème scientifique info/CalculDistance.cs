using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Cette classe va permettre de lire les coordonnées (latitude, longitude) de deux stations du fichier excel (converti préalablement en CSV) et déterminer, 
    /// grâce à la formule de Haversine, la distance entre ces deux stations. 
    /// </summary>
    internal class CalculDistance
    {

        #region : Affichage de la distance entre deux stations 
        public static void DistanceEntreDeuxStations()
        {
            string chemin = @"C:\Users\thoma\OneDrive\Documents\GitHub\PSI_2025_ThoFaizMart\Feuille_noeud_convertie.csv";

            // Vérifier que le fichier existe
            if (!File.Exists(chemin))
            {
                Console.WriteLine("Erreur: Le fichier " + chemin + " n'existe pas.");
                return;
            }

            Console.WriteLine("Entrez le nom de deux stations dont vous voulez connaître la distance de l'une à l'autre.");
            Console.WriteLine("La saisie se fait sans accents (grave, aigu, circonflexe), sans cédille et sans tréma, et en respectant les espaces");
            Console.WriteLine();
            Console.Write("Station 1 : ");
            string station1 = Console.ReadLine();
            Console.Write("Station 2 : ");
            string station2 = Console.ReadLine();

            // Rechercher les coordonnées des stations
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
        #endregion


        #region : Récupération des coordonnées des stations 
        static (double, double) AcquisitionCoordonnees(string chemin, string stationName)
        {
            try
            {
                string[] lignes = File.ReadAllLines(chemin);

                foreach (string ligne in lignes.Skip(1)) // On ignore l'en-tête
                {
                    var data = ligne.Split(',');
                    if (data.Length < 5)
                    {
                        continue;
                    }
                    string nomStation = data[2].Trim().Trim('"');

                    // Recherche avec correspondance exacte (insensible à la casse)
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
            return (0, 0); // On retourne (0,0) si la station n'existe pas
        }
        #endregion


        #region Calcul de la distance par la formule de Haversine 
        static double Haversine(double lat1, double long1, double lat2, double long2)
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

        static double ConversionEnRadian(double degre)
        {
            return Math.PI * degre / 180.0;
        }
        #endregion


        public static void BellmanFordAlgo(Noeud<string> source, List<Noeud<string>> noeuds, List<Lien<string>> liens)
        {
            // Initialisation des distances
            var distances = new Dictionary<Noeud<string>, double>();
            foreach (var noeud in noeuds)
            {
                distances[noeud] = double.PositiveInfinity;
            }
            distances[source] = 0;

            // Relaxation des arêtes
            for (int i = 1; i < noeuds.Count; i++)
            {
                foreach (var lien in liens)
                {
                    if (distances[lien.Noeud1] + lien.Poids < distances[lien.Noeud2])
                    {
                        distances[lien.Noeud2] = distances[lien.Noeud1] + lien.Poids;
                    }
                    if (distances[lien.Noeud2] + lien.Poids < distances[lien.Noeud1])
                    {
                        distances[lien.Noeud1] = distances[lien.Noeud2] + lien.Poids;
                    }
                }
            }

            // Vérification de la présence de cycles de poids négatif
            foreach (var lien in liens)
            {
                if (distances[lien.Noeud1] + lien.Poids < distances[lien.Noeud2] ||
                    distances[lien.Noeud2] + lien.Poids < distances[lien.Noeud1])
                {
                    Console.WriteLine("Le graphe contient un cycle de poids négatif.");
                    return;
                }
            }

            // Affichage des distances
            foreach (var kvp in distances)
            {
                Console.WriteLine($"Distance de {source.Id} à {kvp.Key.Id}: {kvp.Value}");
            }
        }


        public static List<Noeud<string>> Dijkstra(Noeud<string> depart, Noeud<string> arrivee, List<Noeud<string>> noeuds, List<Lien<string>> liens)
        {
            // Initialisation des distances et des prédécesseurs
            var distances = new Dictionary<Noeud<string>, double>();
            var predecesseurs = new Dictionary<Noeud<string>, Noeud<string>>();
            var nonTraites = new List<Noeud<string>>(noeuds);

            foreach (var noeud in noeuds)
            {
                distances[noeud] = double.PositiveInfinity;
                predecesseurs[noeud] = null;
            }
            distances[depart] = 0;

            // Tant qu'il y a des nœuds non traités
            while (nonTraites.Count > 0)
            {
                // Trouver le nœud non traité avec la distance minimale
                Noeud<string> noeudCourant = nonTraites.OrderBy(n => distances[n]).First();
                nonTraites.Remove(noeudCourant);

                // Si le nœud courant est le nœud d'arrivée, on peut arrêter
                if (noeudCourant.Equals(arrivee))
                {
                    break;
                }

                // Mettre à jour les distances des voisins
                foreach (var lien in liens)
                {
                    if (lien.Noeud1.Equals(noeudCourant) && distances.ContainsKey(lien.Noeud2))
                    {
                        double distance = distances[noeudCourant] + lien.Poids;
                        if (distance < distances[lien.Noeud2])
                        {
                            distances[lien.Noeud2] = distance;
                            predecesseurs[lien.Noeud2] = noeudCourant;
                        }
                    }
                    else if (lien.Noeud2.Equals(noeudCourant) && distances.ContainsKey(lien.Noeud1))
                    {
                        double distance = distances[noeudCourant] + lien.Poids;
                        if (distance < distances[lien.Noeud1])
                        {
                            distances[lien.Noeud1] = distance;
                            predecesseurs[lien.Noeud1] = noeudCourant;
                        }
                    }
                }
            }

            // Reconstruire le chemin
            var chemin = new List<Noeud<string>>();
            for (Noeud<string> noeud = arrivee; noeud != null; noeud = predecesseurs[noeud])
            {
                chemin.Insert(0, noeud);
            }

            return chemin;
        }


    } 
}



