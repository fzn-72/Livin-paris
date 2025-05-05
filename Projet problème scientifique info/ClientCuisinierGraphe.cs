using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using MySql.Data.MySqlClient;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Classe représentant le graphe des relations entre clients et cuisiniers
    /// </summary>
    internal class ClientCuisinierGraphe
    {
        //private static string connectionString = "Server=localhost;Database=livinparis2;Uid=root;Pwd=REDACTED;";
        private static string connectionString = "Server=localhost;Database=livinparis2;Uid=root;Pwd=REDACTED;";

        private Graphe<string> graphe;
        private Dictionary<string, string> typesNoeuds;
        private Dictionary<string, string> nomsNoeuds;  
        private Dictionary<string, int> couleurs;   

        /// <summary>
        /// Constructeur de la classe
        /// </summary>
        public ClientCuisinierGraphe()
        {
            graphe = new Graphe<string>();
            typesNoeuds = new Dictionary<string, string>();
            nomsNoeuds = new Dictionary<string, string>();
            couleurs = new Dictionary<string, int>();
        }

        /// <summary>
        /// Propriété permettant d'accéder au graphe sous-jacent.
        /// </summary>
        public Graphe<string> Graphe
        {
            get { return graphe; }
        }

        /// <summary>
        /// Propriété permettant d'accéder aux types des nœuds.
        /// </summary>
        public Dictionary<string, string> TypesNoeuds
        {
            get { return typesNoeuds; }
        }

        /// <summary>
        /// Propriété permettant d'accéder aux noms des nœuds.
        /// </summary>
        public Dictionary<string, string> NomsNoeuds
        {
            get { return nomsNoeuds; }
        }

        /// <summary>
        /// Propriété permettant d'accéder aux couleurs des nœuds.
        /// </summary>
        public Dictionary<string, int> Couleurs
        {
            get { return couleurs; }
        }

        #region Construction du graphe 
        /// <summary>
        /// Construit le graphe des relations entre clients et cuisiniers
        /// </summary>
        /// <returns>True si le graphe a été construit avec succès</returns>
        public bool ConstruireGraphe()
        {
            try
            {
                Console.WriteLine("Construction du graphe des relations clients-cuisiniers...");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string requete = @"
                        SELECT DISTINCT c.id_client, c.nom_client, c.prenom_client, 
                                        cu.id_cuisinier, cu.nom_cuisinier, cu.prenom_cuisinier
                        FROM COMMANDE cmd
                        JOIN CLIENT c ON cmd.id_client = c.id_client
                        JOIN LIGNE_COMMANDE lc ON cmd.id_commande = lc.id_commande
                        JOIN CUISINIER cu ON lc.id_cuisinier = cu.id_cuisinier
                        WHERE cmd.statut_commande = 'Livrée'";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string idClient = "C" + reader["id_client"].ToString();
                            string nomClient = reader["nom_client"].ToString() + " " + reader["prenom_client"].ToString();

                            string idCuisinier = "K" + reader["id_cuisinier"].ToString();
                            string nomCuisinier = reader["nom_cuisinier"].ToString() + " " + reader["prenom_cuisinier"].ToString();

                            if (!typesNoeuds.ContainsKey(idClient))
                            {
                                typesNoeuds[idClient] = "client";
                                nomsNoeuds[idClient] = nomClient;
                            }

                            if (!typesNoeuds.ContainsKey(idCuisinier))
                            {
                                typesNoeuds[idCuisinier] = "cuisinier";
                                nomsNoeuds[idCuisinier] = nomCuisinier;
                            }

                            graphe.AjouterLien(idClient, idCuisinier);
                            Console.WriteLine($"Ajout d'une arête entre {nomClient} (client) et {nomCuisinier} (cuisinier)");
                        }
                    }
                }

                Console.WriteLine($"Graphe construit avec succès : {typesNoeuds.Count} nœuds et {graphe.LiensNoeuds.Count} arêtes.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la construction du graphe : {ex.Message}");
                return false;
            }
        }
        #endregion

        #region Algo de WelshPowell pour la coloration
        /// <summary>
        /// Applique l'algorithme de Welsh-Powell pour colorier le graphe.
        /// </summary>
        /// <returns>Le nombre de couleurs utilisées.</returns>
        public int AlgoWelshPowell()
        {
            Console.WriteLine("Application de l'algorithme de Welsh-Powell...");

            couleurs.Clear();

            foreach (var noeud in graphe.ListeAdjacence.Keys)
            {
                couleurs[noeud] = -1;
            }

            List<string> noeudsTries = graphe.TrierSommetsParDegre();

            Console.WriteLine("Nœuds triés par degré décroissant :");
            foreach (var noeud in noeudsTries)
            {
                int degre = graphe.ListeAdjacence[noeud].Count;
                Console.WriteLine($"Nœud {noeud} ({nomsNoeuds[noeud]}) - Degré : {degre}");
            }

            int couleurActuelle = 0;

            while (noeudsTries.Any(n => couleurs[n] == -1))
            {
                Console.WriteLine($"Traitement de la couleur {couleurActuelle}");

                foreach (var noeud in noeudsTries)
                {
                    if (couleurs[noeud] == -1)
                    {

                        bool peutColorer = true;

                        foreach (var voisin in graphe.ListeAdjacence[noeud])
                        {
                            if (couleurs.ContainsKey(voisin) && couleurs[voisin] == couleurActuelle)
                            {
                                peutColorer = false;
                                Console.WriteLine($"  Nœud {noeud} ({nomsNoeuds[noeud]}) ne peut pas être coloré en {couleurActuelle} car voisin de {voisin} ({nomsNoeuds[voisin]})");
                                break;
                            }
                        }
                        if (peutColorer)
                        {
                            couleurs[noeud] = couleurActuelle;
                            Console.WriteLine($"  Nœud {noeud} ({nomsNoeuds[noeud]}) coloré en {couleurActuelle}");
                        }
                    }
                }

                couleurActuelle++;
            }

            int erreurs = 0;
            foreach (var lien in graphe.LiensNoeuds)
            {
                string id1 = lien.Noeud1.Id;
                string id2 = lien.Noeud2.Id;

                if (couleurs[id1] == couleurs[id2])
                {
                    erreurs++;
                    Console.WriteLine($"ERREUR: Nœuds voisins avec même couleur: {id1} ({nomsNoeuds[id1]}) et {id2} ({nomsNoeuds[id2]}) - Couleur: {couleurs[id1]}");
                }
            }

            Console.WriteLine($"Coloration terminée. {couleurActuelle} couleurs utilisées. {erreurs} erreurs détectées.");

            return couleurActuelle;
        }
        #endregion

        #region Vérifie si le graphe est planaire et biparti
        /// <summary>
        /// Vérifie si le graphe est biparti.
        /// Un graphe est biparti si ses sommets peuvent être divisés en deux ensembles disjoints
        /// tels que chaque arête relie un sommet d'un ensemble à un sommet de l'autre ensemble.
        /// </summary>
        /// <returns>True si le graphe est biparti, False sinon.</returns>
        public bool EstBiparti()
        {
            Console.WriteLine("Vérification si le graphe est biparti...");

            /// Un graphe est biparti si et seulement si il peut être colorié avec 2 couleurs
            /// Nous allons essayer de colorier le graphe avec 2 couleurs (0 et 1)

            Dictionary<string, int> coloration = new Dictionary<string, int>();

            foreach (var noeud in graphe.ListeAdjacence.Keys)
            {
                coloration[noeud] = -1;
            }

            foreach (var noeud in graphe.ListeAdjacence.Keys)
            {
                if (coloration[noeud] == -1)
                {
                    coloration[noeud] = 0;

                    Queue<string> file = new Queue<string>();   // Utiliser un parcours en largeur (DFS) pour colorier les nœuds
                    file.Enqueue(noeud);

                    while (file.Count > 0)
                    {
                        string noeudCourant = file.Dequeue();
                        int couleurCourante = coloration[noeudCourant];
                        int couleurVoisin = 1 - couleurCourante;

                        foreach (var voisin in graphe.ListeAdjacence[noeudCourant])
                        {
                            if (coloration[voisin] == -1) /// Si le voisin n'est pas encore coloré
                            {
                                coloration[voisin] = couleurVoisin;
                                file.Enqueue(voisin);
                            }
                            else if (coloration[voisin] != couleurVoisin) /// Si le voisin a déjà une couleur différente de celle attendue
                            {
                                Console.WriteLine($"Le graphe n'est pas biparti : conflit entre {noeudCourant} (couleur {couleurCourante}) et {voisin} (couleur {coloration[voisin]})");
                                return false;
                            }
                        }
                    }
                }
            }

            bool clientsMemeCouleur = true;
            bool cuisiniersMemeCouleur = true;

            int? couleurClients = null;
            int? couleurCuisiniers = null;

            foreach (var noeud in typesNoeuds.Keys)
            {
                if (typesNoeuds[noeud] == "client")
                {
                    if (couleurClients == null)
                    {
                        couleurClients = coloration[noeud];
                    }
                    else if (coloration[noeud] != couleurClients)
                    {
                        clientsMemeCouleur = false;
                    }
                }
                else if (typesNoeuds[noeud] == "cuisinier")
                {
                    if (couleurCuisiniers == null)
                    {
                        couleurCuisiniers = coloration[noeud];
                    }
                    else if (coloration[noeud] != couleurCuisiniers)
                    {
                        cuisiniersMemeCouleur = false;
                    }
                }
            }

            bool estBiparti = clientsMemeCouleur && cuisiniersMemeCouleur && couleurClients != couleurCuisiniers;

            if (estBiparti)
            {
                Console.WriteLine("Le graphe est biparti : tous les clients ont une couleur et tous les cuisiniers ont l'autre couleur.");
            }
            else
            {
                Console.WriteLine("Le graphe n'est pas biparti selon la vérification des types de nœuds.");
            }

            return estBiparti;
        }

        /// <summary>
        /// Vérifie si le graphe est planaire.
        /// Un graphe est planaire s'il peut être dessiné sur un plan sans que ses arêtes ne se croisent.
        /// </summary>
        /// <returns>True si le graphe est planaire, False sinon.</returns>
        public bool EstPlanaire()
        {
            Console.WriteLine("Vérification si le graphe est planaire...");

            int n = graphe.ListeAdjacence.Count;
            int m = graphe.LiensNoeuds.Count;

            Console.WriteLine($"Nombre de nœuds : {n}");
            Console.WriteLine($"Nombre d'arêtes : {m}");

            if (n < 3)   /// Vérifier la formule d'Euler pour les graphes planaires : m ≤ 3n - 6 pour n ≥ 3
            {
                Console.WriteLine("Le graphe a moins de 3 nœuds, il est donc planaire.");
                return true;
            }

            int maxAretes = 3 * n - 6;
            bool estPlanaire = m <= maxAretes;

            if (estPlanaire)
            {
                Console.WriteLine($"Le graphe est potentiellement planaire : {m} ≤ 3*{n}-6 = {maxAretes}");
            }
            else
            {
                Console.WriteLine($"Le graphe n'est pas planaire : {m} > 3*{n}-6 = {maxAretes}");
            }

            Console.WriteLine("Note : La vérification complète selon le théorème de Kuratowski n'est pas implémentée.");

            return estPlanaire;
        }
        #endregion

        #region Identification de groupes indépendants
        /// <summary>
        /// Identifie les groupes indépendants à partir des résultats de la coloration.
        /// Un groupe indépendant est un ensemble de nœuds tel qu'aucun des nœuds de cet ensemble ne sont adjacents.
        /// </summary>
        /// <returns>Une liste de groupes indépendants, chaque groupe étant une liste d'identifiants de nœuds.</returns>
        public List<List<string>> IdentifierGroupesIndependants()
        {
            Console.WriteLine("Identification des groupes indépendants...");

            /// Vérifier que la coloration a été effectuée
            if (couleurs.Count == 0)
            {
                Console.WriteLine("La coloration n'a pas été effectuée. Exécution de l'algorithme de Welsh-Powell...");
                AlgoWelshPowell();
            }

            /// Créer un groupe pour chaque couleur
            Dictionary<int, List<string>> groupesParCouleur = new Dictionary<int, List<string>>();

            foreach (var noeud in couleurs.Keys)
            {
                int couleur = couleurs[noeud];

                if (!groupesParCouleur.ContainsKey(couleur))
                {
                    groupesParCouleur[couleur] = new List<string>();
                }

                groupesParCouleur[couleur].Add(noeud);
            }

            /// Convertir en liste de groupes
            List<List<string>> groupesIndependants = new List<List<string>>();

            foreach (var groupe in groupesParCouleur.Values)
            {
                groupesIndependants.Add(groupe);

                Console.WriteLine($"Groupe indépendant (couleur {groupesParCouleur.Keys.ToList()[groupesIndependants.Count - 1]}) :");
                foreach (var noeud in groupe)
                {
                    Console.WriteLine($"  - {noeud} ({nomsNoeuds[noeud]}) : {typesNoeuds[noeud]}");
                }
            }

            List<List<string>> groupesClientsIndependants = new List<List<string>>();

            foreach (var groupe in groupesIndependants)
            {
                List<string> clientsGroupe = groupe.Where(noeud => typesNoeuds[noeud] == "client").ToList();

                if (clientsGroupe.Count > 0)
                {
                    groupesClientsIndependants.Add(clientsGroupe);

                    Console.WriteLine($"Groupe de clients indépendants :");
                    foreach (var client in clientsGroupe)
                    {
                        Console.WriteLine($"  - {client} ({nomsNoeuds[client]})");
                    }
                }
            }

            return groupesClientsIndependants;
        }
        #endregion

        #region Génération de l'image 
        /// <summary>
        /// Visualise le graphe en générant une image.
        /// </summary>
        /// <param name="cheminImage">Chemin où sauvegarder l'image générée.</param>
        /// <returns>True si la visualisation a réussi, False sinon.</returns>
        public bool VisualiserGraphe(string cheminImage)
        {
            try
            {
                Console.WriteLine("Visualisation du graphe...");

                /// Vérifier que le graphe n'est pas vide
                if (graphe.ListeAdjacence.Count == 0)
                {
                    Console.WriteLine("Le graphe est vide. Impossible de générer une visualisation.");
                    return false;
                }

                /// Vérifier que la coloration a été effectuée
                if (couleurs.Count == 0)
                {
                    Console.WriteLine("La coloration n'a pas été effectuée. Exécution de l'algorithme de Welsh-Powell...");
                    AlgoWelshPowell();
                }

                int largeur = 1200;  /// Paramètres de l'image
                int hauteur = 800;
                int margeHorizontale = 150;
                int margeVerticale = 100;

                List<string> clients = typesNoeuds.Where(kv => kv.Value == "client").Select(kv => kv.Key).ToList();    /// Séparer les clients et les cuisiniers
                List<string> cuisiniers = typesNoeuds.Where(kv => kv.Value == "cuisinier").Select(kv => kv.Key).ToList();

                Dictionary<string, Point> positions = new Dictionary<string, Point>();   /// Calculer les positions des nœuds

                int espaceVerticalClients = (hauteur - 2 * margeVerticale) / Math.Max(clients.Count, 1);
                for (int i = 0; i < clients.Count; i++)
                {
                    positions[clients[i]] = new Point(
                        margeHorizontale,
                        margeVerticale + i * espaceVerticalClients + espaceVerticalClients / 2
                    );
                }

                int espaceVerticalCuisiniers = (hauteur - 2 * margeVerticale) / Math.Max(cuisiniers.Count, 1);
                for (int i = 0; i < cuisiniers.Count; i++)
                {
                    positions[cuisiniers[i]] = new Point(
                        largeur - margeHorizontale,
                        margeVerticale + i * espaceVerticalCuisiniers + espaceVerticalCuisiniers / 2
                    );
                }

                using (Bitmap bitmap = new Bitmap(largeur, hauteur))
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    g.Clear(Color.White);

                    Color[] couleursGraphe = new Color[]
                    {
                        Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Purple,
                        Color.Orange, Color.Pink, Color.Cyan, Color.Magenta, Color.Brown
                    };

                    Pen penLien = new Pen(Color.Black, 2);   /// Dessiner les liens
                    foreach (var lien in graphe.LiensNoeuds)
                    {
                        Point p1 = positions[lien.Noeud1.Id];
                        Point p2 = positions[lien.Noeud2.Id];
                        g.DrawLine(penLien, p1, p2);
                    }

                    Font font = new Font("Arial", 10);  /// Dessiner les nœuds avec leur couleur
                    Brush brushTexte = Brushes.White;

                    foreach (var noeud in graphe.ListeAdjacence.Keys)
                    {
                        Point p = positions[noeud];

                        int couleurIndex = couleurs.ContainsKey(noeud) ? couleurs[noeud] % couleursGraphe.Length : 0;
                        Brush brushNoeud = new SolidBrush(couleursGraphe[couleurIndex]);

                        if (typesNoeuds.ContainsKey(noeud) && typesNoeuds[noeud] == "client")
                        {
                            g.FillEllipse(brushNoeud, p.X - 15, p.Y - 15, 30, 30);  /// Client : cercle
                        }
                        else
                        {
                            g.FillRectangle(brushNoeud, p.X - 15, p.Y - 15, 30, 30);  /// Cuisinier : carré
                        }

                        string nomAffiche = nomsNoeuds.ContainsKey(noeud) ? nomsNoeuds[noeud] : noeud.ToString();   /// Afficher le nom du nœud
                        SizeF tailleTexte = g.MeasureString(nomAffiche, font);

                        float xTexte = typesNoeuds[noeud] == "client" ? p.X - tailleTexte.Width - 20 : p.X + 20;   /// Positionner le texte à côté du nœud (à gauche pour les clients, à droite pour les cuisiniers)
                        float yTexte = p.Y - tailleTexte.Height / 2;

                        g.DrawString(nomAffiche, font, Brushes.Black, xTexte, yTexte);  /// Afficher l'identifiant du nœud au centre
                        g.DrawString(noeud.ToString(), font, brushTexte, p.X - 7, p.Y - 7);
                    }

                    int legendeX = 50;  /// Ajouter une légende
                    int legendeY = 50;
                    Font fontLegende = new Font("Arial", 12);
                    Brush brushTexteLegende = Brushes.Black;

                    g.DrawString("Légende :", fontLegende, brushTexteLegende, legendeX, legendeY);
                    legendeY += 25;

                    g.FillEllipse(Brushes.Gray, legendeX, legendeY, 20, 20);   /// Légende des types de nœuds
                    g.DrawString("Client", fontLegende, brushTexteLegende, legendeX + 30, legendeY);
                    legendeY += 30;

                    g.FillRectangle(Brushes.Gray, legendeX, legendeY, 20, 20);
                    g.DrawString("Cuisinier", fontLegende, brushTexteLegende, legendeX + 30, legendeY);
                    legendeY += 30;

                    for (int i = 0; i < Math.Min(couleursGraphe.Length, 5); i++)
                    {
                        Brush brushCouleur = new SolidBrush(couleursGraphe[i]);
                        g.FillRectangle(brushCouleur, legendeX, legendeY, 20, 20);
                        g.DrawString($"Couleur {i}", fontLegende, brushTexteLegende, legendeX + 30, legendeY);
                        legendeY += 30;
                    }

                    bitmap.Save(cheminImage, ImageFormat.Png);
                }

                Console.WriteLine($"Le graphe a été dessiné et sauvegardé sous '{cheminImage}'.");

                try   ///Ouvrir l'image
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(cheminImage) { UseShellExecute = true });
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de l'ouverture de l'image : {ex.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la visualisation du graphe : {ex.Message}");
                return false;
            }
        }
        #endregion
    }
}
