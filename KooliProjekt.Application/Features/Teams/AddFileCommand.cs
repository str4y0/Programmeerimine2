using System.IO;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Teams
{
    public class AddFileCommand : IRequest<OperationResult>
    {
        public int TeamId { get; set; }
        public string FileName { get; set; }
        public Stream InputStream { get; set; }
    }
}
