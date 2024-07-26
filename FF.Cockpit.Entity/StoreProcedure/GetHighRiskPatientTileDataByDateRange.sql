/****** Object:  StoredProcedure [dbo].[GetHighRiskPatientTileDataByDateRange]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetHighRiskPatientTileDataByDateRange]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetHighRiskPatientTileDataByDateRange]
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
--[GetHighRiskPatientTileDataByDateRange] null,'2023-09-27' WITH RECOMPILE;
ALTER PROCEDURE [dbo].[GetHighRiskPatientTileDataByDateRange] 
@FromDate datetime,
@ToDate datetime
AS 
BEGIN
SET NOCOUNT ON;

SET @FromDate = CONVERT(DATE,ISNull(@FromDate ,(select Min(SessionDate) from MergeHighRiskPatientTileData))); 
SET @ToDate = CONVERT(DATE,@ToDate);

DECLARE @nonZeroSessionCount int;

select 
    PatientId,PatientName,hrp.DateOfBirth,mt.Phone,mt.Mobile,mt.Email
	,SessionDate,BodyScanCount,ExcisionCount,MarkerCount,MicroImageCount,
	SessionDuration, 
    row_number() over (partition by PatientId,PatientName,hrp.DateOfBirth,SessionDate,BodyScanCount,ExcisionCount,MarkerCount,MicroImageCount,
	SessionDuration order by PatientId) as RowNbr  
	Into #tempTable
	From MergeHighRiskPatientTileData hrp
	Left Join MasterTable mt ON mt.ObjectId=hrp.PatientId
	order by PatientId 

--	Select * from #tempTable  

SELECT PatientId
,PatientName
,Phone,Mobile,Email
,CONVERT(DATE,DateOfBirth)As DateOfBirth
,(CASE WHEN SUM(BodyScanCount)>  0 THEN SUM(BodyScanCount)/(SUM(CASE WHEN BodyScanCount > 0 THEN 1 ELSE 0 END)) ELSE 0 END) AS BodyScanCount
,SUM(ExcisionCount) AS ExcisionCount 
,SUM(MarkerCount) AS MarkerCount
,SUM(MicroImageCount) AS MicroImageCount
,(CASE WHEN SUM(SessionDuration) > 0  THEN SUM(SessionDuration) / (SUM(CASE WHEN SessionDuration > 0 THEN 1 ELSE 0 END)) ELSE 0 END) AS AvgSessionDuration
,Count(PatientId) as SessionsCount
FROM #tempTable  
WHERE RowNbr=1 and   SessionDate >= @FromDate AND SessionDate < @ToDate
Group by PatientId,PatientName,DateOfBirth,Phone,Mobile,Email
order by PatientId 

Drop table #tempTable

END
GO
