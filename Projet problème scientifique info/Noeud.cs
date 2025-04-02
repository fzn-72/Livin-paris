using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Un noeud représente un membre de l'association. On identifie un membre par un identifiant (id)
    /// </summary>
    public class Noeud<T>
    {
        private T id;

        /// <summary>
        /// Le constructeur naturel de la classe Noeud prend en argument l'identifiant unique du noeud 
        /// </summary>
        /// <param name="id"></param>
        public Noeud(T id, T valeur)
        {
            this.id = id;
            Valeur = valeur;
        }

        /// <summary>
        /// Cette propriété permet d'accéder à l'attribut "id" (générique) en lecture (dans d'autres classes)
        /// </summary>
        public T Id
        { 
            get
            {
                return id;
            }
        }

        public T Valeur { get; set; }

    }
}
