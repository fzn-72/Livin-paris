using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_problème_scientifique_info
{
    internal class Graphe
    {
        private Dictionary<int, List<int>> ListeAdjacence;
        private int[,] MatriceAdjacence;
        private int nbNoeuds;
        private List<Lien> liensNoeuds;

        public Graphe(int nbNoeuds)
        {
            ListeAdjacence = new Dictionary<int, List<int>>();
            MatriceAdjacence = new int[nbNoeuds, nbNoeuds];
            this.nbNoeuds = nbNoeuds;
            liensNoeuds = new List<Lien>();
        }

        public void AjouterLien(int i, int j)
        {
            Noeud noeud1 = new Noeud(i);
            Noeud noeud2 = new Noeud(j);
            liensNoeuds.Add(new Lien(noeud1, noeud2));
            // Liste d'adjacence
            if (!ListeAdjacence.ContainsKey(i))
            {
                ListeAdjacence[i] = new List<int>();
            }
            if (!ListeAdjacence.ContainsKey(j))
            {
                ListeAdjacence[j] = new List<int>();
            }
            ListeAdjacence[i].Add(j);
            ListeAdjacence[j].Add(i);

            // Matrice d'adjacence
            MatriceAdjacence[i, j] = 1;
            MatriceAdjacence[j, i] = 1;
        }
    }
}
