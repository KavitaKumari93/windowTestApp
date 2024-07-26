USE [FotoFinder.Universe]
GO
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetFotoFinderDevicesInformation]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetFotoFinderDevicesInformation]
    	as
		begin
			select 1
		end')
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetFotoFinderDevicesInformation]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec GetFotoFinderDevicesInformation

ALTER PROCEDURE [dbo].[GetFotoFinderDevicesInformation]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

 select MT.Manufacturer,
 MT.SerialNumber,
 CurrentCounter,
 DeviceModel,
 FirstUsage,
 LAStMaintenance, 
 LAStMaintenanceCounter,
 LAStUsage,
 MaxImageReleASes,
 MaxServiceIntervalDays
 from 
 [FotoFinder.Universe].[dbo].ServiceIntervalTable AS ST 
 inner Join [FotoFinder.Universe].[dbo].MaintenanceTable AS MT on ST.DeviceModel=MT.Body and MT.Manufacturer=ST.Manufacturer

END
GO
