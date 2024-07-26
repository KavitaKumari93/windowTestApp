IF OBJECT_ID('SyncVideoDownloadLogs', 'U') IS  NULL 
BEGIN
CREATE TABLE SyncVideoDownloadLogs
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Message] [nvarchar](max) NULL,
	[MessageTemplate] [nvarchar](max) NULL,
	[Level] [nvarchar](max) NULL,
	[TimeStamp] [datetime] NULL,
	[Exception] [nvarchar](max) NULL,
	[Properties] [nvarchar](max) NULL
)
END

GO