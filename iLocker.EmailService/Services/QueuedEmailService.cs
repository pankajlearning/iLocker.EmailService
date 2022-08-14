using iLocker.EmailService.DomainModels;
using iLocker.EmailService.Services.Contracts;

namespace iLocker.EmailService.Services;

/// <summary>
/// Queued email service
/// </summary>
public class QueuedEmailService : IQueuedEmailService
{
    #region Fields

    //private readonly IRepository<QueuedEmail> _queuedEmailRepository;

    #endregion

    #region Ctor

    //public QueuedEmailService(IRepository<QueuedEmail> queuedEmailRepository)
    //{
    //    _queuedEmailRepository = queuedEmailRepository;
    //}

    #endregion

    #region Methods

    /// <summary>
    /// Add a queued email
    /// </summary>
    /// <param name="queuedEmail">Queued email</param>        
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task AddQueuedEmailAsync(QueuedEmail queuedEmail)
    {
        //await _queuedEmailRepository.InsertAsync(queuedEmail);
    }

    /// <summary>
    /// Updates a queued email
    /// </summary>
    /// <param name="queuedEmail">Queued email</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdateQueuedEmailAsync(QueuedEmail queuedEmail)
    {
        //await _queuedEmailRepository.UpdateAsync(queuedEmail);
    }

    /// <summary>
    /// Deleted a queued email
    /// </summary>
    /// <param name="queuedEmail">Queued email</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteQueuedEmailAsync(QueuedEmail queuedEmail)
    {
        //await _queuedEmailRepository.DeleteAsync(queuedEmail);
    }

    /// <summary>
    /// Deleted a queued emails
    /// </summary>
    /// <param name="queuedEmails">Queued emails</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteQueuedEmailsAsync(IList<QueuedEmail> queuedEmails)
    {
        //await _queuedEmailRepository.DeleteAsync(queuedEmails);
    }

    /// <summary>
    /// Gets a queued email by identifier
    /// </summary>
    /// <param name="queuedEmailId">Queued email identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the queued email
    /// </returns>
    public virtual async Task<QueuedEmail> GetQueuedEmailAsync(long queuedEmailId)
    {
        //return await _queuedEmailRepository.GetByIdAsync(queuedEmailId, cache => default);
    }

    /// <summary>
    /// Gets all queued email
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the queued email list
    /// </returns>
    public virtual async Task<IList<QueuedEmail>> GetQueuedEmailsAsync()
    {
        //var emailAccounts = await _emailAccountRepository.GetAllAsync(query =>
        //{
        //    return from ea in query
        //           orderby ea.Id
        //           select ea;
        //}, cache => default);

        //return emailAccounts;
    }

    /// <summary>
    /// Deletes already sent emails
    /// </summary>
    /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
    /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the number of deleted emails
    /// </returns>
    public virtual async Task<int> DeleteAlreadySentEmailsAsync(DateTime? createdFromUtc, DateTime? createdToUtc)
    {
        //var query = _queuedEmailRepository.Table;

        //// only sent emails
        //query = query.Where(qe => qe.SentOnUtc.HasValue);

        //if (createdFromUtc.HasValue)
        //    query = query.Where(qe => qe.CreatedOnUtc >= createdFromUtc);
        //if (createdToUtc.HasValue)
        //    query = query.Where(qe => qe.CreatedOnUtc <= createdToUtc);

        //var emails = query.ToArray();

        //await DeleteQueuedEmailsAsync(emails);

        //return emails.Length;
    }

    /// <summary>
    /// Delete all queued emails
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteAllEmailsAsync()
    {
        //await _queuedEmailRepository.TruncateAsync();
    }

    #endregion
}
