using iLocker.EmailService.DomainModels;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace iLocker.EmailService.Services.Contracts;

/// <summary>
/// Email account service
/// </summary>
public interface IEmailAccountService
{
    /// <summary>
    /// Add an email account
    /// </summary>
    /// <param name="emailAccount">Email account</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task AddEmailAccountAsync(EmailAccount emailAccount);

    /// <summary>
    /// Updates an email account
    /// </summary>
    /// <param name="emailAccount">Email account</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateEmailAccountAsync(EmailAccount emailAccount);

    /// <summary>
    /// Deletes an email account
    /// </summary>
    /// <param name="emailAccount">Email account</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteEmailAccountAsync(EmailAccount emailAccount);

    /// <summary>
    /// Gets an email account by identifier
    /// </summary>
    /// <param name="emailAccountId">The email account identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the email account
    /// </returns>
    Task<EmailAccount> GetEmailAccountAsync(long emailAccountId);

    /// <summary>
    /// Gets all email accounts
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the email accounts list
    /// </returns>
    Task<IList<EmailAccount>> GetEmailAccountsAsync();

    /// <summary>
    /// Gets default email account 
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the email account
    /// </returns>
    Task<EmailAccount> GetDefaultEmailAccountAsync();
}
