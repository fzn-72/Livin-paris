using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Projet_problème_scientifique_info;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Classe représentant le graphe des relations entre clients et cuisiniers
    /// </summary>
    internal class ClientCuisinierGraphe
    {
        private static string connectionString = "Server=localhost;Database=livinparis2;Uid=root;Pwd=REDACTED;";
        private Dictionary<string, HashSet<string>> graphe;

        /// <summary>
        /// Constructeur
        /// </summary>
        public ClientCuisinierGraphe()
        {
            graphe = new Dictionary<string, HashSet<string>>();
        }

        /// <summary>
        /// Construit le graphe des relations entre clients et cuisiniers
        /// </summary>
        /// <returns>True si le graphe a été construit avec succès</returns>
        public bool ConstruireGraphe()
        {
            try
            {
                // S'assurer que la vue existe
                GestionBDD.CreerVueRelationsClientCuisinier();

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Utiliser la vue pour récupérer les relations
                    string requete = @"
                        SELECT id_client, nom_client, prenom_client, 
                               id_cuisinier, nom_cuisinier, prenom_cuisinier
                        FROM VUE_RELATIONS_CLIENT_CUISINIER";

                    MySqlCommand cmd = new MySqlCommand(requete, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string idClient = reader["id_client"].ToString();
                            string nomClient = reader["nom_client"].ToString();
                            string prenomClient = reader["prenom_client"].ToString();
                            string clientLabel = $"{prenomClient} {nomClient} ({idClient})";

                            string idCuisinier = reader["id_cuisinier"].ToString();
                            string nomCuisinier = reader["nom_cuisinier"].ToString();
                            string prenomCuisinier = reader["prenom_cuisinier"].ToString();
                            string cuisinierLabel = $"{prenomCuisinier} {nomCuisinier} ({idCuisinier})";

                            // Ajouter la relation client -> cuisinier
                            if (!graphe.ContainsKey(clientLabel))
                            {
                                graphe[clientLabel] = new HashSet<string>();
                            }
                            graphe[clientLabel].Add(cuisinierLabel);

                            // Ajouter la relation cuisinier -> client (graphe non orienté)
                            if (!graphe.ContainsKey(cuisinierLabel))
                            {
                                graphe[cuisinierLabel] = new HashSet<string>();
                            }
                            graphe[cuisinierLabel].Add(clientLabel);
                        }
                    }
                }

                Console.WriteLine($"Graphe construit avec {graphe.Count} sommets.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la construction du graphe : {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Affiche le graphe
        /// </summary>
        public void AfficherGraphe()
        {
            if (graphe.Count == 0)
            {
                Console.WriteLine("Le graphe est vide. Veuillez d'abord le construire.");
                return;
            }

            Console.WriteLine("Graphe des relations clients-cuisiniers :");
            Console.WriteLine("==========================================");

            foreach (var sommet in graphe)
            {
                Console.WriteLine($"{sommet.Key} est connecté à :");
                foreach (var voisin in sommet.Value)
                {
                    Console.WriteLine($"  - {voisin}");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Recherche le plus court chemin entre deux sommets
        /// </summary>
        /// <param name="depart">Sommet de départ</param>
        /// <param name="arrivee">Sommet d'arrivée</param>
        /// <returns>Liste des sommets formant le chemin</returns>
        public List<string> RechercherChemin(string depart, string arrivee)
        {
            if (!graphe.ContainsKey(depart) || !graphe.ContainsKey(arrivee))
            {
                Console.WriteLine("Un des sommets n'existe pas dans le graphe.");
                return new List<string>();
            }

            // Utiliser l'algorithme BFS pour trouver le plus court chemin
            Dictionary<string, string> predecesseurs = new Dictionary<string, string>();
            Queue<string> file = new Queue<string>();
            HashSet<string> visites = new HashSet<string>();

            file.Enqueue(depart);
            visites.Add(depart);

            bool trouve = false;
            while (file.Count > 0 && !trouve)
            {
                string sommetCourant = file.Dequeue();

                if (sommetCourant == arrivee)
                {
                    trouve = true;
                    break;
                }

                foreach (var voisin in graphe[sommetCourant])
                {
                    if (!visites.Contains(voisin))
                    {
                        file.Enqueue(voisin);
                        visites.Add(voisin);
                        predecesseurs[voisin] = sommetCourant;
                    }
                }
            }

            // Reconstruire le chemin
            List<string> chemin = new List<string>();
            if (trouve)
            {
                string sommet = arrivee;
                while (sommet != depart)
                {
                    chemin.Add(sommet);
                    sommet = predecesseurs[sommet];
                }
                chemin.Add(depart);
                chemin.Reverse();
            }

            return chemin;
        }

        /// <summary>
        /// Affiche le chemin entre deux sommets
        /// </summary>
        /// <param name="depart">Sommet de départ</param>
        /// <param name="arrivee">Sommet d'arrivée</param>
        public void AfficherChemin(string depart, string arrivee)
        {
            List<string> chemin = RechercherChemin(depart, arrivee);

            if (chemin.Count == 0)
            {
                Console.WriteLine($"Aucun chemin trouvé entre {depart} et {arrivee}.");
                return;
            }

            Console.WriteLine($"Chemin le plus court entre {depart} et {arrivee} :");
            Console.WriteLine("==========================================");

            for (int i = 0; i < chemin.Count; i++)
            {
                Console.Write(chemin[i]);
                if (i < chemin.Count - 1)
                {
                    Console.Write(" -> ");
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Recherche tous les chemins entre deux sommets
        /// </summary>
        /// <param name="depart">Sommet de départ</param>
        /// <param name="arrivee">Sommet d'arrivée</param>
        /// <returns>Liste des chemins trouvés</returns>
        public List<List<string>> RechercherTousChemins(string depart, string arrivee)
        {
            List<List<string>> chemins = new List<List<string>>();
            if (!graphe.ContainsKey(depart) || !graphe.ContainsKey(arrivee))
            {
                return chemins;
            }

            HashSet<string> visites = new HashSet<string>();
            List<string> cheminCourant = new List<string>();
            cheminCourant.Add(depart);
            visites.Add(depart);

            RechercherTousCheminsRecursif(depart, arrivee, visites, cheminCourant, chemins);

            return chemins;
        }

        /// <summary>
        /// Méthode récursive pour rechercher tous les chemins
        /// </summary>
        private void RechercherTousCheminsRecursif(string sommetCourant, string arrivee, HashSet<string> visites, List<string> cheminCourant, List<List<string>> chemins)
        {
            if (sommetCourant == arrivee)
            {
                chemins.Add(new List<string>(cheminCourant));
                return;
            }

            foreach (var voisin in graphe[sommetCourant])
            {
                if (!visites.Contains(voisin))
                {
                    visites.Add(voisin);
                    cheminCourant.Add(voisin);

                    RechercherTousCheminsRecursif(voisin, arrivee, visites, cheminCourant, chemins);

                    visites.Remove(voisin);
                    cheminCourant.RemoveAt(cheminCourant.Count - 1);
                }
            }
        }

        /// <summary>
        /// Visualise le graphe et l'enregistre dans un fichier image
        /// </summary>
        /// <param name="cheminFichier">Chemin du fichier où enregistrer l'image</param>
        /// <returns>True si la visualisation a été générée avec succès</returns>
        public bool VisualiserGraphe(string cheminFichier)
        {
            try
            {
                if (graphe.Count == 0)
                {
                    Console.WriteLine("Le graphe est vide. Impossible de générer une visualisation.");
                    return false;
                }

                // Paramètres de visualisation
                int largeur = 2000;
                int hauteur = 1500;
                int margeImage = 50;
                float rayonNoeud = 20;
                Font fontNom = new Font("Arial", 10);
                Color couleurClient = Color.Blue;
                Color couleurCuisinier = Color.Red;
                Color couleurLien = Color.Green;

                // Créer l'image
                using (Bitmap bitmap = new Bitmap(largeur, hauteur))
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    g.Clear(Color.White);

                    // Séparer les clients et les cuisiniers
                    List<string> clients = new List<string>();
                    List<string> cuisiniers = new List<string>();

                    foreach (var sommet in graphe.Keys)
                    {
                        if (sommet.Contains("(CL"))
                        {
                            clients.Add(sommet);
                        }
                        else if (sommet.Contains("(CU"))
                        {
                            cuisiniers.Add(sommet);
                        }
                    }

                    // Calculer les positions des noeuds
                    Dictionary<string, PointF> positions = new Dictionary<string, PointF>();

                    // Positionner les clients en cercle sur la gauche
                    int nbClients = clients.Count;
                    double rayonCercleClients = Math.Min(hauteur, largeur / 2) * 0.4;
                    double centreCercleClientsX = largeur / 4;
                    double centreCercleClientsY = hauteur / 2;

                    for (int i = 0; i < nbClients; i++)
                    {
                        double angle = 2 * Math.PI * i / nbClients;
                        float x = (float)(centreCercleClientsX + rayonCercleClients * Math.Cos(angle));
                        float y = (float)(centreCercleClientsY + rayonCercleClients * Math.Sin(angle));
                        positions[clients[i]] = new PointF(x, y);
                    }

                    // Positionner les cuisiniers en cercle sur la droite
                    int nbCuisiniers = cuisiniers.Count;
                    double rayonCercleCuisiniers = Math.Min(hauteur, largeur / 2) * 0.4;
                    double centreCercleCuisiniersX = 3 * largeur / 4;
                    double centreCercleCuisiniersY = hauteur / 2;

                    for (int i = 0; i < nbCuisiniers; i++)
                    {
                        double angle = 2 * Math.PI * i / nbCuisiniers;
                        float x = (float)(centreCercleCuisiniersX + rayonCercleCuisiniers * Math.Cos(angle));
                        float y = (float)(centreCercleCuisiniersY + rayonCercleCuisiniers * Math.Sin(angle));
                        positions[cuisiniers[i]] = new PointF(x, y);
                    }

                    // Dessiner les liens
                    using (Pen penLien = new Pen(couleurLien, 1))
                    {
                        foreach (var client in clients)
                        {
                            foreach (var voisin in graphe[client])
                            {
                                if (cuisiniers.Contains(voisin))
                                {
                                    g.DrawLine(penLien, positions[client], positions[voisin]);
                                }
                            }
                        }
                    }

                    // Dessiner les noeuds clients
                    using (Brush brushClient = new SolidBrush(couleurClient))
                    using (Pen penClient = new Pen(couleurClient, 2))
                    {
                        foreach (var client in clients)
                        {
                            PointF pos = positions[client];
                            g.FillEllipse(brushClient, pos.X - rayonNoeud, pos.Y - rayonNoeud, 2 * rayonNoeud, 2 * rayonNoeud);
                            g.DrawEllipse(penClient, pos.X - rayonNoeud, pos.Y - rayonNoeud, 2 * rayonNoeud, 2 * rayonNoeud);

                            // Dessiner le nom du client
                            string nomCourt = client.Split('(')[0].Trim(); // Prendre juste le prénom et nom
                            SizeF tailleTexte = g.MeasureString(nomCourt, fontNom);
                            g.DrawString(nomCourt, fontNom, Brushes.Black, pos.X - tailleTexte.Width / 2, pos.Y + rayonNoeud + 5);
                        }
                    }

                    // Dessiner les noeuds cuisiniers
                    using (Brush brushCuisinier = new SolidBrush(couleurCuisinier))
                    using (Pen penCuisinier = new Pen(couleurCuisinier, 2))
                    {
                        foreach (var cuisinier in cuisiniers)
                        {
                            PointF pos = positions[cuisinier];
                            g.FillEllipse(brushCuisinier, pos.X - rayonNoeud, pos.Y - rayonNoeud, 2 * rayonNoeud, 2 * rayonNoeud);
                            g.DrawEllipse(penCuisinier, pos.X - rayonNoeud, pos.Y - rayonNoeud, 2 * rayonNoeud, 2 * rayonNoeud);

                            // Dessiner le nom du cuisinier
                            string nomCourt = cuisinier.Split('(')[0].Trim(); // Prendre juste le prénom et nom
                            SizeF tailleTexte = g.MeasureString(nomCourt, fontNom);
                            g.DrawString(nomCourt, fontNom, Brushes.Black, pos.X - tailleTexte.Width / 2, pos.Y + rayonNoeud + 5);
                        }
                    }

                    // Dessiner la légende
                    int xLegende = 50;
                    int yLegende = 50;
                    int espacementLegende = 30;

                    // Titre de la légende
                    using (Font fontTitre = new Font("Arial", 14, FontStyle.Bold))
                    {
                        g.DrawString("Légende", fontTitre, Brushes.Black, xLegende, yLegende);
                        yLegende += 30;
                    }

                    // Client
                    g.FillEllipse(new SolidBrush(couleurClient), xLegende, yLegende, 20, 20);
                    g.DrawString("Client", fontNom, Brushes.Black, xLegende + 30, yLegende + 5);
                    yLegende += espacementLegende;

                    // Cuisinier
                    g.FillEllipse(new SolidBrush(couleurCuisinier), xLegende, yLegende, 20, 20);
                    g.DrawString("Cuisinier", fontNom, Brushes.Black, xLegende + 30, yLegende + 5);
                    yLegende += espacementLegende;

                    // Relation
                    g.DrawLine(new Pen(couleurLien, 2), xLegende, yLegende + 10, xLegende + 20, yLegende + 10);
                    g.DrawString("Relation (commande livrée)", fontNom, Brushes.Black, xLegende + 30, yLegende + 5);

                    // Enregistrer l'image
                    bitmap.Save(cheminFichier, ImageFormat.Png);
                    Console.WriteLine($"Visualisation enregistrée sous '{cheminFichier}'");

                    // Ouvrir l'image
                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(cheminFichier) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de l'ouverture de l'image: {ex.Message}");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la génération de la visualisation: {ex.Message}");
                return false;
            }
        }
    }
}
