using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Teams
{
    public class GetTeamQueryHandler : IRequestHandler<GetTeamQuery, OperationResult<TeamDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetTeamQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<TeamDto>> Handle(GetTeamQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<TeamDto>();
            result.Value = await _dbContext.Teams
                .AsNoTracking()
                .Where(team => team.Id == request.Id)
                .Select(team => new TeamDto
                {
                    Id = team.Id,
                    Name = team.Name
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
