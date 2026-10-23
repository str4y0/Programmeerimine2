using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Predictions
{
    public class GetPredictionQueryHandler : IRequestHandler<GetPredictionQuery, OperationResult<PredictionDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetPredictionQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PredictionDto>> Handle(GetPredictionQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PredictionDto>();
            result.Value = await _dbContext.Predictions
                .AsNoTracking()
                .Where(prediction => prediction.Id == request.Id)
                .Select(prediction => new PredictionDto
                {
                    Id = prediction.Id,
                    UserId = prediction.UserId,
                    UserName = prediction.User.Name,
                    MatchId = prediction.MatchId,
                    TournamentName = prediction.Match.Tournament.Name,
                    Stage = prediction.Match.Stage,
                    MatchStartTime = prediction.Match.StartTime,
                    HomeTeamName = prediction.Match.HomeTeam.Name,
                    AwayTeamName = prediction.Match.AwayTeam.Name,
                    PredictedHomeGoals = prediction.HomeTeamGoals,
                    PredictedAwayGoals = prediction.AwayTeamGoals,
                    PredictedAt = prediction.PredictedAt,
                    ActualHomeGoals = prediction.Match.HomeTeamGoals,
                    ActualAwayGoals = prediction.Match.AwayTeamGoals,
                    Points = prediction.Points
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
