USE [FotoFinder.Universe]
GO
	/****** Object:  StoredProcedure [dbo].[InsertOrUpdateModulePermissionByRoleId]    Script Date: 3/22/2024 3:58:09 PM ******/
IF NOT EXISTS (SELECT *  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[InsertOrUpdateModulePermissionByRoleId]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[InsertOrUpdateModulePermissionByRoleId]
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

ALTER PROCEDURE [dbo].[InsertOrUpdateModulePermissionByRoleId] 
	-- Add the parameters for the stored procedure here
	@roleId int, 
	@moduleId int,
	@access bit 
AS
BEGIN
	
	IF EXISTS (SELECT * FROM ModulePermission WHERE RoleId=@roleId and ModuleId=@moduleId)
	BEGIN
		Update ModulePermission SET 
			IsAccessible=@access WHERE RoleId=@roleId and ModuleId=@moduleId

	END
 ELSE
	BEGIN
	
	 -- Insert statements for procedure here
		INSERT INTO ModulePermission(RoleId,ModuleId,IsAccessible) 
		VALUES(@roleId,@moduleId,@access)
		
	END
END
GO