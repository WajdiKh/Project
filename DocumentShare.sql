SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Transfert].[DocumentShare]
(
    [Id]               INT IDENTITY(1,1) NOT NULL,
    [DocumentId]       INT NOT NULL,
    [SharedWithUserId] INT NOT NULL,
    [Email]            NVARCHAR(320) NOT NULL,
    [SharedDate]       DATETIMEOFFSET(7) NOT NULL,
    [ExpiryDate]       DATETIMEOFFSET(7) NOT NULL,
    [CreateurId] INT NULL,
    CONSTRAINT [PK_DocumentShare]
        PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_DocumentShare_DocumentId'
      AND object_id = OBJECT_ID('[Transfert].[DocumentShare]')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DocumentShare_DocumentId]
    ON [Transfert].[DocumentShare] ([DocumentId])
    INCLUDE ([Id], [Email], [SharedWithUserId], [CreateurId], [SharedDate], [ExpiryDate]);
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_DocumentShare_SharedWithUserId_ExpiryDate'
      AND object_id = OBJECT_ID('[Transfert].[DocumentShare]')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DocumentShare_SharedWithUserId_ExpiryDate]
    ON [Transfert].[DocumentShare] ([SharedWithUserId], [ExpiryDate])
    INCLUDE ([Id], [DocumentId], [Email], [SharedDate]);
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_DocumentShare_Email_ExpiryDate'
      AND object_id = OBJECT_ID('[Transfert].[DocumentShare]')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DocumentShare_Email_ExpiryDate]
    ON [Transfert].[DocumentShare] ([Email], [ExpiryDate])
    INCLUDE ([Id], [DocumentId], [SharedWithUserId], [SharedDate]);
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_DocumentShare_DocumentId_SharedWithUserId_Email'
      AND object_id = OBJECT_ID('[Transfert].[DocumentShare]')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DocumentShare_DocumentId_SharedWithUserId_Email]
    ON [Transfert].[DocumentShare] ([DocumentId], [SharedWithUserId], [Email])
    INCLUDE ([Id], [SharedDate], [ExpiryDate]);
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_DocumentShare_CreateurId'
      AND object_id = OBJECT_ID('[Transfert].[DocumentShare]')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DocumentShare_CreateurId]
    ON [Transfert].[DocumentShare] ([CreateurId]);
END
GO
