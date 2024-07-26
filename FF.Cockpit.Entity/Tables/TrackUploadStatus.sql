
IF OBJECT_ID('TrackUploadStatus', 'U') IS  NULL 
BEGIN
CREATE TABLE TrackUploadStatus
(
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [PatientId] [int] NULL,
    [ImageNumber] [int] NOT NULL,
    [MachineName] [varchar](50) NULL,
    [MacAddress] [nvarchar](50) NULL,
    [UploadTime] [datetime] NULL,
    [ImageUploadStatus] [int] NOT NULL FOREIGN KEY REFERENCES MasterSyncStatus(Id) DEFAULT(1),
    [IconUploadStatus] [int] NOT NULL FOREIGN KEY REFERENCES MasterSyncStatus(Id) DEFAULT(1),
    [CreatedOn] [datetime] NULL,
    [CreatedBy] [int] NULL,
    [ModifiedOn] [datetime] NULL,
    [ModifiedBy] [int] NULL,
    [IsDeleted] [bit] NULL DEFAULT(0),
    [DeletedBy] [int] NULL,
    [DeletedOn] [datetime] NULL
)
END
GO