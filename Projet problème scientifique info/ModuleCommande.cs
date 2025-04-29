using MySql.Data.MySqlClient;
using Projet_problème_scientifique_info;
using System;
using System.Collections.Generic;
using System.Data;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Classe gérant les fonctionnalités liées aux commandes
    /// </summary>
    internal class ModuleCommande
    {
        private static string connectionString = "Server=localhost;Database=livinparis2;Uid=root;Pwd=REDACTED;";

        /// <summary>
        /// Affiche le menu principal du module commande
        /// </summary>
        public static void MenuPrincipalCommandes()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("MODULE COMMANDES");

                // Afficher différentes options selon le type d'utilisateur
                if (SessionUtilisateur.TypeUtilisateur == "client")
                {
                    Console.WriteLine("1. Créer une nouvelle commande");
                    Console.WriteLine("2. Voir mes commandes");
                    Console.WriteLine("3. Afficher les détails d'une commande");
                    Console.WriteLine("4. Annuler une commande");
                    Console.WriteLine("5. Évaluer un cuisinier");
                    Console.WriteLine("6. Retour au menu principal");
                }
                else if (SessionUtilisateur.TypeUtilisateur == "cuisinier")
                {
                    Console.WriteLine("1. Voir les commandes à préparer");
                    Console.WriteLine("2. Afficher les détails d'une commande");
                    Console.WriteLine("3. Mettre à jour le statut d'une ligne de commande");
                    Console.WriteLine("4. Voir mes évaluations");
                    Console.WriteLine("5. Voir les statistiques de mes plats");
                    Console.WriteLine("6. Retour au menu principal");
                }
                else if (SessionUtilisateur.TypeUtilisateur == "admin")
                {
                    Console.WriteLine("1. Créer une nouvelle commande");
                    Console.WriteLine("2. Rechercher une commande");
                    Console.WriteLine("3. Afficher les détails d'une commande");
                    Console.WriteLine("4. Modifier une commande");
                    Console.WriteLine("5. Supprimer une commande");
                    Console.WriteLine("6. Gérer les lignes de commande");
                    Console.WriteLine("7. Suivre l'état des commandes");
                    Console.WriteLine("8. Assigner un cuisinier à une ligne de commande");
                    Console.WriteLine("9. Calculer le prix total d'une commande");
                    Console.WriteLine("10. Retour au menu principal");
                }
                else
                {
                    // Utilisateur non connecté ou type inconnu
                    Console.WriteLine("1. Consulter les plats disponibles");
                    Console.WriteLine("2. Retour au menu principal");
                }

                Console.Write("\nChoisissez une option : ");
                string choix = Console.ReadLine();

                if (SessionUtilisateur.TypeUtilisateur == "client")
                {
                    switch (choix)
                    {
                        case "1":
                            CreerNouvelleCommandeClient();
                            break;
                        case "2":
                            VoirCommandesClient();
                            break;
                        case "3":
                            AfficherDetailsCommande();
                            break;
                        case "4":
                            AnnulerCommandeClient();
                            break;
                        case "5":
                            EvaluerCuisinier();
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
                    switch (choix)
                    {
                        case "1":
                            VoirCommandesAPreparer();
                            break;
                        case "2":
                            AfficherDetailsCommande();
                            break;
                        case "3":
                            MettreAJourStatutLigneCommande();
                            break;
                        case "4":
                            VoirEvaluationsCuisinier();
                            break;
                        case "5":
                            VoirStatistiquesPlats();
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
                    switch (choix)
                    {
                        case "1":
                            CreerNouvelleCommande();
                            break;
                        case "2":
                            RechercherCommande();
                            break;
                        case "3":
                            AfficherDetailsCommande();
                            break;
                        case "4":
                            ModifierCommande();
                            break;
                        case "5":
                            SupprimerCommande();
                            break;
                        case "6":
                            GererLignesCommande();
                            break;
                        case "7":
                            SuivreEtatCommandes();
                            break;
                        case "8":
                            AssignerCuisinierLigneCommande();
                            break;
                        case "9":
                            CalculerPrixTotalCommande();
                            break;
                        case "10":
                            continuer = false;
                            break;
                        default:
                            GestionBDD.AfficherMessageErreur("Option invalide.");
                            break;
                    }
                }
                else
                {
                    switch (choix)
                    {
                        case "1":
                            //ModulePlat.AfficherPlatsDisponibles();
                            ModulePlat.ConsulterPlatsDisponibles();
                            break;
                        case "2":
                            continuer = false;
                            break;
                        default:
                            GestionBDD.AfficherMessageErreur("Option invalide.");
                            break;
                    }
                }
            }
        }

        #region Fonctionnalités pour les clients
        /// <summary>
        /// Permet à un client de créer une nouvelle commande
        /// </summary>
        private static void CreerNouvelleCommandeClient()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("CRÉER UNE NOUVELLE COMMANDE");

            // Vérifier que l'utilisateur est connecté en tant que client
            if (!SessionUtilisateur.EstConnecte() || SessionUtilisateur.TypeUtilisateur != "client")
            {
                GestionBDD.AfficherMessageErreur("Vous devez être connecté en tant que client pour créer une commande.");
                return;
            }

            // Récupérer l'ID du client connecté
            string idClient = SessionUtilisateur.IdUtilisateur;

            // Générer un nouvel ID de commande
            int idCommande = GestionBDD.GenererNouvelID("COMMANDE", "id_commande");

            // Saisir les informations de la commande
            Console.Write("Commentaire (facultatif) : ");
            string commentaire = Console.ReadLine();

            // Créer la commande
            Dictionary<string, object> parametresCommande = new Dictionary<string, object>
          {
              { "@IdCommande", idCommande },
              { "@Commentaire", commentaire },
              { "@Statut", "En attente" },
              { "@IdClient", idClient }
          };

            string queryCommande = "INSERT INTO COMMANDE (id_commande, date_commande, commentaire_commande, statut_commande, id_client) " +
                                  "VALUES (@IdCommande, NOW(), @Commentaire, @Statut, @IdClient)";

            int resultatCommande = GestionBDD.ExecuterRequete(queryCommande, parametresCommande);

            if (resultatCommande > 0)
            {
                GestionBDD.AfficherMessageSucces($"Commande #{idCommande} créée avec succès !");

                // Ajouter des lignes de commande
                Console.Write("Voulez-vous ajouter des plats à cette commande maintenant ? (O/N) : ");
                if (Console.ReadLine().ToUpper() == "O")
                {
                    AjouterLignesCommande(idCommande.ToString());
                }
            }
            else
            {
                GestionBDD.AfficherMessageErreur("Erreur lors de la création de la commande.");
            }
        }

        /// <summary>
        /// Ajoute une ou plusieurs lignes à une commande existante
        /// </summary>
        private static void AjouterLignesCommande(string idCommande)
        {
            bool continuerAjout = true;

            while (continuerAjout)
            {
                AjouterLigneCommande(idCommande);

                Console.Write("\nVoulez-vous ajouter une autre ligne ? (O/N) : ");
                continuerAjout = Console.ReadLine().ToUpper() == "O";
            }

            // Recalculer le prix total de la commande
            decimal nouveauPrix = CalculerPrixTotal(idCommande);
            if (nouveauPrix >= 0)
            {
                Dictionary<string, object> parametresPrix = new Dictionary<string, object>
              {
                  { "@IdCommande", idCommande },
                  { "@NouveauPrix", nouveauPrix + " euros" }
              };

                string requetePrix = "UPDATE COMMANDE SET prix_total_commande = @NouveauPrix WHERE id_commande = @IdCommande";
                GestionBDD.ExecuterRequete(requetePrix, parametresPrix);
            }
        }

        /// <summary>
        /// Ajoute une ligne à une commande
        /// </summary>
        private static void AjouterLigneCommande(string idCommande)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("AJOUTER UNE LIGNE DE COMMANDE");

            // Afficher les plats disponibles
            string requetePlats = "SELECT id_plat, nom_plat, type_plat, prix_unitaire_plat, nationalite_plat, " +
                                 "regime_alimentaire_plat FROM PLAT ORDER BY type_plat, nom_plat";
            DataTable plats = GestionBDD.ExecuterRequeteTable(requetePlats, new Dictionary<string, object>());

            if (plats == null || plats.Rows.Count == 0)
            {
                GestionBDD.AfficherMessageErreur("Aucun plat disponible.");
                return;
            }

            Console.WriteLine("Plats disponibles :");
            Console.WriteLine("ID\tNom\t\tType\t\tPrix\t\tNationalité\tRégime");
            Console.WriteLine("-------------------------------------------------------------------------");

            foreach (DataRow plat in plats.Rows)
            {
                string idPlat1 = plat["id_plat"].ToString();
                string nomPlat = plat["nom_plat"] == DBNull.Value ? "N/A" : plat["nom_plat"].ToString();
                string typePlat = plat["type_plat"] == DBNull.Value ? "N/A" : plat["type_plat"].ToString();
                string prixPlat = plat["prix_unitaire_plat"] == DBNull.Value ? "N/A" : plat["prix_unitaire_plat"].ToString();
                string nationalite = plat["nationalite_plat"] == DBNull.Value ? "N/A" : plat["nationalite_plat"].ToString();
                string regime = plat["regime_alimentaire_plat"] == DBNull.Value ? "N/A" : plat["regime_alimentaire_plat"].ToString();

                Console.WriteLine($"{idPlat1}\t{nomPlat,-15}{typePlat,-15}{prixPlat}\t{nationalite,-15}{regime}");
            }

            Console.Write("\nID du plat à ajouter : ");
            string idPlat = Console.ReadLine();

            // Vérifier si le plat existe
            string requeteVerifPlat = "SELECT COUNT(*) FROM PLAT WHERE id_plat = @IdPlat";
            Dictionary<string, object> parametresVerifPlat = new Dictionary<string, object>
          {
              { "@IdPlat", idPlat }
          };

            object resultatVerifPlat = GestionBDD.ExecuterScalaire(requeteVerifPlat, parametresVerifPlat);
            if (resultatVerifPlat == null || Convert.ToInt32(resultatVerifPlat) == 0)
            {
                GestionBDD.AfficherMessageErreur("Plat introuvable.");
                return;
            }

            // Récupérer le prix unitaire du plat
            string requetePrixPlat = "SELECT prix_unitaire_plat FROM PLAT WHERE id_plat = @IdPlat";
            Dictionary<string, object> parametresPrixPlat = new Dictionary<string, object>
          {
              { "@IdPlat", idPlat }
          };

            object resultatPrixPlat = GestionBDD.ExecuterScalaire(requetePrixPlat, parametresPrixPlat);
            string prixUnitaire = resultatPrixPlat == null ? "0" : resultatPrixPlat.ToString();

            Console.Write("Date de livraison souhaitée (YYYY-MM-DD) : ");
            string dateLivraisonVoulue = Console.ReadLine();

            Console.Write("Métro le plus proche pour la livraison : ");
            string metroLivraison = Console.ReadLine();

            // Générer un nouvel ID de ligne
            int idLigne = GestionBDD.GenererNouvelID("LIGNE_COMMANDE", "id_ligne");

            // Ajouter la ligne de commande
            Dictionary<string, object> parametresLigne = new Dictionary<string, object>
          {
              { "@IdLigne", idLigne },
              { "@IdCommande", idCommande },
              { "@IdPlat", idPlat },
              { "@PrixUnitaire", prixUnitaire },
              { "@DateLivraisonVoulue", dateLivraisonVoulue },
              { "@MetroLivraison", metroLivraison },
              { "@StatusLigne", "En attente" }
          };

            string requeteLigne = "INSERT INTO LIGNE_COMMANDE (id_ligne, id_commande, id_plat, prix_unitaire, " +
                                 "date_livraison_voulue, metro_le_plus_proche_livraison, statut_ligne) " +
                                 "VALUES (@IdLigne, @IdCommande, @IdPlat, @PrixUnitaire, " +
                                 "@DateLivraisonVoulue, @MetroLivraison, @StatusLigne)";

            int resultat = GestionBDD.ExecuterRequete(requeteLigne, parametresLigne);

            if (resultat > 0)
            {
                // Ajouter également dans la table appartient
                Dictionary<string, object> parametresAppartient = new Dictionary<string, object>
              {
                  { "@IdPlat", idPlat },
                  { "@IdCommande", idCommande }
              };

                string requeteAppartient = "INSERT INTO appartient (id_plat, id_commande) VALUES (@IdPlat, @IdCommande)";
                GestionBDD.ExecuterRequete(requeteAppartient, parametresAppartient);

                GestionBDD.AfficherMessageSucces("Ligne de commande ajoutée avec succès !");
            }
            else
            {
                GestionBDD.AfficherMessageErreur("Erreur lors de l'ajout de la ligne de commande.");
            }
        }


        /// <summary>
        /// Permet de calculer le prix total d'une commande
        /// </summary>
        private static void CalculerPrixTotalCommande()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("CALCULER LE PRIX TOTAL D'UNE COMMANDE");

            Console.Write("ID de la commande : ");
            string idCommande = Console.ReadLine();

            try
            {
                decimal prixTotal = CalculerPrixTotal(idCommande);
                if (prixTotal >= 0)
                {
                    Console.WriteLine($"\nPrix total de la commande #{idCommande} : {prixTotal} euros");

                    Console.Write("\nVoulez-vous mettre à jour le prix total dans la base de données ? (O/N) : ");
                    if (Console.ReadLine().ToUpper() == "O")
                    {
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();
                            string requetePrix = "UPDATE COMMANDE SET prix_total_commande = @NouveauPrix WHERE id_commande = @IdCommande";
                            MySqlCommand cmdPrix = new MySqlCommand(requetePrix, connection);
                            cmdPrix.Parameters.AddWithValue("@IdCommande", idCommande);
                            cmdPrix.Parameters.AddWithValue("@NouveauPrix", prixTotal + " euros");
                            int resultat = cmdPrix.ExecuteNonQuery();
                            if (resultat > 0)
                            {
                                GestionBDD.AfficherMessageSucces("Prix total mis à jour avec succès !");
                            }
                            else
                            {
                                GestionBDD.AfficherMessageErreur("Erreur lors de la mise à jour du prix total.");
                            }
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
        /// Calcule le prix total d'une commande
        /// </summary>
        private static decimal CalculerPrixTotal(string idCommande)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string requete = "SELECT SUM(prix_unitaire) FROM LIGNE_COMMANDE WHERE id_commande = @IdCommande";
                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@IdCommande", idCommande);

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToDecimal(result);
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur lors du calcul du prix total : {ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// Affiche les commandes du client connecté
        /// </summary>
        private static void VoirCommandesClient()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MES COMMANDES");

            // Vérifier que l'utilisateur est connecté en tant que client
            if (!SessionUtilisateur.EstConnecte() || SessionUtilisateur.TypeUtilisateur != "client")
            {
                GestionBDD.AfficherMessageErreur("Vous devez être connecté en tant que client pour voir vos commandes.");
                return;
            }

            string idClient = SessionUtilisateur.IdUtilisateur;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete = "SELECT c.id_commande, c.date_commande, c.date_livraison, c.statut_commande, c.prix_total_commande, " +
                                    "COUNT(lc.id_ligne) AS nombre_plats " +
                                    "FROM COMMANDE c " +
                                    "LEFT JOIN LIGNE_COMMANDE lc ON c.id_commande = lc.id_commande " +
                                    "WHERE c.id_client = @IdClient " +
                                    "GROUP BY c.id_commande, c.date_commande, c.date_livraison, c.statut_commande, c.prix_total_commande " +
                                    "ORDER BY c.date_commande DESC";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@IdClient", idClient);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("\nID\tDate commande\tDate livraison\tStatut\t\tPrix total\tNombre de plats");
                            Console.WriteLine("-------------------------------------------------------------------------");

                            while (reader.Read())
                            {
                                string id = reader["id_commande"].ToString();
                                string dateCommande = reader["date_commande"] == DBNull.Value ? "N/A" : Convert.ToDateTime(reader["date_commande"]).ToString("dd/MM/yyyy");
                                string dateLivraison = reader["date_livraison"] == DBNull.Value ? "N/A" : Convert.ToDateTime(reader["date_livraison"]).ToString("dd/MM/yyyy");
                                string statut = reader["statut_commande"] == DBNull.Value ? "N/A" : reader["statut_commande"].ToString();
                                string prixTotal = reader["prix_total_commande"] == DBNull.Value ? "N/A" : reader["prix_total_commande"].ToString();
                                string nombrePlats = reader["nombre_plats"].ToString();

                                Console.WriteLine($"{id}\t{dateCommande}\t{dateLivraison}\t{statut,-15}{prixTotal,-15}{nombrePlats}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Vous n'avez pas encore de commandes.");
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
        /// Permet à un client d'annuler une de ses commandes
        /// </summary>
        private static void AnnulerCommandeClient()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("ANNULER UNE COMMANDE");

            // Vérifier que l'utilisateur est connecté en tant que client
            if (!SessionUtilisateur.EstConnecte() || SessionUtilisateur.TypeUtilisateur != "client")
            {
                GestionBDD.AfficherMessageErreur("Vous devez être connecté en tant que client pour annuler une commande.");
                return;
            }

            string idClient = SessionUtilisateur.IdUtilisateur;

            // Afficher les commandes du client qui peuvent être annulées
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete = "SELECT c.id_commande, c.date_commande, c.statut_commande, c.prix_total_commande " +
                                    "FROM COMMANDE c " +
                                    "WHERE c.id_client = @IdClient " +
                                    "AND c.statut_commande IN ('En attente', 'Confirmée') " +
                                    "ORDER BY c.date_commande DESC";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@IdClient", idClient);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("\nCommandes pouvant être annulées :");
                            Console.WriteLine("ID\tDate commande\tStatut\t\tPrix total");
                            Console.WriteLine("-----------------------------------------------------");

                            while (reader.Read())
                            {
                                string id = reader["id_commande"].ToString();
                                string dateCommande = reader["date_commande"] == DBNull.Value ? "N/A" : Convert.ToDateTime(reader["date_commande"]).ToString("dd/MM/yyyy");
                                string statut1 = reader["statut_commande"] == DBNull.Value ? "N/A" : reader["statut_commande"].ToString();
                                string prixTotal = reader["prix_total_commande"] == DBNull.Value ? "N/A" : reader["prix_total_commande"].ToString();

                                Console.WriteLine($"{id}\t{dateCommande}\t{statut1,-15}{prixTotal}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Vous n'avez pas de commandes pouvant être annulées.");
                            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                            Console.ReadKey();
                            return;
                        }
                    }

                    Console.Write("\nID de la commande à annuler : ");
                    string idCommande = Console.ReadLine();

                    // Vérifier que la commande appartient bien au client
                    string requeteVerif = "SELECT COUNT(*) FROM COMMANDE WHERE id_commande = @IdCommande AND id_client = @IdClient";
                    MySqlCommand cmdVerif = new MySqlCommand(requeteVerif, connection);
                    cmdVerif.Parameters.AddWithValue("@IdCommande", idCommande);
                    cmdVerif.Parameters.AddWithValue("@IdClient", idClient);

                    int count = Convert.ToInt32(cmdVerif.ExecuteScalar());
                    if (count == 0)
                    {
                        GestionBDD.AfficherMessageErreur("Cette commande n'existe pas ou ne vous appartient pas.");
                        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        return;
                    }

                    // Vérifier que la commande peut être annulée
                    string requeteStatut = "SELECT statut_commande FROM COMMANDE WHERE id_commande = @IdCommande";
                    MySqlCommand cmdStatut = new MySqlCommand(requeteStatut, connection);
                    cmdStatut.Parameters.AddWithValue("@IdCommande", idCommande);

                    string statut = cmdStatut.ExecuteScalar().ToString();
                    if (statut != "En attente" && statut != "Confirmée")
                    {
                        GestionBDD.AfficherMessageErreur("Cette commande ne peut plus être annulée.");
                        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        return;
                    }

                    // Demander confirmation
                    Console.Write("Êtes-vous sûr de vouloir annuler cette commande ? (O/N) : ");
                    if (Console.ReadLine().ToUpper() != "O")
                    {
                        Console.WriteLine("Annulation de la commande annulée.");
                        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        return;
                    }

                    // Annuler la commande
                    string requeteAnnulation = "UPDATE COMMANDE SET statut_commande = 'Annulée' WHERE id_commande = @IdCommande";
                    MySqlCommand cmdAnnulation = new MySqlCommand(requeteAnnulation, connection);
                    cmdAnnulation.Parameters.AddWithValue("@IdCommande", idCommande);

                    int resultat = cmdAnnulation.ExecuteNonQuery();
                    if (resultat > 0)
                    {
                        GestionBDD.AfficherMessageSucces("Commande annulée avec succès !");
                    }
                    else
                    {
                        GestionBDD.AfficherMessageErreur("Erreur lors de l'annulation de la commande.");
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
        /// Permet à un client d'évaluer un cuisinier
        /// </summary>
        private static void EvaluerCuisinier()
        {

            if (!SessionUtilisateur.EstConnecte() || SessionUtilisateur.TypeUtilisateur != "client")
            {
                GestionBDD.AfficherMessageErreur("Vous devez être connecté en tant que client pour évaluer un cuisinier.");
                return;
            }

            Console.Clear();
            GestionBDD.AfficherTitre("ÉVALUER UN CUISINIER");

            try
            {
                // Récupérer les commandes livrées du client
                string requeteCommandes = @"
                    SELECT DISTINCT c.id_commande, c.date_commande, c.date_livraison, c.statut_commande,
                           cu.id_cuisinier, cu.nom_cuisinier, cu.prenom_cuisinier
                    FROM COMMANDE c
                    JOIN LIGNE_COMMANDE lc ON c.id_commande = lc.id_commande
                    JOIN CUISINIER cu ON lc.id_cuisinier = cu.id_cuisinier
                    WHERE c.id_client = @IdClient AND c.statut_commande = 'Livrée'
                    ORDER BY c.date_livraison DESC";

                Dictionary<string, object> parametresCommandes = new Dictionary<string, object>
                {
                    { "@IdClient", SessionUtilisateur.IdUtilisateur }
                };

                DataTable commandes = GestionBDD.ExecuterRequeteTable(requeteCommandes, parametresCommandes);

                if (commandes == null || commandes.Rows.Count == 0)
                {
                    Console.WriteLine("Vous n'avez pas de commandes livrées à évaluer.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Commandes livrées :");
                Console.WriteLine("ID\tDate livraison\t\tCuisinier");
                Console.WriteLine("-------------------------------------------------------------------------");

                foreach (DataRow row in commandes.Rows)
                {
                    string idCommande = row["id_commande"].ToString();
                    string dateLivraison = row["date_livraison"] != DBNull.Value ? Convert.ToDateTime(row["date_livraison"]).ToString("dd/MM/yyyy HH:mm") : "N/A";
                    string nomCuisinier1 = row["nom_cuisinier"] != DBNull.Value ? row["nom_cuisinier"].ToString() : "N/A";
                    string prenomCuisinier1 = row["prenom_cuisinier"] != DBNull.Value ? row["prenom_cuisinier"].ToString() : "N/A";
                    string cuisinier = $"{prenomCuisinier1} {nomCuisinier1}";

                    Console.WriteLine($"{idCommande}\t{dateLivraison}\t{cuisinier}");
                }

                Console.Write("\nEntrez l'ID de la commande pour laquelle vous souhaitez évaluer le cuisinier : ");
                string idCommandeEval = Console.ReadLine();

                // Vérifier si la commande existe et appartient au client
                string requeteVerifCommande = @"
                    SELECT c.id_commande, cu.id_cuisinier, cu.nom_cuisinier, cu.prenom_cuisinier
                    FROM COMMANDE c
                    JOIN LIGNE_COMMANDE lc ON c.id_commande = lc.id_commande
                    JOIN CUISINIER cu ON lc.id_cuisinier = cu.id_cuisinier
                    WHERE c.id_commande = @IdCommande AND c.id_client = @IdClient AND c.statut_commande = 'Livrée'";

                Dictionary<string, object> parametresVerifCommande = new Dictionary<string, object>
                {
                    { "@IdCommande", idCommandeEval },
                    { "@IdClient", SessionUtilisateur.IdUtilisateur }
                };

                DataTable commandeDetails = GestionBDD.ExecuterRequeteTable(requeteVerifCommande, parametresVerifCommande);

                if (commandeDetails == null || commandeDetails.Rows.Count == 0)
                {
                    GestionBDD.AfficherMessageErreur("Commande introuvable ou non éligible pour évaluation.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                DataRow commande = commandeDetails.Rows[0];
                string idCuisinier = commande["id_cuisinier"].ToString();
                string nomCuisinier = commande["nom_cuisinier"] != DBNull.Value ? commande["nom_cuisinier"].ToString() : "N/A";
                string prenomCuisinier = commande["prenom_cuisinier"] != DBNull.Value ? commande["prenom_cuisinier"].ToString() : "N/A";

                // Vérifier si le client a déjà évalué ce cuisinier pour cette commande
                string requeteVerifEvaluation = @"
                    SELECT e.id_evaluation
                    FROM EVALUATION e
                    JOIN effectue ef ON e.id_evaluation = ef.id_evaluation
                    JOIN reçoit r ON e.id_evaluation = r.id_evaluation
                    WHERE ef.id_client = @IdClient AND r.id_cuisinier = @IdCuisinier AND e.id_commande = @IdCommande";

                Dictionary<string, object> parametresVerifEvaluation = new Dictionary<string, object>
                {
                    { "@IdClient", SessionUtilisateur.IdUtilisateur },
                    { "@IdCuisinier", idCuisinier },
                    { "@IdCommande", idCommandeEval }
                };

                object idEvaluationExistante = GestionBDD.ExecuterScalaire(requeteVerifEvaluation, parametresVerifEvaluation);

                if (idEvaluationExistante != null && idEvaluationExistante != DBNull.Value)
                {
                    Console.WriteLine($"Vous avez déjà évalué {prenomCuisinier} {nomCuisinier} pour cette commande.");
                    Console.Write("Voulez-vous modifier votre évaluation ? (O/N) : ");
                    if (Console.ReadLine().ToUpper() == "O")
                    {
                        // Modifier l'évaluation existante
                        ModifierEvaluation(idEvaluationExistante.ToString(), idCuisinier);
                    }
                    return;
                }

                Console.WriteLine($"\nÉvaluation de {prenomCuisinier} {nomCuisinier} pour la commande #{idCommandeEval}");

                Console.Write("Note (1-10) : ");
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

                // Générer un nouvel ID d'évaluation
                int idEvaluation = GestionBDD.GenererNouvelID("EVALUATION", "id_evaluation");

                // Ajouter l'évaluation
                Dictionary<string, object> parametresEvaluation = new Dictionary<string, object>
                {
                    { "@IdEvaluation", idEvaluation },
                    { "@IdCuisinier", idCuisinier },
                    { "@IdCommande", idCommandeEval },
                    { "@Note", note },
                    { "@Commentaire", commentaire }
                };

                string requeteEvaluation = @"
                    INSERT INTO EVALUATION (id_evaluation, id_cuisinier, id_commande, note_evaluation, commentaire_evaluation, date_evaluation)
                    VALUES (@IdEvaluation, @IdCuisinier, @IdCommande, @Note, @Commentaire, NOW())";

                int resultatEvaluation = GestionBDD.ExecuterRequete(requeteEvaluation, parametresEvaluation);

                if (resultatEvaluation > 0)
                {
                    // Lier l'évaluation au client
                    Dictionary<string, object> parametresEffectue = new Dictionary<string, object>
                    {
                        { "@IdEvaluation", idEvaluation },
                        { "@IdClient", SessionUtilisateur.IdUtilisateur }
                    };

                    string requeteEffectue = "INSERT INTO effectue (id_evaluation, id_client) VALUES (@IdEvaluation, @IdClient)";
                    GestionBDD.ExecuterRequete(requeteEffectue, parametresEffectue);

                    // Mettre à jour la note moyenne du cuisinier
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

                    GestionBDD.AfficherMessageSucces("Évaluation ajoutée avec succès !");
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Erreur lors de l'ajout de l'évaluation.");
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
        //private static void EvaluerCuisinier()
        //{
        //    Console.Clear();
        //    GestionBDD.AfficherTitre("ÉVALUER UN CUISINIER");

        //    // Vérifier que l'utilisateur est connecté en tant que client
        //    if (!SessionUtilisateur.EstConnecte() || SessionUtilisateur.TypeUtilisateur != "client")
        //    {
        //        GestionBDD.AfficherMessageErreur("Vous devez être connecté en tant que client pour évaluer un cuisinier.");
        //        return;
        //    }

        //    string idClient = SessionUtilisateur.IdUtilisateur;

        //    try
        //    {
        //        using (MySqlConnection connection = new MySqlConnection(connectionString))
        //        {
        //            connection.Open();

        //            // Récupérer les cuisiniers qui ont préparé des plats pour ce client
        //            string requete = "SELECT DISTINCT cu.id_cuisinier, cu.nom_cuisinier, cu.prenom_cuisinier " +
        //                            "FROM CUISINIER cu " +
        //                            "JOIN LIGNE_COMMANDE lc ON cu.id_cuisinier = lc.id_cuisinier " +
        //                            "JOIN COMMANDE c ON lc.id_commande = c.id_commande " +
        //                            "WHERE c.id_client = @IdClient " +
        //                            "AND c.statut_commande = 'Livrée' " +
        //                            "ORDER BY cu.nom_cuisinier, cu.prenom_cuisinier";

        //            MySqlCommand cmd = new MySqlCommand(requete, connection);
        //            cmd.Parameters.AddWithValue("@IdClient", idClient);

        //            using (MySqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                if (reader.HasRows)
        //                {
        //                    Console.WriteLine("\nCuisiniers disponibles pour évaluation :");
        //                    Console.WriteLine("ID\tNom\t\tPrénom");
        //                    Console.WriteLine("------------------------------");

        //                    while (reader.Read())
        //                    {
        //                        string id = reader["id_cuisinier"].ToString();
        //                        string nom = reader["nom_cuisinier"] == DBNull.Value ? "N/A" : reader["nom_cuisinier"].ToString();
        //                        string prenom = reader["prenom_cuisinier"] == DBNull.Value ? "N/A" : reader["prenom_cuisinier"].ToString();

        //                        Console.WriteLine($"{id}\t{nom,-15}{prenom}");
        //                    }
        //                }
        //                else
        //                {
        //                    Console.WriteLine("Aucun cuisinier disponible pour évaluation.");
        //                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        //                    Console.ReadKey();
        //                    return;
        //                }
        //            }

        //            Console.Write("\nID du cuisinier à évaluer : ");
        //            string idCuisinier = Console.ReadLine();

        //            // Vérifier si le client a déjà évalué ce cuisinier
        //            string requeteVerif = "SELECT COUNT(*) FROM EVALUATION WHERE id_client = @IdClient AND id_cuisinier = @IdCuisinier";
        //            MySqlCommand cmdVerif = new MySqlCommand(requeteVerif, connection);
        //            cmdVerif.Parameters.AddWithValue("@IdClient", idClient);
        //            cmdVerif.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

        //            int count = Convert.ToInt32(cmdVerif.ExecuteScalar());
        //            if (count > 0)
        //            {
        //                Console.WriteLine("Vous avez déjà évalué ce cuisinier. Voulez-vous modifier votre évaluation ? (O/N) : ");
        //                if (Console.ReadLine().ToUpper() != "O")
        //                {
        //                    return;
        //                }
        //            }

        //            // Saisir l'évaluation
        //            Console.Write("Note (1-10) : ");
        //            string note = Console.ReadLine();

        //            Console.Write("Commentaire : ");
        //            string commentaire = Console.ReadLine();

        //            // Générer un nouvel ID d'évaluation
        //            int idEvaluation = GestionBDD.GenererNouvelID("EVALUATION", "id_evaluation");

        //            // Ajouter ou mettre à jour l'évaluation
        //            string requeteEvaluation;
        //            MySqlCommand cmdEvaluation;

        //            if (count > 0)
        //            {
        //                // Mettre à jour l'évaluation existante
        //                requeteEvaluation = "UPDATE EVALUATION SET note = @Note, commentaire = @Commentaire, date_evaluation = NOW() " +
        //                                   "WHERE id_client = @IdClient AND id_cuisinier = @IdCuisinier";
        //                cmdEvaluation = new MySqlCommand(requeteEvaluation, connection);
        //                cmdEvaluation.Parameters.AddWithValue("@Note", note);
        //                cmdEvaluation.Parameters.AddWithValue("@Commentaire", commentaire);
        //                cmdEvaluation.Parameters.AddWithValue("@IdClient", idClient);
        //                cmdEvaluation.Parameters.AddWithValue("@IdCuisinier", idCuisinier);
        //            }
        //            else
        //            {
        //                // Ajouter une nouvelle évaluation
        //                requeteEvaluation = "INSERT INTO EVALUATION (id_evaluation, id_client, id_cuisinier, note, commentaire, date_evaluation) " +
        //                                   "VALUES (@IdEvaluation, @IdClient, @IdCuisinier, @Note, @Commentaire, NOW())";
        //                cmdEvaluation = new MySqlCommand(requeteEvaluation, connection);
        //                cmdEvaluation.Parameters.AddWithValue("@IdEvaluation", idEvaluation);
        //                cmdEvaluation.Parameters.AddWithValue("@IdClient", idClient);
        //                cmdEvaluation.Parameters.AddWithValue("@IdCuisinier", idCuisinier);
        //                cmdEvaluation.Parameters.AddWithValue("@Note", note);
        //                cmdEvaluation.Parameters.AddWithValue("@Commentaire", commentaire);
        //            }

        //            int resultat = cmdEvaluation.ExecuteNonQuery();
        //            if (resultat > 0)
        //            {
        //                GestionBDD.AfficherMessageSucces("Évaluation enregistrée avec succès !");
        //            }
        //            else
        //            {
        //                GestionBDD.AfficherMessageErreur("Erreur lors de l'enregistrement de l'évaluation.");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
        //    }

        //    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
        //    Console.ReadKey();
        //} //Ancienne version : celle du moduleClient

        /// <summary>
        /// Modifie une évaluation existante
        /// </summary>
        private static void ModifierEvaluation(string idEvaluation, string idCuisinier)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MODIFIER UNE ÉVALUATION");

            try
            {
                // Récupérer les détails de l'évaluation
                string requeteEvaluation = @"
                    SELECT e.note_evaluation, e.commentaire_evaluation
                    FROM EVALUATION e
                    WHERE e.id_evaluation = @IdEvaluation";

                Dictionary<string, object> parametresEvaluation = new Dictionary<string, object>
                {
                    { "@IdEvaluation", idEvaluation }
                };

                DataTable evaluationDetails = GestionBDD.ExecuterRequeteTable(requeteEvaluation, parametresEvaluation);

                if (evaluationDetails == null || evaluationDetails.Rows.Count == 0)
                {
                    GestionBDD.AfficherMessageErreur("Évaluation introuvable.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                DataRow evaluation = evaluationDetails.Rows[0];
                int noteActuelle = evaluation["note_evaluation"] != DBNull.Value ? Convert.ToInt32(evaluation["note_evaluation"]) : 0;
                string commentaireActuel = evaluation["commentaire_evaluation"] != DBNull.Value ? evaluation["commentaire_evaluation"].ToString() : "";

                Console.WriteLine($"Note actuelle : {noteActuelle}");
                Console.WriteLine($"Commentaire actuel : {commentaireActuel}");

                Console.Write("\nNouvelle note (1-10) : ");
                string noteStr = Console.ReadLine();
                if (!int.TryParse(noteStr, out int nouvelleNote) || nouvelleNote < 1 || nouvelleNote > 10)
                {
                    GestionBDD.AfficherMessageErreur("La note doit être un nombre entre 1 et 10.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Nouveau commentaire : ");
                string nouveauCommentaire = Console.ReadLine();

                // Mettre à jour l'évaluation
                Dictionary<string, object> parametresUpdate = new Dictionary<string, object>
                {
                    { "@IdEvaluation", idEvaluation },
                    { "@Note", nouvelleNote },
                    { "@Commentaire", nouveauCommentaire }
                };

                string requeteUpdate = @"
                    UPDATE EVALUATION 
                    SET note_evaluation = @Note, commentaire_evaluation = @Commentaire, date_evaluation = NOW()
                    WHERE id_evaluation = @IdEvaluation";

                int resultatUpdate = GestionBDD.ExecuterRequete(requeteUpdate, parametresUpdate);

                if (resultatUpdate > 0)
                {
                    // Mettre à jour la note moyenne du cuisinier
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

                    GestionBDD.AfficherMessageSucces("Évaluation modifiée avec succès !");
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Erreur lors de la modification de l'évaluation.");
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

        #region Fonctionnalités pour les cuisiniers

        /// <summary>
        /// Affiche les commandes à préparer pour un cuisinier
        /// </summary>
        private static void VoirCommandesAPreparer()
        {
            // Vérifier que l'utilisateur est un cuisinier
            if (!SessionUtilisateur.EstCuisinier())
            {
                SessionUtilisateur.AfficherMessageDroitsInsuffisants();
                return;
            }

            Console.Clear();
            GestionBDD.AfficherTitre("COMMANDES À PRÉPARER");

            try
            {
                string requete = @"
            SELECT DISTINCT c.id_commande, c.date_commande, c.statut_commande, 
                   cl.nom_client, cl.prenom_client
            FROM COMMANDE c
            JOIN LIGNE_COMMANDE lc ON c.id_commande = lc.id_commande
            JOIN PLAT p ON lc.id_plat = p.id_plat
            JOIN CLIENT cl ON c.id_client = cl.id_client
            WHERE p.id_cuisinier = @IdCuisinier
            AND c.statut_commande IN ('En attente', 'En préparation', 'Confirmée')
            ORDER BY c.date_commande ASC";

                Dictionary<string, object> parametres = new Dictionary<string, object>
            {
                { "@IdCuisinier", SessionUtilisateur.IdUtilisateur }
            };

                DataTable commandes = GestionBDD.ExecuterRequeteTable(requete, parametres);

                if (commandes != null && commandes.Rows.Count > 0)
                {
                    Console.WriteLine("ID\tDate commande\t\tStatut\t\tClient");
                    Console.WriteLine("-------------------------------------------------------------------------");

                    foreach (DataRow row in commandes.Rows)
                    {
                        string idCommande = row["id_commande"].ToString();
                        string dateCommande = row["date_commande"] != DBNull.Value ? Convert.ToDateTime(row["date_commande"]).ToString("dd/MM/yyyy HH:mm") : "N/A";
                        string statut = row["statut_commande"] != DBNull.Value ? row["statut_commande"].ToString() : "N/A";
                        string nomClient = row["nom_client"] != DBNull.Value ? row["nom_client"].ToString() : "N/A";
                        string prenomClient = row["prenom_client"] != DBNull.Value ? row["prenom_client"].ToString() : "N/A";
                        string client = $"{prenomClient} {nomClient}";

                        Console.WriteLine($"{idCommande}\t{dateCommande}\t{statut,-15}{client}");
                    }

                    Console.Write("\nVoulez-vous voir les détails d'une commande ? (O/N) : ");
                    if (Console.ReadLine().ToUpper() == "O")
                    {
                        AfficherDetailsCommandeCuisinier();
                    }
                }
                else
                {
                    Console.WriteLine("Aucune commande à préparer.");
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
        /// Affiche les détails d'une commande spécifiquement pour un cuisinier
        /// </summary>
        private static void AfficherDetailsCommandeCuisinier()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("DÉTAILS DE LA COMMANDE");

            Console.Write("Entrez l'ID de la commande : ");
            string idCommande = Console.ReadLine();

            try
            {
                // Vérifier si la commande existe et contient des plats du cuisinier
                string requeteVerif = @"
            SELECT COUNT(*) 
            FROM COMMANDE c
            JOIN LIGNE_COMMANDE lc ON c.id_commande = lc.id_commande
            JOIN PLAT p ON lc.id_plat = p.id_plat
            WHERE c.id_commande = @IdCommande 
            AND p.id_cuisinier = @IdCuisinier";

                Dictionary<string, object> parametresVerif = new Dictionary<string, object>
            {
                { "@IdCommande", idCommande },
                { "@IdCuisinier", SessionUtilisateur.IdUtilisateur }
            };

                int count = Convert.ToInt32(GestionBDD.ExecuterScalaire(requeteVerif, parametresVerif));

                if (count == 0)
                {
                    GestionBDD.AfficherMessageErreur("Commande introuvable ou vous n'avez pas les droits pour la consulter.");
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return;
                }

                // Récupérer les informations de la commande
                string requeteCommande = @"
            SELECT c.id_commande, c.date_commande, c.date_livraison, c.prix_total_commande, c.statut_commande, 
                   c.commentaire_commande, cl.nom_client, cl.prenom_client
            FROM COMMANDE c
            JOIN CLIENT cl ON c.id_client = cl.id_client
            WHERE c.id_commande = @IdCommande";

                Dictionary<string, object> parametresCommande = new Dictionary<string, object>
            {
                { "@IdCommande", idCommande }
            };

                DataTable commandeDetails = GestionBDD.ExecuterRequeteTable(requeteCommande, parametresCommande);

                if (commandeDetails != null && commandeDetails.Rows.Count > 0)
                {
                    DataRow commande = commandeDetails.Rows[0];
                    string dateCommande = commande["date_commande"] != DBNull.Value ? Convert.ToDateTime(commande["date_commande"]).ToString("dd/MM/yyyy HH:mm") : "N/A";
                    string dateLivraison = commande["date_livraison"] != DBNull.Value ? Convert.ToDateTime(commande["date_livraison"]).ToString("dd/MM/yyyy HH:mm") : "N/A";
                    string prix = commande["prix_total_commande"] != DBNull.Value ? commande["prix_total_commande"].ToString() : "N/A";
                    string statut = commande["statut_commande"] != DBNull.Value ? commande["statut_commande"].ToString() : "N/A";
                    string commentaire = commande["commentaire_commande"] != DBNull.Value ? commande["commentaire_commande"].ToString() : "N/A";
                    string nomClient = commande["nom_client"] != DBNull.Value ? commande["nom_client"].ToString() : "N/A";
                    string prenomClient = commande["prenom_client"] != DBNull.Value ? commande["prenom_client"].ToString() : "N/A";

                    Console.WriteLine($"Commande #{idCommande}");
                    Console.WriteLine($"Client : {prenomClient} {nomClient}");
                    Console.WriteLine($"Date de commande : {dateCommande}");
                    Console.WriteLine($"Date de livraison : {dateLivraison}");
                    Console.WriteLine($"Statut : {statut}");
                    Console.WriteLine($"Prix total : {prix}");
                    Console.WriteLine($"Commentaire : {commentaire}");

                    // Récupérer uniquement les lignes de commande qui concernent ce cuisinier
                    string requeteLignes = @"
                SELECT lc.id_ligne, p.nom_plat, lc.prix_unitaire, lc.quantite, lc.statut_ligne,
                       lc.date_livraison_voulue, lc.metro_le_plus_proche_livraison
                FROM LIGNE_COMMANDE lc
                JOIN PLAT p ON lc.id_plat = p.id_plat
                WHERE lc.id_commande = @IdCommande AND p.id_cuisinier = @IdCuisinier";

                    Dictionary<string, object> parametresLignes = new Dictionary<string, object>
                {
                    { "@IdCommande", idCommande },
                    { "@IdCuisinier", SessionUtilisateur.IdUtilisateur }
                };

                    DataTable lignesCommande = GestionBDD.ExecuterRequeteTable(requeteLignes, parametresLignes);

                    if (lignesCommande != null && lignesCommande.Rows.Count > 0)
                    {
                        Console.WriteLine("\nDétail des plats que vous devez préparer :");
                        Console.WriteLine("ID\tPlat\t\tPrix unitaire\tQuantité\tStatut\t\tDate livraison\t\tMétro");
                        Console.WriteLine("-------------------------------------------------------------------------");

                        foreach (DataRow ligne in lignesCommande.Rows)
                        {
                            string idLigne = ligne["id_ligne"].ToString();
                            string nomPlat = ligne["nom_plat"] != DBNull.Value ? ligne["nom_plat"].ToString() : "N/A";
                            string prixUnitaire = ligne["prix_unitaire"] != DBNull.Value ? ligne["prix_unitaire"].ToString() : "N/A";
                            string quantite = ligne["quantite"] != DBNull.Value ? ligne["quantite"].ToString() : "N/A";
                            string statutLigne = ligne["statut_ligne"] != DBNull.Value ? ligne["statut_ligne"].ToString() : "N/A";
                            string dateLivraisonVoulue = ligne["date_livraison_voulue"] != DBNull.Value ? ligne["date_livraison_voulue"].ToString() : "N/A";
                            string metro = ligne["metro_le_plus_proche_livraison"] != DBNull.Value ? ligne["metro_le_plus_proche_livraison"].ToString() : "N/A";

                            Console.WriteLine($"{idLigne}\t{nomPlat,-15}{prixUnitaire,-15}{quantite,-15}{statutLigne,-15}{dateLivraisonVoulue,-20}{metro}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nAucun plat trouvé pour cette commande.");
                    }
                }
                else
                {
                    Console.WriteLine("Commande introuvable.");
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
        /// Permet à un cuisinier de mettre à jour le statut d'une commande
        /// </summary>
        private static void MettreAJourStatutLigneCommande()
        {
            // Vérifier que l'utilisateur est un cuisinier
            if (!SessionUtilisateur.EstCuisinier())
            {
                SessionUtilisateur.AfficherMessageDroitsInsuffisants();
                return;
            }

            Console.Clear();
            GestionBDD.AfficherTitre("METTRE À JOUR LE STATUT D'UNE COMMANDE");

            Console.Write("Entrez l'ID de la commande : ");
            string idCommande = Console.ReadLine();

            try
            {
                // Vérifier si la commande existe et contient des plats créés par le cuisinier
                string requeteVerif = @"
            SELECT DISTINCT c.id_commande, c.statut_commande, cl.nom_client, cl.prenom_client
            FROM COMMANDE c
            JOIN LIGNE_COMMANDE lc ON c.id_commande = lc.id_commande
            JOIN PLAT p ON lc.id_plat = p.id_plat
            JOIN CLIENT cl ON c.id_client = cl.id_client
            WHERE c.id_commande = @IdCommande 
            AND p.id_cuisinier = @IdCuisinier";

                Dictionary<string, object> parametresVerif = new Dictionary<string, object>
        {
            { "@IdCommande", idCommande },
            { "@IdCuisinier", SessionUtilisateur.IdUtilisateur }
        };

                DataTable infoCommande = GestionBDD.ExecuterRequeteTable(requeteVerif, parametresVerif);

                if (infoCommande == null || infoCommande.Rows.Count == 0)
                {
                    GestionBDD.AfficherMessageErreur("Commande introuvable ou ne contenant pas vos plats.");
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
                Console.WriteLine("1. En préparation");
                Console.WriteLine("2. Prêt pour livraison");
                Console.Write("Votre choix : ");
                string choixStatut = Console.ReadLine();

                string nouveauStatut = "";
                switch (choixStatut)
                {
                    case "1": nouveauStatut = "En préparation"; break;
                    case "2": nouveauStatut = "Prêt pour livraison"; break;
                    default:
                        GestionBDD.AfficherMessageErreur("Statut invalide.");
                        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        return;
                }

                // Mettre à jour le statut de la commande
                Dictionary<string, object> parametresUpdate = new Dictionary<string, object>
        {
            { "@IdCommande", idCommande },
            { "@NouveauStatut", nouveauStatut }
        };

                string requeteUpdate = "UPDATE COMMANDE SET statut_commande = @NouveauStatut WHERE id_commande = @IdCommande";
                int resultat = GestionBDD.ExecuterRequete(requeteUpdate, parametresUpdate);

                if (resultat > 0)
                {
                    // Mettre à jour également le statut des lignes de commande pour les plats de ce cuisinier
                    Dictionary<string, object> parametresUpdateLignes = new Dictionary<string, object>
            {
                { "@IdCommande", idCommande },
                { "@NouveauStatut", nouveauStatut }
            };

                    string requeteUpdateLignes = @"
                UPDATE LIGNE_COMMANDE lc
                JOIN PLAT p ON lc.id_plat = p.id_plat
                SET lc.statut_ligne = @NouveauStatut
                WHERE lc.id_commande = @IdCommande 
                AND p.id_cuisinier = @IdCuisinier";

                    parametresUpdateLignes.Add("@IdCuisinier", SessionUtilisateur.IdUtilisateur);
                    GestionBDD.ExecuterRequete(requeteUpdateLignes, parametresUpdateLignes);

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


        /// <summary>
        /// Affiche les évaluations reçues par le cuisinier connecté
        /// </summary>
        private static void VoirEvaluationsCuisinier()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MES ÉVALUATIONS");

            // Vérifier que l'utilisateur est connecté en tant que cuisinier
            if (!SessionUtilisateur.EstConnecte() || SessionUtilisateur.TypeUtilisateur != "cuisinier")
            {
                GestionBDD.AfficherMessageErreur("Vous devez être connecté en tant que cuisinier pour voir vos évaluations.");
                return;
            }

            string idCuisinier = SessionUtilisateur.IdUtilisateur;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete =
                         "SELECT e.id_evaluation, e.note_evaluation, e.commentaire_evaluation, e.date_evaluation, " +
                         "c.id_client, c.nom_client, c.prenom_client " +
                         "FROM EVALUATION e " +
                         "JOIN effectue ef ON e.id_evaluation = ef.id_evaluation " +
                         "JOIN CLIENT c ON ef.id_client = c.id_client " +
                         "JOIN reçoit r ON e.id_evaluation = r.id_evaluation " +
                         "WHERE r.id_cuisinier = @IdCuisinier " +
                         "ORDER BY e.date_evaluation DESC";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("\nID\tNote\tDate\t\tClient\t\tCommentaire");
                            Console.WriteLine("-------------------------------------------------------------------------");

                            while (reader.Read())
                            {
                                string id = reader["id_evaluation"].ToString();
                                string note = reader["note"] == DBNull.Value ? "N/A" : reader["note"].ToString();
                                string dateEvaluation = reader["date_evaluation"] == DBNull.Value ? "N/A" : Convert.ToDateTime(reader["date_evaluation"]).ToString("dd/MM/yyyy");
                                string nomClient = reader["nom_client"] == DBNull.Value ? "N/A" : reader["nom_client"].ToString();
                                string prenomClient = reader["prenom_client"] == DBNull.Value ? "N/A" : reader["prenom_client"].ToString();
                                string client = $"{nomClient} {prenomClient}";
                                string commentaire = reader["commentaire"] == DBNull.Value ? "N/A" : reader["commentaire"].ToString();

                                Console.WriteLine($"{id}\t{note}\t{dateEvaluation}\t{client,-15}{commentaire}");
                            }

                            // Calculer la note moyenne
                            reader.Close();
                            string requeteMoyenne = "SELECT AVG(note) FROM EVALUATION WHERE id_cuisinier = @IdCuisinier";
                            MySqlCommand cmdMoyenne = new MySqlCommand(requeteMoyenne, connection);
                            cmdMoyenne.Parameters.AddWithValue("@IdCuisinier", idCuisinier);
                            object resultatMoyenne = cmdMoyenne.ExecuteScalar();

                            if (resultatMoyenne != null && resultatMoyenne != DBNull.Value)
                            {
                                double moyenne = Convert.ToDouble(resultatMoyenne);
                                Console.WriteLine($"\nNote moyenne : {moyenne:F1}/10");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Vous n'avez pas encore reçu d'évaluations.");
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
        /// Affiche les statistiques des plats préparés par le cuisinier
        /// </summary>
        private static void VoirStatistiquesPlats()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("STATISTIQUES DE MES PLATS");

            // Vérifier que l'utilisateur est connecté en tant que cuisinier
            if (!SessionUtilisateur.EstConnecte() || SessionUtilisateur.TypeUtilisateur != "cuisinier")
            {
                GestionBDD.AfficherMessageErreur("Vous devez être connecté en tant que cuisinier pour voir vos statistiques.");
                return;
            }

            string idCuisinier = SessionUtilisateur.IdUtilisateur;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Statistiques des plats les plus préparés
                    Console.WriteLine("\nPlats les plus préparés :");
                    string requetePlats = "SELECT p.id_plat, p.nom_plat, COUNT(lc.id_ligne) AS nombre_preparations " +
                                         "FROM PLAT p " +
                                         "JOIN LIGNE_COMMANDE lc ON p.id_plat = lc.id_plat " +
                                         "WHERE lc.id_cuisinier = @IdCuisinier " +
                                         "GROUP BY p.id_plat, p.nom_plat " +
                                         "ORDER BY nombre_preparations DESC " +
                                         "LIMIT 10";

                    MySqlCommand cmdPlats = new MySqlCommand(requetePlats, connection);
                    cmdPlats.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                    using (MySqlDataReader reader = cmdPlats.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("ID\tPlat\t\tNombre de préparations");
                            Console.WriteLine("-------------------------------------");

                            while (reader.Read())
                            {
                                string idPlat = reader["id_plat"].ToString();
                                string nomPlat = reader["nom_plat"] == DBNull.Value ? "N/A" : reader["nom_plat"].ToString();
                                string nombrePreparations = reader["nombre_preparations"].ToString();

                                Console.WriteLine($"{idPlat}\t{nomPlat,-15}{nombrePreparations}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Vous n'avez pas encore préparé de plats.");
                        }
                    }

                    // Statistiques des clients servis
                    Console.WriteLine("\nClients les plus servis :");
                    string requeteClients = "SELECT c.id_client, c.nom_client, c.prenom_client, COUNT(DISTINCT lc.id_commande) AS nombre_commandes " +
                                           "FROM CLIENT c " +
                                           "JOIN COMMANDE co ON c.id_client = co.id_client " +
                                           "JOIN LIGNE_COMMANDE lc ON co.id_commande = lc.id_commande " +
                                           "WHERE lc.id_cuisinier = @IdCuisinier " +
                                           "GROUP BY c.id_client, c.nom_client, c.prenom_client " +
                                           "ORDER BY nombre_commandes DESC " +
                                           "LIMIT 10";

                    MySqlCommand cmdClients = new MySqlCommand(requeteClients, connection);
                    cmdClients.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                    using (MySqlDataReader reader = cmdClients.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("ID\tClient\t\tNombre de commandes");
                            Console.WriteLine("-------------------------------------");

                            while (reader.Read())
                            {
                                string idClient = reader["id_client"].ToString();
                                string nomClient = reader["nom_client"] == DBNull.Value ? "N/A" : reader["nom_client"].ToString();
                                string prenomClient = reader["prenom_client"] == DBNull.Value ? "N/A" : reader["prenom_client"].ToString();
                                string client = $"{nomClient} {prenomClient}";
                                string nombreCommandes = reader["nombre_commandes"].ToString();

                                Console.WriteLine($"{idClient}\t{client,-15}{nombreCommandes}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Vous n'avez pas encore servi de clients.");
                        }
                    }

                    // Statistiques par période
                    Console.WriteLine("\nStatistiques par période :");
                    Console.WriteLine("1. Dernière semaine");
                    Console.WriteLine("2. Dernier mois");
                    Console.WriteLine("3. Dernière année");
                    Console.Write("Choisissez une période : ");
                    string choixPeriode = Console.ReadLine();

                    string periode = "";
                    switch (choixPeriode)
                    {
                        case "1":
                            periode = "WEEK";
                            break;
                        case "2":
                            periode = "MONTH";
                            break;
                        case "3":
                            periode = "YEAR";
                            break;
                        default:
                            periode = "MONTH"; // Par défaut
                            break;
                    }

                    string requetePeriode = $"SELECT COUNT(lc.id_ligne) AS nombre_plats, " +
                                           $"COUNT(DISTINCT lc.id_commande) AS nombre_commandes, " +
                                           $"COUNT(DISTINCT co.id_client) AS nombre_clients " +
                                           $"FROM LIGNE_COMMANDE lc " +
                                           $"JOIN COMMANDE co ON lc.id_commande = co.id_commande " +
                                           $"WHERE lc.id_cuisinier = @IdCuisinier " +
                                           $"AND lc.date_livraison_voulue >= DATE_SUB(NOW(), INTERVAL 1 {periode})";

                    MySqlCommand cmdPeriode = new MySqlCommand(requetePeriode, connection);
                    cmdPeriode.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                    using (MySqlDataReader reader = cmdPeriode.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nombrePlats = reader["nombre_plats"] == DBNull.Value ? "0" : reader["nombre_plats"].ToString();
                            string nombreCommandes = reader["nombre_commandes"] == DBNull.Value ? "0" : reader["nombre_commandes"].ToString();
                            string nombreClients = reader["nombre_clients"] == DBNull.Value ? "0" : reader["nombre_clients"].ToString();

                            Console.WriteLine($"\nStatistiques pour la période sélectionnée :");
                            Console.WriteLine($"Nombre de plats préparés : {nombrePlats}");
                            Console.WriteLine($"Nombre de commandes traitées : {nombreCommandes}");
                            Console.WriteLine($"Nombre de clients servis : {nombreClients}");
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

        #region Fonctionnalités communes
        /// <summary>
        /// Affiche les détails d'une commande
        /// </summary>
        public static void AfficherDetailsCommande()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("DÉTAILS D'UNE COMMANDE");

            Console.Write("ID de la commande : ");
            string idCommande = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Vérifier si la commande existe et si l'utilisateur a le droit de la voir
                    string requeteVerif = "";
                    MySqlCommand cmdVerif = new MySqlCommand();
                    cmdVerif.Connection = connection;

                    if (SessionUtilisateur.TypeUtilisateur == "client")
                    {
                        // Un client ne peut voir que ses propres commandes
                        requeteVerif = "SELECT COUNT(*) FROM COMMANDE WHERE id_commande = @IdCommande AND id_client = @IdClient";
                        cmdVerif.CommandText = requeteVerif;
                        cmdVerif.Parameters.AddWithValue("@IdCommande", idCommande);
                        cmdVerif.Parameters.AddWithValue("@IdClient", SessionUtilisateur.IdUtilisateur);
                    }
                    else if (SessionUtilisateur.TypeUtilisateur == "cuisinier")
                    {
                        // Un cuisinier peut voir les commandes qui lui sont assignées
                        requeteVerif = "SELECT COUNT(*) FROM LIGNE_COMMANDE WHERE id_commande = @IdCommande AND id_cuisinier = @IdCuisinier";
                        cmdVerif.CommandText = requeteVerif;
                        cmdVerif.Parameters.AddWithValue("@IdCommande", idCommande);
                        cmdVerif.Parameters.AddWithValue("@IdCuisinier", SessionUtilisateur.IdUtilisateur);
                    }
                    else if (SessionUtilisateur.TypeUtilisateur == "admin")
                    {
                        // Un admin peut voir toutes les commandes
                        requeteVerif = "SELECT COUNT(*) FROM COMMANDE WHERE id_commande = @IdCommande";
                        cmdVerif.CommandText = requeteVerif;
                        cmdVerif.Parameters.AddWithValue("@IdCommande", idCommande);
                    }
                    else
                    {
                        GestionBDD.AfficherMessageErreur("Vous n'avez pas les droits pour voir cette commande.");
                        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        return;
                    }

                    int count = Convert.ToInt32(cmdVerif.ExecuteScalar());
                    if (count == 0)
                    {
                        GestionBDD.AfficherMessageErreur("Cette commande n'existe pas ou vous n'avez pas les droits pour la voir.");
                        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        return;
                    }

                    // Afficher les informations de la commande
                    string requeteCommande = "SELECT c.id_commande, c.date_commande, c.date_livraison, c.statut_commande, " +
                                            "c.prix_total_commande, c.commentaire_commande, " +
                                            "cl.id_client, cl.nom_client, cl.prenom_client, cl.email_client, cl.tel_client " +
                                            "FROM COMMANDE c " +
                                            "JOIN CLIENT cl ON c.id_client = cl.id_client " +
                                            "WHERE c.id_commande = @IdCommande";

                    MySqlCommand cmdCommande = new MySqlCommand(requeteCommande, connection);
                    cmdCommande.Parameters.AddWithValue("@IdCommande", idCommande);

                    using (MySqlDataReader reader = cmdCommande.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string dateCommande = reader["date_commande"] == DBNull.Value ? "N/A" : Convert.ToDateTime(reader["date_commande"]).ToString("dd/MM/yyyy HH:mm");
                            string dateLivraison = reader["date_livraison"] == DBNull.Value ? "N/A" : Convert.ToDateTime(reader["date_livraison"]).ToString("dd/MM/yyyy HH:mm");
                            string statut = reader["statut_commande"] == DBNull.Value ? "N/A" : reader["statut_commande"].ToString();
                            string prixTotal = reader["prix_total_commande"] == DBNull.Value ? "N/A" : reader["prix_total_commande"].ToString();
                            string commentaire = reader["commentaire_commande"] == DBNull.Value ? "N/A" : reader["commentaire_commande"].ToString();
                            string idClient = reader["id_client"].ToString();
                            string nomClient = reader["nom_client"] == DBNull.Value ? "N/A" : reader["nom_client"].ToString();
                            string prenomClient = reader["prenom_client"] == DBNull.Value ? "N/A" : reader["prenom_client"].ToString();
                            string emailClient = reader["email_client"] == DBNull.Value ? "N/A" : reader["email_client"].ToString();
                            string telClient = reader["tel_client"] == DBNull.Value ? "N/A" : reader["tel_client"].ToString();

                            Console.WriteLine($"\nInformations de la commande #{idCommande}");
                            Console.WriteLine($"Date de commande : {dateCommande}");
                            Console.WriteLine($"Date de livraison : {dateLivraison}");
                            Console.WriteLine($"Statut : {statut}");
                            Console.WriteLine($"Prix total : {prixTotal}");
                            Console.WriteLine($"Commentaire : {commentaire}");
                            Console.WriteLine($"\nInformations du client");
                            Console.WriteLine($"ID : {idClient}");
                            Console.WriteLine($"Nom : {nomClient} {prenomClient}");
                            Console.WriteLine($"Email : {emailClient}");
                            Console.WriteLine($"Téléphone : {telClient}");
                        }
                    }

                    // Afficher les lignes de commande
                    string requeteLignes = "SELECT lc.id_ligne, lc.id_plat, p.nom_plat, lc.prix_unitaire, " +
                                          "lc.date_livraison_voulue, lc.statut_ligne, " +
                                          "lc.id_cuisinier, cu.nom_cuisinier, cu.prenom_cuisinier " +
                                          "FROM LIGNE_COMMANDE lc " +
                                          "JOIN PLAT p ON lc.id_plat = p.id_plat " +
                                          "LEFT JOIN CUISINIER cu ON lc.id_cuisinier = cu.id_cuisinier " +
                                          "WHERE lc.id_commande = @IdCommande " +
                                          "ORDER BY lc.id_ligne";

                    MySqlCommand cmdLignes = new MySqlCommand(requeteLignes, connection);
                    cmdLignes.Parameters.AddWithValue("@IdCommande", idCommande);

                    Console.WriteLine("\nLignes de commande :");
                    Console.WriteLine("ID\tPlat\t\tPrix\tDate livraison\tStatut\t\tCuisinier");
                    Console.WriteLine("-------------------------------------------------------------------------");

                    using (MySqlDataReader reader = cmdLignes.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string idLigne = reader["id_ligne"].ToString();
                                string nomPlat = reader["nom_plat"] == DBNull.Value ? "N/A" : reader["nom_plat"].ToString();
                                string prixUnitaire = reader["prix_unitaire"] == DBNull.Value ? "N/A" : reader["prix_unitaire"].ToString();
                                string dateLivraisonVoulue = reader["date_livraison_voulue"] == DBNull.Value ? "N/A" : Convert.ToDateTime(reader["date_livraison_voulue"]).ToString("dd/MM/yyyy");
                                string statutLigne = reader["statut_ligne"] == DBNull.Value ? "N/A" : reader["statut_ligne"].ToString();

                                string cuisinier = "Non assigné";
                                if (reader["id_cuisinier"] != DBNull.Value)
                                {
                                    string nomCuisinier = reader["nom_cuisinier"] == DBNull.Value ? "" : reader["nom_cuisinier"].ToString();
                                    string prenomCuisinier = reader["prenom_cuisinier"] == DBNull.Value ? "" : reader["prenom_cuisinier"].ToString();
                                    cuisinier = $"{nomCuisinier} {prenomCuisinier}";
                                }

                                Console.WriteLine($"{idLigne}\t{nomPlat,-15}{prixUnitaire}\t{dateLivraisonVoulue}\t{statutLigne,-15}{cuisinier}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucune ligne de commande trouvée.");
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

        #region Fonctionnalités pour les administrateurs
        /// <summary>
        /// Permet à un administrateur de créer une nouvelle commande
        /// </summary>
        private static void CreerNouvelleCommande()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("CRÉER UNE NOUVELLE COMMANDE");

            // Vérifier que l'utilisateur est connecté en tant qu'administrateur
            if (!SessionUtilisateur.EstConnecte() || SessionUtilisateur.TypeUtilisateur != "admin")
            {
                GestionBDD.AfficherMessageErreur("Vous devez être connecté en tant qu'administrateur pour créer une commande.");
                return;
            }

            // Sélectionner un client
            Console.WriteLine("Sélectionnez un client :");
            Console.WriteLine("1. Rechercher un client");
            Console.WriteLine("2. Saisir l'ID du client directement");
            Console.Write("Votre choix : ");
            string choixClient = Console.ReadLine();

            string idClient = "";
            if (choixClient == "1")
            {
                // Rechercher un client
                Console.Write("Entrez le nom ou l'email du client : ");
                string recherche = Console.ReadLine();

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        string requete = "SELECT id_client, nom_client, prenom_client, email_client " +
                                        "FROM CLIENT " +
                                        "WHERE nom_client LIKE @Recherche OR email_client LIKE @Recherche " +
                                        "ORDER BY nom_client, prenom_client";

                        MySqlCommand cmd = new MySqlCommand(requete, connection);
                        cmd.Parameters.AddWithValue("@Recherche", $"%{recherche}%");

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                Console.WriteLine("\nRésultats de la recherche :");
                                Console.WriteLine("ID\tNom\t\tPrénom\t\tEmail");
                                Console.WriteLine("-----------------------------------------------------");

                                while (reader.Read())
                                {
                                    string id = reader["id_client"].ToString();
                                    string nom = reader["nom_client"] == DBNull.Value ? "N/A" : reader["nom_client"].ToString();
                                    string prenom = reader["prenom_client"] == DBNull.Value ? "N/A" : reader["prenom_client"].ToString();
                                    string email = reader["email_client"] == DBNull.Value ? "N/A" : reader["email_client"].ToString();

                                    Console.WriteLine($"{id}\t{nom,-15}{prenom,-15}{email}");
                                }

                                Console.Write("\nEntrez l'ID du client choisi : ");
                                idClient = Console.ReadLine();
                            }
                            else
                            {
                                Console.WriteLine("Aucun client trouvé avec ces critères.");
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
                    return;
                }
            }
            else
            {
                // Saisir l'ID directement
                Console.Write("ID du client : ");
                idClient = Console.ReadLine();
            }

            // Vérifier que le client existe
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requeteVerif = "SELECT COUNT(*) FROM CLIENT WHERE id_client = @IdClient";
                    MySqlCommand cmdVerif = new MySqlCommand(requeteVerif, connection);
                    cmdVerif.Parameters.AddWithValue("@IdClient", idClient);

                    int count = Convert.ToInt32(cmdVerif.ExecuteScalar());
                    if (count == 0)
                    {
                        GestionBDD.AfficherMessageErreur("Client introuvable.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
                return;
            }

            // Générer un nouvel ID de commande
            int idCommande = GestionBDD.GenererNouvelID("COMMANDE", "id_commande");

            // Saisir les informations de la commande
            Console.Write("Commentaire (facultatif) : ");
            string commentaire = Console.ReadLine();

            Console.Write("Statut (En attente, Confirmée, En préparation, Prête à livrer, En livraison, Livrée, Annulée) : ");
            string statut = Console.ReadLine();

            // Créer la commande
            Dictionary<string, object> parametresCommande = new Dictionary<string, object>
          {
              { "@IdCommande", idCommande },
              { "@Commentaire", commentaire },
              { "@Statut", statut },
              { "@IdClient", idClient }
          };

            string queryCommande = "INSERT INTO COMMANDE (id_commande, date_commande, commentaire_commande, statut_commande, id_client) " +
                                  "VALUES (@IdCommande, NOW(), @Commentaire, @Statut, @IdClient)";

            int resultatCommande = GestionBDD.ExecuterRequete(queryCommande, parametresCommande);

            if (resultatCommande > 0)
            {
                GestionBDD.AfficherMessageSucces($"Commande #{idCommande} créée avec succès !");

                // Ajouter des lignes de commande
                Console.Write("Voulez-vous ajouter des plats à cette commande maintenant ? (O/N) : ");
                if (Console.ReadLine().ToUpper() == "O")
                {
                    AjouterLignesCommande(idCommande.ToString());
                }
            }
            else
            {
                GestionBDD.AfficherMessageErreur("Erreur lors de la création de la commande.");
            }
        }

        /// <summary>
        /// Permet de rechercher une commande selon différents critères
        /// </summary>
        public static void RechercherCommande()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("RECHERCHER UNE COMMANDE");

            Console.WriteLine("Rechercher par :");
            Console.WriteLine("1. ID de commande");
            Console.WriteLine("2. ID de client");
            Console.WriteLine("3. Nom de client");
            Console.WriteLine("4. Statut");
            Console.WriteLine("5. Date de commande");
            Console.Write("\nVotre choix : ");
            string choixRecherche = Console.ReadLine();

            string requete = "";
            Dictionary<string, object> parametres = new Dictionary<string, object>();

            switch (choixRecherche)
            {
                case "1":
                    Console.Write("ID de commande : ");
                    string idCommande = Console.ReadLine();
                    requete = "SELECT c.id_commande, c.date_commande, c.statut_commande, c.prix_total_commande, " +
                            "cl.id_client, cl.nom_client, cl.prenom_client " +
                            "FROM COMMANDE c " +
                            "JOIN CLIENT cl ON c.id_client = cl.id_client " +
                            "WHERE c.id_commande = @IdCommande";
                    parametres.Add("@IdCommande", idCommande);
                    break;
                case "2":
                    Console.Write("ID de client : ");
                    string idClient = Console.ReadLine();
                    requete = "SELECT c.id_commande, c.date_commande, c.statut_commande, c.prix_total_commande, " +
                            "cl.id_client, cl.nom_client, cl.prenom_client " +
                            "FROM COMMANDE c " +
                            "JOIN CLIENT cl ON c.id_client = cl.id_client " +
                            "WHERE c.id_client = @IdClient " +
                            "ORDER BY c.date_commande DESC";
                    parametres.Add("@IdClient", idClient);
                    break;
                case "3":
                    Console.Write("Nom de client : ");
                    string nomClient = Console.ReadLine();
                    requete = "SELECT c.id_commande, c.date_commande, c.statut_commande, c.prix_total_commande, " +
                            "cl.id_client, cl.nom_client, cl.prenom_client " +
                            "FROM COMMANDE c " +
                            "JOIN CLIENT cl ON c.id_client = cl.id_client " +
                            "WHERE cl.nom_client LIKE @NomClient OR cl.prenom_client LIKE @NomClient " +
                            "ORDER BY c.date_commande DESC";
                    parametres.Add("@NomClient", $"%{nomClient}%");
                    break;
                case "4":
                    Console.Write("Statut (En attente, Confirmée, En préparation, Prête à livrer, En livraison, Livrée, Annulée) : ");
                    string statut = Console.ReadLine();
                    requete = "SELECT c.id_commande, c.date_commande, c.statut_commande, c.prix_total_commande, " +
                            "cl.id_client, cl.nom_client, cl.prenom_client " +
                            "FROM COMMANDE c " +
                            "JOIN CLIENT cl ON c.id_client = cl.id_client " +
                            "WHERE c.statut_commande = @Statut " +
                            "ORDER BY c.date_commande DESC";
                    parametres.Add("@Statut", statut);
                    break;
                case "5":
                    Console.Write("Date de commande (YYYY-MM-DD) : ");
                    string dateCommande = Console.ReadLine();
                    requete = "SELECT c.id_commande, c.date_commande, c.statut_commande, c.prix_total_commande, " +
                            "cl.id_client, cl.nom_client, cl.prenom_client " +
                            "FROM COMMANDE c " +
                            "JOIN CLIENT cl ON c.id_client = cl.id_client " +
                            "WHERE DATE(c.date_commande) = @DateCommande " +
                            "ORDER BY c.date_commande DESC";
                    parametres.Add("@DateCommande", dateCommande);
                    break;
                default:
                    GestionBDD.AfficherMessageErreur("Option invalide.");
                    return;
            }

            try
            {
                DataTable resultats = GestionBDD.ExecuterRequeteTable(requete, parametres);

                if (resultats != null && resultats.Rows.Count > 0)
                {
                    Console.WriteLine("\nRésultats de la recherche :");
                    Console.WriteLine("ID\tDate\t\tStatut\t\tPrix\t\tClient");
                    Console.WriteLine("-----------------------------------------------------");

                    foreach (DataRow row in resultats.Rows)
                    {
                        string id = row["id_commande"].ToString();
                        string date = row["date_commande"] == DBNull.Value ? "N/A" : Convert.ToDateTime(row["date_commande"]).ToString("dd/MM/yyyy");
                        string statut = row["statut_commande"] == DBNull.Value ? "N/A" : row["statut_commande"].ToString();
                        string prix = row["prix_total_commande"] == DBNull.Value ? "N/A" : row["prix_total_commande"].ToString();
                        string nom = row["nom_client"] == DBNull.Value ? "N/A" : row["nom_client"].ToString();
                        string prenom = row["prenom_client"] == DBNull.Value ? "N/A" : row["prenom_client"].ToString();
                        string client = $"{nom} {prenom}";

                        Console.WriteLine($"{id}\t{date}\t{statut,-15}{prix,-15}{client}");
                    }

                    Console.Write("\nVoulez-vous voir les détails d'une commande ? (O/N) : ");
                    if (Console.ReadLine().ToUpper() == "O")
                    {
                        AfficherDetailsCommande();
                    }
                }
                else
                {
                    Console.WriteLine("Aucune commande trouvée avec ces critères.");
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
        /// Permet de modifier une commande existante
        /// </summary>
        private static void ModifierCommande()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MODIFIER UNE COMMANDE");

            Console.Write("ID de la commande à modifier : ");
            string idCommande = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Vérifier si la commande existe
                    string requeteVerif = "SELECT COUNT(*) FROM COMMANDE WHERE id_commande = @IdCommande";
                    MySqlCommand cmdVerif = new MySqlCommand(requeteVerif, connection);
                    cmdVerif.Parameters.AddWithValue("@IdCommande", idCommande);

                    int count = Convert.ToInt32(cmdVerif.ExecuteScalar());
                    if (count == 0)
                    {
                        GestionBDD.AfficherMessageErreur("Commande introuvable.");
                        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        return;
                    }

                    // Récupérer les informations actuelles
                    string requeteInfo = "SELECT c.statut_commande, c.commentaire_commande, c.id_client " +
                                        "FROM COMMANDE c " +
                                        "WHERE c.id_commande = @IdCommande";
                    MySqlCommand cmdInfo = new MySqlCommand(requeteInfo, connection);
                    cmdInfo.Parameters.AddWithValue("@IdCommande", idCommande);

                    string statutActuel = "";
                    string commentaireActuel = "";
                    string idClientActuel = "";

                    using (MySqlDataReader reader = cmdInfo.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            statutActuel = reader["statut_commande"] == DBNull.Value ? "" : reader["statut_commande"].ToString();
                            commentaireActuel = reader["commentaire_commande"] == DBNull.Value ? "" : reader["commentaire_commande"].ToString();
                            idClientActuel = reader["id_client"].ToString();
                        }
                    }

                    Console.WriteLine($"\nModification de la commande #{idCommande}");
                    Console.WriteLine("Laissez vide pour conserver la valeur actuelle.");

                    Console.Write($"Nouveau statut ({statutActuel}) : ");
                    string nouveauStatut = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(nouveauStatut))
                        nouveauStatut = statutActuel;

                    Console.Write($"Nouveau commentaire ({commentaireActuel}) : ");
                    string nouveauCommentaire = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(nouveauCommentaire))
                        nouveauCommentaire = commentaireActuel;

                    Console.Write($"Nouvel ID client ({idClientActuel}) : ");
                    string nouvelIdClient = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(nouvelIdClient))
                        nouvelIdClient = idClientActuel;

                    // Mettre à jour la commande
                    string requeteUpdate = "UPDATE COMMANDE SET " +
                                          "statut_commande = @Statut, " +
                                          "commentaire_commande = @Commentaire, " +
                                          "id_client = @IdClient " +
                                          "WHERE id_commande = @IdCommande";

                    MySqlCommand cmdUpdate = new MySqlCommand(requeteUpdate, connection);
                    cmdUpdate.Parameters.AddWithValue("@IdCommande", idCommande);
                    cmdUpdate.Parameters.AddWithValue("@Statut", nouveauStatut);
                    cmdUpdate.Parameters.AddWithValue("@Commentaire", nouveauCommentaire);
                    cmdUpdate.Parameters.AddWithValue("@IdClient", nouvelIdClient);

                    int resultat = cmdUpdate.ExecuteNonQuery();
                    if (resultat > 0)
                    {
                        GestionBDD.AfficherMessageSucces("Commande modifiée avec succès !");

                        Console.Write("Voulez-vous gérer les lignes de cette commande ? (O/N) : ");
                        if (Console.ReadLine().ToUpper() == "O")
                        {
                            GererLignesCommande(idCommande);
                        }
                    }
                    else
                    {
                        GestionBDD.AfficherMessageErreur("Erreur lors de la modification de la commande.");
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
        /// Permet de supprimer une commande
        /// </summary>
        private static void SupprimerCommande()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("SUPPRIMER UNE COMMANDE");

            Console.Write("ID de la commande à supprimer : ");
            string idCommande = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Vérifier si la commande existe
                    string requeteVerif = "SELECT c.id_commande, c.statut_commande, cl.nom_client, cl.prenom_client " +
                                        "FROM COMMANDE c " +
                                        "JOIN CLIENT cl ON c.id_client = cl.id_client " +
                                        "WHERE c.id_commande = @IdCommande";
                    MySqlCommand cmdVerif = new MySqlCommand(requeteVerif, connection);
                    cmdVerif.Parameters.AddWithValue("@IdCommande", idCommande);

                    using (MySqlDataReader reader = cmdVerif.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string statut = reader["statut_commande"] == DBNull.Value ? "N/A" : reader["statut_commande"].ToString();
                            string nomClient = reader["nom_client"] == DBNull.Value ? "N/A" : reader["nom_client"].ToString();
                            string prenomClient = reader["prenom_client"] == DBNull.Value ? "N/A" : reader["prenom_client"].ToString();

                            reader.Close();

                            Console.WriteLine($"Vous êtes sur le point de supprimer la commande #{idCommande}");
                            Console.WriteLine($"Client : {nomClient} {prenomClient}");
                            Console.WriteLine($"Statut : {statut}");
                            Console.Write("Êtes-vous sûr ? (O/N) : ");
                            if (Console.ReadLine().ToUpper() != "O")
                            {
                                Console.WriteLine("Suppression annulée.");
                                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                                Console.ReadKey();
                                return;
                            }

                            // Supprimer les lignes de commande
                            string deleteLignes = "DELETE FROM LIGNE_COMMANDE WHERE id_commande = @IdCommande";
                            MySqlCommand cmdDeleteLignes = new MySqlCommand(deleteLignes, connection);
                            cmdDeleteLignes.Parameters.AddWithValue("@IdCommande", idCommande);
                            cmdDeleteLignes.ExecuteNonQuery();

                            // Supprimer les relations dans la table appartient
                            string deleteAppartient = "DELETE FROM appartient WHERE id_commande = @IdCommande";
                            MySqlCommand cmdDeleteAppartient = new MySqlCommand(deleteAppartient, connection);
                            cmdDeleteAppartient.Parameters.AddWithValue("@IdCommande", idCommande);
                            cmdDeleteAppartient.ExecuteNonQuery();

                            // Supprimer la commande
                            string deleteCommande = "DELETE FROM COMMANDE WHERE id_commande = @IdCommande";
                            MySqlCommand cmdDeleteCommande = new MySqlCommand(deleteCommande, connection);
                            cmdDeleteCommande.Parameters.AddWithValue("@IdCommande", idCommande);
                            int resultat = cmdDeleteCommande.ExecuteNonQuery();

                            if (resultat > 0)
                            {
                                GestionBDD.AfficherMessageSucces("Commande supprimée avec succès !");
                            }
                            else
                            {
                                GestionBDD.AfficherMessageErreur("Erreur lors de la suppression de la commande.");
                            }
                        }
                        else
                        {
                            reader.Close();
                            GestionBDD.AfficherMessageErreur("Commande introuvable.");
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
        /// Permet de gérer les lignes d'une commande
        /// </summary>
        private static void GererLignesCommande(string idCommande = null)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("GÉRER LES LIGNES DE COMMANDE");

            if (idCommande == null)
            {
                Console.Write("ID de la commande : ");
                idCommande = Console.ReadLine();
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Vérifier si la commande existe
                    string requeteVerif = "SELECT COUNT(*) FROM COMMANDE WHERE id_commande = @IdCommande";
                    MySqlCommand cmdVerif = new MySqlCommand(requeteVerif, connection);
                    cmdVerif.Parameters.AddWithValue("@IdCommande", idCommande);

                    int count = Convert.ToInt32(cmdVerif.ExecuteScalar());
                    if (count == 0)
                    {
                        GestionBDD.AfficherMessageErreur("Commande introuvable.");
                        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        return;
                    }

                    bool continuer = true;
                    while (continuer)
                    {
                        Console.Clear();
                        GestionBDD.AfficherTitre($"GÉRER LES LIGNES DE LA COMMANDE #{idCommande}");

                        // Afficher les lignes actuelles
                        string requeteLignes = "SELECT lc.id_ligne, lc.id_plat, p.nom_plat, lc.prix_unitaire, " +
                                            "lc.date_livraison_voulue, lc.statut_ligne, " +
                                            "lc.id_cuisinier, cu.nom_cuisinier, cu.prenom_cuisinier " +
                                            "FROM LIGNE_COMMANDE lc " +
                                            "JOIN PLAT p ON lc.id_plat = p.id_plat " +
                                            "LEFT JOIN CUISINIER cu ON lc.id_cuisinier = cu.id_cuisinier " +
                                            "WHERE lc.id_commande = @IdCommande " +
                                            "ORDER BY lc.id_ligne";

                        MySqlCommand cmdLignes = new MySqlCommand(requeteLignes, connection);
                        cmdLignes.Parameters.AddWithValue("@IdCommande", idCommande);

                        Console.WriteLine("\nLignes de commande actuelles :");
                        Console.WriteLine("ID\tPlat\t\tPrix\tDate livraison\tStatut\t\tCuisinier");
                        Console.WriteLine("-------------------------------------------------------------------------");

                        using (MySqlDataReader reader = cmdLignes.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string idLigne = reader["id_ligne"].ToString();
                                    string nomPlat = reader["nom_plat"] == DBNull.Value ? "N/A" : reader["nom_plat"].ToString();
                                    string prixUnitaire = reader["prix_unitaire"] == DBNull.Value ? "N/A" : reader["prix_unitaire"].ToString();
                                    string dateLivraisonVoulue = reader["date_livraison_voulue"] == DBNull.Value ? "N/A" : Convert.ToDateTime(reader["date_livraison_voulue"]).ToString("dd/MM/yyyy");
                                    string statutLigne = reader["statut_ligne"] == DBNull.Value ? "N/A" : reader["statut_ligne"].ToString();

                                    string cuisinier = "Non assigné";
                                    if (reader["id_cuisinier"] != DBNull.Value)
                                    {
                                        string nomCuisinier = reader["nom_cuisinier"] == DBNull.Value ? "" : reader["nom_cuisinier"].ToString();
                                        string prenomCuisinier = reader["prenom_cuisinier"] == DBNull.Value ? "" : reader["prenom_cuisinier"].ToString();
                                        cuisinier = $"{nomCuisinier} {prenomCuisinier}";
                                    }

                                    Console.WriteLine($"{idLigne}\t{nomPlat,-15}{prixUnitaire}\t{dateLivraisonVoulue}\t{statutLigne,-15}{cuisinier}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Aucune ligne de commande trouvée.");
                            }
                        }

                        Console.WriteLine("\nQue souhaitez-vous faire ?");
                        Console.WriteLine("1. Ajouter une ligne");
                        Console.WriteLine("2. Modifier une ligne");
                        Console.WriteLine("3. Supprimer une ligne");
                        Console.WriteLine("4. Assigner un cuisinier à une ligne");
                        Console.WriteLine("5. Retour");
                        Console.Write("Votre choix : ");
                        string choix = Console.ReadLine();

                        switch (choix)
                        {
                            case "1":
                                AjouterLigneCommande(idCommande);
                                break;
                            case "2":
                                ModifierLigneCommande();
                                break;
                            case "3":
                                SupprimerLigneCommande();
                                break;
                            case "4":
                                AssignerCuisinierLigneCommande();
                                break;
                            case "5":
                                continuer = false;
                                break;
                            default:
                                GestionBDD.AfficherMessageErreur("Option invalide.");
                                break;
                        }

                        // Recalculer le prix total de la commande
                        if (choix == "1" || choix == "2" || choix == "3")
                        {
                            decimal nouveauPrix = CalculerPrixTotal(idCommande);
                            if (nouveauPrix >= 0)
                            {
                                string requetePrix = "UPDATE COMMANDE SET prix_total_commande = @NouveauPrix WHERE id_commande = @IdCommande";
                                MySqlCommand cmdPrix = new MySqlCommand(requetePrix, connection);
                                cmdPrix.Parameters.AddWithValue("@IdCommande", idCommande);
                                cmdPrix.Parameters.AddWithValue("@NouveauPrix", nouveauPrix + " euros");
                                cmdPrix.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }
        }

        /// <summary>
        /// Permet de modifier une ligne de commande
        /// </summary>
        private static void ModifierLigneCommande()
        {
            Console.Write("ID de la ligne à modifier : ");
            string idLigne = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Vérifier si la ligne existe
                    string requeteVerif = "SELECT lc.id_ligne, lc.id_plat, p.nom_plat, lc.prix_unitaire, " +
                                        "lc.date_livraison_voulue, lc.statut_ligne, lc.id_commande " +
                                        "FROM LIGNE_COMMANDE lc " +
                                        "JOIN PLAT p ON lc.id_plat = p.id_plat " +
                                        "WHERE lc.id_ligne = @IdLigne";
                    MySqlCommand cmdVerif = new MySqlCommand(requeteVerif, connection);
                    cmdVerif.Parameters.AddWithValue("@IdLigne", idLigne);

                    string idPlat = "";
                    string nomPlat = "";
                    string prixUnitaire = "";
                    string dateLivraisonVoulue = "";
                    string statutLigne = "";
                    string idCommande = "";

                    using (MySqlDataReader reader = cmdVerif.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idPlat = reader["id_plat"].ToString();
                            nomPlat = reader["nom_plat"] == DBNull.Value ? "N/A" : reader["nom_plat"].ToString();
                            prixUnitaire = reader["prix_unitaire"] == DBNull.Value ? "N/A" : reader["prix_unitaire"].ToString();
                            dateLivraisonVoulue = reader["date_livraison_voulue"] == DBNull.Value ? "N/A" : Convert.ToDateTime(reader["date_livraison_voulue"]).ToString("yyyy-MM-dd");
                            statutLigne = reader["statut_ligne"] == DBNull.Value ? "N/A" : reader["statut_ligne"].ToString();
                            idCommande = reader["id_commande"].ToString();
                        }
                        else
                        {
                            reader.Close();
                            GestionBDD.AfficherMessageErreur("Ligne de commande introuvable.");
                            return;
                        }
                    }

                    Console.WriteLine($"\nModification de la ligne #{idLigne} - Plat : {nomPlat}");
                    Console.WriteLine("Laissez vide pour conserver la valeur actuelle.");

                    Console.Write($"Nouvel ID de plat ({idPlat}) : ");
                    string nouvelIdPlat = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(nouvelIdPlat))
                        nouvelIdPlat = idPlat;

                    // Vérifier si le nouveau plat existe
                    if (nouvelIdPlat != idPlat)
                    {
                        string requeteVerifPlat = "SELECT COUNT(*) FROM PLAT WHERE id_plat = @IdPlat";
                        MySqlCommand cmdVerifPlat = new MySqlCommand(requeteVerifPlat, connection);
                        cmdVerifPlat.Parameters.AddWithValue("@IdPlat", nouvelIdPlat);
                        int countPlat = Convert.ToInt32(cmdVerifPlat.ExecuteScalar());
                        if (countPlat == 0)
                        {
                            GestionBDD.AfficherMessageErreur("Plat introuvable.");
                            return;
                        }
                    }

                    Console.Write($"Nouveau prix unitaire ({prixUnitaire}) : ");
                    string nouveauPrixUnitaire = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(nouveauPrixUnitaire))
                        nouveauPrixUnitaire = prixUnitaire;

                    Console.Write($"Nouvelle date de livraison voulue ({dateLivraisonVoulue}) : ");
                    string nouvelleDateLivraison = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(nouvelleDateLivraison))
                        nouvelleDateLivraison = dateLivraisonVoulue;

                    Console.Write($"Nouveau statut ({statutLigne}) : ");
                    string nouveauStatut = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(nouveauStatut))
                        nouveauStatut = statutLigne;

                    // Mettre à jour la ligne
                    string requeteUpdate = "UPDATE LIGNE_COMMANDE SET " +
                                          "id_plat = @IdPlat, " +
                                          "prix_unitaire = @PrixUnitaire, " +
                                          "date_livraison_voulue = @DateLivraison, " +
                                          "statut_ligne = @Statut " +
                                          "WHERE id_ligne = @IdLigne";

                    MySqlCommand cmdUpdate = new MySqlCommand(requeteUpdate, connection);
                    cmdUpdate.Parameters.AddWithValue("@IdLigne", idLigne);
                    cmdUpdate.Parameters.AddWithValue("@IdPlat", nouvelIdPlat);
                    cmdUpdate.Parameters.AddWithValue("@PrixUnitaire", nouveauPrixUnitaire);
                    cmdUpdate.Parameters.AddWithValue("@DateLivraison", nouvelleDateLivraison);
                    cmdUpdate.Parameters.AddWithValue("@Statut", nouveauStatut);

                    int resultat = cmdUpdate.ExecuteNonQuery();
                    if (resultat > 0)
                    {
                        // Mettre à jour la table appartient si le plat a changé
                        if (nouvelIdPlat != idPlat)
                        {
                            // Supprimer l'ancienne relation
                            string deleteAppartient = "DELETE FROM appartient WHERE id_plat = @IdPlatAncien AND id_commande = @IdCommande";
                            MySqlCommand cmdDeleteAppartient = new MySqlCommand(deleteAppartient, connection);
                            cmdDeleteAppartient.Parameters.AddWithValue("@IdPlatAncien", idPlat);
                            cmdDeleteAppartient.Parameters.AddWithValue("@IdCommande", idCommande);
                            cmdDeleteAppartient.ExecuteNonQuery();

                            // Ajouter la nouvelle relation
                            string insertAppartient = "INSERT INTO appartient (id_plat, id_commande) VALUES (@IdPlatNouveau, @IdCommande)";
                            MySqlCommand cmdInsertAppartient = new MySqlCommand(insertAppartient, connection);
                            cmdInsertAppartient.Parameters.AddWithValue("@IdPlatNouveau", nouvelIdPlat);
                            cmdInsertAppartient.Parameters.AddWithValue("@IdCommande", idCommande);
                            cmdInsertAppartient.ExecuteNonQuery();
                        }

                        GestionBDD.AfficherMessageSucces("Ligne de commande modifiée avec succès !");
                    }
                    else
                    {
                        GestionBDD.AfficherMessageErreur("Erreur lors de la modification de la ligne de commande.");
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }
        }

        /// <summary>
        /// Permet de supprimer une ligne de commande
        /// </summary>
        private static void SupprimerLigneCommande()
        {
            Console.Write("ID de la ligne à supprimer : ");
            string idLigne = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Vérifier si la ligne existe
                    string requeteVerif = "SELECT lc.id_ligne, lc.id_plat, p.nom_plat, lc.id_commande " +
                                        "FROM LIGNE_COMMANDE lc " +
                                        "JOIN PLAT p ON lc.id_plat = p.id_plat " +
                                        "WHERE lc.id_ligne = @IdLigne";
                    MySqlCommand cmdVerif = new MySqlCommand(requeteVerif, connection);
                    cmdVerif.Parameters.AddWithValue("@IdLigne", idLigne);

                    string idPlat = "";
                    string nomPlat = "";
                    string idCommande = "";

                    using (MySqlDataReader reader = cmdVerif.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idPlat = reader["id_plat"].ToString();
                            nomPlat = reader["nom_plat"] == DBNull.Value ? "N/A" : reader["nom_plat"].ToString();
                            idCommande = reader["id_commande"].ToString();
                        }
                        else
                        {
                            reader.Close();
                            GestionBDD.AfficherMessageErreur("Ligne de commande introuvable.");
                            return;
                        }
                    }

                    Console.WriteLine($"Vous êtes sur le point de supprimer la ligne #{idLigne} - Plat : {nomPlat}");
                    Console.Write("Êtes-vous sûr ? (O/N) : ");
                    if (Console.ReadLine().ToUpper() != "O")
                    {
                        Console.WriteLine("Suppression annulée.");
                        return;
                    }

                    // Supprimer la ligne
                    string deleteLigne = "DELETE FROM LIGNE_COMMANDE WHERE id_ligne = @IdLigne";
                    MySqlCommand cmdDeleteLigne = new MySqlCommand(deleteLigne, connection);
                    cmdDeleteLigne.Parameters.AddWithValue("@IdLigne", idLigne);
                    int resultat = cmdDeleteLigne.ExecuteNonQuery();

                    if (resultat > 0)
                    {
                        // Vérifier s'il reste d'autres lignes avec ce plat dans cette commande
                        string requeteVerifPlat = "SELECT COUNT(*) FROM LIGNE_COMMANDE WHERE id_plat = @IdPlat AND id_commande = @IdCommande";
                        MySqlCommand cmdVerifPlat = new MySqlCommand(requeteVerifPlat, connection);
                        cmdVerifPlat.Parameters.AddWithValue("@IdPlat", idPlat);
                        cmdVerifPlat.Parameters.AddWithValue("@IdCommande", idCommande);
                        int countPlat = Convert.ToInt32(cmdVerifPlat.ExecuteScalar());

                        // S'il n'y a plus de lignes avec ce plat, supprimer la relation dans appartient
                        if (countPlat == 0)
                        {
                            string deleteAppartient = "DELETE FROM appartient WHERE id_plat = @IdPlat AND id_commande = @IdCommande";
                            MySqlCommand cmdDeleteAppartient = new MySqlCommand(deleteAppartient, connection);
                            cmdDeleteAppartient.Parameters.AddWithValue("@IdPlat", idPlat);
                            cmdDeleteAppartient.Parameters.AddWithValue("@IdCommande", idCommande);
                            cmdDeleteAppartient.ExecuteNonQuery();
                        }

                        GestionBDD.AfficherMessageSucces("Ligne de commande supprimée avec succès !");
                    }
                    else
                    {
                        GestionBDD.AfficherMessageErreur("Erreur lors de la suppression de la ligne de commande.");
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }
        }

        /// <summary>
        /// Permet de suivre l'état des commandes
        /// </summary>
        private static void SuivreEtatCommandes()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("SUIVI DES COMMANDES");

            Console.WriteLine("Filtrer par statut :");
            Console.WriteLine("1. Toutes les commandes");
            Console.WriteLine("2. En attente");
            Console.WriteLine("3. Confirmées");
            Console.WriteLine("4. En préparation");
            Console.WriteLine("5. Prêtes à livrer");
            Console.WriteLine("6. En livraison");
            Console.WriteLine("7. Livrées");
            Console.WriteLine("8. Annulées");
            Console.Write("Votre choix : ");
            string choixStatut = Console.ReadLine();

            string filtreStatut = "";
            switch (choixStatut)
            {
                case "2":
                    filtreStatut = "En attente";
                    break;
                case "3":
                    filtreStatut = "Confirmée";
                    break;
                case "4":
                    filtreStatut = "En préparation";
                    break;
                case "5":
                    filtreStatut = "Prête à livrer";
                    break;
                case "6":
                    filtreStatut = "En livraison";
                    break;
                case "7":
                    filtreStatut = "Livrée";
                    break;
                case "8":
                    filtreStatut = "Annulée";
                    break;
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete = "SELECT c.id_commande, c.date_commande, c.date_livraison, c.statut_commande, " +
                                    "c.prix_total_commande, cl.nom_client, cl.prenom_client, " +
                                    "(SELECT COUNT(*) FROM LIGNE_COMMANDE WHERE id_commande = c.id_commande) AS nombre_lignes " +
                                    "FROM COMMANDE c " +
                                    "JOIN CLIENT cl ON c.id_client = cl.id_client ";

                    if (!string.IsNullOrEmpty(filtreStatut))
                    {
                        requete += "WHERE c.statut_commande = @Statut ";
                    }

                    requete += "ORDER BY c.date_commande DESC";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    if (!string.IsNullOrEmpty(filtreStatut))
                    {
                        cmd.Parameters.AddWithValue("@Statut", filtreStatut);
                    }

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("\nID\tDate commande\tDate livraison\tStatut\t\tPrix\t\tClient\t\tLignes");
                            Console.WriteLine("-------------------------------------------------------------------------");

                            while (reader.Read())
                            {
                                string id = reader["id_commande"].ToString();
                                string dateCommande = reader["date_commande"] == DBNull.Value ? "N/A" : Convert.ToDateTime(reader["date_commande"]).ToString("dd/MM/yyyy");
                                string dateLivraison = reader["date_livraison"] == DBNull.Value ? "N/A" : Convert.ToDateTime(reader["date_livraison"]).ToString("dd/MM/yyyy");
                                string statut = reader["statut_commande"] == DBNull.Value ? "N/A" : reader["statut_commande"].ToString();
                                string prix = reader["prix_total_commande"] == DBNull.Value ? "N/A" : reader["prix_total_commande"].ToString();
                                string nomClient = reader["nom_client"] == DBNull.Value ? "N/A" : reader["nom_client"].ToString();
                                string prenomClient = reader["prenom_client"] == DBNull.Value ? "N/A" : reader["prenom_client"].ToString();
                                string client = $"{nomClient} {prenomClient}";
                                string nombreLignes = reader["nombre_lignes"].ToString();

                                Console.WriteLine($"{id}\t{dateCommande}\t{dateLivraison}\t{statut,-15}{prix,-15}{client,-15}{nombreLignes}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucune commande trouvée avec ces critères.");
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
        /// Permet d'assigner un cuisinier à une ligne de commande
        /// </summary>
        private static void AssignerCuisinierLigneCommande()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("ASSIGNER UN CUISINIER À UNE LIGNE DE COMMANDE");

            Console.Write("ID de la ligne de commande : ");
            string idLigne = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Vérifier si la ligne existe
                    string requeteVerif = "SELECT lc.id_ligne, lc.id_plat, p.nom_plat, lc.id_commande, lc.id_cuisinier " +
                                        "FROM LIGNE_COMMANDE lc " +
                                        "JOIN PLAT p ON lc.id_plat = p.id_plat " +
                                        "WHERE lc.id_ligne = @IdLigne";
                    MySqlCommand cmdVerif = new MySqlCommand(requeteVerif, connection);
                    cmdVerif.Parameters.AddWithValue("@IdLigne", idLigne);

                    string nomPlat = "";
                    string idCuisinierActuel = "";

                    using (MySqlDataReader reader = cmdVerif.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            nomPlat = reader["nom_plat"] == DBNull.Value ? "N/A" : reader["nom_plat"].ToString();
                            idCuisinierActuel = reader["id_cuisinier"] == DBNull.Value ? "" : reader["id_cuisinier"].ToString();
                        }
                        else
                        {
                            reader.Close();
                            GestionBDD.AfficherMessageErreur("Ligne de commande introuvable.");
                            return;
                        }
                    }

                    // Afficher les cuisiniers disponibles
                    string requeteCuisiniers = "SELECT id_cuisinier, nom_cuisinier, prenom_cuisinier, specialite_cuisinier " +
                                             "FROM CUISINIER " +
                                             "ORDER BY nom_cuisinier, prenom_cuisinier";
                    MySqlCommand cmdCuisiniers = new MySqlCommand(requeteCuisiniers, connection);

                    Console.WriteLine("\nCuisiniers disponibles :");
                    Console.WriteLine("ID\tNom\t\tPrénom\t\tSpécialité");
                    Console.WriteLine("-----------------------------------------------------");

                    using (MySqlDataReader reader = cmdCuisiniers.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string id = reader["id_cuisinier"].ToString();
                                string nom = reader["nom_cuisinier"] == DBNull.Value ? "N/A" : reader["nom_cuisinier"].ToString();
                                string prenom = reader["prenom_cuisinier"] == DBNull.Value ? "N/A" : reader["prenom_cuisinier"].ToString();
                                string specialite = reader["specialite_cuisinier"] == DBNull.Value ? "N/A" : reader["specialite_cuisinier"].ToString();

                                Console.WriteLine($"{id}\t{nom,-15}{prenom,-15}{specialite}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun cuisinier disponible.");
                            return;
                        }
                    }

                    Console.WriteLine($"\nAssignation d'un cuisinier pour la ligne #{idLigne} - Plat : {nomPlat}");
                    if (!string.IsNullOrEmpty(idCuisinierActuel))
                    {
                        Console.WriteLine($"Cuisinier actuellement assigné : {idCuisinierActuel}");
                    }

                    Console.Write("ID du cuisinier à assigner : ");
                    string idCuisinier = Console.ReadLine();

                    // Vérifier si le cuisinier existe
                    string requeteVerifCuisinier = "SELECT COUNT(*) FROM CUISINIER WHERE id_cuisinier = @IdCuisinier";
                    MySqlCommand cmdVerifCuisinier = new MySqlCommand(requeteVerifCuisinier, connection);
                    cmdVerifCuisinier.Parameters.AddWithValue("@IdCuisinier", idCuisinier);
                    int countCuisinier = Convert.ToInt32(cmdVerifCuisinier.ExecuteScalar());
                    if (countCuisinier == 0)
                    {
                        GestionBDD.AfficherMessageErreur("Cuisinier introuvable.");
                        return;
                    }

                    // Mettre à jour la ligne
                    string requeteUpdate = "UPDATE LIGNE_COMMANDE SET id_cuisinier = @IdCuisinier, statut_ligne = 'En préparation' WHERE id_ligne = @IdLigne";
                    MySqlCommand cmdUpdate = new MySqlCommand(requeteUpdate, connection);
                    cmdUpdate.Parameters.AddWithValue("@IdLigne", idLigne);
                    cmdUpdate.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                    int resultat = cmdUpdate.ExecuteNonQuery();
                    if (resultat > 0)
                    {
                        GestionBDD.AfficherMessageSucces("Cuisinier assigné avec succès !");
                    }
                    else
                    {
                        GestionBDD.AfficherMessageErreur("Erreur lors de l'assignation du cuisinier.");
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
    }
}
