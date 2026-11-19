using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Infrastructure.Files;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Teams
{
    public class AddFileCommandHandler : IRequestHandler<AddFileCommand, OperationResult>
    {
        private readonly IFileClient _fileClient;

        public AddFileCommandHandler(IFileClient fileClient)
        {
            _fileClient = fileClient;
        }

        public async Task<OperationResult> Handle(AddFileCommand request, CancellationToken cancellationToken)
        {
            // Võistkonna olemasolu, faili nime ja tüübi kontrollib AddFileCommandValidator
            await _fileClient.Save(request.FileName, FileStore.ForTeam(request.TeamId), request.InputStream);

            return new OperationResult();
        }
    }
}
