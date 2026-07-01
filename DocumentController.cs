using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BacaratWeb.Areas.Shared.Controllers;
using BacaratWeb.Core.Extensions;
using BacaratWeb.Core.Helper;
using BacaratWeb.Core.Services;
using BacaratWeb.Entities.Commun;
using BacaratWeb.Entities.Transfert;
using BacaratWeb.Helpers.RoleClaims;
using BacaratWeb.Model;
using BacaratWeb.Models.Referentials;
using BacaratWeb.Models.Referentials.Contracts;
using BacaratWeb.Services.Common.Interfaces;
using BacaratWeb.Services.Commun.Interfaces;
using BacaratWeb.Services.Transfert.Interfaces;
using BacaratWeb.Shared;
using BacaratWeb.ViewModel.Transfert;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using BacaratWeb.Extensions;
using System.Globalization;

namespace BacaratWeb.Areas.Transfert.Controllers
{
    [Authorize]
    [Area("Transfert")]
    public class DocumentController : BaseController
    {
        private readonly IDocumentService _documentService;
        private readonly IUtilisateurService _utilisateurService;
        private readonly IRoleAccessUser _roleAccessUser;
        private readonly IViewTranslator _translator;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly IEmailNotificationTemplateService _emailNotificationTemplateService;
        private readonly ILogger<DocumentController> _logger;
        private readonly IEventDossierLogger _eventDossierLogger;
        private readonly IUtilisateurDirectionService _utilisateurDirectionService;

        public DocumentController(
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IReferentielService referentielService,
            IUserInfoService userInfoService,
            IDocumentService documentService,
            IUtilisateurService utilisateurService,
            IRoleAccessUser roleAccessUser,
            IViewTranslator translator,
            IEmailNotificationService emailNotificationService,
            IEmailNotificationTemplateService emailNotificationTemplateService,
            ILogger<DocumentController> logger,
            IEventDossierLogger eventDossierLogger,
            IUtilisateurDirectionService utilisateurDirectionService)
            : base(httpContextAccessor, mapper, referentielService, userInfoService)
        {
            _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
            _utilisateurService = utilisateurService ?? throw new ArgumentNullException(nameof(utilisateurService));
            _roleAccessUser = roleAccessUser ?? throw new ArgumentNullException(nameof(roleAccessUser));
            _translator = translator ?? throw new ArgumentNullException(nameof(translator));
            _emailNotificationService = emailNotificationService ?? throw new ArgumentNullException(nameof(emailNotificationService));
            _emailNotificationTemplateService = emailNotificationTemplateService ?? throw new ArgumentNullException(nameof(emailNotificationTemplateService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _eventDossierLogger = eventDossierLogger ?? throw new ArgumentNullException(nameof(eventDossierLogger));
           _utilisateurDirectionService = utilisateurDirectionService ?? throw new ArgumentNullException(nameof(utilisateurDirectionService)); 
        }

        #region Droits Transfert
        private bool IsAdminGlobal()
        {
            return _roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal);
        }

        private bool CanReadTransfert()
        {
            return IsAdminGlobal()
                || _roleAccessUser.HasClaims(ActiviteModule.TRANSFERT, ClaimUser.READ)
                || _roleAccessUser.HasClaims(ActiviteModule.TRANSFERT, ClaimUser.WRITE);
        }

        private bool CanWriteTransfert()
        {
            return IsAdminGlobal()
                || _roleAccessUser.HasClaims(ActiviteModule.TRANSFERT, ClaimUser.WRITE);
        }

        private IActionResult Forbidden()
        {
            return new StatusCodeResult(StatusCodes.Status403Forbidden);
        }

        private void EnsureCanReadTransfert()
        {
            if (!CanReadTransfert())
                throw new CustomMessageException(_translator.Common["Transfert.Document.ShareUnauthorized"]);
        }

        private void EnsureCanWriteTransfert()
        {
            if (!CanWriteTransfert())
                throw new CustomMessageException(_translator.Common["Transfert.Document.ShareUnauthorized"]);
        }

        private bool CanUserAccessTransfert()
        {
            return IsAdminGlobal()
                || _roleAccessUser.HasClaims(ActiviteModule.TRANSFERT, ClaimUser.READ)
                || _roleAccessUser.HasClaims(ActiviteModule.TRANSFERT, ClaimUser.WRITE);
        }

        private bool CanRecipientAccessTransfert(Utilisateur recipient)
        {
            if (recipient == null)
                return false;

            return _roleAccessUser.HasRolesById(recipient.AspNetUsersId, null, null, RoleUser.AdminGlobal)
                || _utilisateurDirectionService.CanViewDossier((int)ActiviteModule.TRANSFERT, recipient.Id)
                || _utilisateurDirectionService.CanCreateDossier((int)ActiviteModule.TRANSFERT, recipient.Id);
        }

        private void ValidateRecipientsTransfertAccess(
            IEnumerable<string> emails,
            IEnumerable<Utilisateur> recipients)
        {
            var recipientsByEmail = recipients
                .Where(x => !string.IsNullOrWhiteSpace(x.Email))
                .ToDictionary(x => x.Email, StringComparer.OrdinalIgnoreCase);

            foreach (var email in emails)
            {
                if (!recipientsByEmail.TryGetValue(email, out var recipient))
                {
                    continue;
                }

                if (!CanRecipientAccessTransfert(recipient))
                {
                    throw new CustomMessageException(
                        string.Format(
                            _translator.Common["Transfert.Document.RecipientNotAuthorizedTransfer"],
                            email));
                }
            }
        }
        #endregion

        #region Page principale

        [Route("{culture:required}/transfert/document/{mode?}")]
        public IActionResult Index(string mode = "all")
        {
            if (!CanReadTransfert())
                return Forbidden();

            if (mode == "mine" && !CanWriteTransfert())
                mode = "all";

            ViewBag.Mode = mode;
            ViewBag.NomCurrentUser = CurrentUser.Nom;
            ViewBag.PrenomCurrentUser = CurrentUser.Prenom;
            ViewBag.CanReadTransfert = CanReadTransfert();
            ViewBag.CanWriteTransfert = CanWriteTransfert();
            ViewBag.IsAdminGlobal = IsAdminGlobal();

            return View();
        }

        #endregion

        #region Alimentation des dataGrids

        [HttpGet]
        public async Task<IActionResult> GetAllDocuments(CancellationToken token = default)
        {
            if (!CanReadTransfert())
                return Forbidden();

            var documents = await _documentService.GetAllDocumentsAsync(
                CurrentUser.Id,
                CurrentUser.Email,
                token);

            var data = _mapper.Map<IEnumerable<DocumentViewModel>>(documents);

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyDocuments(CancellationToken token = default)
        {
            if (!CanWriteTransfert())
                return Forbidden();

            var documents = await _documentService.GetMyDocumentsAsync(CurrentUser.Id, token);

            var data = _mapper.Map<IEnumerable<DocumentViewModel>>(documents);

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetSharedWithMeDocuments(CancellationToken token = default)
        {
            if (!CanReadTransfert())
                return Forbidden();

            var documentShares = await _documentService.GetSharedWithMeDocumentsAsync(
                CurrentUser.Id,
                CurrentUser.Email,
                token);

            var data = _mapper.Map<IEnumerable<DocumentViewModel>>(documentShares);

            return Json(data);
        }

        #endregion

        #region Ajout document

        [Route("{culture}/transfert/add-document")]
        public IActionResult AddNewDocument()
        {
            if (!CanWriteTransfert())
                return Forbidden();

            return PartialView("/Areas/Transfert/Views/Document/_PartialAddNewDocument.cshtml");
        }

        [HttpPost("{culture:required}/transfert/save-document")]
        public async Task<IActionResult> SaveDocument(
            AddDocumentViewModel model,
            CancellationToken token = default)
        {
            try
            {
                if (!CanWriteTransfert())
                    return Forbidden();

                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrorMessage());

                if (!new[] { 24, 48, 72 }.Contains(model.ExpiryDelayHours))
                    return BadRequest(_translator.Common["Transfert.Document.ExpiryDelayHoursRange"]);

                var extension = Path.GetExtension(model.File.FileName)?.ToLowerInvariant();

                if (!IsAllowedDocumentExtension(extension))
                    return BadRequest(_translator.Common["Transfert.Document.FileExtension"]);

                if (model.File.Length > 10 * 1024 * 1024)
                    return BadRequest(_translator.Common["Transfert.Document.FileSize"]);

                var emails = ExtractShareEmails(model.Emails);

                var recipients = await GetShareRecipientsAsync(emails, token);

                ValidateShareRecipients(emails, recipients);

                ValidateRecipientsTransfertAccess(emails, recipients);

                var statutActif = await _documentService.GetStatutDocumentByCodeAsync(
                    nameof(StatutDocumentTransfert.Active),
                    token);

                if (statutActif == null)
                    throw new CustomMessageException("Active status not found");

                var fileContent = await ReadFileContentAsync(model.File, token);

                var now = DateTimeOffset.Now;

                var expiryDate = CalculateBusinessExpiryDate(
                    now,
                    model.ExpiryDelayHours);

                var document = new Document
                {
                    Name = model.Name,
                    Description = model.Description,
                    OriginalFileName = model.File.FileName,
                    FileExtension = extension,
                    ContentType = model.File.ContentType,
                    FileContent = fileContent,
                    FileSize = model.File.Length,
                    UploadDate = now,
                    ExpiryDate = expiryDate,
                    OwnerId = CurrentUser.Id,
                    StatutDocumentId = statutActif.Id,
                    SecurityCode = GenerateSecurityCode(),
                    EncryptionKey = model.EncryptionKey
                };

                var shares = BuildDocumentShares(
                    recipients,
                    now,
                    expiryDate);

                var saved = await _documentService.AddDocumentWithSharesAsync(
                    document,
                    shares,
                    token);

                if (!saved)
                    return BadRequest(_translator.Common["Transfert.Document.ErrorWhileSavingDocument"]);

                var notificationsSent = await SendNotificationDocumentPartage(
                    document,
                    shares,
                    token);

                if (!notificationsSent)
                    return BadRequest(_translator.Common["Transfert.Document.ShareNotificationError"]);

                return Ok(new { success = true });
            }
            catch (CustomMessageException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.TraceError(ex);

                return StatusCode(
                    500,
                    _translator.Common["Transfert.Document.ErrorWhileSavingDocument"]);
            }
        }

        private bool CanAddNewDocument()
        {
            return _roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal)
                || _roleAccessUser.HasClaims(ActiviteModule.TRANSFERT, ClaimUser.WRITE);
        }

        private static bool IsAllowedDocumentExtension(string extension)
        {
            var allowedExtensions = new[] { ".zip", ".rar", ".tar", ".gz", ".7z" };

            return !string.IsNullOrWhiteSpace(extension)
                && allowedExtensions.Contains(extension);
        }

        private static async Task<byte[]> ReadFileContentAsync(
            IFormFile file,
            CancellationToken token = default)
        {
            using var memoryStream = new MemoryStream();

            await file.CopyToAsync(memoryStream, token);

            return memoryStream.ToArray();
        }

        private static string GenerateSecurityCode()
        {
            return Guid.NewGuid()
                .ToString("N")
                .Substring(0, 6)
                .ToUpperInvariant();
        }

        private static DateTimeOffset CalculateBusinessExpiryDate(DateTimeOffset startDate, int expiryDelayHours)
        {
            var businessDaysToAdd = expiryDelayHours / 24;

            return AddBusinessDays(startDate, businessDaysToAdd);
        }

        private static DateTimeOffset AddBusinessDays(
            DateTimeOffset startDate,
            int businessDaysToAdd)
        {
            var result = startDate;
            var addedBusinessDays = 0;

            while (addedBusinessDays < businessDaysToAdd)
            {
                result = result.AddDays(1);

                if (IsWeekend(result))
                    continue;

                addedBusinessDays++;
            }

            return result;
        }

        private static bool IsWeekend(DateTimeOffset date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday
                || date.DayOfWeek == DayOfWeek.Sunday;
        }

        #endregion

        #region Suppression document

        [HttpPost("{culture:required}/transfert/delete-document")]
        public async Task<IActionResult> DeleteDocument(
            int documentId,
            CancellationToken token = default)
        {
            try
            {
                if (!CanWriteTransfert())
                    return Forbidden();

                var document = await _documentService.GetAsync(documentId, true, token);

                if (document == null)
                    return BadRequest(_translator.Common["Transfert.Document.NotFound"]);

                EnsureCanDeleteDocument(document);

                var deleted = await _documentService.DeleteDocumentAsync(documentId, token);

                if (!deleted)
                    return BadRequest(_translator.Common["Transfert.Document.DeleteError"]);

                return Ok(new { success = true });
            }
            catch (CustomMessageException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.TraceError(ex);

                return StatusCode(
                    500,
                    _translator.Common["Transfert.Document.DeleteError"]);
            }
        }

        private void EnsureCanDeleteDocument(Document document)
        {
            if (document.OwnerId != CurrentUser.Id)
            {
                throw new CustomMessageException(
                    _translator.Common["Transfert.Document.DeleteUnauthorized"]);
            }
        }

        #endregion

        #region Téléchargement document

        [HttpGet("{culture:required}/transfert/download-document/{id:int}")]
        public async Task<IActionResult> Download(
            int id,
            CancellationToken token = default)
        {
            if (!CanReadTransfert())
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    _translator.Common["Transfert.Document.UserNotAuthorizedTransfer"]);
            }

            var document = await _documentService.GetDocumentForDownloadAsync(id, token);

            if (IsDocumentUnavailableForDownload(document))
                return BadRequest(_translator.Common["Transfert.Document.Expired"]);

            if (IsCurrentUserDocumentOwner(document))
                return BuildDownloadFileResult(document);

            var share = await _documentService.GetCurrentUserActiveDocumentShareAsync(
                id,
                CurrentUser.Id,
                CurrentUser.Email,
                token);

            if (share == null)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    _translator.Common["Transfert.Document.ShareInactive"]);
            }

            var vm = new DownloadDocumentViewModel
            {
                DocumentId = id
            };

            ViewBag.NomCurrentUser = CurrentUser.Nom;
            ViewBag.PrenomCurrentUser = CurrentUser.Prenom;

            return View("/Areas/Transfert/Views/Document/Download.cshtml", vm);
        }

        [HttpPost("{culture:required}/transfert/validate-security-code")]
        public async Task<IActionResult> ValidateSecurityCode(
            DownloadDocumentViewModel model,
            CancellationToken token = default)
        {
            try
            {
                if (!CanReadTransfert())
                {
                    return StatusCode(
                        StatusCodes.Status403Forbidden,
                        _translator.Common["Transfert.Document.UserNotAuthorizedTransfer"]);
                }

                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrorMessage());

                var document = await _documentService.GetDocumentForDownloadAsync(model.DocumentId, token);

                if (IsDocumentUnavailableForDownload(document))
                    return BadRequest(_translator.Common["Transfert.Document.Expired"]);

                var share = await _documentService.GetCurrentUserActiveDocumentShareAsync(
                    model.DocumentId,
                    CurrentUser.Id,
                    CurrentUser.Email,
                    token);

                if (share == null)
                {
                    return StatusCode(
                        StatusCodes.Status403Forbidden,
                        _translator.Common["Transfert.Document.ShareInactive"]);
                }

                if (!IsValidSecurityCode(document, model.SecurityCode))
                    return BadRequest(_translator.Common["Transfert.Document.InvalidSecurityCode"]);

                return Ok(new
                {
                    success = true,
                    downloadUrl = Url.Action(
                        nameof(DownloadFile),
                        "Document",
                        new
                        {
                            area = "Transfert",
                            culture = RouteData.Values["culture"],
                            id = model.DocumentId,
                            securityCode = model.SecurityCode
                        })
                });
            }
            catch (Exception ex)
            {
                _logger.TraceError(ex);

                return StatusCode(
                    500,
                    _translator.Common["Transfert.Document.DownloadError"]);
            }
        }

        [HttpGet("{culture:required}/transfert/download-file/{id:int}")]
        public async Task<IActionResult> DownloadFile(
            int id,
            string securityCode,
            CancellationToken token = default)
        {
            try
            {
                if (!CanReadTransfert())
                {
                    return StatusCode(
                        StatusCodes.Status403Forbidden,
                        _translator.Common["Transfert.Document.UserNotAuthorizedTransfer"]);
                }

                var document = await _documentService.GetDocumentForDownloadAsync(id, token);

                if (IsDocumentUnavailableForDownload(document))
                    return BadRequest(_translator.Common["Transfert.Document.Expired"]);

                if (IsCurrentUserDocumentOwner(document))
                {
                    await LogDocumentDownloadEvent(token);
                    return BuildDownloadFileResult(document);
                }

                var share = await _documentService.GetCurrentUserActiveDocumentShareAsync(
                    id,
                    CurrentUser.Id,
                    CurrentUser.Email,
                    token);

                if (share == null)
                {
                    return StatusCode(
                        StatusCodes.Status403Forbidden,
                        _translator.Common["Transfert.Document.ShareInactive"]);
                }

                if (!IsValidSecurityCode(document, securityCode))
                    return BadRequest(_translator.Common["Transfert.Document.InvalidSecurityCode"]);

                await _documentService.UpdateDocumentShareLastDownloadDateAsync(
                    share.Id,
                    DateTimeOffset.UtcNow,
                    token);

                await LogDocumentDownloadEvent(token);

                await SendDownloadNotification(document, token);

                return BuildDownloadFileResult(document);
            }
            catch (Exception ex)
            {
                _logger.TraceError(ex);

                return StatusCode(
                    500,
                    _translator.Common["Transfert.Document.DownloadError"]);
            }
        }

        private static bool IsDocumentUnavailableForDownload(Document document)
        {
            return document == null
                || document.ExpiryDate < DateTimeOffset.Now;
        }

        private bool IsCurrentUserDocumentOwner(Document document)
        {
            return document.OwnerId == CurrentUser.Id;
        }

        private async Task<bool> CanCurrentUserDownloadSharedDocumentAsync(
            int documentId,
            CancellationToken token = default)
        {
            return await _documentService.CanUserDownloadSharedDocumentAsync(
                documentId,
                CurrentUser.Id,
                CurrentUser.Email,
                token);
        }

        private static bool IsValidSecurityCode(Document document, string securityCode)
        {
            return !string.IsNullOrWhiteSpace(securityCode)
                && string.Equals(
                    document.SecurityCode,
                    securityCode,
                    StringComparison.OrdinalIgnoreCase);
        }

        private FileResult BuildDownloadFileResult(Document document)
        {
            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(document.OriginalFileName, out var contentType))
                contentType = "application/octet-stream";

            return File(
                document.FileContent,
                contentType,
                document.OriginalFileName);
        }

        private async Task LogDocumentDownloadEvent(CancellationToken token = default)
        {
            await _eventDossierLogger.LogDossierEvent(
                EactionEventTypeDossier.TELECHARGEMENT_DOCUMENT,
                CurrentUser.Id,
                null,
                ActiviteModule.TRANSFERT,
                x => { },
                token);
        }

        private async Task SendDownloadNotification(
            Document document,
            CancellationToken token = default)
        {
            if (document.Owner == null)
                return;

            var notificationTypes = await _emailNotificationTemplateService
                .GetEmailNotificationTypes(token);

            var documentTelechargeType = notificationTypes
                .FirstOrDefault(x => x.Code == "DocumentTelecharge");

            if (documentTelechargeType == null)
                return;

            var template = await _emailNotificationTemplateService.GetEmailNotification(
                (int)ActiviteModule.TRANSFERT,
                documentTelechargeType.Id,
                document.Owner.LangueId,
                token);

            if (template == null)
                return;

            var cultureInfo = Culture.GetCultureByLanguageId(template.LangueId);

            var bodyArgs = new object[]
            {
                document.Name,
                UtilisateurHelper.FormatUserName(CurrentUser.Prenom, CurrentUser.Nom),
                DateTimeOffset.Now.ToString("dddd d MMMM yyyy 'à' HH:mm '(UTC'zzz')'", cultureInfo)
            };

            var emailNotification = new EmailNotification
            {
                EmailTo = document.Owner.Email,
                Subject = template.Subject,
                Body = string.Format(cultureInfo, template.Body, bodyArgs),
                DateNotification = DateTime.Now
            };

            await _emailNotificationService.PushNotifications(
                new List<EmailNotification> { emailNotification },
                token);
        }

        #endregion

        #region Ajout/Modification Partage

        [HttpGet("{culture:required}/transfert/share-document")]
        public async Task<IActionResult> ShareDocument(
            int documentId,
            CancellationToken token = default)
        {

            if (!CanWriteTransfert())
                return Forbidden();

            var document = await _documentService.GetAsync(documentId, true, token);

            if (document == null)
                return NotFound();

            var now = DateTimeOffset.Now;

            var vm = new ShareDocumentViewModel
            {
                DocumentId = document.Id,
                StartDate = now,
                ExpiryDate = document.ExpiryDate,
                DocumentExpiryDate = document.ExpiryDate
            };

            return PartialView(
                "/Areas/Transfert/Views/Document/_PartialShareDocument.cshtml",
                vm);
        }

        [HttpPost("{culture:required}/transfert/save-share-document")]
        public async Task<IActionResult> SaveShareDocument(
            ShareDocumentViewModel model,
            CancellationToken token = default)
        {
            try
            {
                if (!CanWriteTransfert())
                    return Forbidden();

                if (!ModelState.IsValid)
                    return BadRequest(GetModelStateErrorMessage());

                var document = await GetDocumentForShare(model.DocumentId, token);

                EnsureCanManageDocumentShares(document);

                PrepareShareModel(model, document);

                ValidateShareDates(model, document);

                var emails = ExtractShareEmails(model.Emails);

                var recipients = await GetShareRecipientsAsync(emails, token);

                ValidateShareRecipients(emails, recipients);

                ValidateRecipientsTransfertAccess(emails, recipients);

                var shares = BuildDocumentShares(document, recipients, model);

                var saved = await SaveDocumentSharesAsync(model, shares, token);

                if (!saved)
                {
                    return BadRequest(
                        model.DocumentShareId.HasValue
                            ? _translator.Common["Transfert.Document.ShareUpdateError"]
                            : _translator.Common["Transfert.Document.ShareError"]);
                }

                var notificationsSent = await SendNotificationDocumentPartage(document, shares, token);

                if (!notificationsSent)
                    return BadRequest(_translator.Common["Transfert.Document.ShareNotificationError"]);

                return Ok(new { success = true });
            }
            catch (CustomMessageException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.TraceError(ex);

                return StatusCode(
                    500,
                    model.DocumentShareId.HasValue
                        ? _translator.Common["Transfert.Document.ShareUpdateError"]
                        : _translator.Common["Transfert.Document.ShareError"]);
            }
        }

        private async Task<Document> GetDocumentForShare(
            int documentId,
            CancellationToken token)
        {
            var document = await _documentService.GetAsync(documentId, true, token);

            if (document == null)
            {
                throw new CustomMessageException(
                    _translator.Common["Transfert.Document.DocumentNotFound"]);
            }

            return document;
        }

        private void PrepareShareModel(
            ShareDocumentViewModel model,
            Document document)
        {
            model.StartDate = DateTimeOffset.Now;
            model.DocumentExpiryDate = document.ExpiryDate;
        }

        private void ValidateShareDates(
            ShareDocumentViewModel model,
            Document document)
        {
            if (model.StartDate.Date < DateTimeOffset.Now.Date)
            {
                throw new CustomMessageException(
                    _translator.Common["Transfert.Document.ShareStartDateInvalid"]);
            }

            if (model.ExpiryDate.Date <= model.StartDate.Date)
            {
                throw new CustomMessageException(
                    _translator.Common["Transfert.Document.ShareExpiryDateInvalid"]);
            }

            if (model.ExpiryDate.Date > document.ExpiryDate.Date)
            {
                throw new CustomMessageException(
                    _translator.Common["Transfert.Document.ShareExpiryDateDocumentInvalid"]);
            }
        }

        private static List<string> ExtractShareEmails(string emails)
        {
            return emails
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private async Task<List<Utilisateur>> GetShareRecipientsAsync(
            IEnumerable<string> emails,
            CancellationToken token)
        {
            var recipients = new List<Utilisateur>();

            foreach (var email in emails)
            {
                var user = await _utilisateurService.GetUtilisateurByEmailAsync(email, token);

                if (user != null)
                    recipients.Add(user);
            }

            return recipients;
        }

        private void ValidateShareRecipients(
            IEnumerable<string> emails,
            IEnumerable<Utilisateur> recipients)
        {
            var recipientEmails = recipients
                .Select(x => x.Email)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var invalidEmails = emails
                .Where(x => !recipientEmails.Contains(x))
                .ToList();

            if (invalidEmails.Any())
            {
                throw new CustomMessageException(
                    string.Format(
                        _translator.Common["Transfert.Document.ShareUserNotFound"],
                        string.Join(", ", invalidEmails)));
            }
        }

        private List<DocumentShare> BuildDocumentShares(
            Document document,
            IEnumerable<Utilisateur> recipients,
            ShareDocumentViewModel model)
        {
            return recipients
                .Select(recipient => new DocumentShare
                {
                    DocumentId = document.Id,
                    SharedWithUserId = recipient.Id,
                    CreateurId = CurrentUser.Id,
                    Email = recipient.Email,
                    SharedDate = model.StartDate,
                    ExpiryDate = model.ExpiryDate,
                    IsActive = true
                })
                .ToList();
        }

        private List<DocumentShare> BuildDocumentShares(
            IEnumerable<Utilisateur> recipients,
            DateTimeOffset sharedDate,
            DateTimeOffset expiryDate)
        {
            return recipients
                .Select(recipient => new DocumentShare
                {
                    SharedWithUserId = recipient.Id,
                    CreateurId = CurrentUser.Id,
                    Email = recipient.Email,
                    SharedDate = sharedDate,
                    ExpiryDate = expiryDate,
                    IsActive = true
                })
                .ToList();
        }

        private async Task<bool> SaveDocumentSharesAsync(
            ShareDocumentViewModel model,
            IEnumerable<DocumentShare> shares,
            CancellationToken token)
        {
            return model.DocumentShareId.HasValue
                ? await _documentService.ReplaceDocumentShareAsync(
                    model.DocumentShareId.Value,
                    shares,
                    token)
                : await _documentService.AddDocumentSharesAsync(
                    shares,
                    token);
        }

        private async Task<bool> SendNotificationDocumentPartage(
            Document document,
            IEnumerable<DocumentShare> shares,
            CancellationToken token = default)
        {
            var emailNotifications = new List<EmailNotification>();

            foreach (var share in shares)
            {
                var recipient = await ResolveShareRecipientAsync(share, token);

                if (recipient == null)
                    continue;

                var template = await _emailNotificationTemplateService.GetEmailNotification(
                    (int)ActiviteModule.TRANSFERT,
                    (int)NotificationType.DocumentPartage,
                    recipient.LangueId,
                    token);

                if (template == null)
                    throw new CustomMessageException("Notification template not found");

                var cultureInfo = Culture.GetCultureByLanguageId(template.LangueId);

                emailNotifications.Add(
                    BuildShareEmailNotification(
                        document,
                        share,
                        recipient,
                        template,
                        cultureInfo));
            }

            if (!emailNotifications.Any())
                return false;

            return await _emailNotificationService.PushNotifications(emailNotifications, token);
        }

        private async Task<Utilisateur> ResolveShareRecipientAsync(
            DocumentShare share,
            CancellationToken token)
        {
            if (share.SharedWithUser != null)
                return share.SharedWithUser;

            return await _utilisateurService.GetUtilisateurByEmailAsync(share.Email, token);
        }

        private EmailNotification BuildShareEmailNotification(
            Document document,
            DocumentShare share,
            Utilisateur recipient,
            EmailNotificationTemplate template,
            CultureInfo cultureInfo)
        {
            var bodyArgs = new object[]
            {
                document.Name,
                UtilisateurHelper.FormatUserName(CurrentUser.Prenom, CurrentUser.Nom),
                share.SharedDate.ToString("dddd d MMMM yyyy 'à' HH:mm '(UTC'zzz')'", cultureInfo),
                share.ExpiryDate.ToString("dddd d MMMM yyyy 'à' HH:mm '(UTC'zzz')'", cultureInfo),
                GetTransfertUrl(),
                document.SecurityCode,
                document.EncryptionKey
            };

            return new EmailNotification
            {
                EmailTo = recipient.Email,
                Subject = template.Subject,
                Body = string.Format(cultureInfo, template.Body, bodyArgs),
                DateNotification = DateTime.Now
            };
        }

        private string GetTransfertUrl(string mode = "all")
        {
            return Url.Action(
                nameof(Index),
                "Document",
                new
                {
                    area = "Transfert",
                    culture = this.GetCurrentLanguage().ToLower(CultureInfo.CurrentCulture),
                    mode
                },
                Request.Scheme);
        }

        #endregion

        #region Paramètres de partage

        [HttpGet("{culture:required}/transfert/share-settings")]
        public async Task<IActionResult> ShareSettings(
            int documentId,
            CancellationToken token = default)
        {
            if (!CanWriteTransfert())
                return Forbidden();

            var document = await GetDocumentForShare(documentId, token);

            EnsureCanManageDocumentShares(document);

            var vm = new ShareDocumentSettingsViewModel
            {
                DocumentId = document.Id,
                DocumentName = document.Name
            };

            return PartialView(
                "/Areas/Transfert/Views/Document/_PartialShareSettingsDocument.cshtml",
                vm);
        }

        [HttpGet]
        public async Task<IActionResult> GetDocumentShares(
            int documentId,
            CancellationToken token = default)
        {
            if (!CanWriteTransfert())
                return Forbidden();

            var document = await GetDocumentForShare(documentId, token);

            EnsureCanManageDocumentShares(document);

            var shares = await _documentService.GetDocumentSharesAsync(documentId, token);

            var data = _mapper.Map<IEnumerable<DocumentShareViewModel>>(shares);

            return Json(data);
        }

        [HttpGet("{culture:required}/transfert/edit-share-document")]
        public async Task<IActionResult> EditShareDocument(
            int documentShareId,
            CancellationToken token = default)
        {
            if (!CanWriteTransfert())
                return Forbidden();

            var share = await _documentService.GetDocumentShareAsync(documentShareId, token);

            if (share == null)
                return NotFound(_translator.Common["Transfert.Document.ShareNotFound"]);

            EnsureCanManageDocumentShares(share.Document);

            var vm = new ShareDocumentViewModel
            {
                DocumentShareId = share.Id,
                DocumentId = share.DocumentId,
                Emails = share.Email,
                StartDate = DateTimeOffset.Now,
                ExpiryDate = share.ExpiryDate,
                DocumentExpiryDate = share.Document.ExpiryDate
            };

            return PartialView(
                "/Areas/Transfert/Views/Document/_PartialShareDocument.cshtml",
                vm);
        }

        [HttpPost("{culture:required}/transfert/delete-share-document")]
        public async Task<IActionResult> DeleteShareDocument(
            int documentShareId,
            CancellationToken token = default)
        {
            try
            {
                if (!CanWriteTransfert())
                    return Forbidden();

                var share = await _documentService.GetDocumentShareAsync(documentShareId, token);

                if (share == null)
                    return BadRequest(_translator.Common["Transfert.Document.ShareNotFound"]);

                EnsureCanManageDocumentShares(share.Document);

                var deleted = await _documentService.DeleteDocumentShareAsync(documentShareId, token);

                if (!deleted)
                    return BadRequest(_translator.Common["Transfert.Document.ShareDeleteError"]);

                return Ok(new { success = true });
            }
            catch (CustomMessageException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.TraceError(ex);

                return StatusCode(
                    500,
                    _translator.Common["Transfert.Document.ShareDeleteError"]);
            }
        }

        private void EnsureCanManageDocumentShares(Document document)
        {
            if (document == null)
            {
                throw new CustomMessageException(
                    _translator.Common["Transfert.Document.DocumentNotFound"]);
            }

            var isOwner = document.OwnerId == CurrentUser.Id;
            var isAdminGlobal = _roleAccessUser.HasRoles(null, null, RoleUser.AdminGlobal);

            if (!isOwner && !isAdminGlobal)
            {
                throw new CustomMessageException(
                    _translator.Common["Transfert.Document.ShareUnauthorized"]);
            }
        }

        [HttpPost("{culture:required}/transfert/disable-share-document")]
        public async Task<IActionResult> DisableShareDocument(
            int documentShareId,
            CancellationToken token = default)
        {
            try
            {
                if (!CanWriteTransfert())
                    return Forbidden();

                var share = await _documentService.GetDocumentShareAsync(documentShareId, token);

                if (share == null)
                    return BadRequest(_translator.Common["Transfert.Document.ShareNotFound"]);

                EnsureCanManageDocumentShares(share.Document);

                var disabled = await _documentService.DisableDocumentShareAsync(
                    documentShareId,
                    token);

                if (!disabled)
                    return BadRequest(_translator.Common["Transfert.Document.ShareDeleteError"]);

                return Ok(new { success = true });
            }
            catch (CustomMessageException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.TraceError(ex);

                return StatusCode(
                    500,
                    _translator.Common["Transfert.Document.ShareDeleteError"]);
            }
        }
        #endregion

        #region Helpers Communs

        private string GetModelStateErrorMessage()
        {
            var errorKey = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(errorKey))
                return "Données invalides.";

            return _translator.Common[errorKey];
        }

        #endregion
    }
}
