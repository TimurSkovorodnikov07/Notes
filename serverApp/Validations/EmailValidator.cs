using FluentValidation;

public class EmailValidator : AbstractValidator<string>
{
    public EmailValidator()
    {
        RuleFor(x => x).NotEmpty().EmailAddress().WithMessage("Email not valid");
    }
}

public static class EmailStaticValidator
{
    public static bool IsValid(string email)
    {
        return new EmailValidator().Validate(email).IsValid;
    }
}