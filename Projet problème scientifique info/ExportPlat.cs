using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Classe unique pour l'exportation des plats au format XML
    /// </summary>
    public class ExportPlats
    {
        // Informations générales
        public DateTime DateExport { get; set; }
        public int NombrePlats { get; set; }

        // Liste des plats
        public List<Plat> ListePlats { get; set; }

        // Constructeur
        public ExportPlats()
        {
            DateExport = DateTime.Now;
            ListePlats = new List<Plat>();
        }

        // Classe interne pour représenter un plat
        public class Plat
        {
            public string IdPlat { get; set; }
            public string NomPlat { get; set; }
            public string TypePlat { get; set; }
            public string Nationalite { get; set; }
            public string Prix { get; set; }
            public string PortionsDisponibles { get; set; } // Changé en string pour correspondre au type dans la BDD
            public string Description { get; set; }

            // Informations sur le cuisinier
            public string IdCuisinier { get; set; }
            public string NomCuisinier { get; set; }
            public string PrenomCuisinier { get; set; }
            public decimal NoteCuisinier { get; set; }

            // Listes des ingrédients et évaluations
            public List<Ingredient> Ingredients { get; set; }
            public List<Evaluation> Evaluations { get; set; }

            // Constructeur
            public Plat()
            {
                Ingredients = new List<Ingredient>();
                Evaluations = new List<Evaluation>();
            }

            // Classe interne pour représenter un ingrédient
            public class Ingredient
            {
                public string Nom { get; set; }
                public string Stock { get; set; }

                public string Categorie { get; set; }

                public string Origine { get; set; }
            }

            // Classe interne pour représenter une évaluation
            public class Evaluation
            {
                public int Note { get; set; }
                public string Commentaire { get; set; }
                public DateTime Date { get; set; }
            }
        }
    }


}
