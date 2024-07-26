IF OBJECT_ID('MasterTable', 'U') IS  NULL 
BEGIN 
CREATE TABLE [dbo].[MasterTable](
	[ObjectId] [int] NOT NULL,
	[ProjectCreator] [nvarchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[ObjectGUID] [uniqueidentifier] NULL,
	[TimeStamp] [timestamp] NULL,
	[LastName] [nvarchar](255) NULL,
	[FirstName] [nvarchar](255) NULL,
	[MiddleName] [nvarchar](255) NULL,
	[DateOfBirth] [datetime] NULL,
	[Gender] [nvarchar](1) NULL,
	[BodyHeight] [int] NULL,
	[Deleted] [bit] NULL,
	[PatientShootingDate] [datetime] NULL,
	[PatientStudy] [nvarchar](255) NULL,
	[PatientRecall] [image] NULL,
	[PatientRecordnumber] [nvarchar](50) NULL,
	[Address1] [nvarchar](50) NULL,
	[Address2] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[State] [nvarchar](50) NULL,
	[Zip] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Fax] [nvarchar](50) NULL,
	[WebvaultId] [nvarchar](32) NULL,
	[ExtraData] [xml] NULL,
	[Comment] [nvarchar](4000) NULL,
	[SkinTypeId] [int] NOT NULL,
 CONSTRAINT [PK_MasterTable] PRIMARY KEY CLUSTERED 
(
	[ObjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
