-- --------------------------------------------------------
-- 主机:                           127.0.0.1
-- 服务器版本:                        5.7.31-log - MySQL Community Server (GPL)
-- 服务器操作系统:                      Win64
-- HeidiSQL 版本:                  9.3.0.5038
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- 导出 moba_game 的数据库结构
CREATE DATABASE IF NOT EXISTS `moba_game` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `moba_game`;


-- 导出  表 moba_game.ugame 结构
CREATE TABLE IF NOT EXISTS `ugame` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '记录唯一的id号',
  `uid` int(11) NOT NULL COMMENT '用户唯一的id号',
  `uchip` int(11) NOT NULL DEFAULT '0' COMMENT '用户的金币数目',
  `uchip2` int(11) NOT NULL DEFAULT '0' COMMENT '用户的其他货币或等价物',
  `uchip3` int(11) NOT NULL DEFAULT '0' COMMENT '用户的其他货币或等价物',
  `uvip` int(11) NOT NULL DEFAULT '0' COMMENT '用户在本游戏中的等级',
  `uvip_endtime` int(11) NOT NULL DEFAULT '0' COMMENT 'vip结束时间',
  `udata1` int(11) NOT NULL DEFAULT '0' COMMENT '用户在游戏中的道具1',
  `udata2` int(11) NOT NULL DEFAULT '0' COMMENT '用户在游戏中的道具2',
  `udata3` int(11) NOT NULL DEFAULT '0' COMMENT '用户在游戏中的道具3',
  `uexp` int(11) NOT NULL DEFAULT '0' COMMENT '用户的经验值',
  `ustatus` int(11) NOT NULL DEFAULT '0' COMMENT '0正常,其他不正常',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='存放我们玩家在moba这个游戏中的玩家的游戏数据,主要的游戏数据;\r\n金币,其他货币,道具,游戏中的VIP等级,账号状态,玩家的经验;\r\nuid,来标识玩家的,id作为自增长的唯一id号';

-- 数据导出被取消选择。
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
