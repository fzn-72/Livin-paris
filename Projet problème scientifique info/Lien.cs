using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// 
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
