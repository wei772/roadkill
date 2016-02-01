--USE Roadkill;

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'roadkill_pagecontent')
    DROP TABLE [dbo].[roadkill_pagecontent];

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'roadkill_pages')
    DROP TABLE [roadkill_pages];

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'roadkill_users')
    DROP TABLE [roadkill_users];

-- SCHEMA (taken from Core/Database/Schema/SqlServer)
CREATE TABLE [dbo].[roadkill_pages]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Tags] [nvarchar](255) NULL,
	[CreatedBy] [nvarchar](255) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[ModifiedBy] [nvarchar](255) NULL,
	[ModifiedOn] [datetime] NULL,
	PRIMARY KEY CLUSTERED (Id)
);

CREATE TABLE [dbo].[roadkill_pagecontent]
(
	[Id] [uniqueidentifier] NOT NULL,
	[EditedBy] [nvarchar](255) NOT NULL,
	[EditedOn] [datetime] NOT NULL,
	[VersionNumber] [int] NOT NULL,
	[Text] [nvarchar](MAX) NULL,
	[PageId] [int] NOT NULL,
	PRIMARY KEY NONCLUSTERED (Id)
);

ALTER TABLE [dbo].[roadkill_pagecontent] ADD CONSTRAINT [FK_roadkill_pageid] FOREIGN KEY([pageid]) REFERENCES [dbo].[roadkill_pages] ([id]);

CREATE TABLE [dbo].[roadkill_users]
(
	[Id] [uniqueidentifier] NOT NULL,
	[ActivationKey] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Firstname] [nvarchar](255) NULL,
	[Lastname] [nvarchar](255) NULL,
	[IsEditor] [bit] NOT NULL,
	[IsAdmin] [bit] NOT NULL,
	[IsActivated] [bit] NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[PasswordResetKey] [nvarchar](255) NULL,
	[Salt] [nvarchar](255) NOT NULL,
	[Username] [nvarchar](255) NOT NULL,
	PRIMARY KEY NONCLUSTERED (Id)
);

CREATE CLUSTERED INDEX [roadkill_pagecontent_idx] ON [dbo].[roadkill_pagecontent] (PageId, VersionNumber);
CREATE CLUSTERED INDEX [roadkill_users_idx] ON [dbo].[roadkill_users] (Email);

-- DATA
SET IDENTITY_INSERT roadkill_pages ON;

-- Users
INSERT INTO roadkill_users (id, activationkey, email, firstname, iseditor, isadmin, isactivated, lastname, password, passwordresetkey, salt, username) VALUES ('aabd5468-1c0e-4277-ae10-a0ce00d2fefc','','admin@localhost','Chris','0','1','1','Admin','C882A7933951FCC4197718B104AECC53564FC205','','J::]T!>k5LR|.{U9','admin');
INSERT INTO roadkill_users (id, activationkey, email, firstname, iseditor, isadmin, isactivated, lastname, password, passwordresetkey, salt, username) VALUES ('4d0bc016-1d47-4ad3-a6fe-a11a013ef9c8','3d12daea-16d0-4bd6-9e0c-347f14e0d97d','editor@localhost','','1','0','1','','7715C929E99254C117657B0937E97926443FDAF6','','fO)M`*QU:eH''Xl_%','editor');
