
/****** Object:  StoredProcedure [dbo].[GetScheduledAppointmentTimeByDateRange]    Script Date: 8/23/2023 12:03:48 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetScheduledAppointmentTimeByDateRange]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetScheduledAppointmentTimeByDateRange]
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
/*
--Unit Testing : exec GetScheduledAppointmentTimeByDateRange '2017-08-19 10:55:00.000' , '2050-08-19 10:50:00.000'
--Title:  GetScheduledAppointmentTimeByDateRange
--Creator: Kavita Kumari
--Purpose:  FotoFinder.Universe Application Support
--Functionality:   To get TotalTime per day for an appointment
--CreatedDate:   8/21/2023
--Aplication:  FotoFinder.Universe

--------------------- Modifications:  ---------------------
Date        Developer            Description														
8/21/2023   Sakshi Sharma      Applied cASes To get  data  according to the exact format 
*/


ALTER PROCEDURE [dbo].[GetScheduledAppointmentTimeByDateRange]     
 @FromDate DATETIME,  
 @ToDate DATETIME  
AS    
BEGIN    
 DECLARE @minutes INT;    
 DECLARE @hours VARCHAR;    
    
 SET NOCOUNT ON;    
 WITH CTE AS (SELECT SUM(DATEDIFF(MINUTE,StartTime,EndTime)) TotalMinutes FROM Appointments  AS app  
 join Rooms AS r on r.RoomId=app.RoomId 
 join UsersInformation AS u on u.UserId=app.DoctorId
 WHERE CONVERT(DATE,StartTime) BETWEEN CONVERT(DATE,@FromDate) AND CONVERT(DATE,@ToDate) AND app.IsDeleted=0 and r.IsDeleted=0 and u.IsDeleted=0)    
    SELECT TotalMinutes INTO #AppTemp FROM CTE;  
 DECLARE @totalMin INT = (SELECT TotalMinutes FROM #AppTemp);  
 SELECT 
		CASE WHEN (CONVERT(VARCHAR(2), @totalMin % 60))=0 THEN CONVERT(VARCHAR(2), @totalMin / 60) + ' hr'
		WHEN @totalMin > 60 THEN CONVERT(VARCHAR(2), @totalMin / 60) + ' hr' +' ' + RIGHT('0' + CONVERT(VARCHAR(2), @totalMin % 60), 2 )+' m '
		ELSE CONVERT(VARCHAR(2), @totalMin) + ' m '
	END AS TotalTime
 DROP TABLE #AppTemp  
END
Go
