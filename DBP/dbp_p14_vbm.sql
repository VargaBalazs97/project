-- phpMyAdmin SQL Dump
-- version 4.8.4
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2019. Feb 07. 00:02
-- Kiszolgáló verziója: 10.1.37-MariaDB
-- PHP verzió: 7.3.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `dbp_p14_vbm`
--
CREATE DATABASE IF NOT EXISTS `dbp_p14_vbm` DEFAULT CHARACTER SET utf8 COLLATE utf8_hungarian_ci;
USE `dbp_p14_vbm`;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `accepted_products`
--

CREATE TABLE `accepted_products` (
  `id` int(11) NOT NULL,
  `product_id` int(11) NOT NULL,
  `piece` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `errors`
--

CREATE TABLE `errors` (
  `id` int(11) NOT NULL,
  `error` varchar(30) COLLATE utf8_hungarian_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `personals`
--

CREATE TABLE `personals` (
  `id` int(11) NOT NULL,
  `personal_number` varchar(8) COLLATE utf8_hungarian_ci NOT NULL,
  `name` varchar(50) COLLATE utf8_hungarian_ci NOT NULL,
  `date_of_birth` date DEFAULT NULL,
  `address` varchar(100) COLLATE utf8_hungarian_ci NOT NULL,
  `email` varchar(50) COLLATE utf8_hungarian_ci NOT NULL,
  `phone_number` varchar(13) COLLATE utf8_hungarian_ci NOT NULL,
  `picture` longblob
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `products`
--

CREATE TABLE `products` (
  `id` int(11) NOT NULL,
  `product_name` varchar(30) COLLATE utf8_hungarian_ci NOT NULL,
  `bar_code` varchar(100) COLLATE utf8_hungarian_ci NOT NULL,
  `picture` longblob NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `product_recording`
--

CREATE TABLE `product_recording` (
  `id` int(11) NOT NULL,
  `date` date NOT NULL,
  `time` time NOT NULL,
  `type` varchar(5) COLLATE utf8_hungarian_ci NOT NULL,
  `product_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `scrap_products`
--

CREATE TABLE `scrap_products` (
  `id` int(11) NOT NULL,
  `product_id` int(11) NOT NULL,
  `error_code` int(11) NOT NULL,
  `piece` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `user_name` varchar(8) COLLATE utf8_hungarian_ci NOT NULL,
  `password` varchar(200) COLLATE utf8_hungarian_ci NOT NULL,
  `type` tinyint(1) NOT NULL,
  `personal_number` varchar(8) COLLATE utf8_hungarian_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `accepted_products`
--
ALTER TABLE `accepted_products`
  ADD PRIMARY KEY (`id`),
  ADD KEY `product_id` (`product_id`);

--
-- A tábla indexei `errors`
--
ALTER TABLE `errors`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `personals`
--
ALTER TABLE `personals`
  ADD PRIMARY KEY (`id`),
  ADD KEY `personal_number` (`personal_number`);

--
-- A tábla indexei `products`
--
ALTER TABLE `products`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `product_recording`
--
ALTER TABLE `product_recording`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user_id` (`user_id`),
  ADD KEY `product_id` (`product_id`);

--
-- A tábla indexei `scrap_products`
--
ALTER TABLE `scrap_products`
  ADD PRIMARY KEY (`id`),
  ADD KEY `error_code` (`error_code`),
  ADD KEY `product_id` (`product_id`);

--
-- A tábla indexei `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD KEY `personal_number` (`personal_number`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `personals`
--
ALTER TABLE `personals`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT a táblához `products`
--
ALTER TABLE `products`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT a táblához `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `accepted_products`
--
ALTER TABLE `accepted_products`
  ADD CONSTRAINT `accepted_products_ibfk_1` FOREIGN KEY (`product_id`) REFERENCES `products` (`id`);

--
-- Megkötések a táblához `product_recording`
--
ALTER TABLE `product_recording`
  ADD CONSTRAINT `product_recording_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`),
  ADD CONSTRAINT `product_recording_ibfk_3` FOREIGN KEY (`product_id`) REFERENCES `products` (`id`);

--
-- Megkötések a táblához `scrap_products`
--
ALTER TABLE `scrap_products`
  ADD CONSTRAINT `scrap_products_ibfk_1` FOREIGN KEY (`error_code`) REFERENCES `errors` (`id`),
  ADD CONSTRAINT `scrap_products_ibfk_2` FOREIGN KEY (`product_id`) REFERENCES `products` (`id`);

--
-- Megkötések a táblához `users`
--
ALTER TABLE `users`
  ADD CONSTRAINT `users_ibfk_1` FOREIGN KEY (`personal_number`) REFERENCES `personals` (`personal_number`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
