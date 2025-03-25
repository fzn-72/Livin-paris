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
    public class Lien<T>
    {
        private Noeud<T> noeud1;
        private Noeud<T> noeud2;
        private int poids;

        /// <summary>
        /// Le constructeur de la classe Lien initialise deux noeuds
        /// </summary>
        public Lien(Noeud<T> noeud1, Noeud<T> noeud2, int poids = 0)
        {
            this.noeud1 = noeud1;
            this.noeud2 = noeud2;
            this.poids = poids;
        }

        /// <summary>
        /// Accéder au premier noeud du lien
        /// </summary>
        public Noeud<T> Noeud1
        {
            get { return noeud1; }
        }

        /// <summary>
        /// Accéder au second noeud du lien
        /// </summary>
        public Noeud<T> Noeud2
        {
            get { return noeud2; }
        }

        /// <summary>
        /// Accéder au poids du lien
        /// </summary>
        public int Poids
        {
            get { return poids; }
        }
    }

}
