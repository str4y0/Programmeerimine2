using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Files;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Teams
{
    public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileClient _fileClient;

        public DeleteTeamCommandHandler(ApplicationDbContext dbContext, IFileClient fileClient)
        {
            _dbContext = dbContext;
            _fileClient = fileClient;
        }

        public async Task<OperationResult> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            // Mängu võõrvõtmel Teams'i suunas pole kaskaadkustutamist,
            // seega tiimi, mis on mängudes kasutusel, kustutada ei saa
            var inUse = await _dbContext.Matches
                .AnyAsync(match => match.HomeTeamId == request.Id || match.AwayTeamId == request.Id, cancellationToken);
            if (inUse)
            {
                return result.AddPropertyError("Id", "Team is used in matches and cannot be deleted");
            }

            var deleted = await _dbContext.Teams
                .Where(item => item.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deleted == 0)
            {
                return result.AddPropertyError("Id", "Cannot find team with ID " + request.Id);
            }

            // Kustutatud võistkonna failid ei tohi kettale alles jääda.
            // Failid kustutame alles pärast andmebaasist kustutamist: kui andmebaasi
            // kustutamine ebaõnnestub, jäävad failid alles.
            var fileStore = FileStore.ForTeam(request.Id);
            var fileNames = await _fileClient.List(fileStore);
            foreach (var fileName in fileNames)
            {
                await _fileClient.Delete(fileName, fileStore);
            }

            return result;
        }
    }
}
