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
            string chemin = @"C:\Users\thoma\OneDrive\Documents\GitHub\PSI_2025_ThoFaizMart\Feuille_noeud_convertie.csv";

            Console.Write("Station 1 : ");
            string station1 = Console.ReadLine();
            Console.Write("Station 2 : ");
            string station2 = Console.ReadLine();

            (double lat1, double lon1) = GetStationCoordinates(chemin, station1);
            (double lat2, double lon2) = GetStationCoordinates(chemin, station2);

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

        //#region Méthode qui récup les coordonnées
        //static (double, double) GetStationCoordinates(string filePath, string stationName)
        //{
        //    string[] lignes = File.ReadAllLines(filePath);

        //    foreach (string ligne in lignes.Skip(1)) /// On ignore l'en-tête
        //    {
        //        var data = ligne.Split(','); /// Le séparateur (',') car ligne sous la forme suivante : 1,"1","Porte Maillot","2.282583847361549", ...etc

        //        string nomStation = data[2].Trim().Trim('"'); /// On supprime les guillemets s'il y en a
        //        if (nomStation.Equals(stationName, StringComparison.OrdinalIgnoreCase) == false) continue;

        //        if (double.TryParse(data[4], out double latitude) && double.TryParse(data[3], out double longitude))
        //        {
        //            return (latitude, longitude);
        //        }
        //    }

        //    return (0, 0); /// On retourne (0,0) si la station n'existe pas
        //}
        //#endregion

        static (double, double) GetStationCoordinates(string filePath, string stationName)
        {
            string[] lignes = File.ReadAllLines(filePath);

            foreach (string ligne in lignes.Skip(1)) // Ignorer l'en-tête
            {
                var data = ligne.Split(','); // Séparateur correct

                // Nettoyer les guillemets et récupérer le nom de la station
                string nomStation = data[2].Trim().Trim('"');

                if (nomStation.Trim().Equals(stationName.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    // Nettoyer les valeurs et les convertir
                    if (double.TryParse(data[4].Trim('"'), out double latitude) &&
                        double.TryParse(data[3].Trim('"'), out double longitude))
                    {
                        return (latitude, longitude);
                    }
                }
            }

            return (0, 0); // Retourne (0,0) si la station n'existe pas
        }


        #region Méthode qui implémente la formule de Haversine 
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
            double b = Math.Sqrt(a);

            double c = 2 * R * Math.Asin(b);

            return c;
        }

        static double ConversionEnRadian(double degre)
        {
            double radian = Math.PI * degre / 180.0;
            return radian;
        }

        #endregion
    }

}

