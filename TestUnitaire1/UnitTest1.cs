using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUnitaire1
{
    [TestClass]
    public class UnitTest1
    {
        private Graphe graphe;

        [TestInitialize]
        public void SetUp()
        {
            // Initialisation d'un graphe avec 3 nœuds
            graphe = new Graphe(3);
        }

        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(3, graphe.ListeAdjacence.Count);
        }
    }
}
