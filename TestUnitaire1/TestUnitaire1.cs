using System;
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
        public void TestMethod3()
        {
            Graphe<int> graphe = new Graphe<int>();
            graphe.AjouterLien(0, 1);
            graphe.AjouterLien(1, 2);
            graphe.AjouterLien(0, 2);

            Assert.IsTrue(graphe.ListeAdjacence.ContainsKey(0));
            Assert.IsTrue(graphe.ListeAdjacence[0].Contains(1));

            Assert.AreEqual(1, graphe.MatriceAdjacence[0, 1]);
            Assert.AreEqual(1, graphe.MatriceAdjacence[1, 0]);

            Assert.AreEqual(1, graphe.LiensNoeuds.Count);
        }

    }
}
