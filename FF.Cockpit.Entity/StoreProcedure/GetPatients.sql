/****** Object:  StoredProcedure [dbo].[GetPatients]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetPatients]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetPatients]
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

ALTER PROCEDURE [dbo].[GetPatients]
AS
BEGIN
        -- Search by ObjectId only
        SELECT ObjectId
			  ,ISNULL(PatientRecordnumber, '') AS PatientRecordnumber
			  ,ISNULL(LAStName, '') AS LAStName
			  ,ISNULL(FirstName, '') AS FirstName
			  ,ISNULL(MiddleName, '') AS MiddleName
			  ,CONVERT(date,DateOfBirth) AS DateOfBirth,Gender 
        FROM MasterTable where Deleted=0 order by LAStUpdate desc

END

GO

