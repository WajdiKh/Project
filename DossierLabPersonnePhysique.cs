using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BacaratWeb.Entities.Lab
{
    [Table("DossierLabPersonnePhysique", Schema = "Lab")]
    public sealed class DossierLabPersonnePhysique
    {
        [Key]
        public int Id { get; set; }
        public int PersonnePhysiqueLabId { get; set; }
        public int DossierLabId { get; set; }
        public int? TypeListeCriblageId { get; set; }
        public bool IsDeclarationTracfin { get; set; }

        [ForeignKey(nameof(DossierLabId))]
        [InverseProperty("DossierLabPersonnePhysiques")]
        public DossierLab DossierLab { get; set; }

        [ForeignKey(nameof(PersonnePhysiqueLabId))]
        [InverseProperty("DossierLabPersonnePhysiques")]
        public PersonnePhysiqueLab PersonnePhysiqueLab { get; set; }

        [ForeignKey(nameof(TypeListeCriblageId))]
        [InverseProperty("DossierLabPersonnePhysiques")]
        public TypeListeCriblage TypeListeCriblage { get; set; }
    }
}
