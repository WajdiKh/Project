using System;
using System.ComponentModel.DataAnnotations;

namespace BacaratWeb.ViewModel.Transfert
{
    public class ShareDocumentViewModel
    {
        public int? DocumentShareId { get; set; }

        [Required]
        public int DocumentId { get; set; }

        [Required(ErrorMessage = "Transfert.Document.ShareEmailsRequired")]
        [StringLength(2000, ErrorMessage = "Transfert.Document.ShareEmailMaxLength")]
        public string Emails { get; set; }

        [Required(ErrorMessage = "Transfert.Document.ShareStartDateRequired")]
        public DateTimeOffset StartDate { get; set; }

        [Required(ErrorMessage = "Transfert.Document.ShareExpiryDateRequired")]
        public DateTimeOffset ExpiryDate { get; set; }

        public DateTimeOffset DocumentExpiryDate { get; set; }
    }
}