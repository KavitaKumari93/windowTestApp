USE [FotoFinder.Universe]
GO


/****** Object:  StoredProcedure [dbo].[GetModules]    Script Date: 3/22/2024 3:55:18 PM ******/
IF NOT EXISTS (SELECT *  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetModules]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetModules]
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
ALTER PROCEDURE [dbo].[GetModules]
AS
BEGIN
SELECT ModuleId, ModuleName  FROM [dbo].[Modules] WHERE IsDeleted=0
END
GO