using iLocker.EmailService.DomainModels;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace iLocker.EmailService.Services.Contracts;

/// <summary>
/// Queued email service
/// </summary>
public interface IQueuedEmailService
{
    /// <summary>
    /// Add a queued email
    /// </summary>
    /// <param name="queuedEmail">Queued email</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task<QueuedEmail> AddQueuedEmailAsync(QueuedEmail queuedEmail);

    /// <summary>
    /// Updates a queued email
    /// </summary>
    /// <param name="queuedEmail">Queued email</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateQueuedEmailAsync(QueuedEmail queuedEmail);

    /// <summary>
    /// Deleted a queued email
    /// </summary>
    /// <param name="queuedEmail">Queued email</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteQueuedEmailAsync(QueuedEmail queuedEmail);

    /// <summary>
    /// Deleted a queued emails
    /// </summary>
    /// <param name="queuedEmails">Queued emails</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteQueuedEmailsAsync(IList<QueuedEmail> queuedEmails);

    /// <summary>
    /// Gets a queued email by identifier
    /// </summary>
    /// <param name="queuedEmailId">Queued email identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the queued email
    /// </returns>
    Task<QueuedEmail> GetQueuedEmailAsync(long queuedEmailId);

    /// <summary>
    /// Get all queued emails
    /// </summary>
    /// <param name="queuedEmailIds">queued email identifiers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the queued emails
    /// </returns>
    Task<IList<QueuedEmail>> GetQueuedEmailsAsync();

    /// <summary>
    /// Deletes already sent emails
    /// </summary>
    /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
    /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the number of deleted emails
    /// </returns>
    Task<int> DeleteAlreadySentEmailsAsync(DateTime? createdFromUtc, DateTime? createdToUtc);

    /// <summary>
    /// Delete all queued emails
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteAllQueuedEmailsAsync();
}
