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
        public static void AproposDeLAppli()
        {
            Console.Clear();
            Console.WriteLine("Que souhaitez-vous faire ?");
            Console.WriteLine("\n1. Obtenir un mot sur l'application");
            Console.WriteLine("2. Voir le mode d'emploi de l'application");
            Console.WriteLine("3. Voir la fiche contacts");
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
                default:
                    GestionBDD.AfficherMessageErreur("Option invalide.");
                    break;
            }
        }
        #endregion


        #region Présente qq infos sur l'application
        static void MotSurAppli()
        {
            Console.Clear();
            Console.WriteLine("-Liv’in Paris propose des services de partages de repas entre voisins plus ou moins distants mais toujours dans Paris intramuros.");
            Console.WriteLine("\n-L'application est en constante amélioration. En effet, nous proposons régulièrement de nouvelles mises à jour apportant de nouvelles fonctionnalités ou corrigeant certains bugs.");
            Console.WriteLine("\n-L'application est basée sur la bonne foi des personnes. Notre équipe supervise régulièrement les nouvelles inscriptions, les nouvelles commandes et beaucoup d'autres informations.");
            Console.WriteLine("\n-La base conserve l’historique de toutes les transactions réalisées entre un cuisinier et un client afin de pouvoir identifier les plus grands cuisiniers par mois, par an … et réorganiser au besoin les transactions " +
                "en fonction des trajets effectués, des repas livrés, de la fréquence …  et des retours des clients. En fonction de ces retours, il est possible qu’un client ou un cuisinier puisse être radié de la plateforme.");
            Console.WriteLine("\n-De même, en cas d'activté suspecte ou de mauvais comportement, nous nous réserverons également le droit de vous bannir de la plateforme. ");
            Console.WriteLine("\n-Nous envisageons un développement au-delà de Paris intramuros dans quelques semaines. Merci pour votre compréhension. ");
        }
        #endregion


        #region Présente brièvement l'application
        static void FonctionnementAppli()
        {
            Console.WriteLine("-Vous pouvez rejoindre Liv'in Paris en tant que client, cuisinier ou livreur. Si vous souhaitez rallier nos rangs et devenir administrateur de l'application, n'hésitez pas à envoyer votre candidature.");
            Console.WriteLine("\nIl faut s’inscrire auprès de l’application en déclinant son identité (nom, prénom, adresse, téléphone, adresse email...). En fonction du rôle souhaité, des informations supplémentaires peuvent vous être " +
                "demandées (restaurant d'assignation pour un cuisinier, référent entreprise pour un client ...etc)");
            Console.WriteLine("\n-Pour information, un cuisinier peut devenir client et inversement ou être les 2 à la fois de manière simultanée. Un client peut être un particulier, ou une entreprise locale.");
            Console.WriteLine("\n-Un cuisinier partage sur le sit le plat confectionné en précisant pour chacun des mets s’il s’agit d’une entrée, du plat principal ou d'un dessert.");
            Console.WriteLine("\nLe cuisinier se doit d’assurer la livraison de son plat. Il prend pour cela contact avec un livreur. Celui-ci peut combiner plusieurs livraisons si nécessaire en fonction des zones de livraisons.");
            Console.WriteLine("\n-Pour faciliter les transactions, l’application met à la disposition des livreurs une fonction qui va calculer le plus court chemin entre les 2 adresses : celle du cuisinier et celle du client. " +
                "Dans un premier temps, par soucis de simplification, seul le cas des lignes de métros est traité. Au fil du développement de l'application, de nouvelles options seront proposées (vélos, bus, transiliens, RER...)");
            Console.WriteLine("\n-Le client choisit un restaurant disponible sur l'application et effectue sa commande en choisissant son ou ses plats. Les plats peuvent être génériques " +
                "et obéir à une recette déjà enregistrée ou bien être une déclinaison d’une recette déjà existante.");
            Console.WriteLine("\n-Pour information, la transaction peut être simple ou multiple. En effet, il est possible qu’au sein d’une même commande, le client puisse avoir plusieurs lignes de commandes." +
                "Chaque ligne de commande peut avoir une date et un lieu de livraison différent. En revanche, la transaction financière est réalisée une fois pour toute avant la première livraison.");
        }
        #endregion


        #region Présente les infos contacts 
        static void InformationsContact()
        {
            Console.Clear();
            Console.WriteLine("Cette application a été développée au premier semestre 2025 par : NORAT Faizan, NAUDY-NORMAND Thomas et PASQUIER Martin dans le cadre du module 'PSI' de 2ème année");
            Console.WriteLine("\nPour une question sur son fonctionnement, une suggestion d'amélioration ou une plainte, merci de nous contacter à l'une des adresses suivantes : " +
                "\nfaizan.norat@edu.devinvi.fr\nthomas.naudy-normand@edu.devinci.fr\nmartin.pasquier@edu.devinci.fr");
            Console.WriteLine("\nSi vous souhaitez nous rejoindre, merci de présenter vos motivations dans une courte lettre jointe à votre CV. Nous reviendrons vers vous dans les plus brefs délais.");
        }
        #endregion

    }
}
