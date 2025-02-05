/****** Object:  StoredProcedure [dbo].[GetSessionIntervalTileDataByPatientId]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetSessionIntervalTileDataByPatientId]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetSessionIntervalTileDataByPatientId]
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
ALTER PROCEDURE [dbo].[GetSessionIntervalTileDataByPatientId] 
@PatientId int, 
@selectedFilterDate DateTime
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 'Low' AS FrequencyLevel, '' AS Interval, 
    StartTime AS AppointmentStartAt, 
    EndTime AS AppointmentEndAt
    FROM Appointments
    WHERE PatientId = @PatientId
        AND StartTime >= GETUTCDATE()
        AND IsDeleted = 0 
    ORDER BY StartTime ASC
END
GO
