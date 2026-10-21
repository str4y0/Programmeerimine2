using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;

namespace KooliProjekt.Application.Features.Matches
{
    public class GetMatchQueryHandler : IRequestHandler<GetMatchQuery, Match>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetMatchQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Match> Handle(GetMatchQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Matches.FindAsync(new object[] { request.Id }, cancellationToken);

            return result;
        }
    }
}
