-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: May 20, 2025 at 07:20 AM
-- Server version: 8.0.30
-- PHP Version: 8.3.21

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `ns`
--
CREATE DATABASE IF NOT EXISTS `ns` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE `ns`;

-- --------------------------------------------------------

--
-- Table structure for table `stations`
--

CREATE TABLE `stations` (
  `id` int NOT NULL,
  `name` varchar(255) NOT NULL,
  `hasLift` tinyint(1) NOT NULL,
  `wheelChairAccessible` tinyint(1) NOT NULL,
  `hasToilet` tinyint(1) NOT NULL,
  `hasKiosk` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `stations`
--

INSERT INTO `stations` (`id`, `name`, `hasLift`, `wheelChairAccessible`, `hasToilet`, `hasKiosk`) VALUES
(1, 'Amsterdam Centraal', 1, 1, 1, 1),
(2, 'Amsterdam Sloterdijk', 1, 1, 1, 1),
(3, 'Amsterdam Zuid', 1, 1, 1, 1),
(4, 'Amsterdam Amstel', 1, 1, 1, 1),
(5, 'Amsterdam Bijlmer ArenA', 1, 1, 1, 1),
(6, 'Amsterdam Science Park', 0, 1, 0, 0),
(7, 'Amsterdam Muiderpoort', 0, 1, 0, 0),
(8, 'Rotterdam Centraal', 1, 1, 1, 1),
(9, 'Rotterdam Alexander', 1, 1, 1, 0),
(10, 'Rotterdam Blaak', 1, 1, 1, 1),
(11, 'Rotterdam Lombardijen', 1, 1, 0, 0),
(12, 'Utrecht Centraal', 1, 1, 1, 1),
(13, 'Utrecht Zuilen', 0, 1, 0, 0),
(14, 'Utrecht Overvecht', 0, 1, 0, 0),
(15, 'Utrecht Lunetten', 0, 1, 0, 0),
(16, 'Den Haag Centraal', 1, 1, 1, 1),
(17, 'Den Haag HS', 1, 1, 1, 1),
(18, 'Den Haag Laan van NOI', 1, 1, 1, 0),
(19, 'Den Haag Mariahoeve', 0, 1, 0, 0),
(20, 'Den Haag Ypenburg', 0, 1, 0, 0),
(21, 'Groningen', 1, 1, 1, 1),
(22, 'Groningen Europapark', 1, 1, 1, 0),
(23, 'Groningen Noord', 0, 1, 0, 0),
(24, 'Hoorn', 1, 1, 1, 1),
(25, 'Hoorn Kersenboogerd', 1, 1, 0, 0),
(26, 'Arnhem Centraal', 1, 1, 1, 1),
(27, 'Arnhem Presikhaaf', 0, 1, 0, 0),
(28, 'Arnhem Velperpoort', 0, 1, 0, 0),
(29, 'Arnhem Zuid', 0, 1, 0, 0),
(30, 'Amersfoort Centraal', 1, 1, 1, 1),
(31, 'Amersfoort Schothorst', 1, 1, 0, 0),
(32, 'Amersfoort Vathorst', 1, 1, 0, 0),
(33, 'Leiden Centraal', 1, 1, 1, 1),
(34, 'De Vink (Leiden)', 0, 1, 0, 0),
(35, 'Eindhoven Centraal', 1, 1, 1, 1),
(36, 'Eindhoven Strijp-S', 1, 1, 1, 0),
(37, 'Haarlem', 1, 1, 1, 1),
(38, 'Haarlem Spaarnwoude', 0, 1, 0, 0),
(39, 'Zwolle', 1, 1, 1, 1),
(40, 'Breda', 1, 1, 1, 1),
(41, 'Tilburg', 1, 1, 1, 1),
(42, 'Tilburg Reeshof', 1, 1, 0, 0),
(43, 'Alkmaar', 1, 1, 1, 1),
(44, 'Alkmaar Noord', 0, 1, 0, 0),
(45, 'Maastricht', 1, 1, 1, 1),
(46, 'Heerlen', 1, 1, 1, 1),
(47, 'Hengelo', 1, 1, 1, 1),
(48, 'Deventer', 1, 1, 1, 1),
(49, 'Assen', 1, 1, 1, 1),
(50, 'Vlissingen', 1, 1, 1, 1),
(51, 'Roermond', 1, 1, 1, 1),
(52, 'Oss', 0, 1, 0, 1),
(53, 'Hilversum', 1, 1, 1, 1),
(54, 'Zaandam', 1, 1, 1, 1),
(55, 'Ede-Wageningen', 1, 1, 1, 1),
(56, 'Enschede', 1, 1, 1, 1),
(57, 'Almere Centrum', 1, 1, 1, 1),
(58, 'Lelystad Centrum', 1, 1, 1, 1),
(59, 'Culemborg', 0, 1, 0, 0),
(60, 'Veenendaal Centrum', 0, 1, 0, 0),
(61, 'Barendrecht', 1, 1, 0, 0),
(62, 'Gouda', 1, 1, 1, 1),
(63, 'Schiphol Airport', 1, 1, 1, 1),
(64, 'Zaandijk Zaanse Schans', 0, 1, 0, 0),
(65, 'Hoofddorp', 1, 1, 1, 1),
(66, 'Houten', 1, 1, 1, 0),
(67, 'Weesp', 1, 1, 1, 1),
(68, 'Dordrecht', 1, 1, 1, 1),
(69, 'Rijswijk', 1, 1, 1, 0),
(70, 'Vlaardingen Centrum', 0, 1, 0, 1),
(71, 'Capelle Schollevaar', 0, 1, 0, 0),
(72, 'Harderwijk', 1, 1, 1, 0),
(73, 'Heerenveen', 1, 1, 1, 1),
(74, 'Meppel', 1, 1, 1, 1),
(75, 'Kampen Zuid', 0, 1, 0, 0),
(76, 'Bussum Zuid', 1, 1, 0, 0),
(77, 'Zandvoort aan Zee', 0, 1, 0, 1),
(78, 'Den Helder', 1, 1, 1, 0);

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `id` int NOT NULL,
  `username` varchar(50) NOT NULL,
  `email` varchar(120) NOT NULL,
  `password` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `user_feedback`
--

CREATE TABLE `user_feedback` (
  `id` int NOT NULL,
  `sender` varchar(255) NOT NULL,
  `shortMessage` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `longMessage` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `date_posted` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `station_id` int NOT NULL,
  `is_approved` tinyint(1) NOT NULL DEFAULT '0',
  `is_reviewed` tinyint(1) NOT NULL DEFAULT '0',
  `reviewer_id` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `stations`
--
ALTER TABLE `stations`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `user_feedback`
--
ALTER TABLE `user_feedback`
  ADD PRIMARY KEY (`id`),
  ADD KEY `station_ids` (`station_id`),
  ADD KEY `user_ids` (`reviewer_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `stations`
--
ALTER TABLE `stations`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=79;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `user_feedback`
--
ALTER TABLE `user_feedback`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `user_feedback`
--
ALTER TABLE `user_feedback`
  ADD CONSTRAINT `station_ids` FOREIGN KEY (`station_id`) REFERENCES `stations` (`id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `user_ids` FOREIGN KEY (`reviewer_id`) REFERENCES `users` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
