namespace Intergalaxy.Application.Characters.Commands.ImportCharacter;

public class ImportCharacterCommandValidator : AbstractValidator<ImportCharacterCommand>
{
    public ImportCharacterCommandValidator()
    {
        RuleFor(x => x)
           .Must(HasAtLeastOneField)
           .WithMessage("At least one field must be provided.");

        RuleFor(p => p.ExternalId)
            .Must(id => id > 0)
            .WithMessage("ExternalId must be a positive integer if provided.")
            .When(p => p.ExternalId.HasValue);


        RuleFor(p => p.Page)
           .Must(id => id >= 0)
           .WithMessage("Page must be a positive integer if provided.")
           .When(p => p.Page.HasValue);


    }

    private bool HasAtLeastOneField(ImportCharacterCommand command)
    {
        return !(command.ExternalId == null && command.Page == null);
    }
}
