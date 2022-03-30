CREATE   FUNCTION [dbo].[get_contenu] ( @armoireid BIGINT )
RETURNS TABLE
AS
RETURN
		(SELECT		CAST(conso.Lib_medoc		AS nVARCHAR(255))	AS Medicament,
					CAST(conso.Nb_Medicament	AS BIGINT)			AS Stock
					FROM (SELECT 
							(select nom from [dbo].[medicaments] where id = actif.ac_medicamentid)	AS Lib_medoc,
							CAST(sum(actif.ac_quantite)		AS BIGINT)			AS Nb_Medicament,
							CAST(sum(actif.ac_prise)		AS BIGINT)			AS Deja_Pris
						FROM 
							  (SELECT 
							
									CAST(medocs.ordonnanceid	AS BIGINT)	AS ac_ordonnanceid,
									CAST(medocs.medicamentid	AS BIGINT)	AS ac_medicamentid,
									CAST(medocs.quantite		AS BIGINT)	AS ac_quantite,
									CAST(medocs.prise			AS BIGINT)	AS ac_prise
								FROM [dbo].[ordonnances] O
								inner join [dbo].[medicaments-prescrits] AS medocs ON medocs.ordonnanceid = O.id 
								where dateexpire > CAST(GETDATE() AS DATE) AND datecree < CAST(GETDATE() AS DATE)
							  ) AS actif
						inner join [dbo].[medicaments] mm ON mm.id = actif.ac_medicamentid  
 						group by actif.ac_medicamentid
						) as conso
		)