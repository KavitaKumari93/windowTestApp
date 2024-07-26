IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[InsertOrUpdateMiscellaneousData]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[InsertOrUpdateMiscellaneousData]
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

ALTER PROCEDURE [dbo].[InsertOrUpdateMiscellaneousData] 
	-- Add the parameters for the stored procedure here
	@id int,
	@name nvarchar(50), 
	@value nvarchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON
	SET transaction isolation level read uncommitted

 If(@id = 0)
	BEGIN
		INSERT INTO MiscellaneousData (Name,Value) VALUES(@name,@value);
	END
 ELSE
	BEGIN	
		Update MiscellaneousData SET Value = @value WHERE Id = @id;
	END
END
Go