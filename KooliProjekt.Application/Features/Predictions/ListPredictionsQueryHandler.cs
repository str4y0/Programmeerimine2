using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Predictions
{
    public class ListPredictionsQueryHandler : IRequestHandler<ListPredictionsQuery, OperationResult<PagedResult<PredictionListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListPredictionsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<PredictionListItemDto>>> Handle(ListPredictionsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<PredictionListItemDto>>();
            result.Value = await _dbContext.Predictions
                .AsNoTracking()
                .OrderByDescending(prediction => prediction.PredictedAt)
                .ThenBy(prediction => prediction.Id)
                .Select(prediction => new PredictionListItemDto
                {
                    Id = prediction.Id,
                    UserName = prediction.User.Name,
                    TournamentName = prediction.Match.Tournament.Name,
                    Stage = prediction.Match.Stage,
                    HomeTeamName = prediction.Match.HomeTeam.Name,
                    AwayTeamName = prediction.Match.AwayTeam.Name,
                    PredictedHomeGoals = prediction.HomeTeamGoals,
                    PredictedAwayGoals = prediction.AwayTeamGoals,
                    Points = prediction.Points,
                    PredictedAt = prediction.PredictedAt
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
