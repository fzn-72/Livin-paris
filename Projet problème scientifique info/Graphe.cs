/*using System;
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
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


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
            if (!ListeAdjacence.ContainsKey(i))
                ListeAdjacence[i] = new List<int>();
            if (!ListeAdjacence.ContainsKey(j))
                ListeAdjacence[j] = new List<int>();

            ListeAdjacence[i].Add(j);
            ListeAdjacence[j].Add(i);
            MatriceAdjacence[i, j] = 1;
            MatriceAdjacence[j, i] = 1;

            liensNoeuds.Add(new Lien(new Noeud(i), new Noeud(j)));
        }

        public void ChargerDepuisFichier(string chemin)
        {
            using (StreamReader sr = new StreamReader(chemin))
            {
                string ligne;
                while ((ligne = sr.ReadLine()) != null)
                {
                    if (ligne.StartsWith("%") || ligne.StartsWith("%%"))
                        continue;

                    string[] elements = ligne.Split(' ');
                    if (elements.Length == 2)
                    {
                        int noeud1 = Convert.ToInt32(elements[0]) - 1;
                        int noeud2 = Convert.ToInt32(elements[1]) - 1;
                        AjouterLien(noeud1, noeud2);
                    }
                }
            }
        }

        public void ParcoursLargeur(int sommetDepart)
        {
            HashSet<int> visite = new HashSet<int>();
            List<int> file = new List<int>();

            file.Add(sommetDepart);  /// On ajoute le sommet de départ à la liste
            visite.Add(sommetDepart);

            while (file.Count > 0)
            {
                int noeud = file[0];  /// On prend le premier élément de la liste
                file.RemoveAt(0);     /// Puis on retire cet élément de la liste
                Console.Write(noeud + " ");

                foreach (int voisin in ListeAdjacence[noeud])
                {
                    if (!visite.Contains(voisin))
                    {
                        file.Add(voisin);  /// Ici on ajoute le voisin à la fin de la liste
                        visite.Add(voisin);
                    }
                }
            }
        }


        public void ParcoursProfondeur(int sommetDepart)
        {
            HashSet<int> visite = new HashSet<int>();
            List<int> pile = new List<int>();

            pile.Add(sommetDepart);  /// On ajoute le sommet de départ à la liste

            while (pile.Count > 0)
            {
                int noeud = pile[pile.Count - 1];  /// On prend le dernier élément de la liste
                pile.RemoveAt(pile.Count - 1);     /// On retire cet élément de la liste

                if (!visite.Contains(noeud))
                {
                    Console.Write(noeud + " ");
                    visite.Add(noeud);

                    foreach (int voisin in ListeAdjacence[noeud])
                    {
                        if (!visite.Contains(voisin))
                        {
                            pile.Add(voisin);  /// On ajoute le voisin à la fin de la liste
                        }
                    }
                }
            }
        }


        public bool EstConnexe()
        {
            HashSet<int> visite = new HashSet<int>();
            List<int> file = new List<int>();

            int premierNoeud = ListeAdjacence.Keys.First();
            file.Add(premierNoeud);
            visite.Add(premierNoeud);

            while (file.Count > 0)
            {
                int noeud = file[0];  /// On prend le premier élément
                file.RemoveAt(0);      /// Puis on va supprimer ce premier élément

                foreach (int voisin in ListeAdjacence[noeud])
                {
                    if (!visite.Contains(voisin))
                    {
                        file.Add(voisin);  /// On ajoute à la fin
                        visite.Add(voisin);
                    }
                }
            }

            return visite.Count == ListeAdjacence.Count;
        }


        private bool ContientCycleDFS(int noeud, int parent, HashSet<int> visite)
        {
            visite.Add(noeud);

            foreach (int voisin in ListeAdjacence[noeud])
            {
                if (!visite.Contains(voisin))
                {
                    if (ContientCycleDFS(voisin, noeud, visite))
                        return true;
                }
                else if (voisin != parent)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContientCycle()
        {
            HashSet<int> visite = new HashSet<int>();

            foreach (int noeud in ListeAdjacence.Keys)
            {
                if (!visite.Contains(noeud))
                {
                    if (ContientCycleDFS(noeud, -1, visite))
                        return true;
                }
            }
            return false;
        }


    }
}
