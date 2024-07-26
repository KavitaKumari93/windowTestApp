
/****** Object:  StoredProcedure [dbo].[SP_MergeMasterTableData]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[SP_MergeMasterTableData]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[SP_MergeMasterTableData]
    	AS
		begin
			select 1
		end')
END
Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_MergeMasterTableData]         
AS      
BEGIN      
  DECLARE @resultCount INT;            
  SET @resultCount = (SELECT COUNT(*) FROM [FotoFinder.Universe].dbo.MasterTable)      
 IF(@resultCount > 0)      
    BEGIN      
		IF NOT EXISTS (SELECT 1 FROM [FotoFinder.Cockpit].dbo.MasterTable)      
	   BEGIN      
		INSERT INTO [FotoFinder.Cockpit].dbo.MasterTable (ObjectId, ProjectCreator, LastUpdate, ObjectGUID,  LastName, FirstName, MiddleName, DateOfBirth, Gender, BodyHeight, Deleted, PatientShootingDate, PatientStudy, PatientRecall, PatientRecordnumber, Address1, Address2, City, [State], Zip, Phone, Mobile, Email, Fax, WebvaultId, ExtraData, Comment, SkinTypeId)      
		SELECT ObjectId, ProjectCreator, LastUpdate, ObjectGUID,  LastName, FirstName, MiddleName, DateOfBirth, Gender, BodyHeight, Deleted, PatientShootingDate, PatientStudy, PatientRecall, PatientRecordnumber, Address1, Address2, City, [State], Zip, Phone, Mobile, Email, Fax, WebvaultId, ExtraData, Comment, SkinTypeId     
		FROM [FotoFinder.Universe].dbo.MasterTable;      
	   END      
		ELSE      
	   BEGIN      
		MERGE INTO [FotoFinder.Cockpit].dbo.MasterTable AS target      
		USING [FotoFinder.Universe].dbo.MasterTable AS source      
		ON target.ObjectId = source.ObjectId
		WHEN MATCHED THEN      
		UPDATE SET       
		target.ObjectId 			= source.ObjectId,
		target.ProjectCreator		= source.ProjectCreator,
		target.LastUpdate           = source.LastUpdate,
		target.ObjectGUID           = source.ObjectGUID,
		--target.[TimeStamp]        	= source.[TimeStamp],
		target.LastName             = source.LastName,
		target.FirstName            = source.FirstName,
		target.MiddleName           = source.MiddleName,
		target.DateOfBirth          = source.DateOfBirth,
		target.Gender               = source.Gender,
		target.BodyHeight           = source.BodyHeight,
		target.Deleted              = source.Deleted,
		target.PatientShootingDate  = source.PatientShootingDate,
		target.PatientStudy         = source.PatientStudy,
		target.PatientRecall        = source.PatientRecall,
		target.PatientRecordnumber  = source.PatientRecordnumber,
		target.Address1             = source.Address1,
		target.Address2             = source.Address2,
		target.City                 = source.City,
		target.[State]              = source.[State],
		target.Zip                  = source.Zip,
		target.Phone                = source.Phone,
		target.Mobile               = source.Mobile,
		target.Email                = source.Email,
		target.Fax                  = source.Fax,
		target.WebvaultId           = source.WebvaultId,
		target.ExtraData            = source.ExtraData,
		target.Comment              = source.Comment,
		target.SkinTypeId           = source.SkinTypeId      
		WHEN NOT MATCHED THEN      
		 INSERT (ObjectId, ProjectCreator, LastUpdate, ObjectGUID, LastName, FirstName, MiddleName, DateOfBirth,Gender, BodyHeight, Deleted, PatientShootingDate, PatientStudy, 
				PatientRecall, PatientRecordnumber, Address1, Address2, City, State, Zip, Phone, Mobile, Email, Fax, WebvaultId, ExtraData, Comment, SkinTypeId)    
		 VALUES (source.ObjectId, source.ProjectCreator, source.LastUpdate, source.ObjectGUID, source.LastName, source.FirstName, source.MiddleName, source.DateOfBirth,
				source.Gender, source.BodyHeight, source.Deleted, source.PatientShootingDate, source.PatientStudy, source.PatientRecall, source.PatientRecordnumber, source.Address1, 
				source.Address2, source.City, source.State, source.Zip, source.Phone, source.Mobile, source.Email, source.Fax, source.WebvaultId, source.ExtraData, source.Comment, source.SkinTypeId);      
	   END      
   END            
END   
GO
