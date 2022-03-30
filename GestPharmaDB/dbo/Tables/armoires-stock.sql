CREATE TABLE [dbo].[armoires-stock] (
    [medicamentid] TEXT   NULL,
    [armoireid]    BIGINT NULL,
    [ordonnanceid] BIGINT NULL,
    [quantite]     BIGINT NULL,
    [mediid]       BIGINT NOT NULL,
    CONSTRAINT [armoires-stock_pkey] PRIMARY KEY CLUSTERED ([mediid] ASC),
    CONSTRAINT [armoires-stock_armoireid_fkey] FOREIGN KEY ([armoireid]) REFERENCES [dbo].[armoires] ([id]),
    CONSTRAINT [armoires-stock_mediid_fkey] FOREIGN KEY ([mediid]) REFERENCES [dbo].[medicaments] ([id]),
    CONSTRAINT [armoires-stock_ordonnanceid_fkey] FOREIGN KEY ([ordonnanceid]) REFERENCES [dbo].[ordonnances] ([id])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'armoires-stock';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'armoires-stock', @level2type = N'COLUMN', @level2name = N'medicamentid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'armoires-stock', @level2type = N'COLUMN', @level2name = N'armoireid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'armoires-stock', @level2type = N'COLUMN', @level2name = N'ordonnanceid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'armoires-stock', @level2type = N'COLUMN', @level2name = N'quantite';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TRIAL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'armoires-stock', @level2type = N'COLUMN', @level2name = N'mediid';

