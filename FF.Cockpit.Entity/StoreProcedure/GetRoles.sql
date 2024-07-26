/****** Object:  StoredProcedure [dbo].[GetRoles]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetRoles]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetRoles]
    	AS
		begin
			select 1
		end')
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Title:       GetRoles
-- Author:      Sakshi Sharma
-- Create date: 08/01/2023
-- Description: Get the  data of Roles Table
-- Purpose:     FotoFinder Application Support
-- Application: FotoFinder
-- =============================================

-- Modifications:
-- =============================================
Date                   Developer                    Description    
-- =============================================
*/
Alter PROCEDURE [dbo].[GetRoles]
    
AS
BEGIN

-- SET NOCOUNT ON added to prevent extra result sets from
    SET NOCOUNT ON
    SET transaction isolation level read uncommitted

     SELECT * FROM RolesTable WHERE IsDeleted=0
END
GO
