using iLocker.EmailService.Helper;
using iLocker.EmailService.DomainModels;
using iLocker.EmailService.Services.Contracts;
using iLocker.EmailService.Data;
using QueuedEmail = iLocker.EmailService.DomainModels.QueuedEmail;
using Entities = iLocker.EmailService.Data.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using iLocker.EmailService.Data.Entities;

namespace iLocker.EmailService.Services;

/// <summary>
/// Queued email service
/// </summary>
public class QueuedEmailService : IQueuedEmailService
{
    #region Fields
    private readonly EmailServiceDataContext _emailServiceDataContext;
    private readonly IMapper _mapper;

    #endregion

    #region Ctor

    public QueuedEmailService(EmailServiceDataContext emailServiceDataContext,
        Mapper mapper)
    {
        _emailServiceDataContext = emailServiceDataContext;
        _mapper = mapper;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Add a queued email
    /// </summary>
    /// <param name="queuedEmail">Queued email</param>        
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task<QueuedEmail> AddQueuedEmailAsync(QueuedEmail queuedEmail)
    {
        if (queuedEmail == null)
            throw new ArgumentNullException(nameof(queuedEmail));
        
        var queuedEmailEntity = _mapper.Map<Entities.QueuedEmail>(queuedEmail);
        await _emailServiceDataContext.QueuedEmails.AddAsync(queuedEmailEntity);
        await _emailServiceDataContext.SaveChangesAsync();
        return await GetQueuedEmailAsync(queuedEmailEntity.Id);
    }

    /// <summary>
    /// Updates a queued email
    /// </summary>
    /// <param name="queuedEmail">Queued email</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdateQueuedEmailAsync(QueuedEmail queuedEmail)
    {
        if (queuedEmail == null)
            throw new ArgumentNullException(nameof(queuedEmail));

        var queuedEmailEntity = _mapper.Map<Entities.QueuedEmail>(queuedEmail);
        await _emailServiceDataContext.QueuedEmails.AddAsync(queuedEmailEntity);
        await _emailServiceDataContext.SaveChangesAsync();
    }

    /// <summary>
    /// Deleted a queued email
    /// </summary>
    /// <param name="queuedEmail">Queued email</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteQueuedEmailAsync(QueuedEmail queuedEmail)
    {
        if (queuedEmail == null)
            throw new ArgumentNullException(nameof(queuedEmail));

        var queuedEmailEntity = _mapper.Map<Entities.QueuedEmail>(queuedEmail);
        _emailServiceDataContext.QueuedEmails.Remove(queuedEmailEntity);
        await _emailServiceDataContext.SaveChangesAsync();
    }

    /// <summary>
    /// Deleted a queued emails
    /// </summary>
    /// <param name="queuedEmails">Queued emails</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteQueuedEmailsAsync(IList<QueuedEmail> queuedEmails)
    {
        if (queuedEmails == null)
            throw new ArgumentNullException(nameof(queuedEmails));

        var queuedEmailEntites = _mapper.Map<IList<Entities.QueuedEmail>>(queuedEmails);
        _emailServiceDataContext.QueuedEmails.RemoveRange(queuedEmailEntites);
        await _emailServiceDataContext.SaveChangesAsync();
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
        var queuedEmailEntity = await _emailServiceDataContext.QueuedEmails.SingleOrDefaultAsync(a => a.Id == queuedEmailId);
        return _mapper.Map<QueuedEmail>(queuedEmailEntity);
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
        var queuedEmails = await _emailServiceDataContext.QueuedEmails.ToListAsync();
        return _mapper.Map<IList<QueuedEmail>>(queuedEmails);
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
        var query =  _emailServiceDataContext.QueuedEmails.AsQueryable();

        //// only sent emails
        query = query.Where(qe => qe.SentOnUtc.HasValue);

        if (createdFromUtc.HasValue)
            query = query.Where(qe => qe.CreatedOnUtc >= createdFromUtc);
        if (createdToUtc.HasValue)
            query = query.Where(qe => qe.CreatedOnUtc <= createdToUtc);

        var emails = await query.ToListAsync() ;
        var emailsCount = emails.Count() ;
        await DeleteQueuedEmailsAsync(_mapper.Map<IList<QueuedEmail>>(emails));
        return emailsCount;
    }

    /// <summary>
    /// Delete all queued emails
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteAllQueuedEmailsAsync()
    {
        var queuedEmailEntites = await  _emailServiceDataContext.QueuedEmails.ToListAsync();
        _emailServiceDataContext.QueuedEmails.RemoveRange(queuedEmailEntites);
        await _emailServiceDataContext.SaveChangesAsync();
    }

    #endregion
}
