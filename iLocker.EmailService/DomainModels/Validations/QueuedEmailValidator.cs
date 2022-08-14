
using FluentValidation;
using iLocker.EmailService.DomainModels;
namespace iLocker.EmailService.DomainModels.Validators;
public class QueuedEmailValidator : AbstractValidator<QueuedEmail> 
{
    public QueuedEmailValidator()
    {
        RuleFor(queuedEmail => queuedEmail.From).NotEmpty().WithMessage("From Required")
        .EmailAddress().WithMessage("Invalid Email")
        .MaximumLength(255).WithMessage("Email must be less than 255 characters");

        RuleFor(queuedEmail => queuedEmail.To).NotEmpty().WithMessage("To Required")
        .MaximumLength(255).WithMessage("To must be less than 255 characters");

        RuleFor(queuedEmail => queuedEmail.Subject).NotEmpty().WithMessage("Subject Required");
        
        RuleFor(queuedEmail => queuedEmail.Body).NotEmpty().WithMessage("Body Required");
        
    }
}