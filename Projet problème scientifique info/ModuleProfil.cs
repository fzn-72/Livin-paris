using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_problème_scientifique_info
{
    public class ModuleProfil
    {
        private static string utilisateurConnecteId = "";
        private static string utilisateurConnecteNom = "";
        private static string utilisateurConnectePrenom = "";
        private static string utilisateurConnecteType = "";

        private static string connectionString = "Server=localhost;Database=livinparis;Uid=root;Pwd=REDACTED;";

        #region Menu principal de la classe Moduleprofil

        /// <summary>
        /// Gère le module profil : permet à l'utilisateur de se connecter, créer un compte ou quitter le module (le redirige vers les méthodes correspondantes).
        /// Affiche le sous-menu en boucle jusqu'à ce que l'utilisateur choisisse de quitter.
        /// </summary>
        public static void MenuPrincipalProfil()
        {
            bool continuer = true;
            while (continuer)
            {
                Console.WriteLine("1. Se connecter");
                Console.WriteLine("2. Créer un compte");
                Console.WriteLine("3. Quitter");
                Console.Write("\nChoisissez une option : ");
                string choix2 = Console.ReadLine();

                switch (choix2)
                {
                    case "1":
                        SeConnecter();
                        break;
                    case "2":
                        CreerCompte();
                        break;
                    case "3":
                        continuer = false;
                        break;
                    default:
                        GestionBDD.AfficherMessageErreur("Option invalide.");
                        break;
                }
            }
        }
        #endregion


        #region Méthode de connexion
        static void SeConnecter()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("CONNEXION");

            Console.WriteLine("1. Client");
            Console.WriteLine("2. Cuisinier");
            Console.WriteLine("3. Livreur");
            Console.WriteLine("4. Administrateur");
            Console.Write("\nChoisissez votre type de compte : ");
            string typeCompte = Console.ReadLine();

            Console.Write("Email : ");
            string email = Console.ReadLine();

            Console.Write("Mot de passe : ");
            string motDePasse = Console.ReadLine();

            try
            {
                // Si c'est un admin, on vérifie ses identifiants
                if (typeCompte == "4")
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        string requete = "SELECT id_admin, nom, prenom FROM ADMIN WHERE email = @Email AND mot_de_passe = @MotDePasse";
                        MySqlCommand cmd = new MySqlCommand(requete, connection);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@MotDePasse", motDePasse);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                utilisateurConnecteId = reader["id_admin"].ToString();
                                utilisateurConnecteNom = reader["nom"].ToString();
                                utilisateurConnectePrenom = reader["prenom"].ToString();
                                utilisateurConnecteType = "admin";
                                GestionBDD.AfficherMenuAdmin();
                            }
                            else
                            {
                                GestionBDD.AfficherMessageErreur("Échec de la connexion. Email ou mot de passe incorrect.");
                            }
                        }
                    }
                    return;
                }

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string table = "";
                    string mail = "";
                    string id = "";
                    string nom = "";
                    string prenom = "";
                    string mdp = "mot_de_passe";

                    switch (typeCompte)
                    {
                        case "1":
                            table = "CLIENT";
                            mail = "email";
                            id = "id_client";
                            nom = "nom";
                            prenom = "prénom";
                            utilisateurConnecteType = "client";
                            break;
                        case "2":
                            table = "CUISINIER";
                            mail = "email_cuisinier";
                            id = "id_cuisinier";
                            nom = "nom_cuisinier";
                            prenom = "prénom_cuisinier";
                            mdp = "mot_de_passe";
                            utilisateurConnecteType = "cuisinier";
                            break;
                        case "3":
                            table = "LIVREUR";
                            mail = "email_livreur";
                            id = "ID_livreur";
                            nom = "nom_livreur";
                            prenom = "prénom_livreur";
                            mdp = "mot_de_passe_livreur";
                            utilisateurConnecteType = "livreur";
                            break;
                        default:
                            GestionBDD.AfficherMessageErreur("Type de compte invalide.");
                            return;
                    }

                    string requete = $"SELECT * FROM {table} WHERE {mail} = @Email AND {mdp} = @MotDePasse";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@MotDePasse", motDePasse);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            utilisateurConnecteId = reader[id].ToString();
                            utilisateurConnecteNom = reader[nom].ToString();
                            utilisateurConnectePrenom = reader[prenom].ToString();

                            switch (utilisateurConnecteType)
                            {
                                case "client":
                                    GestionBDD.AfficherMenuClient();
                                    break;
                                case "cuisinier":
                                    GestionBDD.AfficherMenuCuisinier();
                                    break;
                                case "livreur":
                                    GestionBDD.AfficherMenuLivreur();
                                    break;
                            }
                        }
                        else
                        {
                            GestionBDD.AfficherMessageErreur("Échec de la connexion. Email ou mot de passe incorrect.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }
        }
        #endregion


        #region Méthode de création de compte (client, cuisinier, livreur)
        static void CreerCompte()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("CRÉATION DE COMPTE");

            Console.WriteLine("1. Client");
            Console.WriteLine("2. Cuisinier");
            Console.WriteLine("3. Livreur");
            Console.Write("\nChoisissez le type de compte à créer : ");
            string typeCompte = Console.ReadLine();

            switch (typeCompte)
            {
                case "1":
                    CreerCompteClient();
                    break;
                case "2":
                    CreerCompteCuisinier();
                    break;
                case "3":
                    CreerCompteLivreur();
                    break;
                default:
                    GestionBDD.AfficherMessageErreur("Type de compte invalide.");
                    break;
            }

            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        static void CreerCompteClient()
        {
            Console.WriteLine("\n=== CRÉATION DE COMPTE CLIENT ===");

            Console.Write("Type de client (1: Particulier, 2: Entreprise locale) : ");
            string typeClient = Console.ReadLine();
            bool estEntreprise = typeClient == "2";

            Console.Write("IdClient : ");
            string id = Console.ReadLine();

            Console.Write("Nom : ");
            string nom = Console.ReadLine();

            Console.Write("Prénom : ");
            string prenom = Console.ReadLine();

            // Additional fields for business clients
            string nomEntreprise = "";
            string nomReferent = "";

            if (estEntreprise)
            {
                Console.Write("Nom de l'entreprise : ");
                nomEntreprise = Console.ReadLine();

                Console.Write("Nom du référent de commande : ");
                nomReferent = Console.ReadLine();
            }

            Console.Write("Rue : ");
            string rue = Console.ReadLine();

            Console.Write("Code postal : ");
            string codePostal = Console.ReadLine();

            Console.Write("Téléphone : ");
            string tel = Console.ReadLine();

            Console.Write("Email : ");
            string email = Console.ReadLine();

            Console.Write("Mot de passe : ");
            string motDePasse = Console.ReadLine();

            Console.Write("Métro le plus proche : ");
            string metro = Console.ReadLine();

            // Pas besoin de demander la ville car tout le monde habite à Paris
            string ville = "Paris";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string checkTypeColumn = "SHOW COLUMNS FROM CLIENT LIKE 'type_client'";
                    MySqlCommand typeColumnCmd = new MySqlCommand(checkTypeColumn, connection);
                    bool typeColumnExists = typeColumnCmd.ExecuteScalar() != null;

                    //if (!typeColumnExists)
                    //{
                    //    string addTypeColumnQuery = "ALTER TABLE CLIENT ADD COLUMN type_client VARCHAR(20)";
                    //    MySqlCommand addTypeColumnCmd = new MySqlCommand(addTypeColumnQuery, connection);
                    //    addTypeColumnCmd.ExecuteNonQuery();
                    //    Console.WriteLine("Colonne type_client ajoutée à la table CLIENT.");
                    //}

                    // Check if the nom_entreprise column exists
                    string checkEntrepriseColumn = "SHOW COLUMNS FROM CLIENT LIKE 'nom_entreprise'";
                    MySqlCommand entrepriseColumnCmd = new MySqlCommand(checkEntrepriseColumn, connection);
                    bool entrepriseColumnExists = entrepriseColumnCmd.ExecuteScalar() != null;

                    //if (!entrepriseColumnExists)
                    //{
                    //    // Add nom_entreprise column
                    //    string addEntrepriseColumnQuery = "ALTER TABLE CLIENT ADD COLUMN nom_entreprise VARCHAR(100)";
                    //    MySqlCommand addEntrepriseColumnCmd = new MySqlCommand(addEntrepriseColumnQuery, connection);
                    //    addEntrepriseColumnCmd.ExecuteNonQuery();
                    //    Console.WriteLine("Colonne nom_entreprise ajoutée à la table CLIENT.");
                    //}

                    // Check if the nom_referent column exists
                    string checkReferentColumn = "SHOW COLUMNS FROM CLIENT LIKE 'nom_referent'";
                    MySqlCommand referentColumnCmd = new MySqlCommand(checkReferentColumn, connection);
                    bool referentColumnExists = referentColumnCmd.ExecuteScalar() != null;

                    //if (!referentColumnExists)
                    //{
                    //    // Add nom_referent column
                    //    string addReferentColumnQuery = "ALTER TABLE CLIENT ADD COLUMN nom_referent VARCHAR(100)";
                    //    MySqlCommand addReferentColumnCmd = new MySqlCommand(addReferentColumnQuery, connection);
                    //    addReferentColumnCmd.ExecuteNonQuery();
                    //    Console.WriteLine("Colonne nom_referent ajoutée à la table CLIENT.");
                    //}

                    // Vérifier si la colonne mot_de_passe existe
                    string checkColumn = "SHOW COLUMNS FROM CLIENT LIKE 'mot_de_passe'";
                    MySqlCommand columnCmd = new MySqlCommand(checkColumn, connection);
                    bool columnExists = columnCmd.ExecuteScalar() != null;

                    //if (!columnExists)
                    //{
                    //    // Ajouter la colonne mot_de_passe
                    //    string addColumnQuery = "ALTER TABLE CLIENT ADD COLUMN mot_de_passe VARCHAR(50)";
                    //    MySqlCommand addColumnCmd = new MySqlCommand(addColumnQuery, connection);
                    //    addColumnCmd.ExecuteNonQuery();
                    //    Console.WriteLine("Colonne mot_de_passe ajoutée à la table CLIENT.");
                    //}

                    string query = "INSERT INTO CLIENT (id_client, nom, prénom, rue, code_Postal, tel, email, mot_de_passe, " +
                                  "metro_le_plus_proche, ville, type_client, nom_entreprise, nom_referent) " +
                                  "VALUES (@Id, @Nom, @Prénom, @Rue, @CodePostal, @Tel, @Email, @MotDePasse, @Metro, @Ville, " +
                                  "@TypeClient, @NomEntreprise, @NomReferent)";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nom", nom);
                    cmd.Parameters.AddWithValue("@Prénom", prenom);
                    cmd.Parameters.AddWithValue("@Rue", rue);
                    cmd.Parameters.AddWithValue("@CodePostal", codePostal);
                    cmd.Parameters.AddWithValue("@Tel", tel);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@MotDePasse", motDePasse);
                    cmd.Parameters.AddWithValue("@Metro", metro);
                    cmd.Parameters.AddWithValue("@Ville", ville);
                    cmd.Parameters.AddWithValue("@TypeClient", estEntreprise ? "Entreprise" : "Particulier");
                    cmd.Parameters.AddWithValue("@NomEntreprise", nomEntreprise);
                    cmd.Parameters.AddWithValue("@NomReferent", nomReferent);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        Console.WriteLine("Client créé avec succès !");
                    }
                    else
                    {
                        Console.WriteLine("Échec de la création du client.");
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }
        }

        static void CreerCompteCuisinier()
        {
            Console.WriteLine("\n=== CRÉATION DE COMPTE CUISINIER ===");

            Console.Write("ID Cuisinier : ");
            string id = Console.ReadLine();

            Console.Write("Nom : ");
            string nom = Console.ReadLine();

            Console.Write("Prénom : ");
            string prenom = Console.ReadLine();

            Console.Write("Rue : ");
            string rue = Console.ReadLine();

            Console.Write("Code postal : ");
            string codePostal = Console.ReadLine();

            Console.Write("Ville : ");
            string ville = Console.ReadLine();

            Console.Write("Téléphone : ");
            string tel = Console.ReadLine();

            Console.Write("Email : ");
            string email = Console.ReadLine();

            Console.Write("Mot de passe : ");
            string motDePasse = Console.ReadLine();

            Console.Write("Métro le plus proche : ");
            string metro = Console.ReadLine();

            Console.WriteLine("\nDans quel restaurant travaillez-vous ?");
            List<(string id, string nom, string type, decimal capacite)> restaurants = GestionBDD.ObtenirListeRestaurantsDetails();

            if (restaurants.Count == 0)
            {
                Console.WriteLine("Aucun restaurant disponible. Votre profil cuisinier sera créé sans assignation à un restaurant.");
            }
            else
            {
                Console.WriteLine("Liste des restaurants disponibles :");
                foreach (var resto in restaurants)
                {
                    Console.WriteLine($"{resto.id}. {resto.nom} - {resto.type}");
                }

                Console.Write("\nChoisissez un restaurant (ID) : ");
                string idResto = Console.ReadLine();
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO CUISINIER (id_cuisinier, nom_cuisinier, prénom_cuisinier, rue_cuisinier, " +
                                "code_postal_cuisinier, ville_cuisinier, tel_cuisinier, email_cuisinier, mot_de_passe, metro_le_plus_proche_cuisinier) " +
                                "VALUES (@Id, @Nom, @Prenom, @Rue, @CodePostal, @Ville, @Tel, @Email, @MotDePasse, @Metro)";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nom", nom);
                    cmd.Parameters.AddWithValue("@Prenom", prenom);
                    cmd.Parameters.AddWithValue("@Rue", rue);
                    cmd.Parameters.AddWithValue("@CodePostal", codePostal);
                    cmd.Parameters.AddWithValue("@Ville", ville);
                    cmd.Parameters.AddWithValue("@Tel", tel);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@MotDePasse", motDePasse);
                    cmd.Parameters.AddWithValue("@Metro", metro);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine("Cuisinier créé avec succès !");

                        if (restaurants.Count > 0)
                        {
                            Console.Write("\nChoisissez un restaurant (ID) : ");
                            string idResto = Console.ReadLine();

                            var restaurantSelectionne = restaurants.Find(r => r.id == idResto);
                            if (restaurantSelectionne != default)
                            {
                                string assignQuery = "INSERT INTO EMPLOIE (id_cuisinier, id_resto) VALUES (@IdCuisinier, @IdResto)";
                                MySqlCommand assignCmd = new MySqlCommand(assignQuery, connection);
                                assignCmd.Parameters.AddWithValue("@IdCuisinier", id);
                                assignCmd.Parameters.AddWithValue("@IdResto", idResto);

                                try
                                {
                                    assignCmd.ExecuteNonQuery();
                                    Console.WriteLine($"Cuisinier assigné au restaurant {restaurantSelectionne.nom} avec succès !");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Erreur lors de l'assignation au restaurant : {ex.Message}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Restaurant non trouvé. Le cuisinier a été créé sans assignation à un restaurant.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Échec de la création du cuisinier.");
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }
        }

        static void CreerCompteLivreur()
        {
            Console.WriteLine("\n=== CRÉATION DE COMPTE LIVREUR ===");

            Console.Write("ID Livreur : ");
            string id = Console.ReadLine();

            Console.Write("Nom : ");
            string nom = Console.ReadLine();

            Console.Write("Prénom : ");
            string prenom = Console.ReadLine();

            Console.Write("Email : ");
            string email = Console.ReadLine();

            Console.Write("Mot de passe : ");
            string motDePasse = Console.ReadLine();

            Console.Write("Téléphone : ");
            string tel = Console.ReadLine();

            Console.Write("Date d'inscription (YYYY-MM-DD) : ");
            string dateInscription = Console.ReadLine();

            Console.Write("Disponibilité (1 pour disponible, 0 pour indisponible) : ");
            string disponibiliteStr = Console.ReadLine();
            bool disponibilite = disponibiliteStr == "1";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO LIVREUR (ID_livreur, nom_livreur, prénom_livreur, email_livreur, mot_de_passe_livreur, tel_livreur, date_inscription, disponibilité) " +
                                "VALUES (@Id, @Nom, @Prenom, @Email, @MotDePasse, @Tel, @Inscription, @Disponibilite)";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nom", nom);
                    cmd.Parameters.AddWithValue("@Prenom", prenom);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@MotDePasse", motDePasse);
                    cmd.Parameters.AddWithValue("@Tel", tel);
                    cmd.Parameters.AddWithValue("@Inscription", dateInscription);
                    cmd.Parameters.AddWithValue("@Disponibilite", disponibilite);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        Console.WriteLine("Livreur créé avec succès !");
                    }
                    else
                    {
                        Console.WriteLine("Échec de la création du livreur.");
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }
        }
        #endregion

    }
}
