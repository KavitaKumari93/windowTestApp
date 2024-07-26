
/****** Object:  StoredProcedure [dbo].[GetExcisionCount]    Script Date: 09/01/2024 17:45:41 ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetExcisionCount]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetExcisionCount]
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
ALTER PROCEDURE [dbo].[GetExcisionCount]
AS
BEGIN
	SELECT DISTINCT(iit.ObjectID) PatientId, MarkNo, CONVERT (DATE,ISNULL(mrk.CreationTimestamp,iit.Shootingdate))ExcisedDate, MarkText
    FROM [FotoFinder.Universe].[dbo].ImagesInfoTable iit
    LEFT JOIN [FotoFinder.Cockpit].dbo.MasterTable mt ON mt.ObjectId = iit.ObjectID AND mt.Deleted=0  
    INNER JOIN  [FotoFinder.Universe].[dbo].MarkerTable mrk ON mrk.PatientId=iit.ObjectID
    LEFT JOIN [FotoFinder.Universe].[dbo].MarkerStatusAudit MSA ON mrk.MarkNo = MSA.MarkerId   
	WHERE mrk.Deleted = 0 AND mrk.Deactivated = 1 AND mrk.Excised = 1 AND mrk.MarkNo = mrk.ParentMarkNo   
	GROUP BY iit.ObjectID,CONVERT (DATE,ISNULL(mrk.CreationTimestamp,iit.Shootingdate)),Marktext,MarkNo
END
GO