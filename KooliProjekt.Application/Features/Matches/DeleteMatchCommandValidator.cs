using System.Linq;
using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Matches
{
    public class DeleteMatchCommandValidator : AbstractValidator<DeleteMatchCommand>
    {
        public DeleteMatchCommandValidator(ApplicationDbContext dbContext)
        {
            // Mängu ei saa kustutada, kui sellele on ennustusi tehtud
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(1).WithMessage("Id must be greater than or equal to 1.")
                .MustAsync(async (id, cancellationToken) =>
                    !await dbContext.Predictions.AnyAsync(p => p.MatchId == id, cancellationToken))
                .WithMessage("Match has predictions and cannot be deleted");
        }
    }
}
