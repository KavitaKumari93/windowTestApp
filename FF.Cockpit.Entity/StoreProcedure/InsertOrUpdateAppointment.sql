
/****** Object:  StoredProcedure [dbo].[InsertOrUpdateAppointment]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[InsertOrUpdateAppointment]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[InsertOrUpdateAppointment]
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
ALTER PROCEDURE [dbo].[InsertOrUpdateAppointment]

--default fields
@id int,
@notes nvarchar(1000),
@startTime datetime,
@endTime datetime,

--custom fields
@patientId int,
@appointmentTypeId int,
@roomId int,
@doctorId int,
@isRecurrence bit,
@recurrenceMonth int,
@recursiveId int,
@viewType nvarchar(50)
AS
BEGIN

DECLARE @UntillDate DateTime=CASE WHEN @isRecurrence=1 THEN DATEADD(YEAR,5,@startTime) ELSE @startTime END

If(@id > 0)
	BEGIN
		IF(@isRecurrence = 0 AND @recursiveId = 0)--without recurrence
			BEGIN
			    UPDATE Appointments SET 
			    Notes=@notes,
			    StartTime=@startTime,
			    EndTime=@endTime,
			    
				
			    PatientId=@patientId,
				AppointmentTypeId=@appointmentTypeId,
			    RoomId=@roomId,
			    DoctorId=@doctorId,
			    ViewType=@viewType
				WHERE Id=@id
				SELECT @id AS Id
			END
		ELSE IF(@isRecurrence = 0 AND @recursiveId > 0)--exist recurrence off
			BEGIN
				--Delete recurrence series except selected appointment
				Delete Appointments WHERE RecursiveId=@recursiveId

				--Delete recurrence detail
				DELETE AppointmentRecurrence WHERE RecursiveId=@recursiveId

				--Insert new appointment without recurrence
				INSERT INTO Appointments(Notes,StartTime,EndTime,PatientId,RoomId,DoctorId,AppointmentTypeId,ViewType) 
				VALUES(@notes,@startTime,@endTime,@patientId,@roomId,@appointmentTypeId,@doctorId,@viewType)

				SELECT @@IDENTITY AS Id;
			END
		ELSE IF(@isRecurrence = 1 AND @recursiveId = 0)--new recurrence create
			BEGIN
				
				--Delete selected appointment first
				Delete Appointments WHERE Id=@id

				--Insert AppointmentRecurrence details
				INSERT INTO AppointmentRecurrence(RecurrenceMonth) VALUES(@recurrenceMonth);
				SET @recursiveId = @@IDENTITY;

				--Insert recurrence series
				WHILE (@startTime <= @UntillDate)
				BEGIN
					INSERT INTO Appointments(Notes,StartTime,EndTime,PatientId,AppointmentTypeId,RoomId,DoctorId,IsRecurrence,RecursiveId,ViewType) 
					VALUES(@notes,@startTime,@endTime,@patientId,@appointmentTypeId,@roomId,@doctorId,@isRecurrence,@recursiveId,@viewType)
					
					SET @startTime = DATEADD(MONTH,@recurrenceMonth,@startTime)
					SET @endTime   = DATEADD(MONTH,@recurrenceMonth,@endTime)
				END

				SELECT @@IDENTITY AS Id
			END
		ELSE IF(@isRecurrence = 1 AND @recursiveId > 0)--exist recurrence keep AS on and update the details
			BEGIN
				--Delete recurrence series
				DELETE Appointments WHERE RecursiveId=@recursiveId
				
				--Delete recurrence detail
				DELETE AppointmentRecurrence WHERE RecursiveId=@recursiveId
				
				--Insert AppointmentRecurrence details
				INSERT INTO AppointmentRecurrence(RecurrenceMonth) VALUES(@recurrenceMonth);
				SET @recursiveId = @@IDENTITY;

				--Insert recurrence series
				WHILE (@startTime <= @UntillDate)
				BEGIN
					INSERT INTO Appointments(Notes,StartTime,EndTime,PatientId,AppointmentTypeId,RoomId,DoctorId,IsRecurrence,RecursiveId,ViewType) 
					VALUES(@notes,@startTime,@endTime,@patientId,@appointmentTypeId,@roomId,@doctorId,@isRecurrence,@recursiveId,@viewType)
					
					SET @startTime = DATEADD(MONTH,@recurrenceMonth,@startTime)
					SET @endTime   = DATEADD(MONTH,@recurrenceMonth,@endTime)
				END

				SELECT @@IDENTITY AS Id
			END
	END
ELSE
	BEGIN

		IF(@isRecurrence=0)--without recurrence
		BEGIN
			INSERT INTO Appointments(Notes,StartTime,EndTime,PatientId,AppointmentTypeId,RoomId,DoctorId,ViewType) 
			VALUES(@notes,@startTime,@endTime,@patientId,@appointmentTypeId,@roomId,@doctorId,@viewType)
		END

		ELSE--with recurrence
		BEGIN
		    --Insert AppointmentRecurrence details
			INSERT INTO AppointmentRecurrence(RecurrenceMonth) VALUES(@recurrenceMonth);
			SET @recursiveId = @@IDENTITY;

			WHILE (@startTime <= @UntillDate)
			BEGIN
				--Insert Appointment details
				INSERT INTO Appointments(Notes,StartTime,EndTime,PatientId,AppointmentTypeId,RoomId,DoctorId,IsRecurrence,RecursiveId,ViewType) 
				VALUES(@notes,@startTime,@endTime,@patientId,@appointmentTypeId,@roomId,@doctorId,@isRecurrence,@recursiveId,@viewType)
				
				SET @startTime = DATEADD(MONTH,@recurrenceMonth,@startTime)
				SET @endTime   = DATEADD(MONTH,@recurrenceMonth,@endTime)
			END
		 END

		SELECT @@IDENTITY AS Id
	END
END
GO