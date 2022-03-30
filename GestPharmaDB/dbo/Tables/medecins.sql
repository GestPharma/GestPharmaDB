CREATE TABLE [dbo].[medecins] (
    [id_medecin] BIGINT IDENTITY (1, 1) NOT NULL,
    [nom]        TEXT   NULL,
    [inami]      TEXT   NULL,
    [rue]        TEXT   NULL,
    [telephone]  TEXT   NULL,
    [gsm]        TEXT   NULL,
    [fax]        TEXT   NULL,
    [email]      TEXT   NULL,
    [ville]      TEXT   NULL,
    CONSTRAINT [medecins_pkey] PRIMARY KEY CLUSTERED ([id_medecin] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'medecins';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'medecins', @level2type = N'COLUMN', @level2name = N'id_medecin';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'medecins', @level2type = N'COLUMN', @level2name = N'nom';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'medecins', @level2type = N'COLUMN', @level2name = N'inami';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'medecins', @level2type = N'COLUMN', @level2name = N'rue';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'medecins', @level2type = N'COLUMN', @level2name = N'telephone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'medecins', @level2type = N'COLUMN', @level2name = N'gsm';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'medecins', @level2type = N'COLUMN', @level2name = N'fax';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'medecins', @level2type = N'COLUMN', @level2name = N'email';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'medecins', @level2type = N'COLUMN', @level2name = N'ville';

