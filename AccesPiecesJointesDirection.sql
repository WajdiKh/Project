IF OBJECT_ID('[Commun].[AccesPiecesJointesDirection]', 'U') IS NULL
BEGIN
    CREATE TABLE [Commun].[AccesPiecesJointesDirection]
    (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [DirectionSourceId] INT NOT NULL,
        [DirectionCibleId] INT NOT NULL,
        [TypeDocumentLabId] INT NOT NULL,
        [AutoriserRecherche] BIT NOT NULL CONSTRAINT [DF_AccesPiecesJointesDirection_AutoriserRecherche] DEFAULT(0),
        [AutoriserTelechargement] BIT NOT NULL CONSTRAINT [DF_AccesPiecesJointesDirection_AutoriserTelechargement] DEFAULT(0),
        [DateCreation] DATETIMEOFFSET(7) NOT NULL CONSTRAINT [DF_AccesPiecesJointesDirection_DateCreation] DEFAULT(SYSDATETIMEOFFSET()),
        [DateModification] DATETIMEOFFSET(7) NULL,
        [CreateurId] INT NULL,
        [ModificateurId] INT NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_AccesPiecesJointesDirection_IsActive] DEFAULT(1),

        CONSTRAINT [PK_AccesPiecesJointesDirection] PRIMARY KEY CLUSTERED ([Id] ASC),

        CONSTRAINT [FK_AccesPiecesJointesDirection_Direction_Source]
            FOREIGN KEY ([DirectionSourceId]) REFERENCES [Commun].[Direction]([Id]),

        CONSTRAINT [FK_AccesPiecesJointesDirection_Direction_Cible]
            FOREIGN KEY ([DirectionCibleId]) REFERENCES [Commun].[Direction]([Id]),

        CONSTRAINT [FK_AccesPiecesJointesDirection_TypeDocumentLab]
            FOREIGN KEY ([TypeDocumentLabId]) REFERENCES [Lab].[TypeDocumentLab]([Id]),

        CONSTRAINT [FK_AccesPiecesJointesDirection_Utilisateur_Createur]
            FOREIGN KEY ([CreateurId]) REFERENCES [Commun].[Utilisateur]([Id]),

        CONSTRAINT [FK_AccesPiecesJointesDirection_Utilisateur_Modificateur]
            FOREIGN KEY ([ModificateurId]) REFERENCES [Commun].[Utilisateur]([Id]),

        CONSTRAINT [CK_AccesPiecesJointesDirection_DirectionsDifferentes]
            CHECK ([DirectionSourceId] <> [DirectionCibleId])
    );
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'UX_AccesPiecesJointesDirection_Source_Cible_TypeDocument'
      AND object_id = OBJECT_ID('[Commun].[AccesPiecesJointesDirection]')
)
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [UX_AccesPiecesJointesDirection_Source_Cible_TypeDocument]
    ON [Commun].[AccesPiecesJointesDirection]
    (
        [DirectionSourceId],
        [DirectionCibleId],
        [TypeDocumentLabId]
    );
END
GO
IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_AccesPiecesJointesDirection_Recherche'
      AND object_id = OBJECT_ID('[Commun].[AccesPiecesJointesDirection]')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_AccesPiecesJointesDirection_Recherche]
    ON [Commun].[AccesPiecesJointesDirection]
    (
        [DirectionSourceId],
        [AutoriserRecherche],
        [IsActive],
        [DirectionCibleId],
        [TypeDocumentLabId]
    );
END
GO
IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'IX_AccesPiecesJointesDirection_Telechargement'
      AND object_id = OBJECT_ID('[Commun].[AccesPiecesJointesDirection]')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_AccesPiecesJointesDirection_Telechargement]
    ON [Commun].[AccesPiecesJointesDirection]
    (
        [DirectionSourceId],
            [DirectionCibleId],
        [AutoriserTelechargement],
        [IsActive],
        [TypeDocumentLabId]
    );
END
GO

