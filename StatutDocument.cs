using BacaratWeb.Entities.Commun;
using BacaratWeb.Entities.Contracts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BacaratWeb.Entities.Transfert
{
    [Table("StatutDocument", Schema = "Transfert")]
    public sealed class StatutDocument : Translatable
    {
        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset? DateModification { get; set; }

        public int? ModificateurId { get; set; }

        public int? CreateurId { get; set; }

        [ForeignKey(nameof(CreateurId))]
        public Utilisateur Createur { get; set; }

        [ForeignKey(nameof(ModificateurId))]
        public Utilisateur Modificateur { get; set; }

        public bool IsActive { get; set; }
    }
}