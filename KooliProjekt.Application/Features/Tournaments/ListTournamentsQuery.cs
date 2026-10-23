using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Tournaments
{
    public class ListTournamentsQuery : IRequest<OperationResult<PagedResult<TournamentListItemDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
