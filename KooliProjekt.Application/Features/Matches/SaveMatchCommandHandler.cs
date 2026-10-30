using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Matches
{
    public class SaveMatchCommandHandler : IRequestHandler<SaveMatchCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveMatchCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveMatchCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var tournamentExists = await _dbContext.Tournaments
                .AnyAsync(tournament => tournament.Id == request.TournamentId, cancellationToken);
            if (!tournamentExists)
            {
                return result.AddPropertyError("TournamentId", "Cannot find tournament with ID " + request.TournamentId);
            }

            if (request.HomeTeamId.HasValue)
            {
                var homeTeamExists = await _dbContext.Teams
                    .AnyAsync(team => team.Id == request.HomeTeamId.Value, cancellationToken);
                if (!homeTeamExists)
                {
                    return result.AddPropertyError("HomeTeamId", "Cannot find team with ID " + request.HomeTeamId);
                }
            }

            if (request.AwayTeamId.HasValue)
            {
                var awayTeamExists = await _dbContext.Teams
                    .AnyAsync(team => team.Id == request.AwayTeamId.Value, cancellationToken);
                if (!awayTeamExists)
                {
                    return result.AddPropertyError("AwayTeamId", "Cannot find team with ID " + request.AwayTeamId);
                }
            }

            Match match;

            if (request.Id == 0)
            {
                match = new Match();
                _dbContext.Matches.Add(match);
            }
            else
            {
                match = await _dbContext.Matches.FindAsync(new object[] { request.Id }, cancellationToken);
                if (match == null)
                {
                    return result.AddPropertyError("Id", "Cannot find match with ID " + request.Id);
                }
            }

            match.TournamentId = request.TournamentId;
            match.Stage = request.Stage;
            match.StartTime = request.StartTime;
            match.HomeTeamId = request.HomeTeamId;
            match.AwayTeamId = request.AwayTeamId;
            match.HomeTeamGoals = request.HomeTeamGoals;
            match.AwayTeamGoals = request.AwayTeamGoals;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
