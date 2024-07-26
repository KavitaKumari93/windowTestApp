/****** Object:  StoredProcedure [dbo].[InsertOrUpdateRoom]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[InsertOrUpdateRoom]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[InsertOrUpdateRoom]
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
ALTER Procedure [dbo].[InsertOrUpdateRoom]
@id int,
@name nvarchar(100),
@description nvarchar(1000),
@darkThemeColor nvarchar(10),
@lightThemeColor nvarchar(10)
AS
BEGIN

If(@id > 0)
	BEGIN
		Update Rooms SET
			RoomName = @name,
			Description = @description,
			DarkThemeColor= @darkThemeColor,
			LightThemeColor = @lightThemeColor
		WHERE RoomId = @id

		SELECT @id AS RoomId
	END
ELSE
	BEGIN
		INSERT INTO Rooms(RoomName,Description,DarkThemeColor,LightThemeColor) 
		VALUES(@name,@description,@darkThemeColor,@lightThemeColor)
		SELECT @@IDENTITY AS RoomId
	END
END

GO

