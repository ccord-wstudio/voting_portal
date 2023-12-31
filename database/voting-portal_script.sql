USE [master]
GO
/****** Object:  Database [voting_portal]    Script Date: 04/12/2023 01:49:39 ******/
CREATE DATABASE [voting_portal]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'voting_portal', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\voting_portal.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'voting_portal_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\voting_portal_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [voting_portal] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [voting_portal].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [voting_portal] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [voting_portal] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [voting_portal] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [voting_portal] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [voting_portal] SET ARITHABORT OFF 
GO
ALTER DATABASE [voting_portal] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [voting_portal] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [voting_portal] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [voting_portal] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [voting_portal] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [voting_portal] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [voting_portal] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [voting_portal] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [voting_portal] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [voting_portal] SET  DISABLE_BROKER 
GO
ALTER DATABASE [voting_portal] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [voting_portal] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [voting_portal] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [voting_portal] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [voting_portal] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [voting_portal] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [voting_portal] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [voting_portal] SET RECOVERY FULL 
GO
ALTER DATABASE [voting_portal] SET  MULTI_USER 
GO
ALTER DATABASE [voting_portal] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [voting_portal] SET DB_CHAINING OFF 
GO
ALTER DATABASE [voting_portal] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [voting_portal] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [voting_portal] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [voting_portal] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'voting_portal', N'ON'
GO
ALTER DATABASE [voting_portal] SET QUERY_STORE = ON
GO
ALTER DATABASE [voting_portal] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [voting_portal]
GO
/****** Object:  User [test]    Script Date: 04/12/2023 01:49:39 ******/
CREATE USER [test] FOR LOGIN [test] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 04/12/2023 01:49:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Email] [varchar](255) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[Password] [varchar](100) NULL,
	[DOB] [date] NULL,
	[AddressLongitude] [decimal](9, 6) NULL,
	[AddressLatitude] [decimal](8, 6) NULL,
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Country] [varchar](255) NULL,
 CONSTRAINT [PK_UserTable] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Votes]    Script Date: 04/12/2023 01:49:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Votes](
	[VoteID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[QuestionID] [int] NULL,
	[Response] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[VoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Votes]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
USE [master]
GO
ALTER DATABASE [voting_portal] SET  READ_WRITE 
GO
