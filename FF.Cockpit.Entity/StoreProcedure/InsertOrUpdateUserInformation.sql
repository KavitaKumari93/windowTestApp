
/****** Object:  StoredProcedure [dbo].[InsertOrUpdateUserInformation]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[InsertOrUpdateUserInformation]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[InsertOrUpdateUserInformation]
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
ALTER PROCEDURE [dbo].[InsertOrUpdateUserInformation]
@id int,
@name nvarchar(100),
@phoneNumber nvarchar(20),
@email nvarchar(50),
@userroleId int,
@password nvarchar (50),
@photo varbinary(max),
@specialization nvarchar(100),
@description nvarchar(100),
@darkThemeColor nvarchar(100),
@lightThemeColor nvarchar(100)

AS
BEGIN

DECLARE @encryptedPassword varbinary(8000)

-- Encrypt the password using UDF_StringEncryptor function
SET @encryptedPassword = dbo.UDF_StringEncryptor(@password)

IF(@id > 0)
BEGIN
    UPDATE UsersInformation SET 
	    
        UserName = @name,
        PhoneNumber = @phoneNumber,
        Email = @email,
        Password = @encryptedPassword, -- Update encrypted password
        UserRoleId = @userroleId,
        Photo = @photo,
        Specialization = @specialization,
        Description = @description,
        DarkThemeColor = @darkThemeColor,
        LightThemeColor = @lightThemeColor
    WHERE UserId = @id

    SELECT @id AS UserId
END
ELSE
BEGIN
    INSERT INTO UsersInformation(UserName, PhoneNumber, UserRoleId, Email, Password, Photo, Specialization, Description, DarkThemeColor, LightThemeColor) 
    VALUES(@name, @phoneNumber, @userroleId, @email, @encryptedPassword, @photo, @specialization, @description, @darkThemeColor, @lightThemeColor)

    SELECT @@IDENTITY AS UserId
END
END
GO

