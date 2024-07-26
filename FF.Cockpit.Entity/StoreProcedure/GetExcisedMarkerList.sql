-------------GetExcisedMarkerList
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetExcisedMarkerList]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetExcisedMarkerList]
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
ALTER PROCEDURE [dbo].[GetExcisedMarkerList]  
@patientObjectID int, @selectedFilterDate DateTime  
AS  
BEGIN  
    SELECT Distinct  
        MarkText,  
        CASE   
            WHEN MT.CreationTimestamp IS  NULL THEN CONVERT(Date,MSA.StatusChangeDate)  
            ELSE   
             MT.CreationTimestamp  
               
             --MT.CreationTimestamp IS NULL AND MSA.StatusChangeDate IS NOT NULL THEN MSA.StatusChangeDate  
            --WHEN MT.CreationTimestamp IS NULL AND MSA.StatusChangeDate IS NULL THEN 'N/A'  
              
        END AS DateOfExcision  
    FROM   
        [FotoFinder.Universe].[dbo].MarkerTable MT  
        lEFT JOIN [FotoFinder.Universe].[dbo].MarkerStatusAudit MSA ON MT.MarkNo = MSA.MarkerId   
    WHERE   
        MT.Deleted = 0   
        AND MT.Deactivated = 1   
        AND MT.Excised = 1   
        AND MT.PatientId = @patientObjectID   
        AND MT.MarkNo = MT.ParentMarkNo  
        AND  (@selectedFilterDate IS NULL OR CONVERT(DATE,MT.CreationTimestamp) >= @selectedFilterDate)  
END  
Go
