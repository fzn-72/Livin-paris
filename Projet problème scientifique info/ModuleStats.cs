using MySql.Data.MySqlClient;
using Projet_problème_scientifique_info;
using System;
using System.Collections.Generic;
using System.Data;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Classe gérant les statistiques du système de livraison de repas
    /// </summary>
    internal class ModuleStats
    {
        //private static string connectionString = "Server=localhost;Database=livinparis2;Uid=root;Pwd=REDACTED;";
        private static string connectionString = "Server=localhost;Database=livinparis2;Uid=root;Pwd=REDACTED;";

        #region Menu principal Stats
        /// <summary>
        /// Affiche le menu principal des statistiques
        /// </summary>
        public static void MenuPrincipalStats()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("MODULE STATISTIQUES");

                if (SessionUtilisateur.EstConnecte())
                {
                    if (SessionUtilisateur.TypeUtilisateur == "client")
                    {
                        Console.WriteLine("1. Afficher l'historique de mes commandes");
                        Console.WriteLine("2. Afficher mes plats préférés");
                        Console.WriteLine("3. Afficher mes cuisiniers préférés");
                        Console.WriteLine("4. Afficher mes dépenses totales");
                        Console.WriteLine("5. Exporter mes statistiques au format XML");
                        Console.WriteLine("6. Retour au menu principal");
                        Console.Write("\nChoisissez une option : ");
                        string choix = Console.ReadLine();

                        switch (choix)
                        {
                            case "1":
                                AfficherHistoriqueCommandesClient(SessionUtilisateur.IdUtilisateur);
                                break;
                            case "2":
                                AfficherPlatsPreferesClient(SessionUtilisateur.IdUtilisateur);
                                break;
                            case "3":
                                AfficherCuisiniersPreferesClient(SessionUtilisateur.IdUtilisateur);
                                break;
                            case "4":
                                AfficherDepensesTotalesClient(SessionUtilisateur.IdUtilisateur);
                                break;
                            case "5":
                                ExporterStatistiquesClient(SessionUtilisateur.IdUtilisateur);
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
                        Console.WriteLine("1. Afficher mes commandes en cours");
                        Console.WriteLine("2. Afficher mes plats les plus populaires");
                        Console.WriteLine("3. Afficher mes évaluations");
                        Console.WriteLine("4. Afficher mes clients fréquents");
                        Console.WriteLine("5. Afficher mes statistiques de vente");
                        Console.WriteLine("6. Exporter mes statistiques au format XML");
                        Console.WriteLine("7. Retour au menu principal");
                        Console.Write("\nChoisissez une option : ");
                        string choix = Console.ReadLine();

                        switch (choix)
                        {
                            case "1":
                                AfficherCommandesEnCoursCuisinier(SessionUtilisateur.IdUtilisateur);
                                break;
                            case "2":
                                AfficherPlatsPopulairesCuisinier(SessionUtilisateur.IdUtilisateur);
                                break;
                            case "3":
                                AfficherEvaluationsCuisinier(SessionUtilisateur.IdUtilisateur);
                                break;
                            case "4":
                                AfficherClientsFrequentsCuisinier(SessionUtilisateur.IdUtilisateur);
                                break;
                            case "5":
                                AfficherStatistiquesVenteCuisinier(SessionUtilisateur.IdUtilisateur);
                                break;
                            case "6":
                                ExporterStatistiquesCuisinier(SessionUtilisateur.IdUtilisateur);
                                break;
                            case "7":
                                continuer = false;
                                break;
                            default:
                                GestionBDD.AfficherMessageErreur("Option invalide.");
                                break;
                        }
                    }
                    else if (SessionUtilisateur.TypeUtilisateur == "admin")
                    {
                        Console.WriteLine("1. Afficher le nombre de livraisons par cuisinier");
                        Console.WriteLine("2. Afficher les commandes selon une période de temps");
                        Console.WriteLine("3. Afficher la moyenne des prix des commandes");
                        Console.WriteLine("4. Afficher la moyenne des comptes clients");
                        Console.WriteLine("5. Afficher les commandes par nationalité et période");
                        Console.WriteLine("6. Afficher les meilleurs cuisiniers par période");
                        Console.WriteLine("7. Afficher les statistiques globales");
                        Console.WriteLine("8. Exporter les statistiques globales au format XML");
                        Console.WriteLine("9. Retour au menu principal");
                        Console.Write("\nChoisissez une option : ");
                        string choix = Console.ReadLine();

                        switch (choix)
                        {
                            case "1":
                                AfficherLivraisonsParCuisinier();
                                break;
                            case "2":
                                AfficherCommandesParPeriode();
                                break;
                            case "3":
                                AfficherMoyennePrixCommandes();
                                break;
                            case "4":
                                AfficherMoyenneComptesClients();
                                break;
                            case "5":
                                AfficherCommandesClientParNationaliteEtPeriode();
                                break;
                            case "6":
                                AfficherMeilleursCuisiniersParPeriode();
                                break;
                            case "7":
                                AfficherStatistiquesGlobales();
                                break;
                            case "8":
                                ExporterStatistiquesGlobales();
                                break;
                            case "9":
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
                    Console.WriteLine("1. Afficher les statistiques générales");
                    Console.WriteLine("2. Afficher les meilleurs cuisiniers");
                    Console.WriteLine("3. Afficher les plats les plus populaires");
                    Console.WriteLine("4. Exporter les statistiques générales au format XML");
                    Console.WriteLine("5. Retour au menu principal");
                    Console.Write("\nChoisissez une option : ");
                    string choix = Console.ReadLine();

                    switch (choix)
                    {
                        case "1":
                            AfficherStatistiquesGenerales();
                            break;
                        case "2":
                            AfficherMeilleursCuisiniers();
                            break;
                        case "3":
                            AfficherPlatsPopulaires();
                            break;
                        case "4":
                            ExporterStatistiquesGlobales();
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
        }
        #endregion

        #region Statistiques pour clients

        /// <summary>
        /// Affiche l'historique des commandes d'un client
        /// </summary>
        private static void AfficherHistoriqueCommandesClient(string idClient)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("HISTORIQUE DE MES COMMANDES");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT c.id_commande, c.date_commande, c.prix_total_commande, c.statut_commande,
                               COUNT(lc.id_ligne) AS nombre_plats
                        FROM COMMANDE c
                        JOIN LIGNE_COMMANDE lc ON c.id_commande = lc.id_commande
                        WHERE c.id_client = @IdClient
                        GROUP BY c.id_commande, c.date_commande, c.prix_total_commande, c.statut_commande
                        ORDER BY c.date_commande DESC";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdClient", idClient);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-15} {4,-10}",
                                "ID", "Date", "Prix", "Statut", "Nb plats");
                            Console.WriteLine(new string('-', 65));

                            while (reader.Read())
                            {
                                string id = reader["id_commande"].ToString();
                                string date = reader.IsDBNull(reader.GetOrdinal("date_commande")) ? "N/A" : reader["date_commande"].ToString();
                                string prix = reader.IsDBNull(reader.GetOrdinal("prix_total_commande")) ? "N/A" : reader["prix_total_commande"].ToString();
                                string statut = reader.IsDBNull(reader.GetOrdinal("statut_commande")) ? "N/A" : reader["statut_commande"].ToString();
                                int nombrePlats = reader.IsDBNull(reader.GetOrdinal("nombre_plats")) ? 0 : Convert.ToInt32(reader["nombre_plats"]);

                                Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-15} {4,-10}",
                                    id, date, prix, statut, nombrePlats);
                            }

                            Console.WriteLine("\nOptions :");
                            Console.WriteLine("1. Exporter ces données au format XML");
                            Console.WriteLine("2. Retour au menu précédent");

                            Console.Write("\nVotre choix : ");
                            string choix = Console.ReadLine();

                            if (choix == "1")
                            {
                                ExporterStatistiquesClient(idClient);
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
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        /// <summary>
        /// Affiche les plats préférés d'un client (les plus commandés)
        /// </summary>
        private static void AfficherPlatsPreferesClient(string idClient)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MES PLATS PRÉFÉRÉS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT p.id_plat, p.nom_plat, p.type_plat, p.nationalite_plat,
                               COUNT(lc.id_ligne) AS nombre_commandes
                        FROM COMMANDE c
                        JOIN LIGNE_COMMANDE lc ON c.id_commande = lc.id_commande
                        JOIN PLAT p ON lc.id_plat = p.id_plat
                        WHERE c.id_client = @IdClient
                        GROUP BY p.id_plat, p.nom_plat, p.type_plat, p.nationalite_plat
                        ORDER BY nombre_commandes DESC
                        LIMIT 10";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdClient", idClient);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-15} {4,-10}",
                                "ID", "Nom", "Type", "Nationalité", "Commandes");
                            Console.WriteLine(new string('-', 70));

                            while (reader.Read())
                            {
                                string id = reader["id_plat"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom_plat")) ? "N/A" : reader["nom_plat"].ToString();
                                string type = reader.IsDBNull(reader.GetOrdinal("type_plat")) ? "N/A" : reader["type_plat"].ToString();
                                string nationalite = reader.IsDBNull(reader.GetOrdinal("nationalite_plat")) ? "N/A" : reader["nationalite_plat"].ToString();
                                int nombreCommandes = reader.IsDBNull(reader.GetOrdinal("nombre_commandes")) ? 0 : Convert.ToInt32(reader["nombre_commandes"]);

                                Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-15} {4,-10}",
                                    id, nom, type, nationalite, nombreCommandes);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Vous n'avez pas encore commandé de plats.");
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
        /// Affiche les cuisiniers préférés d'un client (les plus commandés)
        /// </summary>
        private static void AfficherCuisiniersPreferesClient(string idClient)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MES CUISINIERS PRÉFÉRÉS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT cu.id_cuisinier, cu.nom_cuisinier, cu.prenom_cuisinier, cu.note_moyenne_cuisinier,
                               COUNT(DISTINCT c.id_commande) AS nombre_commandes
                        FROM COMMANDE c
                        JOIN LIGNE_COMMANDE lc ON c.id_commande = lc.id_commande
                        JOIN PLAT p ON lc.id_plat = p.id_plat
                        JOIN CUISINIER cu ON p.id_cuisinier = cu.id_cuisinier
                        WHERE c.id_client = @IdClient
                        GROUP BY cu.id_cuisinier, cu.nom_cuisinier, cu.prenom_cuisinier, cu.note_moyenne_cuisinier
                        ORDER BY nombre_commandes DESC
                        LIMIT 10";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdClient", idClient);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-5} {1,-30} {2,-10} {3,-10}",
                                "ID", "Nom", "Note", "Commandes");
                            Console.WriteLine(new string('-', 55));

                            while (reader.Read())
                            {
                                string id = reader["id_cuisinier"].ToString();
                                string nom = "";
                                if (!reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) && !reader.IsDBNull(reader.GetOrdinal("prenom_cuisinier")))
                                {
                                    nom = reader["prenom_cuisinier"].ToString() + " " + reader["nom_cuisinier"].ToString();
                                }
                                string note = reader.IsDBNull(reader.GetOrdinal("note_moyenne_cuisinier")) ? "N/A" : reader["note_moyenne_cuisinier"].ToString();
                                int nombreCommandes = reader.IsDBNull(reader.GetOrdinal("nombre_commandes")) ? 0 : Convert.ToInt32(reader["nombre_commandes"]);

                                Console.WriteLine("{0,-5} {1,-30} {2,-10} {3,-10}",
                                    id, nom, note, nombreCommandes);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Vous n'avez pas encore commandé de plats.");
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
        /// Affiche les dépenses totales d'un client
        /// </summary>
        private static void AfficherDepensesTotalesClient(string idClient)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MES DÉPENSES TOTALES");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    /// Dépenses totales
                    string queryTotal = @"
                        SELECT COUNT(id_commande) AS nombre_commandes,
                               SUM(CAST(REPLACE(REPLACE(prix_total_commande, ' euros', ''), ',', '.') AS DECIMAL(10,2))) AS total_depenses
                        FROM COMMANDE
                        WHERE id_client = @IdClient";

                    MySqlCommand cmdTotal = new MySqlCommand(queryTotal, connection);
                    cmdTotal.Parameters.AddWithValue("@IdClient", idClient);

                    using (MySqlDataReader reader = cmdTotal.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int nombreCommandes = reader.IsDBNull(reader.GetOrdinal("nombre_commandes")) ? 0 : Convert.ToInt32(reader["nombre_commandes"]);
                            decimal totalDepenses = reader.IsDBNull(reader.GetOrdinal("total_depenses")) ? 0 : Convert.ToDecimal(reader["total_depenses"]);

                            Console.WriteLine($"Nombre total de commandes : {nombreCommandes}");
                            Console.WriteLine($"Dépenses totales : {totalDepenses:F2} euros");
                            Console.WriteLine($"Dépense moyenne par commande : {(nombreCommandes > 0 ? totalDepenses / nombreCommandes : 0):F2} euros");
                        }
                        else
                        {
                            Console.WriteLine("Aucune donnée disponible.");
                        }
                    }

                    /// Dépenses par mois
                    Console.WriteLine("\nDépenses par mois :");
                    string queryMois = @"
                        SELECT DATE_FORMAT(date_commande, '%Y-%m') AS mois,
                               COUNT(id_commande) AS nombre_commandes,
                               SUM(CAST(REPLACE(REPLACE(prix_total_commande, ' euros', ''), ',', '.') AS DECIMAL(10,2))) AS total_depenses
                        FROM COMMANDE
                        WHERE id_client = @IdClient
                        GROUP BY mois
                        ORDER BY mois DESC
                        LIMIT 12";

                    MySqlCommand cmdMois = new MySqlCommand(queryMois, connection);
                    cmdMois.Parameters.AddWithValue("@IdClient", idClient);

                    using (MySqlDataReader reader = cmdMois.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-10} {1,-15} {2,-15}",
                                "Mois", "Commandes", "Dépenses (euros)");
                            Console.WriteLine(new string('-', 40));

                            while (reader.Read())
                            {
                                string mois = reader.IsDBNull(reader.GetOrdinal("mois")) ? "N/A" : reader["mois"].ToString();
                                int nombreCommandes = reader.IsDBNull(reader.GetOrdinal("nombre_commandes")) ? 0 : Convert.ToInt32(reader["nombre_commandes"]);
                                decimal totalDepenses = reader.IsDBNull(reader.GetOrdinal("total_depenses")) ? 0 : Convert.ToDecimal(reader["total_depenses"]);

                                Console.WriteLine("{0,-10} {1,-15} {2,-15:F2}",
                                    mois, nombreCommandes, totalDepenses);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucune donnée disponible par mois.");
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

        private static void ExporterStatistiquesClient(string idClient)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("EXPORTER LES STATISTIQUES CLIENT");

            try
            {
                ExportStatistiques stats = new ExportStatistiques();
                stats.IdClient = idClient;

                string requeteClient = @"
            SELECT nom_client, prenom_client, email_client
            FROM CLIENT
            WHERE id_client = @IdClient";

                Dictionary<string, object> parametresClient = new Dictionary<string, object>
                {
                    { "@IdClient", idClient }
                };

                DataTable clientDetails = GestionBDD.ExecuterRequeteTable(requeteClient, parametresClient);

                if (clientDetails != null && clientDetails.Rows.Count > 0)
                {
                    DataRow client = clientDetails.Rows[0];
                    stats.NomClient = client["nom_client"] != DBNull.Value ? client["nom_client"].ToString() : "";
                    stats.PrenomClient = client["prenom_client"] != DBNull.Value ? client["prenom_client"].ToString() : "";
                    stats.Email = client["email_client"] != DBNull.Value ? client["email_client"].ToString() : "";
                }

                /// Récupérer l'historique des commandes
                string requeteCommandes = @"
            SELECT c.id_commande, c.date_commande, c.prix_total_commande, c.statut_commande
            FROM COMMANDE c
            WHERE c.id_client = @IdClient
            ORDER BY c.date_commande DESC";

                DataTable commandes = GestionBDD.ExecuterRequeteTable(requeteCommandes, parametresClient);

                if (commandes != null && commandes.Rows.Count > 0)
                {
                    stats.NombreCommandes = commandes.Rows.Count;
                    decimal totalDepenses = 0;

                    foreach (DataRow row in commandes.Rows)
                    {
                        CommandeExport cmd = new CommandeExport
                        {
                            IdCommande = row["id_commande"].ToString(),
                            DateCommande = row["date_commande"] != DBNull.Value ? Convert.ToDateTime(row["date_commande"]) : DateTime.MinValue,
                            PrixTotal = row["prix_total_commande"] != DBNull.Value ? row["prix_total_commande"].ToString() : "0",
                            Statut = row["statut_commande"] != DBNull.Value ? row["statut_commande"].ToString() : ""
                        };

                        /// Convertir le prix total en décimal pour le calcul des dépenses totales
                        if (decimal.TryParse(cmd.PrixTotal.Replace(" euros", "").Replace(",", "."), out decimal prix))
                        {
                            totalDepenses += prix;
                        }

                        /// Récupérer les lignes de commande
                        string requeteLignes = @"
                    SELECT lc.id_ligne, p.nom_plat, lc.prix_unitaire, lc.quantite, 
                           cu.nom_cuisinier, cu.prenom_cuisinier
                    FROM LIGNE_COMMANDE lc
                    JOIN PLAT p ON lc.id_plat = p.id_plat
                    JOIN CUISINIER cu ON p.id_cuisinier = cu.id_cuisinier
                    WHERE lc.id_commande = @IdCommande";

                        Dictionary<string, object> parametresLignes = new Dictionary<string, object>
                        {
                            { "@IdCommande", cmd.IdCommande }
                        };

                        DataTable lignes = GestionBDD.ExecuterRequeteTable(requeteLignes, parametresLignes);

                        if (lignes != null && lignes.Rows.Count > 0)
                        {
                            foreach (DataRow ligne in lignes.Rows)
                            {
                                LigneCommandeExport lc = new LigneCommandeExport
                                {
                                    IdLigne = ligne["id_ligne"].ToString(),
                                    NomPlat = ligne["nom_plat"] != DBNull.Value ? ligne["nom_plat"].ToString() : "",
                                    PrixUnitaire = ligne["prix_unitaire"] != DBNull.Value ? ligne["prix_unitaire"].ToString() : "0",
                                    Quantite = ligne["quantite"] != DBNull.Value ? Convert.ToInt32(ligne["quantite"]) : 0
                                };

                                /// Construire le nom complet du cuisinier
                                string nomCuisinier = ligne["prenom_cuisinier"] != DBNull.Value ? ligne["prenom_cuisinier"].ToString() : "";
                                nomCuisinier += " " + (ligne["nom_cuisinier"] != DBNull.Value ? ligne["nom_cuisinier"].ToString() : "");
                                lc.NomCuisinier = nomCuisinier.Trim();

                                cmd.LignesCommande.Add(lc);
                            }
                        }

                        stats.Commandes.Add(cmd);
                    }

                    stats.DepensesTotales = totalDepenses;
                }

                /// Récupérer les plats préférés
                string requetePlats = @"
            SELECT p.id_plat, p.nom_plat, p.type_plat, p.nationalite_plat,
                   COUNT(lc.id_ligne) AS nombre_commandes
            FROM COMMANDE c
            JOIN LIGNE_COMMANDE lc ON c.id_commande = lc.id_commande
            JOIN PLAT p ON lc.id_plat = p.id_plat
            WHERE c.id_client = @IdClient
            GROUP BY p.id_plat, p.nom_plat, p.type_plat, p.nationalite_plat
            ORDER BY nombre_commandes DESC
            LIMIT 10";

                DataTable plats = GestionBDD.ExecuterRequeteTable(requetePlats, parametresClient);

                if (plats != null && plats.Rows.Count > 0)
                {
                    foreach (DataRow plat in plats.Rows)
                    {
                        PlatPrefere pp = new PlatPrefere
                        {
                            IdPlat = plat["id_plat"].ToString(),
                            NomPlat = plat["nom_plat"] != DBNull.Value ? plat["nom_plat"].ToString() : "",
                            TypePlat = plat["type_plat"] != DBNull.Value ? plat["type_plat"].ToString() : "",
                            Nationalite = plat["nationalite_plat"] != DBNull.Value ? plat["nationalite_plat"].ToString() : "",
                            NombreCommandes = plat["nombre_commandes"] != DBNull.Value ? Convert.ToInt32(plat["nombre_commandes"]) : 0
                        };

                        stats.PlatsPreferees.Add(pp);
                    }
                }

                /// Exporter les statistiques en XML
                string nomFichier = $"StatistiquesClient_{stats.NomClient}_{stats.PrenomClient}_{DateTime.Now:yyyyMMdd_HHmmss}.xml";
                string cheminFichier = ExportXML.ExporterDonnees(stats, nomFichier);

                if (cheminFichier != null)
                {
                    GestionBDD.AfficherMessageSucces($"Les statistiques ont été exportées avec succès dans le fichier : {cheminFichier}");

                    Console.Write("\nVoulez-vous ouvrir le fichier XML ? (O/N) : ");
                    if (Console.ReadLine().ToUpper() == "O")
                    {
                        ExportXML.OuvrirFichierXML(cheminFichier);
                    }
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Erreur lors de l'exportation des statistiques.");
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

        #region Statistiques pour cuisiniers

        /// <summary>
        /// Affiche les commandes en cours pour un cuisinier
        /// </summary>
        private static void AfficherCommandesEnCoursCuisinier(string idCuisinier)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MES COMMANDES EN COURS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT c.id_commande, c.date_commande, c.statut_commande, c.prix_total_commande,
                               cl.nom_client, cl.prenom_client,
                               p.nom_plat, p.type_plat
                        FROM COMMANDE c
                        JOIN CLIENT cl ON c.id_client = cl.id_client
                        JOIN LIGNE_COMMANDE lc ON c.id_commande = lc.id_commande
                        JOIN PLAT p ON lc.id_plat = p.id_plat
                        WHERE p.id_cuisinier = @IdCuisinier
                        AND c.statut_commande IN ('En préparation', 'En attente', 'Confirmée')
                        ORDER BY c.date_commande DESC";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-15} {4,-25} {5,-25}",
                                "ID", "Date", "Statut", "Prix", "Client", "Plat");
                            Console.WriteLine(new string('-', 105));

                            while (reader.Read())
                            {
                                string id = reader["id_commande"].ToString();
                                string date = reader.IsDBNull(reader.GetOrdinal("date_commande")) ? "N/A" : reader["date_commande"].ToString();
                                string statut = reader.IsDBNull(reader.GetOrdinal("statut_commande")) ? "N/A" : reader["statut_commande"].ToString();
                                string prix = reader.IsDBNull(reader.GetOrdinal("prix_total_commande")) ? "N/A" : reader["prix_total_commande"].ToString();

                                string client = "N/A";
                                if (!reader.IsDBNull(reader.GetOrdinal("nom_client")) && !reader.IsDBNull(reader.GetOrdinal("prenom_client")))
                                {
                                    client = reader["prenom_client"].ToString() + " " + reader["nom_client"].ToString();
                                }

                                string plat = reader.IsDBNull(reader.GetOrdinal("nom_plat")) ? "N/A" : reader["nom_plat"].ToString();

                                Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-15} {4,-25} {5,-25}",
                                    id, date, statut, prix, client, plat);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Vous n'avez pas de commandes en cours.");
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
        /// Affiche les plats les plus populaires d'un cuisinier
        /// </summary>
        private static void AfficherPlatsPopulairesCuisinier(string idCuisinier)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MES PLATS LES PLUS POPULAIRES");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT p.id_plat, p.nom_plat, p.type_plat, p.prix_unitaire_plat,
                               COUNT(lc.id_ligne) AS nombre_commandes
                        FROM PLAT p
                        JOIN LIGNE_COMMANDE lc ON p.id_plat = lc.id_plat
                        WHERE p.id_cuisinier = @IdCuisinier
                        GROUP BY p.id_plat, p.nom_plat, p.type_plat, p.prix_unitaire_plat
                        ORDER BY nombre_commandes DESC
                        LIMIT 10";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-15} {4,-10}",
                                "ID", "Nom", "Type", "Prix", "Commandes");
                            Console.WriteLine(new string('-', 70));

                            while (reader.Read())
                            {
                                string id = reader["id_plat"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom_plat")) ? "N/A" : reader["nom_plat"].ToString();
                                string type = reader.IsDBNull(reader.GetOrdinal("type_plat")) ? "N/A" : reader["type_plat"].ToString();
                                string prix = reader.IsDBNull(reader.GetOrdinal("prix_unitaire_plat")) ? "N/A" : reader["prix_unitaire_plat"].ToString();
                                int nombreCommandes = reader.IsDBNull(reader.GetOrdinal("nombre_commandes")) ? 0 : Convert.ToInt32(reader["nombre_commandes"]);

                                Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-15} {4,-10}",
                                    id, nom, type, prix, nombreCommandes);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun de vos plats n'a encore été commandé.");
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
        private static void AfficherEvaluationsCuisinier(string idCuisinier)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MES ÉVALUATIONS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

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
                            Console.WriteLine("Évaluations détaillées :");
                            Console.WriteLine(new string('-', 80));

                            while (reader.Read())
                            {
                                string id = reader["id_evaluation"].ToString();
                                int note = reader.IsDBNull(reader.GetOrdinal("note_evaluation")) ? 0 : Convert.ToInt32(reader["note_evaluation"]);
                                string commentaire = reader.IsDBNull(reader.GetOrdinal("commentaire_evaluation")) ? "" : reader["commentaire_evaluation"].ToString();
                                string date = reader.IsDBNull(reader.GetOrdinal("date_evaluation")) ? "N/A" : reader["date_evaluation"].ToString();

                                string client = "Client anonyme";
                                if (!reader.IsDBNull(reader.GetOrdinal("nom_client")) && !reader.IsDBNull(reader.GetOrdinal("prenom_client")))
                                {
                                    client = reader["prenom_client"].ToString() + " " + reader["nom_client"].ToString();
                                }

                                Console.WriteLine($"Évaluation #{id} - {date}");
                                Console.WriteLine($"Client : {client}");
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

        /// <summary>
        /// Affiche les clients fréquents d'un cuisinier
        /// </summary>
        private static void AfficherClientsFrequentsCuisinier(string idCuisinier)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MES CLIENTS FRÉQUENTS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT cl.id_client, cl.nom_client, cl.prenom_client, cl.email_client,
                               COUNT(DISTINCT c.id_commande) AS nombre_commandes
                        FROM CLIENT cl
                        JOIN COMMANDE c ON cl.id_client = c.id_client
                        JOIN LIGNE_COMMANDE lc ON c.id_commande = lc.id_commande
                        JOIN PLAT p ON lc.id_plat = p.id_plat
                        WHERE p.id_cuisinier = @IdCuisinier
                        GROUP BY cl.id_client, cl.nom_client, cl.prenom_client, cl.email_client
                        ORDER BY nombre_commandes DESC
                        LIMIT 10";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-5} {1,-30} {2,-30} {3,-10}",
                                "ID", "Nom", "Email", "Commandes");
                            Console.WriteLine(new string('-', 75));

                            while (reader.Read())
                            {
                                string id = reader["id_client"].ToString();
                                string nom = "";
                                if (!reader.IsDBNull(reader.GetOrdinal("nom_client")) && !reader.IsDBNull(reader.GetOrdinal("prenom_client")))
                                {
                                    nom = reader["prenom_client"].ToString() + " " + reader["nom_client"].ToString();
                                }
                                string email = reader.IsDBNull(reader.GetOrdinal("email_client")) ? "N/A" : reader["email_client"].ToString();
                                int nombreCommandes = reader.IsDBNull(reader.GetOrdinal("nombre_commandes")) ? 0 : Convert.ToInt32(reader["nombre_commandes"]);

                                Console.WriteLine("{0,-5} {1,-30} {2,-30} {3,-10}",
                                    id, nom, email, nombreCommandes);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Vous n'avez pas encore de clients.");
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
        /// Affiche les statistiques de vente d'un cuisinier
        /// </summary>
        private static void AfficherStatistiquesVenteCuisinier(string idCuisinier)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MES STATISTIQUES DE VENTE");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    /// Statistiques globales
                    string queryGlobal = @"
                        SELECT COUNT(DISTINCT c.id_commande) AS nombre_commandes,
                               COUNT(DISTINCT lc.id_ligne) AS nombre_plats_vendus,
                               SUM(CAST(REPLACE(REPLACE(c.prix_total_commande, ' euros', ''), ',', '.') AS DECIMAL(10,2))) AS chiffre_affaires
                        FROM COMMANDE c
                        JOIN LIGNE_COMMANDE lc ON c.id_commande = lc.id_commande
                        JOIN PLAT p ON lc.id_plat = p.id_plat
                        WHERE p.id_cuisinier = @IdCuisinier";

                    MySqlCommand cmdGlobal = new MySqlCommand(queryGlobal, connection);
                    cmdGlobal.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                    using (MySqlDataReader reader = cmdGlobal.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int nombreCommandes = reader.IsDBNull(reader.GetOrdinal("nombre_commandes")) ? 0 : Convert.ToInt32(reader["nombre_commandes"]);
                            int nombrePlatsVendus = reader.IsDBNull(reader.GetOrdinal("nombre_plats_vendus")) ? 0 : Convert.ToInt32(reader["nombre_plats_vendus"]);
                            decimal chiffreAffaires = reader.IsDBNull(reader.GetOrdinal("chiffre_affaires")) ? 0 : Convert.ToDecimal(reader["chiffre_affaires"]);

                            Console.WriteLine("Statistiques globales :");
                            Console.WriteLine($"Nombre total de commandes : {nombreCommandes}");
                            Console.WriteLine($"Nombre total de plats vendus : {nombrePlatsVendus}");
                            Console.WriteLine($"Chiffre d'affaires total : {chiffreAffaires:F2} euros");
                            Console.WriteLine($"Moyenne par commande : {(nombreCommandes > 0 ? chiffreAffaires / nombreCommandes : 0):F2} euros");
                        }
                    }

                    /// Statistiques par mois
                    Console.WriteLine("\nStatistiques par mois :");
                    string queryMois = @"
                        SELECT DATE_FORMAT(c.date_commande, '%Y-%m') AS mois,
                               COUNT(DISTINCT c.id_commande) AS nombre_commandes,
                               COUNT(DISTINCT lc.id_ligne) AS nombre_plats_vendus,
                               SUM(CAST(REPLACE(REPLACE(c.prix_total_commande, ' euros', ''), ',', '.') AS DECIMAL(10,2))) AS chiffre_affaires
                        FROM COMMANDE c
                        JOIN LIGNE_COMMANDE lc ON c.id_commande = lc.id_commande
                        JOIN PLAT p ON lc.id_plat = p.id_plat
                        WHERE p.id_cuisinier = @IdCuisinier
                        GROUP BY mois
                        ORDER BY mois DESC
                        LIMIT 12";

                    MySqlCommand cmdMois = new MySqlCommand(queryMois, connection);
                    cmdMois.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                    using (MySqlDataReader reader = cmdMois.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15}",
                                "Mois", "Commandes", "Plats vendus", "Chiffre d'affaires (euros)");
                            Console.WriteLine(new string('-', 55));

                            while (reader.Read())
                            {
                                string mois = reader.IsDBNull(reader.GetOrdinal("mois")) ? "N/A" : reader["mois"].ToString();
                                int nombreCommandes = reader.IsDBNull(reader.GetOrdinal("nombre_commandes")) ? 0 : Convert.ToInt32(reader["nombre_commandes"]);
                                int nombrePlatsVendus = reader.IsDBNull(reader.GetOrdinal("nombre_plats_vendus")) ? 0 : Convert.ToInt32(reader["nombre_plats_vendus"]);
                                decimal chiffreAffaires = reader.IsDBNull(reader.GetOrdinal("chiffre_affaires")) ? 0 : Convert.ToDecimal(reader["chiffre_affaires"]);

                                Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15:F2}",
                                    mois, nombreCommandes, nombrePlatsVendus, chiffreAffaires);
                            }

                            /// Ajouter l'option d'exportation XML
                            Console.WriteLine("\nOptions :");
                            Console.WriteLine("1. Exporter ces données au format XML");
                            Console.WriteLine("2. Retour au menu précédent");

                            Console.Write("\nVotre choix : ");
                            string choix = Console.ReadLine();

                            if (choix == "1")
                            {
                                ExporterStatistiquesCuisinier(idCuisinier);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucune donnée disponible par mois.");
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

        private static void ExporterStatistiquesCuisinier(string idCuisinier)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("EXPORTER LES STATISTIQUES CUISINIER");

            try
            {
                StatistiquesCuisinier stats = new StatistiquesCuisinier();
                stats.IdCuisinier = idCuisinier;

                string requeteCuisinier = @"
            SELECT nom_cuisinier, prenom_cuisinier, note_moyenne_cuisinier
            FROM CUISINIER
            WHERE id_cuisinier = @IdCuisinier";

                Dictionary<string, object> parametresCuisinier = new Dictionary<string, object>
                {
                    { "@IdCuisinier", idCuisinier }
                };

                DataTable cuisinierDetails = GestionBDD.ExecuterRequeteTable(requeteCuisinier, parametresCuisinier);

                if (cuisinierDetails != null && cuisinierDetails.Rows.Count > 0)
                {
                    DataRow cuisinier = cuisinierDetails.Rows[0];
                    stats.NomCuisinier = cuisinier["nom_cuisinier"] != DBNull.Value ? cuisinier["nom_cuisinier"].ToString() : "";
                    stats.PrenomCuisinier = cuisinier["prenom_cuisinier"] != DBNull.Value ? cuisinier["prenom_cuisinier"].ToString() : "";
                    stats.NoteMoyenne = cuisinier["note_moyenne_cuisinier"] != DBNull.Value ? Convert.ToDecimal(cuisinier["note_moyenne_cuisinier"]) : 0;
                }


                /// Récupérer les statistiques de vente
                string requeteVentes = @"
    SELECT COUNT(DISTINCT c.id_commande) AS nombre_commandes,
           COUNT(DISTINCT lc.id_ligne) AS nombre_plats_vendus,
           SUM(CAST(REPLACE(REPLACE(c.prix_total_commande, ' euros', ''), ',', '.') AS DECIMAL(10,2))) AS chiffre_affaires
    FROM COMMANDE c
    JOIN LIGNE_COMMANDE lc ON c.id_commande = lc.id_commande
    JOIN PLAT p ON lc.id_plat = p.id_plat
    WHERE p.id_cuisinier = @IdCuisinier";

                DataTable resultatsVentes = GestionBDD.ExecuterRequeteTable(requeteVentes, parametresCuisinier);

                if (resultatsVentes != null && resultatsVentes.Rows.Count > 0)
                {
                    DataRow row = resultatsVentes.Rows[0];
                    stats.NombrePlatsVendus = row["nombre_plats_vendus"] != DBNull.Value ? Convert.ToInt32(row["nombre_plats_vendus"]) : 0;
                    stats.ChiffreAffaires = row["chiffre_affaires"] != DBNull.Value ? Convert.ToDecimal(row["chiffre_affaires"]) : 0;
                }

                /// Récupérer les plats populaires
                string requetePlats = @"
            SELECT p.id_plat, p.nom_plat, p.type_plat, p.prix_unitaire_plat,
                   COUNT(lc.id_ligne) AS nombre_commandes
            FROM PLAT p
            JOIN LIGNE_COMMANDE lc ON p.id_plat = lc.id_plat
            WHERE p.id_cuisinier = @IdCuisinier
            GROUP BY p.id_plat, p.nom_plat, p.type_plat, p.prix_unitaire_plat
            ORDER BY nombre_commandes DESC
            LIMIT 10";

                DataTable plats = GestionBDD.ExecuterRequeteTable(requetePlats, parametresCuisinier);

                if (plats != null && plats.Rows.Count > 0)
                {
                    foreach (DataRow plat in plats.Rows)
                    {
                        PlatPopulaire pp = new PlatPopulaire
                        {
                            IdPlat = plat["id_plat"].ToString(),
                            NomPlat = plat["nom_plat"] != DBNull.Value ? plat["nom_plat"].ToString() : "",
                            TypePlat = plat["type_plat"] != DBNull.Value ? plat["type_plat"].ToString() : "",
                            Prix = plat["prix_unitaire_plat"] != DBNull.Value ? plat["prix_unitaire_plat"].ToString() : "0",
                            NombreCommandes = plat["nombre_commandes"] != DBNull.Value ? Convert.ToInt32(plat["nombre_commandes"]) : 0
                        };

                        stats.PlatsPopulaires.Add(pp);
                    }
                }

                /// Récupérer les clients fréquents
                string requeteClients = @"
            SELECT cl.id_client, cl.nom_client, cl.prenom_client, cl.email_client,
                   COUNT(DISTINCT c.id_commande) AS nombre_commandes
            FROM CLIENT cl
            JOIN COMMANDE c ON cl.id_client = c.id_client
            JOIN LIGNE_COMMANDE lc ON c.id_commande = lc.id_commande
            JOIN PLAT p ON lc.id_plat = p.id_plat
            WHERE p.id_cuisinier = @IdCuisinier
            GROUP BY cl.id_client, cl.nom_client, cl.prenom_client, cl.email_client
            ORDER BY nombre_commandes DESC
            LIMIT 10";

                DataTable clients = GestionBDD.ExecuterRequeteTable(requeteClients, parametresCuisinier);

                if (clients != null && clients.Rows.Count > 0)
                {
                    foreach (DataRow client in clients.Rows)
                    {
                        ClientFrequent cf = new ClientFrequent
                        {
                            IdClient = client["id_client"].ToString(),
                            NomClient = client["nom_client"] != DBNull.Value ? client["nom_client"].ToString() : "",
                            PrenomClient = client["prenom_client"] != DBNull.Value ? client["prenom_client"].ToString() : "",
                            Email = client["email_client"] != DBNull.Value ? client["email_client"].ToString() : "",
                            NombreCommandes = client["nombre_commandes"] != DBNull.Value ? Convert.ToInt32(client["nombre_commandes"]) : 0
                        };

                        stats.ClientsFrequents.Add(cf);
                    }
                }

                /// Exporter les statistiques en XML
                string nomFichier = $"StatistiquesCuisinier_{stats.NomCuisinier}_{stats.PrenomCuisinier}_{DateTime.Now:yyyyMMdd_HHmmss}.xml";
                string cheminFichier = ExportXML.ExporterDonnees(stats, nomFichier);

                if (cheminFichier != null)
                {
                    GestionBDD.AfficherMessageSucces($"Les statistiques ont été exportées avec succès dans le fichier : {cheminFichier}");

                    Console.Write("\nVoulez-vous ouvrir le fichier XML ? (O/N) : ");
                    if (Console.ReadLine().ToUpper() == "O")
                    {
                        ExportXML.OuvrirFichierXML(cheminFichier);
                    }
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Erreur lors de l'exportation des statistiques.");
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

        #region Statistiques pour administrateurs

        /// <summary>
        /// Affiche le nombre de livraisons par cuisinier
        /// </summary>
        private static void AfficherLivraisonsParCuisinier()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("NOMBRE DE LIVRAISONS PAR CUISINIER");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete = @"
                        SELECT c.id_cuisinier, c.nom_cuisinier, c.prenom_cuisinier, 
                               COUNT(DISTINCT lc.id_commande) AS nombre_livraisons
                        FROM CUISINIER c
                        JOIN PLAT p ON c.id_cuisinier = p.id_cuisinier
                        JOIN LIGNE_COMMANDE lc ON p.id_plat = lc.id_plat
                        JOIN COMMANDE cmd ON lc.id_commande = cmd.id_commande
                        WHERE cmd.date_livraison IS NOT NULL
                        GROUP BY c.id_cuisinier, c.nom_cuisinier, c.prenom_cuisinier
                        ORDER BY nombre_livraisons DESC";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-5} {1,-30} {2,-20}",
                                "ID", "Nom", "Nombre de livraisons");
                            Console.WriteLine(new string('-', 55));

                            while (reader.Read())
                            {
                                string id = reader["id_cuisinier"].ToString();
                                string nom = "";
                                if (!reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) && !reader.IsDBNull(reader.GetOrdinal("prenom_cuisinier")))
                                {
                                    nom = reader["prenom_cuisinier"].ToString() + " " + reader["nom_cuisinier"].ToString();
                                }
                                int nombreLivraisons = reader.IsDBNull(reader.GetOrdinal("nombre_livraisons")) ? 0 : Convert.ToInt32(reader["nombre_livraisons"]);

                                Console.WriteLine("{0,-5} {1,-30} {2,-20}",
                                    id, nom, nombreLivraisons);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucune livraison trouvée.");
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
        /// Affiche les commandes selon une période de temps spécifiée par l'utilisateur
        /// </summary>
        private static void AfficherCommandesParPeriode()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("COMMANDES PAR PÉRIODE");

            Console.WriteLine("Veuillez spécifier la période :");
            Console.Write("Date de début (YYYY-MM-DD) : ");
            string dateDebut = Console.ReadLine();
            Console.Write("Date de fin (YYYY-MM-DD) : ");
            string dateFin = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete = @"
                        SELECT c.id_commande, c.date_commande, c.prix_total_commande, c.statut_commande,
                               cl.nom_client, cl.prenom_client
                        FROM COMMANDE c
                        JOIN CLIENT cl ON c.id_client = cl.id_client
                        WHERE c.date_commande BETWEEN @DateDebut AND @DateFin
                        ORDER BY c.date_commande DESC";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@DateDebut", dateDebut);
                    cmd.Parameters.AddWithValue("@DateFin", dateFin);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-15} {4,-30}",
                                "ID", "Date", "Prix", "Statut", "Client");
                            Console.WriteLine(new string('-', 85));

                            while (reader.Read())
                            {
                                string id = reader["id_commande"].ToString();
                                string date = reader.IsDBNull(reader.GetOrdinal("date_commande")) ? "N/A" : reader["date_commande"].ToString();
                                string prix = reader.IsDBNull(reader.GetOrdinal("prix_total_commande")) ? "N/A" : reader["prix_total_commande"].ToString();
                                string statut = reader.IsDBNull(reader.GetOrdinal("statut_commande")) ? "N/A" : reader["statut_commande"].ToString();

                                string client = "N/A";
                                if (!reader.IsDBNull(reader.GetOrdinal("nom_client")) && !reader.IsDBNull(reader.GetOrdinal("prenom_client")))
                                {
                                    client = reader["prenom_client"].ToString() + " " + reader["nom_client"].ToString();
                                }

                                Console.WriteLine("{0,-5} {1,-20} {2,-15} {3,-15} {4,-30}",
                                    id, date, prix, statut, client);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucune commande trouvée pour cette période.");
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
        /// Affiche la moyenne des prix des commandes
        /// </summary>
        private static void AfficherMoyennePrixCommandes()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MOYENNE DES PRIX DES COMMANDES");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete = @"
                        SELECT AVG(CAST(REPLACE(REPLACE(prix_total_commande, ' euros', ''), ',', '.') AS DECIMAL(10,2))) AS moyenne_prix
                        FROM COMMANDE";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);

                    object result = cmd.ExecuteScalar();
                    if (result != null && !Convert.IsDBNull(result))
                    {
                        decimal moyennePrix = Convert.ToDecimal(result);
                        Console.WriteLine($"La moyenne des prix des commandes est de : {moyennePrix:F2} euros");
                    }
                    else
                    {
                        Console.WriteLine("Aucune commande trouvée pour calculer la moyenne.");
                    }

                    /// Moyenne par mois
                    Console.WriteLine("\nMoyenne des prix par mois :");
                    string requeteMois = @"
                        SELECT DATE_FORMAT(date_commande, '%Y-%m') AS mois,
                               COUNT(*) AS nombre_commandes,
                               AVG(CAST(REPLACE(REPLACE(prix_total_commande, ' euros', ''), ',', '.') AS DECIMAL(10,2))) AS moyenne_prix
                        FROM COMMANDE
                        GROUP BY mois
                        ORDER BY mois DESC
                        LIMIT 12";

                    MySqlCommand cmdMois = new MySqlCommand(requeteMois, connection);

                    using (MySqlDataReader reader = cmdMois.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-10} {1,-15} {2,-15}",
                                "Mois", "Nb commandes", "Moyenne (euros)");
                            Console.WriteLine(new string('-', 40));

                            while (reader.Read())
                            {
                                string mois = reader.IsDBNull(reader.GetOrdinal("mois")) ? "N/A" : reader["mois"].ToString();
                                int nombreCommandes = reader.IsDBNull(reader.GetOrdinal("nombre_commandes")) ? 0 : Convert.ToInt32(reader["nombre_commandes"]);
                                decimal moyennePrix = reader.IsDBNull(reader.GetOrdinal("moyenne_prix")) ? 0 : Convert.ToDecimal(reader["moyenne_prix"]);

                                Console.WriteLine("{0,-10} {1,-15} {2,-15:F2}",
                                    mois, nombreCommandes, moyennePrix);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucune donnée disponible par mois.");
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
        /// Affiche la moyenne des comptes clients (montant moyen dépensé par client)
        /// </summary>
        private static void AfficherMoyenneComptesClients()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MOYENNE DES COMPTES CLIENTS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete = @"
                        SELECT cl.id_client, cl.nom_client, cl.prenom_client,
                               SUM(CAST(REPLACE(REPLACE(c.prix_total_commande, ' euros', ''), ',', '.') AS DECIMAL(10,2))) AS total_depense,
                               SUM(CAST(REPLACE(REPLACE(prix_total_commande, ' euros', ''), ',', '.') AS DECIMAL(10,2))) AS total_depense,
                               COUNT(c.id_commande) AS nombre_commandes
                        FROM CLIENT cl
                        JOIN COMMANDE c ON cl.id_client = c.id_client
                        GROUP BY cl.id_client, cl.nom_client, cl.prenom_client";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            decimal totalDepenses = 0;
                            int nombreClients = 0;

                            Console.WriteLine("{0,-5} {1,-30} {2,-15} {3,-15}",
                                "ID", "Nom", "Total dépensé (euros)", "Nb commandes");
                            Console.WriteLine(new string('-', 65));

                            while (reader.Read())
                            {
                                string id = reader["id_client"].ToString();
                                string nom = "";
                                if (!reader.IsDBNull(reader.GetOrdinal("nom_client")) && !reader.IsDBNull(reader.GetOrdinal("prenom_client")))
                                {
                                    nom = reader["prenom_client"].ToString() + " " + reader["nom_client"].ToString();
                                }
                                decimal totalDepense = reader.IsDBNull(reader.GetOrdinal("total_depense")) ? 0 : Convert.ToDecimal(reader["total_depense"]);
                                int nombreCommandes = reader.IsDBNull(reader.GetOrdinal("nombre_commandes")) ? 0 : Convert.ToInt32(reader["nombre_commandes"]);

                                totalDepenses += totalDepense;
                                nombreClients++;

                                Console.WriteLine("{0,-5} {1,-30} {2,-15:F2} {3,-15}",
                                    id, nom, totalDepense, nombreCommandes);
                            }

                            if (nombreClients > 0)
                            {
                                decimal moyenneDepenses = totalDepenses / nombreClients;
                                Console.WriteLine("\n------------------------------------------------------------------");
                                Console.WriteLine($"Moyenne des dépenses par client : {moyenneDepenses:F2} euros");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun client avec des commandes trouvé.");
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
        /// Affiche les commandes d'un client selon la nationalité des plats et la période
        /// </summary>
        private static void AfficherCommandesClientParNationaliteEtPeriode()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("COMMANDES PAR NATIONALITÉ ET PÉRIODE");

            Console.Write("ID du client : ");
            string idClient = Console.ReadLine();

            Console.Write("Nationalité du plat (laisser vide pour toutes) : ");
            string nationalite = Console.ReadLine();

            Console.WriteLine("Période :");
            Console.Write("Date de début (YYYY-MM-DD) : ");
            string dateDebut = Console.ReadLine();
            Console.Write("Date de fin (YYYY-MM-DD) : ");
            string dateFin = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete = @"
                        SELECT c.id_commande, c.date_commande, c.prix_total_commande, c.statut_commande,
                               p.nom_plat, p.nationalite_plat, p.type_plat
                        FROM COMMANDE c
                        JOIN CLIENT cl ON c.id_client = cl.id_client
                        JOIN LIGNE_COMMANDE lc ON c.id_commande = lc.id_commande
                        JOIN PLAT p ON lc.id_plat = p.id_plat
                        WHERE cl.id_client = @IdClient
                        AND c.date_commande BETWEEN @DateDebut AND @DateFin";

                    if (!string.IsNullOrWhiteSpace(nationalite))
                    {
                        requete += " AND p.nationalite_plat = @Nationalite";
                    }

                    requete += " ORDER BY c.date_commande DESC";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.Parameters.AddWithValue("@IdClient", idClient);
                    cmd.Parameters.AddWithValue("@DateDebut", dateDebut);
                    cmd.Parameters.AddWithValue("@DateFin", dateFin);

                    if (!string.IsNullOrWhiteSpace(nationalite))
                    {
                        cmd.Parameters.AddWithValue("@Nationalite", nationalite);
                    }

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-5} {1,-20} {2,-25} {3,-15} {4,-15} {5,-15}",
                                "ID", "Date", "Plat", "Type", "Nationalité", "Prix");
                            Console.WriteLine(new string('-', 95));

                            while (reader.Read())
                            {
                                string id = reader["id_commande"].ToString();
                                string date = reader.IsDBNull(reader.GetOrdinal("date_commande")) ? "N/A" : reader["date_commande"].ToString();
                                string nomPlat = reader.IsDBNull(reader.GetOrdinal("nom_plat")) ? "N/A" : reader["nom_plat"].ToString();
                                string typePlat = reader.IsDBNull(reader.GetOrdinal("type_plat")) ? "N/A" : reader["type_plat"].ToString();
                                string nationalitePlat = reader.IsDBNull(reader.GetOrdinal("nationalite_plat")) ? "N/A" : reader["nationalite_plat"].ToString();
                                string prix = reader.IsDBNull(reader.GetOrdinal("prix_total_commande")) ? "N/A" : reader["prix_total_commande"].ToString();

                                Console.WriteLine("{0,-5} {1,-20} {2,-25} {3,-15} {4,-15} {5,-15}",
                                    id, date, nomPlat, typePlat, nationalitePlat, prix);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucune commande trouvée pour ce client avec ces critères.");
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
        /// Affiche les meilleurs cuisiniers par période (mois, année)
        /// </summary>
        private static void AfficherMeilleursCuisiniersParPeriode()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MEILLEURS CUISINIERS PAR PÉRIODE");

            Console.WriteLine("Choisissez la période :");
            Console.WriteLine("1. Par mois");
            Console.WriteLine("2. Par année");
            Console.Write("Votre choix : ");
            string choixPeriode = Console.ReadLine();

            string formatDate = "";
            string labelPeriode = "";

            switch (choixPeriode)
            {
                case "1":
                    formatDate = "%Y-%m";
                    labelPeriode = "Mois";
                    break;
                case "2":
                    formatDate = "%Y";
                    labelPeriode = "Année";
                    break;
                default:
                    GestionBDD.AfficherMessageErreur("Option invalide.");
                    return;
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete = $@"
                        SELECT DATE_FORMAT(c.date_commande, '{formatDate}') AS periode,
                               cu.id_cuisinier, cu.nom_cuisinier, cu.prenom_cuisinier,
                               COUNT(DISTINCT c.id_commande) AS nombre_commandes,
                               SUM(CAST(REPLACE(REPLACE(c.prix_total_commande, ' euros', ''), ',', '.') AS DECIMAL(10,2))) AS chiffre_affaires,
                               AVG(cu.note_moyenne_cuisinier) AS note_moyenne
                        FROM COMMANDE c
                        JOIN LIGNE_COMMANDE lc ON c.id_commande = lc.id_commande
                        JOIN PLAT p ON lc.id_plat = p.id_plat
                        JOIN CUISINIER cu ON p.id_cuisinier = cu.id_cuisinier
                        GROUP BY periode, cu.id_cuisinier, cu.nom_cuisinier, cu.prenom_cuisinier
                        ORDER BY periode DESC, chiffre_affaires DESC";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            string periodeActuelle = "";

                            while (reader.Read())
                            {
                                string periode = reader.IsDBNull(reader.GetOrdinal("periode")) ? "N/A" : reader["periode"].ToString();

                                if (periode != periodeActuelle)
                                {
                                    if (!string.IsNullOrEmpty(periodeActuelle))
                                    {
                                        Console.WriteLine("\n------------------------------------------------------------------");
                                    }

                                    periodeActuelle = periode;
                                    Console.WriteLine($"\n{labelPeriode} : {periode}");
                                    Console.WriteLine("------------------------------------------------------------------");
                                    Console.WriteLine("{0,-5} {1,-30} {2,-20} {3,-20} {4,-15}",
                                        "ID", "Nom", "Nb commandes", "Chiffre d'affaires (euros)", "Note moyenne");
                                    Console.WriteLine("------------------------------------------------------------------");
                                }

                                string id = reader["id_cuisinier"].ToString();
                                string nom = "";
                                if (!reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) && !reader.IsDBNull(reader.GetOrdinal("prenom_cuisinier")))
                                {
                                    nom = reader["prenom_cuisinier"].ToString() + " " + reader["nom_cuisinier"].ToString();
                                }
                                int nombreCommandes = reader.IsDBNull(reader.GetOrdinal("nombre_commandes")) ? 0 : Convert.ToInt32(reader["nombre_commandes"]);
                                decimal chiffreAffaires = reader.IsDBNull(reader.GetOrdinal("chiffre_affaires")) ? 0 : Convert.ToDecimal(reader["chiffre_affaires"]);
                                decimal noteMoyenne = reader.IsDBNull(reader.GetOrdinal("note_moyenne")) ? 0 : Convert.ToDecimal(reader["note_moyenne"]);

                                Console.WriteLine("{0,-5} {1,-30} {2,-20} {3,-20:F2} {4,-15:F1}",
                                    id, nom, nombreCommandes, chiffreAffaires, noteMoyenne);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucune donnée trouvée pour cette période.");
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
        /// Affiche les statistiques globales du système
        /// </summary>
        private static void AfficherStatistiquesGlobales()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("STATISTIQUES GLOBALES");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string queryGeneral = @"
                        SELECT 
                            (SELECT COUNT(*) FROM CLIENT) AS nombre_clients,
                            (SELECT COUNT(*) FROM CUISINIER) AS nombre_cuisiniers,
                            (SELECT COUNT(*) FROM PLAT) AS nombre_plats,
                            (SELECT COUNT(*) FROM COMMANDE) AS nombre_commandes,
                            (SELECT SUM(CAST(REPLACE(REPLACE(prix_total_commande, ' euros', ''), ',', '.') AS DECIMAL(10,2))) FROM COMMANDE) AS chiffre_affaires_total";

                    MySqlCommand cmdGeneral = new MySqlCommand(queryGeneral, connection);

                    using (MySqlDataReader reader = cmdGeneral.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int nombreClients = reader.IsDBNull(reader.GetOrdinal("nombre_clients")) ? 0 : Convert.ToInt32(reader["nombre_clients"]);
                            int nombreCuisiniers = reader.IsDBNull(reader.GetOrdinal("nombre_cuisiniers")) ? 0 : Convert.ToInt32(reader["nombre_cuisiniers"]);
                            int nombrePlats = reader.IsDBNull(reader.GetOrdinal("nombre_plats")) ? 0 : Convert.ToInt32(reader["nombre_plats"]);
                            int nombreCommandes = reader.IsDBNull(reader.GetOrdinal("nombre_commandes")) ? 0 : Convert.ToInt32(reader["nombre_commandes"]);
                            decimal chiffreAffairesTotal = reader.IsDBNull(reader.GetOrdinal("chiffre_affaires_total")) ? 0 : Convert.ToDecimal(reader["chiffre_affaires_total"]);

                            Console.WriteLine("Statistiques générales :");
                            Console.WriteLine($"Nombre de clients : {nombreClients}");
                            Console.WriteLine($"Nombre de cuisiniers : {nombreCuisiniers}");
                            Console.WriteLine($"Nombre de plats : {nombrePlats}");
                            Console.WriteLine($"Nombre de commandes : {nombreCommandes}");
                            Console.WriteLine($"Chiffre d'affaires total : {chiffreAffairesTotal:F2} euros");
                            Console.WriteLine($"Moyenne par commande : {(nombreCommandes > 0 ? chiffreAffairesTotal / nombreCommandes : 0):F2} euros");
                        }
                    }

                    /// Statistiques par mois
                    Console.WriteLine("\nStatistiques par mois :");
                    string queryMois = @"
                        SELECT DATE_FORMAT(date_commande, '%Y-%m') AS mois,
                               COUNT(*) AS nombre_commandes,
                               SUM(CAST(REPLACE(REPLACE(prix_total_commande, ' euros', ''), ',', '.') AS DECIMAL(10,2))) AS chiffre_affaires
                        FROM COMMANDE
                        GROUP BY mois
                        ORDER BY mois DESC
                        LIMIT 12";

                    MySqlCommand cmdMois = new MySqlCommand(queryMois, connection);

                    using (MySqlDataReader reader = cmdMois.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-10} {1,-15} {2,-20}",
                                "Mois", "Nb commandes", "Chiffre d'affaires (euros)");
                            Console.WriteLine(new string('-', 45));

                            while (reader.Read())
                            {
                                string mois = reader.IsDBNull(reader.GetOrdinal("mois")) ? "N/A" : reader["mois"].ToString();
                                int nombreCommandes = reader.IsDBNull(reader.GetOrdinal("nombre_commandes")) ? 0 : Convert.ToInt32(reader["nombre_commandes"]);
                                decimal chiffreAffaires = reader.IsDBNull(reader.GetOrdinal("chiffre_affaires")) ? 0 : Convert.ToDecimal(reader["chiffre_affaires"]);

                                Console.WriteLine("{0,-10} {1,-15} {2,-20:F2}",
                                    mois, nombreCommandes, chiffreAffaires);
                            }

                            Console.WriteLine("\nOptions :");
                            Console.WriteLine("1. Exporter ces données au format XML");
                            Console.WriteLine("2. Retour au menu précédent");

                            Console.Write("\nVotre choix : ");
                            string choix = Console.ReadLine();

                            if (choix == "1")
                            {
                                ExporterStatistiquesGlobales();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucune donnée disponible par mois.");
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

        private static void ExporterStatistiquesGlobales()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("EXPORTER LES STATISTIQUES GLOBALES");

            try
            {
                StatistiquesGlobales stats = new StatistiquesGlobales();

                /// Récupérer les statistiques générales
                string requeteGeneral = @"
            SELECT 
                (SELECT COUNT(*) FROM CLIENT) AS nombre_clients,
                (SELECT COUNT(*) FROM CUISINIER) AS nombre_cuisiniers,
                (SELECT COUNT(*) FROM PLAT) AS nombre_plats,
                (SELECT COUNT(*) FROM COMMANDE) AS nombre_commandes,
                (SELECT SUM(CAST(REPLACE(REPLACE(prix_total_commande, ' euros', ''), ',', '.') AS DECIMAL(10,2))) FROM COMMANDE) AS chiffre_affaires_total";

                DataTable general = GestionBDD.ExecuterRequeteTable(requeteGeneral, null);

                if (general != null && general.Rows.Count > 0)
                {
                    DataRow row = general.Rows[0];
                    stats.NombreClients = row["nombre_clients"] != DBNull.Value ? Convert.ToInt32(row["nombre_clients"]) : 0;
                    stats.NombreCuisiniers = row["nombre_cuisiniers"] != DBNull.Value ? Convert.ToInt32(row["nombre_cuisiniers"]) : 0;
                    stats.NombrePlats = row["nombre_plats"] != DBNull.Value ? Convert.ToInt32(row["nombre_plats"]) : 0;
                    stats.NombreCommandes = row["nombre_commandes"] != DBNull.Value ? Convert.ToInt32(row["nombre_commandes"]) : 0;
                    stats.ChiffreAffairesTotal = row["chiffre_affaires_total"] != DBNull.Value ? Convert.ToDecimal(row["chiffre_affaires_total"]) : 0;
                }

                /// Récupérer les statistiques par mois
                string requeteMois = @"
            SELECT DATE_FORMAT(date_commande, '%Y-%m') AS mois,
                   COUNT(*) AS nombre_commandes,
                   SUM(CAST(REPLACE(REPLACE(prix_total_commande, ' euros', ''), ',', '.') AS DECIMAL(10,2))) AS chiffre_affaires
            FROM COMMANDE
            GROUP BY mois
            ORDER BY mois DESC
            LIMIT 12";

                DataTable mois = GestionBDD.ExecuterRequeteTable(requeteMois, null);

                if (mois != null && mois.Rows.Count > 0)
                {
                    foreach (DataRow row in mois.Rows)
                    {
                        StatistiquesMensuelles sm = new StatistiquesMensuelles
                        {
                            Mois = row["mois"] != DBNull.Value ? row["mois"].ToString() : "",
                            NombreCommandes = row["nombre_commandes"] != DBNull.Value ? Convert.ToInt32(row["nombre_commandes"]) : 0,
                            ChiffreAffaires = row["chiffre_affaires"] != DBNull.Value ? Convert.ToDecimal(row["chiffre_affaires"]) : 0
                        };

                        stats.StatistiquesMensuelles.Add(sm);
                    }
                }

                /// Récupérer les meilleurs cuisiniers
                string requeteCuisiniers = @"
    SELECT cu.id_cuisinier, cu.nom_cuisinier, cu.prenom_cuisinier, cu.note_moyenne_cuisinier,
           COUNT(DISTINCT c.id_commande) AS nombre_commandes,
           SUM(CAST(REPLACE(REPLACE(c.prix_total_commande, ' euros', ''), ',', '.') AS DECIMAL(10,2))) AS chiffre_affaires
    FROM CUISINIER cu
    JOIN PLAT p ON cu.id_cuisinier = p.id_cuisinier
    JOIN LIGNE_COMMANDE lc ON p.id_plat = lc.id_plat
    JOIN COMMANDE c ON lc.id_commande = c.id_commande
    GROUP BY cu.id_cuisinier, cu.nom_cuisinier, cu.prenom_cuisinier, cu.note_moyenne_cuisinier
    ORDER BY chiffre_affaires DESC
    LIMIT 10";

                DataTable cuisiniers = GestionBDD.ExecuterRequeteTable(requeteCuisiniers, null);

                if (cuisiniers != null && cuisiniers.Rows.Count > 0)
                {
                    foreach (DataRow row in cuisiniers.Rows)
                    {
                        MeilleurCuisinier mc = new MeilleurCuisinier
                        {
                            IdCuisinier = row["id_cuisinier"].ToString(),
                            NomCuisinier = row["nom_cuisinier"] != DBNull.Value ? row["nom_cuisinier"].ToString() : "",
                            PrenomCuisinier = row["prenom_cuisinier"] != DBNull.Value ? row["prenom_cuisinier"].ToString() : "",
                            NoteMoyenne = row["note_moyenne_cuisinier"] != DBNull.Value ? Convert.ToDecimal(row["note_moyenne_cuisinier"]) : 0,
                            NombreCommandes = row["nombre_commandes"] != DBNull.Value ? Convert.ToInt32(row["nombre_commandes"]) : 0,
                            ChiffreAffaires = row["chiffre_affaires"] != DBNull.Value ? Convert.ToDecimal(row["chiffre_affaires"]) : 0
                        };

                        stats.MeilleursCuisiniers.Add(mc);
                    }
                }

                /// Ajouter une requête pour récupérer les plats les plus populaires
                string requetePlatsPopulaires = @"
    SELECT p.id_plat, p.nom_plat, p.type_plat, p.nationalite_plat, p.prix_unitaire_plat,
           COUNT(lc.id_ligne) AS nombre_commandes,
           c.nom_cuisinier, c.prenom_cuisinier
    FROM PLAT p
    JOIN LIGNE_COMMANDE lc ON p.id_plat = lc.id_plat
    JOIN CUISINIER c ON p.id_cuisinier = c.id_cuisinier
    GROUP BY p.id_plat, p.nom_plat, p.type_plat, p.nationalite_plat, p.prix_unitaire_plat,
             c.nom_cuisinier, c.prenom_cuisinier
    ORDER BY nombre_commandes DESC
    LIMIT 10";

                DataTable platsPopulaires = GestionBDD.ExecuterRequeteTable(requetePlatsPopulaires, null);

                /// Exporter les statistiques en XML
                string nomFichier = $"StatistiquesGlobales_{DateTime.Now:yyyyMMdd_HHmmss}.xml";
                string cheminFichier = ExportXML.ExporterDonnees(stats, nomFichier);

                if (cheminFichier != null)
                {
                    GestionBDD.AfficherMessageSucces($"Les statistiques ont été exportées avec succès dans le fichier : {cheminFichier}");

                    Console.Write("\nVoulez-vous ouvrir le fichier XML ? (O/N) : ");
                    if (Console.ReadLine().ToUpper() == "O")
                    {
                        ExportXML.OuvrirFichierXML(cheminFichier);
                    }
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Erreur lors de l'exportation des statistiques.");
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

        #region Statistiques publiques

        /// <summary>
        /// Affiche les statistiques générales pour les utilisateurs non connectés
        /// </summary>
        private static void AfficherStatistiquesGenerales()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("STATISTIQUES GÉNÉRALES");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    /// Nombre de plats disponibles par type
                    string queryPlats = @"
                        SELECT type_plat, COUNT(*) AS nombre_plats
                        FROM PLAT
                        WHERE portion_dispo_plat > 0
                        GROUP BY type_plat
                        ORDER BY nombre_plats DESC";

                    MySqlCommand cmdPlats = new MySqlCommand(queryPlats, connection);

                    Console.WriteLine("Plats disponibles par type :");
                    using (MySqlDataReader reader = cmdPlats.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-20} {1,-10}",
                                "Type", "Nombre");
                            Console.WriteLine(new string('-', 30));

                            while (reader.Read())
                            {
                                string type = reader.IsDBNull(reader.GetOrdinal("type_plat")) ? "N/A" : reader["type_plat"].ToString();
                                int nombrePlats = reader.IsDBNull(reader.GetOrdinal("nombre_plats")) ? 0 : Convert.ToInt32(reader["nombre_plats"]);

                                Console.WriteLine("{0,-20} {1,-10}",
                                    type, nombrePlats);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun plat disponible.");
                        }
                    }

                    /// Nombre de plats disponibles par nationalité
                    Console.WriteLine("\nPlats disponibles par nationalité :");
                    string queryNationalites = @"
                        SELECT nationalite_plat, COUNT(*) AS nombre_plats
                        FROM PLAT
                        WHERE portion_dispo_plat > 0
                        GROUP BY nationalite_plat
                        ORDER BY nombre_plats DESC";

                    MySqlCommand cmdNationalites = new MySqlCommand(queryNationalites, connection);

                    using (MySqlDataReader reader = cmdNationalites.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-20} {1,-10}",
                                "Nationalité", "Nombre");
                            Console.WriteLine(new string('-', 30));

                            while (reader.Read())
                            {
                                string nationalite = reader.IsDBNull(reader.GetOrdinal("nationalite_plat")) ? "N/A" : reader["nationalite_plat"].ToString();
                                int nombrePlats = reader.IsDBNull(reader.GetOrdinal("nombre_plats")) ? 0 : Convert.ToInt32(reader["nombre_plats"]);

                                Console.WriteLine("{0,-20} {1,-10}",
                                    nationalite, nombrePlats);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun plat disponible.");
                        }
                    }

                    /// Nombre de cuisiniers par spécialité
                    Console.WriteLine("\nCuisiniers par spécialité :");
                    string queryCuisiniers = @"
                        SELECT specialite_cuisinier AS specialite, COUNT(*) AS nombre_cuisiniers
                        FROM CUISINIER
                        GROUP BY specialite_cuisinier
                        ORDER BY nombre_cuisiniers DESC";

                    MySqlCommand cmdCuisiniers = new MySqlCommand(queryCuisiniers, connection);

                    using (MySqlDataReader reader = cmdCuisiniers.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-20} {1,-10}",
                                "Spécialité", "Nombre");
                            Console.WriteLine(new string('-', 30));

                            while (reader.Read())
                            {
                                string specialite = reader.IsDBNull(reader.GetOrdinal("specialite")) ? "N/A" : reader["specialite"].ToString();
                                int nombreCuisiniers = reader.IsDBNull(reader.GetOrdinal("nombre_cuisiniers")) ? 0 : Convert.ToInt32(reader["nombre_cuisiniers"]);

                                Console.WriteLine("{0,-20} {1,-10}",
                                    specialite, nombreCuisiniers);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun cuisinier disponible.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            /// Ajouter l'option d'exportation XML
            Console.WriteLine("\nOptions :");
            Console.WriteLine("1. Exporter ces données au format XML");
            Console.WriteLine("2. Retour au menu précédent");

            Console.Write("\nVotre choix : ");
            string choix = Console.ReadLine();

            if (choix == "1")
            {
                ExporterStatistiquesGlobales();
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        /// <summary>
        /// Affiche les meilleurs cuisiniers pour les utilisateurs non connectés
        /// </summary>
        private static void AfficherMeilleursCuisiniers()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MEILLEURS CUISINIERS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT c.id_cuisinier, c.nom_cuisinier, c.prenom_cuisinier, c.dispo_cuisinier,
                               c.note_moyenne_cuisinier, COUNT(p.id_plat) AS nombre_plats
                        FROM CUISINIER c
                        LEFT JOIN PLAT p ON c.id_cuisinier = p.id_cuisinier
                        GROUP BY c.id_cuisinier, c.nom_cuisinier, c.prenom_cuisinier, c.dispo_cuisinier, c.note_moyenne_cuisinier
                        ORDER BY c.note_moyenne_cuisinier DESC
                        LIMIT 10";

                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-5} {1,-30} {2,-20} {3,-10} {4,-10}",
                                "ID", "Nom", "Spécialité", "Note", "Nb plats");
                            Console.WriteLine(new string('-', 75));

                            while (reader.Read())
                            {
                                string id = reader["id_cuisinier"].ToString();
                                string nom = "";
                                if (!reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) && !reader.IsDBNull(reader.GetOrdinal("prenom_cuisinier")))
                                {
                                    nom = reader["prenom_cuisinier"].ToString() + " " + reader["nom_cuisinier"].ToString();
                                }
                                string specialite = reader.IsDBNull(reader.GetOrdinal("dispo_cuisinier")) ? "N/A" : reader["dispo_cuisinier"].ToString();
                                string note = reader.IsDBNull(reader.GetOrdinal("note_moyenne_cuisinier")) ? "N/A" : reader["note_moyenne_cuisinier"].ToString();
                                int nombrePlats = reader.IsDBNull(reader.GetOrdinal("nombre_plats")) ? 0 : Convert.ToInt32(reader["nombre_plats"]);

                                Console.WriteLine("{0,-5} {1,-30} {2,-20} {3,-10} {4,-10}",
                                    id, nom, specialite, note, nombrePlats);
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
        /// Affiche les plats les plus populaires pour les utilisateurs non connectés
        /// </summary>
        private static void AfficherPlatsPopulaires()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("PLATS LES PLUS POPULAIRES");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT p.id_plat, p.nom_plat, p.type_plat, p.nationalite_plat, p.prix_unitaire_plat,
                               COUNT(lc.id_ligne) AS nombre_commandes,
                               c.nom_cuisinier, c.prenom_cuisinier
                        FROM PLAT p
                        JOIN LIGNE_COMMANDE lc ON p.id_plat = lc.id_plat
                        JOIN CUISINIER c ON p.id_cuisinier = c.id_cuisinier
                        GROUP BY p.id_plat, p.nom_plat, p.type_plat, p.nationalite_plat, p.prix_unitaire_plat,
                                 c.nom_cuisinier, c.prenom_cuisinier
                        ORDER BY nombre_commandes DESC
                        LIMIT 10";

                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-15} {4,-10} {5,-10} {6,-25}",
                                "ID", "Nom", "Type", "Nationalité", "Prix", "Commandes", "Cuisinier");
                            Console.WriteLine(new string('-', 105));

                            while (reader.Read())
                            {
                                string id = reader["id_plat"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom_plat")) ? "N/A" : reader["nom_plat"].ToString();
                                string type = reader.IsDBNull(reader.GetOrdinal("type_plat")) ? "N/A" : reader["type_plat"].ToString();
                                string nationalite = reader.IsDBNull(reader.GetOrdinal("nationalite_plat")) ? "N/A" : reader["nationalite_plat"].ToString();
                                string prix = reader.IsDBNull(reader.GetOrdinal("prix_unitaire_plat")) ? "N/A" : reader["prix_unitaire_plat"].ToString();
                                int nombreCommandes = reader.IsDBNull(reader.GetOrdinal("nombre_commandes")) ? 0 : Convert.ToInt32(reader["nombre_commandes"]);

                                string cuisinier = "N/A";
                                if (!reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) && !reader.IsDBNull(reader.GetOrdinal("prenom_cuisinier")))
                                {
                                    cuisinier = reader["prenom_cuisinier"].ToString() + " " + reader["nom_cuisinier"].ToString();
                                }

                                Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-15} {4,-10} {5,-10} {6,-25}",
                                    id, nom, type, nationalite, prix, nombreCommandes, cuisinier);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun plat commandé trouvé.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nOptions :");
            Console.WriteLine("1. Exporter la liste des plats au format XML");
            Console.WriteLine("2. Retour au menu précédent");

            Console.Write("\nVotre choix : ");
            string choix = Console.ReadLine();

            if (choix == "1")
            {
                ModulePlat.ExporterListePlats();
            }
            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        #endregion

    }
}
