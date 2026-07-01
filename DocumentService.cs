using BacaratWeb.Entities.Transfert;
using BacaratWeb.Services.Commun;
using BacaratWeb.Services.Transfert.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using BacaratWeb.Model.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace BacaratWeb.Services.Transfert
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

        public async Task<bool> AddDocumentAsync(Document document, CancellationToken token = default)
        {
            await Context.Documents.AddAsync(document, token);

            return await Context.SaveChangesAsync(token) > 0;
        }

        public async Task<IEnumerable<Document>> GetMyDocumentsAsync(int ownerId, CancellationToken token = default)
        {
            return await Context.Documents
                .Include(x => x.Owner)
                .Include(x => x.StatutDocument)
                .Where(x => x.OwnerId == ownerId)
                .OrderByDescending(x => x.UploadDate)
                .ToListAsync(token);
        }

        public async Task<List<DocumentShare>> GetSharedWithMeDocumentsAsync(
            int userId,
            string email,
            CancellationToken token = default)
        {
            return await Context.DocumentShares
                .Include(x => x.Document)
                    .ThenInclude(x => x.Owner)
                .Include(x => x.Document)
                    .ThenInclude(x => x.StatutDocument)
                .Where(x =>
                    x.IsActive
                    && (
                        x.SharedWithUserId == userId
                        || x.Email == email
                    ))
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

        public async Task<Document> GetDocumentByIdAndOwnerAsync(int documentId, int ownerId, CancellationToken token = default)
        {
            return await Context.Documents
                .Include(x => x.DocumentShares)
                .FirstOrDefaultAsync(x => x.Id == documentId && x.OwnerId == ownerId, token);
        }

        public async Task<bool> ShareDocumentAsync(int documentId, IEnumerable<DocumentShare> shares, CancellationToken token = default)
        {
            var existingShares = Context.DocumentShares.Where(x => x.DocumentId == documentId);

            Context.DocumentShares.RemoveRange(existingShares);

            await Context.DocumentShares.AddRangeAsync(shares, token);

            return await Context.SaveChangesAsync(token) > 0;
        }

        public async Task<IEnumerable<DocumentShare>> GetDocumentSharesAsync(int documentId, CancellationToken token = default)
        {
            return await Context.DocumentShares
                .Include(x => x.Createur)
                .Include(x => x.SharedWithUser)
                .Where(x => x.DocumentId == documentId)
                .OrderByDescending(x => x.SharedDate)
                .ToListAsync(token);
        }

        public async Task<DocumentShare> GetDocumentShareAsync(int documentShareId, CancellationToken token = default)
        {
            return await Context.DocumentShares
                .Include(x => x.Document)
                    .ThenInclude(x => x.Owner)
                .Include(x => x.Createur)
                .Include(x => x.SharedWithUser)
                .FirstOrDefaultAsync(x => x.Id == documentShareId, token);
        }

        public async Task<bool> AddDocumentSharesAsync(IEnumerable<DocumentShare> shares, CancellationToken token = default)
        {
            var sharesToAdd = shares.ToList();

            if (!sharesToAdd.Any())
                return false;

            await Context.DocumentShares.AddRangeAsync(sharesToAdd, token);

            return await Context.SaveChangesAsync(token) > 0;
        }

        public async Task<bool> ReplaceDocumentShareAsync(int documentShareId, IEnumerable<DocumentShare> replacementShares, CancellationToken token = default)
        {
            var existingShare = await Context.DocumentShares
                .FirstOrDefaultAsync(x => x.Id == documentShareId, token);

            if (existingShare == null)
                return false;

            Context.DocumentShares.Remove(existingShare);

            var sharesToAdd = replacementShares.ToList();

            if (sharesToAdd.Any())
                await Context.DocumentShares.AddRangeAsync(sharesToAdd, token);

            return await Context.SaveChangesAsync(token) > 0;
        }

        public async Task<bool> DeleteDocumentShareAsync(int documentShareId, CancellationToken token = default)
        {
            var share = await Context.DocumentShares
                .FirstOrDefaultAsync(x => x.Id == documentShareId, token);

            if (share == null)
                return false;

            Context.DocumentShares.Remove(share);

            return await Context.SaveChangesAsync(token) > 0;
        }

        public async Task<List<Document>> GetAllDocumentsAsync(
            int userId,
            string email,
            CancellationToken token = default)
        {
            return await Context.Documents
                .Include(x => x.Owner)
                .Include(x => x.StatutDocument)
                .Include(x => x.DocumentShares)
                .Where(x =>
                    x.OwnerId == userId
                    || x.DocumentShares.Any(s =>
                        s.IsActive
                        && (
                            s.SharedWithUserId == userId
                            || s.Email == email
                        )))
                .Distinct()
                .OrderByDescending(x => x.UploadDate)
                .ToListAsync(token);
        }

        public async Task<Document> GetDocumentForDownloadAsync(int documentId, CancellationToken token = default)
        {
            return await Context.Documents
                .Include(x => x.Owner)
                .Include(x => x.DocumentShares)
                .FirstOrDefaultAsync(x => x.Id == documentId, token);
        }

        public async Task<bool> CanUserDownloadSharedDocumentAsync(
            int documentId,
            int userId,
            string email,
            CancellationToken token = default)
        {
            var now = DateTimeOffset.Now;

            return await Context.DocumentShares
                .Include(x => x.Document)
                .AnyAsync(x =>
                    x.DocumentId == documentId
                    && x.IsActive
                    && (
                        x.SharedWithUserId == userId
                        || x.Email == email
                    )
                    && x.SharedDate <= now
                    && x.ExpiryDate >= now
                    && x.Document.ExpiryDate >= now,
                    token);
        }
        public async Task<bool> DeleteDocumentAsync(int documentId, CancellationToken token = default)
        {
            var document = await Context.Documents
                .Include(x => x.DocumentShares)
                .FirstOrDefaultAsync(x => x.Id == documentId, token);

            if (document == null)
                return false;

            if (document.DocumentShares != null && document.DocumentShares.Any())
                Context.DocumentShares.RemoveRange(document.DocumentShares);

            Context.Documents.Remove(document);

            return await Context.SaveChangesAsync(token) > 0;
        }

        public async Task<bool> AddDocumentWithSharesAsync(
            Document document,
            IEnumerable<DocumentShare> shares,
            CancellationToken token = default)
        {
            var sharesToAdd = shares?.ToList() ?? new List<DocumentShare>();

            if (!sharesToAdd.Any())
                return false;

            await Context.Documents.AddAsync(document, token);

            foreach (var share in sharesToAdd)
            {
                share.Document = document;
                share.IsActive = true;
            }

            await Context.DocumentShares.AddRangeAsync(sharesToAdd, token);

            return await Context.SaveChangesAsync(token) > 0;
        }

        public async Task<DocumentShare> GetCurrentUserActiveDocumentShareAsync(
            int documentId,
            int userId,
            string email,
            CancellationToken token = default)
        {
            var now = DateTimeOffset.Now;

            return await Context.DocumentShares
                .Include(x => x.Document)
                    .ThenInclude(x => x.Owner)
                .FirstOrDefaultAsync(x =>
                    x.DocumentId == documentId
                    && x.IsActive
                    && x.SharedDate <= now
                    && x.ExpiryDate >= now
                    && x.Document.ExpiryDate >= now
                    && (
                        x.SharedWithUserId == userId
                        || x.Email == email
                    ),
                    token);
        }

        public async Task<bool> DisableDocumentShareAsync(
            int documentShareId,
            CancellationToken token = default)
        {
            var share = await Context.DocumentShares
                .FirstOrDefaultAsync(x => x.Id == documentShareId, token);

            if (share == null)
                return false;

            share.IsActive = false;

            return await Context.SaveChangesAsync(token) > 0;
        }

        public async Task<bool> UpdateDocumentShareLastDownloadDateAsync(
            int documentShareId,
            DateTimeOffset lastDownloadDate,
            CancellationToken token = default)
        {
            var share = await Context.DocumentShares
                .FirstOrDefaultAsync(x => x.Id == documentShareId, token);

            if (share == null)
                return false;

            share.LastDownloadDate = lastDownloadDate;

            return await Context.SaveChangesAsync(token) > 0;
        }
    }
}
