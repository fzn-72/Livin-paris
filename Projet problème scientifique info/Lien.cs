using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Un lien représente une connexion entre deux membres (noeuds) de l'association. Ainsi, dans cette classe, seuls deux noeuds sont initialisés, 
    /// représentant deux membres différents de l'association. Un poids est aussi attribué à chaque lien. On suppose par défaut que ce poids est nul 
    /// (graphe non pondéré). Le sujet indique que "Les relations entre les membres de l’association sont supposées réciproques". Ainsi, s'il existe 
    /// un lien entre le membre A et le membre B, il y en a aussi un entre B et A; ce qui implique que le graphe représentant la situation sera non-orienté.
    /// </summary>
    internal class Lien
    {
        private Noeud noeud1;
        private Noeud noeud2;
        private int poids;

        /// <summary>
        /// Le constructeur naturel de la classe Lien permet d'initialiser deux noeuds (utile dans la classe Graphe) 
        /// </summary>
        /// <param name="noeud1"></param>
        /// <param name="noeud2"></param>
        public Lien(Noeud noeud1, Noeud noeud2, int poids = 0)
        { 
            this.noeud1 = noeud1;
            this.noeud2 = noeud2;
            this.poids = poids;
        }

        /// <summary>
        /// Cette propriété permet d'accéder à l'attribut "noeud1" (Noeud) en lecture 
        /// </summary>
        public Noeud Noeud1
        { 
            get 
            { 
                return noeud1; 
            } 
        }

        /// <summary>
        /// Cette propriété permet d'accéder à l'attribut "noeud2" (Noeud) en lecture 
        /// </summary>
        public Noeud Noeud2
        {
            get
            {
                return noeud2;
            }
        }

        /// <summary>
        /// Cette propriété permet d'accéder à l'attribut "poids" (int) en lecture 
        /// </summary>
        public int Poids
        {
            get 
            {
                return poids;
            }
        }

        /// <summary>
        /// Cette méthode permet d'accéder au poids de chaque lien
        /// </summary>
        /// <returns></returns>
        public int ObtenirPoids()
        {
            return poids;
        }
    }
}
