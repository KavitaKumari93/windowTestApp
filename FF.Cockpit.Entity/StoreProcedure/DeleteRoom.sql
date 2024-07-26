
USE [FotoFinder.Universe]
GO
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[DeleteRoom]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[DeleteRoom]
    	as
		begin
			select 1
		end')
END

GO
/****** Object:  StoredProcedure [dbo].[DeleteRoom]    Script Date: 6/27/2023 10:23:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[DeleteRoom]
@id int 
AS
BEGIN
	UPDATE Rooms SET IsDeleted=1 WHERE RoomId=@id;
	SELECT @id AS RoomId;
END
GO
