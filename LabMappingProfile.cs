using System;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using AutoMapper;
using BacaratWeb.Areas.Lab.Models;
using BacaratWeb.Areas.Lab.Models.Delegation;
using BacaratWeb.Core.Extensions;
using BacaratWeb.Entities.Commun;
using BacaratWeb.Entities.Contracts;
using BacaratWeb.Entities.Extensions;
using BacaratWeb.Entities.Lab;
using BacaratWeb.Models.Referentials;
using BacaratWeb.Shared;
using BacaratWeb.ViewModel.Commun.Referentials;
using BacaratWeb.ViewModel.Lab;

namespace BacaratWeb.Mappers
{
	public class LabMappingProfile : Profile
    {
        public LabMappingProfile()
        {

            CreateMap<Confidentiel, ConfidentielViewModel>().ReverseMap();
            CreateMap<DossierLabQLBResult, DossierLabQLBResultViewModel>();
            CreateMap<InformationRequestCloseReason, InformationRequestCloseReasonViewModel>().ReverseMap();
            CreateMap<DossierLabRow, DossierLabRowViewModel>()
                .ForMember(dest => dest.DateDeclaration, opt => opt.MapFrom(src => src.DateDeclaration == null ? (DateTime?)null : src.DateDeclaration.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification == null ? (DateTime?)null : src.DateModification.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.LastDateRelance, opt => opt.MapFrom(src => src.LastDateRelance == null ? (DateTime?)null : src.LastDateRelance.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateReception, opt => opt.MapFrom(src => src.DateReception == null ? (DateTime?)null : src.DateReception.Value.Date))
                .ForMember(dest => dest.DateReponse, opt => opt.MapFrom(src => src.DateReponse == null ? (DateTime?)null : src.DateReponse.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateSaisie, opt => opt.MapFrom(src => src.DateSaisie == null ? (DateTime?)null : src.DateSaisie.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateCloture, opt => opt.MapFrom(src => src.DateCloture == null ? (DateTime?)null : src.DateCloture.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateDerniereDeclarationDgt, opt => opt.MapFrom(src => src.DateDerniereDeclarationDgt == null ? (DateTime?)null : src.DateDerniereDeclarationDgt.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateRuptureRelation, opt => opt.MapFrom(src => src.DateRuptureRelation == null ? (DateTime?)null : src.DateRuptureRelation.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateTier, opt => opt.MapFrom(src => src.DateTier == null ? (DateTime?)null : src.DateTier.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.FinPeriodeFaitsConsideres, opt => opt.MapFrom(src => src.FinPeriodeFaitsConsideres == null ? (DateTime?)null : src.FinPeriodeFaitsConsideres.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DebutPeriodeFaitsConsideres, opt => opt.MapFrom(src => src.DebutPeriodeFaitsConsideres == null ? (DateTime?)null : src.DebutPeriodeFaitsConsideres.GetValueOrDefault().DateTime)).ForMember(dest => dest.FinPeriodeFaitsConsideres, opt => opt.MapFrom(src => src.FinPeriodeFaitsConsideres == null ? (DateTime?)null : src.FinPeriodeFaitsConsideres.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateCessationRelations, opt => opt.MapFrom(src => src.DateCessationRelations == null ? (DateTime?)null : src.DateCessationRelations.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateEntreeEnRelation, opt => opt.MapFrom(src => src.DateEntreeEnRelation == null ? (DateTime?)null : src.DateEntreeEnRelation.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.IsNoResponse, opt => opt.MapFrom(src =>
                    src.DemandeInformationLabs.Any()
                        ? src.DemandeInformationLabs.Any(y => y.StatutDemandeInformationLabId == 1 || y.StatutDemandeInformationLabId == 3)
                            ? 1 : 0 : 0))
                .ForMember(dest => dest.IsWaitingRead, opt => opt.MapFrom(src =>
                    src.DemandeInformationLabs.Any()
                        ? src.DemandeInformationLabs.Any(y => y.StatutDemandeInformationLabId == 4 && !y.IsResponse)
                            ? 1 : 0 : 0))
                .ForMember(dest => dest.DateEnvoiDeclaration, opt => opt.MapFrom(src => src.DateEnvoiDeclaration == null ? (DateTime?)null : src.DateEnvoiDeclaration.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateEnregistrementTracfin, opt => opt.MapFrom(src => src.DateEnregistrementTracfin == null ? (DateTime?)null : src.DateEnregistrementTracfin.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateRequestARTracfin, opt => opt.MapFrom(src => src.DateRequestARTracfin == null ? (DateTime?)null : src.DateRequestARTracfin.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateReponseDemandeInfo, opt => opt.MapFrom(src => src.DateReponseDemandeInfo == null ? (DateTime?)null : src.DateReponseDemandeInfo.GetValueOrDefault().DateTime))
                .ReverseMap()
                .ForMember(dest => dest.DateDeclaration, opt => opt.MapFrom(src => src.DateDeclaration == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateDeclaration.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateModification.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.LastDateRelance, opt => opt.MapFrom(src => src.LastDateRelance == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.LastDateRelance.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateReception, opt => opt.MapFrom(src => src.DateReception == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateReception.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateDerniereDeclarationDgt, opt => opt.MapFrom(src => src.DateDerniereDeclarationDgt == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateDerniereDeclarationDgt.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateReponse, opt => opt.MapFrom(src => src.DateReponse == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateReponse.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateSaisie, opt => opt.MapFrom(src => src.DateSaisie == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateSaisie.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateCloture, opt => opt.MapFrom(src => src.DateCloture == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateCloture.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateRuptureRelation, opt => opt.MapFrom(src => src.DateRuptureRelation == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateRuptureRelation.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateTier, opt => opt.MapFrom(src => src.DateTier == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateTier.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.FinPeriodeFaitsConsideres, opt => opt.MapFrom(src => src.FinPeriodeFaitsConsideres == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.FinPeriodeFaitsConsideres.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DebutPeriodeFaitsConsideres, opt => opt.MapFrom(src => src.DebutPeriodeFaitsConsideres == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DebutPeriodeFaitsConsideres.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateCessationRelations, opt => opt.MapFrom(src => src.DateCessationRelations == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateCessationRelations.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateEntreeEnRelation, opt => opt.MapFrom(src => src.DateEntreeEnRelation == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateEntreeEnRelation.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateEnvoiDeclaration, opt => opt.MapFrom(src => src.DateEnvoiDeclaration == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateEnvoiDeclaration.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateEnregistrementTracfin, opt => opt.MapFrom(src => src.DateEnregistrementTracfin == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateEnregistrementTracfin.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateRequestARTracfin, opt => opt.MapFrom(src => src.DateRequestARTracfin == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateRequestARTracfin.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateReponseDemandeInfo, opt => opt.MapFrom(src => src.DateReponseDemandeInfo == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateReponseDemandeInfo.GetValueOrDefault(), DateTimeKind.Utc)));

            CreateMap<PersonnePhysiqueItemLab, PersonnePhysiqueItemLabViewModel>()
                .ForMember(dest => dest.DateNaissance, opt => opt.MapFrom(src => src.DateNaissance == null ? (DateTime?)null : src.DateNaissance.GetValueOrDefault().DateTime))
                .ReverseMap()
                .ForMember(dest => dest.DateNaissance, opt => opt.MapFrom(src => src.DateNaissance == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateNaissance.GetValueOrDefault(), DateTimeKind.Utc)));
            CreateMap<PersonneMoraleItemLab, PersonneMoraleItemLabViewModel>()
                .ForMember(dest => dest.DateImmatriculation, opt => opt.MapFrom(src => src.DateImmatriculation == null ? (DateTime?)null : src.DateImmatriculation.GetValueOrDefault().DateTime))
                .ReverseMap()
                .ForMember(dest => dest.DateImmatriculation, opt => opt.MapFrom(src => src.DateImmatriculation == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateImmatriculation.GetValueOrDefault(), DateTimeKind.Utc)));

            CreateMap<DossierLab, DossierLabViewModel>()
                .ForMember(dest => dest.DateCreation, opt => opt.MapFrom(src => src.DateCreation == null ? (DateTime?)null : src.DateCreation.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification == null ? (DateTime?)null : src.DateModification.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateReception, opt => opt.MapFrom(src => src.DateReception == null ? (DateTime?)null : src.DateReception.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateReponse, opt => opt.MapFrom(src => src.DateReponse == null ? (DateTime?)null : src.DateReponse.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateCloture, opt => opt.MapFrom(src => src.DateCloture == null ? (DateTime?)null : src.DateCloture.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.PresentationComiteDate, opt => opt.MapFrom(src => src.PresentationComiteDate == null ? (DateTime?)null : src.PresentationComiteDate.GetValueOrDefault().DateTime))
                 .ForMember(dest => dest.DateDeclarationLocale, opt => opt.MapFrom(src => src.DateDeclarationLocale == null ? (DateTime?)null : src.DateDeclarationLocale.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateDerniereDeclarationDgt, opt => opt.MapFrom(src => src.DateDerniereDeclarationDgt == null ? (DateTime?)null : src.DateDerniereDeclarationDgt.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.MotifsSoupcons,
                    opt => opt.MapFrom(src =>
                        Encoding.UTF8.GetString(src.MotifsSoupcons) != "Null"
                            ? Encoding.UTF8.GetString(src.MotifsSoupcons)
                            : string.Empty))

                 .ForMember(dest => dest.Createur_FullName,
                    opt => opt.MapFrom(src => $"{src.Utilisateur.Nom.ToUpper(CultureInfo.CurrentCulture)} {src.Utilisateur.Prenom}"))
                .ForMember(dest => dest.Modificateur_FullName,
                    opt => opt.MapFrom(src => $"{src.Modificateur.Nom.ToUpper(CultureInfo.CurrentCulture)} {src.Modificateur.Prenom}"))
                .ReverseMap()
                .ForMember(dest => dest.DateCreation, opt => opt.MapFrom(src => src.DateCreation == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateCreation.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateModification.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateReception, opt => opt.MapFrom(src => src.DateReception == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateReception.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateReponse, opt => opt.MapFrom(src => src.DateReponse == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateReponse.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateCloture, opt => opt.MapFrom(src => src.DateCloture == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateCloture.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.PresentationComiteDate, opt => opt.MapFrom(src => src.PresentationComiteDate == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.PresentationComiteDate.GetValueOrDefault(), DateTimeKind.Utc)))
                 .ForMember(dest => dest.DateDeclarationLocale, opt => opt.MapFrom(src => src.DateDeclarationLocale == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateDeclarationLocale.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateDerniereDeclarationDgt, opt => opt.MapFrom(src => src.DateDerniereDeclarationDgt == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateDerniereDeclarationDgt.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.MotifsSoupcons,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.MotifsSoupcons)));

            CreateMap<Profession, ProfessionViewModel>()
             .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
             .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
             .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
             .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription)).ReverseMap();
            CreateMap<Profession, SelectedItem>()
              .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
              .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
              .ReverseMap();

            //CreateMap<DossierLabHisto, DossierLabHistoViewModel>().ReverseMap();
            CreateMap<DossierLab, DossierLabHisto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => 0));
            CreateMap<Coordonnee, CoordonneeViewModel>().ReverseMap();
            CreateMap<CoordonneePersonnePhysiqueLab, CoordonneePersonnePhysiqueLabViewModel>().ReverseMap();
            CreateMap<AutreNationalitePersonnePhysiqueLab, AutreNationalitePersonnePhysiqueLabViewModel>().ReverseMap();
            CreateMap<FormeJuridique, FormeJuridiqueViewModel>().ReverseMap();
            CreateMap<TypeCompte, TypeCompteViewModel>().ReverseMap();
            CreateMap<LienPersonneMorale, LienPersonneMoraleViewModel>().ForMember(dest => dest.RaisonSociale,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.RaisonSociale) != "Null"
                        ? Encoding.UTF8.GetString(src.RaisonSociale)
                        : string.Empty))
                .ForMember(dest => dest.Immatriculation,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.Immatriculation) != "Null"
                        ? Encoding.UTF8.GetString(src.Immatriculation)
                        : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.RaisonSociale,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.RaisonSociale)))
                .ForMember(dest => dest.Immatriculation,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Immatriculation)));
            CreateMap<LienPersonnePhysique, LienPersonnePhysiqueViewModel>()
                .ForMember(dest => dest.DateNaissance, opt => opt.MapFrom(src => src.DateNaissance == null ? (DateTime?)null : src.DateNaissance.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.NomNaissance,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.NomNaissance) != "Null"
                        ? Encoding.UTF8.GetString(src.NomNaissance)
                        : string.Empty))
                .ForMember(dest => dest.Prenom,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.Prenom) != "Null"
                        ? Encoding.UTF8.GetString(src.Prenom)
                        : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.DateNaissance, opt => opt.MapFrom(src => src.DateNaissance == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateNaissance.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.NomNaissance,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.NomNaissance)))
                .ForMember(dest => dest.Prenom,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Prenom)));
            CreateMap<TypeLienSupport, TypeLienSupportViewModel>().ReverseMap();
            CreateMap<TypeLienPersonnePhysiquePhysique, TypeLienPersonnePhysiqueViewModel>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
               .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
               .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
               .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription)).ReverseMap();
            CreateMap<TypeLienPersonneMoralePhysique, TypeLienPersonneMoralePhysiqueLabViewModel>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
               .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
               .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
               .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription)).ReverseMap();
            CreateMap<TypeLienPersonnePhysiqueMorale, TypeLienPersonnePhysiqueMoraleLabViewModel>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
               .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
               .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
               .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription)).ReverseMap();
            CreateMap<TypeLienPersonneMoraleMorale, TypeLienPersonneMoraleViewModel>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
               .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
               .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
               .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription)).ReverseMap();
            CreateMap<TypeLienPersonneMoralePhysique, SelectedItem>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
               .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
               .ReverseMap();

            CreateMap<TypeLienPersonneMoraleMorale, SelectedItem>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
               .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
               .ReverseMap();

            CreateMap<CategorieLienLab, SelectedItem>()
               .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
               .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
               .ReverseMap();

            CreateMap<TypeLienPersonnePhysiquePhysique, SelectedItem>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
               .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
               .ReverseMap();
            CreateMap<TypeLienPersonnePhysiqueMorale, SelectedItem>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
               .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
               .ReverseMap();

            CreateMap<FormeJuridique, SelectedItem>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
               .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
               .ReverseMap();

            CreateMap<IdentificationProfessionnelle, SelectedItem>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
                .ReverseMap();

            CreateMap<TypeLienSupport, SelectedItem>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
               .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
               .ReverseMap();
            CreateMap<TypeReferenceLab, SelectedItem>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
                .ReverseMap();
            CreateMap<TypeGarantie, TypeGarantieViewModel>()
                .ReverseMap();
            CreateMap<TypeOperation, TypeOperationViewModel>()
                .ReverseMap();
            CreateMap<CriteresAlerteOrigine, CriteresAlerteOrigineViewModel>()
                .ReverseMap();
            CreateMap<ModaliteFinancement, ModaliteFinancementViewModel>()
                .ReverseMap();
            CreateMap<NatureSoupconFraudeFiscale, SelectedItem>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
                .ReverseMap();
            CreateMap<NatureInfractionPenale, SelectedItem>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
                .ReverseMap();
            CreateMap<RepresentantLegal, RepresentantLegalViewModel>()
                .ForMember(dest => dest.NomNaissance,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.NomNaissance) != "Null"
                        ? Encoding.UTF8.GetString(src.NomNaissance)
                        : string.Empty))
                .ForMember(dest => dest.Prenoms,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.Prenoms) != "Null"
                        ? Encoding.UTF8.GetString(src.Prenoms)
                        : string.Empty))
                .ForMember(dest => dest.DateNaissance, opt => opt.MapFrom(src => src.DateNaissance.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.VilleNaissance,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.VilleNaissance) != "Null"
                        ? Encoding.UTF8.GetString(src.VilleNaissance)
                        : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.DateNaissance, opt => opt.MapFrom(src => src.DateNaissance == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateNaissance.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.NomNaissance,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.NomNaissance)))
                .ForMember(dest => dest.Prenoms,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Prenoms)))
                .ForMember(dest => dest.VilleNaissance,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.VilleNaissance)));
            CreateMap<PieceIdentite, PieceIdentiteViewModel>()
                .ForMember(dest => dest.DateValiditeDebut, opt => opt.MapFrom(src => src.DateValiditeDebut == null ? (DateTime?)null : src.DateValiditeDebut.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateValiditeFin, opt => opt.MapFrom(src => src.DateValiditeFin == null ? (DateTime?)null : src.DateValiditeFin.GetValueOrDefault().DateTime))
                .ReverseMap()
                .ForMember(dest => dest.DateValiditeDebut, opt => opt.MapFrom(src => src.DateValiditeDebut == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateValiditeDebut.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateValiditeFin, opt => opt.MapFrom(src => src.DateValiditeFin == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateValiditeFin.GetValueOrDefault(), DateTimeKind.Utc)));
            CreateMap<Dirigeant, DirigeantViewModel>()
                .ForMember(dest => dest.DateNaissance, opt => opt.MapFrom(src => src.DateNaissance == null ? (DateTime?)null : src.DateNaissance.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.BeDate, opt => opt.MapFrom(src => src.BeDate == null ? (DateTime?)null : src.BeDate.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.BeDateNaissance, opt => opt.MapFrom(src => src.BeDateNaissance == null ? (DateTime?)null : src.BeDateNaissance.GetValueOrDefault().DateTime))
                .ReverseMap()
                .ForMember(dest => dest.DateNaissance, opt => opt.MapFrom(src => src.DateNaissance == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateNaissance.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.BeDate, opt => opt.MapFrom(src => src.BeDate == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.BeDate.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.BeDateNaissance, opt => opt.MapFrom(src => src.BeDateNaissance == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.BeDateNaissance.GetValueOrDefault(), DateTimeKind.Utc)));
            CreateMap<SupportFinancierPersonneMorale, SupportFinancierPersonneMoraleViewModel>()
                .ForMember(dest => dest.DateOuvertureCompte, opt => opt.MapFrom(src => src.DateOuvertureCompte == null ? (DateTime?)null : src.DateOuvertureCompte.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.Iban,
                    opt => opt.MapFrom(src => src.Iban))
                .ForMember(dest => dest.Nom,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.Nom) != "Null"
                        ? Encoding.UTF8.GetString(src.Nom)
                        : string.Empty))
                .ForMember(dest => dest.Prenom,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.Prenom) != "Null"
                        ? Encoding.UTF8.GetString(src.Prenom)
                        : string.Empty))
                .ForMember(dest => dest.VilleNaissance,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.VilleNaissance) != "Null"
                        ? Encoding.UTF8.GetString(src.VilleNaissance)
                        : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.DateOuvertureCompte, opt => opt.MapFrom(src => src.DateOuvertureCompte == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateOuvertureCompte.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.Iban,
                    opt => opt.MapFrom(src => src.Iban))
                .ForMember(dest => dest.Nom,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Nom)))
                .ForMember(dest => dest.Prenom,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Prenom)))
                .ForMember(dest => dest.VilleNaissance,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.VilleNaissance)));

            CreateMap<PersonneMoraleLab, PersonneMoraleLabViewModel>()
                .ForMember(dest => dest.DateCessationRelations, opt => opt.MapFrom(src => src.DateCessationRelations == null ? (DateTime?)null : src.DateCessationRelations.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateEntreeEnRelation, opt => opt.MapFrom(src => src.DateEntreeEnRelation == null ? (DateTime?)null : src.DateEntreeEnRelation.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateImmatriculation, opt => opt.MapFrom(src => src.DateImmatriculation == null ? (DateTime?)null : src.DateImmatriculation.GetValueOrDefault().DateTime))
                  .ForMember(dest => dest.NumeroImmatriculation, opt => opt.MapFrom(src => src.NumeroImmatriculation))
                .ForMember(dest => dest.RaisonSociale, opt => opt.MapFrom(src => src.RaisonSociale))
                .ForMember(dest => dest.Sigle,
                    opt => opt.MapFrom(src =>
                        Encoding.UTF8.GetString(src.Sigle) != "Null"
                            ? Encoding.UTF8.GetString(src.Sigle)
                            : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.DateCessationRelations, opt => opt.MapFrom(src => src.DateCessationRelations == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateCessationRelations.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateEntreeEnRelation, opt => opt.MapFrom(src => src.DateEntreeEnRelation == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateEntreeEnRelation.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateImmatriculation, opt => opt.MapFrom(src => src.DateImmatriculation == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateImmatriculation.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.RaisonSociale, opt => opt.MapFrom(src => src.RaisonSociale))
                .ForMember(dest => dest.Sigle, opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Sigle)));

            CreateMap<NonConnuLab, NonConnuLabViewModel>().ReverseMap();

            CreateMap<PersonnePhysiqueLab, PersonnePhysiqueLabViewModel>()
                .ForMember(dest => dest.DateCessationRelations, opt => opt.MapFrom(src => src.DateCessationRelations == null ? (DateTime?)null : src.DateCessationRelations.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateEntreeEnRelation, opt => opt.MapFrom(src => src.DateEntreeEnRelation == null ? (DateTime?)null : src.DateEntreeEnRelation.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateNaissance, opt => opt.MapFrom(src => src.DateNaissance == null ? (DateTime?)null : src.DateNaissance.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.AutreIdentiteDateNaissance, opt => opt.MapFrom(src => src.AutreIdentiteDateNaissance == null ? (DateTime?)null : src.AutreIdentiteDateNaissance.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.NomNaissance, opt => opt.MapFrom(src => src.NomNaissance))
                .ForMember(dest => dest.NomUsuel, opt => opt.MapFrom(src => src.NomUsuel))
                .ForMember(dest => dest.Prenoms, opt => opt.MapFrom(src => src.Prenoms))
                .ForMember(dest => dest.PpeId, opt => opt.MapFrom(src => src.PpeId))
                .ForMember(dest => dest.LieuNaissance,
                    opt => opt.MapFrom(src =>
                        Encoding.UTF8.GetString(src.LieuNaissance) != "Null"
                            ? Encoding.UTF8.GetString(src.LieuNaissance)
                            : string.Empty))

                .ForMember(dest => dest.ElementClesRelation,
                    opt => opt.MapFrom(src =>
                        Encoding.UTF8.GetString(src.ElementClesRelation) != "Null"
                            ? Encoding.UTF8.GetString(src.ElementClesRelation)
                            : string.Empty))
                .ForMember(dest => dest.SurfaceFinanciere,
                    opt => opt.MapFrom(src =>
                        Encoding.UTF8.GetString(src.SurfaceFinanciere) != "Null"
                            ? Encoding.UTF8.GetString(src.SurfaceFinanciere)
                            : string.Empty))
                .ForMember(dest => dest.IsUtilisationAutreIdentite, opt => opt.MapFrom(src => src.IsUtilisationAutreIdentite))
                .ForMember(dest => dest.IsDonneesConnexion, opt => opt.MapFrom(src => src.IsDonneesConnexion))
                .ForMember(dest => dest.NationalitesId, opt => opt.MapFrom(src => src.NationalitePersonnePhysiqueLabs != null ? src.NationalitePersonnePhysiqueLabs.Select(n => n.PaysId) : null))
                .ForMember(dest => dest.AutreIdentiteNationalitesId, opt => opt.MapFrom(src => src.AutreNationalitePersonnePhysiqueLabs != null ? src.AutreNationalitePersonnePhysiqueLabs.Select(n => n.PaysId) : null))
                .ForMember(dest => dest.PpeTypesId, opt => opt.MapFrom(src => src.PpeTypePersonnePhysiqueLabs != null ? src.PpeTypePersonnePhysiqueLabs.Select(n => n.PpeTypeId) : null))
                .ReverseMap()
                .ForMember(dest => dest.DateCessationRelations, opt => opt.MapFrom(src => src.DateCessationRelations == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateCessationRelations.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateEntreeEnRelation, opt => opt.MapFrom(src => src.DateEntreeEnRelation == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateEntreeEnRelation.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateNaissance, opt => opt.MapFrom(src => src.DateNaissance == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateNaissance.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.AutreIdentiteDateNaissance, opt => opt.MapFrom(src => src.AutreIdentiteDateNaissance == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.AutreIdentiteDateNaissance.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.NomNaissance, opt => opt.MapFrom(src => src.NomNaissance))
                .ForMember(dest => dest.NomUsuel, opt => opt.MapFrom(src => src.NomUsuel))
                .ForMember(dest => dest.Prenoms, opt => opt.MapFrom(src => src.Prenoms))
                .ForMember(dest => dest.IsUtilisationAutreIdentite, opt => opt.MapFrom(src => src.IsUtilisationAutreIdentite == 1))
                .ForMember(dest => dest.IsDonneesConnexion, opt => opt.MapFrom(src => src.IsDonneesConnexion == 1))
                .ForMember(dest => dest.LieuNaissance,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.LieuNaissance)))
                .ForMember(dest => dest.ElementClesRelation,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.ElementClesRelation)))
                .ForMember(dest => dest.SurfaceFinanciere,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.SurfaceFinanciere)))
                .ForMember(dest => dest.NationalitePersonnePhysiqueLabs, opt => opt.MapFrom(src => src.NationalitesId != null ? src.NationalitesId.Select(n => new NationalitePersonnePhysiqueLab { PaysId = n }) : null))
                .ForMember(dest => dest.AutreNationalitePersonnePhysiqueLabs, opt => opt.MapFrom(src => src.AutreIdentiteNationalitesId != null ? src.AutreIdentiteNationalitesId.Select(n => new AutreNationalitePersonnePhysiqueLab { PaysId = n }) : null))
                .ForMember(dest => dest.PpeTypePersonnePhysiqueLabs, opt => opt.MapFrom(src => src.PpeTypesId != null ? src.PpeTypesId.Select(n => new PpeTypePersonnePhysiqueLab { PpeTypeId = n }) : null));
                

            CreateMap<PersonnePhysiqueLabLienEntite, PersonnePhysiqueLabLienEntitesViewModel>().ReverseMap();


            CreateMap<PersonneMoraleLabLienEntite, PersonneMoraleLabLienEntitesViewModel>().ReverseMap();
            CreateMap<DossierLabPersonnePhysique, DossierLabPersonnePhysiqueViewModel>().ReverseMap();
            CreateMap<DossierLabPersonneMorale, DossierLabPersonneMoraleViewModel>().ReverseMap();
            CreateMap<DossierLabNonConnu, DossierLabNonConnueViewModel>().ReverseMap();

            CreateMap<SupportFinancierPersonnePhysique, SupportFinancierPersonnePhysiqueViewModel>()
                    .ForMember(dest => dest.Iban, opt => opt.MapFrom(src => src.Iban))
                    .ForMember(dest => dest.DateOuvertureCompte, opt => opt.MapFrom(src => src.DateOuvertureCompte == null ? (DateTime?)null : src.DateOuvertureCompte.GetValueOrDefault().DateTime))
                .ReverseMap()
                    .ForMember(dest => dest.Iban, opt => opt.MapFrom(src => src.Iban))
                    .ForMember(dest => dest.DateOuvertureCompte, opt => opt.MapFrom(src => src.DateOuvertureCompte == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateOuvertureCompte.GetValueOrDefault(), DateTimeKind.Utc)));

            CreateMap<SupportFinancierNonConnu, SupportFinancierNonConnuViewModel>()
                    .ForMember(dest => dest.Iban, opt => opt.MapFrom(src => src.Iban))
                    .ForMember(dest => dest.DateOuvertureCompte, opt => opt.MapFrom(src => src.DateOuvertureCompte == null ? (DateTime?)null : src.DateOuvertureCompte.GetValueOrDefault().DateTime))
                .ReverseMap()
                    .ForMember(dest => dest.Iban, opt => opt.MapFrom(src => src.Iban))
                    .ForMember(dest => dest.DateOuvertureCompte, opt => opt.MapFrom(src => src.DateOuvertureCompte == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateOuvertureCompte.GetValueOrDefault(), DateTimeKind.Utc)));

            CreateMap<ActiviteProfessionnellePersonnePhysiqueLabViewModel, ActiviteProfessionnellePersonnePhysiqueLab>().ReverseMap();
            CreateMap<CategorieDocument, CategorieDocumentViewModel>().ReverseMap();
            CreateMap<TypeDocumentLab, TypeDocumentLabViewModel>()
                .ForPath(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForPath(d => d.Code, opt => opt.MapFrom(s => s.Code))
                .ForPath(d => d.NameFr, opt => opt.MapFrom(s => s.FrenchName))
                .ForPath(d => d.NameEn, opt => opt.MapFrom(s => s.EnglishName))
            .ReverseMap();

            CreateMap<TypeDocumentLabViewModel, TypeDocumentLab>()
                .ForPath(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForPath(d => d.Code, opt => opt.MapFrom(s => s.Code))
                .ForPath(d => d.FrenchName, opt => opt.MapFrom(s => s.NameFr))
                .ForPath(d => d.EnglishName, opt => opt.MapFrom(s => s.NameEn))
            .ReverseMap();
            CreateMap<DocumentDossierLab, DocumentDossierLabViewModel>()
                .ForPath(d => d.TypeDocumentLabId, opt => opt.MapFrom(s => s.TypeDocumentLabId))
                .ForPath(d => d.TypeDocumentLab, opt => opt.MapFrom(s => s.TypeDocumentLab))
                .ForPath(d => d.CountryRelase, opt => opt.MapFrom(s => s.CountryRelase))
                .ForPath(d => d.CategorieDocument, opt => opt.MapFrom(s => s.CategorieDocument))
                .ForPath(d => d.DateValidity, opt => opt.MapFrom(s => s.DateValidity.GetValueOrDefault().DateTime))
                .ForPath(d => d.DateDocument, opt => opt.MapFrom(s => s.DateDocument.GetValueOrDefault().DateTime))
                .ReverseMap()
                .ForMember(dest => dest.DateValidity, opt => opt.MapFrom(src => src.DateValidity == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateValidity.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateDocument, opt => opt.MapFrom(src => src.DateDocument == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateDocument.GetValueOrDefault(), DateTimeKind.Utc)));

            CreateMap<AttachmentViewModel, DocumentDossierLabViewModel>();
            CreateMap<OperationSuspectDeclarationTracfin, OperationSuspectDeclarationTracfinViewModel>().ReverseMap();
            CreateMap<OperationEnCoursDeclarationTracfin, OperationEnCoursDeclarationTracfinViewModel>()
                    .ForMember(dest => dest.DateLimiteExecution, opt => opt.MapFrom(src => src.DateLimiteExecution == null ? (DateTime?)null : src.DateLimiteExecution.GetValueOrDefault().DateTime))
                    .ReverseMap()
                    .ForMember(dest => dest.DateLimiteExecution, opt => opt.MapFrom(src => src.DateLimiteExecution == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateLimiteExecution.GetValueOrDefault(), DateTimeKind.Utc)));
            CreateMap<DocumentDeclarationTracfin, DocumentDeclarationTracfinViewModel>()
                .ForMember(dest => dest.DateCreation, opt => opt.MapFrom(src => src.DateDocument.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateDocument, opt => opt.MapFrom(src => src.DateDocument.GetValueOrDefault().DateTime))
                .ReverseMap()
                .ForMember(dest => dest.DateDocument, opt => opt.MapFrom(src => src.DateDocument == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateDocument.GetValueOrDefault(), DateTimeKind.Utc)));
            CreateMap<DocumentLab, DocumentLabViewModel>()
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate.DateTime))
                .ForMember(dest => dest.LastAccessDate, opt => opt.MapFrom(src => src.LastAccessDate.DateTime))
                .ForMember(dest => dest.LastWriteDate, opt => opt.MapFrom(src => src.LastWriteDate.DateTime))
              .ForMember(dest => dest.FileContent,
                  opt => opt.MapFrom(src => src.FileContent != null ? Convert.ToBase64String(src.FileContent) : string.Empty))
              .ReverseMap()
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.CreationDate, DateTimeKind.Utc)))
                .ForMember(dest => dest.LastAccessDate, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.LastAccessDate, DateTimeKind.Utc)))
                .ForMember(dest => dest.LastWriteDate, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.LastWriteDate, DateTimeKind.Utc)))
                .ForMember(dest => dest.FileContent,
                  opt => opt.MapFrom(src => src.FileContent != null ? Convert.FromBase64String(src.FileContent) : null));

            CreateMap<DeclarationTracfinFile, DeclarationTracfinFileViewModel>()
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.FileType, opt => opt.MapFrom(src => src.FileType))
                .ForMember(dest => dest.FileContent,
                  opt => opt.MapFrom(src => src.FileContent != null ? Convert.ToBase64String(src.FileContent) : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.FileType, opt => opt.MapFrom(src => src.FileType))
                .ForMember(dest => dest.FileContent,
                  opt => opt.MapFrom(src => src.FileContent != null ? Convert.FromBase64String(src.FileContent) : null));

            CreateMap<TypeClientLab, TypeClientLabViewModel>()
               .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
               .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
               .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
               .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription))
              .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification ?? src.DateCreation))
              .ForMember(dest => dest.Createur_FullName, opt => opt.MapFrom(src => src.Createur != null ? $"{src.Createur.Prenom} {src.Createur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
              .ForMember(dest => dest.Modificateur_FullName, opt => opt.MapFrom(src => src.Modificateur != null ? $"{src.Modificateur.Prenom} {src.Modificateur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
              .ReverseMap();

            CreateMap<TypeImplicationLab, TypeImplicationLabViewModel>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
             .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
             .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
             .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription))
            .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification ?? src.DateCreation))
            .ForMember(dest => dest.Createur_FullName, opt => opt.MapFrom(src => src.Createur != null ? $"{src.Createur.Prenom} {src.Createur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
            .ForMember(dest => dest.Modificateur_FullName, opt => opt.MapFrom(src => src.Modificateur != null ? $"{src.Modificateur.Prenom} {src.Modificateur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
            .ReverseMap();


            CreateMap<ProfessionalIdentificationLab, ProfessionalIdentificationLabViewModel>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
             .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
             .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
             .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription))
            .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification ?? src.DateCreation))
            .ForMember(dest => dest.Createur_FullName, opt => opt.MapFrom(src => src.Createur != null ? $"{src.Createur.Prenom} {src.Createur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
            .ForMember(dest => dest.Modificateur_FullName, opt => opt.MapFrom(src => src.Modificateur != null ? $"{src.Modificateur.Prenom} {src.Modificateur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
            .ReverseMap();

            CreateMap<TypeRelationAffaireLab, TypeRelationAffaireLabViewModel>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
                .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
                .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification ?? src.DateCreation))
                .ForMember(dest => dest.Createur_FullName, opt => opt.MapFrom(src => src.Createur != null ? $"{src.Createur.Prenom} {src.Createur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
                .ForMember(dest => dest.Modificateur_FullName, opt => opt.MapFrom(src => src.Modificateur != null ? $"{src.Modificateur.Prenom} {src.Modificateur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
                .ReverseMap();


            CreateMap<SecteurLab, SecteurLabViewModel>()
              .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification ?? src.DateCreation))
              .ForMember(dest => dest.Createur_FullName, opt => opt.MapFrom(src => src.Createur != null ? $"{src.Createur.Prenom} {src.Createur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
              .ForMember(dest => dest.Modificateur_FullName, opt => opt.MapFrom(src => src.Modificateur != null ? $"{src.Modificateur.Prenom} {src.Modificateur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
              .ReverseMap();

            CreateMap<EntiteLab, EntiteLabViewModel>()
              .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification ?? src.DateCreation))
              .ForMember(dest => dest.Createur_FullName, opt => opt.MapFrom(src => src.Createur != null ? $"{src.Createur.Prenom} {src.Createur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
              .ForMember(dest => dest.Modificateur_FullName, opt => opt.MapFrom(src => src.Modificateur != null ? $"{src.Modificateur.Prenom} {src.Modificateur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
              .ReverseMap();

            CreateMap<EntiteLab, SelectedItem>()
                .ForMember(r => r.NameFr, p => p.MapFrom(pt => pt.Lisp))
                .ForMember(r => r.NameEn, p => p.MapFrom(pt => pt.Lisp))
              .ReverseMap();

            CreateMap<TypeClientLab, SelectedItemTypeClient>()
                .ForMember(r => r.NameFr, p => p.MapFrom(pt => pt.FrenchName))
                .ForMember(r => r.NameEn, p => p.MapFrom(pt => pt.EnglishName))
                .ForMember(r => r.IsCorporate, p => p.MapFrom(pt => pt.IsCorporate))
                .ForMember(r => r.Id, p => p.MapFrom(pt => pt.Id));
            CreateMap<TypeFonctionDirigeant, SelectedItem>()
                .ForMember(r => r.NameFr, p => p.MapFrom(pt => pt.FrenchName))
                .ForMember(r => r.NameEn, p => p.MapFrom(pt => pt.EnglishName))
                .ForMember(r => r.Id, p => p.MapFrom(pt => pt.Id));
            CreateMap<TypeClientLabViewModel, SelectedItem>();
            CreateMap<TypeClientLabViewModel, SelectedItemTypeClient>();

            CreateMap<TypeRelationAffaireLabViewModel, SelectedItem>();

            CreateMap<AavViewModel, AavReference>().ReverseMap();
            CreateMap<DeclarationTracfinNatureInfractionPenale, DeclarationTracfinNatureInfractionPenaleViewModel>().ReverseMap();
            CreateMap<DeclarationTracfinNatureSoupconFraudeFiscale, DeclarationTracfinNatureSoupconFraudeFiscaleViewModel>().ReverseMap();

            CreateMap<DeclarationTracfinContributeurDsftViewModel, DeclarationTracfinContributeurDsft>()
                .ReverseMap();

            CreateMap<DeclarationTracfin, DeclarationTracfinViewModel>()
                .ForMember(dest => dest.DateDeclaration, opt => opt.MapFrom(src => src.DateDeclaration == null ? (DateTime?)null : src.DateDeclaration.GetValueOrDefault().DateTime))
                //.ForMember(dest => dest.DateDeclarationNewDS, opt => opt.MapFrom(src => src.DateDeclaration == null ? (DateTime?)null : src.DateDeclaration.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateRuptureRelation, opt => opt.MapFrom(src => src.DateRuptureRelation == null ? (DateTime?)null : src.DateRuptureRelation.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateTimeExecution, opt => opt.MapFrom(src => src.DateTimeExecution == null ? (DateTime?)null : src.DateTimeExecution.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DebutPeriodeFaitsConsideres, opt => opt.MapFrom(src => src.DebutPeriodeFaitsConsideres == null ? (DateTime?)null : src.DebutPeriodeFaitsConsideres.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.FinPeriodeFaitsConsideres, opt => opt.MapFrom(src => src.FinPeriodeFaitsConsideres == null ? (DateTime?)null : src.FinPeriodeFaitsConsideres.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.IsClosingStatus, opt => opt.MapFrom(src => src.DossierLab.StatutDossierId == (int)StatutDossierLabEnum.Cloture))
               .ForMember(dest => dest.Analyses,
                    opt => opt.MapFrom(src =>
                        Encoding.UTF8.GetString(src.Analyses) != "Null"
                            ? Encoding.UTF8.GetString(src.Analyses)
                            : string.Empty))
               .ForMember(dest => dest.Motifs,
                    opt => opt.MapFrom(src =>
                        Encoding.UTF8.GetString(src.Motifs) != "Null"
                            ? Encoding.UTF8.GetString(src.Motifs)
                            : string.Empty))
                .ForMember(dest => dest.NameInfractionFr,
                        opt => opt.MapFrom(src => src.DeclarationTracfinNatureInfractionPenales.Any() ? string.Join(",", src.DeclarationTracfinNatureInfractionPenales.Select(x => x.NatureInfractionPenale.FrenchName)) : string.Empty))
                .ForMember(dest => dest.NameInfractionEn,
                        opt => opt.MapFrom(src => src.DeclarationTracfinNatureInfractionPenales.Any() ? string.Join(",", src.DeclarationTracfinNatureInfractionPenales.Select(x => x.NatureInfractionPenale.EnglishName)) : string.Empty))
                .ForMember(dest => dest.DateEnvoiDeclaration, opt => opt.MapFrom(src => src.DateEnvoiDeclaration == null ? (DateTime?)null : src.DateEnvoiDeclaration.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateRequestARTracfin, opt => opt.MapFrom(src => src.DateRequestARTracfin == null ? (DateTime?)null : src.DateRequestARTracfin.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateEnregistrementTracfin, opt => opt.MapFrom(src => src.DateEnregistrementTracfin == null ? (DateTime?)null : src.DateEnregistrementTracfin.GetValueOrDefault().DateTime))
                .ReverseMap()
                .ForMember(dest => dest.DateDeclaration, opt => opt.MapFrom(src => src.DateDeclaration == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateDeclaration.GetValueOrDefault(), DateTimeKind.Utc)))
                //.ForMember(dest => dest.DateDeclaration, opt => opt.MapFrom(src => src.DateDeclarationNewDS == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateDeclarationNewDS.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateRuptureRelation, opt => opt.MapFrom(src => src.DateRuptureRelation == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateRuptureRelation.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateTimeExecution, opt => opt.MapFrom(src => src.DateTimeExecution == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateTimeExecution.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DebutPeriodeFaitsConsideres, opt => opt.MapFrom(src => src.DebutPeriodeFaitsConsideres == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DebutPeriodeFaitsConsideres.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.FinPeriodeFaitsConsideres, opt => opt.MapFrom(src => src.FinPeriodeFaitsConsideres == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.FinPeriodeFaitsConsideres.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.Analyses,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Analyses)))
                .ForMember(dest => dest.Motifs,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Motifs)))
                .ForMember(dest => dest.DateEnvoiDeclaration, opt => opt.MapFrom(src => src.DateEnvoiDeclaration == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateEnvoiDeclaration.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateRequestARTracfin, opt => opt.MapFrom(src => src.DateRequestARTracfin == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateRequestARTracfin.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateEnregistrementTracfin, opt => opt.MapFrom(src => src.DateEnregistrementTracfin == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateEnregistrementTracfin.GetValueOrDefault(), DateTimeKind.Utc)));


            CreateMap<FonctionLabViewModel, SelectedItem>();
            CreateMap<FonctionViewModel, FonctionLab>();

            CreateMap<DossierLabOperation, DossierLabOperationViewModel>().ReverseMap();
            CreateMap<DossierLabScenario, DossierLabScenarioViewModel>().ReverseMap();

            CreateMap<EventSearchLab, EventSearchLabViewModel>().ReverseMap();

            CreateMap<OrigineLabViewModel, SelectedItem>();
            CreateMap<OrigineLabViewModel, OrigineLab>();
            CreateMap<OrigineLab, OrigineLabViewModel>()
              .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
              .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.EnglishName) ? src.FrenchName : src.EnglishName))
              .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
              .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription))
              .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification ?? src.DateCreation))
              .ForMember(dest => dest.Createur_FullName, opt => opt.MapFrom(src => src.Createur != null ? $"{src.Createur.Prenom} {src.Createur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
              .ForMember(dest => dest.Modificateur_FullName, opt => opt.MapFrom(src => src.Modificateur != null ? $"{src.Modificateur.Prenom} {src.Modificateur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
              .ReverseMap();

            CreateMap<OrigineGroupeLabViewModel, SelectedItem>();
            CreateMap<OrigineGroupeLabViewModel, OrigineGroupeLab>();
            CreateMap<OrigineGroupeLab, OrigineGroupeLabViewModel>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
                .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
                .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification ?? src.DateCreation))
                .ForMember(dest => dest.Createur_FullName, opt => opt.MapFrom(src => src.Createur != null ? $"{src.Createur.Prenom} {src.Createur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
                .ForMember(dest => dest.Modificateur_FullName, opt => opt.MapFrom(src => src.Modificateur != null ? $"{src.Modificateur.Prenom} {src.Modificateur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
                .ReverseMap();

            CreateMap<TypeLegislationLabViewModel, TypeLegislationLab>();
            CreateMap<TypeLegislationLab, TypeLegislationLabViewModel>()
             .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
             .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
             .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
             .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription))
             .ReverseMap();

            CreateMap<TypeLegislationLab, SelectedItem>()
             .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
             .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
             .ReverseMap();

            CreateMap<CategorieLabViewModel, SelectedItem>();
            CreateMap<CategorieLabViewModel, CategorieLab>();
            CreateMap<CategorieLab, CategorieLabViewModel>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
                .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
                .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification ?? src.DateCreation))
                .ForMember(dest => dest.Createur_FullName, opt => opt.MapFrom(src => src.Createur != null ? $"{src.Createur.Prenom} {src.Createur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
                .ForMember(dest => dest.Modificateur_FullName, opt => opt.MapFrom(src => src.Modificateur != null ? $"{src.Modificateur.Prenom} {src.Modificateur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
                .ReverseMap();

            CreateMap<EmailTemplateLabViewModel, EmailTemplateLab>();
            CreateMap<EmailTemplateLab, EmailTemplateLabViewModel>();

            CreateMap<CategorieGroupeLabViewModel, SelectedItem>();
            CreateMap<CategorieGroupeLabViewModel, CategorieGroupeLab>();
            CreateMap<CategorieGroupeLab, CategorieGroupeLabViewModel>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
                .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
                .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification ?? src.DateCreation))
                .ForMember(dest => dest.Createur_FullName, opt => opt.MapFrom(src => src.Createur != null ? $"{src.Createur.Prenom} {src.Createur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
                .ForMember(dest => dest.Modificateur_FullName, opt => opt.MapFrom(src => src.Modificateur != null ? $"{src.Modificateur.Prenom} {src.Modificateur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
                .ReverseMap();

            CreateMap<ModeEnvoieTracfin, ModeEnvoieTracfinViewModel>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
                .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
                .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification ?? src.DateCreation))
                .ForMember(dest => dest.Createur_FullName, opt => opt.MapFrom(src => src.Createur != null ? $"{src.Createur.Prenom} {src.Createur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
                .ForMember(dest => dest.Modificateur_FullName, opt => opt.MapFrom(src => src.Modificateur != null ? $"{src.Modificateur.Prenom} {src.Modificateur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
                .ReverseMap();

            CreateMap<GroupeCategorieLab, GroupeCategorieLabViewModel>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
                .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
                .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification ?? src.DateCreation))
                .ForMember(dest => dest.Createur_FullName, opt => opt.MapFrom(src => src.Createur != null ? $"{src.Createur.Prenom} {src.Createur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
                .ForMember(dest => dest.Modificateur_FullName, opt => opt.MapFrom(src => src.Modificateur != null ? $"{src.Modificateur.Prenom} {src.Modificateur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
                .ReverseMap();

            CreateMap<Delegation, DelegationViewModel>()
                .ForMember(dest => dest.DateCreation, opt => opt.MapFrom(src => src.DateCreation == null ? (DateTime?)null : src.DateCreation.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification == null ? (DateTime?)null : src.DateModification.GetValueOrDefault().DateTime))
                .ReverseMap()
                .ForMember(dest => dest.DateCreation, opt => opt.MapFrom(src => src.DateCreation == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateCreation.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateModification.GetValueOrDefault(), DateTimeKind.Utc)));
            CreateMap<GroupeCategorieLabViewModel, SelectedItem>();

            CreateMap<GroupeOrigineLab, GroupeOrigineLabViewModel>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
                .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
                .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification ?? src.DateCreation))
                .ForMember(dest => dest.Createur_FullName, opt => opt.MapFrom(src => src.Createur != null ? $"{src.Createur.Prenom} {src.Createur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
                .ForMember(dest => dest.Modificateur_FullName, opt => opt.MapFrom(src => src.Modificateur != null ? $"{src.Modificateur.Prenom} {src.Modificateur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
                .ReverseMap();

            CreateMap<GroupeOrigineLabViewModel, SelectedItem>();

            CreateMap<FonctionLab, FonctionLabViewModel>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
                .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
                .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription))
                .ReverseMap();

            CreateMap<FonctionLabViewModel, SelectedItem>();

            CreateMap<ScenarioLab, SelectedItem>()
              .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
              .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
              .ReverseMap();

            CreateMap<ApplicationScenarioLab, SelectedItem>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
                .ReverseMap();

            CreateMap<ApplicationScenarioLab, ApplicationScenarioLabViewModel>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
                .ReverseMap();

            CreateMap<SecteurEconomiqueLab, SelectedItem>()
              .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
              .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
              .ReverseMap();

            CreateMap<ScenarioLabViewModel, SelectedItem>();
            CreateMap<ScenarioLab, ScenarioLabViewModel>()
              .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
              .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
              .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
              .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription))
              .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification ?? src.DateCreation))
              .ForMember(dest => dest.Createur_FullName, opt => opt.MapFrom(src => src.Createur != null ? $"{src.Createur.Prenom} {src.Createur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
              .ForMember(dest => dest.Modificateur_FullName, opt => opt.MapFrom(src => src.Modificateur != null ? $"{src.Modificateur.Prenom} {src.Modificateur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
              .ReverseMap();

            CreateMap<SecteurEconomiqueLabViewModel, SelectedItem>();
            CreateMap<SecteurEconomiqueLab, SecteurEconomiqueLabViewModel>()
                .ForMember(dest => dest.DateCreation, opt => opt.MapFrom(src => src.DateCreation.DateTime))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification == null ? (DateTime?)null : src.DateModification.GetValueOrDefault().DateTime))
              .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
              .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
              .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
              .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription))
              .ReverseMap()
                .ForMember(dest => dest.DateCreation, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.DateCreation, DateTimeKind.Utc)))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateModification.GetValueOrDefault(), DateTimeKind.Utc)));

            CreateMap<OrganismeLab, OrganismeLabViewModel>()
               //.ForMember(dest => dest.Pays, opt => opt.MapFrom(src => src.Pays.IsoFrenchName))
               //.ForMember(dest => dest.TypeVoie, opt => opt.MapFrom(src => src.TypeVoie.FrenchName))
               //.ForMember(dest => dest.ComplementVoie, opt => opt.MapFrom(src => src.ComplementVoie.FrenchName))
               //.ForMember(dest => dest.Direction, opt => opt.MapFrom(src => src.Direction.Nom))
               .ReverseMap();

            CreateMap<OrganismeLab, SelectedItem>()
               .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.Libelle))
               .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.Libelle))
               .ReverseMap();



            CreateMap<DemandeInformationLab, DemandeInformationLabViewModel>()
                .ForMember(dest => dest.Demande,
                     opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.Demande) != "Null"
                         ? Encoding.UTF8.GetString(src.Demande)
                         : string.Empty))
                .ForMember(dest => dest.Retour,
                     opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.Retour) != "Null"
                         ? Encoding.UTF8.GetString(src.Retour)
                         : string.Empty))
                .ForMember(dest => dest.Titre,
                     opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.Titre) != "Null"
                         ? Encoding.UTF8.GetString(src.Titre)
                         : string.Empty))
                .ForMember(dest => dest.DateCreation, opt => opt.MapFrom(src => src.DateCreation))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification == null ? (DateTime?)null : src.DateModification.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateRetour, opt => opt.MapFrom(src => src.DateRetour == null ? (DateTime?)null : src.DateRetour.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateCloture, opt => opt.MapFrom(src => src.DateCloture == null ? (DateTime?)null : src.DateCloture.GetValueOrDefault().DateTime))
                .ForMember(r => r.LastDateRelance, p => p.MapFrom(pt => pt.LastDateRelance == null ? (DateTime?)null : pt.LastDateRelance.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.CodeUniqueDossier, opt => opt.MapFrom(src => src.DossierLab.CodeUnique ?? string.Empty))
                .ForMember(dest => dest.CategorieFr, opt => opt.MapFrom(src => src.DossierLab.Categorie.FrenchName ?? string.Empty))
                .ForMember(dest => dest.CategorieEn, opt => opt.MapFrom(src => src.DossierLab.Categorie.EnglishName ?? string.Empty))
                .ForMember(dest => dest.PaysFr, opt => opt.MapFrom(src => src.DossierLab.Pays.IsoFrenchName ?? string.Empty))
                .ForMember(dest => dest.PaysEn, opt => opt.MapFrom(src => src.DossierLab.Pays.IsoEnglishName ?? string.Empty))
                .ForMember(dest => dest.Beneficiaire, opt => opt.MapFrom(src => src.DossierLab.DossierLabOperations.FirstOrDefault().Beneficiaire ?? string.Empty))
                .ForMember(dest => dest.DonneurOrdre, opt => opt.MapFrom(src => src.DossierLab.DossierLabOperations.FirstOrDefault().DonneurOrdre ?? string.Empty))
                .ForMember(dest => dest.Montant, opt => opt.MapFrom(src => src.DossierLab.DossierLabOperations.FirstOrDefault().Montant))
                .ForMember(dest => dest.DeviseId, opt => opt.MapFrom(src => src.DossierLab.DossierLabOperations.FirstOrDefault().DeviseId))
                .ForMember(dest => dest.IsAutorisationDgt, opt => opt.MapFrom(src => src.DossierLab.IsAutorisationDGT))
                .ForMember(dest => dest.IsHomonymieDgt, opt => opt.MapFrom(src => src.DossierLab.IsDgt))
                .ForMember(dest => dest.Conversation,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.Conversation) != "Null"
                        ? Encoding.UTF8.GetString(src.Conversation)
                        : string.Empty)).ReverseMap()
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateModification.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateRetour, opt => opt.MapFrom(src => src.DateRetour == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateRetour.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.Demande, opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Demande)))
                .ForMember(dest => dest.Retour, opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Retour)))
                .ForMember(dest => dest.Titre, opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Titre)))
                .ForMember(dest => dest.Conversation, opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Conversation)));

            CreateMap<DocumentDemandeInformationLab, DocumentDemandeInformationLabViewModel>()
                .ForMember(dest => dest.DateCreation, opt => opt.MapFrom(src => src.DateCreation == null ? (DateTime?)null : src.DateCreation.GetValueOrDefault().DateTime))
                .ReverseMap()
                .ForMember(dest => dest.DateCreation, opt => opt.MapFrom(src => src.DateCreation == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateCreation.GetValueOrDefault(), DateTimeKind.Utc)));
            CreateMap<TypeDemandeInformationLab, TypeDemandeInformationLabViewModel>().ReverseMap();
            CreateMap<TypeDemandeInformationLab, SelectedItem>()
                .ForMember(r => r.NameFr, p => p.MapFrom(pt => pt.FrenchName))
                .ForMember(r => r.NameEn, p => p.MapFrom(pt => pt.EnglishName))
                .ForMember(r => r.Code, p => p.MapFrom(pt => pt.Code))
                .ForMember(r => r.Id, p => p.MapFrom(pt => pt.Id));

            CreateMap<TypeDemandeInformationLabViewModel, SelectedItem>()
                .ForMember(r => r.NameFr, p => p.MapFrom(pt => pt.NameFr))
                .ForMember(r => r.NameEn, p => p.MapFrom(pt => pt.NameEn))
                .ForMember(r => r.Code, p => p.MapFrom(pt => pt.Code))
                .ForMember(r => r.Id, p => p.MapFrom(pt => pt.Id));


            CreateMap<StatutDemandeInformationLab, StatutDemandeInformationLabViewModel>()
                .ForMember(dest => dest.DateCreation, opt => opt.MapFrom(src => src.DateCreation.DateTime))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification == null ? (DateTime?)null : src.DateModification.GetValueOrDefault().DateTime))
                .ReverseMap()
                .ForMember(dest => dest.DateCreation, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.DateCreation, DateTimeKind.Utc)))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateModification.GetValueOrDefault(), DateTimeKind.Utc)));
            CreateMap<StatutDemandeInformationLab, SelectedItem>()
                .ForMember(r => r.NameFr, p => p.MapFrom(pt => pt.FrenchName))
                .ForMember(r => r.NameEn, p => p.MapFrom(pt => pt.EnglishName))
                .ForMember(r => r.Code, p => p.MapFrom(pt => pt.Code))
                .ForMember(r => r.Id, p => p.MapFrom(pt => pt.Id));
            CreateMap<StatutDemandeInformationLabViewModel, SelectedItem>()
                .ForMember(r => r.NameFr, p => p.MapFrom(pt => pt.NameFr))
                .ForMember(r => r.NameEn, p => p.MapFrom(pt => pt.NameEn))
                .ForMember(r => r.Code, p => p.MapFrom(pt => pt.Code))
                .ForMember(r => r.Id, p => p.MapFrom(pt => pt.Id));

            CreateMap<TypeDeclarationTracfin, TypeDeclarationTracFinViewModel>()
              .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
              .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
              .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
              .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription)).ReverseMap();
            CreateMap<TypeDeclarationTracfin, SelectedItem>()
              .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
              .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
              .ReverseMap();


            CreateMap<PrincipalInstrumentFinancier, PrincipalInstrumentFinancierViewModel>()
              .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
              .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
              .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
              .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription)).ReverseMap();
            CreateMap<PrincipalInstrumentFinancier, SelectedItem>()
              .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
              .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
              .ReverseMap();


            CreateMap<CategorieTracfin, CategorieTracfinViewModel>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
              .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
              .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
              .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription)).ReverseMap();
            CreateMap<CategorieTracfin, SelectedItem>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
              .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
              .ReverseMap();

            CreateMap<StatutOperation, StatutOperationViewModel>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
              .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
              .ForMember(dest => dest.DescriptionFr, opt => opt.MapFrom(src => src.FrenchDescription))
              .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.EnglishDescription)).ReverseMap();
            CreateMap<StatutOperation, SelectedItem>()
                .ForMember(dest => dest.NameFr, opt => opt.MapFrom(src => src.FrenchName))
              .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.EnglishName))
              .ReverseMap();

            CreateMap<TypeDocument, SelectedItem>()
                .ForMember(r => r.NameFr, p => p.MapFrom(pt => pt.FrenchName))
                .ForMember(r => r.NameEn, p => p.MapFrom(pt => pt.EnglishName))
                .ForMember(r => r.Code, p => p.MapFrom(pt => pt.Code))
                .ForMember(r => r.Id, p => p.MapFrom(pt => pt.Id));

            CreateMap<NatureDs, NatureDsViewModel>()
                .ReverseMap();

			CreateMap<CategorieContributeur, CategorieContributeurViewModel>()
				.ReverseMap();

            CreateMap<Contributeur, ContributeurViewModel>()
                .ReverseMap();

            CreateMap<DossierLabAction, DossierLabActionViewModel>()
                .ForMember(dest => dest.DateCreation, opt => opt.MapFrom(src => src.DateCreation == null ? (DateTime?)null : src.DateCreation.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification == null ? (DateTime?)null : src.DateModification.GetValueOrDefault().DateTime))
                .ForMember(dest => dest.Libelle,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.Libelle) != "Null"
                        ? Encoding.UTF8.GetString(src.Libelle)
                        : string.Empty))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.Description) != "Null"
                        ? Encoding.UTF8.GetString(src.Description)
                        : string.Empty))
                .ForMember(dest => dest.Createur_FullName, opt => opt.MapFrom(src => src.Createur != null ? $"{src.Createur.Prenom} {src.Createur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
                .ForMember(dest => dest.Modificateur_FullName, opt => opt.MapFrom(src => src.Modificateur != null ? $"{src.Modificateur.Prenom} {src.Modificateur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.DateCreation, opt => opt.MapFrom(src => src.DateCreation == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateCreation.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.DateModification == null ? (DateTimeOffset?)null : DateTime.SpecifyKind(src.DateModification.GetValueOrDefault(), DateTimeKind.Utc)))
                .ForMember(dest => dest.Libelle, opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Libelle)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Description)));

            CreateMap<OrigineLab, OrigineLabViewModel>()
            .ForMember(dest => dest.Createur_FullName, opt => opt.MapFrom(src => src.Createur != null ? $"{src.Createur.Prenom} {src.Createur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
            .ForMember(dest => dest.Modificateur_FullName, opt => opt.MapFrom(src => src.Modificateur != null ? $"{src.Modificateur.Prenom} {src.Modificateur.Nom.ToUpper(CultureInfo.CurrentCulture)}" : string.Empty))
            .ReverseMap();

            CreateMap<ContratOperationsCompaniesAssurances, ContratOperationsCompaniesAssurancesViewModel>()
                .ReverseMap();
            CreateMap<OperationSusceptibleOpposition, OperationSusceptibleOppositionViewModel>()
                .ReverseMap();
            CreateMap<OperationsCompaniesAssurances, OperationsCompaniesAssurancesViewModel>()
                .ForMember(dest => dest.PaysDepartId, opt => opt.MapFrom(src => src.PaysDepart != null ? src.PaysDepart.Select(p => p.PaysId) : null))
                .ForMember(dest => dest.PaysArriveeId, opt => opt.MapFrom(src => src.PaysArrivee != null ? src.PaysArrivee.Select(p => p.PaysId) : null))
                .ForMember(dest => dest.TypesGaranties, opt => opt.MapFrom(src => src.TypesGaranties != null ? src.TypesGaranties.Select(t => t.TypeGarantieId) : null))
                .ForMember(dest => dest.ProfessionId, opt => opt.MapFrom(src => src.DeclarationTracfin.OrganismeLab != null ? src.DeclarationTracfin.OrganismeLab.ProfessionId : null))
                .ForMember(dest => dest.IsAssuranceVie, opt => opt.MapFrom(src => src.DeclarationTracfin.IsQuestionDeclarationActivite))
                .ReverseMap()
                .ForMember(dest => dest.PaysDepart, opt => opt.MapFrom(src => src.PaysDepartId != null ? src.PaysDepartId.Select(id => new PaysDepartOperationsCompaniesAssurances { PaysId = id }) : null))
                .ForMember(dest => dest.PaysArrivee, opt => opt.MapFrom(src => src.PaysArriveeId != null ? src.PaysArriveeId.Select(id => new PaysArriveeOperationsCompaniesAssurances { PaysId = id }) : null))
                .ForMember(dest => dest.TypesGaranties, opt => opt.MapFrom(src => src.TypesGaranties != null ? src.TypesGaranties.Select(id => new TypeGarantieOperationsCompaniesAssurances { TypeGarantieId = id }) : null));

            CreateMap<OperationsCompaniesImmobiliers, OperationsCompaniesImmobiliersViewModel>()
                .ForMember(dest => dest.PaysDepartId, opt => opt.MapFrom(src => src.PaysDepart != null ? src.PaysDepart.Select(p => p.PaysId) : null))
                .ForMember(dest => dest.PaysArriveeId, opt => opt.MapFrom(src => src.PaysArrivee != null ? src.PaysArrivee.Select(p => p.PaysId) : null))
                .ForMember(dest => dest.TypesOperations, opt => opt.MapFrom(src => src.TypesOperations != null ? src.TypesOperations.Select(t => t.TypeOperationId) : null))
                .ForMember(dest => dest.CriteresAlerteOrigines, opt => opt.MapFrom(src => src.CriteresAlerteOrigines != null ? src.CriteresAlerteOrigines.Select(t => t.CriteresAlerteOrigineId) : null))
                .ForMember(dest => dest.ModaliteFinancements, opt => opt.MapFrom(src => src.ModaliteFinancements != null ? src.ModaliteFinancements.Select(t => t.ModaliteFinancementId) : null))
                .ReverseMap()
                .ForMember(dest => dest.PaysDepart, opt => opt.MapFrom(src => src.PaysDepartId != null ? src.PaysDepartId.Select(id => new PaysDepartOperationsCompaniesImmobiliers { PaysId = id }) : null))
                .ForMember(dest => dest.PaysArrivee, opt => opt.MapFrom(src => src.PaysArriveeId != null ? src.PaysArriveeId.Select(id => new PaysArriveeOperationsCompaniesImmobiliers { PaysId = id }) : null))
                .ForMember(dest => dest.TypesOperations, opt => opt.MapFrom(src => src.TypesOperations != null ? src.TypesOperations.Select(id => new TypeOperationOperationsCompaniesImmobiliers { TypeOperationId = id }) : null))
                .ForMember(dest => dest.CriteresAlerteOrigines, opt => opt.MapFrom(src => src.CriteresAlerteOrigines != null ? src.CriteresAlerteOrigines.Select(id => new CriteresAlerteOrigineOperationsCompaniesImmobiliers { CriteresAlerteOrigineId = id }) : null))
                .ForMember(dest => dest.ModaliteFinancements, opt => opt.MapFrom(src => src.ModaliteFinancements != null ? src.ModaliteFinancements.Select(id => new ModaliteFinancementOperationsCompaniesImmobiliers { ModaliteFinancementId = id }) : null));

        }
    }
}
