using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Matches
{
    public class ListMatchesQuery : IRequest<OperationResult<PagedResult<MatchListItemDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
