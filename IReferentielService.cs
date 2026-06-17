using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BacaratWeb.Core.Services;
using BacaratWeb.Entities.Amf;
using BacaratWeb.Entities.AvisIm;
using BacaratWeb.Entities.AvisSi;
using BacaratWeb.Entities.Commun;
using BacaratWeb.Entities.CommunAvis;
using BacaratWeb.Entities.Escalade;
using BacaratWeb.Entities.Fraude;
using BacaratWeb.Entities.Gda;
using BacaratWeb.Entities.Lab;
using BacaratWeb.Models.Referentials;
using BacaratWeb.Services.Commun.Services.Interfaces;
using BacaratWeb.Shared;

namespace BacaratWeb.Services.Commun.Interfaces
{
    public interface IReferentielService
    {
        IQueryable<Activite> GetActiviteByUser(IUserInfoService userInfoService);

        IEnumerable<Activite> GetActiviteHabilitesUser(IUserInfoService userInfoService);
        Task<IEnumerable<Pays>> GetPaysByDirectionAsync(CancellationToken token = default);
        Task<IEnumerable<Activite>> GetActiviteUtilisateur(string aspNetUserId, CancellationToken token);

        IQueryable<Activite> GetActivites();
        Activite GetActiviteById(int id);
        IQueryable<Activite> GetActivitesRattachement();
        Task<IEnumerable<TypeDate>> GetTypesDate(IEnumerable<TypeDateEnum> codes);
        IEnumerable<TypeException> GetTypeExceptions();
        IQueryable<AspNetRoleClaim> GetAspNetRoleClaims(string roleId);

        IQueryable<AspNetRole> GetAspNetRoles();

        AspNetUser GetAspNetUser(IUserInfoService userInfoService);
        AspNetUser GetAspNetUser(string userName);

        IQueryable<AspNetUser> GetAspNetUsers();

        IQueryable<AutorisationPpf> GetAutorisationPpfs();
        IQueryable<BamCeiling> GetBamCeilings();
        IQueryable<StatutActionsGda> GetStatutActionsGdas();
        IQueryable<StatutImmediateActionsGda> GetStatutImmediateActionsGdas();
        IQueryable<StatutOscGda> GetStatutOscGdas();

        IQueryable<DebitCreditOsc> GetDebitCreditOscs();


        IQueryable<TypesLiens> GetTypesLiens();
        IQueryable<TypologiesActifs> GetTypologiesActifs(); 
        IQueryable<TypesActifs> GetTypesActifs();
        IQueryable<CcBlocked> GetCcBlocked();
        IQueryable<SafetyInstructions> GetSafetyInstructions();
        IQueryable<AvisLab> GetAvisLabs();
        IQueryable<NatureSoupconFraudeFiscale> GetNatureSoupconFraudeFiscalesLab();
        IQueryable<NatureInfractionPenale> GetNatureInfractionPenalesLab();

        IQueryable<VisaLab> GetVisaLabs();

        IQueryable<TypeLegislationLab> GetTypeLegislationLabs();

        IQueryable<CanalEntreeEnRelation> GetCanalsEntreeEnRelation();

        IQueryable<CategorieCanalBdf> GetCategorieCanalBdf();

        IQueryable<CategorieEtablissementDeclarant> GetCategorieEtablissementDeclarant();
        IQueryable<ReferentielImmediateActionsGda> GetReferentielImmediateActionsGda();

        IQueryable<CategorieFraude> GetCategorieFraudes();
        IQueryable<CategorieGDR> GetCategorieGDRs();
        IQueryable<ApplicationAquisition> GetApplicationAquisitions();
        IQueryable<CinematiqueGDR> GetCinematiqueGDRs();
        IQueryable<CapaciteJuridique> GetCapaciteJuridiques();
        IQueryable<CategorieGda> GetCategorieGdas();

        IQueryable<CategorieLab> GetCategorieLabs();

        IQueryable<CategorieGroupeLab> GetCategorieGroupeLabs();

        IQueryable<OrigineGroupeLab> GetOrigineGroupeLabs();

        IQueryable<CategorieTracfin> GetCategorieTracfins();

        IQueryable<GroupeCategorieLab> GetGroupeCategorieLabs();

        IQueryable<GroupeOrigineLab> GetGroupeOrigineLabs();
		IQueryable<NatureDs> GetNaturesDs();
		IQueryable<CategorieContributeur> GetCategoriesContributeurs();
        IQueryable<Contributeur> GetContributeurs();
        IQueryable<FonctionLab> GetFonctionsLabs();
        IQueryable<InformationRequestCloseReason> GetInformationRequestCloseReasons();
        IQueryable<CategorieModeOperatoire> GetCategorieModeOperatoire();

        IQueryable<CategorieMotifRejetCheque> GetCategorieMotifRejetCheque();

        IQueryable<CategorieTypeCollecteCheque> GetCategorieTypeCollecteCheque();

        IQueryable<CategorieTypeCollecteCheque> GetCategorieTypeCollecteCheques();

        IQueryable<CategorieTypePaiement> GetCategorieTypePaiements();

        IQueryable<CategorieTypologieBdf> GetCategorieTypologieBdf();

        IQueryable<TypologieGda> GetTypologieGda();
        IQueryable<TypeDegel> GetTypeDegel();

        IQueryable<Civilite> GetCivilites();

        IQueryable<ComplementMotifRejetCheque> GetComplementMotifRejetCheques();

        IQueryable<ComplementVoie> GetComplementVoies();

        Utilisateur GetCurrentUser(string userId);

        IQueryable<Devise> GetDevises();

        IQueryable<Bunit> GetBunits();

        IQueryable<DirectionAccessible> GetDirectionAccessibles(int activiteId, int directionId);

        IEnumerable<Direction> GetDirectionsAccessiblesByActivite(int activiteId, int utilisateurId);

        Task<Direction> GetDirectionWithUserId(int activiteId, int utilisateurId,
            CancellationToken token = default);

        Task<IEnumerable<Direction>> GetDirectionWithAccessibles(int activiteId, int utilisateurId, CancellationToken token = default);
        IQueryable<DirectionAccessible> GetDirectionPouvantAccedes(int activiteId, int directionId);

        Task<IEnumerable<Direction>> GetDirectionAccessibleWithAccessDs(int activiteId, int utilisateurId, CancellationToken token = default);

        IQueryable<Direction> GetDirectionByActivite(int activiteId, IUserInfoService userInfoService);

        IQueryable<Direction> GetDirectionHabiliteByUtilisateurId(int utilisateurId);

        IEnumerable<Direction> GetDirectionHabiliteUserLecture(int activiteId, IUserInfoService userInfoService);
        IEnumerable<Direction> GetDirectionHabiliteUserBlocked(int activiteId, IUserInfoService userInfoService);
        IEnumerable<Direction> GetDirectionsFlge(int activiteId, IUserInfoService userInfoService);
        IEnumerable<Direction> GetDirectionHabiliteUserEcriture(int activiteId, IUserInfoService userInfoService);
        IEnumerable<Direction> GetDirectionHabiliteUserReferentiel(int activiteId, IUserInfoService userInfoService);
        IEnumerable<Direction> GetDirectionFlgeHabiliteUserEcriture(int activiteId, IUserInfoService userInfoService);

        IEnumerable<Direction> GetDirectionHabiliteUser(int activiteId, IUserInfoService userInfoService);
        IEnumerable<Direction> GetDirectionHabilites(int activiteId, int utilisateurId);
        IQueryable<Direction> GetDirectionActiviteAspNetUserRoleSearch(int activiteId,
                                                                        int roleUtilisateurId,
                                                                        IUserInfoService userInfoService);
        IEnumerable<Direction> GetDirectionHabiliteConfidents(int activiteId, int utilisateurId);
        IQueryable<Direction> GetDirectionActiviteAspNetUserRoleDelegation(int activiteId,
                                                                            int roleUtilisateurId,
                                                                            IUserInfoService userInfoService);

        IQueryable<Direction> GetDirectionParentAndEntites(int activiteId, int utilisateurId);

        Task<IEnumerable<Direction>> GetEntiteLocalByDirection(int directionId, CancellationToken token = default);


        IQueryable<Direction> GetDirectionPorteurs(int activiteId, int utilisateurId);

        Task<Direction> GetSpecificDirection(int activiteId,
                                             int roleUtilisateurId,
                                             int utilisateurId,
                                             CancellationToken token = default);

        IQueryable<Direction> GetDirectionFlgeUtilisateur(IUserInfoService userInfoService, int activiteId);

        IQueryable<Direction> GetDirectionUserAdmin(int activiteId, IUserInfoService userInfoService);

        IQueryable<Activite> GetActiviteUtilisateurByDirection(int directionId, int utilisateurId);

        Task<IEnumerable<Direction>> GetDirectionUtilisateurActivite(int activiteId,
                                                                     int utilisateurId,
                                                                     CancellationToken token = default);


        Task<IEnumerable<Direction>> GetDirectionFlgeParents(int activiteId, int utilisateurId, CancellationToken token);

        Task<IEnumerable<Direction>> GetDirectionEntiteLocales(int activiteId,
                                                               int utilisateurId,
                                                               CancellationToken token = default);
        Task<IEnumerable<EntiteLab>> GetEntitesLabByDirectionAsync(int directionId, bool isActive, CancellationToken token = default);

        Task<IEnumerable<CategorieLab>> GetCategorieLabsByDirectionAsync(int directionId,  bool isActive, bool isSanction, bool isReadOnly, CancellationToken token);

        Task<IEnumerable<TypeClientLab>> GetTypeClientLabsByDirectionAsync(int directionId, bool isActive, CancellationToken token);

        Task<IEnumerable<SecteurEconomiqueLab>> GetSecteurEconomiqueByDirectionAsync(int directionId, bool isActive, CancellationToken token);

        IQueryable<Direction> GetDirections();
        IQueryable<Direction> GetDdcDirections();
        IEnumerable<Direction> GetDirectionsActive();
        IQueryable<Direction> GetDirectionsAccessibles(IEnumerable<int> directions);

        IQueryable<DossierLabAction> GetDossierLabActions();
        IQueryable<DroitCompte> GetDroitComptes();

        EntiteFraude GetEntiteById(int id);
        EntiteGda GetEntiteGdaById(int id);
        Direction GetDirectionById(int id);
        IEnumerable<Direction> GetDepartmentsByIdList(IList<int> idList);
        IQueryable<EntiteFraude> GetEntiteFraudeByUserId(IUserInfoService userInfoService);

        IQueryable<EntiteFraude> GetEntitieByDirection(int directionId, bool isInclude = false);

        IQueryable<EntiteFraude> GetEntities();

        IQueryable<EntiteGda> GetEntiteGdaByUserId(IUserInfoService userInfoService);

        IQueryable<EntiteGda> GetEntitieGdaByDirection(int directionId, bool isInclude = false);

        IQueryable<EntiteGda> GetEntitieGdas();

        EntiteLab GetEntiteLabById(int id);

        IQueryable<EntiteLab> GetEntiteLabByUserId(IUserInfoService userInfoService);

        IQueryable<EntiteLab> GetEntiteLabByDirection(int directionId, bool isInclude = false);

        IQueryable<EntiteLab> GetEntiteLabs();

        IQueryable<EtatFraude> GetEtatFraude();

        IQueryable<FormeJuridique> GetFormeJuridiques();
        IQueryable<IdentificationProfessionnelle> GetIdentificationProfessionnelles();
        
        IQueryable<HistoriqueConnexion> GetHistoriqueConnexionByUserId(IUserInfoService userInfoService);

        IQueryable<HistoriqueConnexion> GetHistoriqueConnexions();

        IQueryable<Impact> GetImpacts();
        IQueryable<RegimeSanctionGda> GetRegimeSanctionGdas();

        IQueryable<Langue> GetLangues();

        IQueryable<MotifExemptionNotification> GetMotifExemptionNotifications();
        IQueryable<MotifExemptionNotification> GetMotifExemptionNotificationsFraude();
        IQueryable<MotifExemptionNotificationGda> GetMotifExemptionNotificationGdas();

        IQueryable<TypeLienSupport> GetTypeLienSupports();
        IQueryable<TypeReferenceLab> GetTypeReferenceLabs();

        IQueryable<OrigineFraude> GetOrigineFraudes();
        IQueryable<OrigineGda> GetOrigineGdas();

        IQueryable<OrigineLab> GetOrigineLabs();
        IQueryable<OrigineLab> GetOrigineLabUpdateForms();

        IQueryable<OrganismeLab> GetOrganismeLabs();
        
        IQueryable<Pays> GetPays();

        IQueryable<Pays> GetAllPays();

        IQueryable<PrincipalInstrumentFinancier> GetPrincipalInstrumentFinanciers();

        IQueryable<Produit> GetProduits(); 
        IQueryable<VoixRecouvrement> GetVoixRecouvrements();
        IQueryable<ReferentielActionHorsCompte> GetReferentielActionHorsComptes();

        IQueryable<Profession> GetProfessions();

        IQueryable<RoleClient> GetRolesClient();

        IQueryable<ScenarioNorkom> GetScenarioNorkoms();

        IQueryable<ScenarioLab> GetScenarioLabs();
        IQueryable<ApplicationScenarioLab> GetApplicationScenarioLabs();

        IQueryable<SecteurFraude> GetSecteurByDirection(int directionId);

        SecteurFraude GetSecteurById(int id);

        IQueryable<SecteurGda> GetSecteurGdaByDirection(int directionId);

        SecteurGda GetSecteurGdaById(int id);

        IQueryable<SecteurProfessionnel> GetSecteurProfessionnels();

        IQueryable<SecteurProfessionnel> GetSecteurProfessionnelAmfs();

        IQueryable<SecteurFraude> GetSecteurs();
        IQueryable<SecteurGda> GetSecteurGdas();

        IQueryable<SecteurLab> GetSecteurLabByDirection(int directionId);

        SecteurLab GetSecteurLabById(int id);

        IQueryable<SecteurLab> GetSecteurLabs();

        IQueryable<SecteurEconomiqueLab> GetSecteurEconomiqueLabs();

        IQueryable<Sexe> GetSexes();

        IQueryable<SituationFamiliale> GetSituationFamiliales();

        IQueryable<StatutDossierFraude> GetStatutDossierFraudes();
        IQueryable<StatutDossierGda> GetStatutDossierGdas();

        IQueryable<StatutDossierLab> GetStatutDossierLabs();
        IQueryable<StatutDemandeInformationLab> GetStatutDemandeInformationLabs();
        IQueryable<TypeGarantie> GetTypesGaranties();
        IQueryable<TypeOperation> GetTypesOperations();
        IQueryable<CriteresAlerteOrigine> GetCriteresAlerteOrigines();
        IQueryable<ModaliteFinancement> GetModaliteFinancements();
        IQueryable<StatutDemandeInformation> GetStatutDemandeInformationEscalades();
        
        IQueryable<StatutDemandeInformationAvisSi> GetStatutDemandeInformationAvisSis();
        
        IQueryable<StatutDemandeInformationAvis> GetStatutDemandeInformationAviss();
        
        IQueryable<StatutDemandeInformationFraude> GetStatutDemandeInformationFraudes();
        IQueryable<StatutDemandeInformationGda> GetStatutDemandeInformationGdas();

        IQueryable<StatutOperation> GetStatutOperations();
        
        IQueryable<StatutPersonneFraude> GetStatutPersonneFraude();
        IQueryable<StatutPersonneGda> GetStatutPersonneGda();


        IQueryable<TypeAdresse> GetTypeAdresses();
        IQueryable<TypeFonctionDirigeant> GetTypeFonctionDirigeants();
        
        IQueryable<TypeContrat> GetTypeContrats();

        IQueryable<CategorieDocument> GetCategorieDocuments();
        IQueryable<TypeDocumentLab> GetTypeDocumentLabs();

        IQueryable<TypeImplicationLab> GetTypeImplicationLabs(bool isActive= true);
        IQueryable<ProfessionalIdentificationLab> GetProfessionalIdentificationLabs(bool isActive= true);
        IQueryable<TypeClientFraude> GetTypeClientFraudes();
        IQueryable<ScenarioFraude> GetScenarioFraudes();
        IQueryable<RegimeJuridiqueGda> GetRegimeJuridiqueGdas();

        IQueryable<TypeClientLab> GetTypeClientLabs();
        IQueryable<TypeClientLab> GetTypeClientLabs(int directionId);
        IQueryable<TypeRelationAffaireLab> GetTypeRelationAffaireLabs();
        IQueryable<TypeRelationAffaireFraude> GetTypeRelationAffaireFraudes();
        IQueryable<TypeRelationAffaireGda> GetTypeRelationAffaireGdas();

        IQueryable<TypeCompte> GetTypeComptes();

        IQueryable<TypeDeclarationTracfin> GetTypeDeclarationTracFins();

        Task<IEnumerable<Utilisateur>> GetUtilisateurDelegueByActivite(int activiteId, int utilidsateurId, CancellationToken token = default);

        Task<IEnumerable<Utilisateur>> GetUtilisateursAssociated(int activiteId, int utilidsateurId, CancellationToken token = default);

        IEnumerable<Utilisateur> GetAdminFLGEByDirection(int directionId, int activiteId);

        IQueryable<TypeDocument> GetTypeDocuments();

        IQueryable<TypeLienPersonneMoraleMorale> GetTypeLienPersonneMoraleMorales();

        IQueryable<TypeLienPersonnePhysiquePhysique> GetTypeLienPersonnePhysiquePhysiques();
        IQueryable<CategorieLienLab> GetCategorieLienLab();

        IQueryable<TypeLienPersonneMoralePhysique> GetTypeLienPersonneMoralePhysiques();

        IQueryable<TypeLienPersonnePhysiqueMorale> GetTypeLienPersonnePhysiqueMorales();

        IQueryable<InteretDroitVote> GetInteretDroitVotesCollection();

        IQueryable<EntiteGroupeType> GetEntiteGroupeTypesCollection();

        IQueryable<TypeFond> GetTypeFondsCollection();

        IQueryable<TypePieceIdentite> GetTypePieceIdentites();

        IQueryable<TypeVoie> GetTypeVoies();

        IQueryable<TypeListeCriblage> GetTypeListeCriblages();

        IQueryable<UtilisateurDirection> GetUtilisateurDirectionByUtilisateurId(int utilisateurId);

        IQueryable<UtilisateurDirection> GetUtilisateurDirections();
        IQueryable<Utilisateur> GetUtilisateurHabilitesUser(IUserInfoService userInfoService);

        IQueryable<Utilisateur> GetUtilisateurs();
        
        Task<IEnumerable<Utilisateur>> GetUtilisateursAsync(CancellationToken token = default);

        IEnumerable<Utilisateur> GetUtilisateurConnexionHistos(int directionId, int activiteId);

        Task<IEnumerable<Utilisateur>> GetUtilisateurInDirectionAsync(int directionId,
                                                                      int activiteId,
                                                                      CancellationToken token);
        Task<IEnumerable<Utilisateur>> GetUtilisateurInDirectionSpecRoleAsync(int directionId,
                                                                 int activiteId, int roleId
                                                                 , CancellationToken token);


        Task<IEnumerable<Utilisateur>> GetAllUtilisateurLinkedwithParentdirection(int activiteId,
                                                                                  int utilisateurId,
                                                                                  List<string> roleIds,
                                                                                  CancellationToken token);

        Task<IEnumerable<Direction>> GetDirectionLinkedwithParentdirection(int activiteId,
            int utilisateurId,
            CancellationToken token);

        Task<IEnumerable<Utilisateur>> GetUtilisateurInDdcAsync(int activiteId,int directionId, CancellationToken token);
        Task<IEnumerable<Utilisateur>> GetUtilisateurValideurInDdcAsync(bool isValidateurFlge, int directionId, int activiteId, CancellationToken token);

        Task<IEnumerable<Utilisateur>> GetUtilisateurWithoutSpecificUserAsync(int utilisateurId,
                                                                              int directionId,
                                                                              int activiteId,
                                                                              CancellationToken token);

        Task<IEnumerable<Utilisateur>> GetUtilisateursConcernesAsync(int utilisateurId,
                                                                     int directionId,
                                                                     int activiteId,
                                                                     CancellationToken token);

        Task<IEnumerable<Utilisateur>> GetUtilisateurInDirectionAllAsync(int directionId,
            int activiteId,
            CancellationToken token);

        bool IsEntiteUsedDossierFraude(int entiteId);
        bool IsEntiteUsedDossierLab(int entiteId);
        bool IsOrigineUsedDossierFraude(int origineId);
        bool IsOrigineUsedDossierLab(int origineId);
        bool IsCategorieUsedCategorieLab(int CategorieId);

        bool IsCategorieUsedGroupeCategorieLab(int CategorieId);

        bool IsCategorieUsedGroupeOrigineLab(int CategorieId);

        bool IsTypeClientFraudeUsedPersonnePhysiqueFraude(int typeClientId);
        bool IsTypeClientFraudeUsedPersonneMoraleFraude(int typeClientId);
        bool IsRegimeJuridiqueGdaUsedPersonnePhysiqueGda(int typeClientId);
        bool IsRegimeJuridiqueGdaUsedPersonneMoraleGda(int typeClientId);
        bool IsTypeClientLabUsedPersonnePhysiqueLab(int typeClientId);
        bool IsTypeClientLabUsedPersonneMoraleLab(int typeClientId);

        bool IsSecteurUsedEntite(int secteurId);
        bool IsSecteurLabUsedEntiteLab(int secteurId);

        IQueryable<Defaillance> GetDefaillances();
        IQueryable<Couleur> GetCouleurs();
        IQueryable<CategorieAvoir> GetCategoriesAvoir();
        IQueryable<ZoneGeographique> GetZoneGeographiques();
        IQueryable<Domaine> GetDomaines();
        IQueryable<StatutEscalade> GetStatutEscalades();
        IQueryable<StatutAvis> GetStatutAviss();
        IQueryable<StatutAvis> GetStatutAvisIms();
        IQueryable<IntermediaryAgreementNotice> GetIntermediaryAgreementNotices();
        IQueryable<FinalAgreementNotice> GetFinalAgreementNotices();
        IQueryable<StatutAvisSi> GetStatutAvisSis();

        IQueryable<Evenement> GetEvenements();

        IQueryable<Application> GetApplications();

        IQueryable<Environnement> GetEnvironnements();
        //SAvis
        IQueryable<DefaillanceAvis> GetDefaillanceSAviss();
        IQueryable<TypologieDemandeAvis> GetTypologieDemandeSAviss();
        IQueryable<CategorieAvis> GetCategorieSAviss();
        IQueryable<CategorieOutilAvis> GetCategorieOutilSAviss();
        IQueryable<SousCategorieAvis> GetSousCategorieSAviss();
        IQueryable<DomaineAvis> GetDomaineSAviss();
        IQueryable<TypologieClientAvis> GetTypologieClientSAviss();
        IQueryable<ApplicationAvis> GetApplicationSAviss();
        IQueryable<EnvironnementAvis> GetEnvironnementSAviss();

        IQueryable<PrioriteAvis> GetPrioriteSAviss();
        Task<IEnumerable<StatutAvis>> GetStatutDossierSAvisAsync(CancellationToken token = default);
        IQueryable<AppartenanceDocumentAvis> GetAppartenanceDocumentSAviss();
        EquipeAnalyseAvis GetUserEquipeAnalyseSAvis(int utilisateurId);
        IEnumerable<EquipeAnalyseAvis> GetUserEquipeAnalysesSAvis(int utilisateurId);
        IQueryable<ActionTypeDemandeInformationAvis> GetActionTypeDemandeInformationSAviss();
        Task<IEnumerable<TypologieDemandeAvis>> GetTypologieDemandeSAvisAsync(CancellationToken token);
        Task<IEnumerable<ActionTypeDemandeInformationAvis>> GetActionTypeDemandeSAvisAsync(CancellationToken token = default);
        //Task<IEnumerable<Traduction>> GetSAvisTranslations(CancellationToken token = default);
        //Fin

        //AvisIm
        IQueryable<DefaillanceAvis> GetDefaillanceAvisIms();
        IQueryable<TypologieDemandeAvis> GetTypologieDemandeAvisIms();
        IQueryable<CategorieAvis> GetCategorieAvisIms();
        IQueryable<SousCategorieAvis> GetSousCategorieAvisIms();
        IQueryable<DomaineAvis> GetDomaineAvisIms();
        IQueryable<TypologieClientAvis> GetTypologieClientAvisIms();
        IQueryable<ApplicationAvis> GetApplicationAvisIms();
        IQueryable<EnvironnementAvis> GetEnvironnementAvisIms();
        Task<IEnumerable<StatutAvis>> GetStatutDossierAvisImAsync(CancellationToken token = default);
        IQueryable<AppartenanceDocumentAvis> GetAppartenanceDocumentAvisIms();
        EquipeAnalyseAvis GetUserEquipeAnalyseAvisIm(int utilisateurId);
        IEnumerable<EquipeAnalyseAvis> GetUserEquipeAnalysesAvisIm(int utilisateurId);
        IQueryable<ActionTypeDemandeInformationAvis> GetActionTypeDemandeInformationAvisIms();
        Task<IEnumerable<TypologieDemandeAvis>> GetTypologieDemandeAvisImAsync(CancellationToken token);
        Task<IEnumerable<ActionTypeDemandeInformationAvis>> GetActionTypeDemandeAvisImAsync(CancellationToken token = default);
        //Task<IEnumerable<Traduction>> GetAvisImTranslations(CancellationToken token = default);

        IQueryable<LienUs> GetLienUss();
        IQueryable<UsLinkNature> GetUsLinkNatures();
        IQueryable<TypeFormulaire> GetTypeFormulaires();
        IQueryable<TypeFormulaire> GetTypeFormulairesTotus();

        IQueryable<TypeFormulaire> GetTypeFormulairesNotTotus();
        //Fin

        IQueryable<DefaillanceAvisSi> GetDefaillanceAvisSis();
        IQueryable<TypologieDemande> GetTypologieDemandes();

        IQueryable<QualificationDossier> GetQualificationDossiers();
        IQueryable<QualificationDossierEscalade> GetQualificationDossierEscalades();
        IQueryable<DemandeDerogation> GetDemandeDerogations();
        IQueryable<ApplicationAvisSi> GetApplicationAvisSis();
        IQueryable<EnvironnementAvisSi> GetEnvironnementAvisSis();
        IQueryable<DecisionAvisSi> GetDecisionAvisSis();


        Task<IEnumerable<StatutEscalade>> GetStatutDossierEscaladeAsync(CancellationToken token = default);
       
        Task<IEnumerable<StatutAvisSi>> GetStatutDossierAvisSiAsync(CancellationToken token = default);

        IQueryable<AppartenanceDocument> GetAppartenanceDocuments();
        IQueryable<GdaAppartenanceDocument> GetGdaAppartenanceDocuments();
        IQueryable<AppartenanceDocumentAvisSi> GetAppartenanceDocumentAvisSis();

        Task<IEnumerable<Direction>> GetDirectionFlgeAsync(CancellationToken token = default);
        IQueryable<Direction> GetFlgeDepartments();
        Task<IEnumerable<Direction>> GetDirectionAllAsync(CancellationToken token);

        EquipeAnalyse GetUserEquipeAnalyse(int utilisateurId);
        

        EquipeAnalyseAvisSi GetUserEquipeAnalyseAvisSi(int utilisateurId);

        EquipeSuperviseurAvisSi GetUserEquipeSuperviseurAvisSi(int utilisateurId);

        IEnumerable<EquipeAnalyse> GetUserEquipeAnalyses(int utilisateurId);
       

        IEnumerable<EquipeAnalyseAvisSi> GetUserEquipeAnalysesAvisSi(int utilisateurId);

        Task<IEnumerable<Utilisateur>> GetUtilisateurByRoleInDirection(int activity, int entiteId, string role, CancellationToken token = default);

        Task<IEnumerable<Domaine>> GetDomaineAsync(CancellationToken token);
      
        Task<IEnumerable<TypologieDemande>> GetTypologieDemandeAsync(CancellationToken token);

        Task<bool> IsReadonlyUtilisateurAsync(int activiteId, int utilisateur);

        Task<bool> IsReadOnlyUserAsync(int activiteId, int utilisateurId, CancellationToken token = default);
        Task<bool> IsWriteOnlyUserAsync(int activiteId, int utilisateurId, CancellationToken token = default);
        Task<bool> IsIsoleOnlyUserAsync(int activiteId, int utilisateurId, CancellationToken token = default);
        Task<bool> IsEtenduUtilisateurAsync(int activiteId, int utilisateur);
        bool IsEtenduUtilisateur(int activiteId, int utilisateurId);

        IQueryable<UtilisateurDirection> GetUtilisateurDirectionConnexionHistos(int directionId, int activiteId);
        Task<IList<UtilisateurDirection>> GetRoleUtilisateurDirectionByAdmin(int utilisateur, int isAdmin, int active, int? activite = null, int? directionId = null, CancellationToken token = default);
        Task<IList<AspNetRole>> GetAspNetUserRoleByAdminConnexionHistos(int utilisateur, CancellationToken token = default);
        Task<IList<UtilisateurViewModel>> GetUtilisateurByAdminConnexionHistos(IEnumerable<int> adminLocalDirectionIds, IEnumerable<int> adminLocalActiviteIds, bool isGlobalAdmin, int isAdmin, int active,
            bool? isValideur = null, bool? isAdminReferentiel = null, bool? isAuditeur = null);

        IQueryable<ActionTypeDemandeInformation> GetActionTypeDemandeInformations();

        IQueryable<ActionTypeDemandeInformationAvisSi> GetActionTypeDemandeInformationAvisSis();
        Utilisateur GetConnectedUser(string aspUserId);

        IDirectionService Direction { get; set; }
        IActiviteService Activite{ get; set; }

        Task<IEnumerable<ActionTypeDemandeInformation>> GetActionTypeDemandeAsync(CancellationToken token = default);
       
        Task<IEnumerable<ActionTypeDemandeInformationAvisSi>> GetActionTypeDemandeAvisSiAsync(CancellationToken token = default);
        IEnumerable<Utilisateur> GetUtilisateurAdminLocalAsync(int utilisateurId, int activiteId);
        IEnumerable<Utilisateur> GetUtilisateurAdminLocalValidationAsync(int utilisateurId, int activiteId);
        
        Task<Utilisateur> GetUtilisateurById(int id, CancellationToken token = default);


        Task<IEnumerable<Direction>> GetDirectionHabiliteUtilisateur(int utilisateurId, int activiteId, CancellationToken token = default);
        IQueryable<Direction> GetDirectionHabilitionUtilisateur(int utilisateurId, int activiteId);

        IQueryable<Direction> GetDirectionHabilitionAdminReferentiel(int utilisateurId, int activiteId);

        IQueryable<Direction> GetDirectionHabilitions(int utilisateurId);
        IQueryable<Direction> GetDirectionHabilitionsActivite(int utilisateurId, int activiteId);
        Task<IEnumerable<TypeClientFraude>> GetTypeClientFraudesByDirection(int directionId, CancellationToken token = default);
        Task<IEnumerable<RegimeJuridiqueGda>> GetRegimeJuridiqueGdasByDirection(int directionId, CancellationToken token = default);

        Task<IEnumerable<TypeClientLab>> GetTypeClientLabsByDirection(int directionId, CancellationToken token = default);

        Task<IEnumerable<Direction>> GetDirectionByParentId(int parentId, CancellationToken token = default);

        Task<IEnumerable<Direction>> GetDirectionFlgeOnSousEntiteLocals(int activiteId, int utilisateur, CancellationToken token = default);

        Task<IEnumerable<Direction>> GetDirectionFlgeOnEntiteLocals(int activiteId, int utilisateur, CancellationToken token = default);

        Task<IEnumerable<Direction>> GetDirectionFlgesByUser(int activiteId, int utilisateurId, CancellationToken token = default);

        Task<IEnumerable<Direction>> GetDirectionFlges(CancellationToken token = default);

        Task<IEnumerable<Direction>> GetDirectionNotFlges(CancellationToken token = default);

        Task<IEnumerable<Direction>> GetDirectionEntiteLocals(int activiteId, int utilisateur, CancellationToken token = default);
        Task<IEnumerable<Direction>> GetDirectionSousEntiteLocals(int activiteId, int utilisateur, CancellationToken token = default);


        Task<IEnumerable<Direction>> GetAvailableEntiteLocalByDirectionFlge(int activiteId, int utilisateur, int directionFlgeId, CancellationToken token = default);

        Task<IEnumerable<Direction>> GetAvailableSousEntiteLocalByEntiteLocal(int activiteId, int utilisateur, int entiteLocalId, CancellationToken token = default);

        Task<IEnumerable<Utilisateur>> GetUtilisateurResponsableFlges(int activiteId, int directionId, CancellationToken token = default);

        Task<IEnumerable<Direction>> GetDirectionResponsableFlges(int activiteId, int utilisateur, CancellationToken token = default);

        Task<IEnumerable<Direction>> GetDirectionCollaborateurFlges(int activiteId, int utilisateur, CancellationToken token = default);


        Task<IEnumerable<Traduction>> GetCommonTranslations(CancellationToken token = default);

        /// <summary>
        /// Retourne la traduction des templates.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IEnumerable<TraductionTemplate>> GetCommonTranslationTemplates(CancellationToken token = default);
        /// <summary>
        /// Ajout traduction.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="frenchTranslate"></param>
        /// <param name="englisTranslate"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> AddTraduction(string id, string frenchTranslate, string englisTranslate, CancellationToken token = default);

        /// <summary>
        /// Ajout/Edit traduction template.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="frenchTranslate"></param>
        /// <param name="englisTranslate"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> AddEditTraductionTemplate(string id, string frenchTranslate, string englisTranslate, CancellationToken token = default);

        /// <summary>
        /// Edit (objet/body) mail notification template.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="objet"></param>
        /// <param name="body"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> EditMailNotificationTraduction(int id, string objet, string body, CancellationToken token = default);

        /// <summary>
        /// Retourne traduction template par Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<TraductionTemplate> GetTemplateTraductionById(string id, CancellationToken token = default);


        /// <summary>
        /// Génération des requêtes d'insertion traduction.
        /// </summary>
        /// <returns></returns>
        Task<byte[]> GenerationScript(CancellationToken token = default);

        /// <summary>
        /// Une fonction qui retourne un flu byte pour la generation du script d'insertion dans commun.TraductionTemplate.
        /// </summary>
        /// <returns></returns>
        Task<byte[]> GenerationScriptTemplate(CancellationToken token = default);

        /// <summary>
        /// Edit traduction.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="frenchTranslate"></param>
        /// <param name="englisTranslate"></param>
        /// <param name="token"></param>
        Task<bool> EditTraduction(string id, string frenchTranslate, string englisTranslate, CancellationToken token = default);

        /// <summary>
        /// Delete traduction.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        Task<bool> DeleteTraduction(string id, CancellationToken token = default);

        /// <summary>
        /// Retourne email notification par Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<EmailNotificationTraductionTemplateViewModel> GetMailNotificationById(int id, CancellationToken token = default);

        Task<List<EmailNotificationScope>> GetEmailNotificationScopes(CancellationToken token = default);

        /// <summary>
        /// Delete traduction template.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> DeleteTraductionTemplate(string id, CancellationToken token = default);

        Task<IEnumerable<CategorieLab>> GetCategorieLabByDirection(int directionId, CancellationToken token = default);
        Task<IEnumerable<OrigineLab>> GetOrigineLabByDirection(int directionId, CancellationToken token = default);

        Task<bool> IsAccesDossierConfidentialLab(int responsableDossierId, int directionId, int activiteId, CancellationToken token = default);

        Task<Confidentiel> GetConfidentialMessageLab(int directionId, CancellationToken token = default);
        Task<ConfidentielAmf> GetConfidentialMessageAmf(int directionId, CancellationToken token = default);
        Task<IEnumerable<OrigineFraude>> GetOrigineFraudesByDirection(int directionId, CancellationToken token = default);
        Task<IEnumerable<OrigineGda>> GetOrigineGdasByDirection(int directionId, CancellationToken token = default);

        Task<bool> IsOperationLab(int activiteId, int utilisateurId, CancellationToken token = default);

        Task<IEnumerable<UtilisateurDirection>> GetUtilisateurDirections(int utilisateurId, int directionId, int activiteId, CancellationToken tocken = default);

        IQueryable<StatutDossierAmf> GetStatutDossierAmfs();

        Task<bool> IsAccesDossierConfidentialAmf(int responsableDossierId, int directionId, int activiteId, CancellationToken token = default);
        //Task<IEnumerable<Traduction>> GetAmfTranslations(CancellationToken token = default);

        IQueryable<TitreAuquelEntiteAgitAmf> GetTitreAuquelEntiteAgitAmfs();

        IQueryable<TypeActiviteAmf> GetTypeActiviteAmfs();

        IQueryable<TypeInstrumentFinancierAmf> GetTypeInstrumentFinancierAmfs();

        IQueryable<TypeInstrumentUsuellementNegocieAmf> GetTypeInstrumentUsuellementNegocieAmfs();

        IQueryable<TypeProduitDeriveAmf> GetTypeProduitDeriveAmfs();
        IQueryable<SensDemande> GetSensDemandes();
        IQueryable<StatutOrdreOuTransactionAmf> GetStatutOrdreOuTransactionAmfs();
        Task<IEnumerable<UtilisateurDirection>> GetUtilisateurValideurs(int directionId, int activiteId, CancellationToken tocken = default);
        Task<DirectionCoordonnee> GetDirectionCoordonnee(int directionId, CancellationToken token = default);

        IQueryable<StatutUtilisateur> GetStatutUtilisateurs();
        IQueryable<CarnetAdresses> GetCarnetAdresses();

        IQueryable<ModeEnvoieTracfin> GetModeEnvoieTracfin();

        IQueryable<ConfigurationValue> GetConfigurations();

        /// <summary>
        /// Retourne la liste des admins locaux par direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="modulesIds"></param>
        /// <returns></returns>
        IList<UtilisateurViewModel> LoadAdminLocalByDirection(int direction, IList<int> modulesIds);

        /// <summary>
        /// Modifie la culture de l'url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        string GetOtherCultreUrl(string url);
        IEnumerable<Extension> GetExtension();

        IQueryable<UtilisateurDirection> GetAllUtilisateurDirections();
        IQueryable<int> GetDepartmentsHavingLocalAdmin(IList<int> departments);
        bool HasParentAdmin(int departmentId);
        bool HasLocalAdmin(int departmentId);
        IQueryable<Direction> GetAllowedChildrenDepartments(int departmentId, int userId);
        IList<Direction> GetAllowedDepartmentsTree(int rootDepartmentId, int userId);

        IList<UtilisateurViewModel> LoadAdminGlobalByActivite();
        IList<EmailTemplateLab> GetEmailTemplateLab();
        IQueryable<PpeType> GetPpeTypes();
        IQueryable<Direction> GetDirectionHabilitionsActiviteConfidentiel(int utilisateurId, int activiteId);
    }
}
