
using FluentValidation;
using iLocker.EmailService.DomainModels;
namespace iLocker.EmailService.DomainModels.Validators;
public class EmailAccountValidator : AbstractValidator<EmailAccount> 
{
    public EmailAccountValidator()
    {
        RuleFor(emailAccount => emailAccount.Email).NotEmpty().WithMessage("Email Required")
        .EmailAddress().WithMessage("Invalid Email")
        .MaximumLength(255).WithMessage("Email must be less than 255 characters");

        RuleFor(emailAccount => emailAccount.Host).NotEmpty().WithMessage("Host Required")
        .MaximumLength(255).WithMessage("Host must be less than 255 characters");

        RuleFor(emailAccount => emailAccount.Port).GreaterThan(0).WithMessage("Port Required");
        
        RuleFor(emailAccount => emailAccount.Username).NotEmpty().WithMessage("Username Required")
        .MaximumLength(255).WithMessage("Username must be less than 255 characters");

        RuleFor(emailAccount => emailAccount.Password).NotEmpty().WithMessage("Password Required")
        .MaximumLength(255).WithMessage("Password must be less than 255 characters");
    }
}