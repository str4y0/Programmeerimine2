using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Teams
{
    public class ListTeamsQueryHandler : IRequestHandler<ListTeamsQuery, IEnumerable<Team>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListTeamsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Team>> Handle(ListTeamsQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Teams.ToListAsync(cancellationToken);

            return result;
        }
    }
}
