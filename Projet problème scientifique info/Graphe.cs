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
        private Dictionary<int, List<int>> listeAdjacence;
        private int[,] matriceAdjacence;
        private int nbNoeuds;
        private List<Lien> liensNoeuds;

        /// <summary>
        /// Le constructeur naturel de la classe Graphe permet entre autres d'initaliser la matrice "matriceAdjacence", la liste "liensNoeuds" et le dictionnaire "listeAdjacence"
        /// </summary>
        /// <param name="nbNoeuds"></param>
        public Graphe(int nbNoeuds)
        {
            listeAdjacence = new Dictionary<int, List<int>>();
            matriceAdjacence = new int[nbNoeuds, nbNoeuds];
            this.nbNoeuds = nbNoeuds;
            liensNoeuds = new List<Lien>();
        }

        /// <summary>
        /// Cette propriété permet d'accéder à l'attribut "listeAdjacence" (dictionnaire) en lecture 
        /// </summary>
        public Dictionary<int, List<int>> ListeAdjacence
        {
            get
            {
                return listeAdjacence;
            }
        }

        /// <summary>
        /// Cette propriété permet d'accéder à l'attribut "liensNoeuds" (liste) en lecture 
        /// </summary>
        public List<Lien> LiensNoeuds
        {
            get
            {
                return liensNoeuds;
            }
        }

        /// <summary>
        /// Cette méthode ajoute un lien entre les nœuds i et j dans la structure du graphe. Elle vérifie et initialise les listes d'adjacence si nécessaire, puis ajoute la connexion  
        /// en évitant les doublons. Elle met également à jour la matrice d'adjacence pour représenter cette connexion et enregistre le lien dans la liste des "Lien".  
        /// </summary>
        public void AjouterLien(int i, int j)
        {
            if (!listeAdjacence.ContainsKey(i))
            {
                listeAdjacence[i] = new List<int>();
            }
            if (!listeAdjacence.ContainsKey(j))
            {
                listeAdjacence[j] = new List<int>();
            }

            if (!listeAdjacence[i].Contains(j) && !listeAdjacence[j].Contains(i))
            {
                listeAdjacence[i].Add(j);
                listeAdjacence[j].Add(i);
            }

            if (matriceAdjacence[i, j] == 0 && matriceAdjacence[j, i] == 0)
            {
                matriceAdjacence[i, j] = 1;
                matriceAdjacence[j, i] = 1;
            }
            Lien nouveauLien = new Lien(new Noeud(i), new Noeud(j));
            liensNoeuds.Add(nouveauLien);
        }

        //public void AjouterLien(int i, int j)
        //{
        //    if (!listeAdjacence.ContainsKey(i))
        //        listeAdjacence[i] = new List<int>();
        //    if (!listeAdjacence.ContainsKey(j))
        //        listeAdjacence[j] = new List<int>();

        //    listeAdjacence[i].Add(j);
        //    listeAdjacence[j].Add(i);
        //    matriceAdjacence[i, j] = 1;
        //    matriceAdjacence[j, i] = 1;

        //    liensNoeuds.Add(new Lien(new Noeud(i), new Noeud(j)));
        //}

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
                foreach (int voisin in listeAdjacence[noeud])
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
                    foreach (int voisin in listeAdjacence[noeud])
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

            int premierNoeud = listeAdjacence.Keys.First();
            file.Add(premierNoeud);
            visite.Add(premierNoeud);
            while (file.Count > 0)
            {
                int noeud = file[0];  /// On prend le premier élément
                file.RemoveAt(0);      /// Puis on va supprimer ce premier élément
                foreach (int voisin in listeAdjacence[noeud])
                {
                    if (!visite.Contains(voisin))
                    {
                        file.Add(voisin);  /// On ajoute à la fin
                        visite.Add(voisin);
                    }
                }
            }
            return visite.Count == listeAdjacence.Count;
        }

        private bool ContientCycleDFS(int noeud, int parent, HashSet<int> visite)
        {
            visite.Add(noeud);
            foreach (int voisin in listeAdjacence[noeud])
            {
                if (!visite.Contains(voisin))
                {
                    if (ContientCycleDFS(voisin, noeud, visite))
                    {
                        return true;
                    }
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

            foreach (int noeud in listeAdjacence.Keys)
            {
                if (visite.Contains(noeud) == false)
                {
                    if (ContientCycleDFS(noeud, -1, visite))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
