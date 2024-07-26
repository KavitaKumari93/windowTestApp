USE [FotoFinder.Cockpit]
GO
/****** Object:  StoredProcedure [dbo].[InsertOrUpdateCockpitSettings]    Script Date: 1/9/2024 2:28:02 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[InsertOrUpdateCockpitSettings]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[InsertOrUpdateCockpitSettings]
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
ALTER PROCEDURE [dbo].[InsertOrUpdateCockpitSettings] 
    -- Add the parameters for the stored procedure here
	 @lastSyncedOn DateTime
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    SET NOCOUNT ON
    SET transaction isolation level read uncommitted
	
     If ((SELECT Count(Id) from  CockpitSettings)=0)
    BEGIN
	INSERT INTO CockpitSettings(LastSyncedOn) 
    VALUES(@lastSyncedOn)
          SELECT @@IDENTITY AS Id
    END
ELSE
    BEGIN
        Update CockpitSettings SET
               LastSyncedOn=@lastSyncedOn
               WHERE Id = 1;
        SELECT @@IDENTITY AS Id
    END
END
GO


