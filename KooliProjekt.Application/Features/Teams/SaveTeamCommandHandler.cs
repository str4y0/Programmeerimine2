using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Teams
{
    public class SaveTeamCommandHandler : IRequestHandler<SaveTeamCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveTeamCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveTeamCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            Team item;

            if (request.Id == 0)
            {
                item = new Team();
                _dbContext.Teams.Add(item);
            }
            else
            {
                item = await _dbContext.Teams.FindAsync(new object[] { request.Id }, cancellationToken);
                if (item == null)
                {
                    return result.AddPropertyError("Id", "Cannot find team with ID " + request.Id);
                }
            }

            item.Name = request.Name;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
