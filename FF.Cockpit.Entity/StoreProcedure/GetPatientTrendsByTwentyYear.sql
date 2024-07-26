/****** Object:  StoredProcedure [dbo].[GetPatientTrendsByTwentyYear]    Script Date: 3/11/2024 3:04:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetPatientTrendsByTwentyYear]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetPatientTrendsByTwentyYear]
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
ALTER PROCEDURE [dbo].[GetPatientTrendsByTwentyYear] 
@fromDate DateTime, @toDate DateTime
AS

BEGIN
	SET NOCOUNT ON;

SET @fromDate = CONVERT(DATE , @fromDate);
SET @toDate = CONVERT(DATE , @toDate);

--****************************** #Years ***********************************
;with  Years AS
        (
        select  DATEPART(YEAR,@fromDate) AS SessionYear
        union all
        select  SessionYear + 1
        from    Years
        where   SessionYear < DATEPART(YEAR, @toDate)
        )
select  SessionYear Into #YearsSeriesTemp from Years

SELECT YEAR(CONVERT(date,MT.LAStUpdate)) AS PatientYear,COUNT(ObjectId) AS PatientCount
into #PatientTemp
FROM MasterTable MT
WHERE Deleted=0 AND CONVERT(DATE,LAStUpdate) between @fromDate AND @toDate
GROUP BY YEAR(CONVERT(date,MT.LAStUpdate)) ORDER BY YEAR(CONVERT(date,MT.LAStUpdate))


select SessionYear AS [Year],
CASe When PatientCount is null then 0  else  PatientCount END AS [Count]
from #PatientTemp P right join #YearsSeriesTemp Y on Y.SessionYear=P.PatientYear
END
Go

