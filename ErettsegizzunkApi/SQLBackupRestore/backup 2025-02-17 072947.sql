-- MySqlBackup.NET 2.3.8.0
-- Dump Time: 2025-02-17 07:29:47
-- --------------------------------------
-- Server version 10.4.32-MariaDB mariadb.org binary distribution


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 
-- Definition of level
-- 

DROP TABLE IF EXISTS `level`;
CREATE TABLE IF NOT EXISTS `level` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table level
-- 

/*!40000 ALTER TABLE `level` DISABLE KEYS */;
INSERT INTO `level`(`id`,`name`) VALUES(1,'közép'),(2,'emelt');
/*!40000 ALTER TABLE `level` ENABLE KEYS */;

-- 
-- Definition of permission
-- 

DROP TABLE IF EXISTS `permission`;
CREATE TABLE IF NOT EXISTS `permission` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `level` int(1) DEFAULT NULL,
  `name` varchar(32) DEFAULT NULL,
  `description` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `level` (`level`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table permission
-- 

/*!40000 ALTER TABLE `permission` DISABLE KEYS */;
INSERT INTO `permission`(`id`,`level`,`name`,`description`) VALUES(1,0,'Luzer','Webes regisztráció felhasználó'),(2,9,'Administrator','Rendszergazda');
/*!40000 ALTER TABLE `permission` ENABLE KEYS */;

-- 
-- Definition of subject
-- 

DROP TABLE IF EXISTS `subject`;
CREATE TABLE IF NOT EXISTS `subject` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table subject
-- 

/*!40000 ALTER TABLE `subject` DISABLE KEYS */;
INSERT INTO `subject`(`id`,`name`) VALUES(1,'matematika'),(2,'történelem'),(3,'magyar');
/*!40000 ALTER TABLE `subject` ENABLE KEYS */;

-- 
-- Definition of task
-- 

DROP TABLE IF EXISTS `task`;
CREATE TABLE IF NOT EXISTS `task` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `description` varchar(1024) DEFAULT NULL,
  `text` varchar(1024) DEFAULT NULL,
  `answers` varchar(1024) DEFAULT NULL,
  `isCorrect` varchar(20) DEFAULT NULL,
  `subjectId` int(11) DEFAULT NULL,
  `typeId` int(11) DEFAULT NULL,
  `levelId` int(11) DEFAULT NULL,
  `picName` varchar(18) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `subjectId` (`subjectId`),
  KEY `typeId` (`typeId`),
  KEY `levelId` (`levelId`),
  CONSTRAINT `task_ibfk_1` FOREIGN KEY (`subjectId`) REFERENCES `subject` (`id`),
  CONSTRAINT `task_ibfk_2` FOREIGN KEY (`typeId`) REFERENCES `type` (`id`),
  CONSTRAINT `task_ibfk_3` FOREIGN KEY (`levelId`) REFERENCES `level` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=184 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table task
-- 

/*!40000 ALTER TABLE `task` DISABLE KEYS */;
INSERT INTO `task`(`id`,`description`,`text`,`answers`,`isCorrect`,`subjectId`,`typeId`,`levelId`,`picName`) VALUES(66,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(67,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(68,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(69,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(70,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(71,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(72,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(73,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(74,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(75,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(76,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(77,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(78,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(79,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(80,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(81,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(82,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(83,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(84,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(85,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(86,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(87,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(88,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(89,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(90,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(91,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(92,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(93,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(94,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(95,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(96,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(97,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(98,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(99,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(100,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(101,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(102,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(103,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(104,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(105,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(106,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(107,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(108,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(109,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(110,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(111,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(112,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(113,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(114,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(115,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(116,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(117,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(118,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(119,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(120,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(121,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(122,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(123,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(124,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(125,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(126,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(127,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(128,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(129,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(130,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(131,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(132,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(133,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(134,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(135,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(136,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(137,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(138,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(139,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(140,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(141,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(142,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(143,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(144,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(145,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(146,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(147,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(148,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(149,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(150,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(151,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(152,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(153,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(154,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(155,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(156,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(157,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(158,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(159,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(160,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(161,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(162,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(163,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(164,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(165,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(166,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(167,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(168,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(169,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(170,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(171,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(172,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(173,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(174,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(175,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(176,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(177,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(178,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(179,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(180,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png'),(181,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,'asd.png'),(182,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(183,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!','A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,'asd.png');
/*!40000 ALTER TABLE `task` ENABLE KEYS */;

-- 
-- Definition of spaced_repetition
-- 

DROP TABLE IF EXISTS `spaced_repetition`;
CREATE TABLE IF NOT EXISTS `spaced_repetition` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userId` int(11) NOT NULL,
  `taskId` int(11) NOT NULL,
  `lastCorrectTime` timestamp NOT NULL DEFAULT current_timestamp(),
  `intervalDays` int(11) NOT NULL DEFAULT 1,
  `nextDueTime` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `userId` (`userId`),
  KEY `taskId` (`taskId`),
  CONSTRAINT `spaced_repetition_ibfk_1` FOREIGN KEY (`userId`) REFERENCES `user` (`id`),
  CONSTRAINT `spaced_repetition_ibfk_2` FOREIGN KEY (`taskId`) REFERENCES `task` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table spaced_repetition
-- 

/*!40000 ALTER TABLE `spaced_repetition` DISABLE KEYS */;

/*!40000 ALTER TABLE `spaced_repetition` ENABLE KEYS */;

-- 
-- Definition of theme
-- 

DROP TABLE IF EXISTS `theme`;
CREATE TABLE IF NOT EXISTS `theme` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table theme
-- 

/*!40000 ALTER TABLE `theme` DISABLE KEYS */;

/*!40000 ALTER TABLE `theme` ENABLE KEYS */;

-- 
-- Definition of task_theme
-- 

DROP TABLE IF EXISTS `task_theme`;
CREATE TABLE IF NOT EXISTS `task_theme` (
  `taskId` int(11) NOT NULL,
  `themeId` int(11) NOT NULL,
  PRIMARY KEY (`taskId`,`themeId`),
  KEY `themeId` (`themeId`),
  CONSTRAINT `task_theme_ibfk_1` FOREIGN KEY (`taskId`) REFERENCES `task` (`id`),
  CONSTRAINT `task_theme_ibfk_2` FOREIGN KEY (`themeId`) REFERENCES `theme` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table task_theme
-- 

/*!40000 ALTER TABLE `task_theme` DISABLE KEYS */;

/*!40000 ALTER TABLE `task_theme` ENABLE KEYS */;

-- 
-- Definition of type
-- 

DROP TABLE IF EXISTS `type`;
CREATE TABLE IF NOT EXISTS `type` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table type
-- 

/*!40000 ALTER TABLE `type` DISABLE KEYS */;
INSERT INTO `type`(`id`,`name`) VALUES(1,'radio'),(2,'checkbox'),(3,'textbox');
/*!40000 ALTER TABLE `type` ENABLE KEYS */;

-- 
-- Definition of user
-- 

DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `loginName` varchar(16) NOT NULL,
  `HASH` varchar(64) NOT NULL,
  `SALT` varchar(64) NOT NULL,
  `email` varchar(64) NOT NULL,
  `permissionId` int(11) NOT NULL,
  `active` tinyint(1) NOT NULL DEFAULT 0,
  `newsletter` tinyint(1) NOT NULL DEFAULT 0,
  `profilePicturePath` varchar(64) DEFAULT NULL,
  `signupDate` timestamp NOT NULL DEFAULT current_timestamp(),
  `googleUser` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  UNIQUE KEY `loginName` (`loginName`),
  UNIQUE KEY `email` (`email`),
  KEY `permission` (`permissionId`),
  CONSTRAINT `user_ibfk_1` FOREIGN KEY (`permissionId`) REFERENCES `permission` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table user
-- 

/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user`(`id`,`loginName`,`HASH`,`SALT`,`email`,`permissionId`,`active`,`newsletter`,`profilePicturePath`,`signupDate`,`googleUser`) VALUES(1,'kerenyir','12c2d33dd731e56e72092034c052c5f3da97f78781746546c672a59f1400b781','Gm63C4jiWnYvfZfiKUu2cu8AHPNDj8NoHhtQn88yiJhyOunBNSd7tRoWo5wwqg9X','kerenyir@kkszki.hu',2,1,0,'igen.jpg','2025-01-26 14:50:51',0),(9,'a','fac680db4e70db69fa8752a598c8804967c173f35aaf3b349add87468f04c117','qrFsrweUIYT6zdkX4P25t1AnPgpN2dbYTaNpw87ATjoFIrFrLoHp05CUuR0hp8Pl','ezazemailem',2,1,0,'default.jpg','2025-01-27 11:11:23',0),(13,'zsóti','869a6e791b254d25dd4aa21c3b7f798f53c1c2b07d035af886016edc339f848a','nEJ3RLHYKtSWtiLkdQOLAPKu5LKsxuJejWs0gQClkRMCIpcHi1rArpuBz3RDgg8W','gasparzs@kkszki.hu',2,1,0,NULL,'2025-01-28 08:40:25',0);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;

-- 
-- Definition of user_statistics
-- 

DROP TABLE IF EXISTS `user_statistics`;
CREATE TABLE IF NOT EXISTS `user_statistics` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userId` int(11) NOT NULL,
  `statisticsDate` date DEFAULT NULL,
  `mathSuccessfulTasks` text DEFAULT NULL,
  `mathUnsuccessfulTasks` text DEFAULT NULL,
  `historySuccessfulTasks` text DEFAULT NULL,
  `historyUnsuccessfulTasks` text DEFAULT NULL,
  `hungarianSuccessfulTasks` text DEFAULT NULL,
  `hungarianUnsuccessfulTasks` text DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `userId` (`userId`),
  CONSTRAINT `user_statistics_ibfk_1` FOREIGN KEY (`userId`) REFERENCES `user` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table user_statistics
-- 

/*!40000 ALTER TABLE `user_statistics` DISABLE KEYS */;

/*!40000 ALTER TABLE `user_statistics` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2025-02-17 07:29:48
-- Total time: 0:0:0:0:287 (d:h:m:s:ms)
