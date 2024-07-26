IF OBJECT_ID('RaisedEvents', 'U') IS  NULL 
BEGIN 
CREATE TABLE [dbo].[RaisedEvents](
    [EventId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [FileName] [varchar](100) NULL,
    [FileType] [varchar](100) NULL,
    [FilePath] [varchar](500) NULL,
    [ActionType] [varchar](50) NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [CreatedOn] [datetime] DEFAULT GETDATE() NOT NULL,
    [CreatedBy] [int] NULL,
    [UpdatedOn] [datetime] NULL,
    [UpdatedBy] [int] NULL,
    [IsProcessed] [bit] NULL DEFAULT 0
)
END
GO

