using System.ComponentModel.DataAnnotations;

namespace iLocker.EmailService.DomainModels;

/// <summary>
/// Represents an email account
/// </summary>
public partial class EmailAccount
{
    /// <summary>
    /// Gets or sets the entity identifier
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets an email address
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets an email display name
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// Gets or sets an email host
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Gets or sets an email port
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Gets or sets an email user name
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets an email password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets a value that controls whether the SmtpClient uses Secure Sockets Layer (SSL) to encrypt the connection
    /// </summary>
    public bool EnableSsl { get; set; }

    /// <summary>
    /// Gets or sets a value that controls whether the default system credentials of the application are sent with requests.
    /// </summary>
    public bool UseDefaultCredentials { get; set; }

    /// <summary>
    /// Gets or sets a value that controls whether the email account is default or not
    /// </summary>
    public bool Default { get; set; }

    /// <summary>
    /// Gets or sets a value that controls whether the email account is enabled or not
    /// </summary>
    public bool Enabled { get; set; }
}
