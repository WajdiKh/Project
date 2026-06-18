using BacaratWeb.Models.Referentials;

namespace BacaratWeb.Areas.Lab.Models
{
    public sealed class DossierLabPersonneMoraleViewModel
    {
        public int Id { get; set; }
        public int PersonneMoraleLabId { get; set; }
        public int DossierLabId { get; set; }
        public int? TypeListeCriblageId { get; set; }
        public bool IsDeclarationTracfin { get; set; }

        public EVisibleDossier EVisibleDossier { get; set; }
        public string Culture { get; set; }
        public int? DirectionId { get; set; }
        public bool ToDelete { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsNew { get; set; }
        public bool MoralPersonIsAlreadyImported { get; set; }
        public PersonneMoraleLabViewModel PersonneMoraleLab { get; set; } = new();
        public string CryptedId { get; set; }
        public string CryptedPersonneMoraleLabId { get; set; }
        public string CryptedDossierLabId { get; set; }
        public bool showIsDeclarationTracfin { get; set; }
    }
}
