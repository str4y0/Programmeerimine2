using System;
using System.Linq;
using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Predictions
{
    public class SavePredictionCommandValidator : AbstractValidator<SavePredictionCommand>
    {
        public SavePredictionCommandValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0.");

            RuleFor(x => x.UserId)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(1).WithMessage("UserId must be greater than or equal to 1.")
                .MustAsync(async (id, cancellationToken) =>
                    await dbContext.Users.AnyAsync(u => u.Id == id, cancellationToken))
                .WithMessage(x => "Cannot find user with ID " + x.UserId);

            RuleFor(x => x.MatchId)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(1).WithMessage("MatchId must be greater than or equal to 1.")
                .MustAsync(async (id, cancellationToken) =>
                    await dbContext.Matches.AnyAsync(m => m.Id == id, cancellationToken))
                .WithMessage(x => "Cannot find match with ID " + x.MatchId)
                // Ennustust saab teha ainult mängule, milles mõlemad võistkonnad on teada
                .MustAsync(async (id, cancellationToken) =>
                    await dbContext.Matches.AnyAsync(m => m.Id == id && m.HomeTeamId != null && m.AwayTeamId != null, cancellationToken))
                .WithMessage("Predictions can be made only for matches where both teams are known.")
                // Ennustust saab teha (ja muuta) kuni mängu alguseni
                .MustAsync(async (id, cancellationToken) =>
                {
                    var now = DateTime.UtcNow;
                    return await dbContext.Matches.AnyAsync(m => m.Id == id && m.StartTime > now, cancellationToken);
                })
                .WithMessage("Predictions can be made only until the match starts.")
                // Kasutaja saab ühe mängu kohta teha ainult ühe ennustuse
                .MustAsync(async (command, matchId, cancellationToken) =>
                    !await dbContext.Predictions.AnyAsync(
                        p => p.UserId == command.UserId && p.MatchId == matchId && p.Id != command.Id,
                        cancellationToken))
                .WithMessage("The user has already made a prediction for this match.");

            // Väravad on täisarvud, vähemalt 0
            RuleFor(x => x.HomeTeamGoals)
                .GreaterThanOrEqualTo(0).WithMessage("HomeTeamGoals must be greater than or equal to 0.");

            RuleFor(x => x.AwayTeamGoals)
                .GreaterThanOrEqualTo(0).WithMessage("AwayTeamGoals must be greater than or equal to 0.");
        }
    }
}
