IF OBJECT_ID('MergeHighRiskPatientTileData', 'U') IS  NULL 
BEGIN
CREATE TABLE [dbo].[MergeHighRiskPatientTileData](
	[PatientId] [int] NULL,
	[PatientName] [varchar](100) NULL,
	[DateOfBirth] [datetime] NULL,
	[SessionDate] [datetime] NULL,
	[BodyScanCount] [int] NULL,
	[MarkerCount] [int] NULL,
	[MicroImageCount] [int] NULL,
	[ExcisionCount] [int] NULL,
	[SessionDuration] [int] NULL,
	[Flag] [bit] NULL,
	[TBMImagesCount] [int] NULL
) ON [PRIMARY]
END

GO