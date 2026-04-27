using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BacaratWeb.ViewModel.Transfert
{
    public class AddDocumentViewModel
    {
        [Required(ErrorMessage = "Le fichier est obligatoire.")]
        public IFormFile File { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Le destinataire est obligatoire.")]
        [EmailAddress(ErrorMessage = "Email invalide.")]
        public string RecipientEmail { get; set; }

        public DateTime? ExpiryDate { get; set; }
    }
}
