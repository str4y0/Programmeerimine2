using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Predictions
{
    public class DeletePredictionCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
