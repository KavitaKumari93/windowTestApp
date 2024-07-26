USE [FotoFinder.Universe]
GO
/****** Object:  StoredProcedure [dbo].[UpdateDownloadStatus]    Script Date: 1/31/2024 11:05:37 AM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[UpdateDownloadStatus]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[UpdateDownloadStatus]
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
ALTER PROCEDURE [dbo].[UpdateDownloadStatus] 
	-- Add the parameters for the stored procedure here
	@appointmentId int
	,@patientId int
	,@syncDownloadStatus int 
	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON
	SET transaction isolation level read uncommitted
   
		UPDATE [FotoFinder.Cockpit].dbo.CloudSyncInfo 
		SET SyncDownloadStatus = @syncDownloadStatus 
		WHERE PatientId = @patientId AND AppointmentId = @appointmentId
END
GO

