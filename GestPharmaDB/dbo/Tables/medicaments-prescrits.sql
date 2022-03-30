CREATE TABLE [dbo].[medicaments-prescrits] (
    [ordonnanceid] BIGINT NULL,
    [medicamentid] BIGINT NULL,
    [quantite]     BIGINT NULL,
    [prise]        BIGINT NULL,
    CONSTRAINT [medicaments-prescrits_medicamentid_fkey] FOREIGN KEY ([medicamentid]) REFERENCES [dbo].[medicaments] ([id]),
    CONSTRAINT [medicaments-prescrits_ordonnanceid_fkey] FOREIGN KEY ([ordonnanceid]) REFERENCES [dbo].[ordonnances] ([id])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'medicaments-prescrits';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'medicaments-prescrits', @level2type = N'COLUMN', @level2name = N'ordonnanceid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'medicaments-prescrits', @level2type = N'COLUMN', @level2name = N'medicamentid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'medicaments-prescrits', @level2type = N'COLUMN', @level2name = N'quantite';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'medicaments-prescrits', @level2type = N'COLUMN', @level2name = N'prise';

