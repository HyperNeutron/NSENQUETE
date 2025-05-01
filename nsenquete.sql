-- phpMyAdmin SQL Dump
-- version 5.2.2
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: May 01, 2025 at 06:08 PM
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
-- Table structure for table `processed_feedback`
--

CREATE TABLE `processed_feedback` (
  `id` int NOT NULL,
  `name` varchar(50) NOT NULL,
  `small_story` varchar(100) NOT NULL,
  `feedback` varchar(500) NOT NULL,
  `station` varchar(50) NOT NULL,
  `is_approved` tinyint(1) NOT NULL,
  `approved_on` datetime NOT NULL
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
-- Dumping data for table `user_feedback`
--

INSERT INTO `user_feedback` (`id`, `name`, `small_story`, `feedback`, `station`, `feedback_timestamp`) VALUES
(1, 'Testnaam', 'Test klein verhaal', 'Test feedback', 'Utrecht', '2025-05-01 18:07:38');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `processed_feedback`
--
ALTER TABLE `processed_feedback`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `user_feedback`
--
ALTER TABLE `user_feedback`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `processed_feedback`
--
ALTER TABLE `processed_feedback`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=25;

--
-- AUTO_INCREMENT for table `user_feedback`
--
ALTER TABLE `user_feedback`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=28;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
