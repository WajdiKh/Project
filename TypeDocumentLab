using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BacaratWeb.Entities.Lab
{
    [Table("TypeDocumentLab", Schema = "Lab")]
    public sealed class TypeDocumentLab 
    {
        public TypeDocumentLab()
        {          
            DocumentDossierLabs = new HashSet<DocumentDossierLab>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)] public string Code { get; set; }

        [Required]
        [StringLength(500)] public string FrenchName { get; set; }

        [StringLength(500)] public string EnglishName { get; set; }

        public bool IsActive { get; set; }

        [InverseProperty(nameof(DocumentDossierLab.TypeDocumentLab))]
        public ICollection<DocumentDossierLab> DocumentDossierLabs { get; set; }

    }
}
