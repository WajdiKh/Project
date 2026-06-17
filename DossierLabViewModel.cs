using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using BacaratWeb.Models.Referentials;
using BacaratWeb.Providers;
using BacaratWeb.Core.Providers;

namespace BacaratWeb.Areas.Lab.Models
{
    public sealed class DossierLabViewModel
    {
        public DossierLabViewModel()
        {
            DossierLabPersonnePhysiques = new List<DossierLabPersonnePhysiqueViewModel>();
            DossierLabPersonneMorales = new List<DossierLabPersonneMoraleViewModel>();
            DossierLabNonConnus = new List<DossierLabNonConnueViewModel>();
            Attachments = new List<AttachmentViewModel>();
            DocumentDossierLabs = new List<DocumentDossierLabViewModel>();
            DossierLabActions = new List<DossierLabActionViewModel>();
            DirectionItemDatasources = new List<DirectionViewModel>();
            DossierLabOperations = new List<DossierLabOperationViewModel>();
            DossierLabScenarios = new List<DossierLabScenarioViewModel>();
            DeclarationTracfins = new List<DeclarationTracfinViewModel>();
            DeclarationTracfinFiles = new List<DeclarationTracfinFileViewModel>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string CodeUnique { get; set; }

        public int UtilisateurId { get; set; }
        public int? CreateurId { get; set; }

        public int? ModificateurId { get; set; }

        public DateTime? DateCreation { get; set; }

        public DateTime? DateModification { get; set; }

        [RequiredDateTimeOffsetWithCustomTranslatedErrorMessage(ErrorMessage="",_fieldFr ="une date de détection",_fieldEn = "a detection date")] public DateTime? DateReception { get; set; }

        public DateTime? DateReponse { get; set; }

        public DateTime? DateCloture { get; set; }
        [RequiredInt(ErrorMessage = "", _fieldFr = "une direction", _fieldEn = "a direction")] public int? DirectionId { get; set; }

        [RequiredInt(ErrorMessage = "", _fieldFr = "une entité", _fieldEn = "a entity")] public int EntiteId { get; set; }

        [RequiredInt(ErrorMessage = "", _fieldFr = "une catégorie", _fieldEn = "a category")] public int CategorieId { get; set; }
        [RequiredInt(ErrorMessage = "", _fieldFr = "une catégorie groupe", _fieldEn = "a group category")] public int CategorieGroupeLabId { get; set; }

        [RequiredInt(ErrorMessage = "", _fieldFr = "une orgine", _fieldEn = "an orgin")] public int OrigineLabId { get; set; }
        [RequiredInt(ErrorMessage = "", _fieldFr = "une orgine groupe", _fieldEn = "a group orgin")] public int OrigineGroupeLabId { get; set; }

        public int? SecteurEconomiqueId { get; set; }

        public int? PaysId { get; set; }

        public string MotifsSoupcons { get; set; }

        public int? AvisId { get; set; }

        public int? VisaId { get; set; }

        [RequiredInt(ErrorMessage = "", _fieldFr = "un statut", _fieldEn = "a status")] public int StatutDossierId { get; set; }

        public bool Ds { get; set; }

        public DateTime? PresentationComiteDate { get; set; }
        public DateTime? DateDeclarationLocale { get; set; }

        public bool IsDeclarationSoupcon { get; set; }
        public bool Confidentiel { get; set; }

        public UtilisateurViewModel Utilisateur { get; set; }
        public ECreateUpdateDossier ECreateUpdateDossier { get; set; }
        public string ErrorMessage { get; set; }
        public bool Error { get; set; }
        public string Lang { get; set; }
        public bool IsNew { get; set; }
        public string CryptedId { get; set; }
        public string CryptedModificateurId { get; set; }
        public string CryptedUtilisateurId { get; set; }
        public string CryptedCreateurId { get; set; }
        public EVisibleDossier EVisibleDossier { get; set; }
        public List<DossierLabPersonneMoraleViewModel> DossierLabPersonneMorales { get; set; }
        public List<DossierLabPersonnePhysiqueViewModel> DossierLabPersonnePhysiques { get; set; }
        public List<DossierLabNonConnueViewModel> DossierLabNonConnus { get; set; }
        public List<AttachmentViewModel> Attachments { get; set; }
        public List<DocumentDossierLabViewModel> DocumentDossierLabs { get; set; }
        public List<DeclarationTracfinFileViewModel> DeclarationTracfinFiles { get; set; }
        public List<DossierLabActionViewModel> DossierLabActions { get; set; }
        public IEnumerable<DirectionViewModel> DirectionItemDatasources { get; set; }
        public DirectionViewModel Direction { get; set; }
        public List<DossierLabOperationViewModel> DossierLabOperations { get; set; }
        public List<DossierLabScenarioViewModel> DossierLabScenarios { get; set; }
        public List<DeclarationTracfinViewModel> DeclarationTracfins { get; set; }
        public bool IsNewDeclarationTracfin { get; set; }
        public bool IsEditDossierlab { get; set; }
        public string Modificateur_FullName { get; set; }
        public string Createur_FullName { get; set; }


        [DataType(DataType.Upload)]
        public IFormFile XMLDeclarationTracfin { get; set; }

        public UserPrivilege UserPrivilege { get; set; }

        public IEnumerable<SelectedItem> EntitesItemDatasources { get; set; }

        public EntiteLabViewModel EntiteLab { get; set; }
        public bool IsDgt { get; set; }
        public bool IsAutorisationDGT { get; set; }
        public bool IsCov { get; set; }
        public bool IsSuivi { get; set; }
        public bool DossiersFluxMultiples { get; set; }
        public bool SurveillanceDaech { get; set; }
        public bool DerogationPolitiqueGroupe { get; set; }
        public string DossierFraudeCodeUnique { get; set; }
        public int? StatutDossierFraudeId { get; set; }
        public string DossierFraudeCryptedId { get; set; }
        public bool IsAdminGlobal { get; set; }
        public int? DossierFraudeId { get; set; }
        public bool HasRightLabFraude { get; set; }
        public bool IsCreatorDossierLab { get; set; }
        public bool IsDuplicated { get; set; }
        public bool IsCreatedFromFraude { get; set; }
        public string CodeUniqueDossierFraudeSource { get; set; }
        public int? DossierFraudeIdSource { get; set; }
        public string DossierFraudeIdSourceCrypted { get; set; }
        public bool ShowDateDeclaration { get; set; }
        public DateTime? DateDerniereDeclarationDgt { get; set; }
        public int?  DuplicateDossierId { get; set; }
        public string DuplicateDossierCode { get; set; }
        public bool IsTracfin { get; set; }
        public bool IsDetailView { get; set; }
        public bool ShowScenario { get; set; }
    }
}
