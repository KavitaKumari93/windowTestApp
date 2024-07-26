
/****** Object:  StoredProcedure [dbo].[GetRoomOccupancyReportByDateRange]    Script Date: 9/09/2023 10:03:54 AM ******/

--Title: GetRoomOccupancyReportByDateRange
--Creator: KAVITA
--Purpose:  FotoFinder.Universe Application Support
--Functionality:   To get the percentage of the occupied space and available space for all the defined rooms. 
--CreatedDate:  9/09/2023
--Aplication:  FotoFinder.Universe

----------------------- Comments:  ---------------------

--Example:       Exec [GetRoomOccupancyReportByDateRange] @StartDate  ='2023-08-17',@EndDate='2025-11-16

----------------------- Modification   ---------------------

--Date        Developer            Description														
--12/09/2023  KAVITA            Also have to get the rooms which is defined but none of the appointement is created under that 
                                   --so it will consider of 100% available 



/****** Object:  StoredProcedure [dbo].[GetRoomOccupancyReportByDateRange]    Script Date: 8/23/2023 12:03:48 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetRoomOccupancyReportByDateRange]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetRoomOccupancyReportByDateRange]
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
ALTER PROCEDURE [dbo].[GetRoomOccupancyReportByDateRange]
	-- Add the parameters for the stored procedure here
	@FromDate DateTime,@ToDate DateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

DECLARE @datediff int  = (SELECT TOP 1 CONVERT(int, Value) AS startHourInMinutes FROM MiscellaneousData WHERE Name= 'EndHour') - (SELECT TOP 1 CONVERT(int, Value) AS startHourInMinutes FROM MiscellaneousData WHERE Name= 'StartHour')
SET @datediff = @datediff * (DATEDIFF(d, @FromDate,@ToDate) + 1);
SELECT room.RoomId,room.RoomName
	 ,ROUND(CAST(CAST((SUM((CASE WHEN (app.StartTime IS NULL AND app.EndTime IS NULL) THEN 0 ELSE DATEDIFF(MINUTE ,app.StartTime,app.EndTime)END))) AS NUMERIC(8,2)) / @datediff * 100 AS NUMERIC(8,2)),0) AS Occupancy
     ,ROUND(100-CAST(CAST((SUM((CASE WHEN (app.StartTime IS NULL AND app.EndTime IS NULL) THEN 0 ELSE DATEDIFF(MINUTE ,app.StartTime,app.EndTime)END))) AS NUMERIC(8,2)) / @datediff * 100 AS NUMERIC(8,2)),0) AS Available
	 ,SUM((CASE WHEN (app.StartTime IS NULL AND app.EndTime IS NULL) THEN 0 ELSE DATEDIFF(MINUTE ,app.StartTime,app.EndTime)END)) AS TotalMinutes 
FROM Rooms room
LEFT JOIN Appointments app ON app.RoomId=room.RoomId
WHERE room.IsDeleted=0 AND app.IsDeleted=0 AND CONVERT(DATE,app.StartTime) BETWEEN CONVERT(DATE,@FromDate) AND CONVERT(DATE,@ToDate)
GROUP BY room.RoomId,room.RoomName
END
Go

