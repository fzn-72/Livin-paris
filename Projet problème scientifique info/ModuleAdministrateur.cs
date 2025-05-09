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
    /// Classe gérant les fonctionnalités de l'administrateur
    /// </summary>
    internal class ModuleAdministrateur
    {
        //private static string connectionString = "Server=localhost;Database=livinparis2;Uid=root;Pwd=REDACTED;";
        private static string connectionString = "Server=localhost;Database=livinparis2;Uid=root;Pwd=REDACTED;";

        #region Menu principal 
        /// <summary>
        /// Affiche le menu principal du module administrateur
        /// </summary>
        public static void MenuPrincipalAdmin()
        {
            if (!SessionUtilisateur.EstAdmin())
            {
                SessionUtilisateur.AfficherMessageDroitsInsuffisants();
                return;
            }

            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("MODULE ADMINISTRATEUR");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"Bienvenue {SessionUtilisateur.PrenomUtilisateur} {SessionUtilisateur.NomUtilisateur} !");
                Console.ResetColor();
                Console.WriteLine("1. Gestion des utilisateurs");
                Console.WriteLine("2. Supervision des commandes");
                Console.WriteLine("3. Statistiques globales");
                Console.WriteLine("4. Gestion des évaluations");
                Console.WriteLine("5. Gestion des plats");
                Console.WriteLine("6. Se déconnecter");

                Console.Write("\nChoisissez une option : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        GestionUtilisateurs();
                        break;
                    case "2":
                        SupervisionCommandes();
                        break;
                    case "3":
                        ModuleStats.MenuPrincipalStats();
                        break;
                    case "4":
                        GestionEvaluations();
                        break;
                    case "5":
                        GestionPlats();
                        break;
                    case "6":
                        SessionUtilisateur.Deconnecter();
                        continuer = false;
                        Environment.Exit(0);
                        break;
                    default:
                        GestionBDD.AfficherMessageErreur("Option invalide.");
                        break;
                }
            }
        }
        #endregion

        #region Méthodes gestion utilisateurs (clients, cuisiniers, admin)
        /// <summary>
        /// Gère les utilisateurs (clients et cuisiniers)
        /// </summary>
        private static void GestionUtilisateurs()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("GESTION DES UTILISATEURS");
                Console.WriteLine("1. Gestion des clients");
                Console.WriteLine("2. Gestion des cuisiniers");
                Console.WriteLine("3. Créer un compte administrateur");
                Console.WriteLine("4. Retour au menu administrateur");

                Console.Write("\nChoisissez une option : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        ModuleProfil.GererComptesClients();
                        break;
                    case "2":
                        ModuleProfil.GererComptesCuisiniers();
                        break;
                    case "3":
                        CreerCompteAdmin();
                        break;
                    case "4":
                        continuer = false;
                        break;
                    default:
                        GestionBDD.AfficherMessageErreur("Option invalide.");
                        break;
                }
            }
        }

        /// <summary>
        /// Crée un compte administrateur
        /// </summary>
        private static void CreerCompteAdmin()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("CRÉATION DE COMPTE ADMINISTRATEUR");

            Console.Write("Nom : ");
            string nom = Console.ReadLine();

            Console.Write("Prénom : ");
            string prenom = Console.ReadLine();

            Console.Write("Email : ");
            string email = Console.ReadLine();

            Console.Write("Mot de passe : ");
            string motDePasse = Console.ReadLine();

            // Vérifier si l'email existe déjà
            string queryVerif = "SELECT COUNT(*) FROM ADMINISTRATEUR WHERE email_admin = @Email";
            Dictionary<string, object> parametresVerif = new Dictionary<string, object>
            {
                { "@Email", email }
            };

            object resultatVerif = GestionBDD.ExecuterScalaire(queryVerif, parametresVerif);
            int count = resultatVerif != null ? Convert.ToInt32(resultatVerif) : 0;

            if (count > 0)
            {
                GestionBDD.AfficherMessageErreur("Un administrateur avec cet email existe déjà.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            // Générer un nouvel ID
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

        #region Méthodes gestion commandes
        /// <summary>
        /// Supervise toutes les commandes du système
        /// </summary>
        private static void SupervisionCommandes()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("SUPERVISION DES COMMANDES");
                Console.WriteLine("1. Voir toutes les commandes");
                Console.WriteLine("2. Rechercher une commande");
                Console.WriteLine("3. Modifier le statut d'une commande");
                Console.WriteLine("4. Retour au menu administrateur");

                Console.Write("\nChoisissez une option : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        AfficherToutesCommandes();
                        break;
                    case "2":
                        ModuleCommande.RechercherCommande();
                        break;
                    case "3":
                        ModifierStatutCommande();
                        break;
                    case "4":
                        continuer = false;
                        break;
                    default:
                        GestionBDD.AfficherMessageErreur("Option invalide.");
                        break;
                }
            }
        }

        /// <summary>
        /// Affiche toutes les commandes du système
        /// </summary>
        private static void AfficherToutesCommandes()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("TOUTES LES COMMANDES");

            Console.WriteLine("Comment souhaitez-vous trier les commandes ?");
            Console.WriteLine("1. Par date (plus récentes d'abord)");
            Console.WriteLine("2. Par statut");
            Console.WriteLine("3. Par client");
            Console.Write("\nVotre choix : ");
            string choixTri = Console.ReadLine();

            string orderBy = "";
            switch (choixTri)
            {
                case "1":
                    orderBy = "c.date_commande DESC";
                    break;
                case "2":
                    orderBy = "c.statut_commande, c.date_commande DESC";
                    break;
                case "3":
                    orderBy = "cl.nom_client, cl.prenom_client, c.date_commande DESC";
                    break;
                default:
                    orderBy = "c.date_commande DESC";
                    break;
            }

            try
            {
                string requete = $@"
                    SELECT c.id_commande, c.date_commande, c.date_livraison, c.prix_total_commande, c.statut_commande,
                           cl.id_client, cl.nom_client, cl.prenom_client
                    FROM COMMANDE c
                    JOIN CLIENT cl ON c.id_client = cl.id_client
                    ORDER BY {orderBy}";

                DataTable commandes = GestionBDD.ExecuterRequeteTable(requete, null);

                if (commandes != null && commandes.Rows.Count > 0)
                {
                    Console.WriteLine("\nID\tDate commande\t\tDate livraison\t\tPrix\t\tStatut\t\tClient");
                    Console.WriteLine("-------------------------------------------------------------------------");

                    foreach (DataRow row in commandes.Rows)
                    {
                        string idCommande = row["id_commande"].ToString();
                        string dateCommande = row["date_commande"] != DBNull.Value ? Convert.ToDateTime(row["date_commande"]).ToString("dd/MM/yyyy HH:mm") : "N/A";
                        string dateLivraison = row["date_livraison"] != DBNull.Value ? Convert.ToDateTime(row["date_livraison"]).ToString("dd/MM/yyyy HH:mm") : "N/A";
                        string prix = row["prix_total_commande"] != DBNull.Value ? row["prix_total_commande"].ToString() : "N/A";
                        string statut = row["statut_commande"] != DBNull.Value ? row["statut_commande"].ToString() : "N/A";
                        string nomClient = row["nom_client"] != DBNull.Value ? row["nom_client"].ToString() : "N/A";
                        string prenomClient = row["prenom_client"] != DBNull.Value ? row["prenom_client"].ToString() : "N/A";
                        string client = $"{prenomClient} {nomClient}";

                        Console.WriteLine($"{idCommande}\t{dateCommande}\t{dateLivraison}\t{prix}\t{statut,-15}{client}");
                    }

                    Console.Write("\nVoulez-vous voir les détails d'une commande ? (O/N) : ");
                    if (Console.ReadLine().Trim().ToUpper() == "O")
                    {
                        ModuleCommande.AfficherDetailsCommande();
                    }
                }
                else
                {
                    Console.WriteLine("Aucune commande trouvée.");
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
        /// Modifie le statut d'une commande
        /// </summary>
        private static void ModifierStatutCommande()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MODIFIER LE STATUT D'UNE COMMANDE");

            Console.Write("Entrez l'ID de la commande : ");
            string idCommande = Console.ReadLine();

            try
            {
                /// Vérifier si la commande existe
                string requeteVerif = "SELECT c.id_commande, c.statut_commande, cl.nom_client, cl.prenom_client " +
                                     "FROM COMMANDE c " +
                                     "JOIN CLIENT cl ON c.id_client = cl.id_client " +
                                     "WHERE c.id_commande = @IdCommande";

                Dictionary<string, object> parametresVerif = new Dictionary<string, object>
                {
                    { "@IdCommande", idCommande }
                };

                DataTable infoCommande = GestionBDD.ExecuterRequeteTable(requeteVerif, parametresVerif);

                if (infoCommande == null || infoCommande.Rows.Count == 0)
                {
                    GestionBDD.AfficherMessageErreur("Commande introuvable.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                DataRow commande = infoCommande.Rows[0];
                string statutActuel = commande["statut_commande"] != DBNull.Value ? commande["statut_commande"].ToString() : "N/A";
                string nomClient = commande["nom_client"] != DBNull.Value ? commande["nom_client"].ToString() : "N/A";
                string prenomClient = commande["prenom_client"] != DBNull.Value ? commande["prenom_client"].ToString() : "N/A";

                Console.WriteLine($"\nCommande #{idCommande} pour {prenomClient} {nomClient}");
                Console.WriteLine($"Statut actuel : {statutActuel}");

                Console.WriteLine("\nNouveau statut :");
                Console.WriteLine("1. En attente");
                Console.WriteLine("2. En préparation");
                Console.WriteLine("3. En livraison");
                Console.WriteLine("4. Livrée");
                Console.WriteLine("5. Annulée");
                Console.Write("Votre choix : ");
                string choixStatut = Console.ReadLine();

                string nouveauStatut = "";
                switch (choixStatut)
                {
                    case "1": nouveauStatut = "En attente"; break;
                    case "2": nouveauStatut = "En préparation"; break;
                    case "3": nouveauStatut = "En livraison"; break;
                    case "4": nouveauStatut = "Livrée"; break;
                    case "5": nouveauStatut = "Annulée"; break;
                    default:
                        GestionBDD.AfficherMessageErreur("Statut invalide.");
                        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        return;
                }

                /// Mettre à jour le statut de la commande
                Dictionary<string, object> parametresUpdate = new Dictionary<string, object>
                {
                    { "@IdCommande", idCommande },
                    { "@NouveauStatut", nouveauStatut }
                };

                string requeteUpdate = "UPDATE COMMANDE SET statut_commande = @NouveauStatut WHERE id_commande = @IdCommande";
                int resultat = GestionBDD.ExecuterRequete(requeteUpdate, parametresUpdate);

                if (resultat > 0)
                {
                    /// Si la commande est marquée comme livrée, mettre à jour la date de livraison
                    if (nouveauStatut == "Livrée")
                    {
                        string requeteDateLivraison = "UPDATE COMMANDE SET date_livraison = NOW() WHERE id_commande = @IdCommande";
                        GestionBDD.ExecuterRequete(requeteDateLivraison, parametresUpdate);
                    }

                    GestionBDD.AfficherMessageSucces("Statut de la commande mis à jour avec succès !");
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Erreur lors de la mise à jour du statut de la commande.");
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

        #region Méthodes gestion évauations
        /// <summary>
        /// Gère les évaluations du système
        /// </summary>
        private static void GestionEvaluations()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("GESTION DES ÉVALUATIONS");
                Console.WriteLine("1. Voir toutes les évaluations");
                Console.WriteLine("2. Rechercher les évaluations d'un client");
                Console.WriteLine("3. Rechercher les évaluations d'un cuisinier");
                Console.WriteLine("4. Supprimer une évaluation");
                Console.WriteLine("5. Retour au menu administrateur");

                Console.Write("\nChoisissez une option : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        AfficherToutesEvaluations();
                        break;
                    case "2":
                        RechercherEvaluationsClient();
                        break;
                    case "3":
                        RechercherEvaluationsCuisinier();
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
        /// Affiche toutes les évaluations du système
        /// </summary>
        private static void AfficherToutesEvaluations()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("TOUTES LES ÉVALUATIONS");

            try
            {
                string requete = @"
                    SELECT e.id_evaluation, e.note_evaluation, e.date_evaluation, e.commentaire_evaluation,
                           cl.id_client, cl.nom_client, cl.prenom_client,
                           cu.id_cuisinier, cu.nom_cuisinier, cu.prenom_cuisinier
                    FROM EVALUATION e
                    JOIN effectue ef ON e.id_evaluation = ef.id_evaluation
                    JOIN CLIENT cl ON ef.id_client = cl.id_client
                    JOIN reçoit r ON e.id_evaluation = r.id_evaluation
                    JOIN CUISINIER cu ON r.id_cuisinier = cu.id_cuisinier
                    ORDER BY e.date_evaluation DESC";

                DataTable evaluations = GestionBDD.ExecuterRequeteTable(requete, null);

                if (evaluations != null && evaluations.Rows.Count > 0)
                {
                    Console.WriteLine("\nID\tDate\t\tNote\tClient\t\t\tCuisinier\t\tCommentaire");
                    Console.WriteLine("-------------------------------------------------------------------------");

                    foreach (DataRow row in evaluations.Rows)
                    {
                        string idEvaluation = row["id_evaluation"].ToString();
                        string dateEvaluation = row["date_evaluation"] != DBNull.Value ? Convert.ToDateTime(row["date_evaluation"]).ToString("dd/MM/yyyy") : "N/A";
                        string note = row["note_evaluation"] != DBNull.Value ? row["note_evaluation"].ToString() : "N/A";
                        string nomClient = row["nom_client"] != DBNull.Value ? row["nom_client"].ToString() : "N/A";
                        string prenomClient = row["prenom_client"] != DBNull.Value ? row["prenom_client"].ToString() : "N/A";
                        string client = $"{prenomClient} {nomClient}";
                        string nomCuisinier = row["nom_cuisinier"] != DBNull.Value ? row["nom_cuisinier"].ToString() : "N/A";
                        string prenomCuisinier = row["prenom_cuisinier"] != DBNull.Value ? row["prenom_cuisinier"].ToString() : "N/A";
                        string cuisinier = $"{prenomCuisinier} {nomCuisinier}";
                        string commentaire = row["commentaire_evaluation"] != DBNull.Value ? row["commentaire_evaluation"].ToString() : "N/A";

                        Console.WriteLine($"{idEvaluation}\t{dateEvaluation}\t{note}/10\t{client,-20}{cuisinier,-20}{commentaire}");
                    }
                }
                else
                {
                    Console.WriteLine("Aucune évaluation trouvée.");
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
        /// Recherche les évaluations d'un client spécifique
        /// </summary>
        private static void RechercherEvaluationsClient()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("RECHERCHER LES ÉVALUATIONS D'UN CLIENT");

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

            try
            {
                string requete = @"
                    SELECT e.id_evaluation, e.note_evaluation, e.date_evaluation, e.commentaire_evaluation,
                           cu.id_cuisinier, cu.nom_cuisinier, cu.prenom_cuisinier
                    FROM EVALUATION e
                    JOIN effectue ef ON e.id_evaluation = ef.id_evaluation
                    JOIN reçoit r ON e.id_evaluation = r.id_evaluation
                    JOIN CUISINIER cu ON r.id_cuisinier = cu.id_cuisinier
                    WHERE ef.id_client = @IdClient
                    ORDER BY e.date_evaluation DESC";

                Dictionary<string, object> parametres = new Dictionary<string, object>
                {
                    { "@IdClient", idClientStr }
                };

                DataTable evaluations = GestionBDD.ExecuterRequeteTable(requete, parametres);

                if (evaluations != null && evaluations.Rows.Count > 0)
                {
                    var clientDetails = GestionBDD.ObtenirDetailsClient(idClientStr);
                    Console.WriteLine($"\nÉvaluations de {clientDetails.prenom} {clientDetails.nom} :");
                    Console.WriteLine("\nID\tDate\t\tNote\tCuisinier\t\tCommentaire");
                    Console.WriteLine("-------------------------------------------------------------------------");

                    foreach (DataRow row in evaluations.Rows)
                    {
                        string idEvaluation = row["id_evaluation"].ToString();
                        string dateEvaluation = row["date_evaluation"] != DBNull.Value ? Convert.ToDateTime(row["date_evaluation"]).ToString("dd/MM/yyyy") : "N/A";
                        string note = row["note_evaluation"] != DBNull.Value ? row["note_evaluation"].ToString() : "N/A";
                        string nomCuisinier = row["nom_cuisinier"] != DBNull.Value ? row["nom_cuisinier"].ToString() : "N/A";
                        string prenomCuisinier = row["prenom_cuisinier"] != DBNull.Value ? row["prenom_cuisinier"].ToString() : "N/A";
                        string cuisinier = $"{prenomCuisinier} {nomCuisinier}";
                        string commentaire = row["commentaire_evaluation"] != DBNull.Value ? row["commentaire_evaluation"].ToString() : "N/A";

                        Console.WriteLine($"{idEvaluation}\t{dateEvaluation}\t{note}/10\t{cuisinier,-20}{commentaire}");
                    }
                }
                else
                {
                    Console.WriteLine("Aucune évaluation trouvée pour ce client.");
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
        /// Recherche les évaluations d'un cuisinier spécifique
        /// </summary>
        private static void RechercherEvaluationsCuisinier()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("RECHERCHER LES ÉVALUATIONS D'UN CUISINIER");

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

            try
            {
                string requete = @"
                    SELECT e.id_evaluation, e.note_evaluation, e.date_evaluation, e.commentaire_evaluation,
                           cl.id_client, cl.nom_client, cl.prenom_client
                    FROM EVALUATION e
                    JOIN effectue ef ON e.id_evaluation = ef.id_evaluation
                    JOIN CLIENT cl ON ef.id_client = cl.id_client
                    JOIN reçoit r ON e.id_evaluation = r.id_evaluation
                    WHERE r.id_cuisinier = @IdCuisinier
                    ORDER BY e.date_evaluation DESC";

                Dictionary<string, object> parametres = new Dictionary<string, object>
                {
                    { "@IdCuisinier", idCuisinierStr }
                };

                DataTable evaluations = GestionBDD.ExecuterRequeteTable(requete, parametres);

                if (evaluations != null && evaluations.Rows.Count > 0)
                {
                    var cuisiDetails = GestionBDD.ObtenirDetailsCuisinier(idCuisinierStr);
                    Console.WriteLine($"\nÉvaluations de {cuisiDetails.prenom} {cuisiDetails.nom} :");
                    Console.WriteLine("\nID\tDate\t\tNote\tClient\t\t\tCommentaire");
                    Console.WriteLine("-------------------------------------------------------------------------");

                    foreach (DataRow row in evaluations.Rows)
                    {
                        string idEvaluation = row["id_evaluation"].ToString();
                        string dateEvaluation = row["date_evaluation"] != DBNull.Value ? Convert.ToDateTime(row["date_evaluation"]).ToString("dd/MM/yyyy") : "N/A";
                        string note = row["note_evaluation"] != DBNull.Value ? row["note_evaluation"].ToString() : "N/A";
                        string nomClient = row["nom_client"] != DBNull.Value ? row["nom_client"].ToString() : "N/A";
                        string prenomClient = row["prenom_client"] != DBNull.Value ? row["prenom_client"].ToString() : "N/A";
                        string client = $"{prenomClient} {nomClient}";
                        string commentaire = row["commentaire_evaluation"] != DBNull.Value ? row["commentaire_evaluation"].ToString() : "N/A";

                        Console.WriteLine($"{idEvaluation}\t{dateEvaluation}\t{note}/10\t{client,-20}{commentaire}");
                    }

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
        /// Supprime une évaluation
        /// </summary>
        private static void SupprimerEvaluation()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("SUPPRIMER UNE ÉVALUATION");

            Console.Write("Entrez l'ID de l'évaluation à supprimer : ");
            string idEvaluation = Console.ReadLine();

            try
            {
                /// Vérifier si l'évaluation existe
                string requeteVerif = @"
                    SELECT e.id_evaluation, cl.nom_client, cl.prenom_client, cu.nom_cuisinier, cu.prenom_cuisinier, cu.id_cuisinier
                    FROM EVALUATION e
                    JOIN effectue ef ON e.id_evaluation = ef.id_evaluation
                    JOIN CLIENT cl ON ef.id_client = cl.id_client
                    JOIN reçoit r ON e.id_evaluation = r.id_evaluation
                    JOIN CUISINIER cu ON r.id_cuisinier = cu.id_cuisinier
                    WHERE e.id_evaluation = @IdEvaluation";

                Dictionary<string, object> parametresVerif = new Dictionary<string, object>
                {
                    { "@IdEvaluation", idEvaluation }
                };

                DataTable infoEvaluation = GestionBDD.ExecuterRequeteTable(requeteVerif, parametresVerif);

                if (infoEvaluation == null || infoEvaluation.Rows.Count == 0)
                {
                    GestionBDD.AfficherMessageErreur("Évaluation introuvable.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                DataRow evaluation = infoEvaluation.Rows[0];
                string nomClient = evaluation["nom_client"] != DBNull.Value ? evaluation["nom_client"].ToString() : "N/A";
                string prenomClient = evaluation["prenom_client"] != DBNull.Value ? evaluation["prenom_client"].ToString() : "N/A";
                string nomCuisinier = evaluation["nom_cuisinier"] != DBNull.Value ? evaluation["nom_cuisinier"].ToString() : "N/A";
                string prenomCuisinier = evaluation["prenom_cuisinier"] != DBNull.Value ? evaluation["prenom_cuisinier"].ToString() : "N/A";
                string idCuisinier = evaluation["id_cuisinier"].ToString();

                Console.WriteLine($"\nVous êtes sur le point de supprimer l'évaluation de {prenomClient} {nomClient} pour {prenomCuisinier} {nomCuisinier}.");
                Console.Write("Êtes-vous sûr ? (O/N) : ");
                if (Console.ReadLine().ToUpper() != "O")
                {
                    Console.WriteLine("Suppression annulée.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                /// Supprimer l'évaluation
                string requeteDeleteEffectue = "DELETE FROM effectue WHERE id_evaluation = @IdEvaluation";
                Dictionary<string, object> parametresDeleteEffectue = new Dictionary<string, object>
                {
                    { "@IdEvaluation", idEvaluation }
                };
                GestionBDD.ExecuterRequete(requeteDeleteEffectue, parametresDeleteEffectue);

                string requeteDeleteEvaluation = "DELETE FROM EVALUATION WHERE id_evaluation = @IdEvaluation";
                Dictionary<string, object> parametresDeleteEvaluation = new Dictionary<string, object>
                {
                    { "@IdEvaluation", idEvaluation }
                };
                int resultat = GestionBDD.ExecuterRequete(requeteDeleteEvaluation, parametresDeleteEvaluation);

                if (resultat > 0)
                {
                    GestionBDD.AfficherMessageSucces("Évaluation supprimée avec succès !");

                    /// Mettre à jour la note moyenne du cuisinier
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

                    string requeteMiseAJour = "UPDATE CUISINIER SET note_moyenne_cuisinier = @NoteMoyenne WHERE id_cuisinier = @IdCuisinier";
                    Dictionary<string, object> parametresMiseAJour = new Dictionary<string, object>
                    {
                        { "@IdCuisinier", idCuisinier },
                        { "@NoteMoyenne", noteMoyenne }
                    };

                    GestionBDD.ExecuterRequete(requeteMiseAJour, parametresMiseAJour);
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Erreur lors de la suppression de l'évaluation.");
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

        #region Méthodes gestion plats 
        /// <summary>
        /// Gère les plats du système
        /// </summary>
        private static void GestionPlats()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("GESTION DES PLATS");
                Console.WriteLine("1. Voir tous les plats");
                Console.WriteLine("2. Rechercher un plat");
                Console.WriteLine("3. Ajouter un plat");
                Console.WriteLine("4. Modifier un plat");
                Console.WriteLine("5. Supprimer un plat");
                Console.WriteLine("6. Gérer les ingrédients");
                Console.WriteLine("7. Retour au menu administrateur");

                Console.Write("\nChoisissez une option : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        AfficherTousPlats();
                        break;
                    case "2":
                        RechercherPlat();
                        break;
                    case "3":
                        ModulePlat.AjouterNouveauPlat();
                        break;
                    case "4":
                        ModifierPlat();
                        break;
                    case "5":
                        SupprimerPlat();
                        break;
                    case "6":
                        ModulePlat.GererIngredients();
                        break;
                    case "7":
                        continuer = false;
                        break;
                    default:
                        GestionBDD.AfficherMessageErreur("Option invalide.");
                        break;
                }
            }
        }

        /// <summary>
        /// Affiche tous les plats du système
        /// </summary>
        private static void AfficherTousPlats()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("TOUS LES PLATS");

            try
            {
                string requete = @"
                    SELECT p.id_plat, p.nom_plat, p.description_plat, p.type_plat, p.prix_unitaire_plat, 
                           p.regime_alimentaire_plat, p.nationalite_plat, p.date_fabrication_plat,
                           c.nom_cuisinier, c.prenom_cuisinier
                    FROM PLAT p
                    LEFT JOIN CUISINIER c ON p.id_cuisinier = c.id_cuisinier
                    ORDER BY p.type_plat, p.nom_plat";

                DataTable plats = GestionBDD.ExecuterRequeteTable(requete, null);

                if (plats != null && plats.Rows.Count > 0)
                {
                    Console.WriteLine("\nID\tNom\t\tType\t\tPrix\t\tRégime\t\tNationalité\tCuisinier");
                    Console.WriteLine("-------------------------------------------------------------------------");

                    foreach (DataRow row in plats.Rows)
                    {
                        string idPlat = row["id_plat"].ToString();
                        string nomPlat = row["nom_plat"] != DBNull.Value ? row["nom_plat"].ToString() : "N/A";
                        string typePlat = row["type_plat"] != DBNull.Value ? row["type_plat"].ToString() : "N/A";
                        string prixPlat = row["prix_unitaire_plat"] != DBNull.Value ? row["prix_unitaire_plat"].ToString() : "N/A";
                        string regimePlat = row["regime_alimentaire_plat"] != DBNull.Value ? row["regime_alimentaire_plat"].ToString() : "N/A";
                        string nationalitePlat = row["nationalite_plat"] != DBNull.Value ? row["nationalite_plat"].ToString() : "N/A";
                        string nomCuisinier = row["nom_cuisinier"] != DBNull.Value ? row["nom_cuisinier"].ToString() : "N/A";
                        string prenomCuisinier = row["prenom_cuisinier"] != DBNull.Value ? row["prenom_cuisinier"].ToString() : "N/A";
                        string cuisinier = nomCuisinier != "N/A" ? $"{prenomCuisinier} {nomCuisinier}" : "N/A";

                        Console.WriteLine($"{idPlat}\t{nomPlat,-15}{typePlat,-15}{prixPlat,-15}{regimePlat,-15}{nationalitePlat,-15}{cuisinier}");
                    }

                    Console.Write("\nVoulez-vous voir les détails d'un plat ? (O/N) : ");
                    if (Console.ReadLine().ToUpper() == "O")
                    {
                        Console.Write("Entrez l'ID du plat : ");
                        string idPlat = Console.ReadLine();
                        AfficherDetailsPlat(idPlat);
                    }
                }
                else
                {
                    Console.WriteLine("Aucun plat trouvé.");
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
        /// Affiche les détails d'un plat spécifique
        /// </summary>
        private static void AfficherDetailsPlat(string idPlat)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("DÉTAILS DU PLAT");

            try
            {
                string requete = @"
                    SELECT p.id_plat, p.nom_plat, p.description_plat, p.type_plat, p.prix_unitaire_plat, 
                           p.regime_alimentaire_plat, p.nationalite_plat, p.date_fabrication_plat, p.date_peremption_plat,
                           p.portion_dispo_plat, c.nom_cuisinier, c.prenom_cuisinier
                    FROM PLAT p
                    LEFT JOIN CUISINIER c ON p.id_cuisinier = c.id_cuisinier
                    WHERE p.id_plat = @IdPlat";

                Dictionary<string, object> parametres = new Dictionary<string, object>
                {
                    { "@IdPlat", idPlat }
                };

                DataTable platDetails = GestionBDD.ExecuterRequeteTable(requete, parametres);

                if (platDetails != null && platDetails.Rows.Count > 0)
                {
                    DataRow plat = platDetails.Rows[0];
                    string nomPlat = plat["nom_plat"] != DBNull.Value ? plat["nom_plat"].ToString() : "N/A";
                    string descriptionPlat = plat["description_plat"] != DBNull.Value ? plat["description_plat"].ToString() : "N/A";
                    string typePlat = plat["type_plat"] != DBNull.Value ? plat["type_plat"].ToString() : "N/A";
                    string prixPlat = plat["prix_unitaire_plat"] != DBNull.Value ? plat["prix_unitaire_plat"].ToString() : "N/A";
                    string regimePlat = plat["regime_alimentaire_plat"] != DBNull.Value ? plat["regime_alimentaire_plat"].ToString() : "N/A";
                    string nationalitePlat = plat["nationalite_plat"] != DBNull.Value ? plat["nationalite_plat"].ToString() : "N/A";
                    string dateFabrication = plat["date_fabrication_plat"] != DBNull.Value ? Convert.ToDateTime(plat["date_fabrication_plat"]).ToString("dd/MM/yyyy") : "N/A";
                    string datePeremption = plat["date_peremption_plat"] != DBNull.Value ? Convert.ToDateTime(plat["date_peremption_plat"]).ToString("dd/MM/yyyy") : "N/A";
                    string portionDispo = plat["portion_dispo_plat"] != DBNull.Value ? plat["portion_dispo_plat"].ToString() : "N/A";
                    string nomCuisinier = plat["nom_cuisinier"] != DBNull.Value ? plat["nom_cuisinier"].ToString() : "N/A";
                    string prenomCuisinier = plat["prenom_cuisinier"] != DBNull.Value ? plat["prenom_cuisinier"].ToString() : "N/A";
                    string cuisinier = nomCuisinier != "N/A" ? $"{prenomCuisinier} {nomCuisinier}" : "N/A";

                    Console.WriteLine($"Nom : {nomPlat}");
                    Console.WriteLine($"Description : {descriptionPlat}");
                    Console.WriteLine($"Type : {typePlat}");
                    Console.WriteLine($"Prix : {prixPlat}");
                    Console.WriteLine($"Régime alimentaire : {regimePlat}");
                    Console.WriteLine($"Nationalité : {nationalitePlat}");
                    Console.WriteLine($"Date de fabrication : {dateFabrication}");
                    Console.WriteLine($"Date de péremption : {datePeremption}");
                    Console.WriteLine($"Portions disponibles : {portionDispo}");
                    Console.WriteLine($"Cuisinier : {cuisinier}");

                    /// Afficher les ingrédients du plat
                    string requeteIngredients = @"
                        SELECT i.id_ingredient, i.nom_ingredient, i.categorie_ingredient, i.origine_ingredient
                        FROM INGREDIENT i
                        JOIN contient c ON i.id_ingredient = c.id_ingredient
                        WHERE c.id_plat = @IdPlat";

                    DataTable ingredients = GestionBDD.ExecuterRequeteTable(requeteIngredients, parametres);

                    if (ingredients != null && ingredients.Rows.Count > 0)
                    {
                        Console.WriteLine("\nIngrédients :");
                        foreach (DataRow ingredient in ingredients.Rows)
                        {
                            string nomIngredient = ingredient["nom_ingredient"] != DBNull.Value ? ingredient["nom_ingredient"].ToString() : "N/A";
                            string categorieIngredient = ingredient["categorie_ingredient"] != DBNull.Value ? ingredient["categorie_ingredient"].ToString() : "N/A";
                            string origineIngredient = ingredient["origine_ingredient"] != DBNull.Value ? ingredient["origine_ingredient"].ToString() : "N/A";

                            Console.WriteLine($"- {nomIngredient} ({categorieIngredient}, {origineIngredient})");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nAucun ingrédient trouvé pour ce plat.");
                    }
                }
                else
                {
                    Console.WriteLine("Plat introuvable.");
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
        /// Recherche un plat selon différents critères
        /// </summary>
        private static void RechercherPlat()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("RECHERCHER UN PLAT");

            Console.WriteLine("Rechercher par :");
            Console.WriteLine("1. Nom");
            Console.WriteLine("2. Type");
            Console.WriteLine("3. Régime alimentaire");
            Console.WriteLine("4. Nationalité");
            Console.WriteLine("5. Cuisinier");
            Console.Write("\nVotre choix : ");
            string choixRecherche = Console.ReadLine();

            string critere = "";
            string valeur = "";

            switch (choixRecherche)
            {
                case "1":
                    critere = "p.nom_plat";
                    Console.Write("Entrez le nom du plat : ");
                    valeur = Console.ReadLine();
                    break;
                case "2":
                    critere = "p.type_plat";
                    Console.Write("Entrez le type de plat : ");
                    valeur = Console.ReadLine();
                    break;
                case "3":
                    critere = "p.regime_alimentaire_plat";
                    Console.Write("Entrez le régime alimentaire : ");
                    valeur = Console.ReadLine();
                    break;
                case "4":
                    critere = "p.nationalite_plat";
                    Console.Write("Entrez la nationalité : ");
                    valeur = Console.ReadLine();
                    break;
                case "5":
                    critere = "CONCAT(c.prenom_cuisinier, ' ', c.nom_cuisinier)";
                    Console.Write("Entrez le nom du cuisinier : ");
                    valeur = Console.ReadLine();
                    break;
                default:
                    GestionBDD.AfficherMessageErreur("Option invalide.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
            }

            try
            {
                string requete = $@"
                    SELECT p.id_plat, p.nom_plat, p.type_plat, p.prix_unitaire_plat, 
                           p.regime_alimentaire_plat, p.nationalite_plat,
                           c.nom_cuisinier, c.prenom_cuisinier
                    FROM PLAT p
                    LEFT JOIN CUISINIER c ON p.id_cuisinier = c.id_cuisinier
                    WHERE {critere} LIKE @Valeur
                    ORDER BY p.type_plat, p.nom_plat";

                Dictionary<string, object> parametres = new Dictionary<string, object>
                {
                    { "@Valeur", $"%{valeur}%" }
                };

                DataTable plats = GestionBDD.ExecuterRequeteTable(requete, parametres);

                if (plats != null && plats.Rows.Count > 0)
                {
                    Console.WriteLine("\nRésultats de la recherche :");
                    Console.WriteLine("ID\tNom\t\tType\t\tPrix\t\tRégime\t\tNationalité\tCuisinier");
                    Console.WriteLine("-------------------------------------------------------------------------");

                    foreach (DataRow row in plats.Rows)
                    {
                        string idPlat = row["id_plat"].ToString();
                        string nomPlat = row["nom_plat"] != DBNull.Value ? row["nom_plat"].ToString() : "N/A";
                        string typePlat = row["type_plat"] != DBNull.Value ? row["type_plat"].ToString() : "N/A";
                        string prixPlat = row["prix_unitaire_plat"] != DBNull.Value ? row["prix_unitaire_plat"].ToString() : "N/A";
                        string regimePlat = row["regime_alimentaire_plat"] != DBNull.Value ? row["regime_alimentaire_plat"].ToString() : "N/A";
                        string nationalitePlat = row["nationalite_plat"] != DBNull.Value ? row["nationalite_plat"].ToString() : "N/A";
                        string nomCuisinier = row["nom_cuisinier"] != DBNull.Value ? row["nom_cuisinier"].ToString() : "N/A";
                        string prenomCuisinier = row["prenom_cuisinier"] != DBNull.Value ? row["prenom_cuisinier"].ToString() : "N/A";
                        string cuisinier = nomCuisinier != "N/A" ? $"{prenomCuisinier} {nomCuisinier}" : "N/A";

                        Console.WriteLine($"{idPlat}\t{nomPlat,-15}{typePlat,-15}{prixPlat,-15}{regimePlat,-15}{nationalitePlat,-15}{cuisinier}");
                    }

                    Console.Write("\nVoulez-vous voir les détails d'un plat ? (O/N) : ");
                    if (Console.ReadLine().ToUpper() == "O")
                    {
                        Console.Write("Entrez l'ID du plat : ");
                        string idPlat = Console.ReadLine();
                        AfficherDetailsPlat(idPlat);
                    }
                }
                else
                {
                    Console.WriteLine("Aucun plat trouvé avec ce critère.");
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
        /// Modifie un plat existant
        /// </summary>
        private static void ModifierPlat()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MODIFIER UN PLAT");

            Console.Write("Entrez l'ID du plat à modifier : ");
            string idPlat = Console.ReadLine();

            try
            {
                /// Vérifier si le plat existe
                string requeteVerif = "SELECT * FROM PLAT WHERE id_plat = @IdPlat";
                Dictionary<string, object> parametresVerif = new Dictionary<string, object>
                {
                    { "@IdPlat", idPlat }
                };

                DataTable platDetails = GestionBDD.ExecuterRequeteTable(requeteVerif, parametresVerif);

                if (platDetails == null || platDetails.Rows.Count == 0)
                {
                    GestionBDD.AfficherMessageErreur("Plat introuvable.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                DataRow plat = platDetails.Rows[0];
                string nomActuel = plat["nom_plat"] != DBNull.Value ? plat["nom_plat"].ToString() : "";
                string descriptionActuelle = plat["description_plat"] != DBNull.Value ? plat["description_plat"].ToString() : "";
                string typeActuel = plat["type_plat"] != DBNull.Value ? plat["type_plat"].ToString() : "";
                string prixActuel = plat["prix_unitaire_plat"] != DBNull.Value ? plat["prix_unitaire_plat"].ToString() : "";
                string regimeActuel = plat["regime_alimentaire_plat"] != DBNull.Value ? plat["regime_alimentaire_plat"].ToString() : "";
                string nationaliteActuelle = plat["nationalite_plat"] != DBNull.Value ? plat["nationalite_plat"].ToString() : "";
                string portionDispoActuelle = plat["portion_dispo_plat"] != DBNull.Value ? plat["portion_dispo_plat"].ToString() : "";
                string dateFabricationActuelle = plat["date_fabrication_plat"] != DBNull.Value ? Convert.ToDateTime(plat["date_fabrication_plat"]).ToString("yyyy-MM-dd") : "";
                string datePeremptionActuelle = plat["date_peremption_plat"] != DBNull.Value ? Convert.ToDateTime(plat["date_peremption_plat"]).ToString("yyyy-MM-dd") : "";

                Console.WriteLine($"\nModification du plat : {nomActuel}");
                Console.WriteLine("Laissez vide pour conserver la valeur actuelle.");

                Console.Write($"Nouveau nom ({nomActuel}) : ");
                string nouveauNom = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nouveauNom))
                    nouveauNom = nomActuel;

                Console.Write($"Nouvelle description ({descriptionActuelle}) : ");
                string nouvelleDescription = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nouvelleDescription))
                    nouvelleDescription = descriptionActuelle;

                Console.Write($"Nouveau type ({typeActuel}) : ");
                string nouveauType = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nouveauType))
                    nouveauType = typeActuel;

                Console.Write($"Nouveau prix ({prixActuel}) : ");
                string nouveauPrix = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nouveauPrix))
                    nouveauPrix = prixActuel;

                Console.Write($"Nouveau régime alimentaire ({regimeActuel}) : ");
                string nouveauRegime = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nouveauRegime))
                    nouveauRegime = regimeActuel;

                Console.Write($"Nouvelle nationalité ({nationaliteActuelle}) : ");
                string nouvelleNationalite = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nouvelleNationalite))
                    nouvelleNationalite = nationaliteActuelle;

                Console.Write($"Nouvelle portion disponible ({portionDispoActuelle}) : ");
                string nouvellePortionDispo = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nouvellePortionDispo))
                    nouvellePortionDispo = portionDispoActuelle;

                Console.Write($"Nouvelle date de fabrication ({dateFabricationActuelle}) : ");
                string nouvelleDateFabrication = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nouvelleDateFabrication))
                    nouvelleDateFabrication = dateFabricationActuelle;

                Console.Write($"Nouvelle date de péremption ({datePeremptionActuelle}) : ");
                string nouvelleDatePeremption = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nouvelleDatePeremption))
                    nouvelleDatePeremption = datePeremptionActuelle;

                /// Mettre à jour le plat
                string requeteUpdate = @"
                    UPDATE PLAT SET 
                        nom_plat = @NomPlat, 
                        description_plat = @DescriptionPlat, 
                        type_plat = @TypePlat, 
                        prix_unitaire_plat = @PrixUnitaire, 
                        regime_alimentaire_plat = @RegimeAlimentaire, 
                        nationalite_plat = @Nationalite, 
                        portion_dispo_plat = @PortionDispo, 
                        date_fabrication_plat = @DateFabrication, 
                        date_peremption_plat = @DatePeremption 
                    WHERE id_plat = @IdPlat";

                Dictionary<string, object> parametresUpdate = new Dictionary<string, object>
                {
                    { "@IdPlat", idPlat },
                    { "@NomPlat", nouveauNom },
                    { "@DescriptionPlat", nouvelleDescription },
                    { "@TypePlat", nouveauType },
                    { "@PrixUnitaire", nouveauPrix },
                    { "@RegimeAlimentaire", nouveauRegime },
                    { "@Nationalite", nouvelleNationalite },
                    { "@PortionDispo", nouvellePortionDispo },
                    { "@DateFabrication", nouvelleDateFabrication },
                    { "@DatePeremption", nouvelleDatePeremption }
                };

                int resultat = GestionBDD.ExecuterRequete(requeteUpdate, parametresUpdate);

                if (resultat > 0)
                {
                    GestionBDD.AfficherMessageSucces("Plat modifié avec succès !");
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Erreur lors de la modification du plat.");
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
        /// Supprime un plat existant
        /// </summary>
        private static void SupprimerPlat()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("SUPPRIMER UN PLAT");

            Console.Write("Entrez l'ID du plat à supprimer : ");
            string idPlat = Console.ReadLine();

            try
            {
                /// Vérifier si le plat existe
                string requeteVerif = "SELECT nom_plat FROM PLAT WHERE id_plat = @IdPlat";
                Dictionary<string, object> parametresVerif = new Dictionary<string, object>
                {
                    { "@IdPlat", idPlat }
                };

                object nomPlat = GestionBDD.ExecuterScalaire(requeteVerif, parametresVerif);

                if (nomPlat == null || nomPlat == DBNull.Value)
                {
                    GestionBDD.AfficherMessageErreur("Plat introuvable.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine($"\nVous êtes sur le point de supprimer le plat : {nomPlat}");
                Console.Write("Êtes-vous sûr ? (O/N) : ");
                if (Console.ReadLine().ToUpper() != "O")
                {
                    Console.WriteLine("Suppression annulée.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                /// Vérifier si le plat est utilisé dans des commandes
                string requeteVerifCommandes = "SELECT COUNT(*) FROM LIGNE_COMMANDE WHERE id_plat = @IdPlat";
                Dictionary<string, object> parametresVerifCommandes = new Dictionary<string, object>
                {
                    { "@IdPlat", idPlat }
                };

                int nbCommandes = Convert.ToInt32(GestionBDD.ExecuterScalaire(requeteVerifCommandes, parametresVerifCommandes));

                if (nbCommandes > 0)
                {
                    Console.WriteLine($"Ce plat est utilisé dans {nbCommandes} commande(s).");
                    Console.Write("Voulez-vous quand même le supprimer ? (O/N) : ");
                    if (Console.ReadLine().ToUpper() != "O")
                    {
                        Console.WriteLine("Suppression annulée.");
                        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        return;
                    }

                    /// Supprimer les références dans LIGNE_COMMANDE
                    string requeteDeleteLignes = "DELETE FROM LIGNE_COMMANDE WHERE id_plat = @IdPlat";
                    Dictionary<string, object> parametresDeleteLignes = new Dictionary<string, object>
                    {
                        { "@IdPlat", idPlat }
                    };
                    GestionBDD.ExecuterRequete(requeteDeleteLignes, parametresDeleteLignes);
                }

                /// Supprimer les références dans CONTIENT
                string requeteDeleteContient = "DELETE FROM contient WHERE id_plat = @IdPlat";
                Dictionary<string, object> parametresDeleteContient = new Dictionary<string, object>
                {
                    { "@IdPlat", idPlat }
                };
                GestionBDD.ExecuterRequete(requeteDeleteContient, parametresDeleteContient);

                /// Supprimer les références dans APPARTIENT
                string requeteDeleteAppartient = "DELETE FROM appartient WHERE id_plat = @IdPlat";
                Dictionary<string, object> parametresDeleteAppartient = new Dictionary<string, object>
                {
                    { "@IdPlat", idPlat }
                };
                GestionBDD.ExecuterRequete(requeteDeleteAppartient, parametresDeleteAppartient);

                /// Supprimer finalement le plat
                string requeteDeletePlat = "DELETE FROM PLAT WHERE id_plat = @IdPlat";
                Dictionary<string, object> parametresDeletePlat = new Dictionary<string, object>
                {
                    { "@IdPlat", idPlat }
                };
                int resultat = GestionBDD.ExecuterRequete(requeteDeletePlat, parametresDeletePlat);

                if (resultat > 0)
                {
                    GestionBDD.AfficherMessageSucces("Plat supprimé avec succès !");
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Erreur lors de la suppression du plat.");
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
