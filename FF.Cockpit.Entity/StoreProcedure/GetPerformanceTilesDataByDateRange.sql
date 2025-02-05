/****** Object:  StoredProcedure [dbo].[GetPerformanceTilesDataByDateRange]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetPerformanceTilesDataByDateRange]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetPerformanceTilesDataByDateRange]
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
ALTER PROCEDURE [dbo].[GetPerformanceTilesDataByDateRange] 
    @FromDate DATETIME,
    @ToDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    SET @FromDate = CONVERT(DATE,ISNull(@FromDate ,(SELECT MIN(SessionDate) FROM MergeHighRiskPatientTileData))); 
    SET @ToDate = CONVERT(DATE,@ToDate) ;
    DECLARE @OpinionRequestCount INT 
    DECLARE @PatientCount INT  
    DECLARE @ExcisionCount INT  
    --DECLARE @TotalBodyMappingSessions INT 
    DECLARE @LowFrequentPatientCount INT 


--***************Get the count for LowFrequentPatientCount
    SELECT DISTINCT m.PatientId
    INTO #temp1
    FROM MergeHighRiskPatientTileData m
    WHERE Flag=1 AND CONVERT(DATE, SessionDate) NOT BETWEEN @FromDate AND @ToDate

    SELECT DISTINCT m.PatientId
    INTO #temp2
    FROM MergeHighRiskPatientTileData m
    WHERE Flag=1 AND CONVERT(DATE, SessionDate) BETWEEN @FromDate AND @ToDate 

    SELECT @LowFrequentPatientCount =  COUNT(DISTINCT t1.PatientId)  
    FROM #temp1 t1
    LEFT JOIN #temp2 t2 ON t1.PatientId = t2.Patientid
    WHERE t2.patientid IS NULL;

	 SELECT @LowFrequentPatientCount as LowFrequentPatientCount ;
 
    DROP table #temp1
    DROP table #temp2
END
GO
