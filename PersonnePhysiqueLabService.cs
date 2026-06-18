using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BacaratWeb.Entities.Lab;
using BacaratWeb.Model.Entities;
using BacaratWeb.Services.Commun;
using BacaratWeb.Services.Lab.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BacaratWeb.Services.Lab
{
    public class PersonnePhysiqueLabService : BacaratWebService<PersonnePhysiqueLab>, IPersonnePhysiqueLabService
    {
        public PersonnePhysiqueLabService(BacaratWebContext context) : base(context)
        {
        }

        public async Task<bool> AddAsync(PersonnePhysiqueLab entity, CancellationToken token = default)
        {
            await Context.PersonnePhysiqueLabs.AddAsync(entity, token);
            return await SaveAsync(token).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(PersonnePhysiqueLab entity, CancellationToken token = default)
        {
            Context.PersonnePhysiqueLabs.Remove(entity);
            return await SaveAsync(token).ConfigureAwait(false);
        }


        public async Task<bool> ExistsAsync(PersonnePhysiqueLab entity, CancellationToken token = default)
        {
            return await Context
                .PersonnePhysiqueLabs
                .AnyAsync(t => t.Id == entity.Id, token)
                .ConfigureAwait(false);
        }

        public async Task<PersonnePhysiqueLab> GetAsync(int entityId,
            bool include = false,
            CancellationToken token = default)
        {
            return await Context
                .PersonnePhysiqueLabs
                .Where(t => t.Id == entityId)
                .FirstOrDefaultAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<PersonnePhysiqueLab> GetAsync(Guid idPersonne,
            bool include = false,
            CancellationToken token = default)
        {
            if (!include)
                return await Context
                    .PersonnePhysiqueLabs
                    .Where(t => t.IdPersonne == idPersonne)
                    .FirstOrDefaultAsync(token)
                    .ConfigureAwait(false);
            
            return await Context
                    .PersonnePhysiqueLabs
                    .Include(p => p.SupportFinancierPersonnePhysiques)
                    .Where(t => t.IdPersonne == idPersonne)
                    .FirstOrDefaultAsync(token)
                    .ConfigureAwait(false);
        }

        public async Task<int> GetPersonnePhysiqueId(string nom, string prenoms, DateTimeOffset dateNaissance,
            bool include = false,
            CancellationToken token = default)
        {
            var personnePhysique =  await Context
                .PersonnePhysiqueLabs
                .Include(p => p.SupportFinancierPersonnePhysiques)
                .Where(t => t.NomNaissance == nom && t.Prenoms == prenoms && t.DateNaissance == dateNaissance)
                .FirstOrDefaultAsync(token)
                .ConfigureAwait(false);

            if(personnePhysique != null)
                return personnePhysique.Id;
            return 0;
        }

        public async Task<IEnumerable<PersonnePhysiqueLab>> GetManyAsync(bool include = false,
            CancellationToken token = default)
        {
            return await Context.PersonnePhysiqueLabs
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<bool> UpdateAsync(PersonnePhysiqueLab entity, CancellationToken token = default)
        {
            UpdateCore(entity);
            return await SaveAsync(token).ConfigureAwait(false);
        }

        public async Task<bool> ExistsAsync(DossierLabPersonnePhysique entity, CancellationToken token = default)
        {
            return await Context.PersonnePhysiqueLabs
                .AnyAsync(x => x.Id == entity.Id, token)
                .ConfigureAwait(false);
        }
    }
}
