/****** Object:  StoredProcedure [dbo].[GetUsersInformation]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetUsersInformation]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetUsersInformation]
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
ALTER PROCEDURE [dbo].[GetUsersInformation]
AS
BEGIN
    SELECT 
        UserId,
        UserName,
        PhoneNumber,
        Email,
        dbo.UDF_StringDecryptor(Password) AS [Password], -- Decrypt password
        UserRoleId,
        Photo,
        Specialization,
        Description,
        DarkThemeColor,
        LightThemeColor 
    FROM UsersInformation 
    WHERE IsDeleted = 0
END
GO

