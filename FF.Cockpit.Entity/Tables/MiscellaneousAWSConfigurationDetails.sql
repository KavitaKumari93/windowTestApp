/****** Object:  Table [dbo].[MiscellaneousAWSConfigurationDetails]    Script Date: 3/22/2024 3:32:49 PM ******/
IF OBJECT_ID('MiscellaneousAWSConfigurationDetails', 'U') IS  NULL 
BEGIN
CREATE TABLE [dbo].[MiscellaneousAWSConfigurationDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AWSBucketName] [nvarchar](200) NULL,
	[AWSAccessKey] [varbinary](8000) NULL,
    [AWSSecretKey] [varbinary](8000) NULL,
	[AWSBucketRegion] [nvarchar](150) NULL,
	[LocationURL] [nvarchar](500) NULL,
	[IsActive] [bit] default 0,
	[IsDeleted] [bit] default 0,
	[CreatedBy] [nvarchar](10) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [nvarchar](10) NULL,
	[UpdatedDate] [datetime] NULL
) ON [PRIMARY]
END
GO