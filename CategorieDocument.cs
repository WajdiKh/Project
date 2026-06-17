using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BacaratWeb.Entities.AvisIm;
using BacaratWeb.Entities.AvisSi;
using BacaratWeb.Entities.Contracts;
using BacaratWeb.Entities.Escalade;
using BacaratWeb.Entities.Gda;
using BacaratWeb.Entities.Lab;
using BacaratWeb.Entities.SAvis;

namespace BacaratWeb.Entities.Commun
{
    [Table("CategorieDocument", Schema = "Commun")]
    public sealed class CategorieDocument : Translatable
    {

        public CategorieDocument()
        {
            DocumentDossierEscalades = new HashSet<DocumentDossierEscalade>();
            DocumentDossierAvisSis = new HashSet<DocumentDossierAvisSi>();
            DocumentDossierSAviss = new HashSet<DocumentDossierSAvis>();
            DocumentDossierAvisIms = new HashSet<DocumentDossierAvisIm>();
            DocumentDossierLabs = new HashSet<DocumentDossierLab>();
            DocumentsDossierGda = new HashSet<DocumentDossierGda>();
        }
        public DateTimeOffset DateCreation { get; set; }
        public DateTimeOffset? DateModification { get; set; }
        public int? ModificateurId { get; set; }
        public int? CreateurId { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public int? ActiviteId { get; set; }

        [ForeignKey(nameof(CreateurId))]
        [InverseProperty(nameof(Utilisateur.CategorieDocumentCreateurs))]
        public Utilisateur Createur { get; set; }
        [ForeignKey(nameof(ModificateurId))]
        [InverseProperty(nameof(Utilisateur.CategorieDocumentModificateurs))]
        public Utilisateur Modificateur { get; set; }
        [InverseProperty(nameof(DocumentDossierEscalade.CategorieDocument))]
        public ICollection<DocumentDossierEscalade> DocumentDossierEscalades { get; set; }

        [InverseProperty(nameof(DocumentDossierAvisSi.CategorieDocument))]
        public ICollection<DocumentDossierAvisSi> DocumentDossierAvisSis { get; set; }

        [InverseProperty(nameof(DocumentDossierSAvis.CategorieDocument))]
        public ICollection<DocumentDossierSAvis> DocumentDossierSAviss { get; set; }

        [InverseProperty(nameof(DocumentDossierAvisIm.CategorieDocument))]
        public  ICollection<DocumentDossierAvisIm> DocumentDossierAvisIms { get; set; }
        
        [InverseProperty(nameof(DocumentDossierLab.CategorieDocument))]
        public  ICollection<DocumentDossierLab> DocumentDossierLabs { get; set; }
        
        [InverseProperty(nameof(DocumentDossierGda.CategorieDocument))]
        public ICollection<DocumentDossierGda> DocumentsDossierGda { get; set; }
    }
}
