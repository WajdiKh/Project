using BacaratWeb.Models.Referentials;
using System;

namespace BacaratWeb.ViewModel.Transfert
{
    [Serializable]
    public class StatutDocumentViewModel : TranslatableViewModel
    {
        public bool IsActive { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset? DateModification { get; set; }

        public int? CreateurId { get; set; }

        public int? ModificateurId { get; set; }
    }
}