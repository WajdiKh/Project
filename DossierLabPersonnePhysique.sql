CREATE TABLE [Lab].[DossierLabPersonnePhysique] (
    [Id]                    INT IDENTITY (1, 1) NOT NULL,
    [PersonnePhysiqueLabId] INT NOT NULL,
    [DossierLabId]          INT NOT NULL,
    [TypeListeCriblageId] [int] NULL,
    [IsDeclarationTracfin] BIT Not null,

    CONSTRAINT [PK_DossierLabPersonnePhysique] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_DossierLabPersonnePhysique_DossierLab_DossierLabId] FOREIGN KEY ([DossierLabId]) REFERENCES [Lab].[DossierLab] ([Id]),
    CONSTRAINT [FK_DossierLabPersonnePhysique_TypeListeCriblage_TypeListeCriblageId] FOREIGN KEY ([TypeListeCriblageId]) REFERENCES [Lab].[TypeListeCriblage] ([Id]),
    CONSTRAINT [FK_DossierLabPersonnePhysique_PersonnePhysiqueLab_PersonnePhysiqueLabId] FOREIGN KEY ([PersonnePhysiqueLabId]) REFERENCES [Lab].[PersonnePhysiqueLab] ([Id])
);

GO
CREATE NONCLUSTERED INDEX [IX_DossierLabPersonnePhysique_DossierLabId]
    ON [Lab].[DossierLabPersonnePhysique]([DossierLabId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_DossierLabPersonnePhysique_TypeListeCriblageId]
    ON [Lab].[DossierLabPersonnePhysique]([TypeListeCriblageId] ASC);
GO
CREATE NONCLUSTERED INDEX [IX_DossierLabPersonnePhysique_PersonnePhysiqueLabId]
    ON [Lab].[DossierLabPersonnePhysique]([PersonnePhysiqueLabId] ASC);

     
