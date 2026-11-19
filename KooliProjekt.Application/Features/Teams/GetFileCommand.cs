using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Teams
{
    public class GetFileCommand : IRequest<OperationResult<FileDto>>
    {
        public int TeamId { get; set; }
        public string FileName { get; set; }
    }
}
