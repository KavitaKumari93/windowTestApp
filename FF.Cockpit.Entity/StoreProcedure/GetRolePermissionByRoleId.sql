USE [FotoFinder.Universe]
GO
/****** Object:  StoredProcedure [dbo].[GetRolePermissionByRoleId]    Script Date: 2/29/2024 4:14:00 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetRolePermissionByRoleId]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetRolePermissionByRoleId]
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
ALTER PROCEDURE [dbo].[GetRolePermissionByRoleId]
@roleId int
AS
BEGIN
    IF EXISTS (SELECT * FROM RolePermission WHERE RoleId=@roleId)
	BEGIN
		SELECT rp.Id,md.ModuleId,md.ModuleName,rp.RoleId,rp.Permission 
		FROM RolePermission rp
		Left Join Modules md on md.ModuleId=rp.ModuleId
		WHERE rp.RoleId=@roleId and rp.IsDeleted=0
	END
	ELSE
	BEGIN
		SELECT 0 as Id,ModuleId,ModuleName,@roleId as RoleId,0 AS Permission 
		FROM Modules WHERE IsDeleted=0
	END
END

GO
