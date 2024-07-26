/****** Object:  StoredProcedure [dbo].[GetAppointments]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetAppointments]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetAppointments]
    	AS
		begin
			select 1
		end')
END
Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetAppointments]  
@fromDate Datetime,  
@toDate Datetime,  
@patientId int,  
@roomId int,  
@doctorId int  
  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 --SET @fromDate = DATEADD(MI, (DATEDIFF(MI, SYSDATETIME(), SYSUTCDATETIME())), @fromDate);  
 --SET @toDate = DATEADD(MI, (DATEDIFF(MI, SYSDATETIME(), SYSUTCDATETIME())), @toDate);  
   
 SELECT Id,Notes,StartTime,EndTime  
 ,ViewType  
 ,PatientId  
 ,appointmentType.AppointmentTypeId,appointmentType.AppointmentTypeName  
 ,CONCAT(patient.FirstName,' ',patient.MiddleName,' ',patient.LAStname) AS PatientName  
 ,room.RoomId,room.RoomName  
 ,doctor.UserId as DoctorId,doctor.UserName as DoctorName 
 ,DATEDIFF(MINUTE, StartTime, EndTime) AS Duration  
 ,IsRecurrence,app.RecursiveId,appr.RecurrenceMonth  
 ,(CASE WHEN (@roomId IS NULL AND @doctorId IS NULL)  
  THEN  
    (CASE WHEN ViewType='Room View'   
       THEN (SELECT LightThemeColor FROM Rooms AS room WHERE RoomId=app.RoomId)   
       ELSE (SELECT LightThemeColor FROM UsersInformation AS doctor WHERE UserId=app.DoctorId)   
    END)   
  ELSE   
   (CASE WHEN (@roomId IS NOT NULL AND @doctorId IS NULL)  
      THEN (SELECT LightThemeColor FROM Rooms AS room WHERE RoomId=@roomId)   
         ELSE (SELECT LightThemeColor FROM UsersInformation AS doctor WHERE UserId=@doctorId)  
    END)  
  END) AS BackgroundColor  
 FROM Appointments app   
   
 LEFT JOIN AppointmentRecurrence appr ON appr.RecursiveId=app.RecursiveId  
 LEFT JOIN MasterTable patient ON patient.ObjectId=app.PatientId  
 LEFT JOIN AppointmentTypes appointmentType ON appointmentType.AppointmentTypeId=app.AppointmentTypeId  
 LEFT JOIN Rooms room ON room.RoomId=app.RoomId  
 LEFT JOIN UsersInformation doctor ON doctor.UserId=app.DoctorId  
  
 WHERE app.IsDeleted=0 AND room.IsDeleted=0 AND doctor.IsDeleted=0 AND appointmentType.IsDeleted=0  
 AND CONVERT(DATE,app.StartTime) BETWEEN CONVERT(DATE,@fromDate) AND CONVERT(DATE,@toDate)  
   
 AND (@patientId IS NULL OR app.PatientId=@patientId)  
 AND (@roomId IS NULL OR app.RoomId=@roomId)  
 AND (@doctorId IS NULL OR app.DoctorId=@doctorId);  
  
END  
  
GO
