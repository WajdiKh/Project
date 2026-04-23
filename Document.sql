SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Transfert].[Document]
(
    [Id]               INT IDENTITY(1,1) NOT NULL,
    [Name]             NVARCHAR(255) NOT NULL,
    [Description]      NVARCHAR(1000) NULL,
    [OriginalFileName] NVARCHAR(255) NOT NULL,
    [FileExtension]    NVARCHAR(20) NOT NULL,
    [ContentType]      NVARCHAR(100) NULL,
    [FileContent]      VARBINARY(MAX) NOT NULL,
    [FileSize]         BIGINT NOT NULL,
    [UploadDate]       DATETIMEOFFSET(7) NOT NULL,
    [ExpiryDate]       DATETIMEOFFSET(7) NOT NULL,
    [OwnerId]          INT NOT NULL,
    [StatutDocumentId] INT NOT NULL,
    [SecurityCode]     NVARCHAR(6) NOT NULL,
    [EncryptionKey]    NVARCHAR(255) NULL,

    CONSTRAINT [PK_Document]
        PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

ALTER TABLE [Transfert].[Document]
ADD CONSTRAINT [DF_Document_UploadDate]
DEFAULT (SYSDATETIMEOFFSET()) FOR [UploadDate];
GO

ALTER TABLE [Transfert].[Document]
ADD CONSTRAINT [FK_Document_Utilisateur_OwnerId]
FOREIGN KEY ([OwnerId]) REFERENCES [Commun].[Utilisateur]([Id]);
GO

ALTER TABLE [Transfert].[Document]
ADD CONSTRAINT [FK_Document_StatutDocument_StatutDocumentId]
FOREIGN KEY ([StatutDocumentId]) REFERENCES [Transfert].[StatutDocument]([Id]);
GO

CREATE NONCLUSTERED INDEX [IX_Document_OwnerId]
ON [Transfert].[Document]([OwnerId]);
GO

CREATE NONCLUSTERED INDEX [IX_Document_StatutDocumentId]
ON [Transfert].[Document]([StatutDocumentId]);
GO

CREATE NONCLUSTERED INDEX [IX_Document_ExpiryDate]
ON [Transfert].[Document]([ExpiryDate]);
GO