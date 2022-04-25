-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 24, 2022 at 01:58 PM
-- Server version: 10.4.22-MariaDB
-- PHP Version: 8.1.2

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `lms`
--
CREATE DATABASE IF NOT EXISTS `lms` DEFAULT CHARACTER SET utf8 COLLATE utf8_hungarian_ci;
USE `lms`;

-- --------------------------------------------------------

--
-- Table structure for table `authors`
--

CREATE TABLE `authors` (
  `id` int(11) NOT NULL,
  `name` varchar(255) COLLATE utf8_hungarian_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `authors`
--

INSERT INTO `authors` (`id`, `name`) VALUES
(1, 'Franz Kafka'),
(2, 'Genki Kawamura'),
(3, 'Fjodor Mihajlovics Dosztojevszkij'),
(4, 'Hegyesi László'),
(5, 'Kovács Csongor'),
(6, 'Karinthy Frigyes'),
(7, 'Hiro Arikawa'),
(8, 'Feldmár András'),
(9, 'Nikosz Kazantzakisz'),
(10, 'Robert C. Martin'),
(12, 'Natsume Soseki'),
(14, 'Kazuo Ishiguro'),
(15, 'Janne Teller'),
(16, 'Hegyesi László, Kovács Csongor'),
(17, 'Próba Insert');

-- --------------------------------------------------------

--
-- Table structure for table `books`
--

CREATE TABLE `books` (
  `id` int(11) NOT NULL,
  `isbn` varchar(25) COLLATE utf8_hungarian_ci NOT NULL,
  `title` varchar(250) COLLATE utf8_hungarian_ci NOT NULL,
  `category` varchar(100) COLLATE utf8_hungarian_ci NOT NULL,
  `pages` int(11) NOT NULL,
  `publishYear` int(4) NOT NULL,
  `publisher` varchar(100) COLLATE utf8_hungarian_ci NOT NULL,
  `link` varchar(255) COLLATE utf8_hungarian_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `books`
--

INSERT INTO `books` (`id`, `isbn`, `title`, `category`, `pages`, `publishYear`, `publisher`, `link`) VALUES
(1, '9630703769', 'A per', 'Szépirodalom', 286, 1975, 'Európa', 'book_1.jpg'),
(2, '9786155915994', 'Ha a macskák eltűnnének a világból', 'Szépirodalom', 144, 2019, 'XXI. Század', 'book_2.jpg'),
(3, '9789632277288', 'Feljegyzések az egérlyukból', 'Szépirodalom', 166, 2016, 'Helikon', 'book_3.jpg'),
(4, '9789636432164', 'Digitális Elektronika', 'Műszaki', 214, 2011, 'General Press Kiadó', 'book_4.jpg'),
(5, '9789630968379', 'Így írtok ti', 'Szépirodalom', 240, 2014, 'Kossuth', 'book_5.jpg'),
(6, '9786155905605', 'Az utazó macska krónikája', 'Szépirodalom', 256, 2019, 'Művelt Nép Könyvkiad', 'book_6.jpg'),
(7, '9789633041819', 'Életunalom, élettér, életkedv', 'Pszichológia', 136, 2014, 'HVG Könyvek', 'book_7.jpg'),
(8, '9630706342', 'Zorbász, a görög', 'Szépirodalom', 462, 1976, 'Európa', 'book_8.jpg'),
(9, '9639020273', 'Amerika', 'Szépirodalom', 198, 1997, 'Szukits', 'book_9.jpg'),
(10, '9789639637696', 'Tiszta kód', 'Számítástechnikai', 466, 2010, 'Kiskapu', 'book_10.jpg'),
(11, '9789639971127', 'A tudatállapotok szivárványa', 'Pszichológia', 273, 2010, 'Jaffa Kiadó', 'book_11.jpg'),
(12, '9789633049594', 'A tudatállapotok szivárványa', 'Pszichológia', 314, 2020, 'HVG Könyvek', 'book_12.jpg'),
(14, '9781512382396', 'Kokoro', 'Szépirodalom', 238, 2021, 'Mint Editions', 'book_14.jpg'),
(16, '9789634799092', 'Klara és a Nap', 'Szépirodalom', 376, 2022, 'Helikon', 'book_16.jpg'),
(18, '9789632442952', 'Semmi', 'Szépirodalom', 181, 2021, 'Scolar', 'book_18.jpg');

-- --------------------------------------------------------

--
-- Table structure for table `books_authors`
--

CREATE TABLE `books_authors` (
  `bookId` int(11) NOT NULL,
  `authorId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `books_authors`
--

INSERT INTO `books_authors` (`bookId`, `authorId`) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 16),
(5, 6),
(6, 7),
(7, 8),
(8, 9),
(9, 1),
(10, 10),
(11, 8),
(12, 8),
(14, 12),
(16, 14),
(18, 15);

-- --------------------------------------------------------

--
-- Table structure for table `book_stock`
--

CREATE TABLE `book_stock` (
  `id` int(11) NOT NULL,
  `bookId` int(11) NOT NULL,
  `libraryCode` varchar(64) COLLATE utf8_hungarian_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `book_stock`
--

INSERT INTO `book_stock` (`id`, `bookId`, `libraryCode`) VALUES
(3, 2, 'lms-23'),
(4, 12, 'lms-124'),
(6, 1, 'lms-16'),
(7, 2, 'lms-27'),
(8, 1, 'lms-18'),
(9, 1, 'lms-19'),
(10, 2, 'lms-210'),
(11, 3, 'lms-311'),
(12, 3, 'lms-312'),
(13, 4, 'lms-413'),
(14, 5, 'lms-514'),
(15, 5, 'lms-515'),
(16, 2, 'lms-216'),
(17, 6, 'lms-617'),
(18, 7, 'lms-718'),
(19, 8, 'lms-819'),
(20, 9, 'lms-920'),
(21, 10, 'lms-1021'),
(22, 11, 'lms-1122'),
(23, 12, 'lms-1223'),
(24, 14, 'lms-1424'),
(25, 1, 'lms-125'),
(26, 16, 'lms-1626'),
(27, 1, 'lms-127'),
(31, 18, 'lms-1831'),
(32, 1, 'lms-132');

-- --------------------------------------------------------

--
-- Table structure for table `check_out`
--

CREATE TABLE `check_out` (
  `id` int(11) NOT NULL,
  `checkOutCode` varchar(10) COLLATE utf8_hungarian_ci NOT NULL,
  `memberId` int(11) NOT NULL,
  `bookId` int(11) NOT NULL,
  `checkOutDate` date NOT NULL,
  `returnDate` date DEFAULT NULL,
  `checkInDate` date NOT NULL,
  `isReturned` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `check_out`
--

INSERT INTO `check_out` (`id`, `checkOutCode`, `memberId`, `bookId`, `checkOutDate`, `returnDate`, `checkInDate`, `isReturned`) VALUES
(5, '', 1, 4, '2022-02-11', '2000-01-01', '2022-02-26', 0),
(6, '7WE7GX-006', 1, 20, '2022-04-03', '2000-01-01', '2022-04-12', 0),
(7, '9HEC2Q-007', 1, 13, '2022-04-24', '2000-01-01', '2022-04-26', 0);

-- --------------------------------------------------------

--
-- Table structure for table `members`
--

CREATE TABLE `members` (
  `id` int(11) NOT NULL,
  `email` varchar(40) COLLATE utf8_hungarian_ci NOT NULL,
  `readersCode` varchar(128) COLLATE utf8_hungarian_ci NOT NULL,
  `firstName` varchar(20) COLLATE utf8_hungarian_ci NOT NULL,
  `lastName` varchar(30) COLLATE utf8_hungarian_ci NOT NULL,
  `class` varchar(10) COLLATE utf8_hungarian_ci NOT NULL,
  `role` int(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `members`
--

INSERT INTO `members` (`id`, `email`, `readersCode`, `firstName`, `lastName`, `class`, `role`) VALUES
(1, 'horvathl1@kkszki.hu', '438129-001', 'Horváth', 'Leticia', '2/14.c', 0),
(2, 'novakg@kkszki.hu', '732193-002', 'Novák', 'Gábor', '2/14.c', 0);

-- --------------------------------------------------------

--
-- Table structure for table `reservation`
--

CREATE TABLE `reservation` (
  `id` int(11) NOT NULL,
  `memberId` int(11) NOT NULL,
  `bookId` int(11) NOT NULL,
  `reservationDate` date NOT NULL,
  `checkOutId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- --------------------------------------------------------

--
-- Table structure for table `user`
--

CREATE TABLE `user` (
  `id` int(11) NOT NULL,
  `username` varchar(40) COLLATE utf8_hungarian_ci NOT NULL,
  `password` varchar(64) COLLATE utf8_hungarian_ci NOT NULL,
  `salt` varchar(255) COLLATE utf8_hungarian_ci NOT NULL,
  `email` varchar(40) COLLATE utf8_hungarian_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `user`
--

INSERT INTO `user` (`id`, `username`, `password`, `salt`, `email`) VALUES
(1, 'a', 'bb0fe1da893203ad2984ed0a2724f2c7', 'zLCdfzS/KxW2IP3CHQ+PJw==', 'admin@lms.hu');

-- --------------------------------------------------------

--
-- Table structure for table `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) COLLATE utf8_hungarian_ci NOT NULL,
  `ProductVersion` varchar(32) COLLATE utf8_hungarian_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `authors`
--
ALTER TABLE `authors`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `books`
--
ALTER TABLE `books`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `books_authors`
--
ALTER TABLE `books_authors`
  ADD KEY `bookId` (`bookId`,`authorId`),
  ADD KEY `authorId` (`authorId`);

--
-- Indexes for table `book_stock`
--
ALTER TABLE `book_stock`
  ADD PRIMARY KEY (`id`),
  ADD KEY `bookId` (`bookId`);

--
-- Indexes for table `check_out`
--
ALTER TABLE `check_out`
  ADD PRIMARY KEY (`id`),
  ADD KEY `memberId` (`memberId`,`bookId`),
  ADD KEY `bookId` (`bookId`);

--
-- Indexes for table `members`
--
ALTER TABLE `members`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `reservation`
--
ALTER TABLE `reservation`
  ADD PRIMARY KEY (`id`),
  ADD KEY `checkOutId` (`checkOutId`),
  ADD KEY `bookId` (`bookId`) USING BTREE,
  ADD KEY `memberId` (`memberId`);

--
-- Indexes for table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `authors`
--
ALTER TABLE `authors`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

--
-- AUTO_INCREMENT for table `books`
--
ALTER TABLE `books`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT for table `book_stock`
--
ALTER TABLE `book_stock`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=33;

--
-- AUTO_INCREMENT for table `check_out`
--
ALTER TABLE `check_out`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `members`
--
ALTER TABLE `members`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `reservation`
--
ALTER TABLE `reservation`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `user`
--
ALTER TABLE `user`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `books_authors`
--
ALTER TABLE `books_authors`
  ADD CONSTRAINT `books_authors_ibfk_1` FOREIGN KEY (`bookId`) REFERENCES `books` (`id`),
  ADD CONSTRAINT `books_authors_ibfk_2` FOREIGN KEY (`authorId`) REFERENCES `authors` (`id`);

--
-- Constraints for table `book_stock`
--
ALTER TABLE `book_stock`
  ADD CONSTRAINT `book_stock_ibfk_1` FOREIGN KEY (`bookId`) REFERENCES `books` (`id`);

--
-- Constraints for table `check_out`
--
ALTER TABLE `check_out`
  ADD CONSTRAINT `check_out_ibfk_1` FOREIGN KEY (`memberId`) REFERENCES `members` (`id`),
  ADD CONSTRAINT `check_out_ibfk_2` FOREIGN KEY (`bookId`) REFERENCES `book_stock` (`id`);

--
-- Constraints for table `reservation`
--
ALTER TABLE `reservation`
  ADD CONSTRAINT `reservation_ibfk_1` FOREIGN KEY (`memberId`) REFERENCES `members` (`id`),
  ADD CONSTRAINT `reservation_ibfk_2` FOREIGN KEY (`checkOutId`) REFERENCES `check_out` (`id`),
  ADD CONSTRAINT `reservation_ibfk_3` FOREIGN KEY (`bookId`) REFERENCES `book_stock` (`bookId`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
