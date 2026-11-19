using FluentValidation;

namespace KooliProjekt.Application.Features.Predictions
{
    public class GetPredictionQueryValidator : AbstractValidator<GetPredictionQuery>
    {
        public GetPredictionQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThanOrEqualTo(1).WithMessage("Id must be greater than or equal to 1.");
        }
    }
}
