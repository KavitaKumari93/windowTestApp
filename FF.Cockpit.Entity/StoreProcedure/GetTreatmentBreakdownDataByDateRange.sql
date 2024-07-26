
/****** Object:  StoredProcedure [dbo].[GetTreatmentBreakdownDataByDateRange]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetTreatmentBreakdownDataByDateRange]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetTreatmentBreakdownDataByDateRange]
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
-- GetTreatmentBreakdownDataByDateRange null,'2023-11-01'
ALTER PROCEDURE [dbo].[GetTreatmentBreakdownDataByDateRange] --null,'20231207'
@FromDate datetime,@ToDate datetime
 
AS
 
BEGIN
    SET NOCOUNT ON;
    SET @FromDate = CONVERT(DATE,ISNull(@FromDate ,(select Min(SessionDate) from MergeHighRiskPatientTileData))); 
	SET @ToDate = CONVERT(DATE,@ToDate) ;
 
DECLARE @TotalBodyMappingSession int=0, @TotalSession  int=0, @DermoscopeSession int =0;
 
 --********************@TotalBodyMappingSession  =>>>  {Session where Bodyscan>0}*********************
    set @TotalBodyMappingSession= ( SELECT CASE WHEN SUM(Patients) IS NULL THEN 0 ELSE SUM(Patients) END 
    FROM (
    SELECT COUNT(DISTINCT PatientId) AS Patients  FROM MergeHighRiskPatientTileData
	WHERE BodyScanCount >0 AND SessionDate BETWEEN @FromDate AND @ToDate
	GROUP BY PatientId ) AS tbl);

----********************@Total Session under @from and @todate*********************

	set @TotalSession =(SELECT Count(*) AS totalPatients 
    FROM (
     SELECT CONVERT(DATE,SessionDate) as Sessions,PatientId as  PatientId from  MergeHighRiskPatientTileData 
      where CONVERT(DATE,SessionDate)  between @FromDate and @ToDate
       group by PatientId,CONVERT(DATE,SessionDate)) AS tbl);

--   --PRINT @TotalSession
SET @DermoscopeSession = (@TotalSession - @TotalBodyMappingSession);
 
SELECT 'Dermoscopy' AS Title,@DermoscopeSession AS Value,'#6D5740' AS Color 
UNION
SELECT 'Bodymapping' AS Title,@TotalBodyMappingSession AS Value,'#FF0000' AS Color
 

END
GO
