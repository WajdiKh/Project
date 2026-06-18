using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BacaratWeb.Entities.Lab;
using BacaratWeb.Model.Entities;
using BacaratWeb.Services.Commun;
using BacaratWeb.Services.Lab.Interfaces;
using System;

namespace BacaratWeb.Services.Lab
{
    public class PersonneMoraleLabService : BacaratWebService<PersonneMoraleLab>, IPersonneMoraleLabService
    {
        public PersonneMoraleLabService(BacaratWebContext context) : base(context)
        {
        }

        public async Task<bool> AddAsync(PersonneMoraleLab entity, CancellationToken token = default)
        {
            await Context.PersonneMoraleLabs.AddAsync(entity, token);
            return await SaveAsync(token).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(PersonneMoraleLab entity, CancellationToken token = default)
        {
            Context.PersonneMoraleLabs.Remove(entity);
            return await SaveAsync(token).ConfigureAwait(false);
        }


        public async Task<bool> ExistsAsync(PersonneMoraleLab entity, CancellationToken token = default) => await Context
                .PersonneMoraleLabs
            .AnyAsync(t => t.Id == entity.Id, token)
            .ConfigureAwait(false);

        public async Task<PersonneMoraleLab> GetAsync(int entityId,
                                                      bool include = false,
                                                      CancellationToken token = default) => await Context
                .PersonneMoraleLabs
            .Where(t => t.Id == entityId)
            .FirstOrDefaultAsync(token)
            .ConfigureAwait(false);

        public async Task<PersonneMoraleLab> GetAsync(
            Guid idPersonne,
            bool include = false,
            CancellationToken token = default)
        {
            if (!include)
                return await Context
                 .PersonneMoraleLabs
                .Where(t => t.IdPersonne == idPersonne)
                .FirstOrDefaultAsync(token)
                .ConfigureAwait(false);

            return await Context
                 .PersonneMoraleLabs
                 .Include(p => p.SupportFinancierPersonneMorales)
                .Where(t => t.IdPersonne == idPersonne)
                .FirstOrDefaultAsync(token)
                .ConfigureAwait(false);
        }
        public async Task<IEnumerable<PersonneMoraleLab>> GetManyAsync(bool include = false,
                                                                       CancellationToken token = default) => await Context.PersonneMoraleLabs
            .ToListAsync(token)
            .ConfigureAwait(false);

        public async Task<bool> UpdateAsync(PersonneMoraleLab entity, CancellationToken token = default)
        {
            UpdateCore(entity);
            return await SaveAsync(token).ConfigureAwait(false);
        }

        public async Task<bool> ExistsAsync(DossierLabPersonneMorale entity, CancellationToken token = default) => await Context.PersonneMoraleLabs
            .AnyAsync(x => x.Id == entity.Id, token)
            .ConfigureAwait(false);
    }
}
