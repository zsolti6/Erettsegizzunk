-- MySqlBackup.NET 2.3.8.0
-- Dump Time: 2025-01-17 08:43:00
-- --------------------------------------
-- Server version 10.4.28-MariaDB mariadb.org binary distribution


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 
-- Definition of szint
-- 

DROP TABLE IF EXISTS `szint`;
CREATE TABLE IF NOT EXISTS `szint` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nev` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table szint
-- 

/*!40000 ALTER TABLE `szint` DISABLE KEYS */;
INSERT INTO `szint`(`id`,`nev`) VALUES(1,'közép'),(2,'emelt');
/*!40000 ALTER TABLE `szint` ENABLE KEYS */;

-- 
-- Definition of feladatok
-- 

DROP TABLE IF EXISTS `feladatok`;
CREATE TABLE IF NOT EXISTS `feladatok` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `leiras` varchar(1024) DEFAULT NULL,
  `megoldasok` varchar(1024) DEFAULT NULL,
  `helyese` varchar(20) DEFAULT NULL,
  `tantargyId` int(11) DEFAULT NULL,
  `tipusId` int(11) DEFAULT NULL,
  `szintId` int(11) DEFAULT NULL,
  `kepNev` varchar(18) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `tantargyId` (`tantargyId`),
  KEY `tipusId` (`tipusId`),
  KEY `szintId` (`szintId`),
  CONSTRAINT `feladatok_ibfk_1` FOREIGN KEY (`tantargyId`) REFERENCES `tantargyak` (`id`),
  CONSTRAINT `feladatok_ibfk_2` FOREIGN KEY (`tipusId`) REFERENCES `tipus` (`id`),
  CONSTRAINT `feladatok_ibfk_3` FOREIGN KEY (`szintId`) REFERENCES `szint` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=162 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table feladatok
-- 

/*!40000 ALTER TABLE `feladatok` DISABLE KEYS */;
INSERT INTO `feladatok`(`id`,`leiras`,`megoldasok`,`helyese`,`tantargyId`,`tipusId`,`szintId`,`kepNev`) VALUES(13,'Adott az A = {1; 2; 3; 4} és a B = {1; 2; 4; 8} halmaz. Elemei felsorolásával adja meg az A ∩ B halmazt vesszővel elválasztva!','1,2,4;','1;',1,3,1,NULL),(14,'Adott az A = {1; 2; 3; 4} és a B = {1; 2; 4; 8} halmaz. Elemei felsorolásával adja meg az A ∪ B halmazt vesszővel elválasztva!','1,2,3,4,8;','1;',1,3,1,NULL),(15,'Adott az A = {1; 2; 3; 4} és a B = {1; 2; 4; 8} halmaz. Elemei felsorolásával adja meg az A \\ B halmazt vesszővel elválasztva!','3;','1;',1,3,1,NULL),(16,'Egy pékségben fehér kenyeret és rozskenyeret is árusítanak. Egyik reggel az első 30 vevő közül 22-en fehér kenyeret, 17-en pedig rozskenyeret vásároltak. Hányan vásároltak mindkét fajta kenyérből, ha mind a 30 vevő vett a két fajta kenyér valamelyikéből?','9;','1;',1,3,1,NULL),(17,'Egy számtani sorozat első tagja 6, hetedik tagja pedig 36. Adja meg a sorozat negyedik tagját!','21;','1;',1,3,1,NULL),(18,'Adott az A = {1; 2; 3; 4} és a B = {1; 2; 4; 8} halmaz. Elemei felsorolásával adja meg az A ∩ B halmazt vesszővel elválasztva!','1,2,4;','1;',1,3,1,NULL),(19,'Adott az A = {1; 2; 3; 4} és a B = {1; 2; 4; 8} halmaz. Elemei felsorolásával adja meg az A ∪ B halmazt vesszővel elválasztva!','1,2,3,4,8;','1;',1,3,1,NULL),(20,'Adott az A = {1; 2; 3; 4} és a B = {1; 2; 4; 8} halmaz. Elemei felsorolásával adja meg az A \\ B halmazt vesszővel elválasztva!','3;','1;',1,3,1,NULL),(21,'Egy pékségben fehér kenyeret és rozskenyeret is árusítanak. Egyik reggel az első 30 vevő közül 22-en fehér kenyeret, 17-en pedig rozskenyeret vásároltak. Hányan vásároltak mindkét fajta kenyérből, ha mind a 30 vevő vett a két fajta kenyér valamelyikéből?','9;','1;',1,3,1,NULL),(22,'Egy számtani sorozat első tagja 6, hetedik tagja pedig 36. Adja meg a sorozat negyedik tagját!','21;','1;',1,3,1,NULL),(23,'Hány átlója van egy konvex nyolcszögnek?','20;','1;',1,3,1,NULL),(24,'Hány különböző 4-gyel osztható négyjegyű szám készíthető a 2, 3, 4, 5 számjegyekből, ha egy-egy számhoz mindegyik számjegyet egyszer használjuk fel?','6;','1;',1,3,1,NULL),(25,'Egy számítógépes játékban Bélának 4-szer annyi pontja van, mint Andrásnak. Hány pontja van Bélának, ha kettejüknek együtt 6500 pontjuk van?','5200;','1;',1,3,1,NULL),(26,'Annának két 5-öse, négy 4-ese és két 3-asa van biológiából. Adja meg Anna biológiajegyeinek szórását!','0.707;','1;',1,3,1,NULL),(27,'Egy mértani sorozat nyolcadik tagja 1020, kilencedik tagja 1023. Adja meg a sorozat hányadosát!','1000;','1;',1,3,1,NULL),(28,'Egy mértani sorozat nyolcadik tagja 1020, kilencedik tagja 1023. Adja meg a sorozat első tagját!','0.1;','1;',1,3,1,NULL),(29,'Két szabályos dobókockával egyszer dobunk. Mennyi annak a valószínűsége, hogy a dobott számok összege négyzetszám lesz? ','0.194;','1;',1,3,1,NULL),(30,'Adott az f: R → R, f(x) = (x3)2 - 4 függvény. Melyik számot rendeli az f függvény az x = 2,5-hez?','3.75;','1;',1,3,1,NULL),(31,'Adott az f: R → R, f(x) = (x3)2 - 4 függvény. Határozza meg az f függvény zérushelyeit vesszővel elválasztva!','1,5;','1;',1,3,1,NULL),(32,'Az A és B halmazokról tudjuk, hogy A ∪ B = {1; 2; 3; 4; 5; 6}, A ∩ B = {1; 2} és A \\ B = {3; 4}. Adja meg az A halmazt elemei felsorolásával, vesszővel elválasztva!','1,2,3,4;','1;',1,3,1,NULL),(33,'Az A és B halmazokról tudjuk, hogy A ∪ B = {1; 2; 3; 4; 5; 6}, A ∩ B = {1; 2} és A \\ B = {3; 4}. Adja meg a B halmazt elemei felsorolásával, vesszővel elválasztva!','1,2,5,6;','1;',1,3,1,NULL),(34,'Egy derékszögű háromszög egyik befogója 24 cm, átfogója 25 cm hosszú. Hány cm hosszú a másik befogó?','7;','1;',1,3,1,NULL),(35,'Hány darab négyjegyű, különböző számjegyekből álló (pozitív) páratlan szám alkotható az 1, 2, 3, 4 számjegyekből?','12;','1;',1,3,1,NULL),(36,'Adja meg a értékét, ha tudjuk, hogy a1/2 = 4.','16;','1;',1,3,1,NULL),(37,'Egy számtani sorozat nyolcadik tagja 6-tal nagyobb, mint a negyedik tagja. A sorozat hatodik tagja 6. Számítsa ki a sorozat első 6 tagjának az összegét!','13.5;','1;',1,3,1,NULL),(38,'Hány csúcsa van egy hatszög alapú gúlának?','7;','1;',1,3,1,NULL),(39,'Hány lapja van egy hatszög alapú gúlának?','7;','1;',1,3,1,NULL),(40,'Hány éle van egy hatszög alapú gúlának?','12;','1;',1,3,1,NULL),(41,'Egy szám 2-es alapú logaritmusa 6. Mennyi a szám kétszeresének a 2-es alapú logaritmusa?','7;','1;',1,3,1,NULL),(42,'Egy városban a polgármester-választáson a győztes jelöltre a szavazáson résztvevők 55%-a szavazott, így 10 593 szavazatot kapott. Hányan vettek részt ebben a városban a szavazáson?','19260;','1;',1,3,1,NULL),(43,'Balázs magyar irodalomból a következő jegyeket szerezte az első félévben: 1, 5, 5, 5. Számítsa ki Balázs jegyeinek átlagát!','4;','1;',1,3,1,NULL),(44,'Balázs magyar irodalomból a következő jegyeket szerezte az első félévben: 1, 5, 5, 5. Számítsa ki Balázs jegyeinek szórását!','1.73;','1;',1,3,1,NULL),(45,'Egy piros, egy fekete és egy fehér szabályos dobókockával egyszerre dobunk. Határozza meg annak a valószínűségét, hogy a dobás eredménye három különböző szám lesz!','0.556;','1;',1,3,1,NULL),(46,'Oldja meg a valós számok halmazán az alábbi egyenletet! 18 ∙ (7x + 96) + 19 ∙ (5x – 56) = 1990','6;','1;',1,3,1,NULL),(47,'Írja fel az 1896 és az 1956 prímtényezős felbontását, és adja meg az 1896 és az 1956 összes közös (pozitív) osztóját vesszővel elválasztva!','1,2,3,4,6,12;','1;',1,3,1,NULL),(48,'Egy szabályos tízszög egy oldalának hossza 10 cm. Számítsa ki a tízszög területét!','770;','1;',1,3,1,NULL),(49,'Egy szabályos sokszög átlóinak a száma 2015. Hány oldalú a sokszög?','65;','1;',1,3,1,NULL),(50,'Egy építkezésre teherautókkal szállítják a homokot. Öt egyforma teherautó mindegyikének nyolcszor kellene fordulnia, hogy az összes homokot odaszállítsák. Hány fordulóval tudná odaszállítani ugyanezt a mennyiségű homokot négy ugyanekkora teherautó?','10;','1;',1,3,1,NULL),(51,'Egy derékszögű háromszög két befogója 10 és 24 cm hosszú. Számítsa ki az átfogó hosszát!','26;','1;',1,3,1,NULL),(52,'Egy meteorológiai állomáson november első hetében az alábbi napi hőmérsékleti maximumokat mérték (°C-ban): 9, 5, 6, 9, 6, 6, 8. Adja meg az adatok átlagát!','7;','1;',1,3,1,NULL),(53,'Egy meteorológiai állomáson november első hetében az alábbi napi hőmérsékleti maximumokat mérték (°C-ban): 9, 5, 6, 9, 6, 6, 8. Adja meg az adatok terjedelmét!','4;','1;',1,3,1,NULL),(54,'Egy meteorológiai állomáson november első hetében az alábbi napi hőmérsékleti maximumokat mérték (°C-ban): 9, 5, 6, 9, 6, 6, 8. Adja meg az adatok mediánját!','6;','1;',1,3,1,NULL),(55,'Egy dobozban 10 piros és néhány zöld golyó van. Tudjuk, hogy ha egy golyót kihúzunk véletlenszerűen a dobozból, akkor 2/3 annak a valószínűsége, hogy a golyó piros. Hány zöld golyó van a dobozban?','5;','1;',1,3,1,NULL),(56,'Egy vasúti tartálykocsi tömege üres tartállyal 23,8 tonna. Ebben a tartálykocsiban maximum 60 000 liter üzemanyagot szállíthatnak. Egy liter üzemanyag tömege 0,85 kg. Hány tonna a tartálykocsi tömege tele tartállyal?','74.8;','1;',1,3,1,NULL),(57,'Egy kör egyenlete: (x - 2)2 + (y - 4)2 = 25. Adja meg a kör 1pontjának koordinátáit!','2,4;','1;',1,3,1,NULL),(58,'Egy kör egyenlete: (x - 2)2 + (y - 4)2 = 25. Adja meg a kör sugarát!','5;','1;',1,3,1,NULL),(59,'Egy szabályos pénzérmét háromszor feldobunk. Határozza meg annak a valószínűségét, hogy a három dobás közül pontosan egy lesz fej!','0.375;','1;',1,3,1,NULL),(60,'Adott a valós számok halmazán értelmezett f függvény: f(x) = (x - 3)2 + 2. Mit rendel az f függvény az x = 3,5-hez?','2.25;','1;',1,3,1,NULL),(61,'Adott a valós számok halmazán értelmezett f függvény: f(x) = (x - 3)2 + 2. Mely számokhoz rendeli az f függvény a 6-ot?','1.5;','1;',1,3,1,NULL),(62,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző! Pillanatnyi benyomások rögzítése, nominális stílus.','impresszionizmus;klasszicizmus;naturalizmus;romantika','1;0;0;0',3,1,1,NULL),(63,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző! Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,NULL),(64,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző! A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(65,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző! A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,NULL),(66,'Adott az A = {1; 2; 3; 4} és a B = {1; 2; 4; 8} halmaz. Elemei felsorolásával adja meg az A ∩ B halmazt vesszővel elválasztva!','1,2,4;','1;',1,3,1,NULL),(67,'Adott az A = {1; 2; 3; 4} és a B = {1; 2; 4; 8} halmaz. Elemei felsorolásával adja meg az A ∪ B halmazt vesszővel elválasztva!','1,2,3,4,8;','1;',1,3,1,NULL),(68,'Adott az A = {1; 2; 3; 4} és a B = {1; 2; 4; 8} halmaz. Elemei felsorolásával adja meg az A \\ B halmazt vesszővel elválasztva!','3;','1;',1,3,1,NULL),(69,'Egy pékségben fehér kenyeret és rozskenyeret is árusítanak. Egyik reggel az első 30 vevő közül 22-en fehér kenyeret, 17-en pedig rozskenyeret vásároltak. Hányan vásároltak mindkét fajta kenyérből, ha mind a 30 vevő vett a két fajta kenyér valamelyikéből?','9;','1;',1,3,1,NULL),(70,'Egy számtani sorozat első tagja 6, hetedik tagja pedig 36. Adja meg a sorozat negyedik tagját!','21;','1;',1,3,1,NULL),(71,'Hány átlója van egy konvex nyolcszögnek?','20;','1;',1,3,1,NULL),(72,'Hány különböző 4-gyel osztható négyjegyű szám készíthető a 2, 3, 4, 5 számjegyekből, ha egy-egy számhoz mindegyik számjegyet egyszer használjuk fel?','6;','1;',1,3,1,NULL),(73,'Egy számítógépes játékban Bélának 4-szer annyi pontja van, mint Andrásnak. Hány pontja van Bélának, ha kettejüknek együtt 6500 pontjuk van?','5200;','1;',1,3,1,NULL),(74,'Annának két 5-öse, négy 4-ese és két 3-asa van biológiából. Adja meg Anna biológiajegyeinek szórását!','0.707;','1;',1,3,1,NULL),(75,'Egy mértani sorozat nyolcadik tagja 1020, kilencedik tagja 1023. Adja meg a sorozat hányadosát!','1000;','1;',1,3,1,NULL),(76,'Egy mértani sorozat nyolcadik tagja 1020, kilencedik tagja 1023. Adja meg a sorozat első tagját!','0.1;','1;',1,3,1,NULL),(77,'Két szabályos dobókockával egyszer dobunk. Mennyi annak a valószínűsége, hogy a dobott számok összege négyzetszám lesz? ','0.194;','1;',1,3,1,NULL),(78,'Adott az f: R → R, f(x) = (x3)2 - 4 függvény. Melyik számot rendeli az f függvény az x = 2,5-hez?','3.75;','1;',1,3,1,NULL),(79,'Adott az f: R → R, f(x) = (x3)2 - 4 függvény. Határozza meg az f függvény zérushelyeit vesszővel elválasztva!','1,5;','1;',1,3,1,NULL),(80,'Az A és B halmazokról tudjuk, hogy A ∪ B = {1; 2; 3; 4; 5; 6}, A ∩ B = {1; 2} és A \\ B = {3; 4}. Adja meg az A halmazt elemei felsorolásával, vesszővel elválasztva!','1,2,3,4;','1;',1,3,1,NULL),(81,'Az A és B halmazokról tudjuk, hogy A ∪ B = {1; 2; 3; 4; 5; 6}, A ∩ B = {1; 2} és A \\ B = {3; 4}. Adja meg a B halmazt elemei felsorolásával, vesszővel elválasztva!','1,2,5,6;','1;',1,3,1,NULL),(82,'Egy derékszögű háromszög egyik befogója 24 cm, átfogója 25 cm hosszú. Hány cm hosszú a másik befogó?','7;','1;',1,3,1,NULL),(83,'Hány darab négyjegyű, különböző számjegyekből álló (pozitív) páratlan szám alkotható az 1, 2, 3, 4 számjegyekből?','12;','1;',1,3,1,NULL),(84,'Adja meg a értékét, ha tudjuk, hogy a1/2 = 4.','16;','1;',1,3,1,NULL),(85,'Egy számtani sorozat nyolcadik tagja 6-tal nagyobb, mint a negyedik tagja. A sorozat hatodik tagja 6. Számítsa ki a sorozat első 6 tagjának az összegét!','13.5;','1;',1,3,1,NULL),(86,'Hány csúcsa van egy hatszög alapú gúlának?','7;','1;',1,3,1,NULL),(87,'Hány lapja van egy hatszög alapú gúlának?','7;','1;',1,3,1,NULL),(88,'Hány éle van egy hatszög alapú gúlának?','12;','1;',1,3,1,NULL),(89,'Egy szám 2-es alapú logaritmusa 6. Mennyi a szám kétszeresének a 2-es alapú logaritmusa?','7;','1;',1,3,1,NULL),(90,'Egy városban a polgármester-választáson a győztes jelöltre a szavazáson résztvevők 55%-a szavazott, így 10 593 szavazatot kapott. Hányan vettek részt ebben a városban a szavazáson?','19260;','1;',1,3,1,NULL),(91,'Balázs magyar irodalomból a következő jegyeket szerezte az első félévben: 1, 5, 5, 5. Számítsa ki Balázs jegyeinek átlagát!','4;','1;',1,3,1,NULL),(92,'Balázs magyar irodalomból a következő jegyeket szerezte az első félévben: 1, 5, 5, 5. Számítsa ki Balázs jegyeinek szórását!','1.73;','1;',1,3,1,NULL),(93,'Egy piros, egy fekete és egy fehér szabályos dobókockával egyszerre dobunk. Határozza meg annak a valószínűségét, hogy a dobás eredménye három különböző szám lesz!','0.556;','1;',1,3,1,NULL),(94,'Oldja meg a valós számok halmazán az alábbi egyenletet! 18 ∙ (7x + 96) + 19 ∙ (5x – 56) = 1990','6;','1;',1,3,1,NULL),(95,'Írja fel az 1896 és az 1956 prímtényezős felbontását, és adja meg az 1896 és az 1956 összes közös (pozitív) osztóját vesszővel elválasztva!','1,2,3,4,6,12;','1;',1,3,1,NULL),(96,'Egy szabályos tízszög egy oldalának hossza 10 cm. Számítsa ki a tízszög területét!','770;','1;',1,3,1,NULL),(97,'Egy szabályos sokszög átlóinak a száma 2015. Hány oldalú a sokszög?','65;','1;',1,3,1,NULL),(98,'Egy építkezésre teherautókkal szállítják a homokot. Öt egyforma teherautó mindegyikének nyolcszor kellene fordulnia, hogy az összes homokot odaszállítsák. Hány fordulóval tudná odaszállítani ugyanezt a mennyiségű homokot négy ugyanekkora teherautó?','10;','1;',1,3,1,NULL),(99,'Egy derékszögű háromszög két befogója 10 és 24 cm hosszú. Számítsa ki az átfogó hosszát!','26;','1;',1,3,1,NULL),(100,'Egy meteorológiai állomáson november első hetében az alábbi napi hőmérsékleti maximumokat mérték (°C-ban): 9, 5, 6, 9, 6, 6, 8. Adja meg az adatok átlagát!','7;','1;',1,3,1,NULL),(101,'Egy meteorológiai állomáson november első hetében az alábbi napi hőmérsékleti maximumokat mérték (°C-ban): 9, 5, 6, 9, 6, 6, 8. Adja meg az adatok terjedelmét!','4;','1;',1,3,1,NULL),(102,'Egy meteorológiai állomáson november első hetében az alábbi napi hőmérsékleti maximumokat mérték (°C-ban): 9, 5, 6, 9, 6, 6, 8. Adja meg az adatok mediánját!','6;','1;',1,3,1,NULL),(103,'Egy dobozban 10 piros és néhány zöld golyó van. Tudjuk, hogy ha egy golyót kihúzunk véletlenszerűen a dobozból, akkor 2/3 annak a valószínűsége, hogy a golyó piros. Hány zöld golyó van a dobozban?','5;','1;',1,3,1,NULL),(104,'Egy vasúti tartálykocsi tömege üres tartállyal 23,8 tonna. Ebben a tartálykocsiban maximum 60 000 liter üzemanyagot szállíthatnak. Egy liter üzemanyag tömege 0,85 kg. Hány tonna a tartálykocsi tömege tele tartállyal?','74.8;','1;',1,3,1,NULL),(105,'Egy kör egyenlete: (x - 2)2 + (y - 4)2 = 25. Adja meg a kör 1pontjának koordinátáit!','2,4;','1;',1,3,1,NULL),(106,'Egy kör egyenlete: (x - 2)2 + (y - 4)2 = 25. Adja meg a kör sugarát!','5;','1;',1,3,1,NULL),(107,'Egy szabályos pénzérmét háromszor feldobunk. Határozza meg annak a valószínűségét, hogy a három dobás közül pontosan egy lesz fej!','0.375;','1;',1,3,1,NULL),(108,'Adott a valós számok halmazán értelmezett f függvény: f(x) = (x - 3)2 + 2. Mit rendel az f függvény az x = 3,5-hez?','2.25;','1;',1,3,1,NULL),(109,'Adott a valós számok halmazán értelmezett f függvény: f(x) = (x - 3)2 + 2. Mely számokhoz rendeli az f függvény a 6-ot?','1.5;','1;',1,3,1,NULL),(110,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző! Pillanatnyi benyomások rögzítése, nominális stílus.','impresszionizmus;klasszicizmus;naturalizmus;romantika','1;0;0;0',3,1,1,NULL),(111,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző! Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,NULL),(112,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző! A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(113,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző! A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,NULL),(114,'Adott az A = {1; 2; 3; 4} és a B = {1; 2; 4; 8} halmaz. Elemei felsorolásával adja meg az A ∩ B halmazt vesszővel elválasztva!','1,2,4;','1;',1,3,1,NULL),(115,'Adott az A = {1; 2; 3; 4} és a B = {1; 2; 4; 8} halmaz. Elemei felsorolásával adja meg az A ∪ B halmazt vesszővel elválasztva!','1,2,3,4,8;','1;',1,3,1,NULL),(116,'Adott az A = {1; 2; 3; 4} és a B = {1; 2; 4; 8} halmaz. Elemei felsorolásával adja meg az A \\ B halmazt vesszővel elválasztva!','3;','1;',1,3,1,NULL),(117,'Egy pékségben fehér kenyeret és rozskenyeret is árusítanak. Egyik reggel az első 30 vevő közül 22-en fehér kenyeret, 17-en pedig rozskenyeret vásároltak. Hányan vásároltak mindkét fajta kenyérből, ha mind a 30 vevő vett a két fajta kenyér valamelyikéből?','9;','1;',1,3,1,NULL),(118,'Egy számtani sorozat első tagja 6, hetedik tagja pedig 36. Adja meg a sorozat negyedik tagját!','21;','1;',1,3,1,NULL),(119,'Hány átlója van egy konvex nyolcszögnek?','20;','1;',1,3,1,NULL),(120,'Hány különböző 4-gyel osztható négyjegyű szám készíthető a 2, 3, 4, 5 számjegyekből, ha egy-egy számhoz mindegyik számjegyet egyszer használjuk fel?','6;','1;',1,3,1,NULL),(121,'Egy számítógépes játékban Bélának 4-szer annyi pontja van, mint Andrásnak. Hány pontja van Bélának, ha kettejüknek együtt 6500 pontjuk van?','5200;','1;',1,3,1,NULL),(122,'Annának két 5-öse, négy 4-ese és két 3-asa van biológiából. Adja meg Anna biológiajegyeinek szórását!','0.707;','1;',1,3,1,NULL),(123,'Egy mértani sorozat nyolcadik tagja 1020, kilencedik tagja 1023. Adja meg a sorozat hányadosát!','1000;','1;',1,3,1,NULL),(124,'Egy mértani sorozat nyolcadik tagja 1020, kilencedik tagja 1023. Adja meg a sorozat első tagját!','0.1;','1;',1,3,1,NULL),(125,'Két szabályos dobókockával egyszer dobunk. Mennyi annak a valószínűsége, hogy a dobott számok összege négyzetszám lesz? ','0.194;','1;',1,3,1,NULL),(126,'Adott az f: R → R, f(x) = (x3)2 - 4 függvény. Melyik számot rendeli az f függvény az x = 2,5-hez?','3.75;','1;',1,3,1,NULL),(127,'Adott az f: R → R, f(x) = (x3)2 - 4 függvény. Határozza meg az f függvény zérushelyeit vesszővel elválasztva!','1,5;','1;',1,3,1,NULL),(128,'Az A és B halmazokról tudjuk, hogy A ∪ B = {1; 2; 3; 4; 5; 6}, A ∩ B = {1; 2} és A \\ B = {3; 4}. Adja meg az A halmazt elemei felsorolásával, vesszővel elválasztva!','1,2,3,4;','1;',1,3,1,NULL),(129,'Az A és B halmazokról tudjuk, hogy A ∪ B = {1; 2; 3; 4; 5; 6}, A ∩ B = {1; 2} és A \\ B = {3; 4}. Adja meg a B halmazt elemei felsorolásával, vesszővel elválasztva!','1,2,5,6;','1;',1,3,1,NULL),(130,'Egy derékszögű háromszög egyik befogója 24 cm, átfogója 25 cm hosszú. Hány cm hosszú a másik befogó?','7;','1;',1,3,1,NULL),(131,'Hány darab négyjegyű, különböző számjegyekből álló (pozitív) páratlan szám alkotható az 1, 2, 3, 4 számjegyekből?','12;','1;',1,3,1,NULL),(132,'Adja meg a értékét, ha tudjuk, hogy a1/2 = 4.','16;','1;',1,3,1,NULL),(133,'Egy számtani sorozat nyolcadik tagja 6-tal nagyobb, mint a negyedik tagja. A sorozat hatodik tagja 6. Számítsa ki a sorozat első 6 tagjának az összegét!','13.5;','1;',1,3,1,NULL),(134,'Hány csúcsa van egy hatszög alapú gúlának?','7;','1;',1,3,1,NULL),(135,'Hány lapja van egy hatszög alapú gúlának?','7;','1;',1,3,1,NULL),(136,'Hány éle van egy hatszög alapú gúlának?','12;','1;',1,3,1,NULL),(137,'Egy szám 2-es alapú logaritmusa 6. Mennyi a szám kétszeresének a 2-es alapú logaritmusa?','7;','1;',1,3,1,NULL),(138,'Egy városban a polgármester-választáson a győztes jelöltre a szavazáson résztvevők 55%-a szavazott, így 10 593 szavazatot kapott. Hányan vettek részt ebben a városban a szavazáson?','19260;','1;',1,3,1,NULL),(139,'Balázs magyar irodalomból a következő jegyeket szerezte az első félévben: 1, 5, 5, 5. Számítsa ki Balázs jegyeinek átlagát!','4;','1;',1,3,1,NULL),(140,'Balázs magyar irodalomból a következő jegyeket szerezte az első félévben: 1, 5, 5, 5. Számítsa ki Balázs jegyeinek szórását!','1.73;','1;',1,3,1,NULL),(141,'Egy piros, egy fekete és egy fehér szabályos dobókockával egyszerre dobunk. Határozza meg annak a valószínűségét, hogy a dobás eredménye három különböző szám lesz!','0.556;','1;',1,3,1,NULL),(142,'Oldja meg a valós számok halmazán az alábbi egyenletet! 18 ∙ (7x + 96) + 19 ∙ (5x – 56) = 1990','6;','1;',1,3,1,NULL),(143,'Írja fel az 1896 és az 1956 prímtényezős felbontását, és adja meg az 1896 és az 1956 összes közös (pozitív) osztóját vesszővel elválasztva!','1,2,3,4,6,12;','1;',1,3,1,NULL),(144,'Egy szabályos tízszög egy oldalának hossza 10 cm. Számítsa ki a tízszög területét!','770;','1;',1,3,1,NULL),(145,'Egy szabályos sokszög átlóinak a száma 2015. Hány oldalú a sokszög?','65;','1;',1,3,1,NULL),(146,'Egy építkezésre teherautókkal szállítják a homokot. Öt egyforma teherautó mindegyikének nyolcszor kellene fordulnia, hogy az összes homokot odaszállítsák. Hány fordulóval tudná odaszállítani ugyanezt a mennyiségű homokot négy ugyanekkora teherautó?','10;','1;',1,3,1,NULL),(147,'Egy derékszögű háromszög két befogója 10 és 24 cm hosszú. Számítsa ki az átfogó hosszát!','26;','1;',1,3,1,NULL),(148,'Egy meteorológiai állomáson november első hetében az alábbi napi hőmérsékleti maximumokat mérték (°C-ban): 9, 5, 6, 9, 6, 6, 8. Adja meg az adatok átlagát!','7;','1;',1,3,1,NULL),(149,'Egy meteorológiai állomáson november első hetében az alábbi napi hőmérsékleti maximumokat mérték (°C-ban): 9, 5, 6, 9, 6, 6, 8. Adja meg az adatok terjedelmét!','4;','1;',1,3,1,NULL),(150,'Egy meteorológiai állomáson november első hetében az alábbi napi hőmérsékleti maximumokat mérték (°C-ban): 9, 5, 6, 9, 6, 6, 8. Adja meg az adatok mediánját!','6;','1;',1,3,1,NULL),(151,'Egy dobozban 10 piros és néhány zöld golyó van. Tudjuk, hogy ha egy golyót kihúzunk véletlenszerűen a dobozból, akkor 2/3 annak a valószínűsége, hogy a golyó piros. Hány zöld golyó van a dobozban?','5;','1;',1,3,1,NULL),(152,'Egy vasúti tartálykocsi tömege üres tartállyal 23,8 tonna. Ebben a tartálykocsiban maximum 60 000 liter üzemanyagot szállíthatnak. Egy liter üzemanyag tömege 0,85 kg. Hány tonna a tartálykocsi tömege tele tartállyal?','74.8;','1;',1,3,1,NULL),(153,'Egy kör egyenlete: (x - 2)2 + (y - 4)2 = 25. Adja meg a kör 1pontjának koordinátáit!','2,4;','1;',1,3,1,NULL),(154,'Egy kör egyenlete: (x - 2)2 + (y - 4)2 = 25. Adja meg a kör sugarát!','5;','1;',1,3,1,NULL),(155,'Egy szabályos pénzérmét háromszor feldobunk. Határozza meg annak a valószínűségét, hogy a három dobás közül pontosan egy lesz fej!','0.375;','1;',1,3,1,NULL),(156,'Adott a valós számok halmazán értelmezett f függvény: f(x) = (x - 3)2 + 2. Mit rendel az f függvény az x = 3,5-hez?','2.25;','1;',1,3,1,NULL),(157,'Adott a valós számok halmazán értelmezett f függvény: f(x) = (x - 3)2 + 2. Mely számokhoz rendeli az f függvény a 6-ot?','1.5;','1;',1,3,1,NULL),(158,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző! Pillanatnyi benyomások rögzítése, nominális stílus.','impresszionizmus;klasszicizmus;naturalizmus;romantika','1;0;0;0',3,1,1,NULL),(159,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző! Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;1;0;0',3,1,1,NULL),(160,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző! A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;1;0',3,1,1,NULL),(161,'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző! A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.','impresszionizmus;klasszicizmus;naturalizmus;romantika','0;0;0;1',3,1,1,NULL);
/*!40000 ALTER TABLE `feladatok` ENABLE KEYS */;

-- 
-- Definition of permission
-- 

DROP TABLE IF EXISTS `permission`;
CREATE TABLE IF NOT EXISTS `permission` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Level` int(1) DEFAULT NULL,
  `Name` varchar(32) DEFAULT NULL,
  `Description` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Szint` (`Level`),
  UNIQUE KEY `Nev` (`Name`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table permission
-- 

/*!40000 ALTER TABLE `permission` DISABLE KEYS */;
INSERT INTO `permission`(`Id`,`Level`,`Name`,`Description`) VALUES(1,0,'Luzer','Webes regisztráció felhasználó'),(2,9,'Administrator','Rendszergazda');
/*!40000 ALTER TABLE `permission` ENABLE KEYS */;

-- 
-- Definition of tantargyak
-- 

DROP TABLE IF EXISTS `tantargyak`;
CREATE TABLE IF NOT EXISTS `tantargyak` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nev` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table tantargyak
-- 

/*!40000 ALTER TABLE `tantargyak` DISABLE KEYS */;
INSERT INTO `tantargyak`(`id`,`nev`) VALUES(1,'matematika'),(2,'történelem'),(3,'magyar');
/*!40000 ALTER TABLE `tantargyak` ENABLE KEYS */;

-- 
-- Definition of tema
-- 

DROP TABLE IF EXISTS `tema`;
CREATE TABLE IF NOT EXISTS `tema` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nev` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table tema
-- 

/*!40000 ALTER TABLE `tema` DISABLE KEYS */;

/*!40000 ALTER TABLE `tema` ENABLE KEYS */;

-- 
-- Definition of feladatok_tema
-- 

DROP TABLE IF EXISTS `feladatok_tema`;
CREATE TABLE IF NOT EXISTS `feladatok_tema` (
  `feladatokId` int(11) NOT NULL,
  `temaId` int(11) NOT NULL,
  PRIMARY KEY (`feladatokId`,`temaId`),
  KEY `temaId` (`temaId`),
  CONSTRAINT `feladatok_tema_ibfk_1` FOREIGN KEY (`feladatokId`) REFERENCES `feladatok` (`id`),
  CONSTRAINT `feladatok_tema_ibfk_2` FOREIGN KEY (`temaId`) REFERENCES `tema` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table feladatok_tema
-- 

/*!40000 ALTER TABLE `feladatok_tema` DISABLE KEYS */;

/*!40000 ALTER TABLE `feladatok_tema` ENABLE KEYS */;

-- 
-- Definition of tipus
-- 

DROP TABLE IF EXISTS `tipus`;
CREATE TABLE IF NOT EXISTS `tipus` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nev` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table tipus
-- 

/*!40000 ALTER TABLE `tipus` DISABLE KEYS */;
INSERT INTO `tipus`(`id`,`nev`) VALUES(1,'radio'),(2,'checkbox'),(3,'textbox');
/*!40000 ALTER TABLE `tipus` ENABLE KEYS */;

-- 
-- Definition of user
-- 

DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `LoginName` varchar(16) NOT NULL,
  `HASH` varchar(64) NOT NULL,
  `SALT` varchar(64) NOT NULL,
  `Name` varchar(64) DEFAULT NULL,
  `PermissionId` int(11) NOT NULL,
  `Active` tinyint(1) NOT NULL,
  `Email` varchar(64) NOT NULL,
  `ProfilePicturePath` varchar(64) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `LoginNev` (`LoginName`),
  UNIQUE KEY `Email` (`Email`),
  KEY `Jog` (`PermissionId`),
  CONSTRAINT `user_ibfk_1` FOREIGN KEY (`PermissionId`) REFERENCES `permission` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table user
-- 

/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user`(`Id`,`LoginName`,`HASH`,`SALT`,`Name`,`PermissionId`,`Active`,`Email`,`ProfilePicturePath`) VALUES(1,'kerenyir','d5fe0e517520122f1ab363b6b7ee9ae616e7ad393693ef00d81a7f287a79931a','Gm63C4jiWnYvfZfiKUu2cu8AHPNDj8NoHhtQn88yiJhyOunBNSd7tRoWo5wwqg9X','Kerényi Róbert',2,1,'kerenyir@kkszki.hu','img\\kerenyir.jpg');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;

-- 
-- Definition of token
-- 

DROP TABLE IF EXISTS `token`;
CREATE TABLE IF NOT EXISTS `token` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userId` int(11) DEFAULT NULL,
  `token` varchar(40) DEFAULT NULL,
  `aktiv` tinyint(1) DEFAULT NULL,
  `login` datetime DEFAULT NULL,
  `logout` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `userId` (`userId`),
  CONSTRAINT `token_ibfk_1` FOREIGN KEY (`userId`) REFERENCES `user` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- 
-- Dumping data for table token
-- 

/*!40000 ALTER TABLE `token` DISABLE KEYS */;
INSERT INTO `token`(`id`,`userId`,`token`,`aktiv`,`login`,`logout`) VALUES(-1,1,'8c0dace4-a220-4d47-806d-c0d52edcc562',1,'2025-01-14 09:28:03','0001-01-01 00:00:00'),(1,1,'562295eb-71af-42fa-b8ca-6e2608f70d1b',1,'2025-01-14 09:28:48','0001-01-01 00:00:00'),(2,1,'4cdb725e-9dd8-4c07-ac1c-daaced7439e3',1,'2025-01-14 09:31:05','0001-01-01 00:00:00'),(3,1,'24223031-c90a-444e-9461-9e4b249baa3d',1,'2025-01-14 09:31:39','0001-01-01 00:00:00'),(4,1,'10585d5b-f8bc-4e5b-9c32-c55998b1bc88',1,'2025-01-14 09:42:36','0001-01-01 00:00:00'),(5,1,'9e473755-aaa3-49e8-9f64-b6ed9d24aeb5',1,'2025-01-14 09:44:11','0001-01-01 00:00:00'),(6,1,'0860b616-8e3a-40f7-8c75-c256c9587b86',1,'2025-01-14 10:17:12','0001-01-01 00:00:00'),(7,1,'abc59562-728e-4697-8809-8dc5154e29e4',1,'2025-01-14 10:17:22','0001-01-01 00:00:00'),(8,1,'82d51baf-a8e4-4f7b-a211-38fc722953f5',1,'2025-01-14 10:17:46','0001-01-01 00:00:00'),(9,1,'1d895dea-98db-42f6-8c72-7d06f144bdd7',1,'2025-01-14 10:19:47','0001-01-01 00:00:00'),(10,1,'d9b293b9-afd2-49e7-aedf-1c963beb95c5',1,'2025-01-14 10:21:12','0001-01-01 00:00:00'),(11,1,'815eb984-d3f5-43fe-94d9-d7276a902341',1,'2025-01-14 10:25:21','0001-01-01 00:00:00'),(12,1,'09a4dc7a-2dff-4148-a28d-db6d6b7c8b6e',1,'2025-01-14 10:27:36','0001-01-01 00:00:00'),(13,1,'3dd0f598-a90e-41aa-8aff-1667ae6847e5',1,'2025-01-14 10:29:10','0001-01-01 00:00:00'),(14,1,'7c777055-fbb9-45d6-82c6-c32e5e67ce82',1,'2025-01-14 10:29:13','0001-01-01 00:00:00'),(15,1,'86e54cae-f4d2-4261-abbd-0d1a304313e8',1,'2025-01-14 10:29:42','0001-01-01 00:00:00'),(16,1,'e0a2e4fa-a6ee-4056-a4f4-202101d82600',1,'2025-01-14 10:29:49','0001-01-01 00:00:00'),(17,1,'214a5a82-3c71-4a48-800e-571d4e69fffb',1,'2025-01-14 10:30:06','0001-01-01 00:00:00'),(18,1,'31522c46-9fd2-4cbf-8f7d-dc9e0fbe9e9a',1,'2025-01-14 10:30:36','0001-01-01 00:00:00'),(19,1,'98f85826-b52a-486d-92a7-a71426d1ff30',1,'2025-01-14 10:31:58','0001-01-01 00:00:00'),(20,1,'76e09e5d-c63f-4a1f-a820-3f28ed56155b',1,'2025-01-14 10:33:03','0001-01-01 00:00:00'),(21,1,'7e92586b-afbd-4f46-9314-0229a6ae0fea',1,'2025-01-14 10:36:44','0001-01-01 00:00:00'),(22,1,'4fc50f33-9ed1-46a1-9e55-46f67660d640',0,'2025-01-14 10:38:33','2025-01-14 10:38:34'),(23,1,'7b2e7021-a8ae-4421-89e6-93397e5d61e2',0,'2025-01-14 10:40:03','2025-01-14 10:40:16'),(24,1,'1ac6a2ff-276a-4cb0-a589-5ed94050c313',0,'2025-01-14 11:32:22','2025-01-14 11:33:30'),(25,1,'f597a53a-9511-4699-a6aa-0eeaf49e4519',0,'2025-01-14 11:35:24','2025-01-14 11:36:34'),(26,1,'3705afad-2190-447e-ad29-9f5892193844',0,'2025-01-14 11:37:10','2025-01-14 11:38:49'),(27,1,'598c827e-d3cf-425a-906e-de2fdb1a2469',0,'2025-01-14 11:41:21','2025-01-14 11:43:00'),(28,1,'e59c1a07-db64-49eb-a0d4-a7b5f23388ff',1,'2025-01-14 11:48:32','0001-01-01 00:00:00'),(29,1,'20298d2a-333c-44f6-be02-75a648930c09',0,'2025-01-14 11:50:55','0001-01-01 00:00:00'),(30,1,'251b8e35-e34b-4e12-a959-0f11406698ab',0,'2025-01-15 08:27:58','2025-01-15 08:29:11'),(31,1,'1fb4a29d-f261-4f2a-ad02-3c3a4fb89f99',0,'2025-01-15 08:31:13','2025-01-15 08:32:09'),(32,1,'55428b1c-4b6d-46d1-8849-c99f05fc70e4',0,'2025-01-15 08:34:21','2025-01-15 08:35:18'),(33,1,'79efb236-1f37-4279-945a-005d9e2b4029',0,'2025-01-15 08:40:10','2025-01-15 08:40:43'),(34,1,'8160decd-5a83-48fd-95de-90d36499195e',0,'2025-01-15 08:41:34','2025-01-15 08:42:19'),(35,1,'8b217f6f-94c7-49d4-b61c-83a79bcf928b',0,'2025-01-15 08:49:42','2025-01-15 08:49:53'),(36,1,'55738566-9ee1-4581-9f37-3c0c6010bb75',0,'2025-01-15 08:59:09','2025-01-15 08:59:25'),(37,1,'e3b9ae54-ba53-4088-a8d7-1a09b2d8a880',0,'2025-01-15 09:05:01','2025-01-15 09:05:17'),(38,1,'5bc1b4ef-f4b0-49d9-bf77-216ea6e7e186',0,'2025-01-15 09:37:05','2025-01-15 09:37:27'),(39,1,'b103fc4c-87d9-4672-ac7f-b81b405bf3bf',0,'2025-01-15 10:14:13','2025-01-15 10:14:27'),(40,1,'7fdef7fb-7708-4a82-b2d5-03eb730ed9f2',0,'2025-01-15 10:34:54','2025-01-15 10:35:12'),(41,1,'6d137e63-7712-4975-9ea1-b107708f2536',0,'2025-01-15 11:27:49','2025-01-15 11:28:16'),(42,1,'4498df41-0119-4be0-b6d7-a91527f98c60',0,'2025-01-15 11:28:47','2025-01-15 11:28:55'),(43,1,'7890408b-bb66-4440-97d0-17614b978b95',0,'2025-01-15 11:29:36','2025-01-15 11:29:40'),(44,1,'700f983f-b099-439b-8c11-4a8406d616d7',0,'2025-01-15 11:36:17','2025-01-15 11:36:25'),(45,1,'81996053-8e6e-4207-8207-60d2f01b2443',0,'2025-01-15 11:37:17','2025-01-15 11:37:25'),(46,1,'9582f45e-e89a-48c4-960d-3b76909bd36b',0,'2025-01-15 11:38:07','2025-01-15 11:38:17'),(47,1,'48c93ad3-9683-41fd-8d75-ab1cfb7a5dd1',0,'2025-01-15 11:38:32','2025-01-15 11:38:35'),(48,1,'f2480874-a5ef-4696-84bd-d7ce4a4d05ea',0,'2025-01-15 11:39:39','2025-01-15 11:39:42'),(49,1,'3f5809c1-2709-441a-9448-4139f6da4ece',0,'2025-01-15 11:40:11','2025-01-15 11:40:16'),(50,1,'232c5edb-d0eb-479b-aefc-b976782f7a6a',0,'2025-01-15 11:40:52','2025-01-15 11:40:58'),(51,1,'9698a0e5-ed07-45bc-aaf1-9fd8a801090a',0,'2025-01-15 11:41:22','2025-01-15 11:41:26'),(52,1,'9e85840a-b67d-4479-9f88-6ecbd40b1fb4',0,'2025-01-15 11:43:26','2025-01-15 11:43:33');
/*!40000 ALTER TABLE `token` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2025-01-17 08:43:00
-- Total time: 0:0:0:0:307 (d:h:m:s:ms)
