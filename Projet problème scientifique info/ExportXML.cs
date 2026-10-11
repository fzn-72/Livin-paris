using System.IO;
using System.Xml.Serialization;
using System;

namespace Projet_problème_scientifique_info
{
    public static class ExportXML
    {
        private static string dossierExport = Path.GetFullPath(
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\Fichiers XML"
            )
        ); 
        private static readonly string cheminEditix = @"C:\Program Files (x86)\editix-free-xmleditor2010\bin\Editix-XMLEditor.exe";

        /// <summary>
        /// Exporte des données statistiques au format XML
        /// </summary>
        public static string ExporterDonnees<T>(T donnees, string nomFichier)
        {
            try
            {
                if (!Directory.Exists(dossierExport))
                {
                    Directory.CreateDirectory(dossierExport);
                }

                string cheminFichier = Path.Combine(dossierExport, nomFichier);

                XmlSerializer xs = new XmlSerializer(typeof(T));
                using (StreamWriter wr = new StreamWriter(cheminFichier))
                {
                    xs.Serialize(wr, donnees);
                }

                return cheminFichier;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'exportation XML : {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Ouvre un fichier XML avec EditiX
        /// </summary>
        public static void OuvrirFichierXML(string cheminFichier)
        {
            try
            {
                if (File.Exists(cheminFichier))
                {
                    System.Diagnostics.Process.Start(cheminEditix, $"\"{cheminFichier}\"");
                }
                else
                {
                    Console.WriteLine("Le fichier XML n'existe pas.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ouverture avec EditiX : {ex.Message}");
            }
        }
    }
}
