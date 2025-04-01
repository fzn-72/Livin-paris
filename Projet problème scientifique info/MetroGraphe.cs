using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Drawing;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Cette classe représente un graphe spécifique pour le réseau de métro parisien.
    /// Elle hérite de la classe Graphe<int> et ajoute des fonctionnalités spécifiques pour
    /// charger les données des stations et des connexions depuis des fichiers CSV.
    /// </summary>
    public class MetroGraphe : Graphe<int>
    {
        // Dictionnaire pour stocker les stations avec leur ID comme clé
        private Dictionary<int, Station> stations;

        // Chemins vers les fichiers CSV
        private string cheminFichierNoeuds;
        private string cheminFichierArcs;

        /// <summary>
        /// Constructeur de la classe MetroGraphe qui initialise les chemins vers les fichiers CSV.
        /// </summary>
        /// <param name="cheminFichierNoeuds">Chemin vers le fichier CSV des stations</param>
        /// <param name="cheminFichierArcs">Chemin vers le fichier CSV des arcs</param>
        public MetroGraphe(string cheminFichierNoeuds, string cheminFichierArcs)
        {
            this.cheminFichierNoeuds = cheminFichierNoeuds;
            this.cheminFichierArcs = cheminFichierArcs;
            this.stations = new Dictionary<int, Station>();
        }

        /// <summary>
        /// Propriété permettant d'accéder au dictionnaire des stations
        /// </summary>
        public Dictionary<int, Station> Stations
        {
            get { return stations; }
        }

        /// <summary>
        /// Méthode qui charge les stations depuis le fichier CSV et les ajoute au dictionnaire des stations.
        /// </summary>
        /// <returns>Le nombre de stations chargées</returns>
        public int ChargerStations()
        {
            try
            {
                // Vérifier si le fichier existe
                if (!File.Exists(cheminFichierNoeuds))
                {
                    Console.WriteLine($"Erreur : Le fichier {cheminFichierNoeuds} n'existe pas.");
                    return 0;
                }

                string[] lignes = File.ReadAllLines(cheminFichierNoeuds);
                Console.WriteLine($"Nombre de lignes dans le fichier : {lignes.Length}");

                // On ignore la première ligne (en-tête)
                for (int i = 1; i < lignes.Length; i++)
                {
                    try
                    {
                        string ligne = lignes[i];

                        // Diviser la ligne en utilisant la virgule comme séparateur
                        string[] donnees = ligne.Split(',');

                        if (donnees.Length >= 7)
                        {
                            // Nettoyer et parser chaque élément
                            string idStr = donnees[0].Trim().Trim('"');
                            string numLigne = donnees[1].Trim().Trim('"');
                            string nomStation = donnees[2].Trim().Trim('"');
                            string longitudeStr = donnees[3].Trim().Trim('"').Replace('.', ',');
                            string latitudeStr = donnees[4].Trim().Trim('"').Replace('.', ',');
                            string commune = donnees[5].Trim().Trim('"');
                            string codeInsee = donnees[6].Trim().Trim('"');

                            // Tenter de parser les valeurs numériques
                            if (!int.TryParse(idStr, out int id))
                            {
                                Console.WriteLine($"Erreur de parsing de l'ID: {idStr}");
                                continue;
                            }

                            if (!double.TryParse(longitudeStr, out double longitude))
                            {
                                Console.WriteLine($"Erreur de parsing de la longitude: {longitudeStr}");
                                continue;
                            }

                            if (!double.TryParse(latitudeStr, out double latitude))
                            {
                                Console.WriteLine($"Erreur de parsing de la latitude: {latitudeStr}");
                                continue;
                            }

                            // Créer et ajouter la station
                            Station station = new Station(id, numLigne, nomStation, longitude, latitude, commune, codeInsee);
                            stations.Add(id, station);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors du traitement de la ligne {i}: {ex.Message}");
                    }
                }

                Console.WriteLine($"Nombre de stations chargées : {stations.Count}");
                return stations.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des stations : {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Méthode qui charge les arcs depuis le fichier CSV et crée les liens entre les stations.
        /// </summary>
        /// <returns>Le nombre de liens créés</returns>
        public int ChargerArcs()
        {
            int nbLiensCrees = 0;

            try
            {
                // Vérifier si le fichier existe
                if (!File.Exists(cheminFichierArcs))
                {
                    Console.WriteLine($"Erreur : Le fichier {cheminFichierArcs} n'existe pas.");
                    return 0;
                }

                string[] lignes = File.ReadAllLines(cheminFichierArcs);

                // On ignore la première ligne (en-tête)
                for (int i = 1; i < lignes.Length; i++)
                {
                    try
                    {
                        string ligne = lignes[i];

                        // Diviser la ligne en utilisant la virgule comme séparateur
                        string[] donnees = ligne.Split(',');

                        if (donnees.Length >= 6)
                        {
                            string idStr = donnees[0].Trim().Trim('"');
                            string precedent = donnees[2].Trim().Trim('"');
                            string suivant = donnees[3].Trim().Trim('"');
                            string tempsStr = donnees[4].Trim().Trim('"');

                            if (!int.TryParse(idStr, out int stationId))
                            {
                                Console.WriteLine($"Erreur de parsing de l'ID de station: {idStr}");
                                continue;
                            }

                            // Vérifier si la station a un précédent
                            if (precedent != "X" && int.TryParse(precedent, out int idPrecedent))
                            {
                                // Récupérer le temps entre les stations
                                if (tempsStr != "" && int.TryParse(tempsStr, out int tempsParcours))
                                {
                                    // Ajouter le lien avec le poids (temps de parcours)
                                    AjouterLienPondere(idPrecedent, stationId, tempsParcours);
                                    nbLiensCrees++;
                                }
                            }

                            // Vérifier si la station a un suivant
                            if (suivant != "X" && int.TryParse(suivant, out int idSuivant))
                            {
                                // Récupérer le temps entre les stations
                                if (tempsStr != "" && int.TryParse(tempsStr, out int tempsParcours))
                                {
                                    // Ajouter le lien avec le poids (temps de parcours)
                                    AjouterLienPondere(stationId, idSuivant, tempsParcours);
                                    nbLiensCrees++;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors du traitement de la ligne {i}: {ex.Message}");
                    }
                }

                Console.WriteLine($"Nombre de liens créés : {nbLiensCrees}");
                return nbLiensCrees;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des arcs : {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Méthode qui ajoute un lien pondéré entre deux stations dans le graphe.
        /// </summary>
        /// <param name="idStation1">ID de la première station</param>
        /// <param name="idStation2">ID de la deuxième station</param>
        /// <param name="poids">Poids du lien (temps de parcours)</param>
        private void AjouterLienPondere(int idStation1, int idStation2, int poids)
        {
            // Vérifier si les stations existent dans la liste d'adjacence
            if (!ListeAdjacence.ContainsKey(idStation1))
            {
                ListeAdjacence[idStation1] = new List<int>();
            }
            if (!ListeAdjacence.ContainsKey(idStation2))
            {
                ListeAdjacence[idStation2] = new List<int>();
            }

            // Ajouter les liens dans la liste d'adjacence
            if (!ListeAdjacence[idStation1].Contains(idStation2))
            {
                ListeAdjacence[idStation1].Add(idStation2);
            }
            if (!ListeAdjacence[idStation2].Contains(idStation1))
            {
                ListeAdjacence[idStation2].Add(idStation1);
            }

            // Créer les objets Noeud pour les stations
            Noeud<int> noeud1 = new Noeud<int>(idStation1, idStation1);
            Noeud<int> noeud2 = new Noeud<int>(idStation2, idStation2);

            // Créer le lien pondéré
            Lien<int> lien = new Lien<int>(noeud1, noeud2, poids);

            // Ajouter le lien à la liste des liens du graphe
            LiensNoeuds.Add(lien);
        }

        /// <summary>
        /// Méthode qui construit le graphe complet du métro en chargeant les stations et les arcs.
        /// </summary>
        /// <returns>True si le graphe a été construit avec succès, False sinon</returns>
        public bool ConstruireGraphe()
        {
            try
            {
                int nbStations = ChargerStations();
                if (nbStations == 0)
                {
                    Console.WriteLine("Erreur : Aucune station n'a été chargée.");
                    return false;
                }

                int nbLiens = ChargerArcs();
                if (nbLiens == 0)
                {
                    Console.WriteLine("Erreur : Aucun lien n'a été créé.");
                    return false;
                }

                Console.WriteLine($"Graphe construit avec succès : {nbStations} stations et {nbLiens} liens.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la construction du graphe : {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Méthode qui visualise le graphe du métro en générant une image.
        /// </summary>
        /// <param name="cheminImage">Chemin où sauvegarder l'image générée</param>
        /// <param name="largeur">Largeur de l'image en pixels</param>
        /// <param name="hauteur">Hauteur de l'image en pixels</param>
        /// <returns>True si la visualisation a réussi, False sinon</returns>
        public bool VisualiserGraphe(string cheminImage, int largeur = 1200, int hauteur = 800)
        {
            try
            {
                // Créer une instance de VisualisationGraphe avec notre graphe
                VisualisationGraphe<int> visualisation = new VisualisationGraphe<int>(this, largeur, hauteur);

                // Dessiner le graphe et sauvegarder l'image
                visualisation.DessinerGraphe(cheminImage);

                Console.WriteLine($"Le graphe du métro a été dessiné et sauvegardé sous '{cheminImage}'.");

                // Ouvrir l'image
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(cheminImage) { UseShellExecute = true });
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de l'ouverture de l'image : {ex.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la visualisation du graphe : {ex.Message}");
                return false;
            }
        }
    }
}