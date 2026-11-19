using System.Collections.Generic;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Teams
{
    public class ListFilesQuery : IRequest<OperationResult<IList<string>>>
    {
        public int TeamId { get; set; }
    }
}
