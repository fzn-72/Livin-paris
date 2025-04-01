using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_problème_scientifique_info
{
    /// <summary>
    /// Cette classe représente une station de métro avec ses coordonnées géographiques et ses informations administratives.
    /// </summary>
    public class Station
    {
        private int id;
        private string ligne;
        private string nom;
        private double longitude;
        private double latitude;
        private string commune;
        private string codeInsee;

        /// <summary>
        /// Constructeur de la classe Station qui initialise toutes les propriétés d'une station de métro.
        /// </summary>
        /// <param name="id">Identifiant unique de la station</param>
        /// <param name="ligne">Numéro de la ligne de métro</param>
        /// <param name="nom">Nom de la station</param>
        /// <param name="longitude">Coordonnée de longitude</param>
        /// <param name="latitude">Coordonnée de latitude</param>
        /// <param name="commune">Nom de la commune</param>
        /// <param name="codeInsee">Code INSEE de la commune</param>
        public Station(int id, string ligne, string nom, double longitude, double latitude, string commune, string codeInsee)
        {
            this.id = id;
            this.ligne = ligne;
            this.nom = nom;
            this.longitude = longitude;
            this.latitude = latitude;
            this.commune = commune;
            this.codeInsee = codeInsee;
        }

        /// <summary>
        /// Propriété permettant d'accéder à l'identifiant de la station
        /// </summary>
        public int Id
        {
            get { return id; }
        }

        /// <summary>
        /// Propriété permettant d'accéder au numéro de ligne
        /// </summary>
        public string Ligne
        {
            get { return ligne; }
        }

        /// <summary>
        /// Propriété permettant d'accéder au nom de la station
        /// </summary>
        public string Nom
        {
            get { return nom; }
        }

        /// <summary>
        /// Propriété permettant d'accéder à la longitude de la station
        /// </summary>
        public double Longitude
        {
            get { return longitude; }
        }

        /// <summary>
        /// Propriété permettant d'accéder à la latitude de la station
        /// </summary>
        public double Latitude
        {
            get { return latitude; }
        }

        /// <summary>
        /// Propriété permettant d'accéder au nom de la commune
        /// </summary>
        public string Commune
        {
            get { return commune; }
        }

        /// <summary>
        /// Propriété permettant d'accéder au code INSEE de la commune
        /// </summary>
        public string CodeInsee
        {
            get { return codeInsee; }
        }

        /// <summary>
        /// Surcharge de la méthode ToString pour afficher les informations de la station de manière lisible
        /// </summary>
        /// <returns>Une chaîne de caractères représentant la station</returns>
        public override string ToString()
        {
            return $"Station {id}: {nom} (Ligne {ligne})";
        }
    }
}