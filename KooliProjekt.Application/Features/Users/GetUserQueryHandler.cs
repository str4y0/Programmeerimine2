using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, OperationResult<UserDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetUserQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserDto>();

            // NB! Teadlikult ei võeta kaasa PasswordHash'i ega e-posti
            result.Value = await _dbContext.Users
                .AsNoTracking()
                .Where(user => user.Id == request.Id)
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Role = user.Role
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (result.Value == null)
            {
                return result;
            }

            // Kasutaja ennustused eraldi päringuga
            result.Value.Predictions = await _dbContext.Predictions
                .AsNoTracking()
                .Where(prediction => prediction.UserId == request.Id)
                .OrderByDescending(prediction => prediction.PredictedAt)
                .Select(prediction => new UserPredictionDto
                {
                    Id = prediction.Id,
                    MatchId = prediction.MatchId,
                    TournamentName = prediction.Match.Tournament.Name,
                    Stage = prediction.Match.Stage,
                    HomeTeamName = prediction.Match.HomeTeam.Name,
                    AwayTeamName = prediction.Match.AwayTeam.Name,
                    PredictedHomeGoals = prediction.HomeTeamGoals,
                    PredictedAwayGoals = prediction.AwayTeamGoals,
                    Points = prediction.Points,
                    PredictedAt = prediction.PredictedAt
                })
                .ToListAsync(cancellationToken);

            result.Value.TotalPoints = result.Value.Predictions.Sum(prediction => prediction.Points ?? 0);

            return result;
        }
    }
}
