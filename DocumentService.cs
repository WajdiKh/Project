using BacaratWeb.Entities.Transfert;
using BacaratWeb.Model.Entities;
using BacaratWeb.Services.Transfert.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BacaratWeb.Services.Transfert.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly BacaratWebContext _context;

        public DocumentService(BacaratWebContext context)
        {
            _context = context;
        }

        public async Task<bool> AddDocumentAsync(
            Document document,
            DocumentShare documentShare,
            CancellationToken token = default)
        {
            await _context.Documents
                .AddAsync(document, token)
                .ConfigureAwait(false);

            documentShare.Document = document;

            await _context.DocumentShares
                .AddAsync(documentShare, token)
                .ConfigureAwait(false);

            return await _context.SaveChangesAsync(token)
                .ConfigureAwait(false) > 0;
        }

        public async Task<IEnumerable<Document>> GetMyDocumentsAsync(
            int ownerId,
            CancellationToken token = default)
        {
            return await _context.Documents
                .Include(x => x.Owner)
                .Include(x => x.StatutDocument)
                .Where(x => x.OwnerId == ownerId)
                .OrderByDescending(x => x.UploadDate)
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<DocumentShare>> GetSharedWithMeDocumentsAsync(
            int userId,
            string email,
            CancellationToken token = default)
        {
            return await _context.DocumentShares
                .Include(x => x.Document)
                    .ThenInclude(x => x.Owner)
                .Include(x => x.Document)
                    .ThenInclude(x => x.StatutDocument)
                .Where(x => x.SharedWithUserId == userId || x.Email == email)
                .OrderByDescending(x => x.SharedDate)
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<StatutDocument> GetStatutDocumentByCodeAsync(
            string code,
            CancellationToken token = default)
        {
            return await _context.StatutDocuments
                .FirstOrDefaultAsync(x => x.Code == code && x.IsActive, token)
                .ConfigureAwait(false);
        }
    }
}