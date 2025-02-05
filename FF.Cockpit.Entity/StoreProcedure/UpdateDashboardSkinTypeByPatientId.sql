/****** Object:  StoredProcedure [dbo].[UpdateDashboardSkinTypeByPatientId]    Script Date: 7/25/2023 5:41:32 PM ******/
IF NOT EXISTS (SELECT
    *
  FROM sys.objects
  WHERE object_id = OBJECT_ID(N'[dbo].[UpdateDashboardSkinTypeByPatientId]')
  AND type IN (N'P', N'PC'))
BEGIN
  EXEC ('CREATE PROCEDURE [dbo].[UpdateDashboardSkinTypeByPatientId]
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
--exec UpdateDashboardSkinTypeByPatientId 7429
-- =============================================
-- Author:		KAVITA
-- Create date: 04 May 2023
-- Description:Update Patient Skin type by patient id

-- MOdified By:		04 May 2023
-- Modified date: 01 May 2023
-- Description:	Update Patient Skin type by patient id
-- =============================================

ALTER PROCEDURE [dbo].[UpdateDashboardSkinTypeByPatientId]
@PatientId int,
@SkinTypeId smallint
AS

BEGIN
	SET NOCOUNT ON;

	Update MasterTable set SkinTypeId=@SkinTypeId 
	where ObjectId=@PatientId
	
END
GO

