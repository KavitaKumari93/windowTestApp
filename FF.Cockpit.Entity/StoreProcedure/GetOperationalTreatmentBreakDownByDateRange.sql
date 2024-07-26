/****** Object:  StoredProcedure [dbo].[GetOperationalTreatmentBreakDownByDateRange]    Script Date: 8/23/2023 11:58:13 AM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetOperationalTreatmentBreakDownByDateRange]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetOperationalTreatmentBreakDownByDateRange]
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
ALTER PROCEDURE [dbo].[GetOperationalTreatmentBreakDownByDateRange]
    -- Add the parameters for the stored procedure here
      @FromDate datetime,@ToDate datetime
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

SELECT appt.AppointmentTypeName AS Title,COUNT(appt.AppointmentTypeId) AS Value,appt.AppointmentTypeColor AS Color
FROM Appointments AS app
INNER JOIN MasterTable patient on app.PatientId=patient.ObjectId
INNER JOIN AppointmentTypes appt on app.AppointmentTypeId=appt.AppointmentTypeId
INNER JOIN Rooms room on app.RoomId=room.RoomId
INNER JOIN UsersInformation users on app.DoctorId=users.UserId
WHERE app.IsDeleted=0 AND patient.Deleted=0 AND room.IsDeleted=0 AND users.IsDeleted=0
AND CONVERT(DATE,app.StartTime) BETWEEN CONVERT(DATE,@fromDate) AND CONVERT(DATE,@toDate) 
GROUP BY appt.AppointmentTypeName,appt.AppointmentTypeId,appt.AppointmentTypeColor
END
GO
