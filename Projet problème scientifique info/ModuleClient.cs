using MySql.Data.MySqlClient;
using Projet_problème_scientifique_info;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Classe gérant les fonctionnalités spécifiques au menu client
    /// </summary>
    internal class ModuleClient
    {
        //private static string connectionString = "Server=localhost;Database=livinparis2;Uid=root;Pwd=REDACTED;";
        private static string connectionString = "Server=localhost;Database=livinparis2;Uid=root;Pwd=REDACTED;";


        /// <summary>
        /// Affiche le menu principal du client connecté
        /// </summary>
        public static void MenuPrincipalClient()
        {
            /// Vérifier que l'utilisateur est un client
            if (!SessionUtilisateur.EstClient())
            {
                SessionUtilisateur.AfficherMessageDroitsInsuffisants();
                return;
            }

            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("MENU CLIENT");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"Bienvenue {SessionUtilisateur.PrenomUtilisateur} {SessionUtilisateur.NomUtilisateur} !");
                Console.ResetColor();   
                Console.WriteLine();
                Console.WriteLine("1. Accéder au module de plats");
                Console.WriteLine("2. Accéder au module de commandes");
                Console.WriteLine("3. Accéder à mes statistiques");
                Console.WriteLine("4. Gérer mon profil");
                Console.WriteLine("5. Gérer mes adresses");
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
                        ModuleStats.MenuPrincipalStats();
                        break;
                    case "4":
                        GererMonProfil();
                        break;
                    case "5":
                        GererMesAdresses();
                        break;
                    case "6":
                        SessionUtilisateur.Deconnecter();
                        continuer = false;
                        break;
                    default:
                        GestionBDD.AfficherMessageErreur("Option invalide.");
                        break;
                }
            }
        }

        #region Gestion du profil
        /// <summary>
        /// Permet au client de gérer son profil
        /// </summary>
        private static void GererMonProfil()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("GÉRER MON PROFIL");

            try
            {
                /// Récupérer les informations du client
                string requeteClient = @"
                    SELECT nom_client, prenom_client, email_client, tel_client, type_client, 
                           nom_entreprise, nom_referent, code_postal_client, rue_client, metro_le_plus_proche_client
                    FROM CLIENT
                    WHERE id_client = @IdClient";

                Dictionary<string, object> parametresClient = new Dictionary<string, object>
                {
                    { "@IdClient", SessionUtilisateur.IdUtilisateur }
                };

                DataTable clientDetails = GestionBDD.ExecuterRequeteTable(requeteClient, parametresClient);

                if (clientDetails == null || clientDetails.Rows.Count == 0)
                {
                    GestionBDD.AfficherMessageErreur("Erreur lors de la récupération des informations du profil.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                DataRow client = clientDetails.Rows[0];
                string nom = client["nom_client"] != DBNull.Value ? client["nom_client"].ToString() : "";
                string prenom = client["prenom_client"] != DBNull.Value ? client["prenom_client"].ToString() : "";
                string email = client["email_client"] != DBNull.Value ? client["email_client"].ToString() : "";
                string telephone = client["tel_client"] != DBNull.Value ? client["tel_client"].ToString() : "";
                string typeClient = client["type_client"] != DBNull.Value ? client["type_client"].ToString() : "";
                string nomEntreprise = client["nom_entreprise"] != DBNull.Value ? client["nom_entreprise"].ToString() : "";
                string nomReferent = client["nom_referent"] != DBNull.Value ? client["nom_referent"].ToString() : "";
                string codePostal = client["code_postal_client"] != DBNull.Value ? client["code_postal_client"].ToString() : "";
                string rue = client["rue_client"] != DBNull.Value ? client["rue_client"].ToString() : "";
                string metro = client["metro_le_plus_proche_client"] != DBNull.Value ? client["metro_le_plus_proche_client"].ToString() : "";

                Console.WriteLine("Informations actuelles :");
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
                Console.WriteLine($"Adresse : {rue}, {codePostal}");
                Console.WriteLine($"Métro le plus proche : {metro}");

                Console.WriteLine("\nQue souhaitez-vous modifier ?");
                Console.WriteLine("1. Nom");
                Console.WriteLine("2. Prénom");
                Console.WriteLine("3. Email");
                Console.WriteLine("4. Mot de passe");
                Console.WriteLine("5. Téléphone");
                Console.WriteLine("6. Type de client");
                Console.WriteLine("7. Adresse principale");
                Console.WriteLine("8. Retour au menu client");

                Console.Write("\nVotre choix : ");
                string choix = Console.ReadLine();

                string nouveauNom = nom;
                string nouveauPrenom = prenom;
                string nouvelEmail = email;
                string nouveauMotDePasse = "";
                string nouveauTelephone = telephone;
                string nouveauTypeClient = typeClient;
                string nouveauNomEntreprise = nomEntreprise;
                string nouveauNomReferent = nomReferent;
                string nouveauCodePostal = codePostal;
                string nouvelleRue = rue;
                string nouveauMetro = metro;

                switch (choix)
                {
                    case "1":
                        Console.Write($"Nouveau nom ({nom}) : ");
                        string inputNom = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(inputNom))
                            nouveauNom = inputNom;
                        break;
                    case "2":
                        Console.Write($"Nouveau prénom ({prenom}) : ");
                        string inputPrenom = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(inputPrenom))
                            nouveauPrenom = inputPrenom;
                        break;
                    case "3":
                        Console.Write($"Nouvel email ({email}) : ");
                        string inputEmail = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(inputEmail))
                        {
                            /// Vérifier si l'email existe déjà
                            string requeteVerifEmail = "SELECT COUNT(*) FROM CLIENT WHERE email_client = @Email AND id_client != @IdClient";
                            Dictionary<string, object> parametresVerifEmail = new Dictionary<string, object>
                            {
                                { "@Email", inputEmail },
                                { "@IdClient", SessionUtilisateur.IdUtilisateur }
                            };
                            int countEmail = Convert.ToInt32(GestionBDD.ExecuterScalaire(requeteVerifEmail, parametresVerifEmail));
                            if (countEmail > 0)
                            {
                                GestionBDD.AfficherMessageErreur("Cet email est déjà utilisé par un autre compte.");
                                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                                Console.ReadKey();
                                return;
                            }
                            nouvelEmail = inputEmail;
                        }
                        break;
                    case "4":
                        Console.Write("Nouveau mot de passe : ");
                        nouveauMotDePasse = Console.ReadLine();
                        break;
                    case "5":
                        Console.Write($"Nouveau téléphone ({telephone}) : ");
                        string inputTelephone = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(inputTelephone))
                            nouveauTelephone = inputTelephone;
                        break;
                    case "6":
                        Console.Write($"Nouveau type de client ({typeClient}) : ");
                        string inputTypeClient = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(inputTypeClient))
                            nouveauTypeClient = inputTypeClient;

                        if (nouveauTypeClient.ToLower() == "entreprise")
                        {
                            Console.Write($"Nouveau nom d'entreprise ({nomEntreprise}) : ");
                            string inputNomEntreprise = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(inputNomEntreprise))
                                nouveauNomEntreprise = inputNomEntreprise;

                            Console.Write($"Nouveau nom de référent ({nomReferent}) : ");
                            string inputNomReferent = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(inputNomReferent))
                                nouveauNomReferent = inputNomReferent;
                        }
                        break;
                    case "7":
                        Console.Write($"Nouveau code postal ({codePostal}) : ");
                        string inputCodePostal = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(inputCodePostal))
                            nouveauCodePostal = inputCodePostal;

                        Console.Write($"Nouvelle rue ({rue}) : ");
                        string inputRue = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(inputRue))
                            nouvelleRue = inputRue;

                        Console.Write($"Nouveau métro le plus proche ({metro}) : ");
                        string inputMetro = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(inputMetro))
                            nouveauMetro = inputMetro;
                        break;
                    case "8":
                        return;
                    default:
                        GestionBDD.AfficherMessageErreur("Option invalide.");
                        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        return;
                }

                /// Construire la requête de mise à jour
                string requeteUpdate = "UPDATE CLIENT SET nom_client = @Nom, prenom_client = @Prenom, email_client = @Email, " +
                                      "tel_client = @Tel, type_client = @TypeClient, nom_entreprise = @NomEntreprise, " +
                                      "nom_referent = @NomReferent, code_postal_client = @CodePostal, rue_client = @Rue, " +
                                      "metro_le_plus_proche_client = @Metro";

                Dictionary<string, object> parametresUpdate = new Dictionary<string, object>
                {
                    { "@IdClient", SessionUtilisateur.IdUtilisateur },
                    { "@Nom", nouveauNom },
                    { "@Prenom", nouveauPrenom },
                    { "@Email", nouvelEmail },
                    { "@Tel", nouveauTelephone },
                    { "@TypeClient", nouveauTypeClient },
                    { "@NomEntreprise", string.IsNullOrEmpty(nouveauNomEntreprise) ? DBNull.Value : (object)nouveauNomEntreprise },
                    { "@NomReferent", string.IsNullOrEmpty(nouveauNomReferent) ? DBNull.Value : (object)nouveauNomReferent },
                    { "@CodePostal", nouveauCodePostal },
                    { "@Rue", nouvelleRue },
                    { "@Metro", nouveauMetro }
                };

                if (!string.IsNullOrEmpty(nouveauMotDePasse))
                {
                    requeteUpdate += ", mot_de_passe_client = @MotDePasse";
                    parametresUpdate.Add("@MotDePasse", nouveauMotDePasse);
                }

                requeteUpdate += " WHERE id_client = @IdClient";

                int resultat = GestionBDD.ExecuterRequete(requeteUpdate, parametresUpdate);

                if (resultat > 0)
                {
                    GestionBDD.AfficherMessageSucces("Profil mis à jour avec succès !");

                    /// Mettre à jour les informations de session si le nom ou prénom a changé
                    if (nouveauNom != nom || nouveauPrenom != prenom || nouvelEmail != email)
                    {
                        SessionUtilisateur.Connecter(SessionUtilisateur.IdUtilisateur, nouveauNom, nouveauPrenom, nouvelEmail, SessionUtilisateur.TypeUtilisateur);
                    }
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Erreur lors de la mise à jour du profil.");
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

        #region Gestion des adresses 
        /// <summary>
        /// Permet au client de gérer ses adresses
        /// </summary>
        private static void GererMesAdresses()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("GÉRER MES ADRESSES");

                try
                {
                    /// Récupérer les adresses du client
                    string requeteAdresses = @"
                        SELECT id_adresse, code_porte, complement_adresse, étage, numero_appartement
                        FROM ADRESSE
                        WHERE id_client = @IdClient";

                    Dictionary<string, object> parametresAdresses = new Dictionary<string, object>
                    {
                        { "@IdClient", SessionUtilisateur.IdUtilisateur }
                    };

                    DataTable adresses = GestionBDD.ExecuterRequeteTable(requeteAdresses, parametresAdresses);

                    if (adresses != null && adresses.Rows.Count > 0)
                    {
                        Console.WriteLine("Vos adresses :");
                        Console.WriteLine("ID\tCode porte\tComplément\tÉtage\tN° Appartement");
                        Console.WriteLine("-------------------------------------------------------------------------");

                        foreach (DataRow row in adresses.Rows)
                        {
                            string idAdresse = row["id_adresse"].ToString();
                            string codePorte = row["code_porte"] != DBNull.Value ? row["code_porte"].ToString() : "N/A";
                            string complement = row["complement_adresse"] != DBNull.Value ? row["complement_adresse"].ToString() : "N/A";
                            string etage = row["étage"] != DBNull.Value ? row["étage"].ToString() : "N/A";
                            string numeroAppartement = row["numero_appartement"] != DBNull.Value ? row["numero_appartement"].ToString() : "N/A";

                            Console.WriteLine($"{idAdresse}\t{codePorte}\t\t{complement}\t\t{etage}\t{numeroAppartement}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Vous n'avez pas encore d'adresses complémentaires.");
                    }

                    Console.WriteLine("\nQue souhaitez-vous faire ?");
                    Console.WriteLine("1. Ajouter une adresse");
                    Console.WriteLine("2. Modifier une adresse");
                    Console.WriteLine("3. Supprimer une adresse");
                    Console.WriteLine("4. Retour au menu client");

                    Console.Write("\nVotre choix : ");
                    string choix = Console.ReadLine();

                    switch (choix)
                    {
                        case "1":
                            AjouterAdresse();
                            break;
                        case "2":
                            ModifierAdresse();
                            break;
                        case "3":
                            SupprimerAdresse();
                            break;
                        case "4":
                            continuer = false;
                            break;
                        default:
                            GestionBDD.AfficherMessageErreur("Option invalide.");
                            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    continuer = false;
                }
            }
        }

        /// <summary>
        /// Ajoute une adresse pour le client connecté
        /// </summary>
        private static void AjouterAdresse()
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
                    { "@IdClient", SessionUtilisateur.IdUtilisateur },
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
        /// Modifie une adresse du client connecté
        /// </summary>
        private static void ModifierAdresse()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MODIFIER UNE ADRESSE");

            Console.Write("Entrez l'ID de l'adresse à modifier : ");
            string idAdresse = Console.ReadLine();

            try
            {
                /// Vérifier si l'adresse existe et appartient au client
                string requeteVerif = @"
                    SELECT code_porte, complement_adresse, étage, numero_appartement
                    FROM ADRESSE
                    WHERE id_adresse = @IdAdresse AND id_client = @IdClient";

                Dictionary<string, object> parametresVerif = new Dictionary<string, object>
                {
                    { "@IdAdresse", idAdresse },
                    { "@IdClient", SessionUtilisateur.IdUtilisateur }
                };

                DataTable adresseDetails = GestionBDD.ExecuterRequeteTable(requeteVerif, parametresVerif);

                if (adresseDetails == null || adresseDetails.Rows.Count == 0)
                {
                    GestionBDD.AfficherMessageErreur("Adresse introuvable ou n'appartenant pas à votre compte.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                DataRow adresse = adresseDetails.Rows[0];
                string codePorteActuel = adresse["code_porte"] != DBNull.Value ? adresse["code_porte"].ToString() : "";
                string complementAdresseActuel = adresse["complement_adresse"] != DBNull.Value ? adresse["complement_adresse"].ToString() : "";
                int etageActuel = adresse["étage"] != DBNull.Value ? Convert.ToInt32(adresse["étage"]) : 0;
                string numeroAppartementActuel = adresse["numero_appartement"] != DBNull.Value ? adresse["numero_appartement"].ToString() : "";

                Console.WriteLine($"\nModification de l'adresse : {idAdresse}");
                Console.WriteLine("Laissez vide pour conserver la valeur actuelle.");

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

                Dictionary<string, object> parametres = new Dictionary<string, object>
                {
                    { "@IdAdresse", idAdresse },
                    { "@CodePorte", nouveauCodePorte },
                    { "@ComplementAdresse", nouveauComplementAdresse },
                    { "@Etage", nouvelEtage },
                    { "@NumeroAppartement", nouveauNumeroAppartement }
                };

                string query = "UPDATE ADRESSE SET code_porte = @CodePorte, complement_adresse = @ComplementAdresse, " +
                              "étage = @Etage, numero_appartement = @NumeroAppartement " +
                              "WHERE id_adresse = @IdAdresse";

                int resultat = GestionBDD.ExecuterRequete(query, parametres);

                if (resultat > 0)
                {
                    GestionBDD.AfficherMessageSucces("Adresse modifiée avec succès !");
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Erreur lors de la modification de l'adresse.");
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
        /// Supprime une adresse du client connecté
        /// </summary>
        private static void SupprimerAdresse()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("SUPPRIMER UNE ADRESSE");

            Console.Write("Entrez l'ID de l'adresse à supprimer : ");
            string idAdresse = Console.ReadLine();

            try
            {
                string requeteVerif = "SELECT COUNT(*) FROM ADRESSE WHERE id_adresse = @IdAdresse AND id_client = @IdClient";
                Dictionary<string, object> parametresVerif = new Dictionary<string, object>
                {
                    { "@IdAdresse", idAdresse },
                    { "@IdClient", SessionUtilisateur.IdUtilisateur }
                };

                int count = Convert.ToInt32(GestionBDD.ExecuterScalaire(requeteVerif, parametresVerif));

                if (count == 0)
                {
                    GestionBDD.AfficherMessageErreur("Adresse introuvable ou n'appartenant pas à votre compte.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Êtes-vous sûr de vouloir supprimer cette adresse ? (O/N) : ");
                if (Console.ReadLine().ToUpper() != "O")
                {
                    Console.WriteLine("Suppression annulée.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                Dictionary<string, object> parametres = new Dictionary<string, object>
                {
                    { "@IdAdresse", idAdresse }
                };

                string query = "DELETE FROM ADRESSE WHERE id_adresse = @IdAdresse";

                int resultat = GestionBDD.ExecuterRequete(query, parametres);

                if (resultat > 0)
                {
                    GestionBDD.AfficherMessageSucces("Adresse supprimée avec succès !");
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Erreur lors de la suppression de l'adresse.");
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
    }
}
