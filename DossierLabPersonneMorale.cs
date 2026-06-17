using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BacaratWeb.Entities.Lab
{
    [Table("DossierLabPersonneMorale", Schema = "Lab")]
    public sealed class DossierLabPersonneMorale
    {
       

        [Key]
        public int Id { get; set; }

        public int PersonneMoraleLabId { get; set; }

        public int DossierLabId { get; set; }

        public int? TypeListeCriblageId { get; set; }
        public bool IsDeclarationTracfin { get; set; }

        [ForeignKey(nameof(DossierLabId))]
        [InverseProperty("DossierLabPersonneMorales")]
        public DossierLab DossierLab { get; set; }

        [ForeignKey(nameof(PersonneMoraleLabId))]
        [InverseProperty("DossierLabPersonneMorales")]
        public PersonneMoraleLab PersonneMoraleLab { get; set; }

        [ForeignKey(nameof(TypeListeCriblageId))]
        [InverseProperty("DossierLabPersonneMorales")]
        public TypeListeCriblage TypeListeCriblage { get; set; }


    }
}
