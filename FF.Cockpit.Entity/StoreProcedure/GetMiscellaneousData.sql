USE [FotoFinder.Universe]
GO
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[GetMiscellaneousData]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[GetMiscellaneousData]
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

/*
-- =============================================
-- Title:       GetMiscellaneousData
-- Author:      Shareef Mohammad
-- Create date: 18/09/2023
-- Description: Get the  data of GetMiscellaneousData
-- Purpose:     FotoFinder Application Support
-- Application: FotoFinder
-- =============================================

-- Modifications:
-- =============================================
Date                   Developer                    Description    
-- =============================================
*/
ALTER PROCEDURE [dbo].[GetMiscellaneousData]
    
AS
BEGIN

-- SET NOCOUNT ON added to prevent extra result sets from
    SET NOCOUNT ON
    SET transaction isolation level read uncommitted

     SELECT * FROM MiscellaneousData WHERE IsDeleted=0
END