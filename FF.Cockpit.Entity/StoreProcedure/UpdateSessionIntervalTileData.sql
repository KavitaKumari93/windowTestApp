/****** Object:  StoredProcedure [dbo].[UpdateSessionIntervalTileData]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[UpdateSessionIntervalTileData]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[UpdateSessionIntervalTileData]
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
-- UpdateSessionIntervalTileData 7429,

ALTER PROCEDURE [dbo].[UpdateSessionIntervalTileData]
@PatientId int,
@AppointmentDate datetime

AS

BEGIN
	SET NOCOUNT ON;

	--Update  @AppointmentDate=@AppointmentDate
	--where @PatientId=@PatientId
	select @PatientId;
END
GO
