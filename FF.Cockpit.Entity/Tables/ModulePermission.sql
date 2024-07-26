U/****** Object:  Table [dbo].[ModulePermission]    Script Date: 3/22/2024 3:34:30 PM ******/
IF OBJECT_ID('[ModulePermission]', 'U') IS  NULL 
BEGIN
CREATE TABLE [dbo].[ModulePermission]
    (
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] FOREIGN KEY REFERENCES Modules(ModuleId),
	[RoleId] int FOREIGN KEY REFERENCES RolesTable(RoleId),
	[IsAccessible] bit default 0,
	[IsDeleted] bit default 0,
	[CreatedBy] [nvarchar](10) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](10) NULL,
	[UpdatedDate] [datetime] NULL
	)
END
GO
