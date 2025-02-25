using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_problème_scientifique_info
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graphe g = new Graphe(34);

            g.AjouterLien(1, 0);
            g.AjouterLien(2, 0);
            g.AjouterLien(3, 0);
            g.AjouterLien(4, 0);
        }
    }
}
