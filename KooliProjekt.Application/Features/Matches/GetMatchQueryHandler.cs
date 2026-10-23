using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Matches
{
    public class GetMatchQueryHandler : IRequestHandler<GetMatchQuery, OperationResult<MatchDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetMatchQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<MatchDto>> Handle(GetMatchQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<MatchDto>();
            result.Value = await _dbContext.Matches
                .AsNoTracking()
                .Where(match => match.Id == request.Id)
                .Select(match => new MatchDto
                {
                    Id = match.Id,
                    TournamentId = match.TournamentId,
                    TournamentName = match.Tournament.Name,
                    Stage = match.Stage,
                    StartTime = match.StartTime,
                    HomeTeamId = match.HomeTeamId,
                    HomeTeamName = match.HomeTeam.Name,
                    AwayTeamId = match.AwayTeamId,
                    AwayTeamName = match.AwayTeam.Name,
                    HomeTeamGoals = match.HomeTeamGoals,
                    AwayTeamGoals = match.AwayTeamGoals,
                    PredictionCount = _dbContext.Predictions.Count(prediction => prediction.MatchId == match.Id)
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
