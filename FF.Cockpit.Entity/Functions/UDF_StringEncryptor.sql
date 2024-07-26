/****** Object:  UserDefinedFunction [dbo].[UDF_StringEncryptor]    Script Date: 04-04-2024 11:09:03 ******/
IF EXISTS ( SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UDF_StringEncryptor]') AND type IN (N'FN', N'IF', N'TF', N'FS', N'FT') )
BEGIN
    EXEC dbo.sp_executesql @statement = N' DROP FUNCTION [dbo].[UDF_StringEncryptor]'
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE
   FUNCTION [dbo].[UDF_StringEncryptor]
(
@str nvarchar(4000)
)
RETURNS varbinary
(8000)
AS
BEGIN
DECLARE
@encrypt varbinary
(8000),@key nvarchar(max)='# _@hc@_!product!@SDM!'
SET
@encrypt = ENCRYPTBYPASSPHRASE
(
@key,@str
)
RETURN 
(
@encrypt
)
END
GO
