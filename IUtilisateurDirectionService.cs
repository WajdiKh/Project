using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using BacaratWeb.Entities.Commun;

namespace BacaratWeb.Services.Commun.Interfaces
{
    public interface IUtilisateurDirectionService : IBacaratWebService<UtilisateurDirection>
    {
        Task<UtilisateurDirection> GetAsync(Expression<Func<UtilisateurDirection, bool>> filter, string[] includeParams = null,
                            CancellationToken token = default);

        Task<bool> AnyAsync(Expression<Func<UtilisateurDirection, bool>> filter, CancellationToken cancellationToken = default);

        Task<IList<UtilisateurDirection>> GetListAsync(Expression<Func<UtilisateurDirection, bool>> filter,
            string[] includeParams = null,
            CancellationToken cancellationToken = default);
        Task<IList<UtilisateurDirection>> GetList(Expression<Func<UtilisateurDirection, bool>> filter, string[] includeParams = null,
                            CancellationToken cancellationToken = default);
        Task<IEnumerable<Activite>> GetActivite(int id, CancellationToken token = default);
        Task<IEnumerable<Activite>> GetActivitesByUserId(int userId, CancellationToken token = default);
        Task<IEnumerable<Direction>> GetDirections(string userId, CancellationToken token = default);
        Task<IEnumerable<Utilisateur>> GetMany(int directionId, CancellationToken token = default);
        Task<ICollection<Utilisateur>> GetUtisateurDirections(int directionId, string activiteCode,
                                                              CancellationToken token = default);
        Task<IList<Utilisateur>> GetActiveUtisateurs(List<int> directionIds, List<int> activiteIds, CancellationToken token = default);
        Task<bool> UserHasFraudActivity(int userId, int activiteId, CancellationToken token = default);
        Task<bool> IsSpecialOnDirection(int userId, int directionId, int activiteId, CancellationToken cancellationToken = default);

        Task<bool> IsExistOnDirectionAndActivity(int userId, int directionId, int activiteId,
            CancellationToken cancellationToken = default);
        Task<bool> CanCreateDossier(int userId, int activiteId, CancellationToken token);
        bool CanCreateDossierOnDirection(int userId, int directionId, int activiteId);
        public IQueryable<Direction> GetWriteAuthorizedDirections(int userId, int activiteId);
        bool CanCreateDossierOnDirectionGda(int userId, int directionId, int activiteId, int directionColloaborateurEscaladeId);
        bool CanViewDossierOnDirection(int userId, int directionId, int activiteId);
        bool CanViewDossier(int userId, int activiteId);
        bool CanCreateDossier(int userId, int activiteId);
        bool IsIsoleOnlyUser(int activiteId, int utilisateurId);
        Task<bool> IsEtendu(int userId, CancellationToken cancellationToken);
        Task<bool> IsIsole(int userId, int directionId, int activiteId, CancellationToken cancellationToken = default);
        Task<bool> IsPendingInActivity(int userId, int directionId, int activiteId, CancellationToken cancellationToken = default);
        Task<bool> IsConfidentialInActivity(int userId, int directionId, int activiteId, CancellationToken cancellationToken = default);

        bool IsIsolatedOnDirection(int userId, int directionId, int activiteId);
        bool IsPendingOnDirection(int userId, int directionId, int activiteId);
        bool IsConfidentialOnDirection(int userId, int directionId, int activiteId);
        bool IsEtendu(int userId);
        bool IsConfidential(int userId);
        bool IsEtenduOnDirection(int userId, int directionId, int activiteId);
        bool IsADMINREFERENTIELOnDirection(int userId, int directionId, int activiteId);
        bool IsVALIDATIONOnDirection(int userId, int directionId, int activiteId);
        bool IsCONTACTPRINCIPALOnDirection(int userId, int directionId, int activiteId);
        bool IsSANCTIONOnDirection(int userId, int directionId, int activiteId);

        void RevoquerUtilisateurBydirection(int userId, int directionId, int activiteId, int currentUserId);
        void ValiderUtilisateurBydirection(int userId, int directionId, int activiteId, int currentUserId);
        Task<IEnumerable<Direction>> GetDirectionFlges(int activiteId, int userId = default, CancellationToken token = default);
        Task<bool> UserHasLabActivity(int userId, CancellationToken token = default);
        Task<IEnumerable<Direction>> GetDirectionAccessibleByPays(int paysId, CancellationToken cancellationToken = default);
        Task<bool> IsIsValideurInActivity(int userId, int directionId, int activiteId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Direction>> GetDirectionValideurs(int userId, int activiteId, CancellationToken cancellationToken = default);
        Task<bool> IsValidateurLab(int userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Utilisateur>> GetUtilisateurPendingConfident(int directionId, int activiteId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Utilisateur>> GetUtilisateurHabiliteConfident(int directionId, int activiteId, CancellationToken cancellationToken = default);

        Task<bool> IsUe(int userId, int activiteId, CancellationToken cancellationToken = default);

        Task<bool> IsPendingInAllDirectionAsync(int utilisateursId, int activiteId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Utilisateur>> GetUtilisateurPending(int userId, int activiteId, CancellationToken cancellationToken = default);
        bool IsExistUserByMailActivite(string email, int activiteId, int direction);
        bool IsExistUtilisateurDirectionActive(int utilisateurId);
    }
}