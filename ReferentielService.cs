using BacaratWeb.Entities.Transfert;

public async Task<IEnumerable<StatutDocument>> GetStatutDocuments(CancellationToken token = default)
{
    return await Context.StatutDocuments
        .Where(x => x.IsActive)
        .ToListAsync(token)
        .ConfigureAwait(false);
}