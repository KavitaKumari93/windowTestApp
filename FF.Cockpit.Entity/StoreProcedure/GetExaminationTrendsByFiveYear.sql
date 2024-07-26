IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetExaminationTrendsByFiveYear]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetExaminationTrendsByFiveYear]
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
--GetExaminationTrendsByFiveYear '2018-09-30','2023-09-30'
--GetExaminationTrendsByFiveYear '2018-10-01','2023-09-30'
ALTER PROCEDURE [dbo].[GetExaminationTrendsByFiveYear] 
@fromDate DateTime, @toDate DateTime
AS

BEGIN

SET @fromDate = CONVERT(DATE , @fromDate);
SET @toDate = CONVERT(DATE , @toDate);

----****************************** #YearsQuarterList ***********************************
; with  YearsQuarter AS
        (select distinct year(dateadd(day,rnum,@fromDate)) yr,
          datepart(quarter,dateadd(day,rnum,@fromDate)) qtr
           from (select row_number() over(order by (select null)) as rnum 
           from master..spt_values) t
            where dateadd(day,rnum,@fromDate) <= @toDate)
           
Select  * Into #YearsQuarterTemp From   YearsQuarter
--Select  * from #YearsQuarterTemp

SELECT PatientId AS PatientId,(CASE WHEN bsmt.BodyScanCount IS NOT NULL THEN bsmt.BodyScanCount ELSE 0 END) AS BodyScanCount
,YEAR(CONVERT(date,SessionDate)) AS SessionYear,DATEPART(qq,CONVERT(DATE,SessionDate)) AS SessionQuarter
INTO #BodyScans
FROM MergeHighRiskPatientTileData bsmt
GROUP BY PatientId,BodyScanCount,CONVERT(date,SessionDate)

--select * from #BodyScans where PatientId=30
--*****************Get DermoscopeSessionCount ***********************
SELECT SessionYear,SessionQuarter,COUNT(BodyScanCount) AS DermoscopeSessionCount 
INTO #DermoscopeSession_QuarterWise
FROM #BodyScans
WHERE BodyScanCount=0 GROUP BY SessionYear,SessionQuarter ORDER BY SessionYear,SessionQuarter

--Select * from #DermoscopeSession_QuarterWise

--*****************Get BodyMappingSessionCount ***********************
SELECT SessionYear,SessionQuarter,COUNT(BodyScanCount) AS BodyMappingSessionCount 
INTO #BodyMappingSession_QuarterWise
FROM #BodyScans
WHERE BodyScanCount<>0 GROUP BY SessionYear,SessionQuarter ORDER BY SessionYear,SessionQuarter
--Select * from #BodyMappingSession_QuarterWise


SELECT 

CASE WHEN bodyMapping.SessionYear IS NULL THEN dermoscope.SessionYear ELSE bodyMapping.SessionYear  END  AS SessionYear,
 CASE WHEN bodyMapping.SessionQuarter IS NULL THEN dermoscope.SessionQuarter Else bodyMapping.SessionQuarter  END AS SessionQuarter,
 CASE WHEN bodyMapping.BodyMappingSessionCount  IS NULL THEN 0   Else bodyMapping.BodyMappingSessionCount   END AS BodyMappingSessionCount,
 CASE WHEN  dermoscope.DermoscopeSessionCount IS NULL THEN 0 Else dermoscope.DermoscopeSessionCount END AS DermoscopeSessionCount
into #FinalData
FROM #BodyMappingSession_QuarterWise bodyMapping
Full OUTER JOIN  #DermoscopeSession_QuarterWise dermoscope 
ON bodyMapping.SessionYear=dermoscope.SessionYear AND bodyMapping.SessionQuarter=dermoscope.SessionQuarter

--select * from #FinalData

select YQ.yr  as [Year],
YQ.qtr  as [Quarter],
Case WHEN BodyMappingSessionCount IS NULL THEN 0 ELSE BodyMappingSessionCount END AS  BodyMappingSessionCount,
Case WHEN DermoscopeSessionCount IS NULL THEN 0 ELSE DermoscopeSessionCount END AS  DermoscopeSessionCount  
FROM  #FinalData DF
RIGHT JOIN #YearsQuarterTemp YQ on DF.SessionYear=YQ.yr and Df.SessionQuarter =YQ.qtr
Order By YQ.yr,YQ.qtr
--DROP TABLE #TotalSessions_QuarterWise
--DROP TABLE #BodyScanDetailsTemp
DROP TABLE #BodyScans
DROP TABLE #DermoscopeSession_QuarterWise
DROP TABLE #BodyMappingSession_QuarterWise
DROP TABLE #FinalData
DROP TABLE #YearsQuarterTemp

END
GO