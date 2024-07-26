
---Create MiscellaneousData Table
IF OBJECT_ID('MiscellaneousData', 'U') IS  NULL 
BEGIN
Create Table MiscellaneousData
(
	[Id] [int] IDENTITY(1,1) NOT NULL Primary Key,
	[Name] [nvarchar](50) NULL,
	[Value] [nvarchar](100) NULL,
	[IsDeleted] [bit] default 0,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [datetime] NULL,
)
END

INSERT INTO MiscellaneousData(Name,Value) values ('StartHour','360');
INSERT INTO MiscellaneousData(Name,Value) values ('EndHour','1200');
GO
