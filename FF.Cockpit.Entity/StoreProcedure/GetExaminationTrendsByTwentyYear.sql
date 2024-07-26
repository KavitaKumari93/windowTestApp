/****** Object:  StoredProcedure [dbo].[GetExaminationTrendsByTwentyYear]    Script Date: 12/11/2023 7:00:35 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetExaminationTrendsByTwentyYear]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetExaminationTrendsByTwentyYear]
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
ALTER PROCEDURE [dbo].[GetExaminationTrendsByTwentyYear] 
@fromDate DateTime, @toDate DateTime
AS

BEGIN

SET @fromDate = CONVERT(DATE , @fromDate);
SET @toDate = CONVERT(DATE , @toDate);

--****************************** #Years ***********************************
; with  Years AS
        (
        select  DATEPART(YEAR,@fromDate) AS SessionYear
        union all
        select  SessionYear + 1
        from    Years
        where   SessionYear < DATEPART(YEAR, @toDate)
        )
Select  SessionYear Into #YearsSeriesTemp From   Years
--****************************** #TotalSessions_YearWise ***********************************
--SELECT iit.ObjectID AS PatientId,YEAR(CONVERT(date,Shootingdate)) AS SessionYear,CONVERT(DATE,Shootingdate) AS Shootingdate
--INTO #TotalSessions_YearWise
--FROM [FotoFinder.Universe].[dbo].ImagesInfoTable iit 
--INNER JOIN MasterTable mt on mt.ObjectId=iit.ObjectID
--WHERE iit.Deleted=0 AND mt.Deleted=0 AND CONVERT(DATE,shootingdate) between @fromDate AND @toDate
--GROUP BY iit.ObjectID,CONVERT(DATE,Shootingdate)

--****************************** #BodyScans ***********************************

--SELECT COUNT(bsmt.BodyScanMoleId) BodyScanCount,bst.PatientId AS PatientId
--INTO #BodyScanDetailsTemp	   
--FROM [FotoFinder.Universe].[dbo].BodyScanMoleTable bsmt
--JOIN [FotoFinder.Universe].[dbo].BodyScanMoleImageTable bsmit ON bsmt.BodyScanMoleImageId=bsmit.BodyScanMoleImageId
--JOIN [FotoFinder.Universe].[dbo].BodyMappingSessionImageRelationTable bmsir ON bsmit.ImageId=bmsir.ImageNumber
--JOIN [FotoFinder.Universe].[dbo].BodyMappingSessionTable bst ON bst.SessionId=bmsir.SessionId
--WHERE bst.Deleted=0 and bsmit.BodyScanTypeId=0 and CONVERT(DATE,SessionTimeStamp) BETWEEN @fromDate AND @toDate
--GROUP BY CAST(bst.SessionTimeStamp AS DATE),bst.PatientId 

SELECT PatientId,
(CASE WHEN BodyScanCount IS NOT NULL THEN BodyScanCount ELSE 0 END) AS BodyScanCount
,YEAR(CONVERT(date,SessionDate)) AS SessionYear
INTO #BodyScans
FROM MergeHighRiskPatientTileData where
CONVERT(DATE,SessionDate) between @fromDate AND @toDate
GROUP BY PatientId,BodyScanCount,CONVERT(DATE,SessionDate)


--select * from #BodyScans
--*****************Get DermoscopeSessionCount ***********************
SELECT SessionYear,COUNT(BodyScanCount) AS DermoscopeSessionCount 
INTO #DermoscopeSession_YearWise FROM #BodyScans
WHERE BodyScanCount=0 GROUP BY SessionYear ORDER BY SessionYear
--select * from #DermoscopeSession_YearWise
--*****************Get BodyMappingSessionCount ***********************
SELECT SessionYear,COUNT(BodyScanCount) AS BodyMappingSessionCount 
INTO #BodyMappingSession_YearWise FROM #BodyScans
WHERE BodyScanCount<>0 GROUP BY SessionYear ORDER BY SessionYear
--select * from #BodyMappingSession_YearWise


Select yearseries.SessionYear [Year],
CASE WHEN BodyMappingSessionCount IS NULL THEN 0 ELSE BodyMappingSessionCount END AS BodyMappingSessionCount,
CASE WHEN DermoscopeSessionCount IS NULL THEN 0 ELSE DermoscopeSessionCount END AS  DermoscopeSessionCount
from (SELECT 
CASE WHEN bodyMapping.SessionYear IS NULL THEN dermoscope.SessionYear ELSE bodyMapping.SessionYear END AS SessionYear,
CASE WHEN bodyMapping.BodyMappingSessionCount IS NULL THEN 0 ELSE bodyMapping.BodyMappingSessionCount END AS BodyMappingSessionCount,
CASE WHEN dermoscope.DermoscopeSessionCount IS NULL THEN 0 ELSE dermoscope.DermoscopeSessionCount END AS  DermoscopeSessionCount
FROM #BodyMappingSession_YearWise bodyMapping
FULL OUTER JOIN #DermoscopeSession_YearWise dermoscope  ON bodyMapping.SessionYear=dermoscope.SessionYear)as tbl
RIGHT JOIN  #YearsSeriesTemp yearseries on tbl.SessionYear=yearseries.SessionYear



DROP TABLE #YearsSeriesTemp 
DROP TABLE #BodyScans
DROP TABLE #DermoscopeSession_YearWise
DROP TABLE #BodyMappingSession_YearWise
END
GO