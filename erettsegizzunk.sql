-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 10, 2024 at 06:29 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `erettsegizzunk`
--
CREATE DATABASE IF NOT EXISTS `erettsegizzunk` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_hungarian_ci;
USE `erettsegizzunk`;

DELIMITER $$
--
-- Procedures
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetFilteredRandomFeladat` (IN `Tantargy` VARCHAR(255), IN `Szint` VARCHAR(255))   BEGIN
    SELECT 
        feladatok.id,
        feladatok.leiras,
        feladatok.megoldasok,
        feladatok.helyese,
        feladatok.tantargyId,
        feladatok.tipusId,
        feladatok.szintId,

        -- Tantargyak
        tantargyak.id AS TantargyOwnId,
        tantargyak.nev AS TantargyNev,

        -- Szint
        szint.id AS SzintOwnId,
        szint.nev AS SzintNev,

        -- Tipus
        tipus.id AS TipusOwnId,
        tipus.nev AS TipusNev,

        -- Tema (optional based on LEFT JOIN)
        tema.id AS TemaOwnId,
        tema.nev AS TemaNev

    FROM feladatok
    INNER JOIN tantargyak ON tantargyak.id = feladatok.tantargyId
    INNER JOIN szint ON szint.id = feladatok.szintId
    INNER JOIN tipus ON tipus.id = feladatok.tipusId
    LEFT JOIN feladatok_tema ON feladatok_tema.feladatokId = feladatok.id
    LEFT JOIN tema ON tema.id = feladatok_tema.temaId

    WHERE tantargyak.nev = Tantargy AND szint.nev = Szint
    ORDER BY RAND()
    LIMIT 15;
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `feladatok`
--

CREATE TABLE `feladatok` (
  `id` int(11) NOT NULL,
  `leiras` varchar(1024) DEFAULT NULL,
  `megoldasok` varchar(1024) DEFAULT NULL,
  `helyese` varchar(20) DEFAULT NULL,
  `tantargyId` int(11) DEFAULT NULL,
  `tipusId` int(11) DEFAULT NULL,
  `szintId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- Dumping data for table `feladatok`
--

INSERT INTO `feladatok` (`id`, `leiras`, `megoldasok`, `helyese`, `tantargyId`, `tipusId`, `szintId`) VALUES
(1, 'igen', 'jo;jo;rossz;rossz', '1;1;0;0', 2, 2, 1),
(2, 'jah', 'jo;rossz;jo;rossz', '1;0;1;0', 2, 2, 1),
(3, 'vm', 'asd', '1;1;0;0', 1, 1, 2),
(4, 'jahezisaz', 'jo;nem;nem;nem', '1;0;0;0', 2, 1, 1),
(5, 'ez megint töri', 'nem;jo;nem;nem', '0;1;0;0', 2, 1, 1),
(6, 'ezten itt most madzsar', 'fejtse kifele bazdki', 'mittom en', 3, 3, 1),
(7, 'ezten magyjar tyú', 'emettet fejtesd tyú', 'nem tom', 3, 3, 2);

-- --------------------------------------------------------

--
-- Table structure for table `feladatok_tema`
--

CREATE TABLE `feladatok_tema` (
  `feladatokId` int(11) NOT NULL,
  `temaId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Table structure for table `szint`
--

CREATE TABLE `szint` (
  `id` int(11) NOT NULL,
  `nev` varchar(15) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- Dumping data for table `szint`
--

INSERT INTO `szint` (`id`, `nev`) VALUES
(1, 'közép'),
(2, 'emelt');

-- --------------------------------------------------------

--
-- Table structure for table `tantargyak`
--

CREATE TABLE `tantargyak` (
  `id` int(11) NOT NULL,
  `nev` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- Dumping data for table `tantargyak`
--

INSERT INTO `tantargyak` (`id`, `nev`) VALUES
(1, 'matematika'),
(2, 'történelem'),
(3, 'magyar');

-- --------------------------------------------------------

--
-- Table structure for table `tema`
--

CREATE TABLE `tema` (
  `id` int(11) NOT NULL,
  `nev` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tipus`
--

CREATE TABLE `tipus` (
  `id` int(11) NOT NULL,
  `nev` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- Dumping data for table `tipus`
--

INSERT INTO `tipus` (`id`, `nev`) VALUES
(1, 'radio'),
(2, 'checkbox'),
(3, 'textbox');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `feladatok`
--
ALTER TABLE `feladatok`
  ADD PRIMARY KEY (`id`),
  ADD KEY `tantargyId` (`tantargyId`),
  ADD KEY `tipusId` (`tipusId`),
  ADD KEY `szintId` (`szintId`);

--
-- Indexes for table `feladatok_tema`
--
ALTER TABLE `feladatok_tema`
  ADD PRIMARY KEY (`feladatokId`,`temaId`),
  ADD KEY `temaId` (`temaId`);

--
-- Indexes for table `szint`
--
ALTER TABLE `szint`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tantargyak`
--
ALTER TABLE `tantargyak`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tema`
--
ALTER TABLE `tema`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tipus`
--
ALTER TABLE `tipus`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `feladatok`
--
ALTER TABLE `feladatok`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `szint`
--
ALTER TABLE `szint`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `tantargyak`
--
ALTER TABLE `tantargyak`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `tema`
--
ALTER TABLE `tema`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tipus`
--
ALTER TABLE `tipus`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `feladatok`
--
ALTER TABLE `feladatok`
  ADD CONSTRAINT `feladatok_ibfk_1` FOREIGN KEY (`tantargyId`) REFERENCES `tantargyak` (`id`),
  ADD CONSTRAINT `feladatok_ibfk_2` FOREIGN KEY (`tipusId`) REFERENCES `tipus` (`id`),
  ADD CONSTRAINT `feladatok_ibfk_3` FOREIGN KEY (`szintId`) REFERENCES `szint` (`id`);

--
-- Constraints for table `feladatok_tema`
--
ALTER TABLE `feladatok_tema`
  ADD CONSTRAINT `feladatok_tema_ibfk_1` FOREIGN KEY (`feladatokId`) REFERENCES `feladatok` (`id`),
  ADD CONSTRAINT `feladatok_tema_ibfk_2` FOREIGN KEY (`temaId`) REFERENCES `tema` (`id`);
COMMIT;


DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetFilteredRandomFeladat`(IN `Tantargy` VARCHAR(255), IN `Szint` VARCHAR(255))
BEGIN
    SELECT 
        feladatok.id,
        feladatok.leiras,
        feladatok.megoldasok,
        feladatok.helyese,
        feladatok.tantargyId,
        feladatok.tipusId,
        feladatok.szintId,

        -- Tantargyak
        tantargyak.id AS TantargyOwnId,
        tantargyak.nev AS TantargyNev,

        -- Szint
        szint.id AS SzintOwnId,
        szint.nev AS SzintNev,

        -- Tipus
        tipus.id AS TipusOwnId,
        tipus.nev AS TipusNev,

        -- Tema (optional based on LEFT JOIN)
        tema.id AS TemaOwnId,
        tema.nev AS TemaNev

    FROM feladatok
    INNER JOIN tantargyak ON tantargyak.id = feladatok.tantargyId
    INNER JOIN szint ON szint.id = feladatok.szintId
    INNER JOIN tipus ON tipus.id = feladatok.tipusId
    LEFT JOIN feladatok_tema ON feladatok_tema.feladatokId = feladatok.id
    LEFT JOIN tema ON tema.id = feladatok_tema.temaId

    WHERE tantargyak.nev = Tantargy AND szint.nev = Szint
    ORDER BY RAND()
    LIMIT 15;
END$$
DELIMITER ;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
