IF OBJECT_ID('TrackDownloadStatus', 'U') IS  NULL 
BEGIN
CREATE TABLE TrackDownloadStatus
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[PatientId] [int] NULL,
	[ImageNumber] [int] NOT NULL,
	[MachineName] [varchar](50) NULL,
	[MacAddress] [nvarchar](50) NULL,
	[DownloadTime] [datetime] NULL,
	[ImageDownloadStatus] [int] NOT NULL FOREIGN KEY REFERENCES MasterSyncStatus(Id) DEFAULT(6),
	[IconDownloadStatus] [int] NOT NULL FOREIGN KEY REFERENCES MasterSyncStatus(Id) DEFAULT(6),
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsDeleted] [bit] NULL DEFAULT(0),
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
)
END
GO