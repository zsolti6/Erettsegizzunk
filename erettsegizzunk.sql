-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Feb 06. 14:14
-- Kiszolgáló verziója: 10.4.28-MariaDB
-- PHP verzió: 8.2.4

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
CREATE DATABASE IF NOT EXISTS `erettsegizzunk` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_hungarian_ci;
USE `erettsegizzunk`;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `level`
--

CREATE TABLE `level` (
  `id` int(11) NOT NULL,
  `name` varchar(15) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `level`
--

INSERT INTO `level` (`id`, `name`) VALUES
(1, 'közép'),
(2, 'emelt');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `permission`
--

CREATE TABLE `permission` (
  `id` int(11) NOT NULL,
  `level` int(1) DEFAULT NULL,
  `name` varchar(32) DEFAULT NULL,
  `description` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `permission`
--

INSERT INTO `permission` (`id`, `level`, `name`, `description`) VALUES
(1, 0, 'Luzer', 'Webes regisztráció felhasználó'),
(2, 9, 'Administrator', 'Rendszergazda');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `spaced_repetition`
--

CREATE TABLE `spaced_repetition` (
  `id` int(11) NOT NULL,
  `userId` int(11) NOT NULL,
  `taskId` int(11) NOT NULL,
  `lastCorrectTime` timestamp NOT NULL DEFAULT current_timestamp(),
  `intervalDays` int(11) NOT NULL DEFAULT 1,
  `nextDueTime` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `subject`
--

CREATE TABLE `subject` (
  `id` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `subject`
--

INSERT INTO `subject` (`id`, `name`) VALUES
(1, 'matematika'),
(2, 'történelem'),
(3, 'magyar');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `task`
--

CREATE TABLE `task` (
  `id` int(11) NOT NULL,
  `description` varchar(1024) DEFAULT NULL,
  `text` varchar(1024) DEFAULT NULL,
  `answers` varchar(1024) DEFAULT NULL,
  `isCorrect` varchar(20) DEFAULT NULL,
  `subjectId` int(11) DEFAULT NULL,
  `typeId` int(11) DEFAULT NULL,
  `levelId` int(11) DEFAULT NULL,
  `picName` varchar(18) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `task`
--

INSERT INTO `task` (`id`, `description`, `text`, `answers`, `isCorrect`, `subjectId`, `typeId`, `levelId`, `picName`) VALUES
(13, 'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!', 'Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.', 'impresszionizmus;klasszicizmus;naturalizmus;romantika', '0;1;0;0', 3, 1, 1, 'asd.png'),
(14, 'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!', 'A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.', 'impresszionizmus;klasszicizmus;naturalizmus;romantika', '0;0;1;0', 3, 1, 1, NULL),
(15, 'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző!', 'A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.', 'impresszionizmus;klasszicizmus;naturalizmus;romantika', '0;0;0;1', 3, 1, 1, 'asd.png');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `task_theme`
--

CREATE TABLE `task_theme` (
  `taskId` int(11) NOT NULL,
  `themeId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `theme`
--

CREATE TABLE `theme` (
  `id` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `type`
--

CREATE TABLE `type` (
  `id` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `type`
--

INSERT INTO `type` (`id`, `name`) VALUES
(1, 'radio'),
(2, 'checkbox'),
(3, 'textbox');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `user`
--

CREATE TABLE `user` (
  `id` int(11) NOT NULL,
  `loginName` varchar(16) NOT NULL,
  `HASH` varchar(64) NOT NULL,
  `SALT` varchar(64) NOT NULL,
  `email` varchar(64) NOT NULL,
  `permissionId` int(11) NOT NULL,
  `active` tinyint(1) NOT NULL DEFAULT 0,
  `newsletter` tinyint(1) NOT NULL DEFAULT 0,
  `profilePicturePath` varchar(64) DEFAULT NULL,
  `signupDate` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `user`
--

INSERT INTO `user` (`id`, `loginName`, `HASH`, `SALT`, `email`, `permissionId`, `active`, `newsletter`, `profilePicturePath`, `signupDate`) VALUES
(1, 'kerenyir', '12c2d33dd731e56e72092034c052c5f3da97f78781746546c672a59f1400b781', 'Gm63C4jiWnYvfZfiKUu2cu8AHPNDj8NoHhtQn88yiJhyOunBNSd7tRoWo5wwqg9X', 'kerenyir@kkszki.hu', 2, 1, 0, 'igen.jpg', '2025-01-26 13:50:51'),
(9, 'a', 'fac680db4e70db69fa8752a598c8804967c173f35aaf3b349add87468f04c117', 'qrFsrweUIYT6zdkX4P25t1AnPgpN2dbYTaNpw87ATjoFIrFrLoHp05CUuR0hp8Pl', 'ezazemailem', 2, 1, 0, NULL, '2025-01-27 10:11:23'),
(13, 'zsóti', '869a6e791b254d25dd4aa21c3b7f798f53c1c2b07d035af886016edc339f848a', 'nEJ3RLHYKtSWtiLkdQOLAPKu5LKsxuJejWs0gQClkRMCIpcHi1rArpuBz3RDgg8W', 'gasparzs@kkszki.hu', 2, 1, 0, NULL, '2025-01-28 07:40:25');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `user_statistics`
--

CREATE TABLE `user_statistics` (
  `id` int(11) NOT NULL,
  `userId` int(11) NOT NULL,
  `statisticsDate` date DEFAULT NULL,
  `mathSuccessfulTasks` text DEFAULT NULL,
  `mathUnsuccessfulTasks` text DEFAULT NULL,
  `historySuccessfulTasks` text DEFAULT NULL,
  `historyUnsuccessfulTasks` text DEFAULT NULL,
  `hungarianSuccessfulTasks` text DEFAULT NULL,
  `hungarianUnsuccessfulTasks` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `level`
--
ALTER TABLE `level`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `permission`
--
ALTER TABLE `permission`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `level` (`level`),
  ADD UNIQUE KEY `name` (`name`);

--
-- A tábla indexei `spaced_repetition`
--
ALTER TABLE `spaced_repetition`
  ADD PRIMARY KEY (`id`),
  ADD KEY `userId` (`userId`),
  ADD KEY `taskId` (`taskId`);

--
-- A tábla indexei `subject`
--
ALTER TABLE `subject`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `task`
--
ALTER TABLE `task`
  ADD PRIMARY KEY (`id`),
  ADD KEY `subjectId` (`subjectId`),
  ADD KEY `typeId` (`typeId`),
  ADD KEY `levelId` (`levelId`);

--
-- A tábla indexei `task_theme`
--
ALTER TABLE `task_theme`
  ADD PRIMARY KEY (`taskId`,`themeId`),
  ADD KEY `themeId` (`themeId`);

--
-- A tábla indexei `theme`
--
ALTER TABLE `theme`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `type`
--
ALTER TABLE `type`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `loginName` (`loginName`),
  ADD UNIQUE KEY `email` (`email`),
  ADD KEY `permission` (`permissionId`);

--
-- A tábla indexei `user_statistics`
--
ALTER TABLE `user_statistics`
  ADD PRIMARY KEY (`id`),
  ADD KEY `userId` (`userId`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `level`
--
ALTER TABLE `level`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT a táblához `permission`
--
ALTER TABLE `permission`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT a táblához `spaced_repetition`
--
ALTER TABLE `spaced_repetition`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `subject`
--
ALTER TABLE `subject`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT a táblához `task`
--
ALTER TABLE `task`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT a táblához `theme`
--
ALTER TABLE `theme`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `type`
--
ALTER TABLE `type`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT a táblához `user`
--
ALTER TABLE `user`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT a táblához `user_statistics`
--
ALTER TABLE `user_statistics`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `spaced_repetition`
--
ALTER TABLE `spaced_repetition`
  ADD CONSTRAINT `spaced_repetition_ibfk_1` FOREIGN KEY (`userId`) REFERENCES `user` (`id`),
  ADD CONSTRAINT `spaced_repetition_ibfk_2` FOREIGN KEY (`taskId`) REFERENCES `task` (`id`);

--
-- Megkötések a táblához `task`
--
ALTER TABLE `task`
  ADD CONSTRAINT `task_ibfk_1` FOREIGN KEY (`subjectId`) REFERENCES `subject` (`id`),
  ADD CONSTRAINT `task_ibfk_2` FOREIGN KEY (`typeId`) REFERENCES `type` (`id`),
  ADD CONSTRAINT `task_ibfk_3` FOREIGN KEY (`levelId`) REFERENCES `level` (`id`);

--
-- Megkötések a táblához `task_theme`
--
ALTER TABLE `task_theme`
  ADD CONSTRAINT `task_theme_ibfk_1` FOREIGN KEY (`taskId`) REFERENCES `task` (`id`),
  ADD CONSTRAINT `task_theme_ibfk_2` FOREIGN KEY (`themeId`) REFERENCES `theme` (`id`);

--
-- Megkötések a táblához `user`
--
ALTER TABLE `user`
  ADD CONSTRAINT `user_ibfk_1` FOREIGN KEY (`permissionId`) REFERENCES `permission` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Megkötések a táblához `user_statistics`
--
ALTER TABLE `user_statistics`
  ADD CONSTRAINT `user_statistics_ibfk_1` FOREIGN KEY (`userId`) REFERENCES `user` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
