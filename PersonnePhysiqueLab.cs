using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BacaratWeb.Entities.Commun;
using BacaratWeb.Core.DataEncryption.Attributes;

namespace BacaratWeb.Entities.Lab
{
    [Table("PersonnePhysiqueLab", Schema = "Lab")]
    public sealed class PersonnePhysiqueLab
    {
        public PersonnePhysiqueLab()
        {
            ActiviteProfessionnellePersonnePhysiqueLabs = new HashSet<ActiviteProfessionnellePersonnePhysiqueLab>();
            AutreNationalitePersonnePhysiqueLabs = new HashSet<AutreNationalitePersonnePhysiqueLab>();
            CoordonneePersonnePhysiqueLabs = new HashSet<CoordonneePersonnePhysiqueLab>();
            DossierLabPersonnePhysiques = new HashSet<DossierLabPersonnePhysique>();
            LienPersonneMorales = new HashSet<LienPersonneMorale>();
            LienPersonnePhysiques = new HashSet<LienPersonnePhysique>();
            PersonnePhysiqueLabLienEntites = new HashSet<PersonnePhysiqueLabLienEntite>();
            PieceIdentites = new HashSet<PieceIdentite>();
            SupportFinancierPersonnePhysiques = new HashSet<SupportFinancierPersonnePhysique>();
            NationalitePersonnePhysiqueLabs = new HashSet<NationalitePersonnePhysiqueLab>();
            PpeTypePersonnePhysiqueLabs = new HashSet<PpeTypePersonnePhysiqueLab>();
            NationaliteAutreIdentiteLabs = new HashSet<NationaliteAutreIdentiteLab>();
        }

        [Key]
        public int Id { get; set; }
        public int? TypeClientId { get; set; }
        public int? CiviliteId { get; set; }
        [Required]
        [StringLength(400)]
        public string NomNaissance { get; set; }
        public string SoundexNomFr { get; set; }
        public string SoundexNomEn { get; set; }
        [StringLength(400)]
        public string NomUsuel { get; set; }
        public string SoundexNomUsuelFr { get; set; }
        public string SoundexNomUsuelEn { get; set; }
        [Required]
        [StringLength(400)]
        public string Prenoms { get; set; }
        public string SoundexPrenomFr { get; set; }
        public string SoundexPrenomEn { get; set; }
        public int? SexeId { get; set; }
        public int? SituationFamilialeId { get; set; }
        public bool? Ppe { get; set; }
        public DateTimeOffset? DateNaissance { get; set; }
        public int? PaysNaissanceId { get; set; }
        [Encrypted] public byte[] LieuNaissance { get; set; }
      
        public DateTimeOffset? DateEntreeEnRelation { get; set; }
        public int? CanalEntreeEnRelationId { get; set; }
        public bool CessationRelation { get; set; }
        public int? TypeRelationAffaireLabId { get; set; }
        public int? PpeId { get; set; }
        public TypeRelationAffaireLab TypeRelationAffaireLab { get; set; }
        public DateTimeOffset? DateCessationRelations { get; set; }
        public int? SecteurProfessionnelId { get; set; }
        public int? NationaliteId { get; set; }
        public bool IsOccasionnel { get; set; }
        public bool IsHabituel { get; set; }
        public bool CompteEko { get; set; }
        public bool DroitAuCompte { get; set; }
        [StringLength(1000)]
        [Encrypted] public byte[] ElementClesRelation { get; set; }
        [StringLength(1000)]
        [Encrypted] public byte[] SurfaceFinanciere { get; set; }
        [StringLength(50)]
        public string IdentifiantClient { get; set; }
        public bool DetentionCoffre { get; set; }
        
        public string Alias { get; set; }
        public string IdFiscal { get; set; }
        public string DateNaissanceEstimation { get; set; }
        public int? TypeImplicationId { get; set; }



        //IsUtilisationAutreIdentite
        public bool IsUtilisationAutreIdentite { get; set; }
        public bool IsDonneesConnexion { get; set; }
        public string AutreIdentiteNomNaissance { get; set; }
        public string AutreIdentiteNomUsuel { get; set; }
        
        public string AutreIdentitePrenoms { get; set; }
        public string AutreIdentiteAlias { get; set; }
        public int? AutreIdentiteSexeId { get; set; }

        public DateTimeOffset? AutreIdentiteDateNaissance { get; set; }
        public int? AutreIdentitePaysNaissanceId { get; set; }
        public string AutreIdentiteLieuNaissance { get; set; }
        public string AutreIdentiteDateNaissanceEstimation { get; set; }
        public string Profession { get; set; }
        public int? IdentificationProfessionnelleId { get; set; }
        public int? PaysDeRegistreId { get; set; }
        public string AutreIdentificationProfessionnelle { get; set; }
        public string NumeroImmatriculation { get; set; }
        public string DenominationSociale { get; set; }
        public int? FormeJuridiqueId { get; set; }
        public string ActivitePrincipale { get; set; }

        public int? NumeroVoie { get; set; }
        public int? ComplementVoieId { get; set; }
        [StringLength(200)] public string Voie { get; set; }
        [StringLength(200)] public string Ville { get; set; }
        [StringLength(10)] public string CodePostal { get; set; }
        public int? TypeVoieId { get; set; }
        [StringLength(200)] public string Complement { get; set; }
        public int? PaysId { get; set; }
        public int? NatureRelationClientId { get; set; }
        public Guid? IdPersonne { get; set; }
        public int? FigureSanctionId { get; set; }
        public int? ActiviteCriminelleId { get; set; }
        public string FigureSanctionPrecision { get; set; }
        public string ActiviteCriminellePrecision { get; set; }
        public string NotorieteDefavorable { get; set; }



        public int? DossierLabId { get; set; }
        public string CodeDossier { get; set; }


        [ForeignKey(nameof(CanalEntreeEnRelationId))]
        [InverseProperty("PersonnePhysiqueLabs")]
        public CanalEntreeEnRelation CanalEntreeEnRelation { get; set; }
        [ForeignKey(nameof(CiviliteId))]
        [InverseProperty("PersonnePhysiqueLabs")]
        public Civilite Civilite { get; set; }
        [ForeignKey(nameof(NationaliteId))]
        [InverseProperty(nameof(Pays.PersonnePhysiqueLabNationalites))]
        public Pays Nationalite { get; set; }
        [ForeignKey(nameof(PaysNaissanceId))]
        [InverseProperty(nameof(Pays.PersonnePhysiqueLabPaysNaissances))]
        public Pays PaysNaissance { get; set; }
        [ForeignKey(nameof(SecteurProfessionnelId))]
        [InverseProperty("PersonnePhysiqueLabs")]
        public SecteurProfessionnel SecteurProfessionnel { get; set; }
        [ForeignKey(nameof(SexeId))]
        [InverseProperty("PersonnePhysiqueLabs")]
        public Sexe Sexe { get; set; }
        [ForeignKey(nameof(SituationFamilialeId))]
        [InverseProperty("PersonnePhysiqueLabs")]
        public SituationFamiliale SituationFamiliale { get; set; }
        [ForeignKey(nameof(TypeClientId))]
        [InverseProperty(nameof(TypeClientLab.PersonnePhysiqueLabs))]
        public TypeClientLab TypeClient { get; set; }
        [InverseProperty(nameof(ActiviteProfessionnellePersonnePhysiqueLab.PersonnePhysiqueLab))]
        public ICollection<ActiviteProfessionnellePersonnePhysiqueLab> ActiviteProfessionnellePersonnePhysiqueLabs { get; set; }
        [InverseProperty(nameof(AutreNationalitePersonnePhysiqueLab.PersonnePhysiqueLab))]
        public ICollection<AutreNationalitePersonnePhysiqueLab> AutreNationalitePersonnePhysiqueLabs { get; set; }
        [InverseProperty(nameof(CoordonneePersonnePhysiqueLab.PersonnePhysiqueLab))]
        public ICollection<CoordonneePersonnePhysiqueLab> CoordonneePersonnePhysiqueLabs { get; set; }
        [InverseProperty(nameof(DossierLabPersonnePhysique.PersonnePhysiqueLab))]
        public ICollection<DossierLabPersonnePhysique> DossierLabPersonnePhysiques { get; set; }
        [InverseProperty(nameof(LienPersonneMorale.PersonnePhysiqueLab))]
        public ICollection<LienPersonneMorale> LienPersonneMorales { get; set; }
        [InverseProperty(nameof(LienPersonnePhysique.PersonnePhysiqueLab))]
        public ICollection<LienPersonnePhysique> LienPersonnePhysiques { get; set; }
        [InverseProperty(nameof(PersonnePhysiqueLabLienEntite.PersonnePhysiqueLab))]
        public ICollection<PersonnePhysiqueLabLienEntite> PersonnePhysiqueLabLienEntites { get; set; }
        [InverseProperty(nameof(PieceIdentite.PersonnePhysiqueLab))]
        public ICollection<PieceIdentite> PieceIdentites { get; set; }
        [InverseProperty(nameof(SupportFinancierPersonnePhysique.PersonnePhysiqueLab))]
        public ICollection<SupportFinancierPersonnePhysique> SupportFinancierPersonnePhysiques { get; set; }
        [InverseProperty(nameof(NationalitePersonnePhysiqueLab.PersonnePhysiqueLab))]
        public ICollection<NationalitePersonnePhysiqueLab> NationalitePersonnePhysiqueLabs { get; set; }
        [InverseProperty(nameof(NationaliteAutreIdentiteLab.PersonnePhysiqueLab))]
        public ICollection<NationaliteAutreIdentiteLab> NationaliteAutreIdentiteLabs { get; set; }

        
        
        [InverseProperty(nameof(PpeTypePersonnePhysiqueLab.PersonnePhysiqueLab))]
        public ICollection<PpeTypePersonnePhysiqueLab> PpeTypePersonnePhysiqueLabs { get; set; }
    }
}
