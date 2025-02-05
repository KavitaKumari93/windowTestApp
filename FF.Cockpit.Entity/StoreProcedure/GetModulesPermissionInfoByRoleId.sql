USE [FotoFinder.Universe]
GO
IF NOT EXISTS (SELECT *  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetModulesPermissionInfoByRoleId]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetModulesPermissionInfoByRoleId]
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
ALTER PROCEDURE [dbo].GetModulesPermissionInfoByRoleId
@roleId int
AS
BEGIN
SELECT ModuleId, RoleId as UserRoleId, IsAccessible  FROM [dbo].[ModulePermission] WHERE IsDeleted=0 and RoleId=@roleId
END
GO


