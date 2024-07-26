IF OBJECT_ID('PatientMedicalHistoryTable', 'U') IS  NULL 
BEGIN
  CREATE TABLE [dbo].[PatientMedicalHistoryTable](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PatientID] [int] NULL,
	[IsSkinCancerAnalysed] bit,
	[IsCancerAnalysed] bit,
	[IsGeneticalConspicuousAnalsyed] bit,
	[LAStUpdatedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
