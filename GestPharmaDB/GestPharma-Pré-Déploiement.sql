SET DATEFORMAT dmy;
USE BDPM;
BEGIN TRANSACTION;
DROP TABLE IF EXISTS [dbo].[medicaments-prescrits];
DROP TABLE IF EXISTS [dbo].[armoires-stock];

DROP TABLE IF EXISTS [dbo].[ordonnances];
DROP TABLE IF EXISTS [dbo].[medicaments];

DROP TABLE IF EXISTS [dbo].[medecins];
DROP TABLE IF EXISTS [dbo].[pharmacies];
DROP TABLE IF EXISTS [dbo].[armoires];

DROP FUNCTION  IF EXISTS [dbo].[check_if_armoires_exists];
DROP FUNCTION  IF EXISTS [dbo].[check_if_medecins_exists];
DROP FUNCTION  IF EXISTS [dbo].[check_if_medicaments_exists];
DROP FUNCTION  IF EXISTS [dbo].[check_if_pharmacies_exists];

DROP FUNCTION  IF EXISTS [dbo].[get_contenu];
DROP FUNCTION  IF EXISTS [dbo].[get_pilulier];

COMMIT;
GO
