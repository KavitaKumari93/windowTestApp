USE [FotoFinder.Universe]
GO
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetFollowUpSessionTileDataByPatientId]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetFollowUpSessionTileDataByPatientId]
    	as
		begin
			select 1
		end')
END

GO
/****** Object:  StoredProcedure [dbo].[GetFollowUpSessionTileDataByPatientId]    Script Date: 08-05-2023 19:06:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- GetFollowUpSessionTileDataByPatientId 5053 ,null
ALTER PROCEDURE [dbo].[GetFollowUpSessionTileDataByPatientId] 
@PatientId int,@selectedFilterDate DateTime
AS

BEGIN  
SET NOCOUNT ON;  
        SET @selectedFilterDate = CONVERT(DATE,ISNull(@selectedFilterDate ,(select Min(SessionDate) from MergeHighRiskPatientTileData))); 
    

  
    select 
    PatientId,SessionDate,MicroImageCount,
	SessionDuration, TBMImagesCount as TBMImageCount,
    row_number() over (partition by PatientId,PatientName,DateOfBirth,SessionDate,BodyScanCount,ExcisionCount,MarkerCount,MicroImageCount,
	 SessionDuration order by PatientId) as RowNbr  
	Into #tempTable
	From MergeHighRiskPatientTileData where  PatientId=@PatientId order by  SessionDate desc

	--Select * from #tempTable  
   SELECT 
   'Session ' + CAST(ROW_NUMBER() OVER (ORDER BY CONVERT(date,SessionDate)) AS VARCHAR(500)) AS SessionName ,
    PatientId,
	CONVERT(date,SessionDate) SessionDate,
	SessionDuration,
	MicroImageCount,
	TBMImageCount
	--Into #tempTable
	From #tempTable where PatientId=@PatientId  and RowNbr=1
	and SessionDate>=@selectedFilterDate
	order by SessionDate desc ;

	--Select * from #tempTable 
END
GO
