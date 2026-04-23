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
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Nom))
                .ForMember(dest => dest.StatutDocumentName, opt => opt.MapFrom(src => src.StatutDocument.FrenchName));

            CreateMap<DocumentShare, DocumentViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Document.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Document.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Document.Description))
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.Document.ContentType))
                .ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.Document.FileSize))
                .ForMember(dest => dest.UploadDate, opt => opt.MapFrom(src => src.Document.UploadDate))
                .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.Document.ExpiryDate))
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Document.Owner.Nom))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.StatutDocumentName, opt => opt.MapFrom(src => src.Document.StatutDocument.FrenchName));
        }
    }
}