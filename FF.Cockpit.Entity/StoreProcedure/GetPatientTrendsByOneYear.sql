/****** Object:  StoredProcedure [dbo].[GetPatientTrendsByOneYear]    Script Date: 12/11/2023 7:00:35 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetPatientTrendsByOneYear]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetPatientTrendsByOneYear]
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
--GetPatientTrendsByOneYear '2022-10-11','2023-10-30'
ALTER PROCEDURE [dbo].[GetPatientTrendsByOneYear] 
@fromDate DateTime, @toDate DateTime
AS

BEGIN
SET NOCOUNT ON;

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

SELECT 

CONVERT(CHAR(3),CONVERT(DATE,LAStUpdate), 0) AS PatientMonth,
MONTH(LAStUpdate) AS TrendMonth,
YEAR(LAStUpdate) AS TrendYear,
COUNT(ObjectId) AS PatientCount
INTO #PatientTemp
FROM MasterTable WHERE Deleted=0 AND CONVERT(DATE,LAStUpdate) BETWEEN @fromDate AND @toDate
GROUP BY CONVERT(CHAR(3),CONVERT(DATE,LAStUpdate), 0),MONTH(LAStUpdate) ,Year(LAStUpdate)
ORDER BY YEAR(LAStUpdate),MONTH(LAStUpdate)

select 
CASE WHEN PatientMonth Is Null THEN CONVERT(CHAR(3),(DateName( MONTH , DateAdd( MONTH , Mn.Monthnumber , 0 ) - 1 ))) Else PatientMonth End AS [Month],
CASE WHEN PatientCount Is Null THEN 0  ELSE  PatientCount End AS [Count]
FROM #PatientTemp AS Final right join #MonthNumber 
AS Mn on Mn.Monthnumber=Final.TrendMonth
END

Go
