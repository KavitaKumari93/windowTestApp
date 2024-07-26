IF OBJECT_ID('MasterSyncStatus', 'U') IS  NULL 
BEGIN
CREATE TABLE MasterSyncStatus
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Status] [varchar](50) NULL,
)

INSERT [dbo].[MasterSyncStatus] ([Status]) VALUES ('Upload Pending')
INSERT [dbo].[MasterSyncStatus] ([Status]) VALUES ('Upload Completed')
INSERT [dbo].[MasterSyncStatus] ([Status]) VALUES ('Upload Failed')
INSERT [dbo].[MasterSyncStatus] ([Status]) VALUES ('Upload Partially')
INSERT [dbo].[MasterSyncStatus] ([Status]) VALUES ('File Edited')
INSERT [dbo].[MasterSyncStatus] ([Status]) VALUES ('Download Pending')
INSERT [dbo].[MasterSyncStatus] ([Status]) VALUES ('Download Completed')
INSERT [dbo].[MasterSyncStatus] ([Status]) VALUES ('Download Failed')
INSERT [dbo].[MasterSyncStatus] ([Status]) VALUES ('Download Partially')
END
Go
