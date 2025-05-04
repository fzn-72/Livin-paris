using MySql.Data.MySqlClient;
using Projet_problème_scientifique_info;
using System;
using System.Collections.Generic;
using System.Data;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Classe gérant les fonctionnalités liées aux cuisiniers
    /// </summary>
    internal class ModuleCuisinier
    {
        /// <summary>
        /// Affiche le menu principal du module cuisinier avec les différentes options disponibles
        /// </summary>
        /// 
        public static void MenuPrincipalCuisinier()
        {
            // Vérifier que l'utilisateur est un cuisinier ou un administrateur
            if (!SessionUtilisateur.EstCuisinier() && !SessionUtilisateur.EstAdmin())
            {
                SessionUtilisateur.AfficherMessageDroitsInsuffisants();
                return;
            }

            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("MENU PRINCIPAL CUISINIER");

                if (SessionUtilisateur.EstCuisinier())
                {
                    Console.WriteLine($"Bienvenue {SessionUtilisateur.PrenomUtilisateur} {SessionUtilisateur.NomUtilisateur} !");
                    Console.WriteLine();
                }

                Console.WriteLine("1. Accéder au module de plats");
                Console.WriteLine("2. Gérer mon profil");
                Console.WriteLine("3. Accéder au module de commandes");
                Console.WriteLine("4. Afficher mes statistiques");
                Console.WriteLine("5. Se déconnecter");

                Console.Write("\nChoisissez une option : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        ModulePlat.MenuPrincipalPlats();
                        break;
                    case "2":
                        ModuleProfil.MenuPrincipalProfil();
                        break;
                    case "3":
                        ModuleCommande.MenuPrincipalCommandes();
                        break;
                    case "4":
                        ModuleStats.MenuPrincipalStats();
                        break;
                    case "5":
                        if (SessionUtilisateur.EstCuisinier())
                        {
                            SessionUtilisateur.Deconnecter();
                        }
                        continuer = false;
                        break;
                    default:
                        GestionBDD.AfficherMessageErreur("Option invalide.");
                        break;
                }
            }
        }
    }
}