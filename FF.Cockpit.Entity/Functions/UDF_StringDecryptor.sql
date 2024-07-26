/****** Object:  UserDefinedFunction [dbo].[UDF_StringDecryptor]    Script Date: 04-04-2024 11:08:35 ******/

IF EXISTS ( SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UDF_StringDecryptor]') AND type IN (N'FN', N'IF', N'TF', N'FS', N'FT') )
BEGIN
    EXEC dbo.sp_executesql @statement = N' DROP FUNCTION [dbo].[UDF_StringDecryptor]'
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create FUNCTION [dbo].[UDF_StringDecryptor]
(
@encrypt varbinary(8000)
)
RETURNS nvarchar(4000)
AS
BEGIN
DECLARE
@decrypt varbinary(4000),
@key nvarchar(max)= '# _@hc@_!product!@SDM!'  
SET
@decrypt = DECRYPTBYPASSPHRASE
(
@key,@encrypt
)
  RETURN 
(
@decrypt
)
END
GO