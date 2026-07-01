using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BacaratWeb.ViewModel.Transfert
{
    public class AddDocumentViewModel
    {
        [Required(ErrorMessage = "Transfert.Document.FileRequired")]
        public IFormFile File { get; set; }

        [Required(ErrorMessage = "Transfert.Document.NameRequired")]
        [StringLength(255, ErrorMessage = "Transfert.Document.NameMaxLength")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "Transfert.Document.DescriptionMaxLength")]
        public string Description { get; set; }

        [StringLength(255, ErrorMessage = "Transfert.Document.EncryptionKeyMaxLength")]
        public string EncryptionKey { get; set; }

        [Range(24, 72, ErrorMessage = "Transfert.Document.ExpiryDelayHoursRange")]
        public int ExpiryDelayHours { get; set; } = 48;
        
        [Required(ErrorMessage = "Transfert.Document.ShareEmailRequired")]
        public string Emails { get; set; }
    }
}
