using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BacaratWeb.Core.Extensions;
using BacaratWeb.Core.Helper;
using BacaratWeb.Entities.Commun;
using BacaratWeb.Entities.Contracts;
using BacaratWeb.Entities.Escalade;
using BacaratWeb.Entities.Extensions;
using BacaratWeb.Entities.Gda;
using BacaratWeb.Model.Entities;
using BacaratWeb.Model.Helpers;
using BacaratWeb.Models.Referentials;
using BacaratWeb.Services.Commun;
using BacaratWeb.Services.Commun.Interfaces;
using BacaratWeb.Services.Gda.Interfaces;
using BacaratWeb.Services.SearchPredicates;
using BacaratWeb.Shared;
using BacaratWeb.Shared.SearchCriterias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace BacaratWeb.Services.Gda
{
    public class DossierGdaService : BacaratWebService<DossierGda>, IDossierGdaService
    {
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly IEmailNotificationTemplateService _emailNotificationTemplateService;
        private readonly IReferentielService _referentielService;
        private readonly ILogger<DossierGdaService> _logger;

        public DossierGdaService(BacaratWebContext context,
            IEmailNotificationService emailNotificationService,
            IEmailNotificationTemplateService emailNotificationTemplateService,
            IReferentielService referentielService,
            ILogger<DossierGdaService> logger) : base(context)
        {
            _emailNotificationService = emailNotificationService;
            _emailNotificationTemplateService = emailNotificationTemplateService;
            _referentielService = referentielService;
            _logger = logger;
        }

        public static int NbLignes { get; set; }

        public async Task<bool> AddAsync(DossierGda entity, CancellationToken token = default)
        {
            await using var transaction = await Context.Database.BeginTransactionAsync(token).ConfigureAwait(false);
            try
            {
                entity?.TitulairesComptes.ForEach(tc =>
                {
                    if (tc.TitulaireCompte.PersonnePhysique != null)
                        tc.TitulaireCompte = tc.TitulaireCompte.PersonnePhysique;

                    if (tc.TitulaireCompte.PersonneMorale != null)
                        tc.TitulaireCompte = tc.TitulaireCompte.PersonneMorale;
                });

                if (entity != null)
                {
                    await Context.DossierGdas.AddAsync(entity, token);

                    await UpdateDossierGdaOscsAsync(entity, token);

                    var newEntriesToNotify = GetEntriesToNotifyStateChange();
                    
                    await SaveAsync(token).ConfigureAwait(false);
                    
                    await transaction.CommitAsync(token).ConfigureAwait(false);
                    
                    await PushEntitiesStateChangeNotifications(entity, newEntriesToNotify);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
                await transaction.RollbackAsync(token).ConfigureAwait(false);
                return false;
            }

            return true;
        }

        public async Task UpdateDossierGdaOscsAsync(DossierGda entity, CancellationToken token)
        {
            if (entity.OscChanges == null)
                return;
            
            foreach (var ins in entity.OscChanges.inserted)
            {
                ins.DossierGda = entity;

                await Context
                    .DossierGdaOscs
                    .AddAsync(ins, token);
            }

            foreach (var upd in entity.OscChanges.updated)
            {
                Context
                    .DossierGdaOscs
                    .Update(upd);
            }

            foreach (var rem in entity.OscChanges.removed)
            {
                var victim = Context
                    .DossierGdaOscs
                    .FirstOrDefault(x => x.Id == rem.Id);

                if (victim != null)
                    Context
                        .DossierGdaOscs
                        .Remove(victim);
            }
        }

        public async Task<bool> DeleteAsync(DossierGda entity, CancellationToken token = default)
        {
            try
            {
                var dossierHistos = await Context.DossierGdaHistos
                    .Where(e => e.DossierGdaId == entity.Id)
                    .ToListAsync(token)
                    .ConfigureAwait(false);

                dossierHistos.ForEach(dh =>
                {
                    dh.DossierGdaId = null;
                    Context.DossierGdaHistos.Update(dh);
                    Context.SaveChanges();
                });

                var eventDossierGdas = await Context.EventDossiers
                    .Where(e => e.DossierGdaId == entity.Id)
                    .ToListAsync(token)
                    .ConfigureAwait(false);

                eventDossierGdas?.ForEach(d =>
                {
                    Context.EventDossiers.Remove(d);
                    Context.SaveChanges();
                });


                var demandeInformations = await Context.DemandeInformationGdas
                    .Include(x => x.DocumentDemandeInformationGdaRequests)
                    .Include(x => x.DocumentDemandeInformationGdaResponses)
                    .Where(e => e.DossierGdaId == entity.Id)
                    .ToListAsync(token)
                    .ConfigureAwait(false);

                foreach (var dmf in demandeInformations)
                {
                    foreach (var drqs in dmf.DocumentDemandeInformationGdaRequests)
                        Context.DocumentDemandeInformationGdas.Remove(drqs);

                    foreach (var drps in dmf.DocumentDemandeInformationGdaResponses)
                        Context.DocumentDemandeInformationGdas.Remove(drps);

                    Context.DemandeInformationGdas.Remove(dmf);
                    await Context.SaveChangesAsync(token);
                }


                foreach (var pp in entity.DossierGdaPersonnePhysiques)
                {
                    pp.DossierGda = null;
                    pp.PersonnePhysiqueGda = null;

                    Context.DossierGdaPersonnePhysiques.Remove(pp);
                }

                foreach (var pm in entity.DossierGdaPersonneMorales)
                {
                    pm.DossierGda = null;
                    pm.PersonneMoraleGda = null;

                    Context.DossierGdaPersonneMorales.Remove(pm);
                }

                foreach (var ac in entity.DossierGdaImmediateActions)
                {
                    ac.Modificateur = null;
                    ac.Createur = null;
                    ac.DossierGda = null;
                    Context.DossierGdaImmediateActions.Remove(ac);
                }

                foreach (var ac in entity.DossierGdaCurrentActions)
                {
                    ac.Modificateur = null;
                    ac.Createur = null;
                    ac.DossierGda = null;
                    Context.DossierGdaCurrentActions.Remove(ac);
                }

                foreach (var ac in entity.DossierGdaOscs)
                {
                    ac.Modificateur = null;
                    ac.Createur = null;
                    ac.DossierGda = null;
                    Context.DossierGdaOscs.Remove(ac);
                }

                entity.TitulairesComptes?.ForEach(etcc =>
                {
                    if (etcc.DossierGdaId == entity.Id) Context.DossierGdaTransactionnelClients.Remove(etcc);
                });


                Context.DossierDegelPartielGdas.RemoveRange(Context.DossierDegelPartielGdas
                    .Where(e => e.DossierGdaId == entity.Id).ToList());
                Context.DossierGdas.Remove(entity);
                await SaveAsync(token).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
                return false;
            }

            return true;
        }


        public async Task<bool> DeleteRangeAsync(IEnumerable<DossierGda> entities, CancellationToken token)
        {
            await using var transaction = await Context.Database.BeginTransactionAsync(token).ConfigureAwait(false);
            try
            {
                var dossierGdas = entities.ToList();
                foreach (var entity in dossierGdas)
                {
                    entity.DeleteRelatedPersonnesMorales(Context);
                    entity.DeleteRelatedPersonnesPhysiques(Context);
                    entity.DeleteRelatedDucuments(Context);
                    entity.DeleteRelatedImmediateActions(Context);
                    entity.DeleteRelatedCurrentActions(Context);
                    entity.DeleteRelatedOscs(Context);
                }


                Context.DossierGdas.RemoveRange(dossierGdas);

                await SaveAsync(token).ConfigureAwait(false);

                await transaction.CommitAsync(token).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
                await transaction.RollbackAsync(token).ConfigureAwait(false);
                return false;
            }

            return true;
        }

        public async Task<bool>
            ExistsAsync(DossierGda entity, CancellationToken token = default)
        {
            return await Context
                .DossierGdas
                .AnyAsync(t => t.Id == entity.Id, token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<DossierGdaRow>> FindDossiers(GdaSearchCriteria searchCriteria,
            Ref<int> nbLignes,
            CancellationToken token = default)
        {
            IEnumerable<DossierGdaRow> result;

            try
            {
                var predicate = new GdaPredicateBuilderService().BuildExpression(searchCriteria);

                var allGdaCasesSatisfyingPredicate = Context.DossierGdas
                    .Include(d => d.StatutDossierGda)
                    .Include(d => d.RegimeSanctionGda)
                    .Include(d => d.DossierGdaPersonnePhysiques)
                    .Include(d => d.DocumentsDossierGda)
                    .ThenInclude(d => d.AppartenanceDocument)
                    .Where(predicate)
                    .OrderByDescending(x => x.Id)
                    .Take(2000);

                if (searchCriteria.IsSearchByNomRaisonSociale
                    || searchCriteria.IsSearchByPrenom
                    || searchCriteria.IsSearchByDateNaissance)
                {
                    if (searchCriteria.IsSearchByMotifsSoupson)
                    {
                        result = await allGdaCasesSatisfyingPredicate
                            .Select(DossierGdaExpression.DossierGdaWithMotifWithPersonTiers())
                            .Select(x => DossierGdaRow.ToExtendedResult(x, searchCriteria))
                            .Where(x => x.MotifsSoupcons.Contains(searchCriteria.MotifsSoupcons))
                            .ToListAsync(token).ConfigureAwait(false);
                        nbLignes.Value = await Context.DossierGdas.Where(predicate).CountAsync(token)
                            .ConfigureAwait(false);
                    }
                    else
                    {
                        result = await allGdaCasesSatisfyingPredicate
                            .Select(DossierGdaExpression.DossierGdaNoMotifWithPersonTiers())
                            .Select(x => DossierGdaRow.ToExtendedResult(x, searchCriteria))
                            .ToListAsync(token).ConfigureAwait(false);
                        nbLignes.Value = await Context.DossierGdas.Where(predicate).CountAsync(token)
                            .ConfigureAwait(false);
                    }
                }
                else
                {
                    if (searchCriteria.IsSearchByMotifsSoupson)
                    {
                        result = await allGdaCasesSatisfyingPredicate
                            .Select(DossierGdaExpression.DossierGdaWithMotifNoPersonTiers())
                            .Select(x => DossierGdaRow.ToExtendedResult(x, searchCriteria))
                            .Where(x => x.MotifsSoupcons.Contains(searchCriteria.MotifsSoupcons))
                            .ToListAsync(token).ConfigureAwait(false);
                        nbLignes.Value = await Context.DossierGdas.Where(predicate).CountAsync(token)
                            .ConfigureAwait(false);
                    }
                    else
                    {
                        result = await allGdaCasesSatisfyingPredicate
                            .Select(DossierGdaExpression.DossierGdaNoMotifNoPersonTiers())
                            .Select(x => DossierGdaRow.ToExtendedResult(x, searchCriteria))
                            .ToListAsync(token).ConfigureAwait(false);
                        nbLignes.Value = await Context.DossierGdas.Where(predicate).CountAsync(token)
                            .ConfigureAwait(false);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                result = Enumerable.Empty<DossierGdaRow>();
            }
            catch (AggregateException)
            {
                result = Enumerable.Empty<DossierGdaRow>();
            }

            return result;
        }

        public Task<IEnumerable<DossierGdaPersonneLieeItem>> FindReportinTierDossiers(
            GdaSearchCriteria searchCriteria,
            CancellationToken token = default)
        {
            IEnumerable<DossierGdaPersonneLieeItem> results;
            try
            {
                var predicatePersonneGeleeMoraleReporting =
                    new GdaPersonneGeleeMoralePredicateBuilderService().BuildExpression(searchCriteria);
                var predicatePersonneGeleePhysiqueReporting =
                    new GdaPersonneGeleePhysiquePredicateBuilderService().BuildExpression(searchCriteria);

                var predicatePersonneLieeMoraleReporting =
                    new GdaPersonneLieeMoralePredicateBuilderService().BuildExpression(searchCriteria);

                var predicatePersonneLieePhysiqueReporting =
                    new GdaPersonneLieePhysiquePredicateBuilderService().BuildExpression(searchCriteria);

                //personne liée morales.
                var typologieavoirspersonnelieemorale = Context.TypologieAvoirPersonneLieePersonneMorales
                    .Include(x => x.Devise)
                    .Include(x => x.Pays)
                    .Include(x => x.TypesActifsGdas)
                   .Include("PersonneMoraleGda.DossierGdaPersonneMorales.DossierGda.Direction")
                    .Include("PersonneMoraleGda.DossierGdaPersonneMorales.DossierGda.RegimeSanctionGda")
                    .Include("PersonneMoraleGda.DossierGdaPersonneMorales.DossierGda.StatutDossierGda")
                    .Include("PersonneMoraleGda.LienPersonneMoraleGdas.TypesLiens")
                    .Include("PersonneMoraleGda.LienPersonneMoralePpGdas.TypesLiens")
                    .Where(x => x.IsToFreeze == true).Where(predicatePersonneLieeMoraleReporting)
                    .ToList()
                    .Select(x => DossierGdaPersonneLieeItem.ToExtendPersonneLieeMoraleResult(x, searchCriteria))
                    .Where(x => x.StatutDossierId == (int)StatutDossierGdaEnum.InEffect && !x.IsRelationCommercialeCloturee)
                    .OrderByDescending(x => x.Id).ToList();
                var result = typologieavoirspersonnelieemorale;

                var geleeSansLien = typologieavoirspersonnelieemorale.Select(x => x.PersonneMoraleGdaId).ToList();

                //Typo avoir personnes gelee morales.
                var typologieavoirspersonnegeleemorale = Context.TypologieAvoirPersonneGeleePersonneMorales
                    .Include(x => x.Devise)
                    .Include(x => x.Pays)
                    .Include(x => x.TypesActifsGdas)
                    .Include(y => y.PersonneMoraleGda)
                    .Include("PersonneMoraleGda.DossierGdaPersonneMorales.DossierGda.Direction")
                    .Include("PersonneMoraleGda.DossierGdaPersonneMorales.DossierGda.RegimeSanctionGda")
                    .Include("PersonneMoraleGda.DossierGdaPersonneMorales.DossierGda.StatutDossierGda")
                    .Include("PersonneMoraleGda.LienPersonneMoraleGdas.TypesLiens")
                    .Where(x => !geleeSansLien.Contains(x.PersonneMoraleGdaId))
                    .Where(predicatePersonneGeleeMoraleReporting)
                    .ToList()
                    .Select(x => DossierGdaPersonneLieeItem.ToExtendPersonneGeleeMoraleResult(x, searchCriteria))
                    .Where(x => x.StatutDossierId == (int)StatutDossierGdaEnum.InEffect && !x.IsRelationCommercialeCloturee)
                    .OrderByDescending(x => x.Id)
                    .ToList();
                result.AddRange(typologieavoirspersonnegeleemorale);

                //personne liée physique.
                var typologieavoirspersonnelieephysique = Context.TypologieAvoirPersonneLieePersonnePhysiques
                    .Include(x => x.Devise)
                    .Include(x => x.Pays)
                    .Include(x => x.TypesActifsGdas)
                    .Include("PersonnePhysiqueGda.DossierGdaPersonnePhysiques.DossierGda.Direction")
                    .Include("PersonnePhysiqueGda.DossierGdaPersonnePhysiques.DossierGda.RegimeSanctionGda")
                    .Include("PersonnePhysiqueGda.DossierGdaPersonnePhysiques.DossierGda.StatutDossierGda")
                    .Include("PersonnePhysiqueGda.LienPersonnePhysiqueGdas.TypesLiens")
                    .Include("PersonnePhysiqueGda.LienPersonnePhysiquePmGdas.TypesLiens")
                    .Where(x => x.IsToFreeze == true).Where(predicatePersonneLieePhysiqueReporting)
                    .ToList()
                    .Select(x => DossierGdaPersonneLieeItem.ToExtendPersonneLieePhysiqueResult(x, searchCriteria))
                    .Where(x => x.StatutDossierId == (int)StatutDossierGdaEnum.InEffect && !x.IsRelationCommercialeCloturee)
                    .OrderByDescending(x => x.Id)
                    .ToList();

                result.AddRange(typologieavoirspersonnelieephysique);

                var geleePhysiqueSansLien =
                    typologieavoirspersonnelieephysique.Select(x => x.PersonneMoraleGdaId).ToList();

                //Typo avoir personnes gelee physique.
                var typologieavoirspersonnegeleephysique = Context.TypologieAvoirPersonneGeleePersonnePhysiques
                    .Include(x => x.Devise)
                    .Include(x => x.Pays)
                    .Include(x => x.TypesActifsGdas)
                    .Include("PersonnePhysiqueGda.DossierGdaPersonnePhysiques.DossierGda.Direction")
                    .Include("PersonnePhysiqueGda.DossierGdaPersonnePhysiques.DossierGda.RegimeSanctionGda")
                    .Include("PersonnePhysiqueGda.DossierGdaPersonnePhysiques.DossierGda.StatutDossierGda")
                    .Where(x => !geleePhysiqueSansLien.Contains(x.PersonnePhysiqueGdaId))
                    .Where(predicatePersonneGeleePhysiqueReporting)
                    .ToList()
                    .Select(x => DossierGdaPersonneLieeItem.ToExtendPersonneGeleePhysiqueResult(x, searchCriteria))
                    .OrderByDescending(x => x.Id)
                    .Where(x => x.StatutDossierId == (int)StatutDossierGdaEnum.InEffect && !x.IsRelationCommercialeCloturee)
                    .ToList();

                result.AddRange(typologieavoirspersonnegeleephysique);


                return Task.FromResult<IEnumerable<DossierGdaPersonneLieeItem>>(result);
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
                results = Enumerable.Empty<DossierGdaPersonneLieeItem>();
            }
            catch (AggregateException e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
                results = Enumerable.Empty<DossierGdaPersonneLieeItem>();
            }


            return Task.FromResult(results);
        }


        public Task<IEnumerable<DossierGdaResult>> FindReportinTierTransactionnelDossiers(
         GdaSearchCriteria searchCriteria,
         CancellationToken token = default)
        {
            IEnumerable<DossierGdaResult> result;
            try
            {
                var predicateReporting = new GdaPredicateBuilderService().BuildExpression(searchCriteria);
                var sql = Context.DossierGdas.Where(predicateReporting).ToQueryString();
                return Task.FromResult(Context.DossierGdas
                    .Include(x => x.StatutDossierGda)
                    .Include(x => x.Direction)
                    .Include(x => x.Utilisateur)
                    .Include(x => x.Modificateur)
                    .Include(x => x.CategorieGdas)
                    .Include(x => x.RegimeSanctionGda)
                    .Include(x => x.TypologieGda)
                    .Include(x => x.Pays)
                    .Include(x => x.DeviseTransaction)
                    .Include(x => x.DossierGdaPersonnePhysiques)
                    .ThenInclude(y => y.PersonnePhysiqueGda)
                    .Include(x => x.DossierGdaPersonneMorales)
                    .ThenInclude(y => y.PersonneMoraleGda)
                    .Include("TitulairesComptes.TitulaireCompte.PersonnePhysique")
                    .Include("TitulairesComptes.TitulaireCompte.PersonneMorale")
                    .Include(x => x.DossierDegelPartielGdas)
                    .Where(predicateReporting)
                    .Where(x => x.CategorieId == 2 && x.TypeDegelId != 1 && x.StatutDossierId == (int)StatutDossierGdaEnum.InEffect)
                    .Select(x => DossierGdaResult.ToExtendedResult(x, searchCriteria))
                    .ToList().Where(x => x.MontantFonds != 0 && x.MontantFonds != null));
            }
            catch (OperationCanceledException)
            {
                result = Enumerable.Empty<DossierGdaResult>();
            }
            catch (AggregateException)
            {
                result = Enumerable.Empty<DossierGdaResult>();
            }

            return Task.FromResult(result);
        }

        public Task<IEnumerable<DossierGdaResult>> FindReportingDossiers(GdaSearchCriteria searchCriteria,
            CancellationToken token = default)
        {
            IEnumerable<DossierGdaResult> result;
            try
            {
                var predicateReporting = new GdaPredicateBuilderService().BuildExpression(searchCriteria);
                return Task.FromResult<IEnumerable<DossierGdaResult>>(Context.DossierGdas
                    .Include(x => x.StatutDossierGda)
                    .Include(x => x.Direction)
                    .Include(x => x.Utilisateur)
                    .Include(x => x.Modificateur)
                    .Include(x => x.CategorieGdas)
                    .Include(x => x.RegimeSanctionGda)
                    .Include(x => x.TypologieGda)
                    .Include(x => x.Pays)
                    .Where(predicateReporting)
                    .Select(DossierGdaExpression.DossierGdaReportingExpression())
                    .Select(x => DossierGdaResult.ToExtendedResult(x, searchCriteria)).AsParallel()
                    .OrderByDescending(x => x.Id)
                    .Where(x => string.IsNullOrEmpty(searchCriteria.MotifsSoupcons) ||
                                x.MotifsSoupcons.Contains(searchCriteria.MotifsSoupcons))
                    .WithCancellation(token)
                    .ToList());
            }
            catch (OperationCanceledException)
            {
                result = Enumerable.Empty<DossierGdaResult>();
            }
            catch (AggregateException)
            {
                result = Enumerable.Empty<DossierGdaResult>();
            }

            return Task.FromResult(result);
        }

        public async Task<PersonneMoraleResultGda> FindPersonneMoraleAsync(
            PersonneMoraleGdaSearchCriteria searchCriteria,
            CancellationToken token = default)
        {
            var predicate = new PersonneMoraleGdaPredicateBuilderService().BuildExpression(searchCriteria);

            var person = await Context.PersonneMoraleGdas.FirstOrDefaultAsync(predicate, token)
                .ConfigureAwait(false);

            if (person != null)
                return new PersonneMoraleResultGda
                {
                    Id = person.Id,
                    NumeroCompte = person.NumeroCompte,
                    RaisonSociale = person.RaisonSociale
                };
            return null;
        }

        public async Task<PersonnePhysiqueResultGda> FindPersonnePhysiqueAsync(
            PersonnePhysiqueGdaSearchCriteria searchCriteria,
            CancellationToken token = default)
        {
            var predicate = new PersonnePhysiqueGdaPredicateBuilderService().BuildExpression(searchCriteria);
            var person = await Context.PersonnePhysiqueGdas.FirstOrDefaultAsync(predicate, token)
                .ConfigureAwait(false);
            if (person != null)
                return new PersonnePhysiqueResultGda
                {
                    NomNaissance = person.NomNaissance,
                    Prenoms = person.Prenoms,
                    DateNaissance = person.DateNaissance,
                    NumeroCompte = person.NumeroCompte,
                    Id = person.Id
                };
            return null;
        }

        public Task<Dictionary<int, List<DossierGda>>> GetArchivedAsync(CancellationToken token = default)
        {
            try
            {
                var result = Context.DossierGdas
                    .AsNoTracking()
                    .Include(x => x.Utilisateur)
                    .Include(x => x.Modificateur)
                    .Include(x => x.Direction)
                    .AsParallel()
                    .GroupBy(x => x.ModificateurId ?? x.UtilisateurId)
                    .Select(g => new { Utilisateur = g.Key, Dossiers = g.ToList() });

                return Task.FromResult(result.ToDictionary(g => g.Utilisateur, g => g.Dossiers));
            }
            catch (OperationCanceledException)
            {
            }
            catch (AggregateException)
            {
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
            }

            return Task.FromResult<Dictionary<int, List<DossierGda>>>(null);
        }


        public Task<Dictionary<int, List<DossierGda>>> GetInvestigatedAsync(CancellationToken token = default)
        {
            try
            {
                var result = Context.DossierGdas
                    .AsNoTracking()
                    .Include(x => x.Utilisateur)
                    .Include(x => x.Modificateur)
                    .Include(x => x.Direction)
                    .AsParallel()
                    .GroupBy(x => x.ModificateurId ?? x.UtilisateurId)
                    .Select(g => new { Utilisateur = g.Key, Dossiers = g.ToList() });

                return Task.FromResult(result.ToDictionary(g => g.Utilisateur, g => g.Dossiers));
            }
            catch (OperationCanceledException)
            {
            }
            catch (AggregateException)
            {
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
            }

            return Task.FromResult<Dictionary<int, List<DossierGda>>>(null);
        }

        public async Task<DossierGda> GetAsync(int entityId, bool include = false, CancellationToken token = default)
        {
            DossierGda result = null;

            try
            {
                if (include)
                {
                    result = await Context.DossierGdas
                        .Include(x => x.Utilisateur)
                        .Include(x => x.Modificateur)
                        .Include(x => x.DossierGdaPersonneMorales)
                        .ThenInclude(y => y.PersonneMoraleGda)
                        .ThenInclude(z => z.TypeRelationAffaireGda)
                        .Include(x => x.DossierGdaPersonnePhysiques)
                        .ThenInclude(y => y.PersonnePhysiqueGda)
                        .ThenInclude(z => z.TypeRelationAffaireGda)
                        .Include(x => x.DossierGdaImmediateActions)
                        .ThenInclude(a => a.Action)
                        .ThenInclude(a => a.Color)
                        .Include(x => x.DossierGdaCurrentActions)
                        .Include("DossierGdaImmediateActions.Createur")
                        .Include(x => x.DossierDegelPartielGdas)
                        .Include("DossierGdaImmediateActions.StatutImmediateActionsGda")
                        .Include("DossierGdaCurrentActions.Createur")
                        .Include("DossierGdaCurrentActions.StatutActionsGda")
                        .Include("DossierGdaCurrentActions.ReferentielActionHorsCompte")
                        .Include("TitulairesComptes.TitulaireCompte.PersonnePhysique")
                        .Include("TitulairesComptes.TitulaireCompte.PersonneMorale")
                        .Where(t => t.Id == entityId)
                        .FirstOrDefaultAsync(token)
                        .ConfigureAwait(false);

                    if (result != null)
                    {
                        result.DossierGdaPersonnePhysiques = await Context.DossierGdaPersonnePhysiques
                            .Include("PersonnePhysiqueGda.TypologieAvoirPersonneGeleePersonnePhysiques")
                            .Include("PersonnePhysiqueGda.TypologieAvoirPersonneLieePersonnePhysiques")
                            .Include("PersonnePhysiqueGda.LienPersonnePhysiqueGdas")
                            .Include("PersonnePhysiqueGda.LienPersonnePhysiquePmGdas")
                            .Where(x => x.DossierGdaId == entityId)
                            .ToListAsync(token).ConfigureAwait(false);

                        result.DossierGdaPersonneMorales = await Context.DossierGdaPersonneMorales
                            .Include("PersonneMoraleGda.TypologieAvoirPersonneGeleePersonneMorales")
                            .Include("PersonneMoraleGda.TypologieAvoirPersonneLieePersonneMorales")
                            .Include("PersonneMoraleGda.LienPersonneMoraleGdas")
                            .Include("PersonneMoraleGda.LienPersonneMoralePpGdas")
                            .Where(x => x.DossierGdaId == entityId)
                            .ToListAsync(token).ConfigureAwait(false);


                        result.DossierGdaOscs = await Context.DossierGdaOscs
                            .Include(x => x.DebitCreditOsc)
                            .Include(x => x.StatutActionsGda)
                            .Include(x => x.Createur)
                            .Where(x => x.DossierGdaId == entityId)
                            .OrderByDescending(osc => osc.DateCreation)
                            .Select(d => new DossierGdaOsc
                            {
                                Id = d.Id,
                                CodeIdentifiant = d.CodeIdentifiant,
                                Commentaire = d.Commentaire,
                                CreateurId = d.CreateurId,
                                DateCreation = d.DateCreation,
                                DateDeclaration = d.DateDeclaration,
                                DateResiliation = d.DateResiliation,
                                DebitCreditId = d.DebitCreditId,
                                DebitCreditOsc = d.DebitCreditOsc,
                                DossierGdaId = d.DossierGdaId,
                                IsRealise = d.IsRealise,
                                IsRetablis = d.IsRetablis,
                                IsManquementDgt = d.IsManquementDgt,
                                Montant = d.Montant,
                                OperationsSurCompte = d.OperationsSurCompte,
                                DateModification = d.DateModification,
                                ResponsableMoe = d.ResponsableMoe,
                                StatutActionsGda = d.StatutActionsGda != null
                                    ? new StatutActionsGda
                                    {
                                        Id = d.StatutActionsGda.Id,
                                        Code = d.StatutActionsGda.Code,
                                        FrenchName = d.StatutActionsGda.FrenchName,
                                        EnglishName = d.StatutActionsGda.EnglishName,
                                        IsActive = d.StatutActionsGda.IsActive
                                    }
                                    : null,
                                Createur = d.Createur != null
                                    ? new Utilisateur
                                    {
                                        Id = d.Createur.Id,
                                        Nom = d.Createur.Nom,
                                        Prenom = d.Createur.Prenom
                                    }
                                    : null,
                                StatutOscId = d.StatutOscId
                            })
                            .ToListAsync(token).ConfigureAwait(false);


                        result.DocumentsDossierGda = await Context.DocumentsDossierGda
                            .Include(d => d.DocumentGda)
                            .Include(d => d.CategorieDocument)
                            .Include(d => d.AppartenanceDocument)
                            .Where(p => p.DossierGdaId == entityId)
                            .Select(d => new DocumentDossierGda
                            {
                                Id = d.Id,
                                DocumentName = d.DocumentName,
                                AfficherDocument = d.AfficherDocument,
                                AppartenanceDocumentId = d.AppartenanceDocumentId,
                                AppartenanceDocument = d.AppartenanceDocument != null
                                    ? new GdaAppartenanceDocument
                                    {
                                        Id = d.AppartenanceDocument.Id,
                                        Code = d.AppartenanceDocument.Code,
                                        Libelle = d.AppartenanceDocument.Libelle,
                                        Description = d.AppartenanceDocument.Description
                                    }
                                    : null,
                                Date = d.Date,
                                DocumentGdaId = d.DocumentGdaId,
                                FileName = d.FileName,
                                CategorieDocumentId = d.CategorieDocumentId,
                                CategorieDocument = d.CategorieDocument != null
                                    ? new CategorieDocument
                                    {
                                        Id = d.CategorieDocument.Id,
                                        FrenchName = d.CategorieDocument.FrenchName,
                                        EnglishName = d.CategorieDocument.EnglishName
                                    }
                                    : null,
                                DossierGdaId = d.DossierGdaId,
                                DocumentGda = d.DocumentGda != null
                                    ? new DocumentGda
                                    {
                                        Id = d.DocumentGda.Id,
                                        FileType = d.DocumentGda.FileType,
                                        FileSize = d.DocumentGda.FileSize,
                                        Name = d.DocumentGda.Name
                                    }
                                    : null,
                                UtilisateurId = d.UtilisateurId,
                                Utilisateur = new Utilisateur
                                {
                                    Id = d.Utilisateur.Id,
                                    Nom = d.Utilisateur.Nom,
                                    Prenom = d.Utilisateur.Prenom
                                }
                            })
                            .ToListAsync(token).ConfigureAwait(false);
                    }
                }
                else
                {
                    result = await Context.DossierGdas
                        .Where(t => t.Id == entityId)
                        .AsTracking()
                        .FirstOrDefaultAsync(token)
                        .ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (AggregateException)
            {
            }

            return result;
        }

        public async Task<DossierGda> GetOscAsync(int id, CancellationToken token = default)
        {
            DossierGda result = null;

            try
            {
                result = await Context.DossierGdas
                    .Include(x => x.DossierGdaOscs)
                    .Include("DossierGdaOscs.Createur")
                    .Include("DossierGdaOscs.Modificateur")
                    .Include("DossierGdaOscs.DebitCreditOsc")
                    .Include("DossierGdaOscs.StatutActionsGda")
                    .Where(t => t.Id == id)
                    .FirstOrDefaultAsync(token)
                    .ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
            catch (AggregateException)
            {
            }

            return result;
        }

        public async Task<List<DossierGdaOsc>> GetDossierGdaOscs(int id, CancellationToken token = default)
        {
            return await Context
                      .DossierGdaOscs
                      .Where(d => d.DossierGdaId == id)
                      .ToListAsync(token);
        }

        public async Task<DossierGda> GetAsync(string code, CancellationToken token = default)
        {
            DossierGda result = null;
            try
            {
                result = await Context.DossierGdas
                    .Include(x => x.DossierGdaPersonneMorales)
                    .ThenInclude(y => y.PersonneMoraleGda)
                    .Include(x => x.DossierGdaPersonnePhysiques)
                    .ThenInclude(y => y.PersonnePhysiqueGda)
                    .Include(x => x.DossierGdaImmediateActions)
                    .ThenInclude(a => a.Action)
                    .ThenInclude(a => a.Color)
                    .Include("DossierGdaImmediateActions.Createur")
                    .Include("DossierGdaImmediateActions.Modificateur")
                    .Include("DossierGdaImmediateActions.StatutImmediateActionsGda")
                    .Include(x => x.DossierGdaCurrentActions)
                    .Include("DossierGdaCurrentActions.Createur")
                    .Include("DossierGdaCurrentActions.Modificateur")
                    .Include("DossierGdaCurrentActions.StatutActionsGda")
                    .Include("DossierGdaCurrentActions.ActionActionsGda")
                    .Include(x => x.DossierGdaOscs)
                    .Include("DossierGdaOscs.Createur")
                    .Include("DossierGdaOscs.Modificateur")
                    .Include("DossierGdaOscs.StatutActionsGda")
                    .Include("DossierGdaOscs.DebitCreditOsc")
                    .Include(d => d.DocumentsDossierGda)
                    .Where(t => t.CodeUnique == code)
                    .FirstOrDefaultAsync(token)
                    .ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
            catch (AggregateException)
            {
            }

            return result;
        }

        public async Task<DossierGda> GetDossierByDetails(string code, CancellationToken token = default)
        {
            DossierGda result = null;
            try
            {
                result = await Context.DossierGdas
                    .Include("DossierGdaPersonneMorales.PersonneMoraleGda.RegimeJuridiqueGda")
                    .Include("DossierGdaPersonnePhysiques.PersonnePhysiqueGda.Civilite")
                    .Include("DossierGdaPersonnePhysiques.PersonnePhysiqueGda.RegimeJuridiqueGda")
                    .Include(x => x.DossierGdaImmediateActions)
                    .ThenInclude(a => a.Action)
                    .ThenInclude(a => a.Color)
                    .Include(x => x.DossierGdaOscs)
                    .Include(x => x.Direction.Secteurs)
                    .Include(x => x.Utilisateur)
                    .Include(x => x.Modificateur)
                    .Include(x => x.StatutDossierGda)
                    .Include(x => x.CategorieGdas)
                    .Include(x => x.Pays)
                    .Include(x => x.RegimeSanctionGda)
                    .Include(x => x.StatutDossierGda)
                    .Include(x => x.Pays)
                    .Include(x => x.TypologieGda)
                    .Include("DossierGdaImmediateActions.Createur")
                    .Include("DossierGdaCurrentActions.Createur")
                    .Include("DossierGdaOscs.Createur")
                    .Include(d => d.DocumentsDossierGda)
                    .Where(t => t.CodeUnique == code)
                    .FirstOrDefaultAsync(token)
                    .ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
            catch (AggregateException)
            {
            }

            return result;
        }

        public async Task<IEnumerable<DossierGda>> GetByUtilisateudAsync(int utilisateurId,
            CancellationToken token = default)
        {
            return await Context.DossierGdas
                //.Where(d => d.UtilisateurId == utilisateurId)
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public string GetMailGeneriqueEntite(int dossierId,
            CancellationToken token = default)
        {
            return Context.DossierGdas
                .FirstOrDefault(d => d.Id == dossierId)
                ?.EmailGeneriqueEntite;
        }

        public string GetMailGeneriqueDdc(int dossierId,
            CancellationToken token = default)
        {
            return Context.DossierGdas
                .FirstOrDefault(d => d.Id == dossierId)
                ?.EmailGeneriqueDdc;
        }


        public async Task<IEnumerable<DossierGda>> GetManyAsync(bool include = false,
            CancellationToken token = default)
        {
            try
            {
                var result = await Context.DossierGdas
                    .Include(x => x.Utilisateur)
                    .Include(x => x.DocumentsDossierGda)
                    .ThenInclude(y => y.DocumentGda)
                    .Include(x => x.DossierGdaPersonnePhysiques)
                    .ThenInclude(y => y.PersonnePhysiqueGda)
                    .Include("DossierGdaPersonnePhysiques.StatutPersonneGda")
                    .Include("DossierGdaPersonnePhysiques.StatutPersonneGda")
                    .Include(x => x.DossierGdaPersonneMorales)
                    .ThenInclude(y => y.PersonneMoraleGda)
                    .Include("DossierGdaPersonneMorales.StatutPersonneGda")
                    .Include("DossierGdaPersonneMorales.StatutPersonneGda")
                    .ToListAsync(token)
                    .ConfigureAwait(false);

                return result;
            }
            catch (OperationCanceledException)
            {
            }
            catch (AggregateException)
            {
            }

            return null;
        }

        public IEnumerable<PersonnePhysiqueItemGda> GetPersonnesPhysiquesByDossierId(int dossierId, string language)
        {
            var personnePhysiques = Context.DossierGdaPersonnePhysiques
                .Include(x => x.PersonnePhysiqueGda)
                .Where(x => x.DossierGdaId == dossierId)
                .AsEnumerable()
                .Select(x => new PersonnePhysiqueItemGda
                {
                    FullName = $"{x.PersonnePhysiqueGda.NomNaissance} {x.PersonnePhysiqueGda.Prenoms}",
                    Nom = x.PersonnePhysiqueGda.NomNaissance,
                    Prenom = x.PersonnePhysiqueGda.Prenoms,

                    DateNaissance = x.PersonnePhysiqueGda.DateNaissance,
                    NumeroCompte = x.PersonnePhysiqueGda.NumeroCompte
                });

            return personnePhysiques;
        }

        public IEnumerable<PersonneMoraleItemGda> GetPersonnesMoralesByDossierId(int dossierId, string language)
        {
            var personneMorales = Context.DossierGdaPersonneMorales
                .Include(x => x.PersonneMoraleGda)
                .Where(x => x.DossierGdaId == dossierId)
                .AsEnumerable()
                .Select(x => new PersonneMoraleItemGda
                {
                    RaisonSociale = x.PersonneMoraleGda.RaisonSociale,
                    NumeroImmatriculation = x.PersonneMoraleGda.NumeroImmatriculation,
                    DateImmatriculation = x.PersonneMoraleGda.DateImmatriculation,
                    NumeroCompte = x.PersonneMoraleGda.NumeroCompte
                });

            return personneMorales;
        }

        public async Task<bool> UpdateAsync(DossierGda entity, CancellationToken token = default)
        {
            await using var transaction = await Context.Database.BeginTransactionAsync(token).ConfigureAwait(false);
            try
            {
                entity.DossierGdaPersonnePhysiques?.ForEach(d =>
                {
                    Context.Entry(d).State = d.Id <= 0 ? EntityState.Added : EntityState.Modified;
                    if (d.PersonnePhysiqueGda != null)
                    {
                        if (d.PersonnePhysiqueGda.Id > 0)
                        {
                            Context.Entry(d.PersonnePhysiqueGda).State = EntityState.Modified;

                            d.PersonnePhysiqueGda.TypologieAvoirPersonneGeleePersonnePhysiques?.ForEach(p =>
                            {
                                Context.Entry(p).State = p.Id <= 0 ? EntityState.Added : EntityState.Modified;
                            });

                            d.PersonnePhysiqueGda.TypologieAvoirPersonneLieePersonnePhysiques?.ForEach(p =>
                            {
                                Context.Entry(p).State = p.Id <= 0 ? EntityState.Added : EntityState.Modified;
                            });

                            d.PersonnePhysiqueGda.LienPersonnePhysiqueGdas?.ForEach(p =>
                            {
                                Context.Entry(p).State = p.Id <= 0 ? EntityState.Added : EntityState.Modified;
                            });
                            d.PersonnePhysiqueGda.LienPersonnePhysiquePmGdas?.ForEach(p =>
                            {
                                Context.Entry(p).State = p.Id <= 0 ? EntityState.Added : EntityState.Modified;
                            });

                            Context
                                .LienPersonnePhysiqueGdas
                                .Where(lpp => lpp.PersonnePhysiqueGdaId == d.PersonnePhysiqueGda.Id)
                                .ToList()
                                .ForEach(lpp =>
                                {
                                    if (d.PersonnePhysiqueGda.LienPersonnePhysiqueGdas?.All(l => l.Id != lpp.Id) !=
                                        false) Context.Entry(lpp).State = EntityState.Deleted;
                                });

                            Context
                                .LienPersonnePhysiquePmGdas
                                .Where(lppm => lppm.PersonnePhysiqueGdaId == d.PersonnePhysiqueGda.Id)
                                .ToList()
                                .ForEach(lppm =>
                                {
                                    if (d.PersonnePhysiqueGda.LienPersonnePhysiquePmGdas?.All(l => l.Id != lppm.Id) !=
                                        false) Context.Entry(lppm).State = EntityState.Deleted;
                                });

                            Context
                                .TypologieAvoirPersonneGeleePersonnePhysiques
                                .Where(t => t.PersonnePhysiqueGdaId == d.PersonnePhysiqueGda.Id)
                                .ToList()
                                .ForEach(t =>
                                {
                                    if (d.PersonnePhysiqueGda.TypologieAvoirPersonneGeleePersonnePhysiques?.All(dt =>
                                            dt.Id != t.Id) != false) Context.Entry(t).State = EntityState.Deleted;
                                });

                            Context
                                .TypologieAvoirPersonneLieePersonnePhysiques
                                .Where(t => t.PersonnePhysiqueGdaId == d.PersonnePhysiqueGda.Id)
                                .ToList()
                                .ForEach(t =>
                                {
                                    if (d.PersonnePhysiqueGda.TypologieAvoirPersonneLieePersonnePhysiques?.All(dt =>
                                            dt.Id != t.Id) != false) Context.Entry(t).State = EntityState.Deleted;
                                });
                        }
                        else
                        {
                            Context.Entry(d.PersonnePhysiqueGda).State = EntityState.Added;
                            d.PersonnePhysiqueGda.TypologieAvoirPersonneGeleePersonnePhysiques?.ForEach(p =>
                            {
                                Context.Entry(p).State = EntityState.Added;
                            });

                            d.PersonnePhysiqueGda.TypologieAvoirPersonneLieePersonnePhysiques?.ForEach(p =>
                            {
                                Context.Entry(p).State = EntityState.Added;
                            });

                            d.PersonnePhysiqueGda.LienPersonnePhysiqueGdas?.ForEach(p =>
                            {
                                Context.Entry(p).State = EntityState.Added;
                            });

                            d.PersonnePhysiqueGda.LienPersonnePhysiquePmGdas?.ForEach(p =>
                            {
                                Context.Entry(p).State = EntityState.Added;
                            });
                        }
                    }
                });

                Context
                    .DossierGdaPersonnePhysiques
                    .Where(dpp => dpp.DossierGdaId == entity.Id)
                    .ToList()
                    .ForEach(dpp =>
                    {
                        if (entity.DossierGdaPersonnePhysiques?.All(t => t.Id != dpp.Id) != false)
                        {
                            Context
                                .TypologieAvoirPersonneGeleePersonnePhysiques
                                .Where(k => k.PersonnePhysiqueGdaId == dpp.PersonnePhysiqueGdaId).AsEnumerable()
                                .ForEach(n => { Context.Entry(n).State = EntityState.Deleted; });

                            Context
                                .TypologieAvoirPersonneLieePersonnePhysiques
                                .Where(k => k.PersonnePhysiqueGdaId == dpp.PersonnePhysiqueGdaId).AsEnumerable()
                                .ForEach(n => { Context.Entry(n).State = EntityState.Deleted; });

                            Context
                                .LienPersonnePhysiqueGdas
                                .Where(k => k.PersonnePhysiqueGdaId == dpp.PersonnePhysiqueGdaId).AsEnumerable()
                                .ForEach(n => { Context.Entry(n).State = EntityState.Deleted; });

                            Context
                                .LienPersonnePhysiquePmGdas
                                .Where(k => k.PersonnePhysiqueGdaId == dpp.PersonnePhysiqueGdaId).AsEnumerable()
                                .ForEach(n => { Context.Entry(n).State = EntityState.Deleted; });
                            Context.Entry(dpp).State = EntityState.Deleted;
                        }
                    });

                //**************************
                entity.DossierGdaPersonneMorales?.ForEach(d =>
                {
                    Context.Entry(d).State = d.Id <= 0 ? EntityState.Added : EntityState.Modified;
                    if (d.PersonneMoraleGda != null)
                    {
                        if (d.PersonneMoraleGda.Id > 0)
                        {
                            Context.Entry(d.PersonneMoraleGda).State = EntityState.Modified;

                            d.PersonneMoraleGda.TypologieAvoirPersonneGeleePersonneMorales?.ForEach(p =>
                            {
                                Context.Entry(p).State = p.Id <= 0 ? EntityState.Added : EntityState.Modified;
                            });

                            d.PersonneMoraleGda.TypologieAvoirPersonneLieePersonneMorales?.ForEach(p =>
                            {
                                Context.Entry(p).State = p.Id <= 0 ? EntityState.Added : EntityState.Modified;
                            });

                            d.PersonneMoraleGda.LienPersonneMoraleGdas?.ForEach(p =>
                            {
                                Context.Entry(p).State = p.Id <= 0 ? EntityState.Added : EntityState.Modified;
                            });
                            d.PersonneMoraleGda.LienPersonneMoralePpGdas?.ForEach(p =>
                            {
                                Context.Entry(p).State = p.Id <= 0 ? EntityState.Added : EntityState.Modified;
                            });

                            Context.LienPersonneMoraleGdas
                                .Where(lpm => lpm.PersonneMoraleGdaId == d.PersonneMoraleGda.Id)
                                .ToList()
                                .ForEach(lpm =>
                                {
                                    if (d.PersonneMoraleGda.LienPersonneMoraleGdas?.All(l => l.Id != lpm.Id) != false)
                                        Context.Entry(lpm).State = EntityState.Deleted;
                                });

                            Context
                                .LienPersonneMoralePpGdas
                                .Where(lppm => lppm.PersonneMoraleGdaId == d.PersonneMoraleGda.Id)
                                .ToList()
                                .ForEach(lppm =>
                                {
                                    if (d.PersonneMoraleGda.LienPersonneMoralePpGdas?.All(l => l.Id != lppm.Id) !=
                                        false) Context.Entry(lppm).State = EntityState.Deleted;
                                });

                            Context
                                .TypologieAvoirPersonneGeleePersonneMorales
                                .Where(t => t.PersonneMoraleGdaId == d.PersonneMoraleGda.Id)
                                .ToList()
                                .ForEach(t =>
                                {
                                    if (d.PersonneMoraleGda.TypologieAvoirPersonneGeleePersonneMorales?.All(dt =>
                                            dt.Id != t.Id) != false) Context.Entry(t).State = EntityState.Deleted;
                                });

                            Context
                                .TypologieAvoirPersonneLieePersonneMorales
                                .Where(t => t.PersonneMoraleGdaId == d.PersonneMoraleGda.Id)
                                .ToList()
                                .ForEach(t =>
                                {
                                    if (d.PersonneMoraleGda.TypologieAvoirPersonneLieePersonneMorales?.All(dt =>
                                            dt.Id != t.Id) != false) Context.Entry(t).State = EntityState.Deleted;
                                });
                        }
                        else
                        {
                            Context.Entry(d.PersonneMoraleGda).State = EntityState.Added;
                            d.PersonneMoraleGda.TypologieAvoirPersonneGeleePersonneMorales?.ForEach(p =>
                            {
                                Context.Entry(p).State = EntityState.Added;
                            });

                            d.PersonneMoraleGda.TypologieAvoirPersonneLieePersonneMorales?.ForEach(p =>
                            {
                                Context.Entry(p).State = EntityState.Added;
                            });

                            d.PersonneMoraleGda.LienPersonneMoraleGdas?.ForEach(p =>
                            {
                                Context.Entry(p).State = EntityState.Added;
                            });

                            d.PersonneMoraleGda.LienPersonneMoralePpGdas?.ForEach(p =>
                            {
                                Context.Entry(p).State = EntityState.Added;
                            });
                        }
                    }
                });

                Context
                    .DossierGdaPersonneMorales
                    .Where(p => p.DossierGdaId == entity.Id)
                    .ToList()
                    .ForEach(o =>
                    {
                        if (entity.DossierGdaPersonneMorales?.All(t => t.Id != o.Id) != false)
                        {
                            Context
                                .TypologieAvoirPersonneGeleePersonneMorales
                                .Where(k => k.PersonneMoraleGdaId == o.PersonneMoraleGdaId).AsEnumerable()
                                .ForEach(n => { Context.Entry(n).State = EntityState.Deleted; });

                            Context
                                .TypologieAvoirPersonneLieePersonneMorales
                                .Where(k => k.PersonneMoraleGdaId == o.PersonneMoraleGdaId).AsEnumerable()
                                .ForEach(n => { Context.Entry(n).State = EntityState.Deleted; });

                            Context
                                .LienPersonneMoraleGdas
                                .Where(k => k.PersonneMoraleGdaId == o.PersonneMoraleGdaId).AsEnumerable()
                                .ForEach(n => { Context.Entry(n).State = EntityState.Deleted; });

                            Context
                                .LienPersonneMoralePpGdas
                                .Where(k => k.PersonneMoraleGdaId == o.PersonneMoraleGdaId).AsEnumerable()
                                .ForEach(n => { Context.Entry(n).State = EntityState.Deleted; });
                            Context.Entry(o).State = EntityState.Deleted;
                        }
                    });

                //*****************************
                entity.DossierGdaCurrentActions?.ForEach(d =>
                {
                    Context.Entry(d).State = d.Id <= 0 ? EntityState.Added : EntityState.Modified;
                });

                entity.DossierDegelPartielGdas?
                    .ForEach(d => { Context.Entry(d).State = d.Id <= 0 ? EntityState.Added : EntityState.Modified; });

                Context.DossierDegelPartielGdas
                    .Where(p => p.DossierGdaId == entity.Id).AsEnumerable()
                    .ForEach(o =>
                    {
                        if (entity.DossierDegelPartielGdas?.Any(t => t.Id == o.Id && t.Id > 0) != true)
                            Context.Entry(o).State = EntityState.Deleted;
                    });

                var idsPm = entity.DossierGdaPersonneMorales?.Select(x => x.Id).ToList();
                var dossiersPm = Context.DossierGdaPersonneMorales
                    .Where(p => p.DossierGdaId == entity.Id &&
                                (idsPm == null || !idsPm.Contains(p.Id)))
                    .ToList();
                Context.DossierGdaPersonneMorales.RemoveRange(dossiersPm.Where(p => p.DossierGdaId == entity.Id));
                //DossierGdaCurrentActions
                var idsActionCurrent = entity.DossierGdaCurrentActions?.Select(x => x.Id).ToList();

                var actionCurrents = Context.DossierGdaCurrentActions
                    .Where(p => p.DossierGdaId == entity.Id &&
                                (idsActionCurrent == null || !idsActionCurrent.Contains(p.Id)))
                    .ToList();

                Context.DossierGdaCurrentActions.RemoveRange(actionCurrents.Where(p => p.DossierGdaId == entity.Id));

                entity.DossierGdaPersonnePhysiques?.ForEach(d =>
                {
                    d.PersonnePhysiqueGda.SoundexNomFr =
                        BacaratWebContext.Soundex(d.PersonnePhysiqueGda.NomNaissance, Language.French);
                    d.PersonnePhysiqueGda.SoundexNomEn =
                        BacaratWebContext.Soundex(d.PersonnePhysiqueGda.NomNaissance, Language.English);
                    d.PersonnePhysiqueGda.SoundexNomUsuelFr =
                        BacaratWebContext.Soundex(d.PersonnePhysiqueGda.NomUsuel, Language.French);
                    d.PersonnePhysiqueGda.SoundexNomUsuelEn =
                        BacaratWebContext.Soundex(d.PersonnePhysiqueGda.NomUsuel, Language.English);
                    d.PersonnePhysiqueGda.SoundexPrenomFr =
                        BacaratWebContext.Soundex(d.PersonnePhysiqueGda.Prenoms, Language.French);
                    d.PersonnePhysiqueGda.SoundexPrenomEn =
                        BacaratWebContext.Soundex(d.PersonnePhysiqueGda.Prenoms, Language.English);
                });

                entity.DossierGdaPersonneMorales?.ForEach(d =>
                {
                    d.PersonneMoraleGda.SoundexRaisonFr =
                        BacaratWebContext.Soundex(d.PersonneMoraleGda.RaisonSociale, Language.French);
                    d.PersonneMoraleGda.SoundexRaisonEn =
                        BacaratWebContext.Soundex(d.PersonneMoraleGda.RaisonSociale, Language.English);
                });

                entity
                    .DossierGdaImmediateActions
                    ?.ForEach(s =>
                    {
                        if (s.Id <= 0)
                        {
                            Context.Entry(s).State = EntityState.Added;
                            return;
                        }

                        var objectFromDataBase = Context.DossierGdaImmediateActions.Single(x => x.Id == s.Id);
                        if (EntitiesComparer.AreEntitiesEqual(s, objectFromDataBase))
                        {
                            Context.Entry(s).State = EntityState.Unchanged;
                            return;
                        }

                        s.ModificateurId = entity.ModificateurId;
                        s.DateModification = DateTimeOffset.Now;

                        Context.Entry(s).State = EntityState.Modified;
                    });

                entity.DocumentsDossierGda
                    ?.ForEach(d =>
                    {
                        Context.Entry(d).State = d.Id <= 0 ? EntityState.Added : EntityState.Modified;
                        if (d.DocumentGda == null)
                            return;

                        Context.Entry(d.DocumentGda).State =
                            d.DocumentGda.Id <= 0 ? EntityState.Added : EntityState.Modified;
                    });

                Context.DocumentsDossierGda.Where(dd => dd.DossierGdaId == entity.Id).AsEnumerable()
                    .ForEach(o =>
                    {
                        if (entity.DocumentsDossierGda?.All(edd => edd.Id != o.Id) != false)
                            Context.Entry(o).State = EntityState.Deleted;
                    });

                entity.TitulairesComptes
                    .ForEach(tc =>
                    {
                        if (tc.TitulaireCompte?.PersonnePhysique != null)
                        {
                            var personnePhysique = tc.TitulaireCompte.PersonnePhysique;
                            Context.Entry(personnePhysique).State =
                                personnePhysique.Id <= 0 ? EntityState.Added : EntityState.Modified;

                            tc.TitulaireCompte = personnePhysique;
                        }

                        if (tc.TitulaireCompte?.PersonneMorale != null)
                        {
                            var personneMorale = tc.TitulaireCompte.PersonneMorale;
                            Context.Entry(personneMorale).State =
                                personneMorale.Id <= 0 ? EntityState.Added : EntityState.Modified;

                            tc.TitulaireCompte = personneMorale;
                        }

                        Context.Entry(tc).State = tc.Id <= 0 ? EntityState.Added : EntityState.Modified;
                    });

                Context.DossierGdaTransactionnelClients
                    .Where(dtc => dtc.DossierGdaId == entity.Id).AsEnumerable()
                    .ForEach(dtc =>
                    {
                        if (entity.TitulairesComptes?.All(etc => etc.Id != dtc.Id) != false)
                            Context.Entry(dtc).State = EntityState.Deleted;
                    });

                await UpdateDossierGdaOscsAsync(entity, token);

                await UpdateCore(entity, transaction, token);
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
                await transaction.RollbackAsync(token).ConfigureAwait(false);
                return false;
            }

            return true;
        }

        private List<EntityEntryBeforeSave> GetEntriesToNotifyStateChange()
        {
            var result = Context
                .ChangeTracker
                .Entries()
                .Where(entry => entry.State == EntityState.Added &&
                                (
                                    entry.Entity.GetType().Name == nameof(DossierGdaOsc) ||
                                    entry.Entity.GetType().Name == nameof(DossierGdaCurrentAction) ||
                                    entry.Entity.GetType().Name == nameof(TypologieAvoirPersonneGeleePersonneMorale) ||
                                    entry.Entity.GetType().Name == nameof(TypologieAvoirPersonneGeleePersonnePhysique) ||
                                    entry.Entity.GetType().Name == nameof(TypologieAvoirPersonneLieePersonneMorale) ||
                                    entry.Entity.GetType().Name == nameof(TypologieAvoirPersonneLieePersonnePhysique)||
                                    entry.Entity.GetType().Name == nameof(DocumentDossierGda)
                                )
                );
            var actionA2Id = Context.ReferentielImmediateActionsGdas
                .Single(x => x.Code == nameof(DossierGdaImmediateActionEnum.A2)).Id;
            var addedImmediateActionEntities = Context.ChangeTracker.Entries().Where(entry =>
                (entry.State == EntityState.Added || entry.State == EntityState.Modified) &&
                entry.Entity.GetType().Name == nameof(DossierGdaImmediateAction)
                && (entry.Entity as DossierGdaImmediateAction)?.ActionId == actionA2Id);
            result = result.Union(addedImmediateActionEntities);


            var dateBlocage = Context.ChangeTracker.Entries()
                .Where(entry => entry.State == EntityState.Modified
                                && entry.Entity.GetType().Name == nameof(DossierGda)
                                && Context.DossierGdas.Single(x => x.Id == (entry.Entity as DossierGda).Id)
                                    .DateBlocage == null
                                && (entry.Entity as DossierGda)?.DateBlocage != Context.DossierGdas
                                    .Single(x => x.Id == (entry.Entity as DossierGda).Id).DateBlocage);
            result = result.Union(dateBlocage);

            //BLOC DEGEL TRANSACTION
            var typeDegel = Context.ChangeTracker.Entries()
              .Where(entry => entry.State == EntityState.Modified
                              && entry.Entity.GetType().Name == nameof(DossierGda)
                              && (entry.Entity as DossierGda)?.TypeDegelId != Context.DossierGdas
                                  .Single(x => x.Id == (entry.Entity as DossierGda).Id).TypeDegelId);

            result = result.Union(typeDegel);
            var dateDegelTransaction = Context.ChangeTracker.Entries()
               .Where(entry => entry.State == EntityState.Modified
                               && entry.Entity.GetType().Name == nameof(DossierGda)
                               && (entry.Entity as DossierGda)?.DateDegelTransaction != Context.DossierGdas
                                   .Single(x => x.Id == (entry.Entity as DossierGda).Id).DateDegelTransaction);
    
            result = result.Union(dateDegelTransaction);
            var motifDegelTotal = Context.ChangeTracker.Entries()
               .Where(entry => entry.State == EntityState.Modified
                               && entry.Entity.GetType().Name == nameof(DossierGda)
                               && (entry.Entity as DossierGda)?.MotifDegel != Context.DossierGdas
                                   .Single(x => x.Id == (entry.Entity as DossierGda).Id).MotifDegel);
            result = result.Union(motifDegelTotal);
            var dossierDegelPartiel = Context
               .ChangeTracker
               .Entries()
               .Where(entry => entry.State == EntityState.Added &&
                               entry.Entity.GetType().Name == nameof(DossierDegelPartielGda));
            result = result.Union(dossierDegelPartiel);
            var finalresult = result.Select(e =>
            {
                var customName = e.Entity.GetType().Name;
                var isDegel = typeDegel.Any(x => x.Entity == e.Entity) || motifDegelTotal.Any(x => x.Entity == e.Entity) || dateDegelTransaction.Any(x => x.Entity == e.Entity) || dossierDegelPartiel.Any(x => x.Entity == e.Entity);
                if (isDegel)
                {
                    customName = "DegelTransactionGda";
                }
                return new EntityEntryBeforeSave
                {
                    Entity = e.Entity,
                    State = e.State,
                    CustomEntityName = customName
                };

            }).ToList();
            return finalresult;
            //return result.Select(e => new EntityEntryBeforeSave { Entity = e.Entity, State = e.State }).ToList();
        }

        public async Task<IEnumerable<DocumentDossierGda>> GetDocumentDossierGdas(int dossierId,
            CancellationToken token = default)
        {
            var result = await Context.DocumentsDossierGda
                .Include(x => x.DocumentGda)
                .Where(x => x.DossierGdaId == dossierId || dossierId == 0)
                .ToListAsync(token)
                .ConfigureAwait(false);

            return result;
        }

        public async Task<IEnumerable<DocumentDossierGda>> GetDocumentIdentiteDossierGdas(
            CancellationToken token = default)
        {
            var result = await Context.DocumentsDossierGda
                .Include(x => x.DocumentGda)
                .Where(x => (x.DocumentGda.Name.ToUpper().Contains("CNI") ||
                             x.DocumentGda.Name.ToUpper().Contains("IDENTI") ||
                             x.DocumentGda.Name.ToUpper().Contains("CARTE") ||
                             x.DocumentGda.Name.ToUpper().Contains("CONDUI") ||
                             x.DocumentGda.Name.ToUpper().Contains("GREENCARD") ||
                             x.DocumentGda.Name.ToUpper().Contains("LIVRET") ||
                             x.DocumentGda.Name.ToUpper().Contains("ATTESTATION DE CONTR") ||
                             x.DocumentGda.Name.ToUpper().Contains("TITRE") ||
                             x.DocumentName.ToUpper().Contains("CNI") ||
                             x.DocumentName.ToUpper().Contains("IDENTI") ||
                             x.DocumentName.ToUpper().Contains("CARTE") ||
                             x.DocumentName.ToUpper().Contains("CONDUI") ||
                             x.DocumentName.ToUpper().Contains("GREENCARD") ||
                             x.DocumentName.ToUpper().Contains("LIVRET") ||
                             x.DocumentName.ToUpper().Contains("ATTESTATION DE CONTR") ||
                             x.DocumentName.ToUpper().Contains("TITRE")) &&
                            x.DocumentGda.CreationDate >= DateTimeOffset.Now.AddMonths(-2) &&
                            (x.DocumentGda.FileType.Contains("image") || x.DocumentGda.FileType.Contains("PDF")))
                .ToListAsync(token)
                .ConfigureAwait(false);

            return result;
        }

        public IEnumerable<EntiteGda> GetEntitiesByDirection(int directionId)
        {
            return Context.EntiteGdas
                .Where(x => x.DirectionId == directionId && x.IsActive)
                .ToList();
        }


        public IQueryable<EntiteGda> GeEntityByDirectionId(int directionId)
        {
            var result = Context.EntiteGdas
                .Where(x => x.DirectionId == directionId && x.IsActive).Select(e => new EntiteGda
                {
                    Id = e.Id,
                    Lisp = e.Lisp,
                    Liadr4 = e.Liadr4,
                    Code = e.Code,
                    SegmentClient = e.SegmentClient,
                    SecteurId = e.SecteurId
                });

            return result;
        }

        public int GetDossierOrder(int directionId)
        {
            var result = Context.DossierGdaHistos
                .Where(c => c.DirectionId == directionId)
                .Select(x => x.CodeUnique)
                .Distinct();

            if (result.Any())
                return result.Count() + 1;
            return 1;
        }

        public async Task<bool> UpdateDocumentAsync(DossierGda entity, CancellationToken token = default)
        {
            await using var transaction = await Context.Database.BeginTransactionAsync(token).ConfigureAwait(false);
            try
            {
                entity.DocumentsDossierGda
                    .ForEach(d =>
                    {
                        Context.Entry(d).State = d.Id <= 0 ? EntityState.Added : EntityState.Modified;

                        if (d.DocumentGda != null)
                            Context.Entry(d.DocumentGda).State =
                                d.DocumentGda.Id <= 0 ? EntityState.Added : EntityState.Modified;
                    });


                base.UpdateCore(entity);
                await SaveAsync(token).ConfigureAwait(false);

                await transaction.CommitAsync(token).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
                await transaction.RollbackAsync(token).ConfigureAwait(false);
                return false;
            }

            return true;
        }


        public async Task<DossierEscalade> GetDossierEscaladeByCodeUnique(string codeUnique,
            CancellationToken token = default)
        {
            DossierEscalade dossierEscalade;
            try
            {
                dossierEscalade = await Context.DossierEscalades.Where(x => x.Code == codeUnique)
                    .Include("DossierEscaladePersonneMorales.PersonneMoraleEscalade")
                    .Include("DossierEscaladePersonnePhysiques.PersonnePhysiqueEscalade")
                    .Include(x => x.DocumentDossierEscalades)
                    .ThenInclude(a => a.Utilisateur)
                    .Include("DocumentDossierEscalades.AppartenanceDocument")
                    .Include("DocumentDossierEscalades.DocumentEscalade")
                    .Include(x => x.Createur)
                    .ThenInclude(y => y.DirectionAttache)
                    .Include(d => d.DirectionFlge)
                    .Include(d => d.ResponsableFlge)
                    .ThenInclude(r => r.DirectionAttache)
                    .Include(d => d.EntiteLocal)
                    .Include(d => d.EntiteConcerneDossierEscalades)
                    .ThenInclude(d => d.SousEntite)
                    .Include("TitulairesComptesCredites.TitulaireCompte.PersonnePhysique")
                    .Include("TitulairesComptesCredites.TitulaireCompte.PersonneMorale")
                    .Include("TitulairesComptesDebites.TitulaireCompte.PersonnePhysique")
                    .Include("TitulairesComptesDebites.TitulaireCompte.PersonneMorale")
                    .FirstAsync(token);
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
                return null;
            }

            return dossierEscalade;
        }

        public async Task<GdaAppartenanceDocument> GetDocumentDossierGdaAppartenance(CancellationToken token = default)
        {
            GdaAppartenanceDocument gdaAppartenanceDocument;
            try
            {
                gdaAppartenanceDocument = await Context.GdaAppartenanceDocuments.FirstAsync(token);
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
                return null;
            }

            return gdaAppartenanceDocument;
        }

        public async Task<Utilisateur> GetDocumentDossierGdaUser(CancellationToken token = default)
        {
            Utilisateur utilisateur;
            try
            {
                utilisateur = await Context.Utilisateurs.FirstAsync(token);
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
                return null;
            }

            return utilisateur;
        }

        public IQueryable<DossierGda> GetDossiersByIdRegistreNationale(string idRegistreNational)
        {
            var relationalCategoryId = _referentielService.GetCategorieGdas().Single(cat => cat.Code == nameof(CategorieGdaEnum.Relational)).Id;
            return Context.DossierGdas
                .Where(d => d.IdRegistreNationale == idRegistreNational && d.CategorieId == relationalCategoryId)
                .Include(d => d.DirectionCollaborateurEscalade);
        }

        public async Task<bool> DisableDossierGda(int dossierGdaId, CancellationToken cancellationToken = default)
        {
            await using var transaction = await Context
                .Database
                .BeginTransactionAsync(cancellationToken)
                .ConfigureAwait(false);
            try
            {
                var dossierGda = await Context.DossierGdas
                    .Where(d => d.Id == dossierGdaId)
                    .AsTracking()
                    .SingleOrDefaultAsync(cancellationToken)
                    .ConfigureAwait(false);
                dossierGda.IsActive = false;
                dossierGda.DossierEscaladeCodeUnique = null;

                var dossierEscalade = await Context.DossierEscalades
                    .Where(d => d.DossierGdaId == dossierGdaId)
                    .AsTracking()
                    .SingleOrDefaultAsync(cancellationToken)
                    .ConfigureAwait(false);
                if (dossierEscalade != null)
                {
                    dossierEscalade.DossierGdaId = null;
                    dossierEscalade.OldDossierGdaId = dossierGdaId;
                }

                var result = await SaveAsync(cancellationToken).ConfigureAwait(false);
                await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                return result;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                _logger.TraceError(e);
                return false;
            }
        }

        public async Task<bool> EnableDossierGda(int dossierGdaId, CancellationToken cancellationToken = default)
        {
            await using var transaction = await Context
                .Database
                .BeginTransactionAsync(cancellationToken)
                .ConfigureAwait(false);
            try
            {
                var dossierGda = await Context.DossierGdas
                    .Where(d => d.Id == dossierGdaId)
                    .AsTracking()
                    .SingleOrDefaultAsync(cancellationToken)
                    .ConfigureAwait(false);
                dossierGda.IsActive = true;

                var dossierEscalade = await Context.DossierEscalades
                    .Where(d => d.OldDossierGdaId == dossierGdaId)
                    .AsTracking()
                    .SingleOrDefaultAsync(cancellationToken)
                    .ConfigureAwait(false);

                if (dossierEscalade != null)
                {
                    dossierEscalade.DossierGdaId = dossierGdaId;
                    dossierEscalade.OldDossierGdaId = null;
                    dossierGda.DossierEscaladeCodeUnique = dossierEscalade.Code;
                }

                var result = await SaveAsync(cancellationToken).ConfigureAwait(false);
                await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
                return result;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                _logger.TraceError(e);
                return false;
            }
        }

        private async Task PushEntitiesStateChangeNotifications(DossierGda dossierGda,
            IEnumerable<EntityEntryBeforeSave> entries)
        {
            if (entries == null)
                return;

            foreach (var entry in entries) await PushEntityStateChangeNotification(dossierGda, entry);
        }

        private EntityState ResolveState(EntityEntryBeforeSave entry, string entityName)
        {
            if (entityName == "DegelTransactionGda")
            {
                return EntityState.Modified;
            }
            return entry.State;
        }
        private async Task PushEntityStateChangeNotification(DossierGda dossierGda, EntityEntryBeforeSave entry)
        {
            try
            {
                var isDdcMailActiveGda = Context.Directions.Any(x =>
                    x.Id == dossierGda.DirectionColloaborateurEscaladeId && x.IsDdcMailActiveGda);

                if (isDdcMailActiveGda)
                {
                    var isDdcUser = dossierGda.IsDdcUser;
                    var entityName = entry.CustomEntityName?? entry.Entity.GetType().Name;
                    var state = ResolveState(entry, entityName);
                    var notificationTypeId =
                        Context.EmailNotificationTypes.Single(t =>
                                t.Code ==
                                $"{entityName.Replace("PersonneMorale", "").Replace("PersonnePhysique", "")}{state}")
                            .Id;
                    var statutOsc = string.Empty;
                    var emailTemplate = await _emailNotificationTemplateService
                        .GetEmailNotification(
                            (int)ActiviteModule.Gda,
                            notificationTypeId, 1)
                        .ConfigureAwait(false);

                    if (emailTemplate == null)
                        throw new CustomMessageException("email template not found");

                    if (entry.Entity.GetType().Name == nameof(DossierGdaOsc))
                    {
                        var dosc = (DossierGdaOsc)entry.Entity;
                        statutOsc = Context.StatutActionsGdas.FirstOrDefault(x => x.IsActive && x.Id == dosc.StatutOscId)
                            ?.FrenchName;
                    }
                    if (notificationTypeId == (int)NotificationType.DegelTransactionGdaModified)
                    {
                        var emailNotificationToGeneriqueEntite = new EmailNotification
                        {
                            DateNotification = DateTimeOffset.UtcNow,
                            EmailTo = dossierGda.EmailGeneriqueEntite,
                            Subject = string.Format(emailTemplate.Subject, dossierGda.CodeUnique),
                            Body = string.Format(emailTemplate.Body, dossierGda.CodeUnique)
                        };

                        var emailNotificationToAnalystEntite = new EmailNotification
                        {
                            DateNotification = DateTimeOffset.UtcNow,
                            EmailTo = dossierGda.EmailCollaborateur,
                            Subject = string.Format(emailTemplate.Subject, dossierGda.CodeUnique),
                            Body = string.Format(emailTemplate.Body, dossierGda.CodeUnique)
                        };
                        var emailNotificationToAnalystDdc = new EmailNotification
                        {
                            DateNotification = DateTimeOffset.UtcNow,
                            EmailTo = dossierGda.EmailResponsable,
                            Subject = string.Format(emailTemplate.Subject, dossierGda.CodeUnique),
                            Body = string.Format(emailTemplate.Body, dossierGda.CodeUnique)
                        };
                        var emailNotificationToGeneriqueDdc = new EmailNotification
                        {
                            DateNotification = DateTimeOffset.UtcNow,
                            EmailTo = dossierGda.EmailGeneriqueDdc,
                            Subject = string.Format(emailTemplate.Subject, dossierGda.CodeUnique),
                            Body = string.Format(emailTemplate.Body, dossierGda.CodeUnique)
                        };


                        if (!await _emailNotificationService.PushNotification(emailNotificationToGeneriqueEntite).ConfigureAwait(false)
                            || !await _emailNotificationService.PushNotification(emailNotificationToAnalystEntite).ConfigureAwait(false)
                            || !await _emailNotificationService.PushNotification(emailNotificationToAnalystDdc).ConfigureAwait(false)
                            || !await _emailNotificationService.PushNotification(emailNotificationToGeneriqueDdc).ConfigureAwait(false))
                            throw new CustomMessageException("failed to send notifications");

                    }
                    if (notificationTypeId == (int)NotificationType.DocumentDossierGdaAdded)
                    {
                        if (isDdcUser)
                        {
                            var emailNotificationToGeneriqueEntite = new EmailNotification
                            {
                                DateNotification = DateTimeOffset.UtcNow,
                                EmailTo = dossierGda.EmailGeneriqueEntite,
                                Subject = string.Format(emailTemplate.Subject,dossierGda.CodeUnique),
                                Body = string.Format(emailTemplate.Body,dossierGda.CodeUnique)
                            };

                            var emailNotificationToAnalystEntite = new EmailNotification
                            {
                                DateNotification = DateTimeOffset.UtcNow,
                                EmailTo = dossierGda.EmailCollaborateur,
                                Subject = string.Format( emailTemplate.Subject,dossierGda.CodeUnique),
                                Body = string.Format( emailTemplate.Body,dossierGda.CodeUnique)
                            };

                            if (!await _emailNotificationService.PushNotification(emailNotificationToGeneriqueEntite).ConfigureAwait(false) || !await _emailNotificationService.PushNotification(emailNotificationToAnalystEntite).ConfigureAwait(false))
                                throw new CustomMessageException("failed to send notifications");
                        }
                        else
                        {
                            var emailNotificationToAnalystDdc = new EmailNotification
                            {
                                DateNotification = DateTimeOffset.UtcNow,
                                EmailTo = dossierGda.EmailResponsable,
                                Subject = string.Format( emailTemplate.Subject,dossierGda.CodeUnique),
                                Body = string.Format( emailTemplate.Body,dossierGda.CodeUnique)
                            };
                            var emailNotificationToGeneriqueDdc = new EmailNotification
                            {
                                DateNotification = DateTimeOffset.UtcNow,
                                EmailTo = dossierGda.EmailGeneriqueDdc,
                                Subject = string.Format(emailTemplate.Subject,dossierGda.CodeUnique),
                                Body = string.Format( emailTemplate.Body, dossierGda.CodeUnique)
                            };
                            if (!await _emailNotificationService.PushNotification(emailNotificationToAnalystDdc).ConfigureAwait(false) || !await _emailNotificationService.PushNotification(emailNotificationToGeneriqueDdc).ConfigureAwait(false))
                                throw new CustomMessageException("failed to send notifications");
                        }

                    }
                    if (notificationTypeId != (int)NotificationType.DegelTransactionGdaModified && notificationTypeId != (int)NotificationType.DocumentDossierGdaAdded)
                    {
                        var emailNotification = new EmailNotification
                        {
                            DateNotification = DateTimeOffset.UtcNow,

                            EmailTo = dossierGda.EmailGeneriqueDdc,

                            Subject = string.Format(
                          emailTemplate.Subject,
                          dossierGda.CodeUnique
                      ),

                            Body = string.Format(
                          emailTemplate.Body,
                          dossierGda.CodeUnique,
                          dossierGda.EntiteCollaborateur, statutOsc)
                        };


                        var emailNotificationDdcAnalyst = new EmailNotification
                        {
                            DateNotification = DateTimeOffset.UtcNow,

                            EmailTo = dossierGda.EmailResponsable,

                            Subject = string.Format(
                                emailTemplate.Subject,
                                dossierGda.CodeUnique
                            ),

                            Body = string.Format(
                                emailTemplate.Body,
                                dossierGda.CodeUnique,
                                dossierGda.EntiteCollaborateur, statutOsc)
                        };

                        if (!await _emailNotificationService.PushNotification(emailNotification).ConfigureAwait(false) || !await _emailNotificationService.PushNotification(emailNotificationDdcAnalyst).ConfigureAwait(false))
                            throw new CustomMessageException("failed to send notifications");

                    }              
                }
            }
            catch (Exception ex)
            {
                _logger.TraceError(ex);
            }
        }

        private async Task UpdateCore(DossierGda entity, IDbContextTransaction transaction, CancellationToken token)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.ChangeTracker
                .TrackGraph(entity,
                    e => { e.Entry.State = e.Entry.IsKeySet ? EntityState.Unchanged : EntityState.Modified; });
            var newEntriesToNotify = GetEntriesToNotifyStateChange();
            await Context.SaveChangesAsync(token);
            await transaction.CommitAsync(token).ConfigureAwait(false);
            await PushEntitiesStateChangeNotifications(entity, newEntriesToNotify);
        }

        public async Task<Dictionary<int, string>> GetCodeUniqueDestinataire(int id, CancellationToken token)
        {
            var result = new Dictionary<int, string>();
            if (Context.DossierGdas.Any(x => x.DossierGdaSourceId == id))
            {
                var dossierDestinataire = await Context.DossierGdas.SingleAsync(x => x.DossierGdaSourceId == id, token).ConfigureAwait(false);
                result.Add(dossierDestinataire.Id, dossierDestinataire.CodeUnique);

            }
            return result;
        }
    }
}
