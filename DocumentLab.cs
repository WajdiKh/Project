using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BacaratWeb.Core.DataEncryption.Attributes;

namespace BacaratWeb.Entities.Lab
{
    [Table("DocumentLab", Schema = "Lab")]
    public sealed class DocumentLab
    {
        public DocumentLab()
        {
            DocumentDossierLabs = new HashSet<DocumentDossierLab>();
            DocumentDemandeInformationLabs= new HashSet<DocumentDemandeInformationLab>();
          
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(1000)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string FileType { get; set; }

        public long FileSize { get; set; }

        [Required]
        [Encrypted] public byte[] FileContent { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset LastWriteDate { get; set; }

        public DateTimeOffset LastAccessDate { get; set; }

        public bool IsDirectory { get; set; }

        public bool IsOffline { get; set; }

        public bool IsHidden { get; set; }

        public bool IsArchive { get; set; }

        public bool IsSystem { get; set; }

        public bool IsTemporary { get; set; }

        [InverseProperty(nameof(DocumentLab))]
        public ICollection<DocumentDossierLab> DocumentDossierLabs { get; set; }

        [InverseProperty(nameof(DocumentLab))]
        public ICollection<DocumentDemandeInformationLab> DocumentDemandeInformationLabs { get; set; }


    }
}
