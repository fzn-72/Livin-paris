using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Projet_problème_scientifique_info
{
    public class Graphe
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
            for (int i = 0; i < nbNoeuds; i++)
            {
                listeAdjacence[i] = new List<int>();
            }
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
        /// Cette propriété permet d'accéder à l'attribut "matriceAdjacence" (matrice) en lecture 
        /// </summary>
        public int[,] MatriceAdjacence
        {
            get 
            { 
                return matriceAdjacence; 
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

        /// <summary>
        /// ECRIS ICI MARTIN
        /// </summary>
        /// <param name="chemin"></param>
        public void ChargerDepuisFichier(string chemin)
        {
            foreach (string ligne in File.ReadLines(chemin))
            {
                if (ligne.Length > 0 && ligne[0] != '%') // Vérifie que la ligne n'est ni vide ni un commentaire
                {
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

        /// <summary>
        /// ECRIS ICI MARTIN
        /// </summary>
        /// <param name="sommetDepart"></param>
        public void ParcoursLargeur(int sommetDepart)
        {
            Dictionary<int, bool> visite = new Dictionary<int, bool>();
            List<int> file = new List<int>();

            file.Add(sommetDepart);
            visite[sommetDepart] = true;

            while (file.Count > 0)
            {
                int noeud = file[0];
                file.RemoveAt(0);
                Console.Write(noeud + " ");

                if (listeAdjacence.ContainsKey(noeud)) 
                {
                    foreach (int voisin in listeAdjacence[noeud])
                    {
                        if (!visite.ContainsKey(voisin)) 
                        {
                            file.Add(voisin);
                            visite[voisin] = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ECRIS ICI MARTIN
        /// </summary>
        /// <param name="sommetDepart"></param>
        public void ParcoursProfondeur(int sommetDepart)
        {
            Dictionary<int, bool> visite = new Dictionary<int, bool>();
            List<int> pile = new List<int>();

            foreach (var sommet in listeAdjacence.Keys)
            {
                visite[sommet] = false;
            }

            pile.Add(sommetDepart);

            while (pile.Count > 0)
            {
                int noeud = pile[pile.Count - 1];
                pile.RemoveAt(pile.Count - 1);    

                if (!visite[noeud])
                {
                    Console.Write(noeud + " ");
                    visite[noeud] = true;

                    foreach (int voisin in listeAdjacence[noeud])
                    {
                        if (!visite[voisin])
                        {
                            pile.Add(voisin);  
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  ECRIS ICI MARTIN
        /// </summary>
        /// <returns></returns>
        public bool EstConnexe()
        {
            Dictionary<int, bool> visite = new Dictionary<int, bool>();
            List<int> file = new List<int>();

            int premierNoeud = listeAdjacence.Keys.First();
            file.Add(premierNoeud);
            visite[premierNoeud] = true;

            while (file.Count > 0)
            {
                int noeud = file[0];  
                file.RemoveAt(0);     

                foreach (int voisin in listeAdjacence[noeud])
                {
                    if (!visite.ContainsKey(voisin) || !visite[voisin])
                    {
                        file.Add(voisin);  
                        visite[voisin] = true;
                    }
                }
            }
            return visite.Count == listeAdjacence.Count;
        }

        /// <summary>
        /// ECRIS ICI MARTIN
        /// </summary>
        /// <param name="noeud"></param>
        /// <param name="parent"></param>
        /// <param name="visite"></param>
        /// <returns></returns>
        private bool ContientCycleDFS(int noeud, int parent, Dictionary<int, bool> visite)
        {
            visite[noeud] = true;
            foreach (int voisin in listeAdjacence[noeud])
            {
                if (visite.ContainsKey(voisin) == false || visite[voisin] == false)
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

        /// <summary>
        /// ECRIS ICI MARTIN
        /// </summary>
        /// <returns></returns>
        public bool ContientCycle()
        {
            Dictionary<int, bool> visite = new Dictionary<int, bool>();

            foreach (int noeud in listeAdjacence.Keys)
            {
                if (visite.ContainsKey(noeud) == false || visite[noeud] == false)
                {
                    if (ContientCycleDFS(noeud, -1, visite))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// ECRIS ICI MARTIN
        /// </summary>
        public void AnalyserProprietesGraphe()
        {
            int ordre = listeAdjacence.Count; 
            int taille = 0; 

            bool graphePondere = false; 
            bool grapheOriente = false;

            foreach (var lien in liensNoeuds)
            {
                if (lien.Poids != 0)
                {
                    graphePondere = true;
                }
                taille++;
                if (listeAdjacence[lien.Noeud2.Id].Contains(lien.Noeud1.Id) == false)
                {
                    grapheOriente = true;
                }
            }

            if (grapheOriente == false)
            {
                taille /= 2;
            }

            Console.WriteLine();
            Console.WriteLine("Voici une analyse des propriétés du graphe :");
            Console.WriteLine();

            Console.WriteLine("-Ordre du graphe (=nombre de sommets) : " + ordre);
            Console.WriteLine();

            Console.WriteLine("-Taille du graphe (=nombre d'arêtes) : " + taille);
            Console.WriteLine();

            Console.WriteLine("-Le graphe est-il orienté ?");
            if (grapheOriente == true) 
            {
                Console.WriteLine("OUI : Le graphe est bien orienté");
            }
            else
            {
                Console.WriteLine("NON : Le graphe n'est pas orienté");
            }
            Console.WriteLine();

            Console.WriteLine("-Le graphe est-il pondéré ?");
            if (graphePondere == true)
            { 
                Console.WriteLine("OUI : Le graphe est bien pondéré");    
            }
            else
            {
                Console.WriteLine("NON : Le graphe n'est pas pondéré");
            }
            Console.WriteLine();
        }
    }
}
