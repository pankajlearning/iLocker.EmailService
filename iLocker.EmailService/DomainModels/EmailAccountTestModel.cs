using System.ComponentModel.DataAnnotations;

namespace iLocker.EmailService.DomainModels;

/// <summary>
/// Represents an test email account model
/// </summary>
public partial record EmailAccountModel
{
    /// <summary>
    /// Gets or sets the entity identifier
    /// </summary>
    public long Id { get; set; }

    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    public string DisplayName { get; set; }

    public string Host { get; set; }

    public int Port { get; set; }

    public string Username { get; set; }

    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool EnableSsl { get; set; }

    public bool UseDefaultCredentials { get; set; }

    public bool IsDefaultEmailAccount { get; set; }

    public string SendTestEmailTo { get; set; }

}