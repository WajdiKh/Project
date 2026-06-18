using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BacaratWeb.Models.Referentials;
using BacaratWeb.Providers;

namespace BacaratWeb.Areas.Lab.Models
{
    public sealed class PersonnePhysiqueLabViewModel
    {
        public PersonnePhysiqueLabViewModel()
        {
            AutreNationalitePersonnePhysiqueLabs = new List<AutreNationalitePersonnePhysiqueLabViewModel>();
            CoordonneePersonnePhysiqueLabs = new List<CoordonneePersonnePhysiqueLabViewModel>();
            LienPersonneMorales = new List<LienPersonneMoraleViewModel>();
            LienPersonnePhysiques = new List<LienPersonnePhysiqueViewModel>();
            PieceIdentites = new List<PieceIdentiteViewModel>();
            SupportFinancierPersonnePhysiques = new List<SupportFinancierPersonnePhysiqueViewModel>();
            ActiviteProfessionnellePersonnePhysiqueLabs = new List<ActiviteProfessionnellePersonnePhysiqueLabViewModel>();
        }

        public int? CiviliteId { get; set; }

        public DateTime? DateCessationRelations { get; set; }

        public DateTime? DateNaissance { get; set; }

        public int Id { get; set; }
        public string CryptedId { get; set; }

        public string Lang { get; set; }

        public List<LienPersonneMoraleViewModel> LienPersonneMorales { get; set; }

        public List<LienPersonnePhysiqueViewModel> LienPersonnePhysiques { get; set; }

        public List<CoordonneePersonnePhysiqueLabViewModel> CoordonneePersonnePhysiqueLabs { get; set; }
        public List<AutreNationalitePersonnePhysiqueLabViewModel> AutreNationalitePersonnePhysiqueLabs { get; set; }
        public List<ActiviteProfessionnellePersonnePhysiqueLabViewModel> ActiviteProfessionnellePersonnePhysiqueLabs { get; set; }

        public string LieuNaissance { get; set; }

        public int? NationaliteId { get; set; }

        [RequiredStringWithCustomTranslatedErrorMessage(_fieldFr = "le nom de la personne physique", _fieldEn = "the name of the natural person")][StringLength(120)] public string NomNaissance { get; set; }
        [StringLength(120)] public string NomUsuel { get; set; }

        public int? PaysNaissanceId { get; set; }

        public List<PieceIdentiteViewModel> PieceIdentites { get; set; }

        [RequiredStringWithCustomTranslatedErrorMessage(_fieldFr = "le prénom de la personne physique", _fieldEn = "the first name of the natural person")][StringLength(120)] public string Prenoms { get; set; }

        public int? SecteurProfessionnelId { get; set; }

        public int? SexeId { get; set; }

        public int? SituationFamilialeId { get; set; }

        public List<SupportFinancierPersonnePhysiqueViewModel> SupportFinancierPersonnePhysiques { get; set; }

        public bool? Ppe { get; set; }
        [StringLength(1000)] public string SurfaceFinanciere { get; set; }

        public int? TypeClientId { get; set; }

        public int? NatureRelationClientId { get; set; }
        public DateTime? DateEntreeEnRelation { get; set; }
        public int? CanalEntreeEnRelationId { get; set; }
        public bool CessationRelation { get; set; }
        public int? TypeRelationAffaireLabId { get; set; }
        public TypeRelationAffaireLabViewModel TypeRelationAffaireLab { get; set; }
        public bool DetentionCoffre { get; set; }
        [StringLength(700)] public string ElementClesRelation { get; set; }
        public bool IsHabituel { get; set; }
        public bool IsOccasionnel { get; set; }
        [StringLength(50)] public string IdentifiantClient { get; set; }
        public bool CompteEko { get; set; }
        public bool DroitAuCompte { get; set; }



        public IList<PaysViewModel> Nationalites { get; set; }
        public IList<PaysViewModel> NationalitesToDisplay { get; set; }
        public IList<int> NationalitesId { get; set; }


        public IList<PpeTypeViewModel> PpeTypesToDisplay { get; set; }
        public IList<int> PpeTypesId { get; set; }
        public int? PpeId { get; set; }
        [StringLength(400)]
        public string Alias { get; set; }
        [StringLength(400)]
        public string IdFiscal { get; set; }
        [StringLength(400)]
        public string DateNaissanceEstimation { get; set; }
        public int? TypeImplicationId { get; set; }


        //IsUtilisationAutreIdentite
        public int? IsUtilisationAutreIdentite { get; set; }
        public int? IsDonneesConnexion { get; set; }
        [StringLength(120)] public string AutreIdentiteNomNaissance { get; set; }
        [StringLength(120)] public string AutreIdentiteNomUsuel { get; set; }

        [StringLength(120)] public string AutreIdentitePrenoms { get; set; }
        [StringLength(400)]
        public string AutreIdentiteAlias { get; set; }
        public int? AutreIdentiteSexeId { get; set; }

        public DateTime? AutreIdentiteDateNaissance { get; set; }
        public int? AutreIdentitePaysNaissanceId { get; set; }
        [StringLength(400)]
        public string AutreIdentiteLieuNaissance { get; set; }
        public IList<PaysViewModel> AutreIdentiteNationalitesToDisplay { get; set; }
        public IList<int> AutreIdentiteNationalitesId { get; set; }
        [StringLength(400)]
        public string AutreIdentiteDateNaissanceEstimation { get; set; }
        [StringLength(400)]
        public string Profession { get; set; }
        public int? IdentificationProfessionnelleId { get; set; } = 10;
        [StringLength(400)]
        public string AutreIdentificationProfessionnelle { get; set; }
        [StringLength(64)]
        public string NumeroImmatriculation { get; set; }
        [StringLength(64)]
        public string DenominationSociale { get; set; }
        public int? PaysDeRegistreId { get; set; }
        public int? FormeJuridiqueId { get; set; }
        [StringLength(400)]
        public string ActivitePrincipale { get; set; }
        public int? NumeroVoie { get; set; }
        public int? ComplementVoieId { get; set; }
        [StringLength(200)][Required] public string Voie { get; set; }
        [StringLength(200)][Required] public string Ville { get; set; }
        [StringLength(10)] public string CodePostal { get; set; }
        public int? TypeVoieId { get; set; }
        [StringLength(32)] public string Complement { get; set; }
        public int? PaysId { get; set; }
        public bool IsReadOnly { get; set; }

        public int? FigureSanctionId { get; set; } = 2;
        public int? ActiviteCriminelleId { get; set; } = 3;
        [StringLength(400)]
        public string FigureSanctionPrecision { get; set; }
        [StringLength(400)]
        public string ActiviteCriminellePrecision { get; set; }
        [StringLength(700)]
        public string NotorieteDefavorable { get; set; }
        public DateTime? DateNaissanceDisplay
        {
            get
            {
                if (DateNaissance == default)
                    return null;
                return DateNaissance;
            }
        }
        public Guid? IdPersonne { get; set; } = Guid.NewGuid();
        public string TypeClient { get; set; }
        public string TypeImplication { get; set; }
        public string PaysNaissance { get; set; }
        public string AutrePaysNaissance { get; set; }
        public string ClientId { get; set; }
        public string Sexe { get; set; }
        public string AutreSexe { get; set; }
        public string Civilite { get; set; }
        public string SituationFamiliale { get; set; }
        public string PpeType { get; set; }
        public string CanalEntreeEnRelation { get; set; }
        public string NatureRelationClient { get; set; }
        public string TypeCriblage { get; set; }
        public string RelationAffaireLab { get; set; }
        public string SecteurProfessionnel { get; set; }
        public string FormeJuridique { get; set; }
        public string PaysDeRegistre {  get; set; }
        public string AutreNationalite { get; set; }
        public string TypeVoie { get; set; }
        public string ComplementVoie { get; set; }
        public string PaysAdresse { get; set; }
        public string IdentificationProfessionnelle { get; set; }
        public int? DossierLabId { get; set; }
        public string CodeDossier { get; set; }
    }
}
