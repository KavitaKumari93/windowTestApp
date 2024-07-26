
IF OBJECT_ID('ExcisedMarkersData', 'U') IS  NULL 
BEGIN 
CREATE TABLE [dbo].[ExcisedMarkersData](
	[PatientId] [int] NULL,
	[MarkNo] [int] NULL,
	[ExcisedDate] [datetime] NULL,
	[MarkText] [int] NULL
) ON [PRIMARY]
END
GO
