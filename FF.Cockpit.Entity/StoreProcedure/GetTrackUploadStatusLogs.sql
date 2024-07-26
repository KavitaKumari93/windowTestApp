/****** Object:  StoredProcedure [dbo].[GetTrackUploadStatusLogs]    Script Date: 9/20/2023 2:44:12 PM ******/

IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetTrackUploadStatusLogs]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetTrackUploadStatusLogs]
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
ALTER PROCEDURE [dbo].[GetTrackUploadStatusLogs]
@patientId int  
AS  
BEGIN    
SET NOCOUNT ON;
    SELECT 
        i.ObjectID AS PatientId, 
        CONCAT(mt.FirstName,' ', mt.LAStName) PatientName,
        t.MachineName,
        t.MacAddress,
        t.UploadTime,
        i.ImageNumber, 
        i.ImageName,
        m.Status AS ImageUploadStatus,
        NULL AS IconName,
        NULL AS IconUploadStatus, 
        NULL AS VideoId, 
        NULL AS VideoName,
        NULL AS VideoUploadStatus,
        'Image' AS FileType
    FROM MasterTable mt
    INNER JOIN TrackUploadStatus t ON t.PatientId = mt.ObjectId
    INNER JOIN [FotoFinder.Universe].[dbo].ImagesInfoTable i ON i.ImageNumber = t.ImageNumber 
    INNER JOIN MasterSyncStatus m ON m.Id = t.ImageUploadStatus
    WHERE mt.ObjectId = @patientId
 
    UNION ALL
 
    SELECT 
        i.ObjectID AS PatientId, 
        CONCAT(mt.FirstName,' ', mt.LAStName) PatientName,
        t.MachineName,
        t.MacAddress,
        t.UploadTime,
        i.ImageNumber, 
        NULL AS ImageName,
        NULL AS ImageUploadStatus, 
        i.IconName,
        m.Status AS IconUploadStatus,
        NULL AS VideoId, 
        NULL AS VideoName,
        NULL AS VideoUploadStatus,
        'Icon' AS FileType
    FROM MasterTable mt
    INNER JOIN TrackUploadStatus t ON t.PatientId = mt.ObjectId
    INNER JOIN [FotoFinder.Universe].[dbo].ImagesInfoTable i ON i.ImageNumber = t.ImageNumber 
    INNER JOIN MasterSyncStatus m ON m.Id = t.IconUploadStatus
    WHERE mt.ObjectId = @patientId
 
    UNION ALL
 
    SELECT 
        vd.PatientId AS PatientId, 
        CONCAT(mt.FirstName,' ', mt.LAStName) PatientName,
        vd.MachineName,
        vd.MacAddress,
        vd.UploadTime,
        NULL AS ImageNumber, 
        NULL AS ImageName,
        NULL AS ImageUploadStatus, 
        NULL AS IconName,
        NULL AS IconUploadStatus, 
        vd.VideoId AS VideoId, 
        v.FileName AS VideoName,
        m.Status AS VideoUploadStatus,
        'Video' AS FileType
    FROM MasterTable mt
    INNER JOIN TrackVideoUploadStatus vd on vd.PatientId = mt.ObjectId
    INNER JOIN [FotoFinder.Universe].[dbo].VideoInfoTable v ON v.VideoId = vd.VideoId
    INNER JOIN MasterSyncStatus m ON m.Id = vd.VideoUploadStatus
    WHERE vd.PatientId = @patientId
END
GO
