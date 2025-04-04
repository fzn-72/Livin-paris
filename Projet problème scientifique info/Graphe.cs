using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Projet_problème_scientifique_info
{
    public class Graphe<T>
    {
        private Dictionary<T, List<T>> listeAdjacence;
        private int[,] matriceAdjacence;
        private int nbNoeuds;
        private List<Lien<T>> liensNoeuds;

        /// <summary>
        /// Constructeur par défaut de la classe Graphe. Initialise un graphe vide.
        /// </summary>
        public Graphe()
        {
            listeAdjacence = new Dictionary<T, List<T>>();
            liensNoeuds = new List<Lien<T>>();
        }

        #region Autre constructeur
        /// <summary>
        /// Constructeur de la classe Graphe. Initialise un graphe avec un nombre donné de nœuds.
        /// Crée une liste d'adjacence vide, une matrice d'adjacence de dimensions appropriées, et une liste de liens entre nœuds.
        /// </summary>
        /// <param name="nbNoeuds">Nombre de nœuds dans le graphe.</param>
        public Graphe(int nbNoeuds)
        {
            this.nbNoeuds = nbNoeuds;
            listeAdjacence = new Dictionary<T, List<T>>();
            liensNoeuds = new List<Lien<T>>();
            matriceAdjacence = new int[nbNoeuds, nbNoeuds];

            for (int i = 0; i < nbNoeuds; i++)
            {
                listeAdjacence.Add((T)Convert.ChangeType(i, typeof(T)), new List<T>());
            }
        }
        #endregion

        /// <summary>
        /// Propriété permettant d'accéder à l'attribut "listeAdjacence" (dictionnaire) en lecture.
        /// </summary>
        public Dictionary<T, List<T>> ListeAdjacence
        {
            get { return listeAdjacence; }
        }

        /// <summary>
        /// Propriété permettant d'accéder à l'attribut "liensNoeuds" (liste) en lecture.
        /// </summary>
        public List<Lien<T>> LiensNoeuds
        {
            get { return liensNoeuds; }
        }

        /// <summary>
        /// Propriété permettant d'accéder à l'attribut "matriceAdjacence" (matrice) en lecture.
        /// </summary>
        public int[,] MatriceAdjacence
        {
            get
            {
                return matriceAdjacence;
            }
        }

        /// <summary>
        /// Ajoute un lien entre les nœuds noeud1 et noeud2 dans la structure du graphe.
        /// Vérifie et initialise les listes d'adjacence si nécessaire, puis ajoute la connexion en évitant les doublons.
        /// Met également à jour la matrice d'adjacence pour représenter cette connexion et enregistre le lien dans la liste des "Lien".
        /// </summary>
        public void AjouterLien(T noeud1, T noeud2)
        {
            if (!listeAdjacence.ContainsKey(noeud1))
            {
                listeAdjacence[noeud1] = new List<T>();
            }
            if (!listeAdjacence.ContainsKey(noeud2))
            {
                listeAdjacence[noeud2] = new List<T>();
            }

            if (!listeAdjacence[noeud1].Contains(noeud2) && !listeAdjacence[noeud2].Contains(noeud1))
            {
                listeAdjacence[noeud1].Add(noeud2);
                listeAdjacence[noeud2].Add(noeud1);
            }

            if (noeud1 is int && noeud2 is int)
            {
                int i = Convert.ToInt32(noeud1);
                int j = Convert.ToInt32(noeud2);
                if (matriceAdjacence[i, j] == 0 && matriceAdjacence[j, i] == 0)
                {
                    matriceAdjacence[i, j] = 1;
                    matriceAdjacence[j, i] = 1;
                }
            }
            Noeud<T> noeud1Obj = new Noeud<T>(noeud1, noeud1);
            Noeud<T> noeud2Obj = new Noeud<T>(noeud2, noeud2);

            Lien<T> nouveauLien = new Lien<T>(noeud1Obj, noeud2Obj);
            liensNoeuds.Add(nouveauLien);
        }

        /// <summary>
        /// Charge les liens du graphe depuis un fichier spécifié.
        /// Chaque ligne du fichier doit contenir deux entiers séparés par un espace, représentant deux nœuds à relier.
        /// Les lignes vides et les commentaires (commençant par '%') sont ignorés.
        /// </summary>
        /// <param name="chemin">Chemin du fichier contenant les liens.</param>
        /// <param name="convert">Fonction de conversion des éléments lus en type T.</param>
        public void ChargerDepuisFichier(string chemin, Func<string, T> convert)
        {
            foreach (string ligne in File.ReadLines(chemin))
            {
                if (ligne.Length > 0 && ligne[0] != '%')
                {
                    string[] elements = ligne.Split(' ');
                    if (elements.Length == 2)
                    {
                        T noeud1 = convert(elements[0]);
                        T noeud2 = convert(elements[1]);
                        AjouterLien(noeud1, noeud2);
                    }
                }
            }
        }

        /// <summary>
        /// Effectue un parcours en largeur (BFS) dans le graphe à partir d'un sommet de départ spécifié.
        /// Explore tous les nœuds atteignables en partant du sommet donné, en parcourant chaque niveau de voisinage avant de passer au suivant.
        /// Les nœuds sont marqués comme visités pour éviter de les explorer plusieurs fois.
        /// </summary>
        /// <param name="sommetDepart">Sommet de départ pour le parcours.</param>
        public void ParcoursLargeur(T sommetDepart)
        {
            Dictionary<T, bool> visite = new Dictionary<T, bool>();
            Queue<T> file = new Queue<T>();

            file.Enqueue(sommetDepart);
            visite[sommetDepart] = true;

            while (file.Count > 0)
            {
                T noeud = file.Dequeue();
                Console.Write(noeud + " ");

                if (listeAdjacence.ContainsKey(noeud))
                {
                    foreach (T voisin in listeAdjacence[noeud])
                    {
                        if (!visite.ContainsKey(voisin))
                        {
                            file.Enqueue(voisin);
                            visite[voisin] = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Effectue un parcours en profondeur (DFS) du graphe à partir d'un sommet donné.
        /// Les sommets sont visités dans l'ordre où ils sont rencontrés, et chaque sommet est marqué comme visité pour éviter les cycles.
        /// Les sommets voisins sont ajoutés à la pile et explorés de manière récursive.
        /// </summary>
        /// <param name="sommetDepart">Sommet de départ pour le parcours.</param>
        public void ParcoursProfondeur(T sommetDepart)
        {
            Dictionary<T, bool> visite = new Dictionary<T, bool>();
            Stack<T> pile = new Stack<T>();

            foreach (var sommet in listeAdjacence.Keys)
            {
                visite[sommet] = false;
            }

            pile.Push(sommetDepart);

            while (pile.Count > 0)
            {
                T noeud = pile.Pop();

                if (!visite[noeud])
                {
                    Console.Write(noeud + " ");
                    visite[noeud] = true;

                    foreach (T voisin in listeAdjacence[noeud])
                    {
                        if (!visite[voisin])
                        {
                            pile.Push(voisin);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Vérifie si le graphe est connexe, c'est-à-dire si tous les sommets sont accessibles à partir de n'importe quel sommet.
        /// Utilise un parcours en largeur (BFS) pour explorer les sommets du graphe et vérifier si tous les sommets sont visités.
        /// </summary>
        /// <returns>True si le graphe est connexe, sinon False.</returns>
        public bool EstConnexe()
        {
            if (listeAdjacence.Count == 0)
                return true;

            Dictionary<T, bool> visite = new Dictionary<T, bool>();
            Queue<T> file = new Queue<T>();

            T premierNoeud = listeAdjacence.Keys.First();
            file.Enqueue(premierNoeud);
            visite[premierNoeud] = true;

            while (file.Count > 0)
            {
                T noeud = file.Dequeue();

                foreach (T voisin in listeAdjacence[noeud])
                {
                    if (!visite.ContainsKey(voisin) || !visite[voisin])
                    {
                        file.Enqueue(voisin);
                        visite[voisin] = true;
                    }
                }
            }

            return visite.Count == listeAdjacence.Count;
        }

        /// <summary>
        /// Vérifie si un cycle existe dans le graphe en utilisant une recherche en profondeur (DFS).
        /// Explore les nœuds du graphe en profondeur tout en suivant les arêtes. Si elle rencontre un nœud déjà visité
        /// qui n'est pas le parent immédiat du nœud courant, cela indique la présence d'un cycle.
        /// </summary>
        /// <param name="noeud">Nœud actuel dans le parcours.</param>
        /// <param name="parent">Parent du nœud actuel.</param>
        /// <param name="visite">Dictionnaire des nœuds visités.</param>
        /// <returns>True si un cycle est détecté, sinon False.</returns>
        private bool ContientCycleDFS(T noeud, T parent, Dictionary<T, bool> visite)
        {
            visite[noeud] = true;
            foreach (T voisin in listeAdjacence[noeud])
            {
                if (!visite.ContainsKey(voisin) || !visite[voisin])
                {
                    if (ContientCycleDFS(voisin, noeud, visite))
                    {
                        return true;
                    }
                }
                else if (!EqualityComparer<T>.Default.Equals(voisin, parent))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Vérifie si le graphe contient un cycle en utilisant une recherche en profondeur (DFS).
        /// Explore tous les nœuds du graphe et pour chaque nœud non visité, appelle la méthode `ContientCycleDFS`
        /// pour vérifier la présence d'un cycle dans le sous-graphe auquel ce nœud appartient.
        /// </summary>
        /// <returns>True si un cycle est détecté, sinon False.</returns>
        public bool ContientCycle()
        {
            Dictionary<T, bool> visite = new Dictionary<T, bool>();

            foreach (T noeud in listeAdjacence.Keys)
            {
                if (!visite.ContainsKey(noeud) || !visite[noeud])
                {
                    if (ContientCycleDFS(noeud, default(T), visite))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Analyse les propriétés du graphe, telles que son ordre (nombre de sommets), sa taille (nombre d'arêtes),
        /// s'il est orienté et s'il est pondéré. Affiche ces informations dans la console.
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
                if (!listeAdjacence[lien.Noeud2.Id].Contains(lien.Noeud1.Id))
                {
                    grapheOriente = true;
                }
            }

            if (!grapheOriente)
            {
                taille /= 2;
            }

            Console.WriteLine();
            Console.WriteLine("Voici une analyse des propriétés du graphe :");
            Console.WriteLine();

            Console.WriteLine("- Ordre du graphe (=nombre de sommets) : " + ordre);
            Console.WriteLine();

            Console.WriteLine("- Taille du graphe (=nombre d'arêtes) : " + taille);
            Console.WriteLine();

            Console.WriteLine("- Le graphe est-il orienté ?");
            if (grapheOriente)
            {
                Console.WriteLine("OUI : Le graphe est bien orienté");
            }
            else
            {
                Console.WriteLine("NON : Le graphe n'est pas orienté");
            }
            Console.WriteLine();

            Console.WriteLine("- Le graphe est-il pondéré ?");
            if (graphePondere)
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
