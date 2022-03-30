CREATE     FUNCTION [dbo].[get_pilulier] ( @nbjourperiode BIGINT )
RETURNS TABLE
AS
RETURN
		(SELECT MAX(CAST(T4.nom AS nVARCHAR(255)))			                    AS medicament,
		        CAST(AVG (T2.quantite)		AS BIGINT) 	AS stock,
		        CAST(SUM (T3.prise)			AS BIGINT) 	AS aprendreparjour,
		        CAST(SUM(T3.prise * 7)		AS BIGINT)  AS nbpourperiode
                  FROM  [dbo].[ordonnances]				AS T1, 
                        [dbo].[armoires-stock]			AS T2 , 
                        [dbo].[medicaments-prescrits]	AS T3, 
                        [dbo].[medicaments]				AS T4
                  WHERE 	
                               T1.dateexpire >= CAST(GETDATE() AS DATE) AND T1.datecree < CAST(GETDATE() AS DATE)
                          AND (T3.medicamentid = T4.id) AND (CHARINDEX(CAST(T2.medicamentid AS nVARCHAR(255)), CAST(T4.nom AS nVARCHAR(255))) > 0) 
                          AND (T1.id = T3.ordonnanceid) 
                  GROUP BY  CAST(T4.nom AS nVARCHAR(255))
                  --ORDER BY  CAST(T4.nom AS nVARCHAR(255))
		)