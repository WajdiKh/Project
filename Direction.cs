using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BacaratWeb.Entities.AvisSi;
using BacaratWeb.Entities.CommunAvis;
using BacaratWeb.Entities.Escalade;
using BacaratWeb.Entities.Fraude;
using BacaratWeb.Entities.Lab;
using BacaratWeb.Entities.SAvis;
using BacaratWeb.Entities.Amf;
using BacaratWeb.Entities.AvisIm;
using BacaratWeb.Entities.Gda;

namespace BacaratWeb.Entities.Commun
{
    [Table("Direction", Schema = "Commun")]
    public sealed class Direction
    {
        public Direction()
        {
            DossierFraudeHistos = new HashSet<DossierFraudeHisto>();
            Delegations = new HashSet<Delegation>();
            DirectionAccessibleDirectionAccs = new HashSet<DirectionAccessible>();
            DirectionAccessibleDirections = new HashSet<DirectionAccessible>();
            DirectionAccessibleExceptionDirectionAccs = new HashSet<DirectionAccessibleException>();
            DirectionAccessibleExceptionDirections = new HashSet<DirectionAccessibleException>();
            DossierEscaladeHistos = new HashSet<DossierEscaladeHisto>();
            DossierEscalades = new HashSet<DossierEscalade>();
            DossierFraudes = new HashSet<DossierFraude>();

            DirectionAccessibleExceptionAmfDirectionAccs = new HashSet<DirectionAccessibleExceptionAmf>();
            DirectionAccessibleExceptionAmfDirections = new HashSet<DirectionAccessibleExceptionAmf>();


            EmailNotificationTemplates = new HashSet<EmailNotificationTemplate>();
            EmailGeneriques = new HashSet<EmailGenerique>();
            Confidentiels = new HashSet<Confidentiel>();
            ConfidentielAmfs = new HashSet<ConfidentielAmf>();
            Entites = new HashSet<EntiteFraude>();
            EntiteGdas = new HashSet<EntiteGda>();
            IndicePies = new HashSet<IndicePie>();
            InverseParent = new HashSet<Direction>();
            OrigineDirectionFraudes = new HashSet<OrigineDirectionFraude>();
            ParametreDirections = new HashSet<ParametreDirection>();
            RegleDirections = new HashSet<RegleDirection>();
            EquipeValidationFlgeDirections = new HashSet<EquipeValidationFlge>();
            Secteurs = new HashSet<SecteurFraude>();
            TypeClientEscalades = new HashSet<TypeClientEscalade>();
            TypeClientFraudes = new HashSet<TypeClientFraude>();
            UtilisateurDirections = new HashSet<UtilisateurDirection>();
            PlanActionAnalyses = new HashSet<PlanActionAnalyse>();
            Utilisateurs = new HashSet<Utilisateur>();
            RoutageFlges = new HashSet<RoutageFlge>();
            EntiteConcernes = new HashSet<EntiteConcerne>();
            GroupeCategorieLabs = new HashSet<GroupeCategorieLab>();
            CategorieLabs = new HashSet<CategorieLab>();
            OrigineLabs = new HashSet<OrigineLab>();
            OrganismeLabs = new HashSet<OrganismeLab>();
            GroupeOrigineLabs = new HashSet<GroupeOrigineLab>();
            DerniereReferenceTracfins = new HashSet<DerniereReferenceTracfin>();
            DossierAvisSiDirectionFlges = new HashSet<DossierAvisSi>();
            DossierAvisSiEntiteLocals = new HashSet<DossierAvisSi>();
            EntiteConcerneAvisSis = new HashSet<EntiteConcerneAvisSi>();
            EquipeValidationFlgeAvisSis = new HashSet<EquipeValidationFlgeAvisSi>();
            PlanActionAnalyseAvisSis = new HashSet<PlanActionAnalyseAvisSi>();
            RoutageFlgeAvisSis = new HashSet<RoutageFlgeAvisSi>();
            TypeClientAvisSis = new HashSet<TypeClientAvisSi>();
            DossierSAvisDirectionFlges = new HashSet<DossierSAvis>();
            DossierSAvisEntiteLocals = new HashSet<DossierSAvis>();
            EntiteConcerneSAviss = new HashSet<EntiteConcerneSAvis>();
            EntiteConcerneAvisIms = new HashSet<EntiteConcerneAvisIm>();
            EquipeValidationFlgeAviss = new HashSet<EquipeValidationFlgeAvis>();
            PlanActionAnalyseAviss = new HashSet<PlanActionAnalyseAvis>();
            RoutageFlgeAviss = new HashSet<RoutageFlgeAvis>();
            TypeClientAviss = new HashSet<TypeClientAvis>();
            
            EntiteLabs = new HashSet<EntiteLab>();
            DossierLabs = new HashSet<DossierLab>();
            DossierLabHistos = new HashSet<DossierLabHisto>();
            SecteurLabs = new HashSet<SecteurLab>();
            TypeClientLabs = new HashSet<TypeClientLab>();
            GroupeCategorieLabs = new HashSet<GroupeCategorieLab>();
            CategorieLabs = new HashSet<CategorieLab>();
            OrigineLabs = new HashSet<OrigineLab>();

            OrganismeLabs = new HashSet<OrganismeLab>();
            GroupeOrigineLabs = new HashSet<GroupeOrigineLab>();
            SecteurEconomiqueLabs = new HashSet<SecteurEconomiqueLab>();
            EventSearchLabs = new HashSet<EventSearchLab>();
            EventSearchFraudes = new HashSet<EventSearchFraude>();
            EventSearchGdas = new HashSet<EventSearchGda>();
            DossierAmfs = new HashSet<DossierAmf>();
            DossierAmfsEntiteBancaire = new HashSet<DossierAmf>();

            DossierAmfHistos = new HashSet<DossierAmfHisto>();
            EventSearchAmfs = new HashSet<EventSearchAmf>();

            DirectionCoordonnees = new HashSet<DirectionCoordonnee>();
            
            EntiteGroupeEntites = new HashSet<EntiteGroupeEntiteAvis>();

            Bunits = new HashSet<Bunit>();
            SecteurGdas = new HashSet<SecteurGda>();
            DossierGdas = new HashSet<DossierGda>();
            OrigineDirectionGdas = new HashSet<OrigineDirectionGda>();
            DossierGdaHistos = new HashSet<DossierGdaHisto>();
            DossierGdasCollaborateur = new HashSet<DossierGda>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(10)]
        public string Abreviation { get; set; }

        [StringLength(50)]
        public string Nom { get; set; }

        [StringLength(35)]
        public string CodeBic { get; set; }

        public string CodeLei { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset? DateModification { get; set; }

        public int? ModificateurId { get; set; }

        public int? CreateurId { get; set; }

        public bool IsActive { get; set; }

        public int? DeviseId { get; set; }
        public bool IsFlge { get; set; }
        public bool IsDDC { get; set; }

        public int? PaysId { get; set; }

        public int? ParentId { get; set; }
        public int? ModeEnvoieTracfinId { get; set; }

        public Direction Parent { get; set; }

        public Pays Pays { get; set; }

        public string Domain { get; set; }
        public int? CarnetAdressesId { get; set; }

        public CarnetAdresses CarnetAdresses { get; set; }
        public string CodeOrion { get; set; }
        public bool IsTotus { get; set; }

        public bool IsTracfin { get; set; }

        public string ErmesCertificatId { get; set; }
        public bool IsDdcMailActiveGda { get; set; }
        public string CodeBanque { get; set; }
        [ForeignKey(nameof(CreateurId))]
        [InverseProperty("DirectionCreateurs")]
        public Utilisateur Createur { get; set; }

        [ForeignKey(nameof(ModeEnvoieTracfinId))]
        [InverseProperty("ModeEnvoieTracfins")]
        public ModeEnvoieTracfin ModeEnvoieTracfin { get; set; }

        [ForeignKey(nameof(DeviseId))]
        [InverseProperty("Directions")]
        public Devise Devise { get; set; }

        [ForeignKey(nameof(ModificateurId))]
        [InverseProperty("DirectionModificateurs")]
        public Utilisateur Modificateur { get; set; }
        [InverseProperty(nameof(Direction))]
        public ICollection<Delegation> Delegations { get; set; }

        [InverseProperty("DirectionAcc")]
        public ICollection<DirectionAccessible> DirectionAccessibleDirectionAccs { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<DirectionAccessible> DirectionAccessibleDirections { get; set; }

        [InverseProperty(nameof(Direction))]
        public DirectionExtend DirectionExtend { get; set; }

        [InverseProperty("DirectionAcc")]
        public ICollection<DirectionAccessibleException> DirectionAccessibleExceptionDirectionAccs { get; set; }

        [InverseProperty("DirectionAcc")]
        public ICollection<DirectionAccessibleExceptionAmf> DirectionAccessibleExceptionAmfDirectionAccs { get; set; }

        [InverseProperty("Direction")]
        public ICollection<DirectionAccessibleExceptionAmf> DirectionAccessibleExceptionAmfDirections { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<DirectionAccessibleException> DirectionAccessibleExceptionDirections { get; set; }

        [InverseProperty("DirectionFlge")]
        public ICollection<DossierEscaladeHisto> DossierEscaladeHistos { get; set; }

        [InverseProperty("DirectionFlge")]
        public ICollection<DossierEscalade> DossierEscalades { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<DossierFraude> DossierFraudes { get; set; }
        [InverseProperty(nameof(Direction))]
        public ICollection<DossierGda> DossierGdas { get; set; }

        [InverseProperty("DirectionCollaborateurEscalade")]
        public ICollection<DossierGda> DossierGdasCollaborateur { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<EmailNotificationTemplate> EmailNotificationTemplates { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<EntiteFraude> Entites { get; set; }
        [InverseProperty(nameof(Direction))]
        public ICollection<EntiteGda> EntiteGdas { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<IndicePie> IndicePies { get; set; }

        public ICollection<Direction> InverseParent { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<OrigineDirectionFraude> OrigineDirectionFraudes { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<OrigineDirectionGda> OrigineDirectionGdas { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<ParametreDirection> ParametreDirections { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<RegleDirection> RegleDirections { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<SecteurFraude> Secteurs { get; set; }
        [InverseProperty(nameof(Direction))]
        public ICollection<SecteurGda> SecteurGdas { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<TypeClientEscalade> TypeClientEscalades { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<TypeClientFraude> TypeClientFraudes { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<UtilisateurDirection> UtilisateurDirections { get; set; }

        [InverseProperty("DirectionAttache")]
        public ICollection<Utilisateur> Utilisateurs { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<DossierFraudeHisto> DossierFraudeHistos { get; set; }
        [InverseProperty(nameof(Direction))]
        public ICollection<DossierGdaHisto> DossierGdaHistos { get; set; }

        [InverseProperty("DirectionPorteur")]
        public ICollection<PlanActionAnalyse> PlanActionAnalyses { get; set; }

        [InverseProperty("EntiteLocal")]
        public ICollection<DossierEscalade> DossierEscaladeEntiteLocals { get; set; }

        [InverseProperty("EntiteLocal")]
        public ICollection<DossierEscaladeHisto> DossierEscaladeHistoEntiteLocals { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<EmailGenerique> EmailGeneriques { get; set; }
        [InverseProperty(nameof(Direction))]
        public ICollection<Confidentiel> Confidentiels { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<ConfidentielAmf> ConfidentielAmfs { get; set; }
        [InverseProperty("DirectionFlge")]
        public ICollection<RoutageFlge> RoutageFlges { get; set; }

        [InverseProperty("SousEntite")]
        public ICollection<EntiteConcerne> EntiteConcernes { get; set; }

        [InverseProperty("Direction")]
        public ICollection<EquipeValidationFlge> EquipeValidationFlgeDirections { get; set; }


        [InverseProperty("Direction")]
        public ICollection<DerniereReferenceTracfin> DerniereReferenceTracfins { get; set; }

        [InverseProperty(nameof(DossierAvisSi.DirectionFlge))]
        public ICollection<DossierAvisSi> DossierAvisSiDirectionFlges { get; set; }
        [InverseProperty(nameof(DossierAvisSi.EntiteLocal))]
        public ICollection<DossierAvisSi> DossierAvisSiEntiteLocals { get; set; }

        [InverseProperty(nameof(EntiteConcerneAvisSi.SousEntite))]
        public ICollection<EntiteConcerneAvisSi> EntiteConcerneAvisSis { get; set; }

        [InverseProperty(nameof(EquipeValidationFlgeAvisSi.Direction))]
        public ICollection<EquipeValidationFlgeAvisSi> EquipeValidationFlgeAvisSis { get; set; }

        [InverseProperty(nameof(PlanActionAnalyseAvisSi.DirectionPorteur))]
        public ICollection<PlanActionAnalyseAvisSi> PlanActionAnalyseAvisSis { get; set; }

        [InverseProperty(nameof(RoutageFlgeAvisSi.DirectionFlge))]
        public ICollection<RoutageFlgeAvisSi> RoutageFlgeAvisSis { get; set; }

        [InverseProperty(nameof(TypeClientAvisSi.Direction))]
        public ICollection<TypeClientAvisSi> TypeClientAvisSis { get; set; }

        [InverseProperty(nameof(DossierSAvis.DirectionFlge))]
        public ICollection<DossierSAvis> DossierSAvisDirectionFlges { get; set; }
        [InverseProperty(nameof(DossierSAvis.EntiteLocal))]
        public ICollection<DossierSAvis> DossierSAvisEntiteLocals { get; set; }


        [InverseProperty(nameof(DossierAvisIm.DirectionFlge))]
        public  ICollection<DossierAvisIm> DossierAvisImDirectionFlges { get; set; }
        [InverseProperty(nameof(DossierAvisIm.EntiteLocal))]
        public  ICollection<DossierAvisIm> DossierAvisImEntiteLocals { get; set; }

        [InverseProperty(nameof(EntiteConcerneSAvis.SousEntite))]
        public  ICollection<EntiteConcerneSAvis> EntiteConcerneSAviss { get; set; }
        [InverseProperty(nameof(EntiteConcerneAvisIm.SousEntite))]
        public  ICollection<EntiteConcerneAvisIm> EntiteConcerneAvisIms { get; set; }


        [InverseProperty(nameof(EquipeValidationFlgeAvis.Direction))]
        public ICollection<EquipeValidationFlgeAvis> EquipeValidationFlgeAviss { get; set; }

        [InverseProperty(nameof(PlanActionAnalyseAvis.DirectionPorteur))]
        public ICollection<PlanActionAnalyseAvis> PlanActionAnalyseAviss { get; set; }

        [InverseProperty(nameof(RoutageFlgeAvis.DirectionFlge))]
        public ICollection<RoutageFlgeAvis> RoutageFlgeAviss { get; set; }

        [InverseProperty(nameof(TypeClientAvis.Direction))]
        public ICollection<TypeClientAvis> TypeClientAviss { get; set; }

        [InverseProperty(nameof(EventSearchLab.DirectionAttache))]
        public ICollection<EventSearchLab> EventSearchLabs { get; set; }

        [InverseProperty(nameof(EventSearchFraude.DirectionAttache))]
        public ICollection<EventSearchFraude> EventSearchFraudes { get; set; }
       
        [InverseProperty(nameof(EventSearchGda.DirectionAttache))]
        public ICollection<EventSearchGda> EventSearchGdas { get; set; }

        [InverseProperty("Entite")]
        public ICollection<EntiteGroupeEntiteAvis> EntiteGroupeEntites { get; set; }

        [InverseProperty("BunitDirection")]
        public ICollection<Bunit> Bunits { get; set; }

        [InverseProperty("Direction")]
        public ICollection<GroupeCategorieLab> GroupeCategorieLabs { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<TypeClientLab> TypeClientLabs { get; set; }
        [InverseProperty(nameof(Direction))]
        public ICollection<SecteurLab> SecteurLabs { get; set; }
        [InverseProperty(nameof(Direction))]
        public ICollection<EntiteLab> EntiteLabs { get; set; }
        [InverseProperty(nameof(Direction))]
        public ICollection<DossierLab> DossierLabs { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<DossierLabHisto> DossierLabHistos { get; set; }
        [InverseProperty("Direction")]
        public ICollection<CategorieLab> CategorieLabs { get; set; }

        [InverseProperty("Direction")]
        public ICollection<OrigineLab> OrigineLabs { get; set; }

        [InverseProperty("Direction")]
        public ICollection<OrganismeLab> OrganismeLabs { get; set; }

        [InverseProperty("Direction")]
        public ICollection<SecteurEconomiqueLab> SecteurEconomiqueLabs { get; set; }

        [InverseProperty("Direction")]
        public ICollection<GroupeOrigineLab> GroupeOrigineLabs { get; set; }


        [InverseProperty(nameof(EventSearchAmf.DirectionAttache))]
        public ICollection<EventSearchAmf> EventSearchAmfs { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<DossierAmf> DossierAmfs { get; set; }

        [InverseProperty(nameof(DossierAmf.EntiteBancaire))]
        public ICollection<DossierAmf> DossierAmfsEntiteBancaire { get; set; }

        [InverseProperty(nameof(Direction))]
        public ICollection<DossierAmfHisto> DossierAmfHistos { get; set; }

        [InverseProperty(nameof(DirectionCoordonnee.Direction))]
        public ICollection<DirectionCoordonnee> DirectionCoordonnees { get; set; }


    }
}
