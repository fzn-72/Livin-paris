using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_problème_scientifique_info
{
    public class ModuleInfosAppli
    {
        #region Menu principal de la classe InfosAppli
        public static void MenuPrincipalInfos()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                GestionBDD.AfficherTitre("MENU INFOS");
                Console.WriteLine("Que souhaitez-vous faire ?");
                Console.WriteLine("\n1. Obtenir un mot sur l'application");
                Console.WriteLine("2. Voir le mode d'emploi de l'application");
                Console.WriteLine("3. Voir la fiche contacts");
                Console.WriteLine("4. Évaluer l'application");
                Console.WriteLine("5. Retour au menu principal");
                Console.Write("\nChoisissez une option : ");

                string choix = Console.ReadLine();
                switch (choix)
                {
                    case "1":
                        MotSurAppli();
                        break;
                    case "2":
                        FonctionnementAppli();
                        break;
                    case "3":
                        InformationsContact();
                        break;
                    case "4":
                        EvaluerAppli();
                        break;
                    case "5":
                        continuer = false;
                        break;
                    default:
                        GestionBDD.AfficherMessageErreur("Option invalide.");
                        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                        break;
                }
            }
        }
        #endregion


        #region Présente qq infos sur l'application
        /// <summary>
        /// Affiche des informations sur l'application
        /// </summary>
        static void MotSurAppli()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("À PROPOS DE L'APPLICATION");

            /// Affichage avec mise en forme pour améliorer la lisibilité
            AfficherTexteFormate("Liv'InParis - Système de gestion de livraison de repas");
            AfficherTexteFormate("Version 3.0", true);

            AfficherTexteFormate("Cette application permet de gérer un service de livraison de repas à domicile. Elle propose " +
                "des services de partages de repas entre voisins plus ou moins distants mais toujours dans Paris intramuros.");

            AfficherTexteFormate("Elle offre des fonctionnalités pour les clients, les cuisiniers et les administrateurs.");

            AfficherTexteFormate("L'application est en constante amélioration. En effet, nous proposons régulièrement de nouvelles " +
                "mises à jour apportant de nouvelles fonctionnalités ou corrigeant certains bugs.");

            AfficherTexteFormate("L'application est basée sur la bonne foi des personnes. Notre équipe supervise régulièrement les " +
                "nouvelles inscriptions, les nouvelles commandes et beaucoup d'autres informations.");

            AfficherTexteFormate("La base conserve l'historique de toutes les transactions réalisées entre un cuisinier et un client " +
                "afin de pouvoir identifier les plus grands cuisiniers (par mois ou par an) et réorganiser au besoin les transactions " +
                "en fonction des trajets effectués, des repas livrés, de la fréquence et des retours des clients. En fonction de ces " +
                "retours, il est possible qu'un client ou un cuisinier puisse être radié de la plateforme.");

            AfficherTexteFormate("De même, en cas d'activté suspecte ou de mauvais comportement, nous nous réserverons également le droit de vous bannir de la plateforme. ");

            AfficherTexteFormate("Nous envisageons dans un avenir proche un développement au-delà de Paris intramuros. Merci pour votre compréhension. ");

            Console.WriteLine("\n------------------------------------------------------------------");
            Console.WriteLine("Appuyez sur une touche pour revenir au menu précédent...");
            Console.ReadKey();
        }
        #endregion

        #region Présente brièvement l'application
        /// <summary>
        /// Affiche des informations sur le fonctionnement de l'application
        /// </summary>
        static void FonctionnementAppli()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("FONCTIONNEMENT DE L'APPLICATION");

            AfficherTexteFormate("Vous pouvez rejoindre Liv'in Paris en tant que client, cuisinier ou livreur. " +
                "Si vous souhaitez rallier nos rangs et devenir administrateur de l'application, n'hésitez pas à envoyer votre candidature.");

            AfficherTexteFormate("Il faut s'inscrire auprès de l'application en déclinant son identité (nom, prénom, " +
                "adresse, téléphone, adresse email...). En fonction du rôle souhaité, des informations supplémentaires peuvent vous être " +
                "demandées (restaurant d'assignation pour un cuisinier, référent entreprise pour un client ...etc)");

            AfficherTexteFormate("Pour information, un cuisinier peut devenir client et inversement ou être les 2 à la " +
                "fois de manière simultanée. Un client peut être un particulier, ou une entreprise locale.");

            AfficherTexteFormate("Un cuisinier partage sur le sit le plat confectionné en précisant pour chacun des mets" +
                " s'il s'agit d'une entrée, du plat principal ou d'un dessert.");

            AfficherTexteFormate("Le cuisinier se doit d'assurer la livraison de son plat. Il prend pour cela contact avec" +
                " un livreur. Celui-ci peut combiner plusieurs livraisons si nécessaire en fonction des zones de livraisons.");

            AfficherTexteFormate("Pour faciliter les transactions, l'application met à la disposition des livreurs une fonction " +
                "qui va calculer le plus court chemin entre les 2 adresses : celle du cuisinier et celle du client. " +
                "Dans un premier temps, par soucis de simplification, seul le cas des lignes de métros est traité. Au fil du " +
                "développement de l'application, de nouvelles options seront proposées (vélos, bus, transiliens, RER...)");

            AfficherTexteFormate("Le client choisit un restaurant disponible sur l'application et effectue sa commande en " +
                "choisissant son ou ses plats. Les plats peuvent être génériques " +
                "et obéir à une recette déjà enregistrée ou bien être une déclinaison d'une recette déjà existante.");

            AfficherTexteFormate("Pour information, la transaction peut être simple ou multiple. En effet, il est possible " +
                "qu'au sein d'une même commande, le client puisse avoir plusieurs lignes de commandes." +
                "Chaque ligne de commande peut avoir une date et un lieu de livraison différent. En revanche, la transaction " +
                "financière est réalisée une fois pour toute avant la première livraison.");

            Console.WriteLine("\nPour résumer, voici les fonctionnalités principales: ");
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("- Gestion des profils utilisateurs (client, cuisinier, administrateur)");
            Console.WriteLine("- Gestion des plats et des ingrédients");
            Console.WriteLine("- Gestion des commandes et des livraisons");
            Console.WriteLine("- Système d'évaluation des cuisiniers");
            Console.WriteLine("- Statistiques détaillées");
            Console.WriteLine("------------------------------------------------------------------");

            AfficherTexteFormate("Pour plus de détails, nous vous invitons à créer un compte et explorer l'application par vous même. " +
                "Vos retours sont précieux, n'hésitez pas à revenir vers notre équipe pour laisser un avis");

            Console.WriteLine("\n------------------------------------------------------------------");
            Console.WriteLine("Appuyez sur une touche pour revenir au menu précédent...");
            Console.ReadKey();
        }
        #endregion

        #region Présente les infos contacts 
        /// <summary>
        /// Affiche des informations sur le développement et la gestion de l'application
        /// </summary>
        static void InformationsContact()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("INFOS CONTACT");

            AfficherTexteFormate("Cette application a été développée au premier semestre 2025 par : NORAT Faizan, " +
                "NAUDY-NORMAND Thomas et PASQUIER Martin dans le cadre du module 'PSI' de 2ème année.");

            Console.WriteLine("\nPour une question sur son fonctionnement, une suggestion d'amélioration ou une plainte, " +
                "merci de nous contacter à l'une des adresses suivantes : ");
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("faizan.norat@edu.devinvi.fr");
            Console.WriteLine("thomas.naudy-normand@edu.devinci.fr");
            Console.WriteLine("martin.pasquier@edu.devinci.fr");
            Console.WriteLine("------------------------------------------------------------------");

            AfficherTexteFormate("Si vous souhaitez nous rejoindre, merci de présenter vos motivations dans une courte " +
                "lettre jointe à votre CV. Nous reviendrons vers vous dans les plus brefs délais.");

            Console.WriteLine("\n------------------------------------------------------------------");
            Console.WriteLine("Appuyez sur une touche pour revenir au menu précédent...");
            Console.ReadKey();
        }
        #endregion

        #region Évaluation de l'application
        /// <summary>
        /// Permet à l'utilisateur d'évaluer l'application
        /// </summary>
        static void EvaluerAppli()
        {
            Console.Clear();
            GestionBDD.AfficherTitre("ÉVALUER L'APPLICATION");

            Console.WriteLine("Votre avis est important pour nous aider à améliorer l'application.");
            Console.WriteLine("Merci de prendre quelques instants pour partager votre expérience.");
            Console.WriteLine("------------------------------------------------------------------");

            int note = 0;
            bool noteValide = false;

            while (!noteValide)
            {
                Console.Write("\nVeuillez noter l'application de 1 à 5 étoiles : ");
                string noteStr = Console.ReadLine();

                if (int.TryParse(noteStr, out note) && note >= 1 && note <= 5)
                {
                    noteValide = true;
                }
                else
                {
                    GestionBDD.AfficherMessageErreur("Veuillez entrer un nombre entre 1 et 5.");
                }
            }

            Console.Write("\nVotre note : ");
            for (int i = 0; i < note; i++)
            {
                Console.Write("|");
            }
            Console.WriteLine();

            /// Demande du commentaire
            Console.WriteLine("\nVeuillez laisser un commentaire (facultatif) :");
            string commentaire = Console.ReadLine();

            /// Demande des aspects à améliorer
            Console.WriteLine("\nQuels aspects de l'application souhaiteriez-vous voir améliorés ? (facultatif)");
            Console.WriteLine("1. Interface utilisateur");
            Console.WriteLine("2. Performance");
            Console.WriteLine("3. Fonctionnalités");
            Console.WriteLine("4. Autre");
            Console.Write("Votre choix (séparés par des virgules si plusieurs) : ");
            string aspectsStr = Console.ReadLine();

            List<string> aspects = new List<string>();
            if (!string.IsNullOrWhiteSpace(aspectsStr))
            {
                string[] choix = aspectsStr.Split(',');
                foreach (string c in choix)
                {
                    switch (c.Trim())
                    {
                        case "1": aspects.Add("Interface utilisateur"); break;
                        case "2": aspects.Add("Performance"); break;
                        case "3": aspects.Add("Fonctionnalités"); break;
                        case "4":
                            Console.Write("Précisez : ");
                            aspects.Add(Console.ReadLine());
                            break;
                    }
                }
            }

            Console.WriteLine("\n------------------------------------------------------------------");
            GestionBDD.AfficherMessageSucces("Merci pour votre évaluation !");
            Console.WriteLine("Votre avis a été enregistré et sera pris en compte pour améliorer l'application.");

            if (note <= 3 && !string.IsNullOrWhiteSpace(commentaire))
            {
                Console.WriteLine("\nNous sommes désolés que votre expérience n'ait pas été entièrement satisfaisante.");
                Console.WriteLine("Un membre de notre équipe examinera votre commentaire et nous ferons de notre mieux pour résoudre les problèmes que vous avez rencontrés.");
            }

            Console.WriteLine("\n------------------------------------------------------------------");
            Console.WriteLine("Appuyez sur une touche pour revenir au menu précédent...");
            Console.ReadKey();
        }
        #endregion

        #region Méthodes utilitaires
        /// <summary>
        /// Affiche un texte formaté avec des sauts de ligne et une meilleure lisibilité
        /// </summary>
        /// <param name="texte">Le texte à afficher</param>
        /// <param name="sautLigneApres">Indique s'il faut ajouter un saut de ligne après le texte</param>
        private static void AfficherTexteFormate(string texte, bool sautLigneApres = false)
        {
            /// Largeur maximale pour le texte (pour éviter les lignes trop longues)
            int largeurMax = 80;

            string[] mots = texte.Split(' ');

            StringBuilder ligne = new StringBuilder();

            foreach (string mot in mots)
            {
                if (ligne.Length + mot.Length + 1 > largeurMax && ligne.Length > 0)
                {
                    Console.WriteLine(ligne.ToString());
                    ligne.Clear();
                }

                if (ligne.Length > 0)
                {
                    ligne.Append(" ");
                }
                ligne.Append(mot);
            }

            if (ligne.Length > 0)
            {
                Console.WriteLine(ligne.ToString());
            }

            if (sautLigneApres)
            {
                Console.WriteLine();
            }
        }
        #endregion
    }
}
