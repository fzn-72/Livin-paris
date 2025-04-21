using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using System.Data.SqlClient;
using Mysqlx.Crud;
using System.ComponentModel;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Point d'entrée principal du programme. Accueille l'utilisateur sur l'application et le redirige vers la classe contenant toutes les méthodes correspondant à l'option qu'il a choisi. 
    /// </summary>
    internal class GestionBDD
    {
        private static string utilisateurConnecteId = "";
        private static string utilisateurConnecteNom = "";
        private static string utilisateurConnectePrenom = "";
        private static string utilisateurConnecteType = "";

        private static string connectionString = "Server=localhost;Database=livinparis;Uid=root;Pwd=REDACTED;";

        static void Main(string[] args)
        {
            AfficherMenuPrincipal();
        }

        #region Menu principal
        /// <summary>
        /// Affiche le menu principal avec les différents modules disponibles (profil, commandes, statistiques, trajets, autres),
        /// puis redirige l'utilisateur vers le module sélectionné en fonction de son choix.
        /// </summary>
        static void AfficherMenuPrincipal()
        {
            Console.Clear();
            AfficherTitre("          Liv'InParis          ");
            AfficherTitre("SYSTÈME DE GESTION DE LIVRAISON DE REPAS");

            Console.WriteLine("Bienvenue. \nVeuillez choisir un module pour voir les options associées.");
            Console.WriteLine();
            Console.WriteLine("1. MODULE PROFIL : Se connecter ou créer un compte");
            Console.WriteLine("2. MODULE COMMANDES : Afficher les détails d'une commande : bientôt disponible");
            Console.WriteLine("3. MODULE STATISTIQUES : Bilans généraux sur les clients, les cuisiniers, les livraisons et les commandes");
            Console.WriteLine("4. MODULE TRAJETS : Calcul de plus court chemin, calcul de distance et visualisation de graphe ");
            Console.WriteLine("5. MODULE AUTRE : En cours de développement, vous pourrez bientôt profiter de votre nouvel espace");
            Console.WriteLine("6. A propos de l'application");
            Console.Write("\nChoisissez une option : ");
            string choix1 = Console.ReadLine();

            switch (choix1)
            {
                case "1":
                    ModuleProfil.MenuPrincipalProfil();
                    break;
                case "2":
                    MenuPrincipalCommandes();
                    break;
                case "3":
                    ModuleStats.MenuPrincipalStats();
                    break;
                case "4":   
                    ModuleTrajets.MenuPrincipalTrajets(); 
                    break;
                case "5":
                    MenuPrincipalAutre();
                    break;
                case "6":
                    ModuleInfosAppli.AproposDeLAppli();
                    break;
                default:
                    AfficherMessageErreur("Option invalide.");
                    break;
            }
        }
        #endregion


        #region MODULE commandes
        static void MenuPrincipalCommandes()
        {

        }
        #endregion

        #region MODULE autre
        static void MenuPrincipalAutre()
        {
            Console.WriteLine("Cette fonctionnalité sera bientôt disponible. Merci pour votre compréhension.");
        }
        #endregion


        #region Menu admin
        public static void AfficherMenuAdmin()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                AfficherTitre($"BIENVENUE ADMINISTRATEUR");
                Console.WriteLine($"Connecté en tant qu'administrateur");
                Console.WriteLine("\n--- MENU ADMINISTRATEUR ---");
                Console.WriteLine();
                Console.WriteLine("Que souhaitez-vous faire ? Veuillez choisir une option dans la liste suivante :");
                Console.WriteLine();
                Console.WriteLine("1. Lister tous les clients");
                Console.WriteLine("2. Lister toutes les commandes");
                Console.WriteLine("3. Supprimer un client");
                Console.WriteLine("4. Ajouter un restaurant");
                Console.WriteLine("5. Gérer les utilisateurs");
                Console.WriteLine("6. Gérer les assignations des cuisiniers aux restaurants");
                Console.WriteLine("7. Me déconnecter");

                Console.Write("\nChoisissez une option : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        ListerClients();
                        break;
                    case "2":
                        ListerCommandes();
                        break;
                    case "3":
                        SupprimerClient();
                        break;
                    case "4":
                        AjouterRestaurant();
                        break;
                    case "5":
                        GererUtilisateurs();
                        break;
                    case "6":
                        GererAssignationsRestaurants();
                        break;
                    case "7":
                        utilisateurConnecteId = "";
                        utilisateurConnecteNom = "";
                        utilisateurConnectePrenom = "";
                        utilisateurConnecteType = "";
                        continuer = false;
                        break;
                    default:
                        AfficherMessageErreur("Option invalide.");
                        break;
                }
            }
        }

        static void ListerLivreurs()
        {
            Console.Clear();
            AfficherTitre("LISTE DES LIVREURS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete = "SELECT ID_livreur, nom_livreur, prénom_livreur, email_livreur, tel_livreur, date_inscription, note_livreur, disponibilité FROM LIVREUR ORDER BY ID_livreur";
                    MySqlCommand cmd = new MySqlCommand(requete, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("ID\tNom\t\tPrénom\t\tEmail\t\t\tTéléphone\tDisponibilité\tNote");
                            Console.WriteLine("-------------------------------------------------------------------------");

                            while (reader.Read())
                            {
                                string id = reader["ID_livreur"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom_livreur")) ? "N/A" : reader["nom_livreur"].ToString();
                                string prenom = reader.IsDBNull(reader.GetOrdinal("prénom_livreur")) ? "N/A" : reader["prénom_livreur"].ToString();
                                string email = reader.IsDBNull(reader.GetOrdinal("email_livreur")) ? "N/A" : reader["email_livreur"].ToString();
                                string tel = reader.IsDBNull(reader.GetOrdinal("tel_livreur")) ? "N/A" : reader["tel_livreur"].ToString();

                                string disponibilite = "Non";
                                if (!reader.IsDBNull(reader.GetOrdinal("disponibilité")))
                                {
                                    disponibilite = Convert.ToBoolean(reader["disponibilité"]) ? "Oui" : "Non";
                                }

                                string note = "N/A";
                                if (!reader.IsDBNull(reader.GetOrdinal("note_livreur")))
                                {
                                    note = Convert.ToDecimal(reader["note_livreur"]).ToString("F1") + "/5";
                                }

                                Console.WriteLine($"{id}\t{nom,-15}{prenom,-15}{email,-25}{tel,-15}{disponibilite,-15}{note}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun livreur trouvé.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        static void SupprimerClient()
        {
            Console.Clear();
            AfficherTitre("SUPPRIMER UN CLIENT");

            Console.WriteLine("Liste des clients :");
            ListerClients(false);

            Console.Write("\nEntrez l'ID du client à supprimer (0 pour annuler) : ");
            string idClient = Console.ReadLine();
            if (idClient == "0")
            {
                Console.WriteLine("Opération annulée.");
                Console.ReadKey();
                return;
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Vérifier si le client existe
                    string checkQuery = "SELECT COUNT(*) FROM CLIENT WHERE id_client = @IdClient";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@IdClient", idClient);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count == 0)
                    {
                        AfficherMessageErreur("Client introuvable.");
                        return;
                    }

                    // Supprimer le client
                    string deleteQuery = "DELETE FROM CLIENT WHERE id_client = @IdClient";
                    MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, connection);
                    deleteCmd.Parameters.AddWithValue("@IdClient", idClient);
                    int result = deleteCmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine("Client supprimé avec succès !");
                    }
                    else
                    {
                        Console.WriteLine("Échec de la suppression du client.");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        static void AjouterRestaurant()
        {
            Console.Clear();
            AfficherTitre("AJOUTER UN RESTAURANT");

            // Afficher la liste des restaurants existants
            Console.WriteLine("Liste des restaurants existants :");
            List<(string id, string nom, string type, decimal capacite)> restaurants = ObtenirListeRestaurantsDetails();

            if (restaurants.Count > 0)
            {
                Console.WriteLine("\nID\tNom\t\tType\t\tCapacité");
                Console.WriteLine("--------------------------------------------------");

                foreach (var resto in restaurants)
                {
                    Console.WriteLine($"{resto.id}\t{resto.nom,-15}{resto.type,-15}{resto.capacite}");
                }
            }
            else
            {
                Console.WriteLine("Aucun restaurant existant.");
            }

            Console.WriteLine("\n--- Ajouter un nouveau restaurant ---");

            Console.Write("ID du restaurant : ");
            string id = Console.ReadLine();

            Console.Write("Nom du restaurant : ");
            string nom = Console.ReadLine();

            Console.Write("Type de restaurant : ");
            string type = Console.ReadLine();

            Console.Write("Capacité : ");
            string capaciteStr = Console.ReadLine();
            decimal capacite = 0;
            decimal.TryParse(capaciteStr, out capacite);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO RESTAURANT (id_resto, nom_resto, type_resto, capacité_resto) VALUES (@Id, @Nom, @Type, @Capacite)";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nom", nom);
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@Capacite", capacite);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine("Restaurant ajouté avec succès !");
                    }
                    else
                    {
                        Console.WriteLine("Échec de l'ajout du restaurant.");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        static void GererUtilisateurs()
        {
            Console.Clear();
            AfficherTitre("GÉRER LES UTILISATEURS");

            Console.WriteLine("1. Réinitialiser un mot de passe");
            Console.WriteLine("2. Lister tous les utilisateurs");
            Console.WriteLine("3. Retour au menu");

            Console.Write("\nChoisissez une option : ");
            string choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    ReinitialisationMotDePasse();
                    break;
                case "2":
                    ListerUtilisateurs();
                    break;
                case "3":
                    return;
                default:
                    AfficherMessageErreur("Option invalide.");
                    break;
            }
        }

        static void ReinitialisationMotDePasse()
        {
            Console.Clear();
            AfficherTitre("RÉINITIALISER UN MOT DE PASSE");

            Console.WriteLine("1. Client");
            Console.WriteLine("2. Cuisinier");
            Console.WriteLine("3. Livreur");
            Console.Write("\nChoisissez le type d'utilisateur : ");
            string typeUtilisateur = Console.ReadLine();

            string table = "";
            string idField = "";
            string emailField = "";
            string mdpField = "";

            switch (typeUtilisateur)
            {
                case "1":
                    table = "CLIENT";
                    idField = "id_client";
                    emailField = "email";
                    mdpField = "mot_de_passe";
                    break;
                case "2":
                    table = "CUISINIER";
                    idField = "id_cuisinier";
                    emailField = "email_cuisinier";
                    mdpField = "mot_de_passe";
                    break;
                case "3":
                    table = "LIVREUR";
                    idField = "ID_livreur";
                    emailField = "email_livreur";
                    mdpField = "mot_de_passe_livreur";
                    break;
                default:
                    AfficherMessageErreur("Type d'utilisateur invalide.");
                    return;
            }

            Console.Write("Email de l'utilisateur : ");
            string email = Console.ReadLine();

            Console.Write("Nouveau mot de passe : ");
            string nouveauMotDePasse = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Vérifier si l'utilisateur existe
                    string checkQuery = $"SELECT {idField} FROM {table} WHERE {emailField} = @Email";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@Email", email);
                    object result = checkCmd.ExecuteScalar();

                    if (result == null)
                    {
                        AfficherMessageErreur("Utilisateur introuvable.");
                        return;
                    }

                    // Mettre à jour le mot de passe
                    string updateQuery = $"UPDATE {table} SET {mdpField} = @MotDePasse WHERE {emailField} = @Email";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
                    updateCmd.Parameters.AddWithValue("@MotDePasse", nouveauMotDePasse);
                    updateCmd.Parameters.AddWithValue("@Email", email);
                    int updateResult = updateCmd.ExecuteNonQuery();

                    if (updateResult > 0)
                    {
                        Console.WriteLine("Mot de passe réinitialisé avec succès !");
                    }
                    else
                    {
                        Console.WriteLine("Échec de la réinitialisation du mot de passe.");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        static void ListerUtilisateurs()
        {
            Console.Clear();
            AfficherTitre("LISTE DES UTILISATEURS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Lister les clients
                    Console.WriteLine("\n--- CLIENTS ---");
                    string queryClients = "SELECT id_client, nom, prénom, email FROM CLIENT ORDER BY id_client";
                    MySqlCommand cmdClients = new MySqlCommand(queryClients, connection);

                    using (MySqlDataReader reader = cmdClients.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("ID\tNom\t\tPrénom\t\tEmail");
                            Console.WriteLine("--------------------------------------------------");

                            while (reader.Read())
                            {
                                string id = reader["id_client"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom")) ? "N/A" : reader["nom"].ToString();
                                string prenom = reader.IsDBNull(reader.GetOrdinal("prénom")) ? "N/A" : reader["prénom"].ToString();
                                string email = reader.IsDBNull(reader.GetOrdinal("email")) ? "N/A" : reader["email"].ToString();

                                Console.WriteLine($"{id}\t{nom,-15}{prenom,-15}{email}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun client trouvé.");
                        }
                    }

                    // Lister les cuisiniers
                    Console.WriteLine("\n--- CUISINIERS ---");
                    string queryCuisiniers = "SELECT id_cuisinier, nom_cuisinier, prénom_cuisinier, email_cuisinier FROM CUISINIER ORDER BY id_cuisinier";
                    MySqlCommand cmdCuisiniers = new MySqlCommand(queryCuisiniers, connection);

                    using (MySqlDataReader reader = cmdCuisiniers.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("ID\tNom\t\tPrénom\t\tEmail");
                            Console.WriteLine("--------------------------------------------------");

                            while (reader.Read())
                            {
                                string id = reader["id_cuisinier"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) ? "N/A" : reader["nom_cuisinier"].ToString();
                                string prenom = reader.IsDBNull(reader.GetOrdinal("prénom_cuisinier")) ? "N/A" : reader["prénom_cuisinier"].ToString();
                                string email = reader.IsDBNull(reader.GetOrdinal("email_cuisinier")) ? "N/A" : reader["email_cuisinier"].ToString();

                                Console.WriteLine($"{id}\t{nom,-15}{prenom,-15}{email}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun cuisinier trouvé.");
                        }
                    }

                    // Lister les livreurs
                    Console.WriteLine("\n--- LIVREURS ---");
                    string queryLivreurs = "SELECT ID_livreur, nom_livreur, prénom_livreur, email_livreur FROM LIVREUR ORDER BY ID_livreur";
                    MySqlCommand cmdLivreurs = new MySqlCommand(queryLivreurs, connection);

                    using (MySqlDataReader reader = cmdLivreurs.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("ID\tNom\t\tPrénom\t\tEmail");
                            Console.WriteLine("--------------------------------------------------");

                            while (reader.Read())
                            {
                                string id = reader["ID_livreur"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom_livreur")) ? "N/A" : reader["nom_livreur"].ToString();
                                string prenom = reader.IsDBNull(reader.GetOrdinal("prénom_livreur")) ? "N/A" : reader["prénom_livreur"].ToString();
                                string email = reader.IsDBNull(reader.GetOrdinal("email_livreur")) ? "N/A" : reader["email_livreur"].ToString();

                                Console.WriteLine($"{id}\t{nom,-15}{prenom,-15}{email}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun livreur trouvé.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        /// <summary>
        /// Gère les assignations des cuisiniers aux restaurants en utilisant la relation EMPLOIE existante dans la base de données.
        /// </summary>
        static void GererAssignationsRestaurants()
        {
            Console.Clear();
            AfficherTitre("GÉRER LES ASSIGNATIONS AUX RESTAURANTS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Afficher les assignations actuelles
                    Console.WriteLine("=== ASSIGNATIONS ACTUELLES ===");
                    string currentAssignmentsQuery = @"
                SELECT c.id_cuisinier, c.nom_cuisinier, c.prénom_cuisinier, 
                       r.id_resto, r.nom_resto, r.type_resto
                FROM EMPLOIE e
                JOIN CUISINIER c ON e.id_cuisinier = c.id_cuisinier
                JOIN RESTAURANT r ON e.id_resto = r.id_resto
                ORDER BY c.nom_cuisinier, c.prénom_cuisinier";

                    MySqlCommand currentAssignmentsCmd = new MySqlCommand(currentAssignmentsQuery, connection);

                    using (MySqlDataReader reader = currentAssignmentsCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("ID Cuisinier\tNom\t\tPrénom\t\tID Restaurant\tNom Restaurant\tType");
                            Console.WriteLine("-------------------------------------------------------------------------");

                            while (reader.Read())
                            {
                                string idCuisinier = reader["id_cuisinier"].ToString();
                                string nomCuisinier = reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) ? "N/A" : reader["nom_cuisinier"].ToString();
                                string prenomCuisinier = reader.IsDBNull(reader.GetOrdinal("prénom_cuisinier")) ? "N/A" : reader["prénom_cuisinier"].ToString();
                                string idResto = reader["id_resto"].ToString();
                                string nomResto = reader.IsDBNull(reader.GetOrdinal("nom_resto")) ? "N/A" : reader["nom_resto"].ToString();
                                string typeResto = reader.IsDBNull(reader.GetOrdinal("type_resto")) ? "N/A" : reader["type_resto"].ToString();

                                Console.WriteLine($"{idCuisinier}\t{nomCuisinier,-15}{prenomCuisinier,-15}{idResto}\t{nomResto,-15}{typeResto}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucune assignation trouvée.");
                        }
                    }

                    // Options
                    Console.WriteLine("\n=== OPTIONS ===");
                    Console.WriteLine("1. Ajouter une assignation");
                    Console.WriteLine("2. Supprimer une assignation");
                    Console.WriteLine("3. Retour au menu");
                    Console.Write("\nVotre choix : ");
                    string choix = Console.ReadLine();

                    switch (choix)
                    {
                        case "1":
                            // Ajouter une assignation
                            Console.WriteLine("\n=== AJOUTER UNE ASSIGNATION ===");

                            // Lister les cuisiniers non assignés
                            Console.WriteLine("Liste des cuisiniers non assignés :");
                            string unassignedCooksQuery = @"
                        SELECT c.id_cuisinier, c.nom_cuisinier, c.prénom_cuisinier 
                        FROM CUISINIER c
                        WHERE c.id_cuisinier NOT IN (
                            SELECT DISTINCT e.id_cuisinier FROM EMPLOIE e
                        )
                        ORDER BY c.nom_cuisinier";

                            MySqlCommand unassignedCooksCmd = new MySqlCommand(unassignedCooksQuery, connection);

                            bool hasUnassignedCooks = false;
                            using (MySqlDataReader reader = unassignedCooksCmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    hasUnassignedCooks = true;
                                    while (reader.Read())
                                    {
                                        string id = reader["id_cuisinier"].ToString();
                                        string nom = reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) ? "N/A" : reader["nom_cuisinier"].ToString();
                                        string prenom = reader.IsDBNull(reader.GetOrdinal("prénom_cuisinier")) ? "N/A" : reader["prénom_cuisinier"].ToString();

                                        Console.WriteLine($"{id}. {nom} {prenom}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Tous les cuisiniers sont déjà assignés à des restaurants.");
                                }
                            }

                            if (!hasUnassignedCooks)
                            {
                                Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                                Console.ReadKey();
                                break;
                            }

                            Console.Write("\nID du cuisinier : ");
                            string idCuisinier = Console.ReadLine();

                            // Lister les restaurants
                            Console.WriteLine("\nListe des restaurants :");
                            List<(string id, string nom, string type, decimal capacite)> restaurants = ObtenirListeRestaurantsDetails();

                            if (restaurants.Count == 0)
                            {
                                Console.WriteLine("Aucun restaurant disponible.");
                                Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                                Console.ReadKey();
                                break;
                            }

                            foreach (var resto in restaurants)
                            {
                                Console.WriteLine($"{resto.id}. {resto.nom} - {resto.type}");
                            }

                            Console.Write("\nID du restaurant : ");
                            string idResto = Console.ReadLine();

                            // Lister les livreurs
                            Console.WriteLine("\nListe des livreurs :");
                            string livreurQuery = "SELECT ID_livreur, nom_livreur, prénom_livreur FROM LIVREUR ORDER BY nom_livreur";
                            MySqlCommand livreurCmd = new MySqlCommand(livreurQuery, connection);

                            using (MySqlDataReader reader = livreurCmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        string id = reader["ID_livreur"].ToString();
                                        string nom = reader.IsDBNull(reader.GetOrdinal("nom_livreur")) ? "N/A" : reader["nom_livreur"].ToString();
                                        string prenom = reader.IsDBNull(reader.GetOrdinal("prénom_livreur")) ? "N/A" : reader["prénom_livreur"].ToString();

                                        Console.WriteLine($"{id}. {nom} {prenom}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Aucun livreur disponible.");
                                    Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                                    Console.ReadKey();
                                    break;
                                }
                            }

                            Console.Write("\nID du livreur : ");
                            string idLivreur = Console.ReadLine();

                            // Ajouter l'assignation avec le livreur
                            string addAssignmentQuery = "INSERT INTO EMPLOIE (id_cuisinier, id_resto, ID_livreur) VALUES (@IdCuisinier, @IdResto, @IdLivreur)";
                            MySqlCommand addAssignmentCmd = new MySqlCommand(addAssignmentQuery, connection);
                            addAssignmentCmd.Parameters.AddWithValue("@IdCuisinier", idCuisinier);
                            addAssignmentCmd.Parameters.AddWithValue("@IdResto", idResto);
                            addAssignmentCmd.Parameters.AddWithValue("@IdLivreur", idLivreur);

                            try
                            {
                                addAssignmentCmd.ExecuteNonQuery();
                                Console.WriteLine("Assignation ajoutée avec succès !");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Erreur lors de l'ajout de l'assignation : {ex.Message}");
                            }
                            break;

                        case "2":
                            // Supprimer une assignation
                            Console.WriteLine("\n=== SUPPRIMER UNE ASSIGNATION ===");
                            Console.Write("ID du cuisinier : ");
                            string idCuisinierToRemove = Console.ReadLine();

                            Console.Write("ID du restaurant : ");
                            string idRestoToRemove = Console.ReadLine();

                            Console.Write("ID du livreur : ");
                            string idLivreurToRemove = Console.ReadLine();

                            string removeAssignmentQuery = "DELETE FROM EMPLOIE WHERE id_cuisinier = @IdCuisinier AND id_resto = @IdResto AND ID_livreur = @IdLivreur";
                            MySqlCommand removeAssignmentCmd = new MySqlCommand(removeAssignmentQuery, connection);
                            removeAssignmentCmd.Parameters.AddWithValue("@IdCuisinier", idCuisinierToRemove);
                            removeAssignmentCmd.Parameters.AddWithValue("@IdResto", idRestoToRemove);
                            removeAssignmentCmd.Parameters.AddWithValue("@IdLivreur", idLivreurToRemove);

                            try
                            {
                                int result = removeAssignmentCmd.ExecuteNonQuery();
                                if (result > 0)
                                {
                                    Console.WriteLine("Assignation supprimée avec succès !");
                                }
                                else
                                {
                                    Console.WriteLine("Aucune assignation trouvée avec ces identifiants.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Erreur lors de la suppression de l'assignation : {ex.Message}");
                            }
                            break;

                        case "3":
                            // Retour au menu
                            return;

                        default:
                            AfficherMessageErreur("Option invalide.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        #endregion

        #region Menu client
        public static void AfficherMenuClient()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                AfficherTitre($"BIENVENUE {utilisateurConnectePrenom} {utilisateurConnecteNom}");
                Console.WriteLine($"Connecté en tant que client (ID: {utilisateurConnecteId})");
                Console.WriteLine("\n--- MENU CLIENT ---");
                Console.WriteLine();
                Console.WriteLine("Que souhaitez-vous faire ? Veuillez choisir une option dans la liste suivante :");
                Console.WriteLine();
                Console.WriteLine("1. Voir mes commandes");
                Console.WriteLine("2. Passer une nouvelle commande");
                Console.WriteLine("3. Modifier mes informations personnelles");
                Console.WriteLine("4. Ajouter une nouvelle adresse");
                Console.WriteLine("5. Consulter le menu des restaurants");
                Console.WriteLine("6. Me déconnecter");

                Console.Write("\nChoisissez une option : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        Console.WriteLine("Vous avez choisi l'option 1: 'Voir mes commandes'");
                        VoirMesCommandes(utilisateurConnecteId);
                        break;
                    case "2":
                        Console.WriteLine("Vous avez choisi l'option 2: 'Passer une nouvelle commande'");
                        PasserNouvelleCommande(utilisateurConnecteId);
                        break;
                    case "3":
                        Console.WriteLine("Vous avez choisi l'option 3: 'Modifier mes informations personnelles'");
                        ModifierInformationsPersonnelles(utilisateurConnecteId, "client");
                        break;
                    case "4":
                        Console.WriteLine("Vous avez choisi l'option 4: 'Ajouter une nouvelle adresse'");
                        AjouterAdresse();
                        break;
                    case "5":
                        Console.WriteLine("Vous avez choisi l'option 5: 'Consulter le menu des restaurants'");
                        ConsulterMenuRestaurants();
                        break;
                    case "6":
                        Console.WriteLine("Vous avez choisi l'option 6: 'Me déconnecter");
                        utilisateurConnecteId = "";
                        utilisateurConnecteNom = "";
                        utilisateurConnectePrenom = "";
                        utilisateurConnecteType = "";
                        continuer = false;
                        break;
                    default:
                        AfficherMessageErreur("Option invalide.");
                        break;
                }
            }
        }
        #endregion

        #region Menu cuisinier
        public static void AfficherMenuCuisinier()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                AfficherTitre($"BIENVENUE {utilisateurConnectePrenom} {utilisateurConnecteNom}");
                Console.WriteLine($"Connecté en tant que cuisinier (ID: {utilisateurConnecteId})");
                Console.WriteLine("\n--- MENU CUISINIER ---");
                Console.WriteLine();
                Console.WriteLine("Que souhaitez-vous faire ? Veuillez choisir une option dans la liste suivante :");
                Console.WriteLine();
                Console.WriteLine("1. Voir les commandes à préparer");
                Console.WriteLine("2. Ajouter un plat au menu");
                Console.WriteLine("3. Modifier mes informations personnelles");
                Console.WriteLine("4. Marquer une commande comme préparée");
                Console.WriteLine("5. Gérer les ingrédients");
                Console.WriteLine("6. Voir les clients servis");
                Console.WriteLine("7. Voir mes plats par fréquence");
                Console.WriteLine("8. Modifier ou annuler une commande");
                Console.WriteLine("9. Me déconnecter");

                Console.Write("\nChoisissez une option : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        VoirCommandesAPreparer(utilisateurConnecteId);
                        break;
                    case "2":
                        AjouterPlatAuMenu();
                        break;
                    case "3":
                        ModifierInformationsPersonnelles(utilisateurConnecteId, "cuisinier");
                        break;
                    case "4":
                        MarquerCommandePreparee();
                        break;
                    case "5":
                        GererIngredients();
                        break;
                    case "6":
                        VoirClientsServis(utilisateurConnecteId);
                        break;
                    case "7":
                        VoirPlatsCuisinesByFrequence(utilisateurConnecteId);
                        break;
                    case "8":
                        ModifierOuAnnulerCommande(utilisateurConnecteId);
                        break;
                    case "9":
                        utilisateurConnecteId = "";
                        utilisateurConnecteNom = "";
                        utilisateurConnectePrenom = "";
                        utilisateurConnecteType = "";
                        continuer = false;
                        break;
                    default:
                        AfficherMessageErreur("Option invalide.");
                        break;
                }
            }
        }
        #endregion

        #region Menu livreur
        public static void AfficherMenuLivreur()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                AfficherTitre($"BIENVENUE {utilisateurConnectePrenom} {utilisateurConnecteNom}");
                Console.WriteLine($"Connecté en tant que livreur (ID: {utilisateurConnecteId})");
                Console.WriteLine("\n--- MENU LIVREUR ---");
                Console.WriteLine();
                Console.WriteLine("Que souhaitez-vous faire ? Veuillez choisir une option dans la liste suivante :");
                Console.WriteLine();
                Console.WriteLine("1. Voir les commandes à livrer");
                Console.WriteLine("2. Consulter mon itinéraire");
                Console.WriteLine("3. Modifier mes informations personnelles");
                Console.WriteLine("4. Marquer une commande comme livrée");
                Console.WriteLine("5. Voir mes évaluations");
                Console.WriteLine("6. Me déconnecter");

                Console.Write("\nChoisissez une option : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        VoirCommandesALivrer(utilisateurConnecteId);
                        break;
                    case "2":
                        ConsulterItineraire(utilisateurConnecteId);
                        break;
                    case "3":
                        ModifierInformationsPersonnelles(utilisateurConnecteId, "livreur");
                        break;
                    case "4":
                        MarquerCommandeLivree();
                        break;
                    case "5":
                        VoirEvaluations(utilisateurConnecteId);
                        break;
                    case "6":
                        utilisateurConnecteId = "";
                        utilisateurConnecteNom = "";
                        utilisateurConnectePrenom = "";
                        utilisateurConnecteType = "";
                        continuer = false;
                        break;
                    default:
                        AfficherMessageErreur("Option invalide.");
                        break;
                }
            }
        }
        #endregion


        #region Méthodes pour le menu client 
        static void VoirMesCommandes(string clientId)
        {
            Console.Clear();
            AfficherTitre("MES COMMANDES");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete = "SELECT c.ID_commande, c.nom, c.prix, c.quantite, c.type_plat, c.date_commande, " +
                                    "r.nom_resto " +
                                    "FROM COMMANDE c " +
                                    "JOIN PASSE p ON c.ID_commande = p.ID_commande " +
                                    "LEFT JOIN RÉCEPTIONNE re ON c.ID_commande = re.ID_commande " +
                                    "LEFT JOIN RESTAURANT r ON re.id_resto = r.id_resto " +
                                    "WHERE p.id_client = @ClientId " +
                                    "ORDER BY c.date_commande DESC";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@ClientId", clientId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("ID\tDate\t\tPlat\t\t\tPrix\tQté\t\tRestaurant");
                            Console.WriteLine("----------------------------------------------------------------------------------");

                            while (reader.Read())
                            {
                                string id = reader["ID_commande"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom")) ? "N/A" : reader["nom"].ToString();
                                string prixStr = reader.IsDBNull(reader.GetOrdinal("prix")) ? "N/A" : reader["prix"].ToString();
                                int quantite = reader.IsDBNull(reader.GetOrdinal("quantite")) ? 0 : Convert.ToInt32(reader["quantite"]);
                                string restaurant = reader.IsDBNull(reader.GetOrdinal("nom_resto")) ? "N/A" : reader["nom_resto"].ToString();
                                string dateCommande = reader.IsDBNull(reader.GetOrdinal("date_commande")) ?
                                    "N/A" : reader["date_commande"].ToString();

                                // Display the price as is since it's already stored as a formatted string
                                Console.WriteLine($"{id}\t{dateCommande}\t{nom,-15}{prixStr}\t{quantite}\t{restaurant}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Vous n'avez pas encore passé de commande.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        /// <summary>
        /// Permet à un client de passer une nouvelle commande en choisissant un restaurant et des plats.
        /// Corrige le problème de complétion prématurée de la commande.
        /// </summary>
        /// <param name="clientId">ID du client qui passe la commande</param>
        static void PasserNouvelleCommande(string clientId)
        {
            Console.Clear();
            AfficherTitre("PASSER UNE NOUVELLE COMMANDE");

            // Afficher la liste des restaurants
            Console.WriteLine("Liste des restaurants disponibles :");
            List<(string id, string nom, string type, decimal capacite)> restaurants = ObtenirListeRestaurantsDetails();

            if (restaurants.Count == 0)
            {
                AfficherMessageErreur("Aucun restaurant disponible.");
                return;
            }

            foreach (var resto in restaurants)
            {
                Console.WriteLine($"{resto.id}. {resto.nom} - {resto.type}");
            }

            Console.Write("\nChoisissez un restaurant (ID) : ");
            string idResto = Console.ReadLine();

            // Afficher les options de commande
            Console.WriteLine("\nComment souhaitez-vous commander ?");
            Console.WriteLine("1. Choisir un menu prédéfini");
            Console.WriteLine("2. Composer mon propre menu");

            Console.Write("Votre choix : ");
            string choixMenu = Console.ReadLine();

            decimal prixTotal = 0;
            List<(string nom, string type, decimal prix, int quantite)> platsChoisis = new List<(string, string, decimal, int)>();

            if (choixMenu == "1")
            {
                // Afficher les menus disponibles pour ce restaurant
                List<(string id, string entree, string plat, string dessert, string boisson, string regime, decimal prix)> menus = ObtenirMenusRestaurant(idResto);

                if (menus.Count == 0)
                {
                    Console.WriteLine("Aucun menu disponible pour ce restaurant.");
                    Console.WriteLine("Vous allez être redirigé vers la composition de votre propre menu.");
                    choixMenu = "2";
                }
                else
                {
                    Console.WriteLine("\nMenus disponibles :");
                    Console.WriteLine("ID\tEntrée\t\tPlat\t\tDessert\t\tBoisson\t\tRégime\t\tPrix");
                    Console.WriteLine("-------------------------------------------------------------------------");

                    foreach (var menu in menus)
                    {
                        Console.WriteLine($"{menu.id}\t{menu.entree,-15}{menu.plat,-15}{menu.dessert,-15}{menu.boisson,-15}{menu.regime,-15}{menu.prix.ToString("F2")} euros");
                    }

                    Console.Write("\nChoisissez un menu (ID) : ");
                    string idMenu = Console.ReadLine();

                    // Trouver le menu sélectionné
                    var menuSelectionne = menus.Find(m => m.id == idMenu);
                    if (menuSelectionne != default)
                    {
                        Console.Write("Quantité : ");
                        int quantite = 1;
                        if (int.TryParse(Console.ReadLine(), out int qte))
                        {
                            quantite = qte;
                        }

                        platsChoisis.Add(($"Menu {menuSelectionne.plat}", menuSelectionne.regime, menuSelectionne.prix, quantite));
                        prixTotal += menuSelectionne.prix * quantite;

                        // Demander si le client veut ajouter autre chose
                        Console.Write("\nSouhaitez-vous ajouter autre chose ? (O/N) : ");
                        if (Console.ReadLine().ToUpper() == "O")
                        {
                            choixMenu = "2";
                        }
                    }
                    else
                    {
                        Console.WriteLine("Menu non trouvé. Vous allez être redirigé vers la composition de votre propre menu.");
                        choixMenu = "2";
                    }
                }
            }

            if (choixMenu == "2")
            {
                bool continuerAjout = true;

                while (continuerAjout)
                {
                    // Composer son propre menu
                    Console.WriteLine("\n--- COMPOSER VOTRE MENU ---");

                    // Prix par défaut pour chaque catégorie
                    decimal prixEntree = 3.0m;
                    decimal prixPlat = 10.0m;
                    decimal prixDessert = 3.0m;
                    decimal prixBoisson = 2m;

                    decimal prixItem = 0;
                    string typeItem = "";

                    Console.WriteLine("Que souhaitez-vous ajouter ?");
                    Console.WriteLine("1. Une entrée");
                    Console.WriteLine("2. Un plat principal");
                    Console.WriteLine("3. Un dessert");
                    Console.WriteLine("4. Une boisson");
                    Console.Write("Votre choix : ");
                    string choixItem = Console.ReadLine();

                    switch (choixItem)
                    {
                        case "1":
                            prixItem = prixEntree;
                            typeItem = "Entrée";
                            break;
                        case "2":
                            prixItem = prixPlat;
                            typeItem = "Plat principal";
                            break;
                        case "3":
                            prixItem = prixDessert;
                            typeItem = "Dessert";
                            break;
                        case "4":
                            prixItem = prixBoisson;
                            typeItem = "Boisson";
                            break;
                        default:
                            Console.WriteLine("Choix invalide.");
                            continue;
                    }

                    Console.Write("Nom du plat : ");
                    string nomPlat = Console.ReadLine();

                    Console.Write("Type de régime (standard, végétarien, halal, sans gluten, etc.) : ");
                    string typePlat = Console.ReadLine();

                    Console.Write("Quantité : ");
                    int quantite = 1;
                    if (int.TryParse(Console.ReadLine(), out int qte))
                    {
                        quantite = qte;
                    }

                    platsChoisis.Add((nomPlat, typePlat, prixItem, quantite));
                    prixTotal += prixItem * quantite;

                    Console.WriteLine($"\nPlat ajouté : {nomPlat} ({quantite}x) - {(prixItem * quantite).ToString("F2")} euros");
                    Console.WriteLine($"Prix total actuel : {prixTotal.ToString("F2")} euros");

                    Console.Write("\nSouhaitez-vous ajouter autre chose ? (O/N) : ");
                    continuerAjout = Console.ReadLine().ToUpper() == "O";
                }
            }

            // Créer une nouvelle commande pour chaque plat choisi
            if (platsChoisis.Count > 0)
            {
                bool toutesCommandesReussies = true;

                foreach (var plat in platsChoisis)
                {
                    int idCommande = CreerNouvelleCommande(clientId, idResto, plat.nom, plat.prix, plat.quantite, plat.type);
                    if (idCommande == 0)
                    {
                        AfficherMessageErreur($"Échec de la création de la commande pour {plat.nom}.");
                        toutesCommandesReussies = false;
                        continue;
                    }
                }

                if (toutesCommandesReussies)
                {
                    Console.WriteLine($"\nCommande complétée avec succès ! Prix total : {prixTotal.ToString("F2")} euros");
                }
                else
                {
                    Console.WriteLine("\nCertaines parties de votre commande n'ont pas pu être traitées.");
                }
            }
            else
            {
                Console.WriteLine("\nAucun plat n'a été choisi. La commande a été annulée.");
            }

            Console.WriteLine("Appuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        static void ModifierInformationsPersonnelles(string userId, string userType)
        {
            Console.Clear();
            AfficherTitre("MODIFIER MES INFORMATIONS PERSONNELLES");

            string table = "";
            string id = "";
            string mail = "";
            string tel = "";

            switch (userType)
            {
                case "client":
                    table = "CLIENT";
                    id = "id_client";
                    mail = "email";
                    tel = "tel";
                    break;
                case "cuisinier":
                    table = "CUISINIER";
                    id = "id_cuisinier";
                    mail = "email_cuisinier";
                    tel = "tel_cuisinier";
                    break;
                case "livreur":
                    table = "LIVREUR";
                    id = "ID_livreur";
                    mail = "email_livreur";
                    tel = "tel_livreur";
                    break;
            }

            Console.WriteLine("Que souhaitez-vous modifier ?");
            Console.WriteLine("1. Email");
            Console.WriteLine("2. Téléphone");
            Console.WriteLine("3. Mot de passe");
            Console.WriteLine("4. Retour au menu");

            Console.Write("\nChoisissez une option : ");
            string choix = Console.ReadLine();

            if (choix == "4")
            {
                return;
            }
            else if (choix == "3")
            {
                ModifierMotDePasse(userId, userType);
                return;
            }

            string champAModifier;
            string message;

            if (choix == "1")
            {
                champAModifier = mail;
                message = "Nouveau mail : ";
            }
            else
            {
                champAModifier = tel;
                message = "Nouveau numéro de téléphone : ";
            }

            Console.Write(message);
            string nouvelleValeur = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete = $"UPDATE {table} SET {champAModifier} = @NouvelleValeur WHERE {id} = @UserId";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@NouvelleValeur", nouvelleValeur);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine("Information mise à jour avec succès !");
                    }
                    else
                    {
                        Console.WriteLine("Échec de la mise à jour.");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        static void ModifierMotDePasse(string userId, string userType)
        {
            Console.Clear();
            AfficherTitre("MODIFIER MON MOT DE PASSE");

            Console.Write("Nouveau mot de passe : ");
            string nouveauMotDePasse = Console.ReadLine();

            Console.Write("Confirmer le nouveau mot de passe : ");
            string confirmationMotDePasse = Console.ReadLine();

            if (nouveauMotDePasse != confirmationMotDePasse)
            {
                AfficherMessageErreur("Les mots de passe ne correspondent pas.");
                return;
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string table = "";
                    string idField = "";
                    string mdpField = "";

                    switch (userType)
                    {
                        case "client":
                            table = "CLIENT";
                            idField = "id_client";
                            mdpField = "mot_de_passe";
                            break;
                        case "cuisinier":
                            table = "CUISINIER";
                            idField = "id_cuisinier";
                            mdpField = "mot_de_passe";
                            break;
                        case "livreur":
                            table = "LIVREUR";
                            idField = "ID_livreur";
                            mdpField = "mot_de_passe_livreur";
                            break;
                    }

                    // Mettre à jour le mot de passe
                    string updateQuery = $"UPDATE {table} SET {mdpField} = @MotDePasse WHERE {idField} = @UserId";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
                    updateCmd.Parameters.AddWithValue("@MotDePasse", nouveauMotDePasse);
                    updateCmd.Parameters.AddWithValue("@UserId", userId);
                    int result = updateCmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine("Mot de passe modifié avec succès !");
                    }
                    else
                    {
                        Console.WriteLine("Échec de la modification du mot de passe.");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        static void ConsulterMenuRestaurants()
        {
            Console.Clear();
            AfficherTitre("MENU DES RESTAURANTS");

            // Afficher la liste des restaurants
            Console.WriteLine("Liste des restaurants disponibles :");
            List<(string id, string nom, string type, decimal capacite)> restaurants = ObtenirListeRestaurantsDetails();

            if (restaurants.Count == 0)
            {
                AfficherMessageErreur("Aucun restaurant disponible.");
                return;
            }

            foreach (var resto in restaurants)
            {
                Console.WriteLine($"{resto.id}. {resto.nom} - {resto.type}");
            }

            Console.Write("\nChoisissez un restaurant (ID) pour voir son menu : ");
            string idResto = Console.ReadLine();

            // Afficher le menu du restaurant
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT id_menu, entrée, plat, dessert, boisson, régime_correspondant " +
                                "FROM MENU WHERE id_resto = @IdResto";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    if (query.Contains("@IdResto"))
                    {
                        cmd.Parameters.AddWithValue("@IdResto", idResto);
                    }

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("\n--- MENU ---");

                            while (reader.Read())
                            {
                                string entrees = reader.IsDBNull(reader.GetOrdinal("entrée")) ? "N/A" : reader["entrée"].ToString();
                                string plat = reader.IsDBNull(reader.GetOrdinal("plat")) ? "N/A" : reader["plat"].ToString();
                                string dessert = reader.IsDBNull(reader.GetOrdinal("dessert")) ? "N/A" : reader["dessert"].ToString();
                                string boisson = reader.IsDBNull(reader.GetOrdinal("boisson")) ? "N/A" : reader["boisson"].ToString();
                                string regime = reader.IsDBNull(reader.GetOrdinal("régime_correspondant")) ? "N/A" : reader["régime_correspondant"].ToString();

                                Console.WriteLine($"Entrées: {entrees}");
                                Console.WriteLine($"Plat: {plat}");
                                Console.WriteLine($"Dessert: {dessert}");
                                Console.WriteLine($"Boisson: {boisson}");
                                Console.WriteLine($"Régime: {regime}");
                                Console.WriteLine("-------------------------");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun menu disponible pour ce restaurant.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }
        #endregion

        #region Méthodes pour le menu cuisinier


        /// <summary>
        /// Affiche les commandes à préparer pour un cuisinier spécifique, en distinguant les commandes non assignées de celles déjà assignées.
        /// </summary>
        static void VoirCommandesAPreparer(string cuisinierId)
        {
            Console.Clear();
            AfficherTitre("COMMANDES À PRÉPARER");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string getRestosQuery = "SELECT id_resto FROM EMPLOIE WHERE id_cuisinier = @CuisinierId"; // Obtenir les restaurants où travaille le cuisinier
                    MySqlCommand getRestosCmd = new MySqlCommand(getRestosQuery, connection);
                    getRestosCmd.Parameters.AddWithValue("@CuisinierId", cuisinierId);

                    List<string> restaurantIds = new List<string>();
                    using (MySqlDataReader reader = getRestosCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            restaurantIds.Add(reader["id_resto"].ToString());
                        }
                    }

                    if (restaurantIds.Count == 0)
                    {
                        Console.WriteLine("Vous n'êtes assigné à aucun restaurant. Veuillez contacter l'administrateur.");
                        Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                        Console.ReadKey();
                        return;
                    }

                    Console.WriteLine("=== COMMANDES NON ASSIGNÉES DE VOS RESTAURANTS ===");

                    string unassignedOrdersQuery = @"
                SELECT c.ID_commande, c.nom, c.quantite, c.type_plat, c.prix,
                       cl.nom AS nom_client, cl.prénom AS prenom_client,
                       r.nom_resto, c.date_commande
                FROM COMMANDE c
                JOIN PASSE p ON c.ID_commande = p.ID_commande
                JOIN CLIENT cl ON p.id_client = cl.id_client
                JOIN RÉCEPTIONNE re ON c.ID_commande = re.ID_commande
                JOIN RESTAURANT r ON re.id_resto = r.id_resto
                LEFT JOIN PRÉPARE pr ON c.ID_commande = pr.ID_commande
                WHERE pr.ID_commande IS NULL
                AND c.date_de_fabrication IS NULL
                AND re.id_resto IN (" + string.Join(",", restaurantIds.Select(id => "'" + id + "'")) + ")";

                    MySqlCommand unassignedCmd = new MySqlCommand(unassignedOrdersQuery, connection);

                    bool hasUnassignedOrders = false;

                    using (MySqlDataReader reader = unassignedCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            hasUnassignedOrders = true;
                            Console.WriteLine("ID\tDate\t\tNom du plat\t\tQté\tPrix\t\tClient\t\tRestaurant");
                            Console.WriteLine("-------------------------------------------------------------------------");

                            while (reader.Read())
                            {
                                string id = reader["ID_commande"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom")) ? "N/A" : reader["nom"].ToString();
                                int quantite = reader.IsDBNull(reader.GetOrdinal("quantite")) ? 0 : Convert.ToInt32(reader["quantite"]);
                                string prix = reader.IsDBNull(reader.GetOrdinal("prix")) ? "N/A" : reader["prix"].ToString();

                                string nomClient = reader.IsDBNull(reader.GetOrdinal("nom_client")) ? "N/A" : reader["nom_client"].ToString();
                                string prenomClient = reader.IsDBNull(reader.GetOrdinal("prenom_client")) ? "" : reader["prenom_client"].ToString();
                                string client = $"{nomClient} {prenomClient}";

                                string restaurant = reader.IsDBNull(reader.GetOrdinal("nom_resto")) ? "N/A" : reader["nom_resto"].ToString();
                                string dateCommande = reader.IsDBNull(reader.GetOrdinal("date_commande")) ?
                                    "N/A" : reader["date_commande"].ToString();
                                Console.WriteLine($"{id}\t{dateCommande}\t{nom,-15}{quantite}\t{prix}\t{client,-20}{restaurant}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucune commande non assignée à préparer.");
                        }
                    }

                    if (hasUnassignedOrders)
                    {
                        Console.Write("\nVoulez-vous prendre en charge une commande ? (O/N) : ");
                        if (Console.ReadLine().ToUpper() == "O")
                        {
                            Console.Write("Entrez l'ID de la commande à prendre en charge : ");
                            string idCommande = Console.ReadLine();

                            // Vérifier que la commande existe et appartient à l'un des restaurants du cuisinier
                            string verifyOrderQuery = @"
                        SELECT c.ID_commande
                        FROM COMMANDE c
                        JOIN RÉCEPTIONNE re ON c.ID_commande = re.ID_commande
                        WHERE c.ID_commande = @IdCommande
                        AND re.id_resto IN (" + string.Join(",", restaurantIds.Select(id => "'" + id + "'")) + ")";

                            MySqlCommand verifyOrderCmd = new MySqlCommand(verifyOrderQuery, connection);
                            verifyOrderCmd.Parameters.AddWithValue("@IdCommande", idCommande);

                            object orderExists = verifyOrderCmd.ExecuteScalar();

                            if (orderExists != null)
                            {
                                string assignQuery = "INSERT INTO PRÉPARE (id_cuisinier, ID_commande) VALUES (@IdCuisinier, @IdCommande)";
                                MySqlCommand assignCmd = new MySqlCommand(assignQuery, connection);
                                assignCmd.Parameters.AddWithValue("@IdCuisinier", cuisinierId);
                                assignCmd.Parameters.AddWithValue("@IdCommande", idCommande);
                                try
                                {
                                    assignCmd.ExecuteNonQuery();
                                    Console.WriteLine("Commande prise en charge avec succès !");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Erreur lors de la prise en charge : {ex.Message}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Cette commande n'existe pas ou n'appartient pas au restaurant où vous êtes assigné(e).");
                            }
                        }
                    }

                    // Afficher les commandes déjà assignées à ce cuisinier
                    Console.WriteLine("\n=== VOS COMMANDES À PRÉPARER ===");

                    string assignedOrdersQuery = @"
                SELECT c.ID_commande, c.nom, c.quantite, c.type_plat, c.prix,
                       cl.nom AS nom_client, cl.prénom AS prenom_client,
                       r.nom_resto, c.date_commande
                FROM COMMANDE c
                JOIN PASSE p ON c.ID_commande = p.ID_commande
                JOIN CLIENT cl ON p.id_client = cl.id_client
                JOIN RÉCEPTIONNE re ON c.ID_commande = re.ID_commande
                JOIN RESTAURANT r ON re.id_resto = r.id_resto
                JOIN PRÉPARE pr ON c.ID_commande = pr.ID_commande
                WHERE pr.id_cuisinier = @CuisinierId
                AND c.date_de_fabrication IS NULL
                ORDER BY c.date_commande ASC";

                    MySqlCommand assignedCmd = new MySqlCommand(assignedOrdersQuery, connection);
                    assignedCmd.Parameters.AddWithValue("@CuisinierId", cuisinierId);

                    using (MySqlDataReader reader = assignedCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("ID\tDate\t\tPlat\t\tQté\tPrix\t\tClient\t\tRestaurant");
                            Console.WriteLine("-------------------------------------------------------------------------");

                            while (reader.Read())
                            {
                                string id = reader["ID_commande"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom")) ? "N/A" : reader["nom"].ToString();
                                int quantite = reader.IsDBNull(reader.GetOrdinal("quantite")) ? 0 : Convert.ToInt32(reader["quantite"]);
                                string prix = reader.IsDBNull(reader.GetOrdinal("prix")) ? "N/A" : reader["prix"].ToString();

                                string nomClient = reader.IsDBNull(reader.GetOrdinal("nom_client")) ? "N/A" : reader["nom_client"].ToString();
                                string prenomClient = reader.IsDBNull(reader.GetOrdinal("prenom_client")) ? "" : reader["prenom_client"].ToString();
                                string client = $"{nomClient} {prenomClient}";

                                string restaurant = reader.IsDBNull(reader.GetOrdinal("nom_resto")) ? "N/A" : reader["nom_resto"].ToString();
                                string dateCommande = reader.IsDBNull(reader.GetOrdinal("date_commande")) ?
                                    "N/A" : reader["date_commande"].ToString();

                                Console.WriteLine($"{id}\t{dateCommande}\t{nom,-15}{quantite}\t{prix}\t{client,-20}{restaurant}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucune commande à préparer.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        /// <summary>
        /// Permet d’ajouter un nouveau plat à un menu en sélectionnant un restaurant, en saisissant les détails du plat,
        /// puis en enregistrant ces données dans la base. Affiche un message de confirmation ou d'erreur selon le résultat.
        /// </summary>
        static void AjouterPlatAuMenu()
        {
            Console.Clear();
            AfficherTitre("AJOUTER UN PLAT AU MENU");

            try
            {
                Console.WriteLine("Liste des restaurants disponibles :");
                List<(string id, string nom, string type, decimal capacite)> restaurants = ObtenirListeRestaurantsDetails();

                if (restaurants.Count == 0)
                {
                    AfficherMessageErreur("Aucun restaurant disponible.");
                    return;
                }

                foreach (var resto in restaurants)
                {
                    Console.WriteLine($"{resto.id}. {resto.nom} - {resto.type}");
                }

                Console.Write("\nChoisissez un restaurant (ID) : ");
                string idResto = Console.ReadLine();

                var restaurantSelectionne = restaurants.Find(r => r.id == idResto); // Vérifier si le restaurant existe
                if (restaurantSelectionne == default)
                {
                    AfficherMessageErreur("Restaurant non trouvé.");
                    return;
                }

                Console.WriteLine("\n--- Informations du plat ---");

                Console.Write("ID du menu : ");
                string idMenu = Console.ReadLine();

                Console.Write("Entrée : ");
                string entree = Console.ReadLine();

                Console.Write("Plat principal : ");
                string plat = Console.ReadLine();

                Console.Write("Dessert : ");
                string dessert = Console.ReadLine();

                Console.Write("Boisson : ");
                string boisson = Console.ReadLine();

                Console.Write("Régime particulier (standard, végétarien, etc.) : ");
                string regime = Console.ReadLine();

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO MENU (id_menu, entrée, plat, dessert, boisson, régime_correspondant, id_resto) " +
                                "VALUES (@IdMenu, @Entree, @Plat, @Dessert, @Boisson, @Regime, @IdResto)";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdMenu", idMenu);
                    cmd.Parameters.AddWithValue("@Entree", entree);
                    cmd.Parameters.AddWithValue("@Plat", plat);
                    cmd.Parameters.AddWithValue("@Dessert", dessert);
                    cmd.Parameters.AddWithValue("@Boisson", boisson);
                    cmd.Parameters.AddWithValue("@Regime", regime);
                    cmd.Parameters.AddWithValue("@IdResto", idResto);
                    
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine("Plat ajouté au menu avec succès !");
                    }
                    else
                    {
                        Console.WriteLine("Échec de l'ajout du plat au menu.");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        /// <summary>
        /// Affiche la liste des clients ayant été servis par un cuisinier donné, avec leurs coordonnées,
        /// le nombre de commandes passées et la date de la dernière commande, en les récupérant depuis la BDD.
        /// </summary>
        static void VoirClientsServis(string cuisinierId)
        {
            Console.Clear();
            AfficherTitre("CLIENTS SERVIS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete = "SELECT DISTINCT cl.id_client, cl.nom, cl.prénom, cl.email, cl.tel, " +
                                "COUNT(c.ID_commande) as nb_commandes, " +
                                "MAX(c.date_commande) as derniere_commande " +
                                "FROM CLIENT cl " +
                                "JOIN PASSE p ON cl.id_client = p.id_client " +
                                "JOIN COMMANDE c ON p.ID_commande = c.ID_commande " +
                                "JOIN PRÉPARE pr ON c.ID_commande = pr.ID_commande " +
                                "WHERE pr.id_cuisinier = @CuisinierId " +
                                "GROUP BY cl.id_client, cl.nom, cl.prénom, cl.email, cl.tel " +
                                "ORDER BY nb_commandes DESC";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    if (requete.Contains("@CuisinierId"))
                    {
                        cmd.Parameters.AddWithValue("@CuisinierId", cuisinierId);
                    }

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("ID\tNom\t\tPrénom\t\tEmail\t\t\tTéléphone\tNb Commandes\tDernière Commande");
                            Console.WriteLine("-----------------------------------------------------------------------------------------------------");

                            while (reader.Read())
                            {
                                string id = reader["id_client"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom")) ? "N/A" : reader["nom"].ToString();
                                string prenom = reader.IsDBNull(reader.GetOrdinal("prénom")) ? "N/A" : reader["prénom"].ToString();
                                string email = reader.IsDBNull(reader.GetOrdinal("email")) ? "N/A" : reader["email"].ToString();
                                string tel = reader.IsDBNull(reader.GetOrdinal("tel")) ? "N/A" : reader["tel"].ToString();
                                int nbCommandes = reader.IsDBNull(reader.GetOrdinal("nb_commandes")) ? 0 : Convert.ToInt32(reader["nb_commandes"]);
                                string derniereCommande = reader.IsDBNull(reader.GetOrdinal("derniere_commande")) ? "N/A" : reader["derniere_commande"].ToString();

                                Console.WriteLine($"{id}\t{nom,-15}{prenom,-15}{email,-25}{tel,-15}{nbCommandes,-15}{derniereCommande}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Vous n'avez servi encore aucun client ou la base n'a pas encore été actualisée.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }


        /// <summary>
        /// Affiche les plats préparés par un cuisinier, triés par fréquence de préparation,
        /// en indiquant le type de plat, le nombre total de préparations et la quantité préparée.
        /// </summary>
        static void VoirPlatsCuisinesByFrequence(string cuisinierId)
        {
            Console.Clear();
            AfficherTitre("MES PLATS PAR FRÉQUENCE");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete = "SELECT c.nom, c.type_plat, COUNT(c.ID_commande) as nb_preparations, " +
                                "SUM(c.quantite) as quantite_totale " +
                                "FROM COMMANDE c " +
                                "JOIN PRÉPARE pr ON c.ID_commande = pr.ID_commande " +
                                "WHERE pr.id_cuisinier = @CuisinierId " +
                                "GROUP BY c.nom, c.type_plat " +
                                "ORDER BY nb_preparations DESC";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    if (requete.Contains("@CuisinierId"))
                    {
                        cmd.Parameters.AddWithValue("@CuisinierId", cuisinierId);
                    }

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("Plat\t\tType\t\tNombre de préparations\tQuantité totale");
                            Console.WriteLine("---------------------------------------------------------------------");

                            while (reader.Read())
                            {
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom")) ? "N/A" : reader["nom"].ToString();
                                string type = reader.IsDBNull(reader.GetOrdinal("type_plat")) ? "N/A" : reader["type_plat"].ToString();
                                int nbPreparations = reader.IsDBNull(reader.GetOrdinal("nb_preparations")) ? 0 : Convert.ToInt32(reader["nb_preparations"]);
                                int quantiteTotale = reader.IsDBNull(reader.GetOrdinal("quantite_totale")) ? 0 : Convert.ToInt32(reader["quantite_totale"]);

                                Console.WriteLine($"{nom,-15}{type,-15}{nbPreparations,-25}{quantiteTotale}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Vous n'avez encore préparé aucun plat.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        /// <summary>
        /// Permet à un cuisinier de consulter ses commandes en attente de préparation, puis de choisir d’en modifier 
        /// ou d’en annuler une. Seules les commandes sans date de fabrication (= pas encore prêtes) sont concernées.
        /// </summary>
        static void ModifierOuAnnulerCommande(string cuisinierId)
        {
            Console.Clear();
            AfficherTitre("MODIFIER OU ANNULER UNE COMMANDE");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete = "SELECT c.ID_commande, c.nom, c.quantite, c.type_plat, c.prix, " +
                                "cl.nom AS nom_client, cl.prénom AS prenom_client, " +
                                "r.nom_resto, c.date_commande " +
                                "FROM COMMANDE c " +
                                "JOIN PASSE p ON c.ID_commande = p.ID_commande " +
                                "JOIN CLIENT cl ON p.id_client = cl.id_client " +
                                "JOIN PRÉPARE pr ON c.ID_commande = pr.ID_commande " +
                                "LEFT JOIN RÉCEPTIONNE re ON c.ID_commande = re.ID_commande " +
                                "LEFT JOIN RESTAURANT r ON re.id_resto = r.id_resto " +
                                "WHERE pr.id_cuisinier = @CuisinierId AND c.date_de_fabrication IS NULL " +
                                "ORDER BY c.date_commande ASC";
                   
                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    if (requete.Contains("@CuisinierId"))
                    {
                        cmd.Parameters.AddWithValue("@CuisinierId", cuisinierId);
                    }

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("ID\tDate\t\tPlat\t\tQté\tPrix\t\tClient\t\tRestaurant");
                            Console.WriteLine("-------------------------------------------------------------------------");

                            while (reader.Read())
                            {
                                string id = reader["ID_commande"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom")) ? "N/A" : reader["nom"].ToString();
                                int quantite = reader.IsDBNull(reader.GetOrdinal("quantite")) ? 0 : Convert.ToInt32(reader["quantite"]);
                                decimal prix = reader.IsDBNull(reader.GetOrdinal("prix")) ? 0 : Convert.ToDecimal(reader["prix"]);

                                string nomClient = reader.IsDBNull(reader.GetOrdinal("nom_client")) ? "N/A" : reader["nom_client"].ToString();
                                string prenomClient = reader.IsDBNull(reader.GetOrdinal("prenom_client")) ? "" : reader["prenom_client"].ToString();
                                string client = $"{nomClient} {prenomClient}";

                                string restaurant = reader.IsDBNull(reader.GetOrdinal("nom_resto")) ? "N/A" : reader["nom_resto"].ToString();
                                string dateCommande = reader.IsDBNull(reader.GetOrdinal("date_commande")) ?
                                    "N/A" : reader["date_commande"].ToString();

                                Console.WriteLine($"{id}\t{dateCommande}\t{nom,-15}{quantite}\t{prix.ToString("F2")} euros\t{client,-20}{restaurant}");
                            }

                            Console.Write("\nEntrez l'ID de la commande à modifier/annuler (0 pour annuler) : ");
                            string idCommande = Console.ReadLine();

                            if (idCommande != "0")
                            {
                                Console.WriteLine("\nQue souhaitez-vous faire ?");
                                Console.WriteLine("1. Modifier la commande");
                                Console.WriteLine("2. Annuler la commande");
                                Console.Write("Votre choix : ");
                                string action = Console.ReadLine();

                                if (action == "1")
                                {
                                    ModifierCommande(idCommande);
                                }
                                else if (action == "2")
                                {
                                    AnnulerCommande(idCommande);
                                }
                                else
                                {
                                    Console.WriteLine("Option invalide.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucune commande à préparer.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        /// <summary>
        /// Permet de modifier les détails d'une commande existante (nom du plat, quantité, prix, type de plat).
        /// Les valeurs actuelles sont proposées par défaut, et le cuisinier peut choisir de les modifier.
        /// </summary>
        static void ModifierCommande(string idCommande)
        {
            Console.Clear();
            AfficherTitre("MODIFIER LA COMMANDE");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Récupérer les informations actuelles de la commande
                    string query = "SELECT nom, quantite, prix, type_plat FROM COMMANDE WHERE ID_commande = @IdCommande";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdCommande", idCommande);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nomActuel = reader.IsDBNull(reader.GetOrdinal("nom")) ? "" : reader["nom"].ToString();
                            int quantiteActuelle = reader.IsDBNull(reader.GetOrdinal("quantite")) ? 0 : Convert.ToInt32(reader["quantite"]);
                            decimal prixActuel = reader.IsDBNull(reader.GetOrdinal("prix")) ? 0 : Convert.ToDecimal(reader["prix"]);
                            string typePlatActuel = reader.IsDBNull(reader.GetOrdinal("type_plat")) ? "" : reader["type_plat"].ToString();

                            reader.Close();

                            Console.WriteLine($"Modification de la commande : {nomActuel}");

                            Console.Write($"Nouveau nom du plat ({nomActuel}) : ");
                            string nouveauNom = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauNom))
                                nouveauNom = nomActuel;

                            Console.Write($"Nouvelle quantité ({quantiteActuelle}) : ");
                            string quantiteInput = Console.ReadLine();
                            int nouvelleQuantite = quantiteActuelle;
                            if (!string.IsNullOrWhiteSpace(quantiteInput) && int.TryParse(quantiteInput, out int parsedQuantite))
                                nouvelleQuantite = parsedQuantite;

                            Console.Write($"Nouveau prix ({prixActuel.ToString("F2")} euros) : ");
                            string prixInput = Console.ReadLine();
                            decimal nouveauPrix = prixActuel;
                            if (!string.IsNullOrWhiteSpace(prixInput) && decimal.TryParse(prixInput, out decimal parsedPrix))
                                nouveauPrix = parsedPrix;

                            Console.Write($"Nouveau type de plat ({typePlatActuel}) : ");
                            string nouveauTypePlat = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauTypePlat))
                                nouveauTypePlat = typePlatActuel;

                            string updateQuery = "UPDATE COMMANDE SET nom = @Nom, quantite = @Quantite, prix = @Prix, type_plat = @TypePlat " +
                                                "WHERE ID_commande = @IdCommande";

                            MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
                            updateCmd.Parameters.AddWithValue("@IdCommande", idCommande);
                            updateCmd.Parameters.AddWithValue("@Nom", nouveauNom);
                            updateCmd.Parameters.AddWithValue("@Quantite", nouvelleQuantite);
                            updateCmd.Parameters.AddWithValue("@Prix", nouveauPrix);
                            updateCmd.Parameters.AddWithValue("@TypePlat", nouveauTypePlat);

                            int result = updateCmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                Console.WriteLine("Commande modifiée avec succès !");
                            }
                            else
                            {
                                Console.WriteLine("Échec de la modification de la commande.");
                            }
                        }
                        else
                        {
                            reader.Close();
                            Console.WriteLine($"Aucune commande trouvée avec l'ID {idCommande}.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }
        }

        /// <summary>
        /// Permet d'annuler une commande spécifique après confirmation.
        /// Supprime les entrées associées dans les tables PASSE, PRÉPARE, RÉCEPTIONNE et COMMANDE.
        /// </summary>
        static void AnnulerCommande(string idCommande)
        {
            Console.Clear();
            AfficherTitre("ANNULER LA COMMANDE");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Récupérer les informations de la commande
                    string query = "SELECT c.nom, cl.nom AS nom_client, cl.prénom AS prenom_client " +
                                  "FROM COMMANDE c " +
                                  "JOIN PASSE p ON c.ID_commande = p.ID_commande " +
                                  "JOIN CLIENT cl ON p.id_client = cl.id_client " +
                                  "WHERE c.ID_commande = @IdCommande";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdCommande", idCommande);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nomPlat = reader.IsDBNull(reader.GetOrdinal("nom")) ? "N/A" : reader["nom"].ToString();
                            string nomClient = reader.IsDBNull(reader.GetOrdinal("nom_client")) ? "N/A" : reader["nom_client"].ToString();
                            string prenomClient = reader.IsDBNull(reader.GetOrdinal("prenom_client")) ? "" : reader["prenom_client"].ToString();
                            string client = $"{nomClient} {prenomClient}";

                            reader.Close();

                            Console.WriteLine($"Vous êtes sur le point d'annuler la commande suivante :");
                            Console.WriteLine($"Plat : {nomPlat}");
                            Console.WriteLine($"Client : {client}");
                            Console.Write("\nÊtes-vous sûr de vouloir annuler cette commande ? (O/N) : ");

                            if (Console.ReadLine().ToUpper() == "O")
                            {
                                // Supprimer les relations dans les tables associées
                                string deletePasse = "DELETE FROM PASSE WHERE ID_commande = @IdCommande";
                                MySqlCommand cmdDeletePasse = new MySqlCommand(deletePasse, connection);
                                cmdDeletePasse.Parameters.AddWithValue("@IdCommande", idCommande);
                                cmdDeletePasse.ExecuteNonQuery();

                                string deletePrepare = "DELETE FROM PRÉPARE WHERE ID_commande = @IdCommande";
                                MySqlCommand cmdDeletePrepare = new MySqlCommand(deletePrepare, connection);
                                cmdDeletePrepare.Parameters.AddWithValue("@IdCommande", idCommande);
                                cmdDeletePrepare.ExecuteNonQuery();
                                
                                string deleteReceptionne = "DELETE FROM RÉCEPTIONNE WHERE ID_commande = @IdCommande";
                                MySqlCommand cmdDeleteReceptionne = new MySqlCommand(deleteReceptionne, connection);
                                cmdDeleteReceptionne.Parameters.AddWithValue("@IdCommande", idCommande);
                                cmdDeleteReceptionne.ExecuteNonQuery();

                                string deleteCommande = "DELETE FROM COMMANDE WHERE ID_commande = @IdCommande";
                                MySqlCommand cmdDeleteCommande = new MySqlCommand(deleteCommande, connection);
                                cmdDeleteCommande.Parameters.AddWithValue("@IdCommande", idCommande);
                                int result = cmdDeleteCommande.ExecuteNonQuery();

                                if (result > 0)
                                {
                                    Console.WriteLine("Commande annulée avec succès !");
                                }
                                else
                                {
                                    Console.WriteLine("Échec de l'annulation de la commande.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Annulation abandonnée.");
                            }
                        }
                        else
                        {
                            reader.Close();
                            Console.WriteLine($"Aucune commande trouvée avec l'ID {idCommande}.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }
        }

        /// <summary>
        /// Permet à un cuisinier de marquer une commande comme préparée, en vérifiant qu'il est bien assigné à cette commande
        /// ou qu'elle appartient à l'un de ses restaurants.
        /// </summary>
        static void MarquerCommandePreparee()
        {
            Console.Clear();
            AfficherTitre("MARQUER UNE COMMANDE COMME PRÉPARÉE");

            Console.Write("ID de la commande : ");
            string idCommande = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Vérifier d'abord si le cuisinier est assigné à cette commande
                    string checkAssignmentQuery = @"
                SELECT pr.id_cuisinier 
                FROM PRÉPARE pr 
                WHERE pr.ID_commande = @IdCommande 
                AND pr.id_cuisinier = @CuisinierId";

                    MySqlCommand checkAssignmentCmd = new MySqlCommand(checkAssignmentQuery, connection);
                    checkAssignmentCmd.Parameters.AddWithValue("@IdCommande", idCommande);
                    checkAssignmentCmd.Parameters.AddWithValue("@CuisinierId", utilisateurConnecteId);

                    object assignedCook = checkAssignmentCmd.ExecuteScalar();

                    if (assignedCook == null)
                    {
                        // Si non assigné, vérifier si la commande appartient à l'un des restaurants du cuisinier
                        string checkRestaurantQuery = @"
                    SELECT re.id_resto
                    FROM COMMANDE c
                    JOIN RÉCEPTIONNE re ON c.ID_commande = re.ID_commande
                    JOIN EMPLOIE e ON re.id_resto = e.id_resto
                    WHERE c.ID_commande = @IdCommande
                    AND e.id_cuisinier = @CuisinierId";

                        MySqlCommand checkRestaurantCmd = new MySqlCommand(checkRestaurantQuery, connection);
                        checkRestaurantCmd.Parameters.AddWithValue("@IdCommande", idCommande);
                        checkRestaurantCmd.Parameters.AddWithValue("@CuisinierId", utilisateurConnecteId);

                        object orderRestaurant = checkRestaurantCmd.ExecuteScalar();

                        if (orderRestaurant == null)
                        {
                            Console.WriteLine("Vous ne pouvez pas marquer cette commande comme préparée car elle n'appartient pas à l'un de vos restaurants.");
                            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                            Console.ReadKey();
                            return;
                        }

                        // Si la commande appartient à l'un des restaurants du cuisinier mais n'est pas assignée,
                        // vérifier si elle est déjà assignée à un autre cuisinier
                        string checkOtherAssignmentQuery = @"
                    SELECT pr.id_cuisinier 
                    FROM PRÉPARE pr 
                    WHERE pr.ID_commande = @IdCommande";

                        MySqlCommand checkOtherAssignmentCmd = new MySqlCommand(checkOtherAssignmentQuery, connection);
                        checkOtherAssignmentCmd.Parameters.AddWithValue("@IdCommande", idCommande);

                        object otherCook = checkOtherAssignmentCmd.ExecuteScalar();

                        if (otherCook != null)
                        {
                            Console.WriteLine("Cette commande est déjà prise en charge par un autre cuisinier.");
                            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                            Console.ReadKey();
                            return;
                        }

                        // Si non assignée à un cuisinier, l'assigner à ce cuisinier
                        string assignQuery = "INSERT INTO PRÉPARE (id_cuisinier, ID_commande) VALUES (@IdCuisinier, @IdCommande)";
                        MySqlCommand assignCmd = new MySqlCommand(assignQuery, connection);
                        assignCmd.Parameters.AddWithValue("@IdCuisinier", utilisateurConnecteId);
                        assignCmd.Parameters.AddWithValue("@IdCommande", idCommande);
                        assignCmd.ExecuteNonQuery();

                        Console.WriteLine("La commande vous a été assignée.");
                    }

                    // Marquer la commande comme préparée
                    string updateQuery = "UPDATE COMMANDE SET date_de_fabrication = NOW() WHERE ID_commande = @IdCommande";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
                    updateCmd.Parameters.AddWithValue("@IdCommande", idCommande);

                    int result = updateCmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine("Commande marquée comme préparée avec succès !");
                    }
                    else
                    {
                        Console.WriteLine("Échec de la mise à jour de la commande.");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        /// <summary>
        /// Gère le menu interactif de gestion des ingrédients. Permet d'afficher, ajouter, 
        /// modifier, supprimer un ingrédient ou de retourner au menu principal.
        /// </summary>
        static void GererIngredients()
        {
            bool retourMenu = false;
            while (!retourMenu)
            {
                Console.Clear();
                AfficherTitre("GESTION DES INGRÉDIENTS");
                Console.WriteLine("1. Afficher la liste des ingrédients");
                Console.WriteLine("2. Ajouter un ingrédient");
                Console.WriteLine("3. Modifier un ingrédient");
                Console.WriteLine("4. Supprimer un ingrédient");
                Console.WriteLine("5. Retour au menu principal");
                Console.Write("\nVotre choix : ");

                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        AfficherListeIngredients();
                        break;
                    case "2":
                        AjouterIngredient();
                        break;
                    case "3":
                        ModifierIngredient();
                        break;
                    case "4":
                        SupprimerIngredient();
                        break;
                    case "5":
                        retourMenu = true;
                        break;
                    default:
                        Console.WriteLine("Choix invalide. Appuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        /// <summary>
        /// Affiche la liste complète des ingrédients présents dans la base de données. Les informations affichées incluent 
        /// l'ID, le nom, le régime, l'origine, le type, la présence d'allergènes, la quantité, le stock et la date de péremption.
        /// </summary>
        static void AfficherListeIngredients()
        {
            Console.Clear();
            AfficherTitre("LISTE DES INGRÉDIENTS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT id_ingredient, nom_ingredient, régime_ingredient, origine_ingredient, " +
                                  "type_ingredient, allergene_ingredient, quantité_ingredient, stock_ingredient, date_peremption " +
                                  "FROM INGREDIENT ORDER BY nom_ingredient";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("ID\tNom\t\tRégime\t\tOrigine\t\tType\t\tAllergène\tQuantité\tStock\t\tDate de péremption");
                            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------------------");

                            while (reader.Read())
                            {
                                string id = reader["id_ingredient"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom_ingredient")) ? "N/A" : reader["nom_ingredient"].ToString();
                                string regime = reader.IsDBNull(reader.GetOrdinal("régime_ingredient")) ? "N/A" : reader["régime_ingredient"].ToString();
                                string origine = reader.IsDBNull(reader.GetOrdinal("origine_ingredient")) ? "N/A" : reader["origine_ingredient"].ToString();
                                string type = reader.IsDBNull(reader.GetOrdinal("type_ingredient")) ? "N/A" : reader["type_ingredient"].ToString();

                                string allergene = "Non";
                                if (!reader.IsDBNull(reader.GetOrdinal("allergene_ingredient")))
                                {
                                    allergene = Convert.ToBoolean(reader["allergene_ingredient"]) ? "Oui" : "Non";
                                }

                                string quantite = reader.IsDBNull(reader.GetOrdinal("quantité_ingredient")) ? "N/A" : reader["quantité_ingredient"].ToString();
                                string stock = reader.IsDBNull(reader.GetOrdinal("stock_ingredient")) ? "N/A" : reader["stock_ingredient"].ToString();
                                string datePeremption = reader.IsDBNull(reader.GetOrdinal("date_peremption")) ? "N/A" : reader["date_peremption"].ToString();

                                Console.WriteLine($"{id}\t{nom,-15}{regime,-15}{origine,-15}{type,-15}{allergene,-15}{quantite,-15}{stock,-15}{datePeremption}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun ingrédient trouvé.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        /// <summary>
        /// Permet d'ajouter un nouvel ingrédient dans la base de données. Le cuisinier entre les informations 
        /// suivantes : ID, nom, régime alimentaire, origine, type d'ingrédient, allergène, quantité, stock et date de péremption.
        /// </summary>
        static void AjouterIngredient()
        {
            Console.Clear();
            AfficherTitre("AJOUTER UN INGRÉDIENT");

            Console.Write("ID de l'ingrédient : ");
            string id = Console.ReadLine();

            Console.Write("Nom de l'ingrédient : ");
            string nom = Console.ReadLine();

            Console.Write("Régime alimentaire (végétarien, végan, etc.) : ");
            string regime = Console.ReadLine();

            Console.Write("Origine de l'ingrédient : ");
            string origine = Console.ReadLine();

            Console.Write("Type d'ingrédient (légume, viande, etc.) : ");
            string type = Console.ReadLine();

            Console.Write("Est-ce un allergène ? (O/N) : ");
            bool allergene = Console.ReadLine().ToUpper() == "O";

            Console.Write("Quantité : ");
            int quantite;
            if (!int.TryParse(Console.ReadLine(), out quantite))
            {
                quantite = 0;
            }

            Console.Write("Stock (disponible, épuisé, etc.) : ");
            string stock = Console.ReadLine();

            Console.Write("Date de péremption (YYYY-MM-DD) : ");
            string datePeremption = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO INGREDIENT (id_ingredient, nom_ingredient, régime_ingredient, origine_ingredient, " +
                                  "type_ingredient, allergene_ingredient, quantité_ingredient, stock_ingredient, date_peremption) " +
                                  "VALUES (@Id, @Nom, @Regime, @Origine, @Type, @Allergene, @Quantite, @Stock, @DatePeremption)";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nom", nom);
                    cmd.Parameters.AddWithValue("@Regime", regime);
                    cmd.Parameters.AddWithValue("@Origine", origine);
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@Allergene", allergene);
                    cmd.Parameters.AddWithValue("@Quantite", quantite);
                    cmd.Parameters.AddWithValue("@Stock", stock);
                    cmd.Parameters.AddWithValue("@DatePeremption", datePeremption);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine("Ingrédient ajouté avec succès !");
                    }
                    else
                    {
                        Console.WriteLine("Échec de l'ajout de l'ingrédient.");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        /// <summary>
        /// Permet de modifier un ingrédient existant dans la base de données. Le cuisinier entre l'ID de l'ingrédient 
        /// qu'il souhaite modifier, puis il peut modifier les informations suivantes : nom, régime alimentaire, origine, 
        /// type, allergène, quantité, stock et date de péremption.
        /// </summary>
        static void ModifierIngredient()
        {
            Console.Clear();
            AfficherTitre("MODIFIER UN INGRÉDIENT");

            Console.Write("ID de l'ingrédient à modifier : ");
            string id = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM INGREDIENT WHERE id_ingredient = @Id";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nomActuel = reader.IsDBNull(reader.GetOrdinal("nom_ingredient")) ? "" : reader["nom_ingredient"].ToString();
                            string regimeActuel = reader.IsDBNull(reader.GetOrdinal("régime_ingredient")) ? "" : reader["régime_ingredient"].ToString();
                            string origineActuelle = reader.IsDBNull(reader.GetOrdinal("origine_ingredient")) ? "" : reader["origine_ingredient"].ToString();
                            string typeActuel = reader.IsDBNull(reader.GetOrdinal("type_ingredient")) ? "" : reader["type_ingredient"].ToString();
                            bool allergeneActuel = !reader.IsDBNull(reader.GetOrdinal("allergene_ingredient")) && Convert.ToBoolean(reader["allergene_ingredient"]);
                            int quantiteActuelle = reader.IsDBNull(reader.GetOrdinal("quantité_ingredient")) ? 0 : Convert.ToInt32(reader["quantité_ingredient"]);
                            string stockActuel = reader.IsDBNull(reader.GetOrdinal("stock_ingredient")) ? "" : reader["stock_ingredient"].ToString();
                            string datePeremptionActuelle = reader.IsDBNull(reader.GetOrdinal("date_peremption")) ? "" : reader["date_peremption"].ToString();

                            reader.Close();

                            Console.WriteLine($"Modification de l'ingrédient : {nomActuel}");

                            Console.Write($"Nouveau nom ({nomActuel}) : ");
                            string nouveauNom = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauNom))
                                nouveauNom = nomActuel;

                            Console.Write($"Nouveau régime ({regimeActuel}) : ");
                            string nouveauRegime = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauRegime))
                                nouveauRegime = regimeActuel;

                            Console.Write($"Nouvelle origine ({origineActuelle}) : ");
                            string nouvelleOrigine = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouvelleOrigine))
                                nouvelleOrigine = origineActuelle;

                            Console.Write($"Nouveau type ({typeActuel}) : ");
                            string nouveauType = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauType))
                                nouveauType = typeActuel;

                            Console.Write($"Est-ce un allergène ? (O/N) ({(allergeneActuel ? "O" : "N")}) : ");
                            string allergeneInput = Console.ReadLine().ToUpper();
                            bool nouvelAllergene = allergeneActuel;
                            if (allergeneInput == "O" || allergeneInput == "N")
                                nouvelAllergene = allergeneInput == "O";

                            Console.Write($"Nouvelle quantité ({quantiteActuelle}) : ");
                            string quantiteInput = Console.ReadLine();
                            int nouvelleQuantite = quantiteActuelle;
                            if (!string.IsNullOrWhiteSpace(quantiteInput) && int.TryParse(quantiteInput, out int parsedQuantite))
                                nouvelleQuantite = parsedQuantite;

                            Console.Write($"Nouveau stock ({stockActuel}) : ");
                            string nouveauStock = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauStock))
                                nouveauStock = stockActuel;

                            Console.Write($"Nouvelle date de péremption ({datePeremptionActuelle}) : ");
                            string nouvelleDatePeremption = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouvelleDatePeremption))
                                nouvelleDatePeremption = datePeremptionActuelle;

                            string updateQuery = "UPDATE INGREDIENT SET nom_ingredient = @Nom, régime_ingredient = @Regime, " +
                                                "origine_ingredient = @Origine, type_ingredient = @Type, allergene_ingredient = @Allergene, " +
                                                "quantité_ingredient = @Quantite, stock_ingredient = @Stock, date_peremption = @DatePeremption " +
                                                "WHERE id_ingredient = @Id";

                            MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
                            updateCmd.Parameters.AddWithValue("@Id", id);
                            updateCmd.Parameters.AddWithValue("@Nom", nouveauNom);
                            updateCmd.Parameters.AddWithValue("@Regime", nouveauRegime);
                            updateCmd.Parameters.AddWithValue("@Origine", nouvelleOrigine);
                            updateCmd.Parameters.AddWithValue("@Type", nouveauType);
                            updateCmd.Parameters.AddWithValue("@Allergene", nouvelAllergene);
                            updateCmd.Parameters.AddWithValue("@Quantite", nouvelleQuantite);
                            updateCmd.Parameters.AddWithValue("@Stock", nouveauStock);
                            updateCmd.Parameters.AddWithValue("@DatePeremption", nouvelleDatePeremption);

                            int result = updateCmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                Console.WriteLine("Ingrédient modifié avec succès !");
                            }
                            else
                            {
                                Console.WriteLine("Échec de la modification de l'ingrédient.");
                            }
                        }
                        else
                        {
                            reader.Close();
                            Console.WriteLine($"Aucun ingrédient trouvé avec l'ID {id}.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        /// <summary>
        /// Permet de supprimer un ingrédient de la base de données. Le cuisinier entre l'ID de l'ingrédient qu'il souhaite supprimer, 
        /// et il doit confirmer la suppression. Si l'ingrédient existe, il sera supprimé de la base de données.
        /// </summary>
        static void SupprimerIngredient()
        {
            Console.Clear();
            AfficherTitre("SUPPRIMER UN INGRÉDIENT");

            Console.Write("ID de l'ingrédient à supprimer : ");
            string id = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Vérifier si l'ingrédient existe
                    string checkQuery = "SELECT nom_ingredient FROM INGREDIENT WHERE id_ingredient = @Id";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@Id", id);

                    object result = checkCmd.ExecuteScalar();

                    if (result != null)
                    {
                        string nom = result.ToString();

                        Console.Write($"Êtes-vous sûr de vouloir supprimer l'ingrédient '{nom}' ? (O/N) : ");
                        if (Console.ReadLine().ToUpper() == "O")
                        {
                            string deleteQuery = "DELETE FROM INGREDIENT WHERE id_ingredient = @Id";
                            MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, connection);
                            deleteCmd.Parameters.AddWithValue("@Id", id);

                            int deleteResult = deleteCmd.ExecuteNonQuery();

                            if (deleteResult > 0)
                            {
                                Console.WriteLine("Ingrédient supprimé avec succès !");
                            }
                            else
                            {
                                Console.WriteLine("Échec de la suppression de l'ingrédient.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Suppression annulée.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Aucun ingrédient trouvé avec l'ID {id}.");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
        #endregion

        #region Méthodes pour le menu livreur
        /// <summary>
        /// Affiche les commandes préparées et prêtes à être livrées pour un livreur spécifique.
        /// </summary>
        /// <param name="livreurId">ID du livreur</param>
        static void VoirCommandesALivrer(string livreurId)
        {
            Console.Clear();
            AfficherTitre("COMMANDES À LIVRER");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete = "SELECT c.ID_commande, c.nom, c.quantite, c.type_plat, " +
                                "cl.nom AS nom_client, cl.prénom AS prenom_client, " +
                                "cl.code_postal, cl.ville AS nom_ville, cl.rue AS complément_d_adresse " +
                                "FROM COMMANDE c " +
                                "JOIN PASSE p ON c.ID_commande = p.ID_commande " +
                                "JOIN CLIENT cl ON p.id_client = cl.id_client " +
                                "JOIN LIVRE l ON cl.id_client = l.id_client " +
                                "JOIN RÉCEPTIONNE re ON c.ID_commande = re.ID_commande " +
                                "JOIN EMPLOIE e ON re.id_resto = e.ID_resto " +
                                "WHERE l.ID_livreur = @LivreurId " +
                                "AND e.ID_livreur = @LivreurId " +
                                "AND c.date_de_fabrication IS NOT NULL " +  // S'assurer que la commande est préparée
                                "AND c.date_paiement IS NOT NULL " +        // S'assurer que la commande est payée
                                "AND c.date_livraison IS NULL " +           // S'assurer que la commande n'est pas encore livrée
                                "ORDER BY c.date_de_fabrication ASC";
                    
                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@LivreurId", livreurId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("ID\tPlat\t\tQté\tClient\t\tAdresse");
                            Console.WriteLine("-------------------------------------------------------------------------");

                            while (reader.Read())
                            {
                                string id = reader["ID_commande"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom")) ? "N/A" : reader["nom"].ToString();
                                int quantite = reader.IsDBNull(reader.GetOrdinal("quantite")) ? 0 : Convert.ToInt32(reader["quantite"]);

                                string nomClient = reader.IsDBNull(reader.GetOrdinal("nom_client")) ? "N/A" : reader["nom_client"].ToString();
                                string prenomClient = reader.IsDBNull(reader.GetOrdinal("prenom_client")) ? "" : reader["prenom_client"].ToString();
                                string client = $"{nomClient} {prenomClient}";

                                string codePostal = reader.IsDBNull(reader.GetOrdinal("code_postal")) ? "" : reader["code_postal"].ToString();
                                string ville = reader.IsDBNull(reader.GetOrdinal("nom_ville")) ? "" : reader["nom_ville"].ToString();
                                string complement = reader.IsDBNull(reader.GetOrdinal("complément_d_adresse")) ? "" : reader["complément_d_adresse"].ToString();
                                string adresse = $"{complement}, {codePostal} {ville}";

                                Console.WriteLine($"{id}\t{nom,-15}{quantite}\t{client,-20}{adresse}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucune commande prête et payée à livrer.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }


        /// <summary>
        /// Affiche l'historique des itinéraires d'un livreur en récupérant les trajets associés depuis la base de données.
        /// Montre les informations principales (date, départ, arrivée, distance, durée) ou un message s’il n’y a aucun trajet.
        /// </summary>
        static void ConsulterItineraire(string livreurId)
        {
            Console.Clear();
            AfficherTitre("CONSULTER MON ITINÉRAIRE");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete = "SELECT t.id_trajet, t.duree_trajet, t.date_trajet, t.point_départ_trajet, " +
                                    "t.distance_trajet, t.point_arrivee_trajet " +
                                    "FROM TRAJET t " +
                                    "JOIN PARCOURT p ON t.id_trajet = p.id_trajet " +
                                    "WHERE p.ID_livreur = @LivreurId " +
                                    "ORDER BY t.date_trajet DESC";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@LivreurId", livreurId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("ID\tDate\t\tDépart\t\tArrivée\t\tDistance\tDurée");
                            Console.WriteLine("-------------------------------------------------------------------------");

                            while (reader.Read())
                            {
                                string id = reader["id_trajet"].ToString();
                                string dateTrajet = reader.IsDBNull(reader.GetOrdinal("date_trajet")) ? "N/A" : reader["date_trajet"].ToString();
                                string depart = reader.IsDBNull(reader.GetOrdinal("point_départ_trajet")) ? "N/A" : reader["point_départ_trajet"].ToString();
                                string arrivee = reader.IsDBNull(reader.GetOrdinal("point_arrivee_trajet")) ? "N/A" : reader["point_arrivee_trajet"].ToString();
                                string distance = reader.IsDBNull(reader.GetOrdinal("distance_trajet")) ? "N/A" : reader["distance_trajet"].ToString();
                                string duree = reader.IsDBNull(reader.GetOrdinal("duree_trajet")) ? "N/A" : reader["duree_trajet"].ToString();

                                Console.WriteLine($"{id}\t{dateTrajet}\t{depart,-15}{arrivee,-15}{distance}\t{duree}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun itinéraire trouvé.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        /// <summary>
        /// Permet à un livreur de marquer une commande comme livrée, en vérifiant d'abord que la commande a été préparée.
        /// </summary>
        static void MarquerCommandeLivree()
        {
            Console.Clear();
            AfficherTitre("MARQUER UNE COMMANDE COMME LIVRÉE");

            Console.Write("ID de la commande : ");
            string idCommande = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Vérifier si la commande existe et a été préparée
                    string checkQuery = "SELECT date_de_fabrication FROM COMMANDE WHERE ID_commande = @IdCommande";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@IdCommande", idCommande);
                    object datePreparation = checkCmd.ExecuteScalar();

                    if (datePreparation == null || Convert.IsDBNull(datePreparation))
                    {
                        AfficherMessageErreur("Cette commande n'existe pas ou n'a pas encore été préparée.");
                        Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                        Console.ReadKey();
                        return;
                    }

                    // Vérifier que le livreur est bien assigné à cette commande
                    string checkLivreurQuery = @"
                SELECT e.ID_livreur 
                FROM COMMANDE c
                JOIN RÉCEPTIONNE r ON c.ID_commande = r.ID_commande
                JOIN EMPLOIE e ON r.id_resto = e.ID_resto
                WHERE c.ID_commande = @IdCommande 
                AND e.ID_livreur = @LivreurId";

                    MySqlCommand checkLivreurCmd = new MySqlCommand(checkLivreurQuery, connection);
                    checkLivreurCmd.Parameters.AddWithValue("@IdCommande", idCommande);
                    checkLivreurCmd.Parameters.AddWithValue("@LivreurId", utilisateurConnecteId);

                    object assignedLivreur = checkLivreurCmd.ExecuteScalar();

                    if (assignedLivreur == null)
                    {
                        AfficherMessageErreur("Vous n'êtes pas assigné à cette commande.");
                        Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                        Console.ReadKey();
                        return;
                    }

                    // Marquer la commande comme livrée
                    string query = "UPDATE COMMANDE SET date_livraison = NOW() WHERE ID_commande = @IdCommande";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdCommande", idCommande);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine("Commande marquée comme livrée avec succès !");
                    }
                    else
                    {
                        Console.WriteLine("Échec de la mise à jour de la livraison.");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        /// <summary>
        /// Affiche les évaluations d'un livreur en récupérant sa note moyenne depuis la BDD sur MySQL.
        /// En cas d'erreur ou d'absence d'évaluation, un message est affiché, et l'utilisateur peut revenir au menu.
        /// </summary>
        static void VoirEvaluations(string livreurId)
        {
            Console.Clear();
            AfficherTitre("MES ÉVALUATIONS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT note_livreur FROM LIVREUR WHERE ID_livreur = @LivreurId";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@LivreurId", livreurId);

                    object result = cmd.ExecuteScalar();

                    if (result != null && !Convert.IsDBNull(result))
                    {
                        decimal note = Convert.ToDecimal(result);
                        Console.WriteLine($"Votre note moyenne : {note:F1}/5");
                    }
                    else
                    {
                        Console.WriteLine("Aucune évaluation disponible.");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }
        #endregion


        #region Méthodes utiles
        //static List<(string id, string nom)> ObtenirListeRestaurants()
        //{
        //    var restaurants = new List<(string id, string nom)>();

        //    try
        //    {
        //        using (MySqlConnection connection = new MySqlConnection(connectionString))
        //        {
        //            connection.Open();

        //            string query = "SELECT id_resto, nom_resto FROM RESTAURANT";

        //            MySqlCommand cmd = new MySqlCommand(query, connection);

        //            using (MySqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    string id = reader["id_resto"].ToString();
        //                    string nom = reader["nom_resto"].ToString();
        //                    restaurants.Add((id, nom));
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Erreur : {ex.Message}");
        //    }

        //    return restaurants;
        //}

        public static List<(string id, string nom, string type, decimal capacite)> ObtenirListeRestaurantsDetails()
        {
            var restaurants = new List<(string id, string nom, string type, decimal capacite)>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT id_resto, nom_resto, type_resto, capacité_resto FROM RESTAURANT";

                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader["id_resto"].ToString();
                            string nom = reader["nom_resto"].ToString();
                            string type = reader.IsDBNull(reader.GetOrdinal("type_resto")) ? "N/A" : reader["type_resto"].ToString();
                            decimal capacite = 0;
                            if (!reader.IsDBNull(reader.GetOrdinal("capacité_resto")))
                            {
                                capacite = Convert.ToDecimal(reader["capacité_resto"]);
                            }
                            restaurants.Add((id, nom, type, capacite));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }

            return restaurants;
        }

        static List<(string id, string entree, string plat, string dessert, string boisson, string regime, decimal prix)> ObtenirMenusRestaurant(string idResto)
        {
            var menus = new List<(string id, string entree, string plat, string dessert, string boisson, string regime, decimal prix)>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Vérifier si la table MENU contient une colonne id_resto
                    string checkColumn = "SHOW COLUMNS FROM MENU LIKE 'id_resto'";
                    MySqlCommand checkCmd = new MySqlCommand(checkColumn, connection);
                    bool hasIdResto = checkCmd.ExecuteScalar() != null;

                    string query;
                    if (hasIdResto)
                    {
                        // Si MENU contient id_resto, utiliser cette relation directe
                        query = "SELECT id_menu, entrée, plat, dessert, boisson, régime_correspondant FROM MENU WHERE id_resto = @IdResto";
                    }
                    else
                    {
                        // Utiliser une requête simple pour MENU
                        query = "SELECT id_menu, entrée, plat, dessert, boisson, régime_correspondant FROM MENU";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    if (query.Contains("@IdResto"))
                    {
                        cmd.Parameters.AddWithValue("@IdResto", idResto);
                    }

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader["id_menu"].ToString();
                            string entree = reader.IsDBNull(reader.GetOrdinal("entrée")) ? "N/A" : reader["entrée"].ToString();
                            string plat = reader.IsDBNull(reader.GetOrdinal("plat")) ? "N/A" : reader["plat"].ToString();
                            string dessert = reader.IsDBNull(reader.GetOrdinal("dessert")) ? "N/A" : reader["dessert"].ToString();
                            string boisson = reader.IsDBNull(reader.GetOrdinal("boisson")) ? "N/A" : reader["boisson"].ToString();
                            string regime = reader.IsDBNull(reader.GetOrdinal("régime_correspondant")) ? "Standard" : reader["régime_correspondant"].ToString();

                            // Prix calculé en fonction des éléments du menu
                            decimal prix = 0;
                            if (entree != "N/A") prix += 5.0m;
                            if (plat != "N/A") prix += 12.0m;
                            if (dessert != "N/A") prix += 4.0m;
                            if (boisson != "N/A") prix += 2.5m;

                            menus.Add((id, entree, plat, dessert, boisson, regime, prix));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }

            return menus;
        }

        static void AjouterPlatCommande(int idCommande)
        {
            Console.WriteLine("\n--- AJOUTER UN PLAT ---");

            Console.Write("Nom du plat : ");
            string nomPlat = Console.ReadLine();

            Console.Write("Prix : ");
            decimal prix;
            if (!decimal.TryParse(Console.ReadLine(), out prix))
            {
                AfficherMessageErreur("Prix invalide.");
                return;
            }

            Console.Write("Quantité : ");
            int quantite;
            if (!int.TryParse(Console.ReadLine(), out quantite))
            {
                AfficherMessageErreur("Quantité invalide.");
                return;
            }

            Console.Write("Type de plat : ");
            string typePlat = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE COMMANDE SET " +
                                    "nom = @Nom, " +
                                    "prix = @Prix, " +
                                    "quantite = @Quantite, " +
                                    "type_plat = @TypePlat " +
                                    "WHERE ID_commande = @IdCommande";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Nom", nomPlat);
                    cmd.Parameters.AddWithValue("@Prix", prix);
                    cmd.Parameters.AddWithValue("@Quantite", quantite);
                    cmd.Parameters.AddWithValue("@TypePlat", typePlat);
                    cmd.Parameters.AddWithValue("@IdCommande", idCommande);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine("Plat ajouté à la commande avec succès !");
                    }
                    else
                    {
                        Console.WriteLine("Échec de l'ajout du plat à la commande.");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }
        }
        #endregion

        #region Méthodes supplémentaires
        
        static void ListerClients(bool pauseAfter = true)
        {
            Console.Clear();
            AfficherTitre("LISTE DES CLIENTS");

            if (pauseAfter) // N'afficher les options que si on est dans le menu complet
            {
                Console.WriteLine("Comment souhaitez-vous trier la liste ?");
                Console.WriteLine("1. Par ID client");
                Console.WriteLine("2. Par ordre alphabétique (nom)");
                Console.WriteLine("3. Par noms de rue");
                Console.WriteLine("4. Par montant des achats cumulés");
                Console.Write("\nVotre choix : ");
                string choixTri = Console.ReadLine();

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        string requete;
                        switch (choixTri)
                        {
                            case "2": // Par ordre alphabétique
                                requete = "SELECT c.id_client, c.nom, c.prénom, c.email, c.tel, c.rue " +
                                        "FROM CLIENT c ORDER BY c.nom, c.prénom";
                                break;
                            case "3": // Par rue
                                requete = "SELECT c.id_client, c.nom, c.prénom, c.email, c.tel, c.rue " +
                                        "FROM CLIENT c ORDER BY c.rue";
                                break;
                            case "4": // Par montant des achats cumulés
                                requete = "SELECT c.id_client, c.nom, c.prénom, c.email, c.tel, c.rue, " +
                                        "SUM(co.prix * co.quantite) as total_achats " +
                                        "FROM CLIENT c " +
                                        "LEFT JOIN PASSE p ON c.id_client = p.id_client " +
                                        "LEFT JOIN COMMANDE co ON p.ID_commande = co.ID_commande " +
                                        "GROUP BY c.id_client, c.nom, c.prénom, c.email, c.tel, c.rue " +
                                        "ORDER BY total_achats DESC";
                                break;
                            default: // Par ID 
                                requete = "SELECT c.id_client, c.nom, c.prénom, c.email, c.tel, c.rue " +
                                        "FROM CLIENT c ORDER BY c.id_client";
                                break;
                        }

                        MySqlCommand cmd = new MySqlCommand(requete, connection);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                if (choixTri == "4")
                                {
                                    Console.WriteLine("ID\tNom\t\tPrénom\t\tEmail\t\t\tTéléphone\tRue\t\tTotal Achats");
                                    Console.WriteLine("-----------------------------------------------------------------------------------------------------");

                                    while (reader.Read())
                                    {
                                        string id = reader["id_client"].ToString();
                                        string nom = reader.IsDBNull(reader.GetOrdinal("nom")) ? "N/A" : reader["nom"].ToString();
                                        string prenom = reader.IsDBNull(reader.GetOrdinal("prénom")) ? "N/A" : reader["prénom"].ToString();
                                        string email = reader.IsDBNull(reader.GetOrdinal("email")) ? "N/A" : reader["email"].ToString();
                                        string tel = reader.IsDBNull(reader.GetOrdinal("tel")) ? "N/A" : reader["tel"].ToString();
                                        string rue = reader.IsDBNull(reader.GetOrdinal("rue")) ? "N/A" : reader["rue"].ToString();

                                        decimal totalAchats = 0;
                                        if (!reader.IsDBNull(reader.GetOrdinal("total_achats")))
                                        {
                                            totalAchats = Convert.ToDecimal(reader["total_achats"]);
                                        }

                                        Console.WriteLine($"{id}\t{nom,-15}{prenom,-15}{email,-25}{tel,-15}{rue,-15}{totalAchats.ToString("F2")} euros");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("ID\tNom\t\tPrénom\t\tEmail\t\t\tTéléphone\tRue");
                                    Console.WriteLine("-----------------------------------------------------------------------------------------------------");

                                    while (reader.Read())
                                    {
                                        string id = reader["id_client"].ToString();
                                        string nom = reader.IsDBNull(reader.GetOrdinal("nom")) ? "N/A" : reader["nom"].ToString();
                                        string prenom = reader.IsDBNull(reader.GetOrdinal("prénom")) ? "N/A" : reader["prénom"].ToString();
                                        string email = reader.IsDBNull(reader.GetOrdinal("email")) ? "N/A" : reader["email"].ToString();
                                        string tel = reader.IsDBNull(reader.GetOrdinal("tel")) ? "N/A" : reader["tel"].ToString();
                                        string rue = reader.IsDBNull(reader.GetOrdinal("rue")) ? "N/A" : reader["rue"].ToString();

                                        Console.WriteLine($"{id}\t{nom,-15}{prenom,-15}{email,-25}{tel,-15}{rue}");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Aucun client trouvé.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    AfficherMessageErreur($"Erreur : {ex.Message}");
                }
            }
            //else
            //{
            //    try
            //    {
            //        using (MySqlConnection connection = new MySqlConnection(connectionString))
            //        {
            //            connection.Open();

            //            string requete = "SELECT id_client, nom, prénom, email, tel FROM CLIENT ORDER BY id_client";
            //            MySqlCommand cmd = new MySqlCommand(requete, connection);

            //            using (MySqlDataReader reader = cmd.ExecuteReader())
            //            {
            //                if (reader.HasRows)
            //                {
            //                    Console.WriteLine("ID\tNom\t\tPrénom\t\tEmail\t\t\tTéléphone");
            //                    Console.WriteLine("-------------------------------------------------------------------------");

            //                    while (reader.Read())
            //                    {
            //                        string id = reader["id_client"].ToString();
            //                        string nom = reader.IsDBNull(reader.GetOrdinal("nom")) ? "N/A" : reader["nom"].ToString();
            //                        string prenom = reader.IsDBNull(reader.GetOrdinal("prénom")) ? "N/A" : reader["prénom"].ToString();
            //                        string email = reader.IsDBNull(reader.GetOrdinal("email")) ? "N/A" : reader["email"].ToString();
            //                        string tel = reader.IsDBNull(reader.GetOrdinal("tel")) ? "N/A" : reader["tel"].ToString();

            //                        Console.WriteLine($"{id}\t{nom,-15}{prenom,-15}{email,-25}{tel}");
            //                    }
            //                }
            //                else
            //                {
            //                    Console.WriteLine("Aucun client trouvé.");
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        AfficherMessageErreur($"Erreur : {ex.Message}");
            //    }
            //}

            if (pauseAfter)
            {
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
        }

        static void ListerCommandes()
        {
            Console.Clear();
            AfficherTitre("LISTE DES COMMANDES");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete = "SELECT c.ID_commande, c.nom, c.prix, c.quantite, c.type_plat, c.date_commande, " +
                                    "cl.nom AS nom_client, cl.prénom AS prenom_client, r.nom_resto " +
                                    "FROM COMMANDE c " +
                                    "JOIN PASSE p ON c.ID_commande = p.ID_commande " +
                                    "JOIN CLIENT cl ON p.id_client = cl.id_client " +
                                    "LEFT JOIN RÉCEPTIONNE re ON c.ID_commande = re.ID_commande " +
                                    "LEFT JOIN RESTAURANT r ON re.id_resto = r.id_resto " +
                                    "ORDER BY c.date_commande DESC";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("ID\tDate\t\tPlat\t\tPrix\tQté\tClient\t\tRestaurant");
                            Console.WriteLine("-------------------------------------------------------------------------");

                            while (reader.Read())
                            {
                                string id = reader["ID_commande"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom")) ? "N/A" : reader["nom"].ToString();
                                string prixStr = reader.IsDBNull(reader.GetOrdinal("prix")) ? "N/A" : reader["prix"].ToString();
                                int quantite = reader.IsDBNull(reader.GetOrdinal("quantite")) ? 0 : Convert.ToInt32(reader["quantite"]);

                                string nomClient = reader.IsDBNull(reader.GetOrdinal("nom_client")) ? "N/A" : reader["nom_client"].ToString();
                                string prenomClient = reader.IsDBNull(reader.GetOrdinal("prenom_client")) ? "" : reader["prenom_client"].ToString();
                                string client = $"{nomClient} {prenomClient}";

                                string restaurant = reader.IsDBNull(reader.GetOrdinal("nom_resto")) ? "N/A" : reader["nom_resto"].ToString();
                                string dateCommande = reader.IsDBNull(reader.GetOrdinal("date_commande")) ?
                                    "N/A" : reader["date_commande"].ToString();

                                Console.WriteLine($"{id}\t{dateCommande}\t{nom,-15}{prixStr}\t{quantite}\t{client,-20}{restaurant}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucune commande trouvée.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        static void AjouterAdresse()
        {
            Console.Clear();
            AfficherTitre("AJOUTER UNE ADRESSE");

            Console.Write("ID de l'adresse : ");
            string idAdresse = Console.ReadLine();

            Console.Write("Code postal : ");
            string codePostal = Console.ReadLine();

            Console.Write("Ville : ");
            string ville = Console.ReadLine();

            Console.Write("Complément d'adresse : ");
            string complement = Console.ReadLine();

            Console.Write("Étage : ");
            string etage = Console.ReadLine();

            Console.Write("Numéro d'appartement : ");
            string numeroAppartement = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string queryAdresse = "INSERT INTO ADRESSE (id_adresse, code_postal, nom_ville, complément_d_adresse, étage, numéro_appartement) " +
                                         "VALUES (@IdAdresse, @CodePostal, @Ville, @Complement, @Etage, @NumeroAppartement)";

                    MySqlCommand cmdAdresse = new MySqlCommand(queryAdresse, connection);
                    cmdAdresse.Parameters.AddWithValue("@IdAdresse", idAdresse);
                    cmdAdresse.Parameters.AddWithValue("@CodePostal", codePostal);
                    cmdAdresse.Parameters.AddWithValue("@Ville", ville);
                    cmdAdresse.Parameters.AddWithValue("@Complement", complement);
                    cmdAdresse.Parameters.AddWithValue("@Etage", etage);
                    cmdAdresse.Parameters.AddWithValue("@NumeroAppartement", numeroAppartement);

                    int resultAdresse = cmdAdresse.ExecuteNonQuery();

                    if (resultAdresse > 0)
                    {
                        string queryTransmet = "INSERT INTO TRANSMET (id_client, id_adresse) VALUES (@IdClient, @IdAdresse)";
                        MySqlCommand cmdTransmet = new MySqlCommand(queryTransmet, connection);
                        cmdTransmet.Parameters.AddWithValue("@IdClient", utilisateurConnecteId);
                        cmdTransmet.Parameters.AddWithValue("@IdAdresse", idAdresse);
                        int resultTransmet = cmdTransmet.ExecuteNonQuery();

                        if (resultTransmet > 0)
                        {
                            Console.WriteLine("Adresse ajoutée et liée au client avec succès !");
                        }
                        else
                        {
                            Console.WriteLine("Échec de la liaison de l'adresse au client.");
                        }    
                    }
                    else
                    {
                        Console.WriteLine("Échec de la création de l'adresse.");
                    }
                }
            }
            catch (Exception ex)
            {
                AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        static bool AssocierClientCommande(string idClient, int idCommande)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO PASSE (id_client, ID_commande) VALUES (@idClient, @idCommande)";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@idClient", idClient);
                    cmd.Parameters.AddWithValue("@idCommande", idCommande);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'association : " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Assigne une commande à un cuisinier travaillant dans le restaurant spécifié.
        /// Utilise la relation EMPLOIE pour trouver les cuisiniers d'un restaurant.
        /// </summary>
        /// <summary>
        /// Assigne une commande à un cuisinier travaillant dans le restaurant spécifié.
        /// Utilise la relation EMPLOIE pour trouver les cuisiniers d'un restaurant.
        /// </summary>
        static void AssignerCommandeACuisinier(int idCommande, string idResto, MySqlConnection connection)
        {
            try
            {
                // Trouver un cuisinier qui travaille dans ce restaurant
                string findCookQuery = "SELECT id_cuisinier FROM EMPLOIE WHERE ID_resto = @IdResto LIMIT 1";
                MySqlCommand findCookCmd = new MySqlCommand(findCookQuery, connection);
                findCookCmd.Parameters.AddWithValue("@IdResto", idResto);
                object cookId = findCookCmd.ExecuteScalar();

                if (cookId != null && !Convert.IsDBNull(cookId))
                {
                    // Assigner la commande à ce cuisinier
                    string assignQuery = "INSERT INTO PRÉPARE (id_cuisinier, ID_commande) VALUES (@IdCuisinier, @IdCommande)";
                    MySqlCommand assignCmd = new MySqlCommand(assignQuery, connection);
                    assignCmd.Parameters.AddWithValue("@IdCuisinier", cookId.ToString());
                    assignCmd.Parameters.AddWithValue("@IdCommande", idCommande);
                    assignCmd.ExecuteNonQuery();
                }
                else
                {
                    // Si aucun cuisinier n'est assigné à ce restaurant, trouver n'importe quel cuisinier
                    string findAnyCookQuery = "SELECT id_cuisinier FROM CUISINIER LIMIT 1";
                    MySqlCommand findAnyCookCmd = new MySqlCommand(findAnyCookQuery, connection);
                    object anyCookId = findAnyCookCmd.ExecuteScalar();

                    if (anyCookId != null && !Convert.IsDBNull(anyCookId))
                    {
                        // Assigner la commande à ce cuisinier
                        string assignQuery = "INSERT INTO PRÉPARE (id_cuisinier, ID_commande) VALUES (@IdCuisinier, @IdCommande)";
                        MySqlCommand assignCmd = new MySqlCommand(assignQuery, connection);
                        assignCmd.Parameters.AddWithValue("@IdCuisinier", anyCookId.ToString());
                        assignCmd.Parameters.AddWithValue("@IdCommande", idCommande);
                        assignCmd.ExecuteNonQuery();

                        // Trouver un livreur disponible
                        string findLivreurQuery = "SELECT ID_livreur FROM LIVREUR WHERE disponibilité = 1 LIMIT 1";
                        MySqlCommand findLivreurCmd = new MySqlCommand(findLivreurQuery, connection);
                        object livreurId = findLivreurCmd.ExecuteScalar();

                        if (livreurId != null && !Convert.IsDBNull(livreurId))
                        {
                            // Assigner également ce cuisinier au restaurant pour les commandes futures
                            string assignRestoQuery = "INSERT INTO EMPLOIE (ID_resto, id_cuisinier, ID_livreur) VALUES (@IdResto, @IdCuisinier, @IdLivreur)";
                            MySqlCommand assignRestoCmd = new MySqlCommand(assignRestoQuery, connection);
                            assignRestoCmd.Parameters.AddWithValue("@IdResto", idResto);
                            assignRestoCmd.Parameters.AddWithValue("@IdCuisinier", anyCookId.ToString());
                            assignRestoCmd.Parameters.AddWithValue("@IdLivreur", livreurId.ToString());
                            assignRestoCmd.ExecuteNonQuery();
                        }
                        else
                        {
                            // Si aucun livreur disponible, utiliser le premier livreur trouvé
                            string findAnyLivreurQuery = "SELECT ID_livreur FROM LIVREUR LIMIT 1";
                            MySqlCommand findAnyLivreurCmd = new MySqlCommand(findAnyLivreurQuery, connection);
                            object anyLivreurId = findAnyLivreurCmd.ExecuteScalar();

                            if (anyLivreurId != null && !Convert.IsDBNull(anyLivreurId))
                            {
                                string assignRestoQuery = "INSERT INTO EMPLOIE (ID_resto, id_cuisinier, ID_livreur) VALUES (@IdResto, @IdCuisinier, @IdLivreur)";
                                MySqlCommand assignRestoCmd = new MySqlCommand(assignRestoQuery, connection);
                                assignRestoCmd.Parameters.AddWithValue("@IdResto", idResto);
                                assignRestoCmd.Parameters.AddWithValue("@IdCuisinier", anyCookId.ToString());
                                assignRestoCmd.Parameters.AddWithValue("@IdLivreur", anyLivreurId.ToString());
                                assignRestoCmd.ExecuteNonQuery();
                            }
                            else
                            {
                                Console.WriteLine("Aucun livreur trouvé dans la base de données. Impossible d'assigner le cuisinier au restaurant.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'assignation de la commande : {ex.Message}");
            }
        }

        /// <summary>
        /// Crée une nouvelle commande dans la base de données et l'associe à un restaurant.
        /// </summary>
        static int CreerNouvelleCommande(string idClient, string idResto, string nomPlat, decimal prix, int quantite, string typePlat)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Obtenir la date actuelle
                    string dateCommande = DateTime.Now.ToString("yyyy-MM-dd");

                    // Générer un nouvel ID de commande
                    int newIdCommande = 0;
                    string getMaxIdQuery = "SELECT MAX(ID_commande) FROM COMMANDE";
                    MySqlCommand getMaxIdCmd = new MySqlCommand(getMaxIdQuery, connection);
                    object maxId = getMaxIdCmd.ExecuteScalar();

                    if (maxId != null && !Convert.IsDBNull(maxId))
                    {
                        newIdCommande = Convert.ToInt32(maxId) + 1;
                    }
                    else
                    {
                        newIdCommande = 1;
                    }

                    string prixFormatted = prix.ToString("F2") + " euros";

                    string query = "INSERT INTO COMMANDE (ID_commande, date_commande, nom, prix, quantite, type_plat) " +
                                  "VALUES (@IdCommande, @DateCommande, @Nom, @Prix, @Quantite, @TypePlat)";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdCommande", newIdCommande);
                    cmd.Parameters.AddWithValue("@DateCommande", dateCommande);
                    cmd.Parameters.AddWithValue("@Nom", nomPlat);
                    cmd.Parameters.AddWithValue("@Prix", prixFormatted); 
                    cmd.Parameters.AddWithValue("@Quantite", quantite);
                    cmd.Parameters.AddWithValue("@TypePlat", typePlat);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        string queryReceptionne = "INSERT INTO RÉCEPTIONNE (id_resto, ID_commande) VALUES (@IdResto, @IdCommande)";
                        MySqlCommand cmdReceptionne = new MySqlCommand(queryReceptionne, connection);
                        cmdReceptionne.Parameters.AddWithValue("@IdResto", idResto);
                        cmdReceptionne.Parameters.AddWithValue("@IdCommande", newIdCommande);
                        cmdReceptionne.ExecuteNonQuery();

                        // Associer la commande à un cuisinier du restaurant
                        AssignerCommandeACuisinier(newIdCommande, idResto, connection);

                        // Associer la commande au client
                        string queryPasse = "INSERT INTO PASSE (id_client, ID_commande) VALUES (@IdClient, @IdCommande)";
                        MySqlCommand cmdPasse = new MySqlCommand(queryPasse, connection);
                        cmdPasse.Parameters.AddWithValue("@IdClient", idClient);
                        cmdPasse.Parameters.AddWithValue("@IdCommande", newIdCommande);
                        cmdPasse.ExecuteNonQuery();

                        string updatePayment = "UPDATE COMMANDE SET date_paiement = NOW() WHERE ID_commande = @IdCommande";
                        MySqlCommand updateCmd = new MySqlCommand(updatePayment, connection);
                        updateCmd.Parameters.AddWithValue("@IdCommande", newIdCommande);
                        updateCmd.ExecuteNonQuery();

                        return newIdCommande;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }

            return 0;
        }
        #endregion

        #region Méthodes d'affichage

        /// <summary>
        /// Affiche un titre encadré avec une couleur bleue pour le mettre en valeur (grâce à System.ConsoleColor), suivi d'une ligne de séparation adaptée à sa longueur.
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
        #endregion 
    }
}
