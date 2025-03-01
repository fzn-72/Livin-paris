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
        public void TestMethod1()
        {
            Graphe graphe = new Graphe(3);
            Assert.AreEqual(3, graphe.ListeAdjacence.Count);
        }

        /// <summary>
        /// Test unitaire qui vérifie que le poids d'un lien créé par défaut entre deux nœuds est bien initialisé à 0.
        /// </summary>
        [TestMethod]
        public void TestMethod2() 
        {
            Noeud noeud1 = new Noeud(1); 
            Noeud noeud2 = new Noeud(2);
            var lienDefault = new Lien(noeud1, noeud2);
            Assert.AreEqual(0, lienDefault.Poids);
        }

        /// <summary>
        /// Test unitaire qui vérifie l'ajout d'un lien entre deux nœuds dans le graphe, 
        /// en s'assurant que la liste d'adjacence et la matrice d'adjacence sont bien mises à jour. 
        /// De plus, le nombre de liens dans le graphe est vérifié ainsi que les attributs des liens créés.
        /// </summary>
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
