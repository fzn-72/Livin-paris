using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_problème_scientifique_info
{
    public class ModuleStats
    {

        private static string utilisateurConnecteId = "";
        private static string utilisateurConnecteNom = "";
        private static string utilisateurConnectePrenom = "";
        private static string utilisateurConnecteType = "";

        private static string connectionString = "Server=localhost;Database=livinparis;Uid=root;Pwd=REDACTED;";

        #region Menu principal de la classe ModuleStats

        /// <summary>
        /// Gère le module statistiques : propose différentes analyses (livraisons, commandes, moyennes, etc.)
        /// et exécute l'affichage correspondant selon le choix de l'utilisateur, jusqu’à retour au menu principal.
        /// </summary>
        public static void MenuPrincipalStats()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("MODULE STATISTIQUES");
                Console.WriteLine("Choisissez une option :");
                Console.WriteLine("1. Afficher le nombre de livraisons par cuisinier");
                Console.WriteLine("2. Afficher les commandes par période");
                Console.WriteLine("3. Afficher la moyenne des prix des commandes");
                Console.WriteLine("4. Afficher la moyenne des comptes clients");
                Console.WriteLine("5. Afficher les commandes par client selon la nationalité des plats et la période");
                Console.WriteLine("6. Retour au menu principal");

                Console.Write("\nVotre choix : ");
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
                        AfficherCommandesParNationalite();
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

        static void AfficherLivraisonsParCuisinier()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("NOMBRE DE LIVRAISONS PAR CUISINIER");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT c.id_cuisinier, c.nom_cuisinier, c.prénom_cuisinier, 
                       COUNT(p.ID_commande) AS nombre_livraisons,
                       r.nom_resto
                FROM CUISINIER c
                LEFT JOIN PRÉPARE p ON c.id_cuisinier = p.id_cuisinier
                LEFT JOIN COMMANDE cmd ON p.ID_commande = cmd.ID_commande
                LEFT JOIN RÉCEPTIONNE re ON cmd.ID_commande = re.ID_commande
                LEFT JOIN RESTAURANT r ON re.id_resto = r.id_resto
                WHERE cmd.date_livraison IS NOT NULL
                GROUP BY c.id_cuisinier, c.nom_cuisinier, c.prénom_cuisinier, r.nom_resto
                ORDER BY nombre_livraisons DESC";

                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("ID\tNom\t\tPrénom\t\tRestaurant\t\tNombre de livraisons");
                            Console.WriteLine("-------------------------------------------------------------------------");

                            while (reader.Read())
                            {
                                string id = reader["id_cuisinier"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) ? "N/A" : reader["nom_cuisinier"].ToString();
                                string prenom = reader.IsDBNull(reader.GetOrdinal("prénom_cuisinier")) ? "N/A" : reader["prénom_cuisinier"].ToString();
                                string restaurant = reader.IsDBNull(reader.GetOrdinal("nom_resto")) ? "N/A" : reader["nom_resto"].ToString();
                                int nombreLivraisons = reader.IsDBNull(reader.GetOrdinal("nombre_livraisons")) ? 0 : Convert.ToInt32(reader["nombre_livraisons"]);

                                Console.WriteLine($"{id}\t{nom,-15}{prenom,-15}{restaurant,-20}{nombreLivraisons}");
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

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        static void AfficherCommandesParPeriode()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("COMMANDES PAR PÉRIODE");

            Console.WriteLine("Choisissez une période :");
            Console.WriteLine("1. Aujourd'hui");
            Console.WriteLine("2. Cette semaine");
            Console.WriteLine("3. Ce mois-ci");
            Console.WriteLine("4. Période personnalisée");
            Console.Write("\nVotre choix : ");
            string choixPeriode = Console.ReadLine();

            string dateDebut = DateTime.Now.ToString("yyyy-MM-dd");
            string dateFin = DateTime.Now.ToString("yyyy-MM-dd");

            switch (choixPeriode)
            {
                case "1": // Aujourd'hui
                    break;
                case "2": // Cette semaine
                          // Premier jour de la semaine (lundi)
                    dateDebut = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + (int)DayOfWeek.Monday).ToString("yyyy-MM-dd");
                    break;
                case "3": // Ce mois-ci
                    dateDebut = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                    break;
                case "4": // Période personnalisée
                    Console.Write("\nDate de début (YYYY-MM-DD) : ");
                    dateDebut = Console.ReadLine();
                    Console.Write("Date de fin (YYYY-MM-DD) : ");
                    dateFin = Console.ReadLine();
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

                    string query = @"
                SELECT c.ID_commande, c.nom, c.prix, c.quantite, c.date_commande,
                       cl.nom AS nom_client, cl.prénom AS prenom_client,
                       r.nom_resto
                FROM COMMANDE c
                JOIN PASSE p ON c.ID_commande = p.ID_commande
                JOIN CLIENT cl ON p.id_client = cl.id_client
                LEFT JOIN RÉCEPTIONNE re ON c.ID_commande = re.ID_commande
                LEFT JOIN RESTAURANT r ON re.id_resto = r.id_resto
                WHERE c.date_commande BETWEEN @DateDebut AND @DateFin
                ORDER BY c.date_commande DESC";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@DateDebut", dateDebut);
                    cmd.Parameters.AddWithValue("@DateFin", dateFin);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine($"\nCommandes du {dateDebut} au {dateFin} :");
                            Console.WriteLine("ID\tDate\t\tPlat\t\tPrix\tQté\tClient\t\tRestaurant");
                            Console.WriteLine("-------------------------------------------------------------------------");

                            int totalCommandes = 0;
                            while (reader.Read())
                            {
                                string id = reader["ID_commande"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom")) ? "N/A" : reader["nom"].ToString();
                                string prix = reader.IsDBNull(reader.GetOrdinal("prix")) ? "N/A" : reader["prix"].ToString();
                                int quantite = reader.IsDBNull(reader.GetOrdinal("quantite")) ? 0 : Convert.ToInt32(reader["quantite"]);

                                string nomClient = reader.IsDBNull(reader.GetOrdinal("nom_client")) ? "N/A" : reader["nom_client"].ToString();
                                string prenomClient = reader.IsDBNull(reader.GetOrdinal("prenom_client")) ? "" : reader["prenom_client"].ToString();
                                string client = $"{nomClient} {prenomClient}";

                                string restaurant = reader.IsDBNull(reader.GetOrdinal("nom_resto")) ? "N/A" : reader["nom_resto"].ToString();
                                string dateCommande = reader.IsDBNull(reader.GetOrdinal("date_commande")) ?
                                    "N/A" : Convert.ToDateTime(reader["date_commande"]).ToString("yyyy-MM-dd");

                                Console.WriteLine($"{id}\t{dateCommande}\t{nom,-15}{prix}\t{quantite}\t{client,-20}{restaurant}");
                                totalCommandes++;
                            }

                            Console.WriteLine($"\nNombre total de commandes : {totalCommandes}");
                        }
                        else
                        {
                            Console.WriteLine($"Aucune commande trouvée pour la période du {dateDebut} au {dateFin}.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        static void AfficherMoyennePrixCommandes()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MOYENNE DES PRIX DES COMMANDES");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                    SELECT 
                        AVG(CAST(SUBSTRING_INDEX(prix, ' ', 1) AS DECIMAL(10,2))) AS prix_moyen,
                        MIN(CAST(SUBSTRING_INDEX(prix, ' ', 1) AS DECIMAL(10,2))) AS prix_min,
                        MAX(CAST(SUBSTRING_INDEX(prix, ' ', 1) AS DECIMAL(10,2))) AS prix_max,
                        COUNT(*) AS nombre_commandes,
                        DATE_FORMAT(date_commande, '%Y-%m') AS mois
                    FROM COMMANDE
                    WHERE prix REGEXP '^[0-9]+(\\.[0-9]+)? '
                    GROUP BY DATE_FORMAT(date_commande, '%Y-%m')
                    ORDER BY mois DESC";


                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("Période\t\tNombre de commandes\tPrix moyen\tPrix minimum\tPrix maximum");
                            Console.WriteLine("-------------------------------------------------------------------------");

                            decimal prixMoyenGlobal = 0;
                            int totalCommandes = 0;

                            while (reader.Read())
                            {
                                string mois = reader.IsDBNull(reader.GetOrdinal("mois")) ? "N/A" : reader["mois"].ToString();
                                decimal prixMoyen = reader.IsDBNull(reader.GetOrdinal("prix_moyen")) ? 0 : Convert.ToDecimal(reader["prix_moyen"]);
                                decimal prixMin = reader.IsDBNull(reader.GetOrdinal("prix_min")) ? 0 : Convert.ToDecimal(reader["prix_min"]);
                                decimal prixMax = reader.IsDBNull(reader.GetOrdinal("prix_max")) ? 0 : Convert.ToDecimal(reader["prix_max"]);
                                int nombreCommandes = reader.IsDBNull(reader.GetOrdinal("nombre_commandes")) ? 0 : Convert.ToInt32(reader["nombre_commandes"]);

                                Console.WriteLine($"{mois}\t\t{nombreCommandes}\t\t\t{prixMoyen:F2} euros\t{prixMin:F2} euros\t{prixMax:F2} euros");

                                prixMoyenGlobal += prixMoyen * nombreCommandes;
                                totalCommandes += nombreCommandes;
                            }

                            if (totalCommandes > 0)
                            {
                                prixMoyenGlobal /= totalCommandes;
                                Console.WriteLine($"\nPrix moyen global : {prixMoyenGlobal:F2} euros");
                                Console.WriteLine($"Nombre total de commandes : {totalCommandes}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucune commande trouvée.");
                        }
                    }

                    // Afficher également la moyenne par type de plat
                    Console.WriteLine("\n--- MOYENNE PAR TYPE DE PLAT ---");

                    string queryByType = @"
                    SELECT 
                        type_plat,
                        AVG(CAST(SUBSTRING_INDEX(prix, ' ', 1) AS DECIMAL(10,2))) AS prix_moyen,
                        COUNT(*) AS nombre_commandes
                    FROM COMMANDE
                    WHERE type_plat IS NOT NULL AND prix REGEXP '^[0-9]+(\\.[0-9]+)? '
                    GROUP BY type_plat
                    ORDER BY prix_moyen DESC";

                    MySqlCommand cmdByType = new MySqlCommand(queryByType, connection);

                    using (MySqlDataReader reader = cmdByType.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("Type de plat\t\tNombre de commandes\tPrix moyen");
                            Console.WriteLine("----------------------------------------------------------");

                            while (reader.Read())
                            {
                                string typePlat = reader.IsDBNull(reader.GetOrdinal("type_plat")) ? "Non spécifié" : reader["type_plat"].ToString();
                                decimal prixMoyen = reader.IsDBNull(reader.GetOrdinal("prix_moyen")) ? 0 : Convert.ToDecimal(reader["prix_moyen"]);
                                int nombreCommandes = reader.IsDBNull(reader.GetOrdinal("nombre_commandes")) ? 0 : Convert.ToInt32(reader["nombre_commandes"]);

                                Console.WriteLine($"{typePlat,-20}\t{nombreCommandes}\t\t\t{prixMoyen:F2} euros");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        static void AfficherMoyenneComptesClients()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MOYENNE DES COMPTES CLIENTS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                    SELECT 
                        cl.id_client,
                        cl.nom,
                        cl.prénom,
                        COUNT(c.ID_commande) AS nombre_commandes,
                        SUM(CAST(SUBSTRING_INDEX(c.prix, ' ', 1) AS DECIMAL(10,2)) * c.quantite) AS montant_total,
                        AVG(CAST(SUBSTRING_INDEX(c.prix, ' ', 1) AS DECIMAL(10,2)) * c.quantite) AS montant_moyen,
                        MAX(c.date_commande) AS derniere_commande
                    FROM CLIENT cl
                    LEFT JOIN PASSE p ON cl.id_client = p.id_client
                    LEFT JOIN COMMANDE c ON p.ID_commande = c.ID_commande
                    WHERE c.prix REGEXP '^[0-9]+(\\.[0-9]+)? '
                    GROUP BY cl.id_client, cl.nom, cl.prénom
                    ORDER BY montant_total DESC";

                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("ID\tNom\t\tPrénom\t\tNb Commandes\tTotal\t\tMoyenne\t\tDernière commande");
                            Console.WriteLine("-------------------------------------------------------------------------------------------");

                            decimal totalGlobal = 0;
                            int totalClients = 0;
                            int totalCommandes = 0;

                            while (reader.Read())
                            {
                                string id = reader["id_client"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom")) ? "N/A" : reader["nom"].ToString();
                                string prenom = reader.IsDBNull(reader.GetOrdinal("prénom")) ? "N/A" : reader["prénom"].ToString();
                                int nombreCommandes = reader.IsDBNull(reader.GetOrdinal("nombre_commandes")) ? 0 : Convert.ToInt32(reader["nombre_commandes"]);
                                decimal montantTotal = reader.IsDBNull(reader.GetOrdinal("montant_total")) ? 0 : Convert.ToDecimal(reader["montant_total"]);
                                decimal montantMoyen = reader.IsDBNull(reader.GetOrdinal("montant_moyen")) ? 0 : Convert.ToDecimal(reader["montant_moyen"]);
                                string derniereCommande = reader.IsDBNull(reader.GetOrdinal("derniere_commande")) ? "Jamais" : Convert.ToDateTime(reader["derniere_commande"]).ToString("yyyy-MM-dd");

                                Console.WriteLine($"{id}\t{nom,-15}{prenom,-15}{nombreCommandes}\t\t{montantTotal:F2} euros\t{montantMoyen:F2} euros\t{derniereCommande}");

                                if (nombreCommandes > 0)
                                {
                                    totalGlobal += montantTotal;
                                    totalClients++;
                                    totalCommandes += nombreCommandes;
                                }
                            }

                            if (totalClients > 0)
                            {
                                decimal moyenneParClient = totalGlobal / totalClients;
                                decimal moyenneParCommande = totalGlobal / totalCommandes;

                                Console.WriteLine("\n--- STATISTIQUES GLOBALES ---");
                                Console.WriteLine($"Montant total de toutes les commandes : {totalGlobal:F2} euros");
                                Console.WriteLine($"Nombre total de clients actifs : {totalClients}");
                                Console.WriteLine($"Nombre total de commandes : {totalCommandes}");
                                Console.WriteLine($"Dépense moyenne par client : {moyenneParClient:F2} euros");
                                Console.WriteLine($"Montant moyen par commande : {moyenneParCommande:F2} euros");
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

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        static void AfficherCommandesParNationalite()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("COMMANDES PAR NATIONALITÉ DES PLATS");

            Console.Write("ID du client (laisser vide pour tous les clients) : ");
            string idClient = Console.ReadLine();

            Console.Write("Nationalité/Type de cuisine (laisser vide pour tous les types) : ");
            string typeCuisine = Console.ReadLine();

            Console.Write("Date de début (YYYY-MM-DD, laisser vide pour toutes les dates) : ");
            string dateDebut = Console.ReadLine();

            Console.Write("Date de fin (YYYY-MM-DD, laisser vide pour aujourd'hui) : ");
            string dateFin = Console.ReadLine();

            // Valeurs par défaut
            if (string.IsNullOrWhiteSpace(dateDebut))
            {
                dateDebut = "2000-01-01"; // Date passée par défaut
            }

            if (string.IsNullOrWhiteSpace(dateFin))
            {
                dateFin = DateTime.Now.ToString("yyyy-MM-dd"); // Aujourd'hui par défaut
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                SELECT c.ID_commande, c.nom, c.prix, c.quantite, c.type_plat, c.date_commande,
                       cl.id_client, cl.nom AS nom_client, cl.prénom AS prenom_client,
                       r.nom_resto, r.type_resto
                FROM COMMANDE c
                JOIN PASSE p ON c.ID_commande = p.ID_commande
                JOIN CLIENT cl ON p.id_client = cl.id_client
                LEFT JOIN RÉCEPTIONNE re ON c.ID_commande = re.ID_commande
                LEFT JOIN RESTAURANT r ON re.id_resto = r.id_resto
                WHERE c.date_commande BETWEEN @DateDebut AND @DateFin";

                    if (!string.IsNullOrWhiteSpace(idClient))
                    {
                        query += " AND cl.id_client = @IdClient";
                    }

                    if (!string.IsNullOrWhiteSpace(typeCuisine))
                    {
                        query += " AND (r.type_resto LIKE @TypeCuisine OR c.type_plat LIKE @TypeCuisine)";
                    }

                    query += " ORDER BY c.date_commande DESC";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@DateDebut", dateDebut);
                    cmd.Parameters.AddWithValue("@DateFin", dateFin);

                    if (!string.IsNullOrWhiteSpace(idClient))
                    {
                        cmd.Parameters.AddWithValue("@IdClient", idClient);
                    }

                    if (!string.IsNullOrWhiteSpace(typeCuisine))
                    {
                        cmd.Parameters.AddWithValue("@TypeCuisine", "%" + typeCuisine + "%");
                    }

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine($"\nCommandes du {dateDebut} au {dateFin} :");
                            Console.WriteLine("ID\tDate\t\tPlat\t\tType\t\tPrix\tQté\tClient\t\tRestaurant (Type)");
                            Console.WriteLine("--------------------------------------------------------------------------------------------------");

                            int totalCommandes = 0;
                            decimal montantTotal = 0;

                            while (reader.Read())
                            {
                                string id = reader["ID_commande"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom")) ? "N/A" : reader["nom"].ToString();
                                string prix = reader.IsDBNull(reader.GetOrdinal("prix")) ? "N/A" : reader["prix"].ToString();
                                int quantite = reader.IsDBNull(reader.GetOrdinal("quantite")) ? 0 : Convert.ToInt32(reader["quantite"]);
                                string typePlat = reader.IsDBNull(reader.GetOrdinal("type_plat")) ? "N/A" : reader["type_plat"].ToString();

                                string idClientDb = reader["id_client"].ToString();
                                string nomClient = reader.IsDBNull(reader.GetOrdinal("nom_client")) ? "N/A" : reader["nom_client"].ToString();
                                string prenomClient = reader.IsDBNull(reader.GetOrdinal("prenom_client")) ? "" : reader["prenom_client"].ToString();
                                string client = $"{idClientDb}: {nomClient} {prenomClient}";

                                string restaurant = reader.IsDBNull(reader.GetOrdinal("nom_resto")) ? "N/A" : reader["nom_resto"].ToString();
                                string typeResto = reader.IsDBNull(reader.GetOrdinal("type_resto")) ? "N/A" : reader["type_resto"].ToString();
                                string dateCommande = reader.IsDBNull(reader.GetOrdinal("date_commande")) ?
                                    "N/A" : Convert.ToDateTime(reader["date_commande"]).ToString("yyyy-MM-dd");

                                Console.WriteLine($"{id}\t{dateCommande}\t{nom,-15}{typePlat,-15}{prix}\t{quantite}\t{client,-20}{restaurant} ({typeResto})");
                                totalCommandes++;

                                // Extraire le montant numérique du prix (si format texte)
                                if (prix.Contains("euro"))
                                {
                                    decimal prixNum;
                                    if (decimal.TryParse(prix.Split(' ')[0], out prixNum))
                                    {
                                        montantTotal += prixNum * quantite;
                                    }
                                }
                                else
                                {
                                    decimal prixNum;
                                    if (decimal.TryParse(prix, out prixNum))
                                    {
                                        montantTotal += prixNum * quantite;
                                    }
                                }
                            }

                            Console.WriteLine($"\nNombre total de commandes : {totalCommandes}");
                            Console.WriteLine($"Montant total : {montantTotal:F2} euros");
                        }
                        else
                        {
                            Console.WriteLine("Aucune commande trouvée correspondant aux critères.");
                        }
                    }

                    // Afficher un résumé par type de cuisine
                    if (string.IsNullOrWhiteSpace(typeCuisine))
                    {
                        Console.WriteLine("\n--- RÉSUMÉ PAR TYPE DE CUISINE ---");

                        string summaryQuery = @"
                    SELECT 
                        COALESCE(r.type_resto, c.type_plat, 'Non spécifié') AS type_cuisine,
                        COUNT(c.ID_commande) AS nombre_commandes,
                        SUM(c.quantite) AS quantite_totale
                    FROM COMMANDE c
                    JOIN PASSE p ON c.ID_commande = p.ID_commande
                    LEFT JOIN RÉCEPTIONNE re ON c.ID_commande = re.ID_commande
                    LEFT JOIN RESTAURANT r ON re.id_resto = r.id_resto
                    WHERE c.date_commande BETWEEN @DateDebut AND @DateFin";

                        if (!string.IsNullOrWhiteSpace(idClient))
                        {
                            summaryQuery += " AND p.id_client = @IdClient";
                        }

                        summaryQuery += " GROUP BY type_cuisine ORDER BY nombre_commandes DESC";

                        MySqlCommand summaryCmd = new MySqlCommand(summaryQuery, connection);
                        summaryCmd.Parameters.AddWithValue("@DateDebut", dateDebut);
                        summaryCmd.Parameters.AddWithValue("@DateFin", dateFin);

                        if (!string.IsNullOrWhiteSpace(idClient))
                        {
                            summaryCmd.Parameters.AddWithValue("@IdClient", idClient);
                        }

                        using (MySqlDataReader summaryReader = summaryCmd.ExecuteReader())
                        {
                            if (summaryReader.HasRows)
                            {
                                Console.WriteLine("Type de cuisine\t\tNombre de commandes\tQuantité totale");
                                Console.WriteLine("----------------------------------------------------------");

                                while (summaryReader.Read())
                                {
                                    string typedeCuisine = summaryReader["type_cuisine"].ToString();
                                    int nombreCommandes = Convert.ToInt32(summaryReader["nombre_commandes"]);
                                    int quantiteTotale = Convert.ToInt32(summaryReader["quantite_totale"]);

                                    Console.WriteLine($"{typedeCuisine,-20}\t{nombreCommandes}\t\t\t{quantiteTotale}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Aucune donnée de résumé disponible.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        #endregion

    }
}
