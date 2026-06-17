using AutoMapper;
using BacaratWeb.Areas.Lab.Models;
using BacaratWeb.Areas.Lab.Services.Interfaces;
using BacaratWeb.Services.Fraude.Interfaces;
using BacaratWeb.Services.Lab.Interfaces;
using BacaratWeb.Shared;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BacaratWeb.Areas.Lab.Services
{
    public class LabAttachmentService : ILabAttachmentService
    {
        private readonly IDataProtector _protector;
        private readonly IDossierLabService _dossierLabService;
        private readonly IDocumentDossierLabService _documentDossierLabService;
        private readonly IDossierFraudeService _dossierFraudeService;
        private readonly IDocumentDossierFraudeService _documentDossierFraudeService;
        private readonly IMapper _mapper;
        private readonly ILogger<LabAttachmentService> _logger;
        public LabAttachmentService(IDataProtectionProvider protectionProvider,
            IDossierLabService dossierLabService,
            IDocumentDossierLabService documentDossierLabService,
            IDossierFraudeService dossierFraudeService,
            IDocumentDossierFraudeService documentDossierFraudeService,
            IMapper mapper,
            ILogger<LabAttachmentService> logger)
        {
            _protector = protectionProvider?.CreateProtector("Anti_Tempered_Parameters");
            _dossierLabService = dossierLabService;
            _documentDossierLabService = documentDossierLabService;
            _dossierFraudeService = dossierFraudeService;
            _documentDossierFraudeService = documentDossierFraudeService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IList<DocumentDossierLabViewModel>> GetAttachmentsByDossierId(
            string cryptedDossierId,
            string culture,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (cryptedDossierId == null) return null;

                var id = Convert.ToInt32(_protector.Unprotect(cryptedDossierId));

                var dossierLab = await _dossierLabService.GetAsync(id,
                    false, cancellationToken);

                if (dossierLab == null) return null;

                var documentDossierList = _documentDossierLabService
                        .GetDocumentInfoByDossierId(id, cancellationToken);

                var result = _mapper.Map<List<DocumentDossierLabViewModel>>(documentDossierList);

                if (result.All(x => x.CategorieDocumentId != 10))
                {
                    var declarationfiles = await _dossierLabService.GetDeclarationTracfinFiles(id, culture, cancellationToken);
                    if (declarationfiles.Any())
                    {
                        result.AddRange(declarationfiles);
                    }

                }


                foreach (var document in result)
                {
                    document.CryptedDossierLabId = _protector.Protect(document.DossierLabId.ToString(CultureInfo.CurrentCulture));
                    document.CryptedDocumentLabId = _protector.Protect(document.DocumentLabId.ToString(CultureInfo.CurrentCulture));
                    document.CryptedId = _protector.Protect(document.Id.ToString(CultureInfo.CurrentCulture));
                    document.Id = -1;

                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while getting attachments. {ex}");
                return null;
            }
        }

        public async Task<DossierLabViewModel> GetAttachmentsFraudeByDossierId(string cryptedDossierFraudeId,
    string culture,
    CancellationToken cancellationToken = default)
        {
            var dossierLab = new DossierLabViewModel();
            if (cryptedDossierFraudeId == null) return null;
            try
            {
                var id = Convert.ToInt32(_protector.Unprotect(cryptedDossierFraudeId));
                var dossierFraude = await _dossierFraudeService.GetAsync(id,
                                        false, cancellationToken);
                if (dossierFraude == null) return null;

                var documentDossierList = await _documentDossierFraudeService
                           .GetDocumentByDossierId(id).ConfigureAwait(false);
                if (documentDossierList.Any())
                {
                    dossierLab.Attachments = documentDossierList.Select(x => new AttachmentViewModel
                    {
                        File = GetFileFromByteArray(x.DocumentFraude.FileContent, x.DocumentName, x.FileName, x.DocumentFraude.FileType)

                    }).ToList();
                    dossierLab.DocumentDossierLabs = documentDossierList.Select(x => new DocumentDossierLabViewModel
                    {
                        DocumentName = x.DocumentName,
                        FileName = x.FileName,
                        CategorieDocumentId = (int?)CategorieDocumentEnum.ANA,
                        DocumentLab = new DocumentLabViewModel
                        {
                            FileContent = ConvertByteToString(x.DocumentFraude.FileContent),
                            FileSize = x.DocumentFraude.FileSize,
                            Name = x.DocumentFraude.Name,
                            FileType = x.DocumentFraude.FileType
                        }
                    }).ToList();

                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while getting attachments. {ex}");
            }

            return dossierLab;
        }

        private static string ConvertByteToString(byte[] source)
        {
            return source != null ? Convert.ToBase64String(source) : null;

        }

        private static IFormFile GetFileFromByteArray (byte[] byteFile, string name, string filename, string contentType)
        {
            var stream = new MemoryStream(byteFile);
            IFormFile formFile = new FormFile(stream, baseStreamOffset: 0, length: byteFile.Length, name: name, fileName: filename)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
            return formFile;
        }

    }
}
