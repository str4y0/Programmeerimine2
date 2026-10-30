using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Matches
{
    public class DeleteMatchCommandHandler : IRequestHandler<DeleteMatchCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteMatchCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteMatchCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            // Mängu ennustused kustuvad andmebaasis kaskaadina

            var deleted = await _dbContext.Matches
                .Where(item => item.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deleted == 0)
            {
                return result.AddPropertyError("Id", "Cannot find match with ID " + request.Id);
            }

            return result;
        }
    }
}
