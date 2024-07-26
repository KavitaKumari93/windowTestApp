/****** Object:  StoredProcedure [dbo].[SP_MergeHighRiskPatientTileData]    Script Date: 12/12/2023 3:14:47 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[SP_MergeHighRiskPatientTileData]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[SP_MergeHighRiskPatientTileData]
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
ALTER PROCEDURE [dbo].[SP_MergeHighRiskPatientTileData]             
AS          
BEGIN          
  DECLARE @resultCount INT;          
  CREATE TABLE #HighRiskPatient          
   (          
		PatientId INT,          
		PatientName VARCHAR(100),         
		DateOfBirth DATETIME,          
		SessionDate DATETIME,         
		BodyScanCount INT,          
		MarkerCount INT,          
		MicroImageCount INT,          
		ExcisionCount INT,          
		SessionDuration INT,    
		TBMImagesCount INT,  
		Flag BIT    
   )          
  INSERT INTO #HighRiskPatient EXEC [FotoFinder.Cockpit].dbo.GetHighRiskPatientTileDataByDateRange_ForJob;          
  --SELECT * FROM [FotoFinder.Cockpit].dbo.MergeHighRiskPatientTileData         
  SET @resultCount = (SELECT COUNT(*) FROM #HighRiskPatient)          
 IF(@resultCount > 0)          
    BEGIN          
        IF NOT EXISTS (SELECT 1 FROM [FotoFinder.Cockpit].dbo.MergeHighRiskPatientTileData)          
   BEGIN          
    INSERT INTO [FotoFinder.Cockpit].dbo.MergeHighRiskPatientTileData (PatientId, PatientName, DateOfBirth, SessionDate, BodyScanCount, MarkerCount, MicroImageCount, ExcisionCount, SessionDuration,TBMImagesCount,Flag)          
    SELECT PatientId, PatientName, DateOfBirth, SessionDate, BodyScanCount, MarkerCount, MicroImageCount, ExcisionCount, SessionDuration ,TBMImagesCount,Flag      
    FROM #HighRiskPatient;          
   END          
        ELSE          
	BEGIN          
		MERGE INTO [FotoFinder.Cockpit].dbo.MergeHighRiskPatientTileData AS target          
			USING #HighRiskPatient AS source          
			ON target.PatientId = source.PatientId  AND  target.SessionDate = source.SessionDate AND target.Flag = source.Flag  
		WHEN MATCHED THEN          
			UPDATE SET           
				target.PatientId = source.PatientId,           
				target.PatientName = source.PatientName,          
				target.DateOfBirth = source.DateOfBirth,          
				target.SessionDate = source.SessionDate,          
				target.BodyScanCount = source.BodyScanCount,          
				target.MarkerCount = source.MarkerCount,          
				target.MicroImageCount = source.MicroImageCount,          
				target.ExcisionCount = source.ExcisionCount,          
				target.SessionDuration = source.SessionDuration,  
				target.TBMImagesCount=source.TBMImagesCount,  
				target.Flag = source.Flag    
		WHEN NOT MATCHED BY TARGET THEN
			INSERT (PatientId, PatientName, DateOfBirth, SessionDate, BodyScanCount, MarkerCount, MicroImageCount, ExcisionCount, SessionDuration,TBMImagesCount,Flag)          
			VALUES (source.PatientId, source.PatientName, source.DateOfBirth, source.SessionDate, source.BodyScanCount, source.MarkerCount, source.MicroImageCount, source.ExcisionCount, source.SessionDuration,source.TBMImagesCount,source.Flag)
		WHEN NOT MATCHED BY SOURCE THEN
			DELETE;
   END          
    END          
 DROP TABLE #HighRiskPatient   
 ------------------------------------------------
 -----------DUMP EXCISED COUNT DATA--------------
 ------------------------------------------------
 DECLARE @excisedDumpCount INT;    
 CREATE TABLE #ExcisedData
   (          
    PatientId INT,          
	MarkNo INT,         
    ExcisedDate DATETIME,          
    MarkText INT
   )          
  INSERT INTO #ExcisedData EXEC [FotoFinder.Cockpit].dbo.GetExcisionCount;         
  SET @excisedDumpCount = (SELECT COUNT(*) FROM #ExcisedData)          
	IF(@excisedDumpCount > 0)          
	BEGIN          
		IF NOT EXISTS (SELECT 1 FROM [FotoFinder.Cockpit].dbo.ExcisedMarkersData)          
			BEGIN          
					INSERT INTO [FotoFinder.Cockpit].dbo.ExcisedMarkersData (PatientId, MarkNo, ExcisedDate, MarkText)
					SELECT PatientId, MarkNo, ExcisedDate, MarkText FROM #ExcisedData;          
			END          
		ELSE          
			BEGIN          
				MERGE INTO [FotoFinder.Cockpit].dbo.ExcisedMarkersData AS target USING #ExcisedData AS source ON
				target.PatientId = source.PatientId AND 
				target.MarkNo = source.MarkNo AND 
				target.ExcisedDate = source.ExcisedDate AND 
				target.MarkText = source.MarkText
				
				WHEN MATCHED THEN
					UPDATE SET
						target.PatientId = source.PatientId,           
						target.MarkNo = source.MarkNo,          
						target.ExcisedDate = source.ExcisedDate,          
						target.MarkText = source.MarkText 
				WHEN NOT MATCHED BY TARGET THEN
					INSERT (PatientId, MarkNo, ExcisedDate, MarkText) VALUES (source.PatientId, source.MarkNo, source.ExcisedDate, source.MarkText)
				WHEN NOT MATCHED BY SOURCE THEN
					DELETE;

			END
	END
	DROP TABLE #ExcisedData
END
GO
