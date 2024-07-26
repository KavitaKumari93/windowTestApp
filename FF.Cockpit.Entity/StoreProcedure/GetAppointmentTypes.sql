IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetAppointmentTypes]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetAppointmentTypes]
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
-- Title:       GetAppointmentTypes
-- Author:      Shareef Mohammad
-- Create date: 18/09/2023
-- Description: Get the  data of AppointmentTypes Table
-- Purpose:     FotoFinder Application Support
-- Application: FotoFinder
-- =============================================

-- Modifications:
-- =============================================
Date                   Developer                    Description    
-- =============================================
*/
ALTER PROCEDURE [dbo].[GetAppointmentTypes]
    
AS
BEGIN

-- SET NOCOUNT ON added to prevent extra result sets from
    SET NOCOUNT ON
    SET transaction isolation level read uncommitted

     SELECT * FROM AppointmentTypes WHERE IsDeleted=0
END
GO