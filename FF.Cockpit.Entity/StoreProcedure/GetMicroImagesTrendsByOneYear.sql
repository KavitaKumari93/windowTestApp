/****** Object:  StoredProcedure [dbo].[GetMicroImagesTrendsByOneYear]    Script Date: 3/11/2024 3:10:36 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetMicroImagesTrendsByOneYear]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetMicroImagesTrendsByOneYear]
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
ALTER PROCEDURE [dbo].[GetMicroImagesTrendsByOneYear] 
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
SUM(MicroImageCount) as MicroImageCount INTO #SessionTemp FROM ( SELECT
Distinct(SessionDate) as SessionDate,MicroImageCount
FROM MergeHighRiskPatientTileData WHERE SessionDate BETWEEN  @fromDate  and @toDate 
GROUP BY SessionDate, MicroImageCount) AS tbl GROUP BY Month(SessionDate) ,Year(SessionDate)

--select * from #SessionTemp 

select CONVERT(CHAR(3),(DATENAME( MONTH , DATEADD( MONTH ,Mn.Monthnumber , 0 )-1))) AS [Month],
CASE WHEN MicroImageCount IS NULL THEN 0 ELSE MicroImageCount END AS [Count] FROM #MonthNumber as MN LEFT JOIN #SessionTemp AS ST 
on MN.Monthnumber=ST.SessionMonth and  MN.SessionYear=ST.SessionYear
DROP TABLE #SessionTemp

END

GO
