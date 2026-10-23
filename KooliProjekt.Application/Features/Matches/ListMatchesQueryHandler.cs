using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Matches
{
    public class ListMatchesQueryHandler : IRequestHandler<ListMatchesQuery, OperationResult<PagedResult<MatchListItemDto>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListMatchesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<MatchListItemDto>>> Handle(ListMatchesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<MatchListItemDto>>();
            result.Value = await _dbContext.Matches
                .AsNoTracking()
                .OrderByDescending(match => match.StartTime)
                .ThenBy(match => match.Id)
                .Select(match => new MatchListItemDto
                {
                    Id = match.Id,
                    TournamentName = match.Tournament.Name,
                    Stage = match.Stage,
                    StartTime = match.StartTime,
                    HomeTeamName = match.HomeTeam.Name,
                    AwayTeamName = match.AwayTeam.Name,
                    HomeTeamGoals = match.HomeTeamGoals,
                    AwayTeamGoals = match.AwayTeamGoals
                })
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
