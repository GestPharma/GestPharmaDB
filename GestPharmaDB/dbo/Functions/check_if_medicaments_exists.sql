CREATE   FUNCTION [dbo].[check_if_medicaments_exists] ( @medoc bigint )
RETURNS BIT
AS
BEGIN
	DECLARE @is_checked BIT;
	IF EXISTS (SELECT 1 
						FROM [dbo].[armoires-stock]
						full JOIN [dbo].[medicaments-prescrits]
						ON ([dbo].[medicaments-prescrits].medicamentid = [dbo].[armoires-stock].mediid )
						WHERE ( [dbo].[armoires-stock].armoireid = @medoc)
								OR
								[dbo].[medicaments-prescrits].medicamentid = @medoc )
		BEGIN SET @is_checked = 1; END
	ELSE
		BEGIN SET @is_checked = 0; END

	RETURN @is_checked;
END