#region Ancienne version pour afficher comme avant
/*using System;
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
*/
#endregion

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
            : base() // Appelle le constructeur de Graphe<int>()
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

        #region Obtenir les stations et les regrouper par nom
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

        #region Chargement des stations 
        public int ChargerStations()
        {
            // Utiliser CultureInfo.InvariantCulture pour forcer le point comme séparateur décimal
            var culture = CultureInfo.InvariantCulture;
            stations.Clear(); // S'assurer que le dictionnaire est vide avant chargement

            try
            {
                if (!File.Exists(cheminFichierNoeuds))
                {
                    Console.WriteLine($"Erreur : Le fichier {cheminFichierNoeuds} n'existe pas.");
                    return 0;
                }

                var lignes = File.ReadLines(cheminFichierNoeuds).Skip(1); // Skip header row

                foreach (string ligne in lignes)
                {
                    if (string.IsNullOrWhiteSpace(ligne)) continue; // Ignorer lignes vides

                    string[] donnees = ligne.Split(',');
                    if (donnees.Length >= 7)
                    {
                        string idStr = donnees[0].Trim().Trim('"');
                        string numLigne = donnees[1].Trim().Trim('"');
                        string nomStation = donnees[2].Trim().Trim('"');
                        // --- MODIFICATION PARSING DOUBLE ---
                        // Remplacer la virgule par un point OU utiliser CultureInfo appropriée
                        string longitudeStr = donnees[3].Trim().Trim('"').Replace(',', '.'); // Assure le point comme séparateur
                        string latitudeStr = donnees[4].Trim().Trim('"').Replace(',', '.');  // Assure le point comme séparateur                                                                                    
                        string commune = donnees[5].Trim().Trim('"');
                        string codeInsee = donnees[6].Trim().Trim('"');

                        if (int.TryParse(idStr, out int id) &&
                            double.TryParse(longitudeStr, NumberStyles.Any, culture, out double longitude) && // Utilise la culture Invariante
                            double.TryParse(latitudeStr, NumberStyles.Any, culture, out double latitude)) // Utilise la culture Invariante
                        {
                            if (!stations.ContainsKey(id)) // Éviter doublons d'ID
                            {
                                Station station = new Station(id, numLigne, nomStation, longitude, latitude, commune, codeInsee);
                                stations.Add(id, station);

                                // Ajouter le nœud à la liste d'adjacence si absent (important pour Graphe<T>)
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
                Console.WriteLine(ex.StackTrace); // Afficher plus de détails pour le débogage
                return 0;
            }
        }
        #endregion

        #region Chargement des arcs
        public int ChargerArcs()
        {
            int nbLiensCrees = 0;
            var culture = CultureInfo.InvariantCulture; // Pour parser le temps

            try
            {
                if (!File.Exists(cheminFichierArcs))
                {
                    Console.WriteLine($"Erreur : Le fichier {cheminFichierArcs} n'existe pas.");
                    return 0;
                }

                var lignes = File.ReadLines(cheminFichierArcs).Skip(1); // Skip header row

                foreach (string ligne in lignes)
                {
                    if (string.IsNullOrWhiteSpace(ligne)) continue;

                    string[] donnees = ligne.Split(',');
                    if (donnees.Length >= 6) // Assurez-vous que l'indice 5 (temps) existe
                    {
                        string idStr = donnees[0].Trim().Trim('"');
                        string precedentStr = donnees[2].Trim().Trim('"');
                        string suivantStr = donnees[3].Trim().Trim('"');
                        string tempsStr = donnees[5].Trim().Trim('"').Replace(',', '.'); // Indice 5 pour 'average_time_s', assure le point

                        if (int.TryParse(idStr, out int stationId) && stations.ContainsKey(stationId)) // Vérifier si la station principale existe
                        {
                            // Essayer de parser le temps
                            double tempsParcours = 0; // Temps par défaut si parsing échoue ou non applicable
                            if (double.TryParse(tempsStr, NumberStyles.Any, culture, out double parsedTime))
                            {
                                tempsParcours = parsedTime;
                            }
                            // Note: Le poids est basé sur 'average_time_s'
                            int poids = (int)(tempsParcours); // Convertir en secondes (si c'était déjà en secondes, pas besoin de * 60)

                            // Traiter le lien vers la station précédente
                            if (precedentStr != "X" && int.TryParse(precedentStr, out int precedentId))
                            {
                                if (stations.ContainsKey(precedentId)) // Vérifier si la station précédente existe
                                {
                                    // Ajouter le lien dans le graphe de base
                                    AjouterLien(stationId, precedentId, poids); // Utiliser la surcharge avec poids
                                    nbLiensCrees++;
                                }
                                else { Console.WriteLine($"Avertissement: Station précédente inconnue {precedentId} pour station {stationId}"); }
                            }

                            // Traiter le lien vers la station suivante
                            if (suivantStr != "X" && int.TryParse(suivantStr, out int suivantId))
                            {
                                if (stations.ContainsKey(suivantId)) // Vérifier si la station suivante existe
                                {
                                    // Ajouter le lien dans le graphe de base
                                    AjouterLien(stationId, suivantId, poids); // Utiliser la surcharge avec poids
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
                // Note: nbLiensCrees compte les appels à AjouterLien. Le nombre final dans LiensNoeuds peut être différent si AjouterLien gère les doublons.
                Console.WriteLine($"Appels à AjouterLien effectués: {nbLiensCrees}. Nombre final de liens dans Graphe.LiensNoeuds: {this.LiensNoeuds.Count}");
                return this.LiensNoeuds.Count; // Retourne le nombre réel de liens dans la liste
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des arcs : {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return 0;
            }
        }
        #endregion

        #region Ajout de lien entre noeuds
        /// <summary>
        /// Ajoute un lien pondéré entre deux nœuds.
        /// </summary>
        public void AjouterLien(int noeud1, int noeud2, int poids)
        {
            if (!ListeAdjacence.ContainsKey(noeud1)) ListeAdjacence[noeud1] = new List<int>();
            if (!ListeAdjacence.ContainsKey(noeud2)) ListeAdjacence[noeud2] = new List<int>();

            // Ajout à la liste d'adjacence (non-orienté)
            if (!ListeAdjacence[noeud1].Contains(noeud2)) ListeAdjacence[noeud1].Add(noeud2);
            if (!ListeAdjacence[noeud2].Contains(noeud1)) ListeAdjacence[noeud2].Add(noeud1);

            // Ajout à la liste des liens (évite les doublons exacts de paires non ordonnées si possible)
            // Note: La vérification de doublon ici est basique. Si A->B puis B->A sont ajoutés, les deux seront présents.
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

        #region Algo    
        /// <summary>
        /// Implémente l'algorithme de Welsh-Powell pour colorer un graphe.
        /// Cet algorithme attribue des couleurs aux sommets de manière à ce que deux sommets adjacents
        /// n'aient jamais la même couleur, tout en essayant de minimiser le nombre de couleurs utilisées.
        /// </summary>
        /// <returns>Un dictionnaire associant chaque sommet à sa couleur (représentée par un entier).</returns>
        public Dictionary<int, int> ColorerGrapheWelshPowell()
        {
            Console.WriteLine("Début de la coloration de Welsh-Powell...");

            // Dictionnaire pour stocker la couleur de chaque sommet
            Dictionary<int, int> couleurs = new Dictionary<int, int>();

            // Initialiser tous les sommets avec la couleur -1 (non coloré)
            foreach (var sommet in Stations.Keys)
            {
                couleurs[sommet] = -1;
            }

            // Créer un dictionnaire pour stocker le degré de chaque sommet
            Dictionary<int, int> degres = new Dictionary<int, int>();

            // Calculer le degré de chaque sommet
            foreach (var sommet in Stations.Keys)
            {
                // Compter le nombre de liens pour ce sommet
                int degre = 0;
                foreach (var lien in LiensNoeuds)
                {
                    if (lien.Noeud1.Id == sommet || lien.Noeud2.Id == sommet)
                    {
                        degre++;
                    }
                }
                degres[sommet] = degre;

                // Debug
                Console.WriteLine($"Station {sommet} ({Stations[sommet].Nom}) a un degré de {degre}");
            }

            // Trier les sommets par ordre décroissant de degré
            List<int> sommetsTries = Stations.Keys.ToList();
            sommetsTries.Sort((a, b) => degres[b].CompareTo(degres[a]));

            // Debug
            Console.WriteLine("Ordre de traitement des stations:");
            foreach (var sommet in sommetsTries)
            {
                Console.WriteLine($"Station {sommet} ({Stations[sommet].Nom}) - Degré: {degres[sommet]}");
            }

            // Couleur actuelle
            int couleurActuelle = 0;

            // Tant qu'il reste des sommets non colorés
            while (sommetsTries.Any(s => couleurs[s] == -1))
            {
                Console.WriteLine($"Traitement de la couleur {couleurActuelle}");

                // Pour chaque sommet non coloré dans l'ordre de degré décroissant
                foreach (var sommet in sommetsTries)
                {
                    // Si le sommet n'est pas encore coloré
                    if (couleurs[sommet] == -1)
                    {
                        // Vérifier si on peut attribuer la couleur actuelle
                        bool peutColorer = true;

                        // Récupérer tous les voisins du sommet
                        List<int> voisins = new List<int>();
                        foreach (var lien in LiensNoeuds)
                        {
                            if (lien.Noeud1.Id == sommet)
                            {
                                voisins.Add(lien.Noeud2.Id);
                            }
                            else if (lien.Noeud2.Id == sommet)
                            {
                                voisins.Add(lien.Noeud1.Id);
                            }
                        }

                        // Vérifier tous les voisins du sommet
                        foreach (var voisin in voisins)
                        {
                            // Si un voisin a déjà la couleur actuelle, on ne peut pas l'utiliser
                            if (couleurs.ContainsKey(voisin) && couleurs[voisin] == couleurActuelle)
                            {
                                peutColorer = false;
                                Console.WriteLine($"  Station {sommet} ({Stations[sommet].Nom}) ne peut pas être colorée en {couleurActuelle} car voisine de {voisin} ({Stations[voisin].Nom})");
                                break;
                            }
                        }

                        // Si aucun voisin n'a la couleur actuelle, on l'attribue à ce sommet
                        if (peutColorer)
                        {
                            couleurs[sommet] = couleurActuelle;
                            Console.WriteLine($"  Station {sommet} ({Stations[sommet].Nom}) colorée en {couleurActuelle}");
                        }
                    }
                }

                // Passer à la couleur suivante
                couleurActuelle++;
            }

            // Vérification finale
            int erreurs = 0;
            foreach (var lien in LiensNoeuds)
            {
                int id1 = lien.Noeud1.Id;
                int id2 = lien.Noeud2.Id;

                if (couleurs[id1] == couleurs[id2])
                {
                    erreurs++;
                    Console.WriteLine($"ERREUR: Stations voisines avec même couleur: {id1} ({Stations[id1].Nom}) et {id2} ({Stations[id2].Nom}) - Couleur: {couleurs[id1]}");
                }
            }

            Console.WriteLine($"Coloration terminée. {couleurActuelle} couleurs utilisées. {erreurs} erreurs détectées.");

            return couleurs;
        }
        #endregion
    }
}