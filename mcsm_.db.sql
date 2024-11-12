﻿﻿﻿USE master
GO

-- Drop Database
ALTER DATABASE MCSM_DB SET SINGLE_USER WITH ROLLBACK IMMEDIATE
GO

USE master
GO
DROP DATABASE MCSM_DB

--Create DB
CREATE DATABASE MCSM_DB
GO

USE MCSM_DB
GO

--Drop All Tables
EXEC sp_MSforeachtable @command1 = "DROP TABLE ?"


--Table Role
CREATE TABLE [Role](
	Id uniqueidentifier primary key NOT NULL,
	[Name] nvarchar(50) NOT NULL
);
GO

INSERT [dbo].[Role] ([Id], [Name]) VALUES (N'2cb0985f-3d9e-4d3b-8318-2a67212e782d', N'Admin')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (N'fd396f04-d521-4300-9d16-66efe124f8f6', N'Monk')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (N'be83d816-75ec-4dfc-8da3-3be8077aad40', N'Nun')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (N'12555e1b-14b2-46c9-b49b-cf1835a17204', N'Practitioner')
GO


--Table Account
CREATE TABLE Account(
	Id uniqueidentifier primary key NOT NULL,
	RoleId uniqueidentifier foreign key references [Role](Id) NOT NULL,
	Email varchar(50) unique NOT NULL,
	HashPassword varchar(255) NOT NULL,
	VerifyToken varchar(max) NOT NULL,
	[Status] nvarchar(100) NOT NULL,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
	UpdateAt datetime
);
GO

CREATE TABLE [Level](
    AccountId uniqueidentifier unique foreign key references Account(Id) NOT NULL,
	RoleType nvarchar(50) NOT NULL,
    RankLevel int NOT NULL, 
    RankName nvarchar(50),
	primary key(AccountId)
);
GO

--Table Profile
CREATE TABLE [Profile](
	AccountId uniqueidentifier unique foreign key references Account(Id) NOT NULL,
	FirstName nvarchar(255) NOT NULL,
	LastName nvarchar(255) NOT NULL,
	DateOfBirth datetime NOT NULL,
	PhoneNumber varchar(50) unique NOT NULL,
	Gender nvarchar(100) NOT NULL,
	Avatar varchar(max),
	primary key(AccountId)
);
GO

--Table DeviceToken
CREATE TABLE DeviceToken(
	Id uniqueidentifier primary key NOT NULL,
	AccountId uniqueidentifier foreign key references Account(Id) NOT NULL,
	Token varchar(max) NOT NULL,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE())
);
GO

--Table Ingredient
CREATE TABLE Ingredient(
	Id uniqueidentifier primary key NOT NULL,
	Name nvarchar(50) NOT NULL
);
GO

--Table Allergy
CREATE TABLE Allergy(
	Id uniqueidentifier primary key NOT NULL,
	IngredientId uniqueidentifier foreign key references Ingredient(Id) NOT NULL,
	AccountId uniqueidentifier foreign key references Account(Id) NOT NULL
);
GO

--Table Notification
CREATE TABLE [Notification](
	Id uniqueidentifier primary key NOT NULL,
	AccountId uniqueidentifier foreign key references Account(Id) NOT NULL,
	Title nvarchar(255) NOT NULL,
	Body nvarchar(max) NOT NULL,
	[Link] nvarchar(255) NULL,
	[Type] nvarchar(50) NULL,
	IsRead bit NOT NULL,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE())
);
GO

--Table Article
CREATE TABLE Article(
	Id uniqueidentifier primary key NOT NULL,
	CreatedBy uniqueidentifier foreign key references Account(Id) NOT NULL,
	Banner varchar(255) NOT NULL,
	Content nvarchar(max) NOT NULL,
	IsActive bit NOT NULL,
	IsDeleted bit NOT NULL,
	UpdateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE())
);
GO

--Table Retreat
CREATE TABLE Retreat(
	Id uniqueidentifier primary key NOT NULL,
	CreatedBy uniqueidentifier foreign key references Account(Id) NOT NULL,
	[Name] nvarchar(100) NOT NULL,
	Cost decimal(16,2) NOT NULL,
	Capacity int NOT NULL,
	RemainingSlots int NOT NULL,
	Duration int NOT NULL,
	[Description] nvarchar(max) NULL,
	StartDate date NOT NULL,
	EndDate date NOT NULL,
	[Status] nvarchar(100) NOT NULL,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE())
);
GO


CREATE TABLE RetreatLearningOutcome (
    Id uniqueidentifier PRIMARY KEY NOT NULL,
    RetreatId uniqueidentifier FOREIGN KEY REFERENCES Retreat(Id) NOT NULL,
    Title nvarchar(255) NOT NULL,
    SubTitle nvarchar(255) NULL,
	[Description] nvarchar(max) NULL,
    CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE())
);
GO

CREATE TABLE RetreatFile (
    Id uniqueidentifier PRIMARY KEY NOT NULL,
    RetreatId uniqueidentifier FOREIGN KEY REFERENCES Retreat(Id) NOT NULL,
	[FileName] nvarchar(255),
    [Url] nvarchar(max) NOT NULL,
    Type nvarchar(50) NOT NULL,
    CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE())
);
GO



CREATE TABLE RetreatMonk(
	Id uniqueidentifier primary key NOT NULL,
	MonkId uniqueidentifier foreign key references Account(Id) NOT NULL,
	RetreatId uniqueidentifier foreign key references Retreat(Id) NOT NULL,
);
GO


CREATE TABLE RetreatRegistration(
	Id uniqueidentifier primary key NOT NULL,
	CreateBy uniqueidentifier foreign key references Account(Id) NOT NULL,
	RetreatId uniqueidentifier foreign key references Retreat(Id) NOT NULL,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
	UpdateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
	TotalCost decimal(16,2) NOT NULL,
	TotalParticipants int NOT NULL,
	IsDeleted bit NOT NULL,
	IsPaid bit NOT NULL
);
GO

--Table RetreatRegistrationParticipants
CREATE TABLE RetreatRegistrationParticipants(
	Id uniqueidentifier primary key NOT NULL,
	ParticipantId uniqueidentifier foreign key references Account(Id) NOT NULL,
	RetreatRegId uniqueidentifier foreign key references RetreatRegistration(Id) NOT NULL,
);
GO

DROP TABLE IF EXISTS Payment
GO
--Table Payment
CREATE TABLE Payment(
	Id nvarchar(255) primary key NOT NULL,
	AccountId uniqueidentifier foreign key references Account(Id) NOT NULL,
	RetreatRegId uniqueidentifier foreign key references RetreatRegistration(Id) NOT NULL,
	PaypalOrderId nvarchar(255) NOT NULL,
	PaymentMethod nvarchar(100) NOT NULL,
	Amount decimal(16,2) NOT NULL,
	[Description] nvarchar(255) NULL,
	[Status] nvarchar(100) NOT NULL,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
);
GO

--Table RetreatGroup
CREATE TABLE RetreatGroup(
	Id uniqueidentifier primary key NOT NULL,
	RetreatId uniqueidentifier foreign key references Retreat(Id) NOT NULL,
	MonkId uniqueidentifier foreign key references Account(Id) NOT NULL,
	Name nvarchar(50) NOT NULL
);
GO

--Table RetreatGroupMember
CREATE TABLE RetreatGroupMember(
	Id uniqueidentifier primary key NOT NULL,
	GroupId uniqueidentifier foreign key references RetreatGroup(Id) NOT NULL,
	MemberId uniqueidentifier foreign key references Account(Id) NOT NULL
);
GO

--Table Lesson
CREATE TABLE Lesson(
	Id uniqueidentifier primary key NOT NULL,
	CreatedBy uniqueidentifier foreign key references Account(Id) NOT NULL,
	Title nvarchar(200) NOT NULL,
	Content nvarchar(max) NOT NULL,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
	UpdateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
	IsActive bit NOT NULL,
	IsDeleted bit NOT NULL
);
GO

--Table RoomType
CREATE TABLE RoomType(
	Id uniqueidentifier primary key NOT NULL,
	[Name] nvarchar(50) NOT NULL
);
GO


--Table Room
CREATE TABLE Room(
	Id uniqueidentifier primary key NOT NULL,
	RoomTypeId uniqueidentifier foreign key references RoomType(Id) NOT NULL,
	[Name] nvarchar(50) NOT NULL,
	Capacity int NOT NULL,
	[Status] nvarchar(20) NOT NULL,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE())
);
GO

--Table RetreatLesson
CREATE TABLE RetreatLesson(
	Id uniqueidentifier primary key NOT NULL,
	RetreatId uniqueidentifier foreign key references Retreat(Id) NOT NULL,
	LessonId uniqueidentifier foreign key references Lesson(Id) NOT NULL
);
GO

--Table RetreatSchedule
CREATE TABLE RetreatSchedule(
	Id uniqueidentifier primary key NOT NULL,
	RetreatId uniqueidentifier foreign key references Retreat(Id) NOT NULL,
	GroupId uniqueidentifier foreign key references RetreatGroup(Id) NOT NULL,
	RetreatLessonId uniqueidentifier foreign key references RetreatLesson(Id),
	UsedRoomId uniqueidentifier foreign key references Room(Id),
	LessonDate date NOT NULL,
	LessonStart time NOT NULL,
	LessonEnd time NOT NULL,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE())
);
GO

--Table RetreatGroupMessage
CREATE TABLE RetreatGroupMessage(
	Id uniqueidentifier primary key NOT NULL,
	CreatedBy uniqueidentifier foreign key references Account(Id) NOT NULL,
	GroupId uniqueidentifier foreign key references RetreatGroup(Id) NOT NULL,
	ReplyTo uniqueidentifier,
	Content nvarchar(max) NOT NULL,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
	UpdateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
	IsDeleted bit NOT NULL
);
GO

--Table Tool
CREATE TABLE Tool(
	Id uniqueidentifier primary key NOT NULL,
	[Name] nvarchar(50) NOT NULL,
	[Image] nvarchar(max) NOT NULL,
	TotalTool int NOT NULL,
	[Status] nvarchar(20) NOT NULL,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
);
GO


--Table ToolHistory
CREATE TABLE ToolHistory(
	Id uniqueidentifier primary key NOT NULL,
	CreatedBy uniqueidentifier foreign key references Account(Id) NOT NULL,
	RetreatId uniqueidentifier foreign key references Retreat(Id) NOT NULL,
	ToolId uniqueidentifier foreign key references Tool(Id) NOT NULL,
	NumOfTool int NOT NULL,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE())
);
GO

--Table DishType
CREATE TABLE DishType(
	Id uniqueidentifier primary key NOT NULL,
	[Name] nvarchar(50) NOT NULL
);
GO

--Table Dish
CREATE TABLE Dish(
	Id uniqueidentifier primary key NOT NULL,
	CreatedBy uniqueidentifier foreign key references Account(Id) NOT NULL,
	DishTypeId uniqueidentifier foreign key references DishType(Id) NOT NULL,
	[Name] nvarchar(50) NOT NULL,
	IsHalal bit NOT NULL,
	IsActive bit NOT NULL,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
	UpdateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE())
);
GO

--Table DishIngredient
CREATE TABLE DishIngredient(
	Id uniqueidentifier primary key NOT NULL,
	DishId uniqueidentifier foreign key references Dish(Id) NOT NULL,
	IngredientId uniqueidentifier foreign key references Ingredient(Id) NOT NULL
);
GO

--Table Menu
CREATE TABLE Menu(
	Id uniqueidentifier primary key NOT NULL,
	CreatedBy uniqueidentifier foreign key references Account(Id) NOT NULL,
	CookDate date NOT NULL,
	IsBreakfast bit NOT NULL,
	IsLunch bit NOT NULL,
	IsDinner bit NOT NULL,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
	UpdateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
	IsActive bit NOT NULL
);
GO

--Table MenuDish
CREATE TABLE MenuDish(
	Id uniqueidentifier primary key NOT NULL,
	MenuId uniqueidentifier foreign key references Menu(Id) NOT NULL,
	DishId uniqueidentifier foreign key references Dish(Id) NOT NULL
);
GO

--Table Post
CREATE TABLE Post(
	Id uniqueidentifier primary key NOT NULL,
	CreatedBy uniqueidentifier foreign key references Account(Id) NOT NULL,
	Content nvarchar(MAX),
	[Status] nvarchar(20) NOT NULL,
	UpdateAt datetime,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE())
);
GO

--Table post image
CREATE TABLE PostImage(
	Id uniqueidentifier primary key NOT NULL,
	PostId uniqueidentifier foreign key references Post(Id) NOT NULL,
	[Url] nvarchar(max) NOT NULL,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE())
);
GO

--Table Comment
CREATE TABLE Comment(
	Id uniqueidentifier primary key NOT NULL,
	PostId uniqueidentifier foreign key references Post(Id) NOT NULL,
	ReplyTo uniqueidentifier NULL,
	Content nvarchar(MAX),
	UpdateAt datetime,
	IsDeleted bit NOT NULL DEFAULT 0, 
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
);
GO


--Table Reaction
CREATE TABLE Reaction(
	Id uniqueidentifier primary key NOT NULL,
	PostId uniqueidentifier foreign key references Post(Id) NOT NULL,
	AccountId uniqueidentifier foreign key references Account(Id) NOT NULL,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE())
);
GO

--Table Feedback
CREATE TABLE Feedback(
	Id uniqueidentifier primary key NOT NULL,
	CreatedBy uniqueidentifier foreign key references Account(Id) NOT NULL,
	RetreatId uniqueidentifier foreign key references Retreat(Id) NOT NULL,
	Content nvarchar(max) NOT NULL,
	Rating int NOT NULL,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
	UpdateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
	IsDeleted bit NOT NULL
);
GO