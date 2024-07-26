/****** Object:  StoredProcedure [dbo].[GetMicroImagesTrendsByFiveYear]    Script Date: 3/11/2024 3:10:08 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetMicroImagesTrendsByFiveYear]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetMicroImagesTrendsByFiveYear]
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


ALTER PROCEDURE [dbo].[GetMicroImagesTrendsByFiveYear] 
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
SUM(MicroImageCount) AS MicroImageCount
INTO #sessionTemp from ( SELECT
Distinct(SessionDate) as SessionDate,MicroImageCount
FROM MergeHighRiskPatientTileData WHERE SessionDate BETWEEN  @fromDate  and @toDate 
GROUP BY SessionDate, MicroImageCount) as tbl group by Year(CONVERT(date,SessionDate)) ,DATEPART(qq,CONVERT(date,SessionDate))

select YQ.YearSeries [Year],
YQ.QuarterSeries [Quarter],
CASE WHEN MicroImageCount IS NULL THEN 0  ELSE  MicroImageCount END AS [Count]
FROM #sessionTemp S RIGHT JOIN #YearsQuarterTemp YQ 
On YQ.YearSeries=S.SessionYear AND YQ.QuarterSeries=S.SessionQuarter
order by YQ.YearSeries,YQ.QuarterSeries 
END

GO
