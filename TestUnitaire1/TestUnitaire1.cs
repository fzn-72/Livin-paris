using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Projet_problème_scientifique_info
{
    [TestClass]
    public class TestUnitaire1
    {
        /// <summary>
        /// Test unitaire qui vérifie que le graphe créé avec 3 nœuds contient bien 3 éléments dans sa liste d'adjacence
        /// </summary>
        [TestMethod]
        public void TestMethod2()
        {
            Noeud<int> noeud1 = new Noeud<int>(1, 10); 
            Noeud<int> noeud2 = new Noeud<int>(2, 20);
            var lienDefault = new Lien<int>(noeud1, noeud2);
            Assert.AreEqual(0, lienDefault.Poids);
        }

        [TestMethod]
        public void TestLienInitialisation()
        {          
            var noeud1 = new Noeud<int>(1, 10);
            var noeud2 = new Noeud<int>(2, 20);
            int poids = 5;
          
            var lien = new Lien<int>(noeud1, noeud2, poids);
            
            Assert.AreEqual(noeud1, lien.Noeud1);
            Assert.AreEqual(noeud2, lien.Noeud2);
            Assert.AreEqual(poids, lien.Poids);
        }

        [TestMethod]
        public void TestStationToString()
        {     
            int id = 1;
            string ligne = "Ligne 1";
            string nom = "Station Test";
            double longitude = 2.35;
            double latitude = 48.85;
            string commune = "Paris";
            string codeInsee = "75056";
       
            var station = new Station(id, ligne, nom, longitude, latitude, commune, codeInsee);
            string result = station.ToString();
          
            Assert.AreEqual($"Station {id}: {nom} (Ligne {ligne})", result);
        }


        [TestMethod]
        public void TestConstructeurParDefaut()
        {
            Graphe<int> graphe = new Graphe<int>();

            Assert.IsNotNull(graphe.ListeAdjacence);
            Assert.IsNotNull(graphe.LiensNoeuds);
            Assert.AreEqual(0, graphe.ListeAdjacence.Count);
            Assert.AreEqual(0, graphe.LiensNoeuds.Count);
        }


        [TestMethod]
        public void TestConstructeurAvecNoeuds()
        {          
            int nbNoeuds = 5;
            Graphe<int> graphe = new Graphe<int>(nbNoeuds);
           
            Assert.AreEqual(nbNoeuds, graphe.ListeAdjacence.Count);
            Assert.AreEqual(nbNoeuds, graphe.MatriceAdjacence.GetLength(0));
        }

        [TestMethod]
        public void TestKruskalGrapheSimple()
        {
            var noeud1 = new Noeud<int>(1, 1);
            var noeud2 = new Noeud<int>(2, 2);
            var noeud3 = new Noeud<int>(3, 3);
            var noeud4 = new Noeud<int>(4, 4);

            var noeuds = new List<Noeud<int>> { noeud1, noeud2, noeud3, noeud4 };

            var lien1 = new Lien<int>(noeud1, noeud2, 10);
            var lien2 = new Lien<int>(noeud1, noeud3, 6);
            var lien3 = new Lien<int>(noeud1, noeud4, 5);
            var lien4 = new Lien<int>(noeud2, noeud4, 15);
            var lien5 = new Lien<int>(noeud3, noeud4, 4);

            var liens = new List<Lien<int>> { lien1, lien2, lien3, lien4, lien5 };

            var chemin = CalculDistance.KruskalArbreCouvranceMinimal(noeud1, noeud4, noeuds, liens);

            Assert.IsNotNull(chemin);
            Assert.IsTrue(chemin.Count > 0);
            Assert.AreEqual(noeud1, chemin.First());
            Assert.AreEqual(noeud4, chemin.Last());

            for (int i = 0; i < chemin.Count - 1; i++)  /// Vérifier que le chemin est continu (chaque nœud est connecté au suivant)
            {
                bool estConnecte = liens.Any(l =>
                    (l.Noeud1.Equals(chemin[i]) && l.Noeud2.Equals(chemin[i + 1])) ||
                    (l.Noeud2.Equals(chemin[i]) && l.Noeud1.Equals(chemin[i + 1]))
                );
                Assert.IsTrue(estConnecte, $"Les nœuds {chemin[i].Id} et {chemin[i + 1].Id} ne sont pas connectés");
            }
        }

        [TestMethod]
        public void TestKruskalCheminDirect()
        {
            
            var noeud1 = new Noeud<int>(1, 1);
            var noeud2 = new Noeud<int>(2, 2);
            var noeud3 = new Noeud<int>(3, 3);

            var noeuds = new List<Noeud<int>> { noeud1, noeud2, noeud3 };

            var lien1 = new Lien<int>(noeud1, noeud2, 5);
            var lien2 = new Lien<int>(noeud2, noeud3, 10);
            var lien3 = new Lien<int>(noeud1, noeud3, 20);

            var liens = new List<Lien<int>> { lien1, lien2, lien3 };

            var chemin = CalculDistance.KruskalArbreCouvranceMinimal(noeud1, noeud3, noeuds, liens);

            Assert.IsNotNull(chemin);
            Assert.AreEqual(3, chemin.Count); 
            Assert.AreEqual(noeud1, chemin[0]);
            Assert.AreEqual(noeud2, chemin[1]);
            Assert.AreEqual(noeud3, chemin[2]);
        }

        [TestMethod]
        public void TestKruskalGrapheDeconnecte()
        {
            var noeud1 = new Noeud<int>(1, 1);
            var noeud2 = new Noeud<int>(2, 2);
            var noeud3 = new Noeud<int>(3, 3);
            var noeud4 = new Noeud<int>(4, 4);

            var noeuds = new List<Noeud<int>> { noeud1, noeud2, noeud3, noeud4 };

            var lien1 = new Lien<int>(noeud1, noeud2, 10);
            var lien2 = new Lien<int>(noeud3, noeud4, 5);

            var liens = new List<Lien<int>> { lien1, lien2 };

            var chemin = CalculDistance.KruskalArbreCouvranceMinimal(noeud1, noeud4, noeuds, liens);

            Assert.IsNotNull(chemin);
            Assert.AreEqual(0, chemin.Count); 
        }

        [TestMethod]
        public void TestKruskalGrapheAvecCycles()
        {
            var noeud1 = new Noeud<int>(1, 1);
            var noeud2 = new Noeud<int>(2, 2);
            var noeud3 = new Noeud<int>(3, 3);
            var noeud4 = new Noeud<int>(4, 4);

            var noeuds = new List<Noeud<int>> { noeud1, noeud2, noeud3, noeud4 };

            var lien1 = new Lien<int>(noeud1, noeud2, 10); /// Créer un graphe avec un cycle: 1-2-3-4-1
            var lien2 = new Lien<int>(noeud2, noeud3, 5);
            var lien3 = new Lien<int>(noeud3, noeud4, 8);
            var lien4 = new Lien<int>(noeud4, noeud1, 15);
            var lien5 = new Lien<int>(noeud1, noeud3, 20); 

            var liens = new List<Lien<int>> { lien1, lien2, lien3, lien4, lien5 };
        
            var chemin = CalculDistance.KruskalArbreCouvranceMinimal(noeud1, noeud4, noeuds, liens);

            Assert.IsNotNull(chemin);
            Assert.IsTrue(chemin.Count > 0);
            Assert.AreEqual(noeud1, chemin.First());
            Assert.AreEqual(noeud4, chemin.Last());

            var aretes = new HashSet<(int, int)>();  /// Vérifier que l'APCM contient exactement n-1 arêtes (3 arêtes pour 4 nœuds)
            for (int i = 0; i < chemin.Count - 1; i++)
            {
                int min = System.Math.Min(chemin[i].Id, chemin[i + 1].Id);
                int max = System.Math.Max(chemin[i].Id, chemin[i + 1].Id);
                aretes.Add((min, max));
            }

            Assert.IsTrue(aretes.Count <= noeuds.Count - 1);  /// Le chemin peut contenir moins d'arêtes que l'ACM complet
        }

        [TestMethod]
        public void TestKruskalCheminOptimal()
        {
            var noeud1 = new Noeud<int>(1, 1);
            var noeud2 = new Noeud<int>(2, 2);
            var noeud3 = new Noeud<int>(3, 3);
            var noeud4 = new Noeud<int>(4, 4);
            var noeud5 = new Noeud<int>(5, 5);

            var noeuds = new List<Noeud<int>> { noeud1, noeud2, noeud3, noeud4, noeud5 };

            var lien1 = new Lien<int>(noeud1, noeud2, 10);  /// Créer un graphe où le chemin optimal est 1-3-5
            var lien2 = new Lien<int>(noeud2, noeud5, 8);
            var lien3 = new Lien<int>(noeud1, noeud3, 3);
            var lien4 = new Lien<int>(noeud3, noeud5, 2);
            var lien5 = new Lien<int>(noeud3, noeud4, 7);
            var lien6 = new Lien<int>(noeud4, noeud5, 5);

            var liens = new List<Lien<int>> { lien1, lien2, lien3, lien4, lien5, lien6 };

            var chemin = CalculDistance.KruskalArbreCouvranceMinimal(noeud1, noeud5, noeuds, liens);

            Assert.IsNotNull(chemin);
            Assert.AreEqual(3, chemin.Count); /// Le chemin optimal devrait être 1-3-5
            Assert.AreEqual(noeud1, chemin[0]);
            Assert.AreEqual(noeud3, chemin[1]);
            Assert.AreEqual(noeud5, chemin[2]);
        }

        [TestMethod]
        public void TestKruskalNoeudsIdentiques()
        {
            var noeud1 = new Noeud<int>(1, 1);
            var noeud2 = new Noeud<int>(2, 2);
            var noeud3 = new Noeud<int>(3, 3);

            var noeuds = new List<Noeud<int>> { noeud1, noeud2, noeud3 };

            var lien1 = new Lien<int>(noeud1, noeud2, 5);
            var lien2 = new Lien<int>(noeud2, noeud3, 10);

            var liens = new List<Lien<int>> { lien1, lien2 };

            var chemin = CalculDistance.KruskalArbreCouvranceMinimal(noeud1, noeud1, noeuds, liens);

            Assert.IsNotNull(chemin);
            Assert.AreEqual(1, chemin.Count); 
            Assert.AreEqual(noeud1, chemin[0]);
        }

        [TestMethod]
        public void TestKruskalGrapheVide()
        {
            var noeud1 = new Noeud<int>(1, 1);
            var noeud2 = new Noeud<int>(2, 2);

            var noeuds = new List<Noeud<int>> { noeud1, noeud2 };
            var liens = new List<Lien<int>>();

            var chemin = CalculDistance.KruskalArbreCouvranceMinimal(noeud1, noeud2, noeuds, liens);

            Assert.IsNotNull(chemin);
            Assert.AreEqual(0, chemin.Count);
        }

        [TestMethod]
        public void TestKruskalPoidsNegatifs()
        {
            var noeud1 = new Noeud<int>(1, 1);
            var noeud2 = new Noeud<int>(2, 2);
            var noeud3 = new Noeud<int>(3, 3);

            var noeuds = new List<Noeud<int>> { noeud1, noeud2, noeud3 };

            var lien1 = new Lien<int>(noeud1, noeud2, -5);
            var lien2 = new Lien<int>(noeud2, noeud3, 10);
            var lien3 = new Lien<int>(noeud1, noeud3, 20);

            var liens = new List<Lien<int>> { lien1, lien2, lien3 };

            var chemin = CalculDistance.KruskalArbreCouvranceMinimal(noeud1, noeud3, noeuds, liens);

            Assert.IsNotNull(chemin);
            Assert.AreEqual(3, chemin.Count); /// noeud1 -> noeud2 -> noeud3
            Assert.AreEqual(noeud1, chemin[0]);
            Assert.AreEqual(noeud2, chemin[1]);
            Assert.AreEqual(noeud3, chemin[2]);
        }

        [TestMethod]
        public void TestKruskalPerformance()
        {
            int nbNoeuds = 100;
            var noeuds = new List<Noeud<int>>();
            var liens = new List<Lien<int>>();

            for (int i = 1; i <= nbNoeuds; i++)
            {
                noeuds.Add(new Noeud<int>(i, i));
            }

            for (int i = 0; i < nbNoeuds; i++)
            {
                for (int j = i + 1; j < nbNoeuds; j++)
                {
                    if (j == i + 1 || j == i + 10) /// Connecter aux voisins proches
                    {
                        liens.Add(new Lien<int>(noeuds[i], noeuds[j], (i * j) % 50));
                    }
                }
            }

            var debut = System.DateTime.Now;
            var chemin = CalculDistance.KruskalArbreCouvranceMinimal(noeuds[0], noeuds[nbNoeuds - 1], noeuds, liens);
            var fin = System.DateTime.Now;
            var duree = (fin - debut).TotalMilliseconds;

            Assert.IsNotNull(chemin);
            Assert.IsTrue(chemin.Count > 0);
            Assert.IsTrue(duree < 1000, $"L'algorithme a pris {duree}ms, ce qui est trop long");
        }



    }
}
