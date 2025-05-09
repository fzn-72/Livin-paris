using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Drawing;

namespace Projet_problème_scientifique_info
{
    public class MetroGraphe : Graphe<int>
    {
        private Dictionary<int, Station> stations;
        private string cheminFichierNoeuds;
        private string cheminFichierArcs;

        #region Constructeur de la classe MetroGraphe qui fait appel à celui de la classe Graphe
        public MetroGraphe(string cheminFichierNoeuds, string cheminFichierArcs)
            : base() /// Appelle le constructeur de Graphe<int>()
        {
            this.cheminFichierNoeuds = cheminFichierNoeuds;
            this.cheminFichierArcs = cheminFichierArcs;
            this.stations = new Dictionary<int, Station>();
        }
        #endregion

        public Dictionary<int, Station> Stations
        {
            get
            {
                return stations;
            }
        }

        #region Obtenir les stations et les regrouper par leur nom
        /// <summary>
        /// Retourne un dictionnaire groupant les stations par leur nom unique (ignorant la casse).
        /// </summary>
        public Dictionary<string, List<Station>> GetStationsByName()
        {
            return stations.Values
                .GroupBy(s => s.Nom.Trim(), StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.ToList());
        }
        #endregion

        #region Chargement des stations et des arcs

        /// <summary>
        /// Charge les stations à partir d’un fichier CSV, en extrayant leurs coordonnées, ligne, nom et localisation.
        /// Ignore les doublons, les lignes incomplètes ou mal formatées, et initialise la liste d’adjacence.
        /// Retourne le nombre total de stations valides chargées.
        /// </summary>
        public int ChargerStations()
        {
            var culture = CultureInfo.InvariantCulture; /// permet de forcer le point comme séparateur décimal
            stations.Clear(); 

            try
            {
                if (!File.Exists(cheminFichierNoeuds))
                {
                    Console.WriteLine($"Erreur : Le fichier {cheminFichierNoeuds} n'existe pas.");
                    return 0;
                }

                var lignes = File.ReadLines(cheminFichierNoeuds).Skip(1);

                foreach (string ligne in lignes)
                {
                    if (string.IsNullOrWhiteSpace(ligne)) continue; /// permet d'ignorer les lignes vides

                    string[] donnees = ligne.Split(',');
                    if (donnees.Length >= 7)
                    {
                        string idStr = donnees[0].Trim().Trim('"');
                        string numLigne = donnees[1].Trim().Trim('"');
                        string nomStation = donnees[2].Trim().Trim('"');

                        string longitudeStr = donnees[3].Trim().Trim('"').Replace(',', '.'); /// Permet d'assurer le point comme séparateur
                        string latitudeStr = donnees[4].Trim().Trim('"').Replace(',', '.');                                                                                     
                        string commune = donnees[5].Trim().Trim('"');
                        string codeInsee = donnees[6].Trim().Trim('"');

                        if (int.TryParse(idStr, out int id) &&
                            double.TryParse(longitudeStr, NumberStyles.Any, culture, out double longitude) && 
                            double.TryParse(latitudeStr, NumberStyles.Any, culture, out double latitude)) 
                        {
                            if (!stations.ContainsKey(id)) 
                            {
                                Station station = new Station(id, numLigne, nomStation, longitude, latitude, commune, codeInsee);
                                stations.Add(id, station);

                                if (!ListeAdjacence.ContainsKey(id))
                                {
                                    ListeAdjacence[id] = new List<int>();
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Avertissement: ID de station dupliqué ignoré: {id} ({nomStation})");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Erreur de parsing sur la ligne station: {ligne}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Avertissement: Ligne station incomplète ignorée: {ligne}");
                    }
                }
                Console.WriteLine($"Nombre de stations chargées : {stations.Count}");
                return stations.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des stations : {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return 0;
            }
        }

        /// <summary>
        /// Charge les arcs entre les stations à partir d'un fichier CSV. Chaque ligne valide crée des liens avec les stations 
        /// précédentes et suivantes si elles existent. Gère les erreurs de format, les lignes incomplètes et les stations inconnues.
        /// Retourne le nombre total de liens créés dans le graphe.
        /// </summary>
        public int ChargerArcs()
        {
            int nbLiensCrees = 0;
            var culture = CultureInfo.InvariantCulture;

            try
            {
                if (!File.Exists(cheminFichierArcs))
                {
                    Console.WriteLine($"Erreur : Le fichier {cheminFichierArcs} n'existe pas.");
                    return 0;
                }

                var lignes = File.ReadLines(cheminFichierArcs).Skip(1);

                foreach (string ligne in lignes)
                {
                    if (string.IsNullOrWhiteSpace(ligne)) continue;

                    string[] donnees = ligne.Split(',');
                    if (donnees.Length >= 6)
                    {
                        string idStr = donnees[0].Trim().Trim('"');
                        string precedentStr = donnees[2].Trim().Trim('"');
                        string suivantStr = donnees[3].Trim().Trim('"');
                        string tempsStr = donnees[5].Trim().Trim('"').Replace(',', '.');

                        if (int.TryParse(idStr, out int stationId) && stations.ContainsKey(stationId)) /// Vérifier si la station principale existe
                        {
                            double tempsParcours = 0;
                            if (double.TryParse(tempsStr, NumberStyles.Any, culture, out double parsedTime))
                            {
                                tempsParcours = parsedTime;
                            }

                            int poids = (int)(tempsParcours);

                            if (precedentStr != "X" && int.TryParse(precedentStr, out int precedentId))
                            {
                                if (stations.ContainsKey(precedentId)) /// Vérifier si la station précédente existe
                                {
                                    AjouterLien(stationId, precedentId, poids);
                                    nbLiensCrees++;
                                }
                                else { Console.WriteLine($"Avertissement: Station précédente inconnue {precedentId} pour station {stationId}"); }
                            }

                            if (suivantStr != "X" && int.TryParse(suivantStr, out int suivantId))
                            {
                                if (stations.ContainsKey(suivantId)) /// Vérifier si la station suivante existe
                                {
                                    AjouterLien(stationId, suivantId, poids);
                                    nbLiensCrees++;
                                }
                                else { Console.WriteLine($"Avertissement: Station suivante inconnue {suivantId} pour station {stationId}"); }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Erreur parsing ID ou station inconnue sur ligne arc: {ligne}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Avertissement: Ligne arc incomplète ignorée: {ligne}");
                    }
                }

                Console.WriteLine($"Appels à AjouterLien effectués: {nbLiensCrees}. Nombre final de liens dans Graphe.LiensNoeuds: {this.LiensNoeuds.Count}");
                return this.LiensNoeuds.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des arcs : {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return 0;
            }
        }
        #endregion

        #region Ajout de lien entre les noeuds

        /// <summary>
        /// Ajoute un lien pondéré entre deux nœuds.
        /// </summary>
        public void AjouterLien(int noeud1, int noeud2, int poids)
        {
            if (!ListeAdjacence.ContainsKey(noeud1)) ListeAdjacence[noeud1] = new List<int>();
            if (!ListeAdjacence.ContainsKey(noeud2)) ListeAdjacence[noeud2] = new List<int>();

            if (!ListeAdjacence[noeud1].Contains(noeud2)) ListeAdjacence[noeud1].Add(noeud2);
            if (!ListeAdjacence[noeud2].Contains(noeud1)) ListeAdjacence[noeud2].Add(noeud1);

            bool lienExiste = LiensNoeuds.Any(l =>
                (l.Noeud1.Id == noeud1 && l.Noeud2.Id == noeud2) ||
                (l.Noeud1.Id == noeud2 && l.Noeud2.Id == noeud1)
            );

            Noeud<int> n1 = new Noeud<int>(noeud1, noeud1);
            Noeud<int> n2 = new Noeud<int>(noeud2, noeud2);
            Lien<int> nouveauLien = new Lien<int>(n1, n2, poids);
            LiensNoeuds.Add(nouveauLien);  
        }
        #endregion

    }
}