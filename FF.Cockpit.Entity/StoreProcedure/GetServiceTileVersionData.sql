USE [FotoFinder.Universe]
GO
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetServiceTileVersionData]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetServiceTileVersionData]
    	as
		begin
			select 1
		end')
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetServiceTileVersionData]    Script Date: 07-06-2023 07:01:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetServiceTileVersionData]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select [Version]AS VersionNumber from [FotoFinder.Universe].[dbo].DatabaseData
END
GO
