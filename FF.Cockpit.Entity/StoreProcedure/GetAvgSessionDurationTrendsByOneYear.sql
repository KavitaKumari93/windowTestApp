/****** Object:  StoredProcedure [dbo].[GetAvgSessionDurationTrendsByOneYear]    Script Date: 4/5/2024 5:14:16 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetAvgSessionDurationTrendsByOneYear]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetAvgSessionDurationTrendsByOneYear]
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
ALTER PROCEDURE [dbo].[GetAvgSessionDurationTrendsByOneYear] 
@fromDate DateTime, @toDate DateTime
AS

BEGIN
	SET NOCOUNT ON;

SET @fromDate = CONVERT(DATE , @fromDate);
SET @toDate = CONVERT(DATE , @toDate);

;WITH Months (DATE)
AS
(
   SELECT DATEADD(DAY,1,EOMONTH(@fromDate,-1))
    UNION ALL
    SELECT DATEADD(MONTH,1,date)
    FROM months
    WHERE DATEADD(MONTH,1,date) < EOMONTH(@toDate)
)
SELECT DATEPART(MONTH,DATE)AS Monthnumber, DATEPART(Year,DATE)AS SessionYear INTO #MonthNumber
FROM months
--select * from #MonthNumber


Select  MONTH(SessionDate)  AS SessionMonth,
YEAR(SessionDate) as SessionYear,
SUM(SessionDuration) as SessionDuration, PatientId INTO #SessionTemp FROM ( SELECT 
Distinct(SessionDate) as SessionDate,SessionDuration, PatientId
FROM MergeHighRiskPatientTileData WHERE SessionDate BETWEEN  @fromDate  and @toDate 
GROUP BY SessionDate, SessionDuration,PatientId) AS tbl GROUP BY Month(SessionDate) ,Year(SessionDate),PatientId
--select * from #SessionTemp 

Select SessionMonth,SessionYear,AVG(SessionDuration) as AvgDuration INTO #Temp from #SessionTemp  where SessionDuration>0 Group by SessionYear,SessionMonth

--select * from #Temp 

select CONVERT(CHAR(3),(DATENAME( MONTH , DATEADD( MONTH ,Mn.Monthnumber , 0 )-1))) AS [Month],
CASE WHEN AvgDuration IS NULL THEN 0 ELSE AvgDuration END AS [Count] FROM #MonthNumber as MN LEFT JOIN #Temp AS ST 
on MN.Monthnumber=ST.SessionMonth and  MN.SessionYear=ST.SessionYear

DROP TABLE #Temp
DROP TABLE #SessionTemp

END
GO

