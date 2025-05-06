using MySql.Data.MySqlClient;
using Projet_problème_scientifique_info;
using System;
using System.Collections.Generic;
using System.Data;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Classe gérant les fonctionnalités liées aux profils utilisateurs (clients et cuisiniers)
    /// </summary>
    internal class ModuleProfil
    {
        //private static string connectionString = "Server=localhost;Database=livinparis2;Uid=root;Pwd=REDACTED;";
        private static string connectionString = "Server=localhost;Database=livinparis2;Uid=root;Pwd=REDACTED;";

        #region Menu principal Profil 
        /// <summary>
        /// Affiche le menu principal du module de gestion des profils.
        /// </summary>
        public static void MenuPrincipalProfil()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("MODULE PROFIL");

                if (SessionUtilisateur.EstConnecte())
                {
                    if (SessionUtilisateur.TypeUtilisateur == "client")
                    {
                        Console.WriteLine("1. Voir mon profil");
                        Console.WriteLine("2. Modifier mon profil");
                        Console.WriteLine("3. Gérer mes adresses");
                        Console.WriteLine("4. Changer mon mot de passe");
                        Console.WriteLine("5. Me déconnecter");
                        Console.WriteLine("6. Accéder au menu principal");
                        Console.Write("\nChoisissez une option : ");
                        string choix = Console.ReadLine();

                        switch (choix)
                        {
                            case "1":
                                VoirProfilClient(SessionUtilisateur.IdUtilisateur);
                                break;
                            case "2":
                                ModifierProfilClient(SessionUtilisateur.IdUtilisateur);
                                break;
                            case "3":
                                GererAdressesClient(SessionUtilisateur.IdUtilisateur);
                                break;
                            case "4":
                                ChangerMotDePasse("client", SessionUtilisateur.IdUtilisateur);
                                break;
                            case "5":
                                SessionUtilisateur.Deconnecter();
                                continuer = false;
                                break;
                            case "6":
                                continuer = false;
                                break;
                            default:
                                GestionBDD.AfficherMessageErreur("Option invalide.");
                                break;
                        }
                    }
                    else if (SessionUtilisateur.TypeUtilisateur == "cuisinier")
                    {
                        Console.WriteLine("1. Voir mon profil");
                        Console.WriteLine("2. Modifier mon profil");
                        Console.WriteLine("3. Changer mon mot de passe");
                        Console.WriteLine("4. Voir mes évaluations");
                        Console.WriteLine("5. Me déconnecter");
                        Console.WriteLine("6. Accéder au menu principal");
                        Console.Write("\nChoisissez une option : ");
                        string choix = Console.ReadLine();

                        switch (choix)
                        {
                            case "1":
                                VoirProfilCuisinier(SessionUtilisateur.IdUtilisateur);
                                break;
                            case "2":
                                ModifierProfilCuisinier(SessionUtilisateur.IdUtilisateur);
                                break;
                            case "3":
                                ChangerMotDePasse("cuisinier", SessionUtilisateur.IdUtilisateur);
                                break;
                            case "4":
                                VoirEvaluationsCuisinier(SessionUtilisateur.IdUtilisateur);
                                break;
                            case "5":
                                SessionUtilisateur.Deconnecter();
                                continuer = false;
                                break;
                            case "6":
                                continuer = false;
                                break;
                            default:
                                GestionBDD.AfficherMessageErreur("Option invalide.");
                                break;
                        }
                    }

                    else if (SessionUtilisateur.TypeUtilisateur == "admin")
                    {
                        Console.WriteLine("1. Gérer les comptes clients");
                        Console.WriteLine("2. Gérer les comptes cuisiniers");
                        Console.WriteLine("3. Créer un compte administrateur");
                        Console.WriteLine("4. Voir mon profil");
                        Console.WriteLine("5. Modifier mon profil");
                        Console.WriteLine("6. Changer mon mot de passe");
                        Console.WriteLine("7. Me déconnecter");
                        Console.WriteLine("8. Accéder au menu principal");
                        Console.Write("\nChoisissez une option : ");
                        string choix = Console.ReadLine();

                        switch (choix)
                        {
                            case "1":
                                GererComptesClients();
                                break;
                            case "2":
                                GererComptesCuisiniers();
                                break;
                            case "3":
                                CreerCompteAdmin();
                                break;
                            case "4":
                                VoirProfilAdmin(SessionUtilisateur.IdUtilisateur);
                                break;
                            case "5":
                                ModifierProfilAdmin(SessionUtilisateur.IdUtilisateur);
                                break;
                            case "6":
                                ChangerMotDePasse("admin", SessionUtilisateur.IdUtilisateur);
                                break;
                            case "7":
                                SessionUtilisateur.Deconnecter();
                                continuer = false;
                                break;
                            case "8":
                                continuer = false;
                                break;
                            default:
                                GestionBDD.AfficherMessageErreur("Option invalide.");
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("1. Se connecter");
                    Console.WriteLine("2. Créer un compte");
                    Console.WriteLine("3. Retour au menu principal");
                    Console.Write("\nChoisissez une option : ");
                    string choix = Console.ReadLine();

                    switch (choix)
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
        }
        #endregion

        #region Connexion et création de compte

        /// <summary>
        /// Permet à un utilisateur de se connecter (client, cuisinier ou admin)
        /// </summary>
        private static void SeConnecter()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("CONNEXION");

            Console.WriteLine("Type de compte :");
            Console.WriteLine("1. Client");
            Console.WriteLine("2. Cuisinier");
            Console.WriteLine("3. Administrateur");
            Console.Write("\nVotre choix : ");
            string typeCompte = Console.ReadLine();

            string typeUtilisateur = "";
            switch (typeCompte)
            {
                case "1":
                    typeUtilisateur = "client";
                    break;
                case "2":
                    typeUtilisateur = "cuisinier";
                    break;
                case "3":
                    typeUtilisateur = "admin";
                    break;
                default:
                    GestionBDD.AfficherMessageErreur("Type de compte invalide.");
                    return;
            }

            Console.Write("Email : ");
            string email = Console.ReadLine();

            Console.Write("Mot de passe : ");
            string motDePasse = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "";
                    string idColonne = "";
                    string nomColonne = "";
                    string prenomColonne = "";
                    string emailColonne = "";

                    if (typeUtilisateur == "client")
                    {
                        query = "SELECT id_client, nom_client, prenom_client, email_client FROM CLIENT " +
                               "WHERE email_client = @Email AND mot_de_passe_client = @MotDePasse";
                        idColonne = "id_client";
                        nomColonne = "nom_client";
                        prenomColonne = "prenom_client";
                        emailColonne = "email_client";
                    }
                    else if (typeUtilisateur == "cuisinier")
                    {
                        query = "SELECT id_cuisinier, nom_cuisinier, prenom_cuisinier, email_cuisinier FROM CUISINIER " +
                               "WHERE email_cuisinier = @Email AND mot_de_passe_cuisinier = @MotDePasse";
                        idColonne = "id_cuisinier";
                        nomColonne = "nom_cuisinier";
                        prenomColonne = "prenom_cuisinier";
                        emailColonne = "email_cuisinier";
                    }
                    else if (typeUtilisateur == "admin")
                    {
                        query = "SELECT id_admin, nom_admin, prenom_admin, email_admin FROM ADMINISTRATEUR " +
                               "WHERE email_admin = @Email AND mot_de_passe_admin = @MotDePasse";
                        idColonne = "id_admin";
                        nomColonne = "nom_admin";
                        prenomColonne = "prenom_admin";
                        emailColonne = "email_admin";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@MotDePasse", motDePasse);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string id = reader[idColonne].ToString();
                            string nom = reader.IsDBNull(reader.GetOrdinal(nomColonne)) ? "" : reader[nomColonne].ToString();
                            string prenom = reader.IsDBNull(reader.GetOrdinal(prenomColonne)) ? "" : reader[prenomColonne].ToString();
                            string emailUtilisateur = reader.IsDBNull(reader.GetOrdinal(emailColonne)) ? "" : reader[emailColonne].ToString();

                            SessionUtilisateur.Connecter(id, nom, prenom, typeUtilisateur, emailUtilisateur);
                        }
                        else
                        {
                            GestionBDD.AfficherMessageErreur("Email ou mot de passe incorrect.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }
        }


        private static void CreerCompte()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("CREATION DE COMPTE");

            Console.WriteLine("Type de compte :");
            Console.WriteLine("1. Client");
            Console.WriteLine("2. Cuisinier");
            Console.Write("\nVotre choix : ");
            string typeCompte = Console.ReadLine();

            switch (typeCompte)
            {
                case "1":
                    CreerCompteClient();
                    break;
                case "2":
                    CreerCompteCuisinier();
                    break;
                default:
                    GestionBDD.AfficherMessageErreur("Option invalide.");
                    break;
            }
        }

        /// <summary>
        /// Crée un nouveau compte client
        /// </summary>
        private static void CreerCompteClient()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("CRÉATION DE COMPTE CLIENT");

            Console.Write("Nom : ");
            string nom = Console.ReadLine();

            Console.Write("Prénom : ");
            string prenom = Console.ReadLine();

            Console.Write("Email : ");
            string email = Console.ReadLine();

            Console.Write("Mot de passe : ");
            string motDePasse = Console.ReadLine();

            Console.Write("Téléphone : ");
            string telephone = Console.ReadLine();

            Console.Write("Type de client (particulier, entreprise) : ");
            string typeClient = Console.ReadLine();

            Console.Write("Nom d'entreprise (si applicable) : ");
            string nomEntreprise = Console.ReadLine();

            Console.Write("Nom du référent (si applicable) : ");
            string nomReferent = Console.ReadLine();

            Console.Write("Code postal : ");
            string codePostal = Console.ReadLine();

            Console.Write("Rue : ");
            string rue = Console.ReadLine();

            Console.Write("Métro le plus proche (la saisie se fait sans accents grave, aigu ou circonflexe, sans cédille et sans tréma, " +
                "et en respectant les espaces et les tirets pour les noms de métros composés: ");
            string metro = Console.ReadLine();

            if (EmailClientExiste(email))
            {
                GestionBDD.AfficherMessageErreur("Un compte avec cet email existe déjà.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            int idClient = GestionBDD.GenererNouvelID("CLIENT", "id_client");

            try
            {
                Dictionary<string, object> parametres = new Dictionary<string, object>
                {
                    { "@Id", idClient },
                    { "@Nom", nom },
                    { "@Prenom", prenom },
                    { "@Email", email },
                    { "@Tel", telephone },
                    { "@TypeClient", typeClient },
                    { "@NomEntreprise", string.IsNullOrEmpty(nomEntreprise) ? DBNull.Value : (object)nomEntreprise },
                    { "@NomReferent", string.IsNullOrEmpty(nomReferent) ? DBNull.Value : (object)nomReferent },
                    { "@CodePostal", codePostal },
                    { "@Rue", rue },
                    { "@Metro", metro },
                    { "@MotDePasse", motDePasse }
                };

                string query = "INSERT INTO CLIENT (id_client, nom_client, prenom_client, email_client, tel_client, " +
                              "type_client, nom_entreprise, nom_referent, code_postal_client, rue_client, " +
                              "metro_le_plus_proche_client, mot_de_passe_client, date_inscription_client) " +
                              "VALUES (@Id, @Nom, @Prenom, @Email, @Tel, @TypeClient, @NomEntreprise, @NomReferent, " +
                              "@CodePostal, @Rue, @Metro, @MotDePasse, NOW())";

                int resultat = GestionBDD.ExecuterRequete(query, parametres);

                if (resultat > 0)
                {
                    GestionBDD.AfficherMessageSucces("Compte client créé avec succès !");

                    Console.Write("Souhaitez-vous vous connecter maintenant ? (O/N) : ");
                    string reponse = Console.ReadLine().ToUpper();

                    if (reponse == "O")
                    {
                        SessionUtilisateur.Connecter(idClient.ToString(), nom, prenom, "client", email);
                    }
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Erreur lors de la création du compte client.");
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
        /// Crée un nouveau compte cuisinier
        /// </summary>
        private static void CreerCompteCuisinier()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("CRÉATION DE COMPTE CUISINIER");

            Console.Write("Nom : ");
            string nom = Console.ReadLine();

            Console.Write("Prénom : ");
            string prenom = Console.ReadLine();

            Console.Write("Email : ");
            string email = Console.ReadLine();

            Console.Write("Mot de passe : ");
            string motDePasse = Console.ReadLine();

            Console.Write("Téléphone : ");
            string telephone = Console.ReadLine();

            Console.Write("Spécialité : ");
            string specialite = Console.ReadLine();

            Console.Write("Disponibilité (jours/heures) : ");
            string disponibilite = Console.ReadLine();

            Console.Write("Code postal : ");
            string codePostal = Console.ReadLine();

            Console.Write("Rue : ");
            string rue = Console.ReadLine();

            if (EmailCuisinierExiste(email))
            {
                GestionBDD.AfficherMessageErreur("Un compte avec cet email existe déjà.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            int idCuisinier = GestionBDD.GenererNouvelID("CUISINIER", "id_cuisinier");

            try
            {
                Dictionary<string, object> parametres = new Dictionary<string, object>
                {
                    { "@Id", idCuisinier },
                    { "@Nom", nom },
                    { "@Prenom", prenom },
                    { "@Email", email },
                    { "@Tel", telephone },
                    { "@Specialite", specialite },
                    { "@Disponibilite", disponibilite },
                    { "@CodePostal", codePostal },
                    { "@Rue", rue },
                    { "@MotDePasse", motDePasse },
                    { "@NoteMoyenne", 0 }
                };

                string query = "INSERT INTO CUISINIER (id_cuisinier, nom_cuisinier, prenom_cuisinier, email_cuisinier, tel_cuisinier, " +
                              "specialite_cuisinier, dispo_cuisinier, code_postal_cuisinier, rue_cuisinier, mot_de_passe_cuisinier, note_moyenne_cuisinier, date_inscription_cuisinier) " +
                              "VALUES (@Id, @Nom, @Prenom, @Email, @Tel, @Specialite, @Disponibilite, @CodePostal, @Rue, @MotDePasse, @NoteMoyenne, NOW())";

                int resultat = GestionBDD.ExecuterRequete(query, parametres);

                if (resultat > 0)
                {
                    GestionBDD.AfficherMessageSucces("Compte cuisinier créé avec succès !");

                    Console.Write("Souhaitez-vous vous connecter maintenant ? (O/N) : ");
                    string reponse = Console.ReadLine().ToUpper();

                    if (reponse == "O")
                    {
                        SessionUtilisateur.Connecter(idCuisinier.ToString(), nom, prenom, "cuisinier", email);
                    }
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Erreur lors de la création du compte cuisinier.");
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
        /// Crée un nouveau compte administrateur
        /// </summary>
        private static void CreerCompteAdmin()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("CRÉATION DE COMPTE ADMINISTRATEUR");

            if (!SessionUtilisateur.EstAdmin())
            {
                GestionBDD.AfficherMessageErreur("Vous n'avez pas les droits pour créer un compte administrateur.");
                return;
            }

            Console.Write("Nom : ");
            string nom = Console.ReadLine();

            Console.Write("Prénom : ");
            string prenom = Console.ReadLine();

            Console.Write("Email : ");
            string email = Console.ReadLine();

            Console.Write("Mot de passe : ");
            string motDePasse = Console.ReadLine();


            if (EmailAdminExiste(email))
            {
                GestionBDD.AfficherMessageErreur("Un compte administrateur avec cet email existe déjà.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            int idAdmin = GestionBDD.GenererNouvelID("ADMINISTRATEUR", "id_admin");

            try
            {
                Dictionary<string, object> parametres = new Dictionary<string, object>
                {
                    { "@Id", idAdmin },
                    { "@Nom", nom },
                    { "@Prenom", prenom },
                    { "@Email", email },
                    { "@MotDePasse", motDePasse }
                };

                string query = "INSERT INTO ADMINISTRATEUR (id_admin, nom_admin, prenom_admin, email_admin, mot_de_passe_admin) " +
                              "VALUES (@Id, @Nom, @Prenom, @Email, @MotDePasse)";

                int resultat = GestionBDD.ExecuterRequete(query, parametres);

                if (resultat > 0)
                {
                    GestionBDD.AfficherMessageSucces("Compte administrateur créé avec succès !");
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Erreur lors de la création du compte administrateur.");
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        #endregion

        #region Gestion des profils clients

        /// <summary>
        /// Affiche les détails du profil d'un client
        /// </summary>
        /// <param name="idClient">ID du client</param>
        private static void VoirProfilClient(string idClient)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MON PROFIL");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT id_client, nom_client, prenom_client, email_client, tel_client, 
                               type_client, nom_entreprise, nom_referent, code_postal_client, 
                               rue_client, metro_le_plus_proche_client, date_inscription_client
                        FROM CLIENT
                        WHERE id_client = @IdClient";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdClient", idClient);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nom = reader.IsDBNull(reader.GetOrdinal("nom_client")) ? "N/A" : reader["nom_client"].ToString();
                            string prenom = reader.IsDBNull(reader.GetOrdinal("prenom_client")) ? "N/A" : reader["prenom_client"].ToString();
                            string email = reader.IsDBNull(reader.GetOrdinal("email_client")) ? "N/A" : reader["email_client"].ToString();
                            string telephone = reader.IsDBNull(reader.GetOrdinal("tel_client")) ? "N/A" : reader["tel_client"].ToString();
                            string typeClient = reader.IsDBNull(reader.GetOrdinal("type_client")) ? "N/A" : reader["type_client"].ToString();
                            string nomEntreprise = reader.IsDBNull(reader.GetOrdinal("nom_entreprise")) ? "N/A" : reader["nom_entreprise"].ToString();
                            string nomReferent = reader.IsDBNull(reader.GetOrdinal("nom_referent")) ? "N/A" : reader["nom_referent"].ToString();
                            string codePostal = reader.IsDBNull(reader.GetOrdinal("code_postal_client")) ? "N/A" : reader["code_postal_client"].ToString();
                            string rue = reader.IsDBNull(reader.GetOrdinal("rue_client")) ? "N/A" : reader["rue_client"].ToString();
                            string metro = reader.IsDBNull(reader.GetOrdinal("metro_le_plus_proche_client")) ? "N/A" : reader["metro_le_plus_proche_client"].ToString();
                            string dateInscription = reader.IsDBNull(reader.GetOrdinal("date_inscription_client")) ? "N/A" : reader["date_inscription_client"].ToString();

                            Console.WriteLine($"Nom : {nom}");
                            Console.WriteLine($"Prénom : {prenom}");
                            Console.WriteLine($"Email : {email}");
                            Console.WriteLine($"Téléphone : {telephone}");
                            Console.WriteLine($"Type de client : {typeClient}");

                            if (typeClient.ToLower() == "entreprise")
                            {
                                Console.WriteLine($"Nom d'entreprise : {nomEntreprise}");
                                Console.WriteLine($"Nom du référent : {nomReferent}");
                            }

                            Console.WriteLine($"Adresse : {rue}, {codePostal}, PARIS");
                            Console.WriteLine($"Métro le plus proche : {metro}");
                            Console.WriteLine($"Date d'inscription : {dateInscription}");

                            reader.Close();
                            AfficherAdressesClient(idClient, connection);
                        }
                        else
                        {
                            Console.WriteLine("Profil client non trouvé.");
                        }
                    }
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
        /// Affiche les adresses complémentaires d'un client
        /// </summary>
        /// <param name="idClient">ID du client</param>
        /// <param name="connection">Connexion à la base de données</param>
        private static void AfficherAdressesClient(string idClient, MySqlConnection connection)
        {
            string query = @"
                SELECT id_adresse, code_porte, complement_adresse, étage, numero_appartement
                FROM ADRESSE
                WHERE id_client = @IdClient";

            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@IdClient", idClient);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Console.WriteLine("\nAdresses complémentaires :");
                    Console.WriteLine(new string('-', 50));

                    while (reader.Read())
                    {
                        string idAdresse = reader["id_adresse"].ToString();
                        string codePorte = reader.IsDBNull(reader.GetOrdinal("code_porte")) ? "N/A" : reader["code_porte"].ToString();
                        string complementAdresse = reader.IsDBNull(reader.GetOrdinal("complement_adresse")) ? "N/A" : reader["complement_adresse"].ToString();
                        string etage = reader.IsDBNull(reader.GetOrdinal("étage")) ? "N/A" : reader["étage"].ToString();
                        string numeroAppartement = reader.IsDBNull(reader.GetOrdinal("numero_appartement")) ? "N/A" : reader["numero_appartement"].ToString();

                        Console.WriteLine($"Adresse #{idAdresse}:");
                        Console.WriteLine($"  Code porte : {codePorte}");
                        Console.WriteLine($"  Complément : {complementAdresse}");
                        Console.WriteLine($"  Étage : {etage}");
                        Console.WriteLine($"  N° Appartement : {numeroAppartement}");
                        Console.WriteLine(new string('-', 50));
                    }
                }
                else
                {
                    Console.WriteLine("\nAucune adresse complémentaire enregistrée.");
                }
            }
        }

        /// <summary>
        /// Modifie les informations d'un profil client
        /// </summary>
        /// <param name="idClient">ID du client</param>
        private static void ModifierProfilClient(string idClient)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MODIFIER MON PROFIL");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string queryInfo = @"
                        SELECT nom_client, prenom_client, email_client, tel_client, 
                               type_client, nom_entreprise, nom_referent, code_postal_client, 
                               rue_client, metro_le_plus_proche_client
                        FROM CLIENT
                        WHERE id_client = @IdClient";

                    MySqlCommand cmdInfo = new MySqlCommand(queryInfo, connection);
                    cmdInfo.Parameters.AddWithValue("@IdClient", idClient);

                    using (MySqlDataReader reader = cmdInfo.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nomActuel = reader.IsDBNull(reader.GetOrdinal("nom_client")) ? "" : reader["nom_client"].ToString();
                            string prenomActuel = reader.IsDBNull(reader.GetOrdinal("prenom_client")) ? "" : reader["prenom_client"].ToString();
                            string emailActuel = reader.IsDBNull(reader.GetOrdinal("email_client")) ? "" : reader["email_client"].ToString();
                            string telephoneActuel = reader.IsDBNull(reader.GetOrdinal("tel_client")) ? "" : reader["tel_client"].ToString();
                            string typeClientActuel = reader.IsDBNull(reader.GetOrdinal("type_client")) ? "" : reader["type_client"].ToString();
                            string nomEntrepriseActuel = reader.IsDBNull(reader.GetOrdinal("nom_entreprise")) ? "" : reader["nom_entreprise"].ToString();
                            string nomReferentActuel = reader.IsDBNull(reader.GetOrdinal("nom_referent")) ? "" : reader["nom_referent"].ToString();
                            string codePostalActuel = reader.IsDBNull(reader.GetOrdinal("code_postal_client")) ? "" : reader["code_postal_client"].ToString();
                            string rueActuelle = reader.IsDBNull(reader.GetOrdinal("rue_client")) ? "" : reader["rue_client"].ToString();
                            string metroActuel = reader.IsDBNull(reader.GetOrdinal("metro_le_plus_proche_client")) ? "" : reader["metro_le_plus_proche_client"].ToString();

                            reader.Close();

                            Console.WriteLine("Laissez vide pour conserver la valeur actuelle.");

                            Console.Write($"Nom ({nomActuel}) : ");
                            string nouveauNom = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauNom))
                                nouveauNom = nomActuel;

                            Console.Write($"Prénom ({prenomActuel}) : ");
                            string nouveauPrenom = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauPrenom))
                                nouveauPrenom = prenomActuel;

                            Console.Write($"Email ({emailActuel}) : ");
                            string nouvelEmail = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouvelEmail))
                                nouvelEmail = emailActuel;
                            else if (nouvelEmail != emailActuel && EmailClientExiste(nouvelEmail))
                            {
                                GestionBDD.AfficherMessageErreur("Cet email est déjà utilisé par un autre compte.");
                                return;
                            }

                            Console.Write($"Téléphone ({telephoneActuel}) : ");
                            string nouveauTelephone = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauTelephone))
                                nouveauTelephone = telephoneActuel;

                            Console.Write($"Type de client ({typeClientActuel}) : ");
                            string nouveauTypeClient = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauTypeClient))
                                nouveauTypeClient = typeClientActuel;

                            string nouveauNomEntreprise = nomEntrepriseActuel;
                            string nouveauNomReferent = nomReferentActuel;

                            if (nouveauTypeClient.ToLower() == "entreprise")
                            {
                                Console.Write($"Nom d'entreprise ({nomEntrepriseActuel}) : ");
                                string inputNomEntreprise = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(inputNomEntreprise))
                                    nouveauNomEntreprise = inputNomEntreprise;

                                Console.Write($"Nom du référent ({nomReferentActuel}) : ");
                                string inputNomReferent = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(inputNomReferent))
                                    nouveauNomReferent = inputNomReferent;
                            }

                            Console.Write($"Code postal ({codePostalActuel}) : ");
                            string nouveauCodePostal = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauCodePostal))
                                nouveauCodePostal = codePostalActuel;

                            Console.Write($"Rue ({rueActuelle}) : ");
                            string nouvelleRue = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouvelleRue))
                                nouvelleRue = rueActuelle;

                            Console.Write($"Métro le plus proche ({metroActuel}) : ");
                            string nouveauMetro = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauMetro))
                                nouveauMetro = metroActuel;

                            string queryUpdate = @"
                                UPDATE CLIENT
                                SET nom_client = @Nom,
                                    prenom_client = @Prenom,
                                    email_client = @Email,
                                    tel_client = @Tel,
                                    type_client = @TypeClient,
                                    nom_entreprise = @NomEntreprise,
                                    nom_referent = @NomReferent,
                                    code_postal_client = @CodePostal,
                                    rue_client = @Rue,
                                    metro_le_plus_proche_client = @Metro
                                WHERE id_client = @IdClient";

                            MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, connection);
                            cmdUpdate.Parameters.AddWithValue("@IdClient", idClient);
                            cmdUpdate.Parameters.AddWithValue("@Nom", nouveauNom);
                            cmdUpdate.Parameters.AddWithValue("@Prenom", nouveauPrenom);
                            cmdUpdate.Parameters.AddWithValue("@Email", nouvelEmail);
                            cmdUpdate.Parameters.AddWithValue("@Tel", nouveauTelephone);
                            cmdUpdate.Parameters.AddWithValue("@TypeClient", nouveauTypeClient);
                            cmdUpdate.Parameters.AddWithValue("@NomEntreprise", string.IsNullOrEmpty(nouveauNomEntreprise) ? DBNull.Value : (object)nouveauNomEntreprise);
                            cmdUpdate.Parameters.AddWithValue("@NomReferent", string.IsNullOrEmpty(nouveauNomReferent) ? DBNull.Value : (object)nouveauNomReferent);
                            cmdUpdate.Parameters.AddWithValue("@CodePostal", nouveauCodePostal);
                            cmdUpdate.Parameters.AddWithValue("@Rue", nouvelleRue);
                            cmdUpdate.Parameters.AddWithValue("@Metro", nouveauMetro);

                            int resultat = cmdUpdate.ExecuteNonQuery();

                            if (resultat > 0)
                            {
                                GestionBDD.AfficherMessageSucces("Profil modifié avec succès !");

                                if (nouvelEmail != emailActuel)
                                {
                                    SessionUtilisateur.Connecter(idClient, nouveauNom, nouveauPrenom, "client", nouvelEmail);
                                }
                            }
                            else
                            {
                                GestionBDD.AfficherMessageErreur("Erreur lors de la modification du profil.");
                            }
                        }
                        else
                        {
                            reader.Close();
                            GestionBDD.AfficherMessageErreur("Profil client non trouvé.");
                        }
                    }
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
        /// Gère les adresses d'un client
        /// </summary>
        /// <param name="idClient">ID du client</param>
        private static void GererAdressesClient(string idClient)
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("GESTION DES ADRESSES");

                Console.WriteLine("1. Voir mes adresses");
                Console.WriteLine("2. Ajouter une adresse");
                Console.WriteLine("3. Modifier une adresse");
                Console.WriteLine("4. Supprimer une adresse");
                Console.WriteLine("5. Retour");
                Console.Write("\nChoisissez une option : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        VoirAdressesClient(idClient);
                        break;
                    case "2":
                        AjouterAdresseClient(idClient);
                        break;
                    case "3":
                        ModifierAdresseClient(idClient);
                        break;
                    case "4":
                        SupprimerAdresseClient(idClient);
                        break;
                    case "5":
                        continuer = false;
                        break;
                    default:
                        GestionBDD.AfficherMessageErreur("Option invalide.");
                        break;
                }
            }
        }

        /// <summary>
        /// Affiche les adresses d'un client
        /// </summary>
        /// <param name="idClient">ID du client</param>
        private static void VoirAdressesClient(string idClient)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MES ADRESSES");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    /// Afficher l'adresse principale
                    string queryPrincipale = @"
                        SELECT code_postal_client, rue_client, metro_le_plus_proche_client
                        FROM CLIENT
                        WHERE id_client = @IdClient";

                    MySqlCommand cmdPrincipale = new MySqlCommand(queryPrincipale, connection);
                    cmdPrincipale.Parameters.AddWithValue("@IdClient", idClient);

                    using (MySqlDataReader reader = cmdPrincipale.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string codePostal = reader.IsDBNull(reader.GetOrdinal("code_postal_client")) ? "N/A" : reader["code_postal_client"].ToString();
                            string rue = reader.IsDBNull(reader.GetOrdinal("rue_client")) ? "N/A" : reader["rue_client"].ToString();
                            string metro = reader.IsDBNull(reader.GetOrdinal("metro_le_plus_proche_client")) ? "N/A" : reader["metro_le_plus_proche_client"].ToString();

                            Console.WriteLine("Adresse principale :");
                            Console.WriteLine($"Rue : {rue}");
                            Console.WriteLine($"Code postal : {codePostal}");
                            Console.WriteLine($"Métro le plus proche : {metro}");
                        }
                        else
                        {
                            Console.WriteLine("Adresse principale non trouvée.");
                        }
                    }

                    /// Afficher les adresses complémentaires
                    AfficherAdressesClient(idClient, connection);
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
        /// Ajoute une adresse pour un client
        /// </summary>
        /// <param name="idClient">ID du client</param>
        private static void AjouterAdresseClient(string idClient)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("AJOUTER UNE ADRESSE");

            Console.Write("Code porte : ");
            string codePorte = Console.ReadLine();

            Console.Write("Complément d'adresse : ");
            string complementAdresse = Console.ReadLine();

            Console.Write("Étage : ");
            string etageStr = Console.ReadLine();
            int etage = 0;
            if (!string.IsNullOrWhiteSpace(etageStr) && int.TryParse(etageStr, out int parsedEtage))
            {
                etage = parsedEtage;
            }

            Console.Write("Numéro d'appartement : ");
            string numeroAppartement = Console.ReadLine();

            int idAdresse = GestionBDD.GenererNouvelID("ADRESSE", "id_adresse");

            try
            {
                Dictionary<string, object> parametres = new Dictionary<string, object>
                {
                    { "@IdAdresse", idAdresse },
                    { "@IdClient", idClient },
                    { "@CodePorte", codePorte },
                    { "@ComplementAdresse", complementAdresse },
                    { "@Etage", etage },
                    { "@NumeroAppartement", numeroAppartement }
                };

                string query = "INSERT INTO ADRESSE (id_adresse, id_client, code_porte, complement_adresse, étage, numero_appartement) " +
                              "VALUES (@IdAdresse, @IdClient, @CodePorte, @ComplementAdresse, @Etage, @NumeroAppartement)";

                int resultat = GestionBDD.ExecuterRequete(query, parametres);

                if (resultat > 0)
                {
                    GestionBDD.AfficherMessageSucces("Adresse ajoutée avec succès !");
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Erreur lors de l'ajout de l'adresse.");
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
        /// Modifie une adresse d'un client
        /// </summary>
        /// <param name="idClient">ID du client</param>
        private static void ModifierAdresseClient(string idClient)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MODIFIER UNE ADRESSE");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string queryAdresses = @"
                        SELECT id_adresse, code_porte, complement_adresse, étage, numero_appartement
                        FROM ADRESSE
                        WHERE id_client = @IdClient";

                    MySqlCommand cmdAdresses = new MySqlCommand(queryAdresses, connection);
                    cmdAdresses.Parameters.AddWithValue("@IdClient", idClient);

                    using (MySqlDataReader reader = cmdAdresses.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("Adresses disponibles :");
                            Console.WriteLine(new string('-', 50));

                            while (reader.Read())
                            {
                                string idAdresse1 = reader["id_adresse"].ToString();
                                string codePorte = reader.IsDBNull(reader.GetOrdinal("code_porte")) ? "N/A" : reader["code_porte"].ToString();
                                string complementAdresse = reader.IsDBNull(reader.GetOrdinal("complement_adresse")) ? "N/A" : reader["complement_adresse"].ToString();
                                string etage = reader.IsDBNull(reader.GetOrdinal("étage")) ? "N/A" : reader["étage"].ToString();
                                string numeroAppartement = reader.IsDBNull(reader.GetOrdinal("numero_appartement")) ? "N/A" : reader["numero_appartement"].ToString();

                                Console.WriteLine($"Adresse #{idAdresse1}:");
                                Console.WriteLine($"  Code porte : {codePorte}");
                                Console.WriteLine($"  Complément : {complementAdresse}");
                                Console.WriteLine($"  Étage : {etage}");
                                Console.WriteLine($"  N° Appartement : {numeroAppartement}");
                                Console.WriteLine(new string('-', 50));
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucune adresse complémentaire trouvée.");
                            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                            Console.ReadKey();
                            return;
                        }
                    }

                    Console.Write("\nEntrez l'ID de l'adresse à modifier : ");
                    string idAdresse = Console.ReadLine();

                    string queryVerif = "SELECT COUNT(*) FROM ADRESSE WHERE id_adresse = @IdAdresse AND id_client = @IdClient";
                    MySqlCommand cmdVerif = new MySqlCommand(queryVerif, connection);
                    cmdVerif.Parameters.AddWithValue("@IdAdresse", idAdresse);
                    cmdVerif.Parameters.AddWithValue("@IdClient", idClient);

                    int count = Convert.ToInt32(cmdVerif.ExecuteScalar());
                    if (count == 0)
                    {
                        GestionBDD.AfficherMessageErreur("Adresse introuvable ou n'appartenant pas à ce client.");
                        return;
                    }

                    /// Récupérer les informations actuelles
                    string queryInfo = "SELECT code_porte, complement_adresse, étage, numero_appartement FROM ADRESSE WHERE id_adresse = @IdAdresse";
                    MySqlCommand cmdInfo = new MySqlCommand(queryInfo, connection);
                    cmdInfo.Parameters.AddWithValue("@IdAdresse", idAdresse);

                    using (MySqlDataReader reader = cmdInfo.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string codePorteActuel = reader.IsDBNull(reader.GetOrdinal("code_porte")) ? "" : reader["code_porte"].ToString();
                            string complementAdresseActuel = reader.IsDBNull(reader.GetOrdinal("complement_adresse")) ? "" : reader["complement_adresse"].ToString();
                            int etageActuel = reader.IsDBNull(reader.GetOrdinal("étage")) ? 0 : Convert.ToInt32(reader["étage"]);
                            string numeroAppartementActuel = reader.IsDBNull(reader.GetOrdinal("numero_appartement")) ? "" : reader["numero_appartement"].ToString();

                            reader.Close();

                            Console.WriteLine("\nLaissez vide pour conserver la valeur actuelle.");

                            Console.Write($"Nouveau code porte ({codePorteActuel}) : ");
                            string nouveauCodePorte = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauCodePorte))
                                nouveauCodePorte = codePorteActuel;

                            Console.Write($"Nouveau complément d'adresse ({complementAdresseActuel}) : ");
                            string nouveauComplementAdresse = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauComplementAdresse))
                                nouveauComplementAdresse = complementAdresseActuel;

                            Console.Write($"Nouvel étage ({etageActuel}) : ");
                            string etageInput = Console.ReadLine();
                            int nouvelEtage = etageActuel;
                            if (!string.IsNullOrWhiteSpace(etageInput) && int.TryParse(etageInput, out int parsedEtage))
                                nouvelEtage = parsedEtage;

                            Console.Write($"Nouveau numéro d'appartement ({numeroAppartementActuel}) : ");
                            string nouveauNumeroAppartement = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauNumeroAppartement))
                                nouveauNumeroAppartement = numeroAppartementActuel;
                            
                            /// Mettre à jour l'adresse
                            string queryUpdate = @"
                                UPDATE ADRESSE
                                SET code_porte = @CodePorte,
                                    complement_adresse = @ComplementAdresse,
                                    étage = @Etage,
                                    numero_appartement = @NumeroAppartement
                                WHERE id_adresse = @IdAdresse";

                            MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, connection);
                            cmdUpdate.Parameters.AddWithValue("@IdAdresse", idAdresse);
                            cmdUpdate.Parameters.AddWithValue("@CodePorte", nouveauCodePorte);
                            cmdUpdate.Parameters.AddWithValue("@ComplementAdresse", nouveauComplementAdresse);
                            cmdUpdate.Parameters.AddWithValue("@Etage", nouvelEtage);
                            cmdUpdate.Parameters.AddWithValue("@NumeroAppartement", nouveauNumeroAppartement);

                            int resultat = cmdUpdate.ExecuteNonQuery();

                            if (resultat > 0)
                            {
                                GestionBDD.AfficherMessageSucces("Adresse modifiée avec succès !");
                            }
                            else
                            {
                                GestionBDD.AfficherMessageErreur("Erreur lors de la modification de l'adresse.");
                            }
                        }
                        else
                        {
                            reader.Close();
                            GestionBDD.AfficherMessageErreur("Adresse introuvable.");
                        }
                    }
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
        /// Supprime une adresse d'un client
        /// </summary>
        /// <param name="idClient">ID du client</param>
        private static void SupprimerAdresseClient(string idClient)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("SUPPRIMER UNE ADRESSE");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    /// Afficher les adresses du client
                    string queryAdresses = @"
                        SELECT id_adresse, code_porte, complement_adresse, étage, numero_appartement
                        FROM ADRESSE
                        WHERE id_client = @IdClient";

                    MySqlCommand cmdAdresses = new MySqlCommand(queryAdresses, connection);
                    cmdAdresses.Parameters.AddWithValue("@IdClient", idClient);

                    using (MySqlDataReader reader = cmdAdresses.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("Adresses disponibles :");
                            Console.WriteLine(new string('-', 50));

                            while (reader.Read())
                            {
                                string idAdresse1 = reader["id_adresse"].ToString();
                                string codePorte = reader.IsDBNull(reader.GetOrdinal("code_porte")) ? "N/A" : reader["code_porte"].ToString();
                                string complementAdresse = reader.IsDBNull(reader.GetOrdinal("complement_adresse")) ? "N/A" : reader["complement_adresse"].ToString();
                                string etage = reader.IsDBNull(reader.GetOrdinal("étage")) ? "N/A" : reader["étage"].ToString();
                                string numeroAppartement = reader.IsDBNull(reader.GetOrdinal("numero_appartement")) ? "N/A" : reader["numero_appartement"].ToString();

                                Console.WriteLine($"Adresse #{idAdresse1}:");
                                Console.WriteLine($"  Code porte : {codePorte}");
                                Console.WriteLine($"  Complément : {complementAdresse}");
                                Console.WriteLine($"  Étage : {etage}");
                                Console.WriteLine($"  N° Appartement : {numeroAppartement}");
                                Console.WriteLine(new string('-', 50));
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucune adresse complémentaire trouvée.");
                            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                            Console.ReadKey();
                            return;
                        }
                    }

                    Console.Write("\nEntrez l'ID de l'adresse à supprimer : ");
                    string idAdresse = Console.ReadLine();

                    string queryVerif = "SELECT COUNT(*) FROM ADRESSE WHERE id_adresse = @IdAdresse AND id_client = @IdClient";
                    MySqlCommand cmdVerif = new MySqlCommand(queryVerif, connection);
                    cmdVerif.Parameters.AddWithValue("@IdAdresse", idAdresse);
                    cmdVerif.Parameters.AddWithValue("@IdClient", idClient);

                    int count = Convert.ToInt32(cmdVerif.ExecuteScalar());
                    if (count == 0)
                    {
                        GestionBDD.AfficherMessageErreur("Adresse introuvable ou n'appartenant pas à ce client.");
                        return;
                    }

                    Console.Write("\nÊtes-vous sûr de vouloir supprimer cette adresse ? (O/N) : ");
                    if (Console.ReadLine().ToUpper() != "O")
                    {
                        Console.WriteLine("Suppression annulée.");
                        return;
                    }

                    string queryDelete = "DELETE FROM ADRESSE WHERE id_adresse = @IdAdresse";
                    MySqlCommand cmdDelete = new MySqlCommand(queryDelete, connection);
                    cmdDelete.Parameters.AddWithValue("@IdAdresse", idAdresse);

                    int resultat = cmdDelete.ExecuteNonQuery();

                    if (resultat > 0)
                    {
                        GestionBDD.AfficherMessageSucces("Adresse supprimée avec succès !");
                    }
                    else
                    {
                        GestionBDD.AfficherMessageErreur("Erreur lors de la suppression de l'adresse.");
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        #endregion

        #region Gestion des profils cuisiniers

        /// <summary>
        /// Affiche les détails du profil d'un cuisinier
        /// </summary>
        /// <param name="idCuisinier">ID du cuisinier</param>
        private static void VoirProfilCuisinier(string idCuisinier)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MON PROFIL");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT id_cuisinier, nom_cuisinier, prenom_cuisinier, email_cuisinier, tel_cuisinier, 
                               specialite_cuisinier, dispo_cuisinier, code_postal_cuisinier, rue_cuisinier, 
                               note_moyenne_cuisinier, date_inscription_cuisinier
                        FROM CUISINIER
                        WHERE id_cuisinier = @IdCuisinier";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nom = reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) ? "N/A" : reader["nom_cuisinier"].ToString();
                            string prenom = reader.IsDBNull(reader.GetOrdinal("prenom_cuisinier")) ? "N/A" : reader["prenom_cuisinier"].ToString();
                            string email = reader.IsDBNull(reader.GetOrdinal("email_cuisinier")) ? "N/A" : reader["email_cuisinier"].ToString();
                            string telephone = reader.IsDBNull(reader.GetOrdinal("tel_cuisinier")) ? "N/A" : reader["tel_cuisinier"].ToString();
                            string specialite = reader.IsDBNull(reader.GetOrdinal("specialite_cuisinier")) ? "N/A" : reader["specialite_cuisinier"].ToString();
                            string disponibilite = reader.IsDBNull(reader.GetOrdinal("dispo_cuisinier")) ? "N/A" : reader["dispo_cuisinier"].ToString();
                            string codePostal = reader.IsDBNull(reader.GetOrdinal("code_postal_cuisinier")) ? "N/A" : reader["code_postal_cuisinier"].ToString();
                            string rue = reader.IsDBNull(reader.GetOrdinal("rue_cuisinier")) ? "N/A" : reader["rue_cuisinier"].ToString();
                            string noteMoyenne = reader.IsDBNull(reader.GetOrdinal("note_moyenne_cuisinier")) ? "N/A" : reader["note_moyenne_cuisinier"].ToString();
                            string dateInscription = reader.IsDBNull(reader.GetOrdinal("date_inscription_cuisinier")) ? "N/A" : reader["date_inscription_cuisinier"].ToString();

                            Console.WriteLine($"Nom : {nom}");
                            Console.WriteLine($"Prénom : {prenom}");
                            Console.WriteLine($"Email : {email}");
                            Console.WriteLine($"Téléphone : {telephone}");
                            Console.WriteLine($"Spécialité : {specialite}");
                            Console.WriteLine($"Disponibilité : {disponibilite}");
                            Console.WriteLine($"Adresse : {rue}, {codePostal}, PARIS");
                            Console.WriteLine($"Note moyenne : {noteMoyenne}/10");
                            Console.WriteLine($"Date d'inscription : {dateInscription}");
                        }
                        else
                        {
                            Console.WriteLine("Profil cuisinier non trouvé.");
                        }
                    }
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
        /// Modifie les informations d'un profil cuisinier
        /// </summary>
        /// <param name="idCuisinier">ID du cuisinier</param>
        private static void ModifierProfilCuisinier(string idCuisinier)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MODIFIER MON PROFIL");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Récupérer les informations actuelles
                    string queryInfo = @"
                        SELECT nom_cuisinier, prenom_cuisinier, email_cuisinier, tel_cuisinier, 
                               specialite_cuisinier, dispo_cuisinier, code_postal_cuisinier, rue_cuisinier
                        FROM CUISINIER
                        WHERE id_cuisinier = @IdCuisinier";

                    MySqlCommand cmdInfo = new MySqlCommand(queryInfo, connection);
                    cmdInfo.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                    using (MySqlDataReader reader = cmdInfo.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nomActuel = reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) ? "" : reader["nom_cuisinier"].ToString();
                            string prenomActuel = reader.IsDBNull(reader.GetOrdinal("prenom_cuisinier")) ? "" : reader["prenom_cuisinier"].ToString();
                            string emailActuel = reader.IsDBNull(reader.GetOrdinal("email_cuisinier")) ? "" : reader["email_cuisinier"].ToString();
                            string telephoneActuel = reader.IsDBNull(reader.GetOrdinal("tel_cuisinier")) ? "" : reader["tel_cuisinier"].ToString();
                            string specialiteActuelle = reader.IsDBNull(reader.GetOrdinal("specialite_cuisinier")) ? "" : reader["specialite_cuisinier"].ToString();
                            string disponibiliteActuelle = reader.IsDBNull(reader.GetOrdinal("dispo_cuisinier")) ? "" : reader["dispo_cuisinier"].ToString();
                            string codePostalActuel = reader.IsDBNull(reader.GetOrdinal("code_postal_cuisinier")) ? "" : reader["code_postal_cuisinier"].ToString();
                            string rueActuelle = reader.IsDBNull(reader.GetOrdinal("rue_cuisinier")) ? "" : reader["rue_cuisinier"].ToString();

                            reader.Close();

                            Console.WriteLine("Laissez vide pour conserver la valeur actuelle.");

                            Console.Write($"Nom ({nomActuel}) : ");
                            string nouveauNom = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauNom))
                                nouveauNom = nomActuel;

                            Console.Write($"Prénom ({prenomActuel}) : ");
                            string nouveauPrenom = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauPrenom))
                                nouveauPrenom = prenomActuel;

                            Console.Write($"Email ({emailActuel}) : ");
                            string nouvelEmail = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouvelEmail))
                                nouvelEmail = emailActuel;
                            else if (nouvelEmail != emailActuel && EmailCuisinierExiste(nouvelEmail))
                            {
                                GestionBDD.AfficherMessageErreur("Cet email est déjà utilisé par un autre compte.");
                                return;
                            }

                            Console.Write($"Téléphone ({telephoneActuel}) : ");
                            string nouveauTelephone = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauTelephone))
                                nouveauTelephone = telephoneActuel;

                            Console.Write($"Spécialité ({specialiteActuelle}) : ");
                            string nouvelleSpecialite = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouvelleSpecialite))
                                nouvelleSpecialite = specialiteActuelle;

                            Console.Write($"Disponibilité ({disponibiliteActuelle}) : ");
                            string nouvelleDisponibilite = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouvelleDisponibilite))
                                nouvelleDisponibilite = disponibiliteActuelle;

                            Console.Write($"Code postal ({codePostalActuel}) : ");
                            string nouveauCodePostal = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauCodePostal))
                                nouveauCodePostal = codePostalActuel;

                            Console.Write($"Rue ({rueActuelle}) : ");
                            string nouvelleRue = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouvelleRue))
                                nouvelleRue = rueActuelle;

                            // Mettre à jour le profil
                            string queryUpdate = @"
                                UPDATE CUISINIER
                                SET nom_cuisinier = @Nom,
                                    prenom_cuisinier = @Prenom,
                                    email_cuisinier = @Email,
                                    tel_cuisinier = @Tel,
                                    specialite_cuisinier = @Specialite,
                                    dispo_cuisinier = @Disponibilite,
                                    code_postal_cuisinier = @CodePostal,
                                    rue_cuisinier = @Rue
                                WHERE id_cuisinier = @IdCuisinier";

                            MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, connection);
                            cmdUpdate.Parameters.AddWithValue("@IdCuisinier", idCuisinier);
                            cmdUpdate.Parameters.AddWithValue("@Nom", nouveauNom);
                            cmdUpdate.Parameters.AddWithValue("@Prenom", nouveauPrenom);
                            cmdUpdate.Parameters.AddWithValue("@Email", nouvelEmail);
                            cmdUpdate.Parameters.AddWithValue("@Tel", nouveauTelephone);
                            cmdUpdate.Parameters.AddWithValue("@Specialite", nouvelleSpecialite);
                            cmdUpdate.Parameters.AddWithValue("@Disponibilite", nouvelleDisponibilite);
                            cmdUpdate.Parameters.AddWithValue("@CodePostal", nouveauCodePostal);
                            cmdUpdate.Parameters.AddWithValue("@Rue", nouvelleRue);

                            int resultat = cmdUpdate.ExecuteNonQuery();

                            if (resultat > 0)
                            {
                                GestionBDD.AfficherMessageSucces("Profil modifié avec succès !");

                                // Mettre à jour la session si l'email a changé
                                if (nouvelEmail != emailActuel)
                                {
                                    SessionUtilisateur.Connecter(idCuisinier, nouveauNom, nouveauPrenom, "cuisinier", nouvelEmail);
                                }
                            }
                            else
                            {
                                GestionBDD.AfficherMessageErreur("Erreur lors de la modification du profil.");
                            }
                        }
                        else
                        {
                            reader.Close();
                            GestionBDD.AfficherMessageErreur("Profil cuisinier non trouvé.");
                        }
                    }
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
        /// Affiche les évaluations d'un cuisinier
        /// </summary>
        /// <param name="idCuisinier">ID du cuisinier</param>
        private static void VoirEvaluationsCuisinier(string idCuisinier)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MES ÉVALUATIONS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Récupérer la note moyenne
                    string queryMoyenne = "SELECT note_moyenne_cuisinier FROM CUISINIER WHERE id_cuisinier = @IdCuisinier";
                    MySqlCommand cmdMoyenne = new MySqlCommand(queryMoyenne, connection);
                    cmdMoyenne.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                    object noteMoyenne = cmdMoyenne.ExecuteScalar();
                    if (noteMoyenne != null && !Convert.IsDBNull(noteMoyenne))
                    {
                        Console.WriteLine($"Votre note moyenne : {noteMoyenne}/10\n");
                    }
                    else
                    {
                        Console.WriteLine("Vous n'avez pas encore reçu d'évaluations.\n");
                    }

                    // Récupérer les évaluations
                    string query = @"
                         SELECT e.id_evaluation, e.note_evaluation, e.commentaire_evaluation, e.date_evaluation,
                                c.nom_client, c.prenom_client
                         FROM EVALUATION e
                         JOIN effectue ef ON e.id_evaluation = ef.id_evaluation
                         JOIN CLIENT c ON ef.id_client = c.id_client
                         JOIN reçoit r ON e.id_evaluation = r.id_evaluation
                         WHERE r.id_cuisinier = @IdCuisinier
                         ORDER BY e.date_evaluation DESC";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("Vos évaluations :");
                            Console.WriteLine(new string('-', 80));

                            while (reader.Read())
                            {
                                string idEvaluation = reader["id_evaluation"].ToString();
                                string note = reader.IsDBNull(reader.GetOrdinal("note_evaluation")) ? "N/A" : reader["note_evaluation"].ToString();
                                string commentaire = reader.IsDBNull(reader.GetOrdinal("commentaire_evaluation")) ? "N/A" : reader["commentaire_evaluation"].ToString();
                                string dateEvaluation = reader.IsDBNull(reader.GetOrdinal("date_evaluation")) ? "N/A" : reader["date_evaluation"].ToString();
                                string nomClient = reader.IsDBNull(reader.GetOrdinal("nom_client")) ? "N/A" : reader["nom_client"].ToString();
                                string prenomClient = reader.IsDBNull(reader.GetOrdinal("prenom_client")) ? "N/A" : reader["prenom_client"].ToString();

                                Console.WriteLine($"Évaluation #{idEvaluation} - {dateEvaluation}");
                                Console.WriteLine($"Client : {prenomClient} {nomClient}");
                                Console.WriteLine($"Note : {note}/10");
                                Console.WriteLine($"Commentaire : {commentaire}");
                                Console.WriteLine(new string('-', 80));
                            }
                        }
                        else
                        {
                            Console.WriteLine("Vous n'avez pas encore reçu d'évaluations détaillées.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        #endregion

        #region Gestion des comptes (admin)

        /// <summary>
        /// Gère les comptes clients (admin)
        /// </summary>
        public static void GererComptesClients()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("GESTION DES COMPTES CLIENTS");

                Console.WriteLine("1. Lister tous les clients");
                Console.WriteLine("2. Rechercher un client");
                Console.WriteLine("3. Voir les détails d'un client");
                Console.WriteLine("4. Modifier un client");
                Console.WriteLine("5. Supprimer un client");
                Console.WriteLine("6. Retour");
                Console.Write("\nChoisissez une option : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        ListerClients();
                        break;
                    case "2":
                        RechercherClient();
                        break;
                    case "3":
                        VoirDetailsClient();
                        break;
                    case "4":
                        ModifierClient();
                        break;
                    case "5":
                        SupprimerClient();
                        break;
                    case "6":
                        continuer = false;
                        break;
                    default:
                        GestionBDD.AfficherMessageErreur("Option invalide.");
                        break;
                }
            }
        }

        /// <summary>
        /// Liste tous les clients
        /// </summary>
        private static void ListerClients()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("LISTE DES CLIENTS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Comment souhaitez-vous trier les clients ? Ecrivez 'nom', 'rue' ou 'montant'.");
                    string critere = Console.ReadLine().Trim();
                    string orderBy = "";
                    switch (critere.ToLower())
                    {
                        case "nom": orderBy = "cl.nom_client, cl.prenom_client"; break;
                        case "rue": orderBy = "cl.rue_client"; break;
                        case "montant": orderBy = "total_achats DESC"; break;
                        default: orderBy = "cl.nom_client, cl.prenom_client"; break;
                    }

                    string requete = $@"
        SELECT cl.id_client, cl.nom_client, cl.prenom_client, cl.email_client, cl.rue_client,
               SUM(CAST(REPLACE(REPLACE(c.prix_total_commande, ' euros', ''), ',', '.') AS DECIMAL(10,2))) AS total_achats
        FROM CLIENT cl
        LEFT JOIN COMMANDE c ON cl.id_client = c.id_client
        GROUP BY cl.id_client, cl.nom_client, cl.prenom_client, cl.email_client, cl.rue_client
        ORDER BY {orderBy}";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-25} {4,-15} {5,-15} {6,-20}",
                                "ID", "Nom", "Prénom", "Email", "Téléphone", "Type", "Date d'inscription");
                            Console.WriteLine(new string('-', 110));

                            while (reader.Read())
                            {
                                string id = reader["id_client"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom_client")) ? "N/A" : reader["nom_client"].ToString();
                                string prenom = reader.IsDBNull(reader.GetOrdinal("prenom_client")) ? "N/A" : reader["prenom_client"].ToString();
                                string email = reader.IsDBNull(reader.GetOrdinal("email_client")) ? "N/A" : reader["email_client"].ToString();
                                string telephone = reader.IsDBNull(reader.GetOrdinal("tel_client")) ? "N/A" : reader["tel_client"].ToString();
                                string type = reader.IsDBNull(reader.GetOrdinal("type_client")) ? "N/A" : reader["type_client"].ToString();
                                string dateInscription = reader.IsDBNull(reader.GetOrdinal("date_inscription_client")) ? "N/A" : reader["date_inscription_client"].ToString();

                                Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-25} {4,-15} {5,-15} {6,-20}",
                                    id, nom, prenom, email, telephone, type, dateInscription);
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
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        /// <summary>
        /// Recherche un client par nom, prénom ou email
        /// </summary>
        private static void RechercherClient()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("RECHERCHER UN CLIENT");

            Console.WriteLine("Rechercher par :");
            Console.WriteLine("1. Nom");
            Console.WriteLine("2. Prénom");
            Console.WriteLine("3. Email");
            Console.Write("\nVotre choix : ");
            string choixRecherche = Console.ReadLine();

            string colonne = "";
            switch (choixRecherche)
            {
                case "1":
                    colonne = "nom_client";
                    break;
                case "2":
                    colonne = "prenom_client";
                    break;
                case "3":
                    colonne = "email_client";
                    break;
                default:
                    GestionBDD.AfficherMessageErreur("Option invalide.");
                    return;
            }

            Console.Write("Entrez votre recherche : ");
            string recherche = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = $@"
                        SELECT id_client, nom_client, prenom_client, email_client, tel_client, type_client
                        FROM CLIENT
                        WHERE {colonne} LIKE @Recherche
                        ORDER BY nom_client, prenom_client";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Recherche", $"%{recherche}%");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("\nRésultats de la recherche :");
                            Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-25} {4,-15} {5,-15}",
                                "ID", "Nom", "Prénom", "Email", "Téléphone", "Type");
                            Console.WriteLine(new string('-', 90));

                            while (reader.Read())
                            {
                                string id = reader["id_client"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom_client")) ? "N/A" : reader["nom_client"].ToString();
                                string prenom = reader.IsDBNull(reader.GetOrdinal("prenom_client")) ? "N/A" : reader["prenom_client"].ToString();
                                string email = reader.IsDBNull(reader.GetOrdinal("email_client")) ? "N/A" : reader["email_client"].ToString();
                                string telephone = reader.IsDBNull(reader.GetOrdinal("tel_client")) ? "N/A" : reader["tel_client"].ToString();
                                string type = reader.IsDBNull(reader.GetOrdinal("type_client")) ? "N/A" : reader["type_client"].ToString();

                                Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-25} {4,-15} {5,-15}",
                                    id, nom, prenom, email, telephone, type);
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nAucun client trouvé correspondant à votre recherche.");
                        }
                    }
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
        /// Affiche les détails d'un client
        /// </summary>
        private static void VoirDetailsClient()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("DÉTAILS D'UN CLIENT");

            Console.Write("Entrez l'ID du client : ");
            string idClient = Console.ReadLine();

            VoirProfilClient(idClient);
        }

        /// <summary>
        /// Modifie un client
        /// </summary>
        private static void ModifierClient()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MODIFIER UN CLIENT");

            Console.Write("Entrez l'ID du client à modifier : ");
            string idClient = Console.ReadLine();

            ModifierProfilClient(idClient);
        }

        /// <summary>
        /// Supprime un client
        /// </summary>
        private static void SupprimerClient()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("SUPPRIMER UN CLIENT");

            Console.Write("Entrez l'ID du client à supprimer : ");
            string idClient = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string queryVerif = "SELECT nom_client, prenom_client FROM CLIENT WHERE id_client = @IdClient";
                    MySqlCommand cmdVerif = new MySqlCommand(queryVerif, connection);
                    cmdVerif.Parameters.AddWithValue("@IdClient", idClient);

                    using (MySqlDataReader reader = cmdVerif.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            GestionBDD.AfficherMessageErreur("Client introuvable.");
                            return;
                        }

                        string nom = reader.IsDBNull(reader.GetOrdinal("nom_client")) ? "N/A" : reader["nom_client"].ToString();
                        string prenom = reader.IsDBNull(reader.GetOrdinal("prenom_client")) ? "N/A" : reader["prenom_client"].ToString();
                        reader.Close();

                        Console.WriteLine($"\nVous êtes sur le point de supprimer le client : {prenom} {nom}");
                        Console.Write("Êtes-vous sûr ? (O/N) : ");
                        if (Console.ReadLine().ToUpper() != "O")
                        {
                            Console.WriteLine("Suppression annulée.");
                            return;
                        }

                        string queryAdresses = "DELETE FROM ADRESSE WHERE id_client = @IdClient";
                        MySqlCommand cmdAdresses = new MySqlCommand(queryAdresses, connection);
                        cmdAdresses.Parameters.AddWithValue("@IdClient", idClient);
                        cmdAdresses.ExecuteNonQuery();


                        /// Vérifier si le client a des commandes
                        string queryVerifCommandes = "SELECT COUNT(*) FROM COMMANDE WHERE id_client = @IdClient";
                        MySqlCommand cmdVerifCommandes = new MySqlCommand(queryVerifCommandes, connection);
                        cmdVerifCommandes.Parameters.AddWithValue("@IdClient", idClient);
                        int nbCommandes = Convert.ToInt32(cmdVerifCommandes.ExecuteScalar());

                        if (nbCommandes > 0)
                        {
                            Console.WriteLine($"\nAttention : Ce client a {nbCommandes} commande(s).");
                            Console.Write("Voulez-vous supprimer également toutes ses commandes ? (O/N) : ");
                            if (Console.ReadLine().ToUpper() == "O")
                            {
                                /// Récupérer les IDs des commandes
                                string queryCommandes = "SELECT id_commande FROM COMMANDE WHERE id_client = @IdClient";
                                MySqlCommand cmdCommandes = new MySqlCommand(queryCommandes, connection);
                                cmdCommandes.Parameters.AddWithValue("@IdClient", idClient);

                                List<string> idsCommandes = new List<string>();
                                using (MySqlDataReader readerCommandes = cmdCommandes.ExecuteReader())
                                {
                                    while (readerCommandes.Read())
                                    {
                                        idsCommandes.Add(readerCommandes["id_commande"].ToString());
                                    }
                                }

                                /// Supprimer les lignes de commande pour chaque commande
                                foreach (string idCommande in idsCommandes)
                                {
                                    string queryLignes = "DELETE FROM LIGNE_COMMANDE WHERE id_commande = @IdCommande";
                                    MySqlCommand cmdLignes = new MySqlCommand(queryLignes, connection);
                                    cmdLignes.Parameters.AddWithValue("@IdCommande", idCommande);
                                    cmdLignes.ExecuteNonQuery();
                                }

                                /// Supprimer les commandes
                                string queryDeleteCommandes = "DELETE FROM COMMANDE WHERE id_client = @IdClient";
                                MySqlCommand cmdDeleteCommandes = new MySqlCommand(queryDeleteCommandes, connection);
                                cmdDeleteCommandes.Parameters.AddWithValue("@IdClient", idClient);
                                cmdDeleteCommandes.ExecuteNonQuery();
                            }
                            else
                            {
                                Console.WriteLine("Impossible de supprimer le client sans supprimer ses commandes.");
                                return;
                            }
                        }

                        /// Supprimer le client
                        string queryClient = "DELETE FROM CLIENT WHERE id_client = @IdClient";
                        MySqlCommand cmdClient = new MySqlCommand(queryClient, connection);
                        cmdClient.Parameters.AddWithValue("@IdClient", idClient);
                        int resultat = cmdClient.ExecuteNonQuery();

                        if (resultat > 0)
                        {
                            GestionBDD.AfficherMessageSucces("Client supprimé avec succès !");
                        }
                        else
                        {
                            GestionBDD.AfficherMessageErreur("Erreur lors de la suppression du client.");
                        }
                    }
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
        /// Gère les comptes cuisiniers (admin)
        /// </summary>
        public static void GererComptesCuisiniers()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("GESTION DES COMPTES CUISINIERS");

                Console.WriteLine("1. Lister tous les cuisiniers");
                Console.WriteLine("2. Rechercher un cuisinier");
                Console.WriteLine("3. Ajouter un nouveau cuisinier");
                Console.WriteLine("4. Modifier un cuisinier existant");
                Console.WriteLine("5. Supprimer un cuisinier existant");
                Console.WriteLine("6. Voir les détails d'un cuisinier");
                Console.WriteLine("7. Voir les plats par cuisinier");
                Console.WriteLine("8. Voir les commandes traitées par cuisinier");
                Console.WriteLine("9. Gérer les évaluations des cuisiniers");
                Console.WriteLine("10. Retour au menu précédent");
                Console.Write("\nChoisissez une option : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        ListerCuisiniers();
                        break;
                    case "2":
                        RechercherCuisinier();
                        break;
                    case "3":
                        AjouterCuisinier();
                        break;
                    case "4":
                        ModifierCuisinier();
                        break;
                    case "5":
                        SupprimerCuisinier();
                        break;
                    case "6":
                        VoirDetailsCuisinier();
                        break;
                    case "7":
                        VoirPlatsCuisinier();
                        break;
                    case "8":
                        VoirCommandesCuisinier();
                        break;
                    case "9":
                        GererEvaluationsCuisinier();
                        break;
                    case "10":
                        continuer = false;
                        break;
                    default:
                        GestionBDD.AfficherMessageErreur("Option invalide.");
                        break;
                }
            }
        }


        /// <summary>
        /// Liste tous les cuisiniers
        /// </summary>
        private static void ListerCuisiniers()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("LISTE DES CUISINIERS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT id_cuisinier, nom_cuisinier, prenom_cuisinier, email_cuisinier, tel_cuisinier, 
                               specialite_cuisinier, note_moyenne_cuisinier, date_inscription_cuisinier
                        FROM CUISINIER
                        ORDER BY nom_cuisinier, prenom_cuisinier";

                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-25} {4,-15} {5,-15} {6,-10} {7,-20}",
                                "ID", "Nom", "Prénom", "Email", "Téléphone", "Spécialité", "Note", "Date d'inscription");
                            Console.WriteLine(new string('-', 120));

                            while (reader.Read())
                            {
                                string id = reader["id_cuisinier"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) ? "N/A" : reader["nom_cuisinier"].ToString();
                                string prenom = reader.IsDBNull(reader.GetOrdinal("prenom_cuisinier")) ? "N/A" : reader["prenom_cuisinier"].ToString();
                                string email = reader.IsDBNull(reader.GetOrdinal("email_cuisinier")) ? "N/A" : reader["email_cuisinier"].ToString();
                                string telephone = reader.IsDBNull(reader.GetOrdinal("tel_cuisinier")) ? "N/A" : reader["tel_cuisinier"].ToString();
                                string specialite = reader.IsDBNull(reader.GetOrdinal("specialite_cuisinier")) ? "N/A" : reader["specialite_cuisinier"].ToString();
                                string note = reader.IsDBNull(reader.GetOrdinal("note_moyenne_cuisinier")) ? "N/A" : reader["note_moyenne_cuisinier"].ToString();
                                string dateInscription = reader.IsDBNull(reader.GetOrdinal("date_inscription_cuisinier")) ? "N/A" : reader["date_inscription_cuisinier"].ToString();

                                Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-25} {4,-15} {5,-15} {6,-10} {7,-20}",
                                    id, nom, prenom, email, telephone, specialite, note, dateInscription);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun cuisinier trouvé.");
                        }
                    }
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
        /// Recherche un cuisinier par nom, prénom ou email
        /// </summary>
        private static void RechercherCuisinier()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("RECHERCHER UN CUISINIER");

            Console.WriteLine("Rechercher par :");
            Console.WriteLine("1. Nom");
            Console.WriteLine("2. Prénom");
            Console.WriteLine("3. Email");
            Console.WriteLine("4. Spécialité");
            Console.Write("\nVotre choix : ");
            string choixRecherche = Console.ReadLine();

            string colonne = "";
            switch (choixRecherche)
            {
                case "1":
                    colonne = "nom_cuisinier";
                    break;
                case "2":
                    colonne = "prenom_cuisinier";
                    break;
                case "3":
                    colonne = "email_cuisinier";
                    break;
                case "4":
                    colonne = "specialite_cuisinier";
                    break;
                default:
                    GestionBDD.AfficherMessageErreur("Option invalide.");
                    return;
            }

            Console.Write("Entrez votre recherche : ");
            string recherche = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = $@"
                        SELECT id_cuisinier, nom_cuisinier, prenom_cuisinier, email_cuisinier, tel_cuisinier, 
                               specialite_cuisinier, note_moyenne_cuisinier
                        FROM CUISINIER
                        WHERE {colonne} LIKE @Recherche
                        ORDER BY nom_cuisinier, prenom_cuisinier";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Recherche", $"%{recherche}%");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("\nRésultats de la recherche :");
                            Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-25} {4,-15} {5,-15} {6,-10}",
                                "ID", "Nom", "Prénom", "Email", "Téléphone", "Spécialité", "Note");
                            Console.WriteLine(new string('-', 100));

                            while (reader.Read())
                            {
                                string id = reader["id_cuisinier"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) ? "N/A" : reader["nom_cuisinier"].ToString();
                                string prenom = reader.IsDBNull(reader.GetOrdinal("prenom_cuisinier")) ? "N/A" : reader["prenom_cuisinier"].ToString();
                                string email = reader.IsDBNull(reader.GetOrdinal("email_cuisinier")) ? "N/A" : reader["email_cuisinier"].ToString();
                                string telephone = reader.IsDBNull(reader.GetOrdinal("tel_cuisinier")) ? "N/A" : reader["tel_cuisinier"].ToString();
                                string specialite = reader.IsDBNull(reader.GetOrdinal("specialite_cuisinier")) ? "N/A" : reader["specialite_cuisinier"].ToString();
                                string note = reader.IsDBNull(reader.GetOrdinal("note_moyenne_cuisinier")) ? "N/A" : reader["note_moyenne_cuisinier"].ToString();

                                Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-25} {4,-15} {5,-15} {6,-10}",
                                    id, nom, prenom, email, telephone, specialite, note);
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nAucun cuisinier trouvé correspondant à votre recherche.");
                        }
                    }
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
        /// Ajoute un nouveau cuisinier dans la base de données
        /// </summary>
        private static void AjouterCuisinier()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("AJOUTER UN NOUVEAU CUISINIER");

            // Générer un nouvel ID
            int idCuisinier = GestionBDD.GenererNouvelID("CUISINIER", "id_cuisinier");

            Console.Write("Nom : ");
            string nom = Console.ReadLine();

            Console.Write("Prénom : ");
            string prenom = Console.ReadLine();

            Console.Write("Email : ");
            string email = Console.ReadLine();

            if (EmailCuisinierExiste(email))
            {
                GestionBDD.AfficherMessageErreur("Un cuisinier avec cet email existe déjà.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            Console.Write("Mot de passe : ");
            string motDePasse = Console.ReadLine();

            Console.Write("Téléphone : ");
            string telephone = Console.ReadLine();

            Console.Write("Spécialité : ");
            string specialite = Console.ReadLine();

            Console.Write("Disponibilité : ");
            string disponibilite = Console.ReadLine();

            Console.Write("Note moyenne initiale (0-10, laisser vide pour 0) : ");
            string noteInput = Console.ReadLine();
            decimal noteMoyenne = 0;
            if (!string.IsNullOrWhiteSpace(noteInput) && decimal.TryParse(noteInput, out decimal parsedNote))
            {
                noteMoyenne = Math.Max(0, Math.Min(10, parsedNote)); /// Limiter entre 0 et 10
            }

            try
            {
                Dictionary<string, object> parametres = new Dictionary<string, object>
                {
                    { "@Id", idCuisinier },
                    { "@Nom", nom },
                    { "@Prenom", prenom },
                    { "@Email", email },
                    { "@MotDePasse", motDePasse },
                    { "@Telephone", telephone },
                    { "@Specialite", specialite },
                    { "@Disponibilite", disponibilite },
                    { "@NoteMoyenne", noteMoyenne }
                };

                string query = "INSERT INTO CUISINIER (id_cuisinier, nom_cuisinier, prenom_cuisinier, email_cuisinier, " +
                              "mot_de_passe_cuisinier, tel_cuisinier, specialite_cuisinier, dispo_cuisinier, note_moyenne_cuisinier) " +
                              "VALUES (@Id, @Nom, @Prenom, @Email, @MotDePasse, @Telephone, @Specialite, @Disponibilite, @NoteMoyenne)";

                int result = GestionBDD.ExecuterRequete(query, parametres);

                if (result > 0)
                {
                    GestionBDD.AfficherMessageSucces("Cuisinier ajouté avec succès !");
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Échec de l'ajout du cuisinier.");
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
        /// Modifie un cuisinier
        /// </summary>
        private static void ModifierCuisinier()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MODIFIER UN CUISINIER");

            Console.Write("Entrez l'ID du cuisinier à modifier : ");
            string idCuisinier = Console.ReadLine();

            ModifierProfilCuisinier(idCuisinier);
        }

        /// <summary>
        /// Supprime un cuisinier
        /// </summary>
        private static void SupprimerCuisinier()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("SUPPRIMER UN CUISINIER");

            var cuisiniers = GestionBDD.ObtenirListeCuisiniersDetails();
            if (cuisiniers.Count == 0)
            {
                GestionBDD.AfficherMessageErreur("Aucun cuisinier trouvé.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Liste des cuisiniers :");
            foreach (var cuisinier in cuisiniers)
            {
                Console.WriteLine($"{cuisinier.id}. {cuisinier.prenom} {cuisinier.nom} ({cuisinier.email})");
            }

            Console.Write("\nEntrez l'ID du cuisinier à supprimer (ou 0 pour annuler) : ");
            string idCuisinierStr = Console.ReadLine();

            if (idCuisinierStr == "0")
            {
                return;
            }

            var cuisiDetails = GestionBDD.ObtenirDetailsCuisinier(idCuisinierStr);
            if (string.IsNullOrEmpty(cuisiDetails.id))
            {
                GestionBDD.AfficherMessageErreur("Cuisinier introuvable.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            Console.Write($"Êtes-vous sûr de vouloir supprimer le cuisinier {cuisiDetails.prenom} {cuisiDetails.nom} ? (O/N) : ");
            string confirmation = Console.ReadLine().ToUpper();

            if (confirmation != "O")
            {
                return;
            }

            try
            {
                string queryVerifPlats = "SELECT COUNT(*) FROM PLAT WHERE id_cuisinier = @IdCuisinier";
                Dictionary<string, object> parametresVerifPlats = new Dictionary<string, object>
        {
            { "@IdCuisinier", idCuisinierStr }
        };
                int nbPlats = Convert.ToInt32(GestionBDD.ExecuterScalaire(queryVerifPlats, parametresVerifPlats));

                if (nbPlats > 0)
                {
                    Console.Write($"Le cuisinier a {nbPlats} plats. Voulez-vous quand même le supprimer ? (O/N) : ");
                    string confirmationPlats = Console.ReadLine().ToUpper();

                    if (confirmationPlats != "O")
                    {
                        return;
                    }

                    string queryPlats = "DELETE FROM PLAT WHERE id_cuisinier = @IdCuisinier";
                    Dictionary<string, object> parametresPlats = new Dictionary<string, object>
            {
                { "@IdCuisinier", idCuisinierStr }
            };
                    GestionBDD.ExecuterRequete(queryPlats, parametresPlats);
                }

                string queryVerifEvaluations = @"
            SELECT COUNT(*) 
            FROM reçoit 
            WHERE id_cuisinier = @IdCuisinier";

                Dictionary<string, object> parametresVerifEvaluations = new Dictionary<string, object>
        {
            { "@IdCuisinier", idCuisinierStr }
        };

                int nbEvaluations = Convert.ToInt32(GestionBDD.ExecuterScalaire(queryVerifEvaluations, parametresVerifEvaluations));

                if (nbEvaluations > 0)
                {
                    string queryRelationsEvaluations = @"
                DELETE FROM reçoit 
                WHERE id_cuisinier = @IdCuisinier";

                    Dictionary<string, object> parametresRelationsEvaluations = new Dictionary<string, object>
            {
                { "@IdCuisinier", idCuisinierStr }
            };

                    GestionBDD.ExecuterRequete(queryRelationsEvaluations, parametresRelationsEvaluations);

                    string queryEvaluationsOrphelines = @"
                DELETE FROM EVALUATION 
                WHERE id_evaluation NOT IN (SELECT id_evaluation FROM reçoit)";

                    GestionBDD.ExecuterRequete(queryEvaluationsOrphelines, null);
                }

                /// Vérifier si le cuisinier est assigné à des lignes de commande
                string queryVerifLignesCommande = "SELECT COUNT(*) FROM LIGNE_COMMANDE WHERE id_cuisinier = @IdCuisinier";
                Dictionary<string, object> parametresVerifLignesCommande = new Dictionary<string, object>
        {
            { "@IdCuisinier", idCuisinierStr }
        };
                int nbLignesCommande = Convert.ToInt32(GestionBDD.ExecuterScalaire(queryVerifLignesCommande, parametresVerifLignesCommande));

                if (nbLignesCommande > 0)
                {
                    Console.Write($"Le cuisinier est assigné à {nbLignesCommande} lignes de commande. Voulez-vous quand même le supprimer ? (O/N) : ");
                    string confirmationLignes = Console.ReadLine().ToUpper();

                    if (confirmationLignes != "O")
                    {
                        return;
                    }

                    /// Mettre à NULL l'id_cuisinier dans les lignes de commande
                    string queryLignesCommande = "UPDATE LIGNE_COMMANDE SET id_cuisinier = NULL WHERE id_cuisinier = @IdCuisinier";
                    Dictionary<string, object> parametresLignesCommande = new Dictionary<string, object>
            {
                { "@IdCuisinier", idCuisinierStr }
            };
                    GestionBDD.ExecuterRequete(queryLignesCommande, parametresLignesCommande);
                }

                /// Supprimer le cuisinier
                string queryCuisinier = "DELETE FROM CUISINIER WHERE id_cuisinier = @IdCuisinier";
                Dictionary<string, object> parametresCuisinier = new Dictionary<string, object>
        {
            { "@IdCuisinier", idCuisinierStr }
        };
                int resultat = GestionBDD.ExecuterRequete(queryCuisinier, parametresCuisinier);

                if (resultat > 0)
                {
                    GestionBDD.AfficherMessageSucces("Cuisinier supprimé avec succès !");
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Échec de la suppression du cuisinier.");
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
        /// Affiche les détails d'un cuisinier
        /// </summary>
        private static void VoirDetailsCuisinier()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("DÉTAILS D'UN CUISINIER");

            Console.Write("Entrez l'ID du cuisinier : ");
            string idCuisinier = Console.ReadLine();

            VoirProfilCuisinier(idCuisinier);
        }

        /// <summary>
        /// Affiche les plats préparés par un cuisinier spécifique
        /// </summary>
        private static void VoirPlatsCuisinier()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("PLATS PAR CUISINIER");

            var cuisiniers = GestionBDD.ObtenirListeCuisiniersDetails();
            if (cuisiniers.Count == 0)
            {
                GestionBDD.AfficherMessageErreur("Aucun cuisinier trouvé.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Liste des cuisiniers :");
            foreach (var cuisinier in cuisiniers)
            {
                Console.WriteLine($"{cuisinier.id}. {cuisinier.prenom} {cuisinier.nom} ({cuisinier.email})");
            }

            Console.Write("\nEntrez l'ID du cuisinier (ou 0 pour annuler) : ");
            string idCuisinierStr = Console.ReadLine();

            if (idCuisinierStr == "0")
            {
                return;
            }

            var cuisiDetails = GestionBDD.ObtenirDetailsCuisinier(idCuisinierStr);
            if (string.IsNullOrEmpty(cuisiDetails.id))
            {
                GestionBDD.AfficherMessageErreur("Cuisinier introuvable.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            try
            {
                Console.WriteLine($"\nPlats préparés par : {cuisiDetails.prenom} {cuisiDetails.nom}");

                string requete = @"
                    SELECT p.id_plat, p.nom_plat, p.type_plat, p.prix_unitaire_plat, p.regime_alimentaire_plat, 
                           p.nationalite_plat, COUNT(lc.id_ligne) AS nombre_commandes
                    FROM PLAT p
                    LEFT JOIN LIGNE_COMMANDE lc ON p.id_plat = lc.id_plat
                    WHERE p.id_cuisinier = @IdCuisinier
                    GROUP BY p.id_plat, p.nom_plat, p.type_plat, p.prix_unitaire_plat, p.regime_alimentaire_plat, p.nationalite_plat
                    ORDER BY nombre_commandes DESC";

                Dictionary<string, object> parametres = new Dictionary<string, object>
                {
                    { "@IdCuisinier", idCuisinierStr }
                };

                DataTable plats = GestionBDD.ExecuterRequeteTable(requete, parametres);

                if (plats != null && plats.Rows.Count > 0)
                {
                    Console.WriteLine("\nID\tNom\t\tType\t\tPrix\t\tRégime\t\tNationalité\tNombre de commandes");
                    Console.WriteLine("-------------------------------------------------------------------------");

                    foreach (DataRow row in plats.Rows)
                    {
                        string idPlat = row["id_plat"].ToString();
                        string nomPlat = row["nom_plat"] == DBNull.Value ? "N/A" : row["nom_plat"].ToString();
                        string typePlat = row["type_plat"] == DBNull.Value ? "N/A" : row["type_plat"].ToString();
                        string prixPlat = row["prix_unitaire_plat"] == DBNull.Value ? "N/A" : row["prix_unitaire_plat"].ToString();
                        string regimePlat = row["regime_alimentaire_plat"] == DBNull.Value ? "N/A" : row["regime_alimentaire_plat"].ToString();
                        string nationalitePlat = row["nationalite_plat"] == DBNull.Value ? "N/A" : row["nationalite_plat"].ToString();
                        int nombreCommandes = row["nombre_commandes"] == DBNull.Value ? 0 : Convert.ToInt32(row["nombre_commandes"]);

                        Console.WriteLine($"{idPlat}\t{nomPlat,-15}{typePlat,-15}{prixPlat,-15}{regimePlat,-15}{nationalitePlat,-15}{nombreCommandes}");
                    }
                }
                else
                {
                    Console.WriteLine("Aucun plat trouvé pour ce cuisinier.");
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
        /// Affiche les commandes traitées par un cuisinier spécifique
        /// </summary>
        private static void VoirCommandesCuisinier()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("COMMANDES PAR CUISINIER");

            var cuisiniers = GestionBDD.ObtenirListeCuisiniersDetails();
            if (cuisiniers.Count == 0)
            {
                GestionBDD.AfficherMessageErreur("Aucun cuisinier trouvé.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Liste des cuisiniers :");
            foreach (var cuisinier in cuisiniers)
            {
                Console.WriteLine($"{cuisinier.id}. {cuisinier.prenom} {cuisinier.nom} ({cuisinier.email})");
            }

            Console.Write("\nEntrez l'ID du cuisinier (ou 0 pour annuler) : ");
            string idCuisinierStr = Console.ReadLine();

            if (idCuisinierStr == "0")
            {
                return;
            }

            var cuisiDetails = GestionBDD.ObtenirDetailsCuisinier(idCuisinierStr);
            if (string.IsNullOrEmpty(cuisiDetails.id))
            {
                GestionBDD.AfficherMessageErreur("Cuisinier introuvable.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            try
            {
                Console.WriteLine($"\nCommandes traitées par : {cuisiDetails.prenom} {cuisiDetails.nom}");

                string requete = @"
                    SELECT DISTINCT c.id_commande, c.date_commande, c.prix_total_commande, c.statut_commande,
                           cl.nom_client, cl.prenom_client, COUNT(lc.id_ligne) AS nombre_plats
                    FROM COMMANDE c
                    JOIN CLIENT cl ON c.id_client = cl.id_client
                    JOIN LIGNE_COMMANDE lc ON c.id_commande = lc.id_commande
                    JOIN PLAT p ON lc.id_plat = p.id_plat
                    WHERE p.id_cuisinier = @IdCuisinier
                    GROUP BY c.id_commande, c.date_commande, c.prix_total_commande, c.statut_commande, cl.nom_client, cl.prenom_client
                    ORDER BY c.date_commande DESC";

                Dictionary<string, object> parametres = new Dictionary<string, object>
                {
                    { "@IdCuisinier", idCuisinierStr }
                };

                DataTable commandes = GestionBDD.ExecuterRequeteTable(requete, parametres);

                if (commandes != null && commandes.Rows.Count > 0)
                {
                    Console.WriteLine("\nID\tDate\t\tPrix\t\tStatut\t\tClient\t\tNombre de plats");
                    Console.WriteLine("-------------------------------------------------------------------------");

                    foreach (DataRow row in commandes.Rows)
                    {
                        string idCommande = row["id_commande"].ToString();
                        string dateCommande = row["date_commande"] == DBNull.Value ? "N/A" : Convert.ToDateTime(row["date_commande"]).ToString("dd/MM/yyyy HH:mm");
                        string prixCommande = row["prix_total_commande"] == DBNull.Value ? "N/A" : row["prix_total_commande"].ToString();
                        string statutCommande = row["statut_commande"] == DBNull.Value ? "N/A" : row["statut_commande"].ToString();
                        string nomClient = row["nom_client"] == DBNull.Value ? "N/A" : row["nom_client"].ToString();
                        string prenomClient = row["prenom_client"] == DBNull.Value ? "N/A" : row["prenom_client"].ToString();
                        string client = $"{prenomClient} {nomClient}";
                        int nombrePlats = row["nombre_plats"] == DBNull.Value ? 0 : Convert.ToInt32(row["nombre_plats"]);

                        Console.WriteLine($"{idCommande}\t{dateCommande}\t{prixCommande}\t{statutCommande,-15}{client,-20}{nombrePlats}");
                    }

                    /// Afficher les détails des plats
                    Console.Write("\nVoulez-vous voir les détails des plats pour une commande ? (O/N) : ");
                    if (Console.ReadLine().ToUpper() == "O")
                    {
                        Console.Write("Entrez l'ID de la commande : ");
                        string idCommandeStr = Console.ReadLine();

                        string requetePlats = @"
                            SELECT p.id_plat, p.nom_plat, p.type_plat, p.prix_unitaire_plat, lc.statut_ligne
                            FROM LIGNE_COMMANDE lc
                            JOIN PLAT p ON lc.id_plat = p.id_plat
                            WHERE lc.id_commande = @IdCommande AND p.id_cuisinier = @IdCuisinier
                            ORDER BY p.type_plat";

                        Dictionary<string, object> parametresPlats = new Dictionary<string, object>
                        {
                            { "@IdCommande", idCommandeStr },
                            { "@IdCuisinier", idCuisinierStr }
                        };

                        DataTable plats = GestionBDD.ExecuterRequeteTable(requetePlats, parametresPlats);

                        if (plats != null && plats.Rows.Count > 0)
                        {
                            Console.WriteLine("\nDétails des plats de la commande :");
                            Console.WriteLine("ID\tNom\t\tType\t\tPrix\t\tStatut");
                            Console.WriteLine("-------------------------------------------------------------------------");

                            foreach (DataRow row in plats.Rows)
                            {
                                string idPlat = row["id_plat"].ToString();
                                string nomPlat = row["nom_plat"] == DBNull.Value ? "N/A" : row["nom_plat"].ToString();
                                string typePlat = row["type_plat"] == DBNull.Value ? "N/A" : row["type_plat"].ToString();
                                string prixPlat = row["prix_unitaire_plat"] == DBNull.Value ? "N/A" : row["prix_unitaire_plat"].ToString();
                                string statut = row["statut_ligne"] == DBNull.Value ? "N/A" : row["statut_ligne"].ToString();

                                Console.WriteLine($"{idPlat}\t{nomPlat,-15}{typePlat,-15}{prixPlat,-15}{statut}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun plat trouvé pour cette commande.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Aucune commande trouvée pour ce cuisinier.");
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
        /// Gère les évaluations des cuisiniers
        /// </summary>
        private static void GererEvaluationsCuisinier()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("GESTION DES ÉVALUATIONS");
                Console.WriteLine("Choisissez une option :");
                Console.WriteLine("1. Voir les évaluations d'un cuisinier");
                Console.WriteLine("2. Ajouter une évaluation");
                Console.WriteLine("3. Modifier une évaluation");
                Console.WriteLine("4. Supprimer une évaluation");
                Console.WriteLine("5. Retour");

                Console.Write("\nVotre choix : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        VoirEvaluationsCuisinier();
                        break;
                    case "2":
                        AjouterEvaluation();
                        break;
                    case "3":
                        ModifierEvaluation();
                        break;
                    case "4":
                        SupprimerEvaluation();
                        break;
                    case "5":
                        continuer = false;
                        break;
                    default:
                        GestionBDD.AfficherMessageErreur("Option invalide.");
                        break;
                }
            }
        }

        /// <summary>
        /// Affiche les évaluations d'un cuisinier
        /// </summary>
        private static void VoirEvaluationsCuisinier()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("ÉVALUATIONS D'UN CUISINIER");

            /// Afficher la liste des cuisiniers
            var cuisiniers = GestionBDD.ObtenirListeCuisiniersDetails();
            if (cuisiniers.Count == 0)
            {
                GestionBDD.AfficherMessageErreur("Aucun cuisinier trouvé.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Liste des cuisiniers :");
            foreach (var cuisinier in cuisiniers)
            {
                Console.WriteLine($"{cuisinier.id}. {cuisinier.prenom} {cuisinier.nom} ({cuisinier.email})");
            }

            Console.Write("\nEntrez l'ID du cuisinier (ou 0 pour annuler) : ");
            string idCuisinierStr = Console.ReadLine();

            if (idCuisinierStr == "0")
            {
                return;
            }

            var cuisiDetails = GestionBDD.ObtenirDetailsCuisinier(idCuisinierStr);
            if (string.IsNullOrEmpty(cuisiDetails.id))
            {
                GestionBDD.AfficherMessageErreur("Cuisinier introuvable.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            try
            {
                Console.WriteLine($"\nÉvaluations pour : {cuisiDetails.prenom} {cuisiDetails.nom}");

                string requete = @"
                    SELECT e.id_evaluation, e.note_evaluation, e.commentaire_evaluation, e.date_evaluation,
                    c.nom_client, c.prenom_client
                    FROM EVALUATION e
                    JOIN effectue ef ON e.id_evaluation = ef.id_evaluation
                    JOIN CLIENT c ON ef.id_client = c.id_client
                    JOIN reçoit r ON e.id_evaluation = r.id_evaluation
                    WHERE r.id_cuisinier = @IdCuisinier
                    ORDER BY e.date_evaluation DESC;";


                Dictionary<string, object> parametres = new Dictionary<string, object>
                {
                    { "@IdCuisinier", idCuisinierStr }
                };

                DataTable evaluations = GestionBDD.ExecuterRequeteTable(requete, parametres);

                if (evaluations != null && evaluations.Rows.Count > 0)
                {
                    Console.WriteLine("\nID\tNote\tDate\t\tClient\t\tCommentaire");
                    Console.WriteLine("-------------------------------------------------------------------------");

                    foreach (DataRow row in evaluations.Rows)
                    {
                        string idEvaluation = row["id_evaluation"].ToString();
                        string note = row["note_evaluation"] == DBNull.Value ? "N/A" : row["note_evaluation"].ToString();
                        string dateEvaluation = row["date_evaluation"] == DBNull.Value ? "N/A" : Convert.ToDateTime(row["date_evaluation"]).ToString("dd/MM/yyyy");
                        string nomClient = row["nom_client"] == DBNull.Value ? "N/A" : row["nom_client"].ToString();
                        string prenomClient = row["prenom_client"] == DBNull.Value ? "N/A" : row["prenom_client"].ToString();
                        string client = $"{prenomClient} {nomClient}";
                        string commentaire = row["commentaire_evaluation"] == DBNull.Value ? "N/A" : row["commentaire_evaluation"].ToString();

                        Console.WriteLine($"{idEvaluation}\t{note}\t{dateEvaluation}\t{client,-20}{commentaire}");
                    }

                    /// Calculer et afficher la note moyenne
                    string requeteMoyenne = "SELECT AVG(note_evaluation) FROM EVALUATION WHERE id_cuisinier = @IdCuisinier";
                    object resultatMoyenne = GestionBDD.ExecuterScalaire(requeteMoyenne, parametres);

                    if (resultatMoyenne != null && resultatMoyenne != DBNull.Value)
                    {
                        decimal noteMoyenne = Convert.ToDecimal(resultatMoyenne);
                        Console.WriteLine($"\nNote moyenne : {noteMoyenne:F2}/10");
                    }
                }
                else
                {
                    Console.WriteLine("Aucune évaluation trouvée pour ce cuisinier.");
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
        /// Ajoute une évaluation pour un cuisinier
        /// </summary>
        private static void AjouterEvaluation()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("AJOUTER UNE ÉVALUATION");

            /// Sélectionner un client
            var clients = GestionBDD.ObtenirListeClientsDetails();
            if (clients.Count == 0)
            {
                GestionBDD.AfficherMessageErreur("Aucun client trouvé.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Liste des clients :");
            foreach (var client in clients)
            {
                Console.WriteLine($"{client.id}. {client.prenom} {client.nom} ({client.email})");
            }

            Console.Write("\nEntrez l'ID du client (ou 0 pour annuler) : ");
            string idClientStr = Console.ReadLine();

            if (idClientStr == "0")
            {
                return;
            }

            /// Vérifier si l'ID client existe
            var clientDetails = GestionBDD.ObtenirDetailsClient(idClientStr);
            if (string.IsNullOrEmpty(clientDetails.id))
            {
                GestionBDD.AfficherMessageErreur("Client introuvable.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            var cuisiniers = GestionBDD.ObtenirListeCuisiniersDetails();
            if (cuisiniers.Count == 0)
            {
                GestionBDD.AfficherMessageErreur("Aucun cuisinier trouvé.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nListe des cuisiniers :");
            foreach (var cuisinier in cuisiniers)
            {
                Console.WriteLine($"{cuisinier.id}. {cuisinier.prenom} {cuisinier.nom} ({cuisinier.email})");
            }

            Console.Write("\nEntrez l'ID du cuisinier (ou 0 pour annuler) : ");
            string idCuisinierStr = Console.ReadLine();

            if (idCuisinierStr == "0")
            {
                return;
            }

            var cuisiDetails = GestionBDD.ObtenirDetailsCuisinier(idCuisinierStr);
            if (string.IsNullOrEmpty(cuisiDetails.id))
            {
                GestionBDD.AfficherMessageErreur("Cuisinier introuvable.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            string queryVerifEvaluation = "SELECT COUNT(*) FROM EVALUATION WHERE id_client = @IdClient AND id_cuisinier = @IdCuisinier";
            Dictionary<string, object> parametresVerif = new Dictionary<string, object>
            {
                { "@IdClient", idClientStr },
                { "@IdCuisinier", idCuisinierStr }
            };
            int nbEvaluations = Convert.ToInt32(GestionBDD.ExecuterScalaire(queryVerifEvaluation, parametresVerif));

            if (nbEvaluations > 0)
            {
                Console.Write("Ce client a déjà évalué ce cuisinier. Voulez-vous modifier l'évaluation existante ? (O/N) : ");
                if (Console.ReadLine().ToUpper() == "O")
                {

                    string queryIdEvaluation = "SELECT id_evaluation FROM EVALUATION WHERE id_client = @IdClient AND id_cuisinier = @IdCuisinier";
                    string idEvaluation = GestionBDD.ExecuterScalaire(queryIdEvaluation, parametresVerif).ToString();

                    /// Appeler la méthode de modification avec l'ID récupéré
                    ModifierEvaluationSpecifique(idEvaluation);
                    return;
                }
                else
                {
                    return;
                }
            }

            Console.Write("\nNote (1-10) : ");
            string noteStr = Console.ReadLine();
            if (!int.TryParse(noteStr, out int note) || note < 1 || note > 10)
            {
                GestionBDD.AfficherMessageErreur("La note doit être un nombre entre 1 et 10.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            Console.Write("Commentaire : ");
            string commentaire = Console.ReadLine();

            try
            {
                int idEvaluation = GestionBDD.GenererNouvelID("EVALUATION", "id_evaluation");

                Dictionary<string, object> parametres = new Dictionary<string, object>
                {
                    { "@IdEvaluation", idEvaluation },
                    { "@IdClient", idClientStr },
                    { "@IdCuisinier", idCuisinierStr },
                    { "@Note", note },
                    { "@Commentaire", commentaire }
                };

                string query = "INSERT INTO EVALUATION (id_evaluation, id_client, id_cuisinier, note_evaluation, commentaire_evaluation, date_evaluation) " +
                              "VALUES (@IdEvaluation, @IdClient, @IdCuisinier, @Note, @Commentaire, NOW())";

                int resultat = GestionBDD.ExecuterRequete(query, parametres);

                if (resultat > 0)
                {
                    GestionBDD.AfficherMessageSucces("Évaluation ajoutée avec succès !");

                    MettreAJourNoteMoyenneCuisinier(idCuisinierStr);
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Échec de l'ajout de l'évaluation.");
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
        /// Modifie une évaluation existante
        /// </summary>
        private static void ModifierEvaluation()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MODIFIER UNE ÉVALUATION");

            /// Afficher la liste des évaluations
            string requete = @"
                SELECT e.id_evaluation, cl.nom_client, cl.prenom_client, 
                cu.nom_cuisinier, cu.prenom_cuisinier, 
                e.note_evaluation, e.date_evaluation
                FROM EVALUATION e
                JOIN effectue ef ON e.id_evaluation = ef.id_evaluation
                JOIN CLIENT cl ON ef.id_client = cl.id_client
                JOIN reçoit r ON e.id_evaluation = r.id_evaluation
                JOIN CUISINIER cu ON r.id_cuisinier = cu.id_cuisinier
                ORDER BY e.date_evaluation DESC";

            DataTable evaluations = GestionBDD.ExecuterRequeteTable(requete, null);

            if (evaluations == null || evaluations.Rows.Count == 0)
            {
                GestionBDD.AfficherMessageErreur("Aucune évaluation trouvée.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Liste des évaluations :");
            Console.WriteLine("ID\tClient\t\t\tCuisinier\t\tNote\tDate");
            Console.WriteLine("-------------------------------------------------------------------------");

            foreach (DataRow row in evaluations.Rows)
            {
                string idEvaluation = row["id_evaluation"].ToString();
                string nomClient = row["nom_client"] == DBNull.Value ? "N/A" : row["nom_client"].ToString();
                string prenomClient = row["prenom_client"] == DBNull.Value ? "N/A" : row["prenom_client"].ToString();
                string client = $"{prenomClient} {nomClient}";
                string nomCuisinier = row["nom_cuisinier"] == DBNull.Value ? "N/A" : row["nom_cuisinier"].ToString();
                string prenomCuisinier = row["prenom_cuisinier"] == DBNull.Value ? "N/A" : row["prenom_cuisinier"].ToString();
                string cuisinier = $"{prenomCuisinier} {nomCuisinier}";
                string note = row["note_evaluation"] == DBNull.Value ? "N/A" : row["note_evaluation"].ToString();
                string dateEvaluation = row["date_evaluation"] == DBNull.Value ? "N/A" : Convert.ToDateTime(row["date_evaluation"]).ToString("dd/MM/yyyy");

                Console.WriteLine($"{idEvaluation}\t{client,-20}{cuisinier,-20}{note}\t{dateEvaluation}");
            }

            Console.Write("\nEntrez l'ID de l'évaluation à modifier (ou 0 pour annuler) : ");
            string idEvaluationStr = Console.ReadLine();

            if (idEvaluationStr == "0")
            {
                return;
            }

            ModifierEvaluationSpecifique(idEvaluationStr);
        }

        /// <summary>
        /// Modifie une évaluation spécifique
        /// </summary>
        /// <param name="idEvaluationStr">ID de l'évaluation à modifier</param>
        private static void ModifierEvaluationSpecifique(string idEvaluationStr)
        {
            /// Vérifier si l'ID évaluation existe
            string queryVerifEvaluation = @"
                    SELECT e.id_evaluation, r.id_cuisinier, e.note_evaluation, e.commentaire_evaluation,
                           cl.nom_client, cl.prenom_client,
                           cu.nom_cuisinier, cu.prenom_cuisinier
                    FROM EVALUATION e
                    JOIN effectue ef ON e.id_evaluation = ef.id_evaluation
                    JOIN CLIENT cl ON ef.id_client = cl.id_client
                    JOIN reçoit r ON e.id_evaluation = r.id_evaluation
                    JOIN CUISINIER cu ON r.id_cuisinier = cu.id_cuisinier
                    WHERE e.id_evaluation = @IdEvaluation";


            Dictionary<string, object> parametresVerif = new Dictionary<string, object>
            {
                { "@IdEvaluation", idEvaluationStr }
            };

            DataTable evaluationDetails = GestionBDD.ExecuterRequeteTable(queryVerifEvaluation, parametresVerif);

            if (evaluationDetails == null || evaluationDetails.Rows.Count == 0)
            {
                GestionBDD.AfficherMessageErreur("Évaluation introuvable.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            DataRow evaluation = evaluationDetails.Rows[0];
            string idCuisinier = evaluation["id_cuisinier"].ToString();
            string nomClient = evaluation["nom_client"].ToString();
            string prenomClient = evaluation["prenom_client"].ToString();
            string nomCuisinier = evaluation["nom_cuisinier"].ToString();
            string prenomCuisinier = evaluation["prenom_cuisinier"].ToString();
            int noteActuelle = Convert.ToInt32(evaluation["note_evaluation"]);
            string commentaireActuel = evaluation["commentaire_evaluation"] == DBNull.Value ? "" : evaluation["commentaire_evaluation"].ToString();

            Console.WriteLine($"\nModification de l'évaluation de {prenomClient} {nomClient} pour {prenomCuisinier} {nomCuisinier}");
            Console.WriteLine($"Note actuelle : {noteActuelle}");
            Console.WriteLine($"Commentaire actuel : {commentaireActuel}");

            Console.Write("\nNouvelle note (1-10, laisser vide pour conserver l'actuelle) : ");
            string noteStr = Console.ReadLine();
            int nouvelleNote = noteActuelle;
            if (!string.IsNullOrWhiteSpace(noteStr))
            {
                if (!int.TryParse(noteStr, out nouvelleNote) || nouvelleNote < 1 || nouvelleNote > 10)
                {
                    GestionBDD.AfficherMessageErreur("La note doit être un nombre entre 1 et 10.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }
            }

            Console.Write("Nouveau commentaire (laisser vide pour conserver l'actuel) : ");
            string nouveauCommentaire = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nouveauCommentaire))
            {
                nouveauCommentaire = commentaireActuel;
            }

            try
            {
                Dictionary<string, object> parametres = new Dictionary<string, object>
                {
                    { "@IdEvaluation", idEvaluationStr },
                    { "@Note", nouvelleNote },
                    { "@Commentaire", nouveauCommentaire }
                };

                string query = "UPDATE EVALUATION SET note_evaluation = @Note, commentaire_evaluation = @Commentaire, date_evaluation = NOW() " +
                              "WHERE id_evaluation = @IdEvaluation";

                int resultat = GestionBDD.ExecuterRequete(query, parametres);

                if (resultat > 0)
                {
                    GestionBDD.AfficherMessageSucces("Évaluation modifiée avec succès !");

                    /// Mettre à jour la note moyenne du cuisinier
                    MettreAJourNoteMoyenneCuisinier(idCuisinier);
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Échec de la modification de l'évaluation.");
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
        /// Supprime une évaluation existante
        /// </summary>
        private static void SupprimerEvaluation()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("SUPPRIMER UNE ÉVALUATION");

            string requete = @"
                SELECT e.id_evaluation, cl.nom_client, cl.prenom_client, 
                       cu.nom_cuisinier, cu.prenom_cuisinier, e.note_evaluation, e.date_evaluation
                FROM EVALUATION e
                JOIN effectue ef ON e.id_evaluation = ef.id_evaluation
                JOIN CLIENT cl ON ef.id_client = cl.id_client
                JOIN reçoit r ON e.id_evaluation = r.id_evaluation
                JOIN CUISINIER cu ON r.id_cuisinier = cu.id_cuisinier
                ORDER BY e.date_evaluation DESC";

            DataTable evaluations = GestionBDD.ExecuterRequeteTable(requete, null);

            if (evaluations == null || evaluations.Rows.Count == 0)
            {
                GestionBDD.AfficherMessageErreur("Aucune évaluation trouvée.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Liste des évaluations :");
            Console.WriteLine("ID\tClient\t\t\tCuisinier\t\tNote\tDate");
            Console.WriteLine("-------------------------------------------------------------------------");

            foreach (DataRow row in evaluations.Rows)
            {
                string idEvaluation = row["id_evaluation"].ToString();
                string nomClient1 = row["nom_client"] == DBNull.Value ? "N/A" : row["nom_client"].ToString();
                string prenomClient1 = row["prenom_client"] == DBNull.Value ? "N/A" : row["prenom_client"].ToString();
                string client = $"{prenomClient1} {nomClient1}";
                string nomCuisinier1 = row["nom_cuisinier"] == DBNull.Value ? "N/A" : row["nom_cuisinier"].ToString();
                string prenomCuisinier1 = row["prenom_cuisinier"] == DBNull.Value ? "N/A" : row["prenom_cuisinier"].ToString();
                string cuisinier = $"{prenomCuisinier1} {nomCuisinier1}";
                string note = row["note_evaluation"] == DBNull.Value ? "N/A" : row["note_evaluation"].ToString();
                string dateEvaluation = row["date_evaluation"] == DBNull.Value ? "N/A" : Convert.ToDateTime(row["date_evaluation"]).ToString("dd/MM/yyyy");

                Console.WriteLine($"{idEvaluation}\t{client,-20}{cuisinier,-20}{note}\t{dateEvaluation}");
            }

            Console.Write("\nEntrez l'ID de l'évaluation à supprimer (ou 0 pour annuler) : ");
            string idEvaluationStr = Console.ReadLine();

            if (idEvaluationStr == "0")
            {
                return;
            }

            string queryVerifEvaluation = @"SELECT e.id_evaluation, cu.id_cuisinier, 
                                            cl.nom_client, cl.prenom_client, 
                                            cu.nom_cuisinier, cu.prenom_cuisinier
                                            FROM EVALUATION e
                                            JOIN effectue ef ON e.id_evaluation = ef.id_evaluation
                                            JOIN CLIENT cl ON ef.id_client = cl.id_client
                                            JOIN reçoit r ON e.id_evaluation = r.id_evaluation
                                            JOIN CUISINIER cu ON r.id_cuisinier = cu.id_cuisinier
                                            WHERE e.id_evaluation = @IdEvaluation";

            Dictionary<string, object> parametresVerif = new Dictionary<string, object>
            {
                { "@IdEvaluation", idEvaluationStr }
            };

            DataTable evaluationDetails = GestionBDD.ExecuterRequeteTable(queryVerifEvaluation, parametresVerif);

            if (evaluationDetails == null || evaluationDetails.Rows.Count == 0)
            {
                GestionBDD.AfficherMessageErreur("Évaluation introuvable.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            DataRow evaluation = evaluationDetails.Rows[0];
            string idCuisinier = evaluation["id_cuisinier"].ToString();
            string nomClient = evaluation["nom_client"].ToString();
            string prenomClient = evaluation["prenom_client"].ToString();
            string nomCuisinier = evaluation["nom_cuisinier"].ToString();
            string prenomCuisinier = evaluation["prenom_cuisinier"].ToString();

            Console.Write($"\nÊtes-vous sûr de vouloir supprimer l'évaluation de {prenomClient} {nomClient} pour {prenomCuisinier} {nomCuisinier} ? (O/N) : ");
            string confirmation = Console.ReadLine().ToUpper();

            if (confirmation != "O")
            {
                return;
            }

            try
            {
                Dictionary<string, object> parametres = new Dictionary<string, object>
                {
                    { "@IdEvaluation", idEvaluationStr }
                };

                string query = "DELETE FROM EVALUATION WHERE id_evaluation = @IdEvaluation";

                int resultat = GestionBDD.ExecuterRequete(query, parametres);

                if (resultat > 0)
                {
                    GestionBDD.AfficherMessageSucces("Évaluation supprimée avec succès !");

                    MettreAJourNoteMoyenneCuisinier(idCuisinier);
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Échec de la suppression de l'évaluation.");
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
        /// Met à jour la note moyenne d'un cuisinier
        /// </summary>
        /// <param name="idCuisinier">ID du cuisinier</param>
        private static void MettreAJourNoteMoyenneCuisinier(string idCuisinier)
        {
            try
            {
                /// Calculer la nouvelle note moyenne
                string requeteMoyenne = "SELECT AVG(note_evaluation) FROM EVALUATION WHERE id_cuisinier = @IdCuisinier";
                Dictionary<string, object> parametresMoyenne = new Dictionary<string, object>
                {
                    { "@IdCuisinier", idCuisinier }
                };

                object resultatMoyenne = GestionBDD.ExecuterScalaire(requeteMoyenne, parametresMoyenne);
                decimal noteMoyenne = 0;

                if (resultatMoyenne != null && resultatMoyenne != DBNull.Value)
                {
                    noteMoyenne = Convert.ToDecimal(resultatMoyenne);
                }

                /// Mettre à jour la note moyenne du cuisinier
                string requeteMiseAJour = "UPDATE CUISINIER SET note_moyenne_cuisinier = @NoteMoyenne WHERE id_cuisinier = @IdCuisinier";
                Dictionary<string, object> parametresMiseAJour = new Dictionary<string, object>
                {
                    { "@IdCuisinier", idCuisinier },
                    { "@NoteMoyenne", noteMoyenne }
                };

                GestionBDD.ExecuterRequete(requeteMiseAJour, parametresMiseAJour);
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur lors de la mise à jour de la note moyenne : {ex.Message}");
            }
        }

        #endregion

        #region Méthodes Utilitaires

        /// <summary>
        /// Affiche les détails du profil d'un administrateur
        /// </summary>
        /// <param name="idAdmin">ID de l'administrateur</param>
        private static void VoirProfilAdmin(string idAdmin)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MON PROFIL ADMINISTRATEUR");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                SELECT id_admin, nom_admin, prenom_admin, email_admin
                FROM ADMINISTRATEUR
                WHERE id_admin = @IdAdmin";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdAdmin", idAdmin);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nom = reader.IsDBNull(reader.GetOrdinal("nom_admin")) ? "N/A" : reader["nom_admin"].ToString();
                            string prenom = reader.IsDBNull(reader.GetOrdinal("prenom_admin")) ? "N/A" : reader["prenom_admin"].ToString();
                            string email = reader.IsDBNull(reader.GetOrdinal("email_admin")) ? "N/A" : reader["email_admin"].ToString();

                            Console.WriteLine($"Nom : {nom}");
                            Console.WriteLine($"Prénom : {prenom}");
                            Console.WriteLine($"Email : {email}");
                        }
                        else
                        {
                            Console.WriteLine("Profil administrateur non trouvé.");
                        }
                    }
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
        /// Modifie les informations d'un profil administrateur
        /// </summary>
        /// <param name="idAdmin">ID de l'administrateur</param>
        private static void ModifierProfilAdmin(string idAdmin)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MODIFIER MON PROFIL ADMINISTRATEUR");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string queryInfo = @"
                SELECT nom_admin, prenom_admin, email_admin
                FROM ADMINISTRATEUR
                WHERE id_admin = @IdAdmin";

                    MySqlCommand cmdInfo = new MySqlCommand(queryInfo, connection);
                    cmdInfo.Parameters.AddWithValue("@IdAdmin", idAdmin);

                    using (MySqlDataReader reader = cmdInfo.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nomActuel = reader.IsDBNull(reader.GetOrdinal("nom_admin")) ? "" : reader["nom_admin"].ToString();
                            string prenomActuel = reader.IsDBNull(reader.GetOrdinal("prenom_admin")) ? "" : reader["prenom_admin"].ToString();
                            string emailActuel = reader.IsDBNull(reader.GetOrdinal("email_admin")) ? "" : reader["email_admin"].ToString();

                            reader.Close();

                            Console.WriteLine("Laissez vide pour conserver la valeur actuelle.");

                            Console.Write($"Nom ({nomActuel}) : ");
                            string nouveauNom = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauNom))
                                nouveauNom = nomActuel;

                            Console.Write($"Prénom ({prenomActuel}) : ");
                            string nouveauPrenom = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauPrenom))
                                nouveauPrenom = prenomActuel;

                            Console.Write($"Email ({emailActuel}) : ");
                            string nouvelEmail = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouvelEmail))
                                nouvelEmail = emailActuel;
                            else if (nouvelEmail != emailActuel && EmailAdminExiste(nouvelEmail))
                            {
                                GestionBDD.AfficherMessageErreur("Cet email est déjà utilisé par un autre compte.");
                                return;
                            }

                            string queryUpdate = @"
                        UPDATE ADMINISTRATEUR
                        SET nom_admin = @Nom,
                            prenom_admin = @Prenom,
                            email_admin = @Email
                        WHERE id_admin = @IdAdmin";

                            MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, connection);
                            cmdUpdate.Parameters.AddWithValue("@IdAdmin", idAdmin);
                            cmdUpdate.Parameters.AddWithValue("@Nom", nouveauNom);
                            cmdUpdate.Parameters.AddWithValue("@Prenom", nouveauPrenom);
                            cmdUpdate.Parameters.AddWithValue("@Email", nouvelEmail);

                            int resultat = cmdUpdate.ExecuteNonQuery();

                            if (resultat > 0)
                            {
                                GestionBDD.AfficherMessageSucces("Profil modifié avec succès !");

                                if (nouvelEmail != emailActuel)
                                {
                                    SessionUtilisateur.Connecter(idAdmin, nouveauNom, nouveauPrenom, "admin", nouvelEmail);
                                }
                            }
                            else
                            {
                                GestionBDD.AfficherMessageErreur("Erreur lors de la modification du profil.");
                            }
                        }
                        else
                        {
                            reader.Close();
                            GestionBDD.AfficherMessageErreur("Profil administrateur non trouvé.");
                        }
                    }
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
        /// Change le mot de passe d'un utilisateur
        /// </summary>
        /// <param name="typeUtilisateur">Type d'utilisateur (client, cuisinier, admin)</param>
        /// <param name="idUtilisateur">ID de l'utilisateur</param>
        private static void ChangerMotDePasse(string typeUtilisateur, string idUtilisateur)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("CHANGER MON MOT DE PASSE");

            Console.Write("Ancien mot de passe : ");
            string ancienMotDePasse = Console.ReadLine();

            Console.Write("Nouveau mot de passe : ");
            string nouveauMotDePasse = Console.ReadLine();

            Console.Write("Confirmez le nouveau mot de passe : ");
            string confirmationMotDePasse = Console.ReadLine();

            if (nouveauMotDePasse != confirmationMotDePasse)
            {
                GestionBDD.AfficherMessageErreur("Les mots de passe ne correspondent pas.");
                return;
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string queryVerif = "";
                    string queryUpdate = "";
                    string colonneMdp = "";
                    string colonneId = "";
                    string nomTable = "";

                    if (typeUtilisateur == "client")
                    {
                        nomTable = "CLIENT";
                        colonneMdp = "mot_de_passe_client";
                        colonneId = "id_client";
                    }
                    else if (typeUtilisateur == "cuisinier")
                    {
                        nomTable = "CUISINIER";
                        colonneMdp = "mot_de_passe_cuisinier";
                        colonneId = "id_cuisinier";
                    }
                    else if (typeUtilisateur == "admin")
                    {
                        nomTable = "ADMINISTRATEUR";
                        colonneMdp = "mot_de_passe_admin";
                        colonneId = "id_admin";
                    }
                    else
                    {
                        GestionBDD.AfficherMessageErreur("Type d'utilisateur invalide.");
                        return;
                    }

                    queryVerif = $"SELECT COUNT(*) FROM {nomTable} WHERE {colonneId} = @IdUtilisateur AND {colonneMdp} = @AncienMotDePasse";
                    queryUpdate = $"UPDATE {nomTable} SET {colonneMdp} = @NouveauMotDePasse WHERE {colonneId} = @IdUtilisateur";

                    /// Vérifier l'ancien mot de passe
                    MySqlCommand cmdVerif = new MySqlCommand(queryVerif, connection);
                    cmdVerif.Parameters.AddWithValue("@IdUtilisateur", idUtilisateur);
                    cmdVerif.Parameters.AddWithValue("@AncienMotDePasse", ancienMotDePasse);

                    int count = Convert.ToInt32(cmdVerif.ExecuteScalar());
                    if (count == 0)
                    {
                        GestionBDD.AfficherMessageErreur("Ancien mot de passe incorrect.");
                        return;
                    }

                    /// Mettre à jour le mot de passe
                    MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, connection);
                    cmdUpdate.Parameters.AddWithValue("@IdUtilisateur", idUtilisateur);
                    cmdUpdate.Parameters.AddWithValue("@NouveauMotDePasse", nouveauMotDePasse);

                    int resultat = cmdUpdate.ExecuteNonQuery();

                    if (resultat > 0)
                    {
                        GestionBDD.AfficherMessageSucces("Mot de passe modifié avec succès !");
                    }
                    else
                    {
                        GestionBDD.AfficherMessageErreur("Erreur lors de la modification du mot de passe.");
                    }
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
        /// Vérifie si un email de client existe déjà dans la base de données
        /// </summary>
        /// <param name="email">Email à vérifier</param>
        /// <returns>True si l'email existe, False sinon</returns>
        private static bool EmailClientExiste(string email)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM CLIENT WHERE email_client = @Email";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Email", email);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la vérification de l'email client : {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Vérifie si un email de cuisinier existe déjà dans la base de données
        /// </summary>
        /// <param name="email">Email à vérifier</param>
        /// <returns>True si l'email existe, False sinon</returns>
        private static bool EmailCuisinierExiste(string email)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM CUISINIER WHERE email_cuisinier = @Email";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Email", email);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la vérification de l'email cuisinier : {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Vérifie si un email d'administrateur existe déjà dans la base de données
        /// </summary>
        /// <param name="email">Email à vérifier</param>
        /// <returns>True si l'email existe, False sinon</returns>
        private static bool EmailAdminExiste(string email)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM ADMINISTRATEUR WHERE email_admin = @Email";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Email", email);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la vérification de l'email administrateur : {ex.Message}");
                return false;
            }
        }

        #endregion
    }
}
