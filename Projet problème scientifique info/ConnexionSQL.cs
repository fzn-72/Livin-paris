using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Projet_problème_scientifique_info
{
    internal class ConnexionSQL
    {
        /// <summary>
        /// Méthode pour établir une connexion à la base de données MySQL.
        /// </summary>
        static void Connexion()
        {
            MySqlConnection maConnexion = null;
            try
            {
                /// <summary>
                /// Chaîne de connexion contenant les informations nécessaires pour se connecter à la base de données.
                /// </summary>
                string connexionString = "SERVER=localhost;PORT=3306;" +
                                         "DATABASE=livinparis;" +
                                         "UID=administrateur;PASSWORD=REDACTED";

                /// <summary>
                /// Création d'une nouvelle instance de MySqlConnection avec la chaîne de connexion.
                /// </summary>
                maConnexion = new MySqlConnection(connexionString);

                /// <summary>
                /// Ouverture de la connexion à la base de données.
                /// </summary>
                maConnexion.Open();
            }
            catch (MySqlException e)
            {
                /// <summary>
                /// Gestion des exceptions MySqlException qui peuvent survenir lors de la tentative de connexion.
                /// </summary>
                Console.WriteLine(" ErreurConnexion : " + e.ToString());
                return;
            }
        }
    }
}

