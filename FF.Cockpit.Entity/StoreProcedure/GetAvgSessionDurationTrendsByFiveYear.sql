
/****** Object:  StoredProcedure [dbo].[GetAvgSessionDurationTrendsByFiveYear]    Script Date: 4/5/2024 5:21:18 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetAvgSessionDurationTrendsByFiveYear]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetAvgSessionDurationTrendsByFiveYear]
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
ALTER PROCEDURE [dbo].[GetAvgSessionDurationTrendsByFiveYear] 
@fromDate DateTime, @toDate DateTime
AS

BEGIN
	SET NOCOUNT ON;

SET @fromDate = CONVERT(DATE , @fromDate);
SET @toDate = CONVERT(DATE , @toDate);

----****************************** #YearsQuarterList ***********************************
;with  YearsQuarter AS
        (select distinct year(dateadd(day,rnum,@fromDate)) YearSeries,
          datepart(quarter,dateadd(day,rnum,@fromDate)) QuarterSeries
           from (select row_number() over(order by (select null)) as rnum 
           from master..spt_values) t
            where dateadd(day,rnum,@fromDate) <= @toDate)
           
Select  * Into #YearsQuarterTemp From   YearsQuarter

SELECT Year(CONVERT(date,SessionDate)) AS SessionYear,
DATEPART(qq,CONVERT(date,SessionDate)) AS SessionQuarter,
SUM(SessionDuration) AS SessionDuration,PatientId
INTO #sessionTemp from ( SELECT
Distinct(SessionDate) as SessionDate,SessionDuration,PatientId
FROM MergeHighRiskPatientTileData WHERE SessionDate BETWEEN  @fromDate  and @toDate 
GROUP BY SessionDate, SessionDuration,PatientId) as tbl group by Year(CONVERT(date,SessionDate)) ,DATEPART(qq,CONVERT(date,SessionDate)),PatientId


Select SessionYear,SessionQuarter,AVG(SessionDuration) as AvgDuration INTO #Temp from #SessionTemp  where SessionDuration>0 Group by SessionYear,SessionQuarter


select YQ.YearSeries [Year],
YQ.QuarterSeries [Quarter],
CASE WHEN AvgDuration IS NULL THEN 0  ELSE  AvgDuration END AS [Count]
FROM #Temp S RIGHT JOIN #YearsQuarterTemp YQ 
On YQ.YearSeries=S.SessionYear AND YQ.QuarterSeries=S.SessionQuarter
order by YQ.YearSeries,YQ.QuarterSeries 
END
GO
