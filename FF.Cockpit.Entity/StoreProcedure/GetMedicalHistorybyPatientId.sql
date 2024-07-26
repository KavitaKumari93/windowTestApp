/****** Object:  StoredProcedure [dbo].[GetMedicalHistorybyPatientId]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetMedicalHistorybyPatientId]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetMedicalHistorybyPatientId]
    	AS
		begin
			select 1
		end')
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetMedicalHistorybyPatientId]
 @patientId int

AS

BEGIN
	SET NOCOUNT ON;

	SELECT PatientID,
	IsSkinCancerAnalysed,
	IsCancerAnalysed,
	IsGeneticalConspicuousAnalsyed ,
	LAStUpdatedOn
	FROM PatientMedicalHistoryTable  
	WHERE PatientID=@patientId ORDER BY LAStUpdatedOn DESC
END
GO
