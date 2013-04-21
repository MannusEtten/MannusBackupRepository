/*
SQLyog Ultimate v8.82 
MySQL - 5.1.68 : Database - mannusnl
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`mannusnl` /*!40100 DEFAULT CHARACTER SET latin1 */;

USE `mannusnl`;

/*Table structure for table `backup_configuration` */

DROP TABLE IF EXISTS `backup_configuration`;

CREATE TABLE `backup_configuration` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `naam` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

/*Data for the table `backup_configuration` */

insert  into `backup_configuration`(`id`,`naam`) values (1,'SqlYog');

/*Table structure for table `backup_profile` */

DROP TABLE IF EXISTS `backup_profile`;

CREATE TABLE `backup_profile` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` enum('Client') DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

/*Data for the table `backup_profile` */

insert  into `backup_profile`(`id`,`type`) values (2,'Client');

/*Table structure for table `backup_profile_configuration` */

DROP TABLE IF EXISTS `backup_profile_configuration`;

CREATE TABLE `backup_profile_configuration` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `configurationid` int(11) NOT NULL,
  `profileid` int(11) NOT NULL,
  `naam` varchar(50) NOT NULL,
  `waarde` varchar(250) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_configurationproperty_to_profile` (`profileid`),
  KEY `FK_configurationproperty_to_configuration` (`configurationid`),
  CONSTRAINT `FK_configurationproperty_to_configuration` FOREIGN KEY (`configurationid`) REFERENCES `backup_configuration` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_configurationproperty_to_profile` FOREIGN KEY (`profileid`) REFERENCES `backup_profile` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

/*Data for the table `backup_profile_configuration` */

/*Table structure for table `backup_profileproperty` */

DROP TABLE IF EXISTS `backup_profileproperty`;

CREATE TABLE `backup_profileproperty` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `naam` tinytext NOT NULL,
  `waarde` tinytext NOT NULL,
  `profileid` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_backup_profileproperty` (`profileid`),
  CONSTRAINT `FK_backup_profileproperty` FOREIGN KEY (`profileid`) REFERENCES `backup_profile` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

/*Data for the table `backup_profileproperty` */

insert  into `backup_profileproperty`(`id`,`naam`,`waarde`,`profileid`) values (2,'NumberOfLocalBackups','5',2),(3,'SqlYog','C:\\Program Files (x86)\\SQLyog\\SJA.exe',2);

/*Table structure for table `backup_results` */

DROP TABLE IF EXISTS `backup_results`;

CREATE TABLE `backup_results` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `datum` date NOT NULL,
  `host` varchar(150) NOT NULL,
  `tijd` time NOT NULL,
  `status` varchar(150) NOT NULL,
  `naam` varchar(250) NOT NULL,
  `password` varchar(50) NOT NULL,
  `size` varchar(50) NOT NULL,
  `sizeingb` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=9 DEFAULT CHARSET=latin1;

/*Data for the table `backup_results` */

insert  into `backup_results`(`id`,`datum`,`host`,`tijd`,`status`,`naam`,`password`,`size`,`sizeingb`) values (1,'2013-03-19','Laptop','20:47:06','Afgerond','Mannus','Etten','2225','2'),(2,'2013-03-19','Laptop','20:48:11','Afgerond','Mannus','Etten','2225','2'),(3,'2013-03-19','Laptop','20:49:10','Afgerond','Mannus','Etten','2225','2.2'),(4,'2013-03-19','Laptop','20:54:41','Afgerond','Mannus','Etten','2225','2.2'),(5,'2013-03-19','Laptop','21:56:54','Afgerond','Mannus','Etten','2225','2.2'),(6,'2013-03-19','laptop','22:00:00','fout','c:\\backup\\backup_laptop_19March2013','b','c','d'),(7,'2013-03-19','Laptop','22:31:40','Afgerond','Mannus','Etten','2225','2.2'),(8,'2013-03-19','Laptop','22:33:34','Afgerond','Mannus','Etten','2225','2.2');

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
