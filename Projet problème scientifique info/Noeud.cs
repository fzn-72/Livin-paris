using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Un noeud représente un membre de l'association, identifié par un identifiant unique (id).
    /// </summary>
    public class Noeud<T>
    {
        private T id;

        /// <summary>
        /// Constructeur de la classe Noeud. Initialise un noeud avec un identifiant unique et une valeur.
        /// </summary>
        /// <param name="id">Identifiant unique du noeud.</param>
        /// <param name="valeur">Valeur associée au noeud.</param>
        public Noeud(T id, T valeur)
        {
            this.id = id;
            Valeur = valeur;
        }

        /// <summary>
        /// Propriété permettant d'accéder à l'attribut "id" (générique) en lecture.
        /// </summary>
        public T Id
        {
            get
            {
                return id;
            }
        }

        /// <summary>
        /// Propriété permettant d'accéder ou de modifier la valeur associée au noeud.
        /// </summary>
        public T Valeur { get; set; }

        /// <summary>
        /// Redéfinit la méthode Equals pour comparer deux objets Noeud en fonction de leur identifiant.
        /// </summary>
        /// <param name="obj">Objet à comparer avec le noeud actuel.</param>
        /// <returns>True si les identifiants sont égaux, sinon False.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Noeud<T> other = (Noeud<T>)obj;
            return EqualityComparer<T>.Default.Equals(id, other.id);
        }

        /// <summary>
        /// Redéfinit la méthode GetHashCode pour obtenir un code de hachage basé sur l'identifiant du noeud.
        /// </summary>
        /// <returns>Code de hachage pour l'identifiant du noeud.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(id);
        }
    }
}
