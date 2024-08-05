using FluentValidation;

public static class NoteStaticValidator
{
    public static bool IsValid(NoteEntity note)
    {
        return new NoteValidator().Validate(note).IsValid;
    }
}

public class NoteValidator : AbstractValidator<NoteEntity>
{
    public NoteValidator()
    {
        RuleFor(x => x.Id).NotNull().WithMessage("Id is null");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is empty");
        RuleFor(x => x.Discription).NotEmpty().WithMessage("Discription not valid");
        RuleFor(x => x.TimeOfCreation).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty()
                    .Must(date => date != default(DateTime))
                    .WithMessage("Start date is required");
    }
}
