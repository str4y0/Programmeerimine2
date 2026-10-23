using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Predictions
{
    public class ListPredictionsQuery : IRequest<OperationResult<PagedResult<PredictionListItemDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
