using iLocker.EmailService.Helper;
using iLocker.EmailService.DomainModels;
using iLocker.EmailService.Services.Contracts;
using iLocker.EmailService.Data;
using EmailAccount = iLocker.EmailService.DomainModels.EmailAccount;
using Entities = iLocker.EmailService.Data.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using iLocker.EmailService.Data.Entities;

namespace iLocker.EmailService.Services;

/// <summary>
/// Email account service
/// </summary>
public class EmailAccountService : IEmailAccountService
{
    #region Fields

    private readonly EmailServiceDataContext _emailServiceDataContext;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public EmailAccountService(EmailServiceDataContext emailServiceDataContext,
        IMapper mapper)
    {
        _emailServiceDataContext = emailServiceDataContext;
        _mapper = mapper;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Add an email account
    /// </summary>
    /// <param name="emailAccount">Email account</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task<EmailAccount> AddEmailAccountAsync(EmailAccount emailAccount)
    {
        if (emailAccount == null)
            throw new ArgumentNullException(nameof(emailAccount));

        emailAccount.Email = CommonHelper.EnsureNotNull(emailAccount.Email);
        emailAccount.DisplayName = CommonHelper.EnsureNotNull(emailAccount.DisplayName);
        emailAccount.Host = CommonHelper.EnsureNotNull(emailAccount.Host);
        emailAccount.Username = CommonHelper.EnsureNotNull(emailAccount.Username);
        emailAccount.Password = CommonHelper.EnsureNotNull(emailAccount.Password);

        emailAccount.Email = emailAccount.Email.Trim();
        emailAccount.DisplayName = emailAccount.DisplayName.Trim();
        emailAccount.Host = emailAccount.Host.Trim();
        emailAccount.Username = emailAccount.Username.Trim();
        emailAccount.Password = emailAccount.Password.Trim();
        emailAccount.Port = emailAccount.Port;
        emailAccount.UseDefaultCredentials = emailAccount.UseDefaultCredentials;
        emailAccount.EnableSsl = emailAccount.EnableSsl;
        emailAccount.Default = emailAccount.Default;
        emailAccount.Enabled = emailAccount.Enabled;

        emailAccount.Email = CommonHelper.EnsureMaximumLength(emailAccount.Email, 255);
        emailAccount.DisplayName = CommonHelper.EnsureMaximumLength(emailAccount.DisplayName, 255);
        emailAccount.Host = CommonHelper.EnsureMaximumLength(emailAccount.Host, 255);
        emailAccount.Username = CommonHelper.EnsureMaximumLength(emailAccount.Username, 255);
        emailAccount.Password = CommonHelper.EnsureMaximumLength(emailAccount.Password, 255);

        var emailAccountEntity = _mapper.Map<Entities.EmailAccount>(emailAccount);

        await _emailServiceDataContext.EmailAccounts.AddAsync(emailAccountEntity);
        await _emailServiceDataContext.SaveChangesAsync();
        return await GetEmailAccountAsync(emailAccountEntity.Id);
    }

    /// <summary>
    /// Updates an email account
    /// </summary>
    /// <param name="emailAccount">Email account</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdateEmailAccountAsync(EmailAccount emailAccount)
    {
        if (emailAccount == null)
            throw new ArgumentNullException(nameof(emailAccount));

        emailAccount.Email = CommonHelper.EnsureNotNull(emailAccount.Email);
        emailAccount.DisplayName = CommonHelper.EnsureNotNull(emailAccount.DisplayName);
        emailAccount.Host = CommonHelper.EnsureNotNull(emailAccount.Host);
        emailAccount.Username = CommonHelper.EnsureNotNull(emailAccount.Username);
        emailAccount.Password = CommonHelper.EnsureNotNull(emailAccount.Password);

        emailAccount.Email = emailAccount.Email.Trim();
        emailAccount.DisplayName = emailAccount.DisplayName.Trim();
        emailAccount.Host = emailAccount.Host.Trim();
        emailAccount.Username = emailAccount.Username.Trim();
        emailAccount.Password = emailAccount.Password.Trim();
        emailAccount.Port = emailAccount.Port;
        emailAccount.UseDefaultCredentials = emailAccount.UseDefaultCredentials;
        emailAccount.EnableSsl = emailAccount.EnableSsl;
        emailAccount.Default = emailAccount.Default;
        emailAccount.Enabled = emailAccount.Enabled;

        emailAccount.Email = CommonHelper.EnsureMaximumLength(emailAccount.Email, 255);
        emailAccount.DisplayName = CommonHelper.EnsureMaximumLength(emailAccount.DisplayName, 255);
        emailAccount.Host = CommonHelper.EnsureMaximumLength(emailAccount.Host, 255);
        emailAccount.Username = CommonHelper.EnsureMaximumLength(emailAccount.Username, 255);
        emailAccount.Password = CommonHelper.EnsureMaximumLength(emailAccount.Password, 255);
        emailAccount.Default = emailAccount.Default;
        emailAccount.Enabled = emailAccount.Enabled;

        var emailAccountEntity = _mapper.Map<Entities.EmailAccount>(emailAccount);
        _emailServiceDataContext.EmailAccounts.Update(emailAccountEntity);
        await _emailServiceDataContext.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an email account
    /// </summary>
    /// <param name="emailAccount">Email account</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteEmailAccountAsync(EmailAccount emailAccount)
    {
        if (emailAccount == null)
            throw new ArgumentNullException(nameof(emailAccount));

        if ((await GetEmailAccountsAsync()).Count == 1)
            throw new Exception("You cannot delete this email account. At least one account is required.");

        var emailAccountEntity = _mapper.Map<Entities.EmailAccount>(emailAccount);
        _emailServiceDataContext.EmailAccounts.Remove(emailAccountEntity);
        await _emailServiceDataContext.SaveChangesAsync();
    }

    /// <summary>
    /// Gets an email account by identifier
    /// </summary>
    /// <param name="emailAccountId">The email account identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the email account
    /// </returns>
    public virtual async Task<EmailAccount> GetEmailAccountAsync(long emailAccountId)
    {
        var emailAccountEntity = await _emailServiceDataContext.EmailAccounts.SingleOrDefaultAsync(a => a.Id == emailAccountId);
        return _mapper.Map<EmailAccount>(emailAccountEntity);
    }

    /// <summary>
    /// Gets all email accounts
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the email accounts list
    /// </returns>
    public virtual async Task<IList<EmailAccount>> GetEmailAccountsAsync()
    {
        var emailAccounts = await _emailServiceDataContext.EmailAccounts.ToListAsync();
        return _mapper.Map<List<EmailAccount>>(emailAccounts);
    }

    /// <summary>
    /// Gets default email account 
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the email accounts list
    /// </returns>
    public async Task<EmailAccount> GetDefaultEmailAccountAsync()
    {
        var emailAccountEntity = await _emailServiceDataContext.EmailAccounts.FirstAsync(a => a.Default == true);
        return _mapper.Map<EmailAccount>(emailAccountEntity);
    }


    /// <summary>
    /// set default email account 
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// <returns>A task that represents the asynchronous operation</returns>
    /// </returns>
    public async Task SetDefaultEmailAccountAsync(EmailAccount emailAccount)
    {
        emailAccount.Default = true;
        var existingDefaultAccount =await GetDefaultEmailAccountAsync();
        if (existingDefaultAccount != null)
        {
            existingDefaultAccount.Default = false;
            _emailServiceDataContext.EmailAccounts.Update(_mapper.Map<Entities.EmailAccount>(existingDefaultAccount));
            await _emailServiceDataContext.SaveChangesAsync();
        }
        var emailAccountEntity = _mapper.Map<Entities.EmailAccount>(emailAccount);
        _emailServiceDataContext.EmailAccounts.Update(emailAccountEntity);
        await _emailServiceDataContext.SaveChangesAsync();
    }

    #endregion
}
