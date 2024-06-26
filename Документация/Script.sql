/*    ==Параметры сценариев==

    Версия исходного сервера : SQL Server 2014 (12.0.2000)
    Выпуск исходного ядра СУБД : Выпуск Microsoft SQL Server Express Edition
    Тип исходного ядра СУБД : Изолированный SQL Server

    Версия целевого сервера : SQL Server 2017
    Выпуск целевого ядра СУБД : Выпуск Microsoft SQL Server Standard Edition
    Тип целевого ядра СУБД : Изолированный SQL Server
*/
USE [master]
GO
/****** Object:  Database [PharmacyDB]    Script Date: 09.06.2022 10:01:42 ******/
CREATE DATABASE [HotelDB]
GO
USE [HotelDB]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 09.06.2022 10:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pharmacy]    Script Date: 09.06.2022 10:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pharmacy](
	[PharmacyId] [int] IDENTITY(1,1) NOT NULL,
	[PharmacyName] [nvarchar](100) NOT NULL,
	[Info] [nvarchar](1000) NULL,
	[Address] [nvarchar](1000) NULL,
	[Phone] [nvarchar](50) NULL,
	[Site] [nvarchar](50) NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
	[Rate] [float] NULL,
	[Photo] [nvarchar](50) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[WorkTimeId] [int] NOT NULL,
 CONSTRAINT [PK_Pharmacy] PRIMARY KEY CLUSTERED 
(
	[PharmacyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Service]    Script Date: 09.06.2022 10:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service](
	[ServiceId] [int] IDENTITY(1,1) NOT NULL,
	[ServiceName] [nvarchar](1000) NULL,
 CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED 
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServicePharmacy]    Script Date: 09.06.2022 10:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServicePharmacy](
	[ServicePharmacyId] [int] IDENTITY(1,1) NOT NULL,
	[ServiceId] [int] NOT NULL,
	[PharmacyId] [int] NOT NULL,
 CONSTRAINT [PK_ServicePharmacy] PRIMARY KEY CLUSTERED 
(
	[ServicePharmacyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 09.06.2022 10:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkTime]    Script Date: 09.06.2022 10:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkTime](
	[WorkTimeId] [int] IDENTITY(1,1) NOT NULL,
	[WorkTime] [nvarchar](1000) NULL,
 CONSTRAINT [PK_WorkTime] PRIMARY KEY CLUSTERED 
(
	[WorkTimeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (1, N'Детское хирургическое отделение')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (2, N'Отделение новорожденных')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (3, N'Кардиологическое отделение')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (4, N'Патологоанатомическое отделение')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (5, N'Больница')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (6, N'Гинекологическое отделение')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (7, N'Онкологическое отделение')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (8, N'Отделение реабилитации и восстановительного лечения')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (9, N'Хирургическое отделение')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (10, N'Неврологическое отделение')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (11, N'Онкологическое отделение')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (12, N'Урологическое отделение')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (13, N'Медицинский центр')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (14, N'Многопрофильная клиника')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (15, N'Травматологическое отделение')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (16, N'Стоматологическое отделение')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (17, N'Стоматологическая поликлиника')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (18, N'Медицинская компания')
SET IDENTITY_INSERT [dbo].[Category] OFF
SET IDENTITY_INSERT [dbo].[Pharmacy] ON 

INSERT [dbo].[Pharmacy] ([PharmacyId], [PharmacyName], [Info], [Address], [Phone], [Site], [Latitude], [Longitude], [Rate], [Photo], [CategoryId], [WorkTimeId]) VALUES (1, N'Доктор Дент', N'Ежедневно с 08:00 до 18:00', N'г. Зеленодольск, ​Комсомольская, 9', N'+7 (84371) 5‒63‒39', N'-', 55.850829939083866, 48.503563932338515, 4, N'333.jpg', 17, 1)
INSERT [dbo].[Pharmacy] ([PharmacyId], [PharmacyName], [Info], [Address], [Phone], [Site], [Latitude], [Longitude], [Rate], [Photo], [CategoryId], [WorkTimeId]) VALUES (3, N'Айболит', N'семейный медицинский центр', N'г. Зеленодольск,  ​Гоголя, 49а', N'+7 (843) 255‒41‒41, +7 (84371) 4‒05‒75', N'www.mc-aybolit.ru', 55.85158617833838, 48.517214089764018, 5, N'24444.jpg', 13, 3)
INSERT [dbo].[Pharmacy] ([PharmacyId], [PharmacyName], [Info], [Address], [Phone], [Site], [Latitude], [Longitude], [Rate], [Photo], [CategoryId], [WorkTimeId]) VALUES (4, N'INVITRO', N'Скидка 10% по промокоду «2ГИС»', N'г.Зеленодольск, ул. Ленина, 31', N'8‒800‒200‒363‒0', N'invitro.ru', 55.846137099548095, 48.504968051637704, 4, N'1555.jpg', 18, 2)
INSERT [dbo].[Pharmacy] ([PharmacyId], [PharmacyName], [Info], [Address], [Phone], [Site], [Latitude], [Longitude], [Rate], [Photo], [CategoryId], [WorkTimeId]) VALUES (7, N'Зеленодольская центральная районная больница', N'Ежедневно с 07:00 до 20:00', N'г. Зеленодольск, ​Карла Маркса, 37а', N'+7 (84371) 5‒67‒17
+7 (84371) 5‒68‒05', N'st182.polzdrv.ru', 55.850875209386807, 48.4957460671706, 3, N'22.jpg', 17, 2)
INSERT [dbo].[Pharmacy] ([PharmacyId], [PharmacyName], [Info], [Address], [Phone], [Site], [Latitude], [Longitude], [Rate], [Photo], [CategoryId], [WorkTimeId]) VALUES (8, N'Мир здоровья', N'ПН-ПТ c 7:00 до 18:00, СБ-ВС c 8:00 до 13:00', N'​г.Зеленодольск. ул. Чкалова, 2', N'+7 (84371) 4-65-35', N'farmlend.ru', 55.848950902961292, 48.518380890726306, 4, N'116666.jpg', 13, 2)
SET IDENTITY_INSERT [dbo].[Pharmacy] OFF
SET IDENTITY_INSERT [dbo].[Service] ON 

INSERT [dbo].[Service] ([ServiceId], [ServiceName]) VALUES (1, N'С сайтом')
INSERT [dbo].[Service] ([ServiceId], [ServiceName]) VALUES (2, N'Расчет по картам')
INSERT [dbo].[Service] ([ServiceId], [ServiceName]) VALUES (3, N'С фото')
INSERT [dbo].[Service] ([ServiceId], [ServiceName]) VALUES (4, N'Круглосуточный')
INSERT [dbo].[Service] ([ServiceId], [ServiceName]) VALUES (5, N'Доступно для инвалидов')
INSERT [dbo].[Service] ([ServiceId], [ServiceName]) VALUES (6, N'Выезд на дом')
INSERT [dbo].[Service] ([ServiceId], [ServiceName]) VALUES (7, N'Услуги специалистов')
INSERT [dbo].[Service] ([ServiceId], [ServiceName]) VALUES (8, N'Услуги лаборатории')
INSERT [dbo].[Service] ([ServiceId], [ServiceName]) VALUES (9, N'Медицинские манипуляции')
INSERT [dbo].[Service] ([ServiceId], [ServiceName]) VALUES (10, N'УЗИ')
INSERT [dbo].[Service] ([ServiceId], [ServiceName]) VALUES (11, N'Профосмотр')
INSERT [dbo].[Service] ([ServiceId], [ServiceName]) VALUES (12, N'Ведение беременности')
SET IDENTITY_INSERT [dbo].[Service] OFF
SET IDENTITY_INSERT [dbo].[ServicePharmacy] ON 

INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (21, 2, 7)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (22, 3, 7)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (23, 5, 7)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (24, 1, 1)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (25, 2, 1)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (26, 3, 1)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (27, 1, 3)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (28, 2, 3)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (29, 3, 3)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (30, 5, 3)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (40, 1, 4)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (41, 2, 4)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (42, 3, 4)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (43, 5, 4)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (44, 6, 4)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (45, 7, 4)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (46, 8, 4)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (47, 9, 4)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (48, 10, 4)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (59, 1, 8)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (60, 2, 8)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (61, 3, 8)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (62, 5, 8)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (63, 6, 8)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (64, 7, 8)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (65, 8, 8)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (66, 9, 8)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (67, 10, 8)
INSERT [dbo].[ServicePharmacy] ([ServicePharmacyId], [ServiceId], [PharmacyId]) VALUES (68, 12, 8)
SET IDENTITY_INSERT [dbo].[ServicePharmacy] OFF
INSERT [dbo].[User] ([UserName], [Password]) VALUES (N'admin', N'2')
INSERT [dbo].[User] ([UserName], [Password]) VALUES (N'user', N'1')
SET IDENTITY_INSERT [dbo].[WorkTime] ON 

INSERT [dbo].[WorkTime] ([WorkTimeId], [WorkTime]) VALUES (1, N'Круглосуточно')
INSERT [dbo].[WorkTime] ([WorkTimeId], [WorkTime]) VALUES (2, N'8:00 - 21:00')
INSERT [dbo].[WorkTime] ([WorkTimeId], [WorkTime]) VALUES (3, N'9:00 - 22:00')
INSERT [dbo].[WorkTime] ([WorkTimeId], [WorkTime]) VALUES (4, N'7:00 - 24:00')
SET IDENTITY_INSERT [dbo].[WorkTime] OFF
ALTER TABLE [dbo].[Pharmacy]  WITH CHECK ADD  CONSTRAINT [FK_Pharmacy_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([CategoryId])
GO
ALTER TABLE [dbo].[Pharmacy] CHECK CONSTRAINT [FK_Pharmacy_Category]
GO
ALTER TABLE [dbo].[Pharmacy]  WITH CHECK ADD  CONSTRAINT [FK_Pharmacy_WorkTime] FOREIGN KEY([WorkTimeId])
REFERENCES [dbo].[WorkTime] ([WorkTimeId])
GO
ALTER TABLE [dbo].[Pharmacy] CHECK CONSTRAINT [FK_Pharmacy_WorkTime]
GO
ALTER TABLE [dbo].[ServicePharmacy]  WITH CHECK ADD  CONSTRAINT [FK_ServicePharmacy_Pharmacy] FOREIGN KEY([PharmacyId])
REFERENCES [dbo].[Pharmacy] ([PharmacyId])
GO
ALTER TABLE [dbo].[ServicePharmacy] CHECK CONSTRAINT [FK_ServicePharmacy_Pharmacy]
GO
ALTER TABLE [dbo].[ServicePharmacy]  WITH CHECK ADD  CONSTRAINT [FK_ServicePharmacy_Service] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Service] ([ServiceId])
GO
ALTER TABLE [dbo].[ServicePharmacy] CHECK CONSTRAINT [FK_ServicePharmacy_Service]
GO
USE [master]
GO
ALTER DATABASE [HotelDB] SET  READ_WRITE 
GO
