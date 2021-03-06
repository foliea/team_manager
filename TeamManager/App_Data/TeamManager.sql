﻿CREATE TABLE [dbo].[Team]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(32) NOT NULL,
    [Avatar] NVARCHAR(128) NULL
)

CREATE TABLE [dbo].[Player]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[TeamId] INT NULL REFERENCES [Team]([Id]),
    [Name] NVARCHAR(32) NOT NULL,
    [Avatar] NVARCHAR(128) NULL,
    [Win] INT NOT NULL DEFAULT 0,
    [Loss] INT NOT NULL DEFAULT 0,
    [Tie] INT NOT NULL DEFAULT 0
)