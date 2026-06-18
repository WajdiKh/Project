using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BacaratWeb.Core.Providers;
using BacaratWeb.Models.Referentials;


namespace BacaratWeb.Areas.Lab.Models
{
    public sealed class PersonneMoraleLabViewModel
    {
        public PersonneMoraleLabViewModel()
        {
            LienPersonneMorales = new List<LienPersonneMoraleViewModel>();
            LienPersonnePhysiques = new List<LienPersonnePhysiqueViewModel>();
            RepresentantLegals = new List<RepresentantLegalViewModel>();
            SupportFinancierPersonneMorales = new List<SupportFinancierPersonneMoraleViewModel>();
            Dirigeants = new List<DirigeantViewModel>();
        }

        [StringLength(200)] public string Activite { get; set; }

        public CoordonneeViewModel Coordonnee { get; set; }

        public int? CoordonneeId { get; set; }

        public DateTime? DateCessationRelations { get; set; }

        public DateTime? DateImmatriculation { get; set; }

        public int? FormeJuridiqueId { get; set; }

        public int Id { get; set; }

        [StringLength(50)] public string IdentifiantTvaUe { get; set; }

        [StringLength(64)] public string NumeroImmatriculation { get; set; }
        [StringLength(400)] 
        public string OtherRaisonIdPro { get; set; }
        [StringLength(400)]
        public string MainActivity { get; set; }

        public string Lang { get; set; }

        public List<LienPersonneMoraleViewModel> LienPersonneMorales { get; set; }

        public List<LienPersonnePhysiqueViewModel> LienPersonnePhysiques { get; set; }

        public int? PaysId { get; set; }

        [RequiredIntWithCustomTranslatedErrorMessage(_fieldFr = "la raison sociale", _fieldEn = "the social reason")] [StringLength(120)] public string RaisonSociale { get; set; }

        public List<RepresentantLegalViewModel> RepresentantLegals { get; set; }

        public int? SecteurProfessionnelId { get; set; }

        public string Sigle { get; set; }

        public string SiteInternet { get; set; }

        public List<SupportFinancierPersonneMoraleViewModel> SupportFinancierPersonneMorales
        {
            get;
            set;
        }

        public int? TypeImplicationId { get; set; }
        public int? TypeClientId { get; set; }
        
        public int? ProfessionalIdentificationId { get; set; }
        public bool DroitAuCompte { get; set; }
        public DateTime? DateEntreeEnRelation { get; set; }
        public int? CanalEntreeEnRelationId { get; set; }
        public bool CessationRelation { get; set; }
        public int? TypeRelationAffaireLabId { get; set; }
        public TypeRelationAffaireLabViewModel TypeRelationAffaireLab { get; set; }
        public bool ListeCriblage { get; set; }
        public bool DeclarationSoupcon { get; set; }
        public int TypeCompteId { get; set; }
        public string Iban { get; set; }

        public string CryptedId { get; set; }

        public string CryptedCoordonneeId { get; set; }

        [StringLength(50)] public string IdentifiantClient { get; set; }
        public int? NatureRelationClientId { get; set; } 
        public bool DetentionCoffre { get; set; }
        [StringLength(700)] public string ElementClesRelation { get; set; }
        public int? FigureSanctionId { get; set; } = 2;
        public int? ActiviteCriminelleId { get; set; } = 3;
        [StringLength(400)] public string FigureSanctionPrecision { get; set; }
        [StringLength(400)] public string ActiviteCriminellePrecision { get; set; }
        [StringLength(700)] public string NotorieteDefavorable { get; set; }
        public List<DirigeantViewModel> Dirigeants { get; set; }
        public Guid? IdPersonne { get; set; } = Guid.NewGuid();
        public string ClientId { get; set; }
        public int? PaysDeRegistreId { get; set; }

        public string PaysDeRegistre { get; set; }
        public string Pays { get; set; }
        public string TypeClient { get; set; }
        public string TypeImplication { get; set; }
        public string FormeJuridique { get; set; }
        public string SecteurProfessionnel { get; set; }
        public string RelationAffaireLab { get; set; }
        public string CanalEntreeEnRelation { get; set; }
        public string TypeCriblage { get; set; }
        public string ProfessionalIdentification { get; set; }

        public int? DossierLabId { get; set; }
        public string CodeDossier { get; set; }
    }
}
