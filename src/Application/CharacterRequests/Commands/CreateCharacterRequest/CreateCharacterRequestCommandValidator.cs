using Intergalaxy.Application.CharacterRequests.Commands.CreateCharacterRequest;

public class CreateCharacterRequestCommandValidator
    : AbstractValidator<CreateCharacterRequestCommand>
{
    public CreateCharacterRequestCommandValidator()
    {
        RuleFor(x => x.CharacterId)
            .GreaterThan(0);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Requester)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.EventName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.EventDate)
            .Must(BeInFuture)
            .WithMessage("EventDate must be in the future");
    }

    private bool BeInFuture(DateTime date)
    {
        return date > DateTime.UtcNow;
    }
}
