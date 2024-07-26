
/****** Object:  StoredProcedure [dbo].[InsertOrUpdateCloudSyncInfo]    Script Date: 29/09/2023 17:55:38 ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[InsertOrUpdateCloudSyncInfo]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[InsertOrUpdateCloudSyncInfo]
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

ALTER PROCEDURE [dbo].[InsertOrUpdateCloudSyncInfo] 
    -- Add the parameters for the stored procedure here
    @id int, 
    @appointmentId int,
    @patientId int,
    @downloadDateTime datetime,
    @uploadDateTime datetime,
    @syncType nvarchar(20),
    @syncDownloadStatus int,
	@syncUploadStatus int,
	@isLocalCleared bit
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    SET NOCOUNT ON
    SET transaction isolation level read uncommitted
   
    If(@id = 0)
    BEGIN
	INSERT INTO CloudSyncInfo(AppointmentId,PatientId,DownloadDateTime,UploadDateTime,SyncType,SyncDownloadStatus,SyncUploadStatus,IsLocalCleared) 
        VALUES(@appointmentId,@patientId,@downloadDateTime,@uploadDateTime,@syncType,@syncDownloadStatus,@syncUploadStatus,@isLocalCleared)
          SELECT @@IDENTITY AS Id
    END
ELSE
    BEGIN

 

        Update CloudSyncInfo SET
               PatientId=@patientId,
               DownloadDateTime=@downloadDateTime,
               UploadDateTime=@uploadDateTime,
               SyncType=@syncType,
               --SyncDownloadStatus=@syncDownloadStatus,
               --SyncUploadStatus=@syncUploadStatus,
			   IsLocalCleared=@isLocalCleared
         WHERE AppointmentId = @appointmentId

 

        SELECT @@IDENTITY AS Id
    END
 END
GO
