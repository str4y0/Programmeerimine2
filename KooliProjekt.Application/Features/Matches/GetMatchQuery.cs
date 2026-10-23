using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Matches
{
    public class GetMatchQuery : IRequest<OperationResult<MatchDto>>
    {
        public int Id { get; set; }
    }
}
