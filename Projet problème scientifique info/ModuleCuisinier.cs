using MySql.Data.MySqlClient;
using Projet_problème_scientifique_info;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Classe gérant les fonctionnalités liées aux cuisiniers
    /// </summary>
    internal class ModuleCuisinier
    {
        #region Menu principal de la classe cuisinier

        /// <summary>
        /// Affiche le menu principal du module cuisinier avec les différentes options disponibles et redirige l'utilisateur vers le menu approprié à son choix
        /// </summary>
        /// 
        public static void MenuPrincipalCuisinier()
        {
            if (!SessionUtilisateur.EstCuisinier() && !SessionUtilisateur.EstAdmin())
            {
                SessionUtilisateur.AfficherMessageDroitsInsuffisants();
                return;
            }

            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("MENU PRINCIPAL CUISINIER");

                if (SessionUtilisateur.EstCuisinier())
                {
                    Console.WriteLine($"Bienvenue {SessionUtilisateur.PrenomUtilisateur} {SessionUtilisateur.NomUtilisateur} !");
                    Console.WriteLine();
                }

                Console.WriteLine("1. Accéder au module de plats");
                Console.WriteLine("2. Accéder au module de commandes");
                Console.WriteLine("3. Gérer mon profil");
                Console.WriteLine("4. Afficher mes statistiques");
                Console.WriteLine("5. Calculer le trajet vers l'adresse d'un client");
                Console.WriteLine("6. Se déconnecter");

                Console.Write("\nChoisissez une option : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        ModulePlat.MenuPrincipalPlats();
                        break;
                    case "2":
                        ModuleCommande.MenuPrincipalCommandes();
                        break;
                    case "3":
                        ModuleProfil.MenuPrincipalProfil();
                        break;
                    case "4":
                        ModuleStats.MenuPrincipalStats();
                        break;
                    case "5":
                        CalculerTrajetVersClient();
                        break;
                    case "6":
                        if (SessionUtilisateur.EstCuisinier())
                        {
                            SessionUtilisateur.Deconnecter();
                        }
                        continuer = false;
                        break;
                    default:
                        GestionBDD.AfficherMessageErreur("Option invalide.");
                        break;
                }
            }
        }
        #endregion

        #region Méthodes pour trouver le chemin optimal entre client et cuisinier

        /// <summary>
        /// Calcule la distance et le chemin optimal en métro entre la station du cuisinier connecté et celle d’un client.
        /// Affiche la liste des clients avec leur station, permet d’en sélectionner un, et utilise l’algorithme choisi.
        /// Affiche un message d’erreur si des informations sont manquantes ou incorrectes.
        /// </summary>
        private static void CalculerTrajetVersClient()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("CALCULER LE TRAJET VERS UN CLIENT");

            try
            {
                string requeteCuisinier = @"
                    SELECT metro_le_plus_proche_cuisinier
                    FROM CUISINIER
                    WHERE id_cuisinier = @IdCuisinier";

                Dictionary<string, object> parametresCuisinier = new Dictionary<string, object>
                {
                    { "@IdCuisinier", SessionUtilisateur.IdUtilisateur }
                };

                object stationCuisinier = GestionBDD.ExecuterScalaire(requeteCuisinier, parametresCuisinier);

                if (stationCuisinier == null || stationCuisinier == DBNull.Value)
                {
                    GestionBDD.AfficherMessageErreur("Vous n'avez pas renseigné de station de métro dans votre profil.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                string stationMetroCuisinier = stationCuisinier.ToString();
                Console.WriteLine($"Votre station de métro : {stationMetroCuisinier}");

                string requeteClients = @"
                    SELECT c.id_client, c.nom_client, c.prenom_client, c.metro_le_plus_proche_client
                    FROM CLIENT c
                    WHERE c.metro_le_plus_proche_client IS NOT NULL AND c.metro_le_plus_proche_client <> ''
                    ORDER BY c.nom_client, c.prenom_client";

                DataTable clients = GestionBDD.ExecuterRequeteTable(requeteClients, null);

                if (clients == null || clients.Rows.Count == 0)
                {
                    GestionBDD.AfficherMessageErreur("Aucun client avec une station de métro renseignée n'a été trouvé.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("\nListe des clients :");
                Console.WriteLine("ID\tNom\t\tPrénom\t\tStation de métro");
                Console.WriteLine("-------------------------------------------------------------------------");

                foreach (DataRow row in clients.Rows)
                {
                    string idClient = row["id_client"].ToString();
                    string nomClient = row["nom_client"] != DBNull.Value ? row["nom_client"].ToString() : "N/A";
                    string prenomClient = row["prenom_client"] != DBNull.Value ? row["prenom_client"].ToString() : "N/A";
                    string stationMetroClient = row["metro_le_plus_proche_client"] != DBNull.Value ? row["metro_le_plus_proche_client"].ToString() : "N/A";

                    Console.WriteLine($"{idClient}\t{nomClient,-15}{prenomClient,-15}{stationMetroClient}");
                }

                Console.Write("\nEntrez l'ID du client pour calculer le trajet : ");
                string idClientChoisi = Console.ReadLine();

                string requeteClientChoisi = @"
                    SELECT c.nom_client, c.prenom_client, c.metro_le_plus_proche_client
                    FROM CLIENT c
                    WHERE c.id_client = @IdClient";

                Dictionary<string, object> parametresClientChoisi = new Dictionary<string, object>
                {
                    { "@IdClient", idClientChoisi }
                };

                DataTable clientChoisi = GestionBDD.ExecuterRequeteTable(requeteClientChoisi, parametresClientChoisi);

                if (clientChoisi == null || clientChoisi.Rows.Count == 0)
                {
                    GestionBDD.AfficherMessageErreur("Client introuvable.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                string nomClientChoisi = clientChoisi.Rows[0]["nom_client"] != DBNull.Value ? clientChoisi.Rows[0]["nom_client"].ToString() : "N/A";
                string prenomClientChoisi = clientChoisi.Rows[0]["prenom_client"] != DBNull.Value ? clientChoisi.Rows[0]["prenom_client"].ToString() : "N/A";
                string stationMetroClientChoisi = clientChoisi.Rows[0]["metro_le_plus_proche_client"] != DBNull.Value ? clientChoisi.Rows[0]["metro_le_plus_proche_client"].ToString() : "N/A";

                if (string.IsNullOrEmpty(stationMetroClientChoisi) || stationMetroClientChoisi == "N/A")
                {
                    GestionBDD.AfficherMessageErreur("Ce client n'a pas de station de métro renseignée.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine($"\nCalcul du trajet entre votre station ({stationMetroCuisinier}) et celle de {prenomClientChoisi} {nomClientChoisi} ({stationMetroClientChoisi})");

                Console.WriteLine("\nCalcul de la distance directe entre les deux stations...");
                double distance = CalculerDistanceEntreStations(stationMetroCuisinier, stationMetroClientChoisi);

                if (distance > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"Distance à vol d'oiseau (déterminée par la formule d'Haversine): {distance:F2} km");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("Impossible de calculer la distance directe entre ces stations.");
                }

                Console.Write("\nVoulez-vous calculer le chemin le plus court en métro ? (O/N) : ");
                if (Console.ReadLine().ToUpper() == "O")
                {
                    Console.WriteLine("\nChoisissez l'algorithme de PCC (Plus Court Chemin) à utiliser :");
                    Console.WriteLine("1. Dijkstra (optimal pour les graphes sans poids négatifs)");
                    Console.WriteLine("2. Bellman-Ford (peut gérer les poids négatifs, mais plus lent)");
                    Console.WriteLine("3. Floyd-Warshall (calcule tous les plus courts chemins)");
                    Console.Write("Votre choix (1, 2 ou 3) : ");
                    string choixAlgo = Console.ReadLine();

                    TrouverCheminOptimal(stationMetroCuisinier, stationMetroClientChoisi, choixAlgo);
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        /// <summary>
        /// Calcule la distance entre deux stations de métro en utilisant la formule de Haversine. Cette méthode est très similaire à la méthode DistanceEntreDeuxStations 
        /// de la classe CalculDistance mais il y a qq différences : elle renvoie un double (ce n'est pas un void), il n'y a pas d'interactions avec l'utilisateur...etc
        /// </summary>
        private static double CalculerDistanceEntreStations(string station1, string station2)
        {
            try
            {
                string cheminFichier = @"C:\Users\fzzn7\Documents\GitHub\PSI_2025_ThoFaizMart\Feuille_noeud_convertie.csv";

                if (!System.IO.File.Exists(cheminFichier))
                {
                    Console.WriteLine("Erreur: Le fichier des stations n'existe pas.");
                    return -1;
                }

                (double lat1, double long1) = CalculDistance.AcquisitionCoordonnees(cheminFichier, station1);
                (double lat2, double long2) = CalculDistance.AcquisitionCoordonnees(cheminFichier, station2);

                if (lat1 != 0 && lat2 != 0)
                {
                    double distance = CalculDistance.Haversine(lat1, long1, lat2, long2);
                    return distance;
                }
                else
                {
                    if (lat1 == 0)
                        Console.WriteLine($"Station {station1} introuvable dans le fichier des stations.");
                    if (lat2 == 0)
                        Console.WriteLine($"Station {station2} introuvable dans le fichier des stations.");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du calcul de la distance : {ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// Calcule et affiche le chemin optimal entre deux stations en utilisant l'algorithme choisi (Dijkstra, Bellman-Ford ou Floyd-Warshall).
        /// Charge les données depuis des fichiers CSV, construit le graphe, vérifie la connectivité et exécute l'algorithme.
        /// Affiche un message d'erreur si les données sont invalides ou aucun chemin n'est trouvé.
        /// </summary>
        private static void TrouverCheminOptimal(string stationDepart, string stationArrivee, string choixAlgo)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("CALCUL DU CHEMIN OPTIMAL");
            Console.WriteLine($"De {stationDepart} à {stationArrivee}");

            try
            {
                string cheminFichierNoeuds = @"C:\Users\fzzn7\Documents\GitHub\PSI_2025_ThoFaizMart\Feuille_noeud_convertie.csv";
                string cheminFichierArcs = @"C:\Users\fzzn7\Documents\GitHub\PSI_2025_ThoFaizMart\Feuille_arc_convertie.csv";

                if (!System.IO.File.Exists(cheminFichierNoeuds) || !System.IO.File.Exists(cheminFichierArcs))
                {
                    Console.WriteLine("Erreur: Les fichiers de données du métro n'existent pas.");
                    return;
                }

                var stations = CalculDistance.ChargerStationsDirectement(cheminFichierNoeuds);
                if (stations.Count == 0)
                {
                    Console.WriteLine("Erreur: Aucune station n'a pu être chargée.");
                    return;
                }

                var stationDep = CalculDistance.TrouverStation(stations, stationDepart);
                var stationArr = CalculDistance.TrouverStation(stations, stationArrivee);

                if (stationDep == null || stationArr == null)
                {
                    if (stationDep == null)
                        Console.WriteLine($"Station '{stationDepart}' introuvable !");
                    if (stationArr == null)
                        Console.WriteLine($"Station '{stationArrivee}' introuvable !");
                    return;
                }

                var (noeuds, liens) = CalculDistance.ConstruireGrapheComplet(stations, cheminFichierArcs);
                var mapIdToNoeud = noeuds.ToDictionary(n => n.Id);

                if (!mapIdToNoeud.ContainsKey(stationDep.Id) || !mapIdToNoeud.ContainsKey(stationArr.Id))
                {
                    Console.WriteLine("Erreur: Les stations ne sont pas correctement mappées dans le graphe.");
                    return;
                }

                var noeudDepart = mapIdToNoeud[stationDep.Id];
                var noeudArrivee = mapIdToNoeud[stationArr.Id];

                var grapheAdjacence = CalculDistance.ConstruireGrapheAdjacence(noeuds, liens);
                bool sontConnectes = CalculDistance.VerifierConnectivite(grapheAdjacence, noeudDepart, noeudArrivee);

                if (!sontConnectes)
                {
                    CalculDistance.AjouterConnexionsSupplementaires(stations, noeuds, liens, grapheAdjacence);
                    sontConnectes = CalculDistance.VerifierConnectivite(grapheAdjacence, noeudDepart, noeudArrivee);

                    if (!sontConnectes)
                    {
                        double distance = CalculDistance.Haversine(stationDep.Latitude, stationDep.Longitude, stationArr.Latitude, stationArr.Longitude);
                        int poids = Math.Max((int)(distance * 1000), 1);

                        liens.Add(new Lien<int>(noeudDepart, noeudArrivee, poids));
                        grapheAdjacence[noeudDepart].Add(noeudArrivee);
                        grapheAdjacence[noeudArrivee].Add(noeudDepart);
                    }
                }

                List<Noeud<int>> chemin;

                int algorithmeChoisi = 1;
                switch (choixAlgo)
                {
                    case "2":
                        algorithmeChoisi = 2; /// Bellman-Ford
                        break;
                    case "3":
                        algorithmeChoisi = 3; /// Floyd-Warshall
                        break;
                    default:
                        algorithmeChoisi = 1; /// Par défaut, Dijkstra
                        break;
                }
                string nomAlgo = "";

                switch (algorithmeChoisi)
                {
                    case 1:
                        nomAlgo = "de Dijkstra";
                        break;
                    case 2:
                        nomAlgo = "de Bellman-Ford";
                        break;
                    case 3:
                        nomAlgo = "de Floyd-Warshall";
                        break;
                }

                Console.WriteLine($"\nRecherche du chemin avec l'algorithme {nomAlgo}...");

                switch (algorithmeChoisi)
                {
                    case 2:
                        chemin = CalculDistance.BellmanFordChemin(noeudDepart, noeudArrivee, noeuds, liens);
                        break;
                    case 3:
                        chemin = CalculDistance.FloydWarshallChemin(noeudDepart, noeudArrivee, noeuds, liens);
                        break;
                    default:
                        chemin = CalculDistance.DijkstraChemin(noeudDepart, noeudArrivee, noeuds, liens);
                        break;
                }

                if (chemin.Count <= 1 && !stationDep.Id.Equals(stationArr.Id))
                {
                    Console.WriteLine("Aucun chemin trouvé entre ces deux stations.");
                    return;
                }

                CalculDistance.AfficherChemin(chemin, stations);
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur lors du calcul du chemin : {ex.Message}");
            }
        }
        #endregion
    }
}