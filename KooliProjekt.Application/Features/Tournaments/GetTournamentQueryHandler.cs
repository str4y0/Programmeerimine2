using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.Tournaments
{
    public class GetTournamentQueryHandler : IRequestHandler<GetTournamentQuery, Tournament>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetTournamentQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tournament> Handle(GetTournamentQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Tournaments.FindAsync(new object[] { request.Id }, cancellationToken);

            return result;
        }
    }
}
