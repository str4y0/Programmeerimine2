using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteUserCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            // Kasutaja ennustused kustuvad andmebaasis kaskaadina

            var deleted = await _dbContext.Users
                .Where(item => item.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deleted == 0)
            {
                return result.AddPropertyError("Id", "Cannot find user with ID " + request.Id);
            }

            return result;
        }
    }
}
