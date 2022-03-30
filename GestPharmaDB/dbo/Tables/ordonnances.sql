CREATE TABLE [dbo].[ordonnances] (
    [id]          BIGINT IDENTITY (1, 1) NOT NULL,
    [nom]         TEXT   NULL,
    [datecree]    DATE   NULL,
    [dateexpire]  DATE   NULL,
    [medecinid]   BIGINT NULL,
    [pharmacieid] BIGINT NULL,
    CONSTRAINT [ordonnances_pkey] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [ordonnances_medecinid_fkey] FOREIGN KEY ([medecinid]) REFERENCES [dbo].[medecins] ([id_medecin]),
    CONSTRAINT [ordonnances_pharmacieid_fkey] FOREIGN KEY ([pharmacieid]) REFERENCES [dbo].[pharmacies] ([id])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ordonnances';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ordonnances', @level2type = N'COLUMN', @level2name = N'id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ordonnances', @level2type = N'COLUMN', @level2name = N'nom';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ordonnances', @level2type = N'COLUMN', @level2name = N'datecree';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ordonnances', @level2type = N'COLUMN', @level2name = N'dateexpire';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ordonnances', @level2type = N'COLUMN', @level2name = N'medecinid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ordonnances', @level2type = N'COLUMN', @level2name = N'pharmacieid';

