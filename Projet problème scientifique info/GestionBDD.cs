using MySql.Data.MySqlClient;
using Projet_problème_scientifique_info;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Point d'entrée principal du programme. Accueille l'utilisateur sur l'application et le redirige vers la classe contenant toutes les méthodes correspondant à l'option qu'il a choisi. 
    /// </summary>
    internal class GestionBDD
    {
        //private static string connectionString = "Server=localhost;Database=livinparis2;Uid=root;Pwd=REDACTED;";
        private static string connectionString = "Server=localhost;Database=livinparis2;Uid=root;Pwd=REDACTED;";

        /// <summary>
        /// Point d'entrée du programme
        /// </summary>
        static void Main(string[] args)
        {
            Console.Title = "Liv'InParis - Système de gestion de livraison de repas";

            SessionUtilisateur.Initialiser();

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

                if (SessionUtilisateur.EstConnecte())
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Connecté en tant que : {SessionUtilisateur.NomUtilisateur} ({SessionUtilisateur.TypeUtilisateur})");
                    Console.ResetColor();
                    Console.WriteLine();

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
                    Console.WriteLine("Bienvenue. \nVeuillez choisir un module pour voir les options associées.");
                    Console.WriteLine();
                    Console.WriteLine("1. MODULE PROFIL : Se connecter ou créer un compte afin d'accéder à son espace personnel");
                    Console.WriteLine("2. MODULE PLATS : Consulter les plats disponibles sans se connecter");
                    Console.WriteLine("3. MODULE STATISTIQUES : Consulter les statistiques publiques sans se connecter");
                    Console.WriteLine("4. MODULE TRAJETS : Consulter le graphe du métro, calculer une distance ou un plus court chemin");
                    Console.WriteLine("5. MODULE INFOS : A propos de l'application");
                    Console.WriteLine("6. MODULE AUTRE : à venir");
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
            Console.WriteLine("Merci d'avoir utilisé Liv'InParis. À bientôt !");
            Console.WriteLine("Appuyez sur une touche pour quitter...");
            Console.ReadKey();
        }
        #endregion

        #region Méthodes d'affichage
        /// <summary>
        /// Affiche un titre encadré avec une couleur bleue pour le mettre en valeur, suivi d'une ligne de séparation adaptée à sa longueur.
        /// Cette méthode est appelée à plusieurs reprises dans les différents menus afin d'obtenir un affichage plus clair.
        /// </summary>
        public static void AfficherTitre(string titre)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"--- {titre} ---");
            Console.WriteLine(new string('-', titre.Length + 8));
            Console.ResetColor();
            Console.WriteLine();
        }

        /// <summary>
        /// Affiche un message d'erreur (entré en paramètre de la méthode) en rouge, suivi d'un autre message invitant l'utilisateur à appuyer sur une touche pour continuer.
        /// </summary>
        public static void AfficherMessageErreur(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        /// <summary>
        /// Affiche un message de succès en vert, suivi d'un autre message invitant l'utilisateur à appuyer sur une touche pour continuer.
        /// </summary>
        public static void AfficherMessageSucces(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        /// <summary>
        /// Affiche un message d'information en jaune
        /// </summary>
        public static void AfficherMessageInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        #endregion

        #region Méthodes utilitaires pour la base de données (execution de requêtes, génération d'ID)
        /// <summary>
        /// Génère un nouvel ID pour une table spécifiée
        /// </summary>
        /// <param name="table">Nom de la table</param>
        /// <param name="colonne">Nom de la colonne ID</param>
        /// <returns>Nouvel ID généré</returns>
        public static int GenererNouvelID(string table, string colonne)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT MAX(CAST({colonne} AS UNSIGNED)) FROM {table}";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    object result = cmd.ExecuteScalar();

                    int maxId = 0;
                    if (result != null && result != DBNull.Value)
                    {
                        maxId = Convert.ToInt32(result);
                    }
                    return maxId + 1;
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur lors de la génération d'un nouvel ID : {ex.Message}");
                return new Random().Next(1000, 9999); 
            }
        }

        /// <summary>
        /// Exécute une requête SQL avec des paramètres
        /// </summary>
        /// <param name="requete">Requête SQL à exécuter</param>
        /// <param name="parametres">Dictionnaire des paramètres</param>
        /// <returns>Nombre de lignes affectées</returns>
        public static int ExecuterRequete(string requete, Dictionary<string, object> parametres)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(requete, connection);

                    foreach (var param in parametres)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur lors de l'exécution de la requête : {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Exécute une requête SQL et retourne un scalaire
        /// </summary>
        /// <param name="requete">Requête SQL à exécuter</param>
        /// <param name="parametres">Dictionnaire des paramètres</param>
        /// <returns>Résultat scalaire</returns>
        public static object ExecuterScalaire(string requete, Dictionary<string, object> parametres)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(requete, connection);

                    foreach (var param in parametres)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }

                    object result = cmd.ExecuteScalar();
                    return result ?? DBNull.Value;
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur lors de l'exécution de la requête scalaire : {ex.Message}");
                return DBNull.Value;
            }
        }

        /// <summary>
        /// Exécute une requête SQL et retourne un DataTable
        /// </summary>
        /// <param name="requete">Requête SQL à exécuter</param>
        /// <param name="parametres">Dictionnaire des paramètres</param>
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
                AfficherMessageErreur($"Erreur lors de l'exécution de la requête table : {ex.Message}");
                return new DataTable(); 
            }
        }

        /// <summary>
        /// Vérifie si une valeur existe dans une table
        /// </summary>
        /// <param name="table">Nom de la table</param>
        /// <param name="colonne">Nom de la colonne</param>
        /// <param name="valeur">Valeur à vérifier</param>
        /// <returns>True si la valeur existe, sinon False</returns>
        public static bool ValeurExiste(string table, string colonne, object valeur)
        {
            try
            {
                string requete = $"SELECT COUNT(*) FROM {table} WHERE {colonne} = @Valeur";
                Dictionary<string, object> parametres = new Dictionary<string, object>
                {
                    { "@Valeur", valeur }
                };

                object resultat = ExecuterScalaire(requete, parametres);
                return resultat != null && Convert.ToInt32(resultat) > 0;
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur lors de la vérification de l'existence de la valeur : {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Obtient la liste des clients avec leurs détails
        /// </summary>
        /// <returns>Liste de clients (tuples anonymes)</returns>
        public static List<dynamic> ObtenirListeClientsDetails()
        {
            List<dynamic> clients = new List<dynamic>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT id_client, nom_client, prenom_client, email_client, tel_client FROM CLIENT ORDER BY nom_client, prenom_client";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var client = new
                            {
                                id = reader["id_client"].ToString(),
                                nom = reader.IsDBNull(reader.GetOrdinal("nom_client")) ? "" : reader["nom_client"].ToString(),
                                prenom = reader.IsDBNull(reader.GetOrdinal("prenom_client")) ? "" : reader["prenom_client"].ToString(),
                                email = reader.IsDBNull(reader.GetOrdinal("email_client")) ? "" : reader["email_client"].ToString(),
                                telephone = reader.IsDBNull(reader.GetOrdinal("tel_client")) ? "" : reader["tel_client"].ToString()
                            };
                            clients.Add(client);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur lors de la récupération des clients : {ex.Message}");
            }
            return clients;
        }

        /// <summary>
        /// Obtient les détails d'un client spécifique
        /// </summary>
        /// <param name="idClient">ID du client</param>
        /// <returns>Détails du client (tuple anonyme)</returns>
        public static dynamic ObtenirDetailsClient(string idClient)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT id_client, nom_client, prenom_client, email_client, tel_client, rue_client, code_postal_client FROM CLIENT WHERE id_client = @IdClient";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdClient", idClient);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new
                            {
                                id = reader["id_client"].ToString(),
                                nom = reader.IsDBNull(reader.GetOrdinal("nom_client")) ? "" : reader["nom_client"].ToString(),
                                prenom = reader.IsDBNull(reader.GetOrdinal("prenom_client")) ? "" : reader["prenom_client"].ToString(),
                                email = reader.IsDBNull(reader.GetOrdinal("email_client")) ? "" : reader["email_client"].ToString(),
                                telephone = reader.IsDBNull(reader.GetOrdinal("tel_client")) ? "" : reader["tel_client"].ToString(),
                                rue = reader.IsDBNull(reader.GetOrdinal("rue_client")) ? "" : reader["rue_client"].ToString(),
                                codePostal = reader.IsDBNull(reader.GetOrdinal("code_postal_client")) ? "" : reader["code_postal_client"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur lors de la récupération des détails du client : {ex.Message}");
            }
            return new { id = "", nom = "", prenom = "", email = "", telephone = "", rue = "", codePostal = "" };
        }

        /// <summary>
        /// Obtient la liste des cuisiniers avec leurs détails
        /// </summary>
        /// <returns>Liste de cuisiniers (tuples anonymes)</returns>
        public static List<dynamic> ObtenirListeCuisiniersDetails()
        {
            List<dynamic> cuisiniers = new List<dynamic>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT id_cuisinier, nom_cuisinier, prenom_cuisinier, email_cuisinier, tel_cuisinier, dispo_cuisinier, specialite_cuisinier FROM CUISINIER ORDER BY nom_cuisinier, prenom_cuisinier";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cuisinier = new
                            {
                                id = reader["id_cuisinier"].ToString(),
                                nom = reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) ? "" : reader["nom_cuisinier"].ToString(),
                                prenom = reader.IsDBNull(reader.GetOrdinal("prenom_cuisinier")) ? "" : reader["prenom_cuisinier"].ToString(),
                                email = reader.IsDBNull(reader.GetOrdinal("email_cuisinier")) ? "" : reader["email_cuisinier"].ToString(),
                                telephone = reader.IsDBNull(reader.GetOrdinal("tel_cuisinier")) ? "" : reader["tel_cuisinier"].ToString(),
                                disponibilite = reader.IsDBNull(reader.GetOrdinal("dispo_cuisinier")) ? "" : reader["dispo_cuisinier"].ToString(),
                                specialite = reader.IsDBNull(reader.GetOrdinal("specialite_cuisinier")) ? "" : reader["specialite_cuisinier"].ToString()
                            };
                            cuisiniers.Add(cuisinier);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur lors de la récupération des cuisiniers : {ex.Message}");
            }
            return cuisiniers;
        }

        /// <summary>
        /// Obtient les détails d'un cuisinier spécifique
        /// </summary>
        /// <param name="idCuisinier">ID du cuisinier</param>
        /// <returns>Détails du cuisinier (tuple anonyme)</returns>
        public static dynamic ObtenirDetailsCuisinier(string idCuisinier)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT id_cuisinier, nom_cuisinier, prenom_cuisinier, email_cuisinier, tel_cuisinier, dispo_cuisinier, specialite_cuisinier FROM CUISINIER WHERE id_cuisinier = @IdCuisinier";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new
                            {
                                id = reader["id_cuisinier"].ToString(),
                                nom = reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) ? "" : reader["nom_cuisinier"].ToString(),
                                prenom = reader.IsDBNull(reader.GetOrdinal("prenom_cuisinier")) ? "" : reader["prenom_cuisinier"].ToString(),
                                email = reader.IsDBNull(reader.GetOrdinal("email_cuisinier")) ? "" : reader["email_cuisinier"].ToString(),
                                telephone = reader.IsDBNull(reader.GetOrdinal("tel_cuisinier")) ? "" : reader["tel_cuisinier"].ToString(),
                                disponibilite = reader.IsDBNull(reader.GetOrdinal("dispo_cuisinier")) ? "" : reader["dispo_cuisinier"].ToString(),
                                specialite = reader.IsDBNull(reader.GetOrdinal("specialite_cuisinier")) ? "" : reader["specialite_cuisinier"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur lors de la récupération des détails du cuisinier : {ex.Message}");
            }
            return new { id = "", nom = "", prenom = "", email = "", telephone = "", disponibilite = "", specialite = "" };
        }

        /// <summary>
        /// Obtient la liste des plats disponibles
        /// </summary>
        /// <returns>DataTable contenant les plats</returns>
        public static DataTable ObtenirListePlats()
        {
            string requete = "SELECT id_plat, nom_plat, description_plat, type_plat, prix_unitaire_plat, " +
                            "regime_alimentaire_plat, nationalite_plat, date_fabrication_plat, portion_dispo_plat " +
                            "FROM PLAT WHERE portion_dispo_plat > 0 ORDER BY type_plat, nom_plat";

            return ExecuterRequeteTable(requete, new Dictionary<string, object>());
        }

        /// <summary>
        /// Obtient les détails d'un plat spécifique
        /// </summary>
        /// <param name="idPlat">ID du plat</param>
        /// <returns>DataRow contenant les détails du plat</returns>
        public static DataRow ObtenirDetailsPlat(string idPlat)
        {
            string requete = "SELECT id_plat, nom_plat, description_plat, type_plat, prix_unitaire_plat, " +
                            "regime_alimentaire_plat, nationalite_plat, date_fabrication_plat, date_peremption_plat, portion_dispo_plat " +
                            "FROM PLAT WHERE id_plat = @IdPlat";

            Dictionary<string, object> parametres = new Dictionary<string, object>
            {
                { "@IdPlat", idPlat }
            };

            DataTable resultat = ExecuterRequeteTable(requete, parametres);

            if (resultat != null && resultat.Rows.Count > 0)
            {
                return resultat.Rows[0];
            }

            return null;
        }

        /// <summary>
        /// Obtient les ingrédients d'un plat
        /// </summary>
        /// <param name="idPlat">ID du plat</param>
        /// <returns>DataTable contenant les ingrédients</returns>
        public static DataTable ObtenirIngredientsPlat(string idPlat)
        {
            string requete = "SELECT i.id_ingredient, i.nom_ingredient, i.categorie_ingredient, i.origine_ingredient, i.allergene_ingredient " +
                            "FROM INGREDIENT i " +
                            "JOIN contient c ON i.id_ingredient = c.id_ingredient " +
                            "WHERE c.id_plat = @IdPlat " +
                            "ORDER BY i.nom_ingredient";

            Dictionary<string, object> parametres = new Dictionary<string, object>
            {
                { "@IdPlat", idPlat }
            };

            return ExecuterRequeteTable(requete, parametres);
        }

        /// <summary>
        /// Vérifie si un plat est utilisé dans des commandes actives
        /// </summary>
        /// <param name="idPlat">ID du plat</param>
        /// <returns>True si le plat est utilisé, sinon False</returns>
        public static bool PlatEstUtiliseDansCommandesActives(string idPlat)
        {
            string requete = "SELECT COUNT(*) FROM LIGNE_COMMANDE lc " +
                            "JOIN COMMANDE c ON lc.id_commande = c.id_commande " +
                            "WHERE lc.id_plat = @IdPlat AND c.statut_commande NOT IN ('Livrée', 'Annulée')";

            Dictionary<string, object> parametres = new Dictionary<string, object>
            {
                { "@IdPlat", idPlat }
            };

            object resultat = ExecuterScalaire(requete, parametres);
            return resultat != null && Convert.ToInt32(resultat) > 0;
        }

        /// <summary>
        /// Obtient les évaluations d'un cuisinier
        /// </summary>
        /// <param name="idCuisinier">ID du cuisinier</param>
        /// <returns>DataTable contenant les évaluations</returns>
        public static DataTable ObtenirEvaluationsCuisinier(string idCuisinier)
        {
            string requete =
                    "SELECT e.id_evaluation, e.note_evaluation, e.commentaire_evaluation, e.date_evaluation, " +
                    "c.nom_client, c.prenom_client " +
                    "FROM EVALUATION e " +
                    "JOIN effectue ef ON e.id_evaluation = ef.id_evaluation " +
                    "JOIN CLIENT c ON ef.id_client = c.id_client " +
                    "JOIN reçoit r ON e.id_evaluation = r.id_evaluation " +
                    "WHERE r.id_cuisinier = @IdCuisinier " +
                    "ORDER BY e.date_evaluation DESC";


            Dictionary<string, object> parametres = new Dictionary<string, object>
            {
                { "@IdCuisinier", idCuisinier }
            };

            return ExecuterRequeteTable(requete, parametres);
        }

        /// <summary>
        /// Calcule la note moyenne d'un cuisinier
        /// </summary>
        /// <param name="idCuisinier">ID du cuisinier</param>
        /// <returns>Note moyenne</returns>
        public static double CalculerNoteMoyenneCuisinier(string idCuisinier)
        {
            string requete = "SELECT AVG(CAST(note_evaluation AS DECIMAL(3,1))) FROM EVALUATION WHERE id_cuisinier = @IdCuisinier";

            Dictionary<string, object> parametres = new Dictionary<string, object>
            {
                { "@IdCuisinier", idCuisinier }
            };

            object resultat = ExecuterScalaire(requete, parametres);

            if (resultat != null && resultat != DBNull.Value)
            {
                return Convert.ToDouble(resultat);
            }

            return 0;
        }

        /// <summary>
        /// Obtient les commandes d'un client
        /// </summary>
        /// <param name="idClient">ID du client</param>
        /// <returns>DataTable contenant les commandes</returns>
        public static DataTable ObtenirCommandesClient(string idClient)
        {
            string requete = "SELECT id_commande, date_commande, date_livraison, prix_total_commande, statut_commande, commentaire_commande " +
                            "FROM COMMANDE " +
                            "WHERE id_client = @IdClient " +
                            "ORDER BY date_commande DESC";

            Dictionary<string, object> parametres = new Dictionary<string, object>
            {
                { "@IdClient", idClient }
            };

            return ExecuterRequeteTable(requete, parametres);
        }

        /// <summary>
        /// Obtient les plats préparés par un cuisinier
        /// </summary>
        /// <param name="idCuisinier">ID du cuisinier</param>
        /// <returns>DataTable contenant les plats</returns>
        public static DataTable ObtenirPlatsCuisinier(string idCuisinier)
        {
            string requete = "SELECT p.id_plat, p.nom_plat, p.type_plat, p.prix_unitaire_plat, COUNT(lc.id_ligne) AS nombre_commandes " +
                            "FROM PLAT p " +
                            "JOIN LIGNE_COMMANDE lc ON p.id_plat = lc.id_plat " +
                            "JOIN CUISINIER c ON lc.id_ligne = c.id_ligne " +
                            "WHERE c.id_cuisinier = @IdCuisinier " +
                            "GROUP BY p.id_plat, p.nom_plat, p.type_plat, p.prix_unitaire_plat " +
                            "ORDER BY nombre_commandes DESC";

            Dictionary<string, object> parametres = new Dictionary<string, object>
            {
                { "@IdCuisinier", idCuisinier }
            };

            return ExecuterRequeteTable(requete, parametres);
        }

        /// <summary>
        /// Obtient les statistiques de fréquence des plats
        /// </summary>
        /// <returns>DataTable contenant les statistiques</returns>
        public static DataTable ObtenirStatistiqueFrequencePlats()
        {
            string requete = "SELECT p.id_plat, p.nom_plat, p.type_plat, COUNT(lc.id_ligne) AS nombre_commandes " +
                            "FROM PLAT p " +
                            "LEFT JOIN LIGNE_COMMANDE lc ON p.id_plat = lc.id_plat " +
                            "GROUP BY p.id_plat, p.nom_plat, p.type_plat " +
                            "ORDER BY nombre_commandes DESC";

            return ExecuterRequeteTable(requete, new Dictionary<string, object>());
        }

        /// <summary>
        /// Obtient les statistiques des clients servis par un cuisinier
        /// </summary>
        /// <param name="idCuisinier">ID du cuisinier</param>
        /// <returns>DataTable contenant les statistiques</returns>
        public static DataTable ObtenirStatistiqueClientsServis(string idCuisinier)
        {
            string requete = "SELECT c.id_client, c.nom_client, c.prenom_client, COUNT(lc.id_ligne) AS nombre_commandes " +
                            "FROM CLIENT c " +
                            "JOIN COMMANDE cmd ON c.id_client = cmd.id_client " +
                            "JOIN LIGNE_COMMANDE lc ON cmd.id_commande = lc.id_commande " +
                            "JOIN CUISINIER cuis ON lc.id_ligne = cuis.id_ligne " +
                            "WHERE cuis.id_cuisinier = @IdCuisinier " +
                            "GROUP BY c.id_client, c.nom_client, c.prenom_client " +
                            "ORDER BY nombre_commandes DESC";

            Dictionary<string, object> parametres = new Dictionary<string, object>
            {
                { "@IdCuisinier", idCuisinier }
            };

            return ExecuterRequeteTable(requete, parametres);
        }
        #endregion

        #region Affichage du graphe client-cuisinier
        /// <summary>
        /// Affiche et analyse le graphe des relations entre clients et cuisiniers
        /// </summary>
        public static void AfficherGrapheRelations()
        {
            Console.Clear();
            AfficherTitre("Analyse des relations clients-cuisiniers");

            ClientCuisinierGraphe graphe = new ClientCuisinierGraphe();
            bool grapheConstruit = graphe.ConstruireGraphe();

            if (!grapheConstruit)
            {
                AfficherMessageErreur("Erreur lors de la construction du graphe. Retour au menu principal.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n=== Propriétés du graphe ===");

            int nombreCouleurs = graphe.AlgoWelshPowell(); /// 1.Appliquer l'algorithme de Welsh-Powell pour la coloration
            Console.WriteLine($"\nNombre minimal de couleurs nécessaires : {nombreCouleurs}");

            bool estBiparti = graphe.EstBiparti();  /// 2. Vérifier si le graphe est biparti
            Console.WriteLine($"\nLe graphe est-il biparti ? {(estBiparti ? "OUI" : "NON")}");
            if (estBiparti)
            {
                Console.WriteLine("Justification : Tous les clients peuvent être colorés avec une couleur et tous les cuisiniers avec une autre couleur.");
            }
            else
            {
                Console.WriteLine("Justification : Il existe des clients ou des cuisiniers qui doivent avoir la même couleur que leurs voisins.");
            }

            bool estPlanaire = graphe.EstPlanaire();    /// 3. Vérifier si le graphe est planaire
            Console.WriteLine($"\nLe graphe est-il planaire ? {(estPlanaire ? "OUI" : "NON")}");
            if (estPlanaire)
            {
                Console.WriteLine("Justification : Le nombre d'arêtes est inférieur ou égal à 3n-6, où n est le nombre de sommets.");
            }
            else
            {
                Console.WriteLine("Justification : Le nombre d'arêtes est supérieur à 3n-6, où n est le nombre de sommets.");
            }

            List<List<string>> groupesIndependants = graphe.IdentifierGroupesIndependants();   /// 4. Identifier les groupes indépendants
            Console.WriteLine($"\nNombre de groupes indépendants de clients : {groupesIndependants.Count}");

            string cheminImage = Path.Combine(    /// 5. Visualiser le graphe
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                "graphe_clients_cuisiniers.png");

            bool visualisationReussie = graphe.VisualiserGraphe(cheminImage);

            if (!visualisationReussie)
            {
                AfficherMessageErreur("\nLa visualisation du graphe n'a pas pu être générée.");
            }
            else
            {
                AfficherMessageSucces($"\nLe graphe a été généré et enregistré sous : {cheminImage}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu principal...");
            Console.ReadKey();
        }
        #endregion
    }
}
