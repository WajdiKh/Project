CREATE TABLE [Lab].[DocumentDossierLab] (
    [Id]                     INT                IDENTITY (1, 1) NOT NULL,
    [DocumentLabId]          INT                NOT NULL,
    [DossierLabId]           INT                NOT NULL,
    [TransmettreTracfin]     BIT                NOT NULL,
    [DocumentName]           NVARCHAR (1000)    NOT NULL,
    [IsDeleted]              BIT                NOT NULL,
    [FileName]               NVARCHAR (1000)    NOT NULL,
    [CategorieDocumentId]    INT                NULL,
    [Precision_if_OtherType] NVARCHAR (400)     NULL,
    [IdentityNumber]         NVARCHAR (400)     NULL,
    [CountryRelaseId]        INT                NULL,
    [DateDocument]           DATETIMEOFFSET (7) NULL,
    [DateValidity]           DATETIMEOFFSET (7) NULL,
    [SourceFileDocument]     NVARCHAR (400)     NULL,
    [LevelCNI]               BIT                NULL,
    [AuthorityRelease]       NVARCHAR (400)     NULL,
    [TypeDocumentLabId]      INT                NULL,
    [PersonneAssocieId] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [PK_DocumentDossierLab] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_DocumentDossierLab_CategorieDocument_CategorieDocumentId] FOREIGN KEY ([CategorieDocumentId]) REFERENCES [Commun].[CategorieDocument] ([Id]),
    CONSTRAINT [FK_DocumentDossierLab_DocumentLab_DocumentLabId] FOREIGN KEY ([DocumentLabId]) REFERENCES [Lab].[DocumentLab] ([Id]),
    CONSTRAINT [FK_DocumentDossierLab_DossierLab_DossierLabId] FOREIGN KEY ([DossierLabId]) REFERENCES [Lab].[DossierLab] ([Id]),
    CONSTRAINT [FK_DocumentDossierLab_Pays_CountryRelaseId] FOREIGN KEY ([CountryRelaseId]) REFERENCES [Commun].[Pays] ([Id]),
    CONSTRAINT [FK_DocumentDossierLab_TypeDocumentLab_TypeDocumentLabId] FOREIGN KEY ([TypeDocumentLabId]) REFERENCES [Lab].[TypeDocumentLab] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_DocumentDossierLab_DocumentLabId]
    ON [Lab].[DocumentDossierLab]([DocumentLabId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DocumentDossierLab_DossierLabId]
    ON [Lab].[DocumentDossierLab]([DossierLabId] ASC);
GO
IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_DocumentDossierLab_PiecesIdentite'
      AND object_id = OBJECT_ID('[Lab].[DocumentDossierLab]')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DocumentDossierLab_PiecesIdentite]
    ON [Lab].[DocumentDossierLab]
    (
        [DossierLabId],
        [PersonneAssocieId],
        [TypeDocumentLabId]
    )
    INCLUDE
    (
        [DocumentLabId]
    )
    WHERE [IsDeleted] = 0;
END
GO