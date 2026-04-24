using BacaratWeb.Entities.Transfert;
using BacaratWeb.Services.Commun.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BacaratWeb.Services.Transfert.Services.Interfaces
{
    public interface IDocumentService : IBacaratWebService<Document>
    {
        Task<bool> AddDocumentAsync(Document document, DocumentShare documentShare, CancellationToken token = default);

        Task<IEnumerable<Document>> GetMyDocumentsAsync(int ownerId, CancellationToken token = default);

        Task<IEnumerable<DocumentShare>> GetSharedWithMeDocumentsAsync(int userId, string email, CancellationToken token = default);

        Task<StatutDocument> GetStatutDocumentByCodeAsync(string code, CancellationToken token = default);
    }
}