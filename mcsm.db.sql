﻿USE master
GO

DROP DATABASE IF EXISTS MCSM_DB
GO

CREATE DATABASE MCSM_DB
GO

USE MCSM_DB
GO

--Table Role
DROP TABLE IF EXISTS [Role];
GO
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


--Table account
DROP TABLE IF EXISTS Account;
GO
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

--Table Profile
DROP TABLE IF EXISTS [Profile];
GO
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
