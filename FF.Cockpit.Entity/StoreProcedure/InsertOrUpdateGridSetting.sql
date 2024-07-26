IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[InsertOrUpdateGridSetting]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[InsertOrUpdateGridSetting]
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
ALTER Procedure [dbo].[InsertOrUpdateGridSetting]
  @id int,
  @userId int,
  @moduleId int,
  @gridName varchar(50),
  @gridConfigName varchar(50),
  @gridConfigValue varchar(max)

AS
BEGIN

If(@id > 0)
	BEGIN
	
		UPDATE DataGridSettings SET 
			UserID=@userId,
			ModuleId=@moduleId,
			GridName=@gridName,
			GridConfigName=@gridConfigName,
			GridConfigValue=@gridConfigValue
		WHERE Id=@id

		SELECT @id AS Id
	END
ELSE
	BEGIN
		INSERT INTO DataGridSettings (UserId,ModuleId,GridName,GridConfigName,GridConfigValue) 
		VALUES(@userId,@moduleId,@gridName,@gridConfigName,@gridConfigValue)

		SELECT @@IDENTITY AS Id
	END
END
GO
