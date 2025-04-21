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
            // Arrange & Act
            Graphe<int> graphe = new Graphe<int>();

            // Assert
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

        


    }
}
