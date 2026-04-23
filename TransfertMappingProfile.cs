using AutoMapper;
using BacaratWeb.Entities.Transfert;
using BacaratWeb.ViewModel.Transfert;

namespace BacaratWeb.Mappers
{
    public class TransfertMappingProfile : Profile
    {
        public TransfertMappingProfile()
        {
            CreateMap<StatutDocument, StatutDocumentViewModel>()
                .ReverseMap();

            CreateMap<Document, DocumentViewModel>()
                .ForMember(dest => dest.OwnerName,
                    opt => opt.MapFrom(src => src.Owner != null ? src.Owner.Nom : string.Empty))
                .ForMember(dest => dest.StatutDocumentName,
                    opt => opt.MapFrom(src => src.StatutDocument != null ? src.StatutDocument.FrenchName : string.Empty));

            CreateMap<DocumentShare, DocumentViewModel>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Document != null ? src.Document.Id : 0))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Document != null ? src.Document.Name : string.Empty))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Document != null ? src.Document.Description : string.Empty))
                .ForMember(dest => dest.ContentType,
                    opt => opt.MapFrom(src => src.Document != null ? src.Document.ContentType : string.Empty))
                .ForMember(dest => dest.FileSize,
                    opt => opt.MapFrom(src => src.Document != null ? src.Document.FileSize : 0))
                .ForMember(dest => dest.UploadDate,
                    opt => opt.MapFrom(src => src.Document != null ? src.Document.UploadDate : default))
                .ForMember(dest => dest.ExpiryDate,
                    opt => opt.MapFrom(src => src.Document != null ? src.Document.ExpiryDate : default))
                .ForMember(dest => dest.OwnerName,
                    opt => opt.MapFrom(src =>
                        src.Document != null && src.Document.Owner != null
                            ? src.Document.Owner.Nom
                            : string.Empty))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.StatutDocumentName,
                    opt => opt.MapFrom(src =>
                        src.Document != null && src.Document.StatutDocument != null
                            ? src.Document.StatutDocument.FrenchName
                            : string.Empty));
        }
    }
}