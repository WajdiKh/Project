using AutoMapper;
using BacaratWeb.Entities.Transfert;
using BacaratWeb.ViewModel.Transfert;

namespace BacaratWeb.Mappers
{
    public class TransfertMappingProfile : Profile
    {
        public TransfertMappingProfile()
        {
            // =============================
            // Référentiel
            // =============================
            CreateMap<StatutDocument, StatutDocumentViewModel>()
                .ReverseMap();

            // =============================
            // Mes documents
            // =============================
            CreateMap<Document, DocumentViewModel>()
                .ForMember(dest => dest.OwnerName,
                    opt => opt.MapFrom(src => src.Owner != null ? src.Owner.Nom : string.Empty))
                .ForMember(dest => dest.StatutDocumentNameFr,
                    opt => opt.MapFrom(src => src.StatutDocument != null ? src.StatutDocument.FrenchName : string.Empty))
                .ForMember(dest => dest.StatutDocumentNameEn,
                    opt => opt.MapFrom(src => src.StatutDocument != null ? src.StatutDocument.EnglishName : string.Empty));

            // =============================
            // Documents partagés avec moi
            // =============================
            CreateMap<DocumentShare, DocumentViewModel>()
                // Document
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Document.Id))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Document.Name))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Document.Description))
                .ForMember(dest => dest.ContentType,
                    opt => opt.MapFrom(src => src.Document.ContentType))
                .ForMember(dest => dest.FileSize,
                    opt => opt.MapFrom(src => src.Document.FileSize))
                .ForMember(dest => dest.UploadDate,
                    opt => opt.MapFrom(src => src.Document.UploadDate))
                .ForMember(dest => dest.ExpiryDate,
                    opt => opt.MapFrom(src => src.Document.ExpiryDate))

                // Propriétaire
                .ForMember(dest => dest.OwnerName,
                    opt => opt.MapFrom(src =>
                        src.Document.Owner != null
                            ? src.Document.Owner.Nom
                            : string.Empty))

                // Destinataire
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))

                // Statut multilingue
                .ForMember(dest => dest.StatutDocumentNameFr,
                    opt => opt.MapFrom(src =>
                        src.Document.StatutDocument != null
                            ? src.Document.StatutDocument.FrenchName
                            : string.Empty))

                .ForMember(dest => dest.StatutDocumentNameEn,
                    opt => opt.MapFrom(src =>
                        src.Document.StatutDocument != null
                            ? src.Document.StatutDocument.EnglishName
                            : string.Empty));
        }
    }
}