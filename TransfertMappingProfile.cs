using AutoMapper;
using BacaratWeb.Entities.Transfert;
using BacaratWeb.ViewModel.Transfert;
using System;

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

            CreateMap<Document, DocumentViewModel>()
                .ForMember(dest => dest.OwnerName,
                    opt => opt.MapFrom(src =>
                        src.Owner != null
                            ? ((src.Owner.Prenom ?? string.Empty) + " " + (src.Owner.Nom ?? string.Empty).ToUpper())
                            : string.Empty))
                .ForMember(dest => dest.ContentType,
                           opt => opt
                            .MapFrom(src => string.IsNullOrWhiteSpace(src.FileExtension) ? string.Empty : src.FileExtension.Replace(".", string.Empty).ToLower()))
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.ExpiryDate < DateTimeOffset.Now));

            CreateMap<DocumentShare, DocumentViewModel>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Document.Id))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Document.Name))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Document.Description))
                .ForMember(dest => dest.ContentType, 
                           opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Document.FileExtension) ? string.Empty : src.Document.FileExtension.Replace(".", string.Empty).ToLower()))
                .ForMember(dest => dest.FileSize,
                    opt => opt.MapFrom(src => src.Document.FileSize))
                .ForMember(dest => dest.UploadDate,
                    opt => opt.MapFrom(src => src.Document.UploadDate))
                .ForMember(dest => dest.ExpiryDate,
                    opt => opt.MapFrom(src => src.Document.ExpiryDate))
                .ForMember(dest => dest.OwnerName,
                    opt => opt.MapFrom(src =>
                        src.Document.Owner != null
                            ? ((src.Document.Owner.Prenom ?? string.Empty) + " " + (src.Document.Owner.Nom ?? string.Empty).ToUpper())
                            : string.Empty))
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.Document.ExpiryDate < DateTimeOffset.Now));


            CreateMap<DocumentShare, DocumentShareViewModel>()
                .ForMember(dest => dest.CreateurName,
                           opt => opt.MapFrom(src => src.Createur != null ?
                           ((src.Createur.Prenom ?? string.Empty) + " " + (src.Createur.Nom ?? string.Empty).ToUpper()) :
                           string.Empty));

        }
    }
}
