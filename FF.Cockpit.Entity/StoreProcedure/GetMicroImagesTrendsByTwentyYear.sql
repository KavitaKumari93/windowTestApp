/****** Object:  StoredProcedure [dbo].[GetMicroImagesTrendsByTwentyYear]    Script Date: 3/11/2024 3:13:31 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetMicroImagesTrendsByTwentyYear]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetMicroImagesTrendsByTwentyYear]
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
--GetMicroImagesTrendsByTwentyYear '2005-04-01','2024-12-31'
ALTER PROCEDURE [dbo].[GetMicroImagesTrendsByTwentyYear] 
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

SELECT YEAR(CONVERT(DATE,SessionDate)) AS SessionYear,SUM(MicroImageCount) AS MicroImageCount
INTO #SessionTemp FROM ( SELECT
Distinct(SessionDate) AS SessionDate,MicroImageCount
FROM MergeHighRiskPatientTileData WHERE SessionDate BETWEEN  @fromDate AND @toDate 
GROUP BY SessionDate, MicroImageCount) AS tbl GROUP BY YEAR(CONVERT(date,SessionDate))


select Y.SessionYear AS [Year],
CASE WHEN MicroImageCount IS NULL THEN  0  ELSE  MicroImageCount END AS [Count]
FROM #SessionTemp S RIGHT JOIN #YearsSeriesTemp Y on Y.SessionYear=S.SessionYear
END
GO