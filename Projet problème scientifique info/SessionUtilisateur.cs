using Projet_problème_scientifique_info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Classe statique qui gère les informations de l'utilisateur connecté
    /// </summary>
    public static class SessionUtilisateur
    {
        /// Propriétés de l'utilisateur connecté
        public static string IdUtilisateur { get; private set; } = "";
        public static string NomUtilisateur { get; private set; } = "";
        public static string PrenomUtilisateur { get; private set; } = "";
        public static string TypeUtilisateur { get; private set; } = ""; 
        public static string Email { get; private set; } = "";

        /// <summary>
        /// Initialise la session utilisateur
        /// </summary>
        public static void Initialiser()
        {
            IdUtilisateur = "";
            NomUtilisateur = "";
            PrenomUtilisateur = "";
            TypeUtilisateur = "";
            Email = "";
        }

        /// <summary>
        /// Vérifie si un utilisateur est connecté
        /// </summary>
        /// <returns>True si un utilisateur est connecté, sinon False</returns>
        public static bool EstConnecte()
        {
            return !string.IsNullOrEmpty(IdUtilisateur);
        }

        /// <summary>
        /// Connecte un utilisateur en définissant ses informations
        /// </summary>
        /// <param name="id">ID de l'utilisateur</param>
        /// <param name="nom">Nom complet de l'utilisateur</param>
        /// <param name="type">Type d'utilisateur (client, cuisinier, admin)</param>
        /// <param name="email">Email de l'utilisateur</param>
        public static void Connecter(string id, string nom, string prenom, string type, string email)
        {
            IdUtilisateur = id;
            NomUtilisateur = nom;
            PrenomUtilisateur = prenom;
            TypeUtilisateur = type.ToLower();
            Email = email;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Bienvenue {NomUtilisateur + " " + PrenomUtilisateur} !");
            Console.ResetColor();
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        /// <summary>
        /// Déconnecte l'utilisateur en effaçant ses informations
        /// </summary>
        public static void Deconnecter()
        {
            IdUtilisateur = "";
            NomUtilisateur = "";
            PrenomUtilisateur = "";
            TypeUtilisateur = "";
            Email = "";

            Console.Clear();
            Console.WriteLine("Vous avez été déconnecté.");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Merci d'avoir utilisé Liv'InParis. À bientôt !", ConsoleColor.DarkYellow);
            Console.ResetColor();
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        /// <summary>
        /// Vérifie si l'utilisateur connecté est un client
        /// </summary>
        /// <returns>True si l'utilisateur est un client, sinon False</returns>
        public static bool EstClient()
        {
            return EstConnecte() && TypeUtilisateur == "client";
        }

        /// <summary>
        /// Vérifie si l'utilisateur connecté est un cuisinier
        /// </summary>
        /// <returns>True si l'utilisateur est un cuisinier, sinon False</returns>
        public static bool EstCuisinier()
        {
            return EstConnecte() && TypeUtilisateur == "cuisinier";
        }

        /// <summary>
        /// Vérifie si l'utilisateur connecté est un administrateur
        /// </summary>
        /// <returns>True si l'utilisateur est un administrateur, sinon False</returns>
        public static bool EstAdmin()
        {
            return EstConnecte() && TypeUtilisateur == "admin";
        }

        /// <summary>
        /// Vérifie si l'utilisateur a les droits pour accéder à une fonctionnalité
        /// </summary>
        /// <param name="typesAutorises">Types d'utilisateurs autorisés</param>
        /// <returns>True si l'utilisateur a les droits, False sinon</returns>
        public static bool VerifierDroits(params string[] typesAutorises)
        {
            if (!EstConnecte())
            {
                AfficherMessageDroitsInsuffisants();
                return false;
            }

            if (typesAutorises.Contains(TypeUtilisateur.ToLower()))
            {
                return true;
            }

            AfficherMessageDroitsInsuffisants();
            return false;
        }

        /// <summary>
        /// Affiche un message d'erreur si l'utilisateur n'a pas les droits nécessaires
        /// </summary>
        public static void AfficherMessageDroitsInsuffisants()
        {
            GestionBDD.AfficherMessageErreur("Vous n'avez pas les droits nécessaires pour accéder à cette fonctionnalité.");
        }
    }
}
