
using BacaratWeb.Services.Commun.Interfaces;
using BacaratWeb.Services.Lab.Interfaces.Histo;
using BacaratWeb.Services.Lab.Services.Interfaces;

namespace BacaratWeb.Services.Lab.Interfaces
{
    public interface ILabFacadeService
    {

        ICustomerLabService Customer { get; set; }

        IDelegationService Delegation { get; set; }

        IDocumentDossierLabService DocumentDossier { get; set; }
        IDocumentLabService Document { get; set; }

        IDossierLabService Dossier { get; set; }

        IDossierLabHistoService DossierHisto { get; set; }

        IPersonneMoraleLabService PersonneMorale { get; set; }

        IPersonnePhysiqueLabService PersonnePhysique { get; set; }

        INonConnuLabService NonConnu { get; set; }

        IDossierLabPersonneMoraleService DossierLabPersonneMorale { get; set; }
        IDossierLabScenarioService DossierLabScenario { get; set; }

        IDossierLabPersonnePhysiqueService DossierLabPersonnePhysique { get; set; }

        IReferentielService Referentiel { get; set; }

        IReportingLabService Reporting { get; set; }

        IUtilisateurDirectionService UtilisateurDirectionService { get; set; }

        IEventDossierService EventDossier { get; set; }

        IDemandeInformationLabService DemandeInformationLab { get; set; }

        IDocumentDemandeInformationLabService DocumentDemandeInformationLab { get; set; }

        IEmailNotificationService EmailNotification { get; set; }

        IEmailNotificationTemplateService EmailNotificationTemplate { get; set; }

        IDeclarationTracfinService DeclarationTracfin { get; set; }
        ISupportFinancierPersonnePhysiqueService SupportFinancierPersonnePhysiqueService{ get; set; }
        ISupportFinancierPersonneMoraleService SupportFinancierPersonneMoraleService { get; set; }

        IOrigineLabService OrigineLab { get; set; }
        ICategorieLabService CategorieLab { get; set; }
        IOrganismeLabService OrganismeLab { get; set; }

        IDeclarationTracfinNatureInfractionPenaleService DeclarationTracfinNatureInfractionPenaleService { get; set;}
        IDeclarationTracfinNatureFraudeFiscaleService DeclarationTracfinNatureFraudeFiscaleService { get;set; }
        IEmailTemplateLabService EmailTemplateLabService { get; set; }
    }
}
