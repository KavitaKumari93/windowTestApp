/****** Object:  StoredProcedure [dbo].[GetImageInfoByPatientId]    Script Date: 11/09/2023 17:55:38 ******/

IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetImageInfoByPatientId]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetImageInfoByPatientId]
    	AS
		begin
			select 1
		end')
END
Go
ALTER PROCEDURE [dbo].[GetImageInfoByPatientId] 
@patientId nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
SELECT i.ObjectID PatientId
       ,i.ImageNumber
       ,i.ImageName
       ,i.ImagePath
	   ,CONCAT(i.ImagePath, '', i.ImageName) AS FullImagePath 
      FROM [FotoFinder.Universe].[dbo].ImagesInfoTable i 
	  Join TrackUploadStatus t on t.ImageNumber=i.ImageNumber
      WHERE i.ObjectID in (select splitdata from SplitString(@patientId,','))
	  AND t.ImageUploadStatus=2 and i.Deleted=0
END
GO