/****** Object:  StoredProcedure [dbo].[GetAvgSessionDurationTrendsByTwentyYear]    Script Date: 4/5/2024 5:15:13 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetAvgSessionDurationTrendsByTwentyYear]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetAvgSessionDurationTrendsByTwentyYear]
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
ALTER PROCEDURE [dbo].[GetAvgSessionDurationTrendsByTwentyYear] 
@fromDate DateTime, @toDate DateTime
AS

BEGIN
	SET NOCOUNT ON;

SET @fromDate = CONVERT(DATE , @fromDate);
SET @toDate = CONVERT(DATE , @toDate);

--****************************** #Years ***********************************
;with  Years AS
        (
        SELECT  DATEPART(YEAR,@fromDate) AS SessionYear
        UNION all
        SELECT  SessionYear + 1
        FROM    Years
        WHERE   SessionYear < DATEPART(YEAR, @toDate)
        )
SELECT  SessionYear Into #YearsSeriesTemp FROM Years
--select * from #YearsSeriesTemp

SELECT YEAR(CONVERT(DATE,SessionDate)) AS SessionYear,Sum(SessionDuration)as SessionDuration, PatientId
INTO #SessionTemp FROM ( SELECT
Distinct(SessionDate) AS SessionDate,SessionDuration, PatientId
FROM MergeHighRiskPatientTileData WHERE SessionDate BETWEEN  @fromDate AND @toDate 
GROUP BY SessionDate, SessionDuration,PatientId) AS tbl GROUP BY YEAR(CONVERT(date,SessionDate)) ,DATEPART(qq,CONVERT(DATE,SessionDate)),PatientId
--select * from #SessionTemp 

Select SessionYear,AVG(SessionDuration) as AvgDuration INTO #Temp from #SessionTemp  where SessionDuration>0 Group by SessionYear
--select * from #Temp

select Y.SessionYear AS [Year],
CASE WHEN AvgDuration IS NULL THEN  0  ELSE  AvgDuration END AS [Count]
FROM #Temp S RIGHT JOIN #YearsSeriesTemp Y on Y.SessionYear=S.SessionYear 
END

Drop Table #SessionTemp
Drop Table #Temp
GO
