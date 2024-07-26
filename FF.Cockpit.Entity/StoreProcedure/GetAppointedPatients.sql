USE [FotoFinder.Universe]
GO
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetAppointedPatients]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetAppointedPatients]
    	as
		begin
			select 1
		end')
END

GO
/****** Object:  StoredProcedure [dbo].[GetAppointedPatients]    Script Date: 8/17/2023 11:26:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetAppointedPatients]  
    -- Add the parameters for the stored procedure here  
      @startDate Datetime,  
      @endDate Datetime  
AS  
BEGIN  
   SET NOCOUNT ON;  
   WITH CTE AS 
    (Select 
	patients.ObjectId AS PatientId, 
    CONCAT(patients.FirstName,' ',patients.MiddleName,' ',patients.LAStname) AS PatientName,  
    patients.PatientRecordnumber as PatientRecordNumber,
    r.RoomId,r.RoomName , users.UserName as DoctorName,app.StartTime  
    from Appointments AS app  
    inner join MasterTable patients on app.PatientId=patients.ObjectId  
    inner join Rooms r on app.RoomId=r.RoomId  
    inner join UsersInformation users on app.DoctorId=users.UserId  
 WHERE CONVERT(DATE,app.StartTime) between CONVERT(DATE,@startDate)  and  CONVERT(DATE,@endDate) and app.IsDeleted=0 and r.IsDeleted=0 and users.IsDeleted=0)  
      
   
    SELECT PatientId, PatientName,PatientRecordNumber,RoomId, DoctorName,StartTime,  RoomName,
   CASE WHEN ((select MIN(StartTime) from Appointments where PatientId=PatientId)>=StartTime  )  
    THEN 'NEW' ELSE 'FOLLOW UP' END  AS AppointmentType into #AppTemp  
    FROM CTE  order by StartTime  
  
 SELECT * FROM #AppTemp  
 DROP TABLE #AppTemp;  
END  
GO
