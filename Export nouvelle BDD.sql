-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: localhost    Database: livinparis2
-- ------------------------------------------------------
-- Server version	8.0.40

CREATE database IF NOT EXISTS livinparis2;
USE livinparis2;

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
-- Table structure for table `administrateur`
--

DROP TABLE IF EXISTS `administrateur`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `administrateur` (
  `id_admin` varchar(10) NOT NULL,
  `nom_admin` varchar(50) DEFAULT NULL,
  `prenom_admin` varchar(50) DEFAULT NULL,
  `email_admin` varchar(50) DEFAULT NULL,
  `mot_de_passe_admin` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_admin`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `administrateur`
--

LOCK TABLES `administrateur` WRITE;
/*!40000 ALTER TABLE `administrateur` DISABLE KEYS */;
INSERT INTO `administrateur` VALUES ('1','','','',''),('2','admin','Admin','admin.Admin@livinparis.com','Admin'),('A001','faizan','Admin','faizan.Admin@livinparis.com','Admin1'),('A002','thomas','Admin','thomas.Admin@livinparis.com','Admin2'),('A003','Martin','Admin','martin.Admin@livinparis.com','Admin3');
/*!40000 ALTER TABLE `administrateur` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `adresse`
--

DROP TABLE IF EXISTS `adresse`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `adresse` (
  `id_adresse` varchar(50) NOT NULL,
  `code_porte` varchar(50) DEFAULT NULL,
  `complement_adresse` varchar(20) DEFAULT NULL,
  `étage` varchar(15) DEFAULT NULL,
  `numero_appartement` varchar(50) DEFAULT NULL,
  `id_client` varchar(10) NOT NULL,
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
INSERT INTO `adresse` VALUES ('1','53','TER','0','53','CL002'),('2','22','BIS','2','22','CL004');
/*!40000 ALTER TABLE `adresse` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `appartient`
--

DROP TABLE IF EXISTS `appartient`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `appartient` (
  `id_plat` varchar(10) NOT NULL,
  `id_commande` varchar(10) NOT NULL,
  PRIMARY KEY (`id_plat`,`id_commande`),
  KEY `id_commande` (`id_commande`),
  CONSTRAINT `appartient_ibfk_1` FOREIGN KEY (`id_plat`) REFERENCES `plat` (`id_plat`),
  CONSTRAINT `appartient_ibfk_2` FOREIGN KEY (`id_commande`) REFERENCES `commande` (`id_commande`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `appartient`
--

LOCK TABLES `appartient` WRITE;
/*!40000 ALTER TABLE `appartient` DISABLE KEYS */;
INSERT INTO `appartient` VALUES ('P002','10');
/*!40000 ALTER TABLE `appartient` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `client`
--

DROP TABLE IF EXISTS `client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `client` (
  `id_client` varchar(10) NOT NULL,
  `nom_client` varchar(50) DEFAULT NULL,
  `prenom_client` varchar(50) DEFAULT NULL,
  `type_client` varchar(50) DEFAULT NULL,
  `nom_entreprise` varchar(50) DEFAULT NULL,
  `rue_client` varchar(50) NOT NULL,
  `code_postal_client` varchar(10) DEFAULT NULL,
  `tel_client` varchar(50) DEFAULT NULL,
  `nom_referent` varchar(50) DEFAULT NULL,
  `date_inscription_client` date DEFAULT NULL,
  `email_client` varchar(50) DEFAULT NULL,
  `metro_le_plus_proche_client` varchar(50) DEFAULT NULL,
  `mot_de_passe_client` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_client`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `client`
--

LOCK TABLES `client` WRITE;
/*!40000 ALTER TABLE `client` DISABLE KEYS */;
INSERT INTO `client` VALUES ('1','Andries','Alexis','particulier',NULL,'12 rue des tuileries','75001','07.85.34.56.76',NULL,'2025-04-24','alexis.andries@gmail.com','Tuileries','Alexis'),('CL001','Durand','Alice','particulier',NULL,'12 rue des Lilas','75020','06.10.02.18.35',NULL,'2024-10-01','alice.durand@gmail.com','Télégraphe','Alice'),('CL002','Martin','Lucas','entreprise','TechNova','7 avenue Victor Hugo','75016','06.05.76.19.37','Paul Dubois','2024-10-02','lucas.martin@gmail.com','Victor Hugo','Lucas'),('CL003','Bernard','Emma','particulier',NULL,'5 rue de la République','75011','0608091011',NULL,'2024-10-03','emma.bernard@gmail.com','Rue Saint-Maur','Emma'),('CL004','Petit','Nathan','entreprise','FoodLine','28 rue Saint-Denis','75002','07.62.16.28.02','Julie Moreau','2024-10-04','nathan.petit@gmail.com','Réaumur-Sébastopol','Nathan'),('CL005','Robert','Léa','particulier',NULL,'10 boulevard Voltaire','75011','06.52.19.62.41',NULL,'2024-10-05','lea.robert@gmail.com','Oberkampf','Léa'),('CL006','Richard','Thomas','entreprise','GreenIT','3 rue de la Convention','75015','0619202122','Claire Fabre','2024-10-06','thomas.richard@gmail.com','Convention','Thomas'),('CL007','Durand','Camille','particulier',NULL,'14 rue Jean Jaurès','75019','0623242526',NULL,'2024-10-07','camille.durand@gmail.com','Ourcq','Camille'),('CL008','Moreau','Antoine','entreprise','Batiprix','21 avenue Foch','75016','0627282930','Lucie Garnier','2024-10-08','antoine.moreau@gmail.com','Porte Dauphine','Antoine'),('CL009','Girard','Sophie','particulier',NULL,'9 rue Lafayette','75009','06.52.72.91.17',NULL,'2024-10-09','sophie.girard@gmail.com','Notre-Dame-de-Lorette','Sophie'),('CL010','Leclerc','Julien','entreprise','FlexCorp','11 place de la République','75010','0635363738','Marc Noël','2024-10-10','julien.leclerc@gmail.com','République','Julien');
/*!40000 ALTER TABLE `client` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `commande`
--

DROP TABLE IF EXISTS `commande`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `commande` (
  `id_commande` varchar(10) NOT NULL,
  `prix_total_commande` varchar(50) DEFAULT NULL,
  `date_commande` date DEFAULT NULL,
  `commentaire_commande` varchar(50) DEFAULT NULL,
  `date_livraison` date DEFAULT NULL,
  `statut_commande` varchar(50) DEFAULT NULL,
  `id_client` varchar(10) NOT NULL,
  PRIMARY KEY (`id_commande`),
  KEY `id_client` (`id_client`),
  CONSTRAINT `commande_ibfk_1` FOREIGN KEY (`id_client`) REFERENCES `client` (`id_client`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `commande`
--

LOCK TABLES `commande` WRITE;
/*!40000 ALTER TABLE `commande` DISABLE KEYS */;
INSERT INTO `commande` VALUES ('10','3 euros','2025-04-28','',NULL,'Prêt pour livraison','CL004'),('2',NULL,'2025-04-25','',NULL,'Annulée','CL001'),('3',NULL,'2025-04-25','',NULL,'En attente','CL001'),('5',NULL,'2025-04-26','',NULL,'En attente','CL002'),('6',NULL,'2025-04-27','','2025-04-28','Livrée','CL005'),('7',NULL,'2025-04-27','',NULL,'Annulée','CL004'),('8',NULL,'2025-04-27','',NULL,'Annulée','CL004'),('9','6 euros','2025-04-28','',NULL,'En préparation','CL004');
/*!40000 ALTER TABLE `commande` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `communique`
--

DROP TABLE IF EXISTS `communique`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `communique` (
  `id_client` varchar(10) NOT NULL,
  `id_cuisinier` varchar(10) NOT NULL,
  PRIMARY KEY (`id_client`,`id_cuisinier`),
  KEY `id_cuisinier` (`id_cuisinier`),
  CONSTRAINT `communique_ibfk_1` FOREIGN KEY (`id_client`) REFERENCES `client` (`id_client`),
  CONSTRAINT `communique_ibfk_2` FOREIGN KEY (`id_cuisinier`) REFERENCES `cuisinier` (`id_cuisinier`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `communique`
--

LOCK TABLES `communique` WRITE;
/*!40000 ALTER TABLE `communique` DISABLE KEYS */;
/*!40000 ALTER TABLE `communique` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `concerne`
--

DROP TABLE IF EXISTS `concerne`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `concerne` (
  `id_plat` varchar(10) NOT NULL,
  `id_evaluation` varchar(10) NOT NULL,
  PRIMARY KEY (`id_plat`,`id_evaluation`),
  KEY `id_evaluation` (`id_evaluation`),
  CONSTRAINT `concerne_ibfk_1` FOREIGN KEY (`id_plat`) REFERENCES `plat` (`id_plat`),
  CONSTRAINT `concerne_ibfk_2` FOREIGN KEY (`id_evaluation`) REFERENCES `evaluation` (`id_evaluation`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `concerne`
--

LOCK TABLES `concerne` WRITE;
/*!40000 ALTER TABLE `concerne` DISABLE KEYS */;
/*!40000 ALTER TABLE `concerne` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contient`
--

DROP TABLE IF EXISTS `contient`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contient` (
  `id_plat` varchar(10) NOT NULL,
  `id_ingredient` varchar(10) NOT NULL,
  PRIMARY KEY (`id_plat`,`id_ingredient`),
  KEY `id_ingredient` (`id_ingredient`),
  CONSTRAINT `contient_ibfk_1` FOREIGN KEY (`id_plat`) REFERENCES `plat` (`id_plat`),
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
  `note_moyenne_cuisinier` tinyint DEFAULT NULL,
  `date_inscription_cuisinier` date DEFAULT NULL,
  `prenom_cuisinier` varchar(50) DEFAULT NULL,
  `rue_cuisinier` varchar(50) DEFAULT NULL,
  `code_postal_cuisinier` varchar(10) DEFAULT NULL,
  `dispo_cuisinier` varchar(50) DEFAULT NULL,
  `tel_cuisinier` varchar(30) DEFAULT NULL,
  `email_cuisinier` varchar(50) DEFAULT NULL,
  `metro_le_plus_proche_cuisinier` varchar(50) DEFAULT NULL,
  `mot_de_passe_cuisinier` varchar(50) DEFAULT NULL,
  `specialite_cuisinier` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_cuisinier`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cuisinier`
--

LOCK TABLES `cuisinier` WRITE;
/*!40000 ALTER TABLE `cuisinier` DISABLE KEYS */;
INSERT INTO `cuisinier` VALUES ('C001','Lemoine',8,'2024-09-10','Mathieu','14 rue Oberkampf','75011','Disponible','07.52.74.18.53','mathieu.lemoine@gmail.com','Parmentier','Mathieu','France'),('C002','Garcia',7,'2024-09-12','Sonia','22 avenue Gambetta','75020','Disponible','06.82.51.72.16','sonia.garcia@gmail.com','Gambetta','Sonia','Italie'),('C003','Dupuis',9,'2024-09-14','Nicolas','9 rue Lecourbe','75015','En pause','06.82.10.28.73','nicolas.dupuis@gmail.com','Sèvres-Lecourbe','Nicolas','Bretonne'),('C004','Fabre',6,'2024-09-16','Élodie','18 rue de Rennes','6','Disponible','603344556','elodie.fabre@gmail.com','Rennes','Élodie',NULL),('C005','Benoît',10,'2024-09-18','Olivier','3 avenue de Clichy','75017','Indisponible','06.62.93.73.17','olivier.benoit@gmail.com','Place de Clichy','Olivier','France'),('C006','Collet',5,'2024-09-20','Camille','12 rue de Tolbiac','13','Disponible','605566778','camille.collet@gmail.com','Tolbiac','Camille',NULL),('C007','Roux',9,'2024-09-22','Théo','7 rue du Faubourg Saint-Antoine','12','Disponible','606677889','theo.roux@gmail.com','Reuilly-Diderot','Théo',NULL),('C008','Lemoine',6,'2024-09-24','Isabelle','11 boulevard de la Villette','10','Disponible','607788990','isabelle.lemoine@gmail.com','Colonel Fabien','Isabelle',NULL),('C009','Morel',8,'2024-09-26','Julien','25 rue de Belleville','20','Indisponible','608899001','julien.morel@gmail.com','Belleville','Julien',NULL),('C010','Fernandez',7,'2024-09-28','Anna','6 rue du Louvre','75001','Disponible','06.18.72.17.03','anna.fernandez@gmail.com','Louvre-Rivoli','Anna','Espagne');
/*!40000 ALTER TABLE `cuisinier` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `effectue`
--

DROP TABLE IF EXISTS `effectue`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `effectue` (
  `id_client` varchar(10) NOT NULL,
  `id_evaluation` varchar(10) NOT NULL,
  PRIMARY KEY (`id_client`,`id_evaluation`),
  KEY `id_evaluation` (`id_evaluation`),
  CONSTRAINT `effectue_ibfk_1` FOREIGN KEY (`id_client`) REFERENCES `client` (`id_client`),
  CONSTRAINT `effectue_ibfk_2` FOREIGN KEY (`id_evaluation`) REFERENCES `evaluation` (`id_evaluation`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `effectue`
--

LOCK TABLES `effectue` WRITE;
/*!40000 ALTER TABLE `effectue` DISABLE KEYS */;
/*!40000 ALTER TABLE `effectue` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `evaluation`
--

DROP TABLE IF EXISTS `evaluation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `evaluation` (
  `id_evaluation` varchar(10) NOT NULL,
  `note_evaluation` tinyint DEFAULT NULL,
  `date_evaluation` date DEFAULT NULL,
  `commentaire_evaluation` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_evaluation`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `evaluation`
--

LOCK TABLES `evaluation` WRITE;
/*!40000 ALTER TABLE `evaluation` DISABLE KEYS */;
/*!40000 ALTER TABLE `evaluation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `gère`
--

DROP TABLE IF EXISTS `gère`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `gère` (
  `id_client` varchar(10) NOT NULL,
  `id_admin` varchar(10) NOT NULL,
  PRIMARY KEY (`id_client`,`id_admin`),
  KEY `id_admin` (`id_admin`),
  CONSTRAINT `gère_ibfk_1` FOREIGN KEY (`id_client`) REFERENCES `client` (`id_client`),
  CONSTRAINT `gère_ibfk_2` FOREIGN KEY (`id_admin`) REFERENCES `administrateur` (`id_admin`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `gère`
--

LOCK TABLES `gère` WRITE;
/*!40000 ALTER TABLE `gère` DISABLE KEYS */;
/*!40000 ALTER TABLE `gère` ENABLE KEYS */;
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
  `origine_ingredient` varchar(50) DEFAULT NULL,
  `categorie_ingredient` varchar(50) DEFAULT NULL,
  `allergene_ingredient` tinyint(1) DEFAULT NULL,
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
INSERT INTO `ingredient` VALUES ('I001','Oeuf','France','',1,'30 boîtes','2025-05-29'),('I002','Tomates fraîches','Espagne','Bio',0,'15 cagettes','2025-05-05');
/*!40000 ALTER TABLE `ingredient` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ligne_commande`
--

DROP TABLE IF EXISTS `ligne_commande`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ligne_commande` (
  `id_ligne` varchar(10) NOT NULL,
  `prix_unitaire` varchar(50) DEFAULT NULL,
  `date_livraison_voulue` date DEFAULT NULL,
  `metro_le_plus_proche_livraison` varchar(50) DEFAULT NULL,
  `statut_ligne` varchar(50) DEFAULT NULL,
  `id_plat` varchar(10) NOT NULL,
  `id_commande` varchar(10) NOT NULL,
  `id_cuisinier` varchar(10) DEFAULT NULL,
  `quantite` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_ligne`),
  KEY `id_plat` (`id_plat`),
  KEY `id_commande` (`id_commande`),
  KEY `fk_ligne_cuisinier` (`id_cuisinier`),
  CONSTRAINT `fk_ligne_cuisinier` FOREIGN KEY (`id_cuisinier`) REFERENCES `cuisinier` (`id_cuisinier`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `ligne_commande_ibfk_1` FOREIGN KEY (`id_plat`) REFERENCES `plat` (`id_plat`),
  CONSTRAINT `ligne_commande_ibfk_2` FOREIGN KEY (`id_commande`) REFERENCES `commande` (`id_commande`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ligne_commande`
--

LOCK TABLES `ligne_commande` WRITE;
/*!40000 ALTER TABLE `ligne_commande` DISABLE KEYS */;
INSERT INTO `ligne_commande` VALUES ('1','6 euros','2025-04-28','Reaumur-Sebastopol','En attente','P001','9',NULL,NULL),('2','3,4 euros','2025-04-29','Reaumur-Sebastopol','Prêt pour livraison','P002','10',NULL,NULL);
/*!40000 ALTER TABLE `ligne_commande` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `plat`
--

DROP TABLE IF EXISTS `plat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `plat` (
  `id_plat` varchar(10) NOT NULL,
  `nom_plat` varchar(50) DEFAULT NULL,
  `description_plat` varchar(50) DEFAULT NULL,
  `type_plat` varchar(50) DEFAULT NULL,
  `prix_unitaire_plat` varchar(50) DEFAULT NULL,
  `regime_alimentaire_plat` varchar(50) DEFAULT NULL,
  `portion_dispo_plat` varchar(50) DEFAULT NULL,
  `date_fabrication_plat` date DEFAULT NULL,
  `date_peremption_plat` date DEFAULT NULL,
  `nationalite_plat` varchar(50) DEFAULT NULL,
  `id_cuisinier` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`id_plat`),
  KEY `fk_plat_cuisinier` (`id_cuisinier`),
  CONSTRAINT `fk_plat_cuisinier` FOREIGN KEY (`id_cuisinier`) REFERENCES `cuisinier` (`id_cuisinier`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `plat`
--

LOCK TABLES `plat` WRITE;
/*!40000 ALTER TABLE `plat` DISABLE KEYS */;
INSERT INTO `plat` VALUES ('P001','Pâtes bolognaises','pâtes frâiches halal','plat principal','6 euros','végétarien','20 assiettes','2025-04-26','2025-05-01','italienne','C001'),('P002','Paris-Brest','fait maison','dessert','3,4 euros','standard','5 ','2025-04-26','2025-04-28','France','C002'),('P003','Quiche lorraine','lardons, crème fraîche, pâte brisée','plat principal','5,2 euros','standard','12 parts','2025-04-28','2025-05-03','France','C003'),('P004','Brownie choco noir','fondant maison','dessert','2,8 euros','végétarien','9 parts','2025-04-28','2025-05-02','USA','C005');
/*!40000 ALTER TABLE `plat` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reçoit`
--

DROP TABLE IF EXISTS `reçoit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reçoit` (
  `id_cuisinier` varchar(10) NOT NULL,
  `id_evaluation` varchar(10) NOT NULL,
  PRIMARY KEY (`id_cuisinier`,`id_evaluation`),
  KEY `id_evaluation` (`id_evaluation`),
  CONSTRAINT `reçoit_ibfk_1` FOREIGN KEY (`id_cuisinier`) REFERENCES `cuisinier` (`id_cuisinier`),
  CONSTRAINT `reçoit_ibfk_2` FOREIGN KEY (`id_evaluation`) REFERENCES `evaluation` (`id_evaluation`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reçoit`
--

LOCK TABLES `reçoit` WRITE;
/*!40000 ALTER TABLE `reçoit` DISABLE KEYS */;
/*!40000 ALTER TABLE `reçoit` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `suit`
--

DROP TABLE IF EXISTS `suit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `suit` (
  `id_evaluation` varchar(10) NOT NULL,
  `id_admin` varchar(10) NOT NULL,
  PRIMARY KEY (`id_evaluation`,`id_admin`),
  KEY `id_admin` (`id_admin`),
  CONSTRAINT `suit_ibfk_1` FOREIGN KEY (`id_evaluation`) REFERENCES `evaluation` (`id_evaluation`),
  CONSTRAINT `suit_ibfk_2` FOREIGN KEY (`id_admin`) REFERENCES `administrateur` (`id_admin`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `suit`
--

LOCK TABLES `suit` WRITE;
/*!40000 ALTER TABLE `suit` DISABLE KEYS */;
/*!40000 ALTER TABLE `suit` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `supervise`
--

DROP TABLE IF EXISTS `supervise`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `supervise` (
  `id_commande` varchar(10) NOT NULL,
  `id_admin` varchar(10) NOT NULL,
  PRIMARY KEY (`id_commande`,`id_admin`),
  KEY `id_admin` (`id_admin`),
  CONSTRAINT `supervise_ibfk_1` FOREIGN KEY (`id_commande`) REFERENCES `commande` (`id_commande`),
  CONSTRAINT `supervise_ibfk_2` FOREIGN KEY (`id_admin`) REFERENCES `administrateur` (`id_admin`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `supervise`
--

LOCK TABLES `supervise` WRITE;
/*!40000 ALTER TABLE `supervise` DISABLE KEYS */;
/*!40000 ALTER TABLE `supervise` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `trajet`
--

DROP TABLE IF EXISTS `trajet`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `trajet` (
  `id_trajet` varchar(10) NOT NULL,
  `heure_depart_trajet_` time DEFAULT NULL,
  `date_trajet` date DEFAULT NULL,
  `metro_depart_trajet` varchar(50) DEFAULT NULL,
  `metro_arrivee_trajet` varchar(50) DEFAULT NULL,
  `distance_trajet` varchar(10) DEFAULT NULL,
  `statut_trajet` varchar(50) DEFAULT NULL,
  `commentaire_trajet` varchar(50) DEFAULT NULL,
  `id_cuisinier` varchar(10) NOT NULL,
  PRIMARY KEY (`id_trajet`),
  UNIQUE KEY `id_cuisinier` (`id_cuisinier`),
  CONSTRAINT `trajet_ibfk_1` FOREIGN KEY (`id_cuisinier`) REFERENCES `cuisinier` (`id_cuisinier`)
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

-- Dump completed on 2025-05-01 21:31:58
