CREATE TABLE CLIENT(
   id_client VARCHAR(10),
   nom VARCHAR(50),
   prénom VARCHAR(50),
   rue VARCHAR(50) NOT NULL,
   code_Postal SMALLINT,
   tel SMALLINT,
   email VARCHAR(50),
   metro_le_plus_proche VARCHAR(50),
   ville VARCHAR(30),
   mot_de_passe VARCHAR(50),
   PRIMARY KEY(id_client)
);

CREATE TABLE RESTAURANT(
   ID_resto VARCHAR(50),
   nom_resto VARCHAR(50),
   type_resto VARCHAR(50),
   capacité_resto DECIMAL(3,0),
   PRIMARY KEY(ID_resto)
);

CREATE TABLE CUISINIER_(
   id_cuisinier VARCHAR(10),
   nom_cuisinier VARCHAR(50),
   prénom_cuisinier VARCHAR(50),
   rue_cuisinier VARCHAR(50),
   code_postal_cuisinier SMALLINT,
   ville_cuisinier VARCHAR(50),
   tel_cuisinier SMALLINT,
   email_cuisinier VARCHAR(50),
   metro_le_plus_proche_cuisinier VARCHAR(50),
   mot_de_passe VARCHAR(50),
   PRIMARY KEY(id_cuisinier)
);

CREATE TABLE MENU(
   id_menu VARCHAR(50),
   entrée VARCHAR(50),
   plat VARCHAR(50),
   dessert VARCHAR(50),
   boisson VARCHAR(50),
   régime_correspondant VARCHAR(50),
   ID_resto VARCHAR(50) NOT NULL,
   PRIMARY KEY(id_menu),
   FOREIGN KEY(ID_resto) REFERENCES RESTAURANT(ID_resto)
);

CREATE TABLE COMMANDE(
   ID_commande VARCHAR(10),
   numero_cuisiner SMALLINT,
   nom VARCHAR(50),
   prix DECIMAL(15,3),
   quantite SMALLINT,
   type_plat VARCHAR(50),
   date_de_fabrication DATE,
   date_de_péremption DATE,
   date_commande DATE,
   numero_client SMALLINT,
   régime VARCHAR(50),
   nature VARCHAR(50),
   volume1 VARCHAR(50),
   ingredient2 VARCHAR(50),
   volume2 VARCHAR(50),
   ingredient3 VARCHAR(50),
   volume3 VARCHAR(50),
   PRIMARY KEY(ID_commande)
);

CREATE TABLE TRAJET(
   id_trajet VARCHAR(10),
   duree_trajet_ TIME,
   date_trajet DATE,
   point_départ_trajet VARCHAR(50),
   distance_trajet VARCHAR(10),
   point_arrivee_trajet VARCHAR(50),
   PRIMARY KEY(id_trajet)
);

CREATE TABLE INGREDIENT(
   id_ingredient VARCHAR(10),
   nom_ingredient VARCHAR(50),
   régime_ingredient VARCHAR(100),
   origine_ingredient VARCHAR(50),
   type_ingredient VARCHAR(50),
   allergene_ingredient BOOLEAN,
   quantité_ingredient SMALLINT,
   stock_ingredient VARCHAR(50),
   PRIMARY KEY(id_ingredient)
);

CREATE TABLE LIVREUR(
   ID_livreur VARCHAR(50),
   nom_livreur VARCHAR(50),
   prénom_livreur VARCHAR(50),
   date_inscription DATE,
   note_livreur DECIMAL(1,1),
   email_livreur VARCHAR(50),
   tel_livreur SMALLINT,
   disponibilité BOOLEAN,
   mot_de_passe_livreur VARCHAR(50),
   PRIMARY KEY(ID_livreur)
);

CREATE TABLE ADRESSE(
   id_adresse VARCHAR(50),
   code_postal VARCHAR(50),
   nom_ville VARCHAR(50),
   complément_d_adresse VARCHAR(20),
   étage SMALLINT,
   numéro_appartement DECIMAL(15,0),
   id_client VARCHAR(10) NOT NULL,
   PRIMARY KEY(id_adresse),
   FOREIGN KEY(id_client) REFERENCES CLIENT(id_client)
);

CREATE TABLE lit(
   id_client VARCHAR(10),
   id_menu VARCHAR(50),
   PRIMARY KEY(id_client, id_menu),
   FOREIGN KEY(id_client) REFERENCES CLIENT(id_client),
   FOREIGN KEY(id_menu) REFERENCES MENU(id_menu)
);

CREATE TABLE emploie(
   ID_resto VARCHAR(50),
   id_cuisinier VARCHAR(10),
   ID_livreur VARCHAR(50),
   PRIMARY KEY(ID_resto, id_cuisinier, ID_livreur),
   FOREIGN KEY(ID_resto) REFERENCES RESTAURANT(ID_resto),
   FOREIGN KEY(id_cuisinier) REFERENCES CUISINIER_(id_cuisinier),
   FOREIGN KEY(ID_livreur) REFERENCES LIVREUR(ID_livreur)
);

CREATE TABLE réceptionne(
   ID_resto VARCHAR(50),
   ID_commande VARCHAR(10),
   PRIMARY KEY(ID_resto, ID_commande),
   FOREIGN KEY(ID_resto) REFERENCES RESTAURANT(ID_resto),
   FOREIGN KEY(ID_commande) REFERENCES COMMANDE(ID_commande)
);

CREATE TABLE parcourt(
   id_trajet VARCHAR(10),
   ID_livreur VARCHAR(50),
   PRIMARY KEY(id_trajet, ID_livreur),
   FOREIGN KEY(id_trajet) REFERENCES TRAJET(id_trajet),
   FOREIGN KEY(ID_livreur) REFERENCES LIVREUR(ID_livreur)
);

CREATE TABLE contient(
   id_menu VARCHAR(50),
   id_ingredient VARCHAR(10),
   PRIMARY KEY(id_menu, id_ingredient),
   FOREIGN KEY(id_menu) REFERENCES MENU(id_menu),
   FOREIGN KEY(id_ingredient) REFERENCES INGREDIENT(id_ingredient)
);

CREATE TABLE livre(
   id_client VARCHAR(10),
   ID_livreur VARCHAR(50),
   PRIMARY KEY(id_client, ID_livreur),
   FOREIGN KEY(id_client) REFERENCES CLIENT(id_client),
   FOREIGN KEY(ID_livreur) REFERENCES LIVREUR(ID_livreur)
);

CREATE TABLE passe(
   id_client VARCHAR(10),
   ID_commande VARCHAR(10),
   PRIMARY KEY(id_client, ID_commande),
   FOREIGN KEY(id_client) REFERENCES CLIENT(id_client),
   FOREIGN KEY(ID_commande) REFERENCES COMMANDE(ID_commande)
);

CREATE TABLE prépare(
   id_cuisinier VARCHAR(10),
   ID_commande VARCHAR(10),
   PRIMARY KEY(id_cuisinier, ID_commande),
   FOREIGN KEY(id_cuisinier) REFERENCES CUISINIER_(id_cuisinier),
   FOREIGN KEY(ID_commande) REFERENCES COMMANDE(ID_commande)
);
