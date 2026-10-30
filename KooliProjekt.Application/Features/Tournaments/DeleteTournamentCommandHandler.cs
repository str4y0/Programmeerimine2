using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tournaments
{
    public class DeleteTournamentCommandHandler : IRequestHandler<DeleteTournamentCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteTournamentCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteTournamentCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            // Turniiri mängud ja mängude ennustused kustuvad andmebaasis kaskaadina

            var deleted = await _dbContext.Tournaments
                .Where(item => item.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deleted == 0)
            {
                return result.AddPropertyError("Id", "Cannot find tournament with ID " + request.Id);
            }

            return result;
        }
    }
}
