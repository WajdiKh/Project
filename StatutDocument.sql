SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Transfert].[StatutDocument](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Code]  NOT NULL,
    [FrenchName]  NOT NULL,
    [EnglishName]  NULL,
    [FrenchDescription]  NOT NULL,
    [EnglishDescription]  NULL,
    [DateCreation]  NOT NULL,
    [DateModification]  NULL,
    [ModificateurId] [int] NULL,
    [CreateurId] [int] NULL,
    [IsActive] [bit] NOT NULL,
CONSTRAINT [PK_StatutDocument] PRIMARY KEY CLUSTERED
(
    [Id] ASC
) WITH (
    PAD_INDEX = OFF,
    STATISTICS_NORECOMPUTE = OFF,
    IGNORE_DUP_KEY = OFF,
    ALLOW_ROW_LOCKS = ON,
    ALLOW_PAGE_LOCKS = ON,
    OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Transfert].[StatutDocument]
ADD DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [Transfert].[StatutDocument]
ADD DEFAULT (SYSDATETIMEOFFSET()) FOR [DateCreation]
GO

ALTER TABLE [Transfert].[StatutDocument] WITH CHECK
ADD CONSTRAINT [FK_StatutDocument_Utilisateur_CreateurId]
FOREIGN KEY([CreateurId])
REFERENCES [Commun].[Utilisateur] ([Id])
GO

ALTER TABLE [Transfert].[StatutDocument]
CHECK CONSTRAINT [FK_StatutDocument_Utilisateur_CreateurId]
GO

ALTER TABLE [Transfert].[StatutDocument] WITH CHECK
ADD CONSTRAINT [FK_StatutDocument_Utilisateur_ModificateurId]
FOREIGN KEY([ModificateurId])
REFERENCES [Commun].[Utilisateur] ([Id])
GO

ALTER TABLE [Transfert].[StatutDocument]
CHECK CONSTRAINT [FK_StatutDocument_Utilisateur_ModificateurId]
GO
INSERT INTO [Transfert].[StatutDocument]
(
    [Code],
    [FrenchName],
    [EnglishName],
    [FrenchDescription],
    [EnglishDescription],
    [DateCreation],
    [IsActive]
)
VALUES
(
    N'Active',
    N'Actif',
    N'Active',
    N'Document actif et disponible au téléchargement.',
    N'Active document available for download.',
    SYSDATETIMEOFFSET(),
    1
),
(
    N'Expired',
    N'Expiré',
    N'Expired',
    N'Document expiré et non disponible au téléchargement.',
    N'Expired document unavailable for download.',
    SYSDATETIMEOFFSET(),
    1
);
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Transfert].[Document](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name]  NOT NULL,
    [Description]  NULL,
    [OriginalFileName]  NOT NULL,
    [FileExtension]  NOT NULL,
    [ContentType]  NULL,
    [FileContent] [varbinary](max) NOT NULL,
    [FileSize] [bigint] NOT NULL,
    [UploadDate]  NOT NULL,
    [ExpiryDate]  NOT NULL,
    [OwnerId] [int] NOT NULL,
    [StatutDocumentId] [int] NOT NULL,
    [SecurityCode]  NOT NULL,
    [EncryptionKey]  NULL,
CONSTRAINT [PK_Document] PRIMARY KEY CLUSTERED
(
    [Id] ASC
) WITH (
    PAD_INDEX = OFF,
    STATISTICS_NORECOMPUTE = OFF,
    IGNORE_DUP_KEY = OFF,
    ALLOW_ROW_LOCKS = ON,
    ALLOW_PAGE_LOCKS = ON,
    OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [Transfert].[Document]
ADD DEFAULT (SYSDATETIMEOFFSET()) FOR [UploadDate]
GO

ALTER TABLE [Transfert].[Document] WITH CHECK
ADD CONSTRAINT [FK_Document_Utilisateur_OwnerId]
FOREIGN KEY([OwnerId])
REFERENCES [Commun].[Utilisateur] ([Id])
GO

ALTER TABLE [Transfert].[Document]
CHECK CONSTRAINT [FK_Document_Utilisateur_OwnerId]
GO

ALTER TABLE [Transfert].[Document] WITH CHECK
ADD CONSTRAINT [FK_Document_StatutDocument_StatutDocumentId]
FOREIGN KEY([StatutDocumentId])
REFERENCES [Transfert].[StatutDocument] ([Id])
GO

ALTER TABLE [Transfert].[Document]
CHECK CONSTRAINT [FK_Document_StatutDocument_StatutDocumentId]
GO

CREATE NONCLUSTERED INDEX [IX_Document_OwnerId]
ON [Transfert].[Document]([OwnerId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Document_StatutDocumentId]
ON [Transfert].[Document]([StatutDocumentId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Document_ExpiryDate]
ON [Transfert].[Document]([ExpiryDate] ASC)
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Transfert].[DocumentShare](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [DocumentId] [int] NOT NULL,
    [SharedWithUserId] [int] NOT NULL,
    [Email]  NOT NULL,
    [SharedDate]  NOT NULL,
    [ExpiryDate]  NOT NULL,
CONSTRAINT [PK_DocumentShare] PRIMARY KEY CLUSTERED
(
    [Id] ASC
) WITH (
    PAD_INDEX = OFF,
    STATISTICS_NORECOMPUTE = OFF,
    IGNORE_DUP_KEY = OFF,
    ALLOW_ROW_LOCKS = ON,
    ALLOW_PAGE_LOCKS = ON,
    OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Transfert].[DocumentShare]
ADD DEFAULT (SYSDATETIMEOFFSET()) FOR [SharedDate]
GO

ALTER TABLE [Transfert].[DocumentShare] WITH CHECK
ADD CONSTRAINT [FK_DocumentShare_Document_DocumentId]
FOREIGN KEY([DocumentId])
REFERENCES [Transfert].[Document] ([Id])
GO

ALTER TABLE [Transfert].[DocumentShare]
CHECK CONSTRAINT [FK_DocumentShare_Document_DocumentId]
GO

ALTER TABLE [Transfert].[DocumentShare] WITH CHECK
ADD CONSTRAINT [FK_DocumentShare_Utilisateur_SharedWithUserId]
FOREIGN KEY([SharedWithUserId])
REFERENCES [Commun].[Utilisateur] ([Id])
GO

ALTER TABLE [Transfert].[DocumentShare]
CHECK CONSTRAINT [FK_DocumentShare_Utilisateur_SharedWithUserId]
GO

CREATE NONCLUSTERED INDEX [IX_DocumentShare_DocumentId]
ON [Transfert].[DocumentShare]([DocumentId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_DocumentShare_SharedWithUserId]
ON [Transfert].[DocumentShare]([SharedWithUserId] ASC)
GO

