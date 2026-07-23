using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BacaratWeb.Model.Entities;
using BacaratWeb.Entities.Commun;
using Microsoft.EntityFrameworkCore;
using BacaratWeb.Services.Commun.Interfaces;
using BacaratWeb.Entities.Gda;
using BacaratWeb.Models.Referentials;
using BacaratWeb.Shared;
using NPOI.POIFS.FileSystem;

namespace BacaratWeb.Services.Commun
{
    public class EmailNotificationService : BacaratWebService<EmailNotification>, IEmailNotificationService
    {
        public EmailNotificationService(BacaratWebContext context) : base(context) { }

        public async Task<bool> AddAsync(EmailNotification entity, CancellationToken token = default)
        {
            await Context.EmailNotifications.AddAsync(entity, token);
            return await SaveAsync(token).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(EmailNotification entity, CancellationToken token = default)
        {
            Context.EmailNotifications.Remove(entity);
            return await SaveAsync(token).ConfigureAwait(false);
        }

        public async Task<bool> ExistsAsync(EmailNotification entity, CancellationToken token = default)
        {
            return await Context
                .EmailNotifications
                .AnyAsync(t => t.Id == entity.Id, token)
                .ConfigureAwait(false);
        }

        public async Task<EmailNotification> GetAsync(int entityId,
            bool include = false,
            CancellationToken token = default)
        {
            return await Context.EmailNotifications
                .AsTracking()
                .Where(u => u.Id == entityId)
                .FirstOrDefaultAsync(token)
                .ConfigureAwait(false);
        }


        public async Task<IEnumerable<EmailNotification>> GetManyAsync(bool include = false,
            CancellationToken token = default)
        {
            return await Context.EmailNotifications
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<EmailNotification>>
            GetUnEmailedNotifications(CancellationToken token = default)
        {
            return await Context.EmailNotifications
                .Where(e => !e.DateNotification.HasValue)
                .ToListAsync(token)
                .ConfigureAwait(false);
        }

        public void PurgeEmailedNotification()
        {
            var limitDate = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(30));

            var notifications = Context.EmailNotifications
                .Where(x => x.DateNotification != null && x.DateNotification.Value < limitDate)
                .ToList();

            if (notifications.Any())
            {
                Context.EmailNotifications.RemoveRange(notifications);
                Context.SaveChanges(true);
            }
        }

        public async Task<bool> PushNotification(EmailNotification emailNotification, CancellationToken token = default)
        {
            await using var transaction = await Context.Database.BeginTransactionAsync(token).ConfigureAwait(false);
            try
            {
                await Context.EmailNotifications.AddAsync(emailNotification, token).ConfigureAwait(false);
                var result = await SaveAsync(token).ConfigureAwait(false);
                if (result)
                {
                    await transaction.CommitAsync(token).ConfigureAwait(false);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            await transaction.RollbackAsync(token).ConfigureAwait(false);
            return false;
        }

        public async Task<bool> PushNotifications(IEnumerable<EmailNotification> emailNotification,
            CancellationToken token = default)
        {
            await using var transaction = await Context.Database.BeginTransactionAsync(token).ConfigureAwait(false);
            var result = false;
            try
            {
                await Context.EmailNotifications.AddRangeAsync(emailNotification, token).ConfigureAwait(false);
                result = await SaveAsync(token).ConfigureAwait(false);

                await transaction.CommitAsync(token).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
                await transaction.RollbackAsync(token).ConfigureAwait(false);
            }

            return result;
        }

        public async Task<bool> SetNotificationEmailed(EmailNotification notification,
            CancellationToken token = default)
        {
            if (notification == null) return true;

            await using var transaction = await Context.Database.BeginTransactionAsync(token).ConfigureAwait(false);
            var result = false;
            try
            {
                notification.DateNotification = DateTimeOffset.UtcNow;
                result = await UpdateAsync(notification, token).ConfigureAwait(false);

                await transaction.CommitAsync(token).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
                await transaction.RollbackAsync(token).ConfigureAwait(false);
            }

            return result;
        }

        public async Task<bool> SetNotificationEmailed(IEnumerable<EmailNotification> notifications, int maxDays,
            CancellationToken token = default)
        {
            await using var transaction = await Context.Database.BeginTransactionAsync(token).ConfigureAwait(false);
            var result = false;
            try
            {
                var emailNotifications = notifications.ToList();
                if (emailNotifications.Any())
                {
                    var limitDate = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(maxDays));

                    var notificationsTuPurge = Context.EmailNotifications.Where(x =>
                        x.DateNotification != null && x.DateNotification.Value < limitDate).ToList();

                    if (emailNotifications.Any()) Context.EmailNotifications.RemoveRange(notificationsTuPurge);

                    emailNotifications.ForEach(n =>
                    {
                        n.DateNotification = DateTimeOffset.UtcNow;
                        Context.Entry(n).State = EntityState.Modified;
                    });

                    result = await SaveAsync(token).ConfigureAwait(false);
                }

                await transaction.CommitAsync(token).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
                await transaction.RollbackAsync(token).ConfigureAwait(false);
            }

            return result;
        }

        public async Task<bool> UpdateAsync(EmailNotification entity, CancellationToken token = default)
        {
            await using var transaction = await Context.Database.BeginTransactionAsync(token).ConfigureAwait(false);
            var result = false;
            try
            {
                UpdateCore(entity);
                result = await SaveAsync(token).ConfigureAwait(false);
                await transaction.CommitAsync(token).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Une exception s'est produite : " + e.Message);
                await transaction.RollbackAsync(token).ConfigureAwait(false);
            }
            return result;
        }
    }
}
