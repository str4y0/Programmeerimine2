using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.Teams
{
    public class GetTeamQueryHandler : IRequestHandler<GetTeamQuery, Team>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetTeamQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Team> Handle(GetTeamQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Teams.FindAsync(new object[] { request.Id }, cancellationToken);

            return result;
        }
    }
}
