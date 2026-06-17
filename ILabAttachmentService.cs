using BacaratWeb.Areas.Lab.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BacaratWeb.Areas.Lab.Services.Interfaces
{
    public interface ILabAttachmentService
    {
        Task<IList<DocumentDossierLabViewModel>> GetAttachmentsByDossierId(
            string cryptedDossierId,
            string culture,
            CancellationToken cancellationToken = default);

        Task<DossierLabViewModel> GetAttachmentsFraudeByDossierId(string cryptedDossierFraudeId,
            string culture,
            CancellationToken cancellationToken = default);
    }
}
