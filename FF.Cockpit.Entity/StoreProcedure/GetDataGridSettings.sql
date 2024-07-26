IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetDataGridSettings]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetDataGridSettings]
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
--exec GetDataGridSettings
ALTER PROCEDURE [dbo].[GetDataGridSettings]
	
AS
BEGIN
	SELECT * FROM DataGridSettings WHERE IsDeleted=0
END
GO