/****** Object:  StoredProcedure [dbo].[GetSessionYearsDataByPatientId]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetSessionYearsDataByPatientId]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetSessionYearsDataByPatientId]
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
ALTER PROCEDURE [dbo].[GetSessionYearsDataByPatientId]   
@PatientId int, @selectedFilterDate DateTime  
  
AS  
  
BEGIN  
 SET NOCOUNT ON;  
  SET @selectedFilterDate = CONVERT(DATE , @selectedFilterDate);  
  
  Select 0 AS SessionYear,'All Years' AS SessionYearName  
  
  UNION ALL  
    
  SELECT YEAR(shootingdate) AS SessionYear,CONVERT(varchar(10), YEAR(shootingdate)) AS SessionYearName  
  FROM [FotoFinder.Universe].[dbo].ImagesInfoTable WHERE objectid=@PatientId AND Deleted=0 AND (@selectedFilterDate is null or CONVERT(DATE,shootingdate) > @selectedFilterDate)  
  GROUP BY YEAR(shootingdate)   
END  
GO
