
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[InsertOrUpdateAppointmentType]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[InsertOrUpdateAppointmentType]
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
-- =============================================
-- Title:       InsertOrUpdateAppointmentType
-- Author:      Shareef Mohammad
-- Create date: 18/09/2023
-- Description: Insert Or update the data of Roles Table
-- Purpose:     FotoFinder Application Support
-- Application: FotoFinder
-- =============================================

-- MODIFICATION

-- DATE            DEVELOPER                  DESCRIPTION ----
-- 08/02/2023      Sakshi Sharma              added new parameter @IsActive

ALTER PROCEDURE [dbo].[InsertOrUpdateAppointmentType] 
	-- Add the parameters for the stored procedure here
	@appointmentTypeId int, 
	@appointmentTypeName nvarchar(100),
	@appointmentTypeColor nvarchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON
	SET transaction isolation level read uncommitted

   
	If(@AppointmentTypeId > 0)
	BEGIN
		Update AppointmentTypes SET AppointmentTypeName = @appointmentTypeName
		,AppointmentTypeColor = @appointmentTypeColor
		WHERE AppointmentTypeId = @appointmentTypeId
		SELECT @AppointmentTypeId AS AppointmentTypeId
	END
 ELSE
	BEGIN
	 -- Insert statements for procedure here
		INSERT INTO AppointmentTypes (AppointmentTypeName,AppointmentTypeColor) VALUES(@appointmentTypeName,@appointmentTypeColor)
		SELECT @@IDENTITY AS AppointmentTypeId
	END
END
Go