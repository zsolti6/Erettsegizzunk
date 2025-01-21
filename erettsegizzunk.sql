-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Jan 21. 12:50
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
CREATE DATABASE IF NOT EXISTS `erettsegizzunk` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_hungarian_ci;
USE `erettsegizzunk`;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `feladatok`
--

CREATE TABLE `feladatok` (
  `id` int(11) NOT NULL,
  `leiras` varchar(1024) DEFAULT NULL,
  `szoveg` varchar(1024) DEFAULT NULL,
  `megoldasok` varchar(1024) DEFAULT NULL,
  `helyese` varchar(20) DEFAULT NULL,
  `tantargyId` int(11) DEFAULT NULL,
  `tipusId` int(11) DEFAULT NULL,
  `szintId` int(11) DEFAULT NULL,
  `kepNev` varchar(18) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `feladatok`
--

INSERT INTO `feladatok` (`id`, `leiras`, `szoveg`, `megoldasok`, `helyese`, `tantargyId`, `tipusId`, `szintId`, `kepNev`) VALUES
(56, 'Adott az A = {1; 2; 3; 4} és a B = {1; 2; 4; 8} halmaz. Elemei felsorolásával adja meg az A ∩ B halmazt vesszővel elválasztva!', NULL, '1,2,4;', '1;', 1, 3, 1, NULL),
(57, 'Adott az A = {1; 2; 3; 4} és a B = {1; 2; 4; 8} halmaz. Elemei felsorolásával adja meg az A ∪ B halmazt vesszővel elválasztva!', NULL, '1,2,3,4,8;', '1;', 1, 3, 1, NULL),
(58, 'Adott az A = {1; 2; 3; 4} és a B = {1; 2; 4; 8} halmaz. Elemei felsorolásával adja meg az A \\ B halmazt vesszővel elválasztva!', NULL, '3;', '1;', 1, 3, 1, NULL),
(59, 'Egy pékségben fehér kenyeret és rozskenyeret is árusítanak. Egyik reggel az első 30 vevő közül 22-en fehér kenyeret, 17-en pedig rozskenyeret vásároltak. Hányan vásároltak mindkét fajta kenyérből, ha mind a 30 vevő vett a két fajta kenyér valamelyikéből?', NULL, '9;', '1;', 1, 3, 1, NULL),
(60, 'Egy számtani sorozat első tagja 6, hetedik tagja pedig 36. Adja meg a sorozat negyedik tagját!', NULL, '21;', '1;', 1, 3, 1, NULL),
(61, 'Hány átlója van egy konvex nyolcszögnek?', NULL, '20;', '1;', 1, 3, 1, NULL),
(62, 'Hány különböző 4-gyel osztható négyjegyű szám készíthető a 2, 3, 4, 5 számjegyekből, ha egy-egy számhoz mindegyik számjegyet egyszer használjuk fel?', NULL, '6;', '1;', 1, 3, 1, NULL),
(63, 'Egy számítógépes játékban Bélának 4-szer annyi pontja van, mint Andrásnak. Hány pontja van Bélának, ha kettejüknek együtt 6500 pontjuk van?', NULL, '5200;', '1;', 1, 3, 1, NULL),
(64, 'Annának két 5-öse, négy 4-ese és két 3-asa van biológiából. Adja meg Anna biológiajegyeinek szórását!', NULL, '0.707;', '1;', 1, 3, 1, NULL),
(65, 'Egy mértani sorozat nyolcadik tagja 1020, kilencedik tagja 1023. Adja meg a sorozat hányadosát!', NULL, '1000;', '1;', 1, 3, 1, NULL),
(66, 'Egy mértani sorozat nyolcadik tagja 1020, kilencedik tagja 1023. Adja meg a sorozat első tagját!', NULL, '0.1;', '1;', 1, 3, 1, NULL),
(67, 'Két szabályos dobókockával egyszer dobunk. Mennyi annak a valószínűsége, hogy a dobott számok összege négyzetszám lesz? ', NULL, '0.194;', '1;', 1, 3, 1, NULL),
(68, 'Adott az f: R → R, f(x) = (x3)2 - 4 függvény. Melyik számot rendeli az f függvény az x = 2,5-hez?', NULL, '3.75;', '1;', 1, 3, 1, NULL),
(69, 'Adott az f: R → R, f(x) = (x3)2 - 4 függvény. Határozza meg az f függvény zérushelyeit vesszővel elválasztva!', NULL, '1,5;', '1;', 1, 3, 1, NULL),
(70, 'Az A és B halmazokról tudjuk, hogy A ∪ B = {1; 2; 3; 4; 5; 6}, A ∩ B = {1; 2} és A \\ B = {3; 4}. Adja meg az A halmazt elemei felsorolásával, vesszővel elválasztva!', NULL, '1,2,3,4;', '1;', 1, 3, 1, NULL),
(71, 'Az A és B halmazokról tudjuk, hogy A ∪ B = {1; 2; 3; 4; 5; 6}, A ∩ B = {1; 2} és A \\ B = {3; 4}. Adja meg a B halmazt elemei felsorolásával, vesszővel elválasztva!', NULL, '1,2,5,6;', '1;', 1, 3, 1, NULL),
(72, 'Egy derékszögű háromszög egyik befogója 24 cm, átfogója 25 cm hosszú. Hány cm hosszú a másik befogó?', NULL, '7;', '1;', 1, 3, 1, NULL),
(73, 'Hány darab négyjegyű, különböző számjegyekből álló (pozitív) páratlan szám alkotható az 1, 2, 3, 4 számjegyekből?', NULL, '12;', '1;', 1, 3, 1, NULL),
(74, 'Adja meg a értékét, ha tudjuk, hogy a1/2 = 4.', NULL, '16;', '1;', 1, 3, 1, NULL),
(75, 'Egy számtani sorozat nyolcadik tagja 6-tal nagyobb, mint a negyedik tagja. A sorozat hatodik tagja 6. Számítsa ki a sorozat első 6 tagjának az összegét!', NULL, '13.5;', '1;', 1, 3, 1, NULL),
(76, 'Hány csúcsa van egy hatszög alapú gúlának?', NULL, '7;', '1;', 1, 3, 1, NULL),
(77, 'Hány lapja van egy hatszög alapú gúlának?', NULL, '7;', '1;', 1, 3, 1, NULL),
(78, 'Hány éle van egy hatszög alapú gúlának?', NULL, '12;', '1;', 1, 3, 1, NULL),
(79, 'Egy szám 2-es alapú logaritmusa 6. Mennyi a szám kétszeresének a 2-es alapú logaritmusa?', NULL, '7;', '1;', 1, 3, 1, NULL),
(80, 'Egy városban a polgármester-választáson a győztes jelöltre a szavazáson résztvevők 55%-a szavazott, így 10 593 szavazatot kapott. Hányan vettek részt ebben a városban a szavazáson?', NULL, '19260;', '1;', 1, 3, 1, NULL),
(81, 'Balázs magyar irodalomból a következő jegyeket szerezte az első félévben: 1, 5, 5, 5. Számítsa ki Balázs jegyeinek átlagát!', NULL, '4;', '1;', 1, 3, 1, NULL),
(82, 'Balázs magyar irodalomból a következő jegyeket szerezte az első félévben: 1, 5, 5, 5. Számítsa ki Balázs jegyeinek szórását!', NULL, '1.73;', '1;', 1, 3, 1, NULL),
(83, 'Egy piros, egy fekete és egy fehér szabályos dobókockával egyszerre dobunk. Határozza meg annak a valószínűségét, hogy a dobás eredménye három különböző szám lesz!', NULL, '0.556;', '1;', 1, 3, 1, NULL),
(84, 'Oldja meg a valós számok halmazán az alábbi egyenletet! 18 ∙ (7x + 96) + 19 ∙ (5x – 56) = 1990', NULL, '6;', '1;', 1, 3, 1, NULL),
(85, 'Írja fel az 1896 és az 1956 prímtényezős felbontását, és adja meg az 1896 és az 1956 összes közös (pozitív) osztóját vesszővel elválasztva!', NULL, '1,2,3,4,6,12;', '1;', 1, 3, 1, NULL),
(86, 'Egy szabályos tízszög egy oldalának hossza 10 cm. Számítsa ki a tízszög területét!', NULL, '770;', '1;', 1, 3, 1, NULL),
(87, 'Egy szabályos sokszög átlóinak a száma 2015. Hány oldalú a sokszög?', NULL, '65;', '1;', 1, 3, 1, NULL),
(88, 'Egy építkezésre teherautókkal szállítják a homokot. Öt egyforma teherautó mindegyikének nyolcszor kellene fordulnia, hogy az összes homokot odaszállítsák. Hány fordulóval tudná odaszállítani ugyanezt a mennyiségű homokot négy ugyanekkora teherautó?', NULL, '10;', '1;', 1, 3, 1, NULL),
(89, 'Egy derékszögű háromszög két befogója 10 és 24 cm hosszú. Számítsa ki az átfogó hosszát!', NULL, '26;', '1;', 1, 3, 1, NULL),
(90, 'Egy meteorológiai állomáson november első hetében az alábbi napi hőmérsékleti maximumokat mérték (°C-ban): 9, 5, 6, 9, 6, 6, 8. Adja meg az adatok átlagát!', NULL, '7;', '1;', 1, 3, 1, NULL),
(91, 'Egy meteorológiai állomáson november első hetében az alábbi napi hőmérsékleti maximumokat mérték (°C-ban): 9, 5, 6, 9, 6, 6, 8. Adja meg az adatok terjedelmét!', NULL, '4;', '1;', 1, 3, 1, NULL),
(92, 'Egy meteorológiai állomáson november első hetében az alábbi napi hőmérsékleti maximumokat mérték (°C-ban): 9, 5, 6, 9, 6, 6, 8. Adja meg az adatok mediánját!', NULL, '6;', '1;', 1, 3, 1, NULL),
(93, 'Egy dobozban 10 piros és néhány zöld golyó van. Tudjuk, hogy ha egy golyót kihúzunk véletlenszerűen a dobozból, akkor 2/3 annak a valószínűsége, hogy a golyó piros. Hány zöld golyó van a dobozban?', NULL, '5;', '1;', 1, 3, 1, NULL),
(94, 'Egy vasúti tartálykocsi tömege üres tartállyal 23,8 tonna. Ebben a tartálykocsiban maximum 60 000 liter üzemanyagot szállíthatnak. Egy liter üzemanyag tömege 0,85 kg. Hány tonna a tartálykocsi tömege tele tartállyal?', NULL, '74.8;', '1;', 1, 3, 1, NULL),
(95, 'Egy kör egyenlete: (x - 2)2 + (y - 4)2 = 25. Adja meg a kör 1pontjának koordinátáit!', NULL, '2,4;', '1;', 1, 3, 1, NULL),
(96, 'Egy kör egyenlete: (x - 2)2 + (y - 4)2 = 25. Adja meg a kör sugarát!', NULL, '5;', '1;', 1, 3, 1, NULL),
(97, 'Egy szabályos pénzérmét háromszor feldobunk. Határozza meg annak a valószínűségét, hogy a három dobás közül pontosan egy lesz fej!', NULL, '0.375;', '1;', 1, 3, 1, NULL),
(98, 'Adott a valós számok halmazán értelmezett f függvény: f(x) = (x - 3)2 + 2. Mit rendel az f függvény az x = 3,5-hez?', NULL, '2.25;', '1;', 1, 3, 1, NULL),
(99, 'Adott a valós számok halmazán értelmezett f függvény: f(x) = (x - 3)2 + 2. Mely számokhoz rendeli az f függvény a 6-ot?', NULL, '1.5;', '1;', 1, 3, 1, NULL),
(100, 'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző! Pillanatnyi benyomások rögzítése, nominális stílus.', NULL, 'impresszionizmus;klasszicizmus;naturalizmus;romantika', '1;0;0;0', 3, 1, 1, NULL),
(101, 'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző! Minta- és szabálykövetés: pl. a drámákban a hármas egység szabálya.', NULL, 'impresszionizmus;klasszicizmus;naturalizmus;romantika', '0;1;0;0', 3, 1, 1, NULL),
(102, 'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző! A természettudományokra jellemző megfigyelési mód, dokumentatív hitelesség, biológiailag és társadalmilag determinált szereplők.', NULL, 'impresszionizmus;klasszicizmus;naturalizmus;romantika', '0;0;1;0', 3, 1, 1, NULL),
(103, 'Állapítsa meg, hogy az alábbi információ, állítás melyik adott korstílusra, stílusirányzatra jellemző! A műnemek, műfajok keveredése, erős lirizálódás, eredetiség.', NULL, 'impresszionizmus;klasszicizmus;naturalizmus;romantika', '0;0;0;1', 3, 1, 1, NULL);

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
-- Tábla szerkezet ehhez a táblához `permission`
--

CREATE TABLE `permission` (
  `Id` int(11) NOT NULL,
  `Level` int(1) DEFAULT NULL,
  `Name` varchar(32) DEFAULT NULL,
  `Description` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `permission`
--

INSERT INTO `permission` (`Id`, `Level`, `Name`, `Description`) VALUES
(1, 0, 'Luzer', 'Webes regisztráció felhasználó'),
(2, 9, 'Administrator', 'Rendszergazda');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `szint`
--

CREATE TABLE `szint` (
  `id` int(11) NOT NULL,
  `nev` varchar(15) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `szint`
--

INSERT INTO `szint` (`id`, `nev`) VALUES
(1, 'közép'),
(2, 'emelt');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `tantargyak`
--

CREATE TABLE `tantargyak` (
  `id` int(11) NOT NULL,
  `nev` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `tantargyak`
--

INSERT INTO `tantargyak` (`id`, `nev`) VALUES
(1, 'matematika'),
(2, 'történelem'),
(3, 'magyar');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `tema`
--

CREATE TABLE `tema` (
  `id` int(11) NOT NULL,
  `nev` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `tipus`
--

CREATE TABLE `tipus` (
  `id` int(11) NOT NULL,
  `nev` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `tipus`
--

INSERT INTO `tipus` (`id`, `nev`) VALUES
(1, 'radio'),
(2, 'checkbox'),
(3, 'textbox');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `token`
--

CREATE TABLE `token` (
  `id` int(11) NOT NULL,
  `userId` int(11) DEFAULT NULL,
  `token` varchar(40) DEFAULT NULL,
  `aktiv` tinyint(1) DEFAULT NULL,
  `login` datetime DEFAULT NULL,
  `logout` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `token`
--

INSERT INTO `token` (`id`, `userId`, `token`, `aktiv`, `login`, `logout`) VALUES
(1, 1, '4abda363-75a9-4ac0-9496-659d71cbf155', 0, '2025-01-19 15:17:19', '2025-01-19 15:18:05'),
(2, 1, 'e662c6c5-bc35-40e8-831b-f24c42b146a0', 0, '2025-01-19 15:19:24', '2025-01-19 15:19:33'),
(3, 1, 'b7510371-5b05-4001-9b55-f73dc22f64ac', 0, '2025-01-19 15:20:03', '2025-01-19 15:20:26'),
(4, 1, '97784509-80f5-4c0b-971b-4f4226a07a56', 0, '2025-01-19 15:20:41', '2025-01-19 15:20:55'),
(5, 1, '7f2fd286-4987-4ebe-9055-ad57fb342861', 0, '2025-01-19 15:21:57', '2025-01-19 16:48:35');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `user`
--

CREATE TABLE `user` (
  `Id` int(11) NOT NULL,
  `LoginName` varchar(16) NOT NULL,
  `HASH` varchar(64) NOT NULL,
  `SALT` varchar(64) NOT NULL,
  `Name` varchar(64) DEFAULT NULL,
  `PermissionId` int(11) NOT NULL,
  `Active` tinyint(1) NOT NULL,
  `Email` varchar(64) NOT NULL,
  `ProfilePicturePath` varchar(64) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `user`
--

INSERT INTO `user` (`Id`, `LoginName`, `HASH`, `SALT`, `Name`, `PermissionId`, `Active`, `Email`, `ProfilePicturePath`) VALUES
(1, 'kerenyir', 'd5fe0e517520122f1ab363b6b7ee9ae616e7ad393693ef00d81a7f287a79931a', 'Gm63C4jiWnYvfZfiKUu2cu8AHPNDj8NoHhtQn88yiJhyOunBNSd7tRoWo5wwqg9X', 'Kerényi Róbert', 2, 1, 'kerenyir@kkszki.hu', 'img\\kerenyir.jpg');

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
-- A tábla indexei `permission`
--
ALTER TABLE `permission`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Szint` (`Level`),
  ADD UNIQUE KEY `Nev` (`Name`);

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
-- A tábla indexei `token`
--
ALTER TABLE `token`
  ADD PRIMARY KEY (`id`),
  ADD KEY `userId` (`userId`);

--
-- A tábla indexei `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `LoginNev` (`LoginName`),
  ADD UNIQUE KEY `Email` (`Email`),
  ADD KEY `Jog` (`PermissionId`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `feladatok`
--
ALTER TABLE `feladatok`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=104;

--
-- AUTO_INCREMENT a táblához `permission`
--
ALTER TABLE `permission`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT a táblához `szint`
--
ALTER TABLE `szint`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT a táblához `tantargyak`
--
ALTER TABLE `tantargyak`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT a táblához `tema`
--
ALTER TABLE `tema`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `tipus`
--
ALTER TABLE `tipus`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT a táblához `token`
--
ALTER TABLE `token`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT a táblához `user`
--
ALTER TABLE `user`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `feladatok`
--
ALTER TABLE `feladatok`
  ADD CONSTRAINT `feladatok_ibfk_1` FOREIGN KEY (`tantargyId`) REFERENCES `tantargyak` (`id`),
  ADD CONSTRAINT `feladatok_ibfk_2` FOREIGN KEY (`tipusId`) REFERENCES `tipus` (`id`),
  ADD CONSTRAINT `feladatok_ibfk_3` FOREIGN KEY (`szintId`) REFERENCES `szint` (`id`);

--
-- Megkötések a táblához `feladatok_tema`
--
ALTER TABLE `feladatok_tema`
  ADD CONSTRAINT `feladatok_tema_ibfk_1` FOREIGN KEY (`feladatokId`) REFERENCES `feladatok` (`id`),
  ADD CONSTRAINT `feladatok_tema_ibfk_2` FOREIGN KEY (`temaId`) REFERENCES `tema` (`id`);

--
-- Megkötések a táblához `token`
--
ALTER TABLE `token`
  ADD CONSTRAINT `token_ibfk_1` FOREIGN KEY (`userId`) REFERENCES `user` (`Id`);

--
-- Megkötések a táblához `user`
--
ALTER TABLE `user`
  ADD CONSTRAINT `user_ibfk_1` FOREIGN KEY (`PermissionId`) REFERENCES `permission` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
