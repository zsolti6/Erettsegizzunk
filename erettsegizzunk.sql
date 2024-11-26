-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2024. Nov 26. 08:21
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `erettsegizzunk`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `feladatok`
--

CREATE TABLE `feladatok` (
  `id` int(11) NOT NULL,
  `nev` varchar(255) NOT NULL,
  `leiras` varchar(255) DEFAULT NULL,
  `helyesMegoldas` varchar(255) DEFAULT NULL,
  `rosszMegoldas1` varchar(255) DEFAULT NULL,
  `rosszMegoldas2` varchar(255) DEFAULT NULL,
  `rosszMegoldas3` varchar(255) DEFAULT NULL,
  `tantargyId` int(11) DEFAULT NULL,
  `tipusId` int(11) DEFAULT NULL,
  `szintId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `feladatok_tema`
--

CREATE TABLE `feladatok_tema` (
  `feladatokId` int(11) NOT NULL,
  `temaId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `szint`
--

CREATE TABLE `szint` (
  `id` int(11) NOT NULL,
  `nev` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `szint`
--

INSERT INTO `szint` (`id`, `nev`) VALUES
(1, 'emelt'),
(2, 'közép');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `tantargyak`
--

CREATE TABLE `tantargyak` (
  `id` int(11) NOT NULL,
  `nev` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `tantargyak`
--

INSERT INTO `tantargyak` (`id`, `nev`) VALUES
(1, 'matek'),
(2, 'matematika'),
(3, 'történelem'),
(4, 'magyar');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `tema`
--

CREATE TABLE `tema` (
  `id` int(11) NOT NULL,
  `nev` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `tema`
--

INSERT INTO `tema` (`id`, `nev`) VALUES
(1, 'algebra'),
(2, 'halmazok'),
(3, 'kombinatorika'),
(4, 'koordinátageometria'),
(5, 'síkgeometria'),
(6, 'sorozatok'),
(7, 'statisztika'),
(8, 'térgeometria'),
(9, 'trigonometria');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `tipus`
--

CREATE TABLE `tipus` (
  `id` int(11) NOT NULL,
  `nev` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `tipus`
--

INSERT INTO `tipus` (`id`, `nev`) VALUES
(1, 'radio'),
(2, 'radio'),
(3, 'negyzet'),
(4, 'sajat');

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `feladatok`
--
ALTER TABLE `feladatok`
  ADD PRIMARY KEY (`id`),
  ADD KEY `tantargyId` (`tantargyId`),
  ADD KEY `tipusId` (`tipusId`),
  ADD KEY `szintId` (`szintId`);

--
-- A tábla indexei `feladatok_tema`
--
ALTER TABLE `feladatok_tema`
  ADD PRIMARY KEY (`feladatokId`,`temaId`),
  ADD KEY `temaId` (`temaId`);

--
-- A tábla indexei `szint`
--
ALTER TABLE `szint`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `tantargyak`
--
ALTER TABLE `tantargyak`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `tema`
--
ALTER TABLE `tema`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `tipus`
--
ALTER TABLE `tipus`
  ADD PRIMARY KEY (`id`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `feladatok`
--
ALTER TABLE `feladatok`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `szint`
--
ALTER TABLE `szint`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT a táblához `tantargyak`
--
ALTER TABLE `tantargyak`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT a táblához `tema`
--
ALTER TABLE `tema`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT a táblához `tipus`
--
ALTER TABLE `tipus`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `feladatok`
--
ALTER TABLE `feladatok`
  ADD CONSTRAINT `feladatok_ibfk_1` FOREIGN KEY (`tantargyId`) REFERENCES `tantargyak` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `feladatok_ibfk_2` FOREIGN KEY (`tipusId`) REFERENCES `tipus` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `feladatok_ibfk_3` FOREIGN KEY (`szintId`) REFERENCES `szint` (`id`) ON DELETE CASCADE;

--
-- Megkötések a táblához `feladatok_tema`
--
ALTER TABLE `feladatok_tema`
  ADD CONSTRAINT `feladatok_tema_ibfk_1` FOREIGN KEY (`feladatokId`) REFERENCES `feladatok` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `feladatok_tema_ibfk_2` FOREIGN KEY (`temaId`) REFERENCES `tema` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
