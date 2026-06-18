using BacaratWeb.Models.Referentials;

namespace BacaratWeb.Areas.Lab.Models
{
    public sealed class DossierLabPersonnePhysiqueViewModel
    {        
        public DossierLabPersonnePhysiqueViewModel()
        {
            PersonnePhysiqueLab = new PersonnePhysiqueLabViewModel();
        }
        public int Id { get; set; }
        public int PersonnePhysiqueLabId { get; set; }
        public int DossierLabId { get; set; }
        public int? TypeListeCriblageId { get; set; }
        public bool IsDeclarationTracfin { get; set; }

        public EVisibleDossier EVisibleDossier { get; set; }
        public string Culture { get; set; }
        public int? DirectionId { get; set; }
        public string CryptedPersonnePhysiqueLabId { get; set; }
        public string CryptedDossierLabId { get; set; }
        public string CryptedId { get; set; }
        public bool ToDelete { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsNew { get; set; }
        public bool PhysicalPersonIsAlreadyImported { get; set; }

        public PersonnePhysiqueLabViewModel PersonnePhysiqueLab { get; set; }

        public bool showIsDeclarationTracfin { get; set; }
    }
}
