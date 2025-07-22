-- MySQL dump 10.13  Distrib 8.0.42, for Win64 (x86_64)
--
-- Host: localhost    Database: purplehollow
-- ------------------------------------------------------
-- Server version	8.0.42

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `budget`
--

DROP TABLE IF EXISTS `budget`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `budget` (
  `budgetID` int NOT NULL AUTO_INCREMENT,
  `userID` int DEFAULT NULL,
  `totalBudget` decimal(10,2) DEFAULT NULL,
  `isPaid` tinyint(1) DEFAULT '0',
  `notes` text,
  PRIMARY KEY (`budgetID`),
  UNIQUE KEY `userID` (`userID`),
  CONSTRAINT `budget_ibfk_1` FOREIGN KEY (`userID`) REFERENCES `user` (`userID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `budget`
--

LOCK TABLES `budget` WRITE;
/*!40000 ALTER TABLE `budget` DISABLE KEYS */;
INSERT INTO `budget` VALUES (1,1,20000.00,1,'All vendors paid'),(2,2,18000.00,0,'Final payment due for cake'),(3,3,25000.00,1,'Complete'),(4,4,19000.00,0,'Photographer not yet paid'),(5,5,21000.00,1,'Ready for wedding day');
/*!40000 ALTER TABLE `budget` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `guest`
--

DROP TABLE IF EXISTS `guest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `guest` (
  `guestID` int NOT NULL AUTO_INCREMENT,
  `userID` int DEFAULT NULL,
  `guestFName` varchar(50) DEFAULT NULL,
  `guestLName` varchar(50) DEFAULT NULL,
  `guestDSelection` varchar(50) DEFAULT NULL,
  `guestRSelection` varchar(50) DEFAULT NULL,
  `guestEmail` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`guestID`),
  KEY `userID` (`userID`),
  CONSTRAINT `guest_ibfk_1` FOREIGN KEY (`userID`) REFERENCES `user` (`userID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `guest`
--

LOCK TABLES `guest` WRITE;
/*!40000 ALTER TABLE `guest` DISABLE KEYS */;
INSERT INTO `guest` VALUES (1,1,'John','Doe','Vegan','Reception Only','john.doe@example.com'),(2,1,'Jane','Smith','Vegetarian','All Events','jane.smith@example.com'),(3,2,'Bruce','Wayne','Standard','Ceremony Only','bruce.wayne@example.com'),(4,3,'Clark','Kent','Gluten-Free','All Events','clark.kent@example.com'),(5,4,'Diana','Prince','Standard','Reception Only','diana.prince@example.com');
/*!40000 ALTER TABLE `guest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `itinerary`
--

DROP TABLE IF EXISTS `itinerary`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `itinerary` (
  `itineraryID` int NOT NULL AUTO_INCREMENT,
  `userID` int DEFAULT NULL,
  `itineraryName` varchar(100) DEFAULT NULL,
  `itineraryStartTime` int DEFAULT NULL,
  `itineraryEndTime` int DEFAULT NULL,
  `itineraryDescription` text,
  PRIMARY KEY (`itineraryID`),
  KEY `userID` (`userID`),
  CONSTRAINT `itinerary_ibfk_1` FOREIGN KEY (`userID`) REFERENCES `user` (`userID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `itinerary`
--

LOCK TABLES `itinerary` WRITE;
/*!40000 ALTER TABLE `itinerary` DISABLE KEYS */;
INSERT INTO `itinerary` VALUES (1,1,'Ceremony',1400,1500,'Wedding ceremony at the chapel'),(2,2,'Reception',1600,2200,'Dinner and dancing'),(3,3,'Photo Session',1500,1545,'Photos at the garden'),(4,4,'Cake Cutting',1800,1815,'Cake cutting ceremony'),(5,5,'Send-off',2200,2230,'Bride and groom exit');
/*!40000 ALTER TABLE `itinerary` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `menu`
--

DROP TABLE IF EXISTS `menu`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `menu` (
  `menuID` int NOT NULL AUTO_INCREMENT,
  `userID` int DEFAULT NULL,
  `menuDishName` varchar(100) DEFAULT NULL,
  `menuCategory` varchar(50) DEFAULT NULL,
  `menuDescription` text,
  PRIMARY KEY (`menuID`),
  KEY `userID` (`userID`),
  CONSTRAINT `menu_ibfk_1` FOREIGN KEY (`userID`) REFERENCES `user` (`userID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menu`
--

LOCK TABLES `menu` WRITE;
/*!40000 ALTER TABLE `menu` DISABLE KEYS */;
INSERT INTO `menu` VALUES (1,1,'Roast Beef','Main Course','Slow roasted beef with gravy'),(2,2,'Caesar Salad','Starter','Crisp romaine with Caesar dressing'),(3,3,'Chocolate Cake','Dessert','Rich dark chocolate cake'),(4,4,'Grilled Chicken','Main Course','Herb grilled chicken breast'),(5,5,'Fruit Tart','Dessert','Seasonal fruit tart with custard');
/*!40000 ALTER TABLE `menu` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `task`
--

DROP TABLE IF EXISTS `task`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `task` (
  `taskID` int NOT NULL AUTO_INCREMENT,
  `userID` int DEFAULT NULL,
  `taskDescription` text,
  PRIMARY KEY (`taskID`),
  KEY `userID` (`userID`),
  CONSTRAINT `task_ibfk_1` FOREIGN KEY (`userID`) REFERENCES `user` (`userID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `task`
--

LOCK TABLES `task` WRITE;
/*!40000 ALTER TABLE `task` DISABLE KEYS */;
INSERT INTO `task` VALUES (1,1,'Book venue'),(2,2,'Confirm guest list'),(3,3,'Hire photographer'),(4,4,'Choose music playlist'),(5,5,'Finalize seating plan');
/*!40000 ALTER TABLE `task` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `userID` int NOT NULL AUTO_INCREMENT,
  `username` varchar(50) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `password` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`userID`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (1,'jdoe','jdoe@example.com','pass123'),(2,'asmith','asmith@example.com','pass456'),(3,'bwayne','bwayne@example.com','darkknight'),(4,'ckent','ckent@example.com','superman'),(5,'dprince','dprince@example.com','wonder'),(6,'JoToro','JoToro@example.com','gojo321'),(7,'Tanjiro','tanjiro@example.com','tanj123'),(8,'arg','arg@example.com','arg123'),(9,'po','po@example.com','po123'),(10,'clark','clark@example.com','clark123'),(11,'Fred','Fred@example.com','fred123');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `vendor`
--

DROP TABLE IF EXISTS `vendor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `vendor` (
  `vendorID` int NOT NULL AUTO_INCREMENT,
  `userID` int DEFAULT NULL,
  `vendorName` varchar(100) DEFAULT NULL,
  `vendorCellNum` varchar(20) DEFAULT NULL,
  `vendorProvince` varchar(50) DEFAULT NULL,
  `vendorCity` varchar(50) DEFAULT NULL,
  `vendorPrice` decimal(10,2) DEFAULT NULL,
  `category` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`vendorID`),
  KEY `userID` (`userID`),
  CONSTRAINT `vendor_ibfk_1` FOREIGN KEY (`userID`) REFERENCES `user` (`userID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vendor`
--

LOCK TABLES `vendor` WRITE;
/*!40000 ALTER TABLE `vendor` DISABLE KEYS */;
INSERT INTO `vendor` VALUES (1,1,'Floral Bliss','123-456-7890','Gauteng','Johannesburg',5000.00,'Flowers'),(2,2,'Sweet Cakes','234-567-8901','Western Cape','Cape Town',3500.00,'Cake'),(3,3,'Event Decor','345-678-9012','KwaZulu-Natal','Durban',8000.00,'Decor'),(4,4,'SnapShots','456-789-0123','Gauteng','Pretoria',6000.00,'Photography'),(5,5,'DJ Max','567-890-1234','Gauteng','Midrand',4000.00,'Music');
/*!40000 ALTER TABLE `vendor` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-07-22 16:50:12
