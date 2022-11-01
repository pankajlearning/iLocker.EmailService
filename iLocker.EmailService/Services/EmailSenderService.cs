using iLocker.EmailService.DomainModels;
using iLocker.EmailService.Services.Contracts;
using MimeKit.Text;
using MimeKit;

namespace iLocker.EmailService.Services;

/// <summary>
/// Email sender service
/// </summary>
public class EmailSenderService : IEmailSenderService
{
    #region Fields

    private readonly ISmtpBuilderService _smtpBuilderService;

    #endregion

    #region Ctor

    public EmailSenderService(ISmtpBuilderService smtpBuilderService)
    {
        _smtpBuilderService = smtpBuilderService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Sends an email
    /// </summary>
    /// <param name="emailAccount">Email account to use</param>
    /// <param name="subject">Subject</param>
    /// <param name="body">Body</param>
    /// <param name="fromAddress">From address</param>
    /// <param name="fromName">From display name</param>
    /// <param name="toAddress">To address</param>
    /// <param name="toName">To display name</param>
    /// <param name="replyTo">ReplyTo address</param>
    /// <param name="replyToName">ReplyTo display name</param>
    /// <param name="bcc">BCC addresses list</param>
    /// <param name="cc">CC addresses list</param>
    /// <param name="attachmentFilePath">Attachment file path</param>
    /// <param name="attachmentFileName">Attachment file name. If specified, then this file name will be sent to a recipient. Otherwise, "AttachmentFilePath" name will be used.</param>
    /// <param name="attachedDownloadId">Attachment download ID (another attachment)</param>
    /// <param name="headers">Headers</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task SendEmailAsync(EmailAccount emailAccount, string subject, string body,
        string fromAddress, string fromName, string toAddress, string toName,
        string? replyTo = null, string? replyToName = null,
        IEnumerable<string>? bcc = null, IEnumerable<string>? cc = null,
        string? attachmentFilePath = null, string? attachmentFileName = null,
        int attachedDownloadId = 0, IDictionary<string, string>? headers = null)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(fromName, fromAddress));
        message.To.Add(new MailboxAddress(toName, toAddress));

        if (!string.IsNullOrEmpty(replyTo))
        {
            message.ReplyTo.Add(new MailboxAddress(replyToName, replyTo));
        }

        //BCC
        if (bcc != null)
        {
            foreach (var address in bcc.Where(bccValue => !string.IsNullOrWhiteSpace(bccValue)))
            {
                message.Bcc.Add(new MailboxAddress("", address.Trim()));
            }
        }

        //CC
        if (cc != null)
        {
            foreach (var address in cc.Where(ccValue => !string.IsNullOrWhiteSpace(ccValue)))
            {
                message.Cc.Add(new MailboxAddress("", address.Trim()));
            }
        }

        //content
        message.Subject = subject;

        //headers
        if (headers != null)
            foreach (var header in headers)
            {
                message.Headers.Add(header.Key, header.Value);
            }

        var multipart = new Multipart("mixed")
            {
                new TextPart(TextFormat.Html) { Text = body }
            };

        //Todo add attachements

        message.Body = multipart;

        //send email
        using var smtpClient = await _smtpBuilderService.BuildAsync(emailAccount);
        await smtpClient.SendAsync(message);
        await smtpClient.DisconnectAsync(true);
    }

    #endregion
}
