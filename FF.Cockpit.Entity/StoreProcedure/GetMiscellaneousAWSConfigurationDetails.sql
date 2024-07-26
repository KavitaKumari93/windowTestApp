USE [FotoFinder.Universe]
GO

/****** Object:  StoredProcedure [dbo].[GetMiscellaneousAWSConfigurationDetails]    Script Date: 3/22/2024 3:52:57 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetMiscellaneousAWSConfigurationDetails]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetMiscellaneousAWSConfigurationDetails]
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
ALTER PROCEDURE [dbo].[GetMiscellaneousAWSConfigurationDetails]
AS
BEGIN

-- SET NOCOUNT ON added to prevent extra result sets from
    SET NOCOUNT ON
    SET transaction isolation level read uncommitted

     SELECT Id,
     AWSBucketName,
      dbo.UDF_StringDecryptor(AWSAccessKey) AS AWSAccessKey,
      dbo.UDF_StringDecryptor(AWSSecretKey) AS AWSSecretKey,
     AWSBucketRegion,
     LocationURL,
     IsActive,
     IsDeleted,
     CreatedBy,
     CreatedDate,
     UpdatedBy,
     UpdatedDate FROM MiscellaneousAWSConfigurationDetails WHERE IsDeleted=0
END
GO
