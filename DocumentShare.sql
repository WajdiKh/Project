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

    CONSTRAINT [PK_DocumentShare]
        PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

ALTER TABLE [Transfert].[DocumentShare]
ADD CONSTRAINT [DF_DocumentShare_SharedDate]
DEFAULT (SYSDATETIMEOFFSET()) FOR [SharedDate];
GO

ALTER TABLE [Transfert].[DocumentShare]
ADD CONSTRAINT [FK_DocumentShare_Document_DocumentId]
FOREIGN KEY ([DocumentId]) REFERENCES [Transfert].[Document]([Id]);
GO

ALTER TABLE [Transfert].[DocumentShare]
ADD CONSTRAINT [FK_DocumentShare_Utilisateur_SharedWithUserId]
FOREIGN KEY ([SharedWithUserId]) REFERENCES [Commun].[Utilisateur]([Id]);
GO

CREATE NONCLUSTERED INDEX [IX_DocumentShare_DocumentId]
ON [Transfert].[DocumentShare]([DocumentId]);
GO

CREATE NONCLUSTERED INDEX [IX_DocumentShare_SharedWithUserId]
ON [Transfert].[DocumentShare]([SharedWithUserId]);
GO