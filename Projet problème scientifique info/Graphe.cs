using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_problème_scientifique_info
{
    internal class Graphe
    {
        private int nbNoeuds;
        private int[,] matriceAdjacence;

        public Graphe(int nbNoeuds)
        {
            this.nbNoeuds = nbNoeuds;
        }
    }
}
