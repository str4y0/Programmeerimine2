using System.Linq;
using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Teams
{
    public class DeleteTeamCommandValidator : AbstractValidator<DeleteTeamCommand>
    {
        public DeleteTeamCommandValidator(ApplicationDbContext dbContext)
        {
            // Võistkonda ei saa kustutada, kui ta osaleb mõnes mängus
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(1).WithMessage("Id must be greater than or equal to 1.")
                .MustAsync(async (id, cancellationToken) =>
                    !await dbContext.Matches.AnyAsync(m => m.HomeTeamId == id || m.AwayTeamId == id, cancellationToken))
                .WithMessage("Team is used in matches and cannot be deleted");
        }
    }
}
