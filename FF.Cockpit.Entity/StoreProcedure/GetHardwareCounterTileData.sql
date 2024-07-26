USE [FotoFinder.Universe]
GO
/****** Object:  StoredProcedure [dbo].[GetHardwareCounterTileData]    Script Date: 17-05-2023 11:09:56 ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetHardwareCounterTileData]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetHardwareCounterTileData]
    	as
		begin
			select 1
		end')
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetHardwareCounterTileData] 
AS
BEGIN
	SET NOCOUNT ON;
	Select CAST(ROW_NUMBER() OVER (ORDER BY SerialNumber) AS VARCHAR(500)) AS RowNumber, 
	MT.Manufacturer,MT.Body,
	(MT.CurrentCounter - IsNULL(MT.LAStMaintenanceCounter,0)) AS Counter,
	SIT.MaxImageReleASes AS NextService,
	CONVERT(varchar, ISNULL(MT.LAStMaintenance,MT.FirstUsage), 104) AS LAStServiceDate,
	CONVERT(varchar, DATEADD(DAY, SIT.MaxServiceIntervalDays, ISNULL(MT.LAStMaintenance,MT.FirstUsage)), 104) AS NextServiceDate,
	ROUND((MT.CurrentCounter - ISNULL(MT.LAStMaintenanceCounter,0)) * 100 / SIT.MaxImageReleASes , 1) AS Status,
	'Cleaning and General service is required.' AS Description
	from [FotoFinder.Universe].[dbo].MaintenanceTable MT
Left Join [FotoFinder.Universe].[dbo].ServiceIntervalTable SIT ON SIT.Manufacturer=MT.Manufacturer AND SIT.DeviceModel=MT.Body
END

GO

