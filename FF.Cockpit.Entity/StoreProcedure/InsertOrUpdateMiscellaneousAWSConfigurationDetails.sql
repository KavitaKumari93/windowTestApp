USE [FotoFinder.Universe]
GO
/****** Object:  StoredProcedure [dbo].[InsertOrUpdateMiscellaneousAWSConfigurationDetails]    Script Date: 3/22/2024 3:54:03 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[InsertOrUpdateMiscellaneousAWSConfigurationDetails]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[InsertOrUpdateMiscellaneousAWSConfigurationDetails]
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

ALTER PROCEDURE [dbo].[InsertOrUpdateMiscellaneousAWSConfigurationDetails]
@id int,
@bucket nvarchar(100),
@access nvarchar(20),
@secret nvarchar(50),
@region nvarchar(50),
@url nvarchar(500),
@isActive bit
AS
BEGIN

DECLARE @encryptedAccessKey varbinary(8000)
Declare @encryptedSecretKey varbinary(8000)

-- Encrypt the password using UDF_StringEncryptor function
SET @encryptedAccessKey = dbo.UDF_StringEncryptor(@access)
SET @encryptedSecretKey = dbo.UDF_StringEncryptor(@secret)

If(@id > 0)
    BEGIN
    
        UPDATE MiscellaneousAWSConfigurationDetails SET 
            AWSBucketName=@bucket,
            AWSAccessKey=@encryptedAccessKey,
            AWSSecretKey=@encryptedSecretKey,
          AWSBucketRegion=@region,
            LocationURL=@url,
            IsActive=@isActive
        WHERE Id=@id

        SELECT @id AS Id
    END
ELSE
    BEGIN
        INSERT INTO MiscellaneousAWSConfigurationDetails(AWSBucketName,AWSAccessKey,AWSSecretKey,IsActive,AWSBucketRegion,LocationURL) 
        VALUES(@bucket,@encryptedAccessKey,@encryptedSecretKey,@isActive,@region, @url)

        SELECT @@IDENTITY AS Id
    END
END
GO
