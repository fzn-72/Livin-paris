using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Projet_problème_scientifique_info
{
    internal class ExportJson
    {
        private static string dossierExport = @"C:\Users\fzzn7\Documents\GitHub\PSI_2025_ThoFaizMart\Fichiers JSON";

        /// <summary>
        /// Exporte des données au format JSON
        /// </summary>
        /// <param name="donnees">Objet à exporter</param>
        /// <param name="nomFichier">Nom du fichier de sortie</param>
        /// <returns>Chemin complet du fichier créé ou null en cas d'erreur</returns>
        public static string ExporterDonnees(object donnees, string nomFichier)
        {

            try
            {
                if (!Directory.Exists(dossierExport))
                {
                    Directory.CreateDirectory(dossierExport);
                }

                string cheminFichier = Path.Combine(dossierExport, nomFichier);

                StreamWriter writer = new StreamWriter(cheminFichier);
                JsonTextWriter jwriter = new JsonTextWriter(writer);
                jwriter.Formatting = Formatting.Indented; 

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jwriter, donnees);

                jwriter.Close();
                writer.Close();

                return cheminFichier;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'exportation JSON : {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Ouvre un fichier JSON avec l'application par défaut
        /// </summary>
        /// <param name="cheminFichier">Chemin du fichier à ouvrir</param>
        public static void OuvrirFichierJSON(string cheminFichier)
        {
            try
            {
                if (File.Exists(cheminFichier))
                {
                    System.Diagnostics.Process.Start(cheminFichier);
                }
                else
                {
                    Console.WriteLine("Le fichier n'existe pas.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ouverture du fichier : {ex.Message}");
            }
        }

        /// <summary>
        /// Lit et affiche le contenu d'un fichier JSON
        /// </summary>
        /// <param name="nomFichier">Nom du fichier à lire</param>
        public static void LectureJson(string nomFichier)
        {
            try
            {
                StreamReader reader = new StreamReader(nomFichier);
                JsonTextReader jreader = new JsonTextReader(reader);

                int compteurToken = 1;
                while (jreader.Read())
                {
                    Console.Write("Token n°" + (compteurToken++) + " => ");
                    if (jreader.Value != null)
                    {
                        Console.WriteLine(jreader.TokenType.ToString() + " : " + jreader.Value);
                    }
                    else
                    {
                        Console.WriteLine(jreader.TokenType.ToString() + " : ");
                    }
                }
                jreader.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la lecture du fichier JSON : {ex.Message}");
            }
        }

        /// <summary>
        /// Affiche le contenu d'un fichier JSON avec une mise en forme
        /// </summary>
        /// <param name="nomFichier">Nom du fichier à afficher</param>
        public static void AfficherPrettyJson(string nomFichier)
        {
            try
            {
                string json = File.ReadAllText(nomFichier);
                dynamic parsedJson = JsonConvert.DeserializeObject(json);
                string prettyJson = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
                Console.WriteLine(prettyJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'affichage du fichier JSON : {ex.Message}");
            }
        }

    }
}
