namespace Intergalaxy.Application.CharacterRequests.Commands.UpdateCharacterRequestStatus;

public class UpdateCharacterRequestStatusCommandValidator : AbstractValidator<UpdateCharacterRequestStatusCommand>
{
    public UpdateCharacterRequestStatusCommandValidator()
    {
        RuleFor(x => x.RequestId)
          .GreaterThan(0);

        RuleFor(x => x.NewStatus)
            .IsInEnum();
    }
}
