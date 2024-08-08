using FluentValidation;

public static class UserStaticValidator
{
    public static bool IsValid(UserEntity user)
    {
        return new UserValidator().Validate(user).IsValid;
    }
}

public class UserValidator : AbstractValidator<UserEntity>
{
    public UserValidator()
    {
        RuleFor(x => x.Id).NotNull().WithMessage("Id is null");
        RuleFor(x => x.Name).MaximumLength(24).NotEmpty().WithMessage("Name is empty");
        RuleFor(x => x.Email).MaximumLength(45).NotEmpty().EmailAddress().WithMessage("Email not valid");
        RuleFor(x => x.PasswordHash).NotEmpty().WithMessage("PasswordHash is empty");
    }
}