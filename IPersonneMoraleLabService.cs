
using BacaratWeb.Entities.Lab;
using BacaratWeb.Services.Commun.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace BacaratWeb.Services.Lab.Interfaces
{
    public interface IPersonneMoraleLabService : IBacaratWebService<PersonneMoraleLab>
    {
        Task<PersonneMoraleLab> GetAsync(Guid idPersonne, bool include = false, CancellationToken token = default);
    }
}
