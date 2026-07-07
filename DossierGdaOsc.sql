CREATE TABLE [Gda].[DossierGdaOsc] (
    [Id]                  INT                IDENTITY (1, 1) NOT NULL,
    [DossierGdaId]        INT                NULL,
    [OperationsSurCompte] NVARCHAR (MAX)     NULL,
    [Commentaire]         NVARCHAR (MAX)     NULL,
    [DateCreation]        DATETIMEOFFSET (7) NOT NULL,
    [DateModification]    DATETIMEOFFSET (7) NULL,
    [DateResiliation]     DATETIMEOFFSET (7) NULL,
    [DateDeclaration]     DATETIMEOFFSET (7) NULL,
    [CreateurId]          INT                NOT NULL,
    [ModificateurId]      INT                NULL,
    [StatutOscId]         INT                NULL,
    [CodeIdentifiant]     NVARCHAR (40)      NULL,
    [DebitCreditId]       INT                NULL,
    [Montant]             DECIMAL (18, 2)    NULL,
    [ResponsableMoe]      NVARCHAR (60)      NULL,
    [IsRealise]           BIT                NULL,
    [IsRetablis]          BIT                NULL,
    [IsManquementDgt] BIT NULL, 
    CONSTRAINT [PK_DossierGdaOsc] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_DossierGdaOsc_DebitCreditOsc_DossierGdaId] FOREIGN KEY ([DebitCreditId]) REFERENCES [Gda].[DebitCreditOsc] ([Id]),
    CONSTRAINT [FK_DossierGdaOsc_DossierGda_DossierGdaId] FOREIGN KEY ([DossierGdaId]) REFERENCES [Gda].[DossierGda] ([Id]),
    CONSTRAINT [FK_DossierGdaOsc_StatutActionsGda_DossierGdaId] FOREIGN KEY ([StatutOscId]) REFERENCES [Gda].[StatutActionsGda] ([Id]),
    CONSTRAINT [FK_DossierGdaOsc_Utilisateur_CreateurId] FOREIGN KEY ([CreateurId]) REFERENCES [Commun].[Utilisateur] ([Id]),
    CONSTRAINT [FK_DossierGdaOsc_Utilisateur_ModificateurId] FOREIGN KEY ([ModificateurId]) REFERENCES [Commun].[Utilisateur] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_DossierGdaOsc_DossierGdaId]
    ON [Gda].[DossierGdaOsc]([DossierGdaId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DossierGdaOsc_StatutOscId]
    ON [Gda].[DossierGdaOsc]([StatutOscId] ASC);

    GO
CREATE NONCLUSTERED INDEX [IX_DossierGdaOsc_DebitCreditId]
    ON [Gda].[DossierGdaOsc]([DebitCreditId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DossierGdaOsc_CreateurId]
 ON [Gda].[DossierGdaOsc]([CreateurId] ASC)

 GO
 CREATE NONCLUSTERED INDEX [IX_DossierGdaOsc_ModificateurId]
	ON [Gda].[DossierGdaOsc]([ModificateurId] ASC)
GO
CREATE NONCLUSTERED INDEX [IX_DossierGdaOsc_DossierGdaId_DateCreation]
    ON [Gda].[DossierGdaOsc]([DossierGdaId] ASC, [DateCreation] DESC);

