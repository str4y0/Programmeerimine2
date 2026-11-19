using System.Linq;
using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tournaments
{
    public class DeleteTournamentCommandValidator : AbstractValidator<DeleteTournamentCommand>
    {
        public DeleteTournamentCommandValidator(ApplicationDbContext dbContext)
        {
            // Turniiri ei saa kustutada, kui selle mängudele on ennustusi tehtud
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(1).WithMessage("Id must be greater than or equal to 1.")
                .MustAsync(async (id, cancellationToken) =>
                    !await dbContext.Predictions.AnyAsync(p => p.Match.TournamentId == id, cancellationToken))
                .WithMessage("Tournament has predictions and cannot be deleted");
        }
    }
}
