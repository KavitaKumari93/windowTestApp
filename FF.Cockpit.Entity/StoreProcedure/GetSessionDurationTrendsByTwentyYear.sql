/****** Object:  StoredProcedure [dbo].[GetSessionDurationTrendsByTwentyYear]    Script Date: 3/11/2024 3:08:03 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetSessionDurationTrendsByTwentyYear]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetSessionDurationTrendsByTwentyYear]
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
--GetSessionDurationTrendsByTwentyYear '2005-01-01','2024-03-01'
ALTER PROCEDURE [dbo].[GetSessionDurationTrendsByTwentyYear] 
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

SELECT YEAR(CONVERT(DATE,SessionDate)) AS SessionYear,SUM(SessionDuration) AS SessionDurationCount
INTO #SessionTemp FROM ( SELECT
Distinct(SessionDate) AS SessionDate,SessionDuration
FROM MergeHighRiskPatientTileData WHERE SessionDate BETWEEN  @fromDate AND @toDate 
GROUP BY SessionDate, SessionDuration) AS tbl GROUP BY YEAR(CONVERT(date,SessionDate))

select Y.SessionYear AS [Year],
CASE WHEN SessionDurationCount IS NULL THEN  0  ELSE  SessionDurationCount END AS [Count]
FROM #SessionTemp S RIGHT JOIN #YearsSeriesTemp Y on Y.SessionYear=S.SessionYear
END
GO
