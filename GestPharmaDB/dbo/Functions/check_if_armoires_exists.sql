CREATE   FUNCTION [dbo].[check_if_armoires_exists] ( @armoire bigint )
RETURNS BIT
AS
BEGIN
	DECLARE @is_checked BIT;
	IF EXISTS (SELECT 1 
						FROM [dbo].[armoires-stock]
						WHERE [dbo].[armoires-stock].armoireid = @armoire)
		BEGIN SET @is_checked = 1; END
	ELSE
		BEGIN SET @is_checked = 0; END

	RETURN @is_checked;
END