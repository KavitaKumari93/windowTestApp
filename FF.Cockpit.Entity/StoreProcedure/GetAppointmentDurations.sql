USE [FotoFinder.Universe]
GO
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetAppointmentDurations]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetAppointmentDurations]
    	as
		begin
			select 1
		end')
END

GO
/****** Object:  StoredProcedure [dbo].[GetAppointmentDurations]    Script Date: 6/15/2023 6:10:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec GetAppointmentDurations
ALTER PROCEDURE [dbo].[GetAppointmentDurations]
	
AS
BEGIN
	SELECT * FROM AppointmentDurations WHERE IsDeleted=0
END
GO
