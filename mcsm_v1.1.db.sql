
USE master
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
	[Level] int,
	VerifyToken varchar(max) NOT NULL,
	[Status] nvarchar(100) NOT NULL,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
	UpdateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE())
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
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
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
	AccountId uniqueidentifier foreign key references Account(Id) NOT NULL,
);
GO

--Table Notification
CREATE TABLE [Notification](
	Id uniqueidentifier primary key NOT NULL,
	AccountId uniqueidentifier foreign key references Account(Id) NOT NULL,
	Content nvarchar(max) NOT NULL,
	[Url] nvarchar(255),
	[Type] nvarchar(50) NOT NULL,
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
	Capacity int NOT NULL,
	Duration int NOT NULL,
	StartDate date NOT NULL,
	EndDate date NOT NULL
);
GO

--Table RetreatMonk
CREATE TABLE RetreatMonk(
	Id uniqueidentifier primary key NOT NULL,
	MonkId uniqueidentifier foreign key references Account(Id) NOT NULL,
	RetreatId uniqueidentifier foreign key references Retreat(Id) NOT NULL
);
GO

--Table RetreatRegistration
CREATE TABLE RetreatRegistration(
	Id uniqueidentifier primary key NOT NULL,
	MonkId uniqueidentifier foreign key references Account(Id) NOT NULL,
	PractitionerId uniqueidentifier foreign key references Account(Id) NOT NULL
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
	MemberId uniqueidentifier foreign key references Account(Id) NOT NULL,
);
GO

--Table Lesson
CREATE TABLE Lesson(
	Id uniqueidentifier primary key NOT NULL,
	CreatedBy uniqueidentifier foreign key references Account(Id) NOT NULL,
	Content nvarchar(max) NOT NULL,
	IsActive bit NOT NULL
);
GO

--Table RoomType
CREATE TABLE RoomType(
	Id uniqueidentifier primary key NOT NULL,
	[Name] nvarchar(50) NOT NULL
);
GO


INSERT [dbo].[RoomType] ([Id], [Name]) VALUES (N'152eafd1-d15c-4bfc-bb44-0f8c110272fd', N'Hall')
INSERT [dbo].[RoomType] ([Id], [Name]) VALUES (N'aa4f1943-eae6-4e0b-bf33-e4af7d2dc4fc', N'Bed room')
INSERT [dbo].[RoomType] ([Id], [Name]) VALUES (N'da70d2aa-f2ca-4aa0-aa15-215e2fb2ed44', N'Dining room')
GO


--Table Room
CREATE TABLE Room(
	Id uniqueidentifier primary key NOT NULL,
	RoomTypeId uniqueidentifier foreign key references RoomType(Id) NOT NULL,
	[Name] nvarchar(50) NOT NULL,
	Capacity int NOT NULL,
	IsActive bit NOT NULL
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
	LessonEnd time NOT NULL
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
	TotalTool int NOT NULL,
	AvailableTool int NOT NULL,
	IsActive bit NOT NULL
);
GO

--Table ToolOperation
CREATE TABLE ToolOperation(
	Id uniqueidentifier primary key NOT NULL,
	[Name] nvarchar(50) NOT NULL,
	IsIncrement bit NOT NULL
);
GO

--Table ToolHistory
CREATE TABLE ToolHistory(
	Id uniqueidentifier primary key NOT NULL,
	CreatedBy uniqueidentifier foreign key references Account(Id) NOT NULL,
	RetreatId uniqueidentifier foreign key references Retreat(Id) NOT NULL,
	ToolId uniqueidentifier foreign key references Tool(Id) NOT NULL,
	ToolOpId uniqueidentifier foreign key references ToolOperation(Id) NOT NULL,
	NumOfTool int NOT NULL,
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
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
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
	UpdateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
	IsDeleted bit NOT NULL
);
GO

--Table Comment
CREATE TABLE Comment(
	Id uniqueidentifier primary key NOT NULL,
	PostId uniqueidentifier foreign key references Post(Id) NOT NULL,
	ReplyTo uniqueidentifier,
	Content nvarchar(MAX),
	CreateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
	UpdateAt datetime NOT NULL DEFAULT DATEADD(HOUR, 7, GETUTCDATE()),
	IsDeleted bit NOT NULL
);
GO

--Table Like
CREATE TABLE [Like](
	Id uniqueidentifier primary key NOT NULL,
	PostId uniqueidentifier foreign key references Post(Id) NOT NULL,
	AccountId uniqueidentifier foreign key references Account(Id) NOT NULL,
);

GO
