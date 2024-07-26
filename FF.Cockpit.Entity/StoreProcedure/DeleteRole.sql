USE [FotoFinder.Universe]
GO
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[DeleteRole]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[DeleteRole]
    	as
		begin
			select 1
		end')
END

GO
/****** Object:  StoredProcedure [dbo].[DeleteRole]    Script Date: 6/27/2023 10:23:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Title:       DeleteRoles
-- Author:      Sakshi Sharma
-- Create date: 08/01/2023
-- Description: Delete the data of Roles Table
-- Purpose:     FotoFinder Application Support
-- Application: FotoFinder
-- =============================================

-- Modifications:
-- =============================================
Date                   Developer                    Description    
-- =============================================
*/

ALTER  PROCEDURE [dbo].[DeleteRole]

-- Add the parameters for the stored procedure here
@RoleId int 

AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON
	SET transaction isolation level read uncommitted

	UPDATE RolesTable SET IsDeleted=1 WHERE RoleId=@RoleId;
	SELECT @RoleId AS RoleId;
END
GO



