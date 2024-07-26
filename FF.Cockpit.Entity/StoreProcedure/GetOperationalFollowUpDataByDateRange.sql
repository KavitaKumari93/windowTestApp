/****** Object:  StoredProcedure [dbo].[GetOperationalFollowUpDataByDateRange]    Script Date: 8/17/2023 11:16:26 AM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetOperationalFollowUpDataByDateRange]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetOperationalFollowUpDataByDateRange]
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
ALTER PROCEDURE [dbo].[GetOperationalFollowUpDataByDateRange]   
@startDate datetime,@endDate datetime  
  
AS  
  
BEGIN  
   SET NOCOUNT ON;  
    
 WITH cte AS (Select patients.ObjectId AS PatientuniqueId, CONCAT(patients.FirstName,' ',patients.MiddleName,' ',patients.LAStname) AS PatientName,  
    r.RoomName, users.UserName ,app.StartTime  
    from Appointments AS app  
    inner join MasterTable patients on app.PatientId=patients.ObjectId  
    inner join Rooms r on app.RoomId=r.RoomId  
    inner join UsersInformation users on app.DoctorId=users.UserId  
 WHERE CONVERT(DATE,app.StartTime) between  CONVERT(DATE,@startDate)  and  CONVERT(DATE,@endDate) and app.IsDeleted=0 and r.IsDeleted=0 and users.IsDeleted=0)  
      
   
    SELECT PatientuniqueId, PatientName, RoomName,UserName as DoctorName,StartTime,  
 CASE WHEN ((select MIN(StartTime) from Appointments where PatientId=PatientuniqueId)>=StartTime )  
    THEN 'NEW' ELSE 'FOLLOW UP' END  AS AppointmentType into #AppTemp  
    FROM cte   
   
   
SELECT 'New' AS Title, (select COUNT(AppointmentType) from #AppTemp where AppointmentType='NEW')  AS Value,'#FF0000' AS Color   
UNION  
SELECT 'Follow-Up' AS Title, (select COUNT(AppointmentType) from #AppTemp where AppointmentType='FOLLOW UP') AS Value,'#6D5740' AS Color   
DROP TABLE #AppTemp;  
END  
GO
