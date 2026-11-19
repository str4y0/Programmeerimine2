using System;
using System.Linq;
using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Matches
{
    public class SaveMatchCommandValidator : AbstractValidator<SaveMatchCommand>
    {
        public SaveMatchCommandValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .GreaterThanOrEqualTo(0).WithMessage("Id must be greater than or equal to 0.");

            // Igal mängul peab olema turniir ...
            RuleFor(x => x.TournamentId)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(1).WithMessage("TournamentId must be greater than or equal to 1.")
                .MustAsync(async (id, cancellationToken) =>
                    await dbContext.Tournaments.AnyAsync(t => t.Id == id, cancellationToken))
                .WithMessage(x => "Cannot find tournament with ID " + x.TournamentId);

            // ... etapp (üks neljast) ...
            RuleFor(x => x.Stage)
                .IsInEnum().WithMessage("Stage must be one of: RoundOf16, QuarterFinal, SemiFinal, Final.");

            // ... ja alguse aeg
            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("StartTime is required.");

            // Võistkond võib olla teadmata (null). Kui on antud, peab see olemas olema.
            RuleFor(x => x.HomeTeamId)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(1).WithMessage("HomeTeamId must be greater than or equal to 1.")
                .MustAsync(async (id, cancellationToken) =>
                    await dbContext.Teams.AnyAsync(t => t.Id == id.Value, cancellationToken))
                .WithMessage(x => "Cannot find team with ID " + x.HomeTeamId)
                .When(x => x.HomeTeamId.HasValue);

            RuleFor(x => x.AwayTeamId)
                .Cascade(CascadeMode.Stop)
                .GreaterThanOrEqualTo(1).WithMessage("AwayTeamId must be greater than or equal to 1.")
                .MustAsync(async (id, cancellationToken) =>
                    await dbContext.Teams.AnyAsync(t => t.Id == id.Value, cancellationToken))
                .WithMessage(x => "Cannot find team with ID " + x.AwayTeamId)
                .When(x => x.AwayTeamId.HasValue);

            // Koduvõistkond ja külalisvõistkond ei tohi olla sama võistkond
            RuleFor(x => x.AwayTeamId)
                .NotEqual(x => x.HomeTeamId).WithMessage("Home team and away team must be different teams.")
                .When(x => x.HomeTeamId.HasValue && x.AwayTeamId.HasValue);

            // Väravad on täisarvud, vähemalt 0
            RuleFor(x => x.HomeTeamGoals)
                .GreaterThanOrEqualTo(0).WithMessage("HomeTeamGoals must be greater than or equal to 0.");

            RuleFor(x => x.AwayTeamGoals)
                .GreaterThanOrEqualTo(0).WithMessage("AwayTeamGoals must be greater than or equal to 0.");

            // Tulemuse mõlemad väärtused sisestatakse korraga
            RuleFor(x => x.HomeTeamGoals)
                .NotNull().WithMessage("HomeTeamGoals is required when AwayTeamGoals is entered.")
                .When(x => x.AwayTeamGoals.HasValue);

            RuleFor(x => x.AwayTeamGoals)
                .NotNull().WithMessage("AwayTeamGoals is required when HomeTeamGoals is entered.")
                .When(x => x.HomeTeamGoals.HasValue);

            // Tulemuse saab sisestada alles pärast mängu algust ja ainult siis, kui mõlemad võistkonnad on teada
            RuleFor(x => x.HomeTeamGoals)
                .Cascade(CascadeMode.Stop)
                .Must((command, goals) => command.StartTime <= DateTime.UtcNow)
                .WithMessage("The result can be entered only after the match has started.")
                .Must((command, goals) => command.HomeTeamId.HasValue && command.AwayTeamId.HasValue)
                .WithMessage("The result can be entered only when both teams are known.")
                .When(x => x.HomeTeamGoals.HasValue || x.AwayTeamGoals.HasValue);
        }
    }
}
