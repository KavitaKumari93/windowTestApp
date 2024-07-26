USE [FotoFinder.Universe]
GO
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[DeleteAppointment]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[DeleteAppointment]
    	as
		begin
			select 1
		end')
END

GO
/****** Object:  StoredProcedure [dbo].[DeleteAppointment]    Script Date: 6/27/2023 10:23:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[DeleteAppointment]
@id INT,
@recursiveId INT,
@isFullSeriesDelete  BIT
AS
BEGIN
	IF(@recursiveId = 0)--without recurrence
	BEGIN
		UPDATE Appointments SET IsDeleted=1 WHERE Id=@id;
		SELECT 1;
	END
	ELSE--with recurrence
	BEGIN
		IF(@isFullSeriesDelete = 1)
		BEGIN
			UPDATE Appointments SET IsDeleted=1 WHERE RecursiveId=@recursiveId;
			UPDATE AppointmentRecurrence SET IsDeleted=1 WHERE RecursiveId=@recursiveId;
			SELECT 1;
		END
		ELSE
		BEGIN
			UPDATE Appointments SET IsDeleted=1 WHERE Id=@id;
			SELECT 1;
		END
	END
END

GO