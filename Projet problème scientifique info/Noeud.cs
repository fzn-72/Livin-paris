using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Un noeud représente un membre de l'association 
    /// </summary>
    internal class Noeud
    {
        private int id;
        
        public Noeud(int id)
        {
            this.id = id;
        }

        public int Id
        { 
            get
            {
                return id;
            }
        }

    }
}
