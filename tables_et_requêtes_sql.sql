CREATE DATABASE IF NOT EXISTS livInParis;
USE livInParis;

CREATE TABLE CLIENT(
   id_client SMALLINT,
   nom VARCHAR(50),
   prénom VARCHAR(50),
   rue VARCHAR(50) NOT NULL,
   numero1 SMALLINT NOT NULL,
   code_Postal SMALLINT,
   tel SMALLINT,
   email VARCHAR(50),
   metro_le_plus_proche VARCHAR(50),
   PRIMARY KEY(id_client)
);

CREATE TABLE ENTREPRISE(
   ID_RESTO VARCHAR(50),
   Nom_resto VARCHAR(50),
   Type VARCHAR(50),
   capacité_resto DECIMAL(3,0),
   PRIMARY KEY(ID_RESTO)
);

CREATE TABLE CUISTOT(
   id_cuistot SMALLINT,
   Nom_cuistot VARCHAR(50),
   prénom_cuistot VARCHAR(50),
   rue_cuistot VARCHAR(50),
   numero_cuistot SMALLINT,
   code_postal_cuistot SMALLINT,
   ville_cuistot VARCHAR(50),
   tel_cuistot SMALLINT,
   email_cuistot VARCHAR(50),
   metro_le_plus_proche_cuistot VARCHAR(50),
   PRIMARY KEY(id_cuistot)
);

CREATE TABLE MENU(
   ID_MENU VARCHAR(50),
   entrèes VARCHAR(50),
   plat VARCHAR(50),
   dessert VARCHAR(50),
   boisson VARCHAR(50),
   type_de_régime_suivi VARCHAR(50),
   régime_alimentaires_particuliers VARCHAR(50),
   PRIMARY KEY(ID_MENU)
);

CREATE TABLE COMMANDE(
   ID_comande INT,
   numero_cuisiner SMALLINT,
   nom VARCHAR(50),
   prix DECIMAL(15,3),
   quantite SMALLINT,
   type_plat VARCHAR(50),
   date_de_fabrication VARCHAR(50),
   date_de_péremption VARCHAR(50),
   régime VARCHAR(50),
   nature VARCHAR(50),
   volume1 VARCHAR(50),
   ingredient2 VARCHAR(50),
   volume2 VARCHAR(50),
   ingredient3 VARCHAR(50),
   volume3 VARCHAR(50),
   PRIMARY KEY(ID_comande)
);

CREATE TABLE TRAJET(
   ID_TRAJET INT,
   Duree_Trajet_ TIME,
   destination VARCHAR(50),
   metro_utiles VARCHAR(50),
   PRIMARY KEY(ID_TRAJET)
);

CREATE TABLE INGREDIENT(
   ID_INGREDIENT_ INT,
   arachides BOOLEAN,
   halal BOOLEAN,
   casher BOOLEAN,
   origine_france BOOLEAN,
   lait BOOLEAN,
   sans_gluten VARCHAR(50),
   PRIMARY KEY(ID_INGREDIENT_)
);

CREATE TABLE Livreur(
   ID_Livreur VARCHAR(50),
   Nom_livreur VARCHAR(50),
   prénom_livreur VARCHAR(50),
   inscription DATE,
   note_livreur DECIMAL(1,0),
   PRIMARY KEY(ID_Livreur)
);

CREATE TABLE ADRESSE(
   id_adresse VARCHAR(50),
   code_postal VARCHAR(50),
   nom_ville VARCHAR(50),
   complément_d_adresse VARCHAR(50),
   étage SMALLINT,
   numéro_appartement DECIMAL(15,0),
   id_client SMALLINT NOT NULL,
   PRIMARY KEY(id_adresse),
   FOREIGN KEY(id_client) REFERENCES CLIENT(id_client)
);

CREATE TABLE lisent(
   id_client SMALLINT,
   ID_MENU VARCHAR(50),
   PRIMARY KEY(id_client, ID_MENU),
   FOREIGN KEY(id_client) REFERENCES CLIENT(id_client),
   FOREIGN KEY(ID_MENU) REFERENCES MENU(ID_MENU)
);

CREATE TABLE Transmettre(
   ID_RESTO VARCHAR(50),
   id_cuistot SMALLINT,
   ID_Livreur VARCHAR(50),
   PRIMARY KEY(ID_RESTO, id_cuistot, ID_Livreur),
   FOREIGN KEY(ID_RESTO) REFERENCES ENTREPRISE(ID_RESTO),
   FOREIGN KEY(id_cuistot) REFERENCES CUISTOT(id_cuistot),
   FOREIGN KEY(ID_Livreur) REFERENCES Livreur(ID_Livreur)
);

CREATE TABLE Transférer(
   ID_RESTO VARCHAR(50),
   ID_comande INT,
   PRIMARY KEY(ID_RESTO, ID_comande),
   FOREIGN KEY(ID_RESTO) REFERENCES ENTREPRISE(ID_RESTO),
   FOREIGN KEY(ID_comande) REFERENCES COMMANDE(ID_comande)
);

CREATE TABLE parcourt(
   ID_TRAJET INT,
   ID_Livreur VARCHAR(50),
   PRIMARY KEY(ID_TRAJET, ID_Livreur),
   FOREIGN KEY(ID_TRAJET) REFERENCES TRAJET(ID_TRAJET),
   FOREIGN KEY(ID_Livreur) REFERENCES Livreur(ID_Livreur)
);

CREATE TABLE contient(
   ID_MENU VARCHAR(50),
   ID_INGREDIENT_ INT,
   PRIMARY KEY(ID_MENU, ID_INGREDIENT_),
   FOREIGN KEY(ID_MENU) REFERENCES MENU(ID_MENU),
   FOREIGN KEY(ID_INGREDIENT_) REFERENCES INGREDIENT(ID_INGREDIENT_)
);

CREATE TABLE livre(
   id_client SMALLINT,
   ID_Livreur VARCHAR(50),
   PRIMARY KEY(id_client, ID_Livreur),
   FOREIGN KEY(id_client) REFERENCES CLIENT(id_client),
   FOREIGN KEY(ID_Livreur) REFERENCES Livreur(ID_Livreur)
);

CREATE TABLE passe(
   id_client SMALLINT,
   ID_comande INT,
   PRIMARY KEY(id_client, ID_comande),
   FOREIGN KEY(id_client) REFERENCES CLIENT(id_client),
   FOREIGN KEY(ID_comande) REFERENCES COMMANDE(ID_comande)
);

ALTER TABLE client
ADD ville VARCHAR(50);

ALTER TABLE COMMANDE
ADD numero_client VARCHAR(50);

ALTER TABLE COMMANDE
ADD ingredient1 VARCHAR(50);


ALTER TABLE client
MODIFY COLUMN code_Postal VARCHAR(50);

ALTER TABLE client
MODIFY COLUMN tel VARCHAR(50);

ALTER TABLE CUISTOT
MODIFY COLUMN code_postal_cuistot VARCHAR(50);

ALTER TABLE CUISTOT
MODIFY COLUMN tel_cuistot VARCHAR(50);

INSERT INTO Client (id_client, nom, prénom, rue, numero1, code_Postal, ville, tel, email, metro_le_plus_proche)
VALUES 
('1', 'Durand', 'Medhy', 'Rue Cardinet', '15', '75017', 'Paris','1234567890','Mdurand@gmail.com','Cardinet');

INSERT INTO COMMANDE ( ID_comande, numero_cuisiner, numero_client, nom, prix, quantite, type_plat, date_de_fabrication, date_de_péremption, régime, nature, ingredient1, volume1, ingredient2, volume2, ingredient3, volume3
)
VALUES
('1','1','1','raclette','10','6','plat','10/01/2025','15/01/2025','NULL','Française','raclette fromage','250g','pommes de terre','200g','jambon','200g');

INSERT INTO COMMANDE ( ID_comande, numero_cuisiner, numero_client, nom, prix, quantite, type_plat, date_de_fabrication, date_de_péremption, régime, nature, ingredient1, volume1, ingredient2, volume2, ingredient3, volume3
)
VALUES
('2','1','1','salade de fruit','5','6','dessert','10/01/2025','15/01/2025','végétarien','Indifférent','fraise','100g','kiwi','100g','sucre','10g');

INSERT INTO CUISTOT (id_cuistot, Nom_cuistot, prénom_cuistot, rue_cuistot, numero_cuistot, code_postal_cuistot, ville_cuistot, tel_cuistot, email_cuistot, metro_le_plus_proche_cuistot
) 
VALUES
('1','Dupond','Marie','Rue de la Republique','30','75011','Paris','1234567890','Mdupond@gmail.com','République');

SELECT * FROM CUISTOT
WHERE ville_cuistot = 'Paris';

SELECT * FROM COMMANDE
WHERE type_plat = 'Italien';

SELECT * FROM CUISTOT
WHERE tel_cuistot = 123456789;

SELECT * FROM CUISTOT
WHERE tel_cuistot = 1234567890;

UPDATE COMMANDE
SET prix = 14.50
WHERE ID_comande = 1;

SELECT * FROM COMMANDE
WHERE prix = 14.50;

SELECT * FROM CUISTOT
WHERE code_postal_cuistot > 75000;

SELECT * FROM COMMANDE
WHERE date_de_péremption > '2025-03-10';

SELECT * FROM COMMANDE
WHERE nom LIKE '%Pizza%';














