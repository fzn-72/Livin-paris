using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Un lien représente une connexion entre deux membres (noeuds) de l'association
    /// </summary>
    internal class Lien
    {
        private Noeud noeud1;
        private Noeud noeud2;

        public Lien(Noeud noeud1, Noeud noeud2)
        { 
            this.noeud1 = noeud1;
            this.noeud2 = noeud2;
        }

        public Noeud Noeud1
        { 
            get 
            { 
                return noeud1; 
            } 
        }

        public Noeud Noeud2
        {
            get
            {
                return noeud2;
            }
        }



    }
}
