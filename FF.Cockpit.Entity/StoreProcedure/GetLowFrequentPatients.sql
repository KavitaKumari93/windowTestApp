IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetLowFrequentPatients]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetLowFrequentPatients]
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

 ALTER PROCEDURE [dbo].[GetLowFrequentPatients]    
@FromDate DATETIME,    
@ToDate DATETIME    
AS    
BEGIN    
    -- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.    
    SET NOCOUNT ON;    
     
    SET @FromDate = ISNULL(@FromDate, (SELECT MIN(Shootingdate) FROM [FotoFinder.Universe].[dbo].ImagesInfoTable));    
     
    SET @ToDate = CONVERT(DATE, @ToDate);  
	
	
     SELECT DISTINCT m.PatientId as PatientId , SessionDate as ShootingDate,
	 row_number() over (partition by PatientId,PatientName,DateOfBirth,SessionDate,BodyScanCount,ExcisionCount,MarkerCount,MicroImageCount,
	 TBMImagesCount,SessionDuration order by PatientId) as RowNbr 
    INTO #temp1
    FROM MergeHighRiskPatientTileData m
    WHERE Flag=1  AND  CONVERT(DATE, SessionDate) NOT BETWEEN @FromDate AND @ToDate 

    SELECT DISTINCT m.PatientId as PatientId , SessionDate as ShootingDate
    INTO #temp2
    FROM MergeHighRiskPatientTileData m
    WHERE Flag=1 AND CONVERT(DATE, SessionDate) BETWEEN @FromDate AND @ToDate 

   Select  t1.PatientId,t1.ShootingDate, 
    row_number() over (partition by t1.PatientId order by t1.PatientId) as RowNbr into #temp3
    FROM #temp1 t1
    LEFT JOIN #temp2 t2 ON t1.PatientId = t2.Patientid
    WHERE t2.patientid IS NULL and t1.RowNbr=1 order by PatientId, ShootingDate desc
	
	--select * from #temp3  where RowNbr=1
    -- CTE to filter out ObjectIDs     
--     WITH FilteredRecords AS (    
    SELECT DISTINCT     
        mt.ObjectId,    
        mt.PatientRecordnumber,     
        mt.FirstName,    
        mt.LastName,    
        mt.MiddleName,    
        mt.DateOfBirth,    
        tbl.shootingDate as ShootingDate ,
		Mt.Phone,
		Mt.Mobile,
		Mt.Email,
        CASE      
    WHEN DATEDIFF(DAY, tbl.shootingDate, GETDATE()) BETWEEN 1 AND 7 THEN     
        CAST(DATEDIFF(DAY,tbl.shootingDate, GETDATE()) AS NVARCHAR) + ' Days'     
    WHEN DATEDIFF(DAY,tbl.shootingDate, GETDATE()) > 7 AND DATEDIFF(DAY,tbl.shootingDate, GETDATE()) < 30 THEN     
        CAST(DATEDIFF(WEEK,tbl.shootingDate, GETDATE()) AS NVARCHAR) + ' Weeks and ' + CAST(DATEDIFF(DAY,tbl.shootingDate, GETDATE()) % 7 AS NVARCHAR) + ' Days'      
    WHEN DATEDIFF(DAY,tbl.shootingDate, GETDATE()) = 30 THEN    
        '1 Month'    
    WHEN DATEDIFF(DAY,tbl.shootingDate, GETDATE()) > 30 AND DATEDIFF(DAY,tbl.shootingDate, GETDATE()) < 365 THEN        
        CASE    
            WHEN DATEDIFF(DAY,tbl.shootingDate, GETDATE()) % 30 = 0 THEN    
                CAST(DATEDIFF(MONTH,tbl.shootingDate, GETDATE()) AS NVARCHAR) + ' Months'    
            ELSE    
                CAST(DATEDIFF(MONTH,tbl.shootingDate, GETDATE()) AS NVARCHAR) + ' Months and ' + CAST(DATEDIFF(DAY,tbl.shootingDate, GETDATE()) % 30 AS NVARCHAR) + ' Days'    
        END    
    WHEN DATEDIFF(DAY,tbl.shootingDate, GETDATE()) = 365 THEN    
        '1 Year'    
    WHEN DATEDIFF(DAY,tbl.shootingDate, GETDATE()) > 365 THEN     
        CASE     
            WHEN DATEDIFF(YEAR,tbl.shootingDate, GETDATE()) = 1 THEN    
                '1 Year'    
            ELSE    
                CAST(DATEDIFF(YEAR,tbl.shootingDate, GETDATE()) AS NVARCHAR) + ' Years'    
        END +     
        CASE     
            WHEN DATEDIFF(DAY,tbl.shootingDate, GETDATE()) % 365 = 30 THEN    
                ' and 1 Month'    
            WHEN DATEDIFF(DAY,tbl.shootingDate, GETDATE()) % 365 > 30 THEN    
                ' and ' + CAST(DATEDIFF(DAY,tbl.shootingDate, GETDATE()) % 365 / 30 AS NVARCHAR) + ' Months'    
            ELSE    
                ''    
        END    
        END AS LastUpdate 
  from MasterTable Mt     
  inner join #temp3 tbl on mt.ObjectId=tbl.PatientId   and RowNbr=1
  order by Shootingdate desc 

  drop table #temp1    
  drop table #temp2    
  drop table #temp3    
END    
GO