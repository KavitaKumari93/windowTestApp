/****** Object:  StoredProcedure [dbo].[GetDashboardTilesDataByPatientId]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetDashboardTilesDataByPatientId]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetDashboardTilesDataByPatientId]
    	AS
		begin
			select 1
		end')
END
Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--GetDashboardTilesDataByPatientId 146207,null
--GetDashboardTilesDataByPatientId,null
--GetDashboardTilesDataByPatientId,null
--GetDashboardTilesDataByPatientId,null-
ALTER PROCEDURE [dbo].GetDashboardTilesDataByPatientId   
     @patientObjectID int, @selectedFilterDate DateTime  
AS  
BEGIN  
    SET NOCOUNT ON;  
        SET @selectedFilterDate = CONVERT(DATE , @selectedFilterDate);  
    
	select 
    PatientId,PatientName, CONVERT(DATE , DateOfBirth)as DateOfBirth ,
	SessionDate,MicroImageCount,BodyScanCount,ExcisionCount,MarkerCount,
	SessionDuration,
    row_number() over (partition by PatientId,PatientName,DateOfBirth,SessionDate,BodyScanCount,ExcisionCount,MarkerCount,MicroImageCount,
	 SessionDuration order by PatientId) as RowNbr  
	Into #tempTable
	From MergeHighRiskPatientTileData where  PatientId=@patientObjectID order by  SessionDate desc

	--Select * from #tempTable  where PatientId= 19

	Declare @avgExaminationTime int=0;
 SELECT 
 temp.PatientId
,PatientName as PatientName
,CONVERT(DATE, mt.DateOfBirth) As DOB
,mt.PatientRecordnumber as RecordNumber
,mt.SkinTypeId
,'Undefined' as [Description] 
,IsNull(CONVERT(DATE,tbl2.StartTime),null) as AppointmentStartDate
,IsNull(CONVERT(DATE,tbl2.EndTime),null) as AppointmentEndDate
,SUM(ExcisionCount) AS ExcisionCount 
,SUM(MicroImageCount) AS MicroImagesCount
,Count(temp.PatientId) as SessionCount
,(CASE WHEN SUM(BodyScanCount)>  0 THEN SUM(BodyScanCount)/(SUM(CASE WHEN BodyScanCount > 0 THEN 1 ELSE 0 END)) ELSE 0 END) AS LesionsCount
,  CAST((CASE WHEN SUM(SessionDuration) > 0  THEN SUM(SessionDuration) / (SUM(CASE WHEN SessionDuration > 0 THEN 1 ELSE 0 END)) ELSE 0 END) AS varchar)+ ' m '  as ExaminationTime        
          

FROM #tempTable  as temp
inner Join MasterTable as mt on temp.PatientId =mt.ObjectId
Full Outer Join
(Select top 1 PatientId,StartTime,EndTime  from [dbo].[Appointments]  where PatientId=@patientObjectID ORDER BY EndTime desc ) as tbl2 on tbl2.PatientId=mt.ObjectId
WHERE RowNbr=1 
Group by temp.PatientId,temp.PatientName,mt.DateOfBirth,mt.PatientRecordnumber,mt.SkinTypeId
,tbl2.StartTime,tbl2.EndTime
order by temp.PatientId
Drop table #tempTable
END
GO
