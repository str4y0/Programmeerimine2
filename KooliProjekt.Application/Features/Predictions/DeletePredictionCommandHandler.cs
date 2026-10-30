using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Predictions
{
    public class DeletePredictionCommandHandler : IRequestHandler<DeletePredictionCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeletePredictionCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeletePredictionCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var deleted = await _dbContext.Predictions
                .Where(item => item.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deleted == 0)
            {
                return result.AddPropertyError("Id", "Cannot find prediction with ID " + request.Id);
            }

            return result;
        }
    }
}
