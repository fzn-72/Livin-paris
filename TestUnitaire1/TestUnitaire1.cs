using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Projet_problème_scientifique_info
{
    [TestClass]
    public class TestUnitaire1
    {

        [TestMethod]
        public void TestMethod1()
        {
            Graphe graphe = new Graphe(3);
            Assert.AreEqual(3, graphe.ListeAdjacence.Count);
        }

        [TestMethod]
        public void TestMethod2() 
        {
            Noeud noeud1 = new Noeud(1); 
            Noeud noeud2 = new Noeud(2);
            var lienDefault = new Lien(noeud1, noeud2);
            Assert.AreEqual(0, lienDefault.Poids);
        }

        [TestMethod]
        public void TestMethod3() 
        {
            Graphe graphe = new Graphe(3);
            graphe.AjouterLien(0, 1);

            Assert.IsTrue(graphe.ListeAdjacence.ContainsKey(0));
            Assert.IsTrue(graphe.ListeAdjacence[0].Contains(1));

            Assert.AreEqual(1, graphe.MatriceAdjacence[0, 1]);
            Assert.AreEqual(1, graphe.MatriceAdjacence[1, 0]);

            Assert.AreEqual(1, graphe.LiensNoeuds.Count);
            Assert.AreEqual(0, graphe.LiensNoeuds[0].Noeud1.Id);
        }
    }
}
