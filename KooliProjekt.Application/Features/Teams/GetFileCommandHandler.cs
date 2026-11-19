using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Files;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Teams
{
    public class GetFileCommandHandler : IRequestHandler<GetFileCommand, OperationResult<FileDto>>
    {
        private readonly IFileClient _fileClient;

        public GetFileCommandHandler(IFileClient fileClient)
        {
            _fileClient = fileClient;
        }

        public async Task<OperationResult<FileDto>> Handle(GetFileCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<FileDto>();

            var stream = await _fileClient.Get(request.FileName, FileStore.ForTeam(request.TeamId));

            // Kui faili pole, jääb Value tühjaks ja kontroller tagastab 404 (nagu Get-päringutel)
            if (stream != null)
            {
                result.Value = new FileDto
                {
                    FileName = request.FileName,
                    Content = stream
                };
            }

            return result;
        }
    }
}
