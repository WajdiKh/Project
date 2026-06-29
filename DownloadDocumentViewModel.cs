using System.ComponentModel.DataAnnotations;

namespace BacaratWeb.ViewModel.Transfert
{
    public class DownloadDocumentViewModel
    {
        [Required]
        public int DocumentId { get; set; }

        [Required(ErrorMessage = "Transfert.Document.SecurityCodeRequired")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Transfert.Document.InvalidSecurityCode")]
        public string SecurityCode { get; set; }
    }
}