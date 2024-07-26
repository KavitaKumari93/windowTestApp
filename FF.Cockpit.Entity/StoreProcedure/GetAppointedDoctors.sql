USE [FotoFinder.Universe]
GO
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetAppointedDoctors]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetAppointedDoctors]
    	as
		begin
			select 1
		end')
END

/****** Object:  StoredProcedure [dbo].[GetAppointedDoctors]    Script Date: 8/17/2023 11:27:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
Title: GetAppointedDoctors
Creator: Sakshi Sharma
Purpose:  FotoFinder.Universe Application Support
Functionality:   To get doctors appointment list according to startdate and enddate
CreatedDate:   8/09/2023
Aplication:  FotoFinder.Universe

--------------------- Comments:  ---------------------

Example:       Exec [GetAppointedDoctors]            @StartDate  =     '2023-08-17', 
											                @EndDate    =	  '2025-11-16

--------------------- Modifications:  ---------------------
Date        Developer            Description														
8/10/2023   Sakshi Sharma        Added the parameter for enddate
*/
ALTER Procedure [dbo].[GetAppointedDoctors]     
    @StartDate date,   
    @EndDate date
AS   
BEGIN   
    SET NOCOUNT ON;
    
   WITH cte AS   
(
    SELECT
        users.UserName,
        appt.AppointmentTypeName AS [Subject],
        COUNT(Id) AS totalAppointment,
        SUM(DateDiff(MINUTE, ap.StartTime, ap.EndTime)) AS Duration
    FROM Appointments ap    
    JOIN UsersInformation users ON users.UserId = ap.DoctorId   
    JOIN Rooms r ON r.RoomId = ap.RoomId  
    JOIN AppointmentTypes appt ON appt.AppointmentTypeId = ap.AppointmentTypeId
    WHERE CONVERT(DATE, ap.StartTime) BETWEEN @StartDate AND @EndDate
        AND users.IsDeleted = 0 
        AND ap.IsDeleted = 0 
        AND r.IsDeleted = 0   
    GROUP BY UserName, appt.AppointmentTypeName
)
    
    SELECT 
        UserName as DoctorName,
        STRING_AGG(CONCAT([Subject], '(', totalAppointment, ')'), ', ') AS Appointments,
        SUM(totalAppointment) AS TotalAppointments,
        (
            SELECT 
                CASE 
                    WHEN (CONVERT(VARCHAR(2), SUM(Duration) % 60)) = 0 
                        THEN CONVERT(VARCHAR(2), SUM(Duration) / 60) + ' hr'
                    WHEN SUM(Duration) > 60 
                        THEN CONVERT(VARCHAR(2), SUM(Duration) / 60) + ' hr ' 
                             + RIGHT('0' + CONVERT(VARCHAR(2), SUM(Duration) % 60), 2) + ' m'
                    ELSE CONVERT(VARCHAR(2), SUM(Duration)) + ' m'
                END
        ) AS TotalDuration
    FROM cte 
    GROUP BY UserName;
END
Go