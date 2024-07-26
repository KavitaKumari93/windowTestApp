/****** Object:  StoredProcedure [dbo].[InsertOrUpdateRole]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[InsertOrUpdateRole]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[InsertOrUpdateRole]
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
ALTER PROCEDURE [dbo].[InsertOrUpdateRole] 
	-- Add the parameters for the stored procedure here
	@RoleId int, 
	@RoleName nvarchar(100),
	@IsActive bit 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON
	SET transaction isolation level read uncommitted

   
	If(@RoleId > 0)
	BEGIN
		Update RolesTable SET
			RoleName = @RoleName,
			IsActive = @IsActive
			
		WHERE RoleId = @RoleId

		SELECT @RoleId AS RoleId
	END
 ELSE
	BEGIN
	 -- Insert statements for procedure here
		INSERT INTO RolesTable(RoleName,IsActive) 
		VALUES(@RoleName,@IsActive)
		SELECT @@IDENTITY AS RoleId
	END
END
GO
