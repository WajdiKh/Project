using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BacaratWeb.Entities.Commun;
using BacaratWeb.Core.DataEncryption.Attributes;
using BacaratWeb.Entities.Fraude;
using System.Xml.Linq;

namespace BacaratWeb.Entities.Lab
{
    [Table("DossierLab", Schema = "Lab")]
    public sealed class DossierLab
    {
        public DossierLab()
        {
            DocumentDossierLabs = new HashSet<DocumentDossierLab>();
            DossierLabActions = new HashSet<DossierLabAction>();
            DossierLabHistos = new HashSet<DossierLabHisto>();
            DossierLabPersonneMorales = new HashSet<DossierLabPersonneMorale>();
            DossierLabPersonnePhysiques = new HashSet<DossierLabPersonnePhysique>();
            EventDossiers = new HashSet<EventDossier>();
            DossierLabOperations = new HashSet<DossierLabOperation>();
            PersonneMoraleLabLienEntites = new HashSet<PersonneMoraleLabLienEntite>();
            PersonnePhysiqueLabLienEntites = new HashSet<PersonnePhysiqueLabLienEntite>(); 
            DeclarationTracfins = new HashSet<DeclarationTracfin>();
            DossierLabScenarios = new HashSet<DossierLabScenario>();
            DemandeInformationLabs = new HashSet<DemandeInformationLab>();
            DossierFraudesInLab = new HashSet<DossierFraude>();
            DossierLabNonConnus = new HashSet<DossierLabNonConnu>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string CodeUnique { get; set; }

        public int UtilisateurId { get; set; }
        public int? CreateurId { get; set; }

        public int? ModificateurId { get; set; }
        public int DirectionId { get; set; }
        public int EntiteId { get; set; }
        public DateTimeOffset? DateReception { get; set; }
        public DateTimeOffset? DateReponse { get; set; }
        public DateTimeOffset? DateCreation { get; set; }
        public DateTimeOffset? DateCloture  { get; set; }
        public DateTimeOffset? DateModification { get; set; }
        public int CategorieId { get; set; }
        public int OrigineLabId { get; set; }
        public int? SecteurEconomiqueId { get; set; }
        public int? PaysId { get; set; }
        [Encrypted] public byte[] MotifsSoupcons { get; set; }
        public int? AvisId { get; set; }
        public int? VisaId { get; set; }
        public DateTimeOffset? PresentationComiteDate { get; set; }
        public DateTimeOffset? DateDeclarationLocale { get; set; }
        public int StatutDossierId { get; set; }
        public bool Confidentiel { get; set; }
        public bool IsDeclarationSoupcon { get; set; }
        public int CategorieGroupeLabId { get; set; }
        public int OrigineGroupeLabId { get; set; }
        public bool IsDgt { get; set; }
        public bool IsAutorisationDGT { get; set; }
        public bool IsCov { get; set; }
        public bool IsSuivi { get; set; }
        public int? DossierFraudeId { get; set; }
        public bool DossiersFluxMultiples { get; set; }
        public bool SurveillanceDaech { get; set; }
        public DateTimeOffset? DateDerniereDeclarationDgt { get; set; }
        public int? DsFileEnvoiId { get; set; }
        public int? DsFileArId { get; set; }
        public int? DuplicateDossierId { get; set; }
        public string DuplicateDossierCode { get; set; }
        public bool? DsSended { get; set; }
        public int? SourceApplicationId { get; set; }

        [ForeignKey(nameof(DossierFraudeId))]
        [InverseProperty("DossierLabsInFraude")]
        public DossierFraude DossierFraude { get; set; }

        [ForeignKey(nameof(CategorieGroupeLabId))]
        [InverseProperty("DossierLabs")]
        public CategorieGroupeLab CategorieGroupeLab { get; set; }

        [ForeignKey(nameof(OrigineGroupeLabId))]
        [InverseProperty("DossierLabs")]
        public OrigineGroupeLab OrigineGroupeLab { get; set; }

        [ForeignKey(nameof(AvisId))]
        [InverseProperty("DossierLabs")]
        public AvisLab AvisLab { get; set; }

        [ForeignKey(nameof(CategorieId))]
        [InverseProperty("DossierLabs")]
        public CategorieLab Categorie { get; set; }

        [ForeignKey(nameof(DirectionId))]
        [InverseProperty("DossierLabs")]
        public Direction Direction { get; set; }

        [ForeignKey(nameof(EntiteId))]
        [InverseProperty("DossierLabs")]
        public EntiteLab EntiteLab { get; set; }

        [ForeignKey(nameof(PaysId))]
        [InverseProperty("DossierLabs")]
        public Pays Pays { get; set; }

        [ForeignKey(nameof(SecteurEconomiqueId))]
        [InverseProperty("DossierLabs")]
        public SecteurEconomiqueLab SecteurEconomiqueLab { get; set; }

        [ForeignKey(nameof(StatutDossierId))]
        [InverseProperty("DossierLabs")]
        public StatutDossierLab StatutDossier { get; set; }

        [ForeignKey(nameof(UtilisateurId))]
        [InverseProperty("DossierLabs")]
        public Utilisateur Utilisateur { get; set; }

        [ForeignKey(nameof(CreateurId))]
        [InverseProperty("DossierLabCreateurs")]
        public Utilisateur Createur { get; set; }

        [ForeignKey(nameof(VisaId))]
        [InverseProperty("DossierLabs")]
        public VisaLab Visa { get; set; }

        [ForeignKey(nameof(ModificateurId))]
        [InverseProperty("DossierLabModificateurs")]
        public Utilisateur Modificateur { get; set; }

        [ForeignKey(nameof(OrigineLabId))]
        [InverseProperty("DossierLabs")]
        public OrigineLab OrigineLab { get; set; }

        [InverseProperty(nameof(DossierLab))]
        public ICollection<DocumentDossierLab> DocumentDossierLabs { get; set; }

        [InverseProperty(nameof(DossierLab))]
        public ICollection<DossierLabHisto> DossierLabHistos { get; set; }

        [InverseProperty(nameof(DossierLab))]
        public ICollection<DossierLabAction> DossierLabActions { get; set; }

        [InverseProperty(nameof(DossierLab))]
        public ICollection<DossierLabPersonneMorale> DossierLabPersonneMorales { get; set; }

        [InverseProperty(nameof(DossierLab))]
        public ICollection<DossierLabPersonnePhysique> DossierLabPersonnePhysiques { get; set; }

        [InverseProperty(nameof(DossierLab))]
        public ICollection<DossierLabNonConnu> DossierLabNonConnus { get; set; }

        [InverseProperty(nameof(DossierLab))]
        public ICollection<PersonneMoraleLabLienEntite> PersonneMoraleLabLienEntites { get; set; }


        [InverseProperty(nameof(DossierLab))]
        public ICollection<PersonnePhysiqueLabLienEntite> PersonnePhysiqueLabLienEntites { get; set; }


        [InverseProperty(nameof(DossierLab))]
        public ICollection<EventDossier> EventDossiers { get; set; }

        [InverseProperty(nameof(DossierLab))]
        public ICollection<DossierLabOperation> DossierLabOperations { get; set; }

        [InverseProperty(nameof(DossierLab))]
        public ICollection<DeclarationTracfin> DeclarationTracfins { get; set; }

        [InverseProperty(nameof(DossierLab))]
        public ICollection<DossierLabScenario> DossierLabScenarios { get; set; }

        [InverseProperty(nameof(DossierLab))]
        public ICollection<DemandeInformationLab> DemandeInformationLabs { get; set; }

        [InverseProperty(nameof(DossierLab))]
        public ICollection<DossierFraude> DossierFraudesInLab { get; set; }

        [InverseProperty(nameof(DossierLab))]
        public ICollection<DeclarationTracfinEnvoiEvent> DeclarationTracfinEnvoiEvents { get; set; }


        [ForeignKey(nameof(DsFileEnvoiId))]
        [InverseProperty("DossierLabs")]
        public DeclarationTracfinFile DsFileEnvoi { get; set; }
        public bool DerogationPolitiqueGroupe { get; set; }

}
}
