using BacaratWeb.Services.Commun.Interfaces;
using BacaratWeb.Services.Lab.Interfaces;
using BacaratWeb.Services.Lab.Interfaces.Histo;
using BacaratWeb.Services.Lab.Services.Interfaces;

namespace BacaratWeb.Services.Lab
{
    public class LabFacadeService : ILabFacadeService
    {
        public LabFacadeService(IDelegationService delegationService,
            IDocumentDossierLabService documentDossierLabServie,
            IDossierLabService dossierLabService,
            IDossierLabHistoService dossierLabHistoService,
            IPersonnePhysiqueLabService personnePhysiqueLabService,
            IPersonneMoraleLabService personneMoraleLabService,
            ICustomerLabService customerLabService,
            IReferentielService referentielService,
            IReportingLabService reportingService,
            IUtilisateurDirectionService utilisateurDirectionService,
            IEventDossierService eventDossierService,
            IDemandeInformationLabService demandeInformationLabService,
            IDocumentDemandeInformationLabService documentDemandeInformationLabService,
            IEmailNotificationService emailNotificationService,
            IEmailNotificationTemplateService emailNotificationTemplateService,
            IDeclarationTracfinService declarationTracfinServicee,
            IDossierLabPersonnePhysiqueService dossierLabPersonnePhysiqueService,
            IDossierLabPersonneMoraleService dossierLabPersonneMoraleService,
            IDossierLabScenarioService dossierLabScenarioService,
            IOrigineLabService origineLabService,
            ICategorieLabService categorieLabService,
            IDeclarationTracfinNatureInfractionPenaleService declarationTracfinNatureInfractionPenaleService,
            IDeclarationTracfinNatureFraudeFiscaleService declarationTracfinNatureFraudeFiscaleService,
            ISupportFinancierPersonnePhysiqueService supportFinancierPersonnePhysiqueService,
            ISupportFinancierPersonneMoraleService supportFinancierPersonneMoraleService,
            IOrganismeLabService organismeLabService,
            INonConnuLabService nonConnuLabService,
            IEmailTemplateLabService emailTemplateLabService
        )
                                  
        {
            Delegation = delegationService;
            DocumentDossier = documentDossierLabServie;
            Dossier = dossierLabService;
            DossierHisto = dossierLabHistoService;
            PersonnePhysique = personnePhysiqueLabService;
            PersonneMorale = personneMoraleLabService;
            NonConnu = nonConnuLabService;
            Customer = customerLabService;
            Referentiel = referentielService;
            Reporting = reportingService;
            UtilisateurDirectionService = utilisateurDirectionService;
            EventDossier = eventDossierService;
            DemandeInformationLab = demandeInformationLabService;
            DocumentDemandeInformationLab = documentDemandeInformationLabService;
            EmailNotification = emailNotificationService;
            DossierLabPersonnePhysique = dossierLabPersonnePhysiqueService;
            DossierLabPersonneMorale = dossierLabPersonneMoraleService;
            DossierLabScenario = dossierLabScenarioService;
            DeclarationTracfin = declarationTracfinServicee;
            EmailNotificationTemplate = emailNotificationTemplateService;
            OrigineLab = origineLabService;
            CategorieLab= categorieLabService;
            OrganismeLab = organismeLabService;
            DeclarationTracfinNatureInfractionPenaleService = declarationTracfinNatureInfractionPenaleService;
            DeclarationTracfinNatureFraudeFiscaleService = declarationTracfinNatureFraudeFiscaleService;
            SupportFinancierPersonnePhysiqueService = supportFinancierPersonnePhysiqueService;
            SupportFinancierPersonneMoraleService = supportFinancierPersonneMoraleService;
            EmailTemplateLabService = emailTemplateLabService;
        }

        public IDelegationService Delegation { get; set; }

        public IDocumentLabService Document { get; set; }
        public IDocumentDossierLabService DocumentDossier { get; set; }

        public IDossierLabService Dossier { get; set; }

        public IDossierLabHistoService DossierHisto { get; set; }

        public IPersonneMoraleLabService PersonneMorale { get; set; }

        public IPersonnePhysiqueLabService PersonnePhysique { get; set; }
        public INonConnuLabService NonConnu { get; set; }

        public IDossierLabPersonneMoraleService DossierLabPersonneMorale { get; set; }

        public IDossierLabPersonnePhysiqueService DossierLabPersonnePhysique { get; set; }

        public IDossierLabScenarioService DossierLabScenario { get; set; }

        public IReferentielService Referentiel { get; set; }

        public IReportingLabService Reporting { get; set; }

        public IUtilisateurDirectionService UtilisateurDirectionService { get; set; }

        public IEventDossierService EventDossier { get; set; }

        public IDemandeInformationLabService DemandeInformationLab { get; set; }

        public IDocumentDemandeInformationLabService DocumentDemandeInformationLab { get; set; }

        public IEmailNotificationService EmailNotification { get; set; }

        public IEmailNotificationTemplateService EmailNotificationTemplate { get; set; }

        public IDeclarationTracfinService DeclarationTracfin { get; set; }
        public IOrigineLabService OrigineLab { get; set; }
        public ICategorieLabService CategorieLab { get; set; }
        public IOrganismeLabService OrganismeLab { get; set; }
        public ICustomerLabService Customer { get; set; }

        public IDeclarationTracfinNatureInfractionPenaleService DeclarationTracfinNatureInfractionPenaleService
        {
            get;
            set;
        }

        public IDeclarationTracfinNatureFraudeFiscaleService DeclarationTracfinNatureFraudeFiscaleService { get; set; }
        public ISupportFinancierPersonnePhysiqueService SupportFinancierPersonnePhysiqueService { get; set; }
        public ISupportFinancierPersonneMoraleService SupportFinancierPersonneMoraleService { get; set; }
        public IEmailTemplateLabService EmailTemplateLabService { get; set; }
        public IScenarioLabService ScenarioLabService { get; set; }
    }
}
