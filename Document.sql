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

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Document_OwnerId_UploadDate'
      AND object_id = OBJECT_ID('[Transfert].[Document]')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Document_OwnerId_UploadDate]
    ON [Transfert].[Document] ([OwnerId], [UploadDate] DESC)
    INCLUDE ([Id], [Name], [Description], [FileExtension], [ContentType], [FileSize], [ExpiryDate], [StatutDocumentId]);
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Document_ExpiryDate'
      AND object_id = OBJECT_ID('[Transfert].[Document]')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Document_ExpiryDate]
    ON [Transfert].[Document] ([ExpiryDate])
    INCLUDE ([Id], [OwnerId], [Name], [OriginalFileName]);
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Document_StatutDocumentId'
      AND object_id = OBJECT_ID('[Transfert].[Document]')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Document_StatutDocumentId]
    ON [Transfert].[Document] ([StatutDocumentId]);
END
GO
