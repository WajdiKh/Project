using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BacaratWeb.Areas.Administration.Helpers;
using BacaratWeb.Areas.Lab.Helpers;
using BacaratWeb.Areas.Lab.Models;
using BacaratWeb.Areas.Lab.Models.Delegation;
using BacaratWeb.Areas.Lab.Services.Interfaces;
using BacaratWeb.Areas.Reporting.Views.Lab;
using BacaratWeb.Core.Extensions;
using BacaratWeb.Core.Helper;
using BacaratWeb.Core.Services;
using BacaratWeb.Entities.Commun;
using BacaratWeb.Entities.Contracts;
using BacaratWeb.Entities.Extensions;
using BacaratWeb.Entities.Lab;
using BacaratWeb.Extensions;
using BacaratWeb.Helpers.RoleClaims;
using BacaratWeb.Helpers.Security;
using BacaratWeb.Helpers.Visibility;
using BacaratWeb.Model;
using BacaratWeb.Models.Referentials;
using BacaratWeb.Models.Referentials.Contracts;
using BacaratWeb.Services.Common.Interfaces;
using BacaratWeb.Services.Commun.Interfaces;
using BacaratWeb.Services.Lab.Interfaces;
using BacaratWeb.Services.Lab.Services.Interfaces;
using BacaratWeb.Shared;
using BacaratWeb.Shared.Configurations;
using BacaratWeb.Shared.SearchCriterias;
using BacaratWeb.ViewModel.Lab;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NPOI.XSSF.UserModel;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using Saml2.Authentication.Core.Extensions;

namespace BacaratWeb.Areas.Lab.Controllers
{
    [Authorize]
    [Area("Lab")]
    public class ServiceLabController : Controller
    {
        private readonly IAuditTrace _auditTrace;
        private readonly IEventDossierLogger _eventDossierLogger;
        private readonly ILabAttachmentService _labAttachmentService;
        private readonly ILabFacadeService _labService;
        private readonly ILogger<ServiceLabController> _logger;
        private readonly IMapper _mapper;
        private readonly INationaliteAutreIdentiteLabService _nationaliteAutreIdentiteLabService;
        private readonly INationalitePersonnePhysiqueLabService _nationalitePersonnePhysiqueLabService;
        private readonly IOperationEnCoursViewModelService _operationEnCoursViewModelService;
        private readonly IOperationSuspecteViewModelService _operationSuspecteViewModelService;
        private readonly IPieceIdentiteService _pieceIdentiteService;
        private readonly IPpeTypePersonnePhysiqueLabService _ppeTypePersonnePhysiqueLabService;
        private readonly IDataProtector _protector;
        private readonly IReferentielViewModel _referential;
        private readonly IReferentielService _referentielService;
        private readonly IRoleAccessUser _roleAccessUser;
        private readonly ApplicationSettings _settings;
        private readonly IViewTranslator _translator;
        private readonly IUserInfoService _userInfoService;
        private readonly IUtilisateurDirectionService _utilisateurDirectionService;
        private readonly UtilisateurViewModel currentUser;

        public ServiceLabController(ILabFacadeService labService,
            IReferentielViewModel referential,
            IReferentielService referentielService,
            IUserInfoService userInfoService,
            ILogger<ServiceLabController> logger,
            IAuditTrace auditTrace,
            IRoleAccessUser roleAccessUser,
            IDataProtectionProvider protectionProvider,
            IMapper mapper,
            IViewTranslator translator,
            IHttpContextAccessor httpContext,
            IPieceIdentiteService pieceIdentiteService,
            IOptions<ApplicationSettings> settings,
            IUtilisateurDirectionService utilisateurDirectionService,
            INationalitePersonnePhysiqueLabService nationalitePersonnePhysiqueLabService,
            INationaliteAutreIdentiteLabService nationaliteAutreIdentiteLabService,
            IPpeTypePersonnePhysiqueLabService ppeTypePersonnePhysiqueLabService,
            IOperationSuspecteViewModelService operationSuspecteViewModelService,
            IOperationEnCoursViewModelService operationEnCoursViewModelService,
            ILabAttachmentService labAttachmentService,
            IEventDossierLogger eventDossierLogger)
        {
            _labService = labService;
            _referential = referential;
            _referentielService = referentielService;
            _logger = logger;
            _auditTrace = auditTrace;
            _roleAccessUser = roleAccessUser;
            _protector = protectionProvider?.CreateProtector("Anti_Tempered_Parameters");
            _userInfoService = userInfoService;
            _mapper = mapper;
            _translator = translator;
            _pieceIdentiteService = pieceIdentiteService;
            _settings = settings.Value;
            _utilisateurDirectionService = utilisateurDirectionService;
            _nationalitePersonnePhysiqueLabService = nationalitePersonnePhysiqueLabService;
            _nationaliteAutreIdentiteLabService = nationaliteAutreIdentiteLabService;
            _ppeTypePersonnePhysiqueLabService = ppeTypePersonnePhysiqueLabService;
            _operationEnCoursViewModelService = operationEnCoursViewModelService;
            _operationSuspecteViewModelService = operationSuspecteViewModelService;
            _labAttachmentService = labAttachmentService;
            currentUser = httpContext?.HttpContext?.Session.Get<UtilisateurViewModel>("currentUser");
            if (currentUser == null)
            {
                currentUser =
                    mapper?.Map<UtilisateurViewModel>(
                        _labService?.Referentiel?.GetConnectedUser(userInfoService?.UserId));
                httpContext?.HttpContext?.Session.Set("currentUser", currentUser);
            }

            _eventDossierLogger = eventDossierLogger ?? throw new ArgumentNullException(nameof(eventDossierLogger));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CreateOrUpdateDossierLab(DossierLabViewModel dossierLab,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            try
            {
                var userPrivilege = new UserPrivilege(_labService, currentUser.Id, dossierLab?.DirectionId);
                await userPrivilege.Initialize(User, cancellationToken).ConfigureAwait(false);
                if (!_roleAccessUser.HasActivites(ActiviteModule.Lab))
                    return new JsonResult(new
                    { status = false, message = _translator.Common["PasDePersmissionActiviteDirection"] });

                if (dossierLab != null)
                {
                    var id = this.Uprotect(dossierLab.CryptedId, _protector);
                    dossierLab.Id = id;
                    dossierLab.UserPrivilege = userPrivilege;

                    // dossierLab.UnprotectDossierLab(_protector);

                    if (IsVerifyDataCompatibility(dossierLab, userPrivilege))
                    {
                        var eventDossier = new EventDossierViewModel
                        {
                            UtilisateurEventId = currentUser.Id,
                            CreateurId = currentUser.Id,
                            DateCreation = DateTime.UtcNow,
                            IsActive = true,
                            ActiviteId = (int)ActiviteModule.Lab
                        };
                        var isTracfin = dossierLab.DirectionId != null &&
                                        _referentielService.Direction.IsTracfin(dossierLab.DirectionId.Value);
                        switch (dossierLab.StatutDossierId)
                        {
                            case (int)StatutDossierLabEnum.EnCours:
                                eventDossier.EventDossierTypeId = (int)EactionEventTypeDossier.EN_COURS_LAB;
                                break;
                            case (int)StatutDossierLabEnum.PendingEnCours:
                                eventDossier.EventDossierTypeId = (int)EactionEventTypeDossier.PENDING_EN_COURS_LAB;
                                break;
                            case (int)StatutDossierLabEnum.AttentePriseEnCharge:
                                eventDossier.EventDossierTypeId = (int)EactionEventTypeDossier.AJOUT_LAB;
                                break;
                            case (int)StatutDossierLabEnum.Cloture:
                                if (dossierLab.DeclarationTracfins != null &&
                                    dossierLab.DeclarationTracfins.Any(x => x.EstNouvelleDeclarationTracfin) &&
                                    isTracfin)
                                    eventDossier.EventDossierTypeId =
                                        (int)EactionEventTypeDossier.ATTENTE_ENVOI_TRACFIN;
                                else
                                    eventDossier.EventDossierTypeId = (int)EactionEventTypeDossier.CLOTURE_LAB;
                                break;
                            case (int)StatutDossierLabEnum.AttenteValidation:
                                eventDossier.EventDossierTypeId = (int)EactionEventTypeDossier.ATTENTE_VALIDATION_LAB;
                                break;
                            case (int)StatutDossierLabEnum.SansSuite:
                                eventDossier.EventDossierTypeId = (int)EactionEventTypeDossier.SANS_SUITE_LAB;
                                break;
                            case (int)StatutDossierLabEnum.AttenteDocuments:
                                eventDossier.EventDossierTypeId = (int)EactionEventTypeDossier.ATTENTE_DOCUMENT_LAB;
                                break;
                            case (int)StatutDossierLabEnum.EncoursEnvoiDS:
                                eventDossier.EventDossierTypeId = (int)EactionEventTypeDossier.ENVOI_TRACFIN_DS;
                                break;
                            case (int)StatutDossierLabEnum.AttenteARTracfin:
                                eventDossier.EventDossierTypeId = (int)EactionEventTypeDossier.ATTENTE_AR_TRACFIN;
                                break;
                            case (int)StatutDossierLabEnum.clotureTracfin:
                                eventDossier.EventDossierTypeId = (int)EactionEventTypeDossier.CLOTURE_TRACFIN;
                                break;
                        }

                        dossierLab.DossierLabActions = dossierLab.DossierLabActions.Where(x => !x.ToDelete).ToList();
                        dossierLab.DeclarationTracfins?.ForEach(x =>
                        {
                            x.CryptedId ??= _protector.Protect("0");
                            x.DocumentDeclarationTracfins?.ForEach(d => { d.CryptedId ??= _protector.Protect("0"); });
                            x.OperationSuspectDeclarationTracfins?.ForEach(d =>
                            {
                                d.CryptedId ??= _protector.Protect("0");
                            });
                            x.OperationEnCoursDeclarationTracfins?.ForEach(d =>
                            {
                                d.CryptedId ??= _protector.Protect("0");
                            });
                            x.AavReferences?.ForEach(d => { d.CryptedId ??= _protector.Protect("0"); });

                            if (x.OperationEnCoursDeclarationTracfins != null)
                                x.OperationEnCoursDeclarationTracfins = x.OperationEnCoursDeclarationTracfins
                                    .Where(y => !y.ToDelete).ToList();
                            if (x.OperationSuspectDeclarationTracfins != null)
                                x.OperationSuspectDeclarationTracfins = x.OperationSuspectDeclarationTracfins
                                    .Where(y => !y.ToDelete).ToList();
                            if (x.AavReferences != null)
                                x.AavReferences = x.AavReferences.Where(y => !y.ToDelete).ToList();
                            x.Mail = x.Mail != null ? x.Mail.Replace(" ", "") : "";
                            x.Telephone = x.Telephone != null ? x.Telephone.Replace(" ", "").Replace(".", "") : "";
                            x.Fax = x.Fax != null ? x.Fax.Replace(" ", "").Replace(".", "") : "";
                        });
                        if (dossierLab.DossierLabPersonnePhysiques?.Count > 0)
                        {
                            dossierLab.DossierLabPersonnePhysiques =
                                dossierLab.DossierLabPersonnePhysiques.Where(x => !x.ToDelete).ToList();
                            dossierLab.DossierLabPersonnePhysiques?.ForEach(x =>
                            {
                                x.CryptedId ??= _protector.Protect("0");
                                x.PersonnePhysiqueLab.CryptedId ??= _protector.Protect("0");
                                x.CryptedPersonnePhysiqueLabId ??= _protector.Protect("0");
                                x.CryptedDossierLabId ??= _protector.Protect("0");
                                x.PersonnePhysiqueLab.PaysNaissanceId = x.PersonnePhysiqueLab.PaysNaissanceId != 0
                                    ? x.PersonnePhysiqueLab.PaysNaissanceId
                                    : null;
                                if (!dossierLab.DeclarationTracfins.Any()) x.IsDeclarationTracfin = false;
                                x.PersonnePhysiqueLab.AutreNationalitePersonnePhysiqueLabs?.ForEach(y =>
                                {
                                    y.CryptedId ??= _protector.Protect("0");
                                    y.CryptedPersonnePhysiqueLabId = x.CryptedPersonnePhysiqueLabId;
                                });
                                x.PersonnePhysiqueLab.CoordonneePersonnePhysiqueLabs = x.PersonnePhysiqueLab
                                    .CoordonneePersonnePhysiqueLabs.Where(y => !y.ToDelete).ToList();
                                x.PersonnePhysiqueLab.CoordonneePersonnePhysiqueLabs?.ForEach(y =>
                                {
                                    y.CryptedId ??= _protector.Protect("0");
                                    y.CryptedPersonnePhysiqueLabId = y.CryptedPersonnePhysiqueLabId == null
                                        ? _protector.Protect("0")
                                        : x.CryptedPersonnePhysiqueLabId;
                                    y.CryptedCoordonneeId ??= _protector.Protect("0");
                                    y.Coordonnee.CryptedId ??= _protector.Protect("0");
                                    y.Coordonnee.TelephoneFixe = y.Coordonnee.TelephoneFixe != null
                                        ? y.Coordonnee.TelephoneFixe.Replace(" ", "").Replace(".", "")
                                        : "";
                                    y.Coordonnee.TelephoneMobile = y.Coordonnee.TelephoneMobile != null
                                        ? y.Coordonnee.TelephoneMobile.Replace(" ", "").Replace(".", "")
                                        : "";
                                    y.Coordonnee.TelephoneProfessionnel = y.Coordonnee.TelephoneProfessionnel != null
                                        ? y.Coordonnee.TelephoneProfessionnel.Replace(" ", "").Replace(".", "")
                                        : "";
                                    y.Coordonnee.Fax = y.Coordonnee.Fax != null
                                        ? y.Coordonnee.Fax.Replace(" ", "").Replace(".", "")
                                        : "";
                                    y.Coordonnee.Email = !string.IsNullOrEmpty(y.Coordonnee.Email)
                                        ? y.Coordonnee.Email.Trim()
                                        : null;
                                    y.Coordonnee.SiteWeb = !string.IsNullOrEmpty(y.Coordonnee.SiteWeb)
                                        ? y.Coordonnee.SiteWeb.Trim()
                                        : null;
                                });
                                x.PersonnePhysiqueLab.LienPersonneMorales?.ForEach(y =>
                                {
                                    y.CryptedId ??= _protector.Protect("0");
                                    y.CryptedPersonnePhysiqueLabId = x.CryptedPersonnePhysiqueLabId;
                                });
                                x.PersonnePhysiqueLab.LienPersonnePhysiques?.ForEach(y =>
                                {
                                    y.CryptedId ??= _protector.Protect("0");
                                    y.CryptedPersonnePhysiqueLabId = x.CryptedPersonnePhysiqueLabId;
                                });
                                x.PersonnePhysiqueLab.PieceIdentites?.ForEach(y =>
                                {
                                    y.CryptedId ??= _protector.Protect("0");
                                    y.CryptedPersonnePhysiqueLabId = x.CryptedPersonnePhysiqueLabId;
                                    y.Numero ??= string.Empty;
                                });
                                x.PersonnePhysiqueLab.SupportFinancierPersonnePhysiques?.ForEach(y =>
                                {
                                    y.CryptedId ??= _protector.Protect("0");
                                    y.CryptedPersonnePhysiqueLabId = x.CryptedPersonnePhysiqueLabId;
                                });
                                x.PersonnePhysiqueLab.ActiviteProfessionnellePersonnePhysiqueLabs?.ForEach(y =>
                                {
                                    y.CryptedId ??= _protector.Protect("0");
                                    y.CryptedPersonnePhysiqueLabId = x.CryptedPersonnePhysiqueLabId;
                                });
                            });
                        }

                        if (dossierLab.DossierLabNonConnus?.Count > 0)
                        {
                            dossierLab.DossierLabNonConnus =
                                dossierLab.DossierLabNonConnus.Where(x => !x.ToDelete).ToList();
                            dossierLab.DossierLabNonConnus?.ForEach(x =>
                            {
                                x.CryptedId ??= _protector.Protect("0");
                                x.NonConnuLab.CryptedId ??= _protector.Protect("0");
                                x.CryptedNonConnuLabId ??= _protector.Protect("0");
                                x.CryptedDossierLabId ??= _protector.Protect("0");
                                if (!dossierLab.DeclarationTracfins.Any()) x.IsDeclarationTracfin = false;
                                x.NonConnuLab.SupportFinancierNonConnus?.ForEach(y =>
                                {
                                    y.CryptedId ??= _protector.Protect("0");
                                    y.CryptedNonConnuLabId = x.CryptedNonConnuLabId;
                                });
                                x.NonConnuLab.LienPersonneMorales?.ForEach(y =>
                                {
                                    y.CryptedId ??= _protector.Protect("0");
                                    y.CryptedNonConnuLabId = x.CryptedNonConnuLabId;
                                });
                                x.NonConnuLab.LienPersonnePhysiques?.ForEach(y =>
                                {
                                    y.CryptedId ??= _protector.Protect("0");
                                    y.CryptedNonConnuLabId = x.CryptedNonConnuLabId;
                                });
                            });
                        }

                        if (dossierLab.DossierLabPersonneMorales?.Count > 0)
                        {
                            dossierLab.DossierLabPersonneMorales =
                                dossierLab.DossierLabPersonneMorales.Where(x => !x.ToDelete).ToList();
                            dossierLab.DossierLabPersonneMorales?.ForEach(x =>
                            {
                                if (!dossierLab.DeclarationTracfins.Any()) x.IsDeclarationTracfin = false;

                                x.CryptedId ??= _protector.Protect("0");
                                x.PersonneMoraleLab.CryptedId ??= _protector.Protect("0");
                                x.CryptedPersonneMoraleLabId ??= _protector.Protect("0");
                                x.CryptedDossierLabId ??= _protector.Protect("0");

                                x.PersonneMoraleLab.LienPersonneMorales?.ForEach(y =>
                                {
                                    y.CryptedId ??= _protector.Protect("0");
                                    y.CryptedPersonneMoraleLabId = x.CryptedPersonneMoraleLabId;
                                });
                                x.PersonneMoraleLab.LienPersonnePhysiques?.ForEach(y =>
                                {
                                    y.CryptedId ??= _protector.Protect("0");
                                    y.CryptedPersonneMoraleLabId = x.CryptedPersonneMoraleLabId;
                                });
                                x.PersonneMoraleLab.RepresentantLegals?.ForEach(y =>
                                {
                                    y.CryptedId ??= _protector.Protect("0");
                                    y.CryptedPersonneMoraleLabId = x.CryptedPersonneMoraleLabId;
                                });
                                x.PersonneMoraleLab.Dirigeants?.ForEach(y =>
                                {
                                    y.CryptedId ??= _protector.Protect("0");
                                    y.CryptedPersonneMoraleLabId = x.CryptedPersonneMoraleLabId;
                                    y.Mail = y.Mail?.Replace(" ", "");
                                    y.Telephone = y.Telephone?.Replace(" ", "").Replace(".", "");
                                });
                                x.PersonneMoraleLab.SupportFinancierPersonneMorales?.ForEach(y =>
                                {
                                    y.CryptedId ??= _protector.Protect("0");
                                    y.CryptedPersonneMoraleLabId = x.CryptedPersonneMoraleLabId;
                                });
                                if (x.PersonneMoraleLab?.Coordonnee?.ToDelete == true)
                                    x.PersonneMoraleLab.Coordonnee = null;
                                if (x.PersonneMoraleLab?.Coordonnee is { CryptedId: null })
                                {
                                    x.CryptedId ??= _protector.Protect("0");
                                    x.CryptedPersonneMoraleLabId ??= _protector.Protect("0");
                                    x.PersonneMoraleLab.Coordonnee.CryptedId ??= _protector.Protect("0");
                                    x.PersonneMoraleLab.CryptedCoordonneeId ??= _protector.Protect("0");
                                }

                                if (x.PersonneMoraleLab?.Coordonnee != null)
                                {
                                    x.PersonneMoraleLab.Coordonnee.TelephoneFixe =
                                        x.PersonneMoraleLab.Coordonnee.TelephoneFixe != null
                                            ? x.PersonneMoraleLab.Coordonnee.TelephoneFixe.Replace(" ", "")
                                                .Replace(".", "")
                                            : "";
                                    x.PersonneMoraleLab.Coordonnee.TelephoneMobile =
                                        x.PersonneMoraleLab.Coordonnee.TelephoneMobile != null
                                            ? x.PersonneMoraleLab.Coordonnee.TelephoneMobile.Replace(" ", "")
                                                .Replace(".", "")
                                            : "";
                                    x.PersonneMoraleLab.Coordonnee.TelephoneProfessionnel =
                                        x.PersonneMoraleLab.Coordonnee.TelephoneProfessionnel != null
                                            ? x.PersonneMoraleLab.Coordonnee.TelephoneProfessionnel.Replace(" ", "")
                                                .Replace(".", "")
                                            : "";
                                    x.PersonneMoraleLab.Coordonnee.Fax = x.PersonneMoraleLab.Coordonnee.Fax != null
                                        ? x.PersonneMoraleLab.Coordonnee.Fax.Replace(" ", "").Replace(".", "")
                                        : "";
                                    x.PersonneMoraleLab.Coordonnee.Email =
                                        !string.IsNullOrEmpty(x.PersonneMoraleLab.Coordonnee.Email)
                                            ? x.PersonneMoraleLab.Coordonnee.Email.Trim()
                                            : null;
                                    x.PersonneMoraleLab.Coordonnee.SiteWeb =
                                        !string.IsNullOrEmpty(x.PersonneMoraleLab.Coordonnee.SiteWeb)
                                            ? x.PersonneMoraleLab.Coordonnee.SiteWeb.Trim()
                                            : null;
                                }
                            });
                        }

                        if (dossierLab.DeclarationTracfins != null && dossierLab.DeclarationTracfins.Any())
                        {
                            if (dossierLab.DeclarationTracfins[0].IsSoupconNonFinanciers)
                                dossierLab.DeclarationTracfins[0].OperationSuspectDeclarationTracfins.Clear();

                            if (!dossierLab.DeclarationTracfins[0].IsOperationCoursExecution)
                                dossierLab.DeclarationTracfins[0].OperationEnCoursDeclarationTracfins.Clear();

                            if (dossierLab.DeclarationTracfins.First().OrganismeId.HasValue)
                            {
                                var organismeId = dossierLab.DeclarationTracfins.First().OrganismeId;
                                if (organismeId != null)
                                {
                                    var organisme = await _labService.OrganismeLab
                                        .GetOrganismeById(organismeId.Value,
                                            true, cancellationToken)
                                        .ConfigureAwait(false);
                                    if ((organisme.Profession?.Code != "TPI6" &&
                                         organisme.Profession?.Code != "TPI4") ||
                                        dossierLab.DeclarationTracfins.FirstOrDefault() is
                                        { IsQuestionDeclarationActivite: true })
                                        dossierLab.DeclarationTracfins.FirstOrDefault().OperationsCompaniesAssurances =
                                            null;
                                }
                            }
                        }

                        IgnoreDeletedContributeurs(dossierLab);

                        dossierLab.UnprotectDossierLab(_protector);

                        dossierLab.DossierLabActions.ForEach(x =>
                        {
                            if (x.Id == 0)
                            {
                                x.CreateurId = currentUser.Id;
                                x.ModificateurId = null;
                                x.DateCreation = DateTimeOffset.Now.DateTime;
                            }
                            else
                            {
                                x.ModificateurId = currentUser.Id;
                                x.DateModification = DateTimeOffset.Now.DateTime;
                            }
                        });
                        if (dossierLab.CategorieGroupeLabId != 5 && dossierLab.CategorieGroupeLabId != 6)
                            dossierLab.DateReponse = null;
                        if (dossierLab.IsNew)
                        {
                            dossierLab.CreateurId = currentUser.Id;
                            dossierLab.UtilisateurId = currentUser.Id;
                        }
                        else
                        {
                            dossierLab.DateModification = DateTimeOffset.Now.DateTime;
                            dossierLab.ModificateurId = currentUser.Id;
                        }

                        //us 1945: ich: on met à jour la date cloture si est seulement si le type declaration différent de nouvelle ds.
                        if (dossierLab.DeclarationTracfins != null && !dossierLab.DeclarationTracfins.Any())
                        {
                            if (dossierLab.StatutDossierId == (int)StatutDossierLabEnum.Cloture &&
                                dossierLab.DateCloture == null) dossierLab.DateCloture = DateTime.Now;
                        }
                        else
                        {
                            if (dossierLab.DeclarationTracfins != null &&
                                dossierLab.DeclarationTracfins.All(x => !x.EstNouvelleDeclarationTracfin) &&
                                dossierLab.StatutDossierId == (int)StatutDossierLabEnum.Cloture &&
                                dossierLab.DateCloture == null)
                                dossierLab.DateCloture = DateTime.Now;
                        }

                        var updateAuthorized = false;
                        var errorMessage = string.Empty;

                        UpdateDocumentsDossierLab(dossierLab);

                        if (_roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal))
                        {
                            updateAuthorized = true;
                        }
                        else
                        {
                            if (userPrivilege.Write)
                            {
                                if (dossierLab.UtilisateurId == currentUser.Id)
                                {
                                    updateAuthorized = true;
                                }
                                else
                                {
                                    var isDelegated = await userPrivilege.IsUserDelegatedBy(dossierLab.UtilisateurId,
                                            dossierLab.DirectionId.Value,
                                            cancellationToken)
                                        .ConfigureAwait(false);

                                    if (!isDelegated)
                                        errorMessage = _translator.Common["PasDePersmissionModifierDossierLab"];

                                    updateAuthorized = true;
                                }
                            }
                            else
                            {
                                errorMessage = _translator.Common["PasDePersmissionCreerModifierDossierLab"];
                            }
                        }

                        if (!updateAuthorized) return new JsonResult(new { status = false, message = errorMessage });

                        dossierLab.Confidentiel = dossierLab.Confidentiel && await _labService
                            .UtilisateurDirectionService
                            .IsConfidentialInActivity(currentUser.Id, dossierLab.DirectionId.Value,
                                (int)ActiviteModule.Lab,
                                cancellationToken).ConfigureAwait(false);

                        dossierLab.DeclarationTracfins?.ForEach(x =>
                        {
                            if (string.IsNullOrEmpty(x.DeclarationNumber))
                            {
                                if (dossierLab.DirectionId != null)
                                {
                                    var derniereReference = _labService.DeclarationTracfin
                                        .GetReferenceTracfinAsync(dossierLab.DirectionId.Value, cancellationToken).Result;
                                    x.DeclarationNumber = derniereReference.ToString();
                                }
                            }

                            if (string.IsNullOrEmpty(x.ReferenceInterne)) x.ReferenceInterne = x.DeclarationNumber;
                        });
                        if (dossierLab.StatutDossierId == (int)StatutDossierLabEnum.AttenteValidation)
                        {
                            var entity = _mapper.Map<DossierLab>(dossierLab);
                            if (!dossierLab.IsNew)
                            {
                                var dossierStatutId = await _labService.Dossier
                                    .GetStatutDossierLabAsync(dossierLab.Id, cancellationToken).ConfigureAwait(false);
                                if (dossierStatutId != dossierLab.StatutDossierId)
                                    await AddNotificationMailAttenteValidationDossier(entity.CodeUnique,
                                        entity.DirectionId, cancellationToken).ConfigureAwait(false);
                            }
                        }

                        foreach (var item in dossierLab.DeclarationTracfins?.ToList())
                            if (item.IsQuestionVigilanceAAV && !item.AavReferences.Any())
                                return new JsonResult(new
                                { status = false, message = _translator.Common["ErrorAavObligatoire"] });

                        if (dossierLab.IsNew)
                        {
                            eventDossier.EventDossierTypeId = (int)EactionEventTypeDossier.AJOUT_LAB;
                            if (dossierLab.DossierLabPersonnePhysiques != null)
                                foreach (var personnePhysique in dossierLab.DossierLabPersonnePhysiques)
                                    if (personnePhysique.PhysicalPersonIsAlreadyImported)
                                        personnePhysique.PersonnePhysiqueLab = null;
                            if (dossierLab.DossierLabPersonneMorales != null)
                                foreach (var personneMorale in dossierLab.DossierLabPersonneMorales)
                                    if (personneMorale.MoralPersonIsAlreadyImported)
                                        personneMorale.PersonneMoraleLab = null;
                            if (!string.IsNullOrEmpty(dossierLab.DossierFraudeCryptedId))
                            {
                                var currentLanguage = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture);
                                var dossierLabWithAttachement = await _labAttachmentService
                                    .GetAttachmentsFraudeByDossierId(dossierLab.DossierFraudeCryptedId, currentLanguage,
                                        cancellationToken).ConfigureAwait(false);
                                dossierLab.Attachments = dossierLabWithAttachement.Attachments;
                                dossierLab.DocumentDossierLabs = dossierLabWithAttachement.DocumentDossierLabs;
                            }
                            return await AddDossierAsync(dossierLab, cancellationToken).ConfigureAwait(false);
                        }

                        // récupération des Pp.
                        var msgPp = string.Empty;
                        var listPp = new List<string>();
                        if (dossierLab.DossierLabPersonnePhysiques.Any())
                            foreach (var item in dossierLab.DossierLabPersonnePhysiques)
                                if ((item.PersonnePhysiqueLab.NatureRelationClientId == 1 ||
                                     item.PersonnePhysiqueLab.NatureRelationClientId == 2) &&
                                    !item.PersonnePhysiqueLab.SupportFinancierPersonnePhysiques.Any())
                                {
                                    listPp.Add(item.PersonnePhysiqueLab.NomNaissance + " " +
                                               item.PersonnePhysiqueLab.Prenoms);
                                    msgPp = string.Join(" ,", listPp);
                                }

                        // récupération des Pm.
                        var msgPm = string.Empty;
                        var listPm = new List<string>();
                        if (dossierLab.DossierLabPersonneMorales.Any())
                            foreach (var item in dossierLab.DossierLabPersonneMorales)
                                if ((item.PersonneMoraleLab.NatureRelationClientId == 1 ||
                                     item.PersonneMoraleLab.NatureRelationClientId == 2) &&
                                    !item.PersonneMoraleLab.SupportFinancierPersonneMorales.Any())
                                {
                                    listPm.Add(item.PersonneMoraleLab.RaisonSociale);
                                    msgPm = string.Join(" ,", listPm);
                                }

                        if (!string.IsNullOrEmpty(msgPp) || !string.IsNullOrEmpty(msgPm))
                        {
                            var separteur = !string.IsNullOrEmpty(msgPm) ? ", " : string.Empty;
                            return new JsonResult(new
                            {
                                status = false,
                                message = _translator.Common["ReferenceFinanciereObligatoire"] + msgPp + separteur +
                                          msgPm
                            });
                        }

                        eventDossier.DossierLabId = dossierLab.Id;
                        eventDossier.CodeDossier = dossierLab.CodeUnique;

                        var result = await UpdateDossier(dossierLab, cancellationToken)
                            .ConfigureAwait(false);

                        var status = ((dynamic)result.Value).status;

                        if (status)
                            await _labService.EventDossier
                                .AddAsync(_mapper.Map<EventDossier>(eventDossier), cancellationToken)
                                .ConfigureAwait(false);

                        return result;
                    }

                    return new JsonResult(
                        new { status = false, message = _translator.Common["IsVerifyDataCompatibility"] });
                }

                return new JsonResult(new { status = false, message = _translator.Common["DossierNonValide"] });
            }
            catch (CryptographicException cryptoException)
            {
                _logger.TraceError(cryptoException);
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();
            return new JsonResult(new { status = false, message = _translator.Common["ErrorMessage"] });
        }

        private void IgnoreDeletedContributeurs(DossierLabViewModel dossierLab)
        {
            if (dossierLab?.DeclarationTracfins == null || dossierLab.DeclarationTracfins.Count == 0)
                return;

            foreach (var declarationTracfin in dossierLab.DeclarationTracfins)
                declarationTracfin.Contributeurs = declarationTracfin.Contributeurs?.Where(c => !c.IsDeleted).ToList();
        }

        private void UpdateDocumentsDossierLab(DossierLabViewModel dossierLab)
        {
            if (dossierLab == null) throw new ArgumentNullException(nameof(dossierLab));

            var attachments = dossierLab.Attachments
                .Where(a => !a.ToDelete)
                .ToList();

            var documentsDossierLab = new List<DocumentDossierLabViewModel>();

            foreach (var attachment in attachments)
            {
                var document = _mapper.Map<DocumentDossierLabViewModel>(attachment);

                if (attachment.File != null)
                {
                    document.DocumentLab = attachment.File.Convert();
                    document.FileName = attachment.File.FileName.CleanFileName();
                }

                documentsDossierLab.Add(document);
            }

            dossierLab.DocumentDossierLabs = documentsDossierLab;
        }

        public async Task<JsonResult> EnvoieTracfin(string cryptedDossierId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            var _id = this.Uprotect(cryptedDossierId, _protector);


            var entiteDossier = await _labService.Dossier.GetAsNoTrackingAsync(_id, cancellationToken)
                .ConfigureAwait(false);


            var declaration = await _labService.DeclarationTracfin
                .GetDeclarationByDossierAsync(_id, cancellationToken)
                .ConfigureAwait(false);

            if (declaration.DeclarantId == null && declaration.DateDeclaration == null)
            {
                declaration.DateDeclaration = DateTimeOffset.Now;
                declaration.DeclarantId = currentUser?.Id;

                _labService.DeclarationTracfin.UpdateCore(declaration);
                entiteDossier.StatutDossierId = (int)StatutDossierLabEnum.EncoursEnvoiDS;

                if (await _labService.Dossier.UpdateDossierAsync(entiteDossier, cancellationToken)
                        .ConfigureAwait(false))
                {
                    var eventDossier = new EventDossierViewModel
                    {
                        UtilisateurEventId = currentUser?.Id,
                        CreateurId = currentUser?.Id,
                        DateCreation = DateTime.UtcNow,
                        IsActive = true,
                        ActiviteId = (int)ActiviteModule.Lab,
                        EventDossierTypeId = (int)EactionEventTypeDossier.ENVOI_TRACFIN_DS,
                        DossierLabId = entiteDossier.Id,
                        CodeDossier = entiteDossier.CodeUnique
                    };

                    await _labService.EventDossier.AddAsync(_mapper.Map<EventDossier>(eventDossier), cancellationToken)
                        .ConfigureAwait(false);

                    await _auditTrace.AddAuditEvent(AuditActionType.Update,
                        $"{AuditAction.Lab} Code Unique : {entiteDossier.CodeUnique}",
                        $"Id {entiteDossier.Id}  CodeUnique {entiteDossier.CodeUnique}",
                        cancellationToken).ConfigureAwait(false);
                    var statusDossier =
                        _referential.StatutDossierLabs.FirstOrDefault(x => x.Id == entiteDossier.StatutDossierId);
                    var libelleStatus = this.IsCultureFr() ? statusDossier?.NameFr : statusDossier?.NameEn;
                    return new JsonResult(new
                    {
                        status = true,
                        message = _translator.Common["EnvoiOk"],
                        statusDossier = libelleStatus,
                        id = statusDossier?.Id
                    });
                }

                return new JsonResult(new { status = false, message = _translator.Common["ErrorMessageEnvoiTracfin"] });
            }

            return new JsonResult(new
            {
                status = false,
                message = _translator.Common["ErrorMessageDejaEnvoi"]
            });
        }

        public bool IsVerifyDataCompatibility(DossierLabViewModel dossierLab, UserPrivilege userPrivilege)
        {
            var statutDossierLabs = new List<int>();
            try
            {
                if (dossierLab == null) throw new ArgumentNullException(nameof(dossierLab));


                if (userPrivilege == null) throw new ArgumentNullException(nameof(userPrivilege));
                var hasDirectionRight = _labService.Referentiel
                                            .GetDirectionHabiliteUser((int)ActiviteModule.Lab, _userInfoService)
                                            .Any(x => x.Id == dossierLab.DirectionId) ||
                                        _roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal);

                if (hasDirectionRight)
                {
                    var isValidation = _referential.CategorieLabs.Any(x =>
                        x.Id == dossierLab.CategorieId && x.DirectionId == dossierLab.DirectionId && x.IsActive &&
                        x.IsValidation);
                    var listStatutDossierLab =
                        _referential.StatutDossierLabs.Where(x => x.IsActive).Select(x => x.Id).ToList();

                    if (userPrivilege.IsValideur)
                        statutDossierLabs.AddRange(
                            listStatutDossierLab.Where(x => x is 2 or 3 or 4 or 5 or 6));
                    else if (userPrivilege.IsPending)
                        statutDossierLabs.AddRange(listStatutDossierLab.Where(x => x is 1 or 7));
                    else if (isValidation)
                        statutDossierLabs.AddRange(listStatutDossierLab.Where(x => x is 3 or 4 or 6));
                    else
                        statutDossierLabs.AddRange(
                            listStatutDossierLab.Where(x => x is 2 or 3 or 4 or 5));
                }

                return hasDirectionRight && statutDossierLabs.Contains(dossierLab.StatutDossierId);
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
                return false;
            }
        }

        private async Task<JsonResult> UpdateDossier(DossierLabViewModel dossierUpdate,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();

            try
            {
                TryValidateModel(dossierUpdate);

                dossierUpdate.ModificateurId = currentUser.Id;

                dossierUpdate.DossierLabActions
                    .ForEach(x =>
                    {
                        x.ModificateurId = currentUser.Id;
                        x.DateModification = DateTime.Now;
                    });

                var entity = _mapper.Map<DossierLab>(dossierUpdate);


                if (entity.DeclarationTracfins != null && entity.DeclarationTracfins.Any())
                    if (!entity.DeclarationTracfins.First().IsSoupconNonFinanciers &&
                        entity.DeclarationTracfins.First().OperationSuspectDeclarationTracfins != null)
                        if (!await _labService.OrganismeLab.IsOrganismeLabAssuranceByCode(entity.DirectionId) &&
                            !await _labService.OrganismeLab.IsOrganismeLabImmobilierByCode(entity.DirectionId) &&
                            entity.DeclarationTracfins.First().EstNouvelleDeclarationTracfin
                            && !entity.DeclarationTracfins.First().OperationSuspectDeclarationTracfins.Any())
                            return new JsonResult(new
                            {
                                status = false,
                                message = _translator.Common["ErrorOperationSuspectObligatoire"]
                            });

                var exitDossier = dossierUpdate.StatutDossierId == (int)StatutDossierLabEnum.Cloture ||
                                  dossierUpdate.StatutDossierId == (int)StatutDossierLabEnum.SansSuite
                                  || dossierUpdate.StatutDossierId == (int)StatutDossierLabEnum.AttentePriseEnCharge
                                  || (dossierUpdate.StatutDossierId == (int)StatutDossierLabEnum.AttenteValidation &&
                                      !dossierUpdate.UserPrivilege.IsValideur);

                if (await _labService.Dossier.UpdateAsync(entity, cancellationToken).ConfigureAwait(false))
                {
                    var dossierHisto = _mapper.Map<DossierLabHisto>(entity);
                    dossierHisto.DossierLabId = entity.Id;
                    await AuditUpdatedDossier(dossierHisto, 2, cancellationToken).ConfigureAwait(false);

                    if (dossierUpdate.DeclarationTracfins.Any() &&
                        dossierUpdate.DeclarationTracfins.First().Id == 0)
                        dossierUpdate.DeclarationTracfins.First().Id =
                            (await _labService.DeclarationTracfin
                                .GetDeclarationTracfin(dossierUpdate.Id, cancellationToken).ConfigureAwait(false)).Id;

                    var natureSoupconInfractionPenaleId = dossierUpdate.DeclarationTracfins
                        ?.Select(x => x.NatureSoupconInfractionPinaleId).FirstOrDefault();
                    if (dossierUpdate.DeclarationTracfins != null && dossierUpdate.DeclarationTracfins.Any())
                    {
                        var infractionPenaleEntity = await _labService.DeclarationTracfinNatureInfractionPenaleService
                            .GetAllByIdAsync(dossierUpdate.DeclarationTracfins.FirstOrDefault()?.Id ?? 0, true,
                                cancellationToken).ConfigureAwait(false);

                        if (natureSoupconInfractionPenaleId != null || infractionPenaleEntity.Any())
                            await _labService.DeclarationTracfinNatureInfractionPenaleService
                                .EditDossierLabNatureInfractionPenaleAsync(
                                    dossierUpdate.DeclarationTracfins.FirstOrDefault()?.Id ?? 0,
                                    natureSoupconInfractionPenaleId,
                                    cancellationToken).ConfigureAwait(false);
                    }

                    var natureSoupconFraudeFiscaleId = dossierUpdate.DeclarationTracfins
                        ?.Select(x => x.NatureSoupconFraudeFiscaleId).FirstOrDefault();
                    if (dossierUpdate.DeclarationTracfins != null && dossierUpdate.DeclarationTracfins.Any())
                    {
                        var fraudeFiscaleEntity = await _labService.DeclarationTracfinNatureFraudeFiscaleService
                            .GetAllByIdAsync(dossierUpdate.DeclarationTracfins.FirstOrDefault()?.Id ?? 0, true,
                                cancellationToken).ConfigureAwait(false);
                        if (natureSoupconFraudeFiscaleId != null || fraudeFiscaleEntity.Any())
                            await _labService.DeclarationTracfinNatureFraudeFiscaleService
                                .EditDeclarationTracfinNatureFraudeFiscaleAsync(
                                    dossierUpdate.DeclarationTracfins.FirstOrDefault()?.Id ?? 0,
                                    natureSoupconFraudeFiscaleId,
                                    cancellationToken).ConfigureAwait(false);
                    }

                    if (dossierUpdate.DossierLabPersonnePhysiques.Any())
                        foreach (var pp in dossierUpdate.DossierLabPersonnePhysiques)
                        {
                            var personnePhysiqueLabId = pp.PersonnePhysiqueLabId;
                            if (personnePhysiqueLabId == 0)
                                int.TryParse(_protector.Unprotect(pp.PersonnePhysiqueLab.CryptedId),
                                    out personnePhysiqueLabId);
                            if (personnePhysiqueLabId == 0)
                                personnePhysiqueLabId = await _labService.PersonnePhysique.GetPersonnePhysiqueId(
                                    pp.PersonnePhysiqueLab.NomNaissance,
                                    pp.PersonnePhysiqueLab.Prenoms,
                                    DateTime.SpecifyKind(pp.PersonnePhysiqueLab.DateNaissance.GetValueOrDefault(),
                                        DateTimeKind.Utc), token: cancellationToken).ConfigureAwait(false);
                        }

                    await _auditTrace.AddAuditEvent(AuditActionType.Update,
                        $"{AuditAction.Lab} Code Unique : {entity.CodeUnique}",
                        $"Id {entity.Id}  CodeUnique {entity.CodeUnique}",
                        cancellationToken).ConfigureAwait(false);


                    return new JsonResult(new
                    {
                        status = true,
                        message = _translator.Common["DossierLabSauvegarde"],
                        cryptedId = exitDossier
                            ? null
                            : _protector.Protect(entity.Id.ToString(CultureInfo.CurrentCulture)),
                        exitDossier = entity.StatutDossierId == (int)StatutDossierLabEnum.AttenteValidation &&
                                      !dossierUpdate.UserPrivilege.IsValideur
                    });
                }
                else
                {
                    return new JsonResult(new
                    {
                        status = false,
                        message = _translator.Common["ErrorMessage"] + " UpdateAsync : " +
                                  _translator.Common["PleaseRefresh"]
                    });
                }
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
                return new JsonResult(new { status = false, message = _translator.Common["ErrorMessage"] });
            }
            finally
            {
                _logger.EndTrace();
            }
        }

        private async Task<JsonResult> AddDossierAsync(DossierLabViewModel dossierAdd,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();

            try
            {
                TryValidateModel(dossierAdd);
                if (dossierAdd.DirectionId != null)
                    dossierAdd.CodeUnique = GetCodeUniqueDossier(dossierAdd.DirectionId.Value);

                var entity = _mapper.Map<DossierLab>(dossierAdd);
                entity.DossierFraude = null;
                entity.DateCreation = DateTime.Now;


                entity.DossierLabPersonnePhysiques?.ForEach(d =>
                {
                    if (d?.PersonnePhysiqueLab != null)
                    {
                        d.PersonnePhysiqueLab.SoundexNomFr =
                            Soundex(d.PersonnePhysiqueLab.NomNaissance, Language.French);
                        d.PersonnePhysiqueLab.SoundexNomEn =
                            Soundex(d.PersonnePhysiqueLab.NomNaissance, Language.English);
                        d.PersonnePhysiqueLab.SoundexNomUsuelFr =
                            Soundex(d.PersonnePhysiqueLab.NomUsuel, Language.French);
                        d.PersonnePhysiqueLab.SoundexNomUsuelEn =
                            Soundex(d.PersonnePhysiqueLab.NomUsuel, Language.English);
                        d.PersonnePhysiqueLab.SoundexPrenomFr =
                            Soundex(d.PersonnePhysiqueLab.Prenoms, Language.French);
                        d.PersonnePhysiqueLab.SoundexPrenomEn =
                            Soundex(d.PersonnePhysiqueLab.Prenoms, Language.English);
                        d.PersonnePhysiqueLab.CodeDossier = entity.CodeUnique;
                        d.PersonnePhysiqueLab.DossierLabId = entity.Id;
                    }
                });

                if (entity.DossierLabPersonneMorales != null)
                    entity.DossierLabPersonneMorales?.ForEach(d =>
                    {
                        if (d?.PersonneMoraleLab != null)
                        {
                            d.PersonneMoraleLab.SoundexRaisonFr =
                                Soundex(d.PersonneMoraleLab.RaisonSociale, Language.French);
                            d.PersonneMoraleLab.SoundexRaisonEn =
                                Soundex(d.PersonneMoraleLab.RaisonSociale, Language.English);
                            d.PersonneMoraleLab.CodeDossier = entity.CodeUnique;
                            d.PersonneMoraleLab.DossierLabId = entity.Id;
                        }
                    });


                if (await _labService.Dossier.AddAsync(entity, cancellationToken).ConfigureAwait(false))
                {
                    var natureSoupconInfractionPenaleId = dossierAdd.DeclarationTracfins
                        ?.Select(x => x.NatureSoupconInfractionPinaleId).FirstOrDefault();
                    if (natureSoupconInfractionPenaleId != null)
                        await _labService.DeclarationTracfinNatureInfractionPenaleService
                            .EditDossierLabNatureInfractionPenaleAsync(entity.DeclarationTracfins.FirstOrDefault()?.Id ?? 0,
                                natureSoupconInfractionPenaleId,
                                cancellationToken).ConfigureAwait(false);

                    var natureSoupconFraudeFiscaleId = dossierAdd.DeclarationTracfins
                        ?.Select(x => x.NatureSoupconFraudeFiscaleId).FirstOrDefault();
                    if (natureSoupconFraudeFiscaleId != null)
                        await _labService.DeclarationTracfinNatureFraudeFiscaleService
                            .EditDeclarationTracfinNatureFraudeFiscaleAsync(
                                entity.DeclarationTracfins.FirstOrDefault()?.Id ?? 0,
                                natureSoupconFraudeFiscaleId,
                                cancellationToken).ConfigureAwait(false);

                    if (dossierAdd.DossierLabPersonnePhysiques.Any())
                        foreach (var pp in dossierAdd.DossierLabPersonnePhysiques)
                        {
                            await _labService.PersonnePhysique.GetPersonnePhysiqueId(
                                pp.PersonnePhysiqueLab.NomNaissance,
                                pp.PersonnePhysiqueLab.Prenoms,
                                DateTime.SpecifyKind(pp.PersonnePhysiqueLab.DateNaissance.GetValueOrDefault(),
                                    DateTimeKind.Utc), token: cancellationToken).ConfigureAwait(false);
                        }

                    var dossierHisto = _mapper.Map<DossierLabHisto>(entity);
                    dossierHisto.DossierLabId = entity.Id;
                    await AuditUpdatedDossier(dossierHisto, 1, cancellationToken).ConfigureAwait(false);
                    await _auditTrace.AddAuditEvent(AuditActionType.Add,
                            $"{AuditAction.Lab} Code Unique : {entity.CodeUnique}",
                            $"Id {entity.Id} CodeUnique {entity.CodeUnique}",
                            cancellationToken)
                        .ConfigureAwait(false);
                    var eventDossier = new EventDossierViewModel
                    {
                        UtilisateurEventId = entity.UtilisateurId,
                        CreateurId = entity.UtilisateurId,
                        DateCreation = DateTime.UtcNow,
                        IsActive = true,
                        ActiviteId = (int)ActiviteModule.Lab,
                        EventDossierTypeId = (int)EactionEventTypeDossier.AJOUT_LAB,
                        DossierLabId = entity.Id,
                        CodeDossier = entity.CodeUnique
                    };
                    if (dossierAdd.StatutDossierId == (int)StatutDossierLabEnum.AttenteValidation)
                        await AddNotificationMailAttenteValidationDossier(entity.CodeUnique,
                            entity.DirectionId, cancellationToken).ConfigureAwait(false);

                    await _labService.EventDossier.AddAsync(_mapper.Map<EventDossier>(eventDossier), cancellationToken)
                        .ConfigureAwait(false);

                    var exitDossier = dossierAdd.StatutDossierId == (int)StatutDossierLabEnum.Cloture
                                      || dossierAdd.StatutDossierId == (int)StatutDossierLabEnum.SansSuite
                                      || dossierAdd.StatutDossierId == (int)StatutDossierLabEnum.AttentePriseEnCharge
                                      || (dossierAdd.StatutDossierId == (int)StatutDossierLabEnum.AttenteValidation &&
                                          !dossierAdd.UserPrivilege.IsValideur);


                    return new JsonResult(new
                    {
                        status = true,
                        message = _translator.Common["DossierLabCree"],
                        cryptedId = exitDossier
                            ? null
                            : _protector.Protect(entity.Id.ToString(CultureInfo.CurrentCulture)),
                        exitDossier
                    });
                }
                else
                {
                    return new JsonResult(new { status = false, message = _translator.Common["ErrorMessage"] });
                }
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
                return new JsonResult(new { status = false, message = _translator.Common["ErrorMessage"] });
            }
            finally
            {
                _logger.EndTrace();
            }
        }

        private async Task AuditUpdatedDossier(DossierLabHisto dossierHisto, int action,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            try
            {
                dossierHisto.ActionId = action;
                dossierHisto.Id = 0;
                await _labService.DossierHisto.AddAsync(dossierHisto, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }
            finally
            {
                _logger.EndTrace();
            }
        }

        private string GetCodeUniqueDossier(int directionId)
        {
            var direction = _referential.Directions.FirstOrDefault(x => x.Id == directionId)
                ?.Abreviation
                .ToUpper(CultureInfo.CurrentCulture);
            var id = _labService.Dossier.GetDossierOrder(direction, directionId,
                currentUser.Initiales.ToUpper(CultureInfo.CurrentCulture));

            return
                $"{id}-{direction?.ToUpper(CultureInfo.CurrentCulture)}-{currentUser.Initiales.ToUpper(CultureInfo.CurrentCulture)}-{DateTimeOffset.Now:MMyy}";
        }

        public IActionResult AddActionForm(int order)
        {
            var item = new DossierLabActionViewModel
            {
                CreateurId = currentUser.Id,
                Createur_FullName = currentUser.FullName,
                DateCreation = DateTime.Now
            };
            item.ProtectDossierLabAction(_protector);
            return PartialView("/Areas/Lab/Views/Dossier/_PartialAction.cshtml",
                new Tuple<DossierLabActionViewModel, int>(item, order));
        }


        public IActionResult AddOperationEnCoursForm(int order)
        {
            var item = new OperationEnCoursDeclarationTracfinViewModel();

            var currentLanguage = this.GetCurrentLanguage()
                .ToLower(CultureInfo.CurrentCulture);

            item.Init(currentLanguage, true);

            return PartialView("/Areas/Lab/Views/Dossier/_PartialOperationEnCoursDeclaration.cshtml",
                new Tuple<OperationEnCoursDeclarationTracfinViewModel, int>(item, order));
        }

        public IActionResult AddOperationSuspectForm(int order, int? professionId = null)
        {
            var item = new OperationSuspectDeclarationTracfinViewModel();

            var currentLanguage = this.GetCurrentLanguage()
                .ToLower(CultureInfo.CurrentCulture);

            item.Init(professionId, currentLanguage, true);

            return PartialView("/Areas/Lab/Views/Dossier/_PartialOperationSuspectDeclaration.cshtml",
                new Tuple<OperationSuspectDeclarationTracfinViewModel, int>(item, order));
        }

        public IActionResult AddAttachmentForm(int order, bool estTransmissionArNew)
        {
            var item = new AttachmentViewModel
            {
                Culture = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture),
                EstTransmissionUpdateNew = estTransmissionArNew
            };
            return PartialView("/Areas/Lab/Views/Dossier/_PartialAttachment.cshtml",
                new Tuple<AttachmentViewModel, int>(item, order));
        }

        public IActionResult AddNewAttachmentForm(int order, bool isDs)
        {
            var item = new AttachmentViewModel
            {
                Culture = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture),
                IsDs = isDs
            };
            return PartialView("/Areas/Lab/Views/Dossier/_PartialEditAttachment.cshtml",
                new Tuple<AttachmentViewModel, int>(item, order));
        }

        [HttpPost]
        public IActionResult EditAttachmentForm(AttachmentViewModel item, int order, bool isDs)
        {
            item.Culture = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture);
            item.IsDs = isDs;

            return PartialView("/Areas/Lab/Views/Dossier/_PartialEditAttachment.cshtml",
                new Tuple<AttachmentViewModel, int>(item, order));
        }

        [HttpGet]
        public IActionResult AddFormAttachmentsCollectionItem(int order, string cryptedDossierId)
        {
            return PartialView("/Areas/Lab/Views/Dossier/_PartialFormAttachmentsCollectionItem.cshtml",
                new Tuple<DocumentDossierLabViewModel, int>(
                    new DocumentDossierLabViewModel { CryptedDossierLabId = cryptedDossierId }, order));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddDelegatedUsers(DelegationLabViewModel model, int directionId,
            IList<int> userIds, int? userAffectId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            try
            {
                var delegates = new List<DelegationViewModel>();
                if (!IsVerifyUserHabilitation())
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["PasDePersmissionActiviteDirection"]
                    });

                var utilisateurId = userAffectId.HasValue && userAffectId != 0 ? userAffectId.Value : currentUser.Id;

                delegates.AddRange(userIds.Select(id => new DelegationViewModel
                {
                    UtilisateurId = utilisateurId,
                    ActiviteId = (int)ActiviteModule.Lab,
                    DirectionId = directionId,
                    DelegueId = id
                }));
                var delegationsMp = _mapper.Map<List<Delegation>>(delegates);
                await _labService.Delegation.AddRangeAsync(delegationsMp, cancellationToken).ConfigureAwait(false);

                await _auditTrace.AddAuditEvent(AuditActionType.Add,
                        nameof(AuditAction.Delegation),
                        null,
                        cancellationToken)
                    .ConfigureAwait(false);

                return Json(new ResponseViewModel<bool> { Response = true, Status = true });
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
                return Json(new ResponseViewModel<bool> { Response = false, Status = false });
            }
            finally
            {
                _logger.EndTrace();
            }
        }

        public IActionResult AddPersonneMoraleForm(int order, int? directionId)
        {
            var item = new DossierLabPersonneMoraleViewModel
            {
                EVisibleDossier = EVisibleDossier.Editable,
                Culture = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture),
                IsNew = true,
                DirectionId = directionId
            };
            item.ProtectDossierLabPersonneMorale(_protector);
            return PartialView("/Areas/Lab/Views/Dossier/_PartialPersonneMoraleLab.cshtml",
                new Tuple<DossierLabPersonneMoraleViewModel, int>(item, order));
        }

        public IActionResult AddPersonnePhysiqueForm(int order, int? directionId)
        {
            var item = new DossierLabPersonnePhysiqueViewModel
            {
                EVisibleDossier = EVisibleDossier.Editable,
                Culture = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture),
                IsNew = true,
                DirectionId = directionId
            };
            item.ProtectDossierLabPersonnePhysique(_protector);

            return PartialView("/Areas/Lab/Views/Dossier/_PartialPersonnePhysiqueLab.cshtml",
                new Tuple<DossierLabPersonnePhysiqueViewModel, int>(item, order));
        }

        public IActionResult AddNonConnu(int order, int? directionId)
        {
            var item = new DossierLabNonConnueViewModel
            {
                EVisibleDossier = EVisibleDossier.Editable,
                Culture = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture),
                IsNew = true,
                DirectionId = directionId
            };
            item.ProtectDossierLabNonConnu(_protector);
            return PartialView("/Areas/Lab/Views/Dossier/_PartialNonConnuLab.cshtml",
                new Tuple<DossierLabNonConnueViewModel, int>(item, order));
        }

        public async Task<JsonResult> FindExistPersonneMorale(string raisonSociale, string numerocompte,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();

            var personne = new PersonneMoraleResultLab();

            try
            {
                personne = await _labService.Dossier
                               .FindPersonneMoraleAsync(new PersonneMoraleSearchCriteria
                               { RaisonSociale = raisonSociale, NumeroImmatriculation = numerocompte },
                                   cancellationToken)
                               .ConfigureAwait(false) ??
                           new PersonneMoraleResultLab();
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();
            return new JsonResult(new { id = personne.Id });
        }

        public async Task<JsonResult> FindExistPersonnePhysique(string nomNaissance, string prenoms,
            DateTime dateNaissance, string numerocompte, CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            var personne = new PersonnePhysiqueResultLab();

            try
            {
                personne = await
                               _labService.Dossier
                                   .FindPersonnePhysiqueAsync(new PersonnePhysiqueSearchCriteria
                                   {
                                       NomNaissance = nomNaissance,
                                       Prenoms = prenoms,
                                       DateNaissance = dateNaissance
                                   },
                                       cancellationToken)
                                   .ConfigureAwait(false) ??
                           new PersonnePhysiqueResultLab();
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();
            return new JsonResult(new { id = personne.Id });
        }

        [HttpGet]
        public async Task<JsonResult> GetComboBoxListsByDirection(int directionId, bool isReadOnly,
            bool? onlyActive = true,
            string cryptedDossierId = null, CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            List<SelectedItem> entiteLabs;
            List<SelectedItemTypeClient> typeClientLabs;
            List<CategorieLabViewModel> categorieLabs;
            List<SecteurEconomiqueLabViewModel> secteurEconomiqueLabs;

            if (!IsVerifyUserHabilitation())
                return new JsonResult(new
                {
                    success = false,
                    status = false,
                    message = _translator.Common["PasDePersmissionActiviteDirection"]
                });

            int? origineId = null;
            if (cryptedDossierId != null)
                if (int.TryParse(_protector.Unprotect(cryptedDossierId), out var dossierId) && dossierId > 0)
                    origineId = await _labService.Dossier.GetOrigineDossierLabs(dossierId).ConfigureAwait(false);
            var origineList = await _labService.OrigineLab
                .GetOrigineLabByDirectionId(directionId, origineId, cancellationToken)
                .ConfigureAwait(false);
            var origineLabs = _mapper.Map<List<OrigineLabViewModel>>(origineList);
            var isSanction =
                _labService.UtilisateurDirectionService.IsSANCTIONOnDirection(currentUser.Id, directionId,
                    (int)ActiviteModule.Lab);
            try
            {
                var entiteLabsAsync =
                    (await _labService.Referentiel
                        .GetEntitesLabByDirectionAsync(directionId, onlyActive.GetValueOrDefault(), cancellationToken)
                        .ConfigureAwait(false)).OrderBy(x => x.Lisp).ToList();
                entiteLabs = _mapper.Map<List<SelectedItem>>(entiteLabsAsync);

                var typeClientLabsAsync =
                    (await _labService.Referentiel
                        .GetTypeClientLabsByDirectionAsync(directionId, onlyActive.GetValueOrDefault(),
                            cancellationToken).ConfigureAwait(false)).OrderBy(x => x.FrenchName).ToList();
                typeClientLabs = _mapper.Map<List<SelectedItemTypeClient>>(typeClientLabsAsync);

                var categorieLabsAsync =
                    (await _labService.Referentiel
                        .GetCategorieLabsByDirectionAsync(directionId, onlyActive.GetValueOrDefault(), isSanction,
                            isReadOnly, cancellationToken).ConfigureAwait(false)).OrderBy(x => x.FrenchName).ToList();
                categorieLabs = _mapper.Map<List<CategorieLabViewModel>>(categorieLabsAsync);

                var secteurEconomiqueLabsAsync =
                    (await _labService.Referentiel
                        .GetSecteurEconomiqueByDirectionAsync(directionId, onlyActive.GetValueOrDefault(),
                            cancellationToken).ConfigureAwait(false)).OrderBy(x => x.FrenchName).ToList();
                secteurEconomiqueLabs = _mapper.Map<List<SecteurEconomiqueLabViewModel>>(secteurEconomiqueLabsAsync);
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
                return new JsonResult(new { status = false });
            }

            _logger.EndTrace();
            return new JsonResult(new
            { status = true, entiteLabs, typeClientLabs, categorieLabs, origineLabs, secteurEconomiqueLabs });
        }

        [HttpGet]
        public Task<IEnumerable<SelectedItem>> GetCategorieLabByDirection(int directionId,
            bool? onlyActive = true)
        {
            _logger.BeginTrace();
            IEnumerable<SelectedItem> models = null;
            try
            {
                if (!IsVerifyUserHabilitation())
                    return Task.FromResult<IEnumerable<SelectedItem>>(null);

                if (onlyActive.GetValueOrDefault())
                {
                    var results = _referential.CategorieLabs.Where(x => x.DirectionId == directionId && x.IsActive);
                    models = _mapper.Map<List<SelectedItem>>(results);
                }
                else
                {
                    var results = _referential.CategorieLabs.Where(x => x.DirectionId == directionId);
                    models = _mapper.Map<List<SelectedItem>>(results);
                }
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();

            return Task.FromResult(models);
        }

        [HttpGet]
        public Task<IEnumerable<SelectedItem>> GetOrigineLabByDirection(int directionId, bool? onlyActive = true)
        {
            _logger.BeginTrace();
            IEnumerable<SelectedItem> models = null;
            try
            {
                if (!IsVerifyUserHabilitation())
                    return Task.FromResult<IEnumerable<SelectedItem>>(null);

                if (onlyActive.GetValueOrDefault())
                {
                    var results = _referential.OrigineLabs.Where(x => x.DirectionId == directionId && x.IsActive);
                    models = _mapper.Map<List<SelectedItem>>(results);
                }
                else
                {
                    var results = _referential.OrigineLabs.Where(x => x.DirectionId == directionId);
                    models = _mapper.Map<List<SelectedItem>>(results);
                }
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();

            return Task.FromResult(models);
        }

        [HttpGet]
        public async Task<IList<UtilisateurViewModel>> GetConcernedUsers(int directionId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            var models = new List<UtilisateurViewModel>();
            try
            {
                if (!IsVerifyUserHabilitation() || _labService.Referentiel
                        .GetDirectionHabiliteUserReferentiel((int)ActiviteModule.Lab, _userInfoService)
                        .All(x => x.Id != directionId) && !_roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal))
                    return models;

                var utls = await _labService.Referentiel
                    .GetUtilisateursConcernesAsync(currentUser.Id,
                        directionId,
                        (int)ActiviteModule.Lab,
                        cancellationToken)
                    .ConfigureAwait(false);

                models = _mapper.Map<List<UtilisateurViewModel>>(utls);
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();
            return models;
        }

        [HttpGet]
        public async Task<IList<UtilisateurViewModel>> GetDelegatedUsers(int? userAffectId, int directionId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();

            var models = new List<UtilisateurViewModel>();
            try
            {
                if (!IsVerifyUserHabilitation())
                    return models;
                var utilisateurId = userAffectId.HasValue && userAffectId != 0 ? userAffectId.Value : currentUser.Id;
                var delegs = await _labService.Delegation
                    .GetDelegationsByUserId(utilisateurId, directionId, (int)ActiviteModule.Lab, cancellationToken)
                    .ConfigureAwait(false);

                var delegations = _mapper.Map<List<DelegationViewModel>>(delegs);

                models.AddRange(delegations.Select(item => item.Delegue));
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();

            return models;
        }

        [HttpGet]
        public async Task<IActionResult> VisualisationDs(string id)
        {
            var _id = 0;
            if (!string.IsNullOrEmpty(id)) _id = this.Uprotect(id, _protector);

            var dossierLab = await _labService.Dossier.GetAsync(_id, false).ConfigureAwait(false);

            if (dossierLab != null && !IsVerifyUserHabilitation())
                return new JsonResult(new
                {
                    success = false,
                    status = false,
                    message = _translator.Common["PasDePersmissionActiviteDirection"]
                });

            var declarationTracfin =
                await _labService.DeclarationTracfin.GetDeclarationByDossierAsync(_id).ConfigureAwait(false);
            var item = _mapper.Map<DeclarationTracfinViewModel>(declarationTracfin);
            item.Culture = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture);
            item.IsReadOnly = true;
            item.ProtectDossierLabTracfin(_protector);
            return PartialView("/Areas/Lab/Views/Dossier/_PartialVisualisationDeclarationTracfinView.cshtml",
                new Tuple<DeclarationTracfinViewModel, int>(item, 0));
        }

        [HttpGet]
        public FileResult DeclarationTracfinExportXml(string cryptedId, string code)
        {
            var id = 0;
            if (!string.IsNullOrEmpty(cryptedId)) id = this.Uprotect(cryptedId, _protector);
            var dossierLab = _labService.DeclarationTracfin.GetXmlTracFin(id);
            if (dossierLab != null)
            {
                if (!IsVerifyUserHabilitation())
                    return null;

                var fileStreamResult = File(Encoding.UTF8.GetBytes(dossierLab.ToString()), "application/xml",
                    "DS_" + code + "-" + DateTime.Now.ToString("dd-MM-yyy-HHmmss") + ".xml");
                return fileStreamResult;
            }

            return File("", "application/xml", "DS_" + code + "-" + DateTime.Now.ToString("dd-MM-yyy-HHmmss") + ".xml");
        }

        [HttpGet]
        public async Task<FileResult> DeclarationV2TracfinExportXml(string cryptedId, string code)
        {
            if (!string.IsNullOrEmpty(cryptedId))
            {
                var id = this.Uprotect(cryptedId, _protector);

                var dossierLab = await _labService.Dossier.GetAsync(id, false).ConfigureAwait(false);
                if (dossierLab.DsFileEnvoiId.HasValue)
                {
                    var dossierLabFile = await _labService.Dossier.GetDsDocumentAsync(dossierLab.DsFileEnvoiId.Value)
                        .ConfigureAwait(false);

                    if (dossierLabFile.FileContent is { Length: > 100 })
                    {
                        if (!IsVerifyUserHabilitation())
                            return null;

                        var fileStreamResult = File(dossierLabFile.FileContent, "application/xml",
                            "DS_" + code + "-" + DateTime.Now.ToString("dd-MM-yyy-HHmmss") + ".xml");
                        return fileStreamResult;
                    }
                }
            }

            return null;
        }

        [HttpGet]
        public async Task<bool> HasDeclarationV2TracfinExportXml(string cryptedId, string code)
        {
            var id = 0;
            if (!string.IsNullOrEmpty(cryptedId)) id = this.Uprotect(cryptedId, _protector);
            var dossierLab = await _labService.Dossier.GetAsync(id, false).ConfigureAwait(false);
            if (dossierLab.DsFileEnvoiId == null) return false;
            var dossierLabFile = await _labService.Dossier.GetDsDocumentAsync(dossierLab.DsFileEnvoiId.Value)
                .ConfigureAwait(false);

            if (dossierLabFile.FileContent is { Length: > 100 })
            {
                return IsVerifyUserHabilitation();
            }

            return false;
        }

        [HttpPost]
        public async Task<IActionResult> SearchDossiersLab(LabSearchCriteria searchCriteria,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            if (searchCriteria == null)
                return new JsonResult(new
                {
                    success = false,
                    errorMessage = _translator.Common["ProblèmeSurLesCritèresDeRecherche"]
                });
            HttpContext.Session.Set("LabSearchCriteria", searchCriteria);
            if (!_roleAccessUser.HasActivites(ActiviteModule.Lab))
                return RedirectToAction("Login", "Account");
            //Bug : IsManagementMode == false => ModelState.IsValid = false;
            if (!ModelState.IsValid && searchCriteria.IsManagementMode)
                return new JsonResult(new
                { status = false, message = _translator.Common["ProblèmeSurLesCritèresDeRecherche"] });
            if (searchCriteria == null)
                throw new ArgumentNullException(nameof(searchCriteria));
            //var isManagementMode = searchCriteria.EditionMode == EditionMode.Management;

            var statutPendingIdList = new List<int>
            {
                (int)StatutDossierLabEnum.AttentePriseEnCharge,
                (int)StatutDossierLabEnum.PendingEnCours
            };

            switch (searchCriteria.EditionMode)
            {
                case EditionMode.Management:
                    HttpContext.Session.Set("LabManagementSearchCriteria", searchCriteria);
                    break;
                case EditionMode.Viewer:
                    HttpContext.Session.Set("LabViewerSearchCriteria", searchCriteria);
                    break;
                case EditionMode.Reporting:
                    HttpContext.Session.Set("LabReportingSearchCriteria", searchCriteria);
                    break;
                case EditionMode.ReportingTiers:
                    HttpContext.Session.Set("LabReportingTiersSearchCriteria", searchCriteria);
                    break;
                case EditionMode.Tracfin:
                    HttpContext.Session.Set("LabReportingTracfinSearchCriteria", searchCriteria);
                    break;
                case EditionMode.QLB:
                    HttpContext.Session.Set("LabQLBSearchCriteria", searchCriteria);
                    break;
                case EditionMode.Lien:
                    HttpContext.Session.Set("LabLienSearchCriteria", searchCriteria);
                    break;
            }

            if (searchCriteria.EditionMode != EditionMode.ReportingTiers
                && searchCriteria.EditionMode != EditionMode.Reporting &&
                searchCriteria.EditionMode != EditionMode.Tracfin)
            {
                string errorMessage;
                var isRobot =
                    _roleAccessUser.HasRoles(ActiviteModule.Lab, currentUser.DirectionAttacheId, RoleUser.Robot) &&
                    !searchCriteria.IsManagementMode && !_roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal);
                var nbSearchNow = (await _labService.EventDossier.GetEventSearchLabAsync(token: cancellationToken)
                        .ConfigureAwait(false))
                    .Count(x => x.CreateurId == currentUser.Id && x.DateCreation.Date == DateTime.Now.Date);
                var isNotAttentMaxSearch = nbSearchNow < _settings.MaxSearchDay || !isRobot;
                var dossiers = Enumerable.Empty<DossierLabRowViewModel>();
                var result = new List<DossierLabRow>();
                searchCriteria.Langue = this.CurrentLanguage();
                var nbLignes = new Ref<int>();
                var userDelegatedIds = new List<int?>();
                var userPendindConfidentIds = new List<int?>();


                if (!_roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal) &&
                    !_roleAccessUser.HasRoles(ActiviteModule.Lab, null, RoleUser.AuditGlobal))
                    searchCriteria.Start = searchCriteria.Start >= DateTimeOffset.UtcNow.AddYears(-10)
                        ? searchCriteria.Start
                        : DateTimeOffset.UtcNow.AddYears(-10);

                var startDateInit = searchCriteria.Start;
                //var endDateInit = searchCriteria.End;
                var isNotValidModelState =
                    (!ModelState.IsValid || (string.IsNullOrWhiteSpace(searchCriteria.NomRaisonSociale) &&
                                             string.IsNullOrWhiteSpace(searchCriteria.NumeroImmatriculation))) &&
                    isRobot;
                if (!isNotValidModelState && isNotAttentMaxSearch)
                {
                    if (isRobot && !_utilisateurDirectionService.IsExistUserByMailActivite(searchCriteria.MailDemandeur,
                            (int)ActiviteModule.Lab, currentUser.DirectionAttacheId))
                        return new JsonResult(new
                        { length = nbLignes, result = dossiers, error = _translator.Common["MailIsNotValid"] });

                    try
                    {
                        var dateLimt = DateTime.Now.AddYears(-5);
                        var isEtendu = await _labService.UtilisateurDirectionService
                            .IsEtendu(currentUser.Id, cancellationToken).ConfigureAwait(false);
                        var directHbtIds = (await _labService.Referentiel
                            .GetDirectionHabiliteUtilisateur(currentUser.Id, (int)ActiviteModule.Lab, cancellationToken)
                            .ConfigureAwait(false)).Select(x => (int?)x.Id).ToList();
                        var directHbConfidentIds = _labService.Referentiel
                            .GetDirectionHabiliteConfidents((int)ActiviteModule.Lab, currentUser.Id)
                            .Select(x => (int?)x.Id)
                            .ToList();
                        var isUe = await _labService.UtilisateurDirectionService
                            .IsUe(currentUser.Id, (int)ActiviteModule.Lab, cancellationToken).ConfigureAwait(false);

                        var eventSearch = new EventSearchLabViewModel
                        {
                            CreateurId = currentUser.Id,
                            DirectionAttacheId = currentUser.DirectionAttacheId,
                            DateCreation = DateTime.UtcNow
                        };

                        if ((!searchCriteria.IsManagementMode
                             && !string.IsNullOrEmpty(searchCriteria.CodeUnique))
                            || !string.IsNullOrEmpty(searchCriteria.NomRaisonSociale)
                            || !string.IsNullOrEmpty(searchCriteria.NumeroCompte)
                            || !string.IsNullOrEmpty(searchCriteria.NumeroImmatriculation)
                            || !string.IsNullOrEmpty(searchCriteria.Prenom))
                        {
                            eventSearch.NumeroImmatriculation = searchCriteria.NumeroImmatriculation;
                            eventSearch.NumeroCompte = searchCriteria.NumeroCompte;
                            eventSearch.CodeUnique = searchCriteria.CodeUnique;
                            eventSearch.NomRaisonSociale = searchCriteria.NomRaisonSociale;
                            eventSearch.Prenom = searchCriteria.Prenom;
                            if (isRobot) eventSearch.MailDemandeur = searchCriteria.MailDemandeur;
                            var evnt = _mapper.Map<EventSearchLab>(eventSearch);

                            await _labService.EventDossier.AddEventSearchLab(evnt, cancellationToken)
                                .ConfigureAwait(false);
                        }

                        if (searchCriteria.CodeUnique.IsNotNullOrEmpty())
                            searchCriteria.CodeUnique = searchCriteria.CodeUnique?.TrimStart().TrimEnd();
                        if (searchCriteria.NomRaisonSociale.IsNotNullOrEmpty())
                            searchCriteria.NomRaisonSociale = searchCriteria.NomRaisonSociale?.TrimStart().TrimEnd();
                        if (searchCriteria.Prenom.IsNotNullOrEmpty())
                            if (searchCriteria.Prenom != null)
                                searchCriteria.Prenom = searchCriteria.Prenom.TrimStart().TrimEnd();
                        if (searchCriteria.NumeroCompte.IsNotNullOrEmpty())
                            searchCriteria.NumeroCompte = searchCriteria.NumeroCompte?.TrimStart().TrimEnd();
                        if (searchCriteria.NumeroImmatriculation.IsNotNullOrEmpty())
                            searchCriteria.NumeroImmatriculation =
                                searchCriteria.NumeroImmatriculation.TrimStart().TrimEnd();
                        if (searchCriteria.NumberDeclartionAccuseReceptionReferenceInterne.IsNotNullOrEmpty())
                            searchCriteria.NumberDeclartionAccuseReceptionReferenceInterne = searchCriteria
                                .NumberDeclartionAccuseReceptionReferenceInterne.TrimStart().TrimEnd();
                        if (_roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal) ||
                            _roleAccessUser.HasRoles(ActiviteModule.Lab, null, RoleUser.AuditGlobal) ||
                            (isEtendu && !searchCriteria.IsManagementMode))
                        {
                            isEtendu = true;
                            result.AddRange(await _labService.Dossier
                                .FindDossiersOpti(searchCriteria, nbLignes, cancellationToken)
                                .ConfigureAwait(false));
                        }
                        else
                        {
                            if (searchCriteria.IsManagementMode)
                            {
                                var ligneCount = 0;
                                if (!_roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal) &&
                                    !_roleAccessUser.HasRoles(ActiviteModule.Lab, null, RoleUser.AuditGlobal))
                                {
                                    var directionIds =
                                        (await _labService.Referentiel
                                            .GetDirectionHabiliteUtilisateur(currentUser.Id, (int)ActiviteModule.Lab,
                                                cancellationToken).ConfigureAwait(false)).Select(x => (int?)x.Id)
                                        .ToList();
                                    foreach (var dId in directionIds)
                                    {
                                        searchCriteria.StatutDossierLabIds = null;
                                        searchCriteria.UtilisateurIds = null;
                                        var isValideur = await _labService.UtilisateurDirectionService
                                            .IsIsValideurInActivity(currentUser.Id, dId.Value, (int)ActiviteModule.Lab,
                                                cancellationToken).ConfigureAwait(false);
                                        var isPending = await _labService.UtilisateurDirectionService
                                            .IsPendingInActivity(currentUser.Id, dId.Value, (int)ActiviteModule.Lab,
                                                cancellationToken).ConfigureAwait(false);
                                        var isIsole = await _labService.UtilisateurDirectionService.IsIsole(
                                            currentUser.Id,
                                            dId.Value, (int)ActiviteModule.Lab,
                                            cancellationToken).ConfigureAwait(false);
                                        var isAdminReferentiel = _roleAccessUser.HasClaims(ActiviteModule.Lab,
                                            ClaimUser.ADMINREFERENTIEL, dId);
                                        userPendindConfidentIds.AddRange(
                                            (await _labService.UtilisateurDirectionService
                                                .GetUtilisateurPendingConfident(dId.Value, (int)ActiviteModule.Lab,
                                                    cancellationToken).ConfigureAwait(false)).Select(x => (int?)x.Id)
                                            .ToList());
                                        searchCriteria.DirectionIds = searchCriteria.DirectionId.HasValue
                                            ? null
                                            : new List<int?> { dId };
                                        if (!isAdminReferentiel && isValideur)
                                            searchCriteria.StatutDossierLabIds =
                                                searchCriteria.StatutDossierLabId.HasValue
                                                    ? new List<int?> { searchCriteria.StatutDossierLabId }
                                                    : new List<int?>
                                                    {
                                                        (int)StatutDossierLabEnum.AttenteValidation,
                                                        (int)StatutDossierLabEnum.AttentePriseEnCharge
                                                    };
                                        if (isPending || isIsole)
                                            searchCriteria.UtilisateurIds = new List<int?> { currentUser.Id };
                                        if (isAdminReferentiel)
                                        {
                                            //var utilisateurIds =
                                            //    (await _labService.Referentiel
                                            //        .GetUtilisateurInDirectionAllAsync(dId.Value,
                                            //            (int)ActiviteModule.Lab,
                                            //            cancellationToken).ConfigureAwait(false))
                                            //    .Select(x => (int?)x.Id)
                                            //    .ToList();
                                            //searchCriteria.UtilisateurIds = utilisateurIds;
                                        }

                                        if (isValideur || isPending || isAdminReferentiel || isIsole)
                                        {
                                            result.AddRange(await _labService.Dossier
                                                .FindDossiersOpti(searchCriteria, nbLignes, cancellationToken)
                                                .ConfigureAwait(false));
                                        }
                                        else
                                        {
                                            searchCriteria.StatutDossierLabIds =
                                                searchCriteria.StatutDossierLabId.HasValue
                                                    ? new List<int?> { searchCriteria.StatutDossierLabId }
                                                    : new List<int?> { (int)StatutDossierLabEnum.AttentePriseEnCharge };
                                            result.AddRange(await _labService.Dossier
                                                .FindDossiersOpti(searchCriteria, nbLignes, cancellationToken)
                                                .ConfigureAwait(false));
                                        }

                                        ligneCount = nbLignes.Value;
                                    }
                                }

                                if (searchCriteria.UtilisateurId.HasValue)
                                {
                                    searchCriteria.UtilisateurIds = new List<int?>
                                        { searchCriteria.UtilisateurId.Value };
                                    searchCriteria.StatutDossierLabIds = null;
                                    searchCriteria.DirectionIds = null;
                                    result.AddRange(await _labService.Dossier
                                        .FindDossiersOpti(searchCriteria, nbLignes, cancellationToken)
                                        .ConfigureAwait(false));
                                }
                                else
                                {
                                    var utilisateurIds = new List<int?> { currentUser.Id };
                                    userDelegatedIds =
                                        (await _labService.Delegation
                                            .GetDelegationUtilisateurHabiliteActivite(currentUser.Id,
                                                (int)ActiviteModule.Lab, cancellationToken).ConfigureAwait(false))
                                        .Select(x => (int?)x.UtilisateurId).ToList();
                                    utilisateurIds.AddRange(userDelegatedIds);
                                    searchCriteria.UtilisateurIds = utilisateurIds;
                                    searchCriteria.StatutDossierLabIds = null;
                                    searchCriteria.DirectionIds = null;
                                    result.AddRange(await _labService.Dossier
                                        .FindDossiersOpti(searchCriteria, nbLignes, cancellationToken)
                                        .ConfigureAwait(false));
                                }

                                nbLignes.Value = ligneCount + nbLignes.Value;
                            }
                            else
                            {
                                if (isRobot)
                                {
                                    searchCriteria.SelectedTypeDate = SearchByDateCriteria.Creation;
                                    searchCriteria.Start = DateTime.Now.AddYears(-5);
                                    searchCriteria.End = DateTime.Now;
                                }

                                if (searchCriteria.UtilisateurId.HasValue)
                                    searchCriteria.UtilisateurIds = new List<int?>
                                        { searchCriteria.UtilisateurId.Value };
                                var directAccessibleIds =
                                    (await _labService.Referentiel
                                        .GetDirectionWithAccessibles((int)ActiviteModule.Lab, currentUser.Id,
                                            cancellationToken).ConfigureAwait(false)).Select(x => (int?)x.Id).ToList();

                                if (searchCriteria.DirectionId.HasValue)
                                {
                                    searchCriteria.DirectionIds = new List<int?> { searchCriteria.DirectionId.Value };
                                    result.AddRange(await _labService.Dossier
                                        .FindDossiersOpti(searchCriteria, nbLignes, cancellationToken)
                                        .ConfigureAwait(false));
                                }
                                else
                                {
                                    if (searchCriteria.Start.DateTime.Date >= dateLimt)
                                    {
                                        searchCriteria.DirectionIds = directAccessibleIds.Distinct().ToList();
                                        result.AddRange(await _labService.Dossier
                                            .FindDossiersOpti(searchCriteria, nbLignes, cancellationToken)
                                            .ConfigureAwait(false));
                                    }
                                    else
                                    {
                                        if (searchCriteria.End.DateTime > dateLimt)
                                        {
                                            searchCriteria.DirectionIds = directAccessibleIds.Distinct().ToList();
                                            searchCriteria.Start = dateLimt;
                                            result.AddRange(await _labService.Dossier
                                                .FindDossiersOpti(searchCriteria, nbLignes, cancellationToken)
                                                .ConfigureAwait(false));
                                        }


                                        searchCriteria.Start = startDateInit;
                                        searchCriteria.DirectionIds = directHbtIds;
                                        result.AddRange(await _labService.Dossier
                                            .FindDossiersOpti(searchCriteria, nbLignes, cancellationToken)
                                            .ConfigureAwait(false));
                                    }
                                }

                                result = result.Where(x =>
                                    !(x.StatutDossierId == 1 && !directHbtIds.Contains(x.DirectionId))).ToList();
                                result = result
                                    .Where(x => !(!x.IsParatageCategory && !directHbtIds.Contains(x.DirectionId)))
                                    .ToList();

                                result = result.Where(x =>
                                    !(statutPendingIdList.Contains(x.StatutDossierId) &&
                                      !directHbtIds.Contains(x.DirectionId))).ToList();
                            }
                        }

                        var catgGrpUeps = new List<int> { 3, 4, 5, 6, 7 };

                        var catgGrpVps = new List<int> { 1, 2, 8, 9, 10, 11 };

                        if (result.Any())
                        {
                            result = result.GroupBy(x => x.Id).Select(s => s.FirstOrDefault()).ToList();
                            var dossierConfidents = result.Where(d => d.Confidentiel).ToList();
                            var directionConfidentIds =
                                dossierConfidents.GroupBy(g => g.DirectionId).Select(s => s.Key).ToList();
                            var confidentMessages = new List<ConfidentielViewModel>();
                            foreach (var directId in directionConfidentIds)
                            {
                                var messcfd = _mapper.Map<ConfidentielViewModel>(await _labService.Referentiel
                                    .GetConfidentialMessageLab(directId, cancellationToken).ConfigureAwait(false));
                                if (messcfd != null) confidentMessages.Add(messcfd);
                            }

                            result = result
                                .Where(x =>
                                    !(x.IsConfidentHiden && searchCriteria.EditionMode == EditionMode.Management))
                                .OrderByDescending(x => x.DateSaisie).ToList();

                            dossiers = _mapper.Map<IEnumerable<DossierLabRowViewModel>>(
                                result.OrderByDescending(x => x.DateSaisie));

                            if (isRobot)
                                return new JsonResult(new
                                { length = new Ref<int>(dossiers.Count()), result = dossiers, status = true });

                            foreach (var dos in dossiers)
                            {
                                dos.TempsTraitementDossier = GetDureeTraitement(dos);
                                if (userPendindConfidentIds.Contains(dos.UtilisateurId) && dos.StatutDossierId == 1)
                                    dos.IsPendingConfident = true;


                                if (!searchCriteria.IsManagementMode &&
                                    !(_roleAccessUser.HasRoles(ActiviteModule.Lab, null, RoleUser.AdminGlobal,
                                        RoleUser.AuditGlobal) || isEtendu) &&
                                    !directHbtIds.Contains(dos.DirectionId) &&
                                    (!(isUe && catgGrpVps.Contains(dos.CategorieGroupeLabId)) ||
                                     catgGrpUeps.Contains(dos.CategorieGroupeLabId)))
                                {
                                    dos.AvisSF = dos.VisaSF = dos.Categorie = dos.Origine = dos.CategorieGroupeLab =
                                        dos.OrigineGroupeLab = dos.Entite = dos.Secteur = null;
                                    dos.IsDeclarationSoupcon = dos.IsTransmissionParquet = null;
                                    dos.ReferenceInterne = dos.NumeroAccuseReception = dos.DeclarationNumber = null;
                                    dos.TempsTraitementDossier = null;
                                }

                                if (dos.Confidentiel &&
                                    !_roleAccessUser.HasRoles(ActiviteModule.Lab, null, RoleUser.AdminGlobal,
                                        RoleUser.AuditGlobal) &&
                                    !dos.IsPendingConfident &&
                                    !directHbConfidentIds.Contains(dos.DirectionId))
                                {
                                    if (!(userDelegatedIds.Contains(dos.UtilisateurId) ||
                                          dos.UtilisateurId == currentUser.Id))
                                    {
                                        dos.IsConfidentHiden = true;
                                        var con =
                                            confidentMessages.FirstOrDefault(x => x.DirectionId == dos.DirectionId);
                                        if (con != null && !string.IsNullOrEmpty(con.Message))
                                        {
                                            dos.MessageConfidentiel = $"{con.Message}";
                                            dos.Statut = $"{con.Message}";
                                        }
                                        else
                                        {
                                            dos.Statut = string.Empty;
                                        }

                                        var codes = dos.CodeUnique.Split('-');
                                        dos.CodeUnique = codes != null && codes.Length > 2
                                            ? $"{codes[0]}-{codes[3]}"
                                            : string.Empty;
                                        dos.TierSearchRows = new List<TierSearchRow>
                                            { new() { Description = string.Empty } };
                                        dos.Entite = dos.Origine = dos.ResponsableDossier = dos.Modificateur =
                                            dos.Utilisateur =
                                                dos.AvisSF = dos.Secteur = dos.NumeroAccuseReception = null;
                                        dos.Direction = dos.Categorie = dos.VisaSF = dos.OrigineGroupeLab =
                                            dos.CategorieGroupeLab
                                                = dos.NumeroCompteTier
                                                    = dos.ReferenceInterne
                                                        = null;
                                        dos.TempsTraitementDossier = null;
                                        dos.DeclarationNumber = null;
                                        dos.IsTransmissionParquet = null;
                                        dos.DateSaisie = dos.DateReception =
                                            dos.DateModification = dos.DateCloture = null;
                                        dos.Id = dos.UtilisateurId = 0;
                                        dos.TierSearchRows = null;
                                    }
                                    else
                                    {
                                        dos.IsConfidentHiden = false;
                                    }
                                }

                                dos.ProtectDossierLabItem(_protector);
                            }

                            switch (searchCriteria.SelectedTypeDate)
                            {
                                case SearchByDateCriteria.Creation:
                                    dossiers = dossiers.OrderByDescending(x => x.DateSaisie)
                                        .ThenByDescending(x => x.DateModification).ToList();
                                    break;
                                case SearchByDateCriteria.Closing:
                                    dossiers = dossiers.OrderByDescending(x => x.DateCloture)
                                        .ThenByDescending(x => x.DateModification).ToList();
                                    break;
                                case SearchByDateCriteria.Reception:
                                    dossiers = dossiers.OrderByDescending(x => x.DateReception)
                                        .ThenByDescending(x => x.DateModification).ToList();
                                    break;
                                default:
                                    dossiers = dossiers.OrderByDescending(x => x.DateSaisie)
                                        .ThenByDescending(x => x.DateModification).ToList();
                                    break;
                            }
                        }
                    }
                    catch (OperationCanceledException oce)
                    {
                        _logger.TraceError(oce);
                        return new JsonResult(new
                        {
                            status = false,
                            success = false,
                            message = "TIMEOUT"
                        });
                    }
                    catch (SqlException ex)
                    {
                        // Gestion spécifique de l'erreur de timeout
                        errorMessage =
                            "La recherche n'est pas abouti. Veuillez réessayer. (Une erreur de timeout s'est produite lors de l'exécution de la requête SQL.)";
                        Console.WriteLine(errorMessage);
                        Console.WriteLine($@"Message d'erreur : {ex.Message}");

                        // Actions spécifiques pour gérer le timeout
                        // Par exemple : journalisation, nouvelle tentative, notification, etc.

                        // Vous pouvez également accéder à l'exception interne Win32Exception si nécessaire
                        if (ex.InnerException is Win32Exception win32Ex)
                            Console.WriteLine(
                                $@"Erreur Win32 sous-jacente : {win32Ex.NativeErrorCode} - {win32Ex.Message}");
                        return new JsonResult(new
                        {
                            length = nbLignes,
                            result = dossiers,
                            error = errorMessage,
                            status = false,
                            message = "TIMEOUT"
                        });
                    }
                    catch (Exception e)
                    {
                        errorMessage = "La recherche n'est pas abouti. Veuillez réessayer.";
                        _logger.TraceError(e);
                        return new JsonResult(new
                        { length = nbLignes, result = dossiers, error = errorMessage, status = false });
                    }
                }
                else
                {
                    if (!isNotValidModelState) errorMessage = _translator.Common["MaxSearchToDay"];
                    else if (!ModelState.IsValid) errorMessage = _translator.Common["MailIsNotValid"];
                    else errorMessage = _translator.Common["NomRaisonSocialeObligatoire"];

                    return new JsonResult(new
                    { length = nbLignes, result = dossiers, error = errorMessage, status = false });
                }

                _logger.EndTrace();
                if (nbLignes.Value == 0) nbLignes.Value = dossiers.Count();
                return new JsonResult(new { length = nbLignes, result = dossiers, status = true });
            }

            return await SearchDossiersLabReporting(searchCriteria, cancellationToken).ConfigureAwait(false);
        }

        private static int? GetDureeTraitement(DossierLabRowViewModel dossier)
        {
            var count = 0;
            var dateEnd = dossier.IsDeclarationSoupcon.GetValueOrDefault() && dossier.DateDeclaration != null
                ? dossier.DateDeclaration.GetValueOrDefault()
                : dossier.DateSaisie ?? DateTime.Now;
            var dateStart = dossier.DateReception.GetValueOrDefault();
            var days = dateEnd.Subtract(dateStart);
            for (var i = 0; i < days.Days; i++)
            {
                count++;
                dateStart = dateStart.AddDays(1.0);
            }

            return count;
        }

        public async Task<IActionResult> SearchDossiersLabReporting(LabSearchCriteria searchCriteria,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            if (!_roleAccessUser.HasActivites(ActiviteModule.Lab))
                return RedirectToAction("Login", "Account");
            if (searchCriteria == null)
                throw new ArgumentNullException(nameof(searchCriteria));
            var dossiers = new List<DossierLabRowViewModel>();
            var result = new List<DossierLabRow>();
            var nbLignes = new Ref<int>();
            var isflge = _roleAccessUser.HasClaims(ActiviteModule.Lab, ClaimUser.READ) &&
                         _roleAccessUser.HasClaims(ActiviteModule.Lab, ClaimUser.WRITE) &&
                         _roleAccessUser.IsFlgeDirection() &&
                         searchCriteria.EditionMode == EditionMode.Reporting;
            searchCriteria.Langue = this.CurrentLanguage();
            try
            {
                var isEtendu = await _labService.UtilisateurDirectionService.IsEtendu(currentUser.Id, cancellationToken)
                    .ConfigureAwait(false);
                if (_roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal) ||
                    _roleAccessUser.HasRoles(ActiviteModule.Lab, null, RoleUser.AuditGlobal) ||
                    (isEtendu && !searchCriteria.IsManagementMode))
                {
                    if (searchCriteria.EditionMode == EditionMode.Reporting)
                        result = await _labService.Dossier
                            .FindReportingDossiers(searchCriteria, nbLignes, cancellationToken).ConfigureAwait(false);
                    else if (searchCriteria.EditionMode == EditionMode.Tracfin)
                        result = await _labService.Dossier
                            .FindReportingTracfinDossiers(searchCriteria, nbLignes, cancellationToken)
                            .ConfigureAwait(false);
                    else if (searchCriteria.EditionMode == EditionMode.ReportingTiers)
                        result = await _labService.Dossier
                            .FindReportingTierDossiers(searchCriteria, nbLignes, cancellationToken)
                            .ConfigureAwait(false);
                }
                else
                {
                    var directHbtIds = isflge
                        ? _labService.Referentiel.GetDirectionsFlge((int)ActiviteModule.Lab, _userInfoService)
                            .Select(x => (int?)x.Id).ToList()
                        : _labService.Referentiel
                            .GetDirectionHabiliteUser((int)ActiviteModule.Lab, _userInfoService).Select(x => (int?)x.Id)
                            .ToList();

                    if (searchCriteria.UtilisateurId.HasValue)
                        searchCriteria.UtilisateurIds = new List<int?> { searchCriteria.UtilisateurId.Value };
                    searchCriteria.DirectionIds = directHbtIds.Distinct().ToList();

                    if (searchCriteria.EditionMode == EditionMode.ReportingTiers)
                        result = await _labService.Dossier
                            .FindReportingTierDossiers(searchCriteria, nbLignes, cancellationToken)
                            .ConfigureAwait(false);
                    else if (searchCriteria.EditionMode == EditionMode.Reporting)
                        result = await _labService.Dossier
                            .FindReportingDossiers(searchCriteria, nbLignes, cancellationToken).ConfigureAwait(false);
                    else if (searchCriteria.EditionMode == EditionMode.Tracfin)
                        result = await _labService.Dossier
                            .FindReportingTracfinDossiers(searchCriteria, nbLignes, cancellationToken)
                            .ConfigureAwait(false);

                    result = result.Where(x =>
                        !((x.StatutDossierId == 1 && !directHbtIds.Contains(x.DirectionId)) ||
                          (!x.IsParatageCategory && !directHbtIds.Contains(x.DirectionId)))).ToList();
                }

                if (result.Any())
                    dossiers = _mapper.Map<List<DossierLabRowViewModel>>(result.OrderByDescending(x => x.DateSaisie));
            }
            catch (OperationCanceledException oce)
            {
                _logger.TraceError(oce);
                return new JsonResult(new
                {
                    status = false,
                    success = false,
                    Message = "TIMEOUT"
                });
            }
            catch (SqlException ex)
            {
                _logger.TraceError(ex);
                return new JsonResult(new
                {
                    Status = false,
                    success = false,
                    Message = "TIMEOUT"
                });
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();
            return new JsonResult(new { length = nbLignes, result = dossiers });
        }

        [HttpPost]
        public async Task<IActionResult> SearchReportings(ReportingDossierLabViewModel searchCriteria, bool isAnneeEnCours, bool isAnneePrecedente,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            if (!_roleAccessUser.HasActivites(ActiviteModule.Lab))
                return RedirectToAction("Login", "Account");
            if (searchCriteria == null)
                throw new ArgumentNullException(nameof(searchCriteria));
            searchCriteria.Lang = CultureInfo.CurrentCulture.Name;
            var dossiers = new List<DossierLabQLBResultViewModel>();
            try
            {
                if (searchCriteria.DefaultDirectionId > 0)
                {
                    var year = DateTime.Now.Year;
                    if (isAnneeEnCours)
                    {

                        searchCriteria.End = new DateTime(year, DateTime.Now.Month, DateTime.Now.Day);
                        searchCriteria.Start = new DateTime(year, 1, 1);
                    }
                    if (isAnneePrecedente)
                    {
                        searchCriteria.End = new DateTime(year - 1, 12, 12);
                        searchCriteria.Start = new DateTime(year - 1, 1, 1);
                    }
                    var result = await _labService.DeclarationTracfin.GetQLBDossierDeclarationAsync(
                            searchCriteria.Start,
                            searchCriteria.End, searchCriteria.DefaultDirectionId, searchCriteria.EntiteId,
                            cancellationToken)
                        .ConfigureAwait(false);
                    if (result.Any())
                    {
                        dossiers = _mapper.Map<List<DossierLabQLBResultViewModel>>(result);
                        var sumForYear = result.Where(x => x.TypeCalculCode == TypeCalculEnum.Sum)
                            .GroupBy(y => new
                            {
                                y.Code,
                                y.Caption,
                                y.DateCreation.Year,
                                Quarter = y.DateCreation.Year
                            }).Select(x => new DossierLabQLBResultViewModel
                            {
                                Code = x.Key.Code,
                                Caption = x.Key.Caption,
                                Year = x.Key.Year,
                                Quarter = "TOTAL",
                                Value = x.Sum(y => y.Value)
                            }).ToList();

                        var avgForYear = result.Where(x => x.TypeCalculCode == TypeCalculEnum.Avg)
                            .GroupBy(y => new
                            {
                                y.Code,
                                y.Caption,
                                y.DateCreation.Year,
                                Quarter = y.DateCreation.Year
                            }).Select(x => new DossierLabQLBResultViewModel
                            {
                                Code = x.Key.Code,
                                Caption = x.Key.Caption,
                                Year = x.Key.Year,
                                Quarter = "TOTAL",
                                Value = x.Average(y => y.Value)
                            }).ToList();

                        var pctForYear = result.Where(x =>
                                x.TypeCalculCode == TypeCalculEnum.Pct &&
                                (x.DateCreation.Year == searchCriteria.End.Year ||
                                 x.DateCreation.AddYears(1).Year == searchCriteria.End.Year))
                            .GroupBy(y => new
                            {
                                y.Code,
                                y.Caption
                            }).Select(x => new DossierLabQLBResultViewModel
                            {
                                Code = x.Key.Code,
                                Caption = x.Key.Caption,
                                Year = searchCriteria.End.Year,
                                Quarter = "TOTAL",
                                Value = x.Sum(y => y.Value)
                            }).ToList();

                        var sum = result.Where(x => x.TypeCalculCode == TypeCalculEnum.Sum)
                            .GroupBy(y => new
                            {
                                y.Code,
                                y.Caption,
                                y.DateCreation.Year,
                                Quarter = (y.DateCreation.Month - 1) / 3 + 1
                            }).Select(x => new DossierLabQLBResultViewModel
                            {
                                Code = x.Key.Code,
                                Caption = x.Key.Caption,
                                Year = x.Key.Year,
                                Quarter = "T " + x.Key.Quarter,
                                Value = x.Sum(y => y.Value)
                            }).ToList();
                        var avg = result.Where(x => x.TypeCalculCode == TypeCalculEnum.Avg)
                            .GroupBy(y => new
                            {
                                y.Code,
                                y.Caption,
                                y.DateCreation.Year,
                                Quarter = (y.DateCreation.Month - 1) / 3 + 1
                            }).Select(x => new DossierLabQLBResultViewModel
                            {
                                Code = x.Key.Code,
                                Caption = x.Key.Caption,
                                Year = x.Key.Year,
                                Quarter = "T " + x.Key.Quarter,
                                Value = x.Average(y => y.Value)
                            }).ToList();

                        var pct = result.Where(x => x.TypeCalculCode == TypeCalculEnum.Pct)
                            .GroupBy(y => new
                            {
                                y.Code,
                                y.Caption,
                                y.DateCreation.Year,
                                Quarter = (y.DateCreation.Month - 1) / 3 + 1
                            }).Select(x => new DossierLabQLBResultViewModel
                            {
                                Code = x.Key.Code,
                                Caption = x.Key.Caption,
                                Year = x.Key.Year,
                                Quarter = "T " + x.Key.Quarter,
                                Value = x.Sum(y => y.Value)
                            }).ToList();

                        sum.AddRange(avg);
                        sum.AddRange(avgForYear);
                        sum.AddRange(sumForYear);
                        sum.AddRange(pct);
                        sum.AddRange(pctForYear);
                        dossiers = sum;

                        var qlbCaption = _mapper.Map<List<DossierLabQLBResultViewModel>>(await _labService
                            .DeclarationTracfin.GetQLBDossierDeclarationcaptionAsync(searchCriteria.Lang)
                            .ConfigureAwait(false));
                        var defaultResult = dossiers.FirstOrDefault();
                        qlbCaption.ForEach(x =>
                        {
                            if (defaultResult != null)
                            {
                                x.Quarter = defaultResult.Quarter;
                                x.Year = defaultResult.Year;
                            }
                        });
                        dossiers.AddRange(qlbCaption);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();
            return new JsonResult(new { length = dossiers.Count, result = dossiers.OrderBy(x => x.Code) });
        }

        [HttpGet]
        public async Task<JsonResult> GetEntitesByDirection(int directionId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            List<SelectedItem> entites;

            if (!IsVerifyUserHabilitation())
                return new JsonResult(new { status = false });
            try
            {
                var entiteQuery = (await _labService.Dossier.GetEntitiesByDirection(directionId, cancellationToken)
                    .ConfigureAwait(false)).ToList();
                entites = _mapper.Map<List<SelectedItem>>(entiteQuery);
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
                return new JsonResult(new { status = false });
            }

            _logger.EndTrace();
            return new JsonResult(new { status = true, entites });
        }

        [HttpGet]
        public async Task<IList<UtilisateurViewModel>> GetNotDelegatedUsersLab(int directionId, int? userAffectId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();

            IList<UtilisateurViewModel> models = null;
            try
            {
                if (!IsVerifyUserHabilitation())
                    return null;
                var utilisateurId = userAffectId.HasValue && userAffectId != 0 ? userAffectId.Value : currentUser.Id;

                var delegs = await _labService.Delegation
                    .GetDelegationsByUserId(utilisateurId, directionId, (int)ActiviteModule.Lab, cancellationToken)
                    .ConfigureAwait(false);

                var delegVms = new List<DelegationViewModel>();
                if (delegs != null) delegVms = _mapper.Map<List<DelegationViewModel>>(delegs);

                var allUserInDirections = _mapper.Map<List<UtilisateurViewModel>>(await _labService.Referentiel
                    .GetUtilisateurWithoutSpecificUserAsync(utilisateurId,
                        directionId,
                        (int)ActiviteModule.Lab,
                        cancellationToken)
                    .ConfigureAwait(false));

                models = allUserInDirections.Where(user => !(delegVms != null &&
                                                             delegVms.Any(d => d.DelegueId == user.Id)))
                    .ToList();
            }
            catch (Exception e)
            {
                ViewData["error"] = e.Message;
                _logger.TraceError(e);
            }

            _logger.BeginTrace();

            return models;
        }

        public async Task<PersonneMoraleLabViewModel> GetPersonneMorale(int id,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();

            if (!_roleAccessUser.HasActivites(ActiviteModule.Lab))
                return null;

            PersonneMoraleLabViewModel personneMorale = null;
            try
            {
                var personne =
                    await _labService.PersonneMorale.GetAsync(id, false, cancellationToken).ConfigureAwait(false);
                personneMorale = _mapper.Map<PersonneMoraleLabViewModel>(personne);
                personneMorale.ProtectPersonneMorale(_protector);
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();

            return personneMorale;
        }

        public async Task<PersonnePhysiqueLabViewModel> GetPersonnePhysique(int id,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();

            if (!_roleAccessUser.HasActivites(ActiviteModule.Lab))
                return null;

            PersonnePhysiqueLabViewModel personnePhysique = null;
            try
            {
                var personne =
                    await _labService.PersonnePhysique.GetAsync(id, false, cancellationToken).ConfigureAwait(false);
                personnePhysique = _mapper.Map<PersonnePhysiqueLabViewModel>(personne);
                personnePhysique.ProtectPersonnePhysique(_protector);
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();

            return personnePhysique;
        }

        [HttpGet]
        public async Task<JsonResult> CheckConfidentialUser(int directionId)
        {
            _logger.BeginTrace();
            if (!IsVerifyUserHabilitation())
                return new JsonResult(new
                {
                    success = false,
                    status = false,
                    message = _translator.Common["PasDePersmissionActiviteDirection"]
                });

            var isConfidentialUser = await _labService.UtilisateurDirectionService
                .IsConfidentialInActivity(currentUser.Id, directionId, (int)ActiviteModule.Lab).ConfigureAwait(false);
            return new JsonResult(new { isConfidential = isConfidentialUser });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> RemoveDelegatedUsers(DelegationLabViewModel model, IList<int> delegateIds,
            int? userAffectId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            try
            {
                _logger.BeginTrace();
                if (!IsVerifyUserHabilitation())
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["PasDePersmissionActiviteDirection"]
                    });

                var utilisateurId = userAffectId.HasValue && userAffectId != 0 ? userAffectId.Value : currentUser.Id;

                if (delegateIds == null) return Json(new ResponseViewModel<bool> { Response = false, Status = true });

                foreach (var id in delegateIds)
                {
                    var delegateUser = await _labService.Delegation
                        .GetDelegateAsync(id, utilisateurId, cancellationToken)
                        .ConfigureAwait(false);

                    if (delegateUser == null) continue;

                    await _labService.Delegation.DeleteAsync(delegateUser, cancellationToken).ConfigureAwait(false);

                    await _auditTrace.AddAuditEvent(AuditActionType.Delete,
                            nameof(AuditAction.Delegation),
                            null,
                            cancellationToken)
                        .ConfigureAwait(false);
                }

                return Json(new ResponseViewModel<bool> { Response = true, Status = true });
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
                return Json(new ResponseViewModel<bool> { Response = false, Status = false });
            }
            finally
            {
                _logger.EndTrace();
            }
        }

        [HttpGet]
        public async Task<JsonResult> ReactivateDossiersLab(string cryptedId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            try
            {
                var eventDossier = new EventDossierViewModel
                { DateCreation = DateTime.UtcNow, IsActive = true, ActiviteId = (int)ActiviteModule.Lab };
                var id = this.Uprotect(cryptedId, _protector);
                var dossier = await _labService.Dossier.GetDossierLabAsync(id, cancellationToken).ConfigureAwait(false);

                if (!IsVerifyUserHabilitation())
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["PasDePersmissionActiviteDirection"]
                    });
                dossier.EntiteLab = null;
                dossier.StatutDossier = null;
                dossier.StatutDossierId = (int)StatutDossierLabEnum.EnCours;
                dossier.ModificateurId = currentUser.Id;
                dossier.DateCloture = null;
                dossier.DateModification = DateTimeOffset.Now;

                foreach (var declaration in dossier.DeclarationTracfins)
                    if (declaration.EstNouvelleDeclarationTracfin)
                    {
                        declaration.DateDeclaration = null;
                        declaration.DeclarantId = null;
                        _labService.DeclarationTracfin.UpdateCore(declaration);
                    }

                eventDossier.DossierLabId = id;
                eventDossier.CodeDossier = dossier.CodeUnique;
                eventDossier.CreateurId = currentUser.Id;
                eventDossier.UtilisateurEventId = currentUser.Id;
                eventDossier.EventDossierTypeId = (int)EactionEventTypeDossier.REACTIVATION_LAB;
                _labService.Dossier.UpdateCore(dossier);
                await _labService.EventDossier.AddAsync(_mapper.Map<EventDossier>(eventDossier), cancellationToken)
                    .ConfigureAwait(false);
                var statusDossier = _referential.StatutDossierLabs.FirstOrDefault(x => x.Id == dossier.StatutDossierId);
                var libelleStatus = this.IsCultureFr() ? statusDossier?.NameFr : statusDossier?.NameEn;
                var libelleModificateur = $"{currentUser.Prenom} {currentUser.Nom.ToUpper(CultureInfo.CurrentCulture)}";
                return new JsonResult(new
                {
                    success = true,
                    statusDossier = libelleStatus,
                    modificateur = libelleModificateur,
                    dateModification = DateTime.UtcNow,
                    id = statusDossier?.Id
                });
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();

            return new JsonResult(new
            {
                success = false,
                errorMessage = _translator.Common["PasDePermissionReactiverDossier"]
            });
        }

        [HttpGet]
        public async Task<JsonResult> PrendreEnChargeLab(string cryptedId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            try
            {
                var eventDossier = new EventDossierViewModel
                { DateCreation = DateTime.UtcNow, IsActive = true, ActiviteId = (int)ActiviteModule.Lab };
                var id = this.Uprotect(cryptedId, _protector);
                var dossier = await _labService.Dossier.GetAsync(id, true, cancellationToken).ConfigureAwait(false);
                if (!IsVerifyUserHabilitation())
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["PasDePersmissionActiviteDirection"]
                    });
                dossier.EntiteLab = null;
                dossier.StatutDossier = null;
                dossier.Modificateur = null;
                dossier.Utilisateur = null;
                dossier.StatutDossierId = (int)StatutDossierLabEnum.EnCours;
                dossier.ModificateurId = currentUser.Id;
                dossier.DateModification = DateTimeOffset.Now;
                dossier.UtilisateurId = currentUser.Id;
                eventDossier.DossierLabId = id;
                eventDossier.CodeDossier = dossier.CodeUnique;
                eventDossier.CreateurId = currentUser.Id;
                eventDossier.UtilisateurEventId = currentUser.Id;
                eventDossier.EventDossierTypeId = (int)EactionEventTypeDossier.PRENDRE_CHARGE;
                _labService.Dossier.UpdateCore(dossier);

                await _labService.EventDossier.AddAsync(_mapper.Map<EventDossier>(eventDossier), cancellationToken)
                    .ConfigureAwait(false);
                var statusDossier =
                    _referential.StatutDossierLabs.SingleOrDefault(x => x.Id == dossier.StatutDossierId);
                var libelleStatus = this.IsCultureFr() ? statusDossier?.NameFr : statusDossier?.NameEn;
                var libelleModificateur = $"{currentUser.Prenom} {currentUser.Nom.ToUpper(CultureInfo.CurrentCulture)}";
                var libelleUtilisateur = $"{currentUser.Prenom} {currentUser.Nom.ToUpper(CultureInfo.CurrentCulture)}";

                return new JsonResult(new
                {
                    success = true,
                    statusDossier = libelleStatus,
                    modificateur = libelleModificateur,
                    dateModification = DateTime.UtcNow,
                    Utilisateur = libelleUtilisateur,
                    statutLabId = statusDossier?.Id
                });
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();

            return new JsonResult(new
            {
                success = false,
                errorMessage = _translator.Common["PasDePermissionPrendreEnChargeDossier"]
            });
        }

        public async Task<IActionResult> AllowedUserCommandsManagementMode(string cryptedDossierId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            var allowUpdate = false;
            var allowDelete = false;
            var allowReactiveDossier = false;
            var allowShowDetails = false;
            var allowShowDetailDs = false;
            var allowprendreEnCharge = false;
            var allowExportDs = false;
            var allowDeleteDs = false;
            var allowExtraireDetails = false;
            var allowValidationRefus = false;
            var allowTransmissionMajAr = false;
            var dossierId = 0;
            var isViewDetail = false;
            var dateLimt = DateTime.Now.AddYears(-5);
            var model = new ManageActionDossierLabViewModel();
            var isAdminGlobal = false;
            var isClos = false;
            var estNouvelleDs = false;
            try
            {
                try
                {
                    dossierId = this.Uprotect(cryptedDossierId, _protector);
                }
                catch (Exception e)
                {
                    _logger.TraceError(e);
                }

                if (dossierId != 0)
                {
                    var dossierLab = await _labService.Dossier.GetAsync(dossierId, false, cancellationToken)
                        .ConfigureAwait(false);

                    model.EstNouvelleDS = false;
                    var dateDeclarationIsNull = false;
                    if (dossierLab.DeclarationTracfins != null && dossierLab.DeclarationTracfins.Any())
                    {
                        model.EstNouvelleDS = dossierLab.DeclarationTracfins.All(x => x.EstNouvelleDeclarationTracfin);
                        dateDeclarationIsNull = dossierLab.DeclarationTracfins.All(x => x.DateDeclaration == null);
                    }

                    if (model.EstNouvelleDS)
                        model.EstEnvoiDSTracfin =
                            VerifEstEnvoiDsTracfin(dossierLab, model.EstNouvelleDS, dateDeclarationIsNull);


                    var isDs = dossierLab.IsDeclarationSoupcon && await _labService.DeclarationTracfin
                        .IsExist(dossierId, cancellationToken).ConfigureAwait(false);
                    isClos = dossierLab.StatutDossierId == (int)StatutDossierLabEnum.Cloture ||
                             dossierLab.StatutDossierId == (int)StatutDossierLabEnum.SansSuite;
                    //var isAttentePriseEnCharge =
                    //    dossierLab.StatutDossierId == (int)StatutDossierLabEnum.AttentePriseEnCharge;

                    isAdminGlobal = _roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal);

                    var directHbtIds = _labService.Referentiel
                        .GetDirectionHabiliteUser((int)ActiviteModule.Lab, _userInfoService).Select(x => (int?)x.Id)
                        .ToList();

                    var isEtendu = await _labService.UtilisateurDirectionService
                        .IsEtendu(currentUser.Id, cancellationToken)
                        .ConfigureAwait(false);
                    var userDelegatedIds =
                        (await _labService.Delegation
                            .GetDelegationUtilisateurHabiliteActivite(currentUser.Id, (int)ActiviteModule.Lab,
                                cancellationToken).ConfigureAwait(false)).Select(x => (int?)x.UtilisateurId).ToList();
                    var isOwner = userDelegatedIds.Contains(dossierLab.UtilisateurId) ||
                                  dossierLab.UtilisateurId == currentUser.Id;
                    var isAdminReferentiel = _roleAccessUser.HasClaims(ActiviteModule.Lab, ClaimUser.ADMINREFERENTIEL,
                        dossierLab.DirectionId);
                    var isConfident = !isAdminGlobal && dossierLab.Confidentiel && !isOwner && !isAdminReferentiel;
                    var isValideur = await _labService.UtilisateurDirectionService.IsIsValideurInActivity(
                        currentUser.Id,
                        dossierLab.DirectionId, (int)ActiviteModule.Lab, cancellationToken).ConfigureAwait(false);
                    var isPending = await _labService.UtilisateurDirectionService.IsPendingInActivity(currentUser.Id,
                        dossierLab.DirectionId, (int)ActiviteModule.Lab, cancellationToken).ConfigureAwait(false);
                    if (dossierLab.DeclarationTracfins != null)
                        estNouvelleDs = dossierLab.DeclarationTracfins.Any() &&
                                        dossierLab.DeclarationTracfins.All(x => x.EstNouvelleDeclarationTracfin);

                    isViewDetail = isEtendu
                        ? dossierLab.DateCreation >= DateTime.Now.AddYears(-10)
                        : dossierLab.DateCreation >= dateLimt && directHbtIds.Any(x => x == dossierLab.DirectionId);

                    allowShowDetails = allowExtraireDetails = isEtendu && !isConfident;

                    if (!isConfident && (isAdminReferentiel || isAdminGlobal || isValideur || isOwner ||
                                         dossierLab.StatutDossierId == 1))
                        switch (dossierLab.StatutDossierId)
                        {
                            case (int)StatutDossierLabEnum.Cloture:
                            case (int)StatutDossierLabEnum.SansSuite:
                                allowShowDetails = allowExtraireDetails = allowReactiveDossier = true;
                                allowExportDs = isDs;
                                allowTransmissionMajAr = !estNouvelleDs && isDs;
                                break;

                            case (int)StatutDossierLabEnum.EnCours:
                            case (int)StatutDossierLabEnum.AttenteDocuments:
                                allowShowDetails = allowExtraireDetails = allowUpdate = true;
                                allowDelete = isAdminReferentiel || isAdminGlobal;
                                allowDeleteDs = isDs && (isAdminReferentiel || isAdminGlobal);
                                allowExportDs = isDs;
                                allowTransmissionMajAr = !estNouvelleDs && isDs;
                                break;

                            case (int)StatutDossierLabEnum.AttentePriseEnCharge:
                                allowShowDetails = allowExtraireDetails = true;
                                allowprendreEnCharge = !isOwner || !isPending;
                                allowDelete = isAdminReferentiel || isAdminGlobal;
                                allowDeleteDs = isDs && isAdminReferentiel;
                                break;

                            case (int)StatutDossierLabEnum.PendingEnCours:
                                allowShowDetails = allowExtraireDetails = true;
                                allowUpdate = isOwner || isAdminGlobal;
                                allowDelete = isAdminReferentiel || isAdminGlobal;
                                allowDeleteDs = isDs && isAdminReferentiel;
                                allowTransmissionMajAr = !estNouvelleDs && isDs;
                                break;


                            case (int)StatutDossierLabEnum.AttenteValidation:
                                allowShowDetails = allowExtraireDetails = true;
                                allowValidationRefus = isValideur && dossierLab.UtilisateurId != currentUser.Id;
                                allowUpdate = isValideur;
                                allowDelete = isAdminReferentiel;
                                allowDeleteDs = isDs && (isAdminReferentiel || isAdminGlobal);
                                allowExportDs = isDs;
                                allowTransmissionMajAr = !estNouvelleDs && isDs;

                                break;

                            case (int)StatutDossierLabEnum.EncoursEnvoiDS:

                            case (int)StatutDossierLabEnum.AttenteARTracfin:
                                if (dossierLab.Direction.ModeEnvoieTracfinId is (int)ModeEnvoiTracfinEnum.Manuel
                                    or (int)ModeEnvoiTracfinEnum.Lot) allowReactiveDossier = true;
                                allowShowDetails = allowExtraireDetails = true;
                                allowExportDs = isDs;
                                allowTransmissionMajAr = estNouvelleDs || isDs;
                                break;

                            case (int)StatutDossierLabEnum.clotureTracfin:
                                allowExportDs = isDs;
                                allowShowDetails = allowExtraireDetails = true;
                                allowTransmissionMajAr = estNouvelleDs || isDs;
                                break;
                        }

                    if (isConfident && dossierLab.StatutDossierId == 1)
                    {
                        allowShowDetails = allowExtraireDetails = true;
                        allowprendreEnCharge = !isOwner || !isPending;
                        allowDelete = isAdminReferentiel || isAdminGlobal;
                        allowDeleteDs = isDs && (isAdminReferentiel || isAdminGlobal);
                        //allowExportDs = false;
                        //allowTransmissionMajAR = false;
                    }

                    if (dossierLab.StatutDossierId == 8 || dossierLab.StatutDossierId == 9 ||
                        dossierLab.StatutDossierId == 10) model.EstStatutExportDs = true;
                }

                isViewDetail = isAdminGlobal || isViewDetail;
                model.Add = true;
                model.Update = allowUpdate && isViewDetail;
                model.Delete = allowDelete && isViewDetail;
                model.Audit = allowShowDetails && isViewDetail;
                model.ShowDetails = allowShowDetails && isViewDetail;
                model.ShowDetailDs = allowShowDetailDs;
                model.ExtraireDetails = allowExtraireDetails && isViewDetail;
                model.Reactivate = allowReactiveDossier && isViewDetail;
                model.PrendreEnCharge = allowprendreEnCharge && isViewDetail;
                model.TransmissionUpdateAR = allowTransmissionMajAr && isViewDetail;
                model.AddFile = !estNouvelleDs && !allowTransmissionMajAr && isViewDetail && isClos;
                model.ExportDs = allowExportDs && isViewDetail;
                model.DeleteDs = allowDeleteDs && isViewDetail;
                model.ValidationRefus = allowValidationRefus && isViewDetail;
                model.Duplicate = true;
                model.Export = true;
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            return PartialView("/Areas/Lab/Views/Dossier/_PartialManageActionDossierLab.cshtml", model);
        }

        public async Task<IActionResult> AllowedUserCommandsViewerMode(string cryptedDossierId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();

            var isEtendu = await _labService.UtilisateurDirectionService
                .IsEtendu(currentUser.Id, cancellationToken)
                .ConfigureAwait(false);

            var dossierId = 0;
            var dateLimt = DateTime.Now.AddYears(-5);

            var model = new ManageActionDossierLabViewModel();
            try
            {
                try
                {
                    dossierId = this.Uprotect(cryptedDossierId, _protector);
                }
                catch (Exception e)
                {
                    _logger.TraceError(e);
                }

                bool allowExtraireDetails;

                if (dossierId == 0) return new BadRequestResult();

                var dossierLab = await _labService.Dossier.GetAsync(dossierId, false, cancellationToken)
                    .ConfigureAwait(false);

                var isAuditGlobal = _roleAccessUser.HasRoles(ActiviteModule.Lab, null, RoleUser.AuditGlobal);

                var isHabiliteDirectionDossier = _roleAccessUser
                    .IsHabiliteDirection(ActiviteModule.Lab, dossierLab.DirectionId);

                var isViewDetail = isEtendu
                    ? dossierLab.DateCreation >= DateTime.Now.AddYears(-10)
                    : dossierLab.DateCreation >= dateLimt && isHabiliteDirectionDossier;

                var isDs = dossierLab.IsDeclarationSoupcon && await _labService.DeclarationTracfin
                    .IsExist(dossierId, cancellationToken).ConfigureAwait(false);

                var userDelegatedIds =
                    (await _labService.Delegation
                        .GetDelegationUtilisateurHabiliteActivite(currentUser.Id, (int)ActiviteModule.Lab,
                            cancellationToken).ConfigureAwait(false)).Select(x => (int?)x.UtilisateurId).ToList();

                var isConfident = dossierLab.Confidentiel &&
                                  !(userDelegatedIds.Contains(dossierLab.UtilisateurId) ||
                                    dossierLab.UtilisateurId == currentUser.Id);

                var utilisateurConfidents =
                    (await _labService.UtilisateurDirectionService
                        .GetUtilisateurHabiliteConfident(dossierLab.DirectionId, (int)ActiviteModule.Lab,
                            cancellationToken).ConfigureAwait(false)).Select(x => (int?)x.Id).ToList();
                if (utilisateurConfidents.Contains(dossierLab.UtilisateurId)) isConfident = false;

                var allowShowDetails = allowExtraireDetails = !isConfident;

                var isAdminGlobal = _roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal);

                model.ShowDetails = (allowShowDetails && isViewDetail) || isAdminGlobal || isAuditGlobal;
                model.ShowDetailDs = isDs;
                model.ExtraireDetails = (allowExtraireDetails && isViewDetail) || isAdminGlobal;
                model.Audit = (allowShowDetails && isViewDetail) || isAdminGlobal;
                model.Contact = true;

                model.ViewContent = await EnableViewContent(dossierLab, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return PartialView("/Areas/Lab/Views/Dossier/_PartialManageActionDossierLab.cshtml", model);
        }

        private async Task<bool> EnableViewContent(DossierLab dossierLab, CancellationToken cancellationToken = default)
        {
            var isAdminGlobal = _roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal);

            if (isAdminGlobal) return false;

            var isEtendu = await _utilisateurDirectionService.IsEtendu(currentUser.Id, cancellationToken);

            if (isEtendu) return false;

            var isDs = dossierLab.IsDeclarationSoupcon &&
                       await _labService.DeclarationTracfin.IsExist(dossierLab.Id, cancellationToken);

            if (isDs) return false;

            var isHabiliteDirectionDossier =
                _roleAccessUser.IsHabiliteDirection(ActiviteModule.Lab, dossierLab.DirectionId);

            if (isHabiliteDirectionDossier) return false;

            return await _referentielService
                .GetCategorieGroupeLabs()
                .AnyAsync(c => c.IsDs && c.Id == dossierLab.CategorieGroupeLabId, cancellationToken);
        }

        [HttpGet]
        public IActionResult DeleteDossierLab()
        {
            if (!IsVerifyUserHabilitation())
                return RedirectToAction("Login", "Account");

            return PartialView("/Areas/Lab/Views/Dossier/_PartialDeleteConfirmDossier.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteDossiersLab(
            string dossierId,
            string motifSuppression,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            if (!_roleAccessUser.HasActivites(ActiviteModule.Lab))
                return RedirectToAction("Login", "Account");

            var isValidDelete = false;
            bool result;
            try
            {
                var id = this.Uprotect(dossierId, _protector);

                var dossierLab = await _labService.Dossier.GetDossierLabAsync(id, cancellationToken)
                    .ConfigureAwait(false);
                var declarationTracfin = await _labService.DeclarationTracfin
                    .GetDeclarationByDossierAsync(id, cancellationToken).ConfigureAwait(false);

                if (!IsVerifyUserHabilitation())
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["PasDePersmissionActiviteDirection"]
                    });
                dossierLab.Utilisateur = null;

                var isAdminReferentiel = _roleAccessUser.HasClaims(ActiviteModule.Lab, ClaimUser.ADMINREFERENTIEL,
                    dossierLab.DirectionId);
                var isAdminGlobal = _roleAccessUser.HasRoles(ActiviteModule.Lab, null, RoleUser.AdminGlobal);
                var statutIds = new List<int>
                {
                    (int)StatutDossierLabEnum.EnCours,
                    (int)StatutDossierLabEnum.AttenteDocuments,
                    (int)StatutDossierLabEnum.AttentePriseEnCharge,
                    (int)StatutDossierLabEnum.AttenteValidation
                };

                if ((isAdminReferentiel || isAdminGlobal) && statutIds.Contains(dossierLab.StatutDossierId))
                    isValidDelete = true;

                if (!isValidDelete)
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);

                if (declarationTracfin != null)
                    await _labService.DeclarationTracfin.DeleteAsync(declarationTracfin, cancellationToken)
                        .ConfigureAwait(false);

                result = await _labService.Dossier.DeleteAsync(dossierLab, cancellationToken).ConfigureAwait(false);

                if (result)
                    await _eventDossierLogger.LogDossierEvent(
                        EactionEventTypeDossier.SUPPRESSION_DOSSIER,
                        currentUser.Id,
                        dossierLab.CodeUnique,
                        ActiviteModule.Lab,
                        ed =>
                        {
                            ed.DossierLabId = dossierLab.Id;
                            ed.Motif = motifSuppression;
                            ed.DateCreationDossier = dossierLab.DateCreation;
                        },
                        cancellationToken);
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
                return new JsonResult(new { success = false });
            }

            _logger.EndTrace();
            return new JsonResult(new { success = result });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteDeclationSoupsonLab(string cryptedId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            if (!_roleAccessUser.HasActivites(ActiviteModule.Lab))
                return RedirectToAction("Login", "Account");
            try
            {
                var statutIds = new List<int>
                {
                    (int)StatutDossierLabEnum.EnCours,
                    (int)StatutDossierLabEnum.AttenteDocuments,
                    (int)StatutDossierLabEnum.AttentePriseEnCharge,
                    (int)StatutDossierLabEnum.AttenteValidation
                };

                var id = this.Uprotect(cryptedId, _protector);
                var eventDossier = new EventDossierViewModel
                { DateCreation = DateTime.UtcNow, IsActive = true, ActiviteId = (int)ActiviteModule.Lab };
                var declarationTracfin = await _labService.DeclarationTracfin
                    .GetDeclarationByDossierAsync(id, cancellationToken).ConfigureAwait(false);
                if (declarationTracfin != null)
                {
                    var dossierLab = await _labService
                        .Dossier
                        .GetAsync(id, cancellationToken);

                    if (!IsVerifyUserHabilitation())
                        return new JsonResult(new
                        {
                            success = false,
                            status = false,
                            message = _translator.Common["PasDePersmissionActiviteDirection"]
                        });
                    var isAdminReferentiel = _roleAccessUser.HasClaims(ActiviteModule.Lab, ClaimUser.ADMINREFERENTIEL,
                        dossierLab.DirectionId);
                    var isAdminGlobal = _roleAccessUser.HasRoles(ActiviteModule.Lab, null, RoleUser.AdminGlobal);
                    if (!(statutIds.Contains(dossierLab.StatutDossierId) && (isAdminGlobal || isAdminReferentiel)))
                        return RedirectToAction("Login", "Account");


                    await _labService.DeclarationTracfin.DeleteAsync(declarationTracfin, cancellationToken)
                        .ConfigureAwait(false);

                    dossierLab.EntiteLab = null;
                    dossierLab.StatutDossier = null;
                    dossierLab.IsDeclarationSoupcon = false;
                    dossierLab.ModificateurId = currentUser.Id;
                    dossierLab.DateModification = DateTime.UtcNow;
                    eventDossier.CodeDossier = dossierLab.CodeUnique;

                    await _labService.Dossier.SaveAsyncStandard(cancellationToken);
                }

                eventDossier.DossierLabId = id;
                eventDossier.CreateurId = currentUser.Id;
                eventDossier.UtilisateurEventId = currentUser.Id;
                eventDossier.EventDossierTypeId = (int)EactionEventTypeDossier.DELETE_DECLARATION_SOUPCON_LAB;

                await _labService.EventDossier.AddAsync(_mapper.Map<EventDossier>(eventDossier), cancellationToken)
                    .ConfigureAwait(false);
                var libelleModificateur = $"{currentUser.Prenom} {currentUser.Nom.ToUpper(CultureInfo.CurrentCulture)}";
                _logger.EndTrace();
                return new JsonResult(new
                {
                    success = true,
                    modificateur = libelleModificateur,
                    isDeclarationSoupcon = false,
                    dateModification = DateTime.UtcNow
                });
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
                return new JsonResult(new { success = false });
            }
        }

        public async Task<IEnumerable<SelectedItem>> GetDirectionSearchMode(EditionMode mode)
        {
            _logger.BeginTrace();

            IEnumerable<SelectedItem> directions = null;
            try
            {
                var isEtendu = await _labService.Referentiel
                    .IsEtenduUtilisateurAsync((int)ActiviteModule.Lab, currentUser.Id).ConfigureAwait(false);

                if (_roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal) ||
                    (isEtendu && mode != EditionMode.Management))
                {
                    directions = _mapper.Map<IEnumerable<SelectedItem>>(_referential.Directions);
                }
                else
                {
                    if (mode == EditionMode.Management || mode == EditionMode.Viewer)
                    {
                        IEnumerable<Direction> dirs = _labService.Referentiel
                            .GetDirectionHabiliteUserLecture((int)ActiviteModule.Lab, _userInfoService).ToList();
                        directions = _mapper.Map<IEnumerable<SelectedItem>>(dirs);
                    }
                    else
                    {
                        var directionAccs =
                            _labService.Referentiel.GetDirectionsAccessiblesByActivite((int)ActiviteModule.Lab,
                                currentUser.Id);
                        directions = _mapper.Map<IEnumerable<SelectedItem>>(directionAccs);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();
            return directions;
        }

        public IEnumerable<SelectedItem> GetDirections(bool isManagementMode)
        {
            _logger.BeginTrace();

            IEnumerable<SelectedItem> directions = null;
            try
            {
                if (_roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal))
                {
                    directions = _mapper.Map<IEnumerable<SelectedItem>>(_referential.Directions);
                }
                else
                {
                    if (isManagementMode)
                    {
                        var dirs = _labService.Referentiel
                            .GetDirectionHabiliteUser((int)ActiviteModule.Lab, _userInfoService)
                            .ToList();
                        directions = _mapper.Map<IEnumerable<SelectedItem>>(dirs);
                    }
                    else
                    {
                        var directionAccs = _labService.Referentiel
                            .GetDirectionsAccessiblesByActivite((int)ActiviteModule.Lab, currentUser.Id);
                        directions = _mapper.Map<IEnumerable<SelectedItem>>(directionAccs);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();

            return directions?.Distinct();
        }

        [HttpGet]
        public Task<List<SelectedItem>> GetPays()
        {
            _logger.BeginTrace();
            if (!IsVerifyUserHabilitation())
                return Task.FromResult<List<SelectedItem>>(null);
            List<SelectedItem> result = null;
            try
            {
                result = _mapper.Map<List<SelectedItem>>(_labService.Referentiel.GetPays());
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();

            return Task.FromResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> IsExistPieceIdentite(string numero)
        {
            var isExist = false;
            try
            {
                if (!IsVerifyUserHabilitation())
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["PasDePersmissionActiviteDirection"]
                    });
                isExist = await _pieceIdentiteService.IsExistPieceIdentite(numero).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }
            finally
            {
                _logger.EndTrace();
            }

            return Json(new { Result = isExist });
        }

        [HttpGet]
        public async Task<IActionResult> DownloadLabDocument(string cryptedId, string dossierLabCryptedId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();

            FileContentResult result = null;

            try
            {
                if (!IsVerifyUserHabilitation())
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["PasDePersmissionActiviteDirection"]
                    });
                var documentId = 0;
                var dossierId = 0;
                if (!string.IsNullOrEmpty(cryptedId)) documentId = this.Uprotect(cryptedId.Trim(), _protector);
                if (!string.IsNullOrEmpty(dossierLabCryptedId))
                    dossierId = this.Uprotect(dossierLabCryptedId.Trim(), _protector);

                var documentDossier =
                    await _labService.DocumentDossier.GetDocumentByDossierId(dossierId, cancellationToken);
                var autoriseDownload = documentDossier.Any(x => x.DocumentLabId == documentId);
                if (!autoriseDownload)
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["DocumentNonExistant"]
                    });

                var document = _mapper.Map<DocumentLabViewModel>(await _labService.DocumentDossier
                    .GetDocument(documentId, false, cancellationToken)
                    .ConfigureAwait(false));

                if (document == null || string.IsNullOrEmpty(document.FileContent))
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["DocumentNonExistant"]
                    });
                var fileBytes = Convert.FromBase64String(document.FileContent ?? string.Empty);
                result = File(fileBytes, MediaTypeNames.Application.Octet, document.Name);
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();

            return result;
        }

        [HttpGet]
        public async Task<IActionResult> DownloadLabDeclarationTracfinFile(string cryptedId, string dossierLabCryptedId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();

            FileContentResult result = null;

            try
            {
                if (!IsVerifyUserHabilitation())
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["PasDePersmissionActiviteDirection"]
                    });
                var documentId = 0;
                var dossierId = 0;
                if (!string.IsNullOrEmpty(cryptedId)) documentId = this.Uprotect(cryptedId.Trim(), _protector);
                if (!string.IsNullOrEmpty(dossierLabCryptedId))
                    dossierId = this.Uprotect(dossierLabCryptedId.Trim(), _protector);

                var dsFileArId = await _labService.Dossier.GetDsFileArId(dossierId, cancellationToken);
                if (dsFileArId != documentId)
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["DocumentNonExistant"]
                    });

                var id = 0;
                if (!string.IsNullOrEmpty(cryptedId)) id = this.Uprotect(cryptedId, _protector);

                var document = _mapper.Map<DeclarationTracfinFileViewModel>(_labService.DocumentDossier
                    .GetDeclarationTracfinFile(id));

                if (document == null || string.IsNullOrEmpty(document.FileContent))
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["DocumentNonExistant"]
                    });
                var fileBytes = Convert.FromBase64String(document.FileContent ?? string.Empty);
                result = File(fileBytes, MediaTypeNames.Application.Octet, document.FileName);
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();

            return result;
        }


        [HttpGet]
        public async Task<IActionResult> RemoveLabDocument(string cryptedId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();

            try
            {
                if (!IsVerifyUserHabilitation())
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["PasDePersmissionActiviteDirection"]
                    });
                var id = 0;
                if (!string.IsNullOrEmpty(cryptedId)) id = this.Uprotect(cryptedId, _protector);

                await _labService.DocumentDossier
                    .DeleteDocumentDosssierByDocumentId(id, cancellationToken)
                    .ConfigureAwait(false);

                await _labService.DocumentDossier
                    .DeleteDocument(id, cancellationToken)
                    .ConfigureAwait(false);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> DownloadLabAllDocuments(string cryptedId, string codeUnique,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();

            try
            {
                if (!IsVerifyUserHabilitation() || string.IsNullOrEmpty(cryptedId))
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["PasDePersmissionActiviteDirection"]
                    });

                var id = this.Uprotect(cryptedId, _protector);
                var documents = await _labService.DocumentDossier
                    .GetDocumentByDossierId(id, CancellationToken.None)
                    .ConfigureAwait(false);
                var declarationTracfinFiles =
                    _labService.DocumentDossier.GetDeclarationTracfinFileAllDocument(codeUnique);

                var documentsList = documents.ToList();
                documentsList.AddRange(declarationTracfinFiles);

                // the output bytes of the zip
                byte[] fileBytes;
                // create a working memory stream
                using (var memoryStream = new MemoryStream())
                {
                    // create a zip
                    using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        // interate through the source files
                        foreach (var f in documentsList)
                        {
                            // add the item name to the zip
                            var zipItem = zip.CreateEntry(f.FileName);
                            // add the item bytes to the zip entry by opening the original file and copying the bytes
                            using var originalFileMemoryStream = new MemoryStream(f.DocumentLab.FileContent);
                            await using var entryStream = zipItem.Open();
                            await originalFileMemoryStream.CopyToAsync(entryStream, cancellationToken);
                        }
                    }

                    fileBytes = memoryStream.ToArray();
                }

                // download the constructed zip
                Response.Headers.Add("Content-Disposition",
                    "attachment; filename=Documents_Dossier_Lab_" + codeUnique + ".zip");

                return File(fileBytes, "application/zip");
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();

            return null;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DownloadLabDocumentDemandeInformation(string cryptedId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();

            FileContentResult result = null;

            try
            {
                var id = 0;
                if (!string.IsNullOrEmpty(cryptedId)) id = this.Uprotect(cryptedId, _protector);

                var document = _mapper.Map<DocumentLabViewModel>(await _labService.DocumentDossier
                    .GetDocumentDemandeInformationLabs(id, false, cancellationToken)
                    .ConfigureAwait(false));
                var fileBytes = Convert.FromBase64String(document?.FileContent ?? string.Empty);
                result = File(fileBytes, MediaTypeNames.Application.Octet, document?.Name);
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();

            return result;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DownloadLabAllDocumentsDemandeInformation(string cryptedId, string codeUnique,
            int isRequest,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();

            try
            {
                if (string.IsNullOrEmpty(cryptedId))
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["PasDePersmissionActiviteDirection"]
                    });

                var id = this.Uprotect(cryptedId, _protector);
                var documents = await _labService.DocumentDossier
                    .GetDocumentsDemandeInformationLabs(id, isRequest > 0, cancellationToken)
                    .ConfigureAwait(false);


                // the output bytes of the zip
                byte[] fileBytes;
                // create a working memory stream
                using (var memoryStream = new MemoryStream())
                {
                    // create a zip
                    using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        // interate through the source files
                        foreach (var f in documents)
                        {
                            // add the item name to the zip
                            var zipItem = zip.CreateEntry(f.DocumentLab.Name);
                            // add the item bytes to the zip entry by opening the original file and copying the bytes
                            using var originalFileMemoryStream = new MemoryStream(f.DocumentLab.FileContent);
                            await using var entryStream = zipItem.Open();
                            await originalFileMemoryStream.CopyToAsync(entryStream, cancellationToken);
                        }
                    }

                    fileBytes = memoryStream.ToArray();
                }

                // download the constructed zip
                Response.Headers.Add("Content-Disposition",
                    "attachment; filename=Documents_Demande_Information_Lab_" +
                    (isRequest > 0 ? "Request_" : "Response_") + codeUnique + ".zip");

                return File(fileBytes, "application/zip");
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();

            return null;
        }


        [HttpGet]
        public async Task<IActionResult> DownloadLabDocuments(string cryptedId)
        {
            var lang = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture);
            var oui = "Oui";
            var non = "Non";
            var occasionnel = "Occasionnel";
            var habituel = "Habituel";
            var dossierNumero = "Dossier N° ";

            if (lang == "en")
            {
                oui = "Yes";
                non = "No";
                dossierNumero = "Case N° ";
                habituel = "Usual";
                occasionnel = "occasional";
            }

            var id = 0;
            if (!string.IsNullOrEmpty(cryptedId)) id = this.Uprotect(cryptedId, _protector);
            var dossierLab = await _labService.Dossier.GetDossierByDetails(id).ConfigureAwait(false);
            if (dossierLab != null)
            {
                if (!IsVerifyUserHabilitation())
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["PasDePersmissionActiviteDirection"]
                    });
                var newFile = @"wwwroot/documentations/" + lang + "/extraction/lab/lcb-template-dossier.xlsx";
                var newFileSortis = @"wwwroot/documentations/" + lang + "/extraction/lab/rapport-dossier-Lab_" +
                                    DateTime.Now.ToString("dd-MM-yyy-HHmmss") + ".xlsx";
                XSSFWorkbook hssfwb;
                await using (var file = new FileStream(newFile, FileMode.Open, FileAccess.Read))
                {
                    hssfwb = new XSSFWorkbook(file);
                    file.Close();
                }

                var indexGetSheet = 0;
                //Modification information details dossier
                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(1)?.GetCell(0)
                    ?.SetCellValue(dossierNumero + dossierLab.CodeUnique);
                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(3)?.GetCell(0)?.SetCellValue(dossierLab.Direction?.Nom);
                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(3)?.GetCell(1)?.SetCellValue(dossierLab.EntiteLab?.Lisp);
                if (dossierLab.DateReception != null)
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(3)?.GetCell(2)
                        ?.SetCellValue(dossierLab.DateReception.GetValueOrDefault().Date);
                if (dossierLab.DateCloture != null)
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(3)?.GetCell(3)
                        ?.SetCellValue(dossierLab.DateCloture.GetValueOrDefault().ToString("dd/MM/yyyy"));


                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(5)?.GetCell(1)?.SetCellValue(
                    dossierLab.Utilisateur?.Nom.ToUpper(CultureInfo.CurrentCulture) + " " +
                    dossierLab.Utilisateur?.Prenom);
                if (dossierLab.DateCreation != null)
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(5)?.GetCell(3)
                        ?.SetCellValue(dossierLab.DateCreation.GetValueOrDefault().ToString("dd/MM/yyyy"));

                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(6)?.GetCell(1)?.SetCellValue(
                    dossierLab.Modificateur?.Nom.ToUpper(CultureInfo.CurrentCulture) + " " +
                    dossierLab.Modificateur?.Prenom);
                if (dossierLab.DateModification != null)
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(6)?.GetCell(3)
                        ?.SetCellValue(dossierLab.DateModification.GetValueOrDefault().ToString("dd/MM/yyyy"));

                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(7)?.GetCell(1)
                    ?.SetCellValue(dossierLab.IsDeclarationSoupcon ? oui : non);
                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(7)?.GetCell(3)
                    ?.SetCellValue(dossierLab.Confidentiel ? oui : non);
                var sheet = hssfwb.GetSheetAt(indexGetSheet);
                var row8 = sheet.GetRow(8);
                if (dossierLab.DateDeclarationLocale != null)
                {
                    var str = lang == "fr"
                   ? dossierLab.DateDeclarationLocale?.ToString("dd/MM/yyyy")
                   : dossierLab.DateDeclarationLocale?.ToString("MM/dd/yyyy");
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(8)?.GetCell(1)
                        ?.SetCellValue(str);
                }
                else
                {
                    if (row8 != null)
                    {
                        foreach (var cell in row8.Cells)
                        {
                            cell.SetCellValue(string.Empty);
                            var style = cell.CellStyle;
                            cell.CellStyle = style;
                        }
                    }
                }             

                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(10)?.GetCell(1)?.SetCellValue(lang == "fr"
                    ? dossierLab.CategorieGroupeLab?.FrenchName
                    : dossierLab.CategorieGroupeLab?.EnglishName);
                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(10)?.GetCell(3)?.SetCellValue(lang == "fr"
                    ? dossierLab.Categorie?.FrenchName
                    : dossierLab.Categorie?.EnglishName);
                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(11)?.GetCell(1)?.SetCellValue(lang == "fr"
                    ? dossierLab.OrigineGroupeLab?.FrenchName
                    : dossierLab.OrigineGroupeLab?.EnglishName);
                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(11)?.GetCell(3)?.SetCellValue(lang == "fr"
                    ? dossierLab.OrigineLab?.FrenchName
                    : dossierLab.OrigineLab?.EnglishName);
                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(12)?.GetCell(1)?.SetCellValue(lang == "fr"
                    ? dossierLab.SecteurEconomiqueLab?.FrenchName
                    : dossierLab.SecteurEconomiqueLab?.EnglishName);
                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(12)?.GetCell(3)
                    ?.SetCellValue(lang == "fr" ? dossierLab.Pays?.IsoFrenchName : dossierLab.Pays?.IsoEnglishName);
                var operationLab = dossierLab.DossierLabOperations.FirstOrDefault();
                if (operationLab != null)
                {
                    if (operationLab.Montant > 0)
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(15)?.GetCell(1)
                            ?.SetCellValue((double)operationLab.Montant);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(15)?.GetCell(3)?.SetCellValue(lang == "fr"
                        ? operationLab.Devise?.IsoFrenchName
                        : operationLab.Devise?.IsoEnglishName);

                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(16)?.GetCell(1)?.SetCellValue(lang == "fr"
                        ? operationLab.TypeLegislationLab?.FrenchName
                        : operationLab.TypeLegislationLab?.EnglishName);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(16)?.GetCell(3)
                        ?.SetCellValue(operationLab.IsDeclarationAutorite ? oui : non);

                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(17)?.GetCell(1)
                        ?.SetCellValue(operationLab.Blocage ? oui : non);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(17)?.GetCell(3)
                        ?.SetCellValue(operationLab.Gel ? oui : non);

                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(18)?.GetCell(1)
                        ?.SetCellValue(operationLab.Annulation ? oui : non);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(18)?.GetCell(3)
                        ?.SetCellValue(operationLab.Libere ? oui : non);
                }

                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(20)?.GetCell(1)?.SetCellValue(lang == "fr"
                    ? dossierLab.DossierLabScenarios.FirstOrDefault()?.ScenarioLab?.FrenchName                
                    : dossierLab.DossierLabScenarios.FirstOrDefault()?.ScenarioLab?.EnglishName);
                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(23)?.GetCell(0)
                    ?.SetCellValue(Encoding.UTF8.GetString(dossierLab.MotifsSoupcons ?? Array.Empty<byte>())
                        .HTMLToText());
                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(27)?.GetCell(1)
                    ?.SetCellValue(lang == "fr" ? dossierLab.AvisLab?.FrenchName : dossierLab.AvisLab?.EnglishName);
                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(28)?.GetCell(1)
                    ?.SetCellValue(lang == "fr" ? dossierLab.Visa?.FrenchName : dossierLab.Visa?.EnglishName);
                if (dossierLab.DateModification != null)
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(29)?.GetCell(1)
                        ?.SetCellValue(dossierLab.PresentationComiteDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(30)?.GetCell(1)?.SetCellValue(lang == "fr"
                    ? dossierLab.StatutDossier?.FrenchName
                    : dossierLab.StatutDossier?.EnglishName);
                var row = hssfwb.GetSheetAt(indexGetSheet)?.GetRow(23);
                if (row != null)
                {
                    row.Height = -1;
                }
                var i = 1;
                //Modification Action(s) Menéne(s)
                if (dossierLab.DossierLabActions.Count == 0)
                {
                    hssfwb.RemoveSheetAt(indexGetSheet + 1);
                }
                else
                {
                    indexGetSheet++;
                    foreach (var item in dossierLab.DossierLabActions)
                    {
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(0)
                            ?.SetCellValue(Encoding.UTF8.GetString(item.Libelle ?? Array.Empty<byte>()));
                        if (item.DateCreation != null)
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(1)
                                ?.SetCellValue(item.DateCreation.GetValueOrDefault().ToString("dd/MM/yyyy"));
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(2)?.SetCellValue(
                            item.Createur.Nom.ToUpper(CultureInfo.CurrentCulture) + " " + item.Createur?.Prenom);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(3)
                            ?.SetCellValue(
                                Encoding.UTF8.GetString(item.Description ?? Array.Empty<byte>()).HTMLToText());
                        i++;
                    }
                }

                //Modification information details Declaration Tracfin
                var declarationTracfin = await _labService.DeclarationTracfin
                    .GetDeclarationByDetailsAsync(dossierLab.Id)
                    .ConfigureAwait(false);
                if (declarationTracfin != null)
                {
                    indexGetSheet++;
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(3)?.GetCell(1)
                        ?.SetCellValue(declarationTracfin.DeclarationNumber);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(3)?.GetCell(4)
                        ?.SetCellValue(declarationTracfin.NumeroAccuseReception);
                    //hssfwb.GetSheetAt(indexGetSheet)?.GetRow(4)?.GetCell(1)?.SetCellValue(declarationTracfin.CategorieGroupeTracfin?.FrenchName);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(4)?.GetCell(1)?.SetCellValue(lang == "fr"
                        ? declarationTracfin.CategorieTracfin?.FrenchName
                        : declarationTracfin.CategorieTracfin?.EnglishName);
                    //Categorie de la declaration groupe
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(6)?.GetCell(3)
                        ?.SetCellValue(declarationTracfin.IsDsEffAuTitreI ? oui : non);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(7)?.GetCell(3)
                        ?.SetCellValue(declarationTracfin.IsDsEffAuTitreII ? oui : non);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(8)?.GetCell(3)
                        ?.SetCellValue(declarationTracfin.IsDsComplementaire ? oui : non);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(9)?.GetCell(3)
                        ?.SetCellValue(declarationTracfin.IsDsRelOpeSus ? oui : non);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(10)?.GetCell(3)
                        ?.SetCellValue(declarationTracfin.IsDsEffAvantOpeSus ? oui : non);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(11)?.GetCell(3)
                        ?.SetCellValue(declarationTracfin.IsDsRuptureRelation ? oui : non);

                    if (declarationTracfin.DateRuptureRelation != null)
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(12)?.GetCell(3)?.SetCellValue(declarationTracfin
                            .DateRuptureRelation.GetValueOrDefault().ToString("dd/MM/yyyy"));
                    ////Modification declaration Tracfin Organisme
                    var organismeTracfin = declarationTracfin.OrganismeLab;
                    if (organismeTracfin != null)
                    {
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(15)?.GetCell(0)
                            ?.SetCellValue(organismeTracfin.Direction?.Nom);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(16)?.GetCell(1)?.SetCellValue(lang == "fr"
                            ? organismeTracfin.Profession?.FrenchName
                            : organismeTracfin.Profession?.EnglishName);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(16)?.GetCell(4)
                            ?.SetCellValue(organismeTracfin.NumeroIdentifiantProfessionnel);
                        if (organismeTracfin.NumeroVoie > 0)
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(17)?.GetCell(1)
                                ?.SetCellValue(organismeTracfin.NumeroVoie.ToString());
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(17)?.GetCell(4)?.SetCellValue(lang == "fr"
                            ? organismeTracfin.ComplementVoie?.FrenchName
                            : organismeTracfin.ComplementVoie?.EnglishName);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(18)?.GetCell(1)?.SetCellValue(lang == "fr"
                            ? organismeTracfin.TypeVoie?.FrenchName
                            : organismeTracfin.TypeVoie?.EnglishName);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(18)?.GetCell(4)?.SetCellValue(organismeTracfin.Voie);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(19)?.GetCell(1)
                            ?.SetCellValue(organismeTracfin.Complement);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(19)?.GetCell(4)?.SetCellValue(organismeTracfin.Ville);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(20)?.GetCell(1)?.SetCellValue(lang == "fr"
                            ? organismeTracfin.Pays?.IsoFrenchName
                            : organismeTracfin.Pays?.IsoEnglishName);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(20)?.GetCell(4)
                            ?.SetCellValue(organismeTracfin.CodePostal);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(21)?.GetCell(1)
                            ?.SetCellValue(organismeTracfin.TelephoneFixe);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(21)?.GetCell(4)?.SetCellValue(organismeTracfin.Fax);
                    }

                    ////Modification declaration Tracfin à propos de l'envoi
                    if (declarationTracfin.DateDeclaration != null)
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(24)?.GetCell(1)?.SetCellValue(declarationTracfin
                            .DateDeclaration.GetValueOrDefault().ToString("dd/MM/yyyy"));
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(24)?.GetCell(4)
                        ?.SetCellValue(declarationTracfin.ReferenceInterne);


                    //hssfwb.GetSheetAt(indexGetSheet)?.GetRow(17)?.GetCell(4)?.SetCellValue(declarationTracfin.AccuseReception == true ? Oui : Non);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(25)?.GetCell(1)
                        ?.SetCellValue(declarationTracfin.NumeroPrecedenteDeclaration != null ? oui : non);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(25)?.GetCell(4)?.SetCellValue(lang == "fr"
                        ? declarationTracfin.TypeDeclaration?.FrenchName
                        : declarationTracfin.TypeDeclaration?.EnglishName);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(26)?.GetCell(1)
                        ?.SetCellValue(declarationTracfin.NumeroPrecedenteDeclaration);

                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(28)?.GetCell(1)?.SetCellValue(declarationTracfin.Nom);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(28)?.GetCell(4)?.SetCellValue(declarationTracfin.Prenom);

                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(29)?.GetCell(1)
                        ?.SetCellValue(declarationTracfin.Telephone);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(29)?.GetCell(4)?.SetCellValue(declarationTracfin.Fax);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(30)?.GetCell(1)?.SetCellValue(declarationTracfin.Mail);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(30)?.GetCell(4)
                        ?.SetCellValue(declarationTracfin.IdentifiantErmes);

                    ////Modification declaration Tracfin synthèse
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(33)?.GetCell(1)
                        ?.SetCellValue(Encoding.UTF8.GetString(declarationTracfin.Motifs ?? Array.Empty<byte>())
                            .HTMLToText());
                    if (declarationTracfin.DebutPeriodeFaitsConsideres != null)
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(34)?.GetCell(1)?.SetCellValue(declarationTracfin
                            .DebutPeriodeFaitsConsideres.GetValueOrDefault().ToString("dd/MM/yyyy"));
                    if (declarationTracfin.FinPeriodeFaitsConsideres != null)
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(34)?.GetCell(4)?.SetCellValue(declarationTracfin
                            .FinPeriodeFaitsConsideres.GetValueOrDefault().ToString("dd/MM/yyyy"));

                    if (declarationTracfin.MontantTotal > 0)
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(35)?.GetCell(1)
                            ?.SetCellValue((double)declarationTracfin.MontantTotal);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(35)?.GetCell(4)?.SetCellValue(lang == "fr"
                        ? declarationTracfin.Devise?.IsoFrenchName
                        : declarationTracfin.Devise?.IsoEnglishName);

                    if (declarationTracfin.NombrePersonnePhysiques > 0)
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(36)?.GetCell(1)
                            ?.SetCellValue(declarationTracfin.NombrePersonnePhysiques?.ToString());
                    if (declarationTracfin.NombrePersonneMorales > 0)
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(36)?.GetCell(4)
                            ?.SetCellValue(declarationTracfin.NombrePersonneMorales?.ToString());

                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(37)?.GetCell(1)
                        ?.SetCellValue(declarationTracfin.NombreOperations?.ToString());
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(37)?.GetCell(4)?.SetCellValue(lang == "fr"
                        ? declarationTracfin.StatutOperation?.FrenchName
                        : declarationTracfin.StatutOperation?.EnglishName);

                    if (declarationTracfin.DateTimeExecution != null)
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(38)?.GetCell(1)?.SetCellValue(oui);
                    if (declarationTracfin.DateTimeExecution != null)
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(38)?.GetCell(4)?.SetCellValue(declarationTracfin
                            .DateTimeExecution.GetValueOrDefault().ToString("dd/MM/yyyy"));
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(39)?.GetCell(1)?.SetCellValue(lang == "fr"
                        ? declarationTracfin.PrincipalInstrumentFinancier?.FrenchName
                        : declarationTracfin.PrincipalInstrumentFinancier?.EnglishName);
                    hssfwb.GetSheetAt(indexGetSheet)?.GetRow(42)?.GetCell(0)?.SetCellValue(
                        Encoding.UTF8.GetString(declarationTracfin.Analyses ?? Array.Empty<byte>()).HTMLToText());

                    ////Modification declaration Tracfin pieces
                    i = 45;
                    foreach (var item in declarationTracfin.DocumentDeclarationTracfins)
                    {
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(0)?.SetCellValue(lang == "fr"
                            ? item.TypeDocument?.FrenchName
                            : item.TypeDocument?.EnglishName);
                        if (item.DateDocument != null)
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(1)
                                ?.SetCellValue(item.DateDocument.GetValueOrDefault().ToString("dd/MM/yyyy"));
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(3)
                            ?.SetCellValue(item.Libelle.HTMLToText());
                        i++;
                    }
                }
                else
                {
                    hssfwb.RemoveSheetAt(indexGetSheet + 1);
                }


                ////Modification Tiers Personne(s) Physique(s)
                var personnesPhysiques = await _labService.DossierLabPersonnePhysique
                    .GetDossierLabPersonnePhysiqueByDetailsAsync(dossierLab.Id).ConfigureAwait(false);
                var personnePhysiqueLabs = personnesPhysiques.ToList();
                if (personnesPhysiques != null && !personnePhysiqueLabs.Any())
                {
                    hssfwb.RemoveSheetAt(indexGetSheet + 5);
                    hssfwb.RemoveSheetAt(indexGetSheet + 4);
                    hssfwb.RemoveSheetAt(indexGetSheet + 3);
                    hssfwb.RemoveSheetAt(indexGetSheet + 2);
                    hssfwb.RemoveSheetAt(indexGetSheet + 1);
                }
                else
                {
                    indexGetSheet++;
                    i = 1;
                    foreach (var personnePhysiqueLab in personnePhysiqueLabs)
                    {
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(0)
                            ?.SetCellValue(personnePhysiqueLab.NomNaissance);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(1)
                            ?.SetCellValue(personnePhysiqueLab.NomUsuel);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(2)
                            ?.SetCellValue(personnePhysiqueLab.Prenoms);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(3)
                            ?.SetCellValue(dossierLab.IsDeclarationSoupcon ? oui : non);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(4)
                            ?.SetCellValue(personnePhysiqueLab.IdentifiantClient);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(5)?.SetCellValue(lang == "fr"
                            ? personnePhysiqueLab.Civilite?.FrenchName
                            : personnePhysiqueLab.Civilite?.EnglishName);
                        if (personnePhysiqueLab.DateNaissance != null)
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(6)
                                ?.SetCellValue(personnePhysiqueLab.DateNaissance.Value.DateTime.ToString("dd/MM/yyyy"));

                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(7)
                            ?.SetCellValue(
                                Encoding.UTF8.GetString(personnePhysiqueLab.LieuNaissance ?? Array.Empty<byte>()));
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(8)?.SetCellValue(lang == "fr"
                            ? personnePhysiqueLab.PaysNaissance?.IsoFrenchName
                            : personnePhysiqueLab.PaysNaissance?.IsoEnglishName);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(9)?.SetCellValue(lang == "fr"
                            ? personnePhysiqueLab.Nationalite?.IsoFrenchName
                            : personnePhysiqueLab.Nationalite?.IsoEnglishName);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(10)?.SetCellValue(lang == "fr"
                            ? personnePhysiqueLab.AutreNationalitePersonnePhysiqueLabs.FirstOrDefault()?.Pays
                                ?.IsoFrenchName
                            : personnePhysiqueLab.AutreNationalitePersonnePhysiqueLabs.FirstOrDefault()?.Pays
                                ?.IsoEnglishName);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(11)?.SetCellValue(lang == "fr"
                            ? personnePhysiqueLab.Sexe?.FrenchName
                            : personnePhysiqueLab.Sexe?.EnglishName);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(12)?.SetCellValue(lang == "fr"
                            ? personnePhysiqueLab.SituationFamiliale?.FrenchName
                            : personnePhysiqueLab.SituationFamiliale?.EnglishName);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(13)?.SetCellValue(lang == "fr"
                            ? personnePhysiqueLab.SecteurProfessionnel?.FrenchName
                            : personnePhysiqueLab.SecteurProfessionnel?.EnglishName);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(14)?.SetCellValue(personnePhysiqueLab
                            .ActiviteProfessionnellePersonnePhysiqueLabs.FirstOrDefault()?.ActiviteProfessionnelle);
                        if (personnePhysiqueLab.ActiviteProfessionnellePersonnePhysiqueLabs.Count > 1)
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(15)?.SetCellValue(
                                personnePhysiqueLab.ActiviteProfessionnellePersonnePhysiqueLabs.ToArray()[1]
                                    ?.ActiviteProfessionnelle);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(16)
                            ?.SetCellValue(personnePhysiqueLab.Ppe == true ? oui : non);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(17)?.SetCellValue(lang == "fr"
                            ? personnePhysiqueLab.DossierLabPersonnePhysiques.FirstOrDefault()?.TypeListeCriblage
                                ?.FrenchName
                            : personnePhysiqueLab.DossierLabPersonnePhysiques.FirstOrDefault()?.TypeListeCriblage
                                ?.EnglishName);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(18)
                            ?.SetCellValue(personnePhysiqueLab.DroitAuCompte ? oui : non);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(19)
                            ?.SetCellValue(personnePhysiqueLab.DetentionCoffre ? oui : non);
                        var coordonnee = personnePhysiqueLab.CoordonneePersonnePhysiqueLabs.Select(x => x.Coordonnee)
                            .FirstOrDefault();
                        if (coordonnee != null)
                        {
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(20)?.SetCellValue(lang == "fr"
                                ? coordonnee.TypeAdresse?.FrenchName
                                : coordonnee.TypeAdresse?.EnglishName);
                            if (coordonnee.NumeroVoie > 0)
                                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(21)
                                    ?.SetCellValue(coordonnee.NumeroVoie.ToString());
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(22)?.SetCellValue(lang == "fr"
                                ? coordonnee.ComplementVoie?.FrenchName
                                : coordonnee.ComplementVoie?.EnglishName);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(23)?.SetCellValue(lang == "fr"
                                ? coordonnee.TypeVoie?.FrenchName
                                : coordonnee.TypeVoie?.EnglishName);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(24)?.SetCellValue(coordonnee.Voie);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(25)
                                ?.SetCellValue(coordonnee.Complement);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(26)?.SetCellValue(coordonnee.Ville);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(27)?.SetCellValue(lang == "fr"
                                ? coordonnee.Pays?.IsoFrenchName
                                : coordonnee.Pays?.IsoEnglishName);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(28)
                                ?.SetCellValue(coordonnee.CodePostal);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(29)
                                ?.SetCellValue(coordonnee.TelephoneFixe);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(30)
                                ?.SetCellValue(coordonnee.TelephoneMobile);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(31)?.SetCellValue(coordonnee.Email);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(32)
                                ?.SetCellValue(coordonnee.TelephoneProfessionnel);
                        }

                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(33)?.SetCellValue(
                            Encoding.UTF8.GetString(personnePhysiqueLab.SurfaceFinanciere ?? Array.Empty<byte>())
                                .HTMLToText());
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(34)?.SetCellValue(lang == "fr"
                            ? personnePhysiqueLab.TypeClient?.FrenchName
                            : personnePhysiqueLab.TypeClient?.EnglishName);
                        if (personnePhysiqueLab.DateEntreeEnRelation != null)
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(35)?.SetCellValue(
                                personnePhysiqueLab.DateEntreeEnRelation.Value.DateTime.ToString("dd/MM/yyyy"));
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(36)?.SetCellValue(lang == "fr"
                            ? personnePhysiqueLab.CanalEntreeEnRelation?.FrenchName
                            : personnePhysiqueLab.CanalEntreeEnRelation?.EnglishName);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(37)
                            ?.SetCellValue(personnePhysiqueLab.CessationRelation ? oui : non);
                        if (personnePhysiqueLab.DateCessationRelations != null)
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(38)?.SetCellValue(
                                personnePhysiqueLab.DateCessationRelations.Value.DateTime.ToString("dd/MM/yyyy"));
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(39)?.SetCellValue(
                            Encoding.UTF8.GetString(personnePhysiqueLab.ElementClesRelation ?? Array.Empty<byte>())
                                .HTMLToText());
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(40)
                            ?.SetCellValue(personnePhysiqueLab.CompteEko ? oui : non);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(41)
                            ?.SetCellValue(personnePhysiqueLab.IsHabituel ? habituel : occasionnel);
                        i++;
                    }

                    var pieceIdentites = new List<PieceIdentite>();
                    personnePhysiqueLabs.ForEach(x => { pieceIdentites.AddRange(x.PieceIdentites); });

                    if (pieceIdentites.Count == 0)
                    {
                        hssfwb.RemoveSheetAt(indexGetSheet + 1);
                    }
                    else
                    {
                        indexGetSheet++;
                        i = 1;
                        foreach (var item in pieceIdentites)
                        {
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(0)
                                ?.SetCellValue(item.PersonnePhysiqueLab?.NomNaissance);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(1)
                                ?.SetCellValue(item.PersonnePhysiqueLab?.Prenoms);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(2)?.SetCellValue(lang == "fr"
                                ? item.TypePieceIdentite?.FrenchName
                                : item.TypePieceIdentite?.EnglishName);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(3)?.SetCellValue(item.Numero);
                            if (item.DateValiditeDebut != null)
                                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(4)
                                    ?.SetCellValue(item.DateValiditeDebut.GetValueOrDefault().ToString("dd/MM/yyyy"));
                            if (item.DateValiditeFin != null)
                                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(5)
                                    ?.SetCellValue(item.DateValiditeFin.GetValueOrDefault().ToString("dd/MM/yyyy"));
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(6)?.SetCellValue(item.Autorite);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(7)?.SetCellValue(lang == "fr"
                                ? item.PaysDelivrance?.IsoFrenchName
                                : item.PaysDelivrance?.IsoEnglishName);
                            i++;
                        }
                    }

                    var supportFinancierPersonnePhysiques = new List<SupportFinancierPersonnePhysique>();
                    personnePhysiqueLabs.ForEach(x =>
                    {
                        supportFinancierPersonnePhysiques.AddRange(x.SupportFinancierPersonnePhysiques);
                    });
                    if (supportFinancierPersonnePhysiques.Count == 0)
                    {
                        hssfwb.RemoveSheetAt(indexGetSheet + 1);
                    }
                    else
                    {
                        indexGetSheet++;
                        i = 1;
                        foreach (var item in supportFinancierPersonnePhysiques)
                        {
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(0)
                                ?.SetCellValue(item.PersonnePhysiqueLab?.NomNaissance);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(1)
                                ?.SetCellValue(item.PersonnePhysiqueLab?.Prenoms);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(2)?.SetCellValue(lang == "fr"
                                ? item?.TypeCompte?.FrenchName
                                : item?.TypeCompte?.EnglishName);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(3)?.SetCellValue(item?.Iban);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(4)?.SetCellValue(lang == "fr"
                                ? item?.TypeLienSupport?.FrenchName
                                : item?.TypeLienSupport?.EnglishName);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(5)
                                ?.SetCellValue(item?.DateOuvertureCompte.GetValueOrDefault().ToString("dd/MM/yyyy"));
                            i++;
                        }
                    }

                    var lienPersonnePhysiques = new List<LienPersonnePhysique>();
                    personnePhysiqueLabs.ForEach(x => { lienPersonnePhysiques.AddRange(x.LienPersonnePhysiques); });
                    if (lienPersonnePhysiques.Count == 0)
                    {
                        hssfwb.RemoveSheetAt(indexGetSheet + 1);
                    }
                    else
                    {
                        indexGetSheet++;
                        i = 1;
                        foreach (var item in lienPersonnePhysiques)
                        {
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(0)
                                ?.SetCellValue(item.PersonnePhysiqueLab?.NomNaissance);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(1)
                                ?.SetCellValue(item.PersonnePhysiqueLab?.Prenoms);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(2)
                                ?.SetCellValue(Encoding.UTF8.GetString(item?.NomNaissance ?? Array.Empty<byte>()));
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(3)
                                ?.SetCellValue(Encoding.UTF8.GetString(item?.Prenom ?? Array.Empty<byte>()));
                            if (item.DateNaissance != null)
                                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(4)
                                    ?.SetCellValue(item.DateNaissance.Value.ToString("dd/MM/yyyy"));
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(5)?.SetCellValue(lang == "fr"
                                ? item?.TypeLienPersonnePhysiquePhysique?.FrenchName
                                : item?.TypeLienPersonnePhysiquePhysique?.EnglishName);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(6)?.SetCellValue(item?.Precisions);
                            i++;
                        }
                    }

                    var lienPersonneMorales = new List<LienPersonneMorale>();
                    personnePhysiqueLabs.ForEach(x => { lienPersonneMorales.AddRange(x.LienPersonneMorales); });
                    if (lienPersonneMorales.Count == 0)
                    {
                        hssfwb.RemoveSheetAt(indexGetSheet + 1);
                    }
                    else
                    {
                        indexGetSheet++;
                        i = 1;
                        foreach (var item in lienPersonneMorales)
                        {
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(0)
                                ?.SetCellValue(item.PersonnePhysiqueLab?.NomNaissance);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(1)
                                ?.SetCellValue(item.PersonnePhysiqueLab?.Prenoms);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(2)
                                ?.SetCellValue(Encoding.UTF8.GetString(item?.RaisonSociale ?? Array.Empty<byte>()));
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(3)?.SetCellValue(lang == "fr"
                                ? item?.FormeJuridique?.FrenchName
                                : item?.FormeJuridique?.EnglishName);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(4)
                                ?.SetCellValue(Encoding.UTF8.GetString(item?.Immatriculation ?? Array.Empty<byte>()));
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(5)?.SetCellValue(lang == "fr"
                                ? item?.TypeLienPersonneMoralePhysique?.FrenchName
                                : item?.TypeLienPersonneMoralePhysique?.EnglishName);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(6)?.SetCellValue(item?.Precisions);
                            i++;
                        }
                    }
                }

                //Modification Tiers Personne(s) Morale(s)
                var personnesMorales = await _labService.DossierLabPersonneMorale
                    .GetDossierLabPersonneMoraleByDetailsAsync(dossierLab.Id).ConfigureAwait(false);
                var personneMoraleLabs = personnesMorales.ToList();
                if (personnesMorales != null && !personneMoraleLabs.Any())
                {
                    hssfwb.RemoveSheetAt(indexGetSheet + 5);
                    hssfwb.RemoveSheetAt(indexGetSheet + 4);
                    hssfwb.RemoveSheetAt(indexGetSheet + 3);
                    hssfwb.RemoveSheetAt(indexGetSheet + 2);
                    hssfwb.RemoveSheetAt(indexGetSheet + 1);
                }
                else
                {
                    indexGetSheet++;
                    i = 1;
                    foreach (var personneMoraleLab in personneMoraleLabs)
                    {
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(0)
                            ?.SetCellValue(personneMoraleLab?.RaisonSociale);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(1)
                            ?.SetCellValue(
                                Encoding.UTF8.GetString(personneMoraleLab?.Sigle ?? Array.Empty<byte>()).HTMLToText());
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(2)
                            ?.SetCellValue(dossierLab.IsDeclarationSoupcon ? oui : non);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(3)
                            ?.SetCellValue(personneMoraleLab?.NumeroImmatriculation);
                        if (personneMoraleLab.DateImmatriculation != null)
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(4)
                                ?.SetCellValue(
                                    personneMoraleLab.DateImmatriculation.Value.DateTime.ToString("dd/MM/yyyy"));

                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(5)?.SetCellValue(lang == "fr"
                            ? personneMoraleLab?.FormeJuridique?.FrenchName
                            : personneMoraleLab?.FormeJuridique?.EnglishName);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(6)?.SetCellValue(lang == "fr"
                            ? personneMoraleLab?.SecteurProfessionnel?.FrenchName
                            : personneMoraleLab?.SecteurProfessionnel?.EnglishName);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(7)
                            ?.SetCellValue(personneMoraleLab?.Activite);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(8)?.SetCellValue(lang == "fr"
                            ? personneMoraleLab?.Pays?.IsoFrenchName
                            : personneMoraleLab?.Pays?.IsoEnglishName);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(9)
                            ?.SetCellValue(personneMoraleLab?.IdentifiantTvaUe);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(10)?.SetCellValue(lang == "fr"
                            ? personneMoraleLab?.TypeClient?.FrenchName
                            : personneMoraleLab?.TypeClient?.EnglishName);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(11)
                            ?.SetCellValue(personneMoraleLab?.IdentifiantClient);
                        if (personneMoraleLab.DateEntreeEnRelation != null)
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(12)
                                ?.SetCellValue(
                                    personneMoraleLab.DateEntreeEnRelation.Value.DateTime.ToString("dd/MM/yyyy"));
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(13)?.SetCellValue(lang == "fr"
                            ? personneMoraleLab?.CanalEntreeEnRelation?.FrenchName
                            : personneMoraleLab?.CanalEntreeEnRelation?.EnglishName);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(14)
                            ?.SetCellValue(personneMoraleLab?.CessationRelation == true ? oui : non);
                        if (personneMoraleLab.DateCessationRelations != null)
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(15)?.SetCellValue(
                                personneMoraleLab.DateCessationRelations.Value.DateTime.ToString("dd/MM/yyyy"));
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(16)
                            ?.SetCellValue(personneMoraleLab?.DetentionCoffre == true ? oui : non);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(17)?.SetCellValue(lang == "fr"
                            ? personneMoraleLab?.DossierLabPersonneMorales.FirstOrDefault()?.TypeListeCriblage
                                ?.FrenchName
                            : personneMoraleLab?.DossierLabPersonneMorales.FirstOrDefault()?.TypeListeCriblage
                                ?.EnglishName);
                        hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(18)
                            ?.SetCellValue(personneMoraleLab?.DroitAuCompte == true ? oui : non);
                        if (personneMoraleLab?.Coordonnee != null)
                        {
                            if (personneMoraleLab?.Coordonnee.NumeroVoie > 0)
                                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(19)
                                    ?.SetCellValue(personneMoraleLab.Coordonnee?.NumeroVoie.ToString());
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(20)?.SetCellValue(lang == "fr"
                                ? personneMoraleLab.Coordonnee?.ComplementVoie?.FrenchName
                                : personneMoraleLab.Coordonnee?.ComplementVoie?.EnglishName);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(21)?.SetCellValue(lang == "fr"
                                ? personneMoraleLab.Coordonnee?.TypeVoie?.FrenchName
                                : personneMoraleLab.Coordonnee?.TypeVoie?.EnglishName);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(22)
                                ?.SetCellValue(personneMoraleLab.Coordonnee?.Voie);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(23)
                                ?.SetCellValue(personneMoraleLab.Coordonnee?.Complement);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(24)
                                ?.SetCellValue(personneMoraleLab.Coordonnee?.Ville);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(25)?.SetCellValue(lang == "fr"
                                ? personneMoraleLab.Coordonnee?.Pays?.IsoFrenchName
                                : personneMoraleLab.Coordonnee?.Pays?.IsoEnglishName);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(26)
                                ?.SetCellValue(personneMoraleLab.Coordonnee?.CodePostal);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(27)
                                ?.SetCellValue(personneMoraleLab.Coordonnee?.TelephoneFixe);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(28)
                                ?.SetCellValue(personneMoraleLab.Coordonnee?.Fax);
                        }

                        i++;
                    }

                    var representantLegals = new List<RepresentantLegal>();
                    personneMoraleLabs.ForEach(x => { representantLegals.AddRange(x.RepresentantLegals); });

                    if (representantLegals.Count == 0)
                    {
                        hssfwb.RemoveSheetAt(indexGetSheet + 1);
                    }
                    else
                    {
                        indexGetSheet++;
                        i = 1;
                        foreach (var item in representantLegals)
                        {
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(0)
                                ?.SetCellValue(item.PersonneMoraleLab?.RaisonSociale);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(1)
                                ?.SetCellValue(item.PersonneMoraleLab?.NumeroImmatriculation);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(2)
                                ?.SetCellValue(Encoding.UTF8.GetString(item?.NomNaissance ?? Array.Empty<byte>()));
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(3)
                                ?.SetCellValue(Encoding.UTF8.GetString(item?.Prenoms ?? Array.Empty<byte>()));
                            if (item.DateNaissance != null)
                                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(4)
                                    ?.SetCellValue(item.DateNaissance.GetValueOrDefault().ToString("dd/MM/yyyy"));
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(5)
                                ?.SetCellValue(Encoding.UTF8.GetString(item?.VilleNaissance ?? Array.Empty<byte>()));
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(6)?.SetCellValue(lang == "fr"
                                ? item?.PaysNaissance?.IsoFrenchName
                                : item?.PaysNaissance?.IsoEnglishName);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(7)?.SetCellValue(item?.Fonction);
                            i++;
                        }
                    }

                    var supportFinancierPersonneMorales = new List<SupportFinancierPersonneMorale>();
                    personneMoraleLabs.ForEach(x =>
                    {
                        supportFinancierPersonneMorales.AddRange(x.SupportFinancierPersonneMorales);
                    });
                    if (supportFinancierPersonneMorales.Count == 0)
                    {
                        hssfwb.RemoveSheetAt(indexGetSheet + 1);
                    }
                    else
                    {
                        indexGetSheet++;
                        i = 1;
                        foreach (var item in supportFinancierPersonneMorales)
                        {
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(0)
                                ?.SetCellValue(item.PersonneMoraleLab?.RaisonSociale);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(1)
                                ?.SetCellValue(item.PersonneMoraleLab?.NumeroImmatriculation);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(2)
                                ?.SetCellValue(Encoding.UTF8.GetString(item?.Nom ?? Array.Empty<byte>()));
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(3)
                                ?.SetCellValue(Encoding.UTF8.GetString(item?.Prenom ?? Array.Empty<byte>()));
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(4)
                                ?.SetCellValue(item?.DateNaissance.GetValueOrDefault().ToString("dd/MM/yyyy"));
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(5)
                                ?.SetCellValue(Encoding.UTF8.GetString(item?.VilleNaissance ?? Array.Empty<byte>()));
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(6)?.SetCellValue(lang == "fr"
                                ? item?.TypeLienSupport?.FrenchName
                                : item?.TypeLienSupport?.EnglishName);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(7)?.SetCellValue(lang == "fr"
                                ? item?.TypeCompte?.FrenchName
                                : item?.TypeCompte?.EnglishName);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(8)?.SetCellValue(item?.Iban);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(9)
                                ?.SetCellValue(item?.DateOuvertureCompte.GetValueOrDefault().ToString("dd/MM/yyyy"));
                            i++;
                        }
                    }

                    var lienPersonnePhysiques = new List<LienPersonnePhysique>();
                    personneMoraleLabs.ForEach(x => { lienPersonnePhysiques.AddRange(x.LienPersonnePhysiques); });
                    if (lienPersonnePhysiques.Count == 0)
                    {
                        hssfwb.RemoveSheetAt(indexGetSheet + 1);
                    }
                    else
                    {
                        indexGetSheet++;
                        i = 1;
                        foreach (var item in lienPersonnePhysiques)
                        {
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(0)
                                ?.SetCellValue(item.PersonneMoraleLab?.RaisonSociale);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(1)
                                ?.SetCellValue(item.PersonneMoraleLab?.NumeroImmatriculation);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(2)
                                ?.SetCellValue(Encoding.UTF8.GetString(item?.NomNaissance ?? Array.Empty<byte>()));
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(3)
                                ?.SetCellValue(Encoding.UTF8.GetString(item?.Prenom ?? Array.Empty<byte>()));
                            if (item.DateNaissance != null)
                                hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(4)
                                    ?.SetCellValue(item.DateNaissance.Value.ToString("dd/MM/yyyy"));
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(5)
                                ?.SetCellValue(item?.TypeLienPersonnePhysiqueMorale?.FrenchName);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(6)?.SetCellValue(item?.Precisions);
                            i++;
                        }
                    }

                    var lienPersonneMorales = new List<LienPersonneMorale>();
                    personneMoraleLabs.ForEach(x => { lienPersonneMorales.AddRange(x.LienPersonneMorales); });
                    if (lienPersonneMorales.Count == 0)
                    {
                        hssfwb.RemoveSheetAt(indexGetSheet + 1);
                    }
                    else
                    {
                        indexGetSheet++;
                        i = 1;
                        foreach (var item in lienPersonneMorales)
                        {
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(0)
                                ?.SetCellValue(item.PersonneMoraleLab?.RaisonSociale);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(1)
                                ?.SetCellValue(item.PersonneMoraleLab?.NumeroImmatriculation);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(2)
                                ?.SetCellValue(Encoding.UTF8.GetString(item?.RaisonSociale ?? Array.Empty<byte>()));
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(3)?.SetCellValue(lang == "fr"
                                ? item?.FormeJuridique?.FrenchName
                                : item?.FormeJuridique?.EnglishName);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(4)
                                ?.SetCellValue(Encoding.UTF8.GetString(item?.Immatriculation ?? Array.Empty<byte>()));
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(5)?.SetCellValue(lang == "fr"
                                ? item?.TypeLienPersonneMoraleMorale?.FrenchName
                                : item?.TypeLienPersonneMoraleMorale?.EnglishName);
                            hssfwb.GetSheetAt(indexGetSheet)?.GetRow(i)?.GetCell(6)?.SetCellValue(item?.Precisions);
                            i++;
                        }
                    }
                }

                await using (var file = new FileStream(newFileSortis, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    hssfwb.Write(file);
                    file.Close();
                }

                // the output bytes of the zip
                byte[] fileBytes;
                var documents = await _labService.Dossier.GetDocumentDossierLabs(dossierLab.Id).ConfigureAwait(false);
                // create a working memory stream
                using (var memoryStream = new MemoryStream())
                {
                    // create a zip
                    using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        // interate through the source files
                        foreach (var f in documents)
                        {
                            // add the item name to the zip
                            var zipItem = zip.CreateEntry(f.FileName);
                            // add the item bytes to the zip entry by opening the original file and copying the bytes
                            using var originalFileMemoryStream = new MemoryStream(f.DocumentLab.FileContent);
                            await using var entryStream = zipItem.Open();
                            await originalFileMemoryStream.CopyToAsync(entryStream);
                        }

                        zip.CreateEntryFromFile(newFileSortis, Path.GetFileName(newFileSortis),
                            CompressionLevel.Optimal);
                    }

                    fileBytes = memoryStream.ToArray();
                }

                // download the constructed zip
                Response.Headers.Add("Content-Disposition",
                    "attachment; filename=Lab_dossier_" + dossierLab.CodeUnique + ".zip");
                if (System.IO.File.Exists(newFileSortis))
                    System.IO.File.Delete(newFileSortis);

                return File(fileBytes, "application/zip");
            }

            return null;
        }

        [HttpGet]
        public async Task<IActionResult> DownloadAllLabDocuments()
        {
            // the output bytes of the zip
            try
            {
                var documents = await _labService.Dossier.GetDocumentIdentiteDossierLabs().ConfigureAwait(false);
                // create a working memory stream
                byte[] fileBytes;
                using (var memoryStream = new MemoryStream())
                {
                    // create a zip
                    using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        // interate through the source files
                        foreach (var f in documents)
                        {
                            // add the item name to the zip
                            var zipItem = zip.CreateEntry(f.FileName);
                            // add the item bytes to the zip entry by opening the original file and copying the bytes
                            using var originalFileMemoryStream =
                                new MemoryStream(f.DocumentLab?.FileContent ?? Array.Empty<byte>());
                            await using var entryStream = zipItem.Open();
                            await originalFileMemoryStream.CopyToAsync(entryStream);
                        }
                    }

                    fileBytes = memoryStream.ToArray();
                }

                // download the constructed zip
                Response.Headers.Add("Content-Disposition", "attachment; filename=lab_dossier_all.zip");
                return File(fileBytes, "application/zip");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult AddDossierLabOperationForm(int order)
        {
            if (!IsVerifyUserHabilitation())
                return new JsonResult(new
                {
                    success = false,
                    status = false,
                    message = _translator.Common["PasDePersmissionActiviteDirection"]
                });
            var item = new DossierLabOperationViewModel
            {
                Culture = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture),
                DeviseId = 47
            };
            item.ProtectDossierLabOperation(_protector);
            return PartialView("/Areas/Lab/Views/Dossier/_PartialDossierLabOperation.cshtml",
                new Tuple<DossierLabOperationViewModel, int>(item, order));
        }

        [HttpGet]
        public IActionResult AddDossierLabScenarioForm(int order)
        {
            if (!IsVerifyUserHabilitation())
                return new JsonResult(new
                {
                    success = false,
                    status = false,
                    message = _translator.Common["PasDePersmissionActiviteDirection"]
                });
            var item = new DossierLabScenarioViewModel
            {
                Culture = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture)
            };
            item.ProtectDossierLabScenario(_protector);
            return PartialView("/Areas/Lab/Views/Dossier/_PartialDossierLabScenario.cshtml",
                new Tuple<DossierLabScenarioViewModel, int>(item, order));
        }

        public async Task<IActionResult> AddDeclarationTracfinForm(int order, int directionId)
        {
            var organismeLab = _mapper.Map<OrganismeLabViewModel>(await _labService.Dossier
                .GetOrganismeLabByDirectionAsync(directionId).ConfigureAwait(false));
            if (organismeLab != null)
                organismeLab.Culture = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture);

            var item = new DeclarationTracfinViewModel
            {
                Culture = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture),
                OrganismeLab = organismeLab,
                DeviseId = 47 /*Euro*/,
                IsExamenRonforce = false
            };
            item.ProtectDossierLabTracfin(_protector);

            return PartialView("/Areas/Lab/Views/Dossier/_PartialDeclarationTracfinView.cshtml",
                new Tuple<DeclarationTracfinViewModel, int>(item, order));
        }

        public async Task<IActionResult> AddDeclarationTracfinFormNew(int order, int directionId)
        {
            if (!IsVerifyUserHabilitation())
                return new JsonResult(new
                {
                    success = false,
                    status = false,
                    message = _translator.Common["PasDePersmissionActiviteDirection"]
                });

            var organismeLab = _mapper.Map<OrganismeLabViewModel>(await _labService.Dossier
                .GetOrganismeLabByDirectionAsync(directionId).ConfigureAwait(false));
            if (organismeLab != null)
                organismeLab.Culture = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture);

            var item = new DeclarationTracfinViewModel
            {
                Culture = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture),
                OrganismeLab = organismeLab,
                DeviseId = 47 /*Euro*/,
                IsExamenRonforce = true,
                IsSoupconNonFinanciers = true,
                IsDirectionTracfin = _referentielService.Direction.IsTracfin(directionId)
            };
            item.ProtectDossierLabTracfin(_protector);

            return PartialView("/Areas/Lab/Views/Dossier/_PartialDeclarationTracfinViewNew.cshtml",
                new Tuple<DeclarationTracfinViewModel, int>(item, order));
        }

        public Task<JsonResult> GetProfessionDirection(int directionId,
            CancellationToken cancellationToken = default)
        {
            var isAssurance = false;
            var isBanque = false;
            var isImmobilier = false;
            var profession = _labService.Dossier.GetDirectionProfession(directionId);
            if (!string.IsNullOrEmpty(profession))
            {
                switch (profession)
                {
                    case "TPI6":
                    case "TPI4":
                        isAssurance = true;
                        break;
                    case "TPI26":
                        isImmobilier = true;
                        break;
                    default:
                        isBanque = true;
                        break;
                }
            }
            else
            {
                return Task.FromResult(new JsonResult(new { status = false, isAssurance = false, isBanque = true, isImmobilier = false }));
            }


            return Task.FromResult(new JsonResult(new { status = true, isAssurance, isBanque, isImmobilier }));
        }

        public IActionResult AddCoordonneePersonnePhysiqueForm(int orderPersonnePhysique, int order)
        {
            var item = new CoordonneePersonnePhysiqueLabViewModel
            {
                Culture = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture),
                Coordonnee = new CoordonneeViewModel
                {
                    TypeAdresseId = 1
                },
                Order = order
            };

            item.ProtectCoordonnee(_protector);

            return PartialView("/Areas/Lab/Views/Dossier/_PartialCoordonneePersonnePhysiqueLab.cshtml",
                new Tuple<CoordonneePersonnePhysiqueLabViewModel, int>(item, orderPersonnePhysique));
        }

        public IActionResult AddDirigeantPersonneMoraleForm(int orderPersonneMorale, int order,
            string cryptedDossierLabId, string cryptedPersonneMoraleLabId)
        {
            var item = new DirigeantViewModel
            {
                Culture = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture),
                Order = order,
                Id = 0,
                PersonneMoraleLabId = 0,
                CryptedDossierId = cryptedDossierLabId,
                CryptedPersonneMoraleLabId = cryptedPersonneMoraleLabId
            };

            //item.ProtectDirigeant(_protector);

            return PartialView("/Areas/Lab/Views/Dossier/_PartialDirigeantPersonneMoraleLab.cshtml",
                new Tuple<DirigeantViewModel, int>(item, orderPersonneMorale));
        }

        public IActionResult AddCoordonneePersonneMoraleForm(int orderPersonneMorale)
        {
            var item = new CoordonneeViewModel { Lang = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture) };
            return PartialView("/Areas/Lab/Views/Dossier/_PartialCoordonneePersonneMoraleLab.cshtml",
                new Tuple<CoordonneeViewModel, int>(item, orderPersonneMorale));
        }

        public IActionResult AddActiviteCoordonneePersonnePhysiqueForm(int orderPersonnePhysique)
        {
            var item = new PersonnePhysiqueLabViewModel
            { Lang = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture) };
            return PartialView("/Areas/Lab/Views/Dossier/_PartialActiviteCoordonneePersonnePhysiqueLab.cshtml",
                new Tuple<PersonnePhysiqueLabViewModel, int>(item, orderPersonnePhysique));
        }

        public JsonResult GetUserFullName()
        {
            var userFullName = currentUser.FullName;

            return Json(userFullName);
        }

        [HttpGet]
        public async Task<IList<UtilisateurViewModel>> GetUtilisateurByMode(int mode)
        {
            _logger.BeginTrace();

            IList<UtilisateurViewModel> utilisateurs = null;
            try
            {
                if (!IsVerifyUserHabilitation())
                    return null;
                if (_roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal))
                {
                    utilisateurs = _referential.Utilisateurs.ToList();
                }
                else
                {
                    if (mode == (int)EditionMode.Management)
                    {
                        var isValidateur = await _labService.UtilisateurDirectionService
                            .IsValidateurLab(currentUser.Id)
                            .ConfigureAwait(false);
                        var isAdminReferentiel =
                            _roleAccessUser.HasClaims(ActiviteModule.Lab, ClaimUser.ADMINREFERENTIEL);
                        if (isValidateur || isAdminReferentiel)
                        {
                            var utls = _labService.Referentiel
                                .GetUtilisateurAdminLocalValidationAsync(currentUser.Id, (int)ActiviteModule.Lab);
                            utilisateurs = _mapper.Map<List<UtilisateurViewModel>>(utls);
                        }
                        else
                        {
                            var utilisateursHabilitations = await _labService.Referentiel
                                .GetUtilisateurDelegueByActivite((int)ActiviteModule.Lab, currentUser.Id)
                                .ConfigureAwait(false);
                            utilisateurs = _mapper.Map<IList<UtilisateurViewModel>>(utilisateursHabilitations);
                        }

                        utilisateurs.Add(currentUser);
                    }
                    else
                    {
                        var utilisateursPartages = await _labService.Referentiel
                            .GetUtilisateursAssociated((int)ActiviteModule.Lab, currentUser.Id).ConfigureAwait(false);
                        utilisateurs = _mapper.Map<List<UtilisateurViewModel>>(utilisateursPartages);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();
            return utilisateurs.DistinctBy(x => x.Id).ToList();
        }

        [HttpGet]
        public Task<UtilisateurViewModel> GetConnectedUser()
        {
            return Task.FromResult(currentUser);
        }

        [HttpGet]
        public async Task<JsonResult> LoadStatutDossierLab(int directionId, bool isValidation, bool onlyActive,
            int statutId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            var statutDossierLabs = new List<StatutDossierLabViewModel>();
            try
            {
                if (!IsVerifyUserHabilitation())
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["PasDePersmissionActiviteDirection"]
                    });
                var userPrivilege = new UserPrivilege(_labService, currentUser.Id, directionId);
                await userPrivilege.Initialize(User, cancellationToken).ConfigureAwait(false);

                if (onlyActive)
                {
                    var listStatutDossierLab = _referential.StatutDossierLabs.Where(x => x.IsActive).ToList();

                    if (statutId == (int)StatutDossierLabEnum.clotureTracfin ||
                        statutId == (int)StatutDossierLabEnum.EncoursEnvoiDS ||
                        statutId == (int)StatutDossierLabEnum.AttenteARTracfin)
                    {
                        statutDossierLabs.AddRange(listStatutDossierLab.Where(x =>
                            x.Id == statutId));
                    }
                    //IsValideur: retourner les statuts CLOTURE, SANS_SUITE, ATTENTE_DOCUMENTS, EN_COURS, ATTENTE_VALIDATION (2,5,4,3,6) quelque soit isvalidation de la categorie 
                    //IsPending: ATTENTE_PRISE_EN_CHARGE, PENDING_EN_COURS (1,7)
                    //!IsValideur && !IsPending : !IsValidation: CLOTURE, SANS_SUITE, ATTENTE_DOCUMENTS, EN_COURS (2,5,4,3)
                    //!IsValideur && !IsPending : IsValidation: ATTENTE_VALIDATION,ATTENTE_DOCUMENTS,EN_COURS (3,4,6)
                    else if (userPrivilege.Write || _roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal))
                    {
                        if (userPrivilege.IsValideur)
                            statutDossierLabs.AddRange(listStatutDossierLab.Where(x =>
                                x.Id is 2 or 3 or 4 or 5 or 6));
                        else if (_roleAccessUser.HasRoles(null, null, RoleUser.Robot))
                            statutDossierLabs.AddRange(listStatutDossierLab.Where(x => x.Id is 1));
                        else if (userPrivilege.IsPending)
                            statutDossierLabs.AddRange(listStatutDossierLab.Where(x => x.Id is 1 or 7));
                        else if (isValidation)
                            statutDossierLabs.AddRange(listStatutDossierLab.Where(x =>
                                x.Id is 3 or 4 or 6));
                        else
                            statutDossierLabs.AddRange(listStatutDossierLab.Where(x =>
                                x.Id is 2 or 3 or 4 or 5));
                    }
                    else if (userPrivilege.Read ||
                             _roleAccessUser.HasRoles(ActiviteModule.Lab, null, RoleUser.AuditGlobal))
                    {
                        statutDossierLabs.AddRange(listStatutDossierLab);
                    }
                }
                else
                {
                    statutDossierLabs.AddRange(_referential.StatutDossierLabs.Where(x =>
                        x.Id == statutId));
                }
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
                return new JsonResult(new { status = false });
            }

            _logger.EndTrace();
            return new JsonResult(new { status = true, statutDossierLabs });
        }

        [HttpGet]
        public async Task<IActionResult> ValidationRefusDossierLab(string cryptedId)
        {
            var model = new EditValidationRefusDossierLabViewModel();

            var id = this.Uprotect(cryptedId, _protector);
            var dossierLab = await _labService.Dossier.GetAsync(id, false).ConfigureAwait(false);
            var dossierLabMapped = _mapper.Map<DossierLab>(dossierLab);
            if (dossierLabMapped != null && !IsVerifyUserHabilitation())
                return new JsonResult(new
                {
                    success = false,
                    status = false,
                    message = _translator.Common["PasDePersmissionActiviteDirection"]
                });


            if (dossierLabMapped != null)
                model.IsNewDeclarationTracfin = dossierLabMapped.IsDeclarationSoupcon &&
                                                dossierLabMapped.DeclarationTracfins.FirstOrDefault() is
                                                { EstNouvelleDeclarationTracfin: true } &&
                                                dossierLabMapped.Direction.IsTracfin;


            model.CryptedDossierLabId = cryptedId;
            model.Lang = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture);
            model.StatutDatasources = new List<SelectedItem>
            {
                new() { Id = 4, NameFr = "En cours", NameEn = "In progress" },
                new() { Id = 5, NameFr = "Sans suite", NameEn = "Without continuation" },
                new()
                {
                    Id = 2, NameFr = model.IsNewDeclarationTracfin ? "Attente d'envoi TRACFIN" : "Cloturé",
                    NameEn = model.IsNewDeclarationTracfin ? "Waiting for sending to TRACFIN" : "Closed"
                }
            };
            return PartialView("/Areas/Lab/Views/Dossier/_PartialConfirmationValidationRefusDossierLab.cshtml", model);
        }

        [HttpPost]
        public async Task<JsonResult> ConfirmValidationRefusDossierLab(EditValidationRefusDossierLabViewModel model,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            try
            {
                if (model == null)
                    throw new ArgumentNullException(nameof(model));
                var eventDossier = new EventDossierViewModel
                { DateCreation = DateTime.UtcNow, IsActive = true, ActiviteId = (int)ActiviteModule.Lab };

                var id = this.Uprotect(model.CryptedDossierLabId, _protector);

                var dossier = await _labService.Dossier.GetAsync(id, true, cancellationToken).ConfigureAwait(false);
                var estNouvelleDs = false;
                if (dossier.DeclarationTracfins.Any())
                    estNouvelleDs = dossier.DeclarationTracfins.All(x => x.EstNouvelleDeclarationTracfin);
                if (model.StatutId == 0)
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["StatutObligatoire"]
                    });

                if (!IsVerifyUserHabilitation())
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["PasDePersmissionActiviteDirection"]
                    });


                var oldResponsable = dossier.Utilisateur;

                dossier.EntiteLab = null;
                dossier.StatutDossier = null;
                dossier.StatutDossierId = model.StatutId;
                dossier.ModificateurId = currentUser.Id;
                dossier.DateModification = DateTimeOffset.Now;
                dossier.Utilisateur = null;
                dossier.Modificateur = null;

                eventDossier.DossierLabId = id;
                eventDossier.CodeDossier = dossier.CodeUnique;
                eventDossier.CreateurId = currentUser.Id;
                eventDossier.UtilisateurEventId = currentUser.Id;

                switch (model.StatutId)
                {
                    case (int)StatutDossierLabEnum.Cloture:

                        eventDossier.EventDossierTypeId = estNouvelleDs && dossier.Direction.IsTracfin
                            ? (int)EactionEventTypeDossier.ATTENTE_ENVOI_TRACFIN
                            : (int)EactionEventTypeDossier.CLOTURE_DOSSIER_LAB;

                        dossier.DateCloture = estNouvelleDs ? null : DateTime.UtcNow;

                        break;
                    case (int)StatutDossierLabEnum.EnCours:
                        eventDossier.EventDossierTypeId = (int)EactionEventTypeDossier.EN_COURS_DOSSIER_LAB;
                        break;
                    case (int)StatutDossierLabEnum.SansSuite:
                        eventDossier.EventDossierTypeId = (int)EactionEventTypeDossier.SANS_SUITE_DOSSIER_LAB;
                        break;
                }

                _labService.Dossier.UpdateCore(dossier);

                var eventEnt = _mapper.Map<EventDossier>(eventDossier);
                await _labService.EventDossier.AddAsync(eventEnt, cancellationToken).ConfigureAwait(false);


                var statusDossier =
                    _referential.StatutDossierLabs.SingleOrDefault(x => x.Id == dossier.StatutDossierId);

                var libelleStatus = this.IsCultureFr() ? statusDossier?.NameFr : statusDossier?.NameEn;

                var libelleModificateur =
                    $"{currentUser.Prenom} {currentUser.Nom.ToUpper(CultureInfo.CurrentCulture)}";

                try
                {
                    var notifyTypeId =
                        model.StatutId == (int)StatutDossierLabEnum.Cloture ||
                        model.StatutId == (int)StatutDossierLabEnum.SansSuite
                            ? (int)NotificationType.ValidationDossierLab
                            : (int)NotificationType.NoValidationDossierLab;
                    var emailTemplate = await _labService.EmailNotificationTemplate
                        .GetEmailNotification((int)ActiviteModule.Lab, notifyTypeId, currentUser.LangueId,
                            cancellationToken).ConfigureAwait(false);

                    if (emailTemplate != null)
                    {
                        var emailNotification = new EmailNotification
                        {
                            DateNotification = DateTimeOffset.UtcNow,
                            EmailTo = oldResponsable.Email,
                            Subject = string.Format(emailTemplate.Subject, dossier.CodeUnique),
                            Body = notifyTypeId == (int)NotificationType.NoValidationDossierLab
                                ? string.Format(CultureInfo.CurrentCulture, emailTemplate.Body, dossier.CodeUnique,
                                    $"{currentUser.Prenom} {currentUser.Nom}", model.MotifValidation)
                                : string.Format(CultureInfo.CurrentCulture, emailTemplate.Body, dossier.CodeUnique,
                                    $"{currentUser.Prenom} {currentUser.Nom}")
                        };
                        await _labService.EmailNotification.PushNotification(emailNotification, cancellationToken)
                            .ConfigureAwait(false);
                    }
                    else
                    {
                        _logger.LogInformation(
                            $" *** Validation Dossier Lab: Le template notification mail introuvable pour la  validation dossier Lab de notificationTypeId : {(int)NotificationType.ValidationDossierLab} du dossier code unique : {dossier.CodeUnique}");
                    }
                }
                catch (Exception e)
                {
                    _logger.TraceError(e);
                    _logger.LogInformation(
                        $" *** Validation Dossier Lab: erreor ajout du template notification mail pour la  validation dossier Lab de notificationTypeId : {(int)NotificationType.ValidationDossierLab} du dossier code unique : {dossier.CodeUnique}");
                }

                return new JsonResult(new
                {
                    success = true,
                    statusDossier = libelleStatus,
                    modificateur = libelleModificateur,
                    dateModification = DateTime.UtcNow,
                    dateCloture = dossier.DateCloture,
                    id = statusDossier?.Id
                });
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();

            return new JsonResult(new
            {
                success = false
            });
        }

        public async Task<bool> AddNotificationMailAttenteValidationDossier(string codeUnique, int directionId,
            CancellationToken cancellationToken = default)
        {
            var result = true;
            try
            {
                var utilisateurValideurs = await _labService.Referentiel
                    .GetUtilisateurValideurs(directionId, (int)ActiviteModule.Lab, cancellationToken)
                    .ConfigureAwait(false);
                var utilisateurDirections = utilisateurValideurs.ToList();
                if (!utilisateurDirections.Any())
                {
                    result = false;
                    _logger.LogInformation(
                        $" *** Validation Dossier Lab: nous n'avons pas trouvé d'utilisateurs validateur pour le dossier code unique : {codeUnique}");
                }
                else
                {
                    foreach (var validator in utilisateurDirections)
                    {
                        var emailTemplate = await _labService.EmailNotificationTemplate
                            .GetEmailNotification((int)ActiviteModule.Lab,
                                (int)NotificationType.ValidationAttenteDossierLab, validator.Utilisateur.LangueId,
                                cancellationToken).ConfigureAwait(false);

                        if (emailTemplate != null)
                        {
                            var emailNotification = new EmailNotification
                            {
                                DateNotification = DateTimeOffset.UtcNow,
                                EmailTo = validator.Utilisateur.Email,
                                Subject = string.Format(emailTemplate.Subject, codeUnique),
                                Body = string.Format(CultureInfo.CurrentCulture, emailTemplate.Body, codeUnique)
                            };

                            result = await _labService.EmailNotification
                                .PushNotification(emailNotification, cancellationToken)
                                .ConfigureAwait(false);
                        }
                        else
                        {
                            _logger.LogInformation(
                                $" *** Validation Dossier Lab: Le template notification mail introuvable pour la validation dossier Lab de notificationTypeId : {(int)NotificationType.ValidationAttenteDossierLab} du dossier code unique : {codeUnique}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                _logger.TraceError(e);
                _logger.LogInformation(
                    $" *** Validation Dossier Lab: error ajout du template notification mail pour la  validation dossier Lab de notificationTypeId : {(int)NotificationType.ValidationAttenteDossierLab} du dossier code unique : {codeUnique}");
            }

            return result;
        }

        [HttpPost]
        public async Task<JsonResult> ConfirmTransmissionDossierLab(EditTransmissionDossierLabViewModel model,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            try
            {
                if (model == null)
                    throw new ArgumentNullException(nameof(model));
                var eventDossier = new EventDossierViewModel
                {
                    DateCreation = DateTime.UtcNow,
                    IsActive = true,
                    ActiviteId = (int)ActiviteModule.Lab,
                    EventDossierTypeId = (int)EactionEventTypeDossier.UPDATE_TRANSMISSION_DOSSIER_LAB
                };

                var id = this.Uprotect(model.CryptedDossierLabId, _protector);
                var dossier = await _labService.Dossier.GetAsNoTrackingAsync(id, cancellationToken)
                    .ConfigureAwait(false);
                var estNouveauAccuseReception = dossier.DeclarationTracfins.Any() && dossier.DeclarationTracfins.All(x => x.NumeroAccuseReception != model.NumeroAccuseReception);

                if (model.IsDs)
                {
                    var declaration = await _labService.DeclarationTracfin
                        .GetDeclarationByDossierAsync(id, cancellationToken)
                        .ConfigureAwait(false);

                    declaration.IsTransmissionParquet = model.IsTransmissionParquet;
                    declaration.IsTransmissionAutorite = model.IsTransmissionAutorite;
                    declaration.NumeroAccuseReception = model.NumeroAccuseReception;
                    declaration.ReferenceInterne = model.ReferenceInterne;
                    declaration.DateDeclaration = model.DateDeclaration;
                    _labService.DeclarationTracfin.UpdateCore(declaration);
                }


                dossier.IsSuivi = model.IsSuivi;
                dossier.DateModification = DateTimeOffset.Now;
                dossier.ModificateurId = currentUser.Id;
                dossier.DateReception = model.DateDetection ?? dossier.DateReception;
                dossier.OrigineLabId = model.OrigineLabId;
                dossier.CategorieId = model.CategorieLabId;
                dossier.EntiteId = model.EntiteId;
                dossier.PaysId = model.PaysId;
                dossier.SecteurEconomiqueId = model.SecteurEconomiqueId;

                var estClotureTracfin = dossier.DeclarationTracfins.Any() &&
                                        dossier.DeclarationTracfins.All(x => x.EstNouvelleDeclarationTracfin) &&
                                        dossier.DeclarationTracfins.All(y => y.DateDeclaration != null) &&
                                        (dossier.StatutDossierId == (int)StatutDossierLabEnum.AttenteARTracfin ||
                                         dossier.StatutDossierId == (int)StatutDossierLabEnum.EncoursEnvoiDS) &&
                                        !string.IsNullOrEmpty(model.NumeroAccuseReception);

                if (estClotureTracfin)
                {
                    dossier.StatutDossierId = (int)StatutDossierLabEnum.clotureTracfin;
                    dossier.DateCloture = DateTime.Now;
                }

                await _labService.Dossier.UpdateDossierAsync(dossier, cancellationToken);

                var personnePhysiques =
                    await _labService.DossierLabPersonnePhysique.GetDossierLabPersonnePhysiqueByDetailsAsync(id,
                        cancellationToken);
                var personneMorales =
                    await _labService.DossierLabPersonneMorale.GetDossierLabPersonneMoraleByDetailsAsync(id,
                        cancellationToken);

                foreach (var dLpp in personnePhysiques)
                {
                    var valueToUpdate = model.PersonnePhysiqueLabs.FirstOrDefault(x => x.Id == dLpp.Id);
                    if (valueToUpdate != null)
                    {
                        dLpp.TypeRelationAffaireLabId = valueToUpdate.TypeRelationAffaireLabId;
                        dLpp.DateCessationRelations = valueToUpdate.DateCessationRelations;
                    }

                    await _labService.PersonnePhysique.UpdateAsync(dLpp, cancellationToken);
                }

                foreach (var dLpm in personneMorales)
                {
                    var valueToUpdate = model.PersonneMoraleLabs.FirstOrDefault(x => x.Id == dLpm.Id);
                    if (valueToUpdate != null)
                    {
                        dLpm.TypeRelationAffaireLabId = valueToUpdate.TypeRelationAffaireLabId;
                        dLpm.DateCessationRelations = valueToUpdate.DateCessationRelations;
                    }

                    await _labService.PersonneMorale.UpdateAsync(dLpm, cancellationToken);
                }

                await UpdateDossierLabScenarios(id, model, cancellationToken);

                if (model.Attachments.Any())
                {
                    var docVms = LabHelper.GetFileAttachments(
                        model.Attachments.Where(x =>
                            string.IsNullOrEmpty(_settings.WhiteListExtension) || IsValidExtension(x.File,
                                _settings.WhiteListExtension.Trim().Split(","))).ToList(),
                        dossier.Id,
                        _logger
                    );
                    var docs = _mapper.Map<List<DocumentDossierLab>>(docVms);

                    foreach (var d in docs)
                    {
                        d.CountryRelaseId = d.TypeDocumentLabId == 2 || d.TypeDocumentLabId == 3 ||
                                            d.TypeDocumentLabId == 5 || d.TypeDocumentLabId == 9
                            ? d.CountryRelaseId != 0 ? d.CountryRelaseId : null
                            : null;
                        await _labService.DocumentDossier.AddAsync(d, cancellationToken).ConfigureAwait(false);
                    }
                }

                eventDossier.DossierLabId = id;
                eventDossier.CodeDossier = dossier.CodeUnique;
                eventDossier.CreateurId = currentUser.Id;
                eventDossier.UtilisateurEventId = currentUser.Id;
                var eventEnt = _mapper.Map<EventDossier>(eventDossier);

                if (estNouveauAccuseReception)
                {
                   
                    //await _labService.EventDossier.AddAsync(eventEnt, cancellationToken).ConfigureAwait(false);
                    if (estClotureTracfin)
                    {
                        eventEnt.Id = 0;
                        eventEnt.EventDossierTypeId = (int)EactionEventTypeDossier.CLOTURE_TRACFIN;
                    }
                }
                await _labService.EventDossier.AddAsync(eventEnt, cancellationToken).ConfigureAwait(false);

                var libelleModificateur = $"{currentUser.Prenom} {currentUser.Nom.ToUpper(CultureInfo.CurrentCulture)}";


                return new JsonResult(new
                {
                    success = true,
                    isTransmissionParquet = model.IsTransmissionParquet,
                    isTransmissionAutorite = model.IsTransmissionAutorite,
                    isSuivi = model.IsSuivi,
                    modificateur = libelleModificateur,
                    dateModification = DateTime.UtcNow,
                    numeroAccuseReception = model.NumeroAccuseReception,
                    dateDeclaration = model.DateDeclaration,
                    dateDetection = model.DateDetection,
                    statutDossier = dossier.StatutDossierId
                });
            }

            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();

            return new JsonResult(new
            {
                success = false
            });
        }

        private async Task UpdateDossierLabScenarios(
            int id,
            EditTransmissionDossierLabViewModel model,
            CancellationToken cancellationToken = default)
        {
            var shouldHaveScenario =
                await OriginLabHelper.ShouldHaveScenario(_labService.OrigineLab, model.OrigineLabId, cancellationToken);

            var currentScenarioLabs =
                (await _labService.DossierLabScenario.GetDossierLabScenario(id, cancellationToken)).ToList();

            if (!shouldHaveScenario && currentScenarioLabs.Any())
            {
                foreach (var scenario in currentScenarioLabs)
                    await _labService.DossierLabScenario.DeleteAsync(scenario, cancellationToken);
            }
            else if (currentScenarioLabs.Any())
            {
                foreach (var scenario in currentScenarioLabs)
                {
                    scenario.ApplicationScenarioLabId = model.ApplicationScenarioLabId;
                    scenario.ScenarioLabId = model.ScenarioLabId;
                    await _labService.DossierLabScenario.UpdateAsync(scenario, cancellationToken);
                }
            }
            else
            {
                var scenario = new DossierLabScenario
                {
                    ApplicationScenarioLabId = model.ApplicationScenarioLabId,
                    ScenarioLabId = model.ScenarioLabId,
                    DossierLabId = id
                };
                await _labService.DossierLabScenario.AddAsync(scenario, cancellationToken);
            }
        }

        [HttpGet]
        public async Task<IActionResult> TransmissionDossierLab(string cryptedId, CancellationToken cancellationToken)
        {
            var id = this.Uprotect(cryptedId, _protector);

            var dossierLab = await _labService.Dossier.GetAsync(id, false, token: cancellationToken).ConfigureAwait(false);
            if (dossierLab == null)
                return new JsonResult(new
                {
                    success = false,
                    status = false,
                    message = _translator.Common["DossierIntrouvable"]
                });
            if (!IsVerifyUserHabilitation())
                return new JsonResult(new
                {
                    success = false,
                    status = false,
                    message = _translator.Common["PasDePersmissionActiviteDirection"]
                });
            var personnesPhysiquesLab = await _labService.DossierLabPersonnePhysique.GetDossierLabPersonnePhysique(id, token: cancellationToken)
                .ConfigureAwait(false);
            var personnesPhysiquesLabViewModels =
                _mapper.Map<List<PersonnePhysiqueLabViewModel>>(personnesPhysiquesLab);
            var personneMoralesLab = await _labService.DossierLabPersonneMorale.GetDossierLabPersonneMorale(id, token: cancellationToken)
                .ConfigureAwait(false);
            var personneMoralesLabViewModels =
                _mapper.Map<List<PersonneMoraleLabViewModel>>(personneMoralesLab);
            EditTransmissionDossierLabViewModel model;
            var dossierLabScenario =
                await _labService.DossierLabScenario.GetDossierLabScenario(id, cancellationToken).ConfigureAwait(false);
            var dossierLabScenarioViewModels =
                _mapper.Map<List<DossierLabScenarioViewModel>>(dossierLabScenario);
            var directionId = dossierLab.DirectionId;
            var dossierLabOrigine = await _labService.OrigineLab.GetOrigineLabByDirectionId(directionId, null, cancellationToken)
                .ConfigureAwait(false);
            var origineLabViewModels =
                _mapper.Map<List<OrigineLabViewModel>>(dossierLabOrigine);
            var dossierLabCategorie = await _labService.CategorieLab.GetCategorieLabByDirectionId(directionId, cancellationToken)
                .ConfigureAwait(false);
            var categorieLabViewModels =
                _mapper.Map<List<CategorieLabViewModel>>(dossierLabCategorie);
            var isAdminGlobal = _roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal);

            var entitesDatasource = await GetEntitesDatasource(directionId, cancellationToken);

            if (dossierLab.IsDeclarationSoupcon)
            {
                var declaration = await _labService.DeclarationTracfin.GetDeclarationTracfin(id, cancellationToken).ConfigureAwait(false);
                model = new EditTransmissionDossierLabViewModel
                {
                    CryptedDossierLabId = cryptedId,
                    Lang = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture),
                    IsTransmissionParquet = declaration?.IsTransmissionParquet ?? false,
                    IsTransmissionAutorite = declaration?.IsTransmissionAutorite ?? false,
                    DateDeclaration = declaration?.DateDeclaration,
                    DateDetection = dossierLab.DateReception,
                    IsSuivi = dossierLab.IsSuivi,
                    NumeroAccuseReception = declaration?.NumeroAccuseReception ?? string.Empty,
                    ReferenceInterne = declaration?.ReferenceInterne ?? string.Empty,
                    CodeUniqueDossier = dossierLab.CodeUnique,
                    IsDs = true,
                    PersonnePhysiqueLabs = personnesPhysiquesLabViewModels,
                    PersonneMoraleLabs = personneMoralesLabViewModels,
                    DossierLabPersonneScenarios = dossierLabScenarioViewModels,
                    OrigineLabId = dossierLab.OrigineLabId,
                    CategorieLabId = dossierLab.CategorieId,
                    ApplicationScenarioLabId =
                        dossierLab.DossierLabScenarios?.FirstOrDefault()?.ApplicationScenarioLabId,
                    ScenarioLabId = dossierLab.DossierLabScenarios?.FirstOrDefault()?.ScenarioLabId,
                    OrigineLabs = origineLabViewModels,
                    CategorieLabs = categorieLabViewModels,
                    EntitesDatasource = entitesDatasource,
                    EntiteId = dossierLab.EntiteId,
                    PaysId = dossierLab.PaysId,
                    SecteurEconomiqueId = dossierLab.SecteurEconomiqueId,
                    IsAdminGlobal = isAdminGlobal
                };
            }
            else
            {
                model = new EditTransmissionDossierLabViewModel
                {
                    CryptedDossierLabId = cryptedId,
                    Lang = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture),
                    CodeUniqueDossier = dossierLab.CodeUnique,
                    IsDs = false,
                    IsSuivi = dossierLab.IsSuivi,
                    PersonnePhysiqueLabs = personnesPhysiquesLabViewModels,
                    PersonneMoraleLabs = personneMoralesLabViewModels,
                    DossierLabPersonneScenarios = dossierLabScenarioViewModels,
                    OrigineLabId = dossierLab.OrigineLabId,
                    CategorieLabId = dossierLab.CategorieId,
                    ApplicationScenarioLabId =
                        dossierLab.DossierLabScenarios?.FirstOrDefault()?.ApplicationScenarioLabId,
                    ScenarioLabId = dossierLab.DossierLabScenarios?.FirstOrDefault()?.ScenarioLabId,
                    OrigineLabs = origineLabViewModels,
                    CategorieLabs = categorieLabViewModels,
                    EntitesDatasource = entitesDatasource,
                    EntiteId = dossierLab.EntiteId,
                    SecteurEconomiqueId = dossierLab.SecteurEconomiqueId,
                    PaysId = dossierLab.PaysId
                };
            }

            return PartialView("/Areas/Lab/Views/Dossier/_PartialConfirmationTransmissionDossierLab.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> ShowContacterPopupDossierLab(string cryptedId,
            CancellationToken cancellationToken)
        {
            _logger.BeginTrace();
            if (!IsVerifyUserHabilitation())
                return new JsonResult(new
                {
                    success = false,
                    status = false,
                    message = _translator.Common["PasDePersmissionActiviteDirection"]
                });
            var model = new ContacterResponsableDossierLabViewModel();
            var id = this.Uprotect(cryptedId, _protector);
            var dossierLab = await _labService.Dossier.GetAsync(id, false, cancellationToken).ConfigureAwait(false);
            var lang = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture);
            if (dossierLab == null)
                return new JsonResult(new
                {
                    success = false,
                    status = false,
                    message = _translator.Common["DossierIntrouvable"]
                });
            var mailTemplate = await _labService.EmailNotificationTemplate
                .GetEmailNotification((int)ActiviteModule.Lab,
                    (int)NotificationType.ContactDossierLabFraude,
                    currentUser.LangueId, cancellationToken).ConfigureAwait(false);
            // Vérifier que mailTemplate n'est pas null
            if (mailTemplate == null)
            {
                return new JsonResult(new
                {
                    success = false,
                    status = false,
                    message = _translator.Common["TemplateEmailIntrouvable"]
                });
            }

            if (dossierLab.Confidentiel)
            {
                string mailConfidentiel = null;
                if (dossierLab.Direction?.Confidentiels?.Any() == true)
                {
                    mailConfidentiel = dossierLab.Direction.Confidentiels.FirstOrDefault()?.Email;
                }
                   
                if (!string.IsNullOrEmpty(mailConfidentiel))
                {
                    model.Mail = mailConfidentiel;
                    model.ResponsableDossier = null;
                }
                else
                {
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["DemandeContactMsgError"]
                    });
                }
            }
            else
            {
                model.Mail = dossierLab.Createur?.Email;
                model.ResponsableDossier = dossierLab.Createur?.Nom + " " + dossierLab.Createur?.Prenom;
            }

            model.Confidentiel = dossierLab.Confidentiel;
            model.Lang = lang;
            // Vérifier aussi que currentUser n'est pas null
            if (currentUser != null)
            {
                model.Objet = string.Format(CultureInfo.CurrentCulture, mailTemplate.Subject, dossierLab.CodeUnique,
                    currentUser.Nom + " " + currentUser.Prenom);
            }
            else
            {
                model.Objet = string.Format(CultureInfo.CurrentCulture, mailTemplate.Subject, dossierLab.CodeUnique, "");
            }

            model.CryptedDossierLabId = cryptedId;
            _logger.EndTrace();
            return PartialView("/Areas/Lab/Views/Dossier/_PartialContacterDossierLab.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EnvoieMessageDossierLab(ContacterResponsableDossierLabViewModel model,
            CancellationToken cancellationToken)
        {
            _logger.BeginTrace();

            try
            {
                _logger.BeginTrace();
                if (!IsVerifyUserHabilitation())
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["PasDePersmissionActiviteDirection"]
                    });
                if (!string.IsNullOrEmpty(model.Message))
                {
                    var id = this.Uprotect(model.CryptedDossierLabId, _protector);
                    var dossierLab = await _labService.Dossier.GetAsync(id, false, cancellationToken)
                        .ConfigureAwait(false);
                    var signature = "<br>" + currentUser.Prenom + " " + currentUser.Nom + "<br>" +
                                    "<b>" + currentUser.Email + "</b>";

                    var emails = new List<EmailNotification>
                    {
                        new()
                        {
                            //Envoie mail au reponsable dossier.
                            Body = model.Message + signature,
                            EmailTo = model.Mail,
                            Subject = model.Objet,
                            EmailCc = currentUser.Email
                        }
                    };
                    if (await _labService.EmailNotification.PushNotifications(emails, cancellationToken)
                            .ConfigureAwait(false))
                        return new JsonResult(new
                        {
                            success = true,
                            status = true,
                            message = _translator.Common["emailContactSuccess"]
                        });
                }
                else
                {
                    return new JsonResult(new
                    {
                        success = false,
                        status = false,
                        message = _translator.Common["emailContactError"]
                    });
                }
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }
            finally
            {
                _logger.EndTrace();
            }

            return new JsonResult(new
            {
                success = false,
                status = false,
                message = _translator.Common["emailContactError"]
            });
        }

        private async Task<List<SelectedItem>> GetEntitesDatasource(int directionId, CancellationToken token)
        {
            var entityLabs = await _referentielService.GetEntitesLabByDirectionAsync(directionId, true, token);
            return _mapper.Map<List<SelectedItem>>(entityLabs);
        }


        [HttpGet]
        public async Task<IActionResult> TransmissionDossierLabNew(string cryptedId,
            CancellationToken cancellationToken = default)
        {
            var id = this.Uprotect(cryptedId, _protector);

            var dossierLab = await _labService.Dossier.GetAsync(id, false, token: cancellationToken).ConfigureAwait(false);
            if (dossierLab == null)
                return new JsonResult(new
                {
                    success = false,
                    status = false,
                    message = _translator.Common["DossierIntrouvable"]
                });
            if (!IsVerifyUserHabilitation())
                return new JsonResult(new
                {
                    success = false,
                    status = false,
                    message = _translator.Common["PasDePersmissionActiviteDirection"]
                });
            var personnesPhysiquesLab = await _labService.DossierLabPersonnePhysique.GetDossierLabPersonnePhysique(id, token: cancellationToken)
                .ConfigureAwait(false);
            var personneMoralesLab = await _labService.DossierLabPersonneMorale.GetDossierLabPersonneMorale(id, token: cancellationToken)
                .ConfigureAwait(false);
            var dossierLabScenario =
                await _labService.DossierLabScenario.GetDossierLabScenario(id, cancellationToken).ConfigureAwait(false);
            var directionId = dossierLab.DirectionId;
            var dossierLabOrigine = await _labService.OrigineLab.GetOrigineLabByDirectionId(directionId, null, cancellationToken)
                .ConfigureAwait(false);
            var dossierLabCategorie = await _labService.CategorieLab.GetCategorieLabByDirectionId(directionId, cancellationToken)
                .ConfigureAwait(false);


            var personnesPhysiquesLabViewModels =
                _mapper.Map<List<PersonnePhysiqueLabViewModel>>(personnesPhysiquesLab);
            var personneMoralesLabViewModels =
                _mapper.Map<List<PersonneMoraleLabViewModel>>(personneMoralesLab);
            var dossierLabScenarioViewModels =
                _mapper.Map<List<DossierLabScenarioViewModel>>(dossierLabScenario);
            var origineLabViewModels =
                _mapper.Map<List<OrigineLabViewModel>>(dossierLabOrigine);
            var categorieLabViewModels =
                _mapper.Map<List<CategorieLabViewModel>>(dossierLabCategorie);

            EditTransmissionDossierLabViewModel model;

            var entitesDatasource = await GetEntitesDatasource(directionId, cancellationToken);

            if (dossierLab.IsDeclarationSoupcon)
            {
                var declaration = await _labService.DeclarationTracfin.GetDeclarationTracfin(id, cancellationToken).ConfigureAwait(false);
                var isManuel = dossierLab.Direction?.ModeEnvoieTracfinId == (int)ModeEnvoiTracfinEnum.Manuel;
                var numeroArNotNull = string.IsNullOrEmpty(declaration.NumeroAccuseReception);
                var isAdminGlobal = _roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal);

                model = new EditTransmissionDossierLabViewModel
                {
                    CryptedDossierLabId = cryptedId,
                    Lang = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture),
                    IsTransmissionParquet = declaration.IsTransmissionParquet,
                    IsTransmissionAutorite = declaration.IsTransmissionAutorite,
                    DateDeclaration = declaration.DateDeclaration,
                    DateDetection = dossierLab.DateReception,
                    IsSuivi = dossierLab.IsSuivi,
                    NumeroAccuseReception = declaration.NumeroAccuseReception,
                    ReferenceInterne = declaration.ReferenceInterne,
                    CodeUniqueDossier = dossierLab.CodeUnique,
                    IsDs = true,
                    PersonnePhysiqueLabs = personnesPhysiquesLabViewModels,
                    PersonneMoraleLabs = personneMoralesLabViewModels,
                    DossierLabPersonneScenarios = dossierLabScenarioViewModels,
                    IsDeclarationTracfinManuel = isManuel,
                    IsNullNumeroAccuseReception = numeroArNotNull,
                    OrigineLabId = dossierLab.OrigineLabId,
                    CategorieLabId = dossierLab.CategorieId,
                    ApplicationScenarioLabId =
                        dossierLab.DossierLabScenarios?.FirstOrDefault()?.ApplicationScenarioLabId,
                    ScenarioLabId = dossierLab.DossierLabScenarios?.FirstOrDefault()?.ScenarioLabId,
                    OrigineLabs = origineLabViewModels,
                    CategorieLabs = categorieLabViewModels,
                    EntitesDatasource = entitesDatasource,
                    EntiteId = dossierLab.EntiteId,
                    SecteurEconomiqueId = dossierLab.SecteurEconomiqueId,
                    PaysId = dossierLab.PaysId,
                    IsAdminGlobal = isAdminGlobal
                };
            }
            else
            {
                model = new EditTransmissionDossierLabViewModel
                {
                    CryptedDossierLabId = cryptedId,
                    Lang = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture),
                    CodeUniqueDossier = dossierLab.CodeUnique,
                    IsDs = false,
                    IsSuivi = dossierLab.IsSuivi,
                    PersonnePhysiqueLabs = personnesPhysiquesLabViewModels,
                    PersonneMoraleLabs = personneMoralesLabViewModels,
                    DossierLabPersonneScenarios = dossierLabScenarioViewModels,
                    OrigineLabId = dossierLab.OrigineLabId,
                    CategorieLabId = dossierLab.CategorieId,
                    SecteurEconomiqueId = dossierLab.SecteurEconomiqueId,
                    PaysId = dossierLab.PaysId,
                    ApplicationScenarioLabId =
                        dossierLab.DossierLabScenarios?.FirstOrDefault()?.ApplicationScenarioLabId,
                    ScenarioLabId = dossierLab.DossierLabScenarios?.FirstOrDefault()?.ScenarioLabId,
                    OrigineLabs = origineLabViewModels,
                    CategorieLabs = categorieLabViewModels
                };
            }

            return PartialView("/Areas/Lab/Views/Dossier/_PartialConfirmationTransmissionDossierLabNew.cshtml", model);
        }


        public async Task<IList<DocumentDossierLabViewModel>> GetAttachmentsByDossierId(string cryptedDossierId,
            CancellationToken cancellationToken = default)
        {
            if (cryptedDossierId != null)
            {
                var id = this.Uprotect(cryptedDossierId, _protector);

                if (id > 0)
                {
                    var documentDossierList = await _labService.DocumentDossier
                        .GetDocumentByDossierId(id, cancellationToken)
                        .ConfigureAwait(false);

                    var result = _mapper.Map<List<DocumentDossierLabViewModel>>(documentDossierList);
                    return result;
                }
            }

            return new List<DocumentDossierLabViewModel>();
        }

        [HttpGet]
        public async Task<IList<DocumentDossierLabViewModel>> GetAttachmentByDossierId(string cryptedDossierId,
            CancellationToken cancellationToken = default)
        {
            if (cryptedDossierId != null)
            {
                var id = this.Uprotect(cryptedDossierId, _protector);

                var dossierLab = await _labService.Dossier.GetAsync(id, false, cancellationToken).ConfigureAwait(false);
                if (dossierLab != null && !IsVerifyUserHabilitation())
                    return null;

                if (id > 0)
                {
                    var documentDossierList = _labService.DocumentDossier
                        .GetDocumentInfoByDossierId(id, cancellationToken);

                    var result = _mapper.Map<List<DocumentDossierLabViewModel>>(documentDossierList);
                    var culture = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture);
                    var declarationfiles = await _labService.Dossier.GetDeclarationTracfinFiles(id, culture, cancellationToken)
                        .ConfigureAwait(false);
                    result.AddRange(declarationfiles);

                    foreach (var document in result)
                    {
                        document.CryptedDossierLabId =
                            _protector.Protect(document.DossierLabId.ToString(CultureInfo.CurrentCulture));
                        document.CryptedDocumentLabId =
                            _protector.Protect(document.DocumentLabId.ToString(CultureInfo.CurrentCulture));
                        document.CryptedId = _protector.Protect(document.Id.ToString(CultureInfo.CurrentCulture));
                        document.Id = -1;
                    }

                    return result;
                }
            }

            return null;
        }

        [HttpGet]
        public async Task<IList<OrigineLabViewModel>> GetOriginesByDossierId(string cryptedDossierId, int? directionId,
            CancellationToken cancellationToken = default)
        {
            if (cryptedDossierId != null)
            {
                var id = this.Uprotect(cryptedDossierId, _protector);

                var dossierLab = await _labService.Dossier.GetAsync(id, false, cancellationToken).ConfigureAwait(false);
                if (dossierLab != null && !IsVerifyUserHabilitation())
                    return null;

                if (id > 0)
                {
                    int? origineId = await _labService.Dossier.GetOrigineDossierLabs(id).ConfigureAwait(false);
                    if (origineId > 0)
                    {
                        var origineList = await _labService.OrigineLab
                            .GetOrigineLabByDirectionId(directionId, origineId, cancellationToken)
                            .ConfigureAwait(false);
                        var result = _mapper.Map<List<OrigineLabViewModel>>(origineList);
                        return result;
                    }
                }
            }

            return null;
        }

        [HttpGet]
        public Task<IList<SelectedItem>> GetCategorieTracfin(int? categorieGroupeId, int? categorieTracfinLabId,
            CancellationToken cancellationToken = default)
        {
            if (categorieGroupeId == null) return Task.FromResult<IList<SelectedItem>>(null);
            if (categorieGroupeId == 2)
                return Task.FromResult<IList<SelectedItem>>(categorieTracfinLabId == null || categorieTracfinLabId == 0 || categorieTracfinLabId == 5
                    ? _referential.CategorieTracfins.Where(x => x.Id == 5).OrderBy(x => x.Id).ToList()
                    : _referential.CategorieTracfins.Where(x => x.Id == 5 || x.Id == categorieTracfinLabId)
                        .OrderBy(x => x.Id).ToList());
            return Task.FromResult<IList<SelectedItem>>(_referential.CategorieTracfins.Where(x => x.Id != 5).OrderBy(x => x.Id).ToList());

        }

        public bool IsVerifyUserHabilitation(int? directionId = null)
        {
            try
            {
                var autorize = _roleAccessUser.HasActivites(ActiviteModule.Lab);
                if (directionId != null)
                    autorize = autorize &&
                               (_labService.Referentiel
                                    .GetDirectionHabiliteUser((int)ActiviteModule.Lab, _userInfoService)
                                    .Any(x => x.Id == directionId) ||
                                _roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal));

                return autorize;
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
                return false;
            }
        }

        private static bool IsValidExtension(object value, IEnumerable<string> extensions)
        {
            if (!(value is IFormFile file)) return true;
            var extension = Path.GetExtension(file.FileName);
            return extensions.Contains(extension.ToUpper(CultureInfo.CurrentCulture));
        }

        [HttpGet]
        public async Task<IActionResult> GetCriblagePersonnePhysiqueLabPartial(string name, string firstname,
            string dateNaissance,
            CancellationToken token = default)
        {
            _logger.BeginTrace();
            var model = new CriblagePersonnePhysiqueLabPageViewModel();
            try
            {
                DateTime.TryParse(dateNaissance, out var dateNaissanceValue);
                if (name.IsNotNullOrEmpty()
                    || firstname.IsNotNullOrEmpty()
                    || dateNaissanceValue != DateTimeOffset.MinValue)
                {
                    var isAdminGlobal = _roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal);
                    if (!isAdminGlobal && !_roleAccessUser.HasActivites(ActiviteModule.Lab))
                        return new JsonResult(new { success = false, status = false });

                    model.Lang = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture);
                    var id = _labService.Referentiel.GetDirectionHabiliteUser((int)ActiviteModule.Lab, _userInfoService)
                        .FirstOrDefault()?.Id;
                    if (id != null)
                    {
                        var directionId = (int)id;
                        var listPersonnePhysique = await _labService.DossierLabPersonnePhysique
                            .GetManyByNameAsync(name, firstname, dateNaissanceValue, directionId, _translator, token)
                            .ConfigureAwait(false);

                        var userDelegatedIds = (await _labService.Delegation
                                .GetDelegationUtilisateurHabiliteActivite(currentUser.Id,
                                    (int)ActiviteModule.Lab, token).ConfigureAwait(false))
                            .Select(x => (int?)x.UtilisateurId).ToList();
                        var personnePhysiqueItemLabs = listPersonnePhysique.ToList();
                        foreach (var pp in personnePhysiqueItemLabs)
                            if (pp.Confidentiel)
                                if (!(userDelegatedIds.Contains(pp.CreateurId) ||
                                      pp.CreateurId == currentUser.Id))
                                {
                                    var confidentialMessageLab = await _referentielService
                                        .GetConfidentialMessageLab(pp.DirectionId, token).ConfigureAwait(false);
                                    pp.FullName = confidentialMessageLab?.Message;
                                    pp.Prenom = null;
                                    pp.Nom = null;
                                    pp.DateNaissance = null;
                                    pp.DossierCode = pp.DossierCode.Split('-').Length > 2
                                        ? $"{pp.DossierCode.Split('-')[0]}-{pp.DossierCode.Split('-')[3]}"
                                        : string.Empty;
                                }

                        model.PersonnePhysiqueItems =
                            _mapper.Map<List<PersonnePhysiqueItemLabViewModel>>(personnePhysiqueItemLabs.Take(100));
                    }
                }
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }
            finally
            {
                _logger.EndTrace();
            }

            return PartialView("/Areas/Lab/Views/Dossier/_PartialCriblagePersonnePhysiqueLab.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetCriblagePersonneMoraleLabPartial(string name, string immatriculation,
            CancellationToken token = default)
        {
            _logger.BeginTrace();
            var model = new CriblagePersonneMoraleLabPageViewModel();
            try
            {
                if (name.IsNotNullOrEmpty() || immatriculation.IsNotNullOrEmpty())
                {
                    var isAdminGlobal = _roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal);
                    if (!isAdminGlobal && !_roleAccessUser.HasActivites(ActiviteModule.Lab))
                        return new JsonResult(new { success = false, status = false });

                    model.Lang = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture);
                    var id = _labService.Referentiel.GetDirectionHabiliteUser((int)ActiviteModule.Lab, _userInfoService)
                        .FirstOrDefault()?.Id;
                    if (id != null)
                    {
                        var directionId = (int)id;
                        var listPersonneMorale = await _labService.DossierLabPersonneMorale
                            .GetManyByNameAsync(name, immatriculation, directionId, _translator, token)
                            .ConfigureAwait(false);

                        var userDelegatedIds = (await _labService.Delegation
                                .GetDelegationUtilisateurHabiliteActivite(currentUser.Id,
                                    (int)ActiviteModule.Lab, token).ConfigureAwait(false))
                            .Select(x => (int?)x.UtilisateurId).ToList();
                        var personneMoraleItemLabs = listPersonneMorale.ToList();
                        foreach (var pm in personneMoraleItemLabs)
                            if (pm.Confidentiel)
                                if (!(userDelegatedIds.Contains(pm.CreateurId) ||
                                      pm.CreateurId == currentUser.Id))
                                {
                                    var confidentialMessageLab = await _referentielService
                                        .GetConfidentialMessageLab(pm.DirectionId, token).ConfigureAwait(false);
                                    pm.FullName = confidentialMessageLab?.Message;
                                    pm.RaisonSociale = null;
                                    pm.NumeroImmatriculation = null;
                                    pm.DateImmatriculation = null;
                                    pm.DossierCode = pm.DossierCode.Split('-').Length > 2
                                        ? $"{pm.DossierCode.Split('-')[0]}-{pm.DossierCode.Split('-')[3]}"
                                        : string.Empty;
                                }

                        model.PersonneMoraleItems =
                            _mapper.Map<List<PersonneMoraleItemLabViewModel>>(personneMoraleItemLabs.Take(100));
                    }
                }
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }
            finally
            {
                _logger.EndTrace();
            }

            return PartialView("/Areas/Lab/Views/Dossier/_PartialCriblagePersonneMoraleLab.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetPartialExportDossierLabNew(string cryptedId,
            CancellationToken token = default)
        {
            try
            {
                var id = this.Uprotect(cryptedId, _protector);
                var lang = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture);
                var dossierLab =
                    _mapper.Map<DossierLabViewModel>(await _labService.Dossier.GetAsync(id, true, token)
                        .ConfigureAwait(false));
                dossierLab.Lang = lang;
                if (dossierLab.DeclarationTracfins.Any())
                    foreach (var item in dossierLab.DeclarationTracfins.ToList())
                    {
                        if (item.DeclarantId != null)
                        {
                            var declarant = await _referentielService.GetUtilisateurById((int)item.DeclarantId, token)
                                .ConfigureAwait(false);
                            item.NomDeclarant = declarant.Nom;
                            item.PrenomDeclarant = declarant.Prenom;
                            item.AdresseMailDeclarant = declarant.Email;
                            item.TelephoneDeclarant = declarant.Telephone;
                        }

                        if (dossierLab.DeclarationTracfins.Any(x => x.EstNouvelleDeclarationTracfin))
                        {
                            item.DateDeclarationNewDS = item.DateDeclaration;
                            dossierLab.IsNewDeclarationTracfin = true;
                        }

                        item.DeclarationTracfinNatureInfractionPenaleToDisplay =
                            _mapper.Map<IList<DeclarationTracfinNatureInfractionPenaleViewModel>>(await _labService
                                .DeclarationTracfinNatureInfractionPenaleService.GetAllByIdAsync(item.Id, true, token)
                                .ConfigureAwait(false));
                        item.DeclarationTracfinNatureSoupconFraudeFiscaleToDisplay =
                            _mapper.Map<IList<DeclarationTracfinNatureSoupconFraudeFiscaleViewModel>>(await _labService
                                .DeclarationTracfinNatureFraudeFiscaleService.GetAllByIdAsync(item.Id, true, token)
                                .ConfigureAwait(false));

                        await _operationEnCoursViewModelService.InitOperationsEnCours(
                            item, lang);

                        await _operationSuspecteViewModelService.InitOperationsSuspectes(
                            item, lang);
                    }

                var sexToDisplay = _referentielService.GetSexes();
                var civilities = _referentielService.GetCivilites();
                var situationFamilial = _referentielService.GetSituationFamiliales();
                var typeClients = _referentielService.GetTypeClientLabs();
                var typesImplication = _referentielService.GetTypeImplicationLabs();
                var typeListCriblage = _referentielService.GetTypeListeCriblages();
                var relationAffaires = _referentielService.GetTypeRelationAffaireLabs();
                var canalRelationAffaires = _referentielService.GetCanalsEntreeEnRelation();
                var pays = _referentielService.GetPays();
                var typeAdresses = _referentielService.GetTypeAdresses();
                var typeVoies = _referentielService.GetTypeVoies();
                var secteurPro = _referentielService.GetSecteurProfessionnels();
                var formeJuridiques = _referentielService.GetFormeJuridiques();
                var complementVoies = _referentielService.GetComplementVoies();
                var identificationsPro = _referentielService.GetIdentificationProfessionnelles();
                var fonctions = _referentielService.GetTypeFonctionDirigeants();
                if (dossierLab.DossierLabPersonnePhysiques.Any())
                    foreach (var x in dossierLab.DossierLabPersonnePhysiques)
                    {
                        var paysToDisplay = await _nationalitePersonnePhysiqueLabService
                            .GetAllByIdAsync(x.PersonnePhysiqueLab.Id, true, token).ConfigureAwait(false);
                        x.PersonnePhysiqueLab.NationalitesToDisplay = _mapper.Map<IList<PaysViewModel>>(
                            paysToDisplay.Select(nationalitePersonnePhysiqueLab =>
                                nationalitePersonnePhysiqueLab.Pays));
                        var paysAutreIdentiteToDisplay = await _nationaliteAutreIdentiteLabService
                            .GetAllByIdAsync(x.PersonnePhysiqueLab.Id, true, token)
                            .ConfigureAwait(false);
                        x.PersonnePhysiqueLab.AutreIdentiteNationalitesToDisplay =
                            _mapper.Map<IList<PaysViewModel>>(
                                paysAutreIdentiteToDisplay.Select(nationalitePersonnePhysiqueLab =>
                                    nationalitePersonnePhysiqueLab.Pays));

                        await _ppeTypePersonnePhysiqueLabService
                            .GetAllByIdAsync(x.PersonnePhysiqueLab.Id, token: token)
                            .ConfigureAwait(false);
                        //x.PersonnePhysiqueLab.PpeTypesToDisplay = _mapper.Map<IList<PpeTypeViewModel>>(ppeTypeToDisplay.Select(ppe => ppe.PpeType));
                        if (x.PersonnePhysiqueLab.SexeId.HasValue && sexToDisplay?.Count() > 0)
                            x.PersonnePhysiqueLab.Sexe = sexToDisplay?.First(z => z.Id == x.PersonnePhysiqueLab.SexeId)
                                ?.FrenchName;
                        if (x.PersonnePhysiqueLab.CiviliteId.HasValue && civilities?.Count() > 0)
                            x.PersonnePhysiqueLab.Civilite =
                                civilities?.First(z => z.Id == x.PersonnePhysiqueLab.CiviliteId)?.FrenchName;
                        if (x.PersonnePhysiqueLab.SituationFamilialeId.HasValue && situationFamilial?.Count() > 0)
                            x.PersonnePhysiqueLab.SituationFamiliale = situationFamilial
                                ?.First(z => z.Id == x.PersonnePhysiqueLab.SituationFamilialeId)?.FrenchName;
                        if (x.PersonnePhysiqueLab.TypeClientId.HasValue && typeClients?.Count() > 0)
                            x.PersonnePhysiqueLab.TypeClient = typeClients
                                ?.First(z => z.Id == x.PersonnePhysiqueLab.TypeClientId)?.FrenchName;
                        if (x.PersonnePhysiqueLab.TypeImplicationId.HasValue && typesImplication?.Count() > 0)
                            x.PersonnePhysiqueLab.TypeImplication = typesImplication
                                ?.First(z => z.Id == x.PersonnePhysiqueLab.TypeImplicationId)?.FrenchName;
                        if (x.PersonnePhysiqueLab.PaysNaissanceId.HasValue && pays?.Count() > 0)
                            x.PersonnePhysiqueLab.PaysNaissance = pays
                                ?.First(z => z.Id == x.PersonnePhysiqueLab.PaysNaissanceId)?.IsoFrenchName;
                        if (x.TypeListeCriblageId is > 0 &&
                            typeListCriblage?.Count() > 0)
                            x.PersonnePhysiqueLab.TypeCriblage =
                                typeListCriblage?.First(z => z.Id == x.TypeListeCriblageId)?.FrenchName;
                        if (x.PersonnePhysiqueLab.CanalEntreeEnRelationId.HasValue &&
                            canalRelationAffaires?.Count() > 0)
                            x.PersonnePhysiqueLab.CanalEntreeEnRelation = canalRelationAffaires
                                ?.First(z => z.Id == x.PersonnePhysiqueLab.CanalEntreeEnRelationId)?.FrenchName;
                        if (x.PersonnePhysiqueLab.TypeRelationAffaireLabId.HasValue &&
                            x.PersonnePhysiqueLab.TypeRelationAffaireLabId > 0 && relationAffaires?.Count() > 0)
                            x.PersonnePhysiqueLab.RelationAffaireLab = relationAffaires
                                ?.First(z => z.Id == x.PersonnePhysiqueLab.TypeRelationAffaireLabId)?.FrenchName;
                        if (x.PersonnePhysiqueLab.SecteurProfessionnelId.HasValue &&
                            x.PersonnePhysiqueLab.SecteurProfessionnelId > 0 && secteurPro?.Count() > 0)
                            x.PersonnePhysiqueLab.SecteurProfessionnel = secteurPro
                                ?.First(z => z.Id == x.PersonnePhysiqueLab.SecteurProfessionnelId)?.FrenchName;
                        if (x.PersonnePhysiqueLab.FormeJuridiqueId.HasValue &&
                            x.PersonnePhysiqueLab.FormeJuridiqueId > 0 && formeJuridiques?.Count() > 0)
                            x.PersonnePhysiqueLab.FormeJuridique = formeJuridiques
                                ?.First(z => z.Id == x.PersonnePhysiqueLab.FormeJuridiqueId)?.FrenchName;
                        if (x.PersonnePhysiqueLab.PaysDeRegistreId.HasValue &&
                            x.PersonnePhysiqueLab.PaysDeRegistreId > 0 && pays?.Count() > 0)
                            x.PersonnePhysiqueLab.PaysDeRegistre = pays
                                ?.First(z => z.Id == x.PersonnePhysiqueLab.PaysDeRegistreId)?.IsoFrenchName;
                        if (x.PersonnePhysiqueLab.AutreIdentiteSexeId.HasValue && sexToDisplay?.Count() > 0)
                            x.PersonnePhysiqueLab.AutreSexe = sexToDisplay
                                ?.First(z => z.Id == x.PersonnePhysiqueLab.AutreIdentiteSexeId)?.FrenchName;
                        if (x.PersonnePhysiqueLab.AutreIdentitePaysNaissanceId.HasValue && pays?.Count() > 0)
                            x.PersonnePhysiqueLab.AutrePaysNaissance = pays
                                ?.First(z => z.Id == x.PersonnePhysiqueLab.AutreIdentitePaysNaissanceId)?.IsoFrenchName;
                        if (x.PersonnePhysiqueLab.ComplementVoieId.HasValue &&
                            x.PersonnePhysiqueLab.ComplementVoieId > 0 && complementVoies?.Count() > 0)
                            x.PersonnePhysiqueLab.ComplementVoie = complementVoies
                                ?.First(z => z.Id == x.PersonnePhysiqueLab.ComplementVoieId)?.FrenchName;
                        if (x.PersonnePhysiqueLab.TypeVoieId.HasValue && x.PersonnePhysiqueLab.TypeVoieId > 0 &&
                            typeVoies?.Count() > 0)
                            x.PersonnePhysiqueLab.TypeVoie =
                                typeVoies?.First(z => z.Id == x.PersonnePhysiqueLab.TypeVoieId)?.FrenchName;
                        if (x.PersonnePhysiqueLab.PaysId.HasValue && x.PersonnePhysiqueLab.PaysId > 0 &&
                            pays?.Count() > 0)
                            x.PersonnePhysiqueLab.PaysAdresse =
                                pays?.First(z => z.Id == x.PersonnePhysiqueLab.PaysId)?.IsoFrenchName;
                        if (x.PersonnePhysiqueLab.IdentificationProfessionnelleId.HasValue &&
                            identificationsPro?.Count() > 0)
                            x.PersonnePhysiqueLab.IdentificationProfessionnelle = identificationsPro
                                ?.First(z => z.Id == x.PersonnePhysiqueLab.IdentificationProfessionnelleId)?.FrenchName;

                        if (x.PersonnePhysiqueLab.AutreIdentiteNationalitesToDisplay.Count > 0)
                            foreach (var natio in x.PersonnePhysiqueLab.AutreIdentiteNationalitesToDisplay)
                                x.PersonnePhysiqueLab.AutreNationalite = x.PersonnePhysiqueLab.AutreNationalite + " " +
                                                                         natio.IsoFrenchName + " ,";

                        if (x.PersonnePhysiqueLab.SupportFinancierPersonnePhysiques.Any())
                        {
                            var typesReference = _referentielService.GetTypeReferenceLabs();
                            var typesCompte = _referentielService.GetTypeComptes();
                            var typesLiens = _referentielService.GetTypeLienSupports();
                            foreach (var item in x.PersonnePhysiqueLab.SupportFinancierPersonnePhysiques)
                            {
                                if (item.TypeCompteId.HasValue && item.TypeCompteId > 0 && typesCompte?.Count() > 0)
                                    item.TypeCompte = typesCompte?.First(y => y.Id == item.TypeCompteId);

                                if (item.TypeReferenceLabId.HasValue && item.TypeReferenceLabId > 0 &&
                                    typesReference?.Count() > 0)
                                    item.TypeReferenceLab = typesReference?.First(y => y.Id == item.TypeReferenceLabId);
                                if (item.TypeLienSupportId.HasValue && item.TypeLienSupportId > 0 &&
                                    typesLiens?.Count() > 0)
                                    item.TypeLienSupport = typesLiens?.First(y => y.Id == item.TypeLienSupportId);
                            }
                        }

                        if (x.PersonnePhysiqueLab.LienPersonneMorales.Any() ||
                            x.PersonnePhysiqueLab.LienPersonnePhysiques.Any())
                        {
                            var categoriesLienMoral = _referentielService.GetCategorieLienLab();
                            var typesLienMoral = _referentielService.GetTypeLienPersonneMoralePhysiques();
                            var typesLienPhysique = _referentielService.GetTypeLienPersonnePhysiquePhysiques();
                            foreach (var item in x.PersonnePhysiqueLab.LienPersonneMorales)
                            {
                                if (item.CategorieLienPersonneMoralePhysiqueId.HasValue &&
                                    item.CategorieLienPersonneMoralePhysiqueId.Value > 0 &&
                                    categoriesLienMoral?.Count() > 0)
                                    item.CategorieLien = categoriesLienMoral
                                        ?.First(y => y.Id == item.CategorieLienPersonneMoralePhysiqueId)?.FrenchName;
                                if (!item.CertitudeLien.HasValue || item.CertitudeLien.Value < 0)
                                    item.CertitudeLienName = "";
                                else
                                    item.CertitudeLienName = item.CertitudeLien.HasValue && item.CertitudeLien.Value > 0
                                        ? "Oui"
                                        : "Non";
                                if (item.FormeJuridiqueId.HasValue && item.FormeJuridiqueId > 0 &&
                                    formeJuridiques?.Count() > 0)
                                    item.FormeJuridique = formeJuridiques?.First(z => z.Id == item.FormeJuridiqueId)
                                        ?.FrenchName;
                                if (item.TypeLienPersonneMoralePhysiqueId.HasValue &&
                                    item.TypeLienPersonneMoralePhysiqueId.Value > 0 &&
                                    typesLienMoral?.Count() > 0)
                                    item.TypeLien = typesLienMoral
                                        ?.First(y => y.Id == item.TypeLienPersonneMoralePhysiqueId)?.FrenchName;
                            }

                            foreach (var item in x.PersonnePhysiqueLab.LienPersonnePhysiques)
                            {
                                if (item.CategorieLienPersonnePhysiquePhysiqueId.HasValue &&
                                    item.CategorieLienPersonnePhysiquePhysiqueId.Value > 0 &&
                                    categoriesLienMoral?.Count() > 0)
                                    item.CategorieLien = categoriesLienMoral
                                        ?.First(y => y.Id == item.CategorieLienPersonnePhysiquePhysiqueId)?.FrenchName;
                                if (!item.CertitudeLien.HasValue || item.CertitudeLien.Value < 0)
                                    item.CertitudeLienName = "";
                                else
                                    item.CertitudeLienName = item.CertitudeLien.HasValue && item.CertitudeLien.Value > 0
                                        ? "Oui"
                                        : "Non";
                                if (item.TypeLienPersonnePhysiquePhysiqueId.HasValue &&
                                    item.TypeLienPersonnePhysiquePhysiqueId.Value > 0 &&
                                    typesLienPhysique?.Count() > 0)
                                    item.TypeLien = typesLienPhysique
                                        ?.First(y => y.Id == item.TypeLienPersonnePhysiquePhysiqueId)?.FrenchName;
                            }
                        }

                        if (x.PersonnePhysiqueLab.CoordonneePersonnePhysiqueLabs.Any())
                            foreach (var item in x.PersonnePhysiqueLab.CoordonneePersonnePhysiqueLabs)
                            {
                                if (item.Coordonnee.ComplementVoieId.HasValue && item.Coordonnee.ComplementVoieId > 0 &&
                                    complementVoies?.Count() > 0)
                                    item.Coordonnee.ComplementVoie = complementVoies
                                        ?.First(z => z.Id == item.Coordonnee.ComplementVoieId)?.FrenchName;
                                if (item.Coordonnee.TypeVoieId.HasValue && item.Coordonnee.TypeVoieId > 0 &&
                                    typeVoies?.Count() > 0)
                                    item.Coordonnee.TypeVoie = typeVoies?.First(z => z.Id == item.Coordonnee.TypeVoieId)
                                        ?.FrenchName;
                                if (item.Coordonnee.PaysId.HasValue && item.Coordonnee.PaysId > 0 && pays?.Count() > 0)
                                    item.Coordonnee.Pays =
                                        pays?.First(z => z.Id == item.Coordonnee.PaysId)?.IsoFrenchName;
                                if (item.Coordonnee.TypeAdresseId.HasValue && item.Coordonnee.TypeAdresseId > 0 &&
                                    typeAdresses?.Count() > 0)
                                    item.Coordonnee.TypeAdresse = typeAdresses
                                        ?.First(z => z.Id == item.Coordonnee.TypeAdresseId)?.FrenchName;
                            }
                    }

                if (dossierLab.DossierLabPersonneMorales.Any())
                    foreach (var item in dossierLab.DossierLabPersonneMorales)
                    {
                        if (item.PersonneMoraleLab.PaysDeRegistreId.HasValue &&
                            item.PersonneMoraleLab.PaysDeRegistreId.Value > 0 && pays?.Count() > 0)
                            item.PersonneMoraleLab.PaysDeRegistre = pays
                                ?.First(z => z.Id == item.PersonneMoraleLab.PaysDeRegistreId)?.IsoFrenchName;
                        if (item.PersonneMoraleLab.PaysId.HasValue && item.PersonneMoraleLab.PaysId.Value > 0 &&
                            pays?.Count() > 0)
                            item.PersonneMoraleLab.Pays =
                                pays?.First(z => z.Id == item.PersonneMoraleLab.PaysId)?.IsoFrenchName;
                        if (item.PersonneMoraleLab.TypeClientId.HasValue &&
                            item.PersonneMoraleLab.TypeClientId.Value > 0 && typeClients?.Count() > 0)
                            item.PersonneMoraleLab.TypeClient = typeClients
                                ?.First(z => z.Id == item.PersonneMoraleLab.TypeClientId)?.FrenchName;
                        if (item.PersonneMoraleLab.TypeImplicationId.HasValue &&
                            item.PersonneMoraleLab.TypeImplicationId.Value > 0 && typesImplication?.Count() > 0)
                            item.PersonneMoraleLab.TypeImplication = typesImplication
                                ?.First(z => z.Id == item.PersonneMoraleLab.TypeImplicationId)?.FrenchName;
                        if (item.PersonneMoraleLab.FormeJuridiqueId.HasValue &&
                            item.PersonneMoraleLab.FormeJuridiqueId.Value > 0 && formeJuridiques?.Count() > 0)
                            item.PersonneMoraleLab.FormeJuridique = formeJuridiques
                                ?.First(z => z.Id == item.PersonneMoraleLab.FormeJuridiqueId)?.FrenchName;
                        if (item.PersonneMoraleLab.SecteurProfessionnelId.HasValue &&
                            item.PersonneMoraleLab.SecteurProfessionnelId.Value > 0 && secteurPro?.Count() > 0)
                            item.PersonneMoraleLab.SecteurProfessionnel = secteurPro
                                ?.First(z => z.Id == item.PersonneMoraleLab.SecteurProfessionnelId)?.FrenchName;
                        if (item.TypeListeCriblageId.HasValue && item.TypeListeCriblageId > 0 &&
                            typeListCriblage?.Count() > 0)
                            item.PersonneMoraleLab.TypeCriblage = typeListCriblage
                                ?.First(z => z.Id == item.TypeListeCriblageId)?.FrenchName;
                        if (item.PersonneMoraleLab.Coordonnee != null &&
                            item.PersonneMoraleLab.Coordonnee.TypeVoieId.HasValue &&
                            item.PersonneMoraleLab.Coordonnee?.TypeVoieId > 0 && typeVoies?.Count() > 0)
                            item.PersonneMoraleLab.Coordonnee.TypeVoie = typeVoies
                                ?.First(z => z.Id == item.PersonneMoraleLab.Coordonnee.TypeVoieId)?.FrenchName;
                        if (item.PersonneMoraleLab.Coordonnee != null &&
                            item.PersonneMoraleLab.Coordonnee.ComplementVoieId.HasValue &&
                            item.PersonneMoraleLab.Coordonnee?.ComplementVoieId > 0 && complementVoies?.Count() > 0)
                            item.PersonneMoraleLab.Coordonnee.ComplementVoie = complementVoies
                                ?.First(z => z.Id == item.PersonneMoraleLab.Coordonnee.ComplementVoieId)?.FrenchName;
                        if (item.PersonneMoraleLab.Coordonnee != null &&
                            item.PersonneMoraleLab.Coordonnee.PaysId.HasValue &&
                            item.PersonneMoraleLab.Coordonnee?.PaysId > 0 && pays?.Count() > 0)
                            item.PersonneMoraleLab.Coordonnee.Pays = pays
                                ?.First(z => z.Id == item.PersonneMoraleLab.Coordonnee.PaysId)?.IsoFrenchName;
                        if (item.PersonneMoraleLab.CanalEntreeEnRelationId.HasValue &&
                            canalRelationAffaires?.Count() > 0)
                            item.PersonneMoraleLab.CanalEntreeEnRelation = canalRelationAffaires
                                ?.First(z => z.Id == item.PersonneMoraleLab.CanalEntreeEnRelationId)?.FrenchName;
                        if (item.PersonneMoraleLab.TypeRelationAffaireLabId.HasValue &&
                            item.PersonneMoraleLab.TypeRelationAffaireLabId > 0 && relationAffaires?.Count() > 0)
                            item.PersonneMoraleLab.RelationAffaireLab = relationAffaires
                                ?.First(z => z.Id == item.PersonneMoraleLab.TypeRelationAffaireLabId)?.FrenchName;
                        if (item.PersonneMoraleLab.ProfessionalIdentificationId.HasValue &&
                            item.PersonneMoraleLab.ProfessionalIdentificationId > 0)
                            item.PersonneMoraleLab.ProfessionalIdentification = identificationsPro
                                ?.First(z => z.Id == item.PersonneMoraleLab.ProfessionalIdentificationId)?.FrenchName;
                        foreach (var direigant in item.PersonneMoraleLab.Dirigeants)
                        {
                            switch (direigant.IdentiteDirigeant)
                            {
                                case 0:
                                    direigant.IdentiteDirigeantName = "Inconnu";
                                    break;
                                case -1:
                                    direigant.IdentiteDirigeantName = "Autre";
                                    break;
                                default:
                                {
                                    if (direigant.IdentiteDirigeant.HasValue && direigant.IdentiteDirigeant > 0)
                                    {
                                        var personnePhysique = await _labService.DossierLabPersonnePhysique
                                            .GetPersonnePhysiqueByIdAsync(direigant.IdentiteDirigeant.Value, token)
                                            .ConfigureAwait(false);
                                        direigant.IdentiteDirigeantName =
                                            personnePhysique?.NomNaissance + " " + personnePhysique?.Prenoms;
                                    }

                                    break;
                                }
                            }

                            if (direigant.TypeDirigeant.HasValue)
                                direigant.BeType = direigant.TypeDirigeant.Value == 1
                                    ? "Personne physique"
                                    : "Personne morale";
                            if (direigant.BeComplementVoieId.HasValue && direigant.BeComplementVoieId > 0 &&
                                complementVoies?.Count() > 0)
                                direigant.BeComplementVoie = complementVoies
                                    ?.First(z => z.Id == direigant.BeComplementVoieId)?.FrenchName;
                            if (direigant.BeTypeVoieId.HasValue && direigant.BeTypeVoieId > 0 && typeVoies?.Count() > 0)
                                direigant.BeTypeVoie =
                                    typeVoies?.First(z => z.Id == direigant.BeTypeVoieId)?.FrenchName;
                            if (direigant.FormeJuridiqueId.HasValue && direigant.FormeJuridiqueId.Value > 0 &&
                                formeJuridiques?.Count() > 0)
                                direigant.FormeJuridique = formeJuridiques
                                    ?.First(z => z.Id == direigant.FormeJuridiqueId)?.FrenchName;
                            if (direigant.BePaysId.HasValue && direigant.BePaysId.Value > 0 && pays?.Count() > 0)
                                direigant.BePays = pays?.First(z => z.Id == direigant.BePaysId)?.IsoFrenchName;
                            if (direigant.PaysNaissanceId.HasValue && direigant.PaysNaissanceId.Value > 0 &&
                                pays?.Count() > 0)
                                direigant.PaysNaissance =
                                    pays?.First(z => z.Id == direigant.PaysNaissanceId)?.IsoFrenchName;
                            if (direigant.PaysId.HasValue && direigant.PaysId.Value > 0 && pays?.Count() > 0)
                                direigant.Pays = pays?.First(z => z.Id == direigant.PaysId)?.IsoFrenchName;
                            if (direigant.PaysDeRegistreId.HasValue && direigant.PaysDeRegistreId.Value > 0 &&
                                pays?.Count() > 0)
                                direigant.PaysDeRegistre =
                                    pays?.First(z => z.Id == direigant.PaysDeRegistreId)?.IsoFrenchName;
                            if (direigant.ComplementVoieId.HasValue && direigant.ComplementVoieId > 0 &&
                                complementVoies?.Count() > 0)
                                direigant.ComplementVoie = complementVoies
                                    ?.First(z => z.Id == direigant.ComplementVoieId)?.FrenchName;
                            if (direigant.TypeVoieId.HasValue && direigant.TypeVoieId > 0 && typeVoies?.Count() > 0)
                                direigant.TypeVoie = typeVoies?.First(z => z.Id == direigant.TypeVoieId)?.FrenchName;
                            if (direigant.BePaysNaissanceId.HasValue && direigant.BePaysNaissanceId.Value > 0 &&
                                pays?.Count() > 0)
                                direigant.BePaysNaissance =
                                    pays?.First(z => z.Id == direigant.BePaysNaissanceId)?.IsoFrenchName;
                            if (direigant.PaysDeRegistreId.HasValue && direigant.PaysDeRegistreId.Value > 0 &&
                                pays?.Count() > 0)
                                direigant.PaysDeRegistre =
                                    pays?.First(z => z.Id == direigant.PaysDeRegistreId)?.IsoFrenchName;
                            if (direigant.BeSexeId.HasValue && direigant.BeSexeId.Value > 0 && pays?.Count() > 0)
                                direigant.BeSexe = sexToDisplay?.First(z => z.Id == direigant.BeSexeId)?.FrenchName;
                            if (direigant.ProfessionalIdentificationId.HasValue &&
                                direigant.ProfessionalIdentificationId.Value > 0 && identificationsPro?.Count() > 0)
                                direigant.ProfessionalIdentification = identificationsPro
                                    ?.First(z => z.Id == direigant.ProfessionalIdentificationId)?.FrenchName;
                            if (direigant.FonctionDirigeant.HasValue && direigant.FonctionDirigeant.Value > 0 &&
                                fonctions?.Count() > 0)
                                direigant.FonctionDirigeantName =
                                    fonctions?.First(z => z.Id == direigant.FonctionDirigeant)?.FrenchName + " " +
                                    direigant.AutreFonctionDirigeant;
                            if (direigant.BeNationaliteId.HasValue && direigant.FonctionDirigeant.Value > 0 &&
                                pays?.Count() > 0)
                                direigant.BeNationalite =
                                    pays?.First(z => z.Id == direigant.FonctionDirigeant)?.IsoFrenchName;
                        }

                        if (item.PersonneMoraleLab.SupportFinancierPersonneMorales.Any())
                        {
                            var typesReference = _referentielService.GetTypeReferenceLabs();
                            var typesCompte = _referentielService.GetTypeComptes();
                            var typesLiens = _referentielService.GetTypeLienSupports();
                            foreach (var x in item.PersonneMoraleLab.SupportFinancierPersonneMorales)
                            {
                                if (x.TypeCompteId.HasValue && x.TypeCompteId > 0 && typesCompte?.Count() > 0)
                                    x.TypeCompte = typesCompte?.First(z => z.Id == x.TypeCompteId)?.FrenchName;
                                if (x.TypeReferenceLabId.HasValue && x.TypeReferenceLabId > 0 &&
                                    typesReference?.Count() > 0)
                                    x.TypeReference = typesReference?.First(z => z.Id == x.TypeReferenceLabId)
                                        ?.FrenchName;
                                if (x.TypeLienSupportId.HasValue && x.TypeLienSupportId > 0 && typesLiens?.Count() > 0)
                                    x.TypeLienSupport = typesLiens?.First(z => z.Id == x.TypeLienSupportId)?.FrenchName;
                            }
                        }

                        if (item.PersonneMoraleLab.LienPersonneMorales.Any() ||
                            item.PersonneMoraleLab.LienPersonnePhysiques.Any())
                        {
                            var categoriesLienMoral = _referentielService.GetCategorieLienLab();
                            //var typesLienMoral = _referentielService.GetTypeLienPersonneMoralePhysiques();
                            //var typesLienPhysique = _referentielService.GetTypeLienPersonnePhysiquePhysiques();
                            var lm = _referentielService.GetTypeLienPersonneMoraleMorales();
                            var lienP = _referentielService.GetTypeLienPersonnePhysiqueMorales();
                            foreach (var x in item.PersonneMoraleLab.LienPersonneMorales)
                            {
                                if (x.CategorieLienPersonneMoraleMoraleId.HasValue &&
                                    x.CategorieLienPersonneMoraleMoraleId.Value > 0 &&
                                    categoriesLienMoral?.Count() > 0)
                                    x.CategorieLien = categoriesLienMoral
                                        ?.First(z => z.Id == x.CategorieLienPersonneMoraleMoraleId)?.FrenchName;
                                if (!x.CertitudeLien.HasValue || x.CertitudeLien.Value < 0) x.CertitudeLienName = "";
                                else
                                    x.CertitudeLienName = x.CertitudeLien.HasValue && x.CertitudeLien.Value > 0
                                        ? "Oui"
                                        : "Non";
                                if (x.FormeJuridiqueId.HasValue && x.FormeJuridiqueId > 0 &&
                                    formeJuridiques?.Count() > 0)
                                    x.FormeJuridique = formeJuridiques?.First(z => z.Id == x.FormeJuridiqueId)
                                        ?.FrenchName;
                                if (x.TypeLienPersonneMoraleMoraleId.HasValue &&
                                    x.TypeLienPersonneMoraleMoraleId.Value > 0 &&
                                    lm?.Count() > 0)
                                    x.TypeLien = lm?.First(z => z.Id == x.TypeLienPersonneMoraleMoraleId)?.FrenchName;
                            }

                            foreach (var lp in item.PersonneMoraleLab.LienPersonnePhysiques)
                            {
                                if (lp.CategorieLienPersonnePhysiqueMoraleId.HasValue &&
                                    lp.CategorieLienPersonnePhysiqueMoraleId.Value > 0 &&
                                    categoriesLienMoral?.Count() > 0)
                                    lp.CategorieLien = categoriesLienMoral
                                        ?.First(x => x.Id == lp.CategorieLienPersonnePhysiqueMoraleId)?.FrenchName;
                                if (!lp.CertitudeLien.HasValue || lp.CertitudeLien.Value < 0) lp.CertitudeLienName = "";
                                else
                                    lp.CertitudeLienName = lp.CertitudeLien.HasValue && lp.CertitudeLien.Value > 0
                                        ? "Oui"
                                        : "Non";
                                if (lp.TypeLienPersonnePhysiqueMoraleId.HasValue &&
                                    lp.TypeLienPersonnePhysiqueMoraleId.Value > 0 &&
                                    lienP?.Count() > 0)
                                    lp.TypeLien = lienP?.First(x => x.Id == lp.TypeLienPersonnePhysiqueMoraleId)
                                        ?.FrenchName;
                            }
                        }
                    }

                if (dossierLab.DossierLabNonConnus.Any())
                    foreach (var item in dossierLab.DossierLabNonConnus)
                    {
                        if (item.NonConnuLab.TypeImplicationId.HasValue &&
                            item.NonConnuLab.TypeImplicationId.Value > 0 && typesImplication?.Count() > 0)
                            item.NonConnuLab.TypeImplication = typesImplication
                                ?.First(z => z.Id == item.NonConnuLab.TypeImplicationId)?.FrenchName;
                        if (item.NonConnuLab.SupportFinancierNonConnus.Any())
                        {
                            var typesReference = _referentielService.GetTypeReferenceLabs();
                            var typesCompte = _referentielService.GetTypeComptes();
                            var typesLiens = _referentielService.GetTypeLienSupports();
                            foreach (var x in item.NonConnuLab.SupportFinancierNonConnus)
                            {
                                if (x.TypeCompteId.HasValue && x.TypeCompteId > 0 && typesCompte?.Count() > 0)
                                    x.TypeCompte = typesCompte?.First(z => z.Id == x.TypeCompteId);
                                if (x.TypeReferenceLabId.HasValue && x.TypeReferenceLabId > 0 &&
                                    typesReference?.Count() > 0)
                                    x.TypeReference = typesReference?.First(z => z.Id == x.TypeReferenceLabId)
                                        ?.FrenchName;
                                if (x.TypeLienSupportId.HasValue && x.TypeLienSupportId > 0 && typesLiens?.Count() > 0)
                                    x.TypeLienSupport = typesLiens?.First(z => z.Id == x.TypeLienSupportId);
                            }
                        }

                        if (item.NonConnuLab.LienPersonneMorales.Any() || item.NonConnuLab.LienPersonnePhysiques.Any())
                        {
                            var categoriesLienMoral = _referentielService.GetCategorieLienLab();
                            var lm = _referentielService.GetTypeLienPersonneMoralePhysiques();
                            var lienP = _referentielService.GetTypeLienPersonnePhysiquePhysiques();
                            foreach (var x in item.NonConnuLab.LienPersonneMorales)
                            {
                                if (x.CategorieLienPersonneMoralePhysiqueId.HasValue &&
                                    x.CategorieLienPersonneMoralePhysiqueId.Value > 0 &&
                                    categoriesLienMoral?.Count() > 0)
                                    x.CategorieLien = categoriesLienMoral
                                        ?.First(z => z.Id == x.CategorieLienPersonneMoralePhysiqueId)?.FrenchName;
                                if (!x.CertitudeLien.HasValue || x.CertitudeLien.Value < 0) x.CertitudeLienName = "";
                                else
                                    x.CertitudeLienName = x.CertitudeLien.HasValue && x.CertitudeLien.Value > 0
                                        ? "Oui"
                                        : "Non";
                                if (x.FormeJuridiqueId.HasValue && x.FormeJuridiqueId > 0 &&
                                    formeJuridiques?.Count() > 0)
                                    x.FormeJuridique = formeJuridiques?.First(z => z.Id == x.FormeJuridiqueId)
                                        ?.FrenchName;
                                if (x.TypeLienPersonneMoralePhysiqueId.HasValue &&
                                    x.TypeLienPersonneMoralePhysiqueId.Value > 0 &&
                                    lm?.Count() > 0)
                                    x.TypeLien = lm?.First(z => z.Id == x.TypeLienPersonneMoralePhysiqueId)?.FrenchName;
                            }

                            foreach (var lp in item.NonConnuLab.LienPersonnePhysiques)
                            {
                                if (lp.CategorieLienPersonnePhysiquePhysiqueId.HasValue &&
                                    lp.CategorieLienPersonnePhysiquePhysiqueId.Value > 0 &&
                                    categoriesLienMoral?.Count() > 0)
                                    lp.CategorieLien = categoriesLienMoral
                                        ?.First(x => x.Id == lp.CategorieLienPersonnePhysiquePhysiqueId)?.FrenchName;
                                if (!lp.CertitudeLien.HasValue || lp.CertitudeLien.Value < 0) lp.CertitudeLienName = "";
                                else
                                    lp.CertitudeLienName = lp.CertitudeLien.HasValue && lp.CertitudeLien.Value > 0
                                        ? "Oui"
                                        : "Non";
                                if (lp.TypeLienPersonnePhysiquePhysiqueId.HasValue &&
                                    lp.TypeLienPersonnePhysiquePhysiqueId.Value > 0 &&
                                    lienP?.Count() > 0)
                                    lp.TypeLien = lienP?.First(x => x.Id == lp.TypeLienPersonnePhysiquePhysiqueId)
                                        ?.FrenchName;
                            }
                        }
                    }

                var declarationTracfins = _mapper.Map<DeclarationTracfin>(await _labService.DeclarationTracfin
                    .GetDeclarationByDetailsAsync(id, token).ConfigureAwait(false));
                var newFileSortis = @"wwwroot/documentations/fr/extraction/lab/DS-LCB-FT_" + dossierLab.CodeUnique +
                                    "_" + DateTime.Now.ToString("dd-MM-yyy-HHmmss") + ".pdf";
                var documentDossierList = _labService.DocumentDossier.GetDocumentInfoByDossierId(dossierLab.Id, token);
                var attachementList = _mapper.Map<List<DocumentDossierLabViewModel>>(documentDossierList);
                var documents = await _labService.Dossier.GetDocumentDossierLabs(dossierLab.Id, token)
                    .ConfigureAwait(false);
                var declarationModel =
                    _mapper.Map<DeclarationTracfinViewModel>(dossierLab.DeclarationTracfins?.First());

                var url = Program._reportApiUrl() + "LCBFTReport/";
                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                    using (var _client = new HttpClient(httpClientHandler))
                    {
                        _client.BaseAddress = new Uri(url);
                        _client.DefaultRequestHeaders.Accept.Clear();
                        _client.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
                        var reportModel = new LabReportViewModel
                        {
                            serlializedModelDosLab = dossierLab,
                            serlializedModelDecTrac = declarationTracfins,
                            serlializedModelDeclaration = declarationModel,
                            serlializedModelAttachementList = attachementList
                        };
                        var jsonOptions = new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            NullValueHandling = NullValueHandling.Ignore
                        };
                        var serlializedModels = JsonConvert.SerializeObject(reportModel, jsonOptions);
                        var response =
                            await _client.PostAsJsonAsync("ExportLCBFTReportAsync", serlializedModels, token);
                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadAsStringAsync(token).Result.Replace("\"", string.Empty);
                            await System.IO.File.WriteAllBytesAsync(newFileSortis, Convert.FromBase64String(result),
                                token);
                        }
                        else
                        {
                            return new JsonResult(new
                            {
                                success = true,
                                errorMessage = _translator.Common["ErrorMessage"]
                            });
                        }
                    }
                }

                using (var memoryStream = new MemoryStream())
                {
                    using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var f in documents)
                        {
                            var zipItem = zip.CreateEntry(f.FileName);
                            using var originalFileMemoryStream = new MemoryStream(f.DocumentLab.FileContent);
                            await using var entryStream = zipItem.Open();
                            await originalFileMemoryStream.CopyToAsync(entryStream, token);
                        }

                        zip.CreateEntryFromFile(newFileSortis, Path.GetFileName(newFileSortis),
                            CompressionLevel.Optimal);
                    }

                    var fileBytes = memoryStream.ToArray();
                    Response.Headers.Add("Content-Disposition",
                        "attachment; filename=LCBFT-Dossier_" + dossierLab.CodeUnique + ".zip");
                    if (System.IO.File.Exists(newFileSortis))
                        System.IO.File.Delete(newFileSortis);
                    return File(fileBytes, "application/zip");
                }
            }
            catch (Exception e)
            {
                return Ok(e);
            }
        }

        public void MergePDFs(string targetPath, List<string> pdfs)
        {
            using var targetDoc = new PdfDocument();
            foreach (var pdf in pdfs)
            {
                using (var pdfDoc = PdfReader.Open(pdf, PdfDocumentOpenMode.Import))
                {
                    for (var i = 0; i < pdfDoc.PageCount; i++)
                        targetDoc.AddPage(pdfDoc.Pages[i]);
                }

                if (System.IO.File.Exists(pdf))
                    System.IO.File.Delete(pdf);
            }

            targetDoc.Save(targetPath);
        }

        [HttpGet]
        public async Task<IActionResult> GetPartialExportDossierLab(string cryptedId, CancellationToken token = default)
        {
            try
            {
                var id = this.Uprotect(cryptedId, _protector);

                var dossierLabExport = await _labService.Dossier.GetDossierByDetails(id, token).ConfigureAwait(false);
                if (dossierLabExport != null)
                {
                    var newFileSortis = @"wwwroot/documentations/fr/extraction/lab/DS-LCB-FT-" +
                                        dossierLabExport.CodeUnique + "-" + DateTime.Now.ToString("dd-MM-yyy-HHmmss") +
                                        ".pdf";
                    var declarationTracfins = await _labService.DeclarationTracfin
                        .GetDeclarationByDetailsAsync(dossierLabExport.Id, token).ConfigureAwait(false);
                    var personnesPhysiques = _mapper.Map<IEnumerable<PersonnePhysiqueLabViewModel>>(await _labService
                        .DossierLabPersonnePhysique
                        .GetDossierLabPersonnePhysiqueByDetailsAsync(dossierLabExport.Id, token)
                        .ConfigureAwait(false));
                    var personnesMorales = _mapper.Map<IEnumerable<PersonneMoraleLabViewModel>>(await _labService
                        .DossierLabPersonneMorale.GetDossierLabPersonneMoraleByDetailsAsync(dossierLabExport.Id, token)
                        .ConfigureAwait(false));

                    var lang = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture);
                    var documentDossierList =
                        _labService.DocumentDossier.GetDocumentInfoByDossierId(dossierLabExport.Id, token);
                    _mapper.Map<List<DocumentDossierLabViewModel>>(documentDossierList);
                    var documents = await _labService.Dossier.GetDocumentDossierLabs(dossierLabExport.Id, token)
                        .ConfigureAwait(false);
                    var report = new DossierLabReport(_mapper, dossierLabExport, personnesPhysiques, personnesMorales,
                        declarationTracfins, _translator, lang);
                    report.ExportToPdf(newFileSortis);

                    using var memoryStream = new MemoryStream();
                    using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var f in documents)
                        {
                            var zipItem = zip.CreateEntry(f.FileName);
                            using var originalFileMemoryStream = new MemoryStream(f.DocumentLab.FileContent);
                            await using var entryStream = zipItem.Open();
                            await originalFileMemoryStream.CopyToAsync(entryStream, token);
                        }

                        zip.CreateEntryFromFile(newFileSortis, Path.GetFileName(newFileSortis),
                            CompressionLevel.Optimal);
                    }

                    var fileBytes = memoryStream.ToArray();
                    Response.Headers.Add("Content-Disposition",
                        "attachment; filename=Dosier_lcbft_" + dossierLabExport.CodeUnique + ".zip");
                    if (System.IO.File.Exists(newFileSortis))
                        System.IO.File.Delete(newFileSortis);
                    return File(fileBytes, "application/zip");
                }
            }
            catch (Exception e)
            {
                _logger.TraceError(e, "Error on GetPartialExportDossierLab with cryptedId : " + cryptedId);
            }
            finally
            {
                _logger.EndTrace();
            }

            return NoContent();
        }

        [HttpGet]
        public Task<IActionResult> GetPartialExportTypeDossierLab(CancellationToken token = default)
        {
            _logger.BeginTrace();
            return !IsVerifyUserHabilitation()
                ? Task.FromResult<IActionResult>(RedirectToAction("Login", "Account"))
                : Task.FromResult<IActionResult>(
                    PartialView("/Areas/Lab/Views/Dossier/_PartialExportDossierLabType.cshtml"));
        }

        [HttpGet]
        public async Task<IActionResult> GetPartialExportDemandeInformations(string cryptedId,
            CancellationToken token = default)
        {
            _logger.BeginTrace();
            if (!IsVerifyUserHabilitation())
                return RedirectToAction("Login", "Account");
            try
            {
                var lang = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture);
                var newFileSortis = @"wwwroot/documentations/fr/extraction/" +
                                    (lang == "fr" ? "Demande-information-lcbft_" : "Lcbft-information-request_") +
                                    DateTime.Now.ToString("dd-MM-yyy-HHmmss") + ".pdf";
                var id = this.Uprotect(cryptedId, _protector);
                var demandeInfoEnt =
                    await _labService.DemandeInformationLab.GetAsync(id, true, token).ConfigureAwait(false);
                var documents = new List<DocumentDemandeInformationLab>();


                if (demandeInfoEnt != null)
                {
                    var dossier = await _labService.Dossier.GetDossierLabCategorie(demandeInfoEnt.DossierLabId, token)
                        .ConfigureAwait(false);
                    if (dossier != null) demandeInfoEnt.DossierLab = dossier;
                    var url = Program._reportApiUrl() + "DemandeInfosReport/";
                    using (var httpClientHandler = new HttpClientHandler())
                    {
                        httpClientHandler.ServerCertificateCustomValidationCallback =
                            (message, cert, chain, errors) => true;
                        using (var _client = new HttpClient(httpClientHandler))
                        {
                            _client.BaseAddress = new Uri(url);
                            _client.DefaultRequestHeaders.Accept.Clear();
                            _client.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json"));
                            documents.AddRange(demandeInfoEnt.DocumentDemandeInformationLabRequests);
                            documents.AddRange(demandeInfoEnt.DocumentDemandeInformationLabResponses);
                            demandeInfoEnt.DocumentDemandeInformationLabRequests = null;
                            demandeInfoEnt.DocumentDemandeInformationLabResponses = null;
                            demandeInfoEnt.StatutDemandeInformationLab = null;
                            demandeInfoEnt.TypeDemandeInformationLab.DemandeInformationLabs = null;
                            demandeInfoEnt.Modificateur = null;
                            demandeInfoEnt.DossierLab.DemandeInformationLabs = null;
                            var categorie = new CategorieLab
                            {
                                FrenchName = demandeInfoEnt.DossierLab.Categorie?.FrenchName,
                                EnglishName = demandeInfoEnt.DossierLab.Categorie?.EnglishName
                            };
                            demandeInfoEnt.DossierLab.Categorie = categorie;
                            demandeInfoEnt.DossierLab.PersonneMoraleLabLienEntites = null;
                            demandeInfoEnt.DossierLab.DossierLabPersonnePhysiques = null;
                            demandeInfoEnt.DossierLab.DossierLabPersonneMorales = null;
                            demandeInfoEnt.DossierLab.DossierFraudesInLab = null;
                            demandeInfoEnt.DossierLab.DossierLabNonConnus = null;
                            demandeInfoEnt.DossierLab.DossierLabOperations = null;
                            demandeInfoEnt.DossierLab.DossierLabActions = null;
                            demandeInfoEnt.DossierLab.DossierLabHistos = null;
                            demandeInfoEnt.DossierLab.DossierLabScenarios = null;
                            demandeInfoEnt.DossierLab.DossierFraude = null;
                            demandeInfoEnt.DossierLab.DocumentDossierLabs = null;
                            demandeInfoEnt.DossierLab.Utilisateur = null;
                            demandeInfoEnt.DossierLab.Modificateur = null;
                            var reportModel = new ReportViewModel
                            {
                                Module = ActiviteModule.Lab,
                                Lang = lang,
                                Creator = demandeInfoEnt.Createur?.Nom + " " + demandeInfoEnt.Createur?.Prenom
                            };
                            demandeInfoEnt.Createur = null;
                            reportModel.DataSource = demandeInfoEnt;
                            var jsonOptions = new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                NullValueHandling = NullValueHandling.Ignore
                            };
                            var serlializedModel = JsonConvert.SerializeObject(reportModel, jsonOptions);
                            var response = await _client.PostAsJsonAsync("ExportDemandeInformations", serlializedModel,
                                token);
                            if (response.IsSuccessStatusCode)
                            {
                                var result = response.Content.ReadAsStringAsync(token).Result.Replace("\"", string.Empty);
                                await System.IO.File.WriteAllBytesAsync(newFileSortis, Convert.FromBase64String(result), token);
                            }
                            else
                            {
                                return new JsonResult(new
                                { success = true, errorMessage = _translator.Common["ErrorMessage"] });
                            }
                        }
                    }

                    byte[] fileBytes;

                    using (var memoryStream = new MemoryStream())
                    {
                        using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                        {
                            foreach (var f in documents)
                            {
                                var zipItem = zip.CreateEntry(f.DocumentLab.Name);
                                using var originalFileMemoryStream = new MemoryStream(f.DocumentLab.FileContent);
                                await using var entryStream = zipItem.Open();
                                await originalFileMemoryStream.CopyToAsync(entryStream, token);
                            }

                            zip.CreateEntryFromFile(newFileSortis, Path.GetFileName(newFileSortis),
                                CompressionLevel.Optimal);
                        }

                        fileBytes = memoryStream.ToArray();
                    }

                    Response.Headers.Add("Content-Disposition",
                        "attachment; filename=" +
                        (lang == "fr" ? "Demande-information-lcbft_" : "Lcbft-information-request_") +
                        demandeInfoEnt.DossierLab?.CodeUnique + ".zip");
                    if (System.IO.File.Exists(newFileSortis))
                        System.IO.File.Delete(newFileSortis);
                    return File(fileBytes, "application/zip");
                }

                return Json(new { Result = false, Data = "folder not found" });
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
                return Json(new { Result = false, Data = "error on Export DemandeInformation" });
            }
            finally
            {
                _logger.EndTrace();
            }
        }

        [HttpGet]
        public Task<int> GetPaysByIsoCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return Task.FromResult(0);
            var paysId = _labService.Referentiel.GetPays()
                .Where(x => x.IsoCode3Char == code)
                .Select(x => x.Id)
                .FirstOrDefault();
            return Task.FromResult(paysId);
        }

        [HttpGet]
        public Task<IEnumerable<SelectedItem>> GetIdentiteDirigeantByDossierId(string cryptedDossierId,
            string cryptedPersonneMoraleLabId)
        {
            _logger.BeginTrace();
            var models = new List<SelectedItem>();
            IEnumerable<SelectedItem> selectedItems = null;
            try
            {
                if (!IsVerifyUserHabilitation())
                    return Task.FromResult<IEnumerable<SelectedItem>>(null);
                if (cryptedDossierId != null)
                    if (int.TryParse(_protector.Unprotect(cryptedDossierId), out var dossierId) && dossierId > 0)
                        models = _labService.Dossier.GetPersonnesByDossierId(dossierId).ToList();
                if (cryptedPersonneMoraleLabId != null)
                    if (int.TryParse(_protector.Unprotect(cryptedPersonneMoraleLabId), out var personneMoraleId) &&
                        personneMoraleId > 0)
                        models.RemoveAll(x => x.Id == personneMoraleId);

                var additionalItems = new List<SelectedItem>
                {
                    new() { Id = 0, NameFr = "Inconnue", NameEn = "Unknown" },
                    new() { Id = -1, NameFr = "Autre", NameEn = "Other" }
                };

                selectedItems = models.Union(additionalItems);
            }
            catch (Exception e)
            {
                _logger.TraceError(e);
            }

            _logger.EndTrace();

            return Task.FromResult(selectedItems);
        }

        [HttpGet]
        public IActionResult AddCoordonneeDirigeantPersonneMoraleForm(string index)
        {
            var item = new DirigeantViewModel
            {
                Culture = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture)
            };
            var parts = index.Split('-');
            if (int.TryParse(parts[0], out var orderPersonneMorale) && int.TryParse(parts[1], out var orderCoordonnee))
                item.Order = orderCoordonnee;


            return PartialView("/Areas/Lab/Views/Dossier/_PartialDirigeantCoordonneePersonneMoraleLab.cshtml",
                new Tuple<DirigeantViewModel, int>(item, orderPersonneMorale));
        }

        [HttpGet]
        public IActionResult AddCoordonneeDirigeantBePersonneMoraleForm(string index)
        {
            var item = new DirigeantViewModel
            {
                Culture = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture)
            };
            var parts = index.Split('-');
            if (int.TryParse(parts[0], out var orderPersonneMorale) && int.TryParse(parts[1], out var orderCoordonnee))
                item.Order = orderCoordonnee;


            return PartialView("/Areas/Lab/Views/Dossier/_PartialBenefCoordonneePersonneMoraleLab.cshtml",
                new Tuple<DirigeantViewModel, int>(item, orderPersonneMorale));
        }

        [HttpGet]
        public Task<IList<SelectedItem>> GetReferenceFinanciereIdOperationEnCoursEdit(int personneId,
            string cryptedDossierId)
        {
            if (!IsVerifyUserHabilitation((int)ActiviteModule.Lab))
                Json(new ResponseViewModel<bool> { Response = false, Status = false });

            if (cryptedDossierId != null)
                if (int.TryParse(_protector.Unprotect(cryptedDossierId), out var dossierId) && dossierId > 0)
                {
                    var supportPp =
                        _labService.SupportFinancierPersonnePhysiqueService
                            .GetListItemsSupportFinancierPersonnePhysiqueById(personneId, dossierId);
                    var supportPm =
                        _labService.SupportFinancierPersonneMoraleService
                            .GetListItemsSupportFinancierPersonneMoraleById(personneId, dossierId);
                    supportPp.AddRange(supportPm);
                    return Task.FromResult<IList<SelectedItem>>(supportPp);
                }

            return Task.FromResult<IList<SelectedItem>>(null);
        }

        [HttpGet]
        public Task<IList<SelectedItem>> GetPersonnesSelectedItemList(string cryptedDossierId,
            CancellationToken cancellationToken = default)
        {
            if (!IsVerifyUserHabilitation((int)ActiviteModule.Lab))
                Json(new ResponseViewModel<bool> { Response = false, Status = false });

            if (cryptedDossierId != null)
                if (int.TryParse(_protector.Unprotect(cryptedDossierId), out var dossierLabId) && dossierLabId > 0)
                {
                    var personnePhysiqueLabs =
                        _labService.DossierLabPersonnePhysique.GetDossierLabPersonnePhysiqueToSelectList(
                            dossierLabId, cancellationToken);
                    var personnMoraleLabs =
                        _labService.DossierLabPersonneMorale.GetDossierLabPersonneMoraleToSelectList(
                            dossierLabId, cancellationToken);
                    personnePhysiqueLabs.AddRange(personnMoraleLabs);

                    return Task.FromResult<IList<SelectedItem>>(personnePhysiqueLabs);
                }

            return Task.FromResult<IList<SelectedItem>>(null);
        }

        [HttpGet]
        public Task<IActionResult> GetNatureRelationClientPartialPopup()
        {
            _logger.BeginTrace();

            return Task.FromResult<IActionResult>(PartialView("/Areas/Lab/Views/Dossier/_PartialNatureRelationClientPopup.cshtml"));
        }

        [HttpGet]
        public Task<IActionResult> GetPpePartialPopup()
        {
            _logger.BeginTrace();

            return Task.FromResult<IActionResult>(PartialView("/Areas/Lab/Views/Dossier/_PartialPpePopup.cshtml"));
        }

        [HttpGet]
        public Task<IList<SelectedItem>> GetCategorieById(int categorieId,
            CancellationToken cancellationToken = default)
        {
            if (!IsVerifyUserHabilitation((int)ActiviteModule.Lab))
                Json(new ResponseViewModel<bool> { Response = false, Status = false });
            var linePersonnePhysiqueByCategorieIds =
                _labService.DossierLabPersonnePhysique.GetLienPersonnePhysiqueById(categorieId, cancellationToken);
            return Task.FromResult<IList<SelectedItem>>(linePersonnePhysiqueByCategorieIds);
        }

        [HttpGet]
        public Task<IList<SelectedItem>> GetCategoriePersonneMoralePhysiqueById(int categorieId,
            CancellationToken cancellationToken = default)
        {
            if (!IsVerifyUserHabilitation((int)ActiviteModule.Lab))
                Json(new ResponseViewModel<bool> { Response = false, Status = false });
            var linePersonnePhysiqueByCategorieIds =
                _labService.DossierLabPersonneMorale.GetLienPersonneMoralePhysiqueById(categorieId, cancellationToken);
            return Task.FromResult<IList<SelectedItem>>(linePersonnePhysiqueByCategorieIds);
        }


        [HttpGet]
        public Task<IList<SelectedItem>> GetCategorieLienPersonnePhysiqueMoraleIdById(int categorieId,
            CancellationToken cancellationToken = default)
        {
            if (!IsVerifyUserHabilitation((int)ActiviteModule.Lab))
                Json(new ResponseViewModel<bool> { Response = false, Status = false });
            var linePersonnePhysiqueByCategorieIds =
                _labService.DossierLabPersonnePhysique.GetLienPersonnePhysiqueMoraleIdById(categorieId,
                    cancellationToken);
            return Task.FromResult<IList<SelectedItem>>(linePersonnePhysiqueByCategorieIds);
        }

        [HttpGet]
        public Task<IList<SelectedItem>> GetCategorieLienPersonneMoraleMoraleIdById(int categorieId,
            CancellationToken cancellationToken = default)
        {
            if (!IsVerifyUserHabilitation((int)ActiviteModule.Lab))
                Json(new ResponseViewModel<bool> { Response = false, Status = false });
            var lienPersonneMoraleMoraleByCategorieIds =
                _labService.DossierLabPersonneMorale.GetLienPersonneMoraleMoraleIdById(categorieId, cancellationToken);
            return Task.FromResult<IList<SelectedItem>>(lienPersonneMoraleMoraleByCategorieIds);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<bool> MajStatutDossierClotureTracfin(int? declarantId, DossierLabViewModel dossierLab,
            CancellationToken cancellationToken = default)
        {
            var result = false;
            if (!string.IsNullOrEmpty(dossierLab.CryptedId) && dossierLab.DeclarationTracfins.Any())
            {
                var numeroAccuseReception = dossierLab.DeclarationTracfins.FirstOrDefault()?.NumeroAccuseReception;
                var dateDeclaration = dossierLab.DeclarationTracfins.FirstOrDefault()?.DateDeclaration;
                var id = this.Uprotect(dossierLab.CryptedId, _protector);
                result = await _labService.Dossier
                    .UpdateStatusClotureTracfin(id, numeroAccuseReception, dateDeclaration, declarantId,
                        cancellationToken).ConfigureAwait(false);


                var eventDossier = new EventDossierViewModel
                {
                    DateCreation = DateTime.UtcNow,
                    IsActive = true,
                    ActiviteId = (int)ActiviteModule.Lab,
                    EventDossierTypeId = (int)EactionEventTypeDossier.CLOTURE_TRACFIN,
                    DossierLabId = id,
                    CodeDossier = dossierLab.CodeUnique,
                    CreateurId = currentUser.Id,
                    UtilisateurEventId = currentUser.Id
                };

                var eventEnt = _mapper.Map<EventDossier>(eventDossier);
                await _labService.EventDossier.AddAsync(eventEnt, cancellationToken).ConfigureAwait(false);
            }

            return result;
        }

        [HttpGet]
        public async Task<IActionResult> GetClotureTracfinPartialPopup(int directionId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            var utilisateurs = _mapper.Map<List<UtilisateurViewModel>>(await
                _referentielService
                    .GetUtilisateurInDirectionAsync(directionId, (int)ActiviteModule.Lab, cancellationToken)
                    .ConfigureAwait(false));

            return PartialView("/Areas/Lab/Views/Dossier/_PartialClotureTracfinPopup.cshtml", utilisateurs);
        }

        [HttpPost]
        public bool VerifDeclarationTracfin(DossierLabViewModel dossierLab)
        {
            var result = dossierLab?.DeclarationTracfins != null &&
                         dossierLab.DeclarationTracfins.Any();
            return result;
        }

        private bool VerifEstEnvoiDsTracfin(DossierLab dossierLab, bool estNouvelleDS, bool estDateDeclarationNull)
        {
            var estEnvoiTracfin = false;
            var directionDossier = dossierLab.DirectionId;
            var isDirectionTracfin = dossierLab.Direction.IsTracfin;
            var directionUtilisateurConnecte = currentUser.UtilisateurDirections?.FirstOrDefault(x =>
                x.ActiviteId == (int)ActiviteModule.Lab &&
                x.DirectionId == directionDossier);

            var identifiantErmes = currentUser.UtilisateurDirections?.Where(x =>
                x.ActiviteId == (int)ActiviteModule.Lab &&
                x.DirectionId == directionDossier).Select(x => x.IdentifiantErmes).FirstOrDefault();
            var numeroTeledeclarant = currentUser.UtilisateurDirections?.Where(x =>
                x.ActiviteId == (int)ActiviteModule.Lab &&
                x.DirectionId == directionDossier).Select(x => x.Teledeclarant).FirstOrDefault();
            var estDossierCloture = dossierLab.StatutDossierId == (int)StatutDossierLabEnum.Cloture;
            var isAdminGlobal = _roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal);

            if (isDirectionTracfin)
            {
                if (dossierLab.Direction.ModeEnvoieTracfinId == (int)ModeEnvoiTracfinEnum.Manuel)
                {
                    if (!isAdminGlobal)
                        estEnvoiTracfin = estNouvelleDS &&
                                          directionUtilisateurConnecte?.DirectionId == directionDossier &&
                                          estDossierCloture && dossierLab.Direction.IsTracfin && estDateDeclarationNull;
                    else
                        estEnvoiTracfin = estNouvelleDS && estDossierCloture && estDateDeclarationNull;
                }
                else
                {
                    if (!isAdminGlobal)
                        estEnvoiTracfin = estNouvelleDS &&
                                          estDossierCloture &&
                                          directionUtilisateurConnecte?.DirectionId == directionDossier &&
                                          estDateDeclarationNull && dossierLab.Direction.IsTracfin &&
                                          !string.IsNullOrEmpty(identifiantErmes) &&
                                          !string.IsNullOrEmpty(numeroTeledeclarant);
                    else
                        estEnvoiTracfin = estNouvelleDS &&
                                          estDossierCloture &&
                                          estDateDeclarationNull && !string.IsNullOrEmpty(identifiantErmes) &&
                                          !string.IsNullOrEmpty(numeroTeledeclarant);
                }
            }


            return estEnvoiTracfin;
        }

        private static string Soundex(string input, Language lang)
        {
            var result = new StringBuilder();

            if (!string.IsNullOrEmpty(input))
            {
                var previousCode = "";
                result.Append(input[0]); // keep initial char

                for (var i = 0; i < input.Length; i++) //start at 0 in order to correctly encode "Pf..."
                {
                    var currentLetter = input[i].ToString().ToLower();
                    var currentCode = "";
                    if (lang == Language.English)
                    {
                        if ("bfpv".Contains(currentLetter))
                            currentCode = "1";
                        else if ("cgjkqsxz".Contains(currentLetter))
                            currentCode = "2";
                        else if ("dt".Contains(currentLetter))
                            currentCode = "3";
                        else if (currentLetter == "l")
                            currentCode = "4";
                        else if ("mn".Contains(currentLetter))
                            currentCode = "5";
                        else if (currentLetter == "r")
                            currentCode = "6";
                    }
                    else
                    {
                        if ("bp".Contains(currentLetter))
                            currentCode = "1";
                        else if ("ckq".Contains(currentLetter))
                            currentCode = "2";
                        else if ("dt".Contains(currentLetter))
                            currentCode = "3";
                        else if (currentLetter == "l")
                            currentCode = "4";
                        else if ("mn".Contains(currentLetter))
                            currentCode = "5";
                        else if (currentLetter == "r")
                            currentCode = "6";
                        else if ("gj".Contains(currentLetter))
                            currentCode = "7";
                        else if ("xzs".Contains(currentLetter))
                            currentCode = "8";
                        else if ("fv".Contains(currentLetter))
                            currentCode = "9";
                    }

                    if (currentCode != previousCode && i > 0) // do not add first code to result string
                        result.Append(currentCode);

                    if (result.Length == 4) break;

                    previousCode = currentCode; // always retain previous code, even empty
                }
            }

            if (result.Length < 4)
                result.Append(new string('0', 4 - result.Length));

            return result.ToString().ToUpper();
        }

        [HttpGet]
        public IActionResult EnableAddNewContributeur(int indexDeclarationTracfin,
            int indexContributeur)
        {
            return PartialView("/Areas/Lab/Views/Dossier/Tracfin/_PartialContributeurDSFT.cshtml",
                new Tuple<PartialContributeurDSFTViewModel, int, int>(new PartialContributeurDSFTViewModel(),
                    indexDeclarationTracfin,
                    indexContributeur));
        }

        [HttpGet]
        public IActionResult HasTelephoneNumber()
        {
            return new JsonResult(new
            {
                status = !string.IsNullOrEmpty(currentUser.Telephone)
            });
        }

        [HttpGet]
        public async Task<IActionResult> ViewContent(string cryptedDossierId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            try
            {
                if (!IsVerifyUserHabilitation()) return new UnauthorizedResult();

                if (!int.TryParse(_protector.Unprotect(cryptedDossierId), out var dossierId) || dossierId <= 0)
                    return new BadRequestResult();

                var dossierLab = await _labService
                    .Dossier
                    .GetByIdWithOptionsAsync(dossierId, false, null, cancellationToken);

                if (!await EnableViewContent(dossierLab, cancellationToken)) return new UnauthorizedResult();

                var viewModel = new PartialViewContentViewModel
                {
                    DateDeclarationLocale = dossierLab.DateDeclarationLocale,
                    MotifsSoupcons = Encoding.UTF8.GetString(dossierLab.MotifsSoupcons)
                };

                return PartialView("/Areas/Lab/Views/Dossier/_PartialViewContent.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                _logger.TraceError(ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                _logger.EndTrace();
            }
        }

        [HttpGet]
        public async Task<IActionResult> IsFileEncrypted(string cryptedDocumentId,
            int categorieDocumentId,
            CancellationToken cancellationToken = default)
        {
            _logger.BeginTrace();
            try
            {
                if (!IsVerifyUserHabilitation()) return new UnauthorizedResult();

                if (!int.TryParse(_protector.Unprotect(cryptedDocumentId), out var documentId) || documentId <= 0)
                    return new BadRequestResult();

                var (fileContent, fileName) = await GetDocumentLab(documentId, categorieDocumentId, cancellationToken);

                if(fileContent is null || string.IsNullOrWhiteSpace(fileName))
                {
                    return new BadRequestResult();
                }

                var isEncrypted = FileEncryptionHeuristics.IsFileEncrypted(fileName, fileContent);

                return Ok(new { isEncrypted });
            }
            catch (Exception ex)
            {
                _logger.TraceError(ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                _logger.EndTrace();
            }
        }

        [HttpGet]
        public  Task<Dictionary<string,string>> GetQlbTraductionTemplates(CancellationToken cancellationToken = default)
        {
            if (!IsVerifyUserHabilitation((int)ActiviteModule.Lab))
                Json(new ResponseViewModel<bool> { Response = false, Status = false });
            var lang = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture);
            return _labService.Dossier.GetQlbTraductionTemplates(lang,cancellationToken);
        }

        private async Task<(byte[], string)> GetDocumentLab(int documentId, int categorieDocumentId, CancellationToken cancellationToken)
        {
            if (categorieDocumentId == (int)CategorieDocumentEnum.ARA)
            {
                var declarationTracfinFile = _labService.DocumentDossier.GetDeclarationTracfinFile(documentId);
                return (declarationTracfinFile.FileContent, declarationTracfinFile.FileName);
            }

            var documentlabFile = await _labService.DocumentDossier.GetDocument(documentId, false, cancellationToken);
            return (documentlabFile.FileContent, documentlabFile.Name);
        }
    }
}
