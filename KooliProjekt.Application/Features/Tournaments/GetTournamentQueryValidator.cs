using FluentValidation;

namespace KooliProjekt.Application.Features.Tournaments
{
    public class GetTournamentQueryValidator : AbstractValidator<GetTournamentQuery>
    {
        public GetTournamentQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThanOrEqualTo(1).WithMessage("Id must be greater than or equal to 1.");
        }
    }
}
