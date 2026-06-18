using BacaratWeb.Entities.Lab;
using BacaratWeb.Services.Commun.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace BacaratWeb.Services.Lab.Interfaces
{
    public interface IPersonnePhysiqueLabService : IBacaratWebService<PersonnePhysiqueLab>
    {
        Task<PersonnePhysiqueLab> GetAsync(Guid idPersonne, bool include = false, CancellationToken token = default);

        Task<int> GetPersonnePhysiqueId(string nom, string prenoms, DateTimeOffset dateNaissance,
            bool include = false,
            CancellationToken token = default);
    }
}
