using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BacaratWeb.Entities.Commun
{
    [Table("EmailNotificationType", Schema = "Commun")]
    public sealed class EmailNotificationType
    {
        public EmailNotificationType() => EmailNotificationTemplates = new HashSet<EmailNotificationTemplate>();

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(200)]
        public string Libelle { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset? DateModification { get; set; }

        public int? ModificateurId { get; set; }

        public int? CreateurId { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [ForeignKey(nameof(CreateurId))]
        [InverseProperty("EmailNotificationTypeCreateurs")]
        public Utilisateur Createur { get; set; }

        [ForeignKey(nameof(ModificateurId))]
        [InverseProperty("EmailNotificationTypeModificateurs")]
        public Utilisateur Modificateur { get; set; }

        [InverseProperty(nameof(EmailNotificationType))]
        public ICollection<EmailNotificationTemplate> EmailNotificationTemplates { get; set; }
    }
}
