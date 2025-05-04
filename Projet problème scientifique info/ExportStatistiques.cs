using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Projet_problème_scientifique_info;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Classe représentant les statistiques d'un client pour l'exportation XML
    /// </summary>
    [Serializable]
    public class ExportStatistiques
    {
        public string IdClient { get; set; }
        public string NomClient { get; set; }
        public string PrenomClient { get; set; }
        public string Email { get; set; }
        public List<CommandeExport> Commandes { get; set; }
        public List<PlatPrefere> PlatsPreferees { get; set; }
        public decimal DepensesTotales { get; set; }
        public int NombreCommandes { get; set; }
        public DateTime DateExport { get; set; }

        public ExportStatistiques()
        {
            Commandes = new List<CommandeExport>();
            PlatsPreferees = new List<PlatPrefere>();
            DateExport = DateTime.Now;
        }
    }

    /// <summary>
    /// Classe représentant une commande pour l'exportation XML
    /// </summary>
    [Serializable]
    public class CommandeExport
    {
        public string IdCommande { get; set; }
        public DateTime DateCommande { get; set; }
        public string PrixTotal { get; set; }
        public string Statut { get; set; }
        public List<LigneCommandeExport> LignesCommande { get; set; }

        public CommandeExport()
        {
            LignesCommande = new List<LigneCommandeExport>();
        }
    }

    /// <summary>
    /// Classe représentant une ligne de commande pour l'exportation XML
    /// </summary>
    [Serializable]
    public class LigneCommandeExport
    {
        public string IdLigne { get; set; }
        public string NomPlat { get; set; }
        public string PrixUnitaire { get; set; }
        public int Quantite { get; set; }
        public string NomCuisinier { get; set; }
    }

    /// <summary>
    /// Classe représentant un plat préféré pour l'exportation XML
    /// </summary>
    [Serializable]
    public class PlatPrefere
    {
        public string IdPlat { get; set; }
        public string NomPlat { get; set; }
        public string TypePlat { get; set; }
        public string Nationalite { get; set; }
        public int NombreCommandes { get; set; }
    }

    /// <summary>
    /// Classe représentant les statistiques d'un cuisinier pour l'exportation XML
    /// </summary>
    [Serializable]
    public class StatistiquesCuisinier
    {
        public string IdCuisinier { get; set; }
        public string NomCuisinier { get; set; }
        public string PrenomCuisinier { get; set; }
        public decimal NoteMoyenne { get; set; }
        public List<PlatPopulaire> PlatsPopulaires { get; set; }
        public List<ClientFrequent> ClientsFrequents { get; set; }
        public decimal ChiffreAffaires { get; set; }
        public int NombrePlatsVendus { get; set; }
        public DateTime DateExport { get; set; }

        public StatistiquesCuisinier()
        {
            PlatsPopulaires = new List<PlatPopulaire>();
            ClientsFrequents = new List<ClientFrequent>();
            DateExport = DateTime.Now;
        }
    }

    /// <summary>
    /// Classe représentant un plat populaire pour l'exportation XML
    /// </summary>
    [Serializable]
    public class PlatPopulaire
    {
        public string IdPlat { get; set; }
        public string NomPlat { get; set; }
        public string TypePlat { get; set; }
        public string Prix { get; set; }
        public int NombreCommandes { get; set; }
    }

    /// <summary>
    /// Classe représentant un client fréquent pour l'exportation XML
    /// </summary>
    [Serializable]
    public class ClientFrequent
    {
        public string IdClient { get; set; }
        public string NomClient { get; set; }
        public string PrenomClient { get; set; }
        public string Email { get; set; }
        public int NombreCommandes { get; set; }
    }

    /// <summary>
    /// Classe représentant les statistiques globales pour l'exportation XML
    /// </summary>
    [Serializable]
    public class StatistiquesGlobales
    {
        public int NombreClients { get; set; }
        public int NombreCuisiniers { get; set; }
        public int NombrePlats { get; set; }
        public int NombreCommandes { get; set; }
        public decimal ChiffreAffairesTotal { get; set; }
        public List<StatistiquesMensuelles> StatistiquesMensuelles { get; set; }
        public List<MeilleurCuisinier> MeilleursCuisiniers { get; set; }
        public DateTime DateExport { get; set; }

        public StatistiquesGlobales()
        {
            StatistiquesMensuelles = new List<StatistiquesMensuelles>();
            MeilleursCuisiniers = new List<MeilleurCuisinier>();
            DateExport = DateTime.Now;
        }
    }

    /// <summary>
    /// Classe représentant les statistiques mensuelles pour l'exportation XML
    /// </summary>
    [Serializable]
    public class StatistiquesMensuelles
    {
        public string Mois { get; set; }
        public int NombreCommandes { get; set; }
        public decimal ChiffreAffaires { get; set; }
    }

    /// <summary>
    /// Classe représentant un meilleur cuisinier pour l'exportation XML
    /// </summary>
    [Serializable]
    public class MeilleurCuisinier
    {
        public string IdCuisinier { get; set; }
        public string NomCuisinier { get; set; }
        public string PrenomCuisinier { get; set; }
        public decimal NoteMoyenne { get; set; }
        public int NombreCommandes { get; set; }
        public decimal ChiffreAffaires { get; set; }
    }
}