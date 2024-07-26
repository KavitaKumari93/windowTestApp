
/****** Object:  StoredProcedure [dbo].[GetTrackDownloadStatusLogs]    Script Date: 1/30/2024 5:48:53 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetTrackDownloadStatusLogs]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetTrackDownloadStatusLogs]
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
ALTER PROCEDURE [dbo].[GetTrackDownloadStatusLogs]
@patientId int  
AS  
BEGIN    
SET NOCOUNT ON;
	SELECT 
		i.ObjectID AS PatientId, 
		CONCAT(mt.FirstName,' ', mt.LAStName) PatientName,
		t.MachineName,
		t.MacAddress,
		t.DownloadTime,
		i.ImageNumber, 
		i.ImageName,
		m.Status AS ImageDownloadStatus,
		NULL AS IconName,
		NULL AS IconDownloadStatus, 
		NULL AS VideoId, 
		NULL AS VideoName,
		NULL AS VideoDownloadStatus,
		'Image' AS FileType
	FROM MasterTable mt
	INNER JOIN TrackDownloadStatus t ON t.PatientId = mt.ObjectId
	INNER JOIN [FotoFinder.Universe].[dbo].ImagesInfoTable i ON i.ImageNumber = t.ImageNumber 
	INNER JOIN MasterSyncStatus m ON m.Id = t.ImageDownloadStatus
	WHERE mt.ObjectId = @patientId
 
	UNION ALL
 
	SELECT 
		i.ObjectID AS PatientId, 
		CONCAT(mt.FirstName,' ', mt.LAStName) PatientName,
		t.MachineName,
		t.MacAddress,
		t.DownloadTime,
		i.ImageNumber, 
		NULL AS ImageName,
		NULL AS ImageDownloadStatus, 
		i.IconName,
		m.Status AS IconDownloadStatus,
		NULL AS VideoId, 
		NULL AS VideoName,
		NULL AS VideoDownloadStatus,
		'Icon' AS FileType
	FROM MasterTable mt
	INNER JOIN TrackDownloadStatus t ON t.PatientId = mt.ObjectId
	INNER JOIN [FotoFinder.Universe].[dbo].ImagesInfoTable i ON i.ImageNumber = t.ImageNumber 
	INNER JOIN MasterSyncStatus m ON m.Id = t.IconDownloadStatus
	WHERE mt.ObjectId = @patientId
 
	UNION ALL
 
	SELECT 
		vd.PatientId AS PatientId, 
		CONCAT(mt.FirstName,' ', mt.LAStName) PatientName,
		vd.MachineName,
		vd.MacAddress,
		vd.DownloadTime,
		NULL AS ImageNumber, 
		NULL AS ImageName,
		NULL AS ImageDownloadStatus, 
		NULL AS IconName,
		NULL AS IconDownloadStatus, 
		vd.VideoId AS VideoId, 
		v.FileName AS VideoName,
		m.Status AS VideoDownloadStatus,
		'Video' AS FileType
	FROM MasterTable mt
	INNER JOIN TrackVideoDownloadStatus vd on vd.PatientId = mt.ObjectId
	INNER JOIN [FotoFinder.Universe].[dbo].VideoInfoTable v ON v.VideoId = vd.VideoId
	INNER JOIN MasterSyncStatus m ON m.Id = vd.VideoDownloadStatus
	WHERE vd.PatientId = @patientId
END
GO
