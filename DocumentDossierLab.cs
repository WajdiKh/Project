using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BacaratWeb.Entities.Commun;

namespace BacaratWeb.Entities.Lab
{
    [Table("DocumentDossierLab", Schema = "Lab")]
    public sealed class DocumentDossierLab
    {
        [Key]
        public int Id { get; set; }

        public int DocumentLabId { get; set; }

        public int DossierLabId { get; set; }

        public bool TransmettreTracfin { get; set; }

        [Required]
        [StringLength(1000)]
        public string DocumentName { get; set; }

        [Required]
        [StringLength(1000)]
        public string FileName { get; set; }

        public string Precision_if_OtherType { get; set; }
        public string IdentityNumber { get; set; }

        public int? TypeDocumentLabId { get; set; }
        public DateTimeOffset? DateDocument { get; set; }
        public string AuthorityRelease { get; set; }

        public int? CountryRelaseId { get; set; }
        public DateTimeOffset? DateValidity { get; set; }
        public string SourceFileDocument { get; set; }
        public bool? LevelCNI { get; set; }

        public Guid? PersonneAssocieId { get; set; }
        public bool IsDeleted { get; set; }

        public int? CategorieDocumentId { get; set; }

        [ForeignKey(nameof(DocumentLabId))]
        [InverseProperty("DocumentDossierLabs")]
        public DocumentLab DocumentLab { get; set; }

        [ForeignKey(nameof(DossierLabId))]
        [InverseProperty("DocumentDossierLabs")]
        public DossierLab DossierLab { get; set; }

        [ForeignKey(nameof(CategorieDocumentId))]
        [InverseProperty("DocumentDossierLabs")]
        public CategorieDocument CategorieDocument { get; set; }


        [ForeignKey(nameof(TypeDocumentLabId))]
        [InverseProperty("DocumentDossierLabs")]
        public TypeDocumentLab TypeDocumentLab { get; set; }

        [ForeignKey(nameof(CountryRelaseId))]
        [InverseProperty("DocumentDossierLabs")]
        public Pays CountryRelase { get; set; }
    }
}
