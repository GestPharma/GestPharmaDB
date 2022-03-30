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

COMMIT;
GO
