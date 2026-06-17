using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BacaratWeb.Entities.Commun;
using BacaratWeb.Core.DataEncryption.Attributes;

namespace BacaratWeb.Entities.Lab
{
    [Table("PersonneMoraleLab", Schema = "Lab")]
    public sealed class PersonneMoraleLab
    {
        public PersonneMoraleLab()
        {
            DossierLabPersonneMorales = new HashSet<DossierLabPersonneMorale>();
            LienPersonneMorales = new HashSet<LienPersonneMorale>();
            LienPersonnePhysiques = new HashSet<LienPersonnePhysique>();
            PersonneMoraleLabLienEntites = new HashSet<PersonneMoraleLabLienEntite>();
            RepresentantLegals = new HashSet<RepresentantLegal>();
            SupportFinancierPersonneMorales = new HashSet<SupportFinancierPersonneMorale>();
            Dirigeants = new  HashSet<Dirigeant>();
        }

        [Key]
        public int Id { get; set; }
        public int? TypeClientId { get; set; }
        public int? TypeImplicationId { get; set; }
        public int? ProfessionalIdentificationId { get; set; }
        public int? PaysDeRegistreId { get; set; }
        public string MainActivity { get; set; }
        public string OtherRaisonIdPro { get; set; }
        public DateTimeOffset? DateImmatriculation { get; set; }
        [Required]
        [StringLength(400)]
        public string RaisonSociale { get; set; }
        public string SoundexRaisonFr { get; set; }
        public string SoundexRaisonEn { get; set; }
        [Encrypted] public byte[] Sigle { get; set; }
        public int? PaysId { get; set; }
     
        [StringLength(64)]
        public string NumeroImmatriculation { get; set; }
        public bool DroitAuCompte { get; set; }
        public int? FormeJuridiqueId { get; set; }
        public int? SecteurProfessionnelId { get; set; }
        [Column("IdentifiantTvaUE")]
        [StringLength(50)]
        public string IdentifiantTvaUe { get; set; }
        [StringLength(200)]
        public string Activite { get; set; }
        public DateTimeOffset? DateEntreeEnRelation { get; set; }
        public int? CanalEntreeEnRelationId { get; set; }
        public bool CessationRelation { get; set; }
        public int? TypeRelationAffaireLabId { get; set; }
        public TypeRelationAffaireLab TypeRelationAffaireLab { get; set; }
        public DateTimeOffset? DateCessationRelations { get; set; }
        public int? CoordonneeId { get; set; }
        [StringLength(50)]
        public string IdentifiantClient { get; set; }
        public int? NatureRelationClientId { get; set; }
        public bool DetentionCoffre { get; set; }
        [StringLength(1000)] public string ElementClesRelation { get; set; }

        public int? FigureSanctionId { get; set; }
        public int? ActiviteCriminelleId { get; set; }
        public string FigureSanctionPrecision { get; set; }
        public string ActiviteCriminellePrecision { get; set; }
        public string NotorieteDefavorable { get; set; }
        public Guid? IdPersonne { get; set; }
        public int? DossierLabId { get; set; }
        public string CodeDossier { get; set; }

        [ForeignKey(nameof(CanalEntreeEnRelationId))]
        [InverseProperty("PersonneMoraleLabs")]
        public CanalEntreeEnRelation CanalEntreeEnRelation { get; set; }
        [ForeignKey(nameof(CoordonneeId))]
        [InverseProperty("PersonneMoraleLabs")]
        public Coordonnee Coordonnee { get; set; }
        [ForeignKey(nameof(FormeJuridiqueId))]
        [InverseProperty("PersonneMoraleLabs")]
        public FormeJuridique FormeJuridique { get; set; }
        [ForeignKey(nameof(PaysId))]
        [InverseProperty("PersonneMoraleLabs")]
        public Pays Pays { get; set; }
        [ForeignKey(nameof(SecteurProfessionnelId))]
        [InverseProperty("PersonneMoraleLabs")]
        public SecteurProfessionnel SecteurProfessionnel { get; set; }
        [ForeignKey(nameof(TypeClientId))]
        [InverseProperty(nameof(TypeClientLab.PersonneMoraleLabs))]
        public TypeClientLab TypeClient { get; set; }

        [ForeignKey(nameof(TypeImplicationId))]
        [InverseProperty(nameof(TypeImplicationLab.PersonneMoraleLabs))]
        public TypeImplicationLab TypeImplication { get; set; }

        [ForeignKey(nameof(ProfessionalIdentificationId))]
        [InverseProperty(nameof(TypeImplicationLab.PersonneMoraleLabs))]
        public ProfessionalIdentificationLab ProfessionalIdentification { get; set; }

        [InverseProperty(nameof(DossierLabPersonneMorale.PersonneMoraleLab))]
        public ICollection<DossierLabPersonneMorale> DossierLabPersonneMorales { get; set; }
        [InverseProperty(nameof(LienPersonneMorale.PersonneMoraleLab))]
        public ICollection<LienPersonneMorale> LienPersonneMorales { get; set; }
        [InverseProperty(nameof(LienPersonnePhysique.PersonneMoraleLab))]
        public ICollection<LienPersonnePhysique> LienPersonnePhysiques { get; set; }
        [InverseProperty(nameof(PersonneMoraleLabLienEntite.PersonneMoraleLab))]
        public ICollection<PersonneMoraleLabLienEntite> PersonneMoraleLabLienEntites { get; set; }
        [InverseProperty(nameof(RepresentantLegal.PersonneMoraleLab))]
        public ICollection<RepresentantLegal> RepresentantLegals { get; set; }
        [InverseProperty(nameof(SupportFinancierPersonneMorale.PersonneMoraleLab))]
        public ICollection<SupportFinancierPersonneMorale> SupportFinancierPersonneMorales { get; set; }
        [InverseProperty(nameof(Dirigeant.PersonneMoraleLab))]
        public ICollection<Dirigeant> Dirigeants { get; set; }
    }
}
