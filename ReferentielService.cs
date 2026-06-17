using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BacaratWeb.Core.Extensions;
using BacaratWeb.Core.Services;
using BacaratWeb.Entities.Amf;
using BacaratWeb.Entities.AvisIm;
using BacaratWeb.Entities.AvisSi;
using BacaratWeb.Entities.Commun;
using BacaratWeb.Entities.CommunAvis;
using BacaratWeb.Entities.Escalade;
using BacaratWeb.Entities.Fraude;
using BacaratWeb.Entities.Gda;
using BacaratWeb.Entities.Lab;
using BacaratWeb.Model;
using BacaratWeb.Model.Entities;
using BacaratWeb.Models.Referentials;
using BacaratWeb.Services.Commun.Interfaces;
using BacaratWeb.Services.Commun.Services;
using BacaratWeb.Services.Commun.Services.Interfaces;
using BacaratWeb.Shared;
using Microsoft.EntityFrameworkCore;
using EmailNotificationScope = BacaratWeb.Entities.Commun.EmailNotificationScope;
using Roles = BacaratWeb.Shared.UserRoles;

namespace BacaratWeb.Services.Commun
{
    public class ReferentielService : IReferentielService
    {
        private readonly BacaratWebContext Context;

        public ReferentielService(BacaratWebContext context)
        {
            Context = context;
            Direction = new DirectionService(context);
            Activite = new ActiviteService(context);
        }

        public IQueryable<AppartenanceDocument> GetAppartenanceDocuments()
        {
            return Context.AppartenanceDocuments;
        }

        public IQueryable<GdaAppartenanceDocument> GetGdaAppartenanceDocuments()
        {
            return Context.GdaAppartenanceDocuments;
        }

        public IQueryable<Couleur> GetCouleurs()
        {
            return Context.Couleurs.Where(x => x.IsActive);
        }

        public IQueryable<CategorieAvoir> GetCategoriesAvoir()
        {
            return Context.CategoriesAvoir.Where(x => x.IsActive);
        }
        public IQueryable<ZoneGeographique> GetZoneGeographiques()
        {
            return Context.ZoneGeographiques.Where(x => x.IsActive);
        }
        public IQueryable<AppartenanceDocumentAvis> GetAppartenanceDocumentSAviss()
        {
            return Context.AppartenanceDocumentAviss;
        }

        public IQueryable<AppartenanceDocumentAvis> GetAppartenanceDocumentAvisIms()
        {
            return Context.AppartenanceDocumentAviss;
        }

        public IQueryable<AppartenanceDocumentAvisSi> GetAppartenanceDocumentAvisSis()
        {
            return Context.AppartenanceDocumentAvisSis;
        }

        public async Task<IEnumerable<ActionTypeDemandeInformation>>
            GetActionTypeDemandeAsync(CancellationToken token = default)
        {
            return await Context.ActionTypeDemandeInformations
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<ActionTypeDemandeInformationAvis>>
            GetActionTypeDemandeSAvisAsync(CancellationToken token = default)
        {
            return await Context.ActionTypeDemandeInformationAviss
                .ToListAsync(token)
                .ConfigureAwait(false);
        }


        public async Task<IEnumerable<ActionTypeDemandeInformationAvis>>
            GetActionTypeDemandeAvisImAsync(CancellationToken token = default)
        {
            return await Context.ActionTypeDemandeInformationAviss
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<ActionTypeDemandeInformationAvisSi>>
            GetActionTypeDemandeAvisSiAsync(CancellationToken token = default)
        {
            return await Context.ActionTypeDemandeInformationAvisSis
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public IQueryable<ActionTypeDemandeInformation> GetActionTypeDemandeInformations()
        {
            return Context
                .ActionTypeDemandeInformations
                .Where(x => x.IsActive);
        }

        public IQueryable<ActionTypeDemandeInformationAvis> GetActionTypeDemandeInformationSAviss()
        {
            return Context
                .ActionTypeDemandeInformationAviss
                .Where(x => x.IsActive);
        }

        public IQueryable<ActionTypeDemandeInformationAvis> GetActionTypeDemandeInformationAvisIms()
        {
            return Context
                .ActionTypeDemandeInformationAviss
                .Where(x => x.IsActive);
        }

        public IQueryable<ActionTypeDemandeInformationAvisSi> GetActionTypeDemandeInformationAvisSis()
        {
            return Context
                .ActionTypeDemandeInformationAvisSis
                .Where(x => x.IsActive);
        }

        public IQueryable<Activite> GetActiviteByUser(IUserInfoService userInfoService)
        {
            return Context.UtilisateurDirections
                .Include(d => d.Direction)
                .Include(d => d.Utilisateur)
                .Include(d => d.Activite)
                .Where(d => d.Utilisateur.AspNetUsersId == userInfoService.UserId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated)
                .Select(u => u.Activite);
        }

        public IEnumerable<Activite> GetActiviteHabilitesUser(IUserInfoService userInfoService)
        {
            var activiteList = Context
                .UtilisateurDirections
                .Include(x => x.Direction)
                .Include(x => x.Activite)
                .Include(x => x.Utilisateur)
                .Where(d => d.Utilisateur.AspNetUsersId == userInfoService.UserId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated)
                .Select(u => u.Activite).ToList();

            var activite = activiteList.DistinctBy(x => x.Code);
            return activite;
        }

        public IQueryable<Activite> GetActivites()
        {
            return Context.Activites.Where(x => x.IsActive).Select(e => new Activite
            {
                Id = e.Id,
                Code = e.Code,
                CreateurId = e.CreateurId,
                DateCreation = e.DateCreation,
                EnglishName = e.EnglishName,
                FrenchDescription = e.FrenchDescription,
                FrenchName = e.FrenchName,
                EnglishDescription = e.EnglishDescription,
                RattachementEntite = e.RattachementEntite,
                Validated = e.Validated,
                IsActive = e.IsActive,
                ModificateurId = e.ModificateurId,
                DateModification = e.DateModification
            });
        }
        
        public IQueryable<Activite> GetActivitesRattachement()
        {
            return Context.Activites.Where(x => x.IsActive && x.RattachementEntite == true).Select(e => new Activite
            {
                Id = e.Id,
                Code = e.Code,
                CreateurId = e.CreateurId,
                DateCreation = e.DateCreation,
                EnglishName = e.EnglishName,
                FrenchDescription = e.FrenchDescription,
                FrenchName = e.FrenchName,
                EnglishDescription = e.EnglishDescription,
                RattachementEntite = e.RattachementEntite,
                Validated = e.Validated,
                IsActive = e.IsActive,
                ModificateurId = e.ModificateurId,
                DateModification = e.DateModification
            });
        }

        public async Task<IEnumerable<TypeDate>> GetTypesDate(IEnumerable<TypeDateEnum> codes)
        {
            if (codes == null)
                return null;
            
            var strCodes = codes
                .Select(c => c.ToString())
                .ToList();

            return await Context
                .TypeDates
                .Where(t => t.IsActive && strCodes.Contains(t.Code))
                .ToListAsync();
        }

        public Activite GetActiviteById(int id)
        {
            return Context.Activites.SingleOrDefault(x => x.IsActive && x.Id == id);
        }

        public IEnumerable<Extension> GetExtension()
        {
            return Context.Extensions.ToList();
        }

        public IEnumerable<TypeException> GetTypeExceptions()
        {
            return Context.TypeExceptions.ToList();
        }

        public async Task<IEnumerable<Activite>> GetActiviteUtilisateur(string aspNetUserId, CancellationToken token)
        {
            return await Context
                .UtilisateurDirections
                .Where(d => d.Utilisateur.AspNetUsersId == aspNetUserId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated)
                .GroupBy(g => new
                {
                    g.Activite.Id,
                    g.Activite.Code,
                    g.Activite.EnglishName,
                    g.Activite.FrenchName,
                    g.Activite.FrenchDescription,
                    g.Activite.EnglishDescription
                })
                .Select(u => new Activite
                {
                    Id = u.Key.Id,
                    Code = u.Key.Code,
                    FrenchName = u.Key.FrenchName,
                    FrenchDescription = u.Key.FrenchDescription,
                    EnglishName = u.Key.EnglishName,
                    EnglishDescription = u.Key.EnglishDescription
                })
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public IQueryable<Application> GetApplications()
        {
            return Context.Applications
                .Include(x => x.Environnement)
                .Where(x => x.IsActive);
        }

        public IQueryable<ApplicationAvis> GetApplicationSAviss()
        {
            return Context.ApplicationAviss
                .Include(x => x.EnvironnementAvis)
                .Where(x => x.IsActive);
        }

        public IQueryable<ApplicationAvis> GetApplicationAvisIms()
        {
            return Context.ApplicationAviss
                .Include(x => x.EnvironnementAvis)
                .Where(x => x.IsActive);
        }

        public IQueryable<ApplicationAvisSi> GetApplicationAvisSis()
        {
            return Context.ApplicationAvisSis
                .Include(x => x.EnvironnementAvisSi)
                .Where(x => x.IsActive);
        }

        public IQueryable<AspNetRoleClaim> GetAspNetRoleClaims(string roleId)
        {
            return Context
                .AspNetRoleClaims
                .Include(x => x.Role)
                .Where(u => u.RoleId == roleId)
                .Select(e => new AspNetRoleClaim { RoleId = e.RoleId, ClaimValue = e.ClaimValue, Role = e.Role });
        }

        public IQueryable<AspNetRole> GetAspNetRoles()
        {
            return Context.AspNetRoles;
        }

        public AspNetUser GetAspNetUser(IUserInfoService userInfoService)
        {
            var result = Context.AspNetUsers
                .Where(u => u.Utilisateur.AspNetUsersId == userInfoService.UserId)
                .Include(x => x.AspNetUserClaims)
                .Include(x => x.AspNetUserRoles)
                .FirstOrDefault();
            return result;
        }

        public AspNetUser GetAspNetUser(string userName)
        {
            var result = Context.AspNetUsers.FirstOrDefault(u => u.UserName == userName);
            return result;
        }

        public IQueryable<AspNetUser> GetAspNetUsers()
        {
            return Context.AspNetUsers.Include(x => x.Utilisateur);
        }

        public IQueryable<AutorisationPpf> GetAutorisationPpfs()
        {
            return Context.AutorisationPpfs.Where(x => x.IsActive);
        }

        public IQueryable<BamCeiling> GetBamCeilings()
        {
            return Context.BamCeilings.Where(x => x.IsActive);
        }

        public IQueryable<StatutActionsGda> GetStatutActionsGdas()
        {
            return Context.StatutActionsGdas.Where(x => x.IsActive).Include(x => x.TypologieGel);
        }

        public IQueryable<StatutImmediateActionsGda> GetStatutImmediateActionsGdas()
        {
            return Context.StatutImmediateActionsGdas.Where(x => x.IsActive);
        }

        public IQueryable<StatutOscGda> GetStatutOscGdas()
        {
            return Context.StatutOscGdas.Where(x => x.IsActive);
        }

        public IQueryable<DebitCreditOsc> GetDebitCreditOscs()
        {
            return Context.DebitCreditOscs.Where(x => x.IsActive);
        }

        public IQueryable<TypesLiens> GetTypesLiens()
        {
            return Context.TypesLiens.Where(x => x.IsActive);
        }

        public IQueryable<TypesActifs> GetTypesActifs()
        {
            return Context.TypesActifs
                          .Where(x => x.IsActive)
                          .Include(x => x.CategorieAvoir);
        }
        public IQueryable<TypologiesActifs> GetTypologiesActifs()
        {
            return Context.TypologiesActifs.Where(x => x.IsActive);
        }

        public IQueryable<CcBlocked> GetCcBlocked()
        {
            return Context.CcBlocked.Where(x => x.IsActive);
        }

        public IQueryable<SafetyInstructions> GetSafetyInstructions()
        {
            return Context.SafetyInstructions.Where(x => x.IsActive);
        }

        public IQueryable<AvisLab> GetAvisLabs()
        {
            return Context.AvisLabs;
            /*.Where(x => x.IsActive)*/
        }

        public IQueryable<NatureSoupconFraudeFiscale> GetNatureSoupconFraudeFiscalesLab()
        {
            return Context.NatureSoupconFraudeFiscales;
        }
        public IQueryable<NatureInfractionPenale> GetNatureInfractionPenalesLab()
        {
            return Context.NatureInfractionPenales;
        }
        public IQueryable<VisaLab> GetVisaLabs()
        {
            return Context.VisaLabs;
            /*.Where(x => x.IsActive)*/
        }

        public IQueryable<TypeLegislationLab> GetTypeLegislationLabs()
        {
            return Context.TypeLegislationLabs.Where(x => x.IsActive);
        }

        public IQueryable<CanalEntreeEnRelation> GetCanalsEntreeEnRelation()
        {
            return Context.CanalEntreeEnRelations;
        }

        public IQueryable<CategorieCanalBdf> GetCategorieCanalBdf()
        {
            return Context.CategorieCanalBdfs.Include(x => x.CanalBdf);
        }

        public IQueryable<CategorieEtablissementDeclarant> GetCategorieEtablissementDeclarant()
        {
            return Context.CategorieEtablissementDeclarants.Include(x => x.EtablissementDeclarant);
        }
        public IQueryable<ReferentielImmediateActionsGda> GetReferentielImmediateActionsGda()
        {
            return Context.ReferentielImmediateActionsGdas.Where(x => x.IsActive);
        }

        public IQueryable<CategorieFraude> GetCategorieFraudes()
        {
            return Context.CategorieFraudes.Where(x => x.IsActive);
        }
        public IQueryable<CategorieGDR> GetCategorieGDRs()
        {
            return Context.CategorieGDRs.Where(x => x.IsActive);
        }
        public IQueryable<ApplicationAquisition> GetApplicationAquisitions()
        {
            return Context.ApplicationAquisitions.Where(x => x.IsActive);
        }
        public IQueryable<CinematiqueGDR> GetCinematiqueGDRs()
        {
            return Context.CinematiqueGDRs.Where(x => x.IsActive);
        }
        public IQueryable<CapaciteJuridique> GetCapaciteJuridiques()
        {
            return Context.CapaciteJuridiques.Where(x => x.IsActive);
        }
        public IQueryable<CategorieGda> GetCategorieGdas()
        {
            return Context.CategorieGdas.Where(x => x.IsActive);
        }

        public IQueryable<CategorieLab> GetCategorieLabs()
        {
            return Context.CategorieLabs;
            /*.Where(x => x.IsActive)*/
        }

        public IQueryable<CategorieGroupeLab> GetCategorieGroupeLabs()
        {
            return Context.CategorieGroupeLabs.Where(x => x.IsActive);
        }

        public IQueryable<OrigineGroupeLab> GetOrigineGroupeLabs()
        {
            return Context.OrigineGroupeLabs;
            /*.Where(x => x.IsActive)*/
        }

        public IQueryable<CategorieTracfin> GetCategorieTracfins()
        {
            return Context.CategorieTracfins.Where(x => x.IsActive);
        }

        public IQueryable<GroupeCategorieLab> GetGroupeCategorieLabs()
        {
            return Context.GroupeCategorieLabs.Where(x => x.IsActive);
        }


        public IQueryable<GroupeOrigineLab> GetGroupeOrigineLabs()
        {
            return Context.GroupeOrigineLabs;
        }
        public IQueryable<NatureDs> GetNaturesDs()
        {
            return Context
                .NaturesDs
                .Where(x => x.IsActive);
        }
        public IQueryable<CategorieContributeur> GetCategoriesContributeurs()
        {
            return Context
                .CategoriesContributeur
                .Where(x => x.IsActive);
        }
        public IQueryable<Contributeur> GetContributeurs()
        {
            return Context
                .Contributeurs
                .Include(c => c.Categorie)
                .Where(x => x.IsActive);
        }
        public IQueryable<FonctionLab> GetFonctionsLabs()
        {
            return Context.FonctionsLabs;
        }

        public IQueryable<InformationRequestCloseReason> GetInformationRequestCloseReasons()
        {
            return Context.InformationRequestCloseReasons
                .Where(cr => cr.IsActive);
        }

        public IQueryable<CategorieModeOperatoire> GetCategorieModeOperatoire()
        {
            return Context
                .CategorieModeOperatoires
                .Include(x => x.ModeOperatoire);
        }

        public IQueryable<CategorieMotifRejetCheque> GetCategorieMotifRejetCheque()
        {
            return Context
                .CategorieMotifRejetCheques
                .Include(x => x.MotifRejetCheque);
        }

        public IQueryable<CategorieTypeCollecteCheque> GetCategorieTypeCollecteCheque()
        {
            return Context
                .CategorieTypeCollecteCheques
                .Include(x => x.TypeCollecteCheque);
        }

        public IQueryable<CategorieTypeCollecteCheque> GetCategorieTypeCollecteCheques()
        {
            return Context
                .CategorieTypeCollecteCheques
                .Include(x => x.TypeCollecteCheque);
        }

        public IQueryable<CategorieTypePaiement> GetCategorieTypePaiements()
        {
            return Context
                .CategorieTypePaiements
                .Include(x => x.TypePaiement);
        }

        public IQueryable<CategorieTypologieBdf> GetCategorieTypologieBdf()
        {
            return Context.CategorieTypologieBdfs
                .Include(x => x.TypologieBdf);
        }
        public IQueryable<TypologieGda> GetTypologieGda()
        {
            return Context.TypologieGdas;
        }

        public IQueryable<TypeDegel> GetTypeDegel()
        {
            return Context.TypeDegels;
        }

        public IQueryable<Civilite> GetCivilites()
        {
            return Context.Civilites.Where(x => x.IsActive);
        }

        public IQueryable<ComplementMotifRejetCheque> GetComplementMotifRejetCheques()
        {
            return Context
                .ComplementMotifRejetCheques
                .Include(x => x.MotifRejetCheque);
        }

        public IQueryable<ComplementVoie> GetComplementVoies()
        {
            return Context.ComplementVoies.Where(x => x.IsActive);
        }

        public Utilisateur GetConnectedUser(string aspUserId)
        {
            var user = Context.Utilisateurs
                .Select(x => new Utilisateur
                {
                    Id = x.Id,
                    AspNetUsersId = x.AspNetUsersId,
                    AspNetUsers = x.AspNetUsers,
                    Nom = x.Nom.ToUpper(CultureInfo.CurrentCulture),
                    Prenom = x.Prenom,
                    Email = x.Email,
                    Telephone = x.Telephone,
                    Initiales = x.Initiales,
                    AccesBacarat = x.AccesBacarat,
                    LangueId = x.LangueId,
                    LastPasswordChangedDate = x.LastPasswordChangedDate,
                    DirectionAttacheId = x.DirectionAttacheId,
                    DirectionAttache =
                        x.DirectionAttache != null
                            ? new Direction
                            {
                                Id = x.DirectionAttacheId,
                                Nom = x.DirectionAttache.Nom,
                                Abreviation = x.DirectionAttache.Abreviation,
                                IsFlge = x.DirectionAttache.IsFlge,
                                IsTotus = x.DirectionAttache.IsTotus,
                                IsActive = x.DirectionAttache.IsActive,
                                IsDDC = x.DirectionAttache.IsDDC,
                                ParentId = x.DirectionAttache.ParentId,
                                CarnetAdresses = x.DirectionAttache.CarnetAdresses != null
                                    ? new CarnetAdresses
                                    {
                                        Id = x.DirectionAttache.CarnetAdresses.Id,
                                        NomAdresse = x.DirectionAttache.CarnetAdresses.NomAdresse,
                                        Mail = x.DirectionAttache.CarnetAdresses.Mail
                                    }
                                    : null
                            }
                            : null,
                    UtilisateurDirections =
                        x.UtilisateurDirections
                })
                .FirstOrDefault(x => x.AspNetUsersId == aspUserId);

            return user;
        }


        public async Task<Utilisateur> GetUtilisateurById(int id, CancellationToken token = default)
        {
            var user = await Context.Utilisateurs
                .Select(x => new Utilisateur
                {
                    Id = x.Id,
                    AspNetUsersId = x.AspNetUsersId,
                    Nom = x.Nom.ToUpper(CultureInfo.CurrentCulture),
                    Prenom = x.Prenom,
                    Email = x.Email,
                    Telephone = x.Telephone,
                    Initiales = x.Initiales,
                    AccesBacarat = x.AccesBacarat,
                    LangueId = x.LangueId,
                    DirectionAttacheId = x.DirectionAttacheId
                })
                .FirstOrDefaultAsync(x => x.Id == id, token)
                .ConfigureAwait(false);

            return user;
        }

        public Utilisateur GetCurrentUser(string userId)
        {
            var user = Context.Utilisateurs
                .Include(x => x.AspNetUsers)
                .Include(x => x.DirectionAttache)
                .Select(x => new Utilisateur
                {
                    Id = x.Id,
                    Nom = x.Nom.ToUpper(CultureInfo.CurrentCulture),
                    Prenom = x.Prenom,
                    Email = x.Email,
                    LangueId = x.LangueId,
                    DirectionAttacheId = x.DirectionAttacheId
                })
                .FirstOrDefault(x => x.AspNetUsersId == userId);

            return user;
        }

        public IQueryable<Defaillance> GetDefaillances()
        {
            return Context.Defaillances.Where(x => x.IsActive);
        }

        public IQueryable<DefaillanceAvis> GetDefaillanceSAviss()
        {
            return Context.DefaillanceAviss.Where(x => x.IsActive);
        }

        public IQueryable<DefaillanceAvis> GetDefaillanceAvisIms()
        {
            return Context.DefaillanceAviss.Where(x => x.IsActive);
        }

        public IQueryable<IntermediaryAgreementNotice> GetIntermediaryAgreementNotices()
        {
            return Context.IntermediaryAgreementNotices.Where(x => x.IsActive);
        }

        public IQueryable<FinalAgreementNotice> GetFinalAgreementNotices()
        {
            return Context.FinalAgreementNotices.Where(x => x.IsActive);
        }

        public IQueryable<DefaillanceAvisSi> GetDefaillanceAvisSis()
        {
            return Context.DefaillanceAvisSis.Where(x => x.IsActive);
        }

        public IQueryable<Devise> GetDevises()
        {
            return Context.Devises.Where(x => x.IsActive);
        }


        public IQueryable<Bunit> GetBunits()
        {
            return Context.Bunits.Where(x => x.IsActive);
        }

        public IQueryable<Direction> GetDirectionByActivite(int activiteId, IUserInfoService userInfoService)
        {
            var guid = userInfoService?.UserId;
            var result = Context.UtilisateurDirections
                .Include(x => x.Direction)
                .Include(x => x.Utilisateur)
                .Include(x => x.Activite)
                .Where(u => u.Utilisateur.AspNetUsersId == guid && u.ActiviteId == activiteId &&
                            u.StatutUtilisateurId == (int)EStatutUtilisateur.Validated)
                .GroupBy(g => new { g.Direction.Id, g.Direction.Nom, g.Direction.Abreviation })
                .Select(r => new Direction { Abreviation = r.Key.Abreviation, Id = r.Key.Id, Nom = r.Key.Nom });
            return result;
        }

        public async Task<IEnumerable<Pays>> GetPaysByDirectionAsync(CancellationToken token = default)
        {
            return await Context.Directions
                .Include(x => x.Pays)
                .Where(x => x.IsActive && x.PaysId != null && x.PaysId != 0)
                .GroupBy(g => new { g.Pays.Id, g.Pays.IsoFrenchName, g.Pays.IsoEnglishName })
                .Select(r => new Pays
                { Id = r.Key.Id, IsoFrenchName = r.Key.IsoFrenchName, IsoEnglishName = r.Key.IsoEnglishName })
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Direction>> GetDirectionFlgeAsync(CancellationToken token)
        {
            return await Context
                .Directions
                .Where(x => x.IsFlge && x.IsActive)
                .Select(x => new Direction
                { Id = x.Id, Nom = x.Nom, Abreviation = x.Abreviation, CodeBic = x.CodeBic, CodeLei = x.CodeLei })
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Direction>> GetDirectionAllAsync(CancellationToken token)
        {
            return await Context
                .Directions
                .Where(x => x.IsActive)
                .Select(x => new Direction
                { Id = x.Id, Nom = x.Nom, Abreviation = x.Abreviation, CodeBic = x.CodeBic, CodeLei = x.CodeLei })
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public IQueryable<Direction> GetDirectionHabiliteByUtilisateurId(int utilisateurId)
        {
            return Context
                .UtilisateurDirections
                .Include(x => x.Direction)
                .Include(x => x.Utilisateur)
                .Include(x => x.Utilisateur.AspNetUsers)
                .Where(d => d.UtilisateurId == utilisateurId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated && d.Direction.IsActive)
                .Select(ud => ud.Direction);
        }


        public IQueryable<Direction> GetAllowedChildrenDepartments(int departmentId, int userId)
        {
            return Context
                .Directions
                .Where(d => d.IsActive &&
                            d.ParentId == departmentId);
        }

        public IList<Direction> GetAllowedDepartmentsTree(int rootDepartmentId, int userId)
        {
            var result = new List<Direction>();
            var children = GetAllowedChildrenDepartments(rootDepartmentId, userId).ToList();
            if (children.Any())
            {
                result.AddRange(children);
                result.AddRange(children.SelectMany(child => GetAllowedDepartmentsTree(child.Id, userId)).ToList());
            }

            return result;
        }

        public IEnumerable<Direction> GetDirectionHabiliteUser(int activiteId, IUserInfoService userInfoService)
        {
            var result = new List<Direction>();

            result.AddRange(Context
                .UtilisateurDirections
                .Include(x => x.Direction)
                .Include(x => x.Activite)
                .Include(x => x.Utilisateur)
                .Where(d => d.Utilisateur.AspNetUsersId == userInfoService.UserId &&
                            d.ActiviteId == activiteId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.Direction.IsActive)
                .Select(ud => ud.Direction));

            return result.DistinctBy(x => x.Id);
        }

        public IEnumerable<Direction> GetDirectionHabiliteUserLecture(int activiteId, IUserInfoService userInfoService)
        {
            var result = new List<Direction>();

            result.AddRange(Context
                .UtilisateurDirections
                .Include(x => x.Direction)
                .Include(x => x.Activite)
                .Include(x => x.Utilisateur)
                .Where(d => d.Utilisateur.AspNetUsersId == userInfoService.UserId &&
                            d.ActiviteId == activiteId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.Read &&
                            d.Direction.IsActive)
                .Select(ud => ud.Direction));

            return result.DistinctBy(x => x.Id);
        }

        public IEnumerable<Direction> GetDirectionHabiliteUserBlocked(int activiteId, IUserInfoService userInfoService)
        {
            var result = new List<Direction>();

            result.AddRange(Context
                .UtilisateurDirections
                .Include(x => x.Direction)
                .Include(x => x.Activite)
                .Include(x => x.Utilisateur)
                .Where(d => d.Utilisateur.AspNetUsersId == userInfoService.UserId &&
                            d.ActiviteId == activiteId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Bloqued &&
                            d.Direction.IsActive)
                .Select(ud => ud.Direction));

            return result.DistinctBy(x => x.Id);
        }

        public IEnumerable<Direction> GetDirectionsFlge(int activiteId, IUserInfoService userInfoService)
        {
            var result = new List<Direction>();
            var utilisateurDirections = Context.UtilisateurDirections
                .Include(x => x.Direction)
                .Include(x => x.Activite)
                .Include(x => x.Utilisateur)
                .Where(d => d.Direction.IsFlge &&
                d.Direction.IsActive &&
                d.ActiviteId == activiteId && 
                d.Utilisateur.AspNetUsersId == userInfoService.UserId &&
                d.StatutUtilisateurId == 1).ToList();
            foreach (var item in utilisateurDirections)
            {
                result.AddRange(Context.Directions
                        .Where(x => x.ParentId == item.DirectionId)
                        .ToList());
            }
            if (utilisateurDirections != null && utilisateurDirections.Any())
            {
                result.AddRange(utilisateurDirections.Select(x => x.Direction));
            }
            return result.DistinctBy(x => x.Id);
        }

        public IEnumerable<Direction> GetDirectionHabiliteUserEcriture(int activiteId, IUserInfoService userInfoService)
        {
            var result = new List<Direction>();

            result.AddRange(Context
                .UtilisateurDirections
                .Include(x => x.Direction)
                .Include(x => x.Activite)
                .Include(x => x.Utilisateur)
                .Where(d => d.Utilisateur.AspNetUsersId == userInfoService.UserId &&
                            d.ActiviteId == activiteId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.Write &&
                            d.Direction.IsActive)
                .Select(ud => ud.Direction));

            return result.DistinctBy(x => x.Id);
        }

        public IEnumerable<Direction> GetDirectionFlgeHabiliteUserEcriture(int activiteId, IUserInfoService userInfoService)
        {
            var result = new List<Direction>();

            result.AddRange(Context
                .UtilisateurDirections
                .Include(x => x.Direction)
                .Include(x => x.Activite)
                .Include(x => x.Utilisateur)
                .Where(d => d.Utilisateur.AspNetUsersId == userInfoService.UserId &&
                            d.ActiviteId == activiteId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.Write &&
                            d.Direction.IsActive && d.Direction.IsFlge)
                .Select(ud => ud.Direction));

            return result.DistinctBy(x => x.Id);
        }

        public IEnumerable<Direction> GetDirectionHabiliteUserReferentiel(int activiteId,
            IUserInfoService userInfoService)
        {
            var result = new List<Direction>();

            result.AddRange(Context
                .UtilisateurDirections
                .Include(x => x.Direction)
                .Include(x => x.Activite)
                .Include(x => x.Utilisateur)
                .Where(d => d.Utilisateur.AspNetUsersId == userInfoService.UserId &&
                            d.ActiviteId == activiteId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.AdminReferentiel &&
                            d.Direction.IsActive)
                .Select(ud => ud.Direction));

            return result.DistinctBy(x => x.Id);
        }

        public IEnumerable<Direction> GetDirectionHabilites(int activiteId = 0, int utilisateurId = 0)
        {
            var result = Context.UtilisateurDirections
                .Where(d => d.UtilisateurId == utilisateurId &&
                            (d.ActiviteId == activiteId || activiteId == 0) &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.Direction.IsActive)
                .Select(ud => ud.Direction)
                .ToList();

            return result.DistinctBy(x => x.Id);
        }

        public IEnumerable<Direction> GetDirectionHabiliteConfidents(int activiteId, int utilisateurId)
        {
            var result = Context.UtilisateurDirections
                .Where(d => d.UtilisateurId == utilisateurId &&
                            d.ActiviteId == activiteId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.Confidentiality == true &&
                            d.Direction.IsActive)
                .Select(ud => ud.Direction)
                .ToList();

            return result.DistinctBy(x => x.Id);
        }

        public async Task<IEnumerable<Direction>> GetDirectionHabiliteUtilisateur(int utilisateurId,
            int activiteId,
            CancellationToken token = default)
        {
            var result = await Context
                .UtilisateurDirections
                .Include(x => x.Direction)
                .Where(d => d.UtilisateurId == utilisateurId &&
                            d.ActiviteId == activiteId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.Direction.IsActive)
                .Select(ud => ud.Direction)
                .ToListAsync(token)
                .ConfigureAwait(false);
            return result.DistinctBy(x => x.Id);
        }

        public IQueryable<Direction> GetDirectionHabilitionUtilisateur(int utilisateurId, int activiteId)
        {
            var result = Context
                .UtilisateurDirections
                .Include(x => x.Direction)
                .Where(d => d.UtilisateurId == utilisateurId &&
                            d.ActiviteId == activiteId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.Direction.IsActive)
                .Select(ud => ud.Direction);
            return result;
        }

        public IQueryable<Direction> GetDirectionHabilitionAdminReferentiel(int utilisateurId, int activiteId)
        {
            var result = Context
                .UtilisateurDirections
                .Include(x => x.Direction)
                .Where(d => d.UtilisateurId == utilisateurId &&
                            d.ActiviteId == activiteId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.Direction.IsActive &&
                            d.AdminReferentiel)
                .Select(ud => ud.Direction);
            return result;
        }

        public IQueryable<Direction> GetDirectionHabilitions(int utilisateurId)
        {
            var result = Context
                .UtilisateurDirections
                .Include(x => x.Direction)
                .Where(d => d.UtilisateurId == utilisateurId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.Direction.IsActive)
                .Select(ud => ud.Direction);
            return result;
        }
        public IQueryable<Direction> GetDirectionHabilitionsActivite(int utilisateurId, int activiteId)
        {
            var result = Context
                .UtilisateurDirections
                .Include(x => x.Direction)
                .Where(d => d.UtilisateurId == utilisateurId &&
                 d.ActiviteId == activiteId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.Direction.IsActive)
                .Select(ud => ud.Direction);
            return result;
        }

        public IQueryable<Direction> GetDirectionHabilitionsActiviteConfidentiel(int utilisateurId, int activiteId)
        {
            var result = Context
                .UtilisateurDirections
                .Include(x => x.Direction)
                .Where(d => d.UtilisateurId == utilisateurId &&
                 d.ActiviteId == activiteId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.Direction.IsActive &&
                            d.Confidentiality)
                .Select(ud => ud.Direction);
            return result;
        }
        public IQueryable<Direction> GetDirectionPorteurs(int activiteId, int utilisateurId)
        {
            var directionHabilites = Context.UtilisateurDirections
                .Where(d => d.UtilisateurId == utilisateurId && d.ActiviteId == activiteId)
                .Select(ud => ud.Direction);


            var direcs = Context.Directions
                .Where(x => (directionHabilites.Any(c => c.Id == x.Id) ||
                             directionHabilites.Any(pp => pp.ParentId == x.Id)) &&
                            x.IsActive);

            var result = Context.Directions
                .Where(x => (direcs.Any(c => c.Id == x.Id) || direcs.Any(pp => pp.ParentId == x.Id) || x.Id == 1) &&
                            x.IsActive);

            return result;
        }


        public IQueryable<Direction> GetDirectionParentAndEntites(int activiteId, int utilisateurId)
        {
            var directionHabilites = Context.UtilisateurDirections
                .Where(d => d.UtilisateurId == utilisateurId && d.ActiviteId == activiteId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated)
                .Select(ud => ud.Direction);


            var direcs = Context.Directions.Where(x =>
                (directionHabilites.Any(pp => pp.Id == x.Id) || directionHabilites.Any(pp => pp.Id == x.ParentId)) &&
                x.IsActive);


            var result = Context.Directions.Where(x =>
                (direcs.Any(dd => dd.Id == x.Id) || direcs.Any(dd => dd.Id == x.ParentId)) && x.IsActive);

            return result;
        }


        public async Task<IEnumerable<Direction>> GetDirectionEntiteLocales(int activiteId,
            int utilisateurId,
            CancellationToken token = default)
        {
            var parents = Context.UtilisateurDirections
                .Where(d => d.UtilisateurId == utilisateurId && d.ActiviteId == activiteId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated)
                .Select(ud => ud.Direction.Id);

            var result = await Context.Directions
                .Where(x => parents.Any(c => c == x.Id) && x.IsActive)
                .ToListAsync(token)
                .ConfigureAwait(false);

            return result;
        }

        public async Task<IEnumerable<Direction>> GetDirectionFlgeParents(int activiteId,
            int utilisateurId,
            CancellationToken token)
        {
            var result = await Context.UtilisateurDirections
                .Where(d => d.UtilisateurId == utilisateurId && d.ActiviteId == activiteId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated)
                .Select(ud => ud.Direction)
                .ToListAsync(token)
                .ConfigureAwait(false);

            return result;
        }


        public async Task<IEnumerable<Direction>> GetEntiteLocalByDirection(int directionId,
            CancellationToken token = default)
        {
            var direcs = Context.Directions
                .Where(x => x.ParentId == directionId && x.IsActive);


            var result = await Context.Directions
                .Where(x => (direcs.Any(dd => dd.Id == x.Id) || direcs.Any(dd => dd.Id == x.ParentId)) && x.IsActive)
                .ToListAsync(token)
                .ConfigureAwait(false);

            return result;
        }

        public IQueryable<Direction> GetDirectionActiviteAspNetUserRoleSearch(int activiteId,
            int roleUtilisateurId,
            IUserInfoService userInfoService)
        {
            var parents = Context
                .UtilisateurDirections
                .Where(d => d.Utilisateur.AspNetUsersId == userInfoService.UserId &&
                            d.ActiviteId == activiteId &&
                            d.Direction.IsActive &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.AspNetUserRoles.Any(c => c.RoleId == roleUtilisateurId.ToString()))
                .Select(ud => ud.Direction.Id);

            var result = Context.Directions
                .Where(x => (parents.Any(c => c == x.Id) || parents.Any(pp => pp == x.ParentId)) && x.IsActive);

            return result;
        }


        public IQueryable<Direction> GetDirectionActiviteAspNetUserRoleDelegation(int activiteId,
            int roleUtilisateurId,
            IUserInfoService userInfoService)
        {
            var result = Context
                .UtilisateurDirections
                .Where(d => d.Utilisateur.AspNetUsersId == userInfoService.UserId &&
                            d.ActiviteId == activiteId &&
                            d.Direction.IsActive &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.AspNetUserRoles.Any(c => c.RoleId == roleUtilisateurId.ToString()))
                .Select(ud => ud.Direction);

            return result;
        }


        public async Task<Direction> GetSpecificDirection(int activiteId,
            int roleUtilisateurId,
            int utilisateurId,
            CancellationToken token)
        {
            var result = await Context
                .UtilisateurDirections
                .Where(d => d.UtilisateurId == utilisateurId &&
                            d.ActiviteId == activiteId &&
                            d.Direction.IsActive &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.AspNetUserRoles.Any(c => c.RoleId == roleUtilisateurId.ToString()))
                .Select(ud => ud.Direction)
                .FirstOrDefaultAsync(token)
                .ConfigureAwait(false);

            return result;
        }


        public async Task<IEnumerable<Utilisateur>> GetAllUtilisateurLinkedwithParentdirection(int activiteId,
            int utilisateurId,
            List<string> roleIds,
            CancellationToken token)
        {
            var directionHabilites = Context.UtilisateurDirections
                .Where(d => d.Utilisateur.Id == utilisateurId && d.ActiviteId == activiteId)
                .Select(ud => ud.Direction);

            var direcs = Context.Directions
                .Where(x => (directionHabilites.Any(pp => pp.Id == x.Id) ||
                             directionHabilites.Any(pp => pp.Id == x.ParentId)) &&
                            x.IsActive);


            var directions = Context.Directions
                .Where(x => (direcs.Any(dd => dd.Id == x.Id) || direcs.Any(dd => dd.Id == x.ParentId)) && x.IsActive);

            var utilisateurs = await Context.UtilisateurDirections
                .Include(x => x.Utilisateur)
                .Where(u => directions.Any(d => u.DirectionId == d.Id) &&
                            u.ActiviteId == activiteId &&
                            u.AspNetUserRoles.Select(c => c.RoleId).Any(v => roleIds.Contains(v)))
                .GroupBy(g => new { g.Utilisateur.Id, g.Utilisateur.Nom, g.Utilisateur.Prenom })
                .Select(g => new Utilisateur { Id = g.Key.Id, Nom = g.Key.Nom, Prenom = g.Key.Prenom })
                .ToListAsync(token)
                .ConfigureAwait(false);
            return utilisateurs;
        }


        public async Task<IEnumerable<Direction>> GetDirectionLinkedwithParentdirection(int activiteId,
            int utilisateurId, CancellationToken token)
        {
            var directionHabilites = Context.UtilisateurDirections
                .Where(d => d.Utilisateur.Id == utilisateurId && d.ActiviteId == activiteId)
                .Select(ud => ud.Direction);

            var directions = await Context.Directions
                .Where(x => (directionHabilites.Any(pp => pp.Id == x.Id) ||
                             directionHabilites.Any(pp => pp.Id == x.ParentId)) &&
                            x.IsActive).ToListAsync(token).ConfigureAwait(false);


            return directions;
        }


        public IQueryable<Direction> GetDirectionFlgeUtilisateur(IUserInfoService userInfoService, int activiteId)
        {
            var result = Context
                .UtilisateurDirections
                .Where(d => d.Utilisateur.AspNetUsersId == userInfoService.UserId &&
                            d.ActiviteId == activiteId &&
                            d.Direction.IsActive &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.AspNetUserRoles
                                .Any(c => c.RoleId == Roles.ResponsableFlge.Value ||
                                          c.RoleId == Roles.CollaborateurFlge.Value))
                .Select(ud => ud.Direction);

            return result;
        }

        public IQueryable<Direction> GetDirectionUserAdmin(int activiteId, IUserInfoService userInfoService)
        {
            var result = Context
                .UtilisateurDirections
                .Where(d => d.Utilisateur.AspNetUsersId == userInfoService.UserId &&
                            d.Direction.IsActive &&
                            d.AspNetUserRoles
                                .Any(c => c.RoleId == Roles.AdminGlobal.Value ||
                                          c.RoleId == Roles.AdminLocal.Value) &&
                            d.ActiviteId == activiteId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.Direction.IsActive)
                .AsNoTracking()
                .Select(ud => ud.Direction);
            return result;
        }

        public IQueryable<Activite> GetActiviteUtilisateurByDirection(int directionId, int utilisateurId)
        {
            var result = Context
                .UtilisateurDirections
                .Where(d => d.UtilisateurId == utilisateurId &&
                            d.DirectionId == directionId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated)
                .AsNoTracking()
                .Select(ud => ud.Activite);
            return result;
        }

        public IQueryable<DirectionAccessible> GetDirectionPouvantAccedes(int activiteId, int directionId)
        {
            return Context.DirectionAccessibles
                .Include(x => x.Direction)
                .Include(x => x.DirectionAcc)
                .Where(x => x.DirectionAccId == directionId && x.ActiviteId == activiteId);
        }

        public IQueryable<DirectionAccessible> GetDirectionAccessibles(int activiteId, int directionId)
        {
            var dirAcces = Context.DirectionAccessibles
                .Include(x => x.Direction)
                .Include(x => x.DirectionAcc)
                .Include(x => x.DirectionAcc)
                .Where(x => x.DirectionId == directionId && x.ActiviteId == activiteId);

            return dirAcces;
        }

        public IQueryable<Direction> GetDirectionsAccessibles(IEnumerable<int> directions)
        {
            var results = Context.DirectionAccessibles
                .Include(x => x.Direction)
                .Where(x => directions.Contains(x.DirectionId.Value))
                .Select(x => x.DirectionAcc);
            return results;
        }

        public IEnumerable<Direction> GetDirectionsAccessiblesByActivite(int activiteId, int utilisateurId)
        {
            var resultIsoles = new List<Direction>();

            var directionIsoles = Context.UtilisateurDirections
                .Where(x => x.ActiviteId == activiteId &&
                            x.Isolated == true &&
                            x.UtilisateurId == utilisateurId &&
                            x.StatutUtilisateurId == (int)EStatutUtilisateur.Validated)
                .Select(x => x.Direction)
                .ToList();

            if (directionIsoles.Any()) resultIsoles.AddRange(directionIsoles);

            var directs = Context.UtilisateurDirections
                .Where(x => x.ActiviteId == activiteId &&
                            x.Isolated != true &&
                            x.UtilisateurId == utilisateurId)
                .Select(x => x.DirectionId);


            resultIsoles.AddRange(Context.Directions.Where(x => directs.Any(d => d == x.Id)).ToList());

            resultIsoles.AddRange(Context.DirectionAccessibles
                .Include(x => x.Direction)
                .Include(x => x.DirectionAcc)
                .Where(x => x.ActiviteId == activiteId && directs.Any(d => d == x.DirectionId))
                .Select(x => x.DirectionAcc)
                .Distinct()
                .ToList());

            return resultIsoles.DistinctBy(x => x.Id);
        }

        public async Task<Direction> GetDirectionWithUserId(int activiteId, int utilisateurId,
            CancellationToken token = default)
        {
            var direction = await Context.UtilisateurDirections.Include(x => x.Direction).FirstOrDefaultAsync(x =>
                x.ActiviteId == activiteId &&
                x.UtilisateurId == utilisateurId, token);

            return direction?.Direction;
        }


        public async Task<IEnumerable<Direction>> GetDirectionWithAccessibles(int activiteId, int utilisateurId,
            CancellationToken token = default)
        {
            var results = new List<Direction>();

            var directions = Context.UtilisateurDirections
                .Where(x => x.ActiviteId == activiteId &&
                            x.UtilisateurId == utilisateurId &&
                            x.StatutUtilisateurId == (int)EStatutUtilisateur.Validated)
                .Select(x => x.Direction);


            results.AddRange(directions);

            results.AddRange(await Context.DirectionAccessibles
                .Where(x => x.ActiviteId == activiteId && directions.Any(d => d.Id == x.DirectionId))
                .Select(x => x.DirectionAcc).ToListAsync(token)
                .ConfigureAwait(false));

            return results;
        }


        public async Task<IEnumerable<Direction>> GetDirectionAccessibleWithAccessDs(int activiteId, int utilisateurId,
            CancellationToken token = default)
        {
            var results = new List<Direction>();

            var directions = Context.UtilisateurDirections
                .Where(x => x.ActiviteId == activiteId &&
                            x.UtilisateurId == utilisateurId)
                .Select(x => x.Direction);

            results.AddRange(await Context.DirectionAccessibles
                .Where(x => x.AccesDs == true && x.ActiviteId == activiteId &&
                            directions.Any(d => d.Id == x.DirectionId))
                .Select(x => x.DirectionAcc).ToListAsync(token)
                .ConfigureAwait(false));

            return results;
        }


        public IQueryable<Direction> GetDirections()
        {
            var result = Context.Directions.Where(x => x.IsActive);
            return result;
        }
        public IQueryable<UtilisateurDirection> GetAllUtilisateurDirections()
        {
            return Context.UtilisateurDirections;
        }

        public IQueryable<Direction> GetDdcDirections()
        {
            var result = Context.Directions.Where(x => x.IsActive && x.IsDDC);
            return result;
        }

        public IEnumerable<Direction> GetDirectionsActive()
        {
            var result = Context.Directions.Where(x => x.IsActive)
                .Select(
                    x => new Direction
                    {
                        Id = x.Id,
                        Nom = x.Nom,
                        PaysId = x.PaysId,
                        CodeBic = x.CodeBic,
                        CodeLei = x.CodeLei,
                        IsActive = x.IsActive,
                        IsFlge = x.IsFlge,
                        Abreviation = x.Abreviation,
                        ParentId = x.ParentId
                    }).ToList();

            return result;
        }


        public async Task<IEnumerable<Direction>> GetDirectionUtilisateurActivite(int activiteId,
            int utilisateurId,
            CancellationToken token = default)
        {
            var result = await Context
                .UtilisateurDirections
                .Include(x => x.Direction)
                .Where(d => d.UtilisateurId == utilisateurId &&
                            d.Direction.IsActive &&
                            d.ActiviteId == activiteId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated)
                .Select(ud => ud.Direction)
                .ToListAsync(token)
                .ConfigureAwait(false);
            return result;
        }


        public async Task<IEnumerable<Domaine>> GetDomaineAsync(CancellationToken token)
        {
            return await Context.Domaines
                .Where(x => x.IsActive)
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TypologieDemandeAvis>> GetTypologieDemandeSAvisAsync(CancellationToken token)
        {
            return await Context.TypologieDemandeAviss
                .Where(x => x.IsActive && x.ActiviteId == (int)ActiviteModule.SAvis)
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TypologieDemandeAvis>> GetTypologieDemandeAvisImAsync(CancellationToken token)
        {
            return await Context.TypologieDemandeAviss
                .Where(x => x.IsActive && x.ActiviteId == (int)ActiviteModule.AvisIm)
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TypologieDemande>> GetTypologieDemandeAsync(CancellationToken token)
        {
            return await Context.TypologieDemandes
                .Where(x => x.IsActive)
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public IQueryable<Domaine> GetDomaines()
        {
            return Context.Domaines.Where(x => x.IsActive);
        }

        public IQueryable<TypologieDemandeAvis> GetTypologieDemandeSAviss()
        {
            return Context.TypologieDemandeAviss.Where(x => x.IsActive);
        }

        public IQueryable<TypologieDemandeAvis> GetTypologieDemandeAvisIms()
        {
            return Context.TypologieDemandeAviss.Where(x => x.IsActive);
        }

        public IQueryable<TypologieClientAvis> GetTypologieClientSAviss()
        {
            return Context.TypologieClientAviss.Where(x => x.IsActive);
        }

        public IQueryable<TypologieClientAvis> GetTypologieClientAvisIms()
        {
            return Context.TypologieClientAviss.Where(x => x.IsActive);
        }

        public IQueryable<TypologieDemande> GetTypologieDemandes()
        {
            return Context.TypologieDemandes.Where(x => x.IsActive);
        }

        public IQueryable<QualificationDossier> GetQualificationDossiers()
        {
            return Context.QualificationDossiers.Where(x => x.IsActive);
        }

        public IQueryable<QualificationDossierEscalade> GetQualificationDossierEscalades()
        {
            return Context.QualificationDossierEscalades.Where(x => x.IsActive);
        }

        public IQueryable<DossierLabAction> GetDossierLabActions()
        {
            return Context.DossierLabActions;
        }

        public IQueryable<DroitCompte> GetDroitComptes()
        {
            return Context.DroitComptes.Where(x => x.IsActive);
        }

        public EntiteFraude GetEntiteById(int id)
        {
            return Context
                .EntiteFraudes
                .Include(x => x.Modificateur)
                .Include(x => x.Createur)
                .Include(x => x.Pays)
                .Include(x => x.SecteurFraude)
                .Include(x => x.Direction)
                .FirstOrDefault(e => e.Id == id);
        }
        public EntiteGda GetEntiteGdaById(int id)
        {
            return Context
                .EntiteGdas
                .Include(x => x.Modificateur)
                .Include(x => x.Createur)
                .Include(x => x.Pays)
                .Include(x => x.SecteurGda)
                .Include(x => x.Direction)
                .FirstOrDefault(e => e.Id == id);
        }

        public Direction GetDirectionById(int id)
        {
            return Context
                .Directions
                .Include(x => x.Pays)
                .Include(x => x.Devise)
                .Include(x => x.Parent)
                .Include(x => x.Createur)
                .Include(x => x.Modificateur)
                .FirstOrDefault(e => e.Id == id);
        }

        private async Task<bool> DepartmentExists(int departmentId)
        {
            return await Context
                .Directions
                .AnyAsync(d => d.Id == departmentId)
                .ConfigureAwait(false);
        }

        public IEnumerable<Direction> GetDepartmentsByIdList(IList<int> idList)
        {
            if (idList == null) return null;
            return Context
                .Directions
                .Where(d => idList == null || idList.Contains(d.Id));
        }

        public IQueryable<EntiteFraude> GetEntiteFraudeByUserId(IUserInfoService userInfoService)
        {
            var result = Context.UtilisateurDirections
                .Include(x => x.Direction)
                .ThenInclude(x => x.Entites)
                .Include(x => x.Utilisateur)
                .Where(d => d.Utilisateur.AspNetUsersId == userInfoService.UserId && d.Direction.IsActive)
                .SelectMany(p => p.Direction.Entites);
            return result;
        }


        public IQueryable<EntiteFraude> GetEntitieByDirection(int directionId, bool isInclude = false)
        {
            if (isInclude)
                return Context.EntiteFraudes
                    .Include(x => x.Createur)
                    .Include(x => x.Modificateur)
                    .Include(x => x.SecteurFraude)
                    .Include(x => x.Pays)
                    .Where(x => x.DirectionId == directionId && x.IsActive);

            return Context.EntiteFraudes.Where(x => x.DirectionId == directionId);
        }

        public IQueryable<EntiteGda> GetEntiteGdaByUserId(IUserInfoService userInfoService)
        {
            var result = Context.UtilisateurDirections
                .Include(x => x.Direction)
                .ThenInclude(x => x.EntiteGdas)
                .Include(x => x.Utilisateur)
                .Where(d => d.Utilisateur.AspNetUsersId == userInfoService.UserId && d.Direction.IsActive)
                .SelectMany(p => p.Direction.EntiteGdas);
            return result;
        }


        public IQueryable<EntiteGda> GetEntitieGdaByDirection(int directionId, bool isInclude = false)
        {
            if (isInclude)
                return Context.EntiteGdas
                    .Include(x => x.Createur)
                    .Include(x => x.Modificateur)
                    .Include(x => x.SecteurGda)
                    .Include(x => x.Pays)
                    .Where(x => x.DirectionId == directionId && x.IsActive);

            return Context.EntiteGdas.Where(x => x.DirectionId == directionId);
        }

        public IQueryable<EntiteLab> GetEntiteLabs()
        {
            return Context.EntiteLabs
                .Include(x => x.Direction)
                .Where(x => x.IsActive)
                .Select(x => new EntiteLab
                {
                    Id = x.Id,
                    Code = x.Code,
                    DirectionId = x.DirectionId,
                    IsActive = x.IsActive,
                    IsAgence = x.IsAgence,
                    Liadr4 = x.Liadr4,
                    Lisp = x.Lisp,
                    PaysId = x.PaysId,
                    SecteurId = x.SecteurId,
                    Direction = new Direction
                    {
                        Id = x.Direction.Id,
                        IsDDC = x.Direction.IsDDC,
                        Nom = x.Direction.Nom
                    }
                });
        }

        public async Task<IEnumerable<EntiteLab>> GetEntitesLabByDirectionAsync(int directionId, bool isActive,
            CancellationToken token)
        {
            return await Context.EntiteLabs
                .Include(x => x.Pays)
                .Include(x => x.SecteurLab)
                .Include(x => x.Direction)
                .Where(x => (!isActive || x.IsActive) && x.DirectionId == directionId)
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<CategorieLab>> GetCategorieLabsByDirectionAsync(int directionId, bool isActive, bool isSanction, bool isReadOnly,
            CancellationToken token)
        {
            var result = Context.CategorieLabs.Where(x => (!isActive || x.IsActive) && x.DirectionId == directionId);
            if (isSanction && !isReadOnly)
            {
                result = result.Where(x => x.IsSanction);
            }
            return await result.ToListAsync(token).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TypeClientLab>> GetTypeClientLabsByDirectionAsync(int directionId, bool isActive,
            CancellationToken token)
        {
            return await Context.TypeClientLabs.Where(x => (!isActive || x.IsActive) && x.DirectionId == directionId)
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<SecteurEconomiqueLab>> GetSecteurEconomiqueByDirectionAsync(int directionId,
            bool isActive, CancellationToken token)
        {
            return await Context.SecteurEconomiqueLabs
                .Where(x => (!isActive || x.IsActive) && x.DirectionId == directionId)
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public EntiteLab GetEntiteLabById(int id)
        {
            return Context
                .EntiteLabs
                .Include(x => x.Modificateur)
                .Include(x => x.Createur)
                .Include(x => x.Pays)
                .Include(x => x.SecteurLab)
                .Include(x => x.Direction)
                .FirstOrDefault(e => e.Id == id);
        }

        public IQueryable<EntiteLab> GetEntiteLabByUserId(IUserInfoService userInfoService)
        {
            var result = Context.UtilisateurDirections
                .Include(x => x.Direction)
                .ThenInclude(x => x.EntiteLabs)
                .Include(x => x.Utilisateur)
                .Where(d => d.Utilisateur.AspNetUsersId == userInfoService.UserId && d.Direction.IsActive)
                .SelectMany(p => p.Direction.EntiteLabs);
            return result;
        }


        public IQueryable<EntiteLab> GetEntiteLabByDirection(int directionId, bool isInclude = false)
        {
            if (isInclude)
                return Context.EntiteLabs
                    .Include(x => x.Createur)
                    .Include(x => x.Modificateur)
                    .Include(x => x.SecteurLab)
                    .Include(x => x.Pays)
                    .Where(x => x.DirectionId == directionId && x.IsActive);

            return Context.EntiteLabs.Where(x => x.DirectionId == directionId);
        }

        public IQueryable<EntiteFraude> GetEntities()
        {
            return Context.EntiteFraudes.Where(x => x.IsActive);
        }
        public IQueryable<EntiteGda> GetEntitieGdas()
        {
            return Context.EntiteGdas.Where(x => x.IsActive);
        }

        public IQueryable<Environnement> GetEnvironnements()
        {
            return Context.Environnements.Where(x => x.IsActive);
        }

        public IQueryable<EnvironnementAvis> GetEnvironnementSAviss()
        {
            return Context.EnvironnementAviss.Where(x => x.IsActive);
        }

        public IQueryable<EnvironnementAvis> GetEnvironnementAvisIms()
        {
            return Context.EnvironnementAviss.Where(x => x.IsActive);
        }

        public IQueryable<EnvironnementAvisSi> GetEnvironnementAvisSis()
        {
            return Context.EnvironnementAvisSis.Where(x => x.IsActive);
        }

        public IQueryable<DecisionAvisSi> GetDecisionAvisSis()
        {
            return Context.DecisionAvisSis.Where(x => x.IsActive);
        }

        public IQueryable<EtatFraude> GetEtatFraude()
        {
            return Context.EtatFraudes.Where(x => x.IsActive);
        }


        public IQueryable<Evenement> GetEvenements()
        {
            return Context.Evenements.Where(x => x.IsActive);
        }

        public IQueryable<CategorieAvis> GetCategorieSAviss()
        {
            return Context.CategorieAviss.Where(x => x.IsActive && x.ActiviteId == (int)ActiviteModule.SAvis);
        }
        public IQueryable<CategorieOutilAvis> GetCategorieOutilSAviss()
        {
            return Context.CategorieOutilAviss.Where(x => x.IsActive && x.ActiviteId == (int)ActiviteModule.SAvis);
        }
        public IQueryable<PrioriteAvis> GetPrioriteSAviss()
        {
            return Context.PrioriteAviss.Where(x => x.IsActive);
        }

        public IQueryable<CategorieAvis> GetCategorieAvisIms()
        {
            return Context.CategorieAviss.Where(x => x.IsActive && x.ActiviteId == (int)ActiviteModule.AvisIm);
        }

        public IQueryable<SousCategorieAvis> GetSousCategorieSAviss()
        {
            return Context.SousCategorieAviss.Where(x => x.IsActive && x.ActiviteId == (int)ActiviteModule.SAvis);
        }

        public IQueryable<SousCategorieAvis> GetSousCategorieAvisIms()
        {
            return Context.SousCategorieAviss.Where(x => x.IsActive && x.ActiviteId == (int)ActiviteModule.AvisIm);
        }

        public IQueryable<DomaineAvis> GetDomaineSAviss()
        {
            return Context.DomaineAviss.Where(x => x.IsActive && x.ActiviteId == (int)ActiviteModule.SAvis);
        }

        public IQueryable<DomaineAvis> GetDomaineAvisIms()
        {
            return Context.DomaineAviss.Where(x => x.IsActive && x.ActiviteId == (int)ActiviteModule.AvisIm);
        }

        public IQueryable<DemandeDerogation> GetDemandeDerogations()
        {
            return Context.DemandeDerogations.Where(x => x.IsActive);
        }

        public IQueryable<FormeJuridique> GetFormeJuridiques()
        {
            return Context.FormeJuridiques.Where(x => x.IsActive);
        }

        public IQueryable<IdentificationProfessionnelle> GetIdentificationProfessionnelles()
        {
            return Context.IdentificationProfessionnelles.Where(x => x.IsActive);
        }


        public IQueryable<HistoriqueConnexion> GetHistoriqueConnexionByUserId(IUserInfoService userInfoService)
        {
            return Context.HistoriqueConnexions;
        }

        public IQueryable<HistoriqueConnexion> GetHistoriqueConnexions()
        {
            return Context.HistoriqueConnexions
                .Include(x => x.Utilisateur);
        }


        public IQueryable<Impact> GetImpacts()
        {
            return Context.Impacts.Where(x => x.IsActive);
        }

        public IQueryable<RegimeSanctionGda> GetRegimeSanctionGdas()
        {
            return Context.RegimeSanctionGdas.Where(x => x.IsActive);
        }


        public IQueryable<Langue> GetLangues()
        {
            return Context.Langues;
        }


        public IQueryable<MotifExemptionNotification> GetMotifExemptionNotifications()
        {
            return Context
                .MotifExemptionNotifications
                .Where(x => x.IsActive);
        }
        public IQueryable<MotifExemptionNotification> GetMotifExemptionNotificationsFraude()
        {
            return Context
                .MotifExemptionNotifications
                .Where(x => x.IsActive && x.Code != "DS");
        }
        public IQueryable<MotifExemptionNotificationGda> GetMotifExemptionNotificationGdas()
        {
            return Context
                .MotifExemptionNotificationGdas
                .Where(x => x.IsActive);
        }

        public IQueryable<TypeLienSupport> GetTypeLienSupports()
        {
            return Context.TypeLienSupports.Where(x => x.IsActive);
        }


        public IQueryable<TypeReferenceLab> GetTypeReferenceLabs()
        {
            return Context.TypeReferenceLabs.Where(x => x.IsActive);
        }

        public IQueryable<OrigineFraude> GetOrigineFraudes()
        {
            return Context.OrigineFraudes.Where(x => x.IsActive);
        }
        public IQueryable<OrigineGda> GetOrigineGdas()
        {
            return Context.OrigineGdas;
        }

        public IQueryable<OrigineLab> GetOrigineLabs()
        {
            return Context.OrigineLabs.Where(x => x.IsActive);
        }
        public IQueryable<OrigineLab> GetOrigineLabUpdateForms()
        {
            return Context.OrigineLabs.Where(x => x.IsActive);
        }

        public IQueryable<OrganismeLab> GetOrganismeLabs()
        {
            return Context.OrganismeLabs;
        }

        public IQueryable<Pays> GetPays()
        {
            return Context.Pays.Include(x => x.ZoneGeographique).Where(x => x.IsActive);
        }

        public IQueryable<Pays> GetAllPays()
        {
            return Context.Pays.Include(x => x.ZoneGeographique);
        }

        public IQueryable<PrincipalInstrumentFinancier> GetPrincipalInstrumentFinanciers()
        {
            return Context
                .PrincipalInstrumentFinanciers
                .Where(x => x.IsActive);
        }


        public IQueryable<Produit> GetProduits()
        {
            return Context
                .Produits
                .Include(pr => pr.Modificateur)
                .Where(x => x.IsActive);
        }

        public IQueryable<VoixRecouvrement> GetVoixRecouvrements()
        {
            return Context
                .VoixRecouvrements
                .Include(pr => pr.Modificateur)
                .Where(x => x.IsActive);
        }
        public IQueryable<ReferentielActionHorsCompte> GetReferentielActionHorsComptes()
        {
            return Context.ReferentielActionHorsComptes.Where(x => x.IsActive);
        }

        public IQueryable<Profession> GetProfessions()
        {
            return Context.Professions.Where(x => x.IsActive);
        }

        public IQueryable<RoleClient> GetRolesClient()
        {
            return Context.RoleClients.Where(x => x.IsActive);
        }

        public IQueryable<ScenarioNorkom> GetScenarioNorkoms()
        {
            return Context.ScenarioNorkoms.Where(x => x.IsActive);
        }

        public IQueryable<ScenarioLab> GetScenarioLabs()
        {
            return Context.ScenarioLabs.Where(x => x.IsActive);
        }

        public IQueryable<ApplicationScenarioLab> GetApplicationScenarioLabs()
        {
            return Context.ApplicationScenarioLabs.Where(x => x.IsActive);
        }


        public IQueryable<SecteurFraude> GetSecteurByDirection(int directionId)
        {
            return Context.Secteurs
                .Include(x => x.Modificateur)
                .Where(s => s.DirectionId == directionId);
        }

        public SecteurFraude GetSecteurById(int id)
        {
            return Context.Secteurs
                .Include(x => x.Modificateur)
                .Include(x => x.Createur)
                .Include(x => x.Direction)
                .FirstOrDefault(s => s.Id == id);
        }
        public IQueryable<SecteurGda> GetSecteurGdaByDirection(int directionId)
        {
            return Context.SecteurGdas
                .Include(x => x.Modificateur)
                .Where(s => s.DirectionId == directionId);
        }

        public SecteurGda GetSecteurGdaById(int id)
        {
            return Context.SecteurGdas
                .Include(x => x.Modificateur)
                .Include(x => x.Createur)
                .Include(x => x.Direction)
                .FirstOrDefault(s => s.Id == id);
        }

        public IQueryable<SecteurProfessionnel> GetSecteurProfessionnels()
        {
            return Context.SecteurProfessionnels
                .Where(x => x.IsActive);
        }

        public IQueryable<SecteurProfessionnel> GetSecteurProfessionnelAmfs()
        {
            return Context.SecteurProfessionnels
                .Where(x => x.IsActive);
        }


        public IQueryable<SecteurFraude> GetSecteurs()
        {
            return Context.Secteurs.Where(x => x.IsActive);
        }
        public IQueryable<SecteurGda> GetSecteurGdas()
        {
            return Context.SecteurGdas.Where(x => x.IsActive);
        }

        public IQueryable<SecteurLab> GetSecteurLabByDirection(int directionId)
        {
            return Context.SecteurLabs
                .Include(x => x.Modificateur)
                .Where(s => s.DirectionId == directionId);
        }

        public SecteurLab GetSecteurLabById(int id)
        {
            return Context.SecteurLabs
                .Include(x => x.Modificateur)
                .Include(x => x.Createur)
                .Include(x => x.Direction)
                .FirstOrDefault(s => s.Id == id);
        }

        public IQueryable<SecteurEconomiqueLab> GetSecteurEconomiqueLabs()
        {
            return Context.SecteurEconomiqueLabs;
            /*.Where(x => x.IsActive)*/
        }

        public IQueryable<SecteurLab> GetSecteurLabs()
        {
            return Context.SecteurLabs.Where(x => x.IsActive);
        }

        public IQueryable<Sexe> GetSexes()
        {
            return Context.Sexes.Where(x => x.IsActive);
        }

        public IQueryable<SituationFamiliale> GetSituationFamiliales()
        {
            return Context.SituationFamiliales
                .Where(x => x.IsActive);
        }

        public async Task<IEnumerable<StatutEscalade>>
            GetStatutDossierEscaladeAsync(CancellationToken token = default)
        {
            return await Context.StatutEscalades
                .Select(x => new StatutEscalade
                {
                    Id = x.Id,
                    Code = x.Code,
                    EnglishDescription = x.EnglishDescription,
                    FrenchDescription = x.FrenchDescription,
                    FrenchName = x.FrenchName,
                    EnglishName = x.EnglishName,
                    IsActive = x.IsActive
                })
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<StatutAvis>>
            GetStatutDossierSAvisAsync(CancellationToken token = default)
        {
            return await Context.StatutAviss
                .Select(x => new StatutAvis
                {
                    Id = x.Id,
                    Code = x.Code,
                    EnglishDescription = x.EnglishDescription,
                    FrenchDescription = x.FrenchDescription,
                    FrenchName = x.FrenchName,
                    EnglishName = x.EnglishName,
                    IsActive = x.IsActive
                })
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<StatutAvis>>
            GetStatutDossierAvisImAsync(CancellationToken token = default)
        {
            return await Context.StatutAviss
                .Select(x => new StatutAvis
                {
                    Id = x.Id,
                    Code = x.Code,
                    EnglishDescription = x.EnglishDescription,
                    FrenchDescription = x.FrenchDescription,
                    FrenchName = x.FrenchName,
                    EnglishName = x.EnglishName,
                    IsActive = x.IsActive
                })
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<StatutAvisSi>>
            GetStatutDossierAvisSiAsync(CancellationToken token = default)
        {
            return await Context.StatutAvisSis
                .Select(x => new StatutAvisSi
                {
                    Id = x.Id,
                    Code = x.Code,
                    EnglishDescription = x.EnglishDescription,
                    FrenchDescription = x.FrenchDescription,
                    FrenchName = x.FrenchName,
                    EnglishName = x.EnglishName,
                    IsActive = x.IsActive
                })
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public IQueryable<StatutDossierFraude> GetStatutDossierFraudes()
        {
            return Context.StatutDossierFraudes.Where(x => x.IsActive);
        }
        public IQueryable<StatutDossierGda> GetStatutDossierGdas()
        {
            return Context.StatutDossierGdas.Where(x => x.IsActive);
        }

        public IQueryable<StatutDossierLab> GetStatutDossierLabs()
        {
            return Context.StatutDossierLabs;
            /*.Where(x => x.IsActive)*/
        }

        public IQueryable<StatutDemandeInformationLab> GetStatutDemandeInformationLabs()
        {
            return Context.StatutDemandeInformationLabs;
            /*.Where(x => x.IsActive)*/
        }

        public IQueryable<TypeGarantie> GetTypesGaranties()
        {
            return Context.TypesGaranties;
        }
        public IQueryable<TypeOperation> GetTypesOperations()
        {
            return Context.TypesOperations;
        }

        public IQueryable<CriteresAlerteOrigine> GetCriteresAlerteOrigines()
        {
            return Context.CriteresAlerteOrigines;
        }

        public IQueryable<ModaliteFinancement> GetModaliteFinancements()
        {
            return Context.ModaliteFinancements;
        }

        public IQueryable<StatutDemandeInformation> GetStatutDemandeInformationEscalades()
        {
            return Context.StatutDemandeInformations;
            /*.Where(x => x.IsActive)*/
        }

        public IQueryable<StatutDemandeInformationFraude> GetStatutDemandeInformationFraudes()
        {
            return Context.StatutDemandeInformationFraudes;
            /*.Where(x => x.IsActive)*/
        }
        public IQueryable<StatutDemandeInformationGda> GetStatutDemandeInformationGdas()
        {
            return Context.StatutDemandeInformationGdas;
            /*.Where(x => x.IsActive)*/
        }

        public IQueryable<StatutDemandeInformationAvisSi> GetStatutDemandeInformationAvisSis()
        {
            return Context.StatutDemandeInformationAvisSis;
            /*.Where(x => x.IsActive)*/
        }

        public IQueryable<StatutDemandeInformationAvis> GetStatutDemandeInformationAviss()
        {
            return Context.StatutDemandeInformationAviss;
            /*.Where(x => x.IsActive)*/
        }

        public IQueryable<StatutEscalade> GetStatutEscalades()
        {
            return Context.StatutEscalades.Where(x => x.IsActive);
        }

        public IQueryable<StatutAvis> GetStatutAviss()
        {
            return Context.StatutAviss.Where(x => x.IsActive);
        }

        public IQueryable<StatutAvis> GetStatutAvisIms()
        {
            return Context.StatutAviss.Where(x => x.IsActive);
        }
        public IQueryable<StatutAvisSi> GetStatutAvisSis()
        {
            return Context.StatutAvisSis.Where(x => x.IsActive);
        }

        public IQueryable<StatutOperation> GetStatutOperations()
        {
            return Context.StatutOperations.Where(x => x.IsActive);
        }

        public IQueryable<StatutPersonneFraude> GetStatutPersonneFraude()
        {
            return Context.StatutPersonneFraudes
                .Where(x => x.IsActive);
        }
        public IQueryable<StatutPersonneGda> GetStatutPersonneGda()
        {
            return Context.StatutPersonneGdas
                .Where(x => x.IsActive);
        }


        public IQueryable<TypeAdresse> GetTypeAdresses()
        {
            return Context.TypeAdresses.Where(x => x.IsActive);
        }
        public IQueryable<TypeFonctionDirigeant> GetTypeFonctionDirigeants()
        {
            return Context.TypeFonctionDirigeants.Where(x => x.IsActive);
        }

        public IQueryable<LienUs> GetLienUss()
        {
            return Context.LienUss.Where(x => x.IsActive);
        }

        public IQueryable<UsLinkNature> GetUsLinkNatures()
        {
            return Context.UsLinkNatures.Where(x => x.IsActive);
        }

        public IQueryable<TypeFormulaire> GetTypeFormulaires()
        {
            return Context.TypeFormulaires.Where(x => x.IsActive);
        }
        public IQueryable<TypeFormulaire> GetTypeFormulairesTotus()
        {
            return Context.TypeFormulaires.Where(x => x.IsActive && x.Code == "IDT");
        }

        public IQueryable<TypeFormulaire> GetTypeFormulairesNotTotus()
        {
            return Context.TypeFormulaires.Where(x => x.IsActive && x.Code == "IDNT");
        }

        public IQueryable<TypeContrat> GetTypeContrats()
        {
            return Context.TypeContrats;
        }

        public IQueryable<TypeClientFraude> GetTypeClientFraudes()
        {
            return Context.TypeClientFraudes.Where(x => x.IsActive);
        }
        public IQueryable<ScenarioFraude> GetScenarioFraudes()
        {
            return Context
                .ScenarioFraudes
                .Include(sf => sf.OrigineFraude)
                .Where(x => x.IsActive);
        }
        public IQueryable<RegimeJuridiqueGda> GetRegimeJuridiqueGdas()
        {
            return Context.RegimeJuridiqueGdas.Where(x => x.IsActive);
        }

        public IQueryable<TypeClientLab> GetTypeClientLabs()
        {
            return Context.TypeClientLabs;
            /*.Where(x => x.IsActive)*/
        }

        public IQueryable<TypeClientLab> GetTypeClientLabs(int directionId)
        {
            return Context.TypeClientLabs
                          .AsNoTracking()
                          .Where(tc => tc.DirectionId == directionId);
        }

        public IQueryable<TypeImplicationLab> GetTypeImplicationLabs(bool isActive = true)
        {
            return Context.TypeImplicationLabs.Where(x => x.IsActive == isActive);
        }
        public IQueryable<ProfessionalIdentificationLab> GetProfessionalIdentificationLabs(bool isActive = true)
        {
            return Context.ProfessionalIdentificationLabs.Where(x => x.IsActive == isActive);
        }

        public IQueryable<TypeRelationAffaireLab> GetTypeRelationAffaireLabs()
        {
            return Context.TypeRelationAffaireLabs.Where(x => x.IsActive);
        }

        public IQueryable<TypeRelationAffaireFraude> GetTypeRelationAffaireFraudes()
        {
            return Context.TypeRelationAffaireFraudes.Where(x => x.IsActive);
        }
        public IQueryable<TypeRelationAffaireGda> GetTypeRelationAffaireGdas()
        {
            return Context.TypeRelationAffaireGdas.Where(x => x.IsActive);
        }

        public IQueryable<TypeCompte> GetTypeComptes()
        {
            return Context.TypeComptes.Where(x => x.IsActive);
        }


        public IQueryable<TypeDeclarationTracfin> GetTypeDeclarationTracFins()
        {
            return Context.TypeDeclarationTracfins
                .Where(x => x.IsActive);
        }

        public IQueryable<TypeDocument> GetTypeDocuments()
        {
            return Context.TypeDocuments.Where(x => x.IsActive);
        }

        public IQueryable<CategorieDocument> GetCategorieDocuments()
        {
            return Context.CategorieDocuments.Where(x => x.IsActive);
        }
        public IQueryable<TypeDocumentLab> GetTypeDocumentLabs()
        {
            return Context.TypeDocumentLabs.Where(x => x.IsActive);
        }

        public IQueryable<TypeLienPersonneMoraleMorale> GetTypeLienPersonneMoraleMorales()
        {
            return Context.TypeLienPersonneMoraleMorales
                .Where(x => x.IsActive);
        }

        public IQueryable<TypeLienPersonnePhysiquePhysique> GetTypeLienPersonnePhysiquePhysiques()
        {
            return Context.TypeLienPersonnePhysiquePhysiques
                .Where(x => x.IsActive);
        }

        public IQueryable<CategorieLienLab> GetCategorieLienLab()
        {
            return Context.CategorieLienLabs
                .Where(x => x.IsActive);
        }
        public IQueryable<TypeLienPersonneMoralePhysique> GetTypeLienPersonneMoralePhysiques()
        {
            return Context.TypeLienPersonneMoralePhysiques
                .Where(x => x.IsActive);
        }

        public IQueryable<TypeLienPersonnePhysiqueMorale> GetTypeLienPersonnePhysiqueMorales()
        {
            return Context.TypeLienPersonnePhysiqueMorales
                .Where(x => x.IsActive);
        }

        public IQueryable<InteretDroitVote> GetInteretDroitVotesCollection()
        {
            return Context.InteretDroitVotes
                .Where(x => x.IsActive);
        }
        public IQueryable<EntiteGroupeType> GetEntiteGroupeTypesCollection()
        {
            return Context.EntiteGroupeTypes
                .Where(x => x.IsActive);
        }
        public IQueryable<TypeFond> GetTypeFondsCollection()
        {
            return Context.TypeFonds
                .Where(x => x.IsActive);
        }
        public IQueryable<TypePieceIdentite> GetTypePieceIdentites()
        {
            return Context.TypePieceIdentites
                .Where(x => x.IsActive);
        }

        public IQueryable<TypeVoie> GetTypeVoies()
        {
            return Context.TypeVoies.Where(x => x.IsActive);
        }

        public IQueryable<TypeListeCriblage> GetTypeListeCriblages()
        {
            return Context.TypeListeCriblages;
        }

        public EquipeAnalyse GetUserEquipeAnalyse(int utilisateurId)
        {
            var utilisateurEquipe = Context.UtilisateurEquipeAnalyses
                .Include(e => e.EquipeAnalyse)
                .FirstOrDefault(x => x.UtilisateurId == utilisateurId);
            return utilisateurEquipe?.EquipeAnalyse;
        }

        public EquipeAnalyseAvis GetUserEquipeAnalyseSAvis(int utilisateurId)
        {
            var utilisateurEquipe = Context.UtilisateurEquipeAnalyseAviss
                .Include(e => e.EquipeAnalyseAvis)
                .FirstOrDefault(x => x.UtilisateurId == utilisateurId);
            return utilisateurEquipe?.EquipeAnalyseAvis;
        }

        public EquipeAnalyseAvis GetUserEquipeAnalyseAvisIm(int utilisateurId)
        {
            var utilisateurEquipe = Context.UtilisateurEquipeAnalyseAviss
                .Include(e => e.EquipeAnalyseAvis)
                .FirstOrDefault(x => x.UtilisateurId == utilisateurId);
            return utilisateurEquipe?.EquipeAnalyseAvis;
        }

        public EquipeAnalyseAvisSi GetUserEquipeAnalyseAvisSi(int utilisateurId)
        {
            var utilisateurEquipe = Context.UtilisateurEquipeAnalyseAvisSis
                .Include(e => e.EquipeAnalyseAvisSi)
                .FirstOrDefault(x => x.UtilisateurId == utilisateurId);
            return utilisateurEquipe?.EquipeAnalyseAvisSi;
        }

        public EquipeSuperviseurAvisSi GetUserEquipeSuperviseurAvisSi(int utilisateurId)
        {
            var utilisateurEquipe = Context.UtilisateurEquipeSuperviseurAvisSis
                .Include(e => e.EquipeSuperviseurAvisSi)
                .FirstOrDefault(x => x.UtilisateurId == utilisateurId);
            return utilisateurEquipe?.EquipeSuperviseurAvisSi;
        }

        public IEnumerable<EquipeAnalyse> GetUserEquipeAnalyses(int utilisateurId)
        {
            var utilisateurEquipe = Context.UtilisateurEquipeAnalyses
                .Include(e => e.EquipeAnalyse)
                .Where(x => x.UtilisateurId == utilisateurId).Select(x => x.EquipeAnalyse);
            return utilisateurEquipe;
        }

        public IEnumerable<EquipeAnalyseAvisSi> GetUserEquipeAnalysesAvisSi(int utilisateurId)
        {
            var utilisateurEquipe = Context.UtilisateurEquipeAnalyseAvisSis
                .Include(e => e.EquipeAnalyseAvisSi)
                .Where(x => x.UtilisateurId == utilisateurId).Select(x => x.EquipeAnalyseAvisSi);
            return utilisateurEquipe;
        }

        public IEnumerable<EquipeAnalyseAvis> GetUserEquipeAnalysesSAvis(int utilisateurId)
        {
            var utilisateurEquipe = Context.UtilisateurEquipeAnalyseAviss
                .Include(e => e.EquipeAnalyseAvis)
                .Where(x => x.UtilisateurId == utilisateurId).Select(x => x.EquipeAnalyseAvis);
            return utilisateurEquipe;
        }

        public IEnumerable<EquipeAnalyseAvis> GetUserEquipeAnalysesAvisIm(int utilisateurId)
        {
            var utilisateurEquipe = Context.UtilisateurEquipeAnalyseAviss
                .Include(e => e.EquipeAnalyseAvis)
                .Where(x => x.UtilisateurId == utilisateurId).Select(x => x.EquipeAnalyseAvis);
            return utilisateurEquipe;
        }

        public IEnumerable<Utilisateur> GetUtilisateurConnexionHistos(int directionId, int activiteId)
        {
            return Context
                .UtilisateurDirections
                .Include(x => x.Utilisateur)
                .Include("Utilisateur.HistoriqueConnexions")
                .Include("Utilisateur.Fonction")
                .AsEnumerable()
                .Where(u => u.DirectionId == directionId && u.ActiviteId == activiteId &&
                            u.StatutUtilisateurId == (int)EStatutUtilisateur.Validated)
                .Select(s => s.Utilisateur)
                .DistinctBy(x => x.Id);
        }


        public IEnumerable<Utilisateur> GetAdminFLGEByDirection(int directionFlgeId, int activiteId)
        {
            var results = Context.AspNetUserRoles
                .Include(x => x.UtilisateurDirection.Utilisateur)
                .AsEnumerable()
                .Where(u => u.UtilisateurDirection.DirectionId == directionFlgeId &&
                            u.UtilisateurDirection.ActiviteId == activiteId &&
                            u.UtilisateurDirection.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            u.RoleId == ((int)RoleUser.AdminLocal).ToString())
                .Select(s => s.UtilisateurDirection.Utilisateur).DistinctBy(s => s.Id);

            return results;
        }

        public IQueryable<UtilisateurDirection> GetUtilisateurDirectionByUtilisateurId(int utilisateurId)
        {
            var results = Context.UtilisateurDirections
                .Include(x => x.Activite)
                .Include(x => x.Direction)
                .Include(x => x.AspNetUserRoles)
                .ThenInclude(x => x.Role)
                .Where(d => d.UtilisateurId == utilisateurId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated);

            return results;
        }

        public IQueryable<UtilisateurDirection> GetUtilisateurDirections()
        {
            var results = Context
                .UtilisateurDirections
                .Include(x => x.Direction)
                .Include(x => x.Activite)
                .Include(x => x.Utilisateur)
                .Include(x => x.Utilisateur.AspNetUsers);

            return results;
        }


        public async Task<IEnumerable<Utilisateur>> GetUtilisateurByRoleInDirection(int activityId, int entiteId,
            string role,
            CancellationToken token = default)
        {
            var results = await Context.UtilisateurDirections
                .Where(u => u.DirectionId == entiteId &&
                            u.ActiviteId == activityId &&
                            u.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            u.AspNetUserRoles.Any(c => c.RoleId == role && c.UtilisateurDirectionId == u.Id))
                .Select(x => x.Utilisateur)
                .Distinct()
                .ToListAsync(token)
                .ConfigureAwait(false);

            return results;
        }


        public IQueryable<Utilisateur> GetUtilisateurHabilitesUser(IUserInfoService userInfoService)
        {
            return Context
                .UtilisateurDirections
                .Include(x => x.Direction)
                .Include(x => x.Activite)
                .Include(x => x.Utilisateur)
                .Include(x => x.Utilisateur.AspNetUsers)
                .Where(d => d.Utilisateur.AspNetUsersId == userInfoService.UserId)
                .Select(s => s.Utilisateur)
                .GroupBy(u => u.Id)
                .Select(u => u.First());
        }

        public async Task<IEnumerable<Utilisateur>> GetUtilisateurInDirectionSpecRoleAsync(int directionId,
            int activiteId,
            int roleId,
            CancellationToken token)
        {
            var role = roleId.ToString();

            var result = await Context
                .UtilisateurDirections
                .Include(x => x.Utilisateur)
                .Where(u => u.DirectionId == directionId &&
                            u.ActiviteId == activiteId &&
                            u.AspNetUserRoles.Any(c => c.RoleId == role && c.UtilisateurDirectionId == u.Id) &&
                            u.StatutUtilisateurId == (int)EStatutUtilisateur.Validated && u.Read && u.Write)
                .GroupBy(g => new
                {
                    g.Utilisateur.Id,
                    g.Utilisateur.Nom,
                    g.Utilisateur.Prenom,
                    g.Utilisateur.DirectionAttacheId
                })
                .Select(g => new Utilisateur
                {
                    Id = g.Key.Id,
                    Nom = g.Key.Nom.ToUpper(CultureInfo.CurrentCulture),
                    Prenom = g.Key.Prenom,
                    DirectionAttacheId = g.Key.DirectionAttacheId
                })
                .ToListAsync(token)
                .ConfigureAwait(false);
            return result;
        }


        public async Task<IEnumerable<Utilisateur>> GetUtilisateurInDirectionAsync(int directionId,
            int activiteId,
            CancellationToken token)
        {
            var result = await Context
                .UtilisateurDirections
                .Include(x => x.Utilisateur)
                .Where(u => u.DirectionId == directionId && u.ActiviteId == activiteId &&
                            u.StatutUtilisateurId == (int)EStatutUtilisateur.Validated)
                .GroupBy(g => new
                {
                    g.Utilisateur.Id,
                    g.Utilisateur.Nom,
                    g.Utilisateur.Prenom,
                    g.Utilisateur.DirectionAttacheId
                })
                .Select(g => new Utilisateur
                {
                    Id = g.Key.Id,
                    Nom = g.Key.Nom,
                    Prenom = g.Key.Prenom,
                    DirectionAttacheId = g.Key.DirectionAttacheId
                })
                .ToListAsync(token)
                .ConfigureAwait(false);
            return result;
        }

        public async Task<IEnumerable<Utilisateur>> GetUtilisateurInDirectionAllAsync(int directionId,
            int activiteId,
            CancellationToken token)
        {
            var result = await Context
                .UtilisateurDirections
                .Include(x => x.Utilisateur)
                .Where(u => u.DirectionId == directionId && u.ActiviteId == activiteId)
                .GroupBy(g => new
                {
                    g.Utilisateur.Id,
                    g.Utilisateur.Nom,
                    g.Utilisateur.Prenom,
                    g.Utilisateur.DirectionAttacheId
                })
                .Select(g => new Utilisateur
                {
                    Id = g.Key.Id,
                    Nom = g.Key.Nom,
                    Prenom = g.Key.Prenom,
                    DirectionAttacheId = g.Key.DirectionAttacheId
                })
                .ToListAsync(token)
                .ConfigureAwait(false);
            return result;
        }

        public async Task<IEnumerable<Utilisateur>> GetUtilisateurInDdcAsync(int activiteId, int directionId,
            CancellationToken token)
        {
            return await GetUtilisateurInDirectionAsync(directionId,
                    activiteId,
                    token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Utilisateur>> GetUtilisateurValideurInDdcAsync(bool isValidateurFlge,
            int directionId, int activiteId, CancellationToken token)
        {
            return await GetUtilisateurValideurInDirectionAsync(isValidateurFlge, directionId, activiteId, token)
                .ConfigureAwait(false);
        }

        public IQueryable<Utilisateur> GetUtilisateurs()
        {
            var result = Context.Utilisateurs.Include(x => x.AspNetUsers).Select(x => new Utilisateur
            {
                Id = x.Id,
                AspNetUsersId = x.AspNetUsersId,
                Nom = x.Nom,
                Prenom = x.Prenom,
                Email = x.Email,
                Telephone = x.Telephone,
                Initiales = x.Initiales,
                AccesBacarat = x.AccesBacarat,
                LangueId = x.LangueId,
                DirectionAttacheId = x.DirectionAttacheId,
                AspNetUsers = new AspNetUser
                {
                    UserName = x.AspNetUsers.UserName,
                    Email = x.AspNetUsers.Email
                }
            });
            return result;
        }

        public async Task<IEnumerable<Utilisateur>> GetUtilisateursAsync(CancellationToken token = default)
        {
            var result = await Context.Utilisateurs.Include(x => x.AspNetUsers).Select(x => new Utilisateur
            {
                Id = x.Id,
                AspNetUsersId = x.AspNetUsersId,
                Nom = x.Nom,
                Prenom = x.Prenom,
                Email = x.Email,
                Telephone = x.Telephone,
                Initiales = x.Initiales,
                AccesBacarat = x.AccesBacarat,
                LangueId = x.LangueId,
                DirectionAttacheId = x.DirectionAttacheId,
                AspNetUsers = new AspNetUser
                {
                    UserName = x.AspNetUsers.UserName,
                    Email = x.AspNetUsers.Email
                }
            }).ToListAsync(token).ConfigureAwait(false);

            return result;
        }
        public async Task<IEnumerable<Utilisateur>> GetUtilisateurWithoutSpecificUserAsync(int utilisateurId, int directionId, int activiteId, CancellationToken token)
        {
            List<Utilisateur> result;

            var isConfident = Context.UtilisateurDirections
            .Any(x => x.DirectionId == directionId &&
            x.UtilisateurId == utilisateurId &&
            x.ActiviteId == activiteId &&
            x.Confidentiality == true);

            var isPending = Context.UtilisateurDirections
            .Any(x => x.DirectionId == directionId &&
            x.UtilisateurId == utilisateurId &&
            x.ActiviteId == activiteId &&
            x.PendingAccess == true);

            var isIsole = Context.UtilisateurDirections
            .Any(x => x.DirectionId == directionId &&
            x.UtilisateurId == utilisateurId &&
            x.ActiviteId == activiteId &&
            x.Isolated == true);

            if (isConfident && !isPending)
                return await Context
                .UtilisateurDirections
                .Include(x => x.Utilisateur)
                .Where(u => u.DirectionId == directionId &&
                u.UtilisateurId != utilisateurId &&
                u.ActiviteId == activiteId &&
                u.Confidentiality == true &&
                u.PendingAccess != true &&
                u.StatutUtilisateurId == (int)EStatutUtilisateur.Validated)
                .GroupBy(g => new
                {
                    g.Utilisateur.Id,
                    g.Utilisateur.Nom,
                    g.Utilisateur.Prenom,
                    g.Utilisateur.DirectionAttacheId
                })
                .Select(g => new Utilisateur
                {
                    Id = g.Key.Id,
                    Nom = g.Key.Nom.ToUpper(CultureInfo.CurrentCulture),
                    Prenom = g.Key.Prenom,
                    DirectionAttacheId = g.Key.DirectionAttacheId
                })
                .ToListAsync(token)
                .ConfigureAwait(false);

            var canSeeIsolatedUsers = isIsole && (activiteId == 1 || activiteId == 2);

            if (!isPending)
                result = await Context
                .UtilisateurDirections
                .Include(x => x.Utilisateur)
                .Where(u => u.DirectionId == directionId &&
                u.UtilisateurId != utilisateurId &&
                u.ActiviteId == activiteId &&
                (canSeeIsolatedUsers || u.Isolated != true) &&
                u.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                u.PendingAccess != true &&
                u.AspNetUserRoles.Any(c => c.RoleId != Roles.AdminGlobal.Value))
                .GroupBy(g => new
                {
                    g.Utilisateur.Id,
                    g.Utilisateur.Nom,
                    g.Utilisateur.Prenom,
                    g.Utilisateur.DirectionAttacheId
                })
                .Select(g => new Utilisateur
                {
                    Id = g.Key.Id,
                    Nom = g.Key.Nom.ToUpper(CultureInfo.CurrentCulture),
                    Prenom = g.Key.Prenom,
                    DirectionAttacheId = g.Key.DirectionAttacheId
                })
                .ToListAsync(token)
                .ConfigureAwait(false);
            else
                result = await Context
                .UtilisateurDirections
                .Include(x => x.Utilisateur)
                .Where(u => u.DirectionId == directionId &&
                u.UtilisateurId != utilisateurId &&
                u.ActiviteId == activiteId &&
                (canSeeIsolatedUsers || u.Isolated != true) &&
                u.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                u.PendingAccess == true &&
                u.AspNetUserRoles.Any(c => c.RoleId != Roles.AdminGlobal.Value))
                .GroupBy(g => new
                {
                    g.Utilisateur.Id,
                    g.Utilisateur.Nom,
                    g.Utilisateur.Prenom,
                    g.Utilisateur.DirectionAttacheId
                })
                .Select(g => new Utilisateur
                {
                    Id = g.Key.Id,
                    Nom = g.Key.Nom.ToUpper(CultureInfo.CurrentCulture),
                    Prenom = g.Key.Prenom,
                    DirectionAttacheId = g.Key.DirectionAttacheId
                })
                .ToListAsync(token)
                .ConfigureAwait(false);

            return result;
        }

        public async Task<IEnumerable<Utilisateur>> GetUtilisateursConcernesAsync(int utilisateurId,
            int directionId,
            int activiteId,
            CancellationToken token)
        {
            var result = new List<Utilisateur>();
            var isIsole = await Context.UtilisateurDirections
                .AnyAsync(x => x.DirectionId == directionId &&
                               x.UtilisateurId == utilisateurId &&
                               x.ActiviteId == activiteId &&
                               x.Isolated == true, token)
                .ConfigureAwait(true);
            if (!isIsole)
                result = await Context
                    .UtilisateurDirections
                    .Include(x => x.Utilisateur)
                    .Where(u => u.DirectionId == directionId &&
                                u.ActiviteId == activiteId &&
                                u.PendingAccess != true)
                    .Select(g => new Utilisateur
                    {
                        Id = g.UtilisateurId,
                        Nom = g.Utilisateur.Nom.ToUpper(CultureInfo.CurrentCulture),
                        Prenom = g.Utilisateur.Prenom
                    })
                    .ToListAsync(token)
                    .ConfigureAwait(false);


            return result;
        }

        public IEnumerable<Utilisateur> GetUtilisateurAdminLocalAsync(int utilisateurId,
            int activiteId)
        {
            var directionIds = Context
                .UtilisateurDirections
                .Where(d => d.UtilisateurId == utilisateurId &&
                            d.Direction.IsActive &&
                            d.AspNetUserRoles
                                .Any(c => c.RoleId == "2") &&
                            d.ActiviteId == activiteId &&
                            d.Direction.IsActive)
                .Select(ud => ud.DirectionId);

            var result = Context
                .UtilisateurDirections
                .Include(x => x.Utilisateur)
                .Where(u => directionIds.Any(e => e == u.DirectionId) && u.ActiviteId == activiteId)
                .Select(g => new Utilisateur
                {
                    Id = g.UtilisateurId,
                    Nom = g.Utilisateur.Nom.ToUpper(CultureInfo.CurrentCulture),
                    Prenom = g.Utilisateur.Prenom
                })
                .ToList();

            return result;
        }

        public IEnumerable<Utilisateur> GetUtilisateurAdminLocalValidationAsync(int utilisateurId,
            int activiteId)
        {
            var directionIds = Context
                .UtilisateurDirections
                .Where(d => d.UtilisateurId == utilisateurId &&
                            d.Direction.IsActive &&
                            (d.AspNetUserRoles
                                .Any(c => c.RoleId == "2") || d.Validation) &&
                            d.ActiviteId == activiteId &&
                            d.Direction.IsActive)
                .Select(ud => ud.DirectionId);

            var result = Context
                .UtilisateurDirections
                .Include(x => x.Utilisateur)
                .Where(u => directionIds.Any(e => e == u.DirectionId) && u.ActiviteId == activiteId)
                .Select(g => new Utilisateur
                {
                    Id = g.UtilisateurId,
                    Nom = g.Utilisateur.Nom.ToUpper(CultureInfo.CurrentCulture),
                    Prenom = g.Utilisateur.Prenom
                })
                .ToList();

            return result;
        }


        public bool IsEntiteUsedDossierFraude(int entiteId)
        {
            return Context.DossierFraudes.Any(x => x.EntiteId == entiteId);
        }

        public bool IsEntiteUsedDossierLab(int entiteId)
        {
            return Context.DossierLabs.Any(x => x.EntiteId == entiteId);
        }

        public bool IsOrigineUsedDossierFraude(int origineId)
        {
            return Context.DossierFraudes.Any(x => x.OrigineId == origineId);
        }

        public bool IsOrigineUsedDossierLab(int origineId)
        {
            return Context.DossierLabs.Any(x => x.OrigineLabId == origineId);
        }

        public bool IsCategorieUsedCategorieLab(int categorieId)
        {
            return Context.DossierLabs.Any(x => x.CategorieId == categorieId);
        }

        public bool IsCategorieUsedGroupeCategorieLab(int categorieId)
        {
            return Context.DossierLabs.Any(x => x.CategorieId == categorieId);
        }

        public bool IsCategorieUsedGroupeOrigineLab(int categorieId)
        {
            return Context.DossierLabs.Any(x => x.CategorieId == categorieId);
        }

        public bool IsTypeClientFraudeUsedPersonnePhysiqueFraude(int typeClientId)
        {
            return Context.PersonnePhysiqueFraudes.Any(x => x.TypeClientId == typeClientId);
        }

        public bool IsTypeClientFraudeUsedPersonneMoraleFraude(int typeClientId)
        {
            return Context.PersonneMoraleFraudes.Any(x => x.TypeClientId == typeClientId);
        }
        public bool IsRegimeJuridiqueGdaUsedPersonnePhysiqueGda(int typeClientId)
        {
            return Context.PersonnePhysiqueGdas.Any(x => x.TypeClientId == typeClientId);
        }

        public bool IsRegimeJuridiqueGdaUsedPersonneMoraleGda(int typeClientId)
        {
            return Context.PersonneMoraleGdas.Any(x => x.TypeClientId == typeClientId);
        }

        public bool IsTypeClientLabUsedPersonnePhysiqueLab(int typeClientId)
        {
            return Context.PersonnePhysiqueLabs.Any(x => x.TypeClientId == typeClientId);
        }

        public bool IsTypeClientLabUsedPersonneMoraleLab(int typeClientId)
        {
            return Context.PersonneMoraleLabs.Any(x => x.TypeClientId == typeClientId);
        }

        public bool IsSecteurUsedEntite(int secteurId)
        {
            return Context.EntiteFraudes.Any(x => x.SecteurId == secteurId);
        }
        public bool IsSecteurGdaUsedEntite(int secteurId)
        {
            return Context.EntiteGdas.Any(x => x.SecteurId == secteurId);
        }

        public bool IsSecteurLabUsedEntiteLab(int secteurId)
        {
            return Context.EntiteLabs.Any(x => x.SecteurId == secteurId);
        }

        public IQueryable<UtilisateurDirection> GetUtilisateurDirectionConnexionHistos(int directionId, int activiteId)
        {
            return Context.UtilisateurDirections
                .Include(x => x.Utilisateur)
                .Include(x => x.AspNetUserRoles)
                .Include(x => x.Modificateur)
                .Include(x => x.Utilisateur.AspNetUsers)
                .Include("Utilisateur.HistoriqueConnexions")
                .Include("Utilisateur.Fonction")
                .Where(u => u.DirectionId == directionId && u.ActiviteId == activiteId);
        }

        public async Task<IList<UtilisateurViewModel>> GetUtilisateurByAdminConnexionHistos(
            IEnumerable<int> adminLocalDirectionIds, IEnumerable<int> adminLocalActiviteIds, bool isGlobalAdmin,
            int isAdmin, int active,
            bool? isValideur = null, bool? isAdminReferentiel = null, bool? isAuditeur = null)
        {
            var listUtilisateurs = new List<UtilisateurViewModel>();
            var localDirectionIds = adminLocalDirectionIds.ToList();
            if (localDirectionIds.Any())
            {
                if (isAdmin > 0)
                {
                    var adminUsersQuery = Context.AspNetUserRoles
                        .Include(x => x.UtilisateurDirection.Utilisateur.Modificateur)
                        .Include(x => x.UtilisateurDirection.Utilisateur.DirectionAttache)
                        .Include(x => x.UtilisateurDirection.Utilisateur.TypeContrat)
                        .Include(x => x.UtilisateurDirection.Utilisateur.AspNetUsers)
                        .Include(x => x.UtilisateurDirection.Utilisateur.HistoriqueConnexions)
                        .Include(x => x.UtilisateurDirection.Utilisateur.Fonction)
                        .Include(x => x.UtilisateurDirection.Modificateur)
                        .Where(x => x.UtilisateurDirection.StatutUtilisateurId == active
                                    && (localDirectionIds.Contains(x.UtilisateurDirection.DirectionId) ||
                                        (!localDirectionIds.Any() && isGlobalAdmin))
                                    && (adminLocalActiviteIds.Contains(x.UtilisateurDirection.ActiviteId) ||
                                        (!adminLocalActiviteIds.Any() && isGlobalAdmin))
                                    && (x.RoleId == isAdmin.ToString() || isAdmin == 0)
                                    && ((x.RoleId != ((int)RoleUser.AdminGlobal).ToString() &&
                                         x.RoleId != ((int)RoleUser.Superviseur).ToString() &&
                                         x.RoleId != ((int)RoleUser.AuditGlobal).ToString()) || isGlobalAdmin));

                    adminUsersQuery = adminUsersQuery
                        .Where(r => (isAuditeur == isValideur && isValideur == isAdminReferentiel) ||
                                    (isValideur == true && r.UtilisateurDirection.Validation) ||
                                    (isAdminReferentiel == true && r.UtilisateurDirection.AdminReferentiel) ||
                                    (isAuditeur == true && r.UtilisateurDirection.AspNetUserRoles.Any(roles => roles.RoleId == ((int)RoleUser.AuditGlobal).ToString())));

                    listUtilisateurs = await adminUsersQuery.Select(u => new UtilisateurViewModel
                    {
                        Id = u.UtilisateurDirection.Utilisateur.Id,
                        Nom = u.UtilisateurDirection.Utilisateur.Nom,
                        Prenom = u.UtilisateurDirection.Utilisateur.Prenom,
                        Email = u.UtilisateurDirection.Utilisateur.Email,
                        DirectionAttache = new DirectionViewModel
                        {
                            Id = u.UtilisateurDirection.Utilisateur.DirectionAttache.Id,
                            Nom = u.UtilisateurDirection.Utilisateur.DirectionAttache.Nom
                        },
                        DirectionAttacheId = u.UtilisateurDirection.Utilisateur.DirectionAttacheId,
                        DateCreation = u.UtilisateurDirection.Utilisateur.DateCreation,
                        DateModification = u.UtilisateurDirection.Utilisateur.DateModification,
                        Initiales = u.UtilisateurDirection.Utilisateur.Initiales,
                        Service = u.UtilisateurDirection.Utilisateur.Service,
                        IdentifiantErmes = u.UtilisateurDirection.IdentifiantErmes,
                        Teledeclarant = u.UtilisateurDirection.Teledeclarant,
                        HabilitationGroupe = u.UtilisateurDirection.Utilisateur.HabilitationGroupe,
                        TypeContrat = u.UtilisateurDirection.Utilisateur.TypeContrat != null
                            ? new SelectedItem
                            {
                                Id = u.UtilisateurDirection.Utilisateur.TypeContrat.Id,
                                NameEn = u.UtilisateurDirection.Utilisateur.TypeContrat.EnglishName,
                                NameFr = u.UtilisateurDirection.Utilisateur.TypeContrat.FrenchName
                            }
                            : null,
                        Fonction = u.UtilisateurDirection.Utilisateur.Fonction != null
                            ? new FonctionViewModel
                            {
                                Id = u.UtilisateurDirection.Utilisateur.Fonction.Id,
                                NameEn = u.UtilisateurDirection.Utilisateur.Fonction.EnglishName,
                                NameFr = u.UtilisateurDirection.Utilisateur.Fonction.FrenchName
                            }
                            : null,
                        AspNetUsers = new AspNetUserViewModel
                        {
                            UserName = u.UtilisateurDirection.Utilisateur.AspNetUsers.UserName,
                            Email = u.UtilisateurDirection.Utilisateur.AspNetUsers.Email
                        },
                        HistoriqueConnexions = u.UtilisateurDirection.Utilisateur.HistoriqueConnexions.Select(p =>
                            new HistoriqueConnexionViewModel
                            { UtilisateurId = p.UtilisateurId, DateConnexion = p.DateConnexion }).ToList(),
                        ModificateurFullName = u.UtilisateurDirection.Modificateur != null
                            ? u.UtilisateurDirection.Utilisateur.Modificateur.Prenom + " " +
                              u.UtilisateurDirection.Utilisateur.Modificateur.Nom.ToUpper()
                            : null
                    }).ToListAsync().ConfigureAwait(false);
                }
                else
                {
                    var allUsersQuery = Context.UtilisateurDirections
                        .Include(x => x.Utilisateur.DirectionAttache)
                        .Include(x => x.Utilisateur.TypeContrat)
                        .Include(x => x.Utilisateur.AspNetUsers)
                        .Include(x => x.Utilisateur.HistoriqueConnexions)
                        .Include(x => x.Utilisateur.Fonction)
                        .Include(x => x.Utilisateur.Modificateur)
                        .Where(x => x.StatutUtilisateurId == active
                                    && (localDirectionIds.Contains(x.DirectionId) ||
                                        (!localDirectionIds.Any() && isGlobalAdmin))
                                    && (adminLocalActiviteIds.Contains(x.ActiviteId) ||
                                        (!adminLocalActiviteIds.Any() && isGlobalAdmin)));

                    allUsersQuery = allUsersQuery
                        .Where(r => (isAuditeur == isValideur && isValideur == isAdminReferentiel) ||
                                    (isValideur == true && r.Validation) ||
                                    (isAdminReferentiel == true && r.AdminReferentiel) ||
                                    (isAuditeur == true && r.AspNetUserRoles.Any(roles => roles.RoleId == ((int)RoleUser.AuditGlobal).ToString())));

                    listUtilisateurs = await allUsersQuery.Select(u => new UtilisateurViewModel
                    {
                        Id = u.Utilisateur.Id,
                        Nom = u.Utilisateur.Nom,
                        Prenom = u.Utilisateur.Prenom,
                        Email = u.Utilisateur.Email,
                        DirectionAttache = new DirectionViewModel
                        { Id = u.Utilisateur.DirectionAttache.Id, Nom = u.Utilisateur.DirectionAttache.Nom },
                        DirectionAttacheId = u.Utilisateur.DirectionAttacheId,
                        DateCreation = u.Utilisateur.DateCreation,
                        DateModification = u.Utilisateur.DateModification,
                        Initiales = u.Utilisateur.Initiales,
                        Service = u.Utilisateur.Service,
                        IdentifiantErmes = u.IdentifiantErmes,
                        Teledeclarant = u.Teledeclarant,
                        HabilitationGroupe = u.Utilisateur.HabilitationGroupe,
                        TypeContrat = u.Utilisateur.TypeContrat != null
                            ? new SelectedItem
                            {
                                Id = u.Utilisateur.TypeContrat.Id,
                                NameEn = u.Utilisateur.TypeContrat.EnglishName,
                                NameFr = u.Utilisateur.TypeContrat.FrenchName
                            }
                            : null,
                        Fonction = u.Utilisateur.Fonction != null
                            ? new FonctionViewModel
                            {
                                Id = u.Utilisateur.Fonction.Id,
                                NameEn = u.Utilisateur.Fonction.EnglishName,
                                NameFr = u.Utilisateur.Fonction.FrenchName
                            }
                            : null,

                        AspNetUsers = new AspNetUserViewModel
                        {
                            UserName = u.Utilisateur.AspNetUsers.UserName,
                            Email = u.Utilisateur.AspNetUsers.Email
                        },
                        HistoriqueConnexions = u.Utilisateur.HistoriqueConnexions.Select(p =>
                            new HistoriqueConnexionViewModel
                            { UtilisateurId = p.UtilisateurId, DateConnexion = p.DateConnexion }).ToList(),
                        ModificateurFullName = u.Utilisateur.Modificateur != null
                            ? u.Utilisateur.Modificateur.Prenom + " " + u.Utilisateur.Modificateur.Nom.ToUpper()
                            : null
                    }).ToListAsync().ConfigureAwait(false);
                }
            }

            return listUtilisateurs.DistinctBy(x => x.Id).ToList();
        }

        public async Task<IList<UtilisateurDirection>> GetRoleUtilisateurDirectionByAdmin(int utilisateurId,
            int isAdmin, int active, int? activite = null, int? directionId = null,
            CancellationToken token = default)
        {
            var result = await Context.UtilisateurDirections
                .Include(x => x.Modificateur)
                .Include(x => x.AspNetUserRoles)
                .ThenInclude(x => x.Role)
                .Include(x => x.Activite)
                .Include(x => x.StatutUtilisateur)
                .Include(x => x.Direction)
                .Where(x => x.UtilisateurId == utilisateurId
                            && x.Direction.IsActive
                            && (x.DirectionId == directionId || directionId == null)
                            && (x.ActiviteId == activite || activite == null))
                .ToListAsync(token).ConfigureAwait(false);

            return result;
        }

        public async Task<IList<AspNetRole>> GetAspNetUserRoleByAdminConnexionHistos(int utilisateurId,
            CancellationToken token = default)
        {
            return await Context.AspNetUserRoles.Include(x => x.Role)
                .Where(x => x.UtilisateurDirectionId == utilisateurId).Select(x => x.Role).ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Utilisateur>> GetUtilisateurDelegueByActivite(int activiteId,
            int utilidsateurId, CancellationToken token = default)
        {
            var directionIds = Context.UtilisateurDirections
                .Where(d => d.Utilisateur.Id == utilidsateurId &&
                            d.ActiviteId == activiteId &&
                            d.Isolated != true)
                .Select(ud => ud.Direction.Id);

            var result = await Context.Delegations
                .Include(x => x.Utilisateur)
                .Where(x => x.ActiviteId == activiteId &&
                            x.DelegueId == utilidsateurId &&
                            directionIds.Any(d => d == x.DirectionId))
                .Select(g => new Utilisateur
                {
                    Id = g.UtilisateurId,
                    Nom = g.Utilisateur.Nom.ToUpper(CultureInfo.CurrentCulture),
                    Prenom = g.Utilisateur.Prenom
                }).ToListAsync(token).ConfigureAwait(false);

            return result;
        }


        public async Task<IEnumerable<Utilisateur>> GetUtilisateursAssociated(int activiteId, int utilidsateurId,
            CancellationToken token = default)
        {
            var result = new List<Utilisateur>();


            var directionUserIsolatedIds = Context.UtilisateurDirections
                .Where(d => d.Utilisateur.Id == utilidsateurId
                            && d.ActiviteId == activiteId
                            && d.Isolated == true)
                .Select(ud => ud.DirectionId);

            if (directionUserIsolatedIds.Any())
            {
                var usertIsolated = await Context.UtilisateurDirections.Where(x =>
                        x.ActiviteId == activiteId && directionUserIsolatedIds.Any(d => d == x.DirectionId))
                    .Select(g => new Utilisateur
                    {
                        Id = g.UtilisateurId,
                        Nom = g.Utilisateur.Nom.ToUpper(CultureInfo.CurrentCulture),
                        Prenom = g.Utilisateur.Prenom
                    }).ToListAsync(token);


                if (usertIsolated != null && usertIsolated.Any())
                    result.AddRange(usertIsolated);
            }

            var directionNotIsolatedIds = Context.UtilisateurDirections.Where(d => d.Utilisateur.Id == utilidsateurId
                    && d.ActiviteId == activiteId
                    && d.Isolated != true)
                .Select(ud => ud.DirectionId);

            if (directionNotIsolatedIds.Any())
            {
                var userNotIsolated = await Context.UtilisateurDirections.Where(x =>
                        x.ActiviteId == activiteId && directionNotIsolatedIds.Any(d => d == x.DirectionId))
                    .Select(g => new Utilisateur
                    {
                        Id = g.UtilisateurId,
                        Nom = g.Utilisateur.Nom.ToUpper(CultureInfo.CurrentCulture),
                        Prenom = g.Utilisateur.Prenom
                    }).ToListAsync(token);


                if (userNotIsolated != null && userNotIsolated.Any())
                    result.AddRange(userNotIsolated);
            }

            return result.DistinctBy(x => x.Id);
        }

        public IDirectionService Direction { get; set; }
        public IActiviteService Activite { get; set; }

        public async Task<bool> IsReadonlyUtilisateurAsync(int activiteId, int utilisateurId)
        {
            return !await Context
                .UtilisateurDirections
                .AnyAsync(x => x.UtilisateurId == utilisateurId &&
                               x.ActiviteId == activiteId &&
                               x.Write)
                .ConfigureAwait(false);
        }


        public async Task<bool> IsReadOnlyUserAsync(int activiteId, int utilisateurId,
            CancellationToken token = default)
        {
            var directs = await Context
                .UtilisateurDirections
                .Where(x => x.UtilisateurId == utilisateurId &&
                            x.ActiviteId == activiteId
                ).ToListAsync(token)
                .ConfigureAwait(false);

            foreach (var dir in directs)
                if (dir.Read)
                    return true;

            return false;
        }

        public async Task<bool> IsIsoleOnlyUserAsync(int activiteId, int utilisateurId,
            CancellationToken token = default)
        {
            var result = false;

            var directs = await Context
                .UtilisateurDirections
                .Where(x => x.UtilisateurId == utilisateurId &&
                            x.ActiviteId == activiteId
                ).ToListAsync(token)
                .ConfigureAwait(false);

            foreach (var dir in directs)
                if (dir.Isolated)
                    result = true;
                else
                    return false;

            return result;
        }

        public async Task<bool> IsWriteOnlyUserAsync(int activiteId, int utilisateurId,
            CancellationToken token = default)
        {
            var directs = await Context
                .UtilisateurDirections
                .Where(x => x.UtilisateurId == utilisateurId &&
                            x.ActiviteId == activiteId
                ).ToListAsync(token)
                .ConfigureAwait(false);

            foreach (var dir in directs)
                if (dir.Write)
                    return true;

            return false;
        }


        public async Task<bool> IsEtenduUtilisateurAsync(int activiteId, int utilisateurId)
        {
            return await Context
                .UtilisateurDirections
                .AnyAsync(x => x.UtilisateurId == utilisateurId &&
                               x.ActiviteId == activiteId &&
                               x.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                               x.Etendu == true)
                .ConfigureAwait(false);
        }

        public bool IsEtenduUtilisateur(int activiteId, int utilisateurId)
        {
            return Context
                .UtilisateurDirections
                .Any(x => x.UtilisateurId == utilisateurId &&
                          x.ActiviteId == activiteId &&
                          x.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                          x.Etendu == true);
        }

        public async Task<IEnumerable<TypeClientFraude>> GetTypeClientFraudesByDirection(int directionId,
            CancellationToken token = default)
        {
            var result = await Context.TypeClientFraudes.Where(x => x.DirectionId == directionId).ToListAsync(token)
                .ConfigureAwait(false);
            return result;
        }
        public async Task<IEnumerable<RegimeJuridiqueGda>> GetRegimeJuridiqueGdasByDirection(int directionId,
            CancellationToken token = default)
        {
            var result = await Context.RegimeJuridiqueGdas.ToListAsync(token)
                .ConfigureAwait(false);
            return result;
        }

        public async Task<IEnumerable<TypeClientLab>> GetTypeClientLabsByDirection(int directionId,
            CancellationToken token = default)
        {
            var result = await Context.TypeClientLabs.Where(x => x.DirectionId == directionId).ToListAsync(token)
                .ConfigureAwait(false);
            return result;
        }

        public async Task<IEnumerable<Direction>> GetDirectionByParentId(int parentId,
            CancellationToken token = default)
        {
            var result = await Context.Directions
                .Where(x => x.ParentId == parentId && x.IsActive)
                .ToListAsync(token)
                .ConfigureAwait(false);

            return result;
        }


        public async Task<IEnumerable<Direction>> GetDirectionFlgeOnEntiteLocals(int activiteId, int utilisateur,
            CancellationToken token)
        {
            var result = await Context
                .UtilisateurDirections
                .Where(d => d.UtilisateurId == utilisateur &&
                            d.ActiviteId == activiteId &&
                            d.Direction.IsActive &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.AspNetUserRoles.Any(c => c.RoleId == "6" || c.RoleId == "7"))
                .Select(ud => ud.Direction.Parent).ToListAsync(token).ConfigureAwait(false);
            return result;
        }

        public async Task<IEnumerable<Direction>> GetDirectionFlgeOnSousEntiteLocals(int activiteId, int utilisateur,
            CancellationToken token)
        {
            var entiteLocalIds = Context
                .UtilisateurDirections
                .Where(d => d.UtilisateurId == utilisateur &&
                            d.ActiviteId == activiteId &&
                            d.Direction.IsActive &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.AspNetUserRoles.Any(c => c.RoleId == "8" || c.RoleId == "9"))
                .Select(ud => ud.Direction.ParentId);

            var result = await Context.Directions
                .Where(x => entiteLocalIds.Any(c => c == x.Id) && x.IsActive).Select(x => x.Parent).ToListAsync(token)
                .ConfigureAwait(false);

            return result;
        }

        public async Task<IEnumerable<Direction>> GetDirectionFlgesByUser(int activiteId, int utilisateurId,
            CancellationToken token)
        {
            var result = await Context
                .UtilisateurDirections
                .Where(d => d.UtilisateurId == utilisateurId &&
                            d.ActiviteId == activiteId &&
                            d.Direction.IsActive &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.AspNetUserRoles.Any(c => c.RoleId == "4" || c.RoleId == "5")
                            && d.Direction.IsFlge)
                .Select(ud => ud.Direction).ToListAsync(token).ConfigureAwait(false);

            return result;
        }

        public IQueryable<Direction> GetFlgeDepartments()
        {
            return Context
                .Directions
                .Where(d => d.IsActive && d.IsFlge);
        }

        public async Task<IEnumerable<Direction>> GetDirectionFlges(
            CancellationToken token)
        {
            var result = await Context
                .Directions
                .Where(d => d.IsActive
                            && d.IsFlge).ToListAsync(token).ConfigureAwait(false);

            return result;
        }

        public async Task<IEnumerable<Direction>> GetDirectionNotFlges(
            CancellationToken token)
        {
            var result = await Context
                .Directions
                .Where(d => d.IsActive
                            && !d.IsFlge).ToListAsync(token).ConfigureAwait(false);

            return result;
        }


        public async Task<IEnumerable<Direction>> GetDirectionEntiteLocals(int activiteId, int utilisateur,
            CancellationToken token)
        {
            var result = await Context
                .UtilisateurDirections
                .Where(d => d.UtilisateurId == utilisateur &&
                            d.ActiviteId == activiteId &&
                            d.Direction.IsActive &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.AspNetUserRoles.Any(c => c.RoleId == "6" || c.RoleId == "7"))
                .Select(ud => ud.Direction).ToListAsync(token).ConfigureAwait(false);
            return result;
        }

        public async Task<IEnumerable<Direction>> GetDirectionSousEntiteLocals(int activiteId, int utilisateur,
            CancellationToken token)
        {
            var result = await Context
                .UtilisateurDirections
                .Where(d => d.UtilisateurId == utilisateur &&
                            d.ActiviteId == activiteId &&
                            d.Direction.IsActive &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.AspNetUserRoles.Any(c => c.RoleId == "8" || c.RoleId == "9"))
                .Select(ud => ud.Direction).ToListAsync(token).ConfigureAwait(false);

            return result;
        }


        public async Task<IEnumerable<Direction>> GetAvailableEntiteLocalByDirectionFlge(int activiteId,
            int utilisateur, int directionFlgeId, CancellationToken token)
        {
            var result = await Context
                .UtilisateurDirections
                .Where(d => d.UtilisateurId == utilisateur &&
                            d.ActiviteId == activiteId &&
                            d.Direction.IsActive &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.Direction.ParentId == directionFlgeId &&
                            d.AspNetUserRoles.Any(c => c.RoleId == "8" || c.RoleId == "9"))
                .Select(ud => ud.Direction).ToListAsync(token).ConfigureAwait(false);

            return result;
        }

        public async Task<IEnumerable<Direction>> GetAvailableSousEntiteLocalByEntiteLocal(int activiteId,
            int utilisateur, int entiteLocalId, CancellationToken token)
        {
            var result = await Context
                .UtilisateurDirections
                .Where(d => d.UtilisateurId == utilisateur &&
                            d.ActiviteId == activiteId &&
                            d.Direction.IsActive &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.Direction.ParentId == entiteLocalId &&
                            d.AspNetUserRoles.Any(c => c.RoleId == "8" || c.RoleId == "9"))
                .Select(ud => ud.Direction).ToListAsync(token).ConfigureAwait(false);

            return result;
        }

        public async Task<IEnumerable<Utilisateur>> GetUtilisateurResponsableFlges(int activiteId, int directionId,
            CancellationToken token)
        {
            var result = await Context
                .UtilisateurDirections
                .Where(d => d.DirectionId == directionId &&
                            d.ActiviteId == activiteId &&
                            d.Direction.IsActive &&
                            d.AspNetUserRoles.Any(c => c.RoleId == "4")
                )
                .Select(ud => ud.Utilisateur).ToListAsync(token).ConfigureAwait(false);

            return result;
        }

        public async Task<IEnumerable<Direction>> GetDirectionResponsableFlges(int activiteId, int utilisateur,
            CancellationToken token)
        {
            var result = await Context
                .UtilisateurDirections
                .Where(d => d.UtilisateurId == utilisateur &&
                            d.ActiviteId == activiteId &&
                            d.Direction.IsActive &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.AspNetUserRoles.Any(c => c.RoleId == "4"))
                .Select(ud => ud.Direction).ToListAsync(token).ConfigureAwait(false);

            return result;
        }

        public async Task<IEnumerable<Direction>> GetDirectionCollaborateurFlges(int activiteId, int utilisateur,
            CancellationToken token)
        {
            var result = await Context
                .UtilisateurDirections
                .Where(d => d.UtilisateurId == utilisateur &&
                            d.ActiviteId == activiteId &&
                            d.Direction.IsActive &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                            d.AspNetUserRoles.Any(c => c.RoleId == "5"))
                .Select(ud => ud.Direction).ToListAsync(token).ConfigureAwait(false);

            return result;
        }

        public async Task<IEnumerable<Traduction>> GetCommonTranslations(CancellationToken token)
        {
            return await Context.Translations.ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TraductionTemplate>> GetCommonTranslationTemplates(CancellationToken token)
        {
            return await Context.TraductionTemplates.ToListAsync(token)
                .ConfigureAwait(false);
        }


        /// <summary>
        ///     Ajout traduction.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="frenchTranslate"></param>
        /// <param name="englisTranslate"></param>
        /// <param name="token"></param>
        /// <returns>Ajout d'une nouvelle ligne  traduction.</returns>
        public async Task<bool> AddTraduction(string id, string frenchTranslate, string englisTranslate,
            CancellationToken token = default)
        {
            try
            {
                if (Context.Translations.SingleOrDefault(x => x.Id == id)?.Id == id ||
                    Context.Translations.SingleOrDefault(x => x.FrenchTranslate == frenchTranslate)?.FrenchTranslate ==
                    frenchTranslate)
                    return false;

                var result = await Context.Translations.AddAsync(
                    new Traduction
                    {
                        EnglishTranslate = englisTranslate,
                        Id = id,
                        FrenchTranslate = frenchTranslate
                    }, token).ConfigureAwait(false);
                await Context.SaveChangesAsync(token);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
            }

            return false;
        }

        /// <summary>
        ///     Ajout/edit traduction template.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="frenchTranslate"></param>
        /// <param name="englishTranslate"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> AddEditTraductionTemplate(string id, string frenchTranslate, string englishTranslate,
            CancellationToken token = default)
        {
            try
            {
                var traduction = await Context.TraductionTemplates.SingleOrDefaultAsync(x => x.Id == id, token)
                    .ConfigureAwait(false);
                englishTranslate = TrimString(englishTranslate, false);
                frenchTranslate = TrimString(frenchTranslate, false);
                if (traduction == null && !string.IsNullOrEmpty(id))
                {
                    var result = await Context.TraductionTemplates.AddAsync(
                        new TraductionTemplate
                        {
                            EnglishTranslate = englishTranslate,
                            Id = id,
                            FrenchTranslate = frenchTranslate
                        }, token).ConfigureAwait(false);
                    await Context.SaveChangesAsync(token);
                    return true;
                }

                if (traduction != null && !string.IsNullOrEmpty(id))
                {
                    traduction.FrenchTranslate = frenchTranslate;
                    traduction.EnglishTranslate = englishTranslate;
                    Context.TraductionTemplates.Update(traduction);
                    await Context.SaveChangesAsync(token);
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
            }

            return false;
        }

        /// <summary>
        ///     Edit (objet/body) mail notification template.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="objet"></param>
        /// <param name="body"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> EditMailNotificationTraduction(int id, string objet, string body,
            CancellationToken token = default)
        {
            try
            {
                var emailNotification = await Context.EmailNotificationTemplates
                    .SingleOrDefaultAsync(x => x.Id == id, token).ConfigureAwait(false);
                if (emailNotification != null)
                {
                    body = TrimString(body, true);
                    emailNotification.Body = body;
                    emailNotification.Subject = objet;
                    Context.EmailNotificationTemplates.Update(emailNotification);
                    await Context.SaveChangesAsync(token);
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
            }

            return false;
        }

        /// <summary>
        ///     Une fonction qui va permettre la generation du script d'insertion dans commun.traduction.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<byte[]> GenerationScript(CancellationToken token = default)
        {
            var retour = new List<string>();
            byte[] scriptAsBytes = null;
            try
            {
                var traductionList = await Context.Translations.ToListAsync(token)
                    .ConfigureAwait(false);

                if (traductionList != null && traductionList.Any())
                {
                    retour.Add("USE BacaratWeb");
                    retour.Add("GO");
                    retour.Add("DELETE FROM Commun.Traduction");
                    retour.Add("GO");
                    foreach (var item in traductionList)
                    {
                        var newFrenchTransalte = GenerateTraduction(item.FrenchTranslate);
                        var newEnglishTranslate = GenerateTraduction(item.EnglishTranslate);
                        var resultFrench = string.IsNullOrEmpty(newFrenchTransalte)
                            ? item.FrenchTranslate
                            : newFrenchTransalte;
                        var resultEnglish = string.IsNullOrEmpty(newEnglishTranslate)
                            ? item.EnglishTranslate
                            : newEnglishTranslate;
                        retour.Add(
                            $"INSERT INTO [Commun].[Traduction] ([Id] , [FrenchTranslate], [EnglishTranslate]) VALUES('{item.Id}', '{resultFrench}', '{resultEnglish}')");
                        retour.Add("GO");
                    }

                    scriptAsBytes = retour.SelectMany(s => Encoding.UTF8.GetBytes(s + Environment.NewLine)).ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
            }

            return scriptAsBytes;
        }


        /// <summary>
        ///     Une fonction qui retourne un flu byte pour la generation du script d'insertion dans commun.TraductionTemplate.
        /// </summary>
        /// <param name="token"></param>
        public async Task<byte[]> GenerationScriptTemplate(CancellationToken token = default)
        {
            var retour = new List<string>();
            byte[] scriptAsBytes = null;
            try
            {
                var traductionList = await Context.TraductionTemplates.ToListAsync(token)
                    .ConfigureAwait(false);

                if (traductionList != null && traductionList.Any())
                {
                    retour.Add("USE BacaratWeb");
                    retour.Add("GO");
                    retour.Add("DELETE FROM Commun.TraductionTemplate");
                    retour.Add("GO");
                    foreach (var item in traductionList)
                    {
                        var newFrenchTransalte = GenerateTraduction(item.FrenchTranslate);
                        var newEnglishTranslate = GenerateTraduction(item.EnglishTranslate);
                        var resultFrench = string.IsNullOrEmpty(newFrenchTransalte)
                            ? item.FrenchTranslate
                            : newFrenchTransalte;
                        var resultEnglish = string.IsNullOrEmpty(newEnglishTranslate)
                            ? item.EnglishTranslate
                            : newEnglishTranslate;
                        retour.Add(
                            $"INSERT INTO [Commun].[TraductionTemplate] ([Id] , [FrenchTranslate], [EnglishTranslate]) VALUES('{item.Id}', '{resultFrench}', '{resultEnglish}')");
                        retour.Add("GO");
                    }

                    scriptAsBytes = retour.SelectMany(s => Encoding.UTF8.GetBytes(s + Environment.NewLine)).ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
            }

            return scriptAsBytes;
        }

        /// <summary>
        ///     Edit traduction.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="frenchTranslate"></param>
        /// <param name="englisTranslate"></param>
        /// <param name="token"></param>
        public async Task<bool> EditTraduction(string id, string frenchTranslate, string englisTranslate,
            CancellationToken token = default)
        {
            var traduction = await Context.Translations.SingleOrDefaultAsync(x => x.Id == id, token)
                .ConfigureAwait(false);
            try
            {
                if (traduction != null)
                {
                    traduction.FrenchTranslate = frenchTranslate;
                    traduction.EnglishTranslate = englisTranslate;
                    Context.Translations.Update(traduction);
                    await Context.SaveChangesAsync(token);
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
            }

            return false;
        }

        /// <summary>
        ///     Retourne template traduction par Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TraductionTemplate> GetTemplateTraductionById(string id, CancellationToken token = default)
        {
            return await Context.TraductionTemplates.SingleOrDefaultAsync(x => x.Id == id, token).ConfigureAwait(false);
        }

        /// <summary>
        ///     Retourne email notification par Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<EmailNotificationTraductionTemplateViewModel> GetMailNotificationById(int id,
            CancellationToken token = default)
        {
            var result = new EmailNotificationTraductionTemplateViewModel();
            var emailNotificationTemplate = await Context.EmailNotificationTemplates
                .Include(x => x.Activite)
                .Include(x => x.EmailNotificationType)
                .Include(x => x.Langue)
                .SingleOrDefaultAsync(x => x.Id == id, token).ConfigureAwait(false);
            try
            {
                if (emailNotificationTemplate != null)
                {
                    result.ActiviteCode = emailNotificationTemplate.Activite?.Code;
                    result.Body = emailNotificationTemplate.Body;
                    result.EmailNotificationTypeCode = emailNotificationTemplate.EmailNotificationType.Code;
                    result.Id = emailNotificationTemplate.Id;
                    result.LangueCode = emailNotificationTemplate.Langue.Code;
                    result.Subject = emailNotificationTemplate.Subject;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
            }

            return result;
        }

        public async Task<List<EmailNotificationScope>> GetEmailNotificationScopes(CancellationToken token = default)
        {
            return await Context
                .EmailNotificationScopes
                .ToListAsync(token);
        }

        /// <summary>
        ///     Delete traduction.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        public async Task<bool> DeleteTraduction(string id, CancellationToken token = default)
        {
            var result = false;
            try
            {
                var traduction = await Context.Translations.SingleOrDefaultAsync(x => x.Id == id, token)
                    .ConfigureAwait(false);
                if (traduction != null)
                {
                    Context.Translations.Remove(traduction);
                    await Context.SaveChangesAsync(token).ConfigureAwait(false);
                    result = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
            }

            return result;
        }

        /// <summary>
        ///     Delete traduction template.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        public async Task<bool> DeleteTraductionTemplate(string id, CancellationToken token = default)
        {
            var result = false;
            try
            {
                var traduction = await Context.TraductionTemplates.SingleOrDefaultAsync(x => x.Id == id, token)
                    .ConfigureAwait(false);
                if (traduction != null)
                {
                    Context.TraductionTemplates.Remove(traduction);
                    await Context.SaveChangesAsync(token).ConfigureAwait(false);
                    result = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
            }

            return result;
        }

        public async Task<IEnumerable<CategorieLab>> GetCategorieLabByDirection(int directionId,
            CancellationToken token = default)
        {
            var result = await Context
                .CategorieLabs
                .Where(d => d.DirectionId == directionId)
                .ToListAsync(token)
                .ConfigureAwait(false);

            return result;
        }

        public async Task<IEnumerable<OrigineLab>> GetOrigineLabByDirection(int directionId,
            CancellationToken token = default)
        {
            var result = await Context
                .OrigineLabs
                .Where(d => d.DirectionId == directionId)
                .ToListAsync(token)
                .ConfigureAwait(false);
            return result;
        }


        public async Task<bool> IsAccesDossierConfidentialLab(int responsableDossierId, int directionId, int activiteId,
            CancellationToken token = default)
        {
            var result = await Context.UtilisateurDirections.AnyAsync(d => d.UtilisateurId == responsableDossierId &&
                                                                           d.ActiviteId == activiteId &&
                                                                           d.Direction.IsActive &&
                                                                           d.Confidentiality == true &&
                                                                           d.DirectionId == directionId, token)
                .ConfigureAwait(false);

            return result;
        }


        public async Task<Confidentiel> GetConfidentialMessageLab(int directionId, CancellationToken token = default)
        {
            var result = await Context.Confidentiels.FirstOrDefaultAsync(d => d.DirectionId == directionId, token)
                .ConfigureAwait(false);

            return result;
        }

        public async Task<IEnumerable<OrigineFraude>> GetOrigineFraudesByDirection(int directionId,
            CancellationToken token = default)
        {
            var result = await Context.OrigineDirectionFraudes.Where(x => x.DirectionId == directionId && x.IsActive)
                .Select(ud => ud.Origine).ToListAsync(token).ConfigureAwait(false);
            return result;
        }

        public async Task<IEnumerable<OrigineGda>> GetOrigineGdasByDirection(int directionId,
           CancellationToken token = default)
        {
            var result = await Context.OrigineDirectionGdas.Where(x => x.DirectionId == directionId && x.IsActive)
                .Select(ud => ud.Origine).ToListAsync(token).ConfigureAwait(false);
            return result;
        }


        public async Task<bool> IsOperationLab(int activiteId, int utilisateurId, CancellationToken token = default)
        {
            var directionIds = Context.UtilisateurDirections
                .Where(d => d.Utilisateur.Id == utilisateurId && d.ActiviteId == activiteId)
                .Select(ud => ud.DirectionId).ToList();

            var result = await Context.OrigineLabs.AnyAsync(
                x => x.DirectionId.HasValue && directionIds.Contains(x.DirectionId.Value) &&
                     (x.FonctionLabId == 1 || x.FonctionLabId == 3), token);

            return result;
        }

        public async Task<IEnumerable<UtilisateurDirection>> GetUtilisateurDirections(int utilisateurId,
            int directionId, int activiteId, CancellationToken tocken = default)
        {
            var results = await Context.UtilisateurDirections
                .Include(x => x.Activite)
                .Include(x => x.AspNetUserRoles)
                .ThenInclude(x => x.Role)
                .Where(d => d.UtilisateurId == utilisateurId && d.DirectionId == directionId &&
                            d.ActiviteId == activiteId && d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated)
                .ToListAsync(tocken).ConfigureAwait(false);

            return results;
        }

        public async Task<IEnumerable<UtilisateurDirection>> GetUtilisateurValideurs(int directionId, int activiteId,
            CancellationToken tocken = default)
        {
            var results = await Context.UtilisateurDirections.Include(x => x.Utilisateur)
                .Where(d => d.Validation == true && d.DirectionId == directionId && d.ActiviteId == activiteId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated).Select(x =>
                    new UtilisateurDirection
                    {
                        Id = x.Id,
                        UtilisateurId = x.UtilisateurId,
                        Utilisateur = new Utilisateur
                        {
                            Id = x.UtilisateurId,
                            Email = x.Utilisateur.Email,
                            Nom = x.Utilisateur.Nom,
                            Prenom = x.Utilisateur.Prenom,
                            LangueId = x.Utilisateur.LangueId
                        }
                    }).ToListAsync(tocken).ConfigureAwait(false);
            return results;
        }


        public IQueryable<StatutDossierAmf> GetStatutDossierAmfs()
        {
            return Context.StatutDossierAmfs;
            /*.Where(x => x.IsActive)*/
        }

        public async Task<bool> IsAccesDossierConfidentialAmf(int responsableDossierId, int directionId, int activiteId,
            CancellationToken token = default)
        {
            var result = await Context.UtilisateurDirections.AnyAsync(d => d.UtilisateurId == responsableDossierId &&
                                                                           d.ActiviteId == activiteId &&
                                                                           d.Direction.IsActive &&
                                                                           d.Confidentiality == true &&
                                                                           d.DirectionId == directionId, token)
                .ConfigureAwait(false);

            return result;
        }


        public async Task<ConfidentielAmf> GetConfidentialMessageAmf(int directionId, CancellationToken token = default)
        {
            var result = await Context.ConfidentielAmfs.FirstOrDefaultAsync(d => d.DirectionId == directionId, token)
                .ConfigureAwait(false);

            return result;
        }

        public IQueryable<TitreAuquelEntiteAgitAmf> GetTitreAuquelEntiteAgitAmfs()
        {
            return Context.TitreAuquelEntiteAgitAmfs;
        }

        public IQueryable<TypeActiviteAmf> GetTypeActiviteAmfs()
        {
            return Context.TypeActiviteAmfs;
        }

        IQueryable<TypeInstrumentFinancierAmf> IReferentielService.GetTypeInstrumentFinancierAmfs()
        {
            return Context.TypeInstrumentFinancierAmfs;
        }

        public IQueryable<TypeInstrumentUsuellementNegocieAmf> GetTypeInstrumentUsuellementNegocieAmfs()
        {
            return Context.TypeInstrumentUsuellementNegocieAmfs;
        }

        public IQueryable<TypeProduitDeriveAmf> GetTypeProduitDeriveAmfs()
        {
            return Context.TypeProduitDeriveAmfs;
        }


        public IQueryable<PpeType> GetPpeTypes()
        {
            return Context.PpeTypes;
        }

        public IQueryable<SensDemande> GetSensDemandes()
        {
            return Context.SensDemandes;
        }

        public IQueryable<StatutOrdreOuTransactionAmf> GetStatutOrdreOuTransactionAmfs()
        {
            return Context.StatutOrdreOuTransactionAmfs;
        }

        public async Task<DirectionCoordonnee> GetDirectionCoordonnee(int directionId,
            CancellationToken token = default)
        {
            var directionCoordonnee = await Context.DirectionCoordonnees
                .Include(x => x.Direction)
                .Include(x => x.FormeJuridique)
                .Include(x => x.TypeVoie)
                .Include(x => x.Pays).FirstOrDefaultAsync(x => x.DirectionId == directionId, token)
                .ConfigureAwait(false);
            return directionCoordonnee;
        }

        public IQueryable<StatutUtilisateur> GetStatutUtilisateurs()
        {
            return Context.StatutUtilisateurs.Where(x => x.IsActive);
        }


        public IQueryable<CarnetAdresses> GetCarnetAdresses()
        {
            return Context.CarnetAdresses;
        }

        public IQueryable<ModeEnvoieTracfin> GetModeEnvoieTracfin()
        {
            return Context.ModeEnvoieTracfins;
        }

        public IQueryable<ConfigurationValue> GetConfigurations()
        {
            return Context.ConfigurationValues.Where(x => x.Id != null);
        }


        /// <summary>
        ///     Retourne la liste des admins locaux par direction.
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="modulesIds"></param>
        /// <returns></returns>
        public IList<UtilisateurViewModel> LoadAdminLocalByDirection(int directionId, IList<int> modulesIds)
        {
            var listUtilisateurs = Context.AspNetUserRoles
                .Include(x => x.UtilisateurDirection.Utilisateur)
                .Include(x => x.UtilisateurDirection.Activite)
                .Where(x => x.UtilisateurDirection.Utilisateur.AccesBacarat
                            && x.UtilisateurDirection.StatutUtilisateurId != (int)EStatutUtilisateur.Bloqued
                            && x.UtilisateurDirection.DirectionId == directionId
                            && x.RoleId == ((int)RoleUser.AdminLocal).ToString()
                            && modulesIds.Contains(x.UtilisateurDirection.Activite.Id))
                .Select(u => new UtilisateurViewModel
                {
                    Id = u.UtilisateurDirection.UtilisateurId,
                    Nom = u.UtilisateurDirection.Utilisateur.Nom,
                    Prenom = u.UtilisateurDirection.Utilisateur.Prenom,
                    Email = u.UtilisateurDirection.Utilisateur.Email
                }).AsEnumerable();

            return listUtilisateurs.DistinctBy(x => x.Id).ToList();
        }

        public IList<UtilisateurViewModel> LoadAdminGlobalByActivite()
        {
            var listUtilisateurs = Context.AspNetUserRoles
                .Include(x => x.UtilisateurDirection.Utilisateur)
                .Include(x => x.UtilisateurDirection.Activite)
                .Where(x => x.UtilisateurDirection.Utilisateur.AccesBacarat
                            && x.UtilisateurDirection.StatutUtilisateurId != (int)EStatutUtilisateur.Bloqued
                            && x.RoleId == ((int)RoleUser.AdminGlobal).ToString())
                .Select(u => new UtilisateurViewModel
                {
                    Id = u.UtilisateurDirection.UtilisateurId,
                    Nom = u.UtilisateurDirection.Utilisateur.Nom,
                    Prenom = u.UtilisateurDirection.Utilisateur.Prenom,
                    Email = u.UtilisateurDirection.Utilisateur.Email
                }).AsEnumerable();

            return listUtilisateurs.DistinctBy(x => x.Id).ToList();
        }

        /// <summary>
        ///     Modifie la culture de l'url.
        /// </summary>
        /// <param name="url"></param>
        public string GetOtherCultreUrl(string url)
        {
            var urlArray = url.Split("/");
            if (urlArray[3] == "fr")
                urlArray[3] = "en";
            else
                urlArray[3] = "fr";
            return string.Join("/", urlArray);
        }

        public IQueryable<int> GetDepartmentsHavingLocalAdmin(IList<int> departments)
        {
            return Context.UtilisateurDirections
                .AsNoTracking()
                .Where(ud => departments.Contains(ud.DirectionId) &&
                             ud.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                             ud.AspNetUserRoles.Any(r => r.RoleId == ((int)RoleUser.AdminLocal).ToString()))
                .Select(ud => ud.DirectionId)
                .Distinct();
        }

        public bool HasParentAdmin(int departmentId)
        {
            var department = Context.Directions
                .AsNoTracking()
                .SingleOrDefault(d => d.Id == departmentId);

            if (department == null)
                return false;

            if (department.ParentId == null)
                return false;

            if (HasLocalAdmin(department.ParentId.Value))
                return true;

            return HasParentAdmin(department.ParentId.Value);
        }

        public bool HasLocalAdmin(int departmentId)
        {
            return Context.UtilisateurDirections
                .AsNoTracking()
                .Where(ud => ud.DirectionId == departmentId)
                .Any(ud => ud.StatutUtilisateurId == (int)EStatutUtilisateur.Validated &&
                           ud.AspNetUserRoles.Any(r => r.RoleId == ((int)RoleUser.AdminLocal).ToString()));
        }

        /// <summary>
        ///     Edit traduction template.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="frenchTranslate"></param>
        /// <param name="englisTranslate"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> EditTraductionTemplate(string id, string frenchTranslate, string englisTranslate,
            CancellationToken token = default)
        {
            var traduction = await Context.TraductionTemplates.SingleOrDefaultAsync(x => x.Id == id, token)
                .ConfigureAwait(false);
            try
            {
                if (traduction != null)
                {
                    traduction.FrenchTranslate = frenchTranslate;
                    traduction.EnglishTranslate = englisTranslate;
                    Context.TraductionTemplates.Update(traduction);
                    await Context.SaveChangesAsync(token);
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
            }

            return false;
        }

        public IQueryable<Activite> GetActiviteByDirection(int id, IUserInfoService userInfoService)
        {
            return Context
                .UtilisateurDirections
                .Include(d => d.Activite)
                .Where(d => d.DirectionId == id && d.Utilisateur.AspNetUsersId == userInfoService.UserId)
                .Select(t => t.Activite);
        }


        public IQueryable<Activite> GetFirstActiviteByUser(IUserInfoService userInfoService)
        {
            return Context.UtilisateurDirections
                .Include(d => d.Direction)
                .Include(d => d.Utilisateur)
                .Include(d => d.Activite)
                .Where(d => d.Utilisateur.AspNetUsersId == userInfoService.UserId &&
                            d.StatutUtilisateurId == (int)EStatutUtilisateur.Validated)
                .Select(u => u.Activite)
                .GroupBy(g => g.Code)
                .Select(x => x.First());
        }

        public async Task<IEnumerable<QualificationDossier>> GetQualificationDossierAsync(CancellationToken token)
        {
            return await Context.QualificationDossiers
                .Where(x => x.IsActive)
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<QualificationDossierEscalade>> GetQualificationDossierEscaladeAsync(
            CancellationToken token)
        {
            return await Context.QualificationDossierEscalades
                .Where(x => x.IsActive)
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        private async Task<IEnumerable<Utilisateur>> GetUtilisateurValideurInDirectionAsync(bool isValidateurFlge,
            int directionId, int activiteId, CancellationToken token)
        {
            var result = await Context.AspNetUserRoles
                .Include(x => x.UtilisateurDirection)
                .ThenInclude(x => x.Utilisateur)
                .Where(u => u.UtilisateurDirection.DirectionId == directionId &&
                            u.UtilisateurDirection.ActiviteId == activiteId &&
                            (u.RoleId == "4" || (u.RoleId == "5" && !isValidateurFlge)) &&
                            u.UtilisateurDirection.StatutUtilisateurId == (int)EStatutUtilisateur.Validated)
                .GroupBy(g => new
                {
                    g.UtilisateurDirection.Utilisateur.Id,
                    g.UtilisateurDirection.Utilisateur.Nom,
                    g.UtilisateurDirection.Utilisateur.Prenom,
                    g.UtilisateurDirection.Utilisateur.DirectionAttacheId
                })
                .Select(g => new Utilisateur
                {
                    Id = g.Key.Id,
                    Nom = g.Key.Nom.ToUpper(CultureInfo.CurrentCulture),
                    Prenom = g.Key.Prenom,
                    DirectionAttacheId = g.Key.DirectionAttacheId
                })
                .ToListAsync(token)
                .ConfigureAwait(false);

            return result;
        }


        public IQueryable<CategorieTracfin> GetCategorieTracfinAmfs()
        {
            return Context.CategorieTracfins.Where(x => x.IsActive);
        }

        public IQueryable<TypeInstrumentFinancierAmf> GetTypeInstrumentFinancierAmfs()
        {
            return Context.TypeInstrumentFinancierAmfs;
        }

        public IList<EmailTemplateLab> GetEmailTemplateLab()
        {
            return Context.EmailTemplateLabs.ToList();
        }

        /// <summary>
        ///     Mise en forme des requêtes d'insertion: Ajout apostrophe.
        /// </summary>
        /// <param name="traduction"></param>
        /// <returns></returns>
        private static string GenerateTraduction(string traduction)
        {
            var result = string.Empty;
            if (traduction.Contains("'"))
            {
                var splitedTraduction = traduction.Split(" ");
                foreach (var item in splitedTraduction)
                {
                    var index = item.IndexOf("'", StringComparison.Ordinal);
                    if (index != -1)
                        result += " " + item.Insert(index, "'");
                    else
                        result += " " + item;
                }
            }
            else
            {
                result = traduction;
            }

            return result.TrimStart();
        }

        /// <summary>
        ///     Suppressions des div générés par le htmlEditor.
        /// </summary>
        /// <param name="traduction"></param>
        /// <param name="estMailTemplate"></param>
        /// <returns></returns>
        private static string TrimString(string traduction, bool estMailTemplate)
        {
            var result = traduction;


            const string startDelete =
                "<div class=\"dx-quill-container ql-container\"><div class=\"ql-editor dx-htmleditor-content\" data-gramm=\"false\" contenteditable=\"true\" tabindex=\"0\">";
            const string endDelete =
                "</div><div class=\"ql-clipboard\" contenteditable=\"true\" tabindex=\"-1\"></div></div>";

            if (!string.IsNullOrEmpty(traduction) && traduction.Contains(startDelete))
            {
                result = traduction.Remove(0, startDelete.Length);
                result = result.Remove(result.Length - endDelete.Length, endDelete.Length);
            }

            if (estMailTemplate)
            {
                const string mailStart =
                    "<html xmlns:v=\"urn:schemas-microsoft-com:vml\"   xmlns:o =\"urn:schemas-microsoft-com:office:office\"  xmlns:w =\"urn:schemas-microsoft-com:office:word\"   xmlns:m =\"http://schemas.microsoft.com/office/2004/12/omml\"   xmlns=\"http://www.w3.org/TR/REC-html40\">     <body lang=FR style='tab-interval:35.4pt'>   <div class=WordSection1>";
                const string mailEnd = "  </div>  </body>  </html>";
                result = $"{mailStart}   {result}{mailEnd}";
            }


            return result;
        }
    }
}
