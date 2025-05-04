using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Classe utilitaire pour la gestion de la base de données
    /// </summary>
    internal class GestionBDD
    {
        private static string connectionString = "Server=localhost;Database=livinparis2;Uid=root;Pwd=REDACTED;";

        public static void Main(string[] args)
        {
            Console.Title = "Liv'InParis - Système de gestion de livraison de repas";

            // Initialiser la session utilisateur
            SessionUtilisateur.Initialiser();

            // Afficher le menu principal
            AfficherMenuPrincipal();
        }

        #region Menu principal
        /// <summary>
        /// Affiche le menu principal avec les différents modules disponibles,
        /// puis redirige l'utilisateur vers le module sélectionné en fonction de son choix.
        /// </summary>
        public static void AfficherMenuPrincipal()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                AfficherTitre("          Liv'InParis          ");
                AfficherTitre("SYSTÈME DE GESTION DE LIVRAISON DE REPAS");

                // Vérifier si l'utilisateur est connecté
                if (SessionUtilisateur.EstConnecte())
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Connecté en tant que : {SessionUtilisateur.NomUtilisateur} ({SessionUtilisateur.TypeUtilisateur})");
                    Console.ResetColor();
                    Console.WriteLine();

                    // Afficher le menu en fonction du type d'utilisateur
                    if (SessionUtilisateur.TypeUtilisateur == "client")
                    {
                        ModuleClient.MenuPrincipalClient();
                    }
                    else if (SessionUtilisateur.TypeUtilisateur == "cuisinier")
                    {
                        ModuleCuisinier.MenuPrincipalCuisinier();
                    }
                    else if (SessionUtilisateur.TypeUtilisateur == "admin")
                    {
                        ModuleAdministrateur.MenuPrincipalAdmin();
                    }
                }
                else
                {
                    // Menu pour utilisateur non connecté
                    Console.WriteLine("Bienvenue. \nVeuillez choisir un module pour voir les options associées.");
                    Console.WriteLine();
                    Console.WriteLine("1. MODULE PROFIL : Se connecter ou créer un compte afin d'accéder à son espace personnel");
                    Console.WriteLine("2. MODULE PLATS : Consulter les plats disponibles sans se connecter");
                    Console.WriteLine("3. MODULE STATISTIQUES : Consulter les statistiques publiques sans se connecter");
                    Console.WriteLine("4. MODULE TRAJETS : Consulter le graphe du métro, calculer une distance ou un plus court chemin");
                    Console.WriteLine("5. MODULE INFOS : A propos de l'application");
                    Console.WriteLine("6. MODULE AUTRE : Graphe clients-cuisiniers");
                    Console.WriteLine("7. Quitter l'application");
                    Console.Write("\nChoisissez une option : ");
                    string choix = Console.ReadLine();

                    switch (choix)
                    {
                        case "1":
                            ModuleProfil.MenuPrincipalProfil();
                            break;
                        case "2":
                            ModulePlat.MenuPrincipalPlats();
                            break;
                        case "3":
                            ModuleStats.MenuPrincipalStats();
                            break;
                        case "4":
                            ModuleTrajets.MenuPrincipalTrajets();
                            break;
                        case "5":
                            ModuleInfosAppli.MenuPrincipalInfos();
                            //AProposDeLApplication();
                            break;
                        case "6":
                            ModuleAutre.MenuPrincipalAutre();
                            break;
                        case "7":
                            continuer = false;
                            break;
                        default:
                            AfficherMessageErreur("Option invalide.");
                            break;
                    }
                }
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Merci d'avoir utilisé Liv'InParis. À bientôt !", ConsoleColor.DarkYellow);
            Console.ResetColor();
            Console.WriteLine("Appuyez sur une touche pour quitter...");
            Console.ReadKey();
        }
        #endregion

        /// <summary>
        /// Exécute une requête SQL qui ne retourne pas de résultat (INSERT, UPDATE, DELETE)
        /// </summary>
        /// <param name="requete">Requête SQL à exécuter</param>
        /// <param name="parametres">Paramètres de la requête</param>
        /// <returns>Nombre de lignes affectées</returns>
        public static int ExecuterRequete(string requete, Dictionary<string, object> parametres)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(requete, connection);

                    if (parametres != null)
                    {
                        foreach (var param in parametres)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'exécution de la requête : {ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// Exécute une requête SQL qui retourne une valeur unique (COUNT, SUM, etc.)
        /// </summary>
        /// <param name="requete">Requête SQL à exécuter</param>
        /// <param name="parametres">Paramètres de la requête</param>
        /// <returns>Résultat de la requête</returns>
        public static object ExecuterScalaire(string requete, Dictionary<string, object> parametres)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(requete, connection);

                    if (parametres != null)
                    {
                        foreach (var param in parametres)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'exécution de la requête : {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Exécute une requête SQL qui retourne un tableau de résultats (SELECT)
        /// </summary>
        /// <param name="requete">Requête SQL à exécuter</param>
        /// <param name="parametres">Paramètres de la requête</param>
        /// <returns>DataTable contenant les résultats</returns>
        public static DataTable ExecuterRequeteTable(string requete, Dictionary<string, object> parametres)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(requete, connection);

                    if (parametres != null)
                    {
                        foreach (var param in parametres)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'exécution de la requête : {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Génère un nouvel ID pour une table donnée
        /// </summary>
        /// <param name="nomTable">Nom de la table</param>
        /// <param name="nomColonneId">Nom de la colonne ID</param>
        /// <returns>Nouvel ID</returns>
        public static int GenererNouvelID(string nomTable, string nomColonneId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string requete = $"SELECT MAX({nomColonneId}) FROM {nomTable}";
                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    object result = cmd.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                    {
                        return 1;
                    }
                    else
                    {
                        return Convert.ToInt32(result) + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la génération d'un nouvel ID : {ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// Affiche un message d'erreur en rouge
        /// </summary>
        /// <param name="message">Message d'erreur</param>
        public static void AfficherMessageErreur(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Affiche un message de succès en vert
        /// </summary>
        /// <param name="message">Message de succès</param>
        public static void AfficherMessageSucces(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Affiche un titre en cyan
        /// </summary>
        /// <param name="titre">Titre à afficher</param>
        public static void AfficherTitre(string titre)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('=', 50));
            Console.WriteLine(titre.PadLeft(25 + titre.Length / 2));
            Console.WriteLine(new string('=', 50));
            Console.ResetColor();
            Console.WriteLine();
        }

        /// <summary>
        /// Obtient la liste des clients avec leurs détails
        /// </summary>
        /// <returns>Liste de clients</returns>
        public static List<(string id, string nom, string prenom, string email)> ObtenirListeClientsDetails()
        {
            List<(string id, string nom, string prenom, string email)> clients = new List<(string id, string nom, string prenom, string email)>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string requete = "SELECT id_client, nom_client, prenom_client, email_client FROM CLIENT ORDER BY nom_client, prenom_client";
                    MySqlCommand cmd = new MySqlCommand(requete, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader["id_client"].ToString();
                            string nom = reader.IsDBNull(reader.GetOrdinal("nom_client")) ? "" : reader["nom_client"].ToString();
                            string prenom = reader.IsDBNull(reader.GetOrdinal("prenom_client")) ? "" : reader["prenom_client"].ToString();
                            string email = reader.IsDBNull(reader.GetOrdinal("email_client")) ? "" : reader["email_client"].ToString();

                            clients.Add((id, nom, prenom, email));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des clients : {ex.Message}");
            }

            return clients;
        }

        /// <summary>
        /// Obtient les détails d'un client spécifique
        /// </summary>
        /// <param name="idClient">ID du client</param>
        /// <returns>Détails du client</returns>
        public static (string id, string nom, string prenom, string email) ObtenirDetailsClient(string idClient)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string requete = "SELECT id_client, nom_client, prenom_client, email_client FROM CLIENT WHERE id_client = @IdClient";
                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@IdClient", idClient);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string id = reader["id_client"].ToString();
                            string nom = reader.IsDBNull(reader.GetOrdinal("nom_client")) ? "" : reader["nom_client"].ToString();
                            string prenom = reader.IsDBNull(reader.GetOrdinal("prenom_client")) ? "" : reader["prenom_client"].ToString();
                            string email = reader.IsDBNull(reader.GetOrdinal("email_client")) ? "" : reader["email_client"].ToString();

                            return (id, nom, prenom, email);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des détails du client : {ex.Message}");
            }

            return ("", "", "", "");
        }

        /// <summary>
        /// Obtient la liste des cuisiniers avec leurs détails
        /// </summary>
        /// <returns>Liste de cuisiniers</returns>
        public static List<(string id, string nom, string prenom, string email)> ObtenirListeCuisiniersDetails()
        {
            List<(string id, string nom, string prenom, string email)> cuisiniers = new List<(string id, string nom, string prenom, string email)>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string requete = "SELECT id_cuisinier, nom_cuisinier, prenom_cuisinier, email_cuisinier FROM CUISINIER ORDER BY nom_cuisinier, prenom_cuisinier";
                    MySqlCommand cmd = new MySqlCommand(requete, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader["id_cuisinier"].ToString();
                            string nom = reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) ? "" : reader["nom_cuisinier"].ToString();
                            string prenom = reader.IsDBNull(reader.GetOrdinal("prenom_cuisinier")) ? "" : reader["prenom_cuisinier"].ToString();
                            string email = reader.IsDBNull(reader.GetOrdinal("email_cuisinier")) ? "" : reader["email_cuisinier"].ToString();

                            cuisiniers.Add((id, nom, prenom, email));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des cuisiniers : {ex.Message}");
            }

            return cuisiniers;
        }

        /// <summary>
        /// Obtient les détails d'un cuisinier spécifique
        /// </summary>
        /// <param name="idCuisinier">ID du cuisinier</param>
        /// <returns>Détails du cuisinier</returns>
        public static (string id, string nom, string prenom, string email) ObtenirDetailsCuisinier(string idCuisinier)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string requete = "SELECT id_cuisinier, nom_cuisinier, prenom_cuisinier, email_cuisinier FROM CUISINIER WHERE id_cuisinier = @IdCuisinier";
                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string id = reader["id_cuisinier"].ToString();
                            string nom = reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) ? "" : reader["nom_cuisinier"].ToString();
                            string prenom = reader.IsDBNull(reader.GetOrdinal("prenom_cuisinier")) ? "" : reader["prenom_cuisinier"].ToString();
                            string email = reader.IsDBNull(reader.GetOrdinal("email_cuisinier")) ? "" : reader["email_cuisinier"].ToString();

                            return (id, nom, prenom, email);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des détails du cuisinier : {ex.Message}");
            }

            return ("", "", "", "");
        }

        /// <summary>
        /// Crée la vue SQL pour les relations client-cuisinier si elle n'existe pas déjà
        /// </summary>
        public static void CreerVueRelationsClientCuisinier()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Vérifier si la vue existe déjà
                    string requeteVerif = "SHOW TABLES LIKE 'VUE_RELATIONS_CLIENT_CUISINIER'";
                    MySqlCommand cmdVerif = new MySqlCommand(requeteVerif, connection);
                    bool vueExiste = cmdVerif.ExecuteScalar() != null;

                    if (!vueExiste)
                    {
                        // Créer la vue
                        string requeteCreation = @"
                        CREATE VIEW VUE_RELATIONS_CLIENT_CUISINIER AS
                        SELECT DISTINCT c.id_client, c.nom_client, c.prenom_client, 
                                        cu.id_cuisinier, cu.nom_cuisinier, cu.prenom_cuisinier
                        FROM COMMANDE cmd
                        JOIN CLIENT c ON cmd.id_client = c.id_client
                        JOIN LIGNE_COMMANDE lc ON cmd.id_commande = lc.id_commande
                        JOIN CUISINIER cu ON lc.id_cuisinier = cu.id_cuisinier
                        WHERE cmd.statut_commande = 'Livrée'";

                        MySqlCommand cmdCreation = new MySqlCommand(requeteCreation, connection);
                        cmdCreation.ExecuteNonQuery();

                        Console.WriteLine("Vue VUE_RELATIONS_CLIENT_CUISINIER créée avec succès.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la création de la vue : {ex.Message}");
            }
        }

        /// <summary>
        /// Affiche le graphe des relations entre clients et cuisiniers
        /// </summary>
        public static void AfficherGrapheRelations()
        {
            try
            {
                Console.Clear();
                AfficherTitre("GRAPHE DES RELATIONS CLIENTS-CUISINIERS");

                // Créer la vue si elle n'existe pas déjà
                CreerVueRelationsClientCuisinier();

                // Afficher les informations sur le graphe
                Console.WriteLine("Analyse des relations entre clients et cuisiniers...");
                Console.WriteLine();

                // Récupérer les données pour le graphe
                string requete = @"
                    SELECT c.id_client, CONCAT(c.nom_client, ' ', c.prenom_client) AS nom_complet_client,
                            cu.id_cuisinier, CONCAT(cu.nom_cuisinier, ' ', cu.prenom_cuisinier) AS nom_complet_cuisinier,
                            COUNT(*) AS nombre_commandes
                    FROM COMMANDE cmd
                    JOIN CLIENT c ON cmd.id_client = c.id_client
                    JOIN LIGNE_COMMANDE lc ON cmd.id_commande = lc.id_commande
                    JOIN CUISINIER cu ON lc.id_cuisinier = cu.id_cuisinier
                    GROUP BY c.id_client, nom_complet_client, cu.id_cuisinier, nom_complet_cuisinier
                    ORDER BY nombre_commandes DESC";

                var table = ExecuterRequeteTable(requete, null);

                if (table.Rows.Count == 0)
                {
                    Console.WriteLine("Aucune relation client-cuisinier trouvée dans la base de données.");
                }
                else
                {
                    Console.WriteLine($"Nombre total de relations : {table.Rows.Count}");
                    Console.WriteLine();

                    // Afficher les 10 relations les plus fréquentes
                    Console.WriteLine("Top 10 des relations clients-cuisiniers :");
                    Console.WriteLine("----------------------------------------");

                    int count = 0;
                    foreach (System.Data.DataRow row in table.Rows)
                    {
                        if (count >= 10) break;

                        string client = row["nom_complet_client"].ToString();
                        string cuisinier = row["nom_complet_cuisinier"].ToString();
                        int nbCommandes = Convert.ToInt32(row["nombre_commandes"]);

                        Console.WriteLine($"{client} → {cuisinier} : {nbCommandes} commande(s)");
                        count++;
                    }

                    // Créer et visualiser le graphe
                    ClientCuisinierGraphe graphe = new ClientCuisinierGraphe();
                    bool grapheConstruit = graphe.ConstruireGraphe();

                    if (grapheConstruit)
                    {
                        Console.WriteLine("\nGénération de la visualisation du graphe...");

                        // Créer un chemin pour l'image
                        string cheminImage = System.IO.Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                            "graphe_clients_cuisiniers.png");

                        // Visualiser le graphe
                        bool visualisationReussie = graphe.VisualiserGraphe(cheminImage);

                        if (!visualisationReussie)
                        {
                            Console.WriteLine("\nLa visualisation du graphe n'a pas pu être générée.");
                        }
                        else
                        {
                            Console.WriteLine($"\nLe graphe a été généré et enregistré sous : {cheminImage}");
                            Console.WriteLine("Une fenêtre devrait s'ouvrir pour afficher le graphe.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nLe graphe n'a pas pu être construit correctement.");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur lors de l'affichage du graphe des relations : {ex.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}
