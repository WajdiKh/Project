using BacaratWeb.Entities.Commun;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BacaratWeb.Entities.Transfert
{
    [Table("Document", Schema = "Transfert")]
    public sealed class Document
    {
        public Document()
        {
            DocumentShares = new HashSet<DocumentShare>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [StringLength(255)]
        public string OriginalFileName { get; set; }

        [Required]
        [StringLength(20)]
        public string FileExtension { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }

        [Required]
        public byte[] FileContent { get; set; }

        public long FileSize { get; set; }

        public DateTimeOffset UploadDate { get; set; }

        public DateTimeOffset ExpiryDate { get; set; }

        public int OwnerId { get; set; }

        public int StatutDocumentId { get; set; }

        [Required]
        [StringLength(6)]
        public string SecurityCode { get; set; }

        [StringLength(255)]
        public string EncryptionKey { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public Utilisateur Owner { get; set; }

        [ForeignKey(nameof(StatutDocumentId))]
        public StatutDocument StatutDocument { get; set; }

        public ICollection<DocumentShare> DocumentShares { get; set; }
    }
}