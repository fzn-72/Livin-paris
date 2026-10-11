using MySql.Data.MySqlClient;
using Projet_problème_scientifique_info;
using System;
using System.Collections.Generic;
using System.Data;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Classe gérant les plats du système de livraison de repas
    /// </summary>
    internal class ModulePlat
    {
        private static string connectionString = GestionBDD.ConnectionString;
        #region Menu principal Plat
        /// <summary>
        /// Affiche le menu principal du module Plats
        /// </summary>
        public static void MenuPrincipalPlats()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("MODULE PLATS");

                /// Afficher les options en fonction du type d'utilisateur
                if (SessionUtilisateur.EstConnecte())
                {
                    if (SessionUtilisateur.TypeUtilisateur == "client")
                    {
                        Console.WriteLine("1. Consulter tous les plats disponibles");
                        Console.WriteLine("2. Exporter la liste des plats au format XML");
                        Console.WriteLine("3. Exporter la liste des plats au format JSON");
                        Console.WriteLine("4. Rechercher un plat");
                        Console.WriteLine("5. Voir les détails d'un plat");
                        Console.WriteLine("6. Retour au menu principal");
                        Console.Write("\nChoisissez une option : ");
                        string choix = Console.ReadLine();

                        switch (choix)
                        {
                            case "1":
                                ConsulterPlatsDisponibles();
                                break;
                            case "2":
                                ExporterListePlats();
                                break;
                            case "3":
                                ExporterListePlatsJSON();
                                break;
                            case "4":
                                RechercherPlat();
                                break;
                            case "5":
                                VoirDetailsPlat();
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
                        Console.WriteLine("1. Consulter tous les plats disponibles");
                        Console.WriteLine("2. Exporter la liste des plats disponibles au format XML");
                        Console.WriteLine("3. Exporter la liste des plats disponibles au format JSON");
                        Console.WriteLine("4. Consulter mes plats");
                        Console.WriteLine("5. Ajouter un nouveau plat");
                        Console.WriteLine("6. Modifier un de mes plats");
                        Console.WriteLine("7. Supprimer un de mes plats");
                        Console.WriteLine("8. Gérer les ingrédients");
                        Console.WriteLine("9. Retour au menu principal");
                        Console.Write("\nChoisissez une option : ");
                        string choix = Console.ReadLine();

                        switch (choix)
                        {
                            case "1":
                                ConsulterPlatsDisponibles();
                                break;
                            case "2":
                                ExporterListePlats();
                                break;
                            case "3":
                                ExporterListePlatsJSON();
                                break;
                            case "4":
                                AfficherPlatsCuisinier(SessionUtilisateur.IdUtilisateur);
                                break;
                            case "5":
                                AjouterNouveauPlat();
                                break;
                            case "6":
                                ModifierPlat();
                                break;
                            case "7":
                                SupprimerPlat();
                                break;
                            case "8":
                                GererIngredients();
                                break;
                            case "9":
                                continuer = false;
                                break;
                            default:
                                GestionBDD.AfficherMessageErreur("Option invalide.");
                                break;
                        }
                    }
                    else if (SessionUtilisateur.TypeUtilisateur == "admin")
                    {
                        Console.WriteLine("1. Consulter tous les plats");
                        Console.WriteLine("2. Rechercher un plat");
                        Console.WriteLine("3. Voir les détails d'un plat");
                        Console.WriteLine("4. Ajouter un nouveau plat");
                        Console.WriteLine("5. Modifier un plat");
                        Console.WriteLine("6. Supprimer un plat");
                        Console.WriteLine("7. Gérer les ingrédients");
                        Console.WriteLine("8. Exporter la liste des plats au format XML");
                        Console.WriteLine("9. Exporter la liste des plats au format JSON");
                        Console.WriteLine("10. Retour au menu principal");
                        Console.Write("\nChoisissez une option : ");
                        string choix = Console.ReadLine();

                        switch (choix)
                        {
                            case "1":
                                AfficherTousLesPlats();
                                break;
                            case "2":
                                RechercherPlat();
                                break;
                            case "3":
                                VoirDetailsPlat();
                                break;
                            case "4":
                                AjouterNouveauPlat();
                                break;
                            case "5":
                                ModifierPlat(true); /// Admin peut modifier tous les plats
                                break;
                            case "6":
                                SupprimerPlat(true); /// Admin peut supprimer tous les plats
                                break;
                            case "7":
                                GererIngredients();
                                break;
                            case "8":
                                ExporterListePlats(); 
                                break;
                            case "9":
                                ExporterListePlatsJSON();
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
                else
                {
                    /// Utilisateur non connecté
                    Console.WriteLine("1. Consulter les plats disponibles");
                    Console.WriteLine("2. Exporter la liste des plats au format XML");
                    Console.WriteLine("3. Exporter la liste des plats au format JSON");
                    Console.WriteLine("4. Rechercher un plat");
                    Console.WriteLine("5. Voir les détails d'un plat");
                    Console.WriteLine("6. Retour au menu principal");
                    Console.Write("\nChoisissez une option : ");
                    string choix = Console.ReadLine();

                    switch (choix)
                    {
                        case "1":
                            ConsulterPlatsDisponibles();
                            break;
                        case "2":
                            ExporterListePlats();
                            break;
                        case "3":
                            ExporterListePlatsJSON();
                            break;
                        case "4":
                            RechercherPlat();
                            break;
                        case "5":
                            VoirDetailsPlat();
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
        }
        #endregion

        #region Affichage des plats

        /// <summary>
        /// Permet de consulter les plats disponibles
        /// </summary>
        public static void ConsulterPlatsDisponibles()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("PLATS DISPONIBLES");

                Console.WriteLine("Comment souhaitez-vous filtrer les plats ?");
                Console.WriteLine("1. Tous les plats");
                Console.WriteLine("2. Par type (entrée, plat principal, dessert)");
                Console.WriteLine("3. Par régime alimentaire (végétarien, végan, etc.)");
                Console.WriteLine("4. Par nationalité");
                Console.WriteLine("5. Par prix (croissant)");
                if (SessionUtilisateur.EstCuisinier())
                {
                    Console.WriteLine("6. Retour au menu cuisinier");
                }
                else if (SessionUtilisateur.EstClient())
                {
                    Console.WriteLine("6. Retour au menu client");
                }

                Console.Write("\nVotre choix : ");
                string choixFiltre = Console.ReadLine();

                string requete = "";
                Dictionary<string, object> parametres = new Dictionary<string, object>();

                switch (choixFiltre)
                {
                    case "1":
                        requete = @"
                            SELECT p.id_plat, p.nom_plat, p.description_plat, p.type_plat, p.prix_unitaire_plat, 
                                   p.regime_alimentaire_plat, p.nationalite_plat, p.portion_dispo_plat,
                                   c.nom_cuisinier, c.prenom_cuisinier, c.note_moyenne_cuisinier
                            FROM PLAT p
                            LEFT JOIN CUISINIER c ON p.id_cuisinier = c.id_cuisinier
                            WHERE p.portion_dispo_plat > 0
                            ORDER BY p.type_plat, p.nom_plat";
                        break;
                    case "2":
                        Console.Write("Entrez le type de plat (entrée, plat principal, dessert) : ");
                        string typePlat = Console.ReadLine();
                        requete = @"
                            SELECT p.id_plat, p.nom_plat, p.description_plat, p.type_plat, p.prix_unitaire_plat, 
                                   p.regime_alimentaire_plat, p.nationalite_plat, p.portion_dispo_plat,
                                   c.nom_cuisinier, c.prenom_cuisinier, c.note_moyenne_cuisinier
                            FROM PLAT p
                            LEFT JOIN CUISINIER c ON p.id_cuisinier = c.id_cuisinier
                            WHERE p.type_plat LIKE @TypePlat AND p.portion_dispo_plat > 0
                            ORDER BY p.nom_plat";
                        parametres.Add("@TypePlat", $"%{typePlat}%");
                        break;
                    case "3":
                        Console.Write("Entrez le régime alimentaire (végétarien, végan, etc.) : ");
                        string regimeAlimentaire = Console.ReadLine();
                        requete = @"
                            SELECT p.id_plat, p.nom_plat, p.description_plat, p.type_plat, p.prix_unitaire_plat, 
                                   p.regime_alimentaire_plat, p.nationalite_plat, p.portion_dispo_plat,
                                   c.nom_cuisinier, c.prenom_cuisinier, c.note_moyenne_cuisinier
                            FROM PLAT p
                            LEFT JOIN CUISINIER c ON p.id_cuisinier = c.id_cuisinier
                            WHERE p.regime_alimentaire_plat LIKE @RegimeAlimentaire AND p.portion_dispo_plat > 0
                            ORDER BY p.type_plat, p.nom_plat";
                        parametres.Add("@RegimeAlimentaire", $"%{regimeAlimentaire}%");
                        break;
                    case "4":
                        Console.Write("Entrez la nationalité : ");
                        string nationalite = Console.ReadLine();
                        requete = @"
                            SELECT p.id_plat, p.nom_plat, p.description_plat, p.type_plat, p.prix_unitaire_plat, 
                                   p.regime_alimentaire_plat, p.nationalite_plat, p.portion_dispo_plat,
                                   c.nom_cuisinier, c.prenom_cuisinier, c.note_moyenne_cuisinier
                            FROM PLAT p
                            LEFT JOIN CUISINIER c ON p.id_cuisinier = c.id_cuisinier
                            WHERE p.nationalite_plat LIKE @Nationalite AND p.portion_dispo_plat > 0
                            ORDER BY p.type_plat, p.nom_plat";
                        parametres.Add("@Nationalite", $"%{nationalite}%");
                        break;
                    case "5":
                        requete = @"
                            SELECT p.id_plat, p.nom_plat, p.description_plat, p.type_plat, p.prix_unitaire_plat, 
                                   p.regime_alimentaire_plat, p.nationalite_plat, p.portion_dispo_plat,
                                   c.nom_cuisinier, c.prenom_cuisinier, c.note_moyenne_cuisinier
                            FROM PLAT p
                            LEFT JOIN CUISINIER c ON p.id_cuisinier = c.id_cuisinier
                            WHERE p.portion_dispo_plat > 0
                            ORDER BY CAST(REPLACE(REPLACE(p.prix_unitaire_plat, ' euros', ''), ',', '.') AS DECIMAL(10,2))";
                        break;
                    case "6":
                        continuer = false;
                        continue;
                    default:
                        GestionBDD.AfficherMessageErreur("Option invalide.");
                        continue;
                }

                try
                {
                    DataTable plats = GestionBDD.ExecuterRequeteTable(requete, parametres);

                    if (plats != null && plats.Rows.Count > 0)
                    {
                        Console.WriteLine("\nID\tNom\t\tType\t\tPrix\t\tRégime\t\tNationalité\tCuisinier\t\tNote");
                        Console.WriteLine("-------------------------------------------------------------------------");

                        foreach (DataRow row in plats.Rows)
                        {
                            string idPlat = row["id_plat"].ToString();
                            string nomPlat = row["nom_plat"] != DBNull.Value ? row["nom_plat"].ToString() : "N/A";
                            string typePlat = row["type_plat"] != DBNull.Value ? row["type_plat"].ToString() : "N/A";
                            string prixPlat = row["prix_unitaire_plat"] != DBNull.Value ? row["prix_unitaire_plat"].ToString() : "N/A";
                            string regimePlat = row["regime_alimentaire_plat"] != DBNull.Value ? row["regime_alimentaire_plat"].ToString() : "N/A";
                            string nationalitePlat = row["nationalite_plat"] != DBNull.Value ? row["nationalite_plat"].ToString() : "N/A";
                            string portionDispo = row["portion_dispo_plat"] != DBNull.Value ? row["portion_dispo_plat"].ToString() : "0";
                            string nomCuisinier = row["nom_cuisinier"] != DBNull.Value ? row["nom_cuisinier"].ToString() : "N/A";
                            string prenomCuisinier = row["prenom_cuisinier"] != DBNull.Value ? row["prenom_cuisinier"].ToString() : "N/A";
                            string cuisinier = nomCuisinier != "N/A" ? $"{prenomCuisinier} {nomCuisinier}" : "N/A";
                            string noteCuisinier = row["note_moyenne_cuisinier"] != DBNull.Value ? row["note_moyenne_cuisinier"].ToString() : "N/A";

                            Console.WriteLine($"{idPlat}\t{nomPlat,-15}{typePlat,-15}{prixPlat,-15}{regimePlat,-15}{nationalitePlat,-15}{cuisinier,-20}{noteCuisinier}/10");
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
                        Console.WriteLine("Aucun plat disponible avec ces critères.");
                    }
                    Console.WriteLine("\nOption :");
                    Console.WriteLine("1. Exporter cette liste au format XML");
                    Console.WriteLine("2. Retour au menu précédent");

                    Console.Write("\nVotre choix : ");
                    string choix = Console.ReadLine();

                    if (choix == "1")
                    {
                        ExporterListePlats();
                    }
                }
                catch (Exception ex)
                {
                    GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
                }

                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
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
                           p.regime_alimentaire_plat, p.nationalite_plat, p.portion_dispo_plat,
                           c.id_cuisinier, c.nom_cuisinier, c.prenom_cuisinier, c.note_moyenne_cuisinier
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
                    string portionDispo = plat["portion_dispo_plat"] != DBNull.Value ? plat["portion_dispo_plat"].ToString() : "0";
                    string idCuisinier = plat["id_cuisinier"] != DBNull.Value ? plat["id_cuisinier"].ToString() : "";
                    string nomCuisinier = plat["nom_cuisinier"] != DBNull.Value ? plat["nom_cuisinier"].ToString() : "N/A";
                    string prenomCuisinier = plat["prenom_cuisinier"] != DBNull.Value ? plat["prenom_cuisinier"].ToString() : "N/A";
                    string cuisinier = nomCuisinier != "N/A" ? $"{prenomCuisinier} {nomCuisinier}" : "N/A";
                    string noteCuisinier = plat["note_moyenne_cuisinier"] != DBNull.Value ? plat["note_moyenne_cuisinier"].ToString() : "N/A";

                    Console.WriteLine($"Nom : {nomPlat}");
                    Console.WriteLine($"Description : {descriptionPlat}");
                    Console.WriteLine($"Type : {typePlat}");
                    Console.WriteLine($"Prix : {prixPlat}");
                    Console.WriteLine($"Régime alimentaire : {regimePlat}");
                    Console.WriteLine($"Nationalité : {nationalitePlat}");
                    Console.WriteLine($"Portions disponibles : {portionDispo}");
                    Console.WriteLine($"Cuisinier : {cuisinier}");
                    Console.WriteLine($"Note du cuisinier : {noteCuisinier}/10");

                    string requeteIngredients = @"
                        SELECT i.id_ingredient, i.nom_ingredient, i.categorie_ingredient, i.origine_ingredient, i.allergene_ingredient
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
                            bool estAllergene = ingredient["allergene_ingredient"] != DBNull.Value && Convert.ToBoolean(ingredient["allergene_ingredient"]);

                            Console.WriteLine($"- {nomIngredient} ({categorieIngredient}, {origineIngredient}){(estAllergene ? " [ALLERGÈNE]" : "")}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nAucun ingrédient trouvé pour ce plat.");
                    }

                    /// Afficher les évaluations du cuisinier
                    if (!string.IsNullOrEmpty(idCuisinier))
                    {
                        string requeteEvaluations = @"
                            SELECT e.note_evaluation, e.commentaire_evaluation, e.date_evaluation
                            FROM EVALUATION e
                            JOIN reçoit r ON e.id_evaluation = r.id_evaluation
                            WHERE r.id_cuisinier = @IdCuisinier
                            ORDER BY e.date_evaluation DESC
                            LIMIT 10";

                        Dictionary<string, object> parametresEvaluations = new Dictionary<string, object>
                        {
                            { "@IdCuisinier", idCuisinier }
                        };

                        DataTable evaluations = GestionBDD.ExecuterRequeteTable(requeteEvaluations, parametresEvaluations);

                        if (evaluations != null && evaluations.Rows.Count > 0)
                        {
                            Console.WriteLine("\nDernières évaluations du cuisinier :");
                            foreach (DataRow evaluation in evaluations.Rows)
                            {
                                string note = evaluation["note_evaluation"] != DBNull.Value ? evaluation["note_evaluation"].ToString() : "N/A";
                                string commentaire = evaluation["commentaire_evaluation"] != DBNull.Value ? evaluation["commentaire_evaluation"].ToString() : "N/A";
                                string date = evaluation["date_evaluation"] != DBNull.Value ? Convert.ToDateTime(evaluation["date_evaluation"]).ToString("dd/MM/yyyy") : "N/A";

                                Console.WriteLine($"- {note}/10 le {date} : \"{commentaire}\"");
                            }
                        }
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
        /// Affiche tous les plats (disponibles ou non) - réservé à l'admin
        /// </summary>
        private static void AfficherTousLesPlats()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("TOUS LES PLATS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT p.id_plat, p.nom_plat, p.description_plat, p.type_plat, p.prix_unitaire_plat, 
                               p.regime_alimentaire_plat, p.nationalite_plat, p.portion_dispo_plat,
                               c.nom_cuisinier, c.prenom_cuisinier
                        FROM PLAT p
                        JOIN CUISINIER c ON p.id_cuisinier = c.id_cuisinier
                        ORDER BY p.type_plat, p.nom_plat";

                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-10} {4,-15} {5,-15} {6,-10} {7,-20}",
                                "ID", "Nom", "Type", "Prix", "Régime", "Nationalité", "Portions", "Cuisinier");
                            Console.WriteLine(new string('-', 115));

                            while (reader.Read())
                            {
                                string id = reader["id_plat"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom_plat")) ? "N/A" : reader["nom_plat"].ToString();
                                string type = reader.IsDBNull(reader.GetOrdinal("type_plat")) ? "N/A" : reader["type_plat"].ToString();
                                string prix = reader.IsDBNull(reader.GetOrdinal("prix_unitaire_plat")) ? "N/A" : reader["prix_unitaire_plat"].ToString();
                                string regime = reader.IsDBNull(reader.GetOrdinal("regime_alimentaire_plat")) ? "N/A" : reader["regime_alimentaire_plat"].ToString();
                                string nationalite = reader.IsDBNull(reader.GetOrdinal("nationalite_plat")) ? "N/A" : reader["nationalite_plat"].ToString();
                                string portions = reader.IsDBNull(reader.GetOrdinal("portion_dispo_plat")) ? "0" : reader["portion_dispo_plat"].ToString();

                                string nomCuisinier = "N/A";
                                if (!reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) && !reader.IsDBNull(reader.GetOrdinal("prenom_cuisinier")))
                                {
                                    nomCuisinier = reader["nom_cuisinier"].ToString() + " " + reader["prenom_cuisinier"].ToString();
                                }

                                Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-10} {4,-15} {5,-15} {6,-10} {7,-20}",
                                    id, nom, type, prix, regime, nationalite, portions, nomCuisinier);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun plat trouvé dans la base de données.");
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
        /// Affiche les plats d'un cuisinier spécifique
        /// </summary>
        private static void AfficherPlatsCuisinier(string idCuisinier)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MES PLATS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT p.id_plat, p.nom_plat, p.description_plat, p.type_plat, p.prix_unitaire_plat, 
                               p.regime_alimentaire_plat, p.nationalite_plat, p.portion_dispo_plat
                        FROM PLAT p
                        WHERE p.id_cuisinier = @IdCuisinier
                        ORDER BY p.type_plat, p.nom_plat";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-10} {4,-15} {5,-15} {6,-10}",
                                "ID", "Nom", "Type", "Prix", "Régime", "Nationalité", "Portions");
                            Console.WriteLine(new string('-', 95));

                            while (reader.Read())
                            {
                                string id = reader["id_plat"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom_plat")) ? "N/A" : reader["nom_plat"].ToString();
                                string type = reader.IsDBNull(reader.GetOrdinal("type_plat")) ? "N/A" : reader["type_plat"].ToString();
                                string prix = reader.IsDBNull(reader.GetOrdinal("prix_unitaire_plat")) ? "N/A" : reader["prix_unitaire_plat"].ToString();
                                string regime = reader.IsDBNull(reader.GetOrdinal("regime_alimentaire_plat")) ? "N/A" : reader["regime_alimentaire_plat"].ToString();
                                string nationalite = reader.IsDBNull(reader.GetOrdinal("nationalite_plat")) ? "N/A" : reader["nationalite_plat"].ToString();
                                string portions = reader.IsDBNull(reader.GetOrdinal("portion_dispo_plat")) ? "0" : reader["portion_dispo_plat"].ToString();

                                Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-10} {4,-15} {5,-15} {6,-10}",
                                    id, nom, type, prix, regime, nationalite, portions);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Vous n'avez pas encore créé de plats.");
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
        /// Recherche un plat par nom, type ou nationalité
        /// </summary>
        private static void RechercherPlat()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("RECHERCHER UN PLAT");

            Console.WriteLine("1. Rechercher par nom");
            Console.WriteLine("2. Rechercher par type (entrée, plat principal, dessert, etc.)");
            Console.WriteLine("3. Rechercher par nationalité");
            Console.WriteLine("4. Rechercher par régime alimentaire");
            Console.Write("\nChoisissez une option : ");
            string choix = Console.ReadLine();

            string colonne = "";
            string critere = "";

            switch (choix)
            {
                case "1":
                    colonne = "nom_plat";
                    Console.Write("Entrez le nom (ou une partie du nom) : ");
                    critere = Console.ReadLine();
                    break;
                case "2":
                    colonne = "type_plat";
                    Console.Write("Entrez le type (entrée, plat principal, dessert, etc.) : ");
                    critere = Console.ReadLine();
                    break;
                case "3":
                    colonne = "nationalite_plat";
                    Console.Write("Entrez la nationalité : ");
                    critere = Console.ReadLine();
                    break;
                case "4":
                    colonne = "regime_alimentaire_plat";
                    Console.Write("Entrez le régime alimentaire (standard, végétarien, végan, etc.) : ");
                    critere = Console.ReadLine();
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

                    string query = $@"
                        SELECT p.id_plat, p.nom_plat, p.description_plat, p.type_plat, p.prix_unitaire_plat, 
                               p.regime_alimentaire_plat, p.nationalite_plat, c.nom_cuisinier, c.prenom_cuisinier
                        FROM PLAT p
                        JOIN CUISINIER c ON p.id_cuisinier = c.id_cuisinier
                        WHERE p.{colonne} LIKE @Critere AND p.portion_dispo_plat > 0
                        ORDER BY p.type_plat, p.nom_plat";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Critere", "%" + critere + "%");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("\nRésultats de la recherche :");
                            Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-10} {4,-15} {5,-15} {6,-20}",
                                "ID", "Nom", "Type", "Prix", "Régime", "Nationalité", "Cuisinier");
                            Console.WriteLine(new string('-', 105));

                            while (reader.Read())
                            {
                                string id = reader["id_plat"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom_plat")) ? "N/A" : reader["nom_plat"].ToString();
                                string type = reader.IsDBNull(reader.GetOrdinal("type_plat")) ? "N/A" : reader["type_plat"].ToString();
                                string prix = reader.IsDBNull(reader.GetOrdinal("prix_unitaire_plat")) ? "N/A" : reader["prix_unitaire_plat"].ToString();
                                string regime = reader.IsDBNull(reader.GetOrdinal("regime_alimentaire_plat")) ? "N/A" : reader["regime_alimentaire_plat"].ToString();
                                string nationalite = reader.IsDBNull(reader.GetOrdinal("nationalite_plat")) ? "N/A" : reader["nationalite_plat"].ToString();

                                string nomCuisinier = "N/A";
                                if (!reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) && !reader.IsDBNull(reader.GetOrdinal("prenom_cuisinier")))
                                {
                                    nomCuisinier = reader["nom_cuisinier"].ToString() + " " + reader["prenom_cuisinier"].ToString();
                                }

                                Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-10} {4,-15} {5,-15} {6,-20}",
                                    id, nom, type, prix, regime, nationalite, nomCuisinier);
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nAucun plat trouvé correspondant à votre recherche.");
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
        /// Affiche les détails d'un plat spécifique
        /// </summary>
        private static void VoirDetailsPlat()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("DÉTAILS DU PLAT");

            Console.Write("Entrez l'ID du plat : ");
            string idPlat = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT p.id_plat, p.nom_plat, p.description_plat, p.type_plat, p.prix_unitaire_plat, 
                               p.regime_alimentaire_plat, p.nationalite_plat, p.portion_dispo_plat,
                               p.date_fabrication_plat, p.date_peremption_plat,
                               c.id_cuisinier, c.nom_cuisinier, c.prenom_cuisinier, c.note_moyenne_cuisinier
                        FROM PLAT p
                        JOIN CUISINIER c ON p.id_cuisinier = c.id_cuisinier
                        WHERE p.id_plat = @IdPlat";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdPlat", idPlat);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nom = reader.IsDBNull(reader.GetOrdinal("nom_plat")) ? "N/A" : reader["nom_plat"].ToString();
                            string description = reader.IsDBNull(reader.GetOrdinal("description_plat")) ? "N/A" : reader["description_plat"].ToString();
                            string type = reader.IsDBNull(reader.GetOrdinal("type_plat")) ? "N/A" : reader["type_plat"].ToString();
                            string prix = reader.IsDBNull(reader.GetOrdinal("prix_unitaire_plat")) ? "N/A" : reader["prix_unitaire_plat"].ToString();
                            string regime = reader.IsDBNull(reader.GetOrdinal("regime_alimentaire_plat")) ? "N/A" : reader["regime_alimentaire_plat"].ToString();
                            string nationalite = reader.IsDBNull(reader.GetOrdinal("nationalite_plat")) ? "N/A" : reader["nationalite_plat"].ToString();
                            string portions = reader.IsDBNull(reader.GetOrdinal("portion_dispo_plat")) ? "0" : reader["portion_dispo_plat"].ToString();
                            string dateFabrication = reader.IsDBNull(reader.GetOrdinal("date_fabrication_plat")) ? "N/A" : reader["date_fabrication_plat"].ToString();
                            string datePeremption = reader.IsDBNull(reader.GetOrdinal("date_peremption_plat")) ? "N/A" : reader["date_peremption_plat"].ToString();

                            string idCuisinier = reader["id_cuisinier"].ToString();
                            string nomCuisinier = "N/A";
                            if (!reader.IsDBNull(reader.GetOrdinal("nom_cuisinier")) && !reader.IsDBNull(reader.GetOrdinal("prenom_cuisinier")))
                            {
                                nomCuisinier = reader["nom_cuisinier"].ToString() + " " + reader["prenom_cuisinier"].ToString();
                            }
                            string noteCuisinier = reader.IsDBNull(reader.GetOrdinal("note_moyenne_cuisinier")) ? "N/A" : reader["note_moyenne_cuisinier"].ToString();

                            Console.WriteLine($"Nom : {nom}");
                            Console.WriteLine($"Description : {description}");
                            Console.WriteLine($"Type : {type}");
                            Console.WriteLine($"Prix : {prix}");
                            Console.WriteLine($"Régime alimentaire : {regime}");
                            Console.WriteLine($"Nationalité : {nationalite}");
                            Console.WriteLine($"Portions disponibles : {portions}");
                            Console.WriteLine($"Date de fabrication : {dateFabrication}");
                            Console.WriteLine($"Date de péremption : {datePeremption}");
                            Console.WriteLine($"Cuisinier : {nomCuisinier} (Note moyenne : {noteCuisinier}/10)");

                            /// Afficher les ingrédients du plat
                            Console.WriteLine("\nIngrédients :");
                            AfficherIngredientsPlat(idPlat);
                        }
                        else
                        {
                            Console.WriteLine("Plat non trouvé.");
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
        /// Affiche les ingrédients d'un plat
        /// </summary>
        private static void AfficherIngredientsPlat(string idPlat)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT i.id_ingredient, i.nom_ingredient, i.categorie_ingredient, i.origine_ingredient, i.allergene_ingredient
                        FROM INGREDIENT i
                        JOIN contient c ON i.id_ingredient = c.id_ingredient
                        WHERE c.id_plat = @IdPlat
                        ORDER BY i.nom_ingredient";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdPlat", idPlat);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-15} {4,-10}",
                                "ID", "Nom", "Catégorie", "Origine", "Allergène");
                            Console.WriteLine(new string('-', 70));

                            while (reader.Read())
                            {
                                string id = reader["id_ingredient"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom_ingredient")) ? "N/A" : reader["nom_ingredient"].ToString();
                                string categorie = reader.IsDBNull(reader.GetOrdinal("categorie_ingredient")) ? "N/A" : reader["categorie_ingredient"].ToString();
                                string origine = reader.IsDBNull(reader.GetOrdinal("origine_ingredient")) ? "N/A" : reader["origine_ingredient"].ToString();
                                bool allergene = reader.IsDBNull(reader.GetOrdinal("allergene_ingredient")) ? false : Convert.ToBoolean(reader["allergene_ingredient"]);

                                Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-15} {4,-10}",
                                    id, nom, categorie, origine, allergene ? "Oui" : "Non");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun ingrédient enregistré pour ce plat.");
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

        #region Gestion des plats

        /// <summary>
        /// Ajoute un nouveau plat
        /// </summary>
        public static void AjouterNouveauPlat()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("AJOUTER UN NOUVEAU PLAT");

            if (!(SessionUtilisateur.TypeUtilisateur == "cuisinier" || SessionUtilisateur.TypeUtilisateur == "admin"))
            {
                GestionBDD.AfficherMessageErreur("Vous n'êtes pas autorisé à ajouter des plats.");
                return;
            }

            string idCuisinier = SessionUtilisateur.IdUtilisateur;

            /// Si l'utilisateur est un admin, demander pour quel cuisinier ajouter le plat
            if (SessionUtilisateur.TypeUtilisateur == "admin")
            {
                Console.WriteLine("Vous êtes connecté en tant qu'administrateur.");
                Console.Write("Voulez-vous ajouter ce plat pour un cuisinier spécifique? (O/N) : ");
                if (Console.ReadLine().ToUpper() == "O")
                {
                    Console.Write("Entrez l'ID du cuisinier : ");
                    idCuisinier = Console.ReadLine();
                }
            }

            Console.Write("ID du plat : ");
            string idPlat = Console.ReadLine();

            Console.Write("Nom du plat : ");
            string nomPlat = Console.ReadLine();

            Console.Write("Description du plat : ");
            string descriptionPlat = Console.ReadLine();

            Console.Write("Type de plat (entrée, plat principal, dessert, etc.) : ");
            string typePlat = Console.ReadLine();

            Console.Write("Prix unitaire : ");
            string prixUnitaire = Console.ReadLine();

            Console.Write("Régime alimentaire (standard, végétarien, végan, etc.) : ");
            string regimeAlimentaire = Console.ReadLine();

            Console.Write("Portion disponible : ");
            string portionDispo = Console.ReadLine();

            Console.Write("Date de fabrication (YYYY-MM-DD) : ");
            string dateFabrication = Console.ReadLine();

            Console.Write("Date de péremption (YYYY-MM-DD) : ");
            string datePeremption = Console.ReadLine();

            Console.Write("Nationalité du plat : ");
            string nationalitePlat = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO PLAT (id_plat, nom_plat, description_plat, type_plat, prix_unitaire_plat, " +
                                  "regime_alimentaire_plat, portion_dispo_plat, date_fabrication_plat, date_peremption_plat, nationalite_plat, id_cuisinier) " +
                                  "VALUES (@IdPlat, @NomPlat, @DescriptionPlat, @TypePlat, @PrixUnitaire, " +
                                  "@RegimeAlimentaire, @PortionDispo, @DateFabrication, @DatePeremption, @NationalitePlat, @IdCuisinier)";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdPlat", idPlat);
                    cmd.Parameters.AddWithValue("@NomPlat", nomPlat);
                    cmd.Parameters.AddWithValue("@DescriptionPlat", descriptionPlat);
                    cmd.Parameters.AddWithValue("@TypePlat", typePlat);
                    cmd.Parameters.AddWithValue("@PrixUnitaire", prixUnitaire);
                    cmd.Parameters.AddWithValue("@RegimeAlimentaire", regimeAlimentaire);
                    cmd.Parameters.AddWithValue("@PortionDispo", portionDispo);
                    cmd.Parameters.AddWithValue("@DateFabrication", dateFabrication);
                    cmd.Parameters.AddWithValue("@DatePeremption", datePeremption);
                    cmd.Parameters.AddWithValue("@NationalitePlat", nationalitePlat);
                    cmd.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        GestionBDD.AfficherMessageSucces("Plat ajouté avec succès !");

                        /// Ajouter les ingrédients au plat
                        Console.WriteLine("\nVoulez-vous ajouter des ingrédients à ce plat ? (O/N) : ");
                        if (Console.ReadLine().ToUpper() == "O")
                        {
                            AjouterIngredientsAuPlat(idPlat, connection);
                        }
                    }
                    else
                    {
                        GestionBDD.AfficherMessageErreur("Échec de l'ajout du plat.");
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }
        }

        /// <summary>
        /// Ajoute des ingrédients à un plat
        /// </summary>
        private static void AjouterIngredientsAuPlat(string idPlat, MySqlConnection connection)
        {
            bool continuerAjout = true;

            while (continuerAjout)
            {
                /// Afficher la liste des ingrédients disponibles
                Console.WriteLine("\n--- INGRÉDIENTS DISPONIBLES ---");
                string queryIngredients = "SELECT id_ingredient, nom_ingredient, categorie_ingredient FROM INGREDIENT ORDER BY nom_ingredient";
                MySqlCommand cmdIngredients = new MySqlCommand(queryIngredients, connection);

                using (MySqlDataReader reader = cmdIngredients.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine("{0,-5} {1,-25} {2,-15}",
                            "ID", "Nom", "Catégorie");
                        Console.WriteLine(new string('-', 45));

                        while (reader.Read())
                        {
                            string id = reader["id_ingredient"].ToString();
                            string nom = reader.IsDBNull(reader.GetOrdinal("nom_ingredient")) ? "N/A" : reader["nom_ingredient"].ToString();
                            string categorie = reader.IsDBNull(reader.GetOrdinal("categorie_ingredient")) ? "N/A" : reader["categorie_ingredient"].ToString();

                            Console.WriteLine("{0,-5} {1,-25} {2,-15}",
                                id, nom, categorie);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Aucun ingrédient disponible.");
                        return;
                    }
                }

                Console.Write("\nID de l'ingrédient à ajouter : ");
                string idIngredient = Console.ReadLine();

                /// Ajouter l'ingrédient au plat
                string queryContient = "INSERT INTO contient (id_plat, id_ingredient) VALUES (@IdPlat, @IdIngredient)";
                MySqlCommand cmdContient = new MySqlCommand(queryContient, connection);
                cmdContient.Parameters.AddWithValue("@IdPlat", idPlat);
                cmdContient.Parameters.AddWithValue("@IdIngredient", idIngredient);

                try
                {
                    int result = cmdContient.ExecuteNonQuery();
                    if (result > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Ingrédient ajouté au plat avec succès !");
                        Console.ResetColor();

                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Échec de l'ajout de l'ingrédient au plat.");
                        Console.ResetColor();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur : {ex.Message}");
                }

                Console.Write("\nVoulez-vous ajouter un autre ingrédient ? (O/N) : ");
                continuerAjout = Console.ReadLine().ToUpper() == "O";
            }
        }

        /// <summary>
        /// Modifie un plat existant
        /// </summary>
        private static void ModifierPlat(bool estAdmin = false)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MODIFIER UN PLAT");

            Console.Write("Entrez l'ID du plat à modifier : ");
            string idPlat = Console.ReadLine();

            /// Vérifier si le plat existe et si l'utilisateur a le droit de le modifier
            if (!estAdmin && !PlatAppartientAuCuisinier(idPlat, SessionUtilisateur.IdUtilisateur))
            {
                GestionBDD.AfficherMessageErreur("Vous n'êtes pas autorisé à modifier ce plat ou le plat n'existe pas.");
                return;
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    /// Récupérer les informations actuelles du plat
                    string queryInfo = @"
                        SELECT nom_plat, description_plat, type_plat, prix_unitaire_plat, 
                               regime_alimentaire_plat, portion_dispo_plat, nationalite_plat
                        FROM PLAT
                        WHERE id_plat = @IdPlat";

                    MySqlCommand cmdInfo = new MySqlCommand(queryInfo, connection);
                    cmdInfo.Parameters.AddWithValue("@IdPlat", idPlat);

                    using (MySqlDataReader reader = cmdInfo.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string nomActuel = reader.IsDBNull(reader.GetOrdinal("nom_plat")) ? "" : reader["nom_plat"].ToString();
                            string descriptionActuelle = reader.IsDBNull(reader.GetOrdinal("description_plat")) ? "" : reader["description_plat"].ToString();
                            string typeActuel = reader.IsDBNull(reader.GetOrdinal("type_plat")) ? "" : reader["type_plat"].ToString();
                            string prixActuel = reader.IsDBNull(reader.GetOrdinal("prix_unitaire_plat")) ? "" : reader["prix_unitaire_plat"].ToString();
                            string regimeActuel = reader.IsDBNull(reader.GetOrdinal("regime_alimentaire_plat")) ? "" : reader["regime_alimentaire_plat"].ToString();
                            string portionActuelle = reader.IsDBNull(reader.GetOrdinal("portion_dispo_plat")) ? "" : reader["portion_dispo_plat"].ToString();
                            string nationaliteActuelle = reader.IsDBNull(reader.GetOrdinal("nationalite_plat")) ? "" : reader["nationalite_plat"].ToString();

                            reader.Close();

                            Console.WriteLine("\nInformations actuelles du plat :");
                            Console.WriteLine($"Nom : {nomActuel}");
                            Console.WriteLine($"Description : {descriptionActuelle}");
                            Console.WriteLine($"Type : {typeActuel}");
                            Console.WriteLine($"Prix : {prixActuel}");
                            Console.WriteLine($"Régime alimentaire : {regimeActuel}");
                            Console.WriteLine($"Portion disponible : {portionActuelle}");
                            Console.WriteLine($"Nationalité : {nationaliteActuelle}");

                            Console.WriteLine("\nLaissez vide pour conserver la valeur actuelle.");

                            Console.Write("\nNouveau nom : ");
                            string nouveauNom = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauNom))
                                nouveauNom = nomActuel;

                            Console.Write("Nouvelle description : ");
                            string nouvelleDescription = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouvelleDescription))
                                nouvelleDescription = descriptionActuelle;

                            Console.Write("Nouveau type : ");
                            string nouveauType = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauType))
                                nouveauType = typeActuel;

                            Console.Write("Nouveau prix : ");
                            string nouveauPrix = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauPrix))
                                nouveauPrix = prixActuel;

                            Console.Write("Nouveau régime alimentaire : ");
                            string nouveauRegime = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouveauRegime))
                                nouveauRegime = regimeActuel;

                            Console.Write("Nouvelle portion disponible : ");
                            string nouvellePortion = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouvellePortion))
                                nouvellePortion = portionActuelle;

                            Console.Write("Nouvelle nationalité : ");
                            string nouvelleNationalite = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(nouvelleNationalite))
                                nouvelleNationalite = nationaliteActuelle;

                            string queryUpdate = @"
                                UPDATE PLAT
                                SET nom_plat = @NomPlat,
                                    description_plat = @DescriptionPlat,
                                    type_plat = @TypePlat,
                                    prix_unitaire_plat = @PrixUnitaire,
                                    regime_alimentaire_plat = @RegimeAlimentaire,
                                    portion_dispo_plat = @PortionDispo,
                                    nationalite_plat = @NationalitePlat
                                WHERE id_plat = @IdPlat";

                            MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, connection);
                            cmdUpdate.Parameters.AddWithValue("@NomPlat", nouveauNom);
                            cmdUpdate.Parameters.AddWithValue("@DescriptionPlat", nouvelleDescription);
                            cmdUpdate.Parameters.AddWithValue("@TypePlat", nouveauType);
                            cmdUpdate.Parameters.AddWithValue("@PrixUnitaire", nouveauPrix);
                            cmdUpdate.Parameters.AddWithValue("@RegimeAlimentaire", nouveauRegime);
                            cmdUpdate.Parameters.AddWithValue("@PortionDispo", nouvellePortion);
                            cmdUpdate.Parameters.AddWithValue("@NationalitePlat", nouvelleNationalite);
                            cmdUpdate.Parameters.AddWithValue("@IdPlat", idPlat);

                            int result = cmdUpdate.ExecuteNonQuery();

                            if (result > 0)
                            {
                                GestionBDD.AfficherMessageSucces("Plat modifié avec succès !");

                                /// Proposer de modifier les ingrédients
                                Console.Write("\nVoulez-vous modifier les ingrédients du plat ? (O/N) : ");
                                if (Console.ReadLine().ToUpper() == "O")
                                {
                                    Console.WriteLine("\nIngrédients actuels :");
                                    AfficherIngredientsPlat(idPlat);

                                    Console.Write("\nVoulez-vous supprimer tous les ingrédients et en ajouter de nouveaux ? (O/N) : ");
                                    if (Console.ReadLine().ToUpper() == "O")
                                    {
                                        string queryDelete = "DELETE FROM contient WHERE id_plat = @IdPlat";
                                        MySqlCommand cmdDelete = new MySqlCommand(queryDelete, connection);
                                        cmdDelete.Parameters.AddWithValue("@IdPlat", idPlat);
                                        cmdDelete.ExecuteNonQuery();

                                        AjouterIngredientsAuPlat(idPlat, connection);
                                    }
                                    else
                                    {
                                        Console.Write("\nVoulez-vous ajouter de nouveaux ingrédients ? (O/N) : ");
                                        if (Console.ReadLine().ToUpper() == "O")
                                        {
                                            AjouterIngredientsAuPlat(idPlat, connection);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                GestionBDD.AfficherMessageErreur("Échec de la modification du plat.");
                            }
                        }
                        else
                        {
                            reader.Close();
                            GestionBDD.AfficherMessageErreur("Plat non trouvé.");
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
        /// Supprime un plat
        /// </summary>
        private static void SupprimerPlat(bool estAdmin = false)
        {
            Console.Clear();
            GestionBDD.AfficherTitre("SUPPRIMER UN PLAT");

            Console.Write("Entrez l'ID du plat à supprimer : ");
            string idPlat = Console.ReadLine();

            if (!estAdmin && !PlatAppartientAuCuisinier(idPlat, SessionUtilisateur.IdUtilisateur))
            {
                GestionBDD.AfficherMessageErreur("Vous n'êtes pas autorisé à supprimer ce plat ou le plat n'existe pas.");
                return;
            }

            if (PlatUtiliseDansCommandes(idPlat))
            {
                Console.WriteLine("Ce plat est utilisé dans des commandes existantes.");
                Console.WriteLine("La suppression n'est pas possible pour maintenir l'intégrité des données.");
                Console.WriteLine("Vous pouvez mettre la portion disponible à 0 pour le rendre indisponible.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            Console.Write("\nÊtes-vous sûr de vouloir supprimer ce plat ? (O/N) : ");
            if (Console.ReadLine().ToUpper() != "O")
            {
                Console.WriteLine("Suppression annulée.");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
                return;
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    /// Supprimer d'abord les relations dans la table contient
                    string queryContient = "DELETE FROM contient WHERE id_plat = @IdPlat";
                    MySqlCommand cmdContient = new MySqlCommand(queryContient, connection);
                    cmdContient.Parameters.AddWithValue("@IdPlat", idPlat);
                    cmdContient.ExecuteNonQuery();

                    /// Supprimer le plat
                    string queryPlat = "DELETE FROM PLAT WHERE id_plat = @IdPlat";
                    MySqlCommand cmdPlat = new MySqlCommand(queryPlat, connection);
                    cmdPlat.Parameters.AddWithValue("@IdPlat", idPlat);
                    int result = cmdPlat.ExecuteNonQuery();

                    if (result > 0)
                    {
                        GestionBDD.AfficherMessageSucces("Plat supprimé avec succès !");
                    }
                    else
                    {
                        GestionBDD.AfficherMessageErreur("Échec de la suppression du plat.");
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }
        }

        #endregion

        #region Gestion des ingrédients

        /// <summary>
        /// Gère le menu des ingrédients
        /// </summary>
        public static void GererIngredients()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("GESTION DES INGRÉDIENTS");
                Console.WriteLine("1. Afficher la liste des ingrédients");
                Console.WriteLine("2. Ajouter un ingrédient");
                Console.WriteLine("3. Modifier un ingrédient");
                Console.WriteLine("4. Supprimer un ingrédient");
                Console.WriteLine("5. Retour au menu précédent");
                Console.Write("\nChoisissez une option : ");
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
                        continuer = false;
                        break;
                    default:
                        GestionBDD.AfficherMessageErreur("Option invalide.");
                        break;
                }
            }
        }

        /// <summary>
        /// Affiche la liste des ingrédients
        /// </summary>
        private static void AfficherListeIngredients()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("LISTE DES INGRÉDIENTS");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT id_ingredient, nom_ingredient, categorie_ingredient, origine_ingredient, allergene_ingredient, stock_ingredient
                        FROM INGREDIENT
                        ORDER BY nom_ingredient";

                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-15} {4,-10} {5,-10}",
                                "ID", "Nom", "Catégorie", "Origine", "Allergène", "Stock");
                            Console.WriteLine(new string('-', 80));

                            while (reader.Read())
                            {
                                string id = reader["id_ingredient"].ToString();
                                string nom = reader.IsDBNull(reader.GetOrdinal("nom_ingredient")) ? "N/A" : reader["nom_ingredient"].ToString();
                                string categorie = reader.IsDBNull(reader.GetOrdinal("categorie_ingredient")) ? "N/A" : reader["categorie_ingredient"].ToString();
                                string origine = reader.IsDBNull(reader.GetOrdinal("origine_ingredient")) ? "N/A" : reader["origine_ingredient"].ToString();
                                bool allergene = reader.IsDBNull(reader.GetOrdinal("allergene_ingredient")) ? false : Convert.ToBoolean(reader["allergene_ingredient"]);
                                string stock = reader.IsDBNull(reader.GetOrdinal("stock_ingredient")) ? "N/A" : reader["stock_ingredient"].ToString();

                                Console.WriteLine("{0,-5} {1,-25} {2,-15} {3,-15} {4,-10} {5,-10}",
                                    id, nom, categorie, origine, allergene ? "Oui" : "Non", stock);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun ingrédient trouvé dans la base de données.");
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
        /// Ajoute un nouvel ingrédient
        /// </summary>
        private static void AjouterIngredient()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("AJOUTER UN INGRÉDIENT");

            Console.Write("ID de l'ingrédient : ");
            string idIngredient = Console.ReadLine();

            Console.Write("Nom de l'ingrédient : ");
            string nomIngredient = Console.ReadLine();

            Console.Write("Origine de l'ingrédient : ");
            string origineIngredient = Console.ReadLine();

            Console.Write("Catégorie de l'ingrédient : ");
            string categorieIngredient = Console.ReadLine();

            Console.Write("Est-ce un allergène ? (O/N) : ");
            bool allergeneIngredient = Console.ReadLine().ToUpper() == "O";

            Console.Write("Stock disponible : ");
            string stockIngredient = Console.ReadLine();

            Console.Write("Date de péremption (YYYY-MM-DD) : ");
            string datePeremption = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO INGREDIENT (id_ingredient, nom_ingredient, origine_ingredient, categorie_ingredient, 
                                              allergene_ingredient, stock_ingredient, date_peremption)
                        VALUES (@IdIngredient, @NomIngredient, @OrigineIngredient, @CategorieIngredient, 
                               @AllergeneIngredient, @StockIngredient, @DatePeremption)";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdIngredient", idIngredient);
                    cmd.Parameters.AddWithValue("@NomIngredient", nomIngredient);
                    cmd.Parameters.AddWithValue("@OrigineIngredient", origineIngredient);
                    cmd.Parameters.AddWithValue("@CategorieIngredient", categorieIngredient);
                    cmd.Parameters.AddWithValue("@AllergeneIngredient", allergeneIngredient);
                    cmd.Parameters.AddWithValue("@StockIngredient", stockIngredient);
                    cmd.Parameters.AddWithValue("@DatePeremption", datePeremption);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        GestionBDD.AfficherMessageSucces("Ingrédient ajouté avec succès !");
                    }
                    else
                    {
                        GestionBDD.AfficherMessageErreur("Échec de l'ajout de l'ingrédient.");
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }
        }

        /// <summary>
        /// Modifie un ingrédient existant
        /// </summary>
        private static void ModifierIngredient()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("MODIFIER UN INGRÉDIENT");

            Console.Write("ID de l'ingrédient à modifier : ");
            string idIngredient = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    /// Vérifier si l'ingrédient existe
                    string checkQuery = "SELECT * FROM INGREDIENT WHERE id_ingredient = @IdIngredient";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@IdIngredient", idIngredient);

                    using (MySqlDataReader reader = checkCmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            reader.Close();
                            GestionBDD.AfficherMessageErreur("Ingrédient introuvable.");
                            return;
                        }

                        reader.Read();
                        string nomActuel = reader.IsDBNull(reader.GetOrdinal("nom_ingredient")) ? "" : reader["nom_ingredient"].ToString();
                        string origineActuelle = reader.IsDBNull(reader.GetOrdinal("origine_ingredient")) ? "" : reader["origine_ingredient"].ToString();
                        string categorieActuelle = reader.IsDBNull(reader.GetOrdinal("categorie_ingredient")) ? "" : reader["categorie_ingredient"].ToString();
                        bool allergeneActuel = reader.IsDBNull(reader.GetOrdinal("allergene_ingredient")) ? false : Convert.ToBoolean(reader["allergene_ingredient"]);
                        string stockActuel = reader.IsDBNull(reader.GetOrdinal("stock_ingredient")) ? "" : reader["stock_ingredient"].ToString();
                        string datePeremptionActuelle = reader.IsDBNull(reader.GetOrdinal("date_peremption")) ? "" : reader["date_peremption"].ToString();
                        reader.Close();

                        Console.WriteLine($"\nModification de l'ingrédient : {nomActuel}");
                        Console.WriteLine("Laissez vide pour conserver la valeur actuelle.");

                        Console.Write($"Nouveau nom ({nomActuel}) : ");
                        string nouveauNom = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(nouveauNom))
                            nouveauNom = nomActuel;

                        Console.Write($"Nouvelle origine ({origineActuelle}) : ");
                        string nouvelleOrigine = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(nouvelleOrigine))
                            nouvelleOrigine = origineActuelle;

                        Console.Write($"Nouvelle catégorie ({categorieActuelle}) : ");
                        string nouvelleCategorie = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(nouvelleCategorie))
                            nouvelleCategorie = categorieActuelle;

                        Console.Write($"Est-ce un allergène ? (O/N) ({(allergeneActuel ? "O" : "N")}) : ");
                        string allergeneInput = Console.ReadLine();
                        bool nouvelAllergene = allergeneActuel;
                        if (!string.IsNullOrWhiteSpace(allergeneInput))
                            nouvelAllergene = allergeneInput.ToUpper() == "O";

                        Console.Write($"Nouveau stock ({stockActuel}) : ");
                        string nouveauStock = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(nouveauStock))
                            nouveauStock = stockActuel;

                        Console.Write($"Nouvelle date de péremption ({datePeremptionActuelle}) : ");
                        string nouvelleDatePeremption = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(nouvelleDatePeremption))
                            nouvelleDatePeremption = datePeremptionActuelle;

                        string updateQuery = @"
                            UPDATE INGREDIENT 
                            SET nom_ingredient = @NomIngredient, 
                                origine_ingredient = @OrigineIngredient, 
                                categorie_ingredient = @CategorieIngredient, 
                                allergene_ingredient = @AllergeneIngredient, 
                                stock_ingredient = @StockIngredient, 
                                date_peremption = @DatePeremption 
                            WHERE id_ingredient = @IdIngredient";

                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
                        updateCmd.Parameters.AddWithValue("@IdIngredient", idIngredient);
                        updateCmd.Parameters.AddWithValue("@NomIngredient", nouveauNom);
                        updateCmd.Parameters.AddWithValue("@OrigineIngredient", nouvelleOrigine);
                        updateCmd.Parameters.AddWithValue("@CategorieIngredient", nouvelleCategorie);
                        updateCmd.Parameters.AddWithValue("@AllergeneIngredient", nouvelAllergene);
                        updateCmd.Parameters.AddWithValue("@StockIngredient", nouveauStock);
                        updateCmd.Parameters.AddWithValue("@DatePeremption", nouvelleDatePeremption);

                        int result = updateCmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            GestionBDD.AfficherMessageSucces("Ingrédient modifié avec succès !");
                        }
                        else
                        {
                            GestionBDD.AfficherMessageErreur("Échec de la modification de l'ingrédient.");
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
        /// Supprime un ingrédient
        /// </summary>
        private static void SupprimerIngredient()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("SUPPRIMER UN INGRÉDIENT");

            Console.Write("ID de l'ingrédient à supprimer : ");
            string idIngredient = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    /// Vérifier si l'ingrédient existe
                    string checkQuery = "SELECT nom_ingredient FROM INGREDIENT WHERE id_ingredient = @IdIngredient";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@IdIngredient", idIngredient);
                    object nomIngredient = checkCmd.ExecuteScalar();

                    if (nomIngredient == null)
                    {
                        GestionBDD.AfficherMessageErreur("Ingrédient introuvable.");
                        return;
                    }

                    /// Vérifier si l'ingrédient est utilisé dans des plats
                    string checkUsageQuery = "SELECT COUNT(*) FROM contient WHERE id_ingredient = @IdIngredient";
                    MySqlCommand checkUsageCmd = new MySqlCommand(checkUsageQuery, connection);
                    checkUsageCmd.Parameters.AddWithValue("@IdIngredient", idIngredient);
                    int usageCount = Convert.ToInt32(checkUsageCmd.ExecuteScalar());

                    if (usageCount > 0)
                    {
                        Console.WriteLine($"\nL'ingrédient '{nomIngredient}' est utilisé dans {usageCount} plat(s).");
                        Console.WriteLine("Vous ne pouvez pas le supprimer sans d'abord le retirer de tous les plats.");
                        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        return;
                    }

                    Console.WriteLine($"\nVous êtes sur le point de supprimer l'ingrédient : {nomIngredient}");
                    Console.Write("Êtes-vous sûr ? (O/N) : ");
                    if (Console.ReadLine().ToUpper() != "O")
                    {
                        Console.WriteLine("Suppression annulée.");
                        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        return;
                    }

                    string deleteQuery = "DELETE FROM INGREDIENT WHERE id_ingredient = @IdIngredient";
                    MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, connection);
                    deleteCmd.Parameters.AddWithValue("@IdIngredient", idIngredient);

                    int result = deleteCmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        GestionBDD.AfficherMessageSucces("Ingrédient supprimé avec succès !");
                    }
                    else
                    {
                        GestionBDD.AfficherMessageErreur("Échec de la suppression de l'ingrédient.");
                    }
                }
            }
            catch (Exception ex)
            {
                GestionBDD.AfficherMessageErreur($"Erreur : {ex.Message}");
            }
        }

        #endregion

        #region Méthodes utilitaires

        /// <summary>
        /// Vérifie si un plat appartient au cuisinier connecté
        /// </summary>
        private static bool PlatAppartientAuCuisinier(string idPlat, string idCuisinier)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM PLAT WHERE id_plat = @IdPlat AND id_cuisinier = @IdCuisinier";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdPlat", idPlat);
                    cmd.Parameters.AddWithValue("@IdCuisinier", idCuisinier);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la vérification du plat : {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Vérifie si un plat est utilisé dans des commandes
        /// </summary>
        private static bool PlatUtiliseDansCommandes(string idPlat)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM LIGNE_COMMANDE WHERE id_plat = @IdPlat";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdPlat", idPlat);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la vérification des commandes : {ex.Message}");
                return true; // Par sécurité, on considère que le plat est utilisé en cas d'erreur
            }
        }

        #endregion

        #region Export XML
        /// <summary>
        /// Exporte la liste des plats au format XML
        /// </summary>
        public static void ExporterListePlats()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("EXPORTER LA LISTE DES PLATS");

            try
            {
                ExportPlats exportPlats = new ExportPlats();

                /// Récupérer tous les plats
                string requetePlats = @"
            SELECT p.id_plat, p.nom_plat, p.type_plat, p.nationalite_plat, p.prix_unitaire_plat, 
                   p.portion_dispo_plat, p.description_plat,
                   c.id_cuisinier, c.nom_cuisinier, c.prenom_cuisinier, c.note_moyenne_cuisinier
            FROM PLAT p
            JOIN CUISINIER c ON p.id_cuisinier = c.id_cuisinier
            ORDER BY p.type_plat, p.nom_plat";

                DataTable platsData = GestionBDD.ExecuterRequeteTable(requetePlats, null);

                if (platsData != null && platsData.Rows.Count > 0)
                {
                    exportPlats.NombrePlats = platsData.Rows.Count;

                    foreach (DataRow row in platsData.Rows)
                    {
                        ExportPlats.Plat plat = new ExportPlats.Plat
                        {
                            IdPlat = row["id_plat"].ToString(),
                            NomPlat = row["nom_plat"] != DBNull.Value ? row["nom_plat"].ToString() : "",
                            TypePlat = row["type_plat"] != DBNull.Value ? row["type_plat"].ToString() : "",
                            Nationalite = row["nationalite_plat"] != DBNull.Value ? row["nationalite_plat"].ToString() : "",
                            Prix = row["prix_unitaire_plat"] != DBNull.Value ? row["prix_unitaire_plat"].ToString() : "0",
                            PortionsDisponibles = row["portion_dispo_plat"] != DBNull.Value ? row["portion_dispo_plat"].ToString() : "0",
                            Description = row["description_plat"] != DBNull.Value ? row["description_plat"].ToString() : "",
                            IdCuisinier = row["id_cuisinier"].ToString(),
                            NomCuisinier = row["nom_cuisinier"] != DBNull.Value ? row["nom_cuisinier"].ToString() : "",
                            PrenomCuisinier = row["prenom_cuisinier"] != DBNull.Value ? row["prenom_cuisinier"].ToString() : "",
                            NoteCuisinier = row["note_moyenne_cuisinier"] != DBNull.Value ? Convert.ToDecimal(row["note_moyenne_cuisinier"]) : 0
                        };

                        string requeteIngredients = @"
                    SELECT i.nom_ingredient, i.categorie_ingredient, i.origine_ingredient
                    FROM CONTIENT c
                    JOIN INGREDIENT i ON c.id_ingredient = i.id_ingredient
                    WHERE c.id_plat = @IdPlat";

                        Dictionary<string, object> parametresIngredients = new Dictionary<string, object>
                {
                    { "@IdPlat", plat.IdPlat }
                };

                        DataTable ingredientsData = GestionBDD.ExecuterRequeteTable(requeteIngredients, parametresIngredients);

                        if (ingredientsData != null && ingredientsData.Rows.Count > 0)
                        {
                            foreach (DataRow ingredientRow in ingredientsData.Rows)
                            {
                                ExportPlats.Plat.Ingredient ingredient = new ExportPlats.Plat.Ingredient
                                {
                                    Nom = ingredientRow["nom_ingredient"] != DBNull.Value ? ingredientRow["nom_ingredient"].ToString() : "",
                                    Categorie = ingredientRow["categorie_ingredient"] != DBNull.Value ? ingredientRow["categorie_ingredient"].ToString() : "",
                                    Origine = ingredientRow["origine_ingredient"] != DBNull.Value ? ingredientRow["origine_ingredient"].ToString() : ""
                                };

                                plat.Ingredients.Add(ingredient);
                            }
                        }

                        try
                        {
                            string requeteVerifTable = @"
                        SELECT COUNT(*) 
                        FROM information_schema.tables 
                        WHERE table_schema = 'livinparis2' 
                        AND table_name = 'concerne'";

                            DataTable verifTable = GestionBDD.ExecuterRequeteTable(requeteVerifTable, null);
                            bool tableConcerneExiste = false;

                            if (verifTable != null && verifTable.Rows.Count > 0)
                            {
                                tableConcerneExiste = Convert.ToInt32(verifTable.Rows[0][0]) > 0;
                            }

                            if (tableConcerneExiste)
                            {
                                string requeteEvaluations = @"
                            SELECT e.id_evaluation, e.note_evaluation, e.commentaire_evaluation, e.date_evaluation
                            FROM EVALUATION e
                            JOIN concerne c ON e.id_evaluation = c.id_evaluation
                            WHERE c.id_plat = @IdPlat
                            ORDER BY e.date_evaluation DESC";

                                Dictionary<string, object> parametresEvaluations = new Dictionary<string, object>
                        {
                            { "@IdPlat", plat.IdPlat }
                        };

                                DataTable evaluationsData = GestionBDD.ExecuterRequeteTable(requeteEvaluations, parametresEvaluations);

                                if (evaluationsData != null && evaluationsData.Rows.Count > 0)
                                {
                                    foreach (DataRow evalRow in evaluationsData.Rows)
                                    {
                                        ExportPlats.Plat.Evaluation evaluation = new ExportPlats.Plat.Evaluation
                                        {
                                            Note = evalRow["note_evaluation"] != DBNull.Value ? Convert.ToInt32(evalRow["note_evaluation"]) : 0,
                                            Commentaire = evalRow["commentaire_evaluation"] != DBNull.Value ? evalRow["commentaire_evaluation"].ToString() : "",
                                            Date = evalRow["date_evaluation"] != DBNull.Value ? Convert.ToDateTime(evalRow["date_evaluation"]) : DateTime.MinValue
                                        };

                                        plat.Evaluations.Add(evaluation);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            /// Si une erreur se produit lors de la récupération des évaluations, on les ignore simplement
                            Console.WriteLine($"Note: Impossible de récupérer les évaluations: {ex.Message}");
                        }

                        exportPlats.ListePlats.Add(plat);
                    }

                    /// Exporter les plats en XML
                    string nomFichier = $"ListePlats_{DateTime.Now:yyyyMMdd_HHmmss}.xml";
                    string cheminFichier = ExportXML.ExporterDonnees(exportPlats, nomFichier);

                    if (cheminFichier != null)
                    {
                        GestionBDD.AfficherMessageSucces($"La liste des plats a été exportée avec succès dans le fichier : {cheminFichier}");

                        Console.Write("\nVoulez-vous ouvrir le fichier XML ? (O/N) : ");
                        if (Console.ReadLine().ToUpper() == "O")
                        {
                            ExportXML.OuvrirFichierXML(cheminFichier);
                        }
                    }
                    else
                    {
                        GestionBDD.AfficherMessageErreur("Erreur lors de l'exportation de la liste des plats.");
                    }
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Aucun plat trouvé dans la base de données.");
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

        #region Export JSON
        /// <summary>
        /// Exporte la liste des plats au format JSON
        /// </summary>
        public static void ExporterListePlatsJSON()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("EXPORTER LA LISTE DES PLATS EN JSON");

            try
            {
                /// Réutiliser la même structure de données que pour l'exportation XML
                ExportPlats exportPlats = new ExportPlats();
                exportPlats.DateExport = DateTime.Now;

                /// Récupérer tous les plats
                string requetePlats = @"
            SELECT p.id_plat, p.nom_plat, p.type_plat, p.nationalite_plat, p.prix_unitaire_plat, 
                   p.portion_dispo_plat, p.description_plat,
                   c.id_cuisinier, c.nom_cuisinier, c.prenom_cuisinier, c.note_moyenne_cuisinier
            FROM PLAT p
            JOIN CUISINIER c ON p.id_cuisinier = c.id_cuisinier
            ORDER BY p.type_plat, p.nom_plat";

                DataTable platsData = GestionBDD.ExecuterRequeteTable(requetePlats, null);

                if (platsData != null && platsData.Rows.Count > 0)
                {
                    exportPlats.NombrePlats = platsData.Rows.Count;

                    foreach (DataRow row in platsData.Rows)
                    {
                        ExportPlats.Plat plat = new ExportPlats.Plat
                        {
                            IdPlat = row["id_plat"].ToString(),
                            NomPlat = row["nom_plat"] != DBNull.Value ? row["nom_plat"].ToString() : "",
                            TypePlat = row["type_plat"] != DBNull.Value ? row["type_plat"].ToString() : "",
                            Nationalite = row["nationalite_plat"] != DBNull.Value ? row["nationalite_plat"].ToString() : "",
                            Prix = row["prix_unitaire_plat"] != DBNull.Value ? row["prix_unitaire_plat"].ToString() : "0",
                            PortionsDisponibles = row["portion_dispo_plat"] != DBNull.Value ? row["portion_dispo_plat"].ToString() : "0",
                            Description = row["description_plat"] != DBNull.Value ? row["description_plat"].ToString() : "",
                            IdCuisinier = row["id_cuisinier"].ToString(),
                            NomCuisinier = row["nom_cuisinier"] != DBNull.Value ? row["nom_cuisinier"].ToString() : "",
                            PrenomCuisinier = row["prenom_cuisinier"] != DBNull.Value ? row["prenom_cuisinier"].ToString() : "",
                            NoteCuisinier = row["note_moyenne_cuisinier"] != DBNull.Value ? Convert.ToDecimal(row["note_moyenne_cuisinier"]) : 0
                        };

                        /// Récupérer les ingrédients pour ce plat
                        string requeteIngredients = @"
                    SELECT i.nom_ingredient, i.categorie_ingredient, i.origine_ingredient
                    FROM CONTIENT c
                    JOIN INGREDIENT i ON c.id_ingredient = i.id_ingredient
                    WHERE c.id_plat = @IdPlat";

                        Dictionary<string, object> parametresIngredients = new Dictionary<string, object>
                {
                    { "@IdPlat", plat.IdPlat }
                };

                        DataTable ingredientsData = GestionBDD.ExecuterRequeteTable(requeteIngredients, parametresIngredients);

                        if (ingredientsData != null && ingredientsData.Rows.Count > 0)
                        {
                            foreach (DataRow ingredientRow in ingredientsData.Rows)
                            {
                                ExportPlats.Plat.Ingredient ingredient = new ExportPlats.Plat.Ingredient
                                {
                                    Nom = ingredientRow["nom_ingredient"] != DBNull.Value ? ingredientRow["nom_ingredient"].ToString() : "",
                                    Categorie = ingredientRow["categorie_ingredient"] != DBNull.Value ? ingredientRow["categorie_ingredient"].ToString() : "",
                                    Origine = ingredientRow["origine_ingredient"] != DBNull.Value ? ingredientRow["origine_ingredient"].ToString() : ""
                                };

                                plat.Ingredients.Add(ingredient);
                            }
                        }

                        try
                        {
                            string requeteEvaluations = @"
                        SELECT e.id_evaluation, e.note_evaluation, e.commentaire_evaluation, e.date_evaluation
                        FROM EVALUATION e
                        JOIN concerne c ON e.id_evaluation = c.id_evaluation
                        WHERE c.id_plat = @IdPlat
                        ORDER BY e.date_evaluation DESC";

                            Dictionary<string, object> parametresEvaluations = new Dictionary<string, object>
                    {
                        { "@IdPlat", plat.IdPlat }
                    };

                            DataTable evaluationsData = GestionBDD.ExecuterRequeteTable(requeteEvaluations, parametresEvaluations);

                            if (evaluationsData != null && evaluationsData.Rows.Count > 0)
                            {
                                foreach (DataRow evalRow in evaluationsData.Rows)
                                {
                                    ExportPlats.Plat.Evaluation evaluation = new ExportPlats.Plat.Evaluation
                                    {
                                        Note = evalRow["note_evaluation"] != DBNull.Value ? Convert.ToInt32(evalRow["note_evaluation"]) : 0,
                                        Commentaire = evalRow["commentaire_evaluation"] != DBNull.Value ? evalRow["commentaire_evaluation"].ToString() : "",
                                        Date = evalRow["date_evaluation"] != DBNull.Value ? Convert.ToDateTime(evalRow["date_evaluation"]) : DateTime.MinValue
                                    };

                                    plat.Evaluations.Add(evaluation);
                                }
                            }
                        }
                        catch
                        {
                        }

                        exportPlats.ListePlats.Add(plat);
                    }

                    /// Exporter les plats en JSON
                    string nomFichier = $"ListePlats_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                    string cheminFichier = ExportJson.ExporterDonnees(exportPlats, nomFichier);

                    if (cheminFichier != null)
                    {
                        GestionBDD.AfficherMessageSucces($"La liste des plats a été exportée avec succès dans le fichier : {cheminFichier}");

                        Console.Write("\nVoulez-vous ouvrir le fichier JSON ? (O/N) : ");
                        if (Console.ReadLine().ToUpper() == "O")
                        {
                            ExportJson.OuvrirFichierJSON(cheminFichier);
                        }
                    }
                    else
                    {
                        GestionBDD.AfficherMessageErreur("Erreur lors de l'exportation de la liste des plats.");
                    }
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Aucun plat trouvé dans la base de données.");
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
