USE [FotoFinder.Universe]
GO
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetExaminationHistoryDataByPatientId]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetExaminationHistoryDataByPatientId]
    	as
		begin
			select 1
		end')
END
Go
/****** Object:  StoredProcedure [dbo].[GetExaminationHistoryDataByPatientId]    Script Date: 09-05-2023 09:12:27 ******/
SET ANSI_NULLS ON
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetExaminationHistoryDataByPatientId]   
@PatientId int,@selectedFilterDate DateTime 
AS  
BEGIN  
SET NOCOUNT ON;  
        SET @selectedFilterDate = CONVERT(DATE,ISNull(@selectedFilterDate ,(select Min(SessionDate) from MergeHighRiskPatientTileData))); 
    

  
    select 
    PatientId,PatientName,DateOfBirth,SessionDate,BodyScanCount,ExcisionCount,MarkerCount,MicroImageCount,
	SessionDuration, TBMImagesCount as TBMImageCount,
    row_number() over (partition by PatientId,PatientName,DateOfBirth,SessionDate,BodyScanCount,ExcisionCount,MarkerCount,MicroImageCount,
	TBMImagesCount,SessionDuration order by PatientId) as RowNbr  
	Into #tempTable
	From MergeHighRiskPatientTileData where  PatientId=@PatientId order by  SessionDate desc

	--Select * from #tempTable  
   SELECT 
   'Session ' + CAST(ROW_NUMBER() OVER (ORDER BY CONVERT(date,SessionDate)) AS VARCHAR(500)) AS SessionNumber ,
    PatientId,
	CONVERT(date,SessionDate) SessionDate,
	SessionDuration,
	BodyScanCount,
	CASE When LAG(BodyScanCount, 1) OVER (ORDER BY PatientId) is null Then 0
	ELSE CASE When BodyScanCount<>LAG(BodyScanCount, 1) OVER (ORDER BY PatientId) Then 1 ELSE 0 End 
	END AS IsBodyScanCountChange ,
	ExcisionCount,
	CASE When LAG(ExcisionCount, 1) OVER (ORDER BY PatientId) is null Then 0
	ELSE Case When ExcisionCount<>LAG(ExcisionCount, 1) OVER (ORDER BY PatientId) Then 1 ELSE 0 End 
	END AS IsExcisionCountChange ,
	MarkerCount,
	CASE When LAG(MarkerCount, 1) OVER (ORDER BY PatientId) is null Then 0
	ELSE CASE When MarkerCount<>LAG(MarkerCount, 1) OVER (ORDER BY PatientId) Then 1 ELSE 0 End 
	END AS IsMarkerCountChange ,
	MicroImageCount,
	CASE When LAG(MicroImageCount, 1) OVER (ORDER BY PatientId) is null Then 0
	ELSE CASE When MicroImageCount<>LAG(MicroImageCount, 1) OVER (ORDER BY PatientId) Then 1 ELSE 0 End 
	END AS IsMicroImageCountChange ,
	TBMImageCount,
	CASE When LAG(TBMImageCount, 1) OVER (ORDER BY PatientId) is null Then 0
	ELSE CASE When TBMImageCount<>LAG(TBMImageCount, 1) OVER (ORDER BY PatientId) Then 1 ELSE 0 End 
	END AS IsTBMImageCountChange 
	--Into #tempTable
	From #tempTable where PatientId=@PatientId  and RowNbr=1
	and SessionDate>=@selectedFilterDate
	order by SessionDate desc ;

	--Select * from #tempTable 
END  
GO

