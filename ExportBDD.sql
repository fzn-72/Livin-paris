-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: localhost    Database: livinparis
-- ------------------------------------------------------
-- Server version	8.0.40

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `admin`
--

DROP TABLE IF EXISTS `admin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `admin` (
  `id_admin` int NOT NULL AUTO_INCREMENT,
  `nom` varchar(50) DEFAULT NULL,
  `prenom` varchar(50) DEFAULT NULL,
  `email` varchar(50) DEFAULT NULL,
  `mot_de_passe` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_admin`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `admin`
--

LOCK TABLES `admin` WRITE;
/*!40000 ALTER TABLE `admin` DISABLE KEYS */;
INSERT INTO `admin` VALUES (1,'Administrateur','Système','admin@livinparis.com','admin123'),(2,'NORAT','Faizan','faiz@livinparis.com','Faiz'),(3,'NAUDY','Thomas','naud@livinparis.com','Jackno');
/*!40000 ALTER TABLE `admin` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `adresse`
--

DROP TABLE IF EXISTS `adresse`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `adresse` (
  `id_adresse` varchar(50) NOT NULL,
  `code_postal` varchar(50) DEFAULT NULL,
  `nom_ville` varchar(50) DEFAULT NULL,
  `complément_d_adresse` varchar(50) DEFAULT NULL,
  `étage` smallint DEFAULT NULL,
  `numéro_appartement` decimal(15,0) DEFAULT NULL,
  `id_client` smallint NOT NULL,
  PRIMARY KEY (`id_adresse`),
  KEY `id_client` (`id_client`),
  CONSTRAINT `adresse_ibfk_1` FOREIGN KEY (`id_client`) REFERENCES `client` (`id_client`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `adresse`
--

LOCK TABLES `adresse` WRITE;
/*!40000 ALTER TABLE `adresse` DISABLE KEYS */;
INSERT INTO `adresse` VALUES ('A001','75012','Paris','12Bis Rue des Lilas',3,37,2);
/*!40000 ALTER TABLE `adresse` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `client`
--

DROP TABLE IF EXISTS `client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `client` (
  `id_client` smallint NOT NULL,
  `nom` varchar(50) DEFAULT NULL,
  `prénom` varchar(50) DEFAULT NULL,
  `rue` varchar(50) NOT NULL,
  `code_Postal` varchar(50) DEFAULT NULL,
  `tel` varchar(50) DEFAULT NULL,
  `email` varchar(50) DEFAULT NULL,
  `metro_le_plus_proche` varchar(50) DEFAULT NULL,
  `ville` varchar(50) DEFAULT NULL,
  `mot_de_passe` varchar(50) DEFAULT NULL,
  `type_client` varchar(20) DEFAULT NULL,
  `nom_entreprise` varchar(100) DEFAULT NULL,
  `nom_referent` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id_client`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `client`
--

LOCK TABLES `client` WRITE;
/*!40000 ALTER TABLE `client` DISABLE KEYS */;
INSERT INTO `client` VALUES (1,'Durand','Medhy','122 Rue Cardinet','75017','1234567890','medhy.durand@email.com','Pont Cardinet','Paris','Medhy',NULL,NULL,NULL),(2,'Dupont','Lucas','12 Rue des Lilas','75012','0612345678','lucas.dupont@email.com','Nation','Paris','Lucas',NULL,NULL,NULL),(3,'Martin','Emma','45 Avenue Victor Hugo','75016','0623456789','emma.martin@email.com','Bellecour','Paris','Emma',NULL,NULL,NULL),(4,'Lefevre','Noah','8 Boulevard Haussmann','75017','0634567890','noah.lefevre@email.com','Opera','Paris','Noah',NULL,NULL,NULL),(6,'Morel','Gabriel','5 Place Stanislas','75014','0656789012','gabriel.morel@email.com','Nancy Centre','Paris','Gabriel',NULL,NULL,NULL),(7,'Bernard','Chloé','17 Rue de Bretagne','75015','0667890123','chloe.bernard@email.com','Republique','Paris','Chloé',NULL,NULL,NULL),(8,'Robert','Hugo','22 Allée des Acacias','75013','0678901234','hugo.robert@email.com','Gambetta','Paris','Hugo',NULL,NULL,NULL),(9,'Petit','Manon','14 Quai de la Tournelle','75005','0689012345','manon.petit@email.com','Maubert-Mutualite','Paris','Manon',NULL,NULL,NULL),(10,'Richard','Nathan','19 Rue Saint-Michel','75012','0690123456','nathan.richard@email.com','Capitole','Paris','Nathan',NULL,NULL,NULL),(11,'Garcia','Camille','50 Avenue Jean Jaurès','75011','0611121314','camille.garcia@email.com','Comedie','Paris','Camille',NULL,NULL,NULL),(12,'Renaud','Sacha','22 Allée des Acacias ','75017','0678901234','sacha.renaud@email.com','Gambetta','Paris','Sacha',NULL,NULL,NULL),(13,'Carpentier','Elodie','50 Boulevard Haussmann','75009','0154789632','info@techsolutionsparis.com','Chaussee d\'Antin - La Fayette','Paris','Elodie','Entreprise','Tech Solutions Paris','Claire Dubois');
/*!40000 ALTER TABLE `client` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `commande`
--

DROP TABLE IF EXISTS `commande`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `commande` (
  `ID_commande` varchar(10) NOT NULL,
  `numero_cuisiner` smallint DEFAULT NULL,
  `nom` varchar(50) DEFAULT NULL,
  `prix` varchar(10) DEFAULT NULL,
  `quantite` smallint DEFAULT NULL,
  `type_plat` varchar(50) DEFAULT NULL,
  `date_de_fabrication` varchar(50) DEFAULT NULL,
  `date_de_péremption` varchar(50) DEFAULT NULL,
  `régime` varchar(50) DEFAULT NULL,
  `nature` varchar(50) DEFAULT NULL,
  `volume1` varchar(50) DEFAULT NULL,
  `ingredient2` varchar(50) DEFAULT NULL,
  `volume2` varchar(50) DEFAULT NULL,
  `ingredient3` varchar(50) DEFAULT NULL,
  `volume3` varchar(50) DEFAULT NULL,
  `numero_client` varchar(50) DEFAULT NULL,
  `ingredient1` varchar(50) DEFAULT NULL,
  `date_commande` varchar(50) DEFAULT NULL,
  `date_livraison` date DEFAULT NULL,
  `date_paiement` datetime DEFAULT NULL,
  PRIMARY KEY (`ID_commande`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `commande`
--

LOCK TABLES `commande` WRITE;
/*!40000 ALTER TABLE `commande` DISABLE KEYS */;
INSERT INTO `commande` VALUES ('1',1,'raclette','14.500',6,'plat','2025-04-04 11:17:08','15/01/2025','NULL','Française','250g','pommes de terre','200g','jambon','200g','1','raclette fromage',NULL,'2025-04-04',NULL),('10',NULL,'Menu Poulet à la sauce aigre-douce','23,50 €',1,'halal',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2025-04-04',NULL,'2025-04-04 15:11:39'),('2',1,'salade de fruit','5.000',6,'dessert','2025-04-03 22:21:31','15/01/2025','végétarien','Indifférent','100g','kiwi','100g','sucre','10g','1','fraise',NULL,'2025-04-03',NULL),('3',NULL,NULL,NULL,NULL,NULL,'2025-04-04 11:55:22',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2025-04-03',NULL,'2025-04-03 00:00:00'),('4',NULL,'Menu Smoky Bacon Burger','23.500',1,'','2025-04-03 23:34:47',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2025-04-03',NULL,'2025-04-03 00:00:00'),('5',NULL,'Menu Paneer Butter Masala','47.000',2,'végétarien',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2025-04-03',NULL,NULL),('6',NULL,'Pates','17.000',1,'végétarien',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2025-04-03',NULL,NULL),('7',NULL,'salade','5.000',2,'','2025-04-04 16:22:50',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2025-04-04','2025-04-04',NULL),('8',NULL,'oasis','2.500',1,'',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2025-04-04',NULL,NULL),('9',NULL,'taboulet','3,00 €',1,'',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'2025-04-04',NULL,'2025-04-04 13:26:46');
/*!40000 ALTER TABLE `commande` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contient`
--

DROP TABLE IF EXISTS `contient`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contient` (
  `id_menu` varchar(50) NOT NULL,
  `id_ingredient` varchar(10) NOT NULL,
  PRIMARY KEY (`id_menu`,`id_ingredient`),
  KEY `contient_ibfk_2` (`id_ingredient`),
  CONSTRAINT `contient_ibfk_1` FOREIGN KEY (`id_menu`) REFERENCES `menu` (`id_menu`),
  CONSTRAINT `contient_ibfk_2` FOREIGN KEY (`id_ingredient`) REFERENCES `ingredient` (`id_ingredient`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contient`
--

LOCK TABLES `contient` WRITE;
/*!40000 ALTER TABLE `contient` DISABLE KEYS */;
/*!40000 ALTER TABLE `contient` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cuisinier`
--

DROP TABLE IF EXISTS `cuisinier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cuisinier` (
  `id_cuisinier` varchar(10) NOT NULL,
  `nom_cuisinier` varchar(50) DEFAULT NULL,
  `prénom_cuisinier` varchar(50) DEFAULT NULL,
  `rue_cuisinier` varchar(50) DEFAULT NULL,
  `code_postal_cuisinier` varchar(50) DEFAULT NULL,
  `ville_cuisinier` varchar(50) DEFAULT NULL,
  `tel_cuisinier` varchar(50) DEFAULT NULL,
  `email_cuisinier` varchar(50) DEFAULT NULL,
  `metro_le_plus_proche_cuisinier` varchar(50) DEFAULT NULL,
  `mot_de_passe` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_cuisinier`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cuisinier`
--

LOCK TABLES `cuisinier` WRITE;
/*!40000 ALTER TABLE `cuisinier` DISABLE KEYS */;
INSERT INTO `cuisinier` VALUES ('','','','','','','','','',''),('C000','Dupond','Marie','Rue de la Republique','75011','Paris','1234567890','Mdupond@gmail.com','République','Marie'),('C001','Valois','Athanase','Rue des Charmes','75012','Paris','0698234671','athanase.valois@email.com','Pelleport','Athanase'),('C002','Lombardi','Alessandra','Passage du Marais','75003','Paris','0679845330','alessandra.lombardi@email.com','Filles du Calvaire','Alessandra'),('C003','Devereux','Isolde','Avenue des Églantines','75019','Paris','0645123876','isolde.devereux@email.com','Riquet','Isolde'),('C004','Kovač','Luka','Allée des Mimosas','75014','Paris','0789987653','luka.kovac@email.com','Alésia','kovač2023'),('C005','Hernandez','Paloma','Boulevard des Alouettes','75010','Paris','0611223355','paloma.hernandez@email.com','Gare de l’Est','paloma@hermes'),('C006','Santos','Ricardo','Rue des Oliviers','75013','Paris','0698712453','ricardo.santos@email.com','Place d’Italie','Ricardo'),('C007','Khan','Amira','Sentier des Pervenches','75002','Paris','0654678198','amira.khan@email.com','Grands Boulevards','Amira'),('C008','Moreau','Théodore','Impasse des Cèdres','75015','Paris','0787543120','theodore.moreau@email.com','Javel','moreau2023'),('C009','Marchand','Léa','14 Quai de la Tournelle','75005','Paris','0689012345','léa.marchand@email.com','Maubert-Mutualité','Léa'),('C010','Nguyen','Khoa','8 Rue de Belleville','75020','Paris','0178965412','khoa.nguyen@gmail.com','Jourdain','Khoa');
/*!40000 ALTER TABLE `cuisinier` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `emploie`
--

DROP TABLE IF EXISTS `emploie`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `emploie` (
  `ID_resto` varchar(50) NOT NULL,
  `id_cuisinier` varchar(10) NOT NULL,
  `ID_livreur` varchar(50) NOT NULL,
  PRIMARY KEY (`ID_resto`,`id_cuisinier`,`ID_livreur`),
  KEY `ID_Livreur` (`ID_livreur`),
  KEY `transmettre_ibfk_2` (`id_cuisinier`),
  CONSTRAINT `emploie_ibfk_1` FOREIGN KEY (`ID_resto`) REFERENCES `restaurant` (`ID_resto`),
  CONSTRAINT `emploie_ibfk_2` FOREIGN KEY (`id_cuisinier`) REFERENCES `cuisinier` (`id_cuisinier`),
  CONSTRAINT `emploie_ibfk_3` FOREIGN KEY (`ID_livreur`) REFERENCES `livreur` (`ID_Livreur`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `emploie`
--

LOCK TABLES `emploie` WRITE;
/*!40000 ALTER TABLE `emploie` DISABLE KEYS */;
INSERT INTO `emploie` VALUES ('R001','C003','L004');
/*!40000 ALTER TABLE `emploie` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ingredient`
--

DROP TABLE IF EXISTS `ingredient`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ingredient` (
  `id_ingredient` varchar(10) NOT NULL,
  `nom_ingredient` varchar(50) DEFAULT NULL,
  `régime_ingredient` varchar(100) DEFAULT NULL,
  `origine_ingredient` varchar(50) DEFAULT NULL,
  `type_ingredient` varchar(50) DEFAULT NULL,
  `allergene_ingredient` tinyint(1) DEFAULT NULL,
  `quantité_ingredient` tinyint DEFAULT NULL,
  `stock_ingredient` varchar(50) DEFAULT NULL,
  `date_peremption` date DEFAULT NULL,
  PRIMARY KEY (`id_ingredient`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ingredient`
--

LOCK TABLES `ingredient` WRITE;
/*!40000 ALTER TABLE `ingredient` DISABLE KEYS */;
INSERT INTO `ingredient` VALUES ('I001','Poivre','','espagne','épice',0,10,'8kg','2028-08-21'),('I002','Tomate','vegetarien','France','légume',0,50,'disponible frais','2025-04-08'),('I003','Farine','','France','Céréale',1,0,'disponible sec','2028-09-27');
/*!40000 ALTER TABLE `ingredient` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `lit`
--

DROP TABLE IF EXISTS `lit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `lit` (
  `id_client` smallint NOT NULL,
  `id_menu` varchar(50) NOT NULL,
  PRIMARY KEY (`id_client`,`id_menu`),
  KEY `ID_MENU` (`id_menu`),
  CONSTRAINT `lit_ibfk_1` FOREIGN KEY (`id_client`) REFERENCES `client` (`id_client`),
  CONSTRAINT `lit_ibfk_2` FOREIGN KEY (`id_menu`) REFERENCES `menu` (`id_menu`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `lit`
--

LOCK TABLES `lit` WRITE;
/*!40000 ALTER TABLE `lit` DISABLE KEYS */;
/*!40000 ALTER TABLE `lit` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `livre`
--

DROP TABLE IF EXISTS `livre`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `livre` (
  `id_client` smallint NOT NULL,
  `ID_livreur` varchar(50) NOT NULL,
  PRIMARY KEY (`id_client`,`ID_livreur`),
  KEY `ID_Livreur` (`ID_livreur`),
  CONSTRAINT `livre_ibfk_1` FOREIGN KEY (`id_client`) REFERENCES `client` (`id_client`),
  CONSTRAINT `livre_ibfk_2` FOREIGN KEY (`ID_livreur`) REFERENCES `livreur` (`ID_Livreur`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `livre`
--

LOCK TABLES `livre` WRITE;
/*!40000 ALTER TABLE `livre` DISABLE KEYS */;
/*!40000 ALTER TABLE `livre` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `livreur`
--

DROP TABLE IF EXISTS `livreur`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `livreur` (
  `ID_Livreur` varchar(50) NOT NULL,
  `nom_livreur` varchar(50) DEFAULT NULL,
  `prénom_livreur` varchar(50) DEFAULT NULL,
  `date_inscription` date DEFAULT NULL,
  `note_livreur` decimal(1,0) DEFAULT NULL,
  `email_livreur` varchar(50) DEFAULT NULL,
  `tel_livreur` varchar(50) DEFAULT NULL,
  `disponibilité` tinyint(1) DEFAULT NULL,
  `mot_de_passe_livreur` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`ID_Livreur`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `livreur`
--

LOCK TABLES `livreur` WRITE;
/*!40000 ALTER TABLE `livreur` DISABLE KEYS */;
INSERT INTO `livreur` VALUES ('L001','Rousseau','Nicolas','2025-04-03',NULL,'nicolas.rousseau@email.com','0777678765',NULL,'Nicolas'),('L002','Vermot','Eliott','2025-03-20',4,'eliott.vermot@email.com','0612345678',1,'Eliott'),('L003','Tardieu','Mélina','2025-03-18',5,'melina.tardieu@email.com','0623456789',1,'Mélina'),('L004','Brasseur','Cédric','2025-03-25',3,'cedric.brasseur@email.com','0634567890',0,'Cédric'),('L005','Chauvin','Sofia','2025-02-14',4,'sofia.chauvin@email.com','0645678901',1,'Sofia'),('L006','Dufresne','Bastien','2025-03-01',5,'bastien.dufresne@email.com','0656789012',1,'Bastien'),('L007','Delmas','Nora','2025-01-10',4,'nora.delmas@email.com','0667890123',0,'Nora'),('L008','Lemoine','Maxence','2025-02-28',3,'maxence.lemoine@email.com','0678901234',1,'Maxence'),('L009','Desrosiers','Camille','2025-03-15',4,'camille.desrosiers@email.com','0689012345',1,NULL),('L010','Moussard','Lucien','2025-02-19',NULL,'lucien.moussard@email.com','0765432336',NULL,'Lucien');
/*!40000 ALTER TABLE `livreur` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `menu`
--

DROP TABLE IF EXISTS `menu`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `menu` (
  `id_menu` varchar(50) NOT NULL,
  `entrée` varchar(50) DEFAULT NULL,
  `plat` varchar(50) DEFAULT NULL,
  `dessert` varchar(50) DEFAULT NULL,
  `boisson` varchar(50) DEFAULT NULL,
  `régime_correspondant` varchar(50) DEFAULT NULL,
  `ID_resto` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_menu`),
  KEY `menu_ibfk_2` (`ID_resto`),
  CONSTRAINT `menu_ibfk_2` FOREIGN KEY (`ID_resto`) REFERENCES `restaurant` (`ID_resto`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menu`
--

LOCK TABLES `menu` WRITE;
/*!40000 ALTER TABLE `menu` DISABLE KEYS */;
INSERT INTO `menu` VALUES ('','','','','','',''),('M001','Salade','Suchi','Mochi','Saké','standard','R002'),('M101','Batônnets mozarella','Smoky Bacon Burger','Brownie chocolat','Milkshake fraise','','R008'),('M102','Ouf Cocotte aux Champignons','Magret de Canard Sauce Miel et Thym','Tarte Tatin Caramélisée','Verre de Chardonnay','','R001'),('M103','Samossas aux Légumes','Paneer Butter Masala','Gulab Jamun','Lassi Mangue','végétarien','R006'),('M104','Bruschetta al Pomodoro','Risotto aux Champignons et Truffe','Glace vanille amande','Limoncello Spritz','','R003'),('M105','Raviolis vapeur aux légumes','Poulet à la sauce aigre-douce','Perles de coco','Thé Oolong aux perles de tapioca','halal','R009');
/*!40000 ALTER TABLE `menu` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `parcourt`
--

DROP TABLE IF EXISTS `parcourt`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `parcourt` (
  `id_trajet` varchar(10) NOT NULL,
  `ID_livreur` varchar(50) NOT NULL,
  PRIMARY KEY (`id_trajet`,`ID_livreur`),
  KEY `ID_Livreur` (`ID_livreur`),
  CONSTRAINT `parcourt_ibfk_1` FOREIGN KEY (`id_trajet`) REFERENCES `trajet` (`id_trajet`),
  CONSTRAINT `parcourt_ibfk_2` FOREIGN KEY (`ID_livreur`) REFERENCES `livreur` (`ID_Livreur`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `parcourt`
--

LOCK TABLES `parcourt` WRITE;
/*!40000 ALTER TABLE `parcourt` DISABLE KEYS */;
/*!40000 ALTER TABLE `parcourt` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `passe`
--

DROP TABLE IF EXISTS `passe`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `passe` (
  `id_client` smallint NOT NULL,
  `ID_commande` varchar(10) NOT NULL,
  PRIMARY KEY (`id_client`,`ID_commande`),
  KEY `passe_ibfk_2` (`ID_commande`),
  CONSTRAINT `passe_ibfk_1` FOREIGN KEY (`id_client`) REFERENCES `client` (`id_client`),
  CONSTRAINT `passe_ibfk_2` FOREIGN KEY (`ID_commande`) REFERENCES `commande` (`ID_commande`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `passe`
--

LOCK TABLES `passe` WRITE;
/*!40000 ALTER TABLE `passe` DISABLE KEYS */;
INSERT INTO `passe` VALUES (2,'10'),(1,'3'),(1,'4'),(7,'5'),(11,'6'),(11,'7'),(11,'8'),(2,'9');
/*!40000 ALTER TABLE `passe` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `prépare`
--

DROP TABLE IF EXISTS `prépare`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `prépare` (
  `id_cuisinier` varchar(10) NOT NULL,
  `ID_commande` varchar(10) NOT NULL,
  PRIMARY KEY (`id_cuisinier`,`ID_commande`),
  KEY `ID_commande` (`ID_commande`),
  CONSTRAINT `prépare_ibfk_1` FOREIGN KEY (`id_cuisinier`) REFERENCES `cuisinier` (`id_cuisinier`),
  CONSTRAINT `prépare_ibfk_2` FOREIGN KEY (`ID_commande`) REFERENCES `commande` (`ID_commande`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `prépare`
--

LOCK TABLES `prépare` WRITE;
/*!40000 ALTER TABLE `prépare` DISABLE KEYS */;
INSERT INTO `prépare` VALUES ('','10'),('C007','5'),('C003','7'),('C008','9');
/*!40000 ALTER TABLE `prépare` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `restaurant`
--

DROP TABLE IF EXISTS `restaurant`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `restaurant` (
  `ID_resto` varchar(50) NOT NULL,
  `nom_resto` varchar(50) DEFAULT NULL,
  `type_resto` varchar(50) DEFAULT NULL,
  `capacité_resto` decimal(3,0) DEFAULT NULL,
  PRIMARY KEY (`ID_resto`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `restaurant`
--

LOCK TABLES `restaurant` WRITE;
/*!40000 ALTER TABLE `restaurant` DISABLE KEYS */;
INSERT INTO `restaurant` VALUES ('','','',0),('R001','Le Gourmet Parisien','Français',80),('R002','Sushi Zen','Japonais',50),('R003','La Trattoria','Italien',60),('R004','BBQ King','Américain',100),('R005','El Mariachi','Mexicain',45),('R006','Tandoori Palace','Indien',70),('R007','Chez Mamie','Traditionnel',55),('R008','Burger Express','Fast Food',90),('R009','Dragon Doré','Chinois',65),('R010','Vegan Delight','Végétarien',40),('R011','Bistro Belle Vue','Français',75),('R012','Chez Marcel','Bistrot',40);
/*!40000 ALTER TABLE `restaurant` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `réceptionne`
--

DROP TABLE IF EXISTS `réceptionne`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `réceptionne` (
  `ID_resto` varchar(50) NOT NULL,
  `ID_commande` varchar(10) NOT NULL,
  PRIMARY KEY (`ID_resto`,`ID_commande`),
  KEY `réceptionne_ibfk_2` (`ID_commande`),
  CONSTRAINT `réceptionne_ibfk_1` FOREIGN KEY (`ID_resto`) REFERENCES `restaurant` (`ID_resto`),
  CONSTRAINT `réceptionne_ibfk_2` FOREIGN KEY (`ID_commande`) REFERENCES `commande` (`ID_commande`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `réceptionne`
--

LOCK TABLES `réceptionne` WRITE;
/*!40000 ALTER TABLE `réceptionne` DISABLE KEYS */;
INSERT INTO `réceptionne` VALUES ('R009','10'),('R002','3'),('R008','4'),('R006','5'),('R003','6'),('R001','7'),('R001','8'),('R011','9');
/*!40000 ALTER TABLE `réceptionne` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `trajet`
--

DROP TABLE IF EXISTS `trajet`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `trajet` (
  `id_trajet` varchar(10) NOT NULL,
  `duree_trajet` time DEFAULT NULL,
  `date_trajet` date DEFAULT NULL,
  `point_départ_trajet` varchar(50) DEFAULT NULL,
  `point_arrivee_trajet` varchar(50) DEFAULT NULL,
  `distance_trajet` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`id_trajet`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `trajet`
--

LOCK TABLES `trajet` WRITE;
/*!40000 ALTER TABLE `trajet` DISABLE KEYS */;
/*!40000 ALTER TABLE `trajet` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-04-04 18:04:55
