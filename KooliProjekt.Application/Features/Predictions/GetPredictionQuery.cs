using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Predictions
{
    public class GetPredictionQuery : IRequest<OperationResult<PredictionDto>>
    {
        public int Id { get; set; }
    }
}
