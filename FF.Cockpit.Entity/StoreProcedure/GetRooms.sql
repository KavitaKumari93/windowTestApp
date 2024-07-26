/****** Object:  StoredProcedure [dbo].[GetRooms]    Script Date: 9/12/2023 4:31:35 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetRooms]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetRooms]
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
ALTER PROCEDURE [dbo].[GetRooms]
AS
BEGIN
	 SELECT * FROM Rooms WHERE IsDeleted=0
END
GO

