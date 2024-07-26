USE [FotoFinder.Universe]
GO
/****** Object:  StoredProcedure [dbo].[GetCockpitSettings]    Script Date: 3/11/2024 3:11:42 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetCockpitSettings]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetCockpitSettings]
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
ALTER PROCEDURE [dbo].[GetCockpitSettings]
AS 
BEGIN 
SELECT 
LastSyncedOn 
FROM CockpitSettings
END
GO
