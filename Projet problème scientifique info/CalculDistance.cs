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
        public static void DistanceEntreDeuxStations()
        {
            string filePath = @"C:\Users\fzzn7\Documents\GitHub\PSI_2025_ThoFaizMart\Feuille_noeud_convertie.csv";

            Console.Write("Station 1 : ");
            string station1 = Console.ReadLine();
            Console.Write("Station 2 : ");
            string station2 = Console.ReadLine();

            (double lat1, double lon1) = GetStationCoordinates(filePath, station1);
            (double lat2, double lon2) = GetStationCoordinates(filePath, station2);

            if (lat1 != 0 && lat2 != 0)
            {
                double distance = Haversine(lat1, lon1, lat2, lon2);
                Console.WriteLine($"Distance entre {station1} et {station2} : {distance:F2} km");
            }
            else
            {
                Console.WriteLine("Une ou plusieurs stations introuvables !");
            }
        }

        static (double, double) GetStationCoordinates(string filePath, string stationName)
        {
            using (var sr = new StreamReader(filePath))
            {
                string line;
                bool isFirstLine = true;

                while ((line = sr.ReadLine()) != null)
                {
                    if (isFirstLine) { isFirstLine = false; continue; } // Ignore l'en-tête

                    var data = line.Split(';'); // Adapter si le séparateur est ','

                    if (data.Length >= 3 && data[0].Trim().Equals(stationName, StringComparison.OrdinalIgnoreCase))
                    {
                        return (double.Parse(data[1]), double.Parse(data[2])); // Latitude, Longitude
                    }
                }
            }
            return (0, 0); // Retourne (0,0) si la station n'existe pas
        }

        static double Haversine(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Rayon de la Terre en km
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; // Distance en km
        }

        static double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }

}

