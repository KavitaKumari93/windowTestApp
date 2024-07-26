/****** Object:  StoredProcedure [dbo].[DeleteDoctor]    Script Date: 7/25/2023 5:41:32 PM ******/

IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[DeleteUserByRoleId]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[DeleteUserByRoleId]
    	AS
		begin
			select 1
		end')
END
Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[DeleteUserByRoleId]
@id int 
AS
BEGIN
	UPDATE UsersInformation SET IsDeleted=1 WHERE UserId=@id;
	SELECT @id AS DoctorId;
	ENd
GO
