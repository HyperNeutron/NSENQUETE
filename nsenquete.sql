-- phpMyAdmin SQL Dump
-- version 5.2.2
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: May 11, 2025 at 10:07 PM
-- Server version: 8.0.30
-- PHP Version: 8.1.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `nsenquete`
--

-- --------------------------------------------------------

--
-- Table structure for table `netherlands_train_stations`
--

CREATE TABLE `netherlands_train_stations` (
  `id` int NOT NULL,
  `name` varchar(255) NOT NULL,
  `hasLift` tinyint(1) NOT NULL,
  `wheelChairAccessible` tinyint(1) NOT NULL,
  `hasToilet` tinyint(1) NOT NULL,
  `hasKiosk` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `netherlands_train_stations`
--

INSERT INTO `netherlands_train_stations` (`id`, `name`, `hasLift`, `wheelChairAccessible`, `hasToilet`, `hasKiosk`) VALUES
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
(25, 'Hoorn Kersenboogerd', 1, 1, 0, 0);

-- --------------------------------------------------------

--
-- Table structure for table `processed_feedback`
--

CREATE TABLE `processed_feedback` (
  `id` int NOT NULL,
  `name` varchar(50) NOT NULL,
  `small_story` varchar(100) NOT NULL,
  `feedback` varchar(500) NOT NULL,
  `station` int NOT NULL,
  `is_approved` tinyint(1) NOT NULL,
  `approved_on` datetime NOT NULL,
  `approved_by` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
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
  `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `small_story` varchar(100) NOT NULL,
  `feedback` varchar(500) NOT NULL,
  `station` varchar(50) NOT NULL,
  `feedback_timestamp` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `netherlands_train_stations`
--
ALTER TABLE `netherlands_train_stations`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `processed_feedback`
--
ALTER TABLE `processed_feedback`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`email`);

--
-- Indexes for table `user_feedback`
--
ALTER TABLE `user_feedback`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `netherlands_train_stations`
--
ALTER TABLE `netherlands_train_stations`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=79;

--
-- AUTO_INCREMENT for table `processed_feedback`
--
ALTER TABLE `processed_feedback`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT for table `user_feedback`
--
ALTER TABLE `user_feedback`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=32;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
