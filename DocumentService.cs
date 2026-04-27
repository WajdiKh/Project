using BacaratWeb.Entities.Transfert;
using BacaratWeb.Services.Commun;
using BacaratWeb.Services.Transfert.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using BacaratWeb.Model.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BacaratWeb.Services.Transfert.Services
{
    public class DocumentService : BacaratWebService<Document>, IDocumentService
    {
        public DocumentService(BacaratWebContext context)
            : base(context)
        {
        }

        public async Task<bool> AddAsync(Document entity, CancellationToken token = default)
        {
            await Context.Documents.AddAsync(entity, token).ConfigureAwait(false);
            return await Context.SaveChangesAsync(token).ConfigureAwait(false) > 0;
        }

        public async Task<bool> UpdateAsync(Document entity, CancellationToken token = default)
        {
            Context.Documents.Update(entity);
            return await Context.SaveChangesAsync(token).ConfigureAwait(false) > 0;
        }

        public async Task<bool> DeleteAsync(Document entity, CancellationToken token = default)
        {
            Context.Documents.Remove(entity);
            return await Context.SaveChangesAsync(token).ConfigureAwait(false) > 0;
        }

        public async Task<Document> GetAsync(int id, bool include = false, CancellationToken token = default)
        {
            IQueryable<Document> query = Context.Documents;

            if (include)
            {
                query = query
                    .Include(x => x.Owner)
                    .Include(x => x.StatutDocument)
                    .Include(x => x.DocumentShares);
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id, token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Document>> GetManyAsync(bool include = false, CancellationToken token = default)
        {
            IQueryable<Document> query = Context.Documents;

            if (include)
            {
                query = query
                    .Include(x => x.Owner)
                    .Include(x => x.StatutDocument)
                    .Include(x => x.DocumentShares);
            }

            return await query.ToListAsync(token).ConfigureAwait(false);
        }

        public async Task<bool> ExistsAsync(Document entity, CancellationToken token = default)
        {
            return await Context.Documents
                .AnyAsync(x => x.Id == entity.Id, token)
                .ConfigureAwait(false);
        }

        public async Task<bool> AddDocumentAsync(Document document, DocumentShare documentShare, CancellationToken token = default)
        {
            await Context.Documents.AddAsync(document, token).ConfigureAwait(false);

            documentShare.Document = document;

            await Context.DocumentShares.AddAsync(documentShare, token).ConfigureAwait(false);

            return await Context.SaveChangesAsync(token).ConfigureAwait(false) > 0;
        }

        public async Task<IEnumerable<Document>> GetMyDocumentsAsync(int ownerId, CancellationToken token = default)
        {
            return await Context.Documents
                .Include(x => x.Owner)
                .Include(x => x.StatutDocument)
                .Where(x => x.OwnerId == ownerId)
                .OrderByDescending(x => x.UploadDate)
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<DocumentShare>> GetSharedWithMeDocumentsAsync(int userId, string email, CancellationToken token = default)
        {
            return await Context.DocumentShares
                .Include(x => x.Document)
                    .ThenInclude(x => x.Owner)
                .Include(x => x.Document)
                    .ThenInclude(x => x.StatutDocument)
                .Where(x => x.SharedWithUserId == userId || x.Email == email)
                .OrderByDescending(x => x.SharedDate)
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<StatutDocument> GetStatutDocumentByCodeAsync(string code, CancellationToken token = default)
        {
            return await Context.StatutDocuments
                .FirstOrDefaultAsync(x => x.Code == code && x.IsActive, token)
                .ConfigureAwait(false);
        }
    }
}
