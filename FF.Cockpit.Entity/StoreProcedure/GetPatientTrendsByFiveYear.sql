
/****** Object:  StoredProcedure [dbo].[GetPatientTrendsByFiveYear]    Script Date: 3/11/2024 3:03:35 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetPatientTrendsByFiveYear]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetPatientTrendsByFiveYear]
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
--GetPatientTrendsByFiveYear '2018-09-01','2023-08-30'
ALTER PROCEDURE [dbo].[GetPatientTrendsByFiveYear] 
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
                                        
SELECT Year(CONVERT(date,LAStUpdate)) AS PatientYear,
DATEPART(qq,CONVERT(date,LAStUpdate)) AS PatientQuarter,
Count(ObjectId) AS PatientCount
INTO #PatientTemp
FROM MasterTable WHERE Deleted=0 AND CONVERT(DATE,LAStUpdate) BETWEEN @fromDate AND @toDate
GROUP BY Year(CONVERT(date,LAStUpdate)),DATEPART(qq,CONVERT(date,LAStUpdate)) 
ORDER BY Year(CONVERT(date,LAStUpdate)),DATEPART(qq,CONVERT(date,LAStUpdate))
 
select YQ.YearSeries AS [Year],
YQ.QuarterSeries AS [Quarter],
CASE WHEN PatientCount IS NULL THEN 0  ELSE  PatientCount END AS [Count]
FROM #PatientTemp P RIGHT JOIN #YearsQuarterTemp YQ 
On YQ.YearSeries=P.PatientYear AND YQ.QuarterSeries=P.PatientQuarter
order by YQ.YearSeries,YQ.QuarterSeries 
END

GO