IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetExaminationTrendsByOneYear]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetExaminationTrendsByOneYear]
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
--GetExaminationTrendsByOneYear '2022-11-01','2023-10-30'
ALTER PROCEDURE [dbo].[GetExaminationTrendsByOneYear]
@fromDate DateTime, @toDate DateTime
AS
BEGIN
SET @fromDate = CONVERT(DATE , @fromDate);
SET @toDate = CONVERT(DATE , @toDate);

;with Months (date)
AS
(
   SELECT DATEADD(DAY,1,EOMONTH(@fromDate,-1))
    UNION ALL
    SELECT DATEADD(month,1,date)
    from months
    where DATEADD(month,1,date) < EOMONTH(@toDate)
)
select DATEPART(month,date)as Monthnumber into #MonthNumber
from months

--SELECT * FROM #MonthNumber
--****************************** #TotalSessions_MonthWise ***********************************
select  PatientId,
CONVERT(CHAR(3),CONVERT(DATE,SessionDate), 0) AS SessionMonth,
CONVERT(DATE,SessionDate)AS ShootingDate  
INTO #TotalSessions_MonthWise
from MergeHighRiskPatientTileData
where  CONVERT(DATE,SessionDate) between  @fromDate and @toDate
group by PatientId ,CONVERT(DATE,SessionDate)


--select * from #TotalSessions_MonthWise
--****************************** #BodyScans ***********************************

SELECT COUNT(bsmt.BodyScanMoleId) BodyScanCount,bst.PatientId AS PatientId,
CAST(bst.SessionTimeStamp AS DATE) AS SessionTimeStamp
INTO #BodyScanDetailsTemp	   
FROM [FotoFinder.Universe].[dbo].BodyScanMoleTable bsmt
JOIN [FotoFinder.Universe].[dbo].BodyScanMoleImageTable bsmit ON bsmt.BodyScanMoleImageId=bsmit.BodyScanMoleImageId
JOIN [FotoFinder.Universe].[dbo].BodyMappingSessionImageRelationTable bmsir ON bsmit.ImageId=bmsir.ImageNumber
JOIN [FotoFinder.Universe].[dbo].BodyMappingSessionTable bst ON bst.SessionId=bmsir.SessionId
WHERE bst.Deleted=0 and bsmit.BodyScanTypeId=0 and CONVERT(DATE,SessionTimeStamp) BETWEEN @fromDate AND @toDate
GROUP BY CAST(bst.SessionTimeStamp AS DATE),bst.PatientId 

--select * from #BodyScanDetailsTemp 



SELECT totalSession.PatientId AS PatientId,
(CASE WHEN bsmt.BodyScanCount IS NOT NULL THEN bsmt.BodyScanCount ELSE 0 END) AS BodyScanCount,
CONVERT(CHAR(3),CONVERT(DATE,Shootingdate), 0) AS SessionMonth,
MONTH(Shootingdate)AS MonthNumber,
YEAR(Shootingdate)AS SessionYear
INTO #BodyScans
FROM #BodyScanDetailsTemp bsmt
Right JOIN #TotalSessions_MonthWise totalSession on 
bsmt.PatientId = totalSession.PatientId and  
bsmt.SessionTimeStamp=totalSession.Shootingdate

--Select  *  from #BodyScans
--*****************Get DermoscopeSessionCount ***********************
SELECT SessionMonth,COUNT(BodyScanCount) AS DermoscopeSessionCount ,
SessionYear,MonthNumber
INTO #DermoscopeSession_MonthWise
FROM #BodyScans
WHERE BodyScanCount=0 GROUP BY MonthNumber,SessionYear,SessionMonth ORDER BY SessionMonth
--select * from #DermoscopeSession_MonthWise
----*****************Get BodyMappingSessionCount *********************** 
select SessionMonth,Sum(BodyScanCount)as BodyscanCount into #BodyMappingSession_MonthWise 
from #BodyScans  Group by SessionMonth

--select * from #BodyMappingSession_MonthWise

SELECT 
bodyMapping.SessionMonth  AS SessionMonth,
MonthNumber,
cASe when bodyMapping.BodyscanCount is null then 0 else  BodyscanCount end AS BodyMappingSessionCount,
dermoscope.DermoscopeSessionCount DermoscopeSessionCount
Into #ExaminationTrensOneYearFinalTemp
FROM #BodyMappingSession_MonthWise bodyMapping
INNER JOIN #DermoscopeSession_MonthWise dermoscope 
ON bodyMapping.SessionMonth=dermoscope.SessionMonth
order by SessionYear,MonthNumber

--select * from #ExaminationTrensOneYearFinalTemp

select 
CASe When SessionMonth is null then CONVERT(CHAR(3),(DATENAME( MONTH , DATEADD( MONTH , Mn.Monthnumber , 0 ) - 1 ))) else SessionMonth End AS [Month],
CASe When BodyMappingSessionCount is null then 0  else  BodyMappingSessionCount End AS BodyMappingSessionCount,
CASe When DermoscopeSessionCount is null then 0  else  DermoscopeSessionCount End AS DermoscopeSessionCount 
from #ExaminationTrensOneYearFinalTemp AS Final right join #MonthNumber
AS Mn on Mn.Monthnumber=Final.MonthNumber


DROP TABLE #TotalSessions_MonthWise
DROP TABLE #BodyScanDetailsTemp
DROP TABLE #BodyScans
DROP TABLE #DermoscopeSession_MonthWise
DROP TABLE #BodyMappingSession_MonthWise
DROP TABLE #ExaminationTrensOneYearFinalTemp
 
END
GO