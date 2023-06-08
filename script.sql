USE [master]
GO
/****** Object:  Database [BirdTradingDB]    Script Date: 6/8/2023 6:28:55 PM ******/
CREATE DATABASE [BirdTradingDB]
GO
ALTER DATABASE [BirdTradingDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BirdTradingDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BirdTradingDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BirdTradingDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BirdTradingDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BirdTradingDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BirdTradingDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [BirdTradingDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BirdTradingDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BirdTradingDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BirdTradingDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BirdTradingDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BirdTradingDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BirdTradingDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BirdTradingDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BirdTradingDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BirdTradingDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BirdTradingDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BirdTradingDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BirdTradingDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BirdTradingDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BirdTradingDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BirdTradingDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [BirdTradingDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BirdTradingDB] SET RECOVERY FULL 
GO
ALTER DATABASE [BirdTradingDB] SET  MULTI_USER 
GO
ALTER DATABASE [BirdTradingDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BirdTradingDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BirdTradingDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BirdTradingDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BirdTradingDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BirdTradingDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'BirdTradingDB', N'ON'
GO
ALTER DATABASE [BirdTradingDB] SET QUERY_STORE = OFF
GO
USE [BirdTradingDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 6/8/2023 6:28:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 6/8/2023 6:28:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[TypeId] [int] NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoryType]    Script Date: 6/8/2023 6:28:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[ImageURL] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_CategoryType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 6/8/2023 6:28:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Comment] [nvarchar](max) NOT NULL,
	[Rating] [real] NULL,
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[ShopId] [int] NULL,
 CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 6/8/2023 6:28:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderDate] [datetime2](7) NOT NULL,
	[IsPaid] [bit] NOT NULL,
	[ShipperCode] [nvarchar](max) NOT NULL,
	[CompanyName] [nvarchar](max) NOT NULL,
	[ShipperPhone] [nvarchar](max) NOT NULL,
	[UserId] [int] NOT NULL,
	[AddressId] [int] NULL,
	[ShippingInformationId] [int] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 6/8/2023 6:28:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[OriginalPrice] [decimal](18, 2) NOT NULL,
	[DiscountPrice] [decimal](18, 2) NULL,
	[Quantity] [int] NOT NULL,
	[ImageUrl] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[IsRemoved] [bit] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[ShopId] [int] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShippingInformation]    Script Date: 6/8/2023 6:28:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShippingInformation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[City] [nvarchar](max) NOT NULL,
	[Country] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](max) NOT NULL,
	[IsDefaultAddress] [bit] NOT NULL,
 CONSTRAINT [PK_ShippingInformation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShippingInformationUser]    Script Date: 6/8/2023 6:28:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShippingInformationUser](
	[ShippingInformationsId] [int] NOT NULL,
	[UsersId] [int] NOT NULL,
 CONSTRAINT [PK_ShippingInformationUser] PRIMARY KEY CLUSTERED 
(
	[ShippingInformationsId] ASC,
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShippingSessions]    Script Date: 6/8/2023 6:28:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShippingSessions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Status] [int] NOT NULL,
	[SessionDate] [datetime2](7) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[OrderId] [int] NOT NULL,
 CONSTRAINT [PK_ShippingSessions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Shops]    Script Date: 6/8/2023 6:28:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Shops](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[IsBlocked] [bit] NOT NULL,
	[Rating] [real] NOT NULL,
	[AvatarUrl] [nvarchar](max) NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Shops] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 6/8/2023 6:28:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Role] [int] NOT NULL,
	[AvatarURL] [nvarchar](max) NOT NULL,
	[IsTempUser] [bit] NOT NULL,
	[IsBlocked] [bit] NOT NULL,
	[ShippingInforId] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20230602083541_init', N'7.0.5')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20230608062509_addCategoryType', N'7.0.5')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20230608073729_updateCategoryTypeTable', N'7.0.5')
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (1, N'Birds', 1)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (2, N'Finch & Canary Cages', 2)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (3, N'Conure Cages', 2)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (4, N'Lovebird Cages', 2)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (5, N'Parakeet Cages', 2)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (6, N'Amazon Cages', 2)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (7, N'Parrot Toys', 3)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (8, N'Parakeet Toys', 3)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (9, N'Finch & Canary Toys', 3)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (10, N'Lovebird Toys', 3)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (11, N'Parrot Perches', 4)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (12, N'Conure Perches', 4)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (13, N'Parakeet Perches', 4)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (14, N'Finch & Canary Perches', 4)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (15, N'Parakeet Food', 5)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (16, N'Cockatiel Food', 5)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (17, N'Wild Bird Food', 5)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (18, N'Canary & Finch Food', 5)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (19, N'Conure Food', 5)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (20, N'Parrot & Hookbill Food', 5)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (22, N'Parakeet Treats', 6)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (23, N'Lovebird Treats', 6)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (24, N'Parakeet Feeders', 7)
INSERT [dbo].[Categories] ([Id], [Name], [TypeId]) VALUES (25, N'Lovebird Feeders', 7)
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[CategoryType] ON 

INSERT [dbo].[CategoryType] ([Id], [Type], [ImageURL]) VALUES (1, N'Birds', N'img/pet-birds.png')
INSERT [dbo].[CategoryType] ([Id], [Type], [ImageURL]) VALUES (2, N'Cages', N'img/bird-cages.png')
INSERT [dbo].[CategoryType] ([Id], [Type], [ImageURL]) VALUES (3, N'Toys', N'img/bird-toys.png')
INSERT [dbo].[CategoryType] ([Id], [Type], [ImageURL]) VALUES (4, N'Perches and Playstands', N'img/bird-perches.png')
INSERT [dbo].[CategoryType] ([Id], [Type], [ImageURL]) VALUES (5, N'Foods', N'img/bird-foods.png')
INSERT [dbo].[CategoryType] ([Id], [Type], [ImageURL]) VALUES (6, N'Treats', N'img/bird-treats.png')
INSERT [dbo].[CategoryType] ([Id], [Type], [ImageURL]) VALUES (7, N'Feeders', N'img/bird-feeders.png')
SET IDENTITY_INSERT [dbo].[CategoryType] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([Id], [Name], [OriginalPrice], [DiscountPrice], [Quantity], [ImageUrl], [Description], [IsRemoved], [CategoryId], [ShopId]) VALUES (6, N'Cockatiel', CAST(4000000.00 AS Decimal(18, 2)), NULL, 3, N'img/birds/bird-cockatiel.png', N'Cockatiels are easy birds to care for, making them a desirable pet. Their endless affection facilitates their natural ability to bond with their pet parents. Playpens are a great way to allow them to interact with you outside of their habitat!', 0, 1, 5)
INSERT [dbo].[Products] ([Id], [Name], [OriginalPrice], [DiscountPrice], [Quantity], [ImageUrl], [Description], [IsRemoved], [CategoryId], [ShopId]) VALUES (8, N'Sun Conure', CAST(3500000.00 AS Decimal(18, 2)), NULL, 5, N'img/birds/bird-sun-conure.png', N'The Sun Conure is one of the most popular birds due to their beautiful coloration and fantastic disposition. These vocal and inquisitive birds are content to be with their pet parents for hours.', 0, 1, 5)
INSERT [dbo].[Products] ([Id], [Name], [OriginalPrice], [DiscountPrice], [Quantity], [ImageUrl], [Description], [IsRemoved], [CategoryId], [ShopId]) VALUES (9, N'Blue Parakeet', CAST(3700000.00 AS Decimal(18, 2)), NULL, 4, N'img/birds/bird-blue-parakeet.png', N'Parakeets are small, social, and intelligent birds. Eating a variety of seeds, plants, fruits, and vegetables, parakeets are known to live approximately 15 years in captivity.', 0, 1, 5)
INSERT [dbo].[Products] ([Id], [Name], [OriginalPrice], [DiscountPrice], [Quantity], [ImageUrl], [Description], [IsRemoved], [CategoryId], [ShopId]) VALUES (10, N'Quaker Parakeet', CAST(4200000.00 AS Decimal(18, 2)), NULL, 8, N'img/birds/bird-quaker-parakeet.png', N'Quaker (or Monk) Parakeets are beautiful and intelligent birds. They come in a variety of colors and are larger than the typical Parakeet. The term Parakeet is actually a descriptive term for the body type of these parrots.', 0, 1, 5)
INSERT [dbo].[Products] ([Id], [Name], [OriginalPrice], [DiscountPrice], [Quantity], [ImageUrl], [Description], [IsRemoved], [CategoryId], [ShopId]) VALUES (11, N'Fancy Parakeet', CAST(7300000.00 AS Decimal(18, 2)), NULL, 6, N'img/birds/bird-fancy-parakeet.png', N'Fancy Parakeets are small, social, and intelligent birds. Eating a variety of seeds, plants, fruits, and vegetables. Parakeets are known to live approximately 20 years in captivity.', 0, 1, 5)
INSERT [dbo].[Products] ([Id], [Name], [OriginalPrice], [DiscountPrice], [Quantity], [ImageUrl], [Description], [IsRemoved], [CategoryId], [ShopId]) VALUES (13, N'Green Parakeet', CAST(3700000.00 AS Decimal(18, 2)), NULL, 10, N'img/birds/bird-green-parakeet.png', N'Parakeets are small, social, and intelligent birds. eating a variety of seeds, plants, fruits, and vegetables. Parakeets are known to live approximately 20 years in captivity.', 0, 1, 5)
INSERT [dbo].[Products] ([Id], [Name], [OriginalPrice], [DiscountPrice], [Quantity], [ImageUrl], [Description], [IsRemoved], [CategoryId], [ShopId]) VALUES (15, N'Green Cheek Conure', CAST(4300000.00 AS Decimal(18, 2)), NULL, 8, N'img/birds/bird-green-cheek-conure.png', N'Green cheek conures are highly inquisitive, bold, and engaging birds. Like other conures, they can be playful and affectionate. Their voices are softer than many Conures.', 0, 1, 5)
INSERT [dbo].[Products] ([Id], [Name], [OriginalPrice], [DiscountPrice], [Quantity], [ImageUrl], [Description], [IsRemoved], [CategoryId], [ShopId]) VALUES (16, N'Yellow Canary', CAST(6900000.00 AS Decimal(18, 2)), NULL, 9, N'img/birds/bird-yellow-canary.png', N'Canaries are well known for their beauty and varied colors. Male canaries are loved for their sweet singing.', 0, 1, 5)
INSERT [dbo].[Products] ([Id], [Name], [OriginalPrice], [DiscountPrice], [Quantity], [ImageUrl], [Description], [IsRemoved], [CategoryId], [ShopId]) VALUES (17, N'Zebra Finch', CAST(6800000.00 AS Decimal(18, 2)), NULL, 11, N'img/birds/bird-zebra-finches.png', N'Finches are small, gentle birds that come in a dazzling variety of colors. This bird is brightly colored and very social making it a fan favorite. Their song consists of chirping and "beeping" rather than squawking.', 0, 1, 5)
INSERT [dbo].[Products] ([Id], [Name], [OriginalPrice], [DiscountPrice], [Quantity], [ImageUrl], [Description], [IsRemoved], [CategoryId], [ShopId]) VALUES (18, N'Society Finch', CAST(4300000.00 AS Decimal(18, 2)), NULL, 12, N'img/birds/bird-society-finch.png', N'These birds are a domesticated species as they do not exist in the wild. Society Finches will sing and chirp once acclimated to their surroundings. They can be housed in groups or mixed with other finches.', 0, 1, 5)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[ShippingInformation] ON 

INSERT [dbo].[ShippingInformation] ([Id], [Name], [Address], [City], [Country], [Phone], [IsDefaultAddress]) VALUES (1, N'Duy', N'Long Thach My', N'Ho Chi Minh', N'Viet Nam', N'0988123123', 1)
SET IDENTITY_INSERT [dbo].[ShippingInformation] OFF
GO
SET IDENTITY_INSERT [dbo].[Shops] ON 

INSERT [dbo].[Shops] ([Id], [Name], [Address], [Phone], [Email], [Description], [IsBlocked], [Rating], [AvatarUrl], [UserId]) VALUES (5, N'You & me', N'Long Thach My', N'0988123123', N'youandme@gmail.com', N'We are proud to offer a wide selection of high-quality products that are sure to meet your needs. From clothing and accessories to home decor and kitchenware, we have something for everyone. Our team is dedicated to providing exceptional customer service and ensuring that you have a pleasant shopping experience with us. Whether you''re looking for a gift for a loved one or a special treat for yourself, we are confident that you will find exactly what you need at our shop.', 0, 4.5, N'img/shops/shop-youandme.png', 4)
SET IDENTITY_INSERT [dbo].[Shops] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Name], [Email], [Phone], [Password], [Role], [AvatarURL], [IsTempUser], [IsBlocked], [ShippingInforId]) VALUES (4, N'Duy', N'duy@gmail.com', N'0988123123', N'123123', 2, N'test.png', 0, 0, 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [IX_Categories_TypeId]    Script Date: 6/8/2023 6:28:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_Categories_TypeId] ON [dbo].[Categories]
(
	[TypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderDetails_OrderId]    Script Date: 6/8/2023 6:28:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_OrderDetails_OrderId] ON [dbo].[OrderDetails]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderDetails_ProductId]    Script Date: 6/8/2023 6:28:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_OrderDetails_ProductId] ON [dbo].[OrderDetails]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderDetails_ShopId]    Script Date: 6/8/2023 6:28:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_OrderDetails_ShopId] ON [dbo].[OrderDetails]
(
	[ShopId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_ShippingInformationId]    Script Date: 6/8/2023 6:28:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_Orders_ShippingInformationId] ON [dbo].[Orders]
(
	[ShippingInformationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_UserId]    Script Date: 6/8/2023 6:28:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_Orders_UserId] ON [dbo].[Orders]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Products_CategoryId]    Script Date: 6/8/2023 6:28:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_Products_CategoryId] ON [dbo].[Products]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Products_ShopId]    Script Date: 6/8/2023 6:28:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_Products_ShopId] ON [dbo].[Products]
(
	[ShopId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ShippingInformationUser_UsersId]    Script Date: 6/8/2023 6:28:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_ShippingInformationUser_UsersId] ON [dbo].[ShippingInformationUser]
(
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ShippingSessions_OrderId]    Script Date: 6/8/2023 6:28:56 PM ******/
CREATE NONCLUSTERED INDEX [IX_ShippingSessions_OrderId] ON [dbo].[ShippingSessions]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Shops_UserId]    Script Date: 6/8/2023 6:28:56 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Shops_UserId] ON [dbo].[Shops]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT ((0)) FOR [TypeId]
GO
ALTER TABLE [dbo].[CategoryType] ADD  DEFAULT (N'') FOR [ImageURL]
GO
ALTER TABLE [dbo].[Categories]  WITH CHECK ADD  CONSTRAINT [FK_Categories_CategoryType_TypeId] FOREIGN KEY([TypeId])
REFERENCES [dbo].[CategoryType] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Categories] CHECK CONSTRAINT [FK_Categories_CategoryType_TypeId]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Orders_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Orders_OrderId]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Products_ProductId]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Shops_ShopId] FOREIGN KEY([ShopId])
REFERENCES [dbo].[Shops] ([Id])
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Shops_ShopId]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_ShippingInformation_ShippingInformationId] FOREIGN KEY([ShippingInformationId])
REFERENCES [dbo].[ShippingInformation] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_ShippingInformation_ShippingInformationId]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Users_UserId]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Categories_CategoryId]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Shops_ShopId] FOREIGN KEY([ShopId])
REFERENCES [dbo].[Shops] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Shops_ShopId]
GO
ALTER TABLE [dbo].[ShippingInformationUser]  WITH CHECK ADD  CONSTRAINT [FK_ShippingInformationUser_ShippingInformation_ShippingInformationsId] FOREIGN KEY([ShippingInformationsId])
REFERENCES [dbo].[ShippingInformation] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShippingInformationUser] CHECK CONSTRAINT [FK_ShippingInformationUser_ShippingInformation_ShippingInformationsId]
GO
ALTER TABLE [dbo].[ShippingInformationUser]  WITH CHECK ADD  CONSTRAINT [FK_ShippingInformationUser_Users_UsersId] FOREIGN KEY([UsersId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShippingInformationUser] CHECK CONSTRAINT [FK_ShippingInformationUser_Users_UsersId]
GO
ALTER TABLE [dbo].[ShippingSessions]  WITH CHECK ADD  CONSTRAINT [FK_ShippingSessions_Orders_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShippingSessions] CHECK CONSTRAINT [FK_ShippingSessions_Orders_OrderId]
GO
ALTER TABLE [dbo].[Shops]  WITH CHECK ADD  CONSTRAINT [FK_Shops_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Shops] CHECK CONSTRAINT [FK_Shops_Users_UserId]
GO
USE [master]
GO
ALTER DATABASE [BirdTradingDB] SET  READ_WRITE 
GO
