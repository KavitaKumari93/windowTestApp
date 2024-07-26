/****** Object:  StoredProcedure [dbo].[InsertMedicalHistoryDatabyPatientId]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[InsertMedicalHistoryDatabyPatientId]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[InsertMedicalHistoryDatabyPatientId]
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
ALTER PROCEDURE [dbo].[InsertMedicalHistoryDatabyPatientId] 
(
@patientId int ,
@skinCancerAnalysed bit , 
@cancerAnalysed bit , 
@geneticalConspicuousAnalysed bit 
)
AS
  BEGIN
     
            Insert Into PatientMedicalHistoryTable 
           ( 
             PatientID, 
             IsSkinCancerAnalysed,
             IsCancerAnalysed, 
             IsGeneticalConspicuousAnalsyed, 
             LAStUpdatedOn
           ) 
            VALUES  
			( @patientId,
              @skinCancerAnalysed,
              @cancerAnalysed,
              @geneticalConspicuousAnalysed,
              GETDATE()
			)
 END
GO

