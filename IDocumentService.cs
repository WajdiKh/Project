using BacaratWeb.Services.Commun.Interfaces;
using BacaratWeb.Entities.Transfert;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace BacaratWeb.Services.Transfert.Interfaces
{
    public interface IDocumentService : IBacaratWebService<Document>
    {
        Task<bool> AddDocumentAsync(Document document, CancellationToken token = default);

        Task<IEnumerable<Document>> GetMyDocumentsAsync(int ownerId, CancellationToken token = default);

        Task<List<DocumentShare>> GetSharedWithMeDocumentsAsync(int userId, string email, CancellationToken token = default);

        Task<StatutDocument> GetStatutDocumentByCodeAsync(string code, CancellationToken token = default);

        Task<Document> GetDocumentByIdAndOwnerAsync(int documentId, int ownerId, CancellationToken token = default);

        Task<bool> ShareDocumentAsync(int documentId, IEnumerable<DocumentShare> shares, CancellationToken token = default);

        Task<IEnumerable<DocumentShare>> GetDocumentSharesAsync(int documentId, CancellationToken token = default);

        Task<DocumentShare> GetDocumentShareAsync(int documentShareId, CancellationToken token = default);

        Task<bool> AddDocumentSharesAsync(IEnumerable<DocumentShare> shares, CancellationToken token = default);

        Task<bool> ReplaceDocumentShareAsync(int documentShareId, IEnumerable<DocumentShare> replacementShares, CancellationToken token = default);

        Task<bool> DeleteDocumentShareAsync(int documentShareId, CancellationToken token = default);

        // Nouveau : documents de l'utilisateur connecté + documents partagés avec lui
        Task<List<Document>> GetAllDocumentsAsync(
            int userId,
            string email,
            CancellationToken token = default);

        // Nouveau : récupérer un document avec Owner + Shares pour le téléchargement
        Task<Document> GetDocumentForDownloadAsync(
            int documentId,
            CancellationToken token = default);

        // Nouveau : vérifier qu'un partage actif existe pour l'utilisateur connecté
        Task<bool> CanUserDownloadSharedDocumentAsync(
            int documentId,
            int userId,
            string email,
            CancellationToken token = default);

        // Nouveau : suppression physique du document + partages
        Task<bool> DeleteDocumentAsync(
            int documentId,
            CancellationToken token = default);
        Task<bool> AddDocumentWithSharesAsync(
            Document document,
            IEnumerable<DocumentShare> shares,
            CancellationToken token = default);

        Task<DocumentShare> GetCurrentUserActiveDocumentShareAsync(
            int documentId,
            int userId,
            string email,
            CancellationToken token = default);

        Task<bool> DisableDocumentShareAsync(
            int documentShareId,
            CancellationToken token = default);

        Task<bool> UpdateDocumentShareLastDownloadDateAsync(
            int documentShareId,
            DateTimeOffset lastDownloadDate,
            CancellationToken token = default);
    }
}
