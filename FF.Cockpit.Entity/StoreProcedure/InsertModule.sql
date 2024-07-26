USE [FotoFinder.Universe]
GO
/****** Object:  StoredProcedure [dbo].[InsertModule]    Script Date: 3/22/2024 3:57:13 PM ******/
IF NOT EXISTS (SELECT *  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[InsertModule]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[InsertModule]
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
ALTER  PROCEDURE [dbo].[InsertModule]
	-- Add the parameters for the stored procedure here
	@id int, 
	@moduleName varchar(100)
AS
BEGIN  
SET IDENTITY_INSERT Modules ON;
		INSERT INTO Modules (ModuleId,ModuleName) VALUES(@id,@moduleName)
		SELECT @@IDENTITY AS  Id
END  
GO

