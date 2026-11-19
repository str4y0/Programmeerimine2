using FluentValidation;

namespace KooliProjekt.Application.Features.Predictions
{
    public class DeletePredictionCommandValidator : AbstractValidator<DeletePredictionCommand>
    {
        public DeletePredictionCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThanOrEqualTo(1).WithMessage("Id must be greater than or equal to 1.");
        }
    }
}
