IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[DeleteAppointmentType]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[DeleteAppointmentType]
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
-- =============================================
-- Title:       DeleteAppointmentTypes
-- Author:      Shareef Mohammad
-- Create date: 08/01/2023
-- Description: Delete the data of Appointment Types
-- Purpose:     FotoFinder Application Support
-- Application: FotoFinder
-- =============================================

-- Modifications:
-- =============================================
Date                   Developer                    Description    
-- =============================================
*/
ALTER  PROCEDURE [dbo].[DeleteAppointmentType]

-- Add the parameters for the stored procedure here
@appointmentTypeId int 
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
    SET NOCOUNT ON
    SET transaction isolation level read uncommitted

    UPDATE AppointmentTypes SET IsDeleted=1 WHERE AppointmentTypeId=@appointmentTypeId;
    SELECT @appointmentTypeId AS AppointmentTypeId;
END
GO

