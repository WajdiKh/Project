using System;
using System.Linq;
using System.Text;
using BacaratWeb.Core.Cryptography;
using BacaratWeb.Entities.Amf;
using BacaratWeb.Entities.AvisIm;
using BacaratWeb.Entities.AvisSi;
using BacaratWeb.Entities.Commun;
using BacaratWeb.Entities.CommunAvis;
using BacaratWeb.Entities.Escalade;
using BacaratWeb.Entities.Fraude;
using BacaratWeb.Entities.Gda;
using BacaratWeb.Entities.Lab;
using BacaratWeb.Entities.SAvis;
using BacaratWeb.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using EmailNotificationScope = BacaratWeb.Entities.Commun.EmailNotificationScope;
using TitulaireCompteEscalade = BacaratWeb.Entities.Escalade.TitulaireCompte;
using TitulaireCompteGda = BacaratWeb.Entities.Gda.TitulaireCompte;
using TitulaireComptePersonneMoraleEscalade = BacaratWeb.Entities.Escalade.TitulaireComptePersonneMorale;
using TitulaireComptePersonneMoraleGda = BacaratWeb.Entities.Gda.TitulaireComptePersonneMorale;
using TitulaireComptePersonnePhysiqueEscalade = BacaratWeb.Entities.Escalade.TitulaireComptePersonnePhysique;
using TitulaireComptePersonnePhysiqueGda = BacaratWeb.Entities.Gda.TitulaireComptePersonnePhysique;
using BacaratWeb.Entities.Transfert;

namespace BacaratWeb.Model.Entities
{
    public partial class BacaratWebContext : DbContext
    {
        public virtual DbSet<ActionTypeDemandeInformation> ActionTypeDemandeInformations { get; set; }

        public virtual DbSet<Activite> Activites { get; set; }
        public virtual DbSet<QLBReporting> QLBReportings { get; set; }
        public virtual DbSet<TitulaireCompteGda> TitulairesCompteGda { get; set; }
        public virtual DbSet<TitulaireComptePersonnePhysiqueGda> TitulairesComptePersonnePhysiqueGda { get; set; }
        public virtual DbSet<TitulaireComptePersonneMoraleGda> TitulairesComptePersonneMoraleGda { get; set; }
        public virtual DbSet<DossierGdaTransactionnelClient> DossierGdaTransactionnelClients { get; set; }
        public virtual DbSet<DossierLabQLBResult> DossierLabQLBResults { get; set; }
        public virtual DbSet<OperationsCompaniesAssurances> OperationsCompaniesAssurances { get; set; }
        public virtual DbSet<OperationsCompaniesImmobiliers> OperationsCompaniesImmobiliers { get; set; }
        public virtual DbSet<ContratOperationsCompaniesAssurances> ContratsOperationsCompaniesAssurances { get; set; }
        public virtual DbSet<OperationSusceptibleOpposition> OperationsSusceptiblesOpposition { get; set; }

        public virtual DbSet<PaysDepartOperationsCompaniesAssurances> PaysDepartOperationsCompaniesAssurances
        {
            get;
            set;
        }

        public virtual DbSet<PaysArriveeOperationsCompaniesAssurances> PaysArriveeOperationsCompaniesAssurances
        {
            get;
            set;
        }

        public virtual DbSet<PaysDepartOperationsCompaniesImmobiliers> PaysDepartOperationsCompaniesImmobiliers
        {
            get;
            set;
        }

        public virtual DbSet<PaysArriveeOperationsCompaniesImmobiliers> PaysArriveeOperationsCompaniesImmobiliers
        {
            get;
            set;
        }

        public virtual DbSet<TypeGarantieOperationsCompaniesAssurances> TypesGarantieOperationsCompaniesAssurances
        {
            get;
            set;
        }

        public virtual DbSet<TypeOperationOperationsCompaniesImmobiliers> TypeOperationOperationsCompaniesImmobiliers
        {
            get;
            set;
        }

        public virtual DbSet<CriteresAlerteOrigineOperationsCompaniesImmobiliers>
            CriteresAlerteOrigineOperationsCompaniesImmobiliers { get; set; }

        public virtual DbSet<ModaliteFinancementOperationsCompaniesImmobiliers>
            ModaliteFinancementOperationsCompaniesImmobiliers { get; set; }

        public virtual DbSet<AnalyseDossierEscalade> AnalyseDossierEscalades { get; set; }
        public virtual DbSet<TitulaireCompteEscalade> TitulairesCompteEscalade { get; set; }

        public virtual DbSet<TitulaireComptePersonnePhysiqueEscalade> TitulairesComptePersonnePhysiqueEscalade
        {
            get;
            set;
        }

        public virtual DbSet<TitulaireComptePersonneMoraleEscalade> TitulairesComptePersonneMoraleEscalade { get; set; }
        public virtual DbSet<DossierEscaladeCompteCredite> DossiersEscaladeComptesCredites { get; set; }
        public virtual DbSet<DossierEscaladeCompteDebite> DossiersEscaladeComptesDebites { get; set; }
        public virtual DbSet<AppartenanceDocument> AppartenanceDocuments { get; set; }
        public virtual DbSet<GdaAppartenanceDocument> GdaAppartenanceDocuments { get; set; }

        public virtual DbSet<Application> Applications { get; set; }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

        public virtual DbSet<ConfigurationValue> ConfigurationValues { get; set; }

        public virtual DbSet<AuditTrail> AuditTrails { get; set; }

        public virtual DbSet<AuditTrailEvent> AuditTrailEvents { get; set; }

        public virtual DbSet<AutorisationPpf> AutorisationPpfs { get; set; }

        public virtual DbSet<BamCeiling> BamCeilings { get; set; }
        public virtual DbSet<CcBlocked> CcBlocked { get; set; }
        public virtual DbSet<SafetyInstructions> SafetyInstructions { get; set; }
        public virtual DbSet<AvisLab> AvisLabs { get; set; }
        public virtual DbSet<NatureSoupconFraudeFiscale> NatureSoupconFraudeFiscales { get; set; }
        public virtual DbSet<NatureInfractionPenale> NatureInfractionPenales { get; set; }
        public virtual DbSet<CanalBdf> CanalBdfs { get; set; }
        public virtual DbSet<CanalEntreeEnRelation> CanalEntreeEnRelations { get; set; }
        public virtual DbSet<CategorieCanalBdf> CategorieCanalBdfs { get; set; }

        public virtual DbSet<CategorieEtablissementDeclarant> CategorieEtablissementDeclarants { get; set; }

        public virtual DbSet<CategorieFraude> CategorieFraudes { get; set; }

        public virtual DbSet<CategorieLab> CategorieLabs { get; set; }
        public virtual DbSet<EmailTemplateLab> EmailTemplateLabs { get; set; }
        public virtual DbSet<DossierLabNonConnu> DossierLabNonConnus { get; set; }
        public virtual DbSet<NonConnuLab> NonConnuLabs { get; set; }

        public virtual DbSet<GroupeCategorieLab> GroupeCategorieLabs { get; set; }

        public virtual DbSet<GroupeOrigineLab> GroupeOrigineLabs { get; set; }
        public virtual DbSet<NatureDs> NaturesDs { get; set; }
        public virtual DbSet<CategorieContributeur> CategoriesContributeur { get; set; }
        public virtual DbSet<Contributeur> Contributeurs { get; set; }

        public virtual DbSet<DeclarationTracfinContributeurDsft> DeclarationTracfinContributeursDsft { get; set; }
        public virtual DbSet<FonctionLab> FonctionsLabs { get; set; }
        public virtual DbSet<InformationRequestCloseReason> InformationRequestCloseReasons { get; set; }
        public virtual DbSet<CategorieModeOperatoire> CategorieModeOperatoires { get; set; }

        public virtual DbSet<CategorieMotifRejetCheque> CategorieMotifRejetCheques { get; set; }

        public virtual DbSet<CategorieTracfin> CategorieTracfins { get; set; }

        public virtual DbSet<CategorieTypeCollecteCheque> CategorieTypeCollecteCheques { get; set; }

        public virtual DbSet<CategorieTypePaiement> CategorieTypePaiements { get; set; }

        public virtual DbSet<CategorieTypologieBdf> CategorieTypologieBdfs { get; set; }
        public virtual DbSet<TypologieGda> TypologieGdas { get; set; }
        public virtual DbSet<TypeDegel> TypeDegels { get; set; }

        public virtual DbSet<Civilite> Civilites { get; set; }

        public virtual DbSet<ComplementMotifRejetCheque> ComplementMotifRejetCheques { get; set; }

        public virtual DbSet<ComplementVoie> ComplementVoies { get; set; }

        public virtual DbSet<ConclusionEscalade> ConclusionEscalades { get; set; }

        public virtual DbSet<Coordonnee> Coordonnees { get; set; }

        public virtual DbSet<CarnetAdresses> CarnetAdresses { get; set; }
        public virtual DbSet<TagAvisSi> TagAvisSi { get; set; }

        public virtual DbSet<DeclarantEntiteManager> DeclarantEntiteManagers { get; set; }

        public virtual DbSet<DocumentDeclarationTracfin> DocumentDeclarationTracfins { get; set; }
        public virtual DbSet<OperationSuspectDeclarationTracfin> OperationSuspectDeclarationTracfins { get; set; }
        public virtual DbSet<OperationEnCoursDeclarationTracfin> OperationEnCoursDeclarationTracfins { get; set; }

        public virtual DbSet<DeclarationTracfin> DeclarationTracfins { get; set; }
        public virtual DbSet<DeclarationTracfinFile> DeclarationTracfinFiles { get; set; }
        public virtual DbSet<DeclarationTracfinEnvoiEvent> DeclarationTracfinEnvoiEvents { get; set; }

        public virtual DbSet<Defaillance> Defaillances { get; set; }

        public virtual DbSet<Delegation> Delegations { get; set; }
        public virtual DbSet<PpeType> PpeTypes { get; set; }

        public virtual DbSet<DemandeInformation> DemandeInformations { get; set; }

        public virtual DbSet<PlanActionAnalyse> PlanActionAnalyses { get; set; }

        public virtual DbSet<Devise> Devises { get; set; }

        public virtual DbSet<Bunit> Bunits { get; set; }

        public virtual DbSet<Direction> Directions { get; set; }
        public virtual DbSet<DirectionExtend> DirectionsExtend { get; set; }

        public virtual DbSet<DirectionAccessible> DirectionAccessibles { get; set; }
        public virtual DbSet<PaysAccessible> PaysAccessibles { get; set; }
        public virtual DbSet<DirectionAccessibleException> DirectionAccessibleExceptions { get; set; }
        public virtual DbSet<DirectionAccessibleExceptionAmf> DirectionAccessibleExceptionAmfs { get; set; }
        public virtual DbSet<DirectionCoordonnee> DirectionCoordonnees { get; set; }

        public virtual DbSet<DocumentDemandeInformation> DocumentDemandeInformations { get; set; }

        public virtual DbSet<DocumentPlanActionAnalyse> DocumentPlanActionAnalyses { get; set; }

        public virtual DbSet<DocumentDossierEscalade> DocumentDossierEscalades { get; set; }

        public virtual DbSet<DocumentDossierFraude> DocumentDossierFraudes { get; set; }

        public virtual DbSet<DocumentDossierLab> DocumentDossierLabs { get; set; }
        public virtual DbSet<DocumentDossierAmf> DocumentDossierAmfs { get; set; }
        public virtual DbSet<DocumentEscalade> DocumentEscalades { get; set; }

        public virtual DbSet<DocumentFraude> DocumentFraudes { get; set; }

        public virtual DbSet<DocumentLab> DocumentLabs { get; set; }
        public virtual DbSet<DocumentAmf> DocumentAmfs { get; set; }

        public virtual DbSet<DocumentTool> DocumentTools { get; set; }

        public virtual DbSet<DocumentToolDetail> DocumentToolDetails { get; set; }

        public virtual DbSet<Domaine> Domaines { get; set; }

        public virtual DbSet<Traduction> Translations { get; set; }

        public virtual DbSet<TraductionTemplate> TraductionTemplates { get; set; }

        public virtual DbSet<DossierDefaillance> DossierDefaillances { get; set; }

        public virtual DbSet<DossierEscalade> DossierEscalades { get; set; }

        public virtual DbSet<DossierEscaladeHisto> DossierEscaladeHistos { get; set; }

        public virtual DbSet<DossierEscaladePersonneMorale> DossierEscaladePersonneMorales { get; set; }

        public virtual DbSet<DossierEscaladePersonnePhysique> DossierEscaladePersonnePhysiques { get; set; }
        public virtual DbSet<DossierEscaladeClientMorale> DossierEscaladeClientsMorales { get; set; }
        public virtual DbSet<DossierEscaladeClientPhysique> DossierEscaladeClientsPhysiques { get; set; }
        public virtual DbSet<DossierEscaladeContrepartieMorale> DossierEscaladeContrepartiesMorales { get; set; }
        public virtual DbSet<DossierEscaladeContrepartiePhysique> DossierEscaladeContrepartiesPhysiques { get; set; }

        public virtual DbSet<DossierFraude> DossierFraudes { get; set; }

        public virtual DbSet<DossierFraudeAction> DossierFraudeActions { get; set; }

        public virtual DbSet<DossierLabAction> DossierLabActions { get; set; }

        public virtual DbSet<DossierFraudeHisto> DossierFraudeHistos { get; set; }

        public virtual DbSet<DossierFraudePersonneMorale> DossierFraudePersonneMorales { get; set; }

        public virtual DbSet<DossierFraudePersonnePhysique> DossierFraudePersonnePhysiques { get; set; }

        public virtual DbSet<DossierFraudeResult> DossierFraudeResults { get; set; }
        public virtual DbSet<AavReference> AavReferences { get; set; }

        public virtual DbSet<DossierLab> DossierLabs { get; set; }
        public virtual DbSet<DossierAmf> DossierAmfs { get; set; }
        public virtual DbSet<StatutDossierAmf> StatutDossierAmfs { get; set; }
        public virtual DbSet<DossierLabHisto> DossierLabHistos { get; set; }
        public virtual DbSet<DossierAmfHisto> DossierAmfHistos { get; set; }

        public virtual DbSet<DossierLabPersonneMorale> DossierLabPersonneMorales { get; set; }

        public virtual DbSet<DossierAmfPersonneMorale> DossierAmfPersonneMorales { get; set; }
        public virtual DbSet<DossierLabPersonnePhysique> DossierLabPersonnePhysiques { get; set; }
        public virtual DbSet<DossierAmfPersonnePhysique> DossierAmfPersonnePhysiques { get; set; }
        public virtual DbSet<DossierValidationFinale> DossierValidationFinales { get; set; }

        public virtual DbSet<DossierValidationIntermediaire> DossierValidationIntermediaires { get; set; }

        public virtual DbSet<DroitCompte> DroitComptes { get; set; }

        public virtual DbSet<EmailNotification> EmailNotifications { get; set; }

        public virtual DbSet<EmailNotificationTemplate> EmailNotificationTemplates { get; set; }
        public virtual DbSet<EmailNotificationScope> EmailNotificationScopes { get; set; }

        public virtual DbSet<EmailGenerique> EmailGeneriques { get; set; }
        public virtual DbSet<Confidentiel> Confidentiels { get; set; }
        public virtual DbSet<ConfidentielAmf> ConfidentielAmfs { get; set; }
        public virtual DbSet<EmailNotificationType> EmailNotificationTypes { get; set; }

        public virtual DbSet<EntiteFraude> EntiteFraudes { get; set; }

        public virtual DbSet<EntiteLab> EntiteLabs { get; set; }
        public virtual DbSet<Environnement> Environnements { get; set; }

        public virtual DbSet<EquipeAnalyse> EquipeAnalyses { get; set; }
        public virtual DbSet<EquipeSuperviseurEscalade> EquipeSuperviseurEscalades { get; set; }

        public virtual DbSet<EquipeValidation> EquipeValidations { get; set; }

        public virtual DbSet<EtablissementDeclarant> EtablissementDeclarants { get; set; }

        public virtual DbSet<EtatFraude> EtatFraudes { get; set; }

        public virtual DbSet<Evenement> Evenements { get; set; }

        public virtual DbSet<Fonction> Fonctions { get; set; }

        public virtual DbSet<FormeJuridique> FormeJuridiques { get; set; }
        public virtual DbSet<IdentificationProfessionnelle> IdentificationProfessionnelles { get; set; }


        public virtual DbSet<HistoriqueConnexion> HistoriqueConnexions { get; set; }

        public virtual DbSet<Impact> Impacts { get; set; }

        public virtual DbSet<IndiceChart> IndiceCharts { get; set; }

        public virtual DbSet<IndicePie> IndicePies { get; set; }

        public virtual DbSet<DynamicDashboardTemplate> DynamicDashboardTemplates { get; set; }

        public virtual DbSet<Langue> Langues { get; set; }
        public virtual DbSet<Extension> Extensions { get; set; }

        public virtual DbSet<RepresentantLegal> RepresentantLegals { get; set; }

        public virtual DbSet<Dirigeant> Dirigeants { get; set; }


        public virtual DbSet<LienPersonneMorale> LienPersonneMorales { get; set; }

        public virtual DbSet<LienPersonnePhysique> LienPersonnePhysiques { get; set; }
        public virtual DbSet<CategorieLienLab> CategorieLienLabs { get; set; }

        public virtual DbSet<ModeOperatoire> ModeOperatoires { get; set; }

        public virtual DbSet<MotifExemptionNotification> MotifExemptionNotifications { get; set; }

        public virtual DbSet<MotifRejetCheque> MotifRejetCheques { get; set; }

        public virtual DbSet<TypeLienSupport> TypeLienSupports { get; set; }
        public virtual DbSet<TypeReferenceLab> TypeReferenceLabs { get; set; }

        public virtual DbSet<TypeGarantie> TypesGaranties { get; set; }
        public virtual DbSet<TypeOperation> TypesOperations { get; set; }
        public virtual DbSet<CriteresAlerteOrigine> CriteresAlerteOrigines { get; set; }
        public virtual DbSet<ModaliteFinancement> ModaliteFinancements { get; set; }
        public virtual DbSet<OrigineDirectionFraude> OrigineDirectionFraudes { get; set; }

        public virtual DbSet<OrigineFraude> OrigineFraudes { get; set; }

        public virtual DbSet<OrigineLab> OrigineLabs { get; set; }
        public virtual DbSet<ParametreDirection> ParametreDirections { get; set; }

        public virtual DbSet<Pays> Pays { get; set; }

        public virtual DbSet<PersonneMoraleEscalade> PersonneMoraleEscalades { get; set; }

        public virtual DbSet<PersonneMoraleFraude> PersonneMoraleFraudes { get; set; }

        public virtual DbSet<PersonneMoraleLab> PersonneMoraleLabs { get; set; }
        public virtual DbSet<PersonneMoraleAmf> PersonneMoraleAmfs { get; set; }

        public virtual DbSet<PersonneMoraleLabLienEntite> PersonneMoraleLabLienEntites { get; set; }
        public virtual DbSet<PersonneMoraleResult> PersonneMoraleResults { get; set; }

        public virtual DbSet<PersonneMoraleResultLab> PersonneMoraleResultLabs { get; set; }
        public virtual DbSet<PersonnePhysiqueEscalade> PersonnePhysiqueEscalades { get; set; }

        public virtual DbSet<PersonnePhysiqueFraude> PersonnePhysiqueFraudes { get; set; }

        public virtual DbSet<PersonnePhysiqueLab> PersonnePhysiqueLabs { get; set; }

        public virtual DbSet<PersonnePhysiqueAmf> PersonnePhysiqueAmfs { get; set; }

        public virtual DbSet<PersonnePhysiqueLabLienEntite> PersonnePhysiqueLabLienEntites { get; set; }

        public virtual DbSet<PersonnePhysiqueResult> PersonnePhysiqueResults { get; set; }
        public virtual DbSet<PersonnePhysiqueResultLab> PersonnePhysiqueResultLabs { get; set; }

        public virtual DbSet<PieceIdentite> PieceIdentites { get; set; }

        public virtual DbSet<PrincipalInstrumentFinancier> PrincipalInstrumentFinanciers { get; set; }
        public virtual DbSet<PrincipalInstrumentFinancierAmf> PrincipalInstrumentFinancierAmfs { get; set; }
        public virtual DbSet<Produit> Produits { get; set; }
        public virtual DbSet<VoixRecouvrement> VoixRecouvrements { get; set; }

        public virtual DbSet<Profession> Professions { get; set; }

        public virtual DbSet<DossierLabOperation> DossierLabOperations { get; set; }

        public virtual DbSet<Regle> Regles { get; set; }

        public virtual DbSet<RegleDirection> RegleDirections { get; set; }

        public virtual DbSet<RelationClient> RelationClients { get; set; }

        public virtual DbSet<RoleClient> RoleClients { get; set; }

        public virtual DbSet<Routage> Routages { get; set; }

        public virtual DbSet<ScenarioNorkom> ScenarioNorkoms { get; set; }

        public virtual DbSet<ScenarioLab> ScenarioLabs { get; set; }
        public virtual DbSet<ApplicationScenarioLab> ApplicationScenarioLabs { get; set; }
        public virtual DbSet<SecteurFraude> Secteurs { get; set; }

        public virtual DbSet<SecteurLab> SecteurLabs { get; set; }

        public virtual DbSet<SecteurEconomique> SecteurEconomiques { get; set; }

        public virtual DbSet<SecteurEconomiqueLab> SecteurEconomiqueLabs { get; set; }

        public virtual DbSet<SecteurProfessionnel> SecteurProfessionnels { get; set; }

        public virtual DbSet<Sexe> Sexes { get; set; }

        public virtual DbSet<SituationFamiliale> SituationFamiliales { get; set; }


        public virtual DbSet<StatutPlanActionAnalyse> StatutPlanActionAnalyses { get; set; }

        public virtual DbSet<StatutDemandeInformation> StatutDemandeInformations { get; set; }

        public virtual DbSet<StatutDossierFraude> StatutDossierFraudes { get; set; }

        public virtual DbSet<StatutDossierLab> StatutDossierLabs { get; set; }

        public virtual DbSet<StatutEscalade> StatutEscalades { get; set; }

        public virtual DbSet<StatutOperation> StatutOperations { get; set; }

        public virtual DbSet<StatutPersonneFraude> StatutPersonneFraudes { get; set; }

        public virtual DbSet<SupportFinancierPersonneMorale> SupportFinancierPersonneMorales { get; set; }
        public virtual DbSet<SupportFinancierNonConnu> SupportFinancierNonConnus { get; set; }

        public virtual DbSet<SupportFinancierPersonnePhysique> SupportFinancierPersonnePhysiques { get; set; }


        public virtual DbSet<DossierInstrumentFinancierAmf> DossierInstrumentFinancierAmfs { get; set; }

        public virtual DbSet<DossierProduitDeriveAmf> DossierProduitDeriveAmfs { get; set; }

        public virtual DbSet<Trace> Traces { get; set; }

        public virtual DbSet<TypeAdresse> TypeAdresses { get; set; }
        public virtual DbSet<TypeFonctionDirigeant> TypeFonctionDirigeants { get; set; }
        public virtual DbSet<TypeContrat> TypeContrats { get; set; }
        public virtual DbSet<TypeClientEscalade> TypeClientEscalades { get; set; }
        public virtual DbSet<TypeClientFraude> TypeClientFraudes { get; set; }
        public virtual DbSet<ScenarioFraude> ScenarioFraudes { get; set; }
        public virtual DbSet<TypeClientLab> TypeClientLabs { get; set; }
        public virtual DbSet<TypeImplicationLab> TypeImplicationLabs { get; set; }
        public virtual DbSet<ProfessionalIdentificationLab> ProfessionalIdentificationLabs { get; set; }

        public virtual DbSet<TypeRelationAffaireLab> TypeRelationAffaireLabs { get; set; }
        public virtual DbSet<TypeRelationAffaireFraude> TypeRelationAffaireFraudes { get; set; }
        public virtual DbSet<TypeCollecteCheque> TypeCollecteCheques { get; set; }

        public virtual DbSet<TypeCompte> TypeComptes { get; set; }
        public virtual DbSet<TypeDeclarationTracfin> TypeDeclarationTracfins { get; set; }
        public virtual DbSet<TypeDocument> TypeDocuments { get; set; }

        public virtual DbSet<TypeLegislationLab> TypeLegislationLabs { get; set; }

        public virtual DbSet<TypeLienPersonneMoraleMorale> TypeLienPersonneMoraleMorales { get; set; }

        public virtual DbSet<TypeLienPersonneMoralePhysique> TypeLienPersonneMoralePhysiques { get; set; }

        public virtual DbSet<TypeLienPersonnePhysiqueMorale> TypeLienPersonnePhysiqueMorales { get; set; }

        public virtual DbSet<TypeLienPersonnePhysiquePhysique> TypeLienPersonnePhysiquePhysiques { get; set; }

        public virtual DbSet<TypePaiement> TypePaiements { get; set; }

        public virtual DbSet<TypeParametre> TypeParametres { get; set; }

        public virtual DbSet<TypePieceIdentite> TypePieceIdentites { get; set; }

        public virtual DbSet<TypePieceJointe> TypePieceJointes { get; set; }

        public virtual DbSet<TypeException> TypeExceptions { get; set; }

        public virtual DbSet<TypeVoie> TypeVoies { get; set; }

        public virtual DbSet<TypologieBdf> TypologieBdfs { get; set; }

        public virtual DbSet<UniteEquipe> UniteEquipes { get; set; }

        public virtual DbSet<UserAction> UserActions { get; set; }

        public virtual DbSet<Utilisateur> Utilisateurs { get; set; }

        public virtual DbSet<UtilisateurDirection> UtilisateurDirections { get; set; }

        public virtual DbSet<UtilisateurDocument> UtilisateurDocuments { get; set; }

        public virtual DbSet<UtilisateurEquipeAnalyse> UtilisateurEquipeAnalyses { get; set; }
        public virtual DbSet<UtilisateurEquipeSuperviseurEscalade> UtilisateurEquipeSuperviseurEscalades { get; set; }

        public virtual DbSet<UtilisateurEquipeValidation> UtilisateurEquipeValidations { get; set; }

        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }

        public virtual DbSet<VisaLab> VisaLabs { get; set; }
        public virtual DbSet<ZoneGeographique> ZoneGeographiques { get; set; }
        public virtual DbSet<EntiteConcerne> EntiteConcernes { get; set; }
        public virtual DbSet<DocumentEntiteConcerne> DocumentEntiteConcernes { get; set; }
        public virtual DbSet<EquipeValidationFlge> EquipeValidationFlges { get; set; }
        public virtual DbSet<UtilisateurEquipeValidationFlge> UtilisateurEquipeValidationFlges { get; set; }
        public virtual DbSet<RoutageFlge> RoutageFlges { get; set; }

        public virtual DbSet<EventDossier> EventDossiers { get; set; }

        public virtual DbSet<EventSearchLab> EventSearchLabs { get; set; }

        public virtual DbSet<EventSearchFraude> EventSearchFraudes { get; set; }
        public virtual DbSet<EventSearchGda> EventSearchGdas { get; set; }

        public virtual DbSet<EventSearchAmf> EventSearchAmfs { get; set; }

        public virtual DbSet<EventDossierType> EventDossierTypes { get; set; }

        public virtual DbSet<Annonce> Annonces { get; set; }
        public virtual DbSet<AnnonceType> AnnonceTypes { get; set; }


        public virtual DbSet<DemandeInformationFraude> DemandeInformationFraudes { get; set; }
        public virtual DbSet<StatutDemandeInformationFraude> StatutDemandeInformationFraudes { get; set; }
        public virtual DbSet<TypeDemandeInformationFraude> TypdeDemandeInformationFraudes { get; set; }
        public virtual DbSet<DocumentDemandeInformationFraude> DocumentDemandeInformationFraudes { get; set; }


        public virtual DbSet<StatutDemandeInformationLab> StatutDemandeInformationLabs { get; set; }

        public virtual DbSet<DossierValidationIntermediaireFlge> DossierValidationIntermediaireFlges { get; set; }
        public virtual DbSet<DossierValidationFinaleFlge> DossierValidationFinaleFlge { get; set; }

        public virtual DbSet<CoordonneePersonnePhysiqueLab> CoordonneePersonnePhysiqueLabs { get; set; }

        public virtual DbSet<ActiviteProfessionnellePersonnePhysiqueLab> ActiviteProfessionnellePersonnePhysiqueLabs
        {
            get;
            set;
        }

        public virtual DbSet<AutreNationalitePersonnePhysiqueLab> AutreNationalitePersonnePhysiqueLabs { get; set; }

        public virtual DbSet<DerniereReferenceTracfin> DerniereReferenceTracfins { get; set; }

        public virtual DbSet<TypeListeCriblage> TypeListeCriblages { get; set; }

        public virtual DbSet<DossierLabScenario> DossierLabScenarios { get; set; }

        public virtual DbSet<OrganismeLab> OrganismeLabs { get; set; }
        public virtual DbSet<DemandeInformationLab> DemandeInformationLabs { get; set; }
        public virtual DbSet<DocumentDemandeInformationLab> DocumentDemandeInformationLabs { get; set; }

        public virtual DbSet<OrigineGroupeLab> OrigineGroupeLabs { get; set; }

        public virtual DbSet<CategorieGroupeLab> CategorieGroupeLabs { get; set; }

        public virtual DbSet<ModeEnvoieTracfin> ModeEnvoieTracfins { get; set; }

        public virtual DbSet<TypeListeCriblageAmf> TypeListeCriblageAmfs { get; set; }

        public virtual DbSet<DemandeInformationAmf> DemandeInformationAmfs { get; set; }
        public virtual DbSet<DocumentDemandeInformationAmf> DocumentDemandeInformationAmfs { get; set; }


        public virtual DbSet<ActionTypeDemandeInformationAvisSi> ActionTypeDemandeInformationAvisSis { get; set; }
        public virtual DbSet<AnalyseDossierAvisSi> AnalyseDossierAvisSis { get; set; }

        public virtual DbSet<RejetDdcAvisSi> RejetDdcAvisSis { get; set; }
        public virtual DbSet<AppartenanceDocumentAvisSi> AppartenanceDocumentAvisSis { get; set; }
        public virtual DbSet<ApplicationAvisSi> ApplicationAvisSis { get; set; }
        public virtual DbSet<ConclusionAvisSi> ConclusionAvisSis { get; set; }
        public virtual DbSet<DeclarantEntiteAvisSi> DeclarantEntiteAvisSis { get; set; }
        public virtual DbSet<DeclarantEntiteManagerAvisSi> DeclarantEntiteManagerAvisSis { get; set; }
        public virtual DbSet<DefaillanceAvisSi> DefaillanceAvisSis { get; set; }
        public virtual DbSet<DemandeInformationAvisSi> DemandeInformationAvisSis { get; set; }
        public virtual DbSet<DocumentAvisSi> DocumentAvisSis { get; set; }
        public virtual DbSet<DocumentDemandeInformationAvisSi> DocumentDemandeInformationAvisSis { get; set; }
        public virtual DbSet<DocumentDossierAvisSi> DocumentDossierAvisSis { get; set; }

        public virtual DbSet<TagDossierAvisSi> TagDossierAvisSis { get; set; }
        public virtual DbSet<DomaineCategorieOutil> DomaineCategorieOutils { get; set; }
        public virtual DbSet<NationalitePersonnePhysiqueLab> NationalitePersonnePhysiqueLabs { get; set; }
        public virtual DbSet<NationaliteAutreIdentiteLab> NationaliteAutreIdentiteLabs { get; set; }
        public virtual DbSet<PpeTypePersonnePhysiqueLab> PpeTypePersonnePhysiqueLabs { get; set; }

        public virtual DbSet<DeclarationTracfinNatureInfractionPenale> DeclarationTracfinNatureInfractionPenales
        {
            get;
            set;
        }

        public virtual DbSet<DeclarationTracfinNatureSoupconFraudeFiscale> DeclarationTracfinNatureSoupconFraudeFiscales
        {
            get;
            set;
        }

        public virtual DbSet<DocumentEntiteConcerneAvisSi> DocumentEntiteConcerneAvisSis { get; set; }
        public virtual DbSet<DocumentPlanActionAnalyseAvisSi> DocumentPlanActionAnalyseAvisSis { get; set; }
        public virtual DbSet<TypologieDemande> TypologieDemandes { get; set; }
        public virtual DbSet<DossierAvisSi> DossierAvisSis { get; set; }
        public virtual DbSet<DossierAvisSiHisto> DossierAvisSiHistos { get; set; }
        public virtual DbSet<DossierAvisSiClientMorale> DossierAvisSiClientMorales { get; set; }
        public virtual DbSet<DossierAvisSiClientPhysique> DossierAvisSiClientPhysiques { get; set; }
        public virtual DbSet<DossierAvisSiContrepartieMorale> DossierAvisSiContrepartieMorales { get; set; }
        public virtual DbSet<DossierAvisSiContrepartiePhysique> DossierAvisSiContrepartiePhysiques { get; set; }
        public virtual DbSet<DossierDefaillanceAvisSi> DossierDefaillanceAvisSis { get; set; }
        public virtual DbSet<DossierValidationFinaleAvisSi> DossierValidationFinaleAvisSis { get; set; }
        public virtual DbSet<DossierValidationFinaleFlgeAvisSi> DossierValidationFinaleFlgeAvisSis { get; set; }
        public virtual DbSet<DossierValidationIntermediaireAvisSi> DossierValidationIntermediaireAvisSis { get; set; }

        public virtual DbSet<DossierValidationIntermediaireFlgeAvisSi> DossierValidationIntermediaireFlgeAvisSis
        {
            get;
            set;
        }

        public virtual DbSet<EntiteConcerneAvisSi> EntiteConcerneAvisSis { get; set; }
        public virtual DbSet<EnvironnementAvisSi> EnvironnementAvisSis { get; set; }
        public virtual DbSet<DecisionAvisSi> DecisionAvisSis { get; set; }
        public virtual DbSet<EquipeAnalyseAvisSi> EquipeAnalyseAvisSis { get; set; }
        public virtual DbSet<EquipeSuperviseurAvisSi> EquipeSuperviseurAvisSis { get; set; }
        public virtual DbSet<EquipeValidationAvisSi> EquipeValidationAvisSis { get; set; }
        public virtual DbSet<EquipeValidationFlgeAvisSi> EquipeValidationFlgeAvisSis { get; set; }
        public virtual DbSet<DemandeDerogation> DemandeDerogations { get; set; }
        public virtual DbSet<ClientMoraleAvisSi> ClientMoraleAvisSis { get; set; }
        public virtual DbSet<ClientPhysiqueAvisSi> ClientPhysiqueAvisSis { get; set; }
        public virtual DbSet<ContrepartieMoraleAvisSi> ContrepartieMoraleAvisSis { get; set; }
        public virtual DbSet<ContrepartiePhysiqueAvisSi> ContrepartiePhysiqueAvisSis { get; set; }

        public virtual DbSet<ClientMoraleEscalade> ClientMoraleEscalades { get; set; }
        public virtual DbSet<ClientPhysiqueEscalade> ClientPhysiqueEscalades { get; set; }
        public virtual DbSet<ContrepartieMoraleEscalade> ContrepartieMoraleEscalades { get; set; }
        public virtual DbSet<ContrepartiePhysiqueEscalade> ContrepartiePhysiqueEscalades { get; set; }
        public virtual DbSet<PlanActionAnalyseAvisSi> PlanActionAnalyseAvisSis { get; set; }
        public virtual DbSet<ResponsableFlgeAvisSi> ResponsableFlgeAvisSis { get; set; }
        public virtual DbSet<ResponsableLocalAvisSi> ResponsableLocalAvisSis { get; set; }
        public virtual DbSet<RoutageAvisSi> RoutageAvisSis { get; set; }
        public virtual DbSet<RoutageFlgeAvisSi> RoutageFlgeAvisSis { get; set; }
        public virtual DbSet<StatutAvisSi> StatutAvisSis { get; set; }
        public virtual DbSet<StatutDemandeInformationAvisSi> StatutDemandeInformationAvisSis { get; set; }
        public virtual DbSet<StatutPlanActionAnalyseAvisSi> StatutPlanActionAnalyseAvisSis { get; set; }
        public virtual DbSet<TypeClientAvisSi> TypeClientAvisSis { get; set; }
        public virtual DbSet<UniteEquipeAvisSi> UniteEquipeAvisSis { get; set; }
        public virtual DbSet<UtilisateurEquipeAnalyseAvisSi> UtilisateurEquipeAnalyseAvisSis { get; set; }
        public virtual DbSet<UtilisateurEquipeSuperviseurAvisSi> UtilisateurEquipeSuperviseurAvisSis { get; set; }
        public virtual DbSet<UtilisateurEquipeValidationAvisSi> UtilisateurEquipeValidationAvisSis { get; set; }
        public virtual DbSet<UtilisateurEquipeValidationFlgeAvisSi> UtilisateurEquipeValidationFlgeAvisSis { get; set; }


        public virtual DbSet<ActionTypeDemandeInformationAvis> ActionTypeDemandeInformationAviss { get; set; }
        public virtual DbSet<AnalyseDossierSAvis> AnalyseDossierSAviss { get; set; }
        public virtual DbSet<AnalyseDossierAvisIm> AnalyseDossierAvisIms { get; set; }
        public virtual DbSet<IntermediaryAgreementNotice> IntermediaryAgreementNotices { get; set; }
        public virtual DbSet<FinalAgreementNotice> FinalAgreementNotices { get; set; }
        public virtual DbSet<AppartenanceDocumentAvis> AppartenanceDocumentAviss { get; set; }
        public virtual DbSet<ApplicationAvis> ApplicationAviss { get; set; }
        public virtual DbSet<ConclusionSAvis> ConclusionSAviss { get; set; }
        public virtual DbSet<ConclusionAvisIm> ConclusionAvisIms { get; set; }
        public virtual DbSet<DeclarantEntiteAvis> DeclarantEntiteAviss { get; set; }
        public virtual DbSet<DeclarantEntiteManagerAvis> DeclarantEntiteManagerAviss { get; set; }
        public virtual DbSet<DefaillanceAvis> DefaillanceAviss { get; set; }
        public virtual DbSet<DemandeInformationAvis> DemandeInformationAviss { get; set; }
        public virtual DbSet<DocumentAvis> DocumentAviss { get; set; }
        public virtual DbSet<DocumentDemandeInformationAvis> DocumentDemandeInformationAviss { get; set; }
        public virtual DbSet<DocumentDossierSAvis> DocumentDossierSAviss { get; set; }
        public virtual DbSet<DocumentDossierAvisIm> DocumentDossierAvisIms { get; set; }
        public virtual DbSet<DocumentEntiteConcerneAvis> DocumentEntiteConcerneAviss { get; set; }
        public virtual DbSet<DocumentPlanActionAnalyseAvis> DocumentPlanActionAnalyseAviss { get; set; }
        public virtual DbSet<TypologieDemandeAvis> TypologieDemandeAviss { get; set; }
        public virtual DbSet<TypologieClientAvis> TypologieClientAviss { get; set; }
        public virtual DbSet<DossierSAvis> DossierSAviss { get; set; }
        public virtual DbSet<DossierAvisIm> DossierAvisIms { get; set; }
        public virtual DbSet<TargetEntity> TargetEntitiesOrSubsidiaries { get; set; }
        public virtual DbSet<TargetEntitySubsidiary> Subsidiaries { get; set; }
        public virtual DbSet<TargetEntityOwnership> Owners { get; set; }
        public virtual DbSet<DossierSAvisHisto> DossierSAvisHistos { get; set; }
        public virtual DbSet<DossierAvisImHisto> DossierAvisImHistos { get; set; }
        public virtual DbSet<DossierSAvisPersonneMorale> DossierSAvisPersonneMorales { get; set; }
        public virtual DbSet<DossierSAvisPersonnePhysique> DossierSAvisPersonnePhysiques { get; set; }
        public virtual DbSet<DossierAvisImPersonneMorale> DossierAvisImPersonneMorales { get; set; }
        public virtual DbSet<DossierAvisImPersonnePhysique> DossierAvisImPersonnePhysiques { get; set; }
        public virtual DbSet<DossierDefaillanceSAvis> DossierDefaillanceSAviss { get; set; }
        public virtual DbSet<DossierDefaillanceAvisIm> DossierDefaillanceAvisIms { get; set; }
        public virtual DbSet<DossierValidationFinaleAvis> DossierValidationFinaleAviss { get; set; }
        public virtual DbSet<DossierValidationFinaleFlgeAvis> DossierValidationFinaleFlgeAviss { get; set; }
        public virtual DbSet<DossierValidationIntermediaireAvis> DossierValidationIntermediaireAviss { get; set; }

        public virtual DbSet<DossierValidationIntermediaireFlgeAvis> DossierValidationIntermediaireFlgeAviss
        {
            get;
            set;
        }

        public virtual DbSet<EntiteConcerneSAvis> EntiteConcerneSAviss { get; set; }
        public virtual DbSet<EntiteConcerneAvisIm> EntiteConcerneAvisIms { get; set; }
        public virtual DbSet<EnvironnementAvis> EnvironnementAviss { get; set; }
        public virtual DbSet<EquipeAnalyseAvis> EquipeAnalyseAviss { get; set; }
        public virtual DbSet<EquipeSuperviseurAvis> EquipeSuperviseurAviss { get; set; }
        public virtual DbSet<EquipeValidationAvis> EquipeValidationAviss { get; set; }
        public virtual DbSet<EquipeValidationFlgeAvis> EquipeValidationFlgeAviss { get; set; }
        public virtual DbSet<CategorieAvis> CategorieAviss { get; set; }
        public virtual DbSet<FaqAvis> FaqAviss { get; set; }
        public virtual DbSet<CategorieOutilAvis> CategorieOutilAviss { get; set; }
        public virtual DbSet<PersonneMoraleAvis> PersonneMoraleAviss { get; set; }
        public virtual DbSet<PersonnePhysiqueAvis> PersonnePhysiqueAviss { get; set; }
        public virtual DbSet<PlanActionAnalyseAvis> PlanActionAnalyseAviss { get; set; }
        public virtual DbSet<ResponsableFlgeAvis> ResponsableFlgeAviss { get; set; }
        public virtual DbSet<ResponsableLocalAvis> ResponsableLocalAviss { get; set; }
        public virtual DbSet<RoutageAvis> RoutageAviss { get; set; }
        public virtual DbSet<RoutageFlgeAvis> RoutageFlgeAviss { get; set; }
        public virtual DbSet<StatutAvis> StatutAviss { get; set; }
        public virtual DbSet<StatutDemandeInformationAvis> StatutDemandeInformationAviss { get; set; }
        public virtual DbSet<StatutPlanActionAnalyseAvis> StatutPlanActionAnalyseAviss { get; set; }
        public virtual DbSet<TypeClientAvis> TypeClientAviss { get; set; }
        public virtual DbSet<UniteEquipeAvis> UniteEquipeAviss { get; set; }
        public virtual DbSet<UtilisateurEquipeAnalyseAvis> UtilisateurEquipeAnalyseAviss { get; set; }
        public virtual DbSet<UtilisateurEquipeSuperviseurAvis> UtilisateurEquipeSuperviseurAviss { get; set; }
        public virtual DbSet<UtilisateurEquipeValidationAvis> UtilisateurEquipeValidationAviss { get; set; }
        public virtual DbSet<UtilisateurEquipeValidationFlgeAvis> UtilisateurEquipeValidationFlgeAviss { get; set; }
        public virtual DbSet<QualificationDossier> QualificationDossiers { get; set; }
        public virtual DbSet<QualificationDossierEscalade> QualificationDossierEscalades { get; set; }
        public virtual DbSet<TypeDocumentLab> TypeDocumentLabs { get; set; }
        public virtual DbSet<CategorieDocument> CategorieDocuments { get; set; }
        public virtual DbSet<IdentiteDeclarant> IdentiteDeclarants { get; set; }

        public virtual DbSet<TitreAuquelEntiteAgitAmf> TitreAuquelEntiteAgitAmfs { get; set; }
        public virtual DbSet<TypeActiviteAmf> TypeActiviteAmfs { get; set; }
        public virtual DbSet<TypeInstrumentFinancierAmf> TypeInstrumentFinancierAmfs { get; set; }
        public virtual DbSet<TypeInstrumentUsuellementNegocieAmf> TypeInstrumentUsuellementNegocieAmfs { get; set; }
        public virtual DbSet<TypeProduitDeriveAmf> TypeProduitDeriveAmfs { get; set; }

        public virtual DbSet<DossierOrdreTransactionAmf> DossierOrdreTransactionAmfs { get; set; }
        public virtual DbSet<GroupeEntiteAvis> GroupeEntiteAviss { get; set; }
        public virtual DbSet<EntiteGroupeEntiteAvis> EntiteGroupeEntiteAviss { get; set; }
        public virtual DbSet<DomaineAvis> DomaineAviss { get; set; }
        public virtual DbSet<SousCategorieAvis> SousCategorieAviss { get; set; }

        public virtual DbSet<PrioriteAvis> PrioriteAviss { get; set; }

        public virtual DbSet<SensDemande> SensDemandes { get; set; }

        public virtual DbSet<StatutOrdreOuTransactionAmf> StatutOrdreOuTransactionAmfs { get; set; }
        public virtual DbSet<LienUs> LienUss { get; set; }
        public virtual DbSet<InteretDroitVote> InteretDroitVotes { get; set; }
        public virtual DbSet<EntiteGroupeType> EntiteGroupeTypes { get; set; }
        public virtual DbSet<TypeFond> TypeFonds { get; set; }
        public virtual DbSet<UsLinkNature> UsLinkNatures { get; set; }
        public virtual DbSet<TypeFormulaire> TypeFormulaires { get; set; }
        public virtual DbSet<StatutUtilisateur> StatutUtilisateurs { get; set; }
        public virtual DbSet<DossierDegelPartielGda> DossierDegelPartielGdas { get; set; }
        public virtual DbSet<GDRFluxVirement> GDRFluxVirements { get; set; }
        public virtual DbSet<ApplicationAquisition> ApplicationAquisitions { get; set; }
        public virtual DbSet<CinematiqueGDR> CinematiqueGDRs { get; set; }
        public virtual DbSet<CategorieGDR> CategorieGDRs { get; set; }
        public virtual DbSet<CapaciteJuridique> CapaciteJuridiques { get; set; }
        public virtual DbSet<TypeDate> TypeDates { get; set; }

        #region GDA

        public virtual DbSet<AutorisationPpfGda> AutorisationPpfGdas { get; set; }
        public virtual DbSet<StatutActionsGda> StatutActionsGdas { get; set; }
        public virtual DbSet<StatutImmediateActionsGda> StatutImmediateActionsGdas { get; set; }
        public virtual DbSet<StatutOscGda> StatutOscGdas { get; set; }
        public virtual DbSet<DebitCreditOsc> DebitCreditOscs { get; set; }

        public virtual DbSet<TypesLiens> TypesLiens { get; set; }
        public virtual DbSet<TypesActifs> TypesActifs { get; set; }

        public virtual DbSet<TypologiesActifs> TypologiesActifs { get; set; }
        public virtual DbSet<CanalBdfGda> CanalBdfGdas { get; set; }
        public virtual DbSet<CategorieCanalBdfGda> CategorieCanalBdfGdas { get; set; }
        public virtual DbSet<ReferentielImmediateActionsGda> ReferentielImmediateActionsGdas { get; set; }
        public virtual DbSet<CategorieGda> CategorieGdas { get; set; }
        public virtual DbSet<CategorieModeOperatoireGda> CategorieModeOperatoireGdas { get; set; }
        public virtual DbSet<CategorieMotifRejetChequeGda> CategorieMotifRejetChequeGdas { get; set; }
        public virtual DbSet<CategorieTypePaiementGda> CategorieTypePaiementGdas { get; set; }
        public virtual DbSet<CategorieTypologieBdfGda> CategorieTypologieBdfGdas { get; set; }
        public virtual DbSet<CcBlockedGda> CcBlockedGda { get; set; }
        public virtual DbSet<ComplementMotifRejetChequeGda> ComplementMotifRejetChequeGdas { get; set; }
        public virtual DbSet<DemandeInformationGda> DemandeInformationGdas { get; set; }
        public virtual DbSet<StatutDemandeInformationGda> StatutDemandeInformationGdas { get; set; }
        public virtual DbSet<TypeDemandeInformationGda> TypdeDemandeInformationGdas { get; set; }
        public virtual DbSet<DocumentDemandeInformationGda> DocumentDemandeInformationGdas { get; set; }
        public virtual DbSet<DocumentDossierGda> DocumentsDossierGda { get; set; }
        public virtual DbSet<DocumentGda> DocumentsGda { get; set; }
        public virtual DbSet<DossierGda> DossierGdas { get; set; }
        public virtual DbSet<DossierGdaImmediateAction> DossierGdaImmediateActions { get; set; }
        public virtual DbSet<DossierGdaCurrentAction> DossierGdaCurrentActions { get; set; }

        public virtual DbSet<DossierGdaOsc> DossierGdaOscs { get; set; }
        public virtual DbSet<DossierGdaHisto> DossierGdaHistos { get; set; }
        public virtual DbSet<DossierGdaPersonneMorale> DossierGdaPersonneMorales { get; set; }

        public virtual DbSet<DossierGdaPersonnePhysique> DossierGdaPersonnePhysiques { get; set; }

        //public virtual DbSet<DossierGdaResult> DossierGdaResults { get; set; }
        public virtual DbSet<EntiteGda> EntiteGdas { get; set; }
        public virtual DbSet<EtablissementDeclarantGda> EtablissementDeclarantGdas { get; set; }
        public virtual DbSet<RegimeSanctionGda> RegimeSanctionGdas { get; set; }
        public virtual DbSet<ModeOperatoireGda> ModeOperatoireGdas { get; set; }
        public virtual DbSet<MotifExemptionNotificationGda> MotifExemptionNotificationGdas { get; set; }
        public virtual DbSet<MotifRejetChequeGda> MotifRejetChequeGdas { get; set; }
        public virtual DbSet<OrigineDirectionGda> OrigineDirectionGdas { get; set; }
        public virtual DbSet<OrigineGda> OrigineGdas { get; set; }
        public virtual DbSet<PersonneMoraleGda> PersonneMoraleGdas { get; set; }
        public virtual DbSet<PersonnePhysiqueGda> PersonnePhysiqueGdas { get; set; }
        public virtual DbSet<PersonnePhysiqueResultGda> PersonnePhysiqueResultGdas { get; set; }
        public virtual DbSet<PersonneMoraleResultGda> PersonneMoraleResultGdas { get; set; }
        public virtual DbSet<ReferentielActionHorsCompte> ReferentielActionHorsComptes { get; set; }
        public virtual DbSet<SafetyInstructionsGda> SafetyInstructionsGda { get; set; }
        public virtual DbSet<SecteurGda> SecteurGdas { get; set; }
        public virtual DbSet<StatutDossierGda> StatutDossierGdas { get; set; }
        public virtual DbSet<StatutPersonneGda> StatutPersonneGdas { get; set; }
        public virtual DbSet<RegimeJuridiqueGda> RegimeJuridiqueGdas { get; set; }
        public virtual DbSet<TypeCollecteChequeGda> TypeCollecteChequeGdas { get; set; }
        public virtual DbSet<TypePaiementGda> TypePaiementGdas { get; set; }
        public virtual DbSet<TypeRelationAffaireGda> TypeRelationAffaireGdas { get; set; }
        public virtual DbSet<FondsInvestissementPersonnePhysique> FondsInvestissementPersonnePhysiques { get; set; }
        public virtual DbSet<TypologieAvoirPersonneGeleePersonnePhysique> TypologieAvoirPersonneGeleePersonnePhysiques { get; set; }
        public virtual DbSet<TypologieAvoirPersonneLieePersonnePhysique> TypologieAvoirPersonneLieePersonnePhysiques { get; set; }
        public virtual DbSet<LienPersonnePhysiqueGda> LienPersonnePhysiqueGdas { get; set; }
        public virtual DbSet<LienPersonnePhysiquePmGda> LienPersonnePhysiquePmGdas { get; set; }
        public virtual DbSet<TypologieAvoirPersonneGeleePersonneMorale> TypologieAvoirPersonneGeleePersonneMorales
        {
            get;
            set;
        }

        public virtual DbSet<TypologieAvoirPersonneLieePersonneMorale> TypologieAvoirPersonneLieePersonneMorales
        {
            get;
            set;
        }

        public virtual DbSet<LienPersonneMoraleGda> LienPersonneMoraleGdas { get; set; }

        public virtual DbSet<LienPersonneMoralePpGda> LienPersonneMoralePpGdas { get; set; }
        public virtual DbSet<CategorieAvoir> CategoriesAvoir { get; set; }
        public virtual DbSet<Couleur> Couleurs { get; set; }

        #endregion

        #region Transfert
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DocumentShare> DocumentShares { get; set; }
        public virtual DbSet<StatutDocument> StatutDocuments { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActionTypeDemandeInformation>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);
            });

            modelBuilder.Entity<CategorieDocument>(entity => { entity.Property(e => e.IsActive); });
            modelBuilder.Entity<Activite>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<AnalyseDossierEscalade>(entity =>
            {
                entity.HasIndex(e => e.AnalysteId);
                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.DossierEscaladeId);

                entity.HasOne(d => d.Analyste)
                    .WithMany(p => p.AnalyseDossierEscaladeAnalystes)
                    .HasForeignKey(d => d.AnalysteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnalyseDossierEscalade_Analyste_AnalysteId");

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.AnalyseDossierEscaladeModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnalyseDossierEscalade_Modificateur_ModificateurId");

                entity.HasOne(d => d.DossierEscalade)
                    .WithMany(p => p.AnalyseDossierEscalades)
                    .HasForeignKey(d => d.DossierEscaladeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.Analyse).Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.EnvironnementId);

                entity.HasIndex(e => e.ModificateurId);
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName).HasDatabaseName("RoleNameIndex");
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail).HasDatabaseName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasDatabaseName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<AuditTrail>(entity =>
            {
                entity.HasIndex(e => e.UtilisateurId);

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.AuditTrails)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AuditTrailEvent>(entity => { entity.HasIndex(e => e.AuditTrailId); });

            modelBuilder.Entity<AutorisationPpf>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<AvisLab>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);


                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EnglishDescription).HasMaxLength(1000);

                entity.Property(e => e.EnglishName).HasMaxLength(500);

                entity.Property(e => e.FrenchDescription)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.FrenchName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.IsActive)
                    .IsRequired();

                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.AvisLabCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.AvisLabModificateurs)
                    .HasForeignKey(d => d.ModificateurId);
            });

            modelBuilder.Entity<CanalBdf>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<CanalEntreeEnRelation>(entity => { entity.Property(e => e.IsActive); });

            modelBuilder.Entity<CategorieCanalBdf>(entity =>
            {
                entity.HasIndex(e => e.CanalBdfId);

                entity.HasIndex(e => e.CategorieId);
            });

            modelBuilder.Entity<CategorieEtablissementDeclarant>(entity =>
            {
                entity.HasIndex(e => e.CategorieId);

                entity.HasIndex(e => e.EtablissementDeclarantId);
            });

            modelBuilder.Entity<CategorieFraude>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.ParentId);

                entity.HasIndex(e => e.ProduitId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<CategorieLab>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.DirectionId);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.CategorieLabs)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);


                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EnglishDescription).HasMaxLength(1000);

                entity.Property(e => e.EnglishName).HasMaxLength(500);

                entity.Property(e => e.FrenchDescription)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.FrenchName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.IsActive)
                    .IsRequired();


                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.CategorieLabCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.CategorieLabModificateurs)
                    .HasForeignKey(d => d.ModificateurId);

                entity.HasOne(d => d.GroupeCategorieLab)
                    .WithMany(p => p.CategorieLabs)
                    .HasForeignKey(d => d.GroupeCategorieLabId);

                entity.Property(e => e.Partage)
                    .IsRequired()
                    .HasColumnName("Partage");

                entity.Property(e => e.IsValidation)
                    .IsRequired()
                    .HasColumnName("IsValidation");

                entity.Property(e => e.IsSanction)
                    .IsRequired()
                    .HasColumnName("IsSanction");

                entity.HasIndex(e => e.DelaiIndicatif);
            });


            modelBuilder.Entity<GroupeCategorieLab>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EnglishDescription).HasMaxLength(1000);

                entity.Property(e => e.EnglishName).HasMaxLength(500);

                entity.Property(e => e.FrenchDescription)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.FrenchName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.IsActive)
                    .IsRequired();


                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.GroupeCategorieLabCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.GroupeCategorieLabModificateurs)
                    .HasForeignKey(d => d.ModificateurId);

                entity.HasIndex(e => e.DirectionId);
            });

            modelBuilder.Entity<GroupeOrigineLab>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EnglishDescription).HasMaxLength(1000);

                entity.Property(e => e.EnglishName).HasMaxLength(500);

                entity.Property(e => e.FrenchDescription)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.FrenchName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.IsActive)
                    .IsRequired();


                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.GroupeOrigineLabCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.GroupeOrigineLabModificateurs)
                    .HasForeignKey(d => d.ModificateurId);

                entity.HasIndex(e => e.DirectionId);
            });
            modelBuilder.Entity<CategorieGroupeLab>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EnglishDescription).HasMaxLength(1000);

                entity.Property(e => e.IsDs)
                    .IsRequired()
                    .HasColumnName("IsDS");

                entity.Property(e => e.EnglishName).HasMaxLength(500);

                entity.Property(e => e.FrenchDescription)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.FrenchName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.IsActive)
                    .IsRequired();


                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.CategorieGroupeLabCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.CategorieGroupeLabModificateurs)
                    .HasForeignKey(d => d.ModificateurId);
            });

            modelBuilder.Entity<OrigineGroupeLab>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EnglishDescription).HasMaxLength(1000);

                entity.Property(e => e.EnglishName).HasMaxLength(500);

                entity.Property(e => e.FrenchDescription)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.FrenchName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.IsActive)
                    .IsRequired();

                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.OrigineGroupeLabCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.OrigineGroupeLabModificateurs)
                    .HasForeignKey(d => d.ModificateurId);
            });
            modelBuilder.Entity<CategorieModeOperatoire>(entity =>
            {
                entity.HasIndex(e => e.CategorieId);

                entity.HasIndex(e => e.ModeOperatoireId);
            });

            modelBuilder.Entity<CategorieMotifRejetCheque>(entity =>
            {
                entity.HasIndex(e => e.CategorieId);

                entity.HasIndex(e => e.MotifRejetChequeId);
            });

            modelBuilder.Entity<CategorieTracfin>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<CategorieTypeCollecteCheque>(entity =>
            {
                entity.HasIndex(e => e.CategorieId);

                entity.HasIndex(e => e.TypeCollecteChequeId);
            });

            modelBuilder.Entity<CategorieTypePaiement>(entity =>
            {
                entity.HasIndex(e => e.CategorieId);

                entity.HasIndex(e => e.TypePaiementId);
            });

            modelBuilder.Entity<CategorieTypologieBdf>(entity =>
            {
                entity.HasIndex(e => e.CategorieId);

                entity.HasIndex(e => e.TypologieBdfId);

                entity.HasOne(d => d.Categorie)
                    .WithMany(p => p.CategorieTypologieBdfs)
                    .HasForeignKey(d => d.CategorieId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TypologieBdf)
                    .WithMany(p => p.CategorieTypologieBdfs)
                    .HasForeignKey(d => d.TypologieBdfId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TypologieGda>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<Civilite>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<ComplementMotifRejetCheque>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.MotifRejetChequeId);

                entity.Property(e => e.IsActive);

                entity.HasOne(d => d.MotifRejetCheque)
                    .WithMany(p => p.ComplementMotifRejetCheques)
                    .HasForeignKey(d => d.MotifRejetChequeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ComplementVoie>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<ConclusionEscalade>(entity =>
            {
                entity.HasIndex(e => e.AnalysteId);
                entity.HasOne(d => d.Analyste)
                    .WithMany(p => p.ConclusionEscaladeAnalystes)
                    .HasForeignKey(d => d.AnalysteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConclusionEscalade_Analyste_AnalysteId");

                entity.HasIndex(e => e.ModificateurId);
                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.ConclusionEscaladeModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConclusionEscalade_Modificateur_ModificateurId");
                entity.Property(e => e.Conclusion)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<Coordonnee>(entity =>
            {
                entity.HasIndex(e => e.ComplementVoieId);

                entity.HasIndex(e => e.PaysId);

                entity.HasIndex(e => e.TypeAdresseId);

                entity.HasIndex(e => e.TypeVoieId);

                entity.HasOne(d => d.Pays)
                    .WithMany(p => p.Coordonnees)
                    .HasForeignKey(d => d.PaysId);

                entity.HasOne(d => d.TypeAdresse)
                    .WithMany(p => p.Coordonnees)
                    .HasForeignKey(d => d.TypeAdresseId);

                entity.HasOne(d => d.TypeVoie)
                    .WithMany(p => p.Coordonnees)
                    .HasForeignKey(d => d.TypeVoieId);

                entity.HasOne(d => d.ComplementVoie)
                    .WithMany(p => p.Coordonnees)
                    .HasForeignKey(d => d.ComplementVoieId);
            });

            modelBuilder.Entity<OrganismeLab>(entity =>
            {
                entity.HasIndex(e => e.ComplementVoieId);

                entity.HasIndex(e => e.PaysId);

                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.TypeVoieId);

                entity.HasIndex(e => e.ProfessionId);


                entity.HasOne(d => d.Pays)
                    .WithMany(p => p.OrganismeLabs)
                    .HasForeignKey(d => d.PaysId);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.OrganismeLabs)
                    .HasForeignKey(d => d.DirectionId);

                entity.HasOne(d => d.TypeVoie)
                    .WithMany(p => p.OrganismeLabs)
                    .HasForeignKey(d => d.TypeVoieId);

                entity.HasOne(d => d.ComplementVoie)
                    .WithMany(p => p.OrganismeLabs)
                    .HasForeignKey(d => d.ComplementVoieId);

                entity.HasOne(d => d.Profession)
                    .WithMany(p => p.OrganismeLabs)
                    .HasForeignKey(d => d.ProfessionId);
            });

            modelBuilder.Entity<DocumentDeclarationTracfin>(entity =>
            {
                entity.HasOne(d => d.DeclarationTracfin)
                    .WithMany(p => p.DocumentDeclarationTracfins)
                    .HasForeignKey(d => d.DeclarationTracfinId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TypeDocument)
                    .WithMany(p => p.DocumentDeclarationTracfins)
                    .HasForeignKey(d => d.TypeDocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<OperationSuspectDeclarationTracfin>(entity =>
            {
                entity.HasOne(d => d.DeclarationTracfin)
                    .WithMany(p => p.OperationSuspectDeclarationTracfins)
                    .HasForeignKey(d => d.DeclarationTracfinId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.MontantTotal)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<OperationSusceptibleOpposition>(entity =>
            {
                entity.Property(e => e.Montant)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<OperationEnCoursDeclarationTracfin>(entity =>
            {
                entity.HasOne(d => d.DeclarationTracfin)
                    .WithMany(p => p.OperationEnCoursDeclarationTracfins)
                    .HasForeignKey(d => d.DeclarationTracfinId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(d => d.TypeOperation)
                    .WithMany(p => p.OperationEnCoursDeclarationTracfins)
                    .HasForeignKey(d => d.TypeOperationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<AavReference>(entity =>
            {
                entity.HasIndex(e => e.DeclarationTracfinId);

                entity.HasOne(d => d.DeclarationTracfin)
                    .WithMany(p => p.AavReferences)
                    .HasForeignKey(d => d.DeclarationTracfinId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DeclarationTracfin>(entity =>
            {
                entity.HasOne(d => d.DossierLab)
                    .WithMany(p => p.DeclarationTracfins)
                    .HasForeignKey(d => d.DossierLabId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.OrganismeLab)
                    .WithMany(p => p.DeclarationTracfins)
                    .HasForeignKey(d => d.OrganismeId);

                entity.HasOne(d => d.PrincipalInstrumentFinancier)
                    .WithMany(p => p.DeclarationTracfins)
                    .HasForeignKey(d => d.PrincipalInstrumentId);

                entity.Property(e => e.Motifs)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Analyses)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<Defaillance>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.EvenementId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.DefaillanceCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Delegation>(entity =>
            {
                entity.HasIndex(e => e.ActiviteId);

                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.DelegueId);

                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.UtilisateurId);

                entity.HasOne(d => d.Activite)
                    .WithMany(p => p.Delegations)
                    .HasForeignKey(d => d.ActiviteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Delegue)
                    .WithMany(p => p.DelegationDelegues)
                    .HasForeignKey(d => d.DelegueId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.Delegations)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.DelegationUtilisateurs)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DemandeInformation>(entity =>
            {
                entity.HasIndex(e => e.ActionTypeDemandeInformationId);

                entity.HasIndex(e => e.AnalyseDossierEscaladeId);

                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.DomaineId);

                entity.HasIndex(e => e.EvenementId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.StatutDemandeInformationId);

                entity.HasOne(d => d.AnalyseDossierEscalade)
                    .WithMany(p => p.DemandeInformations)
                    .HasForeignKey(d => d.AnalyseDossierEscaladeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<PlanActionAnalyse>(entity =>
            {
                entity.HasIndex(e => e.AnalyseDossierEscaladeId);

                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.PorteurId);

                entity.HasIndex(e => e.StatutPlanActionAnalyseId);

                entity.HasIndex(e => e.AnalysteDeSuiviId);

                entity.HasOne(d => d.AnalyseDossierEscalade)
                    .WithMany(p => p.PlanActionAnalyses)
                    .HasForeignKey(d => d.AnalyseDossierEscaladeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DirectionPorteur)
                    .WithMany(p => p.PlanActionAnalyses)
                    .HasForeignKey(d => d.DirectionPorteurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Porteur)
                    .WithMany(p => p.PlanActionAnalysePorteurs)
                    .HasForeignKey(d => d.PorteurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.Response)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Intitule)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Description)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Commentaire)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<Devise>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<Bunit>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);
                entity.HasIndex(e => e.DirectionId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<Direction>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.DeviseId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Direction_Direction");

                entity.HasOne(d => d.Pays)
                    .WithMany(p => p.Direction)
                    .HasForeignKey(d => d.PaysId)
                    .HasConstraintName("FK_Direction_Pays");
                entity.Property(e => e.IsActive);


                entity.HasOne(d => d.CarnetAdresses)
                    .WithMany(p => p.Direction)
                    .HasForeignKey(d => d.CarnetAdressesId)
                    .HasConstraintName("FK_Direction_CarnetAdresses");
                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<DirectionAccessible>(entity =>
            {
                entity.HasIndex(e => e.ActiviteId);

                entity.HasIndex(e => e.DirectionAccId);

                entity.HasIndex(e => e.DirectionId);
            });

            modelBuilder.Entity<PaysAccessible>(entity =>
            {
                entity.HasIndex(e => e.PaysAccId);

                entity.HasIndex(e => e.PaysId);
            });

            modelBuilder.Entity<DirectionAccessibleException>(entity =>
            {
                entity.HasIndex(e => e.TypeExceptionId);

                entity.HasIndex(e => e.DirectionAccId);

                entity.HasIndex(e => e.DirectionId);
            });

            modelBuilder.Entity<DocumentDemandeInformation>(entity =>
            {
                entity.HasIndex(e => e.DemandeInformationRequestId);

                entity.HasIndex(e => e.DemandeInformationResponseId)
                    .HasDatabaseName("IX_DocumentDemandeInformation_DemandeInformationReponseId");

                entity.HasIndex(e => e.DocumentEscaladeId);

                entity.HasIndex(e => e.UtilisateurId);

                entity.HasOne(d => d.DocumentEscalade)
                    .WithMany(p => p.DocumentDemandeInformations)
                    .HasForeignKey(d => d.DocumentEscaladeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DocumentDossierEscalade>(entity =>
            {
                entity.HasIndex(e => e.AppartenanceDocumentId);
                entity.HasIndex(e => e.CategorieDocumentId);

                entity.HasIndex(e => e.DocumentEscaladeId);

                entity.HasIndex(e => e.DossierEscaladeId);

                entity.HasIndex(e => e.EquipeAnalyseId);

                entity.HasIndex(e => e.UtilisateurId);

                entity.HasOne(d => d.DocumentEscalade)
                    .WithMany(p => p.DocumentDossierEscalades)
                    .HasForeignKey(d => d.DocumentEscaladeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.DossierEscalade)
                    .WithMany(p => p.DocumentDossierEscalades)
                    .HasForeignKey(d => d.DossierEscaladeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DocumentDossierFraude>(entity =>
            {
                entity.HasIndex(e => e.DocumentFraudeId);

                entity.HasIndex(e => e.DossierFraudeId);

                entity.HasOne(d => d.DocumentFraude)
                    .WithMany(p => p.DocumentDossierFraudes)
                    .HasForeignKey(d => d.DocumentFraudeId);
            });

            modelBuilder.Entity<DocumentDossierLab>(entity =>
            {
                entity.HasIndex(e => e.DocumentLabId);

                entity.HasIndex(e => e.DossierLabId);

                entity.HasIndex(e => e.CountryRelaseId);
                entity.HasIndex(e => e.TypeDocumentLabId);
                entity.HasIndex(e => e.CategorieDocumentId);

                entity.HasOne(d => d.DocumentLab)
                    .WithMany(p => p.DocumentDossierLabs)
                    .HasForeignKey(d => d.DocumentLabId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.DossierLab)
                    .WithMany(p => p.DocumentDossierLabs)
                    .HasForeignKey(d => d.DossierLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CategorieDocument)
                    .WithMany(p => p.DocumentDossierLabs)
                    .HasForeignKey(d => d.CategorieDocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TypeDocumentLab)
                    .WithMany(p => p.DocumentDossierLabs)
                    .HasForeignKey(d => d.TypeDocumentLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CountryRelase)
                    .WithMany(p => p.DocumentDossierLabs)
                    .HasForeignKey(d => d.CountryRelaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DocumentToolDetail>(entity =>
            {
                entity.HasIndex(e => e.ActiviteId).HasDatabaseName("IX_DocumentInformation_Activite_ActiviteId");

                entity.HasIndex(e => e.CreateurId).HasDatabaseName("IX_DocumentInformation_Utilisateur_CreateurId");

                entity.HasIndex(e => e.DocumentToolId).HasDatabaseName("IX_DocumentInformation_Document_DocumentId");

                entity.HasIndex(e => e.LangueId).HasDatabaseName("IX_DocumentInformation_Langue_LangueId");

                entity.HasIndex(e => e.ModificateurId)
                    .HasDatabaseName("IX_DocumentInformation_Utilisateur_ModificateurId");

                entity.HasOne(d => d.Activite)
                    .WithMany(p => p.DocumentToolDetails)
                    .HasForeignKey(d => d.ActiviteId)
                    .HasConstraintName("FK_DocumentInformation_Activite_ActiviteId");

                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.DocumentToolDetailCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .HasConstraintName("FK_DocumentInformation_Utilisateur_CreateurId");

                entity.HasOne(d => d.DocumentTool)
                    .WithMany(p => p.DocumentToolDetails)
                    .HasForeignKey(d => d.DocumentToolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentInformation_DocumentTool_DocumentId");

                entity.HasOne(d => d.Langue)
                    .WithMany(p => p.DocumentToolDetails)
                    .HasForeignKey(d => d.LangueId)
                    .HasConstraintName("FK_DocumentInformation_Langue_LangueId");

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.DocumentToolDetailModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .HasConstraintName("FK_DocumentInformation_Utilisateur_ModificateurId");
            });

            modelBuilder.Entity<Domaine>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);
            });

            modelBuilder.Entity<DossierDefaillance>(entity =>
            {
                entity.HasIndex(e => e.DefaillanceId);

                entity.HasIndex(e => e.DossierId);

                entity.HasOne(d => d.Defaillance)
                    .WithMany(p => p.DossierDefaillances)
                    .HasForeignKey(d => d.DefaillanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Dossier)
                    .WithMany(p => p.DossierDefaillances)
                    .HasForeignKey(d => d.DossierId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<DossierEscalade>(entity =>
            {
                entity.HasIndex(e => e.AnalysteCourantId);

                entity.HasIndex(e => e.AnalysteCourantModificateurId);

                entity.HasIndex(e => e.ApplicationId);

                entity.HasIndex(e => e.ConclusionEscaladeId);

                entity.HasIndex(e => e.DeclarantEntiteManagerId);

                entity.HasIndex(e => e.DomaineId);

                entity.HasIndex(e => e.DossierValidationFinaleId);

                entity.HasIndex(e => e.DossierValidationIntermediaireId);

                entity.HasIndex(e => e.DossierValidationFinaleFlgeId);

                entity.HasIndex(e => e.DossierValidationIntermediaireFlgeId);

                entity.HasIndex(e => e.EvenementId);

                entity.HasIndex(e => e.ResponsableFlgeId);

                entity.HasIndex(e => e.ResponsableLocalId);

                entity.HasIndex(e => e.StatutEscaladeId);

                entity.HasIndex(e => e.ValidateurFinaleCourantId);

                entity.HasIndex(e => e.ValidateurFinaleCourantModificateurId);

                entity.HasIndex(e => e.ValidateurIntermCourantId);

                entity.HasIndex(e => e.ValidateurIntermCourantModificateurId);

                entity.HasIndex(e => e.DeviseId);

                entity.HasOne(d => d.Devise)
                    .WithMany(p => p.DossierEscalades)
                    .HasForeignKey(d => d.DeviseId);

                entity.HasIndex(e => new { e.Id, e.ConclusionEscaladeId })
                    .HasDatabaseName("AK_DossierEscalade_ConclusionEscaladeId")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.DeclarantEntiteManagerId })
                    .HasDatabaseName("AK_DossierEscalade_DeclarantEntiteManagerId")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.DossierValidationFinaleId })
                    .HasDatabaseName("AK_DossierEscalade_DossierValidationFinaleId")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.DossierValidationIntermediaireId })
                    .HasDatabaseName("AK_DossierEscalade_DossierValidationIntermediaireId")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.DossierValidationFinaleFlgeId })
                    .HasDatabaseName("AK_DossierEscalade_DossierValidationFinaleFlgeId")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.DossierValidationIntermediaireFlgeId })
                    .HasDatabaseName("AK_DossierEscalade_DossierValidationIntermediaireFlgeId")
                    .IsUnique();


                entity.HasOne(d => d.AnalysteCourant)
                    .WithMany(p => p.DossierEscaladeAnalysteCourants)
                    .HasForeignKey(d => d.AnalysteCourantId)
                    .HasConstraintName("FK_DossierEscalade_AnalysteCourant_AnalysteCourantId");

                entity.HasOne(d => d.AnalysteCourantModificateur)
                    .WithMany(p => p.DossierEscaladeAnalysteCourantModificateurs)
                    .HasForeignKey(d => d.AnalysteCourantModificateurId)
                    .HasConstraintName("FK_DossierEscalade_AnalysteCourant_AnalysteCourantModificateurId");

                entity.HasOne(d => d.Domaine)
                    .WithMany(p => p.DossierEscalades)
                    .HasForeignKey(d => d.DomaineId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Evenement)
                    .WithMany(p => p.DossierEscalades)
                    .HasForeignKey(d => d.EvenementId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.StatutEscalade)
                    .WithMany(p => p.DossierEscalades)
                    .HasForeignKey(d => d.StatutEscaladeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ValidateurFinaleCourant)
                    .WithMany(p => p.DossierEscaladeValidateurFinaleCourants)
                    .HasForeignKey(d => d.ValidateurFinaleCourantId)
                    .HasConstraintName("FK_DossierEscalade_ValidateurFinaleCourant_ValidateurFinaleCourantId");

                entity.HasOne(d => d.ValidateurFinaleCourantModificateur)
                    .WithMany(p => p.DossierEscaladeValidateurFinaleCourantModificateurs)
                    .HasForeignKey(d => d.ValidateurFinaleCourantModificateurId)
                    .HasConstraintName(
                        "FK_DossierEscalade_ValidateurFinaleCourant_ValidateurFinaleCourantModificateurId");

                entity.HasOne(d => d.ValidateurIntermCourant)
                    .WithMany(p => p.DossierEscaladeValidateurIntermCourants)
                    .HasForeignKey(d => d.ValidateurIntermCourantId)
                    .HasConstraintName("FK_DossierEscalade_ValidateurIntermCourant_ValidateurIntermCourantId");

                entity.HasOne(d => d.ValidateurIntermCourantModificateur)
                    .WithMany(p => p.DossierEscaladeValidateurIntermCourantModificateurs)
                    .HasForeignKey(d => d.ValidateurIntermCourantModificateurId)
                    .HasConstraintName(
                        "FK_DossierEscalade_ValidateurIntermCourant_ValidateurIntermCourantModificateurId");

                entity.Property(e => e.AvisFlge)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.RemonteeResponsableLocal)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.SyntheseAvisResponsableLocal)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.DescriptionEvenement)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.PartieConcerne)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.ProblemeOutil)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.ProgrammeSanction)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireEntiteLocale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireFlge)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireDdc)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.ComplementInformation)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireSuperviseur)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));

                entity.Property(e => e.InfCompleClient)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.InfCompleContrepartie)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder
                .Entity<DocumentEscalade>(entity =>
                {
                    entity.Property(e => e.FileContent)
                        .Metadata
                        .SetValueComparer(
                            new ValueComparer<byte[]>(
                                (c1, c2) => c1.SequenceEqual(c2),
                                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                c => c.ToArray()));
                });

            modelBuilder
                .Entity<DemandeInformation>(entity =>
                {
                    entity.Property(e => e.Demande)
                        .Metadata
                        .SetValueComparer(
                            new ValueComparer<byte[]>(
                                (c1, c2) => c1.SequenceEqual(c2),
                                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                c => c.ToArray()));
                    entity.Property(e => e.Titre)
                        .Metadata
                        .SetValueComparer(
                            new ValueComparer<byte[]>(
                                (c1, c2) => c1.SequenceEqual(c2),
                                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                c => c.ToArray()));
                    entity.Property(e => e.Retour)
                        .Metadata
                        .SetValueComparer(
                            new ValueComparer<byte[]>(
                                (c1, c2) => c1.SequenceEqual(c2),
                                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                c => c.ToArray()));
                });


            modelBuilder.Entity<DossierEscaladeHisto>(entity =>
            {
                entity.HasIndex(e => e.ActionId);

                entity.HasIndex(e => e.AnalysteCourantId);

                entity.HasIndex(e => e.AnalysteCourantModificateurId);

                entity.HasIndex(e => e.ApplicationId);

                entity.HasIndex(e => e.ConclusionEscaladeId);

                entity.HasIndex(e => e.DeclarantEntiteManagerId);

                entity.HasIndex(e => e.DomaineId);

                entity.HasIndex(e => e.DossierEscaladeId);

                entity.HasIndex(e => e.DossierValidationFinaleId);

                entity.HasIndex(e => e.DossierValidationIntermediaireId);

                entity.HasIndex(e => e.EvenementId);

                entity.HasIndex(e => e.ResponsableFlgeId);

                entity.HasIndex(e => e.ResponsableLocalId);

                entity.HasIndex(e => e.StatutEscaladeId);

                entity.HasIndex(e => e.ValidateurFinaleCourantId);

                entity.HasIndex(e => e.ValidateurFinaleCourantModificateurId);

                entity.HasIndex(e => e.ValidateurIntermCourantId);

                entity.HasIndex(e => e.ValidateurIntermCourantModificateurId);

                entity.HasIndex(e => new { e.Id, e.ConclusionEscaladeId })
                    .HasDatabaseName("AK_DossierEscaladeHisto_ConclusionEscaladeId")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.DeclarantEntiteManagerId })
                    .HasDatabaseName("AK_DossierEscaladeHisto_DeclarantEntiteManagerId")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.DossierValidationFinaleId })
                    .HasDatabaseName("AK_DossierEscaladeHisto_DossierValidationFinaleId")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.DossierValidationIntermediaireId })
                    .HasDatabaseName("AK_DossierEscaladeHisto_DossierValidationIntermediaireId")
                    .IsUnique();

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.DossierEscaladeHistos)
                    .HasForeignKey(d => d.ActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.AnalysteCourant)
                    .WithMany(p => p.DossierEscaladeHistoAnalysteCourants)
                    .HasForeignKey(d => d.AnalysteCourantId)
                    .HasConstraintName("FK_DossierEscaladeHisto_AnalysteCourant_AnalysteCourantId");

                entity.HasOne(d => d.AnalysteCourantModificateur)
                    .WithMany(p => p.DossierEscaladeHistoAnalysteCourantModificateurs)
                    .HasForeignKey(d => d.AnalysteCourantModificateurId)
                    .HasConstraintName("FK_DossierEscaladeHisto_AnalysteCourant_AnalysteCourantModificateurId");

                entity.HasOne(d => d.Domaine)
                    .WithMany(p => p.DossierEscaladeHistos)
                    .HasForeignKey(d => d.DomaineId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DossierEscalade)
                    .WithMany(p => p.DossierEscaladeHistos)
                    .HasForeignKey(d => d.DossierEscaladeId)
                    .HasConstraintName("FK_DossierEscaladeHisto_DossierEscaladeHisto_DossierEscaladeId");

                entity.HasOne(d => d.Evenement)
                    .WithMany(p => p.DossierEscaladeHistos)
                    .HasForeignKey(d => d.EvenementId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.StatutEscalade)
                    .WithMany(p => p.DossierEscaladeHistos)
                    .HasForeignKey(d => d.StatutEscaladeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ValidateurFinaleCourant)
                    .WithMany(p => p.DossierEscaladeHistoValidateurFinaleCourants)
                    .HasForeignKey(d => d.ValidateurFinaleCourantId)
                    .HasConstraintName("FK_DossierEscaladeHisto_ValidateurFinaleCourant_ValidateurFinaleCourantId");

                entity.HasOne(d => d.ValidateurFinaleCourantModificateur)
                    .WithMany(p => p.DossierEscaladeHistoValidateurFinaleCourantModificateurs)
                    .HasForeignKey(d => d.ValidateurFinaleCourantModificateurId)
                    .HasConstraintName(
                        "FK_DossierEscaladeHisto_ValidateurFinaleCourant_ValidateurFinaleCourantModificateurId");

                entity.HasOne(d => d.ValidateurIntermCourant)
                    .WithMany(p => p.DossierEscaladeHistoValidateurIntermCourants)
                    .HasForeignKey(d => d.ValidateurIntermCourantId)
                    .HasConstraintName("FK_DossierEscaladeHisto_ValidateurIntermCourant_ValidateurIntermCourantId");

                entity.HasOne(d => d.ValidateurIntermCourantModificateur)
                    .WithMany(p => p.DossierEscaladeHistoValidateurIntermCourantModificateurs)
                    .HasForeignKey(d => d.ValidateurIntermCourantModificateurId)
                    .HasConstraintName(
                        "FK_DossierEscaladeHisto_ValidateurIntermCourant_ValidateurIntermCourantModificateurId");

                entity.Property(e => e.AvisFlge)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));

                entity.Property(e => e.RemonteeResponsableLocal)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.SyntheseAvisResponsableLocal)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.DescriptionEvenement)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.PartieConcerne)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.ProblemeOutil)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.ProgrammeSanction)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<DossierEscaladePersonneMorale>(entity =>
            {
                entity.HasIndex(e => e.DossierEscaladeId);

                entity.HasIndex(e => e.PersonneMoraleEscaladeId);

                entity.HasOne(d => d.DossierEscalade)
                    .WithMany(p => p.DossierEscaladePersonneMorales)
                    .HasForeignKey(d => d.DossierEscaladeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PersonneMoraleEscalade)
                    .WithMany(p => p.DossierEscaladePersonneMorales)
                    .HasForeignKey(d => d.PersonneMoraleEscaladeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DossierEscaladePersonnePhysique>(entity =>
            {
                entity.HasIndex(e => e.DossierEscaladeId);

                entity.HasIndex(e => e.PersonnePhysiqueEscaladeId);

                entity.HasOne(d => d.DossierEscalade)
                    .WithMany(p => p.DossierEscaladePersonnePhysiques)
                    .HasForeignKey(d => d.DossierEscaladeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PersonnePhysiqueEscalade)
                    .WithMany(p => p.DossierEscaladePersonnePhysiques)
                    .HasForeignKey(d => d.PersonnePhysiqueEscaladeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DossierFraude>(entity =>
            {
                entity.HasIndex(e => e.AutorisationPpfId);

                entity.HasIndex(e => e.CanalBdfId);

                entity.HasIndex(e => e.CategorieId);

                entity.HasIndex(e => e.ComplementMotifRejetChequeId)
                    .HasDatabaseName("IX_DossierFraudeHisto_ComplementMotifRejetChequeId");

                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.EntiteId);

                entity.HasIndex(e => e.EtablissementDeclarantId);

                entity.HasIndex(e => e.EtatFraudeId);

                entity.HasIndex(e => e.ImpactId);

                entity.HasIndex(e => e.ModeOperatoireId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.MotifRejetChequeId).HasDatabaseName("IX_DossierFraudeHisto_MotifRejetChequeId");

                entity.HasIndex(e => e.OrigineId);

                entity.HasIndex(e => e.PaysId);

                entity.HasIndex(e => e.ProduitId);

                entity.HasIndex(e => e.ScenarioNorkomId);

                entity.HasIndex(e => e.StatutDossierId);

                entity.HasIndex(e => e.TypeCollecteChequeId);

                entity.HasIndex(e => e.TypePaiementId);

                entity.HasIndex(e => e.TypologieBdfId);

                entity.HasIndex(e => e.UtilisateurId);

                entity.HasOne(d => d.Categorie)
                    .WithMany(p => p.DossierFraudes)
                    .HasForeignKey(d => d.CategorieId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.DossierFraudes)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.EntiteFraude)
                    .WithMany(p => p.DossierFraudes)
                    .HasForeignKey(d => d.EntiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.EtatFraude)
                    .WithMany(p => p.DossierFraudes)
                    .HasForeignKey(d => d.EtatFraudeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Impact)
                    .WithMany(p => p.DossierFraudes)
                    .HasForeignKey(d => d.ImpactId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Origine)
                    .WithMany(p => p.DossierFraudes)
                    .HasForeignKey(d => d.OrigineId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Produit)
                    .WithMany(p => p.DossierFraudes)
                    .HasForeignKey(d => d.ProduitId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.StatutDossier)
                    .WithMany(p => p.DossierFraudes)
                    .HasForeignKey(d => d.StatutDossierId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.DossierFraudeUtilisateurs)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                entity.Property(e => e.MotifsSoupcons)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder
                .Entity<DocumentFraude>()
                .Property(e => e.FileContent)
                .Metadata
                .SetValueComparer(
                    new ValueComparer<byte[]>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToArray()));
            modelBuilder
                .Entity<DemandeInformationFraude>(entity =>
                {
                    entity.Property(e => e.Titre)
                        .Metadata
                        .SetValueComparer(
                            new ValueComparer<byte[]>(
                                (c1, c2) => c1.SequenceEqual(c2),
                                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                c => c.ToArray()));
                    entity.Property(e => e.Retour)
                        .Metadata
                        .SetValueComparer(
                            new ValueComparer<byte[]>(
                                (c1, c2) => c1.SequenceEqual(c2),
                                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                c => c.ToArray()));
                    entity.Property(e => e.Demande)
                        .Metadata
                        .SetValueComparer(
                            new ValueComparer<byte[]>(
                                (c1, c2) => c1.SequenceEqual(c2),
                                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                c => c.ToArray()));
                });


            modelBuilder.Entity<DossierFraudeAction>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.DossierFraudeId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.DossierFraudeActionCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                entity.Property(e => e.Description)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Libelle)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<DossierLabAction>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.DossierLabId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Libelle).IsRequired();

                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.DossierLabActionCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DossierLab)
                    .WithMany(p => p.DossierLabActions)
                    .HasForeignKey(d => d.DossierLabId);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.DossierLabActionModificateurs)
                    .HasForeignKey(d => d.ModificateurId);
                entity.Property(e => e.Libelle)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Description)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<DossierFraudeHisto>(entity =>
            {
                entity.HasIndex(e => e.ActionId);

                entity.HasIndex(e => e.AutorisationPpfId);

                entity.HasIndex(e => e.CanalBdfId);

                entity.HasIndex(e => e.CategorieId);

                entity.HasIndex(e => e.DossierFraudeId);

                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.EntiteId);

                entity.HasIndex(e => e.EtablissementDeclarantId);

                entity.HasIndex(e => e.EtatFraudeId);

                entity.HasIndex(e => e.ImpactId);

                entity.HasIndex(e => e.ModeOperatoireId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.OrigineId);

                entity.HasIndex(e => e.PaysId);

                entity.HasIndex(e => e.ProduitId);

                entity.HasIndex(e => e.ScenarioNorkomId);

                entity.HasIndex(e => e.StatutDossierId);

                entity.HasIndex(e => e.TypeCollecteChequeId);

                entity.HasIndex(e => e.TypePaiementId);

                entity.HasIndex(e => e.TypologieBdfId);

                entity.HasIndex(e => e.UtilisateurId);

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.DossierFraudeHistos)
                    .HasForeignKey(d => d.ActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Categorie)
                    .WithMany(p => p.DossierFraudeHistos)
                    .HasForeignKey(d => d.CategorieId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.EntiteFraude)
                    .WithMany(p => p.DossierFraudeHistos)
                    .HasForeignKey(d => d.EntiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.DossierFraudeHistos)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.EtatFraude)
                    .WithMany(p => p.DossierFraudeHistos)
                    .HasForeignKey(d => d.EtatFraudeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Impact)
                    .WithMany(p => p.DossierFraudeHistos)
                    .HasForeignKey(d => d.ImpactId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Origine)
                    .WithMany(p => p.DossierFraudeHistos)
                    .HasForeignKey(d => d.OrigineId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Produit)
                    .WithMany(p => p.DossierFraudeHistos)
                    .HasForeignKey(d => d.ProduitId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.StatutDossier)
                    .WithMany(p => p.DossierFraudeHistos)
                    .HasForeignKey(d => d.StatutDossierId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.DossierFraudeHistoUtilisateurs)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.MotifsSoupcons)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<DossierFraudePersonneMorale>(entity =>
            {
                entity.HasIndex(e => e.DossierFraudeId);

                entity.HasIndex(e => e.MotifExemptionNotificationId);

                entity.HasIndex(e => e.PersonneMoraleFraudeId);

                entity.HasIndex(e => e.StatutPersonneFraudeId);

                entity.HasOne(d => d.StatutPersonneFraude)
                    .WithMany(p => p.DossierFraudePersonneMorales)
                    .HasForeignKey(d => d.StatutPersonneFraudeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.MotifExemptionNotification)
                    .WithMany(p => p.DossierFraudePersonneMorales)
                    .HasForeignKey(d => d.MotifExemptionNotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DossierFraudePersonnePhysique>(entity =>
            {
                entity.HasIndex(e => e.DossierFraudeId);

                entity.HasIndex(e => e.MotifExemptionNotificationId);

                entity.HasIndex(e => e.PersonnePhysiqueFraudeId);

                entity.HasIndex(e => e.StatutPersonneFraudeId);

                entity.HasOne(d => d.StatutPersonneFraude)
                    .WithMany(p => p.DossierFraudePersonnePhysiques)
                    .HasForeignKey(d => d.StatutPersonneFraudeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.MotifExemptionNotification)
                    .WithMany(p => p.DossierFraudePersonnePhysiques)
                    .HasForeignKey(d => d.MotifExemptionNotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DossierFraudeResult>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("DossierFraudeResult", "Fraude");
            });

            modelBuilder.Entity<DossierLab>(entity =>
            {
                entity.HasIndex(e => e.AvisId);

                entity.HasIndex(e => e.CategorieId);

                entity.HasIndex(e => e.CategorieGroupeLabId);

                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.EntiteId);

                entity.HasIndex(e => e.PaysId);

                entity.HasIndex(e => e.SecteurEconomiqueId);

                entity.HasIndex(e => e.StatutDossierId);

                entity.HasIndex(e => e.UtilisateurId);

                entity.HasIndex(e => e.VisaId);

                entity.Property(e => e.CodeUnique).HasMaxLength(20);

                entity.HasOne(d => d.AvisLab)
                    .WithMany(p => p.DossierLabs)
                    .HasForeignKey(d => d.AvisId);

                entity.HasOne(d => d.Categorie)
                    .WithMany(p => p.DossierLabs)
                    .HasForeignKey(d => d.CategorieId);

                entity.HasOne(d => d.OrigineGroupeLab)
                    .WithMany(p => p.DossierLabs)
                    .HasForeignKey(d => d.OrigineGroupeLabId);

                entity.HasOne(d => d.CategorieGroupeLab)
                    .WithMany(p => p.DossierLabs)
                    .HasForeignKey(d => d.CategorieGroupeLabId);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.DossierLabs)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.EntiteLab)
                    .WithMany(p => p.DossierLabs)
                    .HasForeignKey(d => d.EntiteId);


                entity.HasOne(d => d.Pays)
                    .WithMany(p => p.DossierLabs)
                    .HasForeignKey(d => d.PaysId);


                entity.HasOne(d => d.SecteurEconomiqueLab)
                    .WithMany(p => p.DossierLabs)
                    .HasForeignKey(d => d.SecteurEconomiqueId);

                entity.HasOne(d => d.StatutDossier)
                    .WithMany(p => p.DossierLabs)
                    .HasForeignKey(d => d.StatutDossierId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Visa)
                    .WithMany(p => p.DossierLabs)
                    .HasForeignKey(d => d.VisaId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.DossierLabs)
                    .HasForeignKey(d => d.UtilisateurId);

                entity.Property(e => e.MotifsSoupcons)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder
                .Entity<DocumentLab>(entity =>
                {
                    entity.Property(e => e.FileContent)
                        .Metadata
                        .SetValueComparer(
                            new ValueComparer<byte[]>(
                                (c1, c2) => c1.SequenceEqual(c2),
                                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                c => c.ToArray()));
                });


            modelBuilder
                .Entity<DemandeInformationLab>(entity =>
                {
                    entity.Property(e => e.Titre)
                        .Metadata
                        .SetValueComparer(
                            new ValueComparer<byte[]>(
                                (c1, c2) => c1.SequenceEqual(c2),
                                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                c => c.ToArray()));
                    entity.Property(e => e.Retour)
                        .Metadata
                        .SetValueComparer(
                            new ValueComparer<byte[]>(
                                (c1, c2) => c1.SequenceEqual(c2),
                                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                c => c.ToArray()));
                    entity.Property(e => e.Demande)
                        .Metadata
                        .SetValueComparer(
                            new ValueComparer<byte[]>(
                                (c1, c2) => c1.SequenceEqual(c2),
                                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                c => c.ToArray()));
                    entity.Property(e => e.Conversation)
                        .Metadata
                        .SetValueComparer(
                            new ValueComparer<byte[]>(
                                (c1, c2) => c1.SequenceEqual(c2),
                                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                c => c.ToArray()));
                });


            modelBuilder.Entity<DossierLabHisto>(entity =>
            {
                entity.HasIndex(e => e.ActionId);

                entity.HasIndex(e => e.AvisId);

                entity.HasIndex(e => e.CategorieId);

                entity.HasIndex(e => e.DossierLabId);

                entity.HasIndex(e => e.EntiteId);

                entity.HasIndex(e => e.OrigineLabId);

                entity.HasIndex(e => e.PaysId);

                entity.HasIndex(e => e.SecteurEconomiqueId);

                entity.HasIndex(e => e.StatutDossierId);

                entity.HasIndex(e => e.UtilisateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.VisaId);

                entity.HasIndex(e => e.CategorieGroupeLabId);

                entity.HasIndex(e => e.OrigineGroupeLabId);

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.DossierLabHistos)
                    .HasForeignKey(d => d.ActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.AvisLab)
                    .WithMany(p => p.DossierLabHistos)
                    .HasForeignKey(d => d.AvisId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Categorie)
                    .WithMany(p => p.DossierLabHistos)
                    .HasForeignKey(d => d.CategorieId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DossierLab)
                    .WithMany(p => p.DossierLabHistos)
                    .HasForeignKey(d => d.DossierLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.EntiteLab)
                    .WithMany(p => p.DossierLabHistos)
                    .HasForeignKey(d => d.EntiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.OrigineLab)
                    .WithMany(p => p.DossierLabHistos)
                    .HasForeignKey(d => d.OrigineLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.OrigineGroupeLab)
                    .WithMany(p => p.DossierLabHistos)
                    .HasForeignKey(d => d.OrigineGroupeLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CategorieGroupeLab)
                    .WithMany(p => p.DossierLabHistos)
                    .HasForeignKey(d => d.CategorieGroupeLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Pays)
                    .WithMany(p => p.DossierLabHistos)
                    .HasForeignKey(d => d.PaysId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SecteurEconomiqueLab)
                    .WithMany(p => p.DossierLabHistos)
                    .HasForeignKey(d => d.SecteurEconomiqueId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.StatutDossier)
                    .WithMany(p => p.DossierLabHistos)
                    .HasForeignKey(d => d.StatutDossierId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Visa)
                    .WithMany(p => p.DossierLabHistos)
                    .HasForeignKey(d => d.VisaId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.DossierLabHistoUtilisateurs)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.DossierLabHistoModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.MotifsSoupcons)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<DossierLabPersonneMorale>(entity =>
            {
                entity.HasIndex(e => e.DossierLabId);

                entity.HasIndex(e => e.PersonneMoraleLabId);

                entity.HasOne(d => d.DossierLab)
                    .WithMany(p => p.DossierLabPersonneMorales)
                    .HasForeignKey(d => d.DossierLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PersonneMoraleLab)
                    .WithMany(p => p.DossierLabPersonneMorales)
                    .HasForeignKey(d => d.PersonneMoraleLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<DossierLabPersonnePhysique>(entity =>
            {
                entity.HasIndex(e => e.DossierLabId);

                entity.HasIndex(e => e.PersonnePhysiqueLabId);

                entity.HasOne(d => d.DossierLab)
                    .WithMany(p => p.DossierLabPersonnePhysiques)
                    .HasForeignKey(d => d.DossierLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PersonnePhysiqueLab)
                    .WithMany(p => p.DossierLabPersonnePhysiques)
                    .HasForeignKey(d => d.PersonnePhysiqueLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DossierLabNonConnu>(entity =>
            {
                entity.HasIndex(e => e.DossierLabId);
                entity.HasIndex(e => e.NonConnuLabId);

                entity.HasOne(d => d.DossierLab)
                    .WithMany(p => p.DossierLabNonConnus)
                    .HasForeignKey(d => d.DossierLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.NonConnuLab)
                    .WithMany(p => p.DossierLabNonConnus)
                    .HasForeignKey(d => d.NonConnuLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<DossierValidationFinale>(entity =>
            {
                entity.HasIndex(e => e.ValidateurId);

                entity.HasOne(d => d.Validateur)
                    .WithMany(p => p.DossierValidationFinales)
                    .HasForeignKey(d => d.ValidateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DossierValidationFinale_Validateur_ValidateurId");

                entity.Property(e => e.Validation)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });


            modelBuilder.Entity<DossierValidationFinaleFlge>(entity =>
            {
                entity.HasIndex(e => e.ValidateurId);

                entity.HasOne(d => d.Validateur)
                    .WithMany(p => p.DossierValidationFinaleFlges)
                    .HasForeignKey(d => d.ValidateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DossierValidationFinaleFlge_Validateur_ValidateurId");

                entity.Property(e => e.Validation)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });


            modelBuilder.Entity<DossierValidationIntermediaire>(entity =>
            {
                entity.HasIndex(e => e.ValidateurId);

                entity.HasOne(d => d.Validateur)
                    .WithMany(p => p.DossierValidationIntermediaires)
                    .HasForeignKey(d => d.ValidateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DossierValidationIntermediaire_Validateur_ValidateurId");

                entity.Property(e => e.Validation)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<DossierValidationIntermediaireFlge>(entity =>
            {
                entity.HasIndex(e => e.ValidateurId);

                entity.HasOne(d => d.Validateur)
                    .WithMany(p => p.DossierValidationIntermediaireFlges)
                    .HasForeignKey(d => d.ValidateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DossierValidationIntermediaireFlge_Validateur_ValidateurId");

                entity.Property(e => e.Validation)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });


            modelBuilder.Entity<DroitCompte>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<EmailNotificationTemplate>(entity =>
            {
                entity.HasIndex(e => e.ActiviteId);

                entity.HasIndex(e => e.CreateurId)
                    .HasDatabaseName("IX_EmailNotificationTemplate_Utilisateur_CreateurId");

                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.EmailNotificationTypeId);

                entity.HasIndex(e => e.LangueId);

                entity.HasIndex(e => e.ModificateurId)
                    .HasDatabaseName("IX_EmailNotificationTemplate_Utilisateur_ModificateurId");

                entity.Property(e => e.IsActive);

                entity.HasOne(d => d.Activite)
                    .WithMany(p => p.EmailNotificationTemplates)
                    .HasForeignKey(d => d.ActiviteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.EmailNotificationType)
                    .WithMany(p => p.EmailNotificationTemplates)
                    .HasForeignKey(d => d.EmailNotificationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<EmailGenerique>(entity =>
            {
                entity.HasIndex(e => e.ActiviteId);

                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.CreateurId).HasDatabaseName("IX_EmailGenerique_Utilisateur_CreateurId");


                entity.HasIndex(e => e.ModificateurId).HasDatabaseName("IX_EmailGenerique_Utilisateur_ModificateurId");

                entity.HasOne(d => d.Activite)
                    .WithMany(p => p.EmailGeneriques)
                    .HasForeignKey(d => d.ActiviteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Confidentiel>(entity =>
            {
                entity.HasIndex(e => e.DirectionId);
                entity.HasIndex(e => e.CreateurId).HasDatabaseName("IX_Confidentiel_Utilisateur_CreateurId");
                entity.HasIndex(e => e.ModificateurId).HasDatabaseName("IX_Confidentiel_Utilisateur_ModificateurId");
            });

            modelBuilder.Entity<EmailNotificationType>(entity =>
            {
                entity.HasIndex(e => e.CreateurId).HasDatabaseName("IX_EmailNotificationType_Utilisateur_CreateurId");

                entity.HasIndex(e => e.ModificateurId)
                    .HasDatabaseName("IX_EmailNotificationType_Utilisateur_ModificateurId");

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<EntiteFraude>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.ParentId);

                entity.HasIndex(e => e.PaysId);

                entity.HasIndex(e => e.SecteurId);

                entity.Property(e => e.IsActive);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.Entites)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<EntiteLab>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.PaysId);

                entity.HasIndex(e => e.SecteurId);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IsActive);

                entity.Property(e => e.Liadr4).HasMaxLength(100);

                entity.Property(e => e.Lisp)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SegmentClient).HasMaxLength(50);

                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.EntiteLabCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.EntiteLabs)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.EntiteLabModificateurs)
                    .HasForeignKey(d => d.ModificateurId);

                entity.HasOne(d => d.Pays)
                    .WithMany(p => p.EntiteLabs)
                    .HasForeignKey(d => d.PaysId);

                entity.HasOne(d => d.SecteurLab)
                    .WithMany(p => p.EntiteLabs)
                    .HasForeignKey(d => d.SecteurId);
            });


            modelBuilder.Entity<Environnement>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);
            });

            modelBuilder.Entity<EquipeAnalyse>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.UniteEquipeId);
            });

            modelBuilder.Entity<EquipeValidation>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);
            });

            modelBuilder.Entity<EquipeValidationAvisSi>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);
            });

            modelBuilder.Entity<EtablissementDeclarant>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<EtatFraude>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<Evenement>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.DomaineId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasOne(d => d.Domaine)
                    .WithMany(p => p.Evenements)
                    .HasForeignKey(d => d.DomaineId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Fonction>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<FormeJuridique>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<HistoriqueConnexion>(entity =>
            {
                entity.HasIndex(e => e.UtilisateurId);

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.HistoriqueConnexions)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Impact>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<IndiceChart>(entity =>
            {
                entity.HasIndex(e => e.ActiviteId);

                entity.HasIndex(e => e.UtilisateurId);

                entity.HasOne(d => d.Activite)
                    .WithMany(p => p.IndiceCharts)
                    .HasForeignKey(d => d.ActiviteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.IndiceCharts)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<IndicePie>(entity =>
            {
                entity.HasIndex(e => e.ActiviteId);

                entity.HasIndex(e => e.DirectionId);

                entity.HasOne(d => d.Activite)
                    .WithMany(p => p.IndicePies)
                    .HasForeignKey(d => d.ActiviteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.IndicePies)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<LienPersonneMorale>(entity =>
            {
                entity.HasIndex(e => e.PersonneMoraleLabId);

                entity.HasIndex(e => e.PersonnePhysiqueLabId);

                entity.HasIndex(e => e.FormeJuridiqueId);

                entity.HasIndex(e => e.TypeLienPersonneMoralePhysiqueId);

                entity.HasIndex(e => e.TypeLienPersonneMoraleMoraleId);
            });

            modelBuilder.Entity<LienPersonnePhysique>(entity =>
            {
                entity.HasIndex(e => e.PersonneMoraleLabId);

                entity.HasIndex(e => e.PersonnePhysiqueLabId);

                entity.HasIndex(e => e.TypeLienPersonnePhysiqueMoraleId);

                entity.HasIndex(e => e.TypeLienPersonnePhysiquePhysiqueId);
            });

            modelBuilder.Entity<ModeOperatoire>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });
            modelBuilder.Entity<CategorieLienLab>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });
            modelBuilder.Entity<MotifExemptionNotification>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<MotifRejetCheque>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<TypeLienSupport>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<OrigineDirectionFraude>(entity =>
            {
                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.OrigineId);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.OrigineDirectionFraudes)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Origine)
                    .WithMany(p => p.OrigineDirectionFraudes)
                    .HasForeignKey(d => d.OrigineId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<OrigineFraude>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<OrigineLab>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.DirectionId);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.OrigineLabs)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.EnglishDescription).HasMaxLength(1000);

                entity.Property(e => e.EnglishName).HasMaxLength(500);

                entity.Property(e => e.FrenchDescription)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.FrenchName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.IsActive)
                    .IsRequired();

                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.OrigineLabCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.OrigineLabModificateurs)
                    .HasForeignKey(d => d.ModificateurId);

                entity.HasOne(d => d.GroupeOrigineLab)
                    .WithMany(p => p.OrigineLabs)
                    .HasForeignKey(d => d.GroupeOrigineLabId);

                entity.HasOne(d => d.FonctionLab)
                    .WithMany(p => p.OrigineLabs)
                    .HasForeignKey(d => d.FonctionLabId);
            });

            modelBuilder.Entity<ParametreDirection>(entity =>
            {
                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.TypeParametreId);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.ParametreDirections)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Pays>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.ZoneGeographiqueId);

                entity.Property(e => e.IsActive);
                entity.Property(e => e.IsDrom);
            });


            modelBuilder.Entity<PersonneMoraleEscalade>(entity =>
            {
                entity.HasIndex(e => e.FormeJuridiqueId);

                entity.HasIndex(e => e.PaysId);

                entity.HasIndex(e => e.SecteurProfessionnelId);

                entity.HasIndex(e => e.TypeClientId);

                entity.Property(e => e.Ville)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Sigle)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.RaisonSociale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.IdentifiantTvaUe)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Dirigeant)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CodePostale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Adresse)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<PersonneMoraleAvis>(entity =>
            {
                entity.HasIndex(e => e.FormeJuridiqueId);

                entity.HasIndex(e => e.PaysId);

                entity.HasIndex(e => e.SecteurProfessionnelId);

                entity.HasIndex(e => e.TypeClientId);

                entity.Property(e => e.Ville)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Sigle)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.RaisonSociale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.IdentifiantTvaUe)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Dirigeant)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CodePostale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Adresse)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<ClientMoraleAvisSi>(entity => { entity.HasIndex(e => e.TypeClientAvisSiId); });

            modelBuilder.Entity<ContrepartieMoraleAvisSi>(entity => { entity.HasIndex(e => e.TypeClientAvisSiId); });


            modelBuilder.Entity<ClientMoraleEscalade>(entity =>
            {
                entity.Property(e => e.Adresse)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CodePostale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Dirigeant)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.IdentifiantTvaUe)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.RaisonSociale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Sigle)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Ville)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<ContrepartieMoraleEscalade>(entity =>
            {
                entity.Property(e => e.Adresse)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CodePostale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Dirigeant)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.IdentifiantTvaUe)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.RaisonSociale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Sigle)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Ville)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });


            modelBuilder.Entity<ClientPhysiqueEscalade>(entity =>
            {
                entity.Property(e => e.Adresse)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CodePostale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));

                entity.Property(e => e.LieuNaissance)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.NomNaissance)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.NomUsuel)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Prenoms)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Ville)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<ContrepartiePhysiqueEscalade>(entity =>
            {
                entity.Property(e => e.Adresse)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CodePostale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.LieuNaissance)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.NomNaissance)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.NomUsuel)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Prenoms)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Ville)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });


            modelBuilder.Entity<PersonneMoraleFraude>(entity =>
            {
                entity.HasIndex(e => e.CanalEntreeEnRelationId);

                entity.HasIndex(e => e.FormeJuridiqueId);

                entity.HasIndex(e => e.PaysId);

                entity.HasIndex(e => e.RoleClientId);

                entity.HasIndex(e => e.SecteurProfessionnelId);

                entity.HasIndex(e => e.TypeClientId);

                entity.Property(e => e.Ville)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.SiteInternet)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Sigle)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.IdentifiantTvaUe)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Dirigeant)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CodePostale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Adresse)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<PersonneMoraleLab>(entity =>
            {
                entity.HasOne(d => d.TypeClient)
                    .WithMany(p => p.PersonneMoraleLabs)
                    .HasForeignKey(d => d.TypeClientId);
                entity.HasOne(d => d.TypeImplication)
                    .WithMany(p => p.PersonneMoraleLabs)
                    .HasForeignKey(d => d.TypeImplicationId);
                entity.HasOne(d => d.ProfessionalIdentification)
                    .WithMany(p => p.PersonneMoraleLabs)
                    .HasForeignKey(d => d.ProfessionalIdentificationId);

                entity.Property(e => e.IdPersonne)
                    .HasDefaultValueSql("NEWID()");

                entity.Property(e => e.Sigle)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<NonConnuLab>(entity =>
            {
                entity.HasOne(d => d.TypeImplication)
                    .WithMany(p => p.NonConnuLabs)
                    .HasForeignKey(d => d.TypeImplicationId);

                entity
                    .Property(e => e.IdPersonne)
                    .HasDefaultValueSql("NEWID()");
            });


            modelBuilder.Entity<PersonneMoraleLabLienEntite>(entity =>
            {
                entity.HasIndex(e => e.DossierLabId);


                entity.HasIndex(e => e.EntiteId);

                entity.HasIndex(e => e.PersonneMoraleLabId);

                entity.HasOne(d => d.DossierLab)
                    .WithMany(p => p.PersonneMoraleLabLienEntites)
                    .HasForeignKey(d => d.DossierLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.EntiteLab)
                    .WithMany(p => p.PersonneMoraleLabLienEntites)
                    .HasForeignKey(d => d.EntiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PersonneMoraleLab)
                    .WithMany(p => p.PersonneMoraleLabLienEntites)
                    .HasForeignKey(d => d.PersonneMoraleLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
        });


            modelBuilder.Entity<PersonneMoraleResult>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("PersonneMoraleResult", "Fraude");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<PersonneMoraleResultLab>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("PersonneMoraleResultLab", "Lab");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<PersonnePhysiqueEscalade>(entity =>
            {
                entity.HasIndex(e => e.CiviliteId);

                entity.HasIndex(e => e.NationaliteId);

                entity.HasIndex(e => e.PaysId);

                entity.HasIndex(e => e.PaysNaissanceId);

                entity.HasOne(d => d.Civilite)
                    .WithMany(p => p.PersonnePhysiqueEscalades)
                    .HasForeignKey(d => d.CiviliteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.Ville)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Prenoms)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.NomUsuel)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.NomNaissance)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.LieuNaissance)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CodePostale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Adresse)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<PersonnePhysiqueFraude>(entity =>
            {
                entity.HasIndex(e => e.AutreNationaliteId);

                entity.HasIndex(e => e.CanalEntreeEnRelationId);

                entity.HasIndex(e => e.CiviliteId);

                entity.HasIndex(e => e.NationaliteId);

                entity.HasIndex(e => e.PaysNaissanceId);

                entity.HasIndex(e => e.RelationClientId);

                entity.HasIndex(e => e.RoleClientId);

                entity.HasIndex(e => e.SecteurProfessionnelId);

                entity.HasIndex(e => e.TypeClientId);

                entity.HasOne(d => d.Civilite)
                    .WithMany(p => p.PersonnePhysiqueFraudes)
                    .HasForeignKey(d => d.CiviliteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.Ville)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.LieuNaissance)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CodePostale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Adresse)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<PersonnePhysiqueLab>(entity =>
            {
                entity.HasOne(d => d.Civilite)
                    .WithMany(p => p.PersonnePhysiqueLabs)
                    .HasForeignKey(d => d.CiviliteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Sexe)
                    .WithMany(p => p.PersonnePhysiqueLabs)
                    .HasForeignKey(d => d.SexeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TypeClient)
                    .WithMany(p => p.PersonnePhysiqueLabs)
                    .HasForeignKey(d => d.TypeClientId);

                entity.Property(e => e.IdPersonne)
                    .HasDefaultValueSql("NEWID()");

                entity.Property(e => e.SurfaceFinanciere)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.LieuNaissance)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.ElementClesRelation)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });
            modelBuilder.Entity<PersonnePhysiqueLabLienEntite>(entity =>
            {
                entity.HasIndex(e => e.DossierLabId);

                entity.HasIndex(e => e.EntiteId);

                entity.HasIndex(e => e.PersonnePhysiqueLabId);

                entity.HasOne(d => d.DossierLab)
                    .WithMany(p => p.PersonnePhysiqueLabLienEntites)
                    .HasForeignKey(d => d.DossierLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.EntiteLab)
                    .WithMany(p => p.PersonnePhysiqueLabLienEntites)
                    .HasForeignKey(d => d.EntiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PersonnePhysiqueLab)
                    .WithMany(p => p.PersonnePhysiqueLabLienEntites)
                    .HasForeignKey(d => d.PersonnePhysiqueLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<DossierLabQLBResult>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("DossierLabQLBResult", "Lab");
            });
            modelBuilder.Entity<PersonnePhysiqueResult>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("PersonnePhysiqueResult", "Fraude");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });


            modelBuilder.Entity<PieceIdentite>(entity =>
            {
                entity.HasIndex(e => e.PaysDelivranceId);

                entity.HasIndex(e => e.PersonnePhysiqueLabId);

                entity.HasIndex(e => e.TypePieceIdentiteId);
            });

            modelBuilder.Entity<Dirigeant>(entity => { entity.HasIndex(e => e.PersonneMoraleLabId); });

            modelBuilder.Entity<SupportFinancierPersonnePhysique>(entity =>
            {
                entity.HasIndex(e => e.PersonnePhysiqueLabId);

                entity.HasIndex(e => e.TypeCompteId);

                entity.HasIndex(e => e.TypeLienSupportId);

                entity.HasIndex(e => e.TypeReferenceLabId);

                entity.Property(e => e.Solde)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<SupportFinancierPersonneMorale>(entity =>
            {
                entity.HasIndex(e => e.PersonneMoraleLabId);

                entity.HasIndex(e => e.TypeCompteId);

                entity.HasIndex(e => e.TypeLienSupportId);

                entity.HasIndex(e => e.TypeReferenceLabId);

                entity.Property(e => e.Solde)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<SupportFinancierNonConnu>(entity =>
            {
                entity.HasIndex(e => e.NonConnuLabId);

                entity.HasIndex(e => e.TypeCompteId);

                entity.HasIndex(e => e.TypeLienSupportId);

                entity.HasIndex(e => e.TypeReferenceLabId);

                entity.Property(e => e.Solde)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<TargetEntity>(entity =>
            {
                entity.Property(e => e.YearlyConsolidatedAssets)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.YearlyConsolidatedTurnover)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<TargetEntitySubsidiary>(entity =>
            {
                entity.Property(e => e.OwnershipPercentage)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.SalesRevenue)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Asset)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<TargetEntityOwnership>(entity =>
            {
                entity.Property(e => e.PercentageOfCapital)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.PercentageOfVotingRights)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<OperationsCompaniesImmobiliers>(entity =>
            {
                entity.Property(e => e.MontantTotalImmobilier)
                    .HasColumnType("decimal(18,2)");
            });
            modelBuilder.Entity<OperationsCompaniesImmobiliers>(entity =>
            {
                entity.Property(e => e.EstimationMontantTotalImmobilier)
                    .HasColumnType("decimal(18,2)");
            });
            modelBuilder.Entity<OperationsCompaniesAssurances>(entity =>
            {
                entity.Property(e => e.MontantTotalFluxSuspectsDeclares)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder
                .Entity<DeclarationTracfinFile>()
                .Property(e => e.FileContent)
                .Metadata
                .SetValueComparer(
                    new ValueComparer<byte[]>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToArray()));

            modelBuilder.Entity<PrincipalInstrumentFinancier>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<Produit>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<VoixRecouvrement>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<Profession>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<DossierLabOperation>(entity =>
            {
                entity.HasIndex(e => e.DossierLabId);

                entity.HasIndex(e => e.DeviseId);

                entity.HasOne(d => d.Devise)
                    .WithMany(p => p.DossierLabOperations)
                    .HasForeignKey(d => d.DeviseId);

                entity.HasOne(d => d.DossierLab)
                    .WithMany(p => p.DossierLabOperations)
                    .HasForeignKey(d => d.DossierLabId);

                entity.HasOne(d => d.TypeLegislationLab)
                    .WithMany(p => p.DossierLabOperations)
                    .HasForeignKey(d => d.TypeLegislationLabId);
            });

            modelBuilder.Entity<RegleDirection>(entity =>
            {
                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.RegleId);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.RegleDirections)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Regle)
                    .WithMany(p => p.RegleDirections)
                    .HasForeignKey(d => d.RegleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<RepresentantLegal>(entity =>
            {
                entity.HasIndex(e => e.PaysNaissanceId);

                entity.HasIndex(e => e.PersonneMoraleLabId);

                entity.Property(e => e.VilleNaissance)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Prenoms)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.NomNaissance)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<RoleClient>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<Routage>(entity =>
            {
                entity.HasIndex(e => e.ApplicationId);

                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.EquipeAnalyseId);

                entity.HasIndex(e => e.EquipeValidationFinaleId);

                entity.HasIndex(e => e.EquipeValidationInterId);

                entity.HasIndex(e => e.EvenementId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasOne(d => d.EquipeValidationFinale)
                    .WithMany(p => p.RoutageEquipeValidationFinales)
                    .HasForeignKey(d => d.EquipeValidationFinaleId)
                    .HasConstraintName("FK_Routage_EquipeValidation_EquipeValidationIFinaled");

                entity.HasOne(d => d.EquipeValidationInter)
                    .WithMany(p => p.RoutageEquipeValidationInters)
                    .HasForeignKey(d => d.EquipeValidationInterId)
                    .HasConstraintName("FK_Routage_EquipeValidation_EquipeValidationIInterd");

                entity.HasOne(d => d.Evenement)
                    .WithMany(p => p.Routages)
                    .HasForeignKey(d => d.EvenementId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ScenarioNorkom>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<ScenarioLab>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.ApplicationScenarioLabId);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EnglishDescription).HasMaxLength(1000);

                entity.Property(e => e.EnglishName).HasMaxLength(500);

                entity.Property(e => e.FrenchDescription)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.FrenchName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.IsActive)
                    .IsRequired();

                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.ScenarioLabCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.ScenarioLabModificateurs)
                    .HasForeignKey(d => d.ModificateurId);

                entity.HasOne(d => d.ApplicationScenarioLab)
                    .WithMany(p => p.ScenarioLabs)
                    .HasForeignKey(d => d.ApplicationScenarioLabId);
            });

            modelBuilder.Entity<SecteurFraude>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.DirectionId).HasDatabaseName("IX_Secteur_Direction_DirectionId");

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.Secteurs)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<SecteurLab>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IndicatifDga).HasColumnName("IndicatifDGA");

                entity.Property(e => e.IsActive);

                entity.Property(e => e.Libelle).HasMaxLength(256);

                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.SecteurLabCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.SecteurLabs)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.SecteurLabModificateurs)
                    .HasForeignKey(d => d.ModificateurId);
            });

            modelBuilder.Entity<SecteurEconomique>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<SecteurEconomiqueLab>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);
                entity.HasIndex(e => e.DirectionId);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EnglishDescription).HasMaxLength(1000);

                entity.Property(e => e.EnglishName).HasMaxLength(500);

                entity.Property(e => e.FrenchDescription)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.FrenchName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.IsActive)
                    .IsRequired();

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.SecteurEconomiqueLabs)
                    .HasForeignKey(d => d.DirectionId);

                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.SecteurEconomiqueLabCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.SecteurEconomiqueLabModificateurs)
                    .HasForeignKey(d => d.ModificateurId);
            });

            modelBuilder.Entity<SecteurProfessionnel>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<Sexe>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<SituationFamiliale>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });


            modelBuilder.Entity<StatutDemandeInformation>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);
            });

            modelBuilder.Entity<StatutPlanActionAnalyse>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);
            });

            modelBuilder.Entity<StatutDossierFraude>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<StatutDossierLab>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EnglishDescription).HasMaxLength(1000);

                entity.Property(e => e.EnglishName).HasMaxLength(500);

                entity.Property(e => e.FrenchDescription)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.FrenchName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.IsActive)
                    .IsRequired();

                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.StatutDossierLabCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.StatutDossierLabModificateurs)
                    .HasForeignKey(d => d.ModificateurId);
            });

            modelBuilder.Entity<StatutEscalade>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);
            });

            modelBuilder.Entity<StatutAvis>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);
            });
            modelBuilder.Entity<StatutAvisSi>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);
            });
            modelBuilder.Entity<StatutOperation>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<StatutPersonneFraude>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<TypeAdresse>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<TypeClientEscalade>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);

                entity.Property(e => e.IsCorporate);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.TypeClientEscalades)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TypeClientFraude>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.TypeClientFraudes)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TypeClientLab>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.TypeClientLabs)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TypeCollecteCheque>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<TypeCompte>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<TypeDeclarationTracfin>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<TypeDocument>(entity =>
            {
                entity.HasIndex(e => e.CreateurId).HasDatabaseName("IX_ypeDocument_CreateurId");

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<TypeLegislationLab>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EnglishDescription).HasMaxLength(1000);

                entity.Property(e => e.EnglishName).HasMaxLength(500);

                entity.Property(e => e.FrenchDescription)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.FrenchName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.IsActive)
                    .IsRequired();

                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.TypeLegislationLabCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.TypeLegislationLabModificateurs)
                    .HasForeignKey(d => d.ModificateurId);
            });


            modelBuilder.Entity<TypeLienPersonneMoraleMorale>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.CategorieLienLabId);

                entity.Property(e => e.IsActive);

                entity.HasOne(e => e.CategorieLienLab)
                    .WithMany(e => e.TypeLienPersonneMoraleMorales)
                    .HasForeignKey(e => e.CategorieLienLabId);
            });

            modelBuilder.Entity<TypeLienPersonneMoralePhysique>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.CategorieLienLabId);

                entity.Property(e => e.IsActive);
            });


            modelBuilder.Entity<TypeLienPersonnePhysiquePhysique>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.CategorieLienLabId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<TypeLienPersonnePhysiqueMorale>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.CategorieLienLabId);
            });
            modelBuilder.Entity<TypePaiement>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<TypeParametre>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<TypePieceIdentite>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<TypePieceJointe>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });


            modelBuilder.Entity<TypeException>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);
            });

            modelBuilder.Entity<TypeVoie>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<TypologieBdf>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<UniteEquipe>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);
            });


            modelBuilder.Entity<ConfigurationValue>(entity =>
            {
                entity.HasIndex(e => e.Id).IsUnique().HasFilter("([Id] IS NOT NULL)");

                entity.HasIndex(e => e.Value);
            });

            modelBuilder.Entity<Utilisateur>(entity =>
            {
                entity.HasIndex(e => e.AspNetUsersId).IsUnique().HasFilter("([AspNetUsersId] IS NOT NULL)");

                entity.HasIndex(e => e.DirectionAttacheId);

                entity.HasIndex(e => e.FonctionId);

                entity.HasIndex(e => e.LangueId);

                entity.HasOne(d => d.DirectionAttache)
                    .WithMany(p => p.Utilisateurs)
                    .HasForeignKey(d => d.DirectionAttacheId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Langue)
                    .WithMany(p => p.Utilisateurs)
                    .HasForeignKey(d => d.LangueId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<UtilisateurDirection>(entity =>
            {
                entity.HasIndex(e => e.ActiviteId);

                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.UtilisateurId);

                entity.Property(e => e.Confidentiality).HasDefaultValue(false);

                entity.Property(e => e.Isolated).HasDefaultValue(false);

                entity.Property(e => e.Read).HasDefaultValue(false);

                entity.Property(e => e.Write).HasDefaultValue(false);

                entity.HasOne(d => d.Activite)
                    .WithMany(p => p.UtilisateurDirections)
                    .HasForeignKey(d => d.ActiviteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.UtilisateurDirections)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.UtilisateurDirections)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<UtilisateurDocument>(entity =>
            {
                entity.HasIndex(e => e.UtilisateurId);

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.UtilisateurDocuments)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.Document)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder
                .Entity<DocumentTool>()
                .Property(e => e.FileContent)
                .Metadata
                .SetValueComparer(
                    new ValueComparer<byte[]>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToArray()));

            modelBuilder.Entity<UtilisateurEquipeAnalyse>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.EquipeAnalyseId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.UtilisateurId);

                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.UtilisateurEquipeAnalyseCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .HasConstraintName("FK_UtilisateurEquipeAnalyse_Createur_CreateurId");

                entity.HasOne(d => d.EquipeAnalyse)
                    .WithMany(p => p.UtilisateurEquipeAnalyses)
                    .HasForeignKey(d => d.EquipeAnalyseId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.UtilisateurEquipeAnalyseModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .HasConstraintName("FK_UtilisateurEquipeAnalyse_Modificateur_ModificateurId");

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.UtilisateurEquipeAnalyseUtilisateurs)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<UtilisateurEquipeValidation>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.EquipeValidationId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.UtilisateurId);

                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.UtilisateurEquipeValidationCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .HasConstraintName("FK_UtilisateurEquipeValidation_Createur_CreateurId");

                entity.HasOne(d => d.EquipeValidation)
                    .WithMany(p => p.UtilisateurEquipeValidations)
                    .HasForeignKey(d => d.EquipeValidationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.UtilisateurEquipeValidationModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .HasConstraintName("FK_UtilisateurEquipeValidation_Modificateur_ModificateurId");

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.UtilisateurEquipeValidationUtilisateurs)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasIndex(e => e.UtilisateurDirectionId);

                entity.HasOne(d => d.UtilisateurDirection)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UtilisateurDirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<VisaLab>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.EnglishDescription).HasMaxLength(1000);

                entity.Property(e => e.EnglishName).HasMaxLength(500);

                entity.Property(e => e.FrenchDescription)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.FrenchName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.IsActive)
                    .IsRequired();

                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.VisaLabCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.VisaLabModificateurs)
                    .HasForeignKey(d => d.ModificateurId);
            });

            modelBuilder.Entity<ZoneGeographique>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });


            modelBuilder.Entity<PlanActionAnalyse>(entity =>
            {
                entity.HasIndex(e => e.AnalyseDossierEscaladeId)
                    .HasDatabaseName("IX_PlanActionAnalyse_AnalyseDossierEscaladeId");

                entity.HasIndex(e => e.PorteurId).HasDatabaseName("IX_PlanActionAnalyse_PorteurId");

                entity.HasIndex(e => e.DirectionPorteurId).HasDatabaseName("IX_PlanActionAnalyse_DirectionPorteurId");


                entity.HasOne(d => d.AnalyseDossierEscalade)
                    .WithMany(p => p.PlanActionAnalyses)
                    .HasForeignKey(d => d.AnalyseDossierEscaladeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);


                entity.HasOne(d => d.Porteur)
                    .WithMany(p => p.PlanActionAnalysePorteurs)
                    .HasForeignKey(d => d.PorteurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DirectionPorteur)
                    .WithMany(p => p.PlanActionAnalyses)
                    .HasForeignKey(d => d.DirectionPorteurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<EntiteConcerne>(entity =>
            {
                entity.HasIndex(e => e.CollaboratorSousEntiteId)
                    .HasDatabaseName("IX_EntiteConcerne_Utilisateur_CollaboratorSousEntiteId");

                entity.HasIndex(e => e.CreateurId)
                    .HasDatabaseName("IX_EntiteConcerne_Utilisateur_CreateurId");

                entity.HasIndex(e => e.DeclarantEntiteManagerId)
                    .HasDatabaseName("IX_EntiteConcerne_DeclarantEntiteManager_DeclarantEntiteManagerId");

                entity.HasIndex(e => e.DossierEscaladeId)
                    .HasDatabaseName("IX_EntiteConcerne_DossierEscalade_DossierEscaladeId");

                entity.HasIndex(e => e.ModificateurId)
                    .HasDatabaseName("IX_EntiteConcerne_Utilisateur_ModificateurId");

                entity.HasIndex(e => e.ResponsableSousEntiteId)
                    .HasDatabaseName("IX_EntiteConcerne_Utilisateur_ResponsableSousEntiteId");

                entity.HasIndex(e => e.SousEntiteId)
                    .HasDatabaseName("IX_EntiteConcerne_Direction_SousEntiteId");

                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.EntiteConcerneCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SousEntite)
                    .WithMany(p => p.EntiteConcernes)
                    .HasForeignKey(d => d.SousEntiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.SyntheseAvis)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Remontee)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Commentaire)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<ActiviteProfessionnellePersonnePhysiqueLab>(entity =>
            {
                entity.HasOne(d => d.PersonnePhysiqueLab)
                    .WithMany(p => p.ActiviteProfessionnellePersonnePhysiqueLabs)
                    .HasForeignKey(d => d.PersonnePhysiqueLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AutreNationalitePersonnePhysiqueLab>(entity =>
            {
                entity.HasOne(d => d.Pays)
                    .WithMany(p => p.AutreNationalitePersonnePhysiqueLabs)
                    .HasForeignKey(d => d.PaysId);

                entity.HasOne(d => d.PersonnePhysiqueLab)
                    .WithMany(p => p.AutreNationalitePersonnePhysiqueLabs)
                    .HasForeignKey(d => d.PersonnePhysiqueLabId);
            });

            modelBuilder.Entity<DomaineCategorieOutil>(entity =>
            {
                entity.HasOne(d => d.Domaine)
                    .WithMany(p => p.DomaineCategorieOutils)
                    .HasForeignKey(d => d.DomaineId);

                entity.HasOne(d => d.CategorieOutil)
                    .WithMany(p => p.DomaineCategorieOutils)
                    .HasForeignKey(d => d.CategorieOutilId);
            });

            modelBuilder.Entity<CoordonneePersonnePhysiqueLab>(entity =>
            {
                entity.HasOne(d => d.Coordonnee)
                    .WithMany(p => p.CoordonneePersonnePhysiqueLabs)
                    .HasForeignKey(d => d.CoordonneeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PersonnePhysiqueLab)
                    .WithMany(p => p.CoordonneePersonnePhysiqueLabs)
                    .HasForeignKey(d => d.PersonnePhysiqueLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DerniereReferenceTracfin>(entity =>
            {
                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.DerniereReferenceTracfins)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DossierLabScenario>(entity =>
            {
                entity.HasOne(d => d.DossierLab)
                    .WithMany(p => p.DossierLabScenarios)
                    .HasForeignKey(d => d.DossierLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ScenarioLab)
                    .WithMany(p => p.DossierLabScenarios)
                    .HasForeignKey(d => d.ScenarioLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ApplicationScenarioLab)
                    .WithMany(p => p.DossierLabScenarios)
                    .HasForeignKey(d => d.ApplicationScenarioLabId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<DossierInstrumentFinancierAmf>(entity =>
            {
                entity.HasOne(d => d.DossierAmf)
                    .WithMany(p => p.DossierInstrumentFinancierAmfs)
                    .HasForeignKey(d => d.DossierAmfId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DossierProduitDeriveAmf>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.DossierProduitDeriveAmfCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DossierAmf)
                    .WithMany(p => p.DossierProduitDeriveAmfs)
                    .HasForeignKey(d => d.DossierAmfId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TypeProduitDeriveAmf)
                    .WithMany(p => p.DossierProduitDeriveAmfs)
                    .HasForeignKey(d => d.TypeProduitDeriveAmfId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DossierProduitDeriveAmf_TypeProduitDerive_TypeProduitDeriveAmfId");
            });


            modelBuilder.Entity<TypeLienPersonnePhysiqueMorale>(entity => { entity.Property(e => e.IsActive); });

            //AvisSI

            modelBuilder.Entity<AnalyseDossierAvisSi>(entity =>
            {
                entity.HasIndex(e => e.AnalysteId);
                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.DossierAvisSiId);

                entity.HasOne(d => d.Analyste)
                    .WithMany(p => p.AnalyseDossierAvisSis)
                    .HasForeignKey(d => d.AnalysteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnalyseDossierAvisSi_Analyste_AnalysteId");

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.AnalyseDossierAvisSiModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnalyseDossierAvisSi_Modificateur_ModificateurId");

                entity.HasOne(d => d.DossierAvisSi)
                    .WithMany(p => p.AnalyseDossierAvisSis)
                    .HasForeignKey(d => d.DossierAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<ApplicationAvisSi>(entity =>
            {
                entity.HasOne(d => d.EnvironnementAvisSi)
                    .WithMany(p => p.ApplicationAvisSis)
                    .HasForeignKey(d => d.EnvironnementAvisSiId)
                    .HasConstraintName("FK_ApplicationAvisSi_Environnement_EnvironnementId");
            });


            modelBuilder.Entity<ConclusionAvisSi>(entity =>
            {
                entity.HasOne(d => d.DecisionAvisSi)
                    .WithMany(p => p.ConclusionAvisSis)
                    .HasForeignKey(d => d.DecisionAvisSiId)
                    .HasConstraintName("FK_ConclusionAvisSi_DecisionAvisSi_DecisionAvisSiId");


                entity.HasIndex(e => e.AnalysteId);
                entity.HasOne(d => d.Analyste)
                    .WithMany(p => p.ConclusionAvisSis)
                    .HasForeignKey(d => d.AnalysteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConclusionAvisSi_Analyste_AnalysteId");
                entity.HasIndex(e => e.ModificateurId);
                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.ConclusionAvisSiModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConclusionAvisSi_Modificateur_ModificateurId");
            });


            modelBuilder.Entity<DefaillanceAvisSi>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.DefaillanceAvisSiCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DemandeDerogation)
                    .WithMany(p => p.DefaillanceAvisSis)
                    .HasForeignKey(d => d.DemandeDerogationId)
                    .HasConstraintName("FK_DefaillanceAvisSi_Evenement_EvenementId");
            });


            modelBuilder.Entity<DemandeInformationAvisSi>(entity =>
            {
                entity.HasOne(d => d.ActionTypeDemandeInformationAvisSi)
                    .WithMany(p => p.DemandeInformationAvisSis)
                    .HasForeignKey(d => d.ActionTypeDemandeInformationAvisSiId)
                    .HasConstraintName(
                        "FK_DemandeInformationAvisSi_ActionTypeDemandeInformationAvisSi_ActionTypeDemandeInformationId");

                entity.HasOne(d => d.AnalyseDossierAvisSi)
                    .WithMany(p => p.DemandeInformationAvisSis)
                    .HasForeignKey(d => d.AnalyseDossierAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TypologieDemande)
                    .WithMany(p => p.DemandeInformationAvisSis)
                    .HasForeignKey(d => d.TypologieDemandeId)
                    .HasConstraintName("FK_DemandeInformationAvisSi_Domaine_DomaineId");

                entity.HasOne(d => d.DemandeDerogation)
                    .WithMany(p => p.DemandeInformationAvisSis)
                    .HasForeignKey(d => d.DemandeDerogationId)
                    .HasConstraintName("FK_DemandeInformationAvisSi_DemandeDerogation_EvenementId");
            });


            modelBuilder.Entity<DocumentDemandeInformationAvisSi>(entity =>
            {
                entity.HasOne(d => d.DocumentAvisSi)
                    .WithMany(p => p.DocumentDemandeInformationAvisSis)
                    .HasForeignKey(d => d.DocumentAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<DocumentDossierAvisSi>(entity =>
            {
                entity.HasIndex(e => e.AppartenanceDocumentAvisSiId);

                entity.HasIndex(e => e.DocumentAvisSiId);

                entity.HasIndex(e => e.DossierAvisSiId);

                entity.HasIndex(e => e.EquipeAnalyseAvisSiId);

                entity.HasIndex(e => e.UtilisateurId);

                entity.HasOne(d => d.DocumentAvisSi)
                    .WithMany(p => p.DocumentDossierAvisSis)
                    .HasForeignKey(d => d.DocumentAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DossierAvisSi)
                    .WithMany(p => p.DocumentDossierAvisSis)
                    .HasForeignKey(d => d.DossierAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<DocumentEntiteConcerneAvisSi>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.DocumentEntiteConcerneAvisSiCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DocumentAvisSi)
                    .WithMany(p => p.DocumentEntiteConcerneAvisSis)
                    .HasForeignKey(d => d.DocumentAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.EntiteConcerneAvisSi)
                    .WithMany(p => p.DocumentEntiteConcerneAvisSis)
                    .HasForeignKey(d => d.EntiteConcerneAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.DocumentEntiteConcerneAvisSiModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .HasConstraintName("FK_DocumentEntiteConcerneAvisSi_Modificateur_ModificateurId");
            });


            modelBuilder.Entity<DocumentPlanActionAnalyseAvisSi>(entity =>
            {
                entity.HasOne(d => d.DocumentAvisSi)
                    .WithMany(p => p.DocumentPlanActionAnalyseAvisSis)
                    .HasForeignKey(d => d.DocumentAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PlanActionAnalyseAvisSi)
                    .WithMany(p => p.DocumentPlanActionAnalyseAvisSis)
                    .HasForeignKey(d => d.PlanActionAnalyseAvisSiId)
                    .HasConstraintName("FK_DocumentPlanActionAnalyseAvisSi_PlanActionAnalyse_PlanActionAnalyseId");
            });


            modelBuilder.Entity<TypologieDemande>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.TypologieDemandeCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .HasConstraintName("FK_Domaine_Utilisateur_CreateurId");

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.TypologieDemandeModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .HasConstraintName("FK_Domaine_Utilisateur_ModificateurId");
            });


            modelBuilder.Entity<DossierAvisSi>(entity =>
            {
                entity.HasIndex(e => e.DeclarantEntiteManagerAvisSiId);

                entity.Property(e => e.AssistanceJuridique).HasDefaultValue(false);

                entity.Property(e => e.CorrespondanceAutorite).HasDefaultValue(false);

                entity.HasOne(d => d.AnalysteCourant)
                    .WithMany(p => p.DossierAvisSiAnalysteCourants)
                    .HasForeignKey(d => d.AnalysteCourantId)
                    .HasConstraintName("FK_DossierAvisSi_AnalysteCourant_AnalysteCourantId");

                entity.HasOne(d => d.AnalysteCourantModificateur)
                    .WithMany(p => p.DossierAvisSiAnalysteCourantModificateurs)
                    .HasForeignKey(d => d.AnalysteCourantModificateurId)
                    .HasConstraintName("FK_DossierAvisSi_AnalysteCourant_AnalysteCourantModificateurId");

                entity.HasOne(d => d.DeclarantFlgeManagerAvisSi)
                    .WithMany(p => p.DossierAvisSiDeclarantFlgeManagerAvisSis)
                    .HasForeignKey(d => d.DeclarantFlgeManagerAvisSiId)
                    .HasConstraintName("FK_DossierAvisSi_DeclarantFlgeManagerAvisSi_DeclarantFlgeManagerId");

                entity.HasOne(d => d.CurrentTypologieDemandeAvisSi)
                    .WithMany(p => p.CurrentTypologieDemandeAvisSi)
                    .HasForeignKey(d => d.TypologieDemandeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CurrentDemandeDerogationAvisSi)
                    .WithMany(p => p.CurrentDemandeDerogationAvisSi)
                    .HasForeignKey(d => d.DemandeDerogationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.OldTypologieDemandeAvisSi)
                    .WithMany(p => p.OldTypologieDemandeAvisSi)
                    .HasForeignKey(d => d.OldTypologieDemandeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.OldDemandeDerogationAvisSi)
                    .WithMany(p => p.OldDemandeDerogationAvisSi)
                    .HasForeignKey(d => d.OldDemandeDerogationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.StatutAvisSi)
                    .WithMany(p => p.DossierAvisSis)
                    .HasForeignKey(d => d.StatutAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ValidateurFinaleCourant)
                    .WithMany(p => p.DossierAvisSiValidateurFinaleCourants)
                    .HasForeignKey(d => d.ValidateurFinaleCourantId)
                    .HasConstraintName("FK_DossierAvisSi_ValidateurFinaleCourant_ValidateurFinaleCourantId");

                entity.HasOne(d => d.ValidateurFinaleCourantModificateur)
                    .WithMany(p => p.DossierAvisSiValidateurFinaleCourantModificateurs)
                    .HasForeignKey(d => d.ValidateurFinaleCourantModificateurId)
                    .HasConstraintName(
                        "FK_DossierAvisSi_ValidateurFinaleCourant_ValidateurFinaleCourantModificateurId");

                entity.HasOne(d => d.ValidateurFinaleFlgeCourant)
                    .WithMany(p => p.DossierAvisSiValidateurFinaleFlgeCourants)
                    .HasForeignKey(d => d.ValidateurFinaleFlgeCourantId)
                    .HasConstraintName("FK_DossierAvisSi_ValidateurFinaleFlgeCourant_ValidateurFinaleFlgeCourantId");

                entity.HasOne(d => d.ValidateurFinaleFlgeCourantModificateur)
                    .WithMany(p => p.DossierAvisSiValidateurFinaleFlgeCourantModificateurs)
                    .HasForeignKey(d => d.ValidateurFinaleFlgeCourantModificateurId)
                    .HasConstraintName(
                        "FK_DossierAvisSi_ValidateurFinaleFlgeCourant_ValidateurFinaleFlgeCourantModificateurId");

                entity.HasOne(d => d.ValidateurIntermCourant)
                    .WithMany(p => p.DossierAvisSiValidateurIntermCourants)
                    .HasForeignKey(d => d.ValidateurIntermCourantId)
                    .HasConstraintName("FK_DossierAvisSi_ValidateurIntermCourant_ValidateurIntermCourantId");

                entity.HasOne(d => d.ValidateurIntermCourantModificateur)
                    .WithMany(p => p.DossierAvisSiValidateurIntermCourantModificateurs)
                    .HasForeignKey(d => d.ValidateurIntermCourantModificateurId)
                    .HasConstraintName(
                        "FK_DossierAvisSi_ValidateurIntermCourant_ValidateurIntermCourantModificateurId");

                entity.HasOne(d => d.ValidateurIntermFlgeCourant)
                    .WithMany(p => p.DossierAvisSiValidateurIntermFlgeCourants)
                    .HasForeignKey(d => d.ValidateurIntermFlgeCourantId)
                    .HasConstraintName("FK_DossierAvisSi_ValidateurIntermFlgeCourant_ValidateurIntermFlgeCourantId");

                entity.HasOne(d => d.ValidateurIntermFlgeCourantModificateur)
                    .WithMany(p => p.DossierAvisSiValidateurIntermFlgeCourantModificateurs)
                    .HasForeignKey(d => d.ValidateurIntermFlgeCourantModificateurId)
                    .HasConstraintName(
                        "FK_DossierAvisSi_ValidateurIntermFlgeCourant_ValidateurIntermFlgeCourantModificateurId");

                entity.HasOne(d => d.DossierValidationFinaleAvisSi)
                    .WithMany(p => p.DossierAvisSis)
                    .HasForeignKey(d => d.DossierValidationFinaleAvisSiId);

                entity.HasOne(d => d.DossierValidationIntermediaireAvisSi)
                    .WithMany(p => p.DossierAvisSis)
                    .HasForeignKey(d => d.DossierValidationIntermediaireAvisSiId);

                entity.Property(e => e.DescriptionEvenement)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.ProgrammeSanction)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.SyntheseAvisResponsableLocal)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.RemonteeResponsableLocal)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.AvisFlge)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireEntiteLocale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireFlge)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireDdc)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.ComplementInformation)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireSuperviseur)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireRequalification)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.AnalyseConformite)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.InfCompleClient)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.InfCompleContrepartie)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<DossierAvisSiClientMorale>(entity =>
            {
                entity.HasOne(d => d.DossierAvisSi)
                    .WithMany(p => p.DossierAvisSiClientMorales)
                    .HasForeignKey(d => d.DossierAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ClientMoraleAvisSi)
                    .WithMany(p => p.DossierAvisSiClientMorales)
                    .HasForeignKey(d => d.ClientMoraleAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DossierAvisSiContrepartieMorale>(entity =>
            {
                entity.HasOne(d => d.DossierAvisSi)
                    .WithMany(p => p.DossierAvisSiContrepartieMorales)
                    .HasForeignKey(d => d.DossierAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ContrepartieMoraleAvisSi)
                    .WithMany(p => p.DossierAvisSiContrepartieMorales)
                    .HasForeignKey(d => d.ContrepartieMoraleAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DossierAvisSiClientPhysique>(entity =>
            {
                entity.HasOne(d => d.DossierAvisSi)
                    .WithMany(p => p.DossierAvisSiClientPhysiques)
                    .HasForeignKey(d => d.DossierAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ClientPhysiqueAvisSi)
                    .WithMany(p => p.DossierAvisSiClientPhysiques)
                    .HasForeignKey(d => d.ClientPhysiqueAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DossierAvisSiContrepartiePhysique>(entity =>
            {
                entity.HasOne(d => d.DossierAvisSi)
                    .WithMany(p => p.DossierAvisSiContrepartiePhysiques)
                    .HasForeignKey(d => d.DossierAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ContrepartiePhysiqueAvisSi)
                    .WithMany(p => p.DossierAvisSiContrepartiePhysiques)
                    .HasForeignKey(d => d.ContrepartiePhysiqueAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<DossierDefaillanceAvisSi>(entity =>
            {
                entity.HasOne(d => d.DefaillanceAvisSi)
                    .WithMany(p => p.DossierDefaillanceAvisSis)
                    .HasForeignKey(d => d.DefaillanceAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DossierAvisSi)
                    .WithMany(p => p.DossierDefaillanceAvisSis)
                    .HasForeignKey(d => d.DossierAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DossierDefaillanceAvisSi_DossierAvisSi_DossierId");
            });

            modelBuilder.Entity<DossierValidationFinaleAvisSi>(entity =>
            {
                entity.HasOne(d => d.Validateur)
                    .WithMany(p => p.DossierValidationFinaleAvisSis)
                    .HasForeignKey(d => d.ValidateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DossierValidationFinaleAvisSi_Validateur_ValidateurId");

                entity.Property(e => e.Validation)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<DossierValidationFinaleFlgeAvisSi>(entity =>
            {
                entity.HasOne(d => d.Validateur)
                    .WithMany(p => p.DossierValidationFinaleFlgeAvisSis)
                    .HasForeignKey(d => d.ValidateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DossierValidationFinaleFlgeAvisSi_Validateur_ValidateurId");

                entity.Property(e => e.Validation)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<DossierValidationIntermediaireAvisSi>(entity =>
            {
                entity.HasOne(d => d.Validateur)
                    .WithMany(p => p.DossierValidationIntermediaireAvisSis)
                    .HasForeignKey(d => d.ValidateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DossierValidationIntermediaireAvisSi_Validateur_ValidateurId");

                entity.Property(e => e.Validation)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<DossierValidationIntermediaireFlgeAvisSi>(entity =>
            {
                entity.HasOne(d => d.Validateur)
                    .WithMany(p => p.DossierValidationIntermediaireFlgeAvisSis)
                    .HasForeignKey(d => d.ValidateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DossierValidationIntermediaireFlgeAvisSi_Validateur_ValidateurId");

                entity.Property(e => e.Validation)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<EmailNotificationType>(entity => { entity.Property(e => e.IsActive); });

            modelBuilder.Entity<EntiteConcerneAvisSi>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.EntiteConcerneAvisSiCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SousEntite)
                    .WithMany(p => p.EntiteConcerneAvisSis)
                    .HasForeignKey(d => d.SousEntiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.Commentaire)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<EquipeValidationFlgeAvisSi>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.EquipeValidationFlgeAvisSiCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.EquipeValidationFlgeAvisSis)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<DemandeDerogation>(entity =>
            {
                entity.HasOne(d => d.TypologieDemande)
                    .WithMany(p => p.DemandeDerogations)
                    .HasForeignKey(d => d.TypologieDemandeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DemandeDerogation_Domaine_DomaineId");
            });


            modelBuilder.Entity<ClientPhysiqueAvisSi>(entity =>
            {
                entity.HasIndex(e => e.CiviliteId);

                entity.HasIndex(e => e.NationaliteId);

                entity.HasIndex(e => e.PaysNaissanceId);

                entity.HasOne(d => d.Civilite)
                    .WithMany(p => p.ClientPhysiqueAvisSis)
                    .HasForeignKey(d => d.CiviliteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ContrepartiePhysiqueAvisSi>(entity =>
            {
                entity.HasIndex(e => e.CiviliteId);

                entity.HasIndex(e => e.NationaliteId);

                entity.HasIndex(e => e.PaysNaissanceId);

                entity.HasOne(d => d.Civilite)
                    .WithMany(p => p.ContrepartiePhysiqueAvisSis)
                    .HasForeignKey(d => d.CiviliteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<PlanActionAnalyseAvisSi>(entity =>
            {
                entity.HasOne(d => d.AnalyseDossierAvisSi)
                    .WithMany(p => p.PlanActionAnalyseAvisSis)
                    .HasForeignKey(d => d.AnalyseDossierAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DirectionPorteur)
                    .WithMany(p => p.PlanActionAnalyseAvisSis)
                    .HasForeignKey(d => d.DirectionPorteurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Porteur)
                    .WithMany(p => p.PlanActionAnalyseAvisSiPorteurs)
                    .HasForeignKey(d => d.PorteurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.StatutPlanActionAnalyseAvisSi)
                    .WithMany(p => p.PlanActionAnalyseAvisSis)
                    .HasForeignKey(d => d.StatutPlanActionAnalyseAvisSiId)
                    .HasConstraintName("FK_PlanActionAnalyseAvisSi_StatutPlanActionAnalyse_StatutPlanActionAnalyseId");

                entity.Property(e => e.Response)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Intitule)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Description)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Commentaire)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<RoutageAvisSi>(entity =>
            {
                entity.HasOne(d => d.TypologieDemande)
                    .WithMany(p => p.RoutageAvisSis)
                    .HasForeignKey(d => d.TypologieDemandeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<RoutageFlgeAvisSi>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.RoutageFlgeAvisSiCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DirectionFlge)
                    .WithMany(p => p.RoutageFlgeAvisSis)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoutageFlgeAvisSi_Direction_DirectionFlgeId");

                entity.HasOne(d => d.TypologieDemande)
                    .WithMany(p => p.RoutageFlgeAvisSis)
                    .HasForeignKey(d => d.TypologieDemandeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<TypeClientAvisSi>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);

                entity.Property(e => e.IsCorporate);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.TypeClientAvisSis)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<UtilisateurEquipeAnalyseAvisSi>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.UtilisateurEquipeAnalyseAvisSiCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .HasConstraintName("FK_UtilisateurEquipeAnalyseAvisSi_Createur_CreateurId");

                entity.HasOne(d => d.EquipeAnalyseAvisSi)
                    .WithMany(p => p.UtilisateurEquipeAnalyseAvisSis)
                    .HasForeignKey(d => d.EquipeAnalyseAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.UtilisateurEquipeAnalyseAvisSiModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .HasConstraintName("FK_UtilisateurEquipeAnalyseAvisSi_Modificateur_ModificateurId");

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.UtilisateurEquipeAnalyseAvisSiUtilisateurs)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<UtilisateurEquipeSuperviseurAvisSi>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.UtilisateurEquipeSuperviseurAvisSiCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .HasConstraintName("FK_UtilisateurEquipeSuperviseurAvisSi_Createur_CreateurId");

                entity.HasOne(d => d.EquipeSuperviseurAvisSi)
                    .WithMany(p => p.UtilisateurEquipeSuperviseurAvisSis)
                    .HasForeignKey(d => d.EquipeSuperviseurAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.UtilisateurEquipeSuperviseurAvisSiModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .HasConstraintName("FK_UtilisateurEquipeSuperviseurAvisSi_Modificateur_ModificateurId");

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.UtilisateurEquipeSuperviseurAvisSiUtilisateurs)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<UtilisateurEquipeSuperviseurAvis>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.UtilisateurEquipeSuperviseurAvisCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.EquipeSuperviseurAvis)
                    .WithMany(p => p.UtilisateurEquipeSuperviseurAviss)
                    .HasForeignKey(d => d.EquipeSuperviseurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.UtilisateurEquipeSuperviseurAvisModificateurs)
                    .HasForeignKey(d => d.ModificateurId);

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.UtilisateurEquipeSuperviseurAvisUtilisateurs)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<UtilisateurEquipeValidationAvisSi>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.UtilisateurEquipeValidationAvisSiCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .HasConstraintName("FK_UtilisateurEquipeValidationAvisSi_Createur_CreateurId");

                entity.HasOne(d => d.EquipeValidationAvisSi)
                    .WithMany(p => p.UtilisateurEquipeValidationAvisSis)
                    .HasForeignKey(d => d.EquipeValidationAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UtilisateurEquipeValidationAvisSi_EquipeValidation_EquipeValidationId");

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.UtilisateurEquipeValidationAvisSiModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .HasConstraintName("FK_UtilisateurEquipeValidationAvisSi_Modificateur_ModificateurId");

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.UtilisateurEquipeValidationAvisSiUtilisateurs)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<UtilisateurEquipeValidationFlgeAvisSi>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.UtilisateurEquipeValidationFlgeAvisSiCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .HasConstraintName("FK_UtilisateurEquipeValidationFlgeAvisSi_Createur_CreateurId");

                entity.HasOne(d => d.EquipeValidationFlgeAvisSi)
                    .WithMany(p => p.UtilisateurEquipeValidationFlgeAvisSis)
                    .HasForeignKey(d => d.EquipeValidationFlgeAvisSiId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName(
                        "FK_UtilisateurEquipeValidationFlgeAvisSi_EquipeValidationAvisSi_EquipeValidationAvisSiId");

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.UtilisateurEquipeValidationFlgeAvisSiModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .HasConstraintName("FK_UtilisateurEquipeValidationFlgeAvisSi_Modificateur_ModificateurId");

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.UtilisateurEquipeValidationFlgeAvisSiUtilisateurs)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AnalyseDossierSAvis>(entity =>
            {
                entity.HasIndex(e => e.AnalysteId);
                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.DossierSAvisId);

                entity.HasOne(d => d.Analyste)
                    .WithMany(p => p.AnalyseDossierSAviss)
                    .HasForeignKey(d => d.AnalysteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnalyseDossierSAvis_Analyste_AnalysteId");

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.AnalyseDossierSAvisModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnalyseDossierSAvis_Modificateur_ModificateurId");

                entity.HasOne(d => d.DossierSAvis)
                    .WithMany(p => p.AnalyseDossierSAviss)
                    .HasForeignKey(d => d.DossierSAvisId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.Analyse)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<AnalyseDossierAvisIm>(entity =>
            {
                entity.HasIndex(e => e.AnalysteId);
                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.DossierAvisImId);

                entity.HasOne(d => d.Analyste)
                    .WithMany(p => p.AnalyseDossierAvisIms)
                    .HasForeignKey(d => d.AnalysteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnalyseDossierAvisIm_Analyste_AnalysteId");

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.AnalyseDossierAvisImModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnalyseDossierAvisIm_Modificateur_ModificateurId");

                entity.HasOne(d => d.DossierAvisIm)
                    .WithMany(p => p.AnalyseDossierAvisIms)
                    .HasForeignKey(d => d.DossierAvisImId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.Analyse)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<ApplicationAvis>(entity =>
            {
                entity.HasOne(d => d.EnvironnementAvis)
                    .WithMany(p => p.ApplicationAviss)
                    .HasForeignKey(d => d.EnvironnementId);
            });


            modelBuilder.Entity<ConclusionSAvis>(entity =>
            {
                entity.HasIndex(e => e.AnalysteId);
                entity.HasOne(d => d.Analyste)
                    .WithMany(p => p.ConclusionSAviss)
                    .HasForeignKey(d => d.AnalysteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConclusionSAvis_Analyste_AnalysteId");
                entity.HasIndex(e => e.ModificateurId);
                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.ConclusionSAvisModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConclusionSAvis_Modificateur_ModificateurId");

                modelBuilder
                    .Entity<ConclusionSAvis>()
                    .Property(e => e.Conclusion)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<ConclusionAvisIm>(entity =>
            {
                entity.HasIndex(e => e.AnalysteId);
                entity.HasOne(d => d.Analyste)
                    .WithMany(p => p.ConclusionAvisIms)
                    .HasForeignKey(d => d.AnalysteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConclusionAvisIm_Analyste_AnalysteId");
                entity.HasIndex(e => e.ModificateurId);
                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.ConclusionAvisImModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConclusionAvisIm_Modificateur_ModificateurId");

                modelBuilder
                    .Entity<ConclusionAvisIm>()
                    .Property(e => e.Conclusion)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<DefaillanceAvis>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.DefaillanceAvisCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<DemandeInformationAvis>(entity =>
            {
                entity.HasOne(d => d.ActionTypeDemandeInformationAvis)
                    .WithMany(p => p.DemandeInformationAviss)
                    .HasForeignKey(d => d.ActionTypeDemandeInformationId);

                entity.HasOne(d => d.AnalyseDossierSAvis)
                    .WithMany(p => p.DemandeInformationAviss)
                    .HasForeignKey(d => d.AnalyseDossierId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TypologieDemandeAvis)
                    .WithMany(p => p.DemandeInformationAviss)
                    .HasForeignKey(d => d.TypologieDemandeId);

                entity.HasOne(d => d.AnalyseDossierAvisIm)
                    .WithMany(p => p.DemandeInformationAviss)
                    .HasForeignKey(d => d.AnalyseDossierId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<DocumentDemandeInformationAvis>(entity =>
            {
                entity.HasOne(d => d.DocumentAvis)
                    .WithMany(p => p.DocumentDemandeInformationAviss)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<DocumentDossierSAvis>(entity =>
            {
                entity.HasIndex(e => e.AppartenanceDocumentSAvisId);

                entity.HasIndex(e => e.DocumentSAvisId);

                entity.HasIndex(e => e.DossierSAvisId);

                entity.HasIndex(e => e.EquipeAnalyseSAvisId);

                entity.HasIndex(e => e.UtilisateurId);

                entity.HasOne(d => d.DocumentAvis)
                    .WithMany(p => p.DocumentDossierSAviss)
                    .HasForeignKey(d => d.DocumentSAvisId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DossierSAvis)
                    .WithMany(p => p.DocumentDossierSAviss)
                    .HasForeignKey(d => d.DossierSAvisId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<DocumentDossierAvisIm>(entity =>
            {
                entity.HasIndex(e => e.AppartenanceDocumentAvisImId);

                entity.HasIndex(e => e.DocumentAvisImId);

                entity.HasIndex(e => e.DossierAvisImId);

                entity.HasIndex(e => e.EquipeAnalyseAvisImId);

                entity.HasIndex(e => e.UtilisateurId);

                entity.HasOne(d => d.DocumentAvis)
                    .WithMany(p => p.DocumentDossierAvisIms)
                    .HasForeignKey(d => d.DocumentAvisImId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DossierAvisIm)
                    .WithMany(p => p.DocumentDossierAvisIms)
                    .HasForeignKey(d => d.DossierAvisImId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DocumentEntiteConcerneAvis>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.DocumentEntiteConcerneAvisCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DocumentAvis)
                    .WithMany(p => p.DocumentEntiteConcerneAviss)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.EntiteConcerneSAvis)
                    .WithMany(p => p.DocumentEntiteConcerneAviss)
                    .HasForeignKey(d => d.EntiteConcerneId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.EntiteConcerneAvisIm)
                    .WithMany(p => p.DocumentEntiteConcerneAviss)
                    .HasForeignKey(d => d.EntiteConcerneId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.DocumentEntiteConcerneAvisModificateurs)
                    .HasForeignKey(d => d.ModificateurId);
            });


            modelBuilder.Entity<DocumentPlanActionAnalyseAvis>(entity =>
            {
                entity.HasOne(d => d.DocumentAvis)
                    .WithMany(p => p.DocumentPlanActionAnalyseAviss)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PlanActionAnalyseAvis)
                    .WithMany(p => p.DocumentPlanActionAnalyseAviss)
                    .HasForeignKey(d => d.PlanActionAnalyseId);
            });


            modelBuilder.Entity<TypologieDemandeAvis>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.TypologieDemandeAvisCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.TypologieDemandeAvisModificateurs)
                    .HasForeignKey(d => d.ModificateurId);
            });


            modelBuilder.Entity<DossierSAvis>(entity =>
            {
                entity.Property(e => e.AssistanceJuridique).HasDefaultValue(false);

                entity.Property(e => e.CorrespondanceAutorite).HasDefaultValue(false);

                entity.HasOne(d => d.Superviseur)
                    .WithMany(p => p.DossierSAvisSuperviseurs)
                    .HasForeignKey(d => d.SuperviseurId)
                    .HasConstraintName("FK_DossierSAvis_Superviseur_SuperviseurId");

                entity.HasOne(d => d.AnalysteCourant)
                    .WithMany(p => p.DossierSAvisAnalysteCourants)
                    .HasForeignKey(d => d.AnalysteCourantId)
                    .HasConstraintName("FK_DossierSAvis_AnalysteCourant_AnalysteCourantId");

                entity.HasOne(d => d.AnalysteCourantModificateur)
                    .WithMany(p => p.DossierSAvisAnalysteCourantModificateurs)
                    .HasForeignKey(d => d.AnalysteCourantModificateurId)
                    .HasConstraintName("FK_DossierSAvis_AnalysteCourant_AnalysteCourantModificateurId");

                entity.HasOne(d => d.CurrentTypologieDemandeAvis)
                    .WithMany(p => p.CurrentTypologieDemandeAvis)
                    .HasForeignKey(d => d.TypologieDemandeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.OldTypologieDemandeAvis)
                    .WithMany(p => p.OldTypologieDemandeAvis)
                    .HasForeignKey(d => d.OldTypologieDemandeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TypologieClientAvis)
                    .WithMany(p => p.DossierSAviss)
                    .HasForeignKey(d => d.TypologieClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CurrentCategorieAvis)
                    .WithMany(p => p.CurrentCategorieAvis)
                    .HasForeignKey(d => d.CategorieId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CurrentSousCategorieAvis)
                    .WithMany(p => p.CurrentSousCategorieAvis)
                    .HasForeignKey(d => d.SousCategorieId)
                    .OnDelete(DeleteBehavior.ClientSetNull);


                entity.HasOne(d => d.CurrentCategorieOutilAvis)
                    .WithMany(p => p.CurrentCategorieOutilAvis)
                    .HasForeignKey(d => d.CategorieOutilId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Priorite)
                    .WithMany(p => p.Priorite)
                    .HasForeignKey(d => d.PrioriteId)
                    .HasConstraintName("FK_CommunAvisPriorite_DossierSAvis");

                entity.HasOne(d => d.CurrentDomaineAvis)
                    .WithMany(p => p.CurrentDomaineAvis)
                    .HasForeignKey(d => d.DomaineId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.OldCategorieAvis)
                    .WithMany(p => p.OldCategorieAvis)
                    .HasForeignKey(d => d.OldCategorieId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.OldSousCategorieAvis)
                    .WithMany(p => p.OldSousCategorieAvis)
                    .HasForeignKey(d => d.OldSousCategorieId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.OldDomaineAvis)
                    .WithMany(p => p.OldDomaineAvis)
                    .HasForeignKey(d => d.OldDomaineId)
                    .OnDelete(DeleteBehavior.ClientSetNull);


                entity.HasOne(d => d.OldCategorieOutilAvis)
                    .WithMany(p => p.OldCategorieOutilAvis)
                    .HasForeignKey(d => d.OldCategorieOutilId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TypologieClientAvis)
                    .WithMany(p => p.DossierSAviss)
                    .HasForeignKey(d => d.TypologieClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.StatutAvis)
                    .WithMany(p => p.DossierSAviss)
                    .HasForeignKey(d => d.StatutId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ValidateurFinaleCourant)
                    .WithMany(p => p.DossierSAvisValidateurFinaleCourants)
                    .HasForeignKey(d => d.ValidateurFinaleCourantId)
                    .HasConstraintName("FK_DossierSAvis_ValidateurFinaleCourant_ValidateurFinaleCourantId");

                entity.HasOne(d => d.ValidateurFinaleCourantModificateur)
                    .WithMany(p => p.DossierSAvisValidateurFinaleCourantModificateurs)
                    .HasForeignKey(d => d.ValidateurFinaleCourantModificateurId)
                    .HasConstraintName("FK_DossierSAvis_ValidateurFinaleCourant_ValidateurFinaleCourantModificateurId");

                entity.HasOne(d => d.ValidateurFinaleFlgeCourant)
                    .WithMany(p => p.DossierSAvisValidateurFinaleFlgeCourants)
                    .HasForeignKey(d => d.ValidateurFinaleFlgeCourantId)
                    .HasConstraintName("FK_DossierSAvis_ValidateurFinaleFlgeCourant_ValidateurFinaleFlgeCourantId");

                entity.HasOne(d => d.ValidateurFinaleFlgeCourantModificateur)
                    .WithMany(p => p.DossierSAvisValidateurFinaleFlgeCourantModificateurs)
                    .HasForeignKey(d => d.ValidateurFinaleFlgeCourantModificateurId)
                    .HasConstraintName(
                        "FK_DossierSAvis_ValidateurFinaleFlgeCourant_ValidateurFinaleFlgeCourantModificateurId");

                entity.HasOne(d => d.ValidateurIntermCourant)
                    .WithMany(p => p.DossierSAvisValidateurIntermCourants)
                    .HasForeignKey(d => d.ValidateurIntermCourantId)
                    .HasConstraintName("FK_DossierSAvis_ValidateurIntermCourant_ValidateurIntermCourantId");

                entity.HasOne(d => d.ValidateurIntermCourantModificateur)
                    .WithMany(p => p.DossierSAvisValidateurIntermCourantModificateurs)
                    .HasForeignKey(d => d.ValidateurIntermCourantModificateurId)
                    .HasConstraintName("FK_DossierSAvis_ValidateurIntermCourant_ValidateurIntermCourantModificateurId");

                entity.HasOne(d => d.ValidateurIntermFlgeCourant)
                    .WithMany(p => p.DossierSAvisValidateurIntermFlgeCourants)
                    .HasForeignKey(d => d.ValidateurIntermFlgeCourantId)
                    .HasConstraintName("FK_DossierSAvis_ValidateurIntermFlgeCourant_ValidateurIntermFlgeCourantId");

                entity.HasOne(d => d.ValidateurIntermFlgeCourantModificateur)
                    .WithMany(p => p.DossierSAvisValidateurIntermFlgeCourantModificateurs)
                    .HasForeignKey(d => d.ValidateurIntermFlgeCourantModificateurId)
                    .HasConstraintName(
                        "FK_DossierSAvis_ValidateurIntermFlgeCourant_ValidateurIntermFlgeCourantModificateurId");

                entity.Property(e => e.DescriptionEvenement)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.PartieConcerne)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.ProblemeOutil)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.ProgrammeSanction)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.SyntheseAvisResponsableLocal)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.RemonteeResponsableLocal)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.AvisFlge)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireEntiteLocale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireFlge)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireDdc)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.ComplementInformation)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireSuperviseur)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireRequalification)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });
            modelBuilder.Entity<DossierAvisIm>(entity =>
            {
                entity.Property(e => e.AssistanceJuridique).HasDefaultValue(false);

                entity.Property(e => e.CorrespondanceAutorite).HasDefaultValue(false);

                entity.HasOne(d => d.Superviseur)
                    .WithMany(p => p.DossierAvisImSuperviseurs)
                    .HasForeignKey(d => d.SuperviseurId)
                    .HasConstraintName("FK_DossierAvisIm_Superviseur_SuperviseurId");

                entity.HasOne(d => d.AnalysteCourant)
                    .WithMany(p => p.DossierAvisImAnalysteCourants)
                    .HasForeignKey(d => d.AnalysteCourantId)
                    .HasConstraintName("FK_DossierAvisIm_AnalysteCourant_AnalysteCourantId");

                entity.HasOne(d => d.AnalysteCourantModificateur)
                    .WithMany(p => p.DossierAvisImAnalysteCourantModificateurs)
                    .HasForeignKey(d => d.AnalysteCourantModificateurId)
                    .HasConstraintName("FK_DossierAvisIm_AnalysteCourant_AnalysteCourantModificateurId");

                entity.HasOne(d => d.CurrentTypologieDemandeAvisIm)
                    .WithMany(p => p.CurrentTypologieDemandeAvisIm)
                    .HasForeignKey(d => d.TypologieDemandeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CurrentLienUsAvisIm)
                    .WithMany(p => p.CurrentLienUsAvisIm)
                    .HasForeignKey(d => d.LienUsId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.CurrentTypeFormulaireAvisIm)
                    .WithMany(p => p.CurrentTypeFormulaireAvisIm)
                    .HasForeignKey(d => d.TypeFormulaireId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.OldTypologieDemandeAvisIm)
                    .WithMany(p => p.OldTypologieDemandeAvisIm)
                    .HasForeignKey(d => d.OldTypologieDemandeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.OldLienUsAvisIm)
                    .WithMany(p => p.OldLienUsAvisIm)
                    .HasForeignKey(d => d.OldLienUsId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.OldTypeFormulaireAvisIm)
                    .WithMany(p => p.OldTypeFormulaireAvisIm)
                    .HasForeignKey(d => d.OldTypeFormulaireId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.StatutAvis)
                    .WithMany(p => p.DossierAvisIms)
                    .HasForeignKey(d => d.StatutId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ValidateurFinaleCourant)
                    .WithMany(p => p.DossierAvisImValidateurFinaleCourants)
                    .HasForeignKey(d => d.ValidateurFinaleCourantId)
                    .HasConstraintName("FK_DossierAvisIm_ValidateurFinaleCourant_ValidateurFinaleCourantId");

                entity.HasOne(d => d.ValidateurFinaleCourantModificateur)
                    .WithMany(p => p.DossierAvisImValidateurFinaleCourantModificateurs)
                    .HasForeignKey(d => d.ValidateurFinaleCourantModificateurId)
                    .HasConstraintName(
                        "FK_DossierAvisIm_ValidateurFinaleCourant_ValidateurFinaleCourantModificateurId");

                entity.HasOne(d => d.ValidateurFinaleFlgeCourant)
                    .WithMany(p => p.DossierAvisImValidateurFinaleFlgeCourants)
                    .HasForeignKey(d => d.ValidateurFinaleFlgeCourantId)
                    .HasConstraintName("FK_DossierAvisIm_ValidateurFinaleFlgeCourant_ValidateurFinaleFlgeCourantId");

                entity.HasOne(d => d.ValidateurFinaleFlgeCourantModificateur)
                    .WithMany(p => p.DossierAvisImValidateurFinaleFlgeCourantModificateurs)
                    .HasForeignKey(d => d.ValidateurFinaleFlgeCourantModificateurId)
                    .HasConstraintName(
                        "FK_DossierAvisIm_ValidateurFinaleFlgeCourant_ValidateurFinaleFlgeCourantModificateurId");

                entity.HasOne(d => d.ValidateurIntermCourant)
                    .WithMany(p => p.DossierAvisImValidateurIntermCourants)
                    .HasForeignKey(d => d.ValidateurIntermCourantId)
                    .HasConstraintName("FK_DossierAvisIm_ValidateurIntermCourant_ValidateurIntermCourantId");

                entity.HasOne(d => d.ValidateurIntermCourantModificateur)
                    .WithMany(p => p.DossierAvisImValidateurIntermCourantModificateurs)
                    .HasForeignKey(d => d.ValidateurIntermCourantModificateurId)
                    .HasConstraintName(
                        "FK_DossierAvisIm_ValidateurIntermCourant_ValidateurIntermCourantModificateurId");

                entity.HasOne(d => d.ValidateurIntermFlgeCourant)
                    .WithMany(p => p.DossierAvisImValidateurIntermFlgeCourants)
                    .HasForeignKey(d => d.ValidateurIntermFlgeCourantId)
                    .HasConstraintName("FK_DossierAvisIm_ValidateurIntermFlgeCourant_ValidateurIntermFlgeCourantId");

                entity.HasOne(d => d.ValidateurIntermFlgeCourantModificateur)
                    .WithMany(p => p.DossierAvisImValidateurIntermFlgeCourantModificateurs)
                    .HasForeignKey(d => d.ValidateurIntermFlgeCourantModificateurId)
                    .HasConstraintName(
                        "FK_DossierAvisIm_ValidateurIntermFlgeCourant_ValidateurIntermFlgeCourantModificateurId");

                entity.Property(e => e.DescriptionEvenement)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.LibelleObjet)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.PartieConcerne)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.ProblemeOutil)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.ProgrammeSanction)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.SyntheseAvisResponsableLocal)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.RemonteeResponsableLocal)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.AvisFlge)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireEntiteLocale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireFlge)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireDdc)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.ComplementInformation)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireSuperviseur)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder
                .Entity<DocumentAvis>()
                .Property(e => e.FileContent)
                .Metadata
                .SetValueComparer(
                    new ValueComparer<byte[]>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToArray()));

            modelBuilder.Entity<DossierSAvisPersonneMorale>(entity =>
            {
                entity.HasOne(d => d.DossierSAvis)
                    .WithMany(p => p.DossierSAvisPersonneMorales)
                    .HasForeignKey(d => d.DossierSAvisId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PersonneMoraleAvis)
                    .WithMany(p => p.DossierSAvisPersonneMorales)
                    .HasForeignKey(d => d.PersonneMoraleSAvisId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DossierSAvisPersonnePhysique>(entity =>
            {
                entity.HasOne(d => d.DossierSAvis)
                    .WithMany(p => p.DossierSAvisPersonnePhysiques)
                    .HasForeignKey(d => d.DossierSAvisId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PersonnePhysiqueAvis)
                    .WithMany(p => p.DossierSAvisPersonnePhysiques)
                    .HasForeignKey(d => d.PersonnePhysiqueSAvisId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<DossierDefaillanceSAvis>(entity =>
            {
                entity.HasOne(d => d.DefaillanceAvis)
                    .WithMany(p => p.DossierDefaillanceSAviss)
                    .HasForeignKey(d => d.DefaillanceSAvisId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DossierSAvis)
                    .WithMany(p => p.DossierDefaillanceSAviss)
                    .HasForeignKey(d => d.DossierSAvisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DossierDefaillanceSAvis_DossierSAvis_DossierId");
            });

            modelBuilder.Entity<DossierAvisImPersonneMorale>(entity =>
            {
                entity.HasOne(d => d.DossierAvisIm)
                    .WithMany(p => p.DossierAvisImPersonneMorales)
                    .HasForeignKey(d => d.DossierAvisImId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PersonneMoraleAvis)
                    .WithMany(p => p.DossierAvisImPersonneMorales)
                    .HasForeignKey(d => d.PersonneMoraleAvisImId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DossierAvisImPersonnePhysique>(entity =>
            {
                entity.HasOne(d => d.DossierAvisIm)
                    .WithMany(p => p.DossierAvisImPersonnePhysiques)
                    .HasForeignKey(d => d.DossierAvisImId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PersonnePhysiqueAvis)
                    .WithMany(p => p.DossierAvisImPersonnePhysiques)
                    .HasForeignKey(d => d.PersonnePhysiqueAvisImId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<DossierDefaillanceAvisIm>(entity =>
            {
                entity.HasOne(d => d.DefaillanceAvis)
                    .WithMany(p => p.DossierDefaillanceAvisIms)
                    .HasForeignKey(d => d.DefaillanceAvisImId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DossierAvisIm)
                    .WithMany(p => p.DossierDefaillanceAvisIms)
                    .HasForeignKey(d => d.DossierAvisImId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DossierDefaillanceAvisIm_DossierAvisIm_DossierId");
            });

            modelBuilder.Entity<DossierValidationFinaleAvis>(entity =>
            {
                entity.HasOne(d => d.Validateur)
                    .WithMany(p => p.DossierValidationFinaleAviss)
                    .HasForeignKey(d => d.ValidateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.Validation)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<DossierValidationFinaleFlgeAvis>(entity =>
            {
                entity.HasOne(d => d.Validateur)
                    .WithMany(p => p.DossierValidationFinaleFlgeAviss)
                    .HasForeignKey(d => d.ValidateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.Validation)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<DossierValidationIntermediaireAvis>(entity =>
            {
                entity.HasOne(d => d.Validateur)
                    .WithMany(p => p.DossierValidationIntermediaireAviss)
                    .HasForeignKey(d => d.ValidateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.Validation)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<DossierValidationIntermediaireFlgeAvis>(entity =>
            {
                entity.HasOne(d => d.Validateur)
                    .WithMany(p => p.DossierValidationIntermediaireFlgeAviss)
                    .HasForeignKey(d => d.ValidateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.Validation)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<EmailNotificationType>(entity => { entity.Property(e => e.IsActive); });

            modelBuilder.Entity<EntiteConcerneSAvis>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.EntiteConcerneSAvisCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SousEntite)
                    .WithMany(p => p.EntiteConcerneSAviss)
                    .HasForeignKey(d => d.SousEntiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<EntiteConcerneAvisIm>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.EntiteConcerneAvisImCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.SousEntite)
                    .WithMany(p => p.EntiteConcerneAvisIms)
                    .HasForeignKey(d => d.SousEntiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<EquipeValidationFlgeAvis>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.EquipeValidationFlgeAvisCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.EquipeValidationFlgeAviss)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<EntiteGroupeEntiteAvis>(entity =>
            {
                entity.HasOne(d => d.Entite)
                    .WithMany(p => p.EntiteGroupeEntites)
                    .HasForeignKey(d => d.EntiteId);
            });

            modelBuilder.Entity<PersonnePhysiqueAvis>(entity =>
            {
                entity.HasIndex(e => e.CiviliteId);

                entity.HasIndex(e => e.NationaliteId);

                entity.HasIndex(e => e.PaysId);

                entity.HasIndex(e => e.PaysNaissanceId);

                entity.HasOne(d => d.Civilite)
                    .WithMany(p => p.PersonnePhysiqueAviss)
                    .HasForeignKey(d => d.CiviliteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.Ville)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Prenoms)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.NomUsuel)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.NomNaissance)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.LieuNaissance)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CodePostale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Adresse)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<PlanActionAnalyseAvis>(entity =>
            {
                entity.HasOne(d => d.AnalyseDossierSAvis)
                    .WithMany(p => p.PlanActionAnalyseAviss)
                    .HasForeignKey(d => d.AnalyseDossierId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.AnalyseDossierAvisIm)
                    .WithMany(p => p.PlanActionAnalyseAviss)
                    .HasForeignKey(d => d.AnalyseDossierId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DirectionPorteur)
                    .WithMany(p => p.PlanActionAnalyseAviss)
                    .HasForeignKey(d => d.DirectionPorteurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Porteur)
                    .WithMany(p => p.PlanActionAnalyseAvisPorteurs)
                    .HasForeignKey(d => d.PorteurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.StatutPlanActionAnalyseAvis)
                    .WithMany(p => p.PlanActionAnalyseAviss)
                    .HasForeignKey(d => d.StatutPlanActionAnalyseId);

                entity.Property(e => e.Response)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Intitule)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Description)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Commentaire)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<RoutageAvis>(entity =>
            {
                entity.HasOne(d => d.TypologieDemandeAvis)
                    .WithMany(p => p.RoutageAviss)
                    .HasForeignKey(d => d.TypologieDemandeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<RoutageFlgeAvis>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.RoutageFlgeAvisCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DirectionFlge)
                    .WithMany(p => p.RoutageFlgeAviss)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TypologieDemandeAvis)
                    .WithMany(p => p.RoutageFlgeAviss)
                    .HasForeignKey(d => d.TypologieDemandeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<TypeClientAvis>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);

                entity.Property(e => e.IsCorporate);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.TypeClientAviss)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<UtilisateurEquipeAnalyseAvis>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.UtilisateurEquipeAnalyseAvisCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.EquipeAnalyseAvis)
                    .WithMany(p => p.UtilisateurEquipeAnalyseAviss)
                    .HasForeignKey(d => d.EquipeAnalyseId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.UtilisateurEquipeAnalyseAvisModificateurs)
                    .HasForeignKey(d => d.ModificateurId);

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.UtilisateurEquipeAnalyseAvisUtilisateurs)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<UtilisateurEquipeValidationAvis>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.UtilisateurEquipeValidationAvisCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.EquipeValidationAvis)
                    .WithMany(p => p.UtilisateurEquipeValidationAviss)
                    .HasForeignKey(d => d.EquipeValidationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.UtilisateurEquipeValidationAvisModificateurs)
                    .HasForeignKey(d => d.ModificateurId);

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.UtilisateurEquipeValidationAvisUtilisateurs)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<UtilisateurEquipeValidationFlgeAvis>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.UtilisateurEquipeValidationFlgeAvisCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.EquipeValidationFlgeAvis)
                    .WithMany(p => p.UtilisateurEquipeValidationFlgeAviss)
                    .HasForeignKey(d => d.EquipeValidationFlgeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.UtilisateurEquipeValidationFlgeAvisModificateurs)
                    .HasForeignKey(d => d.ModificateurId);

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.UtilisateurEquipeValidationFlgeAvisUtilisateurs)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DossierAmf>(entity =>
            {
                entity.ToTable("DossierAmf", "Amf");

                entity.HasIndex(e => e.DirectionId, "IX_DossierAmf_DirectionId");

                entity.HasIndex(e => e.ModificateurId, "IX_DossierAmf_ModificateurId");


                entity.HasIndex(e => e.StatutDossierId, "IX_DossierAmf_StatutDossierId");


                entity.HasIndex(e => e.UtilisateurId, "IX_DossierAmf_UtilisateurId");

                entity.HasIndex(e => e.EntiteBancaireId, "IX_DossierAmf_EntiteBancaireId");


                entity.HasIndex(e => new { e.Id, e.IdentiteDeclarantId })
                    .HasDatabaseName("AK_DossierAmf_IdentiteDeclarantId")
                    .IsUnique();

                entity.Property(e => e.CodeUnique)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.DossierAmfs)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.DossierAmfModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .HasConstraintName("FK_DossierAmf_Modificateur_ModificateurId");

                entity.HasOne(d => d.StatutDossier)
                    .WithMany(p => p.DossierAmfs)
                    .HasForeignKey(d => d.StatutDossierId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.DossierAmfUtilisateurs)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.EntiteBancaire)
                    .WithMany(p => p.DossierAmfsEntiteBancaire)
                    .HasForeignKey(d => d.EntiteBancaireId)
                    .HasConstraintName("FK_DossierAmf_Modificateur_EntiteBancaireId");

                entity.Property(e => e.TypeInfractionAutre).HasMaxLength(100);

                entity.Property(e => e.MotifSuspicion)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.InformationSuplementaire)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CaracteristiqueProduitDeriveOtc)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });
            modelBuilder.Entity<DossierAmfHisto>(entity =>
            {
                entity.Property(e => e.TypeInfractionAutre).HasMaxLength(100);

                entity.Property(e => e.MotifSuspicion)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.InformationSuplementaire)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CaracteristiqueProduitDeriveOtc)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<PersonneMoraleAmf>(entity =>
            {
                entity.Property(e => e.RelationAvecEmetteur)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<PersonnePhysiqueAmf>(entity =>
            {
                entity.Property(e => e.RelationAvecEmetteur)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });


            modelBuilder.Entity<GroupeEntiteAvis>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.GroupeEntiteAvisCreateurs)
                    .HasForeignKey(d => d.CreateurId);

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.GroupeEntiteAvisModificateurs)
                    .HasForeignKey(d => d.ModificateurId);

                entity.HasOne(d => d.Activite)
                    .WithMany(p => p.GroupeEntiteAviss)
                    .HasForeignKey(d => d.ActiviteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DossierAmfPersonneMorale>(entity =>
            {
                entity.HasOne(d => d.DossierAmf)
                    .WithMany(p => p.DossierAmfPersonneMorales)
                    .HasForeignKey(d => d.DossierAmfId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PersonneMoraleAmf)
                    .WithMany(p => p.DossierAmfPersonneMorales)
                    .HasForeignKey(d => d.PersonneMoraleAmfId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TypeListeCriblage)
                    .WithMany(p => p.DossierAmfPersonneMorales)
                    .HasForeignKey(d => d.TypeListeCriblageId)
                    .HasConstraintName("FK_DossierAmfPersonneMorale_TypeListeCriblage_TypeListeCriblageId");
            });

            modelBuilder.Entity<DossierAmfPersonnePhysique>(entity =>
            {
                entity.HasOne(d => d.DossierAmf)
                    .WithMany(p => p.DossierAmfPersonnePhysiques)
                    .HasForeignKey(d => d.DossierAmfId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PersonnePhysiqueAmf)
                    .WithMany(p => p.DossierAmfPersonnePhysiques)
                    .HasForeignKey(d => d.PersonnePhysiqueAmfId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.TypeListeCriblage)
                    .WithMany(p => p.DossierAmfPersonnePhysiques)
                    .HasForeignKey(d => d.TypeListeCriblageId)
                    .HasConstraintName("FK_DossierAmfPersonnePhysique_TypeListeCriblage_TypeListeCriblageId");
            });

            modelBuilder.Entity<IdentiteDeclarant>(entity =>
            {
                entity.ToTable("IdentiteDeclarant", "Amf");

                entity.HasIndex(e => e.DeclarantId, "IX_IdentiteDeclarant_DeclarantId");

                entity.HasIndex(e => e.ModificateurId, "IX_IdentiteDeclarant_ModificateurId");

                entity.HasIndex(e => e.TitreAuquelEntiteAgitAmfId, "IX_IdentiteDeclarant_TitreAuquelEntiteAgitAmfId");

                entity.HasIndex(e => e.TypeActiviteAmfId, "IX_IdentiteDeclarant_TypeActiviteId");

                entity.HasIndex(e => e.TypeInstrumentUsuellementNegocieAmfId,
                    "IX_IdentiteDeclarant_TypeInstrumentUsuellementNegocieAmfId");

                entity.Property(e => e.EmailContactComplementaire).IsRequired();

                entity.Property(e => e.FonctionContactComplementaire).IsRequired();

                entity.Property(e => e.NomContactComplementaire).IsRequired();

                entity.Property(e => e.PrenomContactComplementaire).IsRequired();

                entity.Property(e => e.TelephoneContactComplementaire).IsRequired();

                entity.Property(e => e.TitreAuquelEntiteAgitAutre)
                    .IsRequired()
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TitreAuquelEntiteAgitAmfId).HasDefaultValueSql("((0))");

                entity.Property(e => e.TypeActiviteAutre).IsRequired();

                entity.Property(e => e.TypeActiviteAmfId).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Declarant)
                    .WithMany(p => p.IdentiteDeclarantDeclarants)
                    .HasForeignKey(d => d.DeclarantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IdentiteDeclarant_Declatrant_DeclatrantId");

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.IdentiteDeclarantModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .HasConstraintName("FK_IdentiteDeclarant_Modificateur_ModificateurId");

                entity.HasOne(d => d.TitreAuquelEntiteAgitAmf)
                    .WithMany(p => p.IdentiteDeclarants)
                    .HasForeignKey(d => d.TitreAuquelEntiteAgitAmfId);

                entity.HasOne(d => d.TypeActiviteAmf)
                    .WithMany(p => p.IdentiteDeclarants)
                    .HasForeignKey(d => d.TypeActiviteAmfId);

                entity.HasOne(d => d.TypeInstrumentUsuellementNegocieAmf)
                    .WithMany(p => p.IdentiteDeclarants)
                    .HasForeignKey(d => d.TypeInstrumentUsuellementNegocieAmfId);

                entity.Property(e => e.TypeActiviteAutre)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.TitreAuquelEntiteAgitAutre)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.TelephonePersonneDeclarante)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.TelephoneContactComplementaire)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.RelationPersonne)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.PrenomPersonneDeclarante)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.PrenomContactComplementaire)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.NomPersonneDeclarante)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.NomContactComplementaire)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.FonctionPersonneDeclarante)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.FonctionContactComplementaire)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.EmailPersonneDeclarante)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.EmailContactComplementaire)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<DossierOrdreTransactionAmf>(entity =>
            {
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.DossierOrdreTransactionAmfCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DossierOrdreTransactionAmf_Utilisateur_CreateurId");

                entity.HasOne(d => d.DossierAmf)
                    .WithMany(p => p.DossierOrdreTransactionAmfs)
                    .HasForeignKey(d => d.DossierAmfId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DossierOrdreTransactionAmf_DossierAmf_DossierAmfId");

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.DossierOrdreTransactionAmfModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .HasConstraintName("FK_DossierOrdreTransactionAmf_Utilisateur_ModificateurId");

                entity.HasOne(d => d.StatutOrdreOuTransactionAmf)
                    .WithMany(p => p.DossierOrdreTransactionAmfs)
                    .HasForeignKey(d => d.StatutOrdreOuTransactionAmfId)
                    .HasConstraintName(
                        "FK_DossierOrdreTransactionAmf_StatutOrdreOuTransactionAmf_StatutOrdreOuTransactionAmfId");

                entity.HasOne(d => d.SensDemande)
                    .WithMany(p => p.DossierOrdreTransactionAmfs)
                    .HasForeignKey(d => d.SensDemandeId)
                    .HasConstraintName("FK_DossierOrdreTransactionAmf_SensDemande_SensDemandeId");
            });

            modelBuilder.Entity<SousCategorieAvis>(entity =>
            {
                entity.HasOne(d => d.CategorieAvis)
                    .WithMany(p => p.SousCategorieAviss)
                    .HasForeignKey(d => d.CategorieId);
            });

            modelBuilder.Entity<CategorieAvis>(entity =>
            {
                entity.HasOne(d => d.Domaine)
                    .WithMany(p => p.CategorieAviss)
                    .HasForeignKey(d => d.DomaineId);
            });

            modelBuilder.Entity<DomaineAvis>(entity =>
            {
                entity.HasOne(d => d.TypologieDemandeAvis)
                    .WithMany(p => p.DomaineAviss)
                    .HasForeignKey(d => d.TypologieDemandeId);

                entity.HasOne(d => d.Activite)
                    .WithMany(p => p.DomaineAviss)
                    .HasForeignKey(d => d.ActiviteId);
            });

            modelBuilder.Entity<DossierAvisIm>(entity =>
            {
                entity.Property(e => e.CommentaireDdc)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireEntiteLocale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireFlge)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CommentaireSuperviseur)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.ComplementInformation)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<AnalyseDossierAvisIm>(entity =>
            {
                entity.HasIndex(e => e.AnalysteId);
                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.DossierAvisImId);

                entity.HasOne(d => d.Analyste)
                    .WithMany(p => p.AnalyseDossierAvisIms)
                    .HasForeignKey(d => d.AnalysteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnalyseDossierSAvis_Analyste_AnalysteId");

                entity.HasOne(d => d.Modificateur)
                    .WithMany(p => p.AnalyseDossierAvisImModificateurs)
                    .HasForeignKey(d => d.ModificateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AnalyseDossierIm_Modificateur_ModificateurId");

                entity.HasOne(d => d.DossierAvisIm)
                    .WithMany(p => p.AnalyseDossierAvisIms)
                    .HasForeignKey(d => d.DossierAvisImId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.Analyse)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<ConclusionAvisIm>(entity =>
            {
                entity.Property(e => e.Conclusion)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<LienUs>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<DossierDegelPartielGda>(entity =>
            {
                entity.HasOne(d => d.DossierGda)
                    .WithMany(p => p.DossierDegelPartielGdas)
                    .HasForeignKey(d => d.DossierGdaId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ApplicationAquisition>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });
            modelBuilder.Entity<CategorieGDR>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });
            modelBuilder.Entity<CapaciteJuridique>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });
            modelBuilder.Entity<CinematiqueGDR>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });
            modelBuilder.Entity<GDRFluxVirement>(entity =>
            {
                entity.HasIndex(e => e.PaysBanqueCreancierId);

                entity.HasIndex(e => e.ApplicationAquisitionId);

                entity.Property(e => e.DeviseOperationId);
                entity.HasOne(d => d.Pays)
                    .WithMany(p => p.GDRFluxVirements)
                    .HasForeignKey(d => d.PaysBanqueCreancierId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ApplicationAquisition)
                    .WithMany(p => p.GDRFluxVirements)
                    .HasForeignKey(d => d.ApplicationAquisitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Devise)
                    .WithMany(p => p.GDRFluxVirements)
                    .HasForeignKey(d => d.DeviseOperationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            OnModelCreatingPartial(modelBuilder);

            modelBuilder.Entity<FaqAvis>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.ReponseFr)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));

                entity.Property(e => e.QuestionFr)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.ReponseEn)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.QuestionEn)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });


            #region SuiteGda

            modelBuilder.Entity<AutorisationPpfGda>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<CanalBdfGda>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<CategorieCanalBdfGda>(entity =>
            {
                entity.HasIndex(e => e.CanalBdfId);

                entity.HasIndex(e => e.CategorieId);
            });

            modelBuilder.Entity<ReferentielImmediateActionsGda>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<CategorieGda>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<CategorieModeOperatoireGda>(entity =>
            {
                entity.HasIndex(e => e.CategorieId);

                entity.HasIndex(e => e.ModeOperatoireId);
            });

            modelBuilder.Entity<CategorieMotifRejetChequeGda>(entity =>
            {
                entity.HasIndex(e => e.CategorieId);

                entity.HasIndex(e => e.MotifRejetChequeId);
            });


            modelBuilder.Entity<CategorieTypeCollecteChequeGda>(entity =>
            {
                entity.HasIndex(e => e.CategorieId);

                entity.HasIndex(e => e.TypeCollecteChequeId);
            });

            modelBuilder.Entity<CategorieTypePaiementGda>(entity =>
            {
                entity.HasIndex(e => e.CategorieId);

                entity.HasIndex(e => e.TypePaiementId);
            });

            modelBuilder.Entity<CategorieTypologieBdfGda>(entity =>
            {
                entity.HasIndex(e => e.CategorieId);

                entity.HasOne(d => d.CategorieGdas)
                    .WithMany(p => p.CategorieTypologieBdfGdas)
                    .HasForeignKey(d => d.CategorieId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ComplementMotifRejetChequeGda>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.MotifRejetChequeId);

                entity.Property(e => e.IsActive);

                entity.HasOne(d => d.MotifRejetChequeGda)
                    .WithMany(p => p.ComplementMotifRejetChequeGdas)
                    .HasForeignKey(d => d.MotifRejetChequeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder
                .Entity<DemandeInformationGda>(entity =>
                {
                    entity.Property(e => e.Titre)
                        .Metadata
                        .SetValueComparer(
                            new ValueComparer<byte[]>(
                                (c1, c2) => c1.SequenceEqual(c2),
                                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                c => c.ToArray()));
                    entity.Property(e => e.Retour)
                        .Metadata
                        .SetValueComparer(
                            new ValueComparer<byte[]>(
                                (c1, c2) => c1.SequenceEqual(c2),
                                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                c => c.ToArray()));
                    entity.Property(e => e.Demande)
                        .Metadata
                        .SetValueComparer(
                            new ValueComparer<byte[]>(
                                (c1, c2) => c1.SequenceEqual(c2),
                                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                c => c.ToArray()));
                });

            modelBuilder.Entity<DocumentDossierGda>(entity =>
            {
                entity.HasIndex(e => e.DocumentGdaId);

                entity.HasIndex(e => e.DossierGdaId);

                entity.HasOne(d => d.DocumentGda)
                    .WithMany(p => p.DocumentsDossierGda)
                    .HasForeignKey(d => d.DocumentGdaId);
            });

            modelBuilder
                .Entity<DocumentGda>()
                .Property(e => e.FileContent)
                .Metadata
                .SetValueComparer(
                    new ValueComparer<byte[]>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToArray()));

            modelBuilder.Entity<DossierGda>(entity =>
            {
                entity.HasIndex(e => e.CategorieId);

                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.RegimeSanctionId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.PaysId);

                entity.HasIndex(e => e.StatutDossierId);

                entity.HasIndex(e => e.TypologieId);

                entity.HasIndex(e => e.UtilisateurId);

                entity.HasOne(d => d.CategorieGdas)
                    .WithMany(p => p.DossierGdas)
                    .HasForeignKey(d => d.CategorieId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.DossierGdas)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DirectionCollaborateurEscalade)
                    .WithMany(p => p.DossierGdasCollaborateur)
                    .HasForeignKey(d => d.DirectionColloaborateurEscaladeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);


                entity.HasOne(d => d.RegimeSanctionGda)
                    .WithMany(p => p.DossierGdas)
                    .HasForeignKey(d => d.RegimeSanctionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DeviseTransaction)
                    .WithMany(p => p.DossierGdas)
                    .HasForeignKey(d => d.DeviseTransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);


                entity.HasOne(d => d.StatutDossierGda)
                    .WithMany(p => p.DossierGdas)
                    .HasForeignKey(d => d.StatutDossierId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.DossierGdaUtilisateurs)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.Notes)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<DossierGdaImmediateAction>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.DossierGdaId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.DossierGdaImmediateActionCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.StatutImmediateActionsGda)
                    .WithMany(p => p.DossierGdaImmediateActionStatuts)
                    .HasForeignKey(d => d.StatutActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<DossierGdaCurrentAction>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.DossierGdaId);

                entity.HasIndex(e => e.ModificateurId);
                entity.HasOne(d => d.StatutActionsGda)
                    .WithMany(p => p.DossierGdaCurrentActionStatuts)
                    .HasForeignKey(d => d.StatutActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(d => d.ReferentielActionHorsCompte)
                    .WithMany(p => p.DossierGdaCurrentActionActions)
                    .HasForeignKey(d => d.ActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.DossierGdaCurrentActionCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                entity.Property(e => e.Description)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
            });

            modelBuilder.Entity<DossierGdaOsc>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.DossierGdaId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasOne(d => d.Createur)
                    .WithMany(p => p.DossierGdaOscCreateurs)
                    .HasForeignKey(d => d.CreateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.StatutActionsGda)
                    .WithMany(p => p.DossierGdaOscStatuts)
                    .HasForeignKey(d => d.StatutOscId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.DebitCreditOsc)
                    .WithMany(p => p.DossierGdaDebitCredits)
                    .HasForeignKey(d => d.DebitCreditId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<DossierGdaPersonneMorale>(entity =>
            {
                entity.HasIndex(e => e.DossierGdaId);

                entity.HasIndex(e => e.MotifExemptionNotificationId);

                entity.HasIndex(e => e.PersonneMoraleGdaId);
            });

            modelBuilder.Entity<DossierGdaPersonnePhysique>(entity =>
            {
                entity.HasIndex(e => e.DossierGdaId);

                entity.HasIndex(e => e.PersonnePhysiqueGdaId);
            });

            //modelBuilder.Entity<DossierGdaResult>(entity =>
            //{
            //    entity.HasNoKey();

            //    entity.ToView("DossierGdaResult", "Gda");
            //});

            modelBuilder.Entity<EtablissementDeclarantGda>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });


            modelBuilder.Entity<RegimeSanctionGda>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<ModeOperatoireGda>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<MotifExemptionNotificationGda>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<MotifRejetChequeGda>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<OrigineDirectionGda>(entity =>
            {
                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.OrigineId);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.OrigineDirectionGdas)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Origine)
                    .WithMany(p => p.OrigineDirectionGdas)
                    .HasForeignKey(d => d.OrigineId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<OrigineGda>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<PersonneMoraleGda>(entity =>
            {
                entity.HasIndex(e => e.CanalEntreeEnRelationId);

                entity.HasIndex(e => e.FormeJuridiqueId);

                entity.HasIndex(e => e.PaysId);

                entity.HasIndex(e => e.RoleClientId);

                entity.HasIndex(e => e.SecteurProfessionnelId);

                entity.HasIndex(e => e.TypeClientId);

                entity.Property(e => e.Ville)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.SiteInternet)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Sigle)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.IdentifiantTvaUe)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Dirigeant)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CodePostale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Adresse)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));


                entity.Property(e => e.SoldeRessourcesEntree)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.SoldeRessourcesEnCours)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.SoldeFondsEntree)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.SoldeFondsEnCours)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.SoldeAvoirsEntree)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.SoldeAvoirsEnCours)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<PersonnePhysiqueGda>(entity =>
            {
                entity.HasIndex(e => e.AutreNationaliteId);

                entity.HasIndex(e => e.CanalEntreeEnRelationId);

                entity.HasIndex(e => e.CiviliteId);

                entity.HasIndex(e => e.NationaliteId);

                entity.HasIndex(e => e.PaysNaissanceId);

                entity.HasIndex(e => e.RelationClientId);

                entity.HasIndex(e => e.RoleClientId);

                entity.HasIndex(e => e.SecteurProfessionnelId);

                entity.HasIndex(e => e.TypeClientId);

                entity.HasOne(d => d.Civilite)
                    .WithMany(p => p.PersonnePhysiqueGdas)
                    .HasForeignKey(d => d.CiviliteId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.Ville)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.LieuNaissance)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.CodePostale)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));
                entity.Property(e => e.Adresse)
                    .Metadata
                    .SetValueComparer(
                        new ValueComparer<byte[]>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToArray()));


                entity.Property(e => e.SoldeRessourcesEntree)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.SoldeRessourcesEnCours)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.SoldeFondsEntree)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.SoldeFondsEnCours)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.SoldeAvoirsEntree)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.SoldeAvoirsEnCours)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<PersonneMoraleResultGda>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("PersonneMoraleResultGda", "Gda");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<PersonnePhysiqueResultGda>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("PersonnePhysiqueResultGda", "Gda");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<ReferentielActionHorsCompte>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<SecteurGda>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.DirectionId).HasDatabaseName("IX_Secteur_Direction_DirectionId");

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.SecteurGdas)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<StatutDossierGda>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<StatutPersonneGda>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<RegimeJuridiqueGda>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<TypeCollecteChequeGda>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<TypePaiementGda>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<TypologieGda>(entity =>
            {
                entity.HasIndex(e => e.CreateurId);

                entity.HasIndex(e => e.ModificateurId);

                entity.Property(e => e.IsActive);
            });

            modelBuilder.Entity<DossierGdaHisto>(entity =>
            {
                entity.HasIndex(e => e.CategorieId);

                entity.HasIndex(e => e.DossierGdaId);

                entity.HasIndex(e => e.DirectionId);

                entity.HasIndex(e => e.ModificateurId);

                entity.HasIndex(e => e.PaysId);

                entity.HasIndex(e => e.StatutDossierId);

                entity.HasIndex(e => e.UtilisateurId);


                entity.HasOne(d => d.CategorieGdas)
                    .WithMany(p => p.DossierGdaHistos)
                    .HasForeignKey(d => d.CategorieId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.DossierGdaHistos)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);


                entity.HasOne(d => d.StatutDossierGda)
                    .WithMany(p => p.DossierGdaHistos)
                    .HasForeignKey(d => d.StatutDossierId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Utilisateur)
                    .WithMany(p => p.DossierGdaHistoUtilisateurs)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TypologieAvoirPersonneGeleePersonnePhysique>(entity =>
            {
                entity.HasIndex(e => e.PersonnePhysiqueGdaId);

                entity.HasIndex(e => e.PaysId);
                entity.HasIndex(e => e.TypeActifId);
                entity.HasIndex(e => e.DeviseId);
            });
            modelBuilder.Entity<FondsInvestissementPersonnePhysique>(entity =>
            {
                entity.HasIndex(e => e.PersonnePhysiqueGdaId);
            });
            modelBuilder.Entity<TypologieAvoirPersonneLieePersonnePhysique>(entity =>
            {
                entity.HasIndex(e => e.PersonnePhysiqueGdaId);

                entity.HasIndex(e => e.TypologieActifId);
            });

            #endregion
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        [DbFunction(Name = "SoundEx")]
        public static string Soundex(string input, Language lang)
        {
            var result = new StringBuilder();

            if (!string.IsNullOrEmpty(input))
            {
                var previousCode = "";
                result.Append(input[0]); // keep initial char

                for (var i = 0; i < input.Length; i++) //start at 0 in order to correctly encode "Pf..."
                {
                    var currentLetter = input[i].ToString().ToLower();
                    var currentCode = "";
                    if (lang == Language.English)
                    {
                        if ("bfpv".Contains(currentLetter))
                            currentCode = "1";
                        else if ("cgjkqsxz".Contains(currentLetter))
                            currentCode = "2";
                        else if ("dt".Contains(currentLetter))
                            currentCode = "3";
                        else if (currentLetter == "l")
                            currentCode = "4";
                        else if ("mn".Contains(currentLetter))
                            currentCode = "5";
                        else if (currentLetter == "r")
                            currentCode = "6";
                    }
                    else
                    {
                        if ("bp".Contains(currentLetter))
                            currentCode = "1";
                        else if ("ckq".Contains(currentLetter))
                            currentCode = "2";
                        else if ("dt".Contains(currentLetter))
                            currentCode = "3";
                        else if (currentLetter == "l")
                            currentCode = "4";
                        else if ("mn".Contains(currentLetter))
                            currentCode = "5";
                        else if (currentLetter == "r")
                            currentCode = "6";
                        else if ("gj".Contains(currentLetter))
                            currentCode = "7";
                        else if ("xzs".Contains(currentLetter))
                            currentCode = "8";
                        else if ("fv".Contains(currentLetter))
                            currentCode = "9";
                    }

                    if (currentCode != previousCode && i > 0) // do not add first code to result string
                        result.Append(currentCode);

                    if (result.Length == 4) break;

                    previousCode = currentCode; // always retain previous code, even empty
                }
            }

            if (result.Length < 4)
                result.Append(new string('0', 4 - result.Length));

            return result.ToString().ToUpper();
        }
    }
}
