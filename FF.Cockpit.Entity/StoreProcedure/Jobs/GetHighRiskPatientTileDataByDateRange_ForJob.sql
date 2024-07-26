/****** Object:  StoredProcedure [dbo].[GetHighRiskPatientTileDataByDateRange_ForJob]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetHighRiskPatientTileDataByDateRange_ForJob]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetHighRiskPatientTileDataByDateRange_ForJob]
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
--exec [GetHighRiskPatientTileDataByDateRange_ForJob];
ALTER PROCEDURE [dbo].[GetHighRiskPatientTileDataByDateRange_ForJob] 
AS
BEGIN
    SET NOCOUNT ON;
    --Get Patients list in given date range--
    SELECT distinct (iit.ObjectID) AS PatientId,
         CONCAT(FirstName,' ',MiddleName,' ', LAStname)  AS PatientName,
         CONVERT(varchar, DateOfBirth, 104) AS DOB,
         CONVERT(DATE, DateOfBirth)DateOfBirth,
         'Patient came for follow up.One new lesion was marked follow up.' AS Comment
    INTO #patienTemp
    FROM [FotoFinder.Universe].[dbo].ImagesInfoTable iit
    INNER JOIN [FotoFinder.Cockpit].dbo.MasterTable mast ON mast.ObjectId = iit.ObjectID AND mast.Deleted=0
    WHERE iit.Deleted=0 
    ORDER BY CONVERT(DATE, DateOfBirth) desc
 
 
    ------Get Patients BodyScan Count
    select *  
        INTO #BodyScanCountTemp
        FROM 
        (
            SELECT bst.PatientId AS PatientId ,pt.PatientName,pt.DateOfBirth,
            Count(bsmt.BodyScanMoleId) BodyScanCount,
            CAST(bst.SessionTimeStamp AS DATE) As SessionDate
            FROM [FotoFinder.Universe].[dbo].BodyScanMoleTable bsmt
            join [FotoFinder.Universe].[dbo].BodyScanMoleImageTable bsmit on bsmt.BodyScanMoleImageId=bsmit.BodyScanMoleImageId
            join [FotoFinder.Universe].[dbo].BodyMappingSessionImageRelationTable bmsir on bsmit.ImageId=bmsir.ImageNumber
            join [FotoFinder.Universe].[dbo].BodyMappingSessionTable bst on bst.SessionId=bmsir.SessionId
            join #patienTemp pt on pt.PatientId=bst.PatientId
            WHERE
            --bsmt.IsValid=1 
            bst.Deleted=0 and bsmit.BodyScanTypeId=0
            GROUP BY  CAST(bst.SessionTimeStamp AS DATE),
            bst.PatientId,PatientName,DateOfBirth
        ) tbl
    group by PatientId,PatientName,DateOfBirth, SessionDate ,BodyScanCount
 
-- select * from #BodyScanCountTemp where PatientId=1
 
  SELECT  PatientId,shootingdate as SessionDate,Flag INTO #SessionCountTemp 
FROM
  (SELECT DISTINCT CONVERT(DATE, shootingdate) AS shootingdate,
                   patient.patientid AS PatientId,1 As Flag
   FROM #patienTemp AS patient
   INNER JOIN [FotoFinder.Universe].[dbo].ImagesInfoTable AS iit ON iit.ObjectID = patient.PatientId
   WHERE Shootingdate IS NOT NULL
     AND Deleted = 0
   UNION SELECT DISTINCT CONVERT(DATE, creationtimestamp) AS shootingdate,
         patient.patientid AS PatientId,0 As Flag
   FROM #patienTemp AS patient
   INNER JOIN [FotoFinder.Universe].[dbo].MarkerTable AS mt ON mt.PatientId = patient.PatientId
   WHERE creationtimestamp IS NOT NULL
     AND Deleted = 0
    ) AS SessionData
GROUP BY patientId , shootingdate,Flag;
 
--select * from #SessionCountTemp  where PatientId=1
 
  select count(*) as MarkerCount,
  PatientId,
  CONVERT (date,CreationTimestamp)as CreationTimestamp
  INTO #MarkedFollowUpCountTemp 
  from  (SELECT mt.PatientId,
  CASE when MT.CreationTimestamp IS NULL then (SELECT  
  top 1 CONVERT (date,inf.Shootingdate)
  FROM [FotoFinder.Universe].[dbo].ImagesInfoTable inf where inf.MedPoint=MT.MarkNo and inf.ObjectID=mt.PatientId order by ImageNumber) ELSE mt.CreationTimestamp
  END AS CreationTimestamp
  FROM [FotoFinder.Universe].[dbo].MarkerTable mt 
  WHERE  mt.Deleted=0 and mt.MarkNo=mt.ParentMarkNo 
  and mt.IsUserApproved=1
  AND 1=CASE 
  WHEN MT.CreationTimestamp is NOT NULL THEN 
  CASE WHEN  
  CONVERT(date, MT.CreationTimestamp) >= CONVERT(date,(select Min(Shootingdate) from [FotoFinder.Universe].[dbo].ImagesInfoTable )) AND CONVERT(date, MT.CreationTimestamp)< CONVERT(date,GETDATE() )  Then 1  
  ELSE 0
  END
  ELSE 
   CASE WHEN (SELECT  
     top 1 CONVERT (date,inf.Shootingdate)
     FROM [FotoFinder.Universe].[dbo].ImagesInfoTable inf where inf.MedPoint=MT.MarkNo 
     and inf.ObjectID=mt.PatientId order by ImageNumber)
>= CONVERT(date,(select Min(Shootingdate) from [FotoFinder.Universe].[dbo].ImagesInfotable ))  AND (SELECT  
     top 1 CONVERT (date,inf.Shootingdate)
     FROM [FotoFinder.Universe].[dbo].ImagesInfoTable inf where inf.MedPoint=MT.MarkNo and inf.ObjectID=mt.PatientId order by ImageNumber)
< CONVERT(DATE,GetDate()) then 1
      ELSE 0 
      END 
        END 
        GROUP BY MT.CreationTimestamp,mt.PatientId,Mt.MarkNo)tbl  
        GROUP BY PatientId,CONVERT (date,CreationTimestamp)
 
  --select  * from #MarkedFollowUpCountTemp  where PatientId=19
 
   --Get the count for MicroImages
    SELECT COUNT(iit.ImageNumber) AS MicroImageCount,
    patient.PatientId,
    CONVERT (date,iit.Shootingdate)as Shootingdate
    INTO #MicroImageCountTemp1
    FROM #patienTemp AS patient
    inner JOIN [FotoFinder.Universe].[dbo].ImagesInfotable iit ON iit.ObjectID=patient.PatientId 
    WHERE  iit.Deleted=0 AND iit.MedLevel=9 
    GROUP BY patient.PatientId,iit.Shootingdate,PatientName,CONVERT(DATE, DateOfBirth)
 
select PatientId,Sum(MicroImageCount) as MicroImageCount  , Shootingdate 
into #MicroImageCountTemp
from #MicroImageCountTemp1 group by Shootingdate,PatientId
 
--select * from #MicroImageCountTemp where PatientId=1
 
 
   --Get the count for Excision 
 
   SELECT  MarkNo,Marktext,
     patient.PatientId , CONVERT (date,isnull(MT.CreationTimestamp,iit.Shootingdate))ExcisedDate
     Into #ExcisionCountTemp1
     FROM #patienTemp AS patient
     INNER JOIN  [FotoFinder.Universe].[dbo].MarkerTable MT on MT.PatientId=patient.PatientId
     LEFT JOIN [FotoFinder.Universe].[dbo].MarkerStatusAudit MSA ON MT.MarkNo = MSA.MarkerId 
      INNER JOIN  [FotoFinder.Universe].[dbo].ImagesInfoTable iit ON iit.ObjectID=MT.PatientId 
    WHERE 
        MT.Deleted = 0 
        AND MT.Deactivated = 1 
        AND MT.Excised = 1 
        AND MT.MarkNo = MT.ParentMarkNo 
    GROUP BY patient.PatientId,CONVERT (date,isnull(MT.CreationTimestamp,iit.Shootingdate)),Marktext,MarkNo
	
	Select ( count(distinct MarkNo)) as ExcisionCount, PatientId,ExcisedDate into #ExcisionCountTemp  from #ExcisionCountTemp1
	Group By ExcisedDate,PatientId
 
--select * from  #ExcisionCountTemp1 
 
 
--******* Session Duration **********************
 
  SELECT SessionTime  AS SessionDuration,PatientId,Shootingdate  INTO #TotalDurationTemp  FROM 
         ( 
                SELECT (DATEDIFF(MINUTE, min(Shootingdate),max(Shootingdate))) AS SessionTime
                ,CONVERT(date,Shootingdate)As ShootingDate,
                patient.PatientId AS PatientId
                 FROM #patienTemp AS patient INNER JOIN [FotoFinder.Universe].[dbo].ImagesInfoTable AS iit on iit.ObjectID=patient.PatientId
                 WHERE ObjectID=patient.PatientId AND Deleted=0 
                GROUP BY CONVERT(date,Shootingdate),patient.PatientId
           )  AS avgtime 
           where SESSIONTIME > 3 GROUP BY PatientId,CONVERT(date,Shootingdate),SessionTime
 
--select * from #TotalDurationTemp where PatientId=1
 --*****************TBM IMAGES*************************
 SELECT cast(iit.Shootingdate as date) as Shootingdate, count(*) as TBMImagesCount,PatientId
   INTO #TBMImagesCountTemp
   FROM #patienTemp AS patient
   inner JOIN [FotoFinder.Universe].[dbo].ImagesInfotable iit ON iit.ObjectID=patient.PatientId 
   WHERE  iit.Deleted=0 AND iit.MedLevel<>9 
   GROUP BY patient.PatientId, cast(iit.Shootingdate as date) 
ORDER BY cast(iit.Shootingdate as date);

--Final DaTA
 
SELECT
    ISNull(sct.PatientId, bct.PatientId) AS PatientId,
    ISNull(pt.PatientName, bct.PatientName) AS PatientName,
    ISNull(CONVERT(DATE, pt.DateOfBirth), CONVERT(DATE, bct.DateOfBirth)) AS DateOfBirth,
    ISNull(sct.SessionDate, bct.SessionDate) AS SessionDate,
    ISNull(bct.BodyScanCount, 0) AS BodyScanCount ,
    ISNull(mfu.MarkerCount, 0) AS MarkerCount ,
    ISNull(mic.MicroImageCount, 0) AS MicroImageCount ,
    ISNull(ect.ExcisionCount, 0) AS ExcisionCount ,
    ISNull(tdt.SessionDuration, 0) AS SessionDuration ,
	ISNull(tbm.TBMImagesCount, 0) AS TBMImagesCount ,
	sct.Flag
    INTO #FinalTempTbl
FROM #patienTemp as pt inner join
    #SessionCountTemp sct on pt.PatientId=sct.PatientId
    Full OUTER JOIN 
    #BodyScanCountTemp bct ON sct.SessionDate = bct.SessionDate and sct.PatientId = bct.PatientId
        Full OUTER JOIN  
    #MarkedFollowUpCountTemp mfu ON sct.SessionDate = mfu.CreationTimestamp and sct.PatientId = mfu.PatientId
        Full OUTER JOIN 
    #MicroImageCountTemp mic ON sct.SessionDate = mic.Shootingdate and sct.PatientId = mic.PatientId
        Full OUTER JOIN 
    #ExcisionCountTemp ect on sct.SessionDate=ect.ExcisedDate and sct.PatientId=ect.PatientId
        Full OUTER JOIN 
    #TotalDurationTemp tdt on sct.SessionDate=tdt.ShootingDate and sct.PatientId=tdt.PatientId
       left JOIN
	#TBMImagesCountTemp tbm on cast(sct.SessionDate as date) =cast( tbm.ShootingDate as date) and sct.PatientId=tbm.PatientId

    select * from #FinalTempTbl 
    where PatientId is not null 
    group by
    PatientId,PatientName,
    CONVERT(DATE, DateOfBirth),
    SessionDate,BodyScanCount,MarkerCount,
    MicroImageCount,
    ExcisionCount,SessionDuration,Flag,TBMImagesCount
    order by PatientId
 
    DROP TABLE  #FinalTempTbl
    DROP TABLE #PatienTemp
    DROP TABLE #BodyScanCountTemp
    DROP TABLE #MarkedFollowUpCountTemp
    DROP TABLE #MicroImageCountTemp
    DROP TABLE #ExcisionCountTemp
    DROP TABLE #TotalDurationTemp
    DROP TABLE #SessionCountTemp
	DROP TABLE #TBMImagesCountTemp
END
GO
