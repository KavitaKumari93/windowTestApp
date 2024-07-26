
/****** Object:  Table [dbo].[Modules]    Script Date: 3/22/2024 3:34:30 PM ******/
IF OBJECT_ID('Modules', 'U') IS  NULL 
BEGIN
CREATE TABLE [dbo].[Modules]
    (
	[ModuleId] [int] PRIMARY KEY  NOT NULL Identity(1,1),
	[ModuleName] [nvarchar](50) NULL,
	[IsDeleted] [bit] DEFAULT 0,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL
	)
END
GO
