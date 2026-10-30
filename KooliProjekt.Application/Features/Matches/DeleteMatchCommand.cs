using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Matches
{
    public class DeleteMatchCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
