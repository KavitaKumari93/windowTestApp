/****** Object:  StoredProcedure [dbo].[GetUpcomingAppointments]    Script Date: 9/20/2023 2:44:12 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetUpcomingAppointments]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetUpcomingAppointments]
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
--GetUpcomingAppointments '2023-01-01','2024-08-01'
--GetUpcomingAppointments 2023/01/01,2024/06/01
ALTER PROCEDURE [dbo].[GetUpcomingAppointments]
    @startDate Datetime,
    @endDate Datetime
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        app.Id AS AppointmentId, 
        patients.ObjectId AS PatientId, 
        CONCAT(patients.FirstName, ' ', patients.MiddleName, ' ', patients.LAStname) AS PatientName,
        CAST(patients.DateOfBirth AS DATE) AS PatientDOB,
        u.UserName as DoctorName,app.StartTime,

        (COUNT(img.ImageNumber)) AS LocalImagesCount,
        syncInfo.DownloadDateTime,
        syncInfo.UploadDateTime,
        syncInfo.SyncType,
        syncInfo.IsLocalCleared,
		(select Status from MasterSyncStatus where id=syncInfo.SyncDownloadStatus) as SyncDownloadStatus ,
        (select Status from MasterSyncStatus where id=syncInfo.SyncUploadStatus) as SyncUploadStatus,
        CASE 
            WHEN masterSyncStatus.Status = 'Download Completed' AND masterSyncStatus.Status = 'Upload Completed' 
            THEN 'Completed'
            ELSE 'Pending' 
        END AS Status

    FROM 
        Appointments AS app
        INNER JOIN MasterTable patients ON app.PatientId = patients.ObjectId
        INNER JOIN UsersInformation u ON app.DoctorId = u.UserId
        INNER JOIN [FotoFinder.Universe].[dbo].ImagesInfoTable img ON app.PatientId = img.ObjectID
        INNER JOIN CloudSyncInfo syncInfo ON syncInfo.appointmentId = app.Id
        INNER JOIN MasterSyncStatus masterSyncStatus ON masterSyncStatus.Id in (syncInfo.SyncDownloadStatus,syncInfo.SyncDownloadStatus)

    WHERE 
        CAST(app.StartTime AS DATE) BETWEEN CAST(@startDate AS DATE) AND CAST(@endDate AS DATE)
        AND app.IsDeleted = 0 AND u.IsDeleted = 0

    GROUP BY 
        app.Id, patients.ObjectId,
		CONCAT(patients.FirstName, ' ', patients.MiddleName, ' ', patients.LAStname),
        CAST(patients.DateOfBirth AS DATE), 
		u.UserName, app.StartTime,
        syncInfo.DownloadDateTime, syncInfo.UploadDateTime, syncInfo.SyncType, syncInfo.SyncDownloadStatus,
        syncInfo.SyncUploadStatus, syncInfo.IsLocalCleared, masterSyncStatus.Status;
END
GO