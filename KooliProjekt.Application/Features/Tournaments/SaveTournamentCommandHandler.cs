using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tournaments
{
    public class SaveTournamentCommandHandler : IRequestHandler<SaveTournamentCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveTournamentCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveTournamentCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            Tournament item;

            if (request.Id == 0)
            {
                item = new Tournament();
                _dbContext.Tournaments.Add(item);
            }
            else
            {
                item = await _dbContext.Tournaments.FindAsync(new object[] { request.Id }, cancellationToken);
                if (item == null)
                {
                    return result.AddPropertyError("Id", "Cannot find tournament with ID " + request.Id);
                }
            }

            item.Name = request.Name;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
