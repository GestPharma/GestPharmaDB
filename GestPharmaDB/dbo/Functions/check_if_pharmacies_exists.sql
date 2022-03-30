CREATE   FUNCTION [dbo].[check_if_pharmacies_exists] ( @pharma bigint )
RETURNS BIT
AS
BEGIN
	DECLARE @is_checked BIT;
	IF EXISTS (SELECT 1 
						FROM [dbo].[ordonnances]
						WHERE [dbo].[ordonnances].pharmacieid = @pharma)
		BEGIN SET @is_checked = 1; END
	ELSE
		BEGIN SET @is_checked = 0; END

	RETURN @is_checked;
END